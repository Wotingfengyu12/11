using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FieldIot.ProfibusDP
{
    public class DPComm
    {

        public DPComm(System.IO.Ports.SerialPort sp)
        {
            serialPort1 = sp;
        }

        public DPComm()
        {
            serialPort1 = null;
        }

        //常量定义
        private const int MAX_FRAME_LEN = 250;
        private const int MAX_RCV_BUFFER_SIZE = 1024 * 2;

        private const byte SD1 = 0x10;
        private const byte SD2 = 0x68;
        private const byte SD3 = 0xA2;
        private const byte SD4 = 0xDC;
        private const byte SC = 0xE5;
        private const byte END_DELIMITER = 0x16;

        private const byte ADDR_BROADCAST = 127;
        private const byte HSA = 127;
        private const byte ucThisNode = 2;

        private const int MAX_OFFLINE_TIMES = 40;
        public const byte MAX_USER_CFG_NUM = 10;

        private bool bMasterIsOn = false;

        private byte ucWD_Fact_1 = 10;
        private byte ucWD_Fact_2 = 10;
        private byte ucFC_Byte = 0x5D;

        //private byte ucCurrDevNum = 0;
        private List<CPROFIBUS_DEVICE> asActiveDevtList = new List<CPROFIBUS_DEVICE>();        //网段上设备列表
        private List<CDEV_CFG_INFO> asLocalDevCfgList = new List<CDEV_CFG_INFO>();               //网络配置信息列表
        private List<CTRANSFER_BUFFER> asTransBuffetList = new List<CTRANSFER_BUFFER>();               //网络配置信息列表

        private byte ucCurrOperDevIndex = 0;    //指出当前正在操作的从站在asLocalDevCfgList中的序号
        private byte ucNewOperDevIndex = 0;

        bool bManulOper = false;                //如为TRUE，表示当前操作是手工操作
        byte ucSendFrameNums = 0;               //用于记录已经发送多少非FDL_Status报文

        //
        //记录相关定义
        //
        private ushort uiMsgRestLen = 0;                         //用于存放上一次分析剩下的数据
        private byte[] aucMsgRest = new byte[MAX_FRAME_LEN];

        //
        //通信相关定义
        //
        //private int device_num = 0;                         //连接设备数目
        //        private int iCurrDeviceChipID = 0;                  //选中设备对应的CHIP_ID
        //        private string strCurrHardwareID = "";              //选中设备对应的硬件ID
        //private string strCurrComPortNumber = "";           //选中设备对应的端口号
                                                            //        private string strCurrPassword = "";                //选中设备对应的密码
        //int iBaudRate = 9600;

        bool bNetworkChanged = false;

        //先前发出的请求报文，以匹配接收的报文
        public CFRAME_PARSE_NODE cFrameReqPending = new CFRAME_PARSE_NODE();

        //当前搜索的地址
        byte ucNextAddrForSearching = ucThisNode + 1;
        bool bIsReqPending = false;         //用于判断主站是否在待从站响应
        bool bAutoMode = false;

        private System.IO.Ports.SerialPort serialPort1;

        //

        public void addDevice(CDEV_CFG_INFO cfg)
        {
            asLocalDevCfgList.Add(cfg);
        }

        private void DPMasterControl()
        {
            //主站未启动
            if (!bMasterIsOn || asLocalDevCfgList.Count == 0)
            {
                return;
            }

            //本主站还在待从站的响应，此时不时发送报文
            //只有接收到响应或超时后才能发送报文
            if (bIsReqPending)
            {
                if (bManulOper)
                {
                    bManulOper = false;
                }
                else
                {
                    resetSingleSlaveState(ucCurrOperDevIndex);
                }

                bIsReqPending = false;
            }

            if (bAutoMode)
            {
                ucCurrOperDevIndex = ucNewOperDevIndex;

                manageAllSlaves(ucCurrOperDevIndex);

                ucNewOperDevIndex++;
                if (ucNewOperDevIndex == asLocalDevCfgList.Count)
                {
                    ucNewOperDevIndex = 0;
                }
            }

            if (asTransBuffetList.Count > 0)
            {
                ucSendFrameNums++;
                //为了保证每发送若干用户报文就发送一个FDL_Status报文。
                if (ucSendFrameNums <= 8)
                {

                    transferFrame(asTransBuffetList.ElementAt(0).aucFrame, asTransBuffetList.ElementAt(0).ucFrameLen);

                    string strFrameData = "";

                    strFrameData = buildStringTypeInfo(asTransBuffetList.ElementAt(0).ucFrameLen, asTransBuffetList.ElementAt(0).aucFrame);

                    //dataGridView2.Rows.Add(ucThisNode.ToString() + " -> " + asTransBuffetList.ElementAt(0).ucD_Addr, asTransBuffetList.ElementAt(0).strMsgType, strFrameData);
                    //dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                    cFrameReqPending.strMsgType = asTransBuffetList.ElementAt(0).strMsgType;
                    bManulOper = asTransBuffetList.ElementAt(0).bManuOper;
                    asTransBuffetList.RemoveAt(0);
                    bIsReqPending = true;
                }
                else
                {
                    ucSendFrameNums = 0;
                    //向网络上发送FDL_Status.Req报文，以搜索网络上活动设备
                    buildAndSendFDL_StatusReq();
                }
            }
            else
            {
                //向网络上发送FDL_Status.Req报文，以搜索网络上活动设备
                buildAndSendFDL_StatusReq();
            }
        }

        public void SPDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bytetoread = serialPort1.BytesToRead;

            byte[] buff = new byte[MAX_RCV_BUFFER_SIZE];

            serialPort1.Read(buff, 0, bytetoread);

            //在只有本主站一个主站时，接收到报文只能是单个的DP响应报文
            //当网络中还有其它的主站时（不推荐这样用），尤其是在有PLC作为主站时，由于PC串口响应时间有限，则在此处可能会收到一批
            //报文。所以需要对这批报文进行分拆成单个报文
            SimpleParseDPFrame(buff, (ushort)bytetoread);
        }

        //
        //将接收缓冲区中的DP报文分成单个的报文
        //
        public void SimpleParseDPFrame(byte[] aucMsgData, ushort uiMsgLen)
        {
            int i;
            byte ucFrameLen = 0;
            ushort uiMsgWholeLen = 0;
            byte ucSD2LE = 0, ucSD2LEr = 0;
            byte[] aucFrame = new byte[256];
            byte[] aucMsgWhole = new byte[MAX_RCV_BUFFER_SIZE + 250];

            if (uiMsgLen > MAX_RCV_BUFFER_SIZE)
            {
                return;
            }

            //将上一次分析后剩下的不完整数据与新收到的数据连接在一起，且放在前面
            for (i = 0; i < uiMsgRestLen; i++)
            {
                aucMsgWhole[i] = aucMsgRest[i];
            }

            //将新收到的数据放在总数据的后面
            for (i = 0; i < uiMsgLen; i++)
            {
                aucMsgWhole[uiMsgRestLen + i] = aucMsgData[i];
            }

            uiMsgWholeLen = (ushort)(uiMsgRestLen + uiMsgLen);
            uiMsgRestLen = 0;

            //
            //每次收到的报文可能是多个报文的集合，所以需要将其拆分成多个单独的报文
            //
            i = 0;
            while (i < uiMsgWholeLen)
            {
                switch (aucMsgWhole[i])
                {
                    case SD1:
                        ucFrameLen = 6;

                        if ((uiMsgWholeLen - i) >= ucFrameLen)
                        {
                            if (aucMsgWhole[i + 5] == 0x16)
                            {
                                for (int t = 0; t < ucFrameLen; t++)
                                {
                                    aucFrame[t] = aucMsgWhole[t + i];
                                }

                                //处理完整报文
                                processSingleFrameForSerial(aucFrame, ucFrameLen);

                                i += ucFrameLen;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        else
                        {
                            uiMsgRestLen = (ushort)(uiMsgWholeLen - i);
                            for (int t = 0; t < uiMsgRestLen; t++)
                            {
                                aucMsgRest[t] = aucMsgWhole[t + i];
                            }

                            return;
                        }//if ((uiMsgWholeLen - i) >= ucFrameLen)
                        break;

                    case SD2:
                        //判断前4个字节
                        if ((uiMsgWholeLen - i) >= 4)
                        {
                            if (aucMsgWhole[i + 3] == 0x68)
                            {
                                ucSD2LE = aucMsgWhole[i + 1];
                                ucSD2LEr = aucMsgWhole[i + 2];

                                if (ucSD2LE == ucSD2LEr)
                                {
                                    ucFrameLen = (byte)(ucSD2LE + 6);
                                    if ((uiMsgWholeLen - i) >= ucFrameLen)//whole frame
                                    {
                                        if (aucMsgWhole[i + ucSD2LE + 5] == 0x16)
                                        {
                                            for (int t = 0; t < ucFrameLen; t++)
                                            {
                                                aucFrame[t] = aucMsgWhole[t + i];
                                            }

                                            //处理完整报文
                                            processSingleFrameForSerial(aucFrame, ucFrameLen);

                                            i += ucFrameLen;
                                        }
                                        else
                                        {
                                            i++;
                                        }
                                    }
                                    else//no a whole frame
                                    {
                                        uiMsgRestLen = (ushort)(uiMsgWholeLen - i);
                                        for (int t = 0; t < uiMsgRestLen; t++)
                                        {
                                            aucMsgRest[t] = aucMsgWhole[t + i];
                                        }
                                        return;
                                    }//if ((uiMsgWholeLen - i) >= ucFrameLen)//whole frame
                                }
                                else
                                {
                                    i++;
                                }//if (uiSD2LE == uiSD2LEr)
                            }
                            else
                            {
                                i++;
                            }//if (aucMsgWhole[i + 3] == 0x68)
                        }
                        else//rest data is too few
                        {
                            uiMsgRestLen = (ushort)(uiMsgWholeLen - i);
                            for (int t = 0; t < uiMsgRestLen; t++)
                            {
                                aucMsgRest[t] = aucMsgWhole[t + i];
                            }
                            return;
                        }
                        break;

                    case SD3:
                        ucFrameLen = 14;
                        if ((uiMsgWholeLen - i) >= ucFrameLen)
                        {
                            if (aucMsgWhole[i + 13] == 0x16)
                            {
                                for (int t = 0; t < ucFrameLen; t++)
                                {
                                    aucFrame[t] = aucMsgWhole[t + i];
                                }

                                //处理完整报文
                                processSingleFrameForSerial(aucFrame, ucFrameLen);

                                i += ucFrameLen;
                            }
                            else
                            {
                                i++;
                            }//if (aucMsgWhole[i + 13] == 0x16)
                        }
                        else
                        {
                            uiMsgRestLen = (ushort)(uiMsgWholeLen - i);
                            for (int t = 0; t < uiMsgRestLen; t++)
                            {
                                aucMsgRest[t] = aucMsgWhole[t + i];
                            }
                            return;
                        }//if ((uiMsgWholeLen - i) >= ucFrameLen)
                        break;
                    case SD4://TOKEN
                        ucFrameLen = 3;

                        if ((uiMsgWholeLen - i) >= ucFrameLen)
                        {
                            for (int t = 0; t < ucFrameLen; t++)
                            {
                                aucFrame[t] = aucMsgWhole[t + i];
                            }

                            //处理完整报文
                            processSingleFrameForSerial(aucFrame, ucFrameLen);

                            i += ucFrameLen;
                        }
                        else
                        {
                            uiMsgRestLen = (ushort)(uiMsgWholeLen - i);
                            for (int t = 0; t < uiMsgRestLen; t++)
                            {
                                aucMsgRest[t] = aucMsgWhole[t + i];
                            }
                            return;
                        }//if ((uiMsgWholeLen - i) >= ucFrameLen)
                        break;

                    case SC:
                        ucFrameLen = 1;

                        aucFrame[0] = aucMsgWhole[i];

                        //处理完整报文
                        processSingleFrameForSerial(aucFrame, ucFrameLen);

                        i += ucFrameLen;

                        break;
                    default:
                        i++;
                        break;
                }
            }
        }

        //
        //处理从串口接收到的单一报文,报文有错误或不需要处理（如请求报文）则返回FALSE,否则返回TRUE
        //报文在以下情况下会返回FALSE.
        //1、请求报文；2、目的地址不是本机地址的报文（广播地址除外）；3、FCS检查有错误的报文
        //
        public bool processSingleFrameForSerial(byte[] aucFrame, byte ucFrameLen)
        {
            //判断报文的长度不能超过最大长度
            if (ucFrameLen > (byte)MAX_FRAME_LEN)
            {
                return false;
            }

            CFRAME_PARSE_NODE cFrameParseNode = new CFRAME_PARSE_NODE();

            cFrameParseNode.ucFrameLen = ucFrameLen;

            cFrameParseNode.aucFrameData = new byte[cFrameParseNode.ucFrameLen];
            for (int i = 0; i < cFrameParseNode.ucFrameLen; i++)
            {
                cFrameParseNode.aucFrameData[i] = aucFrame[i];
            }

            switch (aucFrame[0])
            {
                case SD1:
                    //长度检查
                    if (ucFrameLen != 6)
                    {
                        return false;
                    }

                    //FCS检查
                    if (!checkFrameFCS(aucFrame, ucFrameLen))
                    {
                        return false;
                    }

                    //解析SD1类型报文
                    parseSD1(aucFrame, ucFrameLen, ref cFrameParseNode);

                    break;

                case SD2:
                    //长度检查
                    if ((aucFrame[1] + 6) != ucFrameLen)
                    {
                        return false;
                    }

                    //FCS检查
                    if (!checkFrameFCS(aucFrame, ucFrameLen))
                    {
                        return false;
                    }

                    //解析SD2类型报文
                    parseSD2(aucFrame, ucFrameLen, ref cFrameParseNode);

                    break;

                case SD3:
                    //长度检查
                    if (ucFrameLen != 14)
                    {
                        return false;
                    }

                    //FCS检查
                    if (!checkFrameFCS(aucFrame, ucFrameLen))
                    {
                        return false;
                    }

                    //解析SD3类型报文
                    parseSD3(aucFrame, ucFrameLen, ref cFrameParseNode);


                    break;

                case SD4://令牌
                    //长度检查
                    if (ucFrameLen != 3)
                    {
                        return false;
                    }

                    //解析SD4类型报文
                    parseSD4(aucFrame, ucFrameLen, ref cFrameParseNode);

                    break;

                case SC:
                    //长度检查
                    if (ucFrameLen != 1)
                    {
                        return false;
                    }

                    //深入分析SC
                    cFrameParseNode.strFrame = "SC";
                    cFrameParseNode.strMsgType = "Short_Ack";
                    cFrameParseNode.strReqRsp = "Rsp";
                    break;
            }

            //处理活动设备列表信息
            buildLiveListInfo(cFrameParseNode);

            //处理报文
            //主要处理与本主站有关的报文
            processDPFrame(cFrameParseNode);

            return false;
        }

        //
        //深入处理DP报文
        //
        private bool processDPFrame(CFRAME_PARSE_NODE cFrameParseNode)
        {

            //UpdateFrame(dataGridView2, cFrameParseNode);

            if (cFrameParseNode.strReqRsp != "Rsp")
            {
                return false;
            }

            if (cFrameParseNode.strService == "FDL_Status")
            {
                processDeviceOnlineStatus(cFrameParseNode.ucS_Address, false);
                return true;
            }

            if (cFrameParseNode.strMsgType == "Get_Diag")
            {

                //表示此诊断响应是手工操作引起，其结果需要显示在相应界面中
                if (bManulOper)
                {
                    bManulOper = false;

                    //显示接收到的数据
                    //myDelegateFunctionForDisplayRspData(dataGridView6, cFrameParseNode.aucData, cFrameParseNode.ucDataLen);

                    //显示接收到的数据长度
                    string strDataLen = string.Format("{0}", cFrameParseNode.ucDataLen);
                    //myDelegateFunctionForDisplayRspDataLen(textBox9, strDataLen);

                    string strTemp0 = parseSlaveStateByDiag(cFrameParseNode.aucData, cFrameParseNode.ucDataLen);

                    string strTemp = "";
                    parseDiagnosticsInfo(cFrameParseNode.aucData, cFrameParseNode.ucDataLen, ref strTemp);
                    //myDelegateFunctionForDisplayRspDataLen(textBox2, Resource.SlaveStatus + strTemp0 + "\r\n" + strTemp);
                }
                //表示此诊断响应是自动操作的结果
                else
                {
                    asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State = parseSlaveStateByDiag(cFrameParseNode.aucData, cFrameParseNode.ucDataLen);

                    if (asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State == "DATA_EXCHANGE")
                    {
                        bNetworkChanged = true;
                    }
                }

                bIsReqPending = false;
            }
            else if (cFrameParseNode.strMsgType == "Short_Ack") //短确认
            {
                //针对SET_PRM/CHK_CFG收到的确认
                if (cFrameReqPending.strMsgType == "Set_Prm" && asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State == "WAIT_PRM")
                {
                    asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State = "WAIT_PRM_SC";
                }
                //针对SET_PRM/CHK_CFG收到的确认
                else if (cFrameReqPending.strMsgType == "Chk_Cfg" && asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State == "WAIT_CFG")
                {
                    asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State = "WAIT_CFG_SC";
                }
                //
                //当从站没有输入数据时，从站针对DATA_EXCHANGE.REQ返回short_ack, 而主站也不做任何处理
                //
                bIsReqPending = false;
            }
            else if (cFrameParseNode.strMsgType == "Data_Exchange")
            {
                if (asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).ucInputLen != cFrameParseNode.ucDataLen)
                {
                    return false;
                }

                for (int i = 0; i < asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).ucInputLen; i++)
                {
                    asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).aucInputData[i] = cFrameParseNode.aucData[i];
                }
                bIsReqPending = false;
            }
            else if (cFrameParseNode.strService == "RS")
            {
                asLocalDevCfgList.ElementAt(ucCurrOperDevIndex).strWK_State = "RS";
                bIsReqPending = false;
            }

            return true;
        }

        //
        //只从响应报文中抽取地址信息，且只考虑源地址，即只考虑从站的响应
        //
        public void buildLiveListInfo(CFRAME_PARSE_NODE cFrameParseNode)
        {
            //处理从站在线
            if (cFrameParseNode.strReqRsp == "Rsp" && (cFrameParseNode.strFrame == "SD1" || cFrameParseNode.strFrame == "SD2"))
            {
                processDeviceOnlineStatus(cFrameParseNode.ucS_Address, false);
            }
        }

        //
        //分析诊断数据
        //
        private void parseDiagnosticsInfo(byte[] aucDiagData, byte ucDataLen, ref string strDiagRlt)
        {
            strDiagRlt = "";

            //byte1
            if ((aucDiagData[0] & 0x02) == 0x02)
            {
                strDiagRlt += Resource.Station_Not_Ready + "(Station_Not_Ready)||";
            }
            if ((aucDiagData[0] & 0x04) == 0x04)
            {
                strDiagRlt += Resource.CFGErr + "(Cfg_Fault)||";
            }
            if ((aucDiagData[0] & 0x08) == 0x08)
            {
                strDiagRlt += Resource.ExtDiag + "(Diag_Ext)||";
            }
            if ((aucDiagData[0] & 0x10) == 0x10)
            {
                strDiagRlt += Resource.NotSupport + "(Not_Supported)||";
            }
            if ((aucDiagData[0] & 0x40) == 0x40)
            {
                strDiagRlt += Resource.PrmErr + "(Prm_Fault)||";
            }

            //byte2
            if ((aucDiagData[1] & 0x01) == 0x01)
            {
                strDiagRlt += Resource.WaitPrm + "(Prm_Req)||";
            }
            if ((aucDiagData[1] & 0x08) == 0x08)
            {
                strDiagRlt += Resource.WatchDog + "(WD_On)||";
            }
            if ((aucDiagData[1] & 0x10) == 0x10)
            {
                strDiagRlt += Resource.Freeze + "(Freeze_Mode)||";
            }
            if ((aucDiagData[1] & 0x20) == 0x20)
            {
                strDiagRlt += Resource.Sync + "(Sync_Mode)||";
            }

            //byte3
            if ((aucDiagData[2] & 0x80) == 0x80)
            {
                strDiagRlt += Resource.DiagOver + "(Diag_Overflow)||";
            }

            //byte4
            strDiagRlt += string.Format(Resource.MasterAddr + "0x{0:X2}||", aucDiagData[3]);

            //byte5,6
            strDiagRlt += string.Format(Resource.Vendor + "ID：0x{0:X2}{1:X2}", aucDiagData[4], aucDiagData[5]);
        }

        //
        //分析诊断数据
        //
        private string parseSlaveStateByDiag(byte[] aucDiagData, byte ucDataLen)
        {
            //WAIT_PRM
            if ((aucDiagData[0] & 0x02) == 0x02 &&
                (aucDiagData[1] & 0x05) == 0x05 &&
                (aucDiagData[3] == 0xFF))
            {
                return "WAIT_PRM";
            }

            //WAIT_CFG
            if ((aucDiagData[0] & 0x02) == 0x02 &&
                (aucDiagData[1] & 0x05) == 0x04)
            {
                return "WAIT_CFG";
            }

            //DATA_EXCHANGE
            if (aucDiagData[0] == 0x00 &&
                (aucDiagData[1] & 0x05) == 0x04)
            {
                return "DATA_EXCHANGE";
            }

            return "OFF_LINE";
        }

        //
        //设备在线状态处理
        //ucOnLine: 0:未上线；1：在线；2：上线后，又掉线
        //bDevType: true: master; false: slave
        //
        public void processDeviceOnlineStatus(byte ucAddress, bool bDevType)
        {
            bool bRlt = false;
            if (ucAddress == 0)
            {
                bRlt = false;
            }

            foreach (CPROFIBUS_DEVICE cDeviceNode in asActiveDevtList)
            {
                //当前设备已经检测到
                if (cDeviceNode.ucAddress == ucAddress)
                {
                    //周期数复位
                    cDeviceNode.ucCycles = 0;

                    //此设备掉线后又上线，则重新刷新设备显示列表，使之上线
                    if (cDeviceNode.ucOnLine == 2)
                    {
                        //表示网络状态发生变化
                        bNetworkChanged = true;

                        //设置设备状态为“当前在线”
                        cDeviceNode.ucOnLine = 1;
                    }

                    //表示找到地址匹配的设备
                    bRlt = true;
                    break;
                }
            }

            //如在当前设备列表中匹配不到此设备，则表明此设备是新上线的设备
            if (!bRlt)
            {
                CPROFIBUS_DEVICE cDeviceNode = new CPROFIBUS_DEVICE();

                cDeviceNode.ucAddress = ucAddress;
                cDeviceNode.ucOnLine = 1;                       //当前在线
                cDeviceNode.bDevType = bDevType;
                cDeviceNode.ucCycles = 0;

                bNetworkChanged = true;

                //将新上线的设备加入到设备列表中
                asActiveDevtList.Add(cDeviceNode);
            }
        }

        //
        //解析SD1类型的报文
        //
        private void parseSD1(byte[] aucFrame, byte ucFrameLen, ref CFRAME_PARSE_NODE cFrameParseNode)
        {
            cFrameParseNode.ucD_Address = (byte)(aucFrame[1] & 0x7f);
            cFrameParseNode.ucS_Address = (byte)(aucFrame[2] & 0x7f);
            cFrameParseNode.ucDSAP = 0;
            cFrameParseNode.ucSSAP = 0;

            cFrameParseNode.ucFC = aucFrame[3];

            cFrameParseNode.strFrame = string.Format("SD1");        //帧类型
            decode_FC(ref cFrameParseNode);                         //服务字段/原语（请求或响应）
            decode_MsgType(ref cFrameParseNode);                    //消息类型MsgType
        }
        //
        //解析SD3类型的报文
        //
        private void parseSD3(byte[] aucFrame, byte ucFrameLen, ref CFRAME_PARSE_NODE cFrameParseNode)
        {
            cFrameParseNode.ucD_Address = (byte)(aucFrame[1] & 0x7f);
            cFrameParseNode.ucS_Address = (byte)(aucFrame[2] & 0x7f);
            cFrameParseNode.ucDSAP = 0;
            cFrameParseNode.ucSSAP = 0;

            cFrameParseNode.ucFC = aucFrame[3];

            cFrameParseNode.strFrame = string.Format("SD3");        //帧类型
        }
        //
        //解析SD4类型的报文
        //
        private void parseSD4(byte[] aucFrame, byte ucFrameLen, ref CFRAME_PARSE_NODE cFrameParseNode)
        {
            //深入分析令牌报文
            cFrameParseNode.strFrame = "SD4";
            cFrameParseNode.ucD_Address = (byte)(aucFrame[1] & 0x7f);                     //地址信息
            cFrameParseNode.ucS_Address = (byte)(aucFrame[2] & 0x7f);

            cFrameParseNode.strService = "Token";
            cFrameParseNode.strMsgType = "Pass_Token";
            cFrameParseNode.strReqRsp = "";

        }
        //
        //解析SD2类型的报文
        //
        private void parseSD2(byte[] aucFrame, byte ucFrameLen, ref CFRAME_PARSE_NODE cFrameParseNode)
        {
            byte ucAddrInfoLen = 0;

            cFrameParseNode.strFrame = string.Format("SD2");        //帧类型

            //
            //解析报文中的地址信息
            //ucAddrInfoLen用于计算地址信息占用了SD2数据域中多少个字节
            //解析出SD2中的设备地址、段地址及SAP信息
            //
            decode_AddressInfo(aucFrame, ucFrameLen, ref cFrameParseNode, ref ucAddrInfoLen);

            //服务字段及请求和响应字段,根据FC字段修改
            cFrameParseNode.ucFC = aucFrame[6];
            decode_FC(ref cFrameParseNode);

            //提取出SD2中的用户数据
            cFrameParseNode.ucDataLen = (byte)(aucFrame[1] - 3 - ucAddrInfoLen);

            if (cFrameParseNode.ucDataLen > 128)
            {
                return;
            }

            if (cFrameParseNode.ucDataLen > 0)
            {
                cFrameParseNode.aucData = new byte[cFrameParseNode.ucDataLen];

                for (int i = 0; i < cFrameParseNode.ucDataLen; i++)
                {
                    cFrameParseNode.aucData[i] = aucFrame[6 + ucAddrInfoLen + 1 + i];
                }
            }

            //消息类型
            decode_MsgType(ref cFrameParseNode);
        }
        //
        //解析报文的SERVICE信息
        //
        public void decode_AddressInfo(byte[] aucFrame, byte ucFrameLen, ref CFRAME_PARSE_NODE cFrameParseNode, ref byte ucAddrInfoLen)
        {
            byte ucS_Addr, ucD_Addr;
            byte ucDAE_Flag = 0, ucDAE_ADDITION_Flag = 0;
            byte ucDataFlag = 0;
            byte ucOffset;


            //目的地址、目的段地址及目的SAP解析
            ucD_Addr = aucFrame[4];
            if ((ucD_Addr >> 7) == 0) //目的地址无扩展部分
            {

                cFrameParseNode.ucD_Address = ucD_Addr;
                cFrameParseNode.ucD_Seg = 0;
                cFrameParseNode.ucDSAP = 0;
            }
            else //目的地址有扩展部分
            {
                cFrameParseNode.ucD_Address = (byte)(ucD_Addr & 0x7F);

                ucDAE_Flag = 1;

                if (((aucFrame[7] & 0x40) >> 6) == 0) //无段地址扩展
                {
                    cFrameParseNode.ucDSAP = (byte)(aucFrame[7] & 0x3F);
                    cFrameParseNode.ucD_Seg = 0;

                    ucDataFlag += 1;
                }
                else //SegAddr+LSAP
                {
                    ucDAE_ADDITION_Flag = 1;
                    cFrameParseNode.ucD_Seg = (byte)(aucFrame[7] & 0x3F);
                    cFrameParseNode.ucDSAP = (byte)(aucFrame[8] & 0x3F);

                    ucDataFlag += 2;
                }
            }//if((ucD_Addr >> 7) == 0)

            //源地址、源段地址及源SAP解析
            ucS_Addr = aucFrame[5];
            if ((ucS_Addr >> 7) == 0) //源地址无扩展部分
            {
                cFrameParseNode.ucS_Address = ucS_Addr;
                cFrameParseNode.ucS_Seg = 0;
                cFrameParseNode.ucSSAP = 0;
            }
            else //源地址有 扩展部分
            {
                cFrameParseNode.ucS_Address = (byte)(ucS_Addr & 0x7F);

                if (ucDAE_Flag == 0)//no DAE
                {
                    ucOffset = 7;
                    //pSAE = pucDataUnit;
                }
                else //with DAE
                {
                    if (ucDAE_ADDITION_Flag == 0) //no DAE additional octet
                    {
                        ucOffset = 8;
                        //pSAE = pucDataUnit+1;
                    }
                    else
                    {
                        ucOffset = 9;
                        //pSAE = pucDataUnit+2;
                    }
                }

                if (((aucFrame[ucOffset] & 0x40) >> 6) == 0)	//no Region/Segment address
                {
                    cFrameParseNode.ucSSAP = (byte)(aucFrame[ucOffset] & 0x3F);
                    cFrameParseNode.ucS_Seg = 0;

                    ucDataFlag += 1;
                }
                else //SegAddr+LSAP
                {
                    cFrameParseNode.ucS_Seg = (byte)(aucFrame[ucOffset] & 0x3F);
                    cFrameParseNode.ucSSAP = (byte)(aucFrame[ucOffset + 1] & 0x3F);

                    ucDataFlag += 2;
                }
            }//if((ucS_Addr >> 7) == 0)

            ucAddrInfoLen = ucDataFlag;
        }
        //
        //分析报文的MsgType，如Data Exchange、 Get Diagnostics等。
        //在此之前需要先解析出服务类型变量
        //
        public void decode_MsgType(ref CFRAME_PARSE_NODE cFrameParseNode)
        {
            byte ucSSAP, ucDSAP;

            ucSSAP = cFrameParseNode.ucSSAP;
            ucDSAP = cFrameParseNode.ucDSAP;

            if (cFrameParseNode.strFrame == "SD1")
            {
                //"SD1"类型帧主要用于FDL_Status和DATA_EXCHANGE（不含输入或输出数据）
                if ((cFrameParseNode.strService == "FDL_Status" && cFrameParseNode.strReqRsp == "Req") ||
                    (cFrameParseNode.strService == "Passive" && cFrameParseNode.strReqRsp == "Rsp"))
                {
                    cFrameParseNode.strMsgType = "Ack.POS";
                    return;
                }
                if ((cFrameParseNode.strService == "Ack.Err.RR" && cFrameParseNode.strReqRsp == "Rsp") ||
                    (cFrameParseNode.strService == "Ack.Err.SAP" && cFrameParseNode.strReqRsp == "Rsp"))
                {
                    cFrameParseNode.strMsgType = "Ack.NEG";
                    return;
                }
                else if ((cFrameParseNode.strReqRsp == "Req" && cFrameParseNode.strService == "SRD_HIGH") ||
                        (cFrameParseNode.strReqRsp == "Rsp" && (cFrameParseNode.strService == "DL" || cFrameParseNode.strService == "DH")))
                {
                    cFrameParseNode.strMsgType = "Data_Exchange";
                }
            }
            else if (cFrameParseNode.strFrame == "SD2")
            {
                //
                //DPV0部分
                //
                if (ucSSAP == 0 && ucDSAP == 0)
                {
                    cFrameParseNode.strMsgType = "Data_Exchange";
                    return;
                }
                else if (ucSSAP == 62 && ucDSAP == 62)
                {
                    cFrameParseNode.strMsgType = "Chk_Cfg";
                    return;
                }
                else if ((ucSSAP == 62 && ucDSAP == 61) || (ucSSAP == 61 && ucDSAP == 62))
                {
                    cFrameParseNode.strMsgType = "Set_Prm";

                    return;
                }
                else if ((ucSSAP == 62 && ucDSAP == 60) || (ucSSAP == 60 && ucDSAP == 62))
                {
                    cFrameParseNode.strMsgType = "Get_Diag";

                    return;
                }
                else if ((ucSSAP == 62 && ucDSAP == 59) || (ucSSAP == 59 && ucDSAP == 62))
                {
                    cFrameParseNode.strMsgType = "Get_Cfg";

                    return;
                }
                else if (ucSSAP == 62 && ucDSAP == 58)
                {
                    cFrameParseNode.strMsgType = "Glb_Ctrl";

                    return;
                }
                else if ((ucSSAP == 62 && ucDSAP == 57) || (ucSSAP == 57 && ucDSAP == 62))
                {
                    cFrameParseNode.strMsgType = "Rd_OutP";

                    return;
                }
                else if ((ucSSAP == 62 && ucDSAP == 56) || (ucSSAP == 56 && ucDSAP == 62))
                {
                    cFrameParseNode.strMsgType = "Rd_InP";

                    return;
                }
                else if ((ucSSAP == 62 && ucDSAP == 55) || (ucSSAP == 55 && ucDSAP == 62))
                {
                    cFrameParseNode.strMsgType = "Set_SlaveAddr";

                    return;
                }
            }
            else if (cFrameParseNode.strFrame == "SD3")
            {

            }
            else
            {
                cFrameParseNode.strMsgType = "unknown_type";
            }

            return;
        }
        //
        //解析报文的SERVICE信息
        //
        public void decode_FC(ref CFRAME_PARSE_NODE cFrameParseNode)
        {
            byte ucFuncCode;

            ucFuncCode = (byte)(cFrameParseNode.ucFC & 0x0F);

            if ((cFrameParseNode.ucFC & 0x40) == 0x40) //Request
            {
                cFrameParseNode.strReqRsp = "Req";
                switch (ucFuncCode)
                {
                    case 0:
                        cFrameParseNode.strService = "TIME_EVENT";
                        break;
                    //
                    //1,2:Reserved
                    //
                    case 3:
                        cFrameParseNode.strService = "SDA_LOW";
                        break;
                    case 4:
                        cFrameParseNode.strService = "SDN_LOW";
                        break;
                    case 5:
                        cFrameParseNode.strService = "SDA_HIGH";
                        break;
                    case 6:
                        cFrameParseNode.strService = "SDN_HIGH";
                        break;
                    case 7:
                        cFrameParseNode.strService = "REQ_DIAG";
                        break;
                    //
                    //8:Reserved
                    //
                    case 9:
                        cFrameParseNode.strService = "FDL_Status";
                        break;
                    //
                    //10,11:Reserved
                    //
                    case 12:
                        cFrameParseNode.strService = "SRD_LOW";
                        break;
                    case 13:
                        cFrameParseNode.strService = "SRD_HIGH";
                        break;
                    case 14:
                        cFrameParseNode.strService = "IDENT";
                        break;
                    //
                    //15:Reserved
                    //
                    default:
                        cFrameParseNode.strService = "Unknown_Req";
                        break;
                }
            }
            else if ((cFrameParseNode.ucFC & 0x40) == 0x00)//Response
            {
                cFrameParseNode.strReqRsp = "Rsp";

                switch (ucFuncCode)
                {
                    case 0:
                        cFrameParseNode.strService = "Passive"; // ("OK");
                        break;
                    case 1:
                        cFrameParseNode.strService = "UE";
                        break;
                    case 2:
                        cFrameParseNode.strService = "Ack.Err.RR";
                        break;
                    case 3:
                        cFrameParseNode.strService = "Ack.Err.SAP";
                        break;
                    //
                    //4~7:Reserved
                    //
                    case 8:
                        cFrameParseNode.strService = "DL";
                        break;
                    case 9:
                        cFrameParseNode.strService = "NR";
                        break;
                    case 10:
                        cFrameParseNode.strService = "DH";
                        break;
                    case 12:
                        cFrameParseNode.strService = "RDL";
                        break;
                    case 13:
                        cFrameParseNode.strService = "RDH";
                        break;
                    //
                    //14,15:Reserved
                    //
                    default:
                        cFrameParseNode.strService = "Unknown_Rsp";
                        break;
                }
            }
        }

        //
        //检查报文的FCS正确性。正确返回true,否则返回false。
        //
        public bool checkFrameFCS(byte[] aucFrame, byte ucFrameLen)
        {
            bool bRlt = true;

            //
            //DP类型报文后面倒数第二个报文是检验码
            //
            byte ucFCS = aucFrame[ucFrameLen - 2];
            byte ucFCS_Rlt = 0;

            switch (aucFrame[0])
            {
                case SD1:
                    for (int i = 0; i < 3; i++)
                    {
                        ucFCS_Rlt += aucFrame[1 + i];
                    }
                    break;
                case SD2:
                    for (int i = 0; i < aucFrame[1]; i++)
                    {
                        ucFCS_Rlt += aucFrame[4 + i];
                    }
                    break;
                case SD3:
                    for (int i = 0; i < 11; i++)
                    {
                        ucFCS_Rlt += aucFrame[1 + i];
                    }
                    break;
                default:
                    return false;
                    //break;
            }

            //判断正确与否？
            if (ucFCS == ucFCS_Rlt)
                bRlt = true;
            else
                bRlt = false;

            return bRlt;
        }

        public void buildAndSendFDL_StatusReq()
        {
            //向网络上发送FDL_Status.Req报文，以搜索网络上活动设备
            if (ucNextAddrForSearching > HSA)
            {
                ucNextAddrForSearching = ucThisNode + 1;
            }

            byte[] aucFrame = new byte[6];
            byte ucFrameLen = 0;

            //生成FDL_Status.Req报文
            buildFDL_StatusReq(ucNextAddrForSearching, ucThisNode, ref aucFrame, ref ucFrameLen);

            //发送FDL_Status请求
            transferFrame(aucFrame, ucFrameLen);
            string strFrameData = "";
            strFrameData = buildStringTypeInfo(ucFrameLen, aucFrame);
            //LOGs here?????
            //dataGridView2.Rows.Add(ucThisNode.ToString() + " -> " + ucNextAddrForSearching.ToString(), "FDL_status", strFrameData);

            //            dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[0].Style.BackColor = Color.Green;

            //dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
            ucNextAddrForSearching++;
        }

        public string buildStringTypeInfo(byte ucDataLen, byte[] aucData)
        {
            string str = "";

            if (ucDataLen == 0)
            {
                return str;
            }

            for (int i = 0; i < ucDataLen - 1; i++)
            {
                str += string.Format("{0:X2} ", aucData[i]);
            }
            str += string.Format("{0:X2}", aucData[ucDataLen - 1]);

            return str;
        }

        //
        //复位所有从站管理状态
        //
        private bool resetSingleSlaveState(byte ucIndex)
        {
            if (asLocalDevCfgList.Count == 0)
            {
                return false;
            }

            if (!(ucIndex >= 0 && ucIndex < asLocalDevCfgList.Count))
            {
                return false;
            }

            asLocalDevCfgList.ElementAt(ucIndex).strWK_State = "OFF_LINE";

            //清空输入数据缓冲区
            if (asLocalDevCfgList.ElementAt(ucIndex).ucInputLen > 0)
            {
                for (int i = 0; i < asLocalDevCfgList.ElementAt(ucIndex).ucInputLen; i++)
                {
                    asLocalDevCfgList.ElementAt(ucIndex).aucInputData[i] = 0;
                }
            }

            //清空输出数据缓冲区
            if (asLocalDevCfgList.ElementAt(ucIndex).ucOutputLen > 0)
            {
                for (int i = 0; i < asLocalDevCfgList.ElementAt(ucIndex).ucOutputLen; i++)
                {
                    asLocalDevCfgList.ElementAt(ucIndex).aucOutputData[i] = 0;
                }
            }

            //清空发送队列是与对应从站相关的数据
            asTransBuffetList.Clear();

            bNetworkChanged = true;

            return true;
        }

        //LiveList处理
        private void refreshLiveListInfo()
        {
            int iNumMaster = 0, iNumSlave = 0;

            //
            //看是否有设备超时掉线
            //
            foreach (CPROFIBUS_DEVICE cDeviceNode in asActiveDevtList)
            {
                //对本主站的信息不处理，即一旦启动，永远在线
                if (cDeviceNode.ucAddress == ucThisNode)
                {
                    continue;
                }

                if (cDeviceNode.ucOnLine == 1)//在线
                {
                    //判断设备是否掉线
                    if (cDeviceNode.ucCycles >= MAX_OFFLINE_TIMES)//刚掉线
                    {
                        cDeviceNode.ucOnLine = 2;
                        bNetworkChanged = true;
                    }
                    else
                    {
                        cDeviceNode.ucCycles++;
                    }
                }
            }

            if (bNetworkChanged)
            {
                foreach (CPROFIBUS_DEVICE cDeviceNode in asActiveDevtList)
                {
                    int iRow = cDeviceNode.ucAddress / 10;
                    int iColumn = cDeviceNode.ucAddress % 10;

                    if (cDeviceNode.ucOnLine == 1)//在线
                    {
                        if (cDeviceNode.bDevType)//master
                        {
                            iNumMaster++;
                            //dataGridView1.Rows[iRow].Cells[iColumn].Style.BackColor = Color.MediumVioletRed;
                        }
                        else//slave
                        {
                            iNumSlave++;
                            //dataGridView1.Rows[iRow].Cells[iColumn].Style.BackColor = Color.Cyan;

                            string strWKstate = "";
                            if (getSlaveWorkingState(cDeviceNode.ucAddress, ref strWKstate))
                            {
                                if (strWKstate == "DATA_EXCHANGE")
                                {
                                    //dataGridView1.Rows[iRow].Cells[iColumn].Style.BackColor = Color.Green;
                                }
                            }
                        }
                    }
                    else if (cDeviceNode.ucOnLine == 2)//在过线，但现在掉线了
                    {
                        //dataGridView1.Rows[iRow].Cells[iColumn].Style.BackColor = Color.Yellow;
                    }
                }

                bNetworkChanged = false;
            }
        }

        //
        //获取指定从站的工作状态，未找到则返回FALSE。
        //
        private bool getSlaveWorkingState(byte ucAddr, ref string strWKState)
        {
            foreach (CDEV_CFG_INFO cDevCfgInfoNode in asLocalDevCfgList)
            {
                if (cDevCfgInfoNode.ucAddrS == ucAddr)
                {
                    strWKState = cDevCfgInfoNode.strWK_State;
                    return true;
                }
            }

            return false;
        }

        //
        //当主站关闭时，将所有设备下线
        //
        private void offlineAllDevicesInLiveListDisplay()
        {
            foreach (CPROFIBUS_DEVICE cDeviceNode in asActiveDevtList)
            {
                if (cDeviceNode.ucOnLine == 1)//在线
                {
                    int iRow = cDeviceNode.ucAddress / 10;
                    int iColumn = cDeviceNode.ucAddress % 10;

                    //dataGridView1.Rows[iRow].Cells[iColumn].Style.BackColor = Color.Yellow;
                }
            }
        }

        //
        //复位所有从站管理状态
        //
        private void resetAllSlaveState()
        {
            ucSendFrameNums = 0; ;

            foreach (CDEV_CFG_INFO cDevCfgInfoNode in asLocalDevCfgList)
            {
                cDevCfgInfoNode.strWK_State = "OFF_LINE";

                if (cDevCfgInfoNode.ucInputLen > 0)
                {
                    for (int i = 0; i < cDevCfgInfoNode.ucInputLen; i++)
                    {
                        cDevCfgInfoNode.aucInputData[i] = 0;
                    }
                }

                if (cDevCfgInfoNode.ucOutputLen > 0)
                {
                    for (int i = 0; i < cDevCfgInfoNode.ucOutputLen; i++)
                    {
                        cDevCfgInfoNode.aucOutputData[i] = 0;
                    }
                }
            }
        }

        //
        //通过物理端口发送报文
        //
        public void transferFrame(byte[] aucFrame, byte ucFrameLen)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(aucFrame, 0, ucFrameLen);
            }
        }

        //
        //构建DDLM_Slave_Diag_Req报文
        //68 05 05 68 8A 82 7D 3C 3E 03 16
        //
        public void build_DDLM_Slave_Diag_Req(byte ucD_Address, byte ucS_Address, ref byte[] aucFrame, ref byte ucFrameLen, byte ucFC)
        {
            aucFrame[0] = SD2;
            aucFrame[1] = 5;
            aucFrame[2] = 5;
            aucFrame[3] = SD2;
            aucFrame[4] = (byte)(0x80 | ucD_Address);
            aucFrame[5] = (byte)(0x80 | ucS_Address);
            aucFrame[6] = ucFC;         //FC
            aucFrame[7] = 60;           //DSAP
            aucFrame[8] = 62;           //SSAP
            aucFrame[9] = calculateFCS(aucFrame, 4, 5);
            aucFrame[10] = END_DELIMITER;

            ucFrameLen = 11;
        }

        //
        //构建DDLM_Set_Prm_Req报文
        //68 0C 0C 68 8A 82 5D 3D 3E F8 0F 10 0B 18 10 00 2E 16
        //
        public void build_DDLM_Set_Prm_Req(byte ucD_Address, byte ucS_Address, ref byte[] aucFrame, ref byte ucFrameLen, byte[] aucData, byte ucDataLen, byte[] aucExtData, byte ucExtDataLen, byte ucFC)
        {
            byte ucSer = 0;

            aucFrame[0] = SD2;
            aucFrame[1] = (byte)(5 + ucDataLen + ucExtDataLen);
            aucFrame[2] = (byte)(5 + ucDataLen + ucExtDataLen);
            aucFrame[3] = SD2;
            aucFrame[4] = (byte)(0x80 | ucD_Address);
            aucFrame[5] = (byte)(0x80 | ucS_Address);
            aucFrame[6] = ucFC;         //FC
            aucFrame[7] = 61;           //DSAP
            aucFrame[8] = 62;           //SSAP
            ucSer = 9;

            for (int i = 0; i < ucDataLen; i++)
            {
                aucFrame[ucSer + i] = aucData[i];
            }
            ucSer += ucDataLen;

            if (ucExtDataLen > 0)
            {
                for (int i = 0; i < ucExtDataLen; i++)
                {
                    aucFrame[ucSer + i] = aucExtData[i];
                }
                ucSer += ucExtDataLen;
            }

            aucFrame[ucSer] = calculateFCS(aucFrame, 4, (byte)(5 + ucDataLen + ucExtDataLen));
            ucSer += 1;

            aucFrame[ucSer] = END_DELIMITER;
            ucSer += 1;

            ucFrameLen = ucSer;
        }

        //
        //构建Chk_Cfg报文
        //68 07 07 68 8A 82 7D 3E 3E 7F 7F 03 16
        //
        public void build_DDLM_Chk_Cfg_Req(byte ucD_Address, byte ucS_Address, ref byte[] aucFrame, ref byte ucFrameLen, byte[] aucData, byte ucDataLen, byte ucFC)
        {
            byte ucSer = 0;

            aucFrame[0] = SD2;
            aucFrame[1] = (byte)(5 + ucDataLen);
            aucFrame[2] = (byte)(5 + ucDataLen);
            aucFrame[3] = SD2;
            aucFrame[4] = (byte)(0x80 | ucD_Address);
            aucFrame[5] = (byte)(0x80 | ucS_Address);
            aucFrame[6] = ucFC;         //FC
            aucFrame[7] = 62;           //DSAP
            aucFrame[8] = 62;           //SSAP
            ucSer = 9;

            for (int i = 0; i < ucDataLen; i++)
            {
                aucFrame[ucSer + i] = aucData[i];
            }
            ucSer += ucDataLen;

            aucFrame[ucSer] = calculateFCS(aucFrame, 4, (byte)(5 + ucDataLen));
            ucSer += 1;

            aucFrame[ucSer] = END_DELIMITER;
            ucSer += 1;

            ucFrameLen = ucSer;
        }

        //
        //构建Data_Exchange报文
        //
        public void build_DDLM_Data_Exchange_Req(byte ucD_Address, byte ucS_Address, ref byte[] aucFrame, ref byte ucFrameLen, byte[] aucData, byte ucDataLen, byte ucFC)
        {
            byte ucSer = 0;

            if (ucDataLen > 0)
            {
                aucFrame[0] = SD2;
                aucFrame[1] = (byte)(3 + ucDataLen);
                aucFrame[2] = (byte)(3 + ucDataLen);
                aucFrame[3] = SD2;
                aucFrame[4] = ucD_Address;
                aucFrame[5] = ucS_Address;
                aucFrame[6] = ucFC;         //FC
                ucSer = 7;

                for (int i = 0; i < ucDataLen; i++)
                {
                    aucFrame[ucSer + i] = aucData[i];
                }
                ucSer += ucDataLen;

                aucFrame[ucSer] = calculateFCS(aucFrame, 4, (byte)(5 + ucDataLen));
                ucSer += 1;

                aucFrame[ucSer] = END_DELIMITER;
                ucSer += 1;

                ucFrameLen = ucSer;
            }
            else
            {
                aucFrame[0] = SD1;
                aucFrame[1] = ucD_Address;
                aucFrame[2] = ucS_Address;
                aucFrame[3] = ucFC;         //FC
                ucSer = 4;

                aucFrame[ucSer] = calculateFCS(aucFrame, 1, 3);
                ucSer += 1;

                aucFrame[ucSer] = END_DELIMITER;
                ucSer += 1;

                ucFrameLen = ucSer;
            }
        }

        //
        //构建RD_InP请求报文
        //
        public void build_DDLM_RD_InP_Req(byte ucD_Address, byte ucS_Address, ref byte[] aucFrame, ref byte ucFrameLen, byte ucFC)
        {
            byte ucSer = 0;

            aucFrame[0] = SD2;
            aucFrame[1] = 5;
            aucFrame[2] = 5;
            aucFrame[3] = SD2;
            aucFrame[4] = (byte)(0x80 | ucD_Address);
            aucFrame[5] = (byte)(0x80 | ucS_Address);
            aucFrame[6] = ucFC;         //FC
            aucFrame[7] = 56;           //DSAP
            aucFrame[8] = 62;           //SSAP
            ucSer = 9;

            aucFrame[ucSer] = calculateFCS(aucFrame, 4, 5);
            ucSer += 1;

            aucFrame[ucSer] = END_DELIMITER;
            ucSer += 1;

            ucFrameLen = ucSer;
        }

        public void diagDevice(byte ucDevIndex)
        {
            //生成诊断报文
            byte[] aucFrame = new byte[255];
            byte ucFrameLen = 0;

            build_DDLM_Slave_Diag_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, ucFC_Byte);
            ucFC_Byte = changeFC_FCB(ucFC_Byte);

            //发送报文
            sendFrame("Get_Diag", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
        }

        private void manageAllSlaves(byte ucDevIndex)
        {
            if (asLocalDevCfgList.Count == 0)
            {
                return;
            }

            byte[] aucFrame = new byte[255];
            byte ucFrameLen = 0;
            byte[] aucData = new byte[16];
            byte ucDataLen = 0;

            //如果当前从站处于“OFF_LINE"状态，则首先需要进行诊断操作
            if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "OFF_LINE")
            {
                //生成诊断报文
                build_DDLM_Slave_Diag_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Get_Diag", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
            else if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "WAIT_PRM")
            {
                //生成SET_PRM报文中数据
                aucData[0] = 0xB8;
                aucData[1] = ucWD_Fact_1;
                aucData[2] = ucWD_Fact_2;
                aucData[3] = asLocalDevCfgList.ElementAt(ucDevIndex).ucMinTsdr;
                aucData[4] = asLocalDevCfgList.ElementAt(ucDevIndex).ucIdentH;
                aucData[5] = asLocalDevCfgList.ElementAt(ucDevIndex).ucIdentL;
                aucData[6] = 0;
                ucDataLen = 7;

                //生成SET_PRM报文                        
                build_DDLM_Set_Prm_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, aucData, ucDataLen,
                                        asLocalDevCfgList.ElementAt(ucDevIndex).aucUserExtPrmData, asLocalDevCfgList.ElementAt(ucDevIndex).ucUserExtPrmDataLen,
                                        ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Set_Prm", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
            else if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "WAIT_PRM_SC")
            {
                //生成诊断报文
                build_DDLM_Slave_Diag_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Get_Diag", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
            else if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "WAIT_CFG")
            {
                //生成并发送CHK_CFG报文
                build_DDLM_Chk_Cfg_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen,
                                        asLocalDevCfgList.ElementAt(ucDevIndex).aucCfgData, asLocalDevCfgList.ElementAt(ucDevIndex).ucCfgDataLen,
                                        ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Chk_Cfg", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
            else if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "WAIT_CFG_SC")
            {
                //生成诊断报文
                build_DDLM_Slave_Diag_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Get_Diag", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
            else if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "DATA_EXCHANGE")
            {
                //生成并发送DATA_EXCHANGE报文
                build_DDLM_Data_Exchange_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).aucOutputData, asLocalDevCfgList.ElementAt(ucDevIndex).ucOutputLen, ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Data_Exchange", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
            else if (asLocalDevCfgList.ElementAt(ucDevIndex).strWK_State == "RS")
            {
                //生成诊断报文
                build_DDLM_Slave_Diag_Req(asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, ucThisNode, ref aucFrame, ref ucFrameLen, ucFC_Byte);
                ucFC_Byte = changeFC_FCB(ucFC_Byte);

                //发送报文
                sendFrame("Get_Diag", aucFrame, ucFrameLen, asLocalDevCfgList.ElementAt(ucDevIndex).ucAddrS, false);
            }
        }

        private byte changeFC_FCB(byte ucFC)
        {
            byte ucFC_Byte = ucFC;

            if (ucFC_Byte == 0x7D)
            {
                ucFC_Byte = 0x5D;
            }
            else if (ucFC_Byte == 0x5D)
            {
                ucFC_Byte = 0x7D;
            }

            return ucFC_Byte;
        }

        //
        //发送报文，保存到发送队列中
        //
        public bool sendFrame(string strMsgType, byte[] aucFrame, byte ucFrameLen, byte ucD_Addr, bool bManuOper)
        {
            if (asTransBuffetList.Count == 16 || ucFrameLen == 0)
            {
                return false;
            }

            CTRANSFER_BUFFER cTransferBufferNode = new CTRANSFER_BUFFER();

            cTransferBufferNode.ucFrameLen = ucFrameLen;
            for (int i = 0; i < ucFrameLen; i++)
            {
                cTransferBufferNode.aucFrame[i] = aucFrame[i];
            }

            cTransferBufferNode.strMsgType = strMsgType;
            cTransferBufferNode.ucD_Addr = ucD_Addr;
            cTransferBufferNode.bManuOper = bManuOper;

            asTransBuffetList.Add(cTransferBufferNode);

            return true;
        }
        //
        //构建FDL_Status报文
        //
        private void buildFDL_StatusReq(byte ucD_Address, byte ucS_Address, ref byte[] aucFrame, ref byte ucFrameLen)
        {
            aucFrame[0] = SD1;
            aucFrame[1] = ucD_Address;
            aucFrame[2] = ucS_Address;
            aucFrame[3] = 0x49;
            aucFrame[4] = calculateFCS(aucFrame, 1, 3);
            aucFrame[5] = END_DELIMITER;

            ucFrameLen = 6;
        }
        //
        //计算报文的FCS。
        //
        public byte calculateFCS(byte[] aucFrame, byte ucOffset, byte ucFrameLen)
        {
            byte ucFCS_Rlt = 0;

            for (int i = 0; i < ucFrameLen; i++)
            {
                ucFCS_Rlt += aucFrame[ucOffset + i];
            }

            return ucFCS_Rlt;
        }



    }

    //
    //报文解析后的部分
    //
    public class CFRAME_PARSE_NODE
    {
        public string strFrame = "";            //SD1,SD2......    
        public string strService = "";
        public string strMsgType = "";
        public string strReqRsp = "";

        public byte ucDSAP = 0;                 //服务访问点
        public byte ucSSAP = 0;

        public byte ucS_Seg = 0;                //网段、地址信息
        public byte ucD_Seg = 0;
        public byte ucD_Address = 0;
        public byte ucS_Address = 0;

        public byte ucDataLen = 0;              //用户数据
        public byte[] aucData;// = new byte[128];

        public byte ucFC = 0;                   //FC信息 
        public byte ucFrameLen = 0;                   //FC长度
        public byte[] aucFrameData;// = new byte[128];
    }

    public class CPROFIBUS_DEVICE
    {
        public byte ucAddress;
        public bool bDevType = false;      //false:slave; true:master
        public byte ucOnLine = 0;           //0：从未上线，1：当前在线，2：上过线，但现在掉线了
        public byte ucCycles = 0;           //定时器每个周期都给已经上线设备的ucCycles变量加1，在收到此设备的在线信息后则清此变量变0
                                            //如ucCycles达到一个上限，则说明此设备已经掉线
    }

    public class CDEV_CFG_INFO
    {
        public byte ucAddrS;

        public byte ucIdentH;
        public byte ucIdentL;

        public byte ucMinTsdr = 11;

        public const byte MAX_IO_LEN = 244;
        public const byte MAX_EXT_PRM_LEN = 244;
        public const byte MAX_CFG_LEN = 244;

        public byte ucInputLen;
        public byte ucOutputLen;

        public byte ucCfgDataLen;
        public byte[] aucCfgData = new byte[MAX_CFG_LEN];

        public byte ucUserExtPrmDataLen;
        public byte[] aucUserExtPrmData = new byte[MAX_EXT_PRM_LEN];

        public string strWK_State = "OFF_LINE";      //"OFF_LINE","WAIT_PRM","WAIT_CFG","DATA_EXCHANGE"

        public byte[] aucInputData = new byte[MAX_IO_LEN];
        public byte[] aucOutputData = new byte[MAX_IO_LEN];

    }

    public class CTRANSFER_BUFFER
    {
        public byte[] aucFrame = new byte[255];
        public byte ucFrameLen = 0;
        public byte ucD_Addr = 0;
        public string strMsgType;
        public bool bManuOper = false;
    }


}
