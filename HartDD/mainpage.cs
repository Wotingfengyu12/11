using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public partial class mainpage
    {
        public const int WM_USER = 0x0400;
        public const int WM_SDC_VAR_CHANGED = WM_USER + 10;
        public const int WM_SDC_STRUCTURE_CHANGED = WM_USER + 11;
        public const int WM_SDC_VAR_CHANGEDVALUE = WM_USER + 12;
        public const int WM_SDC_CLOSEDEVICE = WM_USER + 13;
        public const int WM_SDC_GENERATEDEVICE = WM_USER + 14;
        public const int WM_CREATE_EXTENDEDTAB = WM_USER + 15;//Added by ANOOP
        public const int WM_UPDATE_EXTENDEDTAB = WM_USER + 16;//Added by ANOOP
        public const int WM_DESTROY_CMD48DIALOG = WM_USER + 17;//Added by ANOOP
        public const int WM_SDC_GLOW_GREEN = WM_USER + 18;//Added by VMKP on 160204
        public const int WM_SDC_GLOW_RED = WM_USER + 19; //Added by VMKP on 160204
        public const int WM_SDC_SIZECHILD = WM_USER + 20; //Added by POB on 3 Aug 2004
        public const int WM_SDC_ONWINDOW = WM_USER + 21;//Added by POB on 12 Aug 2004
        public const int WM_SDC_ONDIALOG = WM_USER + 22; //Added by POB on 8 Oct 2004

        int maxVer = 10;//max version of  
        public struct DevTypStruct
        {
            public int devrev;
            public int ddrev;
            public int bffver;
            public string ext;// file Extension
        }

        public ThreadUpdate tusend;

        public readonly string[] hartDicts = { "standard.dct", "draeger.dct", "mmi.dct", "endress_hauser.dct", "siemens.dct" };

        HartDictionary dict;

        public cfgInfo hcfg = new cfgInfo();

        public static DDlDevDescription devDesc;

        public const int STANDARD_DEVICE_HANDLE = 1; /*This is the default (reserved) device handle*/
        public const int SDC_DEVICE_HANDLE = 2; /*This has to be separate from the tables     */
        public const int DICTCNT = 5;

        public string strDeviceRevision;
        public string strDDRevision;
        public string strDeviceType;
        public string strManufacturer;

        public string strInputFileName = "";
        public string locLan = "|en|";

        public int eFFVersionMajor = 0;
        public int eFFVersionMinor = 0;

        public bool bafterSel// = false;
        {
            get
            {
                return baftSel();
            }
        }

        public List<DevTypStruct> DevDDList = new List<DevTypStruct>();

        // data receive
        public byte[] rcvbuf = new byte[256];
        public byte rcvlen = 0;
        public byte gucIndResult = 0;//处理ack桢的结果

        public byte gucRcvState = 0;//接收状态

        //定义发送状态
        public const byte MSG_UNPENDING = 0;
        public const byte MSG_PENDING = 1;

        //定义接收状态
        public const byte WAIT_FOR_START = 10;
        const byte RCV_READ_ADDR = 11;
        const byte RCV_READ_CMD = 12;
        const byte RCV_READ_COUNT = 13;
        const byte RCV_READ_RSPCODE = 14;
        const byte RCV_READ_DATA = 15;
        const byte RCV_READ_CHECK = 16;
        const byte WAIT_FOR_END = 17;
        const byte RCV_DONE = 18;
        //接收校验错误
        const byte CHECKBYTE_ERR = 0x01;
        const byte DATA_ERR = 0x02;
        const byte BYTECOUNT_ERR = 0x04;
        const byte COMMAND_ERR = 0x08;
        const byte ADDRESS_ERR = 0x10;
        const byte DELIMITER_ERR = 0x20;
        const byte RSPCODE_ERR = 0x40;
        public HARTRSPINFO gsRspInfo = new HARTRSPINFO();
        bool bParityErr = false;
        byte gucRcvMsgType = MSG_UNKNOWN;
        //定义接收消息帧类型
        const byte MSG_UNKNOWN = 0;
        const byte MSG_REQ = 1;
        const byte MSG_ACK = 2;
        const byte MSG_OACK = 3;
        const byte MSG_BACK = 4;
        const byte MSG_OBACK = 5;
        const byte MSG_ERR = 6;
        //帧地址
        const byte SHORT_FRAME = 0;
        const byte LONG_FRAME = 1;

        public byte PRIMARY_HOST = 0x20;
        byte gucDllDataSeq = 0;

        const byte PREAMBLE_MAX_NUM = 20;
        const byte PREAMBLE_NUM = 6;
        public const byte DATA_MAX_NUM = 64;

        const byte HPS_DATA_FIELD_SIZE = (DATA_MAX_NUM + PREAMBLE_MAX_NUM + 11);

        const byte HART_SUCCESS = 0xF0;
        const byte HART_ERR_RSPERR = 0xFC;
        const byte HART_ERR_NORSP = 0xFD;
        const byte HART_ERR_TIMEOUT = 0xFE;
        const byte HART_ERROR = 0xFF;

        public object cmdRes;

        public virtual object getCmdRes()
        {
            return cmdRes;
        }

        //end data receive

        public HARTDevice hartDev;
        public CVarList pVarList;
        public CVarList pVarListWrite;

        public mainpage()
        {
            strDeviceRevision = "";
            strDDRevision = "";
            strManufacturer = "";
            strDeviceType = "";

            pVarList = new CVarList();
            pVarListWrite = new CVarList();

            hartDev = new HARTDevice(this);

            hcfg.ddRoot = "c:\\hcf\\ddl\\library\\";//by default
        }

        public object getVarListToRefresh()
        {
            return pVarList;
        }

        public object getVarListToWrite()
        {
            return pVarListWrite;
        }

        public void RemoveVarFromWriteList(object rvar)
        {
            pVarListWrite.Remove((CDDLVar)rvar);
        }

        public void setThread(ThreadUpdate tus)
        {
            tusend = tus;
        }

        public void OnUpdate(uint message, uint id)
        {
            updatetype type = 0;
            switch (message)
            {
                case WM_SDC_VAR_CHANGED:
                    {
                        type = updatetype.UPDATE_ADD_DEVICE;
                    }
                    break;
                case WM_SDC_VAR_CHANGEDVALUE:
                    {
                        type = updatetype.UPDATE_ONE_VALUE;
                    }
                    break;
                case WM_SDC_STRUCTURE_CHANGED:
                    {
                        type = updatetype.UPDATE_STRUCTURE;
                    }
                    break;

                default:
                    break;
            }// endswitch

            if (type != 0)
            {
                Update((int)type, id);
            }
        }

        private void Rcv_00(byte[] pData, byte ucLen, StreamWriter sw)
        {
            byte ucOffset = 0;
            for (byte i = 0; i < rcvlen; i++)
            {
                if (rcvbuf[i] == 0xfe)
                {
                    ucOffset = i;
                    break;
                }
            }

            if (/*(gsRspInfo.aucRspCode[0] != NO_COMMAND_SPECIFIC_ERRORS) ||*/
                ucLen < 12 || pData[0 + ucOffset] != 254/* || pData == NULL) ||
                (gsRspInfo.ucPollAddr > 63)*/)
            {
                return;
            }

            {   // break it up
                //ST RC DATA
                //DATA	FE
                //		HH  Mfg						  HH   Mfg				HHHH   Expanded DevType
                //		HH	DevType					  HH   DevType			  
                //		HH	PreAmb					  HH   Reqst-PreAmb		  HH   Reqst-PreAmb
                //		HH  CmdRev-- 5				  HH   CmdRev-- 6		  HH   CmdRev-- 7
                //		HH	DevRev					  HH   DevRev			  HH   DevRev
                //		HH	SoftRev					  HH   SoftRev			  HH   SoftRev
                //		HH	top 5 bits - Hardwr Rev	  5b   Hdwr Rev			  5b   Hdwr Rev
                //			low 3 bits - Signal Code  3b   SignalCd			  3b   SignalCd
                //		HH	FuncFlags				  HH   FuncFlags		  HH   FuncFlags
                //		HHHHHH DeviceID				HHHHHH DeviceID			HHHHHH DeviceID
                //									  HH   Resp-PreAmbl		  HH   Resp-PreAmbl
                //									  HH   Num Dev-Vars		  HH   Num Dev-Vars
                //									HHHH   Config-Chng-Cnt	HHHH   Config-Chng-Cnt
                //									  HH   Extend-Dev Stat    HH   Extend-Dev Stat
                //															HHHH   Mfg Id Code
                ucOffset -= 2;
                if (pData[6 + ucOffset] == 5)
                {
                    hartDev.setVer(5);
                    //rc = SUCCESS;
                    hartDev.ddbDeviceID.wManufacturer = pData[3 + ucOffset];
                    hartDev.ddbDeviceID.wDeviceType = pData[4 + ucOffset];
                    hartDev.ddbDeviceID.cReqPreambles = pData[5 + ucOffset];
                    hartDev.ddbDeviceID.cUniversalRev = pData[6 + ucOffset];
                    hartDev.ddbDeviceID.cDeviceRev = pData[7 + ucOffset];
                    hartDev.ddbDeviceID.cSoftwareRev = pData[8 + ucOffset];
                    hartDev.ddbDeviceID.cHardwareRev = pData[9 + ucOffset];
                    hartDev.ddbDeviceID.cZeroFlags = pData[10 + ucOffset];

                    hartDev.ddbDeviceID.dwDeviceId = (pData[11 + ucOffset] << 16) + (pData[12 + ucOffset] << 8) + pData[13 + ucOffset];
                    hartDev.sLongAddr.ucMfgIDHostAddrBurst = (byte)(((pData[3 + ucOffset]) & 0xf));// | PRIMARY_HOST);// PRIMARY_HOST;
                    hartDev.sLongAddr.ucDevType = pData[4 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Msb = pData[11 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Mib = pData[12 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Lsb = pData[13 + ucOffset];

                }
                else if (pData[6 + ucOffset] == 6)
                {
                    hartDev.setVer(6);
                    hartDev.ddbDeviceID.wManufacturer = pData[3 + ucOffset];
                    hartDev.ddbDeviceID.wDeviceType = pData[4 + ucOffset];
                    hartDev.ddbDeviceID.cReqPreambles = pData[5 + ucOffset];
                    hartDev.ddbDeviceID.cUniversalRev = pData[6 + ucOffset];
                    hartDev.ddbDeviceID.cDeviceRev = pData[7 + ucOffset];
                    hartDev.ddbDeviceID.cSoftwareRev = pData[8 + ucOffset];
                    hartDev.ddbDeviceID.cHardwareRev = pData[9 + ucOffset];
                    hartDev.ddbDeviceID.cZeroFlags = pData[10 + ucOffset];

                    hartDev.ddbDeviceID.cRespPreambles = pData[14 + ucOffset];
                    hartDev.ddbDeviceID.cMaxDevVars = pData[15 + ucOffset];
                    hartDev.ddbDeviceID.wCfgChngCnt = (ushort)((pData[16 + ucOffset] << 8) + pData[17 + ucOffset]);
                    hartDev.ddbDeviceID.cExtDevStatus = pData[18 + ucOffset];

                    hartDev.ddbDeviceID.dwDeviceId = (pData[11 + ucOffset] << 16) + (pData[12 + ucOffset] << 8) + pData[13 + ucOffset];
                    hartDev.sLongAddr.ucMfgIDHostAddrBurst = (byte)(((pData[3 + ucOffset]) & 0xf));// | PRIMARY_HOST);// PRIMARY_HOST;
                    hartDev.sLongAddr.ucDevType = pData[4 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Msb = pData[11 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Mib = pData[12 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Lsb = pData[13 + ucOffset];

                }
                else if (pData[6 + ucOffset] >= 7)/* this is 'forward compatability'  ooook */
                {
                    hartDev.setVer(7);
                    hartDev.ddbDeviceID.wDeviceType = (ushort)((pData[3 + ucOffset] << 8) + pData[4 + ucOffset]);
                    hartDev.ddbDeviceID.cReqPreambles = pData[5 + ucOffset];
                    hartDev.ddbDeviceID.cUniversalRev = pData[6 + ucOffset];
                    hartDev.ddbDeviceID.cDeviceRev = pData[7 + ucOffset];
                    hartDev.ddbDeviceID.cSoftwareRev = pData[8 + ucOffset];
                    hartDev.ddbDeviceID.cHardwareRev = pData[9 + ucOffset];
                    hartDev.ddbDeviceID.cZeroFlags = pData[10 + ucOffset];
                    hartDev.ddbDeviceID.dwDeviceId = (pData[11 + ucOffset] << 16) + (pData[12 + ucOffset] << 8) + pData[13 + ucOffset];
                    hartDev.ddbDeviceID.cRespPreambles = pData[14 + ucOffset];
                    hartDev.ddbDeviceID.cMaxDevVars = pData[15 + ucOffset];
                    hartDev.ddbDeviceID.wCfgChngCnt = (ushort)((pData[16 + ucOffset] << 8) + pData[17 + ucOffset]);
                    hartDev.ddbDeviceID.cExtDevStatus = pData[18 + ucOffset];
                    hartDev.ddbDeviceID.wManufacturer = (ushort)((pData[19 + ucOffset] << 8) + pData[20 + ucOffset]);

                    hartDev.sLongAddr.ucMfgIDHostAddrBurst = (byte)(((pData[3 + ucOffset]) & 0xf));// | PRIMARY_HOST);// PRIMARY_HOST;
                    hartDev.sLongAddr.ucDevType = pData[4 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Msb = pData[11 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Mib = pData[12 + ucOffset];
                    hartDev.sLongAddr.ucDeviceID_Lsb = pData[13 + ucOffset];

                }
                else
                {// unknown universal rev
                    ;
                }
                strManufacturer = string.Format("{0:X6}", hartDev.ddbDeviceID.wManufacturer);// string.Format("0x{0:X4}", pData[17] << 8 + pData[18]);
                strDeviceType = string.Format("{0:X4}", hartDev.ddbDeviceID.wDeviceType);

                strDeviceRevision = string.Format("{0:D}", hartDev.ddbDeviceID.cDeviceRev);
                strDDRevision = string.Format("{0:D}", hartDev.ddbDeviceID.cUniversalRev);

                saveLogfile("Command 0 recevied.");

                saveLogfile("Device wManufacturer " + strManufacturer + "\r\nDeviceType " + strDeviceType);
            }

        }

        void HartResetDev()
        {
            gucRcvState = WAIT_FOR_START;//RCV_DONE;
                                         //    gucRcvMsgType = MSG_UNKNOWN;
                                         //gucDllDataSeq = 0;
        }

        void ProcessDelimiter(byte ucData)
        {
            switch (ucData & 0x07)//Frame Type
            {
                case 1://Burst Frame
                    gucRcvMsgType = MSG_BACK;
                    break;

                case 2://Master to Slave
                    gucRcvMsgType = MSG_REQ;
                    break;
                case 3:
                    gucRcvMsgType = MSG_UNPENDING;
                    break;
                case 6://Slave to Master
                    gucRcvMsgType = MSG_ACK;
                    break;

                default:
                    break;
            }

            if ((ucData & 0x80) != 0)
            {
                gsRspInfo.ucAddrType = LONG_FRAME;
            }
            else
            {
                gsRspInfo.ucAddrType = SHORT_FRAME;
            }
            gsRspInfo.ucDelimiter = ucData;
        }

        public void HartRcvState(byte ucData)
        {
            switch (gucRcvState)
            {
                case WAIT_FOR_START:
                    if (ucData == 0xFF)//preamble
                    {
                        gsRspInfo.ucPreamNum++;
                    }
                    else if (gsRspInfo.ucPreamNum < 2)
                    {
                        HartResetDev();
                    }
                    else
                    {
                        if (bParityErr)
                        {
                            gsRspInfo.ucBitErr |= DELIMITER_ERR;
                        }
                        else if ((ucData & 0x78) == 0 && (ucData & 0x07) != 0 && gsRspInfo.ucPreamNum > 1)//delimiter
                        {
                            ProcessDelimiter(ucData);
                            gucRcvState = RCV_READ_ADDR;
                            gucDllDataSeq = 0;
                            gsRspInfo.ucState = gucRcvMsgType;
                        }
                        else
                        {
                            gucRcvState = WAIT_FOR_END;
                            gucRcvMsgType = MSG_ERR;
                        }
                    }
                    break;

                case RCV_READ_ADDR:
                    if (bParityErr)
                    {
                        gsRspInfo.ucBitErr |= ADDRESS_ERR;
                    }
                    if (gsRspInfo.ucAddrType == SHORT_FRAME)
                    {
                        gsRspInfo.ucShortAddr = ucData;
                        gsRspInfo.ucHostAddr = (byte)(ucData & 0xf0);
                        gsRspInfo.ucPollAddr = (byte)(ucData & 0x0f);
                        gucRcvState = RCV_READ_CMD;
                        if (gsRspInfo.ucHostAddr != PRIMARY_HOST)//是发送到另一个主机的信息
                        {
                            if (gucRcvMsgType == MSG_ACK)
                            {
                                gucRcvMsgType = MSG_OACK;
                            }
                            else if (gucRcvMsgType == MSG_BACK)
                            {
                                gucRcvMsgType = MSG_OBACK;
                            }
                        }
                    }
                    else
                    {
                        if (gucDllDataSeq >= 5)
                        {
                            HartResetDev();
                            break;
                        }
                        gsRspInfo.aucLongAddr[gucDllDataSeq] = ucData;
                        gucDllDataSeq++;
                        if (gucDllDataSeq == 1)
                        {
                            gsRspInfo.ucHostAddr = (byte)(ucData & 0xf0);
                            if (gsRspInfo.ucHostAddr != PRIMARY_HOST)//是发送到另一个主机的信息
                            {
                                if (gucRcvMsgType == MSG_ACK)
                                {
                                    gucRcvMsgType = MSG_OACK;
                                }
                                else if (gucRcvMsgType == MSG_BACK)
                                {
                                    gucRcvMsgType = MSG_OBACK;
                                }
                            }
                        }
                        else if (gucDllDataSeq >= 5)
                        {
                            gucDllDataSeq = 0;
                            gucRcvState = RCV_READ_CMD;
                        }
                    }

                    if (gucRcvMsgType == MSG_OACK || gucRcvMsgType == MSG_OBACK)
                    {
                        //            gucRcvMsgType = MSG_UNKNOWN;
                        //            gucRcvState = WAIT_FOR_START;
                        HartResetDev();
                    }
                    break;

                case RCV_READ_CMD:
                    if (bParityErr)
                    {
                        gsRspInfo.ucBitErr |= COMMAND_ERR;
                    }
                    if (gucRcvMsgType == MSG_ACK)
                    {
                    }

                    gsRspInfo.ucCmd = ucData;
                    gucRcvState = RCV_READ_COUNT;
                    break;

                case RCV_READ_COUNT:
                    if (bParityErr)
                    {
                        gsRspInfo.ucBitErr |= BYTECOUNT_ERR;
                        gucRcvMsgType = MSG_ERR;
                    }
                    if (ucData > DATA_MAX_NUM)
                    {
                        HartResetDev();
                        break;
                    }
                    gsRspInfo.ucByteCount = ucData;
                    gucRcvState = RCV_READ_RSPCODE;
                    gucDllDataSeq = 0;
                    break;
                case RCV_READ_RSPCODE:
                    if (bParityErr)
                    {
                        gsRspInfo.ucBitErr |= RSPCODE_ERR;
                    }

                    if (gucDllDataSeq >= 2)
                    {
                        HartResetDev();
                        break;
                    }
                    gsRspInfo.aucRspCode[gucDllDataSeq] = ucData;
                    gucDllDataSeq++;
                    if (gucDllDataSeq >= 2)
                    {
                        gucDllDataSeq = 0;
                        if (gsRspInfo.ucByteCount == 2)
                        {
                            gucRcvState = RCV_READ_CHECK;
                        }
                        else
                        {
                            gucRcvState = RCV_READ_DATA;
                        }
                    }
                    break;
                case RCV_READ_DATA:
                    if (bParityErr)
                    {
                        gsRspInfo.ucBitErr |= DATA_ERR;
                    }
                    if (gucDllDataSeq >= DATA_MAX_NUM)
                    {
                        HartResetDev();
                        break;
                    }
                    gsRspInfo.aucBuf[gucDllDataSeq] = ucData;
                    gucDllDataSeq++;
                    if (gucDllDataSeq >= (gsRspInfo.ucByteCount - 2))
                    {
                        gucDllDataSeq = 0;
                        gucRcvState = RCV_READ_CHECK;
                    }
                    break;

                case RCV_READ_CHECK:
                    if (bParityErr)
                    {
                        gsRspInfo.ucBitErr |= CHECKBYTE_ERR;
                    }
                    //ResetGapTimer();
                    gsRspInfo.ucCheckSum = ucData;

                    ProcessRcvFrame();

                    gucRcvState = RCV_DONE;
                    // sidnee add
                    //gucTransmit = XMIT_CNFM_OK;
                    //SetEvent(ghSendRcvEvent);
                    break;

                case WAIT_FOR_END:
                    gucRcvState = WAIT_FOR_START;//RCV_DONE;   
                    break;

                case RCV_DONE:
                    if (ucData == 0xFF)//第一个前同步符
                    {
                        gucRcvState = WAIT_FOR_START;
                        gsRspInfo.ucPreamNum = 1;
                    }
                    break;

                default:
                    HartResetDev();
                    break;
            }
        }

        byte ProcessRcvFrame()
        {
            byte ucResult = 0;
            gucIndResult = HART_ERROR;
            if (gucRcvMsgType == MSG_BACK || gucRcvMsgType == MSG_OBACK)// Burst应答桢
            {
                //        SetBurstTimer();        // 设置定时器
            }
            if (gucRcvMsgType != MSG_ACK && gucRcvMsgType != MSG_BACK)//不是该设备的信息
            {
                ucResult = HART_ERROR;    // 错误结果
            }
            else
            {
                if (!DoParity())//有横向校验错误或纵向校验错误
                {
                    ucResult = HART_ERROR;
                }
                //else if ((gsRspInfo.aucRspCode[0] & 0x80) != 0)//响应码不正确
                else if (gsRspInfo.aucRspCode[0] != 0)//响应码不正确
                {
                    //memcpy(&gsRspOldInfo, &gsRspInfo, sizeof(HartRspInfo));
                    //for (int i = 0; i < Marshal.SizeOf(gsRspInfo)
                    ucResult = HART_ERROR;
                }
                //        else if (gsRspInfo.aucRspCode[0] == COMMAND_NOT_IMPLEMENTED)
                //        {
                //            gstrRspCodeResult = "Error:Command Not Implemented";//命令没有执行
                //            ucResult = HART_ERROR;
                //        }
                else
                {
                    /*            g_strRspCodeResult = 
                     -                 GetResString(_T("ErrCode"), 10000000,
                     -                 _T("No Command Specific Errors"));//清除响应码信息
                     -             // 调用应用层和数据链路层接口，按命令种类号调用处理程序*/
                    ucResult = HartIndicateComm(gsRspInfo.ucCmd, gsRspInfo.aucBuf, (byte)(gsRspInfo.ucByteCount - 2));

                    //if (ucResult != HART_ERROR)
                    {
                        //memcpy(&gsRspOldInfo, &gsRspInfo, sizeof(HartRspInfo));
                        //gucTransmit = XMIT_CNFM_OK;
                        //                SetEvent(ghSendRcvEvent);
                        //#ifdef __SUPPORT_BURST_MODE__
                        //                if (gucRcvMsgType == MSG_BACK)
                        //                {
                        //                    SetEvent(ghRcvBurstEvent);//通知界面获得了Burst信息.
                        //                }
                        //#endif
                    }
                    if (gucRcvMsgType == MSG_ACK)
                    {
                        gucIndResult = ucResult;
                    }
                }
            }
            return ucResult;
        }

        byte HartIndicateComm(byte ucCommandNum, byte[] pucData, byte ucLength)
        {
            /*
            if (ucCommandNum == 0xff) //dont process cmd 0
            {

            }
            else
            */
            {
                CDDLCmd cmd;
                CCmdList cmdlist = (CCmdList)hartDev.getListPtr(itemType_t.iT_Command);
                cmd = cmdlist.getCmdByNumber(ucCommandNum);

                if (cmd != null)
                {
                    //foreach(DATA_ITEM di in cmd.list)
                    if (!cmd.setResponseData(tusend.ucTranNumSent, pucData, ucLength))
                    {
                        return HART_ERR_RSPERR;//data set error
                    }
                    Log(String.Format(Resource.CmdRcvOk, ucCommandNum, tusend.ucTranNumSent));
                }
                else
                {
                    return HART_ERROR;
                }

            }

            return HART_SUCCESS;
        }

        bool DoParity()
        {
            byte ucIndex = 0;
            byte ucCheck = 0;
            byte ucMyCheck = 0;
            byte[] aucTempBuf = new byte[HPS_DATA_FIELD_SIZE];
            //byte sddt;

            ucCheck = gsRspInfo.ucCheckSum;
            aucTempBuf[ucIndex] = gsRspInfo.ucDelimiter;
            ucIndex++;
            if (gsRspInfo.ucAddrType == LONG_FRAME)
            {
                //memcpy(&aucTempBuf[ucIndex], &gsRspInfo.aucLongAddr, 5);
                //ucIndex += 5;
                for (int i = 0; i < 5; i++)
                {
                    aucTempBuf[ucIndex++] = gsRspInfo.aucLongAddr[i];
                }
            }
            else
            {
                aucTempBuf[ucIndex] = gsRspInfo.ucShortAddr;
                ucIndex++;
            }
            aucTempBuf[ucIndex] = gsRspInfo.ucCmd;
            ucIndex++;
            aucTempBuf[ucIndex] = gsRspInfo.ucByteCount;
            ucIndex++;
            aucTempBuf[ucIndex] = gsRspInfo.aucRspCode[0];
            aucTempBuf[ucIndex + 1] = gsRspInfo.aucRspCode[1];
            ucIndex += 2;
            if (gsRspInfo.ucByteCount > 2)
            {
                //memcpy(&aucTempBuf[ucIndex], gsRspInfo.aucBuf, (gsRspInfo.ucByteCount - 2));
                //ucIndex += (byte)(gsRspInfo.ucByteCount - 2);
                for (int i = 0; i < gsRspInfo.ucByteCount - 2; i++)
                {
                    aucTempBuf[ucIndex++] = gsRspInfo.aucBuf[i];
                }
            }
            ucMyCheck = Common.CheckSums(aucTempBuf, ucIndex);

            if (ucCheck == ucMyCheck)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        StreamWriter logSw = null;

        public rtNewDev newDevice(/*Identity_s ident*/StreamWriter sw)
        {
            rtNewDev rt = rtNewDev.eSuc;
            logSw = sw;
            DevDDList.Clear();

            hartDev = new HARTDevice(this);

            returncode rc;
            if (!hcfg.bOffline)
            {
                strManufacturer = "";
                rc = GetIdentity(0, sw);//获得设备信息
                if (rc != returncode.eOk)
                {
                    switch (rc)
                    {
                        case returncode.eSerErr:
                            //MessageBox.Show("串口错误");
                            rt = rtNewDev.eCommSerErr;
                            break;

                        default:
                            //MessageBox.Show("未知错误");
                            rt = rtNewDev.eOther;
                            break;
                    }
                    return rt;
                }

                //Thread th = new Thread(RecvData);
                //th.Start();

                returncode creply = RecvData(sw);

                if (creply == returncode.eOk)
                {
                    //string msgRcv = buildStringTypeInfo(rcvlen, rcvbuf);

                    //dispMsgRcv(msgRcv);

                    if (rcvlen != 0)
                    {
                        Rcv_00(rcvbuf, rcvlen, sw);
                        rcvlen = 0;
                    }
                }

                else if (creply == returncode.eTimeOutErr)
                {
                    //MessageBox.Show(Resource.NoRsp, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return rtNewDev.eCommTimeout;
                }

                else if (creply == returncode.eCloseErr)
                {
                    //MessageBox.Show(Resource.NonePort, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return rtNewDev.eCommSerErr;
                }
                //this.Text = await testString();
            }
            else
            {
                //"C:\hcf\ddl\Library\00601e\e188"
                //strManufacturer = "000026";//"00601e";//
                //strDeviceType = "0006";//"e188";//
                //strManufacturer = "00601e";
                //strDeviceType = "e188";
                //strDeviceRevision = "3";//"1";
                //strDDRevision = "7";// "1";
                if (strManufacturer != null && strManufacturer != "")
                {
                    if(strManufacturer.Contains("0x"))
                    {
                        strManufacturer = strManufacturer.Remove(0, 2);
                    }
                    hartDev.ddbDeviceID.wManufacturer = Convert.ToUInt16(strManufacturer, 16);// 0x26;//0x00601e;//
                    if (strDeviceType.Contains("0x"))
                    {
                        strDeviceType = strDeviceType.Remove(0, 2);
                    }
                    hartDev.ddbDeviceID.wDeviceType = Convert.ToUInt16(strDeviceType, 16);// 0x6;//0xe188;//
                }

            }

            rc = identifyDDfile();

            if (rc == returncode.eOk)
            {
                dict = new HartDictionary(locLan);
                if (eFFVersionMajor < 8)
                {// then we have to load extra stuff
                    buildDicitonary();  // uses chDbdir to open and fill
                }
                if (eFFVersionMajor <= 0x08)    // temporary, delete this line for fm8 v 8.3 [2/25/2014 timj]
                {//   fill all of the items & attributes of the abstract device, also dict & strings
                    getDevice();
                }
            }
            else if(rc == returncode.eFileErr)
            {
                return rtNewDev.eDDFailed;
            }
            else
            {
                return rtNewDev.eNoDD;
            }
            return rt;
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


        private returncode buildDicitonary(/*ref Dictionary dict*/)
        {
            string tmpchDbdir;
            string dictfile = "";
            string[] chDictionaryExtensions = new string[DICTCNT];

            tmpchDbdir = hcfg.ddRoot;

            for (int k = 0; k < DICTCNT; k++)
            {
                if (k == 0)// first
                {
                    dictfile = tmpchDbdir + hartDicts[0];
                }
                else
                {
                    chDictionaryExtensions[k - 1] = tmpchDbdir + hartDicts[k];
                }
                if ((k + 1) == DICTCNT)// last
                {
                    chDictionaryExtensions[k] = "";
                }
            }

            if (dict.makedict(dictfile, chDictionaryExtensions) == 0)
            {
                return returncode.eOk;
            }
            else
            {
                return returncode.eDicErr;
            }
        }

        private returncode identifyDDfile()
        {
            returncode rc = returncode.eErr;

            makeDirTree();

            DevTypStruct find = new DevTypStruct();

            scanDirTree(ref find);

            strInputFileName = hcfg.ddRoot + "\\" + strManufacturer + "\\" + strDeviceType + "\\"
                + string.Format("{0:D2}", find.devrev) + string.Format("{0:D2}", find.ddrev) + "." + find.ext;

            Common.chInputFileName = strInputFileName;
            BinaryReader br;
            // 读取文件
            try
            {
                br = new BinaryReader(new FileStream(strInputFileName, FileMode.Open));
            }
            catch (IOException e11)
            {
                Console.WriteLine(e11.Message + "\n Cannot open file.");
                return returncode.eFileErr;
            }

            DDlDevDescription.DDOD_HEADER header = new DDlDevDescription.DDOD_HEADER();
            header.byManufacturer = new byte[3];

            DDlDevDescription.ReadHeader(br, ref header);

            int mfg = header.byManufacturer[0] << 16 |
                      header.byManufacturer[1] << 8 |
                      header.byManufacturer[2];
            header.device_type = (ushort)(((header.device_type & 0x00ff) << 8) |
                                 ((header.device_type & 0xff00) >> 8));

            eFFVersionMajor = header.tok_rev_major;
            eFFVersionMinor = header.tok_rev_minor;
            if (eFFVersionMajor < 5)// FDI version number
            {
                if (eFFVersionMajor == 3)
                    eFFVersionMajor += 7;// translate to hart version
                if (eFFVersionMajor == 4)
                    eFFVersionMajor += 6;// translate to hart version	// fdi4 => hcf10 [4/3/2014 timj]
            }

            if ((eFFVersionMajor != find.bffver) ||
                 (mfg != hartDev.ddbDeviceID.wManufacturer) ||
                 (header.device_type != hartDev.ddbDeviceID.wDeviceType) ||
                 (header.device_revision != find.devrev) ||
                 (header.dd_revision != find.ddrev))
            {// header info does not match filename
             // error message - 
                //MessageBox.Show("Error: DD File header information does not match file name.");
                br.Close();
                return returncode.eFileErr;
            }
            br.Close();

            //////VerificationLevel//??????

            if (DevDDList.Count > 0)
            {
                rc = returncode.eOk;
            }

            return rc;
        }

        bool bUseStandardDD = false;

        private void scanDirTree(ref DevTypStruct retStruct, bool exactMatch = false)
        {
            List<DevTypStruct> DevList = new List<DevTypStruct>();
            List<DevTypStruct> DDList = new List<DevTypStruct>();
            int maxVersion = -1;

            if (bUseStandardDD)
            {
                switch (strDDRevision)
                {
                    case "5":
                        retStruct.devrev = 5;
                        retStruct.ddrev = 6;
                        retStruct.bffver = 8;
                        retStruct.ext = "fm8";
                        break;

                    case "6":
                        retStruct.devrev = 6;
                        retStruct.ddrev = 3;
                        retStruct.bffver = 8;
                        retStruct.ext = "fm8";
                        break;

                    case "7":
                        retStruct.devrev = 7;
                        retStruct.ddrev = 3;
                        retStruct.bffver = 8;
                        retStruct.ext = "fm8";
                        break;

                    default:
                        retStruct.devrev = 5;
                        retStruct.ddrev = 6;
                        retStruct.bffver = 8;
                        retStruct.ext = "fm8";
                        break;

                }
                return;
            }

            foreach (DevTypStruct d in DevDDList)
            {
                if (
                    (exactMatch && d.devrev == Byte.Parse(strDeviceRevision, System.Globalization.NumberStyles.HexNumber))
                    || (!exactMatch && d.devrev >= maxVersion && d.devrev <= Byte.Parse(strDeviceRevision, System.Globalization.NumberStyles.HexNumber))
                    )
                {
                    DevList.Add(d);
                    maxVersion = d.devrev;
                }
            }

            retStruct.devrev = maxVersion;

            maxVersion = -1;
            foreach (DevTypStruct d in DevList)
            {
                if (exactMatch)
                {
                    if (d.ddrev == Byte.Parse(strDDRevision, System.Globalization.NumberStyles.HexNumber))
                    {
                        DDList.Add(d);
                        maxVersion = d.ddrev;
                    }

                }
                else if (retStruct.devrev == d.devrev)
                {
                    if (d.ddrev > maxVersion)
                    {
                        DDList.Clear();
                        maxVersion = d.ddrev;
                    }
                    if (d.ddrev == maxVersion)
                    {
                        DDList.Add(d);
                    }
                }

            }

            retStruct.ddrev = maxVersion;

            maxVersion = -1;
            foreach (DevTypStruct d in DDList)
            {
                if (d.bffver >= maxVersion && d.bffver <= maxVer)
                {
                    maxVersion = d.bffver;
                    retStruct.ext = d.ext;
                }

            }

            retStruct.bffver = maxVersion;

        }

        void saveLogfile(string format, params object[] args)
        {
            logSw.WriteLine(String.Format(format, args));
        }

        private void makeDirTree(bool bSecondMake = false)
        {
            string path = hcfg.ddRoot + "\\" + strManufacturer + "\\" + strDeviceType;
            DevTypStruct DevDD = new DevTypStruct();
            string filename, fullname, fileBase, fileExt;

            DirectoryInfo root = new DirectoryInfo(path);
            try
            {
                FileInfo[] fi = root.GetFiles();
                foreach (FileInfo f in fi)
                {
                    filename = f.Name;
                    fullname = f.FullName;

                    if (filename.Length != 8 || filename.Substring(4, 1) != ".")
                    {
                        //LOGIT(CLOG_LOG, "Bad Name from search '%s'\n", fileName.c_str());
                    }
                    else
                    {//bffVersion
                        fileBase = filename.Substring(0, 4);
                        fileExt = filename.Substring(5, 3);

                        if ((fileExt[0] != 'f' && fileExt[0] != 'F')
                            || (fileExt[1] != 'm' && fileExt[1] != 'M'))
                        {
                            continue;
                        }

                        int devrev = Convert.ToInt32(fileBase.Substring(0, 2), 16);
                        int ddrev = Convert.ToInt32(fileBase.Substring(2, 2), 16);
                        int bffVersion = 5;

                        char fe2 = fileExt[2];
                        if (fe2 == 's' || fe2 == 'S')
                        {
                            bffVersion = 5;
                        }
                        else
                        if (fe2 == '6')
                        {
                            bffVersion = 6;
                        }
                        else
                        if (fe2 == '8')
                        {
                            bffVersion = 8;
                        }
                        else
                        if (fe2 == 'Y' || fe2 == 'y')
                        {
                            bffVersion = 8;             // FMAHACK eff 8 file suffix of .psy temporarily [6/19/2014 timj]
                        }
                        else
                        if (fileExt[2] == 'A' || fileExt[2] == 'a')
                        {
                            bffVersion = 10;
                        }
                        else
                        if (fe2 == 'A')
                        {
                            bffVersion = 10;
                        }
                        else
                        if (fe2 == 'B')
                        {
                            bffVersion = 11;
                        }
                        else
                        if (fe2 == 'C')
                        {
                            bffVersion = 12;
                        }
                        else
                        if (fe2 == 'D')
                        {
                            bffVersion = 13;
                        }
                        else
                        if (fe2 == 'E')
                        {
                            bffVersion = 14;
                        }
                        else
                        if (fe2 == 'F')
                        {
                            bffVersion = 15;
                        }

                        DevDD.devrev = devrev;
                        DevDD.ddrev = ddrev;
                        DevDD.bffver = bffVersion;
                        DevDD.ext = fileExt;
                        DevDDList.Add(DevDD);

                        /*
                        textBox1.Text += filename;
                        textBox1.Text += "\r\n";*/
                    }

                }
            }
            catch (Exception ex)
            {
                /**
                 * 1.异常消息
                 * 2.异常模块名称
                 * 3.异常方法名称
                 * 4.异常行号
                 */
                String str = "";
                str += ex.Message + "\n";//异常消息
                saveLogfile(str);
                                         //str += ex.StackTrace + "\n";//提示出错位置，不会定位到方法内部去
                                         //str += ex.ToString() + "\n";//将方法内部和外部所有出错的位置提示出来
            }
            if (DevDDList.Count == 0)
            {
                if (bSecondMake)
                {
                    return;
                }
                bUseStandardDD = true;
                strManufacturer = hcfg.defaultManufacturer;
                strDeviceType = hcfg.defaultDeviceType;
                hartDev.ddbDeviceID.wManufacturer = UInt16.Parse(strManufacturer, System.Globalization.NumberStyles.HexNumber);// 0x26;//0x00601e;//
                hartDev.ddbDeviceID.wDeviceType = UInt16.Parse(strDeviceType, System.Globalization.NumberStyles.HexNumber);// 0x6;//0xe188;//
                //MessageBox.Show(this, Resource.String.Warning, Resource.String.DDNofFoundUsingDefault, MessageBoxButtons.Close, MessageBoxIcon.Warning);
                saveLogfile("DD file not found, use the default DD.");
                makeDirTree(true);
            }
        }
        const string RESPONSECD_STR = "response_code";
        const string DEVICESTAT_STR = "device_status";
        const string PORT_NUMBR_STR = "port_number";
        const string FUNCTIONCD_STR = "function_code";
        const string MASTERMAXL_STR = "master_max_seg_len";
        const string DEVICEMAXL_STR = "device_max_seg_len";
        const string MASTERBYTC_STR = "master_byte_cntr";
        const string DEVICEBYTC_STR = "device_byte_cntr";

        private returncode getDevice(/*DD_Key_t wrkingKey, aCdevice* pAbstractDev, CDictionary dictionary, LitStringTable* litStrings,*/ bool isInTokenizer = false) // recurse down the entire hierarchy
        {
            returncode rc = returncode.eOk;
            devDesc = new DDlDevDescription(hcfg.ddRoot);
            rc = devDesc.Initialize(strInputFileName, locLan, dict);

            /*
            char erInputFileName[MAX_FILE_NAME_PATH];

            if (false == bRetVal)// can't be null... && NULL != chInputFileName)
            {
                char tmpFileName[MAX_FILE_NAME_PATH];
                unsigned filelen = strlen(chInputFileName) - 4;
                strncpy(erInputFileName, chInputFileName, filelen + 5);/* stevev-for better reporting /
                strncpy(tmpFileName, chInputFileName, filelen);
                tmpFileName[filelen] = '\0';
                if (tmpFileName)
                {
                    strcat(tmpFileName, ".fms");
                    tmpFileName[filelen + 4] = '\0';
                    strcpy(chInputFileName, tmpFileName);
                    chInputFileName[filelen + 4] = '\0';
                    bRetVal = devDesc.Initialize(chInputFileName, "|en|", dictionary);
                }
            }
            */

            if (rc != returncode.eOk)
            {
                return rc;
            }

            int iDevItemListSize = devDesc.ItemsList.Count;
            devDesc.LoadDeviceDescription(isInTokenizer);
            return rc;
        }

        public virtual void Update(int type, uint SymID)
        {

        }

        public virtual returncode USART_Send(byte[] data, byte len, byte add = 0, StreamWriter sw = null)
        {
            return returncode.eOk;
        }

        public virtual void procRcvData(returncode rc, int trannum, uint cmdnum, cmdOperationType_t operation)
        {

        }

        public virtual void UpdataFormReq(byte[] data, byte len, byte cmdNum, byte transNum)
        {
        }

        public virtual bool AddSubMenu(CDDLMenu menu)
        {
            return true;
        }

        public virtual returncode ReData(StreamWriter sw)
        {
            return returncode.eOk;
        }

        public virtual returncode RecvData(StreamWriter sw)
        {
            return returncode.eOk;
        }

        public virtual void dispMsgRcv(string msg)
        {

        }

        public virtual returncode GetIdentity(byte pollAddr, StreamWriter sw)
        {
            //pollAddr,  chksum

            return returncode.eOk;
        }

        public virtual void UpdateData()
        {

        }

        public void Log(string info, LogType type = LogType.Info)
        {
            Log(info, (int)type);
        }

        public virtual void Log(string info, int logtype)
        {

        }

        public virtual bool baftSel()
        {
            return false;
        }

        public virtual IAsyncResult BeginInvoke(Delegate method)
        {
            return null;
        }

        public virtual IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            return null;
        }

    }

    public enum rtNewDev
    {
        eSuc,
        eNoDD,
        eDDFailed,
        eCommTimeout,
        eCommSerErr,
        eOther
    }


}
