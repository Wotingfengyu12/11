using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfibusDP
{
    public class cParseGSD
    {
        //
        //常量定义
        //
        //定义操作状态
        private const byte PROCESS_Normal = 0;
        private const byte PROCESS_Module = 1;
        private const byte PROCESS_PrmText = 2;
        private const byte PROCESS_ExtUserPrmData = 3;

        private string strWholeInfo = "";
        private bool bMoreFlag = false;
        //GSD文件信息集合变量
        public CGSD_FILE_INFO cGsdFileInfoEntity = new CGSD_FILE_INFO();

        private byte ucOperStatus = PROCESS_Normal;

        public void preProcessGSDStringInfo(string strIn)
        {
            if (strIn == "")
            {
                return;
            }

            //
            //去除字符串中左侧的注释（如果有的话）
            //
            int iLocation = strIn.IndexOf(";", 0);          //找到左侧注释的开始位置
            string strTemp = "";
            if (iLocation == -1)//";"符号不存在
            {
                strTemp = string.Copy(strIn);
            }
            else
            {
                strTemp = strIn.Substring(0, iLocation);    //去除注释
            }
            strTemp = strTemp.Trim();                       //去除前后的空格（如果有的话）
            if (strTemp == "")
            {
                return;
            }

            //
            //判断这行是否完整
            //如果字符串右侧是否在"\"符号，代表这行不是完整一行，还未结束，下一行还有
            //
            int iLocationR = strIn.IndexOf("\\", 0);        //找到右侧"\"开始位置
            if (iLocationR == -1)//不存在"\"符号，即这一行是完整的，或者是多行的最后一行
            {
                bMoreFlag = false; //表示这行是最后一行，或者是完整的一行
            }
            else
            {
                bMoreFlag = true;   //表示后面还有
            }
            strTemp = strTemp.Replace("\\", "");        //去除尾部的"\"字符
            strWholeInfo += strTemp;

            if (bMoreFlag)//表示此行不完整，还不能处理
            {
                return;
            }

            if (strWholeInfo == "")
            {
                return;
            }

            //
            //将strWholeInfo的内容赋给strTemp，同时将strWholeInfo清空
            //
            strTemp = strWholeInfo;
            strWholeInfo = "";

            //分析完整的GSD关键字及其内容
            parseGSDKeyWordInfo(strTemp);
        }
        //
        //逐行分析GSD信息
        //由于在GSD文件中可能存在用分行符"\"将一长行分解为几个短行的情况，因此需要考虑将它们合而为一后才能处理
        //
        void parseGSDKeyWordInfo(string strTemp)
        {
            string strTempU = strTemp.ToUpper();

            //
            //已经发现MODULE的开始部分，下面处理其中的内容
            //
            if (ucOperStatus == PROCESS_Module)
            {
                //
                //处理模块引用，如果是纯数据，则是模块引用，即REFERENCE
                //没有字母，此行数字就是模块引用
                //
                bool bRltC = false;
                char[] acChar = strTemp.ToCharArray();
                for (int i = 0; i < acChar.Length; i++)
                {
                    //如果字符串中有非数字字符，则表示其不是模块REF
                    if (Char.IsLetter(acChar[i]))
                    {
                        bRltC = true;
                        break;
                    }
                }
                if (!bRltC)
                {
                    cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].bModule_Ref_Exist = true;
                    cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].uiModule_Ref = ushort.Parse(strTemp);

                    return;
                }
                //
                //Ext_Module_Prm_Data_Len
                //
                if (judgeSingleKeywordIsExisted(strTemp, "Ext_Module_Prm_Data_Len"))
                {
                    int iLocationE = strTemp.IndexOf("=", 0);

                    string strTempM = strTemp.Substring(iLocationE + 1);
                    strTempM = strTempM.Trim(); //去除前后空格

                    cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].ucExt_Module_Prm_Data_Len = (byte)(ushort.Parse(strTempM));
                    return;
                }
                //
                //Ext_User_Prm_Data_Const
                //
                if (strTemp.Contains("Ext_User_Prm_Data_Const") && !strTemp.Contains("F_Ext_User_Prm_Data_Const") && !strTemp.Contains("X_Ext_User_Prm_Data_Const"))
                {
                    int iLocationE = strTemp.IndexOf("(", 0);
                    int iLocationD = strTemp.IndexOf(")", 0);

                    //取出括号之间的索引值，这是起始REF值
                    if (iLocationE != -1 && iLocationD != -1)
                    {
                        CGSD_DATA_CONST_INFO cDataConstTemp = new CGSD_DATA_CONST_INFO();

                        string strTempM = strTemp.Substring(iLocationE + 1, iLocationD - iLocationE - 1);
                        strTempM = strTempM.Trim(); //去除前后空格
                        cDataConstTemp.ucRefBeg = (byte)(ushort.Parse(strTempM)); //担心此值超出255范围(由于用户输入错误的原因) ,所以先转换为16位数字

                        //处理数据部分
                        int iLocationT = strTemp.IndexOf("=", 0);

                        strTempM = strTemp.Substring(iLocationT + 1);
                        strTempM = strTempM.Trim();//去除前后空格

                        string[] strData = strTempM.Split(new char[] { ',' });
                        for (int i = 0; i < strData.Length; i++)
                        {
                            strData[i] = strData[i].Trim(); //去除前后空格
                            if (strData[i].Contains("0X") || strData[i].Contains("0x"))//HEX格式
                            {
                                string strTTT = strData[i].Substring(2, strData[i].Length - 2);
                                cDataConstTemp.aucData[i] = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                            }
                            else
                            {
                                cDataConstTemp.aucData[i] = (byte)(ushort.Parse(strData[i]));
                            }
                        }
                        cDataConstTemp.ucDataLenL = (byte)strData.Length;
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].Ext_User_Prm_Data_Const_List.Add(cDataConstTemp);
                    }
                    return;
                }
                //
                //Ext_User_Prm_Data_Ref
                //
                if (strTemp.Contains("Ext_User_Prm_Data_Ref") && !strTemp.Contains("F_Ext_User_Prm_Data_Ref") && !strTemp.Contains("X_Ext_User_Prm_Data_Ref"))
                {
                    int iLocationE = strTemp.IndexOf("(", 0);
                    int iLocationD = strTemp.IndexOf(")", 0);

                    //取出括号之间的索引值，这是起始REF值
                    if (iLocationE != -1 && iLocationD != -1)
                    {
                        CGSD_DATA_REF_INFO cDataRefTemp = new CGSD_DATA_REF_INFO();

                        string strTempM = strTemp.Substring(iLocationE + 1, iLocationD - iLocationE - 1);
                        strTempM = strTempM.Trim(); //去除前后空格
                        cDataRefTemp.ucRefBeg = (byte)(ushort.Parse(strTempM)); //担心此值超出255范围(由于用户输入错误的原因) ,所以先转换为16位数字

                        //处理数据部分
                        int iLocationT = strTemp.IndexOf("=", 0);
                        strTempM = strTemp.Substring(iLocationT + 1);
                        strTempM = strTempM.Trim();//去除前后空格

                        if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                            cDataRefTemp.uiData = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cDataRefTemp.uiData = ushort.Parse(strTempM);
                        }
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].Ext_User_Prm_Data_Ref_List.Add(cDataRefTemp);
                    }

                    return;
                }
                //
                //F_Ext_Module_Prm_Data_Len
                //
                if (judgeSingleKeywordIsExisted(strTemp, "F_Ext_Module_Prm_Data_Len"))
                {
                    int iLocationE = strTemp.IndexOf("=", 0);

                    string strTempM = strTemp.Substring(iLocationE + 1);
                    strTempM = strTempM.Trim(); //去除前后空格

                    cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].ucF_Ext_Module_Prm_Data_Len = (byte)(ushort.Parse(strTempM));
                    return;
                }
                //
                //F_Ext_User_Prm_Data_Const
                //
                if (strTemp.Contains("F_Ext_User_Prm_Data_Const"))
                {
                    int iLocationE = strTemp.IndexOf("(", 0);
                    int iLocationD = strTemp.IndexOf(")", 0);

                    //取出括号之间的索引值，这是起始REF值
                    if (iLocationE != -1 && iLocationD != -1)
                    {
                        CGSD_DATA_CONST_INFO cDataConstTemp = new CGSD_DATA_CONST_INFO();

                        string strTempM = strTemp.Substring(iLocationE + 1, iLocationD - iLocationE - 1);
                        strTempM = strTempM.Trim(); //去除前后空格
                        cDataConstTemp.ucRefBeg = (byte)(ushort.Parse(strTempM)); //担心此值超出255范围(由于用户输入错误的原因) ,所以先转换为16位数字

                        //处理数据部分
                        int iLocationT = strTemp.IndexOf("=", 0);

                        strTempM = strTemp.Substring(iLocationT + 1);
                        strTempM = strTempM.Trim();//去除前后空格

                        string[] strData = strTempM.Split(new char[] { ',' });
                        for (int i = 0; i < strData.Length; i++)
                        {
                            strData[i] = strData[i].Trim(); //去除前后空格
                            if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                            {
                                string strTTT = strData[i].Substring(2, strData[i].Length - 2);
                                cDataConstTemp.aucData[i] = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                            }
                            else
                            {
                                cDataConstTemp.aucData[i] = (byte)(ushort.Parse(strData[i]));
                            }
                        }
                        cDataConstTemp.ucDataLenL = (byte)strData.Length;
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].F_Ext_User_Prm_Data_Const_List.Add(cDataConstTemp);
                    }

                    return;
                }
                //
                //F_Ext_User_Prm_Data_Ref
                //
                if (strTemp.Contains("F_Ext_User_Prm_Data_Ref"))
                {
                    int iLocationE = strTemp.IndexOf("(", 0);
                    int iLocationD = strTemp.IndexOf(")", 0);

                    //取出括号之间的索引值，这是起始REF值
                    if (iLocationE != -1 && iLocationD != -1)
                    {
                        CGSD_DATA_REF_INFO cDataRefTemp = new CGSD_DATA_REF_INFO();

                        string strTempM = strTemp.Substring(iLocationE + 1, iLocationD - iLocationE - 1);
                        strTempM = strTempM.Trim(); //去除前后空格
                        cDataRefTemp.ucRefBeg = (byte)(ushort.Parse(strTempM)); //担心此值超出255范围(由于用户输入错误的原因) ,所以先转换为16位数字

                        //处理数据部分
                        int iLocationT = strTemp.IndexOf("=", 0);
                        strTempM = strTemp.Substring(iLocationT + 1);
                        strTempM = strTempM.Trim();//去除前后空格

                        if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                            cDataRefTemp.uiData = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cDataRefTemp.uiData = ushort.Parse(strTempM);
                        }
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].F_Ext_User_Prm_Data_Ref_List.Add(cDataRefTemp);
                    }

                    return;
                }
                //
                //F_IO_StructureDescCRC
                //
                if (judgeSingleKeywordIsExisted(strTemp, "F_IO_StructureDescCRC"))
                {
                    int iLocationE = strTemp.IndexOf("=", 0);
                    if (iLocationE != -1)
                    {
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].bF_IO_StructureDescCRC_Exist = true;

                        string strTempM = strTemp.Substring(iLocationE + 1);
                        strTempM = strTempM.Trim(); //去除前后空格

                        if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                            cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].ulF_IO_StructureDescCRC = ulong.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].ulF_IO_StructureDescCRC = ulong.Parse(strTempM);
                        }
                    }
                    return;
                }
                //
                //F_ParamDescCRC
                //
                if (judgeSingleKeywordIsExisted(strTemp, "F_ParamDescCRC"))
                {
                    int iLocationE = strTemp.IndexOf("=", 0);
                    if (iLocationE != -1)
                    {
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].bF_ParamDescCRC_Exist = true;

                        string strTempM = strTemp.Substring(iLocationE + 1);
                        strTempM = strTempM.Trim(); //去除前后空格

                        if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                            cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].uiF_ParamDescCRC = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].uiF_ParamDescCRC = ushort.Parse(strTempM);
                        }
                    }
                    return;
                }
                //
                //Preset
                //
                if (judgeSingleKeywordIsExisted(strTemp, "Preset"))
                {
                    int iLocationE = strTemp.IndexOf("=", 0);
                    if (iLocationE != -1)
                    {
                        cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].bPreset_Exist = true;

                        //处理数据部分
                        string strTempM = strTemp.Substring(iLocationE + 1);
                        strTempM = strTempM.Trim();//去除前后空格

                        if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                            cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].ucPreset = (byte)(ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber));
                        }
                        else
                        {
                            cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].ucPreset = (byte)(ushort.Parse(strTempM));
                        }
                    }

                    return;
                }
                //
                //Channel_Diag
                //
                if (strTempU.Contains("CHANNEL_DIAG") && !strTempU.Contains("CHANNEL_DIAG_HELP"))
                {
                    CCHANNEL_DIAG cChannelDiagNode = new CCHANNEL_DIAG();

                    //Error_Type
                    int iLocationL = strTemp.IndexOf("(");
                    int iLocationR = strTemp.IndexOf(")");
                    if (iLocationL == -1 || iLocationR == -1)
                    {
                        return;
                    }

                    string strBit = strTemp.Substring(iLocationL + 1, iLocationR - 1 - iLocationL);
                    strBit = strBit.Trim();

                    if (strBit.Contains("0x") || strBit.Contains("0X"))
                    {
                        return;
                    }
                    ushort uiTemp16 = ushort.Parse(strBit);
                    cChannelDiagNode.ucError_Type = (byte)uiTemp16;

                    //Diag_Text
                    int iLocationE = strTemp.IndexOf("=");
                    if (iLocationE == -1)
                    {
                        return;
                    }

                    cChannelDiagNode.strDiag_Text = strTemp.Substring(iLocationE + 2, strTemp.Length - (iLocationE + 2));
                    cChannelDiagNode.strDiag_Text = cChannelDiagNode.strDiag_Text.Trim();

                    cGsdFileInfoEntity.asModuleInfoList[cGsdFileInfoEntity.asModuleInfoList.Count - 1].Channel_Diag_List.Add(cChannelDiagNode);

                    return;
                }
                //
                //"EndModule"
                //
                if (judgeSingleKeywordIsExisted(strTemp, "EndModule"))
                {
                    ucOperStatus = PROCESS_Normal;
                    return;
                }

                return;
            }
            //
            //处理ExtUserPrmData关键字内部的信息
            //
            if (ucOperStatus == PROCESS_ExtUserPrmData)
            {
                //
                //Prm_Text_Ref
                //
                if (judgeSingleKeywordIsExisted(strTemp, "Prm_Text_Ref"))
                {
                    int iLocationE = strTemp.IndexOf("=", 0);
                    if (iLocationE != -1)
                    {
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].bPrm_Text_Ref_Exist = true;

                        //处理数据部分
                        string strTempM = strTemp.Substring(iLocationE + 1);
                        strTempM = strTempM.Trim();//去除前后空格

                        if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                            cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].uiPrm_Text_Ref = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].uiPrm_Text_Ref = ushort.Parse(strTempM);
                        }
                    }

                    return;
                }

                //
                //分析Data_Type_Name
                //
                if (strTempU.Contains("UNSIGNED8"))
                {
                    if (!strTemp.Contains("-") && !strTemp.Contains(","))
                    {
                        return;
                    }

                    cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].strData_Type_Name = "Unsigned8";

                    string[] strData = strTemp.Split(new char[] { ' ' });
                    if (strData.Length < 3)//至少分为三部分
                    {
                        return;
                    }

                    //Default_Value
                    strData[1] = strData[1].Trim();
                    if (strData[1].Contains("0x") || strData[1].Contains("0X"))//HEX格式
                    {
                        string strTTT = strData[1].Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_USIGN8.ucDefault_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strData[1]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_USIGN8.ucDefault_Value = (byte)uiTemp16;
                    }

                    //第二个空格以后就是最小值与最大值的组合，二者之间一般用"-"字符分析开来，但也可能就","字符分开，这个组合之间由于用户
                    //书写习惯不同，也可能有空格出现，这在处理过程中需要注意。
                    //Min/Max_Value                    
                    int iLocationF = strTemp.IndexOf(" ");      //先找到第一个空格的位置
                    if (iLocationF == -1)
                    {
                        return;
                    }

                    string strTempF = strTemp.Substring(iLocationF + 1);    //生成第一个空格之后的字符串
                    strTempF = strTempF.Trim();

                    int iLocationS = strTempF.IndexOf(" ");      //在新串中找到第一个空格的位置
                    if (iLocationS == -1)
                    {
                        return;
                    }

                    string strTempS = strTempF.Substring(iLocationS + 1);    //生成第二个空格之后的字符串，而这个字符串就是最小值与最大值的组合
                    strTempS = strTempS.Trim();

                    char cTempD = ' ';
                    if (strTempS.Contains("-"))
                    {
                        cTempD = '-';
                    }
                    else if (strTempS.Contains(","))
                    {
                        cTempD = ',';
                    }

                    string[] strDataN = strTempS.Split(new char[] { cTempD });

                    //Min
                    strDataN[0] = strDataN[0].Trim();
                    if (strDataN[0].Contains("0x") || strDataN[0].Contains("0X"))//HEX格式
                    {
                        string strTTT = strDataN[0].Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_USIGN8.ucMin_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strDataN[0]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_USIGN8.ucMin_Value = (byte)uiTemp16;
                    }

                    //Max
                    strDataN[1] = strDataN[1].Trim();
                    if (strDataN[1].Contains("0x") || strDataN[1].Contains("0X"))//HEX格式
                    {
                        string strTTT = strDataN[1].Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_USIGN8.ucMax_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strDataN[1]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_USIGN8.ucMax_Value = (byte)uiTemp16;
                    }

                    return;
                }
                else if (strTempU.Contains("BIT") && !strTempU.Contains("BITAREA"))
                {
                    if (!strTemp.Contains("-") && !strTemp.Contains(","))
                    {
                        return;
                    }

                    cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].strData_Type_Name = "Bit";

                    string[] strData = strTemp.Split(new char[] { ' ' });
                    if (strData.Length < 3)////至少分为三部分
                    {
                        return;
                    }

                    //
                    //Ref
                    //
                    int iLocationB1 = strTemp.IndexOf("(");      //先找到"("的位置
                    int iLocationB2 = strTemp.IndexOf(")");      //先找到")"的位置
                    if (iLocationB1 == -1 || iLocationB2 == -1)
                    {
                        return;
                    }

                    string strRef = strTemp.Substring(iLocationB1 + 1, iLocationB2 - 1 - iLocationB1);
                    strRef = strRef.Trim();
                    if (strRef.Contains("0x") || strRef.Contains("0X"))//HEX格式
                    {
                        string strTTT = strRef.Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucBitRef = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strRef);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucBitRef = (byte)uiTemp16;
                    }

                    //
                    //Default_Value
                    //
                    string strTempD = strTemp.Substring(iLocationB2 + 1);
                    strTempD = strTempD.Trim();
                    int iLocationS = strTempD.IndexOf(" ");      //先找到第一个空格的位置
                    if (iLocationS == -1)
                    {
                        return;
                    }
                    string strTempD2 = strTempD.Substring(0, iLocationS);
                    strTempD2 = strTempD2.Trim();
                    if (strTempD2.Contains("0x") || strTempD2.Contains("0X"))//HEX格式
                    {
                        string strTTT = strTempD2.Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucDefault_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strTempD2);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucDefault_Value = (byte)uiTemp16;
                    }

                    //
                    //Min/Max
                    //
                    //第二个空格以后就是最小值与最大值的组合，二者之间一般用"-"字符分析开来，但也可能就","字符分开，这个组合之间由于用户
                    //书写习惯不同，也可能有空格出现，这在处理过程中需要注意。
                    //Min/Max_Value         
                    string strTempF = strTempD.Substring(iLocationS + 1);
                    strTempF = strTempF.Trim();

                    char cTempD = ' ';
                    if (strTempF.Contains("-"))
                    {
                        cTempD = '-';
                    }
                    else if (strTempF.Contains(","))
                    {
                        cTempD = ',';
                    }

                    string[] strDataN = strTempF.Split(new char[] { cTempD });

                    //Min
                    strDataN[0] = strDataN[0].Trim();
                    if (strDataN[0].Contains("0x") || strDataN[0].Contains("0X"))//HEX格式
                    {
                        string strTTT = strDataN[0].Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucMin_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strDataN[0]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucMin_Value = (byte)uiTemp16;
                    }

                    //Max
                    strDataN[1] = strDataN[1].Trim();
                    if (strDataN[1].Contains("0x") || strDataN[1].Contains("0X"))//HEX格式
                    {
                        string strTTT = strDataN[1].Substring(2);
                        ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucMax_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strDataN[1]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BIT.ucMax_Value = (byte)uiTemp16;
                    }

                    return;
                }
                else if (strTempU.Contains("BITAREA"))
                {
                    if (!strTemp.Contains("-") && !strTemp.Contains(","))
                    {
                        return;
                    }

                    cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].strData_Type_Name = "BitArea";

                    string[] strData = strTemp.Split(new char[] { ' ' });
                    if (strData.Length < 3)////至少分为三部分
                    {
                        return;
                    }

                    //
                    //Ref
                    //
                    int iLocationB1 = strTemp.IndexOf("(");      //先找到"("的位置
                    int iLocationB2 = strTemp.IndexOf(")");      //先找到")"的位置
                    if (iLocationB1 == -1 || iLocationB2 == -1)
                    {
                        return;
                    }
                    string strRef = strTemp.Substring(iLocationB1 + 1, iLocationB2 - 1 - iLocationB1);
                    strRef = strRef.Trim();

                    //FirstBit和LastBit之间通过'-'符号分开，
                    string[] strDataR = strRef.Split(new char[] { '-' });

                    //FirstBit
                    if (strDataR[0].Contains("0x") || strDataR[0].Contains("0X"))
                    {
                        return;
                    }
                    ushort uiTemp16 = ushort.Parse(strDataR[0]);      //这么多做是为了防止用户给出的数据超出byte的范围
                    cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucFirstBit = (byte)uiTemp16;

                    //LastBit
                    if (strDataR[1].Contains("0x") || strDataR[1].Contains("0X"))
                    {
                        return;
                    }
                    uiTemp16 = ushort.Parse(strDataR[1]);      //这么多做是为了防止用户给出的数据超出byte的范围
                    cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucLastBit = (byte)uiTemp16;

                    //
                    //Default_Value
                    //
                    string strTempD = strTemp.Substring(iLocationB2 + 1);
                    strTempD = strTempD.Trim();
                    int iLocationS = strTempD.IndexOf(" ");      //先找到第一个空格的位置
                    if (iLocationS == -1)
                    {
                        return;
                    }
                    string strTempD2 = strTempD.Substring(0, iLocationS);
                    strTempD2 = strTempD2.Trim();
                    if (strTempD2.Contains("0x") || strTempD2.Contains("0X"))//HEX格式
                    {
                        string strTTT = strTempD2.Substring(2);
                        uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucDefault_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        uiTemp16 = ushort.Parse(strTempD2);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucDefault_Value = (byte)uiTemp16;
                    }

                    //
                    //Min/Max
                    //
                    //第二个空格以后就是最小值与最大值的组合，二者之间一般用"-"字符分析开来，但也可能就","字符分开，这个组合之间由于用户
                    //书写习惯不同，也可能有空格出现，这在处理过程中需要注意。
                    //Min/Max_Value         
                    string strTempF = strTempD.Substring(iLocationS + 1);
                    strTempF = strTempF.Trim();

                    char cTempD = ' ';
                    if (strTempF.Contains("-"))
                    {
                        cTempD = '-';
                    }
                    else if (strTempF.Contains(","))
                    {
                        cTempD = ',';
                    }

                    string[] strDataN = strTempF.Split(new char[] { cTempD });

                    //Min
                    strDataN[0] = strDataN[0].Trim();
                    if (strDataN[0].Contains("0x") || strDataN[0].Contains("0X"))//HEX格式
                    {
                        string strTTT = strDataN[0].Substring(2);
                        uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucMin_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        uiTemp16 = ushort.Parse(strDataN[0]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucMin_Value = (byte)uiTemp16;
                    }

                    //Max
                    strDataN[1] = strDataN[1].Trim();
                    if (strDataN[1].Contains("0x") || strDataN[1].Contains("0X"))//HEX格式
                    {
                        string strTTT = strDataN[1].Substring(2);
                        uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucMax_Value = (byte)uiTemp16;
                    }
                    else
                    {
                        uiTemp16 = ushort.Parse(strDataN[1]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cGsdFileInfoEntity.ExtUserPrmData_List[cGsdFileInfoEntity.ExtUserPrmData_List.Count - 1].cDataInfo_BITAREA.ucMax_Value = (byte)uiTemp16;
                    }

                    return;
                }
                //aaa

                //
                //EndExtUserPrmData
                //
                if (judgeSingleKeywordIsExisted(strTemp, "EndExtUserPrmData"))
                {
                    ucOperStatus = PROCESS_Normal;
                    return;
                }

            }

            //
            //处理PrmText关键字内部的信息
            //
            if (ucOperStatus == PROCESS_PrmText)
            {
                //
                //Text
                //
                if (strTemp.Contains("Text("))
                {
                    int iLocationB = strTemp.IndexOf("(", 0);
                    int iLocationE = strTemp.IndexOf(")", 0);
                    if (iLocationB != -1 && iLocationE != -1)
                    {
                        CPRM_TEXT_ITEM_INFO cPrmTextItemNode = new CPRM_TEXT_ITEM_INFO();

                        //ID
                        string strTempP = strTemp.Substring(iLocationB + 1, iLocationE - 1 - iLocationB);
                        strTempP = strTempP.Trim();
                        if (strTempP.Contains("0X") || strTempP.Contains("0x"))//HEX格式
                        {
                            string strTTT = strTempP.Substring(2, strTempP.Length - 2);
                            cPrmTextItemNode.uiID = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cPrmTextItemNode.uiID = ushort.Parse(strTempP);
                        }

                        //TEXT
                        int iLocationT = strTemp.IndexOf("=", 0);
                        cPrmTextItemNode.strText = strTemp.Substring(iLocationT + 1);
                        cPrmTextItemNode.strText = cPrmTextItemNode.strText.Trim();
                        cPrmTextItemNode.strText = cPrmTextItemNode.strText.Replace("\"", "");        //去除尾部的"\""字符

                        cGsdFileInfoEntity.PrmText_List[cGsdFileInfoEntity.PrmText_List.Count - 1].asPrm_Text_Item_List.Add(cPrmTextItemNode);

                        return;
                    }
                }

                //
                //"EndPrmText"
                //

                if (judgeSingleKeywordIsExisted(strTemp, "EndPrmText"))
                {
                    ucOperStatus = PROCESS_Normal;
                    return;
                }



                return;
            }

            //
            //4.2 General specifications
            //4.2.1 General DP keywords
            //
            //GSD_Revision
            //
            if (judgeSingleKeywordIsExisted(strTemp, "GSD_Revision"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucGSD_Revision))
                {
                    cGsdFileInfoEntity.bGSD_Revision_Exist = true;
                }
            }
            //
            //Vendor_Name
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Vendor_Name"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strVendor_Name))
                {
                    cGsdFileInfoEntity.bVendor_Name_Exist = true;
                }
            }
            //
            //Model_Name
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Model_Name"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strModel_Name))
                {
                    cGsdFileInfoEntity.bModel_Name_Exist = true;
                }
            }
            //
            //Revision
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Revision"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strRevision))
                {
                    cGsdFileInfoEntity.bRevision_Exist = true;
                }
            }
            //
            //Revision_Number
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Revision_Number"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucRevision_Number))
                {
                    cGsdFileInfoEntity.bRevision_Number_Exist = true;
                }
            }
            //
            //Ident_Number
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Ident_Number"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiIdent_Number))
                {
                    cGsdFileInfoEntity.bIdent_Number_Exist = true;
                }
            }
            //
            //Protocol_Ident
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Protocol_Ident"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucProtocol_Ident))
                {
                    cGsdFileInfoEntity.bProtocol_Ident_Exist = true;
                }
            }
            //
            //Station_Type
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Station_Type"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucStation_Type))
                {
                    cGsdFileInfoEntity.bStation_Type_Exist = true;
                }
            }
            //
            //FMS_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "FMS_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bFMS_supp))
                {
                    cGsdFileInfoEntity.bFMS_supp_Exist = true;
                }
            }
            //
            //Hardware_Release
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Hardware_Release"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strHardware_Release))
                {
                    cGsdFileInfoEntity.bHardware_Release_Exist = true;
                }
            }
            //
            //Software_Release
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Software_Release"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strSoftware_Release))
                {
                    cGsdFileInfoEntity.bSoftware_Release_Exist = true;
                }
            }
            //
            //9.6_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "9.6_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_9_6_supp))
                {
                    cGsdFileInfoEntity.bBaud_9_6_supp_Exist = true;
                }
            }
            //
            //19.2_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "19.2_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_19_2_supp))
                {
                    cGsdFileInfoEntity.bBaud_19_2_supp_Exist = true;
                }
            }
            //
            //45.45_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "45.45_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_45_45_supp))
                {
                    cGsdFileInfoEntity.bBaud_45_45_supp_Exist = true;
                }
            }
            //
            //93.75_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "93.75_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_93_75_supp))
                {
                    cGsdFileInfoEntity.bBaud_93_75_supp_Exist = true;
                }
            }
            //
            //187.5_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "187.5_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_187_5_supp))
                {
                    cGsdFileInfoEntity.bBaud_187_5_supp_Exist = true;
                }
            }
            //
            //500_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "500_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_500_supp))
                {
                    cGsdFileInfoEntity.bBaud_500_supp_Exist = true;
                }
            }
            //
            //1.5M_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "1.5M_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_1_5M_supp))
                {
                    cGsdFileInfoEntity.bBaud_1_5M_supp_Exist = true;
                }
            }
            //
            //3M_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "3M_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_3M_supp))
                {
                    cGsdFileInfoEntity.bBaud_3M_supp_Exist = true;
                }
            }
            //
            //6M_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "6M_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_6M_supp))
                {
                    cGsdFileInfoEntity.bBaud_6M_supp_Exist = true;
                }
            }
            //
            //12M_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "12M_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bBaud_12M_supp))
                {
                    cGsdFileInfoEntity.bBaud_12M_supp_Exist = true;
                }
            }
            //
            //MaxTsdr_9.6
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_9.6"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_9_6))
                {
                    cGsdFileInfoEntity.bMaxTsdr_9_6_Exist = true;
                }
            }
            //
            //MaxTsdr_19.2
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_19.2"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_19_2))
                {
                    cGsdFileInfoEntity.bMaxTsdr_19_2_Exist = true;
                }
            }
            //
            //MaxTsdr_45.45
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_45.45"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_45_45))
                {
                    cGsdFileInfoEntity.bMaxTsdr_45_45_Exist = true;
                }
            }
            //
            //MaxTsdr_93.75
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_93.75"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_93_75))
                {
                    cGsdFileInfoEntity.bMaxTsdr_93_75_Exist = true;
                }
            }
            //
            //MaxTsdr_187.5
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_187.5"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_187_5))
                {
                    cGsdFileInfoEntity.bMaxTsdr_187_5_Exist = true;
                }
            }
            //
            //MaxTsdr_500
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_500"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_500))
                {
                    cGsdFileInfoEntity.bMaxTsdr_500_Exist = true;
                }
            }
            //
            //MaxTsdr_1.5M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_1.5M"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_1_5M))
                {
                    cGsdFileInfoEntity.bMaxTsdr_1_5M_Exist = true;
                }
            }
            //
            //MaxTsdr_3M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_3M"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_3M))
                {
                    cGsdFileInfoEntity.bMaxTsdr_3M_Exist = true;
                }
            }
            //
            //MaxTsdr_6M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_6M"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_6M))
                {
                    cGsdFileInfoEntity.bMaxTsdr_6M_Exist = true;
                }
            }
            //
            //MaxTsdr_12M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "MaxTsdr_12M"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMaxTsdr_12M))
                {
                    cGsdFileInfoEntity.bMaxTsdr_12M_Exist = true;
                }
            }
            //
            //Redundancy
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Redundancy"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bRedundancy))
                {
                    cGsdFileInfoEntity.bRedundancy_Exist = true;
                }
            }
            //
            //Repeater_Ctrl_Sig
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Repeater_Ctrl_Sig"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucRepeater_Ctrl_Sig))
                {
                    cGsdFileInfoEntity.bRepeater_Ctrl_Sig_Exist = true;
                }
            }
            //
            //24V_Pins
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "24V_Pins"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.uc24V_Pins))
                {
                    cGsdFileInfoEntity.b24V_Pins_Exist = true;
                }
            }
            //
            //Implementation_Type
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Implementation_Type"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strImplementation_Type))
                {
                    cGsdFileInfoEntity.bImplementation_Type_Exist = true;
                }
            }
            //
            //Bitmap_Device
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Bitmap_Device"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strBitmap_Device))
                {
                    cGsdFileInfoEntity.bBitmap_Device_Exist = true;
                }
            }
            //
            //Bitmap_Diag
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Bitmap_Diag"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strBitmap_Diag))
                {
                    cGsdFileInfoEntity.bBitmap_Diag_Exist = true;
                }
            }
            //
            //Bitmap_SF
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Bitmap_SF"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strBitmap_SF))
                {
                    cGsdFileInfoEntity.bBitmap_SF_Exist = true;
                }
            }
            //
            //OrderNumber
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "OrderNumber"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strOrderNumber))
                {
                    cGsdFileInfoEntity.bOrderNumber_Exist = true;
                }
            }
            //
            //4.2 General specifications
            //4.2.2 dditional keywords for different physical interfaces
            //
            //暂不支持

            //
            //4.3 Master-related specifications
            //4.3.1 DP Master (Class 1) related keywords
            //
            //
            //Master_Freeze_Mode_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Freeze_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Freeze_Mode_supp))
                {
                    cGsdFileInfoEntity.bMaster_Freeze_Mode_supp_Exist = true;
                }
            }
            //
            //Master_Sync_Mode_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Sync_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Sync_Mode_supp))
                {
                    cGsdFileInfoEntity.bMaster_Sync_Mode_supp_Exist = true;
                }
            }
            //
            //Master_Fail_Safe_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Sync_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Fail_Safe_supp))
                {
                    cGsdFileInfoEntity.bMaster_Fail_Safe_supp_Exist = true;
                }
            }
            //
            //Download_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Download_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDownload_supp))
                {
                    cGsdFileInfoEntity.bDownload_supp_Exist = true;
                }
            }
            //
            //Upload_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Upload_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bUpload_supp))
                {
                    cGsdFileInfoEntity.bUpload_supp_Exist = true;
                }
            }
            //
            //Act_Para_Brct_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Upload_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bAct_Para_Brct_supp))
                {
                    cGsdFileInfoEntity.bAct_Para_Brct_supp_Exist = true;
                }
            }
            //
            //Act_Param_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Act_Param_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bAct_Param_supp))
                {
                    cGsdFileInfoEntity.bAct_Param_supp_Exist = true;
                }
            }
            //
            //Max_MPS_Length
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_MPS_Length"))
            {
                if (parseGsdKeyWord_USIGN32(strTemp, ref cGsdFileInfoEntity.ulMax_MPS_Length))
                {
                    cGsdFileInfoEntity.bMax_MPS_Length_Exist = true;
                }
            }
            //
            //Max_Lsdu_MS
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Lsdu_MS"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Lsdu_MS))
                {
                    cGsdFileInfoEntity.bMax_Lsdu_MS_Exist = true;
                }
            }
            //
            //Max_Lsdu_MM
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Lsdu_MM"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Lsdu_MM))
                {
                    cGsdFileInfoEntity.bMax_Lsdu_MM_Exist = true;
                }
            }
            //
            //Min_Poll_Timeout
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Min_Poll_Timeout"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMin_Poll_Timeout))
                {
                    cGsdFileInfoEntity.bMin_Poll_Timeout_Exist = true;
                }
            }
            //
            //Trdy_9.6
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_9.6"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_9_6))
                {
                    cGsdFileInfoEntity.bTrdy_9_6_Exist = true;
                }
            }
            //
            //Trdy_19.2
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_19.2"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_19_2))
                {
                    cGsdFileInfoEntity.bTrdy_19_2_Exist = true;
                }
            }
            //
            //Trdy_31.25
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_31.25"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_31_25))
                {
                    cGsdFileInfoEntity.bTrdy_31_25_Exist = true;
                }
            }
            //
            //Trdy_45.45
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_45.45"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_45_45))
                {
                    cGsdFileInfoEntity.bTrdy_45_45_Exist = true;
                }
            }
            //
            //Trdy_93.75
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_93.75"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_93_75))
                {
                    cGsdFileInfoEntity.bTrdy_93_75_Exist = true;
                }
            }
            //
            //Trdy_187.5
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_187.5"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_187_5))
                {
                    cGsdFileInfoEntity.bTrdy_187_5_Exist = true;
                }
            }
            //
            //Trdy_500
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_500"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_500))
                {
                    cGsdFileInfoEntity.bTrdy_500_Exist = true;
                }
            }
            //
            //Trdy_1.5M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_1.5M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_1_5M))
                {
                    cGsdFileInfoEntity.bTrdy_1_5M_Exist = true;
                }
            }
            //
            //Trdy_3M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_3M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_3M))
                {
                    cGsdFileInfoEntity.bTrdy_3M_Exist = true;
                }
            }
            //
            //Trdy_6M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_6M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_6M))
                {
                    cGsdFileInfoEntity.bTrdy_6M_Exist = true;
                }
            }
            //
            //Trdy_12M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Trdy_12M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTrdy_12M))
                {
                    cGsdFileInfoEntity.bTrdy_12M_Exist = true;
                }
            }
            //
            //Tqui_9.6
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_9.6"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_9_6))
                {
                    cGsdFileInfoEntity.bTqui_9_6_Exist = true;
                }
            }
            //
            //Tqui_19.2
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_19.2"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_19_2))
                {
                    cGsdFileInfoEntity.bTqui_19_2_Exist = true;
                }
            }
            //
            //Tqui_31.25
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_31.25"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_31_25))
                {
                    cGsdFileInfoEntity.bTqui_31_25_Exist = true;
                }
            }
            //
            //Tqui_45.45
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_45.45"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_45_45))
                {
                    cGsdFileInfoEntity.bTqui_45_45_Exist = true;
                }
            }
            //
            //Tqui_93.75
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_93.75"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_93_95))
                {
                    cGsdFileInfoEntity.bTqui_93_75_Exist = true;
                }
            }
            //
            //Tqui_183.5
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_93.75"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_183_5))
                {
                    cGsdFileInfoEntity.bTqui_183_5_Exist = true;
                }
            }
            //
            //Tqui_500
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_500"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_500))
                {
                    cGsdFileInfoEntity.bTqui_500_Exist = true;
                }
            }
            //
            //Tqui_500
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_500"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_500))
                {
                    cGsdFileInfoEntity.bTqui_500_Exist = true;
                }
            }
            //
            //Tqui_1.5M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_1.5M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_1_5M))
                {
                    cGsdFileInfoEntity.bTqui_1_5M_Exist = true;
                }
            }
            //
            //Tqui_6M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_6M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_6M))
                {
                    cGsdFileInfoEntity.bTqui_6M_Exist = true;
                }
            }
            //
            //Tqui_12M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tqui_12M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTqui_12M))
                {
                    cGsdFileInfoEntity.bTqui_12M_Exist = true;
                }
            }
            //
            //Tset_9.6
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_9.6"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_9_6))
                {
                    cGsdFileInfoEntity.bTset_9_6_Exist = true;
                }
            }
            //
            //Tset_19.2
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_19.2"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_19_2))
                {
                    cGsdFileInfoEntity.bTset_19_2_Exist = true;
                }
            }
            //
            //Tset_31.25
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_31.25"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_31_25))
                {
                    cGsdFileInfoEntity.bTset_31_25_Exist = true;
                }
            }
            //
            //Tset_45.45
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_45.45"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_45_45))
                {
                    cGsdFileInfoEntity.bTset_45_45_Exist = true;
                }
            }
            //
            //Tset_93.75
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_93.75"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_93_75))
                {
                    cGsdFileInfoEntity.bTset_93_75_Exist = true;
                }
            }
            //
            //Tset_187.5
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_187.5"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_187_5))
                {
                    cGsdFileInfoEntity.bTset_187_5_Exist = true;
                }
            }
            //
            //Tset_500
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_500"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_500))
                {
                    cGsdFileInfoEntity.bTset_500_Exist = true;
                }
            }
            //
            //Tset_1.5M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_1.5M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_1_5M))
                {
                    cGsdFileInfoEntity.bTset_1_5M_Exist = true;
                }
            }
            //
            //Tset_3M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_3M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_3M))
                {
                    cGsdFileInfoEntity.bTset_3M_Exist = true;
                }
            }
            //
            //Tset_6M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_6M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_6M))
                {
                    cGsdFileInfoEntity.bTset_6M_Exist = true;
                }
            }
            //
            //Tset_12M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tset_12M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTset_12M))
                {
                    cGsdFileInfoEntity.bTset_12M_Exist = true;
                }
            }
            //
            //LAS_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "LAS_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucLAS_Len))
                {
                    cGsdFileInfoEntity.bLAS_Len_Exist = true;
                }
            }
            //
            //Tsdi_9.6
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_9.6"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_9_6))
                {
                    cGsdFileInfoEntity.bTsdi_9_6_Exist = true;
                }
            }
            //
            //Tsdi_19.2
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_19.2"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_19_2))
                {
                    cGsdFileInfoEntity.bTsdi_19_2_Exist = true;
                }
            }
            //
            //Tsdi_31.25
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_31.25"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_31_25))
                {
                    cGsdFileInfoEntity.bTsdi_31_25_Exist = true;
                }
            }
            //
            //Tsdi_45.45
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_45.45"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_45_45))
                {
                    cGsdFileInfoEntity.bTsdi_45_45_Exist = true;
                }
            }
            //
            //Tsdi_93.75
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_93.75"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_93_75))
                {
                    cGsdFileInfoEntity.bTsdi_93_75_Exist = true;
                }
            }
            //
            //Tsdi_187.5
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_187.5"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_187_5))
                {
                    cGsdFileInfoEntity.bTsdi_187_5_Exist = true;
                }
            }
            //
            //Tsdi_500
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_500"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_500))
                {
                    cGsdFileInfoEntity.bTsdi_500_Exist = true;
                }
            }
            //
            //Tsdi_1.5M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_1.5M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_1_5M))
                {
                    cGsdFileInfoEntity.bTsdi_1_5M_Exist = true;
                }
            }
            //
            //Tsdi_3M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_3M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_3M))
                {
                    cGsdFileInfoEntity.bTsdi_3M_Exist = true;
                }
            }
            //
            //Tsdi_6M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_6M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_6M))
                {
                    cGsdFileInfoEntity.bTsdi_6M_Exist = true;
                }
            }
            //
            //Tsdi_12M
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Tsdi_12M"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucTsdi_12M))
                {
                    cGsdFileInfoEntity.bTsdi_12M_Exist = true;
                }
            }
            //
            //Max_Master_Input_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Master_Input_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Master_Input_Len))
                {
                    cGsdFileInfoEntity.bMax_Master_Input_Len_Exist = true;
                }
            }
            //
            //Max_Master_Output_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Master_Output_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Master_Output_Len))
                {
                    cGsdFileInfoEntity.bMax_Master_Output_Len_Exist = true;
                }
            }

            //
            //4.3 Master-related specifications
            //4.3.2 Additional master related keywords for DP extensions
            //
            //
            //DPV1_Master
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DPV1_Master"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDPV1_Master))
                {
                    cGsdFileInfoEntity.bDPV1_Master_Exist = true;
                }
            }
            //
            //C1_Master_Read_Write_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C1_Master_Read_Write_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bC1_Master_Read_Write_supp))
                {
                    cGsdFileInfoEntity.bC1_Master_Read_Write_supp_Exist = true;
                }
            }
            //
            //Master_DPV1_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_DPV1_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_DPV1_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_DPV1_Alarm_supp_Exist = true;
                }
            }
            //
            //Master_Diagnostic_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Diagnostic_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Diagnostic_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_Diagnostic_Alarm_supp_Exist = true;
                }
            }
            //
            //Master_Process_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Process_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Process_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_Process_Alarm_supp_Exist = true;
                }
            }
            //
            //Master_Pull_Plug_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Pull_Plug_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Pull_Plug_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_Pull_Plug_Alarm_supp__Exist = true;
                }
            }
            //
            //Master_Status_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Status_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Status_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_Status_Alarm_supp_Exist = true;
                }
            }
            //
            //Master_Update_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Update_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Update_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_Update_Alarm_supp_Exist = true;
                }
            }
            //
            //Master_Manufacturer_Specific_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Manufacturer_Specific_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Manufacturer_Specific_Alarm_supp))
                {
                    cGsdFileInfoEntity.bMaster_Manufacturer_Specific_Alarm_supp_Exist = true;
                }
            }
            //
            //Master_Extra_Alarm_SAP_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Update_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bMaster_Extra_Alarm_SAP_supp))
                {
                    cGsdFileInfoEntity.bMaster_Extra_Alarm_SAP_supp_Exist = true;
                }
            }
            //
            //Master_Alarm_Sequence_Mode
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Master_Alarm_Sequence_Mode"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMaster_Alarm_Sequence_Mode))
                {
                    cGsdFileInfoEntity.bMaster_Alarm_Sequence_Mode_Exist = true;
                }
            }

            //
            //4.3 Master-related specifications
            //4.3.3 Additional master related keywords for DP-V2        
            //
            //
            //Isochron_Mode_Synchronised
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Isochron_Mode_Synchronised"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucIsochron_Mode_Synchronised))
                {
                    cGsdFileInfoEntity.bIsochron_Mode_Synchronised_Exist = true;
                }
            }
            //
            //DXB_Master_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DXB_Master_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDXB_Master_supp))
                {
                    cGsdFileInfoEntity.bDXB_Master_supp_Exist = true;
                }
            }
            //
            //X_Master_Prm_SAP_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "X_Master_Prm_SAP_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bX_Master_Prm_SAP_supp))
                {
                    cGsdFileInfoEntity.bX_Master_Prm_SAP_supp_Exist = true;
                }
            }

            //
            //4.4 Slave-related specifications
            //4.4.1 Basic DP-Slave related keywords
            //
            //
            //Freeze_Mode_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Freeze_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bFreeze_Mode_supp))
                {
                    cGsdFileInfoEntity.bFreeze_Mode_supp_Exist = true;
                }
            }
            //
            //Sync_Mode_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Sync_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bSync_Mode_supp))
                {
                    cGsdFileInfoEntity.bSync_Mode_supp_Exist = true;
                }
            }
            //
            //Auto_Baud_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Auto_Baud_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bAuto_Baud_supp))
                {
                    cGsdFileInfoEntity.bAuto_Baud_supp_Exist = true;
                }
            }
            //
            //Set_Slave_Add_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Set_Slave_Add_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bSet_Slave_Add_supp))
                {
                    cGsdFileInfoEntity.bSet_Slave_Add_supp_Exist = true;
                }
            }
            //
            //User_Prm_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "User_Prm_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucUser_Prm_Data_Len))
                {
                    cGsdFileInfoEntity.bUser_Prm_Data_Len_Exist = true;
                }
            }
            //
            //User_Prm_Data
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "User_Prm_Data"))
            {
                int iLocationE = strTemp.IndexOf("=", 0);  //等号的位置
                if (iLocationE != -1)
                {
                    cGsdFileInfoEntity.bUser_Prm_Data_Exist = true;

                    string strTempT = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);
                    strTempT = strTempT.Trim();

                    cGsdFileInfoEntity.strUser_Prm_Data = strTempT;

                    //将数据转换为HEX格式的数据
                    string[] strData = strTempT.Split(new char[] { ',' });

                    for (int i = 0; i < strData.Length; i++)
                    {
                        strData[i] = strData[i].Trim();//去除前后空格
                        if (strData[i].Contains("0x") || strData[i].Contains("0X"))//HEX格式
                        {
                            string strTTT = strData[i].Substring(2);
                            ushort uiTemp16 = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);     //这么多做是为了防止用户给出的数据超出byte的范围

                            cGsdFileInfoEntity.aucUser_Prm_Data[i] = (byte)uiTemp16;
                        }
                        else
                        {
                            ushort uiTemp16 = ushort.Parse(strData[i]);      //这么多做是为了防止用户给出的数据超出byte的范围
                            cGsdFileInfoEntity.aucUser_Prm_Data[i] = (byte)uiTemp16;
                        }
                    }

                    cGsdFileInfoEntity.ucUser_Prm_Data_LenL = (byte)strData.Length;
                }
            }
            //
            //Min_Slave_Intervall
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Min_Slave_Intervall"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMin_Slave_Intervallp))
                {
                    cGsdFileInfoEntity.bMin_Slave_Intervall_Exist = true;
                }
            }
            //
            //Modular_Station
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Modular_Station"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bModular_Station))
                {
                    cGsdFileInfoEntity.bModular_Station_Exist = true;
                }
            }
            //
            //Max_Module
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Module"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Module))
                {
                    cGsdFileInfoEntity.bMax_Module_Exist = true;
                }
            }
            //
            //Max_Input_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Input_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Input_Len))
                {
                    cGsdFileInfoEntity.bMax_Input_Len_Exist = true;
                }
            }
            //
            //Max_Output_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Output_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Output_Len))
                {
                    cGsdFileInfoEntity.bMax_Output_Len_Exist = true;
                }
            }
            //
            //Max_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiMax_Data_Len))
                {
                    cGsdFileInfoEntity.bMax_Data_Len_Exist = true;
                }
            }
            //
            //Unit_Diag_Bit
            //
            else if (strTempU.Contains("UNIT_DIAG_BIT") && !strTempU.Contains("X_UNIT_DIAG_BIT") && !strTempU.Contains("UNIT_DIAG_BIT_HELP") && !strTempU.Contains("X_UNIT_DIAG_BIT"))
            {
                CUNIT_DIAG_BIT cUnitDiagBitNode = new CUNIT_DIAG_BIT();

                //Bit
                int iLocationL = strTemp.IndexOf("(");
                int iLocationR = strTemp.IndexOf(")");
                if (iLocationL == -1 || iLocationR == -1)
                {
                    return;
                }

                string strBit = strTemp.Substring(iLocationL + 1, iLocationR - 1 - iLocationL);
                strBit = strBit.Trim();

                if (strBit.Contains("0x") || strBit.Contains("0X"))
                {
                    return;
                }
                cUnitDiagBitNode.uiBit = ushort.Parse(strBit);

                //Text
                int iLocationE = strTemp.IndexOf("=");
                if (iLocationE == -1)
                {
                    return;
                }

                cUnitDiagBitNode.strDiag_Text = strTemp.Substring(iLocationE + 2, strTemp.Length - (iLocationE + 2));
                cUnitDiagBitNode.strDiag_Text = cUnitDiagBitNode.strDiag_Text.Trim();

                cGsdFileInfoEntity.Unit_Diag_Bit_List.Add(cUnitDiagBitNode);
            }

            //X_Unit_Diag_Bit

            //(X_)Unit_Diag_Bit_Help
            //(X_)Unit_Diag_Not_Bit
            //(X_)Unit_Diag_Not_Bit_Help
            //(X_)Unit_Diag_Area
            //UnitDiagType
            //

            //
            //Module
            //public List<CMODULE_INFO> asModuleInfoList = new List<CMODULE_INFO>();
            //         
            else if (judgeSingleKeywordIsExisted(strTemp, "Module"))
            {
                CMODULE_INFO cModuleNode = new CMODULE_INFO();

                if (parseGsdKeyWord_MODULE(strTemp, ref cModuleNode))
                {
                    cGsdFileInfoEntity.asModuleInfoList.Add(cModuleNode);

                    ucOperStatus = PROCESS_Module;
                }
            }
            //
            //Channel_Diag
            //
            else if (strTempU.Contains("CHANNEL_DIAG") && !strTempU.Contains("CHANNEL_DIAG_HELP"))
            {
                CCHANNEL_DIAG cChannelDiagNode = new CCHANNEL_DIAG();

                //Error_Type
                int iLocationL = strTemp.IndexOf("(");
                int iLocationR = strTemp.IndexOf(")");
                if (iLocationL == -1 || iLocationR == -1)
                {
                    return;
                }

                string strBit = strTemp.Substring(iLocationL + 1, iLocationR - 1 - iLocationL);
                strBit = strBit.Trim();

                if (strBit.Contains("0x") || strBit.Contains("0X"))
                {
                    return;
                }
                ushort uiTemp16 = ushort.Parse(strBit);
                cChannelDiagNode.ucError_Type = (byte)uiTemp16;

                //Diag_Text
                int iLocationE = strTemp.IndexOf("=");
                if (iLocationE == -1)
                {
                    return;
                }

                cChannelDiagNode.strDiag_Text = strTemp.Substring(iLocationE + 2, strTemp.Length - (iLocationE + 2));
                cChannelDiagNode.strDiag_Text = cChannelDiagNode.strDiag_Text.Trim();

                cGsdFileInfoEntity.Channel_Diag_List.Add(cChannelDiagNode);
            }
            //Channel_Diag_Help

            //
            //Max_Diag_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Diag_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Diag_Data_Len))
                {
                    cGsdFileInfoEntity.bMax_Diag_Data_Len_Exist = true;
                }
            }
            //
            //Modul_Offset
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Modul_Offset"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucModul_Offset))
                {
                    cGsdFileInfoEntity.bModul_Offset_Exist = true;
                }
            }
            //
            //Slave_Family
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Slave_Family"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strSlave_Family))
                {
                    cGsdFileInfoEntity.bSlave_Family_Exist = true;
                }
            }
            //
            //Diag_Update_Delay
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Diag_Update_Delay"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucDiag_Update_Delay))
                {
                    cGsdFileInfoEntity.bDiag_Update_Delay_Exist = true;
                }
            }
            //
            //Fail_Safe_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Fail_Safe_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bFail_Safe_required))
                {
                    cGsdFileInfoEntity.bFail_Safe_required_Exist = true;
                }
            }
            //
            //Info_Text
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Info_Text"))
            {
                if (parseGsdKeyWord_STRING(strTemp, ref cGsdFileInfoEntity.strInfo_Text))
                {
                    cGsdFileInfoEntity.bInfo_Text_Exist = true;
                }
            }
            //
            //Max_User_Prm_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_User_Prm_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_User_Prm_Data_Len))
                {
                    cGsdFileInfoEntity.bMax_User_Prm_Data_Len_Exist = true;
                }
            }

            //
            //Ext_User_Prm_Data_Ref
            //public List<CGSD_DATA_REF_INFO> Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();
            //
            if (strTemp.Contains("Ext_User_Prm_Data_Ref") && !strTemp.Contains("F_Ext_User_Prm_Data_Ref") && !strTemp.Contains("X_Ext_User_Prm_Data_Ref"))
            {
                int iLocationE = strTemp.IndexOf("(", 0);
                int iLocationD = strTemp.IndexOf(")", 0);

                //取出括号之间的索引值，这是起始REF值
                if (iLocationE != -1 && iLocationD != -1)
                {
                    CGSD_DATA_REF_INFO cDataRefTemp = new CGSD_DATA_REF_INFO();

                    string strTempM = strTemp.Substring(iLocationE + 1, iLocationD - iLocationE - 1);
                    strTempM = strTempM.Trim(); //去除前后空格
                    cDataRefTemp.ucRefBeg = (byte)(ushort.Parse(strTempM)); //担心此值超出255范围(由于用户输入错误的原因) ,所以先转换为16位数字

                    //处理数据部分
                    int iLocationT = strTemp.IndexOf("=", 0);
                    strTempM = strTemp.Substring(iLocationT + 1);
                    strTempM = strTempM.Trim();//去除前后空格

                    if (strTempM.Contains("0X") || strTempM.Contains("0x"))//HEX格式
                    {
                        string strTTT = strTempM.Substring(2, strTempM.Length - 2);
                        cDataRefTemp.uiData = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        cDataRefTemp.uiData = ushort.Parse(strTempM);
                    }
                    cGsdFileInfoEntity.Ext_User_Prm_Data_Ref_List.Add(cDataRefTemp);
                }
            }
            //
            //Ext_User_Prm_Data_Const
            //public List<CGSD_DATA_CONST_INFO> Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();
            //
            if (strTemp.Contains("Ext_User_Prm_Data_Const") && !strTemp.Contains("F_Ext_User_Prm_Data_Const") && !strTemp.Contains("X_Ext_User_Prm_Data_Const"))
            {
                int iLocationE = strTemp.IndexOf("(", 0);
                int iLocationD = strTemp.IndexOf(")", 0);

                //取出括号之间的索引值，这是起始REF值
                if (iLocationE != -1 && iLocationD != -1)
                {
                    CGSD_DATA_CONST_INFO cDataConstTemp = new CGSD_DATA_CONST_INFO();

                    string strTempM = strTemp.Substring(iLocationE + 1, iLocationD - iLocationE - 1);
                    strTempM = strTempM.Trim(); //去除前后空格
                    cDataConstTemp.ucRefBeg = (byte)(ushort.Parse(strTempM)); //担心此值超出255范围(由于用户输入错误的原因) ,所以先转换为16位数字

                    //处理数据部分
                    int iLocationT = strTemp.IndexOf("=", 0);

                    strTempM = strTemp.Substring(iLocationT + 1);
                    strTempM = strTempM.Trim();//去除前后空格

                    string[] strData = strTempM.Split(new char[] { ',' });
                    for (int i = 0; i < strData.Length; i++)
                    {
                        strData[i] = strData[i].Trim(); //去除前后空格
                        if (strData[i].Contains("0X") || strData[i].Contains("0x"))//HEX格式
                        {
                            string strTTT = strData[i].Substring(2, strData[i].Length - 2);
                            cDataConstTemp.aucData[i] = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            cDataConstTemp.aucData[i] = (byte)(ushort.Parse(strData[i]));
                        }
                    }
                    cDataConstTemp.ucDataLenL = (byte)strData.Length;
                    cGsdFileInfoEntity.Ext_User_Prm_Data_Const_List.Add(cDataConstTemp);
                }
            }
            //
            //ExtUserPrmData
            //public List<CEXT_USER_PRM_DATA_INFO> ExtUserPrmData_List = new List<CEXT_USER_PRM_DATA_INFO>();
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "ExtUserPrmData"))
            {
                CEXT_USER_PRM_DATA_INFO cExtUserPrmDataNode = new CEXT_USER_PRM_DATA_INFO();

                if (parseGsdKeyWord_ExtUserPrmData(strTemp, ref cExtUserPrmDataNode))
                {
                    cGsdFileInfoEntity.ExtUserPrmData_List.Add(cExtUserPrmDataNode);
                    ucOperStatus = PROCESS_ExtUserPrmData;
                }

            }

            //CEXT_USER_PRM_DATA_INFO

            //PrmText
            //public List<CPRM_TEXT_INFO> PrmText_List = new List<CPRM_TEXT_INFO>();
            //         
            else if (judgeSingleKeywordIsExisted(strTemp, "PrmText"))
            {
                CPRM_TEXT_INFO cPrmTextNode = new CPRM_TEXT_INFO();

                if (parseGsdKeyWord_PrmText(strTemp, ref cPrmTextNode))
                {
                    cGsdFileInfoEntity.PrmText_List.Add(cPrmTextNode);
                    ucOperStatus = PROCESS_PrmText;
                }
            }

            //
            //Prm_Block_Structure_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Prm_Block_Structure_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bPrm_Block_Structure_supp))
                {
                    cGsdFileInfoEntity.bPrm_Block_Structure_supp_Exist = true;
                }
            }
            //
            //Prm_Block_Structure_req
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Prm_Block_Structure_req"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bPrm_Block_Structure_req))
                {
                    cGsdFileInfoEntity.bPrm_Block_Structure_req_Exist = true;
                }
            }
            //
            //Jokerblock_Slot
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Jokerblock_Slot"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucJokerblock_Slot))
                {
                    cGsdFileInfoEntity.bJokerblock_Slot_Exist = true;
                }
            }
            //
            //Jokerblock_Location
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Jokerblock_Location"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucJokerblock_Location))
                {
                    cGsdFileInfoEntity.bJokerblock_Location_Exist = true;
                }
            }
            //
            //PrmCmd_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "PrmCmd_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bPrmCmd_supp))
                {
                    cGsdFileInfoEntity.bPrmCmd_supp_Exist = true;
                }
            }
            //
            //Slave_Max_Switch_Over_Time
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Slave_Max_Switch_Over_Time"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiSlave_Max_Switch_Over_Time))
                {
                    cGsdFileInfoEntity.bSlave_Max_Switch_Over_Time_Exist = true;
                }
            }
            //
            //Slave_Redundancy_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Jokerblock_Location"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucSlave_Redundancy_supp))
                {
                    cGsdFileInfoEntity.bSlave_Redundancy_supp_Exist = true;
                }
            }
            //
            //Ident_Maintenance_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Ident_Maintenance_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bIdent_Maintenance_supp))
                {
                    cGsdFileInfoEntity.bIdent_Maintenance_supp_Exist = true;
                }
            }
            //
            //Time_Sync_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Time_Sync_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bTime_Sync_supp))
                {
                    cGsdFileInfoEntity.bTime_Sync_supp_Exist = true;
                }
            }
            //
            //Max_iParameter_Size
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_iParameter_Size"))
            {
                if (parseGsdKeyWord_USIGN32(strTemp, ref cGsdFileInfoEntity.ulMax_iParameter_Size))
                {
                    cGsdFileInfoEntity.bMax_iParameter_Size_Exist = true;
                }
            }

            //
            //4.4 Slave-related specifications
            //4.4.2 Additional keywords for module assignment
            //
            //SlotDefinition


            //
            //4.4 Slave-related specifications
            //4.4.3 Slave related keywords for DP extensions
            //PROFIBUS extensions mean the features of DP-V1 (see IEC 61784-1:2003 A3.1) and
            //list of options (see IEC 61784-1:2003 A3.1 and 7.2.3.2.5), compared to DP-V0.
            //
            //
            //DPV1_Slave
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DPV1_Slave"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDPV1_Slave))
                {
                    cGsdFileInfoEntity.bDPV1_Slave_Exist = true;
                }
            }
            //
            //C1_Read_Write_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C1_Read_Write_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bC1_Read_Write_supp))
                {
                    cGsdFileInfoEntity.bC1_Read_Write_supp_Exist = true;
                }
            }
            //
            //C2_Read_Write_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C2_Read_Write_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bC2_Read_Write_supp))
                {
                    cGsdFileInfoEntity.bC2_Read_Write_supp_Exist = true;
                }
            }
            //
            //C1_Max_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C1_Max_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucC1_Max_Data_Len))
                {
                    cGsdFileInfoEntity.bC1_Max_Data_Len_Exist = true;
                }
            }
            //
            //C2_Max_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C2_Max_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucC2_Max_Data_Len))
                {
                    cGsdFileInfoEntity.bC2_Max_Data_Len_Exist = true;
                }
            }
            //
            //C1_Response_Timeout
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C1_Response_Timeout"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiC1_Response_Timeout))
                {
                    cGsdFileInfoEntity.bC1_Response_Timeout_Exist = true;
                }
            }
            //
            //C2_Response_Timeout
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C2_Response_Timeout"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiC2_Response_Timeout))
                {
                    cGsdFileInfoEntity.bC2_Response_Timeout_Exist = true;
                }
            }
            //
            //C1_Read_Write_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C1_Read_Write_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bC1_Read_Write_required))
                {
                    cGsdFileInfoEntity.bC1_Read_Write_required_Exist = true;
                }
            }
            //
            //C2_Max_Count_Channels
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "C2_Max_Count_Channels"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucC2_Max_Count_Channels))
                {
                    cGsdFileInfoEntity.bC2_Max_Count_Channels_Exist = true;
                }
            }
            //
            //Max_Initiate_PDU_Length
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Max_Initiate_PDU_Length"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucMax_Initiate_PDU_Length))
                {
                    cGsdFileInfoEntity.bMax_Initiate_PDU_Length_Exist = true;
                }
            }
            //
            //Diagnostic_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Diagnostic_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDiagnostic_Alarm_supp))
                {
                    cGsdFileInfoEntity.bDiagnostic_Alarm_supp_Exist = true;
                }
            }
            //
            //Process_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Process_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bProcess_Alarm_supp))
                {
                    cGsdFileInfoEntity.bProcess_Alarm_supp_Exist = true;
                }
            }
            //
            //Pull_Plug_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Pull_Plug_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bPull_Plug_Alarm_supp))
                {
                    cGsdFileInfoEntity.bPull_Plug_Alarm_supp_Exist = true;
                }
            }
            //
            //Status_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Status_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bStatus_Alarm_supp))
                {
                    cGsdFileInfoEntity.bStatus_Alarm_supp_Exist = true;
                }
            }
            //
            //Update_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Update_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bUpdate_Alarm_supp))
                {
                    cGsdFileInfoEntity.bUpdate_Alarm_supp_Exist = true;
                }
            }
            //
            //Manufacturer_Specific_Alarm_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Manufacturer_Specific_Alarm_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bManufacturer_Specific_Alarm_supp))
                {
                    cGsdFileInfoEntity.bManufacturer_Specific_Alarm_supp_Exist = true;
                }
            }
            //
            //Extra_Alarm_SAP_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Extra_Alarm_SAP_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bExtra_Alarm_SAP_supp))
                {
                    cGsdFileInfoEntity.bExtra_Alarm_SAP_supp_Exist = true;
                }
            }
            //
            //Alarm_Sequence_Mode_Count
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Alarm_Sequence_Mode_Count"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucAlarm_Sequence_Mode_Count))
                {
                    cGsdFileInfoEntity.bAlarm_Sequence_Mode_Count_Exist = true;
                }
            }
            //
            //Alarm_Type_Mode_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Alarm_Type_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bAlarm_Type_Mode_supp))
                {
                    cGsdFileInfoEntity.bAlarm_Type_Mode_supp_Exist = true;
                }
            }
            //
            //Diagnostic_Alarm_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Diagnostic_Alarm_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDiagnostic_Alarm_required))
                {
                    cGsdFileInfoEntity.bDiagnostic_Alarm_required_Exist = true;
                }
            }
            //
            //Process_Alarm_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Process_Alarm_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bProcess_Alarm_required))
                {
                    cGsdFileInfoEntity.bProcess_Alarm_required_Exist = true;
                }
            }
            //
            //Pull_Plug_Alarm_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Pull_Plug_Alarm_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bPull_Plug_Alarm_required_Exist))
                {
                    cGsdFileInfoEntity.bPull_Plug_Alarm_required = true;
                }
            }
            //
            //Status_Alarm_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Status_Alarm_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bStatus_Alarm_required))
                {
                    cGsdFileInfoEntity.bStatus_Alarm_required_Exist = true;
                }
            }
            //
            //Update_Alarm_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Update_Alarm_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bUpdate_Alarm_required))
                {
                    cGsdFileInfoEntity.bUpdate_Alarm_required_Exist = true;
                }
            }
            //
            //Manufacturer_Specific_Alarm_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Manufacturer_Specific_Alarm_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bManufacturer_Specific_Alarm_required))
                {
                    cGsdFileInfoEntity.bManufacturer_Specific_Alarm_required_Exist = true;
                }
            }
            //
            //DPV1_Data_Types
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DPV1_Data_Types"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bDPV1_Data_Types))
                {
                    cGsdFileInfoEntity.bDPV1_Data_Types_Exist = true;
                }
            }
            //
            //WD_Base_1ms_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "WD_Base_1ms_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bWD_Base_1ms_supp))
                {
                    cGsdFileInfoEntity.bWD_Base_1ms_supp_Exist = true;
                }
            }
            //
            //Check_Cfg_Mode
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Check_Cfg_Mode"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bCheck_Cfg_Mode))
                {
                    cGsdFileInfoEntity.bCheck_Cfg_Mode_Exist = true;
                }
            }

            //
            //4.4 Slave-related specifications
            //4.4.4 Slave related keywords for Data Exchange with Broadcast
            //
            //
            //Publisher_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Publisher_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bPublisher_supp))
                {
                    cGsdFileInfoEntity.bPublisher_supp_Exist = true;
                }
            }
            //
            //Subscriber_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Subscriber_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bSubscriber_supp))
                {
                    cGsdFileInfoEntity.bSubscriber_supp_Exist = true;
                }
            }
            //
            //DXB_Max_Link_Count
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DXB_Max_Link_Count"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucDXB_Max_Link_Count))
                {
                    cGsdFileInfoEntity.bDXB_Max_Link_Count_Exist = true;
                }
            }
            //
            //DXB_Max_Data_Length
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DXB_Max_Data_Length"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucDXB_Max_Data_Length))
                {
                    cGsdFileInfoEntity.bDXB_Max_Data_Length_Exist = true;
                }
            }
            //
            //DXB_Subscribertable_Block_Location
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "DXB_Subscribertable_Block_Location"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucDXB_Subscribertable_Block_Location))
                {
                    cGsdFileInfoEntity.bDXB_Subscribertable_Block_Location_Exist = true;
                }
            }

            //
            //4.4 Slave-related specifications
            //4.4.5 Slave related keywords for Isochronous Mode
            //
            //
            //Isochron_Mode_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Isochron_Mode_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bIsochron_Mode_supp))
                {
                    cGsdFileInfoEntity.bIsochron_Mode_supp_Exist = true;
                }
            }
            //
            //Isochron_Mode_required
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Isochron_Mode_required"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bIsochron_Mode_required))
                {
                    cGsdFileInfoEntity.bIsochron_Mode_required_Exist = true;
                }
            }
            //
            //TBASE_DP
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "TBASE_DP"))
            {
                if (parseGsdKeyWord_USIGN32(strTemp, ref cGsdFileInfoEntity.ulTBASE_DP))
                {
                    cGsdFileInfoEntity.bTBASE_DP_Exist = true;
                }
            }
            //
            //TDP_MIN
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "TDP_MIN"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiTDP_MIN))
                {
                    cGsdFileInfoEntity.bTDP_MIN_Exist = true;
                }
            }
            //
            //TDP_MAX
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "TDP_MAX"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiTDP_MAX))
                {
                    cGsdFileInfoEntity.bTDP_MAXN_Exist = true;
                }
            }
            //
            //T_PLL_W_MAX
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "T_PLL_W_MAX"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiT_PLL_W_MAX))
                {
                    cGsdFileInfoEntity.bT_PLL_W_MAX_Exist = true;
                }
            }
            //
            //TI_MIN
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "TI_MIN"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiTI_MIN))
                {
                    cGsdFileInfoEntity.bTI_MIN_Exist = true;
                }
            }
            //
            //TO_MIN
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "TO_MIN"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiTO_MIN))
                {
                    cGsdFileInfoEntity.bTO_MIN_Exist = true;
                }
            }

            //
            //4.4 Slave-related specifications
            //4.4.6 Slave related keywords for PROFIsafe Profile
            //
            //
            //F_ParamDescCRC
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "F_ParamDescCRC"))
            {
                if (parseGsdKeyWord_USIGN16(strTemp, ref cGsdFileInfoEntity.uiF_ParamDescCRC))
                {
                    cGsdFileInfoEntity.bF_ParamDescCRC_Exist = true;
                }
            }
            //
            //F_IO_StructureDescCRC
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "F_IO_StructureDescCRC"))
            {
                if (parseGsdKeyWord_USIGN32(strTemp, ref cGsdFileInfoEntity.ulF_IO_StructureDescCRC))
                {
                    cGsdFileInfoEntity.bF_IO_StructureDescCRC_Exist = true;
                }
            }

            //F_Ext_User_Prm_Data_Ref
            //F_Ext_User_Prm_Data_Const


            //
            //4.4 Slave-related specifications
            //4.4.7 Slave related keywords for extended parameterization
            //
            //
            //X_Prm_SAP_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "X_Prm_SAP_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bX_Prm_SAP_supp))
                {
                    cGsdFileInfoEntity.bX_Prm_SAP_supp_Exist = true;
                }
            }
            //
            //X_Max_User_Prm_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "X_Max_User_Prm_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucX_Max_User_Prm_Data_Len))
                {
                    cGsdFileInfoEntity.bX_Max_User_Prm_Data_Len_Exist = true;
                }
            }
            //
            //X_Ext_Module_Prm_Data_Len
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "X_Ext_Module_Prm_Data_Len"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucX_Ext_Module_Prm_Data_Len))
                {
                    cGsdFileInfoEntity.bX_Ext_Module_Prm_Data_Len_Exist = true;
                }
            }

            //X_Ext_User_Prm_Data_Ref
            //X_Ext_User_Prm_Data_Const

            //
            //X_Prm_Block_Structure_supp
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "X_Prm_Block_Structure_supp"))
            {
                if (parseGsdKeyWord_BOOL(strTemp, ref cGsdFileInfoEntity.bX_Prm_Block_Structure_supp))
                {
                    cGsdFileInfoEntity.bX_Prm_Block_Structure_supp_Exist = true;
                }
            }


            //
            //4.4 Slave-related specifications
            //4.4.8 Slave related keywords for subsystems
            //
            //
            //Subsys_Dir_Index
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Subsys_Dir_Index"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucSubsys_Dir_Index))
                {
                    cGsdFileInfoEntity.bSubsys_Dir_Index_Exist = true;
                }
            }
            //
            //Subsys_Module_Dir_Index
            //
            else if (judgeSingleKeywordIsExisted(strTemp, "Subsys_Dir_Index"))
            {
                if (parseGsdKeyWord_USIGN8(strTemp, ref cGsdFileInfoEntity.ucSubsys_Module_Dir_Index))
                {
                    cGsdFileInfoEntity.bSubsys_Module_Dir_Index_Exist = true;
                }
            }

            //
            //the others
            //

            //ttt
        }
        //
        //判断字符串中是否存在指定关键字，这里关键字中指“=”左侧的独立关键字，且不带括号的那类
        //如果字符串中没有“=”，则把整个字符串做为一个整体进行比较
        //要求格式：KeyWord = xxxxxxxxxx 或者是 KeyWord
        //
        private bool judgeSingleKeywordIsExisted(string strLine, string strKeyWord)
        {
            bool bRlt = false;

            string strLineU = strLine.ToUpper();
            string strKeyWordU = strKeyWord.ToUpper();

            if (strLineU.Contains(strKeyWordU))
            {
                //如果字符串中有“=”，则“=”左侧的部分做为一个整体进行比较
                if (strLineU.Contains("="))
                {
                    int iLocationE = strLineU.IndexOf("=", 0);
                    if (iLocationE != -1)
                    {
                        string strTempP = strLineU.Substring(0, iLocationE);
                        strTempP = strTempP.Trim(); //去除前后空格

                        //再次判断
                        if (strTempP == strKeyWordU)
                        {
                            bRlt = true;
                        }
                    }
                }
                //如果字符串中没有“=”，则把整个字符串做为一个整体进行比较
                else
                {
                    if (strLineU == strKeyWordU)
                    {
                        bRlt = true;
                    }
                }
            }

            return bRlt;
        }
        //
        //解析ExtUserPrmData关键字所在一行的内容，主要是字符串和数据
        //例如：ExtUserPrmData          = 204 "F_Par_Version"
        //
        bool parseGsdKeyWord_ExtUserPrmData(string strTemp, ref CEXT_USER_PRM_DATA_INFO cExtUserPrmDataNode)
        {
            bool bRlt = true;

            int iLocationE = strTemp.IndexOf("=");         //"="的位置
            if (iLocationE != -1)
            {
                //string strTempP = strTemp.Substring(iLocationE + 1);    //引号后面的子串

                int iLocationF = strTemp.IndexOf("\"");             //"\""的位置
                int iLocationL = strTemp.LastIndexOf("\"");         //最后一个"\""的位置
                if (iLocationF != -1 && iLocationL != -1)
                {
                    //Reference_Number                    
                    string strTempNum = strTemp.Substring(iLocationE + 1, iLocationF - 1 - iLocationE);
                    strTempNum = strTempNum.Trim();

                    //正常情况下，ref应该是十进制数字，但也许有可能用户用HEX格式书写
                    if (strTempNum.Contains("0x") || strTempNum.Contains("0X"))//HEX格式
                    {
                        string strTTT = strTempNum.Substring(2);
                        cExtUserPrmDataNode.uiReference_Number = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        cExtUserPrmDataNode.uiReference_Number = ushort.Parse(strTempNum);
                    }

                    //Ext_User_Prm_Data_Name
                    cExtUserPrmDataNode.strExt_User_Prm_Data_Name = strTemp.Substring(iLocationF + 1, iLocationL - 1 - iLocationF);
                    cExtUserPrmDataNode.strExt_User_Prm_Data_Name = cExtUserPrmDataNode.strExt_User_Prm_Data_Name.Trim();
                }
                else
                {
                    bRlt = false;
                }
            }
            else
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //解析MODULE关键字所在一行的内容，主要是字符串和CFG数据
        //例如：Module= "Class 1 Singleturn" 0xD0
        //
        bool parseGsdKeyWord_PrmText(string strTemp, ref CPRM_TEXT_INFO cPrmTextNode)
        {
            bool bRlt = true;

            int iLocationE = strTemp.IndexOf("=");         //"="的位置
            if (iLocationE != -1)
            {
                string strTempP = strTemp.Substring(iLocationE + 1);    //引号后面的子串

                if (strTempP.Contains("0x") || strTempP.Contains("0X"))//HEX格式
                {
                    string strTTT = strTempP.Substring(2);
                    cPrmTextNode.uiReference_Number = ushort.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    cPrmTextNode.uiReference_Number = ushort.Parse(strTempP);
                }
            }
            else
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //解析MODULE关键字所在一行的内容，主要是字符串和CFG数据
        //例如：Module= "Class 1 Singleturn" 0xD0
        //
        bool parseGsdKeyWord_MODULE(string strTemp, ref CMODULE_INFO cModuleNode)
        {
            bool bRlt = true;

            int iLocationB = strTemp.IndexOf("\"");         //最左侧引号的位置
            int iLocationE = strTemp.LastIndexOf("\"");     //最右侧引号的位置
            if (iLocationB != -1 && iLocationE != -1)
            {

                cModuleNode.strMod_Name = strTemp.Substring(iLocationB + 1, iLocationE - 1 - iLocationB);    //引号内子串长度：iLocationE - 1 - iLocationB
                cModuleNode.strConfig = strTemp.Substring(iLocationE + 1);
                cModuleNode.strConfig = cModuleNode.strConfig.Trim(); //去除前后的空格

                //将CFG数据转换为HEX格式的数据
                string[] strData = cModuleNode.strConfig.Split(new char[] { ',' });

                for (int i = 0; i < strData.Length; i++)
                {
                    strData[i] = strData[i].Trim();//去除前后空格
                    if (strData[i].Contains("0x") || strData[i].Contains("0X"))//HEX格式
                    {
                        string strTTT = strData[i].Substring(2);
                        cModuleNode.aucCfgData[i] = byte.Parse(strTTT, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        ushort uiTemp16 = ushort.Parse(strData[i]);      //这么多做是为了防止用户给出的数据超出byte的范围
                        cModuleNode.aucCfgData[i] = (byte)uiTemp16;
                    }
                }
                cModuleNode.ucCfgLen = (byte)strData.Length;
            }
            else
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //处理布尔类型的数据
        //
        bool parseGsdKeyWord_BOOL(string strTemp, ref bool bData)
        {
            bool bRlt = true;

            int iLocationE = strTemp.IndexOf("=", 0);          //找到"="开始位置
            if (iLocationE != -1)
            {
                string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                if (strInfo == "0")
                {
                    bData = false;
                }
                else if (strInfo == "1")
                {
                    bData = true;
                }
                else
                {
                    bRlt = false;
                }
            }
            else   //"="不存在
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //处理8位数据，即大小不能超过255
        //要求格式：KeyWord = ddd or 0xHHH
        //
        bool parseGsdKeyWord_USIGN8(string strTemp, ref byte ucData)
        {
            bool bRlt = true;
            ulong ulTemp32 = 0;
            int iLocationE = strTemp.IndexOf("=", 0);           //找到"="开始位置

            if (iLocationE != -1)
            {
                string strTempU = strTemp.ToUpper();
                int iLocationH = strTempU.IndexOf("0X", 0);         //找到"0X"开始位置
                if (iLocationH != -1)   //HEX格式
                {
                    string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                    string strValue = strInfo.Substring(2, strInfo.Length - 2);     //除去"0X"两个字符
                    strValue = strValue.TrimStart('0');     //去除前面的多个"0"字符，如果有的话（经常会有）

                    //将字符串转换成32位的数据，然后再转换成8位的数据，这么做的目的是防止用户给出的数据超出255的限制
                    ulTemp32 = ulong.Parse(strValue, System.Globalization.NumberStyles.HexNumber);
                    if (ulTemp32 > 0xFF)
                    {
                        bRlt = false;
                    }
                    else
                    {
                        ucData = (byte)ulTemp32;
                    }
                }
                else//DEC格式
                {
                    string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                    //将字符串转换成32位的数据，然后再转换成8位的数据，这么做的目的是防止用户给出的数据超出255的限制
                    ulTemp32 = ulong.Parse(strInfo);
                    if (ulTemp32 > 0xFF)
                    {
                        bRlt = false;
                    }
                    else
                    {
                        ucData = (byte)ulTemp32;
                    }
                }
            }
            else
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //处理16位数据，即大小不能超过0xFFFF
        //要求格式：KeyWord = ddd or 0xHHH
        //
        bool parseGsdKeyWord_USIGN16(string strTemp, ref ushort uiData)
        {
            bool bRlt = true;
            ulong ulTemp32 = 0;
            int iLocationE = strTemp.IndexOf("=", 0);           //找到"="开始位置

            if (iLocationE != -1)
            {
                string strTempU = strTemp.ToUpper();
                int iLocationH = strTempU.IndexOf("0X", 0);         //找到"0X"开始位置
                if (iLocationH != -1)   //HEX格式
                {
                    string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                    string strValue = strInfo.Substring(2, strInfo.Length - 2);     //除去"0X"两个字符
                    strValue = strValue.TrimStart('0');     //去除前面的多个"0"字符，如果有的话（经常会有）

                    //将字符串转换成32位的数据，然后再转换成对应的数据，这么做的目的是防止用户给出的数据超出限制
                    ulTemp32 = ulong.Parse(strValue, System.Globalization.NumberStyles.HexNumber);
                    if (ulTemp32 > 0xFFFF)
                    {
                        bRlt = false;
                    }
                    else
                    {
                        uiData = (ushort)ulTemp32;
                    }
                }
                else//DEC格式
                {
                    string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                    //将字符串转换成32位的数据，然后再转换成对应的数据，这么做的目的是防止用户给出的数据超出限制
                    ulTemp32 = ulong.Parse(strInfo);
                    if (ulTemp32 > 0xFFFF)
                    {
                        bRlt = false;
                    }
                    else
                    {
                        uiData = (ushort)ulTemp32;
                    }
                }
            }
            else
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //处理32位数据
        //要求格式：KeyWord = ddd or 0xHHH
        //
        bool parseGsdKeyWord_USIGN32(string strTemp, ref ulong ulData)
        {
            bool bRlt = true;
            ulong ulTemp32 = 0;
            int iLocationE = strTemp.IndexOf("=", 0);           //找到"="开始位置

            if (iLocationE != -1)
            {
                string strTempU = strTemp.ToUpper();
                int iLocationH = strTempU.IndexOf("0X", 0);         //找到"0X"开始位置
                if (iLocationH != -1)   //HEX格式
                {
                    string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                    string strValue = strInfo.Substring(2, strInfo.Length - 2);     //除去"0X"两个字符
                    strValue = strValue.TrimStart('0');     //去除前面的多个"0"字符，如果有的话（经常会有）

                    //将字符串转换成32位的数据，然后再转换成对应的数据，这么做的目的是防止用户给出的数据超出限制
                    ulTemp32 = ulong.Parse(strValue, System.Globalization.NumberStyles.HexNumber);
                    ulData = ulTemp32;
                }
                else//DEC格式
                {
                    string strInfo = strTemp.Substring(iLocationE + 1, strTemp.Length - iLocationE - 1);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();   //去除前后的空格（如果有的话）

                    //将字符串转换成32位的数据，然后再转换成对应的数据，这么做的目的是防止用户给出的数据超出限制
                    ulTemp32 = ulong.Parse(strInfo);
                    ulData = ulTemp32;
                }
            }
            else
            {
                bRlt = false;
            }

            return bRlt;
        }
        //
        //处理字符串信息
        //要求格式：keyWord = "xxxxxxxxxxx" 或者是 keyWord = xxxxxxxxxxx （即不带双引号）
        //
        bool parseGsdKeyWord_STRING(string strTemp, ref string strInfo)
        {
            bool bRlt = true;
            int iLocationE = strTemp.IndexOf("=", 0);          //找到第"="的位置
            if (iLocationE != -1)
            {
                string strLast = strTemp.Substring(iLocationE + 1);  //取出"="字符后面的字符串
                strLast = strLast.Trim();              //去除前后的空格（如果有的话）

                int iLocationF = strLast.IndexOf("\"", 0);          //找到第一个引号的位置
                int iLocationL = strLast.LastIndexOf("\"");     //找到最后一个引号的位置

                //表示此字符串带有双引号
                if (iLocationE != -1 && iLocationL != -1)
                {
                    strInfo = strLast.Substring(1, strLast.Length - 2);  //取出数据对应的字符串
                    strInfo = strInfo.Trim();               //去除前后的空格（如果有的话）
                }
                //表示此字符串不带有双引号
                else
                {
                    strInfo = strLast;
                }
            }
            else
            {
                bRlt = false;
            }


            return bRlt;
        }
        //
        //将数据数组转换为字符串，以HEX格式显示，中间以","分隔开来
        //
        public void convertDataArrayToStringUSIGN8(byte[] aucData, byte ucDataLen, ref string strRlt, bool HexFlag)
        {
            strRlt = "";

            for (int i = 0; i < ucDataLen; i++)
            {
                if (HexFlag)
                {
                    if (i < (ucDataLen - 1))
                    {
                        strRlt += string.Format("0x{0:X2}, ", aucData[i]);
                    }
                    else
                    {
                        strRlt += string.Format("0x{0:X2}", aucData[i]);
                    }
                }
                else
                {
                    if (i < (ucDataLen - 1))
                    {
                        strRlt += string.Format("{0:d}, ", aucData[i]);
                    }
                    else
                    {
                        strRlt += string.Format("{0:d}", aucData[i]);
                    }

                }
            }
        }
        //
        //将数据数组转换为字符串，以HEX格式显示，中间以","分隔开来
        //
        public void convertDataArrayToStringUSIGN16(ushort[] auiData, byte ucDataLen, ref string strRlt, bool HexFlag)
        {
            strRlt = "";

            for (int i = 0; i < ucDataLen; i++)
            {
                if (HexFlag)
                {
                    if (i < (ucDataLen - 1))
                    {
                        strRlt += string.Format("0x{0:X2}, ", auiData[i]);
                    }
                    else
                    {
                        strRlt += string.Format("0x{0:X2}", auiData[i]);
                    }
                }
                else
                {
                    if (i < (ucDataLen - 1))
                    {
                        strRlt += string.Format("{0:d}, ", auiData[i]);
                    }
                    else
                    {
                        strRlt += string.Format("{0:d}", auiData[i]);
                    }

                }
            }
        }
        //
        //
        private void displayGSDInfo()//ttt
        {
            string strTemp = "";

            //
            //4.2 General specifications
            //4.2.1 General DP keywords
            //
            strTemp += "*** 4.2 General specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.2.1 General DP keywords";
            strTemp += "\r\n";

            //
            //GSD_Revision
            //
            if (cGsdFileInfoEntity.bGSD_Revision_Exist)
            {
                strTemp += "GSD_Revision        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucGSD_Revision);
                strTemp += "\r\n";
            }
            //
            //Vendor_Name
            //
            if (cGsdFileInfoEntity.bVendor_Name_Exist)
            {
                strTemp += "Vendor_Name         = ";
                strTemp += "\"" + cGsdFileInfoEntity.strVendor_Name + "\"";
                strTemp += "\r\n";
            }
            //
            //Model_Name
            //
            if (cGsdFileInfoEntity.bModel_Name_Exist)
            {
                strTemp += "Model_Name          = ";
                strTemp += "\"" + cGsdFileInfoEntity.strModel_Name + "\"";
                strTemp += "\r\n";
            }
            //
            //Revision
            //
            if (cGsdFileInfoEntity.bRevision_Exist)
            {
                strTemp += "Revision            = ";
                strTemp += "\"" + cGsdFileInfoEntity.strRevision + "\"";
                strTemp += "\r\n";
            }
            //
            //Revision_Number
            //
            if (cGsdFileInfoEntity.bRevision_Number_Exist)
            {
                strTemp += "GSD_Revision        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucRevision_Number);
                strTemp += "\r\n";
            }
            //
            //Ident_Number
            //
            if (cGsdFileInfoEntity.bIdent_Number_Exist)
            {
                strTemp += "Ident_Number        = ";
                strTemp += string.Format("0x{0:X4}", cGsdFileInfoEntity.uiIdent_Number);
                strTemp += "\r\n";
            }
            //
            //Protocol_Ident
            //
            if (cGsdFileInfoEntity.bProtocol_Ident_Exist)
            {
                strTemp += "Protocol_Ident      = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucProtocol_Ident);
                strTemp += "\r\n";
            }
            //
            //Station_Type
            //
            if (cGsdFileInfoEntity.bStation_Type_Exist)
            {
                strTemp += "Station_Type        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucStation_Type);
                strTemp += "\r\n";
            }
            //
            //FMS_supp
            //
            if (cGsdFileInfoEntity.bFMS_supp_Exist)
            {
                strTemp += "FMS_supp            = ";
                if (cGsdFileInfoEntity.bFMS_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Hardware_Release
            //
            if (cGsdFileInfoEntity.bHardware_Release_Exist)
            {
                strTemp += "Hardware_Release    = ";
                strTemp += "\"" + cGsdFileInfoEntity.strHardware_Release + "\"";
                strTemp += "\r\n";
            }
            //
            //Software_Release
            //
            if (cGsdFileInfoEntity.bSoftware_Release_Exist)
            {
                strTemp += "Software_Release    = ";
                strTemp += "\"" + cGsdFileInfoEntity.strSoftware_Release + "\"";
                strTemp += "\r\n";
            }
            //
            //";"
            //
            strTemp += ";";
            strTemp += "\r\n";
            //
            //9.6_supp
            //
            if (cGsdFileInfoEntity.bBaud_9_6_supp_Exist)
            {
                strTemp += "9.6_supp         = ";
                if (cGsdFileInfoEntity.bBaud_9_6_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //19.2_supp
            //
            if (cGsdFileInfoEntity.bBaud_19_2_supp_Exist)
            {
                strTemp += "19.2_supp        = ";
                if (cGsdFileInfoEntity.bBaud_19_2_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //45.45_supp
            //
            if (cGsdFileInfoEntity.bBaud_45_45_supp_Exist)
            {
                strTemp += "45.45_supp       = ";
                if (cGsdFileInfoEntity.bBaud_45_45_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //93.75_supp
            //
            if (cGsdFileInfoEntity.bBaud_93_75_supp_Exist)
            {
                strTemp += "93.75_supp       = ";
                if (cGsdFileInfoEntity.bBaud_93_75_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //187.5_supp
            //
            if (cGsdFileInfoEntity.bBaud_187_5_supp_Exist)
            {
                strTemp += "187.5_supp       = ";
                if (cGsdFileInfoEntity.bBaud_187_5_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //500_supp
            //
            if (cGsdFileInfoEntity.bBaud_500_supp_Exist)
            {
                strTemp += "500_supp         = ";
                if (cGsdFileInfoEntity.bBaud_500_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //1.5M_supp
            //
            if (cGsdFileInfoEntity.bBaud_1_5M_supp_Exist)
            {
                strTemp += "1.5M_supp        = ";
                if (cGsdFileInfoEntity.bBaud_1_5M_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //3M_supp
            //
            if (cGsdFileInfoEntity.bBaud_3M_supp_Exist)
            {
                strTemp += "3M_supp          = ";
                if (cGsdFileInfoEntity.bBaud_3M_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //6M_supp
            //
            if (cGsdFileInfoEntity.bBaud_6M_supp_Exist)
            {
                strTemp += "6M_supp          = ";
                if (cGsdFileInfoEntity.bBaud_6M_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //12M_supp
            //
            if (cGsdFileInfoEntity.bBaud_12M_supp_Exist)
            {
                strTemp += "12M_supp         = ";
                if (cGsdFileInfoEntity.bBaud_12M_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //";"
            //
            strTemp += ";";
            strTemp += "\r\n";
            //
            //MaxTsdr_9.6
            //
            if (cGsdFileInfoEntity.bMaxTsdr_9_6_Exist)
            {
                strTemp += "MaxTsdr_9.6         = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_9_6);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_19.2
            //
            if (cGsdFileInfoEntity.bMaxTsdr_19_2_Exist)
            {
                strTemp += "MaxTsdr_19.2        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_19_2);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_45.45
            //
            if (cGsdFileInfoEntity.bMaxTsdr_45_45_Exist)
            {
                strTemp += "MaxTsdr_45.45       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_45_45);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_93.75
            //
            if (cGsdFileInfoEntity.bMaxTsdr_93_75_Exist)
            {
                strTemp += "MaxTsdr_93.75       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_93_75);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_187.5
            //
            if (cGsdFileInfoEntity.bMaxTsdr_187_5_Exist)
            {
                strTemp += "MaxTsdr_187.5       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_187_5);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_500
            //
            if (cGsdFileInfoEntity.bMaxTsdr_500_Exist)
            {
                strTemp += "MaxTsdr_500         = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_500);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_1.5M
            //
            if (cGsdFileInfoEntity.bMaxTsdr_1_5M_Exist)
            {
                strTemp += "MaxTsdr_1.5M        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_1_5M);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_3M
            //
            if (cGsdFileInfoEntity.bMaxTsdr_3M_Exist)
            {
                strTemp += "MaxTsdr_3M          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_3M);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_6M
            //
            if (cGsdFileInfoEntity.bMaxTsdr_6M_Exist)
            {
                strTemp += "MaxTsdr_6M          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_6M);
                strTemp += "\r\n";
            }
            //
            //MaxTsdr_12M
            //
            if (cGsdFileInfoEntity.bMaxTsdr_12M_Exist)
            {
                strTemp += "MaxTsdr_12M         = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMaxTsdr_12M);
                strTemp += "\r\n";
            }
            //
            //";"
            //
            strTemp += ";";
            strTemp += "\r\n";
            //
            //Redundancy
            //
            if (cGsdFileInfoEntity.bRedundancy_Exist)
            {
                strTemp += "Redundancy            = ";
                if (cGsdFileInfoEntity.bRedundancy)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Repeater_Ctrl_Sig
            //
            if (cGsdFileInfoEntity.bRepeater_Ctrl_Sig_Exist)
            {
                strTemp += "Repeater_Ctrl_Sig     = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucRepeater_Ctrl_Sig);
                strTemp += "\r\n";
            }
            //
            //24V_Pins
            //
            if (cGsdFileInfoEntity.b24V_Pins_Exist)
            {
                strTemp += "24V_Pins              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uc24V_Pins);
                strTemp += "\r\n";
            }
            //
            //Implementation_Type
            //
            if (cGsdFileInfoEntity.bImplementation_Type_Exist)
            {
                strTemp += "Implementation_Type   = ";
                strTemp += "\"" + cGsdFileInfoEntity.strImplementation_Type + "\"";
                strTemp += "\r\n";
            }
            //
            //Bitmap_Device
            //
            if (cGsdFileInfoEntity.bBitmap_Device_Exist)
            {
                strTemp += "Bitmap_Device         = ";
                strTemp += "\"" + cGsdFileInfoEntity.strBitmap_Device + "\"";
                strTemp += "\r\n";
            }
            //
            //Bitmap_Diag
            //
            if (cGsdFileInfoEntity.bBitmap_Diag_Exist)
            {
                strTemp += "Bitmap_Diag           = ";
                strTemp += "\"" + cGsdFileInfoEntity.strBitmap_Diag + "\"";
                strTemp += "\r\n";
            }
            //
            //Bitmap_SF
            //
            if (cGsdFileInfoEntity.bBitmap_SF_Exist)
            {
                strTemp += "Bitmap_SF             = ";
                strTemp += "\"" + cGsdFileInfoEntity.strBitmap_SF + "\"";
                strTemp += "\r\n";
            }
            //
            //OrderNumber
            //
            if (cGsdFileInfoEntity.bOrderNumber_Exist)
            {
                strTemp += "OrderNumber           = ";
                strTemp += "\"" + cGsdFileInfoEntity.strOrderNumber + "\"";
                strTemp += "\r\n";
            }

            //
            //";"
            //
            strTemp += ";";
            strTemp += "\r\n";

            //
            //4.2 General specifications
            //4.2.2 dditional keywords for different physical interfaces
            //
            //暂不支持
            strTemp += "\r\n";
            strTemp += "*** 4.2 General specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.2.2 dditional keywords for different physical interfaces";
            strTemp += "\r\n";

            //
            //4.3 Master-related specifications
            //4.3.1 DP Master (Class 1) related keywords
            //
            strTemp += "\r\n";
            strTemp += "*** 4.3 Master-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.3.1 DP Master (Class 1) related keywords";
            strTemp += "\r\n";
            //
            //Master_Freeze_Mode_supp
            //
            if (cGsdFileInfoEntity.bMaster_Freeze_Mode_supp_Exist)
            {
                strTemp += "Master_Freeze_Mode_supp             = ";
                if (cGsdFileInfoEntity.bMaster_Freeze_Mode_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Sync_Mode_supp
            //
            if (cGsdFileInfoEntity.bMaster_Sync_Mode_supp_Exist)
            {
                strTemp += "Master_Freeze_Mode_supp             = ";
                if (cGsdFileInfoEntity.bMaster_Sync_Mode_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Fail_Safe_supp
            //
            if (cGsdFileInfoEntity.bMaster_Fail_Safe_supp_Exist)
            {
                strTemp += "Master_Freeze_Mode_supp             = ";
                if (cGsdFileInfoEntity.bMaster_Fail_Safe_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Download_supp
            //
            if (cGsdFileInfoEntity.bDownload_supp_Exist)
            {
                strTemp += "Master_Freeze_Mode_supp              = ";
                if (cGsdFileInfoEntity.bDownload_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Upload_supp
            //
            if (cGsdFileInfoEntity.bUpload_supp_Exist)
            {
                strTemp += "Upload_supp                         = ";
                if (cGsdFileInfoEntity.bUpload_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Act_Para_Brct_supp
            //
            if (cGsdFileInfoEntity.bAct_Para_Brct_supp_Exist)
            {
                strTemp += "Act_Para_Brct_supp                  = ";
                if (cGsdFileInfoEntity.bAct_Para_Brct_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Act_Param_supp
            //
            if (cGsdFileInfoEntity.bAct_Param_supp_Exist)
            {
                strTemp += "Act_Param_supp                      = ";
                if (cGsdFileInfoEntity.bAct_Param_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Max_MPS_Length
            //
            if (cGsdFileInfoEntity.bMax_MPS_Length_Exist)
            {
                strTemp += "Max_MPS_Length       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ulMax_MPS_Length);
                strTemp += "\r\n";
            }
            //
            //Max_Lsdu_MS
            //
            if (cGsdFileInfoEntity.bMax_Lsdu_MS_Exist)
            {
                strTemp += "Max_Lsdu_MS          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Lsdu_MS);
                strTemp += "\r\n";
            }
            //
            //Max_Lsdu_MM
            //
            if (cGsdFileInfoEntity.bMax_Lsdu_MM_Exist)
            {
                strTemp += "Max_Lsdu_MM          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Lsdu_MM);
                strTemp += "\r\n";
            }
            //
            //Min_Poll_Timeout
            //
            if (cGsdFileInfoEntity.bMin_Poll_Timeout_Exist)
            {
                strTemp += "Min_Poll_Timeout     = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMin_Poll_Timeout);
                strTemp += "\r\n";
            }
            //
            //Trdy_9.6
            //
            if (cGsdFileInfoEntity.bTrdy_9_6_Exist)
            {
                strTemp += "Trdy_9.6             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_9_6);
                strTemp += "\r\n";
            }
            //
            //Trdy_19.2
            //
            if (cGsdFileInfoEntity.bTrdy_19_2_Exist)
            {
                strTemp += "Trdy_9.6             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_19_2);
                strTemp += "\r\n";
            }
            //
            //Trdy_31.25
            //
            if (cGsdFileInfoEntity.bTrdy_31_25_Exist)
            {
                strTemp += "Trdy_31.25           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_31_25);
                strTemp += "\r\n";
            }
            //
            //Trdy_45.45
            //
            if (cGsdFileInfoEntity.bTrdy_45_45_Exist)
            {
                strTemp += "Trdy_45.45           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_45_45);
                strTemp += "\r\n";
            }
            //
            //Trdy_93.75
            //
            if (cGsdFileInfoEntity.bTrdy_93_75_Exist)
            {
                strTemp += "Trdy_93.75           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_93_75);
                strTemp += "\r\n";
            }
            //
            //Trdy_187.5
            //
            if (cGsdFileInfoEntity.bTrdy_187_5_Exist)
            {
                strTemp += "Trdy_187.5           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_187_5);
                strTemp += "\r\n";
            }
            //
            //Trdy_500
            //
            if (cGsdFileInfoEntity.bTrdy_187_5_Exist)
            {
                strTemp += "Trdy_500             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_187_5);
                strTemp += "\r\n";
            }
            //
            //Trdy_500
            //
            if (cGsdFileInfoEntity.bTrdy_500_Exist)
            {
                strTemp += "Trdy_500             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_500);
                strTemp += "\r\n";
            }
            //
            //Trdy_1.5M
            //
            if (cGsdFileInfoEntity.bTrdy_1_5M_Exist)
            {
                strTemp += "Trdy_1.5M            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_1_5M);
                strTemp += "\r\n";
            }
            //
            //Trdy_3M
            //
            if (cGsdFileInfoEntity.bTrdy_3M_Exist)
            {
                strTemp += "Trdy_3M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_3M);
                strTemp += "\r\n";
            }
            //
            //Trdy_6M
            //
            if (cGsdFileInfoEntity.bTrdy_6M_Exist)
            {
                strTemp += "Trdy_6M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_6M);
                strTemp += "\r\n";
            }
            //
            //Trdy_12M
            //
            if (cGsdFileInfoEntity.bTrdy_12M_Exist)
            {
                strTemp += "Trdy_12M             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTrdy_12M);
                strTemp += "\r\n";
            }
            //
            //Tqui_9.6
            //
            if (cGsdFileInfoEntity.bTqui_9_6_Exist)
            {
                strTemp += "Tqui_9.6             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_9_6);
                strTemp += "\r\n";
            }
            //
            //Tqui_19.2
            //
            if (cGsdFileInfoEntity.bTqui_19_2_Exist)
            {
                strTemp += "Tqui_19.2            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_19_2);
                strTemp += "\r\n";
            }
            //
            //Tqui_31.25
            //
            if (cGsdFileInfoEntity.bTqui_31_25_Exist)
            {
                strTemp += "Tqui_31.25           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_31_25);
                strTemp += "\r\n";
            }
            //
            //Tqui_45.45
            //
            if (cGsdFileInfoEntity.bTqui_45_45_Exist)
            {
                strTemp += "Tqui_45.45           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_45_45);
                strTemp += "\r\n";
            }
            //
            //Tqui_93.75
            //
            if (cGsdFileInfoEntity.bTqui_93_75_Exist)
            {
                strTemp += "Tqui_93.75           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_93_95);
                strTemp += "\r\n";
            }
            //
            //Tqui_183.5
            //
            if (cGsdFileInfoEntity.bTqui_183_5_Exist)
            {
                strTemp += "Tqui_183.5           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_183_5);
                strTemp += "\r\n";
            }
            //
            //Tqui_500
            //
            if (cGsdFileInfoEntity.bTqui_500_Exist)
            {
                strTemp += "Tqui_500             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_500);
                strTemp += "\r\n";
            }
            //
            //Tqui_1.5M
            //
            if (cGsdFileInfoEntity.bTqui_1_5M_Exist)
            {
                strTemp += "Tqui_1.5M            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_1_5M);
                strTemp += "\r\n";
            }
            //
            //Tqui_3M
            //
            if (cGsdFileInfoEntity.bTqui_3M_Exist)
            {
                strTemp += "Tqui_3M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_3M);
                strTemp += "\r\n";
            }
            //
            //Tqui_6M
            //
            if (cGsdFileInfoEntity.bTqui_6M_Exist)
            {
                strTemp += "Tqui_6M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_6M);
                strTemp += "\r\n";
            }
            //
            //Tqui_12M
            //
            if (cGsdFileInfoEntity.bTqui_12M_Exist)
            {
                strTemp += "Tqui_12M             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTqui_12M);
                strTemp += "\r\n";
            }
            //
            //Tset_9.6
            //
            if (cGsdFileInfoEntity.bTset_9_6_Exist)
            {
                strTemp += "Tset_9.6             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_9_6);
                strTemp += "\r\n";
            }
            //
            //Tset_19.2
            //
            if (cGsdFileInfoEntity.bTset_19_2_Exist)
            {
                strTemp += "Tset_19.2            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_19_2);
                strTemp += "\r\n";
            }
            //
            //Tset_31.25
            //
            if (cGsdFileInfoEntity.bTset_31_25_Exist)
            {
                strTemp += "Tset_31.25           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_31_25);
                strTemp += "\r\n";
            }
            //
            //Tset_45.45
            //
            if (cGsdFileInfoEntity.bTset_45_45_Exist)
            {
                strTemp += "Tset_45.45           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_45_45);
                strTemp += "\r\n";
            }
            //
            //Tset_93.75
            //
            if (cGsdFileInfoEntity.bTset_93_75_Exist)
            {
                strTemp += "Tset_93.75           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_93_75);
                strTemp += "\r\n";
            }
            //
            //Tset_183.5
            //
            if (cGsdFileInfoEntity.bTset_187_5_Exist)
            {
                strTemp += "Tset_183.5           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_187_5);
                strTemp += "\r\n";
            }
            //
            //Tset_500
            //
            if (cGsdFileInfoEntity.bTset_500_Exist)
            {
                strTemp += "Tset_500             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_500);
                strTemp += "\r\n";
            }
            //
            //Tset_1.5M
            //
            if (cGsdFileInfoEntity.bTset_1_5M_Exist)
            {
                strTemp += "Tset_1.5M            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_1_5M);
                strTemp += "\r\n";
            }
            //
            //Tset_3M
            //
            if (cGsdFileInfoEntity.bTset_3M_Exist)
            {
                strTemp += "Tset_3M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_3M);
                strTemp += "\r\n";
            }
            //
            //Tset_6M
            //
            if (cGsdFileInfoEntity.bTset_6M_Exist)
            {
                strTemp += "Tset_6M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_6M);
                strTemp += "\r\n";
            }
            //
            //Tset_12M
            //
            if (cGsdFileInfoEntity.bTset_12M_Exist)
            {
                strTemp += "Tset_12M             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTset_12M);
                strTemp += "\r\n";
            }
            //
            //LAS_Len
            //
            if (cGsdFileInfoEntity.bLAS_Len_Exist)
            {
                strTemp += "LAS_Len              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucLAS_Len);
                strTemp += "\r\n";
            }
            //
            //Tsdi_9.6
            //
            if (cGsdFileInfoEntity.bTsdi_9_6_Exist)
            {
                strTemp += "Tsdi_9.6             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_9_6);
                strTemp += "\r\n";
            }
            //
            //Tsdi_19.2
            //
            if (cGsdFileInfoEntity.bTsdi_19_2_Exist)
            {
                strTemp += "Tsdi_19.2            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_19_2);
                strTemp += "\r\n";
            }
            //
            //Tsdi_31.25
            //
            if (cGsdFileInfoEntity.bTsdi_31_25_Exist)
            {
                strTemp += "Tsdi_31.25           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_31_25);
                strTemp += "\r\n";
            }
            //
            //Tsdi_45.45
            //
            if (cGsdFileInfoEntity.bTsdi_45_45_Exist)
            {
                strTemp += "Tsdi_45.45           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_45_45);
                strTemp += "\r\n";
            }
            //
            //Tsdi_93.75
            //
            if (cGsdFileInfoEntity.bTsdi_93_75_Exist)
            {
                strTemp += "Tsdi_93.75           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_93_75);
                strTemp += "\r\n";
            }
            //
            //Tsdi_183.5
            //
            if (cGsdFileInfoEntity.bTsdi_187_5_Exist)
            {
                strTemp += "Tsdi_183.5           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_187_5);
                strTemp += "\r\n";
            }
            //
            //Tsdi_500
            //
            if (cGsdFileInfoEntity.bTsdi_500_Exist)
            {
                strTemp += "Tsdi_500             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_500);
                strTemp += "\r\n";
            }
            //
            //Tsdi_1.5M
            //
            if (cGsdFileInfoEntity.bTsdi_1_5M_Exist)
            {
                strTemp += "Tsdi_1.5M            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_1_5M);
                strTemp += "\r\n";
            }
            //
            //Tsdi_3M
            //
            if (cGsdFileInfoEntity.bTsdi_3M_Exist)
            {
                strTemp += "Tsdi_3M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_3M);
                strTemp += "\r\n";
            }
            //
            //Tsdi_6M
            //
            if (cGsdFileInfoEntity.bTsdi_6M_Exist)
            {
                strTemp += "Tsdi_6M              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_6M);
                strTemp += "\r\n";
            }
            //
            //Tsdi_12M
            //
            if (cGsdFileInfoEntity.bTsdi_12M_Exist)
            {
                strTemp += "Tsdi_12M             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucTsdi_12M);
                strTemp += "\r\n";
            }
            //
            //Max_Master_Input_Len
            //
            if (cGsdFileInfoEntity.bMax_Master_Input_Len_Exist)
            {
                strTemp += "Max_Master_Input_Len             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Master_Input_Len);
                strTemp += "\r\n";
            }
            //
            //Max_Master_Output_Len
            //
            if (cGsdFileInfoEntity.bMax_Master_Output_Len_Exist)
            {
                strTemp += "Max_Master_Output_Len            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Master_Output_Len);
                strTemp += "\r\n";
            }

            //
            //4.3 Master-related specifications
            //4.3.2 Additional master related keywords for DP extensions
            //
            strTemp += "\r\n";
            strTemp += "*** 4.3 Master-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.3.2 Additional master related keywords for DP extensions";
            strTemp += "\r\n";

            //
            //DPV1_Master
            //
            if (cGsdFileInfoEntity.bDPV1_Master_Exist)
            {
                strTemp += "DPV1_Master                         = ";
                if (cGsdFileInfoEntity.bDPV1_Master)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //C1_Master_Read_Write_supp
            //
            if (cGsdFileInfoEntity.bC1_Master_Read_Write_supp_Exist)
            {
                strTemp += "C1_Master_Read_Write_supp           = ";
                if (cGsdFileInfoEntity.bC1_Master_Read_Write_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_DPV1_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_DPV1_Alarm_supp_Exist)
            {
                strTemp += "Master_DPV1_Alarm_supp              = ";
                if (cGsdFileInfoEntity.bMaster_DPV1_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Diagnostic_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_Diagnostic_Alarm_supp_Exist)
            {
                strTemp += "Master_Diagnostic_Alarm_supp        = ";
                if (cGsdFileInfoEntity.bMaster_Diagnostic_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Process_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_Process_Alarm_supp_Exist)
            {
                strTemp += "Master_Process_Alarm_supp           = ";
                if (cGsdFileInfoEntity.bMaster_Process_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Pull_Plug_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_Pull_Plug_Alarm_supp__Exist)
            {
                strTemp += "Master_Pull_Plug_Alarm_supp         = ";
                if (cGsdFileInfoEntity.bMaster_Pull_Plug_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Status_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_Status_Alarm_supp_Exist)
            {
                strTemp += "Master_Status_Alarm_supp            = ";
                if (cGsdFileInfoEntity.bMaster_Status_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Update_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_Update_Alarm_supp_Exist)
            {
                strTemp += "Master_Update_Alarm_supp            = ";
                if (cGsdFileInfoEntity.bMaster_Update_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Manufacturer_Specific_Alarm_supp
            //
            if (cGsdFileInfoEntity.bMaster_Manufacturer_Specific_Alarm_supp_Exist)
            {
                strTemp += "Master_Manufacturer_Specific_Alarm_supp            = ";
                if (cGsdFileInfoEntity.bMaster_Manufacturer_Specific_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Extra_Alarm_SAP_supp
            //
            if (cGsdFileInfoEntity.bMaster_Extra_Alarm_SAP_supp_Exist)
            {
                strTemp += "Master_Extra_Alarm_SAP_supp         = ";
                if (cGsdFileInfoEntity.bMaster_Extra_Alarm_SAP_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Master_Alarm_Sequence_Mode
            //
            if (cGsdFileInfoEntity.bMaster_Alarm_Sequence_Mode_Exist)
            {
                strTemp += "Master_Alarm_Sequence_Mode          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMaster_Alarm_Sequence_Mode);
                strTemp += "\r\n";
            }

            //
            //4.3 Master-related specifications
            //4.3.3 Additional master related keywords for DP-V2        
            //
            strTemp += "\r\n";
            strTemp += "*** 4.3 Master-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.3.3 Additional master related keywords for DP-V2";
            strTemp += "\r\n";

            //
            //Isochron_Mode_Synchronised
            //
            if (cGsdFileInfoEntity.bIsochron_Mode_Synchronised_Exist)
            {
                strTemp += "Isochron_Mode_Synchronised          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucIsochron_Mode_Synchronised);
                strTemp += "\r\n";
            }
            //
            //DXB_Master_supp
            //
            if (cGsdFileInfoEntity.bDXB_Master_supp_Exist)
            {
                strTemp += "DXB_Master_supp           = ";
                if (cGsdFileInfoEntity.bDXB_Master_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //X_Master_Prm_SAP_supp
            //
            if (cGsdFileInfoEntity.bX_Master_Prm_SAP_supp_Exist)
            {
                strTemp += "X_Master_Prm_SAP_supp     = ";
                if (cGsdFileInfoEntity.bX_Master_Prm_SAP_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }

            //
            //4.4 Slave-related specifications
            //4.4.1 Basic DP-Slave related keywords
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.1 Basic DP-Slave related keywords";
            strTemp += "\r\n";

            //
            //Freeze_Mode_supp
            //
            if (cGsdFileInfoEntity.bFreeze_Mode_supp_Exist)
            {
                strTemp += "Freeze_Mode_supp          = ";
                if (cGsdFileInfoEntity.bFreeze_Mode_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Sync_Mode_supp
            //
            if (cGsdFileInfoEntity.bSync_Mode_supp_Exist)
            {
                strTemp += "Sync_Mode_supp            = ";
                if (cGsdFileInfoEntity.bSync_Mode_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Auto_Baud_supp
            //
            if (cGsdFileInfoEntity.bAuto_Baud_supp_Exist)
            {
                strTemp += "Auto_Baud_supp            = ";
                if (cGsdFileInfoEntity.bAuto_Baud_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Set_Slave_Add_supp
            //
            if (cGsdFileInfoEntity.bSet_Slave_Add_supp_Exist)
            {
                strTemp += "Set_Slave_Add_supp        =";
                if (cGsdFileInfoEntity.bSet_Slave_Add_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //User_Prm_Data_Len
            //
            if (cGsdFileInfoEntity.bUser_Prm_Data_Len_Exist)
            {
                strTemp += "User_Prm_Data_Len         = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucUser_Prm_Data_Len);
                strTemp += "\r\n";
            }
            //
            //User_Prm_Data
            //
            if (cGsdFileInfoEntity.bUser_Prm_Data_Exist)
            {
                strTemp += "User_Prm_Data             = ";
                strTemp += cGsdFileInfoEntity.strUser_Prm_Data;
                strTemp += "\r\n";
            }
            //
            //Min_Slave_Intervall
            //
            if (cGsdFileInfoEntity.bMin_Slave_Intervall_Exist)
            {
                strTemp += "Min_Slave_Intervall       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMin_Slave_Intervallp);
                strTemp += "\r\n";
            }
            //
            //Modular_Station
            //
            if (cGsdFileInfoEntity.bModular_Station_Exist)
            {
                strTemp += "Modular_Station           =";
                if (cGsdFileInfoEntity.bModular_Station)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Max_Module
            //
            if (cGsdFileInfoEntity.bMax_Module_Exist)
            {
                strTemp += "Max_Module                = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Module);
                strTemp += "\r\n";
            }
            //
            //Max_Input_Len
            //
            if (cGsdFileInfoEntity.bMax_Input_Len_Exist)
            {
                strTemp += "Max_Input_Len             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Input_Len);
                strTemp += "\r\n";
            }
            //
            //Max_Output_Len
            //
            if (cGsdFileInfoEntity.bMax_Output_Len_Exist)
            {
                strTemp += "Max_Output_Len            = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Output_Len);
                strTemp += "\r\n";
            }
            //
            //Max_Data_Len
            //
            if (cGsdFileInfoEntity.bMax_Data_Len_Exist)
            {
                strTemp += "Max_Data_Len              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiMax_Data_Len);
                strTemp += "\r\n";
            }

            //Unit_Diag_Bit
            strTemp += "\r\n";
            foreach (CUNIT_DIAG_BIT cUnitDiagBitNode in cGsdFileInfoEntity.Unit_Diag_Bit_List)
            {
                strTemp += "Unit_Diag_Bit(" + string.Format("{0:d}", cUnitDiagBitNode.uiBit) + ")" + "=" + "\"" + cUnitDiagBitNode.strDiag_Text + "\"" + "\r\n";
            }


            //(X_)Unit_Diag_Bit
            //(X_)Unit_Diag_Bit_Help
            //(X_)Unit_Diag_Not_Bit
            //(X_)Unit_Diag_Not_Bit_Help
            //(X_)Unit_Diag_Area
            //UnitDiagType

            //
            //Module
            //public List<CMODULE_INFO> asModuleInfoList = new List<CMODULE_INFO>();
            //
            strTemp += "\r\n";
            foreach (CMODULE_INFO cModuleNode in cGsdFileInfoEntity.asModuleInfoList)
            {
                strTemp += "Module" + "  " + "\"" + cModuleNode.strMod_Name + "\"" + "  " + cModuleNode.strConfig + "\r\n";

                //Module_Reference
                if (cModuleNode.bModule_Ref_Exist)
                {
                    strTemp += string.Format("{0:d}", cModuleNode.uiModule_Ref) + "\r\n";
                }

                //Preset
                if (cModuleNode.bPreset_Exist)
                {
                    strTemp += "Preset = " + string.Format("{0:d}", cModuleNode.ucPreset) + "\r\n";
                }

                //F_IO_StructureDescCRC
                if (cModuleNode.bF_IO_StructureDescCRC_Exist)
                {
                    strTemp += "F_IO_StructureDescCRC = " + string.Format("0x{0:X}", cModuleNode.ulF_IO_StructureDescCRC) + "\r\n";
                }

                //Ext_Module_Prm_Data_Len
                if (cModuleNode.ucExt_Module_Prm_Data_Len > 0)
                {
                    strTemp += "Ext_Module_Prm_Data_Len = " + string.Format("{0:d}", cModuleNode.ucExt_Module_Prm_Data_Len) + "\r\n";
                }

                //Ext_User_Prm_Data_Const()
                foreach (CGSD_DATA_CONST_INFO cDataNode in cModuleNode.Ext_User_Prm_Data_Const_List)
                {
                    if (cDataNode.ucDataLenL > 0)
                    {
                        string strTempL = "";
                        convertDataArrayToStringUSIGN8(cDataNode.aucData, cDataNode.ucDataLenL, ref strTempL, true);

                        strTemp += "Ext_User_Prm_Data_Const(" + string.Format("{0:d}", cDataNode.ucRefBeg) + ")" + " = " + strTempL + "\r\n";
                    }
                }

                //Ext_User_Prm_Data_Ref()
                if (cModuleNode.Ext_User_Prm_Data_Ref_List.Count > 0)
                {
                    strTemp += ";" + "\r\n";
                }
                foreach (CGSD_DATA_REF_INFO cDataNode in cModuleNode.Ext_User_Prm_Data_Ref_List)
                {
                    string strTempL = string.Format("{0:d}", cDataNode.uiData);
                    strTemp += "Ext_User_Prm_Data_Ref(" + string.Format("{0:d}", cDataNode.ucRefBeg) + ")" + " = " + strTempL + "\r\n";
                }

                //F_Ext_Module_Prm_Data_Len
                if (cModuleNode.ucF_Ext_Module_Prm_Data_Len > 0)
                {
                    strTemp += "F_Ext_Module_Prm_Data_Len = " + string.Format("{0:d}", cModuleNode.ucF_Ext_Module_Prm_Data_Len) + "\r\n";
                }

                //F_Ext_User_Prm_Data_Const()
                foreach (CGSD_DATA_CONST_INFO cDataNode in cModuleNode.F_Ext_User_Prm_Data_Const_List)
                {
                    if (cDataNode.ucDataLenL > 0)
                    {
                        string strTempL = "";
                        convertDataArrayToStringUSIGN8(cDataNode.aucData, cDataNode.ucDataLenL, ref strTempL, true);

                        strTemp += "F_Ext_User_Prm_Data_Const(" + string.Format("{0:d}", cDataNode.ucRefBeg) + ")" + " = " + strTempL + "\r\n";
                    }
                }

                //Ext_User_Prm_Data_Ref()
                foreach (CGSD_DATA_REF_INFO cDataNode in cModuleNode.F_Ext_User_Prm_Data_Ref_List)
                {
                    string strTempL = string.Format("{0:d}", cDataNode.uiData);
                    strTemp += "F_Ext_User_Prm_Data_Ref(" + string.Format("{0:d}", cDataNode.ucRefBeg) + ")" + " = " + strTempL + "\r\n";
                }

                //F_ParamDescCRC
                if (cModuleNode.bF_ParamDescCRC_Exist)
                {
                    strTemp += "F_ParamDescCRC = " + string.Format("0x{0:X}", cModuleNode.uiF_ParamDescCRC) + "\r\n";
                }

                //Channel_Diag
                strTemp += "\r\n";
                foreach (CCHANNEL_DIAG cChannelDiagNode in cModuleNode.Channel_Diag_List)
                {
                    strTemp += "Channel_Diag(" + string.Format("{0:d}", cChannelDiagNode.ucError_Type) + ")" + "=" + "\"" + cChannelDiagNode.strDiag_Text + "\"" + "\r\n";
                }
                strTemp += "\r\n";

                strTemp += "EndModule" + "\r\n";
                strTemp += "\r\n";
            }
            //
            //Channel_Diag
            //
            strTemp += "\r\n";
            foreach (CCHANNEL_DIAG cChannelDiagNode in cGsdFileInfoEntity.Channel_Diag_List)
            {
                strTemp += "Channel_Diag(" + string.Format("{0:d}", cChannelDiagNode.ucError_Type) + ")" + "=" + "\"" + cChannelDiagNode.strDiag_Text + "\"" + "\r\n";
            }
            strTemp += "\r\n";

            //Channel_Diag_Help

            //
            //Max_Diag_Data_Len
            //
            if (cGsdFileInfoEntity.bMax_Diag_Data_Len_Exist)
            {
                strTemp += "Max_Diag_Data_Len         = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Diag_Data_Len);
                strTemp += "\r\n";
            }
            //
            //Modul_Offset
            //
            if (cGsdFileInfoEntity.bModul_Offset_Exist)
            {
                strTemp += "Modul_Offset              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucModul_Offset);
                strTemp += "\r\n";
            }
            //
            //Slave_Family
            //
            if (cGsdFileInfoEntity.bSlave_Family_Exist)
            {
                strTemp += "Slave_Family              = ";
                strTemp += cGsdFileInfoEntity.strSlave_Family;
                strTemp += "\r\n";
            }
            //
            //Diag_Update_Delay
            //
            if (cGsdFileInfoEntity.bDiag_Update_Delay_Exist)
            {
                strTemp += "Diag_Update_Delay         = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucDiag_Update_Delay);
                strTemp += "\r\n";
            }
            //
            //Fail_Safe_required
            //
            if (cGsdFileInfoEntity.bFail_Safe_required_Exist)
            {
                strTemp += "Fail_Safe_required        =";
                if (cGsdFileInfoEntity.bFail_Safe_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Info_Text
            //
            if (cGsdFileInfoEntity.bInfo_Text_Exist)
            {
                strTemp += "Info_Text             = ";
                strTemp += "\"" + cGsdFileInfoEntity.strInfo_Text + "\"";
                strTemp += "\r\n";
            }
            //
            //Max_User_Prm_Data_Len
            //
            if (cGsdFileInfoEntity.bMax_User_Prm_Data_Len_Exist)
            {
                strTemp += "Max_User_Prm_Data_Len     = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_User_Prm_Data_Len);
                strTemp += "\r\n";
            }

            //
            //Ext_User_Prm_Data_Ref
            //public List<CGSD_DATA_REF_INFO> Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();
            //
            foreach (CGSD_DATA_REF_INFO cDataNode in cGsdFileInfoEntity.Ext_User_Prm_Data_Ref_List)
            {
                string strTempL = string.Format("{0:d}", cDataNode.uiData);
                strTemp += "Ext_User_Prm_Data_Ref(" + string.Format("{0:d}", cDataNode.ucRefBeg) + ")" + "  = " + strTempL + "\r\n";
            }

            //Ext_User_Prm_Data_Const
            //public List<CGSD_DATA_CONST_INFO> Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();
            //Ext_User_Prm_Data_Const()
            foreach (CGSD_DATA_CONST_INFO cDataNode in cGsdFileInfoEntity.Ext_User_Prm_Data_Const_List)
            {
                if (cDataNode.ucDataLenL > 0)
                {
                    string strTempL = "";
                    convertDataArrayToStringUSIGN8(cDataNode.aucData, cDataNode.ucDataLenL, ref strTempL, true);

                    strTemp += "Ext_User_Prm_Data_Const(" + string.Format("{0:d}", cDataNode.ucRefBeg) + ")" + " = " + strTempL + "\r\n";
                }
            }

            //ExtUserPrmData
            //public List<CEXT_USER_PRM_DATA_INFO> ExtUserPrmData_List = new List<CEXT_USER_PRM_DATA_INFO>();
            strTemp += "\r\n";
            foreach (CEXT_USER_PRM_DATA_INFO cExtUserPrmDataNode in cGsdFileInfoEntity.ExtUserPrmData_List)
            {
                strTemp += "ExtUserPrmData   = " + string.Format("{0:d}", cExtUserPrmDataNode.uiReference_Number) + "   \"" + cExtUserPrmDataNode.strExt_User_Prm_Data_Name + "\"";
                strTemp += "\r\n";

                if (cExtUserPrmDataNode.bPrm_Text_Ref_Exist)
                {
                    strTemp += "Prm_Text_Ref = " + string.Format("{0:d}", cExtUserPrmDataNode.uiPrm_Text_Ref);
                }
                strTemp += "\r\n";

                if (cExtUserPrmDataNode.strData_Type_Name != "")
                {
                    if (cExtUserPrmDataNode.strData_Type_Name == "Unsigned8")
                    {
                        strTemp += "Unsigned8  " +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_USIGN8.ucDefault_Value) + "  " +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_USIGN8.ucMin_Value) + "-" +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_USIGN8.ucMax_Value);
                        strTemp += "\r\n";
                    }
                    else if (cExtUserPrmDataNode.strData_Type_Name == "Bit")
                    {
                        strTemp += "Bit(" + string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BIT.ucBitRef) + ")  " +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BIT.ucDefault_Value) + "  " +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BIT.ucMin_Value) + "-" +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BIT.ucMax_Value);
                        strTemp += "\r\n";
                    }
                    else if (cExtUserPrmDataNode.strData_Type_Name == "BitArea")
                    {
                        strTemp += "BitArea(" + string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BITAREA.ucFirstBit) + "-" + string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BITAREA.ucLastBit) + ")  " +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BITAREA.ucDefault_Value) + "  " +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BITAREA.ucMin_Value) + "-" +
                                    string.Format("{0:d}", cExtUserPrmDataNode.cDataInfo_BITAREA.ucMax_Value);
                        strTemp += "\r\n";
                    }

                }

                strTemp += "EndExtUserPrmData";
                strTemp += "\r\n";
                strTemp += "\r\n";
            }

            //PrmText
            //public List<CPRM_TEXT_INFO> PrmText_List = new List<CPRM_TEXT_INFO>();
            strTemp += "\r\n";
            foreach (CPRM_TEXT_INFO cPrmTextNode in cGsdFileInfoEntity.PrmText_List)
            {
                strTemp += "PrmText   = " + string.Format("{0:d}", cPrmTextNode.uiReference_Number);
                strTemp += "\r\n";

                foreach (CPRM_TEXT_ITEM_INFO cPrmTextItemNode in cPrmTextNode.asPrm_Text_Item_List)
                {
                    strTemp += "Text(" + string.Format("{0:d}", cPrmTextItemNode.uiID) + ") = " + "\"" + cPrmTextItemNode.strText + "\"";
                    strTemp += "\r\n";
                }

                strTemp += "EndPrmText";
                strTemp += "\r\n";
                strTemp += "\r\n";
            }

            //
            //Prm_Block_Structure_supp
            //
            if (cGsdFileInfoEntity.bPrm_Block_Structure_supp_Exist)
            {
                strTemp += "Prm_Block_Structure_supp  =";
                if (cGsdFileInfoEntity.bPrm_Block_Structure_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Prm_Block_Structure_req
            //
            if (cGsdFileInfoEntity.bPrm_Block_Structure_req_Exist)
            {
                strTemp += "Prm_Block_Structure_req   =";
                if (cGsdFileInfoEntity.bPrm_Block_Structure_req)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Jokerblock_Slot
            //
            if (cGsdFileInfoEntity.bJokerblock_Slot_Exist)
            {
                strTemp += "Jokerblock_Slot           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucJokerblock_Slot);
                strTemp += "\r\n";
            }
            //
            //Jokerblock_Location
            //
            if (cGsdFileInfoEntity.bJokerblock_Location_Exist)
            {
                strTemp += "Jokerblock_Location       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucJokerblock_Location);
                strTemp += "\r\n";
            }
            //
            //Prm_Block_Structure_req
            //
            if (cGsdFileInfoEntity.bPrm_Block_Structure_req_Exist)
            {
                strTemp += "Prm_Block_Structure_req   =";
                if (cGsdFileInfoEntity.bPrm_Block_Structure_req)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //PrmCmd_supp
            //
            if (cGsdFileInfoEntity.bPrmCmd_supp_Exist)
            {
                strTemp += "PrmCmd_supp               =";
                if (cGsdFileInfoEntity.bPrmCmd_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //Slave_Max_Switch_Over_Time
            //
            if (cGsdFileInfoEntity.bSlave_Max_Switch_Over_Time_Exist)
            {
                strTemp += "Slave_Max_Switch_Over_Time     = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiSlave_Max_Switch_Over_Time);
                strTemp += "\r\n";
            }
            //Slave_Redundancy_supp
            //
            if (cGsdFileInfoEntity.bSlave_Redundancy_supp_Exist)
            {
                strTemp += "Slave_Redundancy_supp     = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucSlave_Redundancy_supp);
                strTemp += "\r\n";
            }
            //
            //Ident_Maintenance_supp
            //
            if (cGsdFileInfoEntity.bIdent_Maintenance_supp_Exist)
            {
                strTemp += "Ident_Maintenance_supp    =";
                if (cGsdFileInfoEntity.bIdent_Maintenance_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Time_Sync_supp
            //
            if (cGsdFileInfoEntity.bTime_Sync_supp_Exist)
            {
                strTemp += "Time_Sync_supp            =";
                if (cGsdFileInfoEntity.bTime_Sync_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //Max_iParameter_Size
            //
            if (cGsdFileInfoEntity.bMax_iParameter_Size_Exist)
            {
                strTemp += "Max_iParameter_Size       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ulMax_iParameter_Size);
                strTemp += "\r\n";
            }

            //
            //4.4 Slave-related specifications
            //4.4.2 Additional keywords for module assignment
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.2 Additional keywords for module assignment";
            strTemp += "\r\n";

            //        //SlotDefinition

            //
            //4.4 Slave-related specifications
            //4.4.3 Slave related keywords for DP extensions
            //PROFIBUS extensions mean the features of DP-V1 (see IEC 61784-1:2003 A3.1) and
            //list of options (see IEC 61784-1:2003 A3.1 and 7.2.3.2.5), compared to DP-V0.
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.3 Slave related keywords for DP extensions";
            strTemp += "\r\n";

            //
            //DPV1_Slave
            //
            if (cGsdFileInfoEntity.bDPV1_Slave_Exist)
            {
                strTemp += "DPV1_Slave                =";
                if (cGsdFileInfoEntity.bDPV1_Slave)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //C1_Read_Write_supp
            //
            if (cGsdFileInfoEntity.bC1_Read_Write_supp_Exist)
            {
                strTemp += "C1_Read_Write_supp        =";
                if (cGsdFileInfoEntity.bC1_Read_Write_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //C2_Read_Write_supp
            //
            if (cGsdFileInfoEntity.bC2_Read_Write_supp_Exist)
            {
                strTemp += "C2_Read_Write_supp        =";
                if (cGsdFileInfoEntity.bC2_Read_Write_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //C1_Max_Data_Len
            //
            if (cGsdFileInfoEntity.bC1_Max_Data_Len_Exist)
            {
                strTemp += "C1_Max_Data_Len           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucC1_Max_Data_Len);
                strTemp += "\r\n";
            }
            //C2_Max_Data_Len
            //
            if (cGsdFileInfoEntity.bC2_Max_Data_Len_Exist)
            {
                strTemp += "C2_Max_Data_Len           = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucC2_Max_Data_Len);
                strTemp += "\r\n";
            }
            //C1_Response_Timeout
            //
            if (cGsdFileInfoEntity.bC1_Response_Timeout_Exist)
            {
                strTemp += "C1_Response_Timeout       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiC1_Response_Timeout);
                strTemp += "\r\n";
            }
            //C2_Response_Timeout
            //
            if (cGsdFileInfoEntity.bC2_Response_Timeout_Exist)
            {
                strTemp += "C2_Response_Timeout       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiC2_Response_Timeout);
                strTemp += "\r\n";
            }
            //
            //C1_Read_Write_required
            //
            if (cGsdFileInfoEntity.bC1_Read_Write_required_Exist)
            {
                strTemp += "C1_Read_Write_required    =";
                if (cGsdFileInfoEntity.bC1_Read_Write_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //C2_Max_Count_Channels
            //
            if (cGsdFileInfoEntity.bC2_Max_Count_Channels_Exist)
            {
                strTemp += "C2_Max_Count_Channels     = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucC2_Max_Count_Channels);
                strTemp += "\r\n";
            }
            //Max_Initiate_PDU_Length
            //
            if (cGsdFileInfoEntity.bMax_Initiate_PDU_Length_Exist)
            {
                strTemp += "Max_Initiate_PDU_Length   = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucMax_Initiate_PDU_Length);
                strTemp += "\r\n";
            }
            //
            //Diagnostic_Alarm_supp
            //
            if (cGsdFileInfoEntity.bDiagnostic_Alarm_supp_Exist)
            {
                strTemp += "Diagnostic_Alarm_supp     =";
                if (cGsdFileInfoEntity.bDiagnostic_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Process_Alarm_supp
            //
            if (cGsdFileInfoEntity.bProcess_Alarm_supp_Exist)
            {
                strTemp += "Process_Alarm_supp        =";
                if (cGsdFileInfoEntity.bProcess_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Pull_Plug_Alarm_supp
            //
            if (cGsdFileInfoEntity.bPull_Plug_Alarm_supp_Exist)
            {
                strTemp += "Pull_Plug_Alarm_supp      =";
                if (cGsdFileInfoEntity.bPull_Plug_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Status_Alarm_supp
            //
            if (cGsdFileInfoEntity.bStatus_Alarm_supp_Exist)
            {
                strTemp += "Status_Alarm_supp         =";
                if (cGsdFileInfoEntity.bStatus_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Update_Alarm_supp
            //
            if (cGsdFileInfoEntity.bUpdate_Alarm_supp_Exist)
            {
                strTemp += "Update_Alarm_supp         =";
                if (cGsdFileInfoEntity.bUpdate_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Manufacturer_Specific_Alarm_supp
            //
            if (cGsdFileInfoEntity.bManufacturer_Specific_Alarm_supp_Exist)
            {
                strTemp += "Manufacturer_Specific_Alarm_supp    =";
                if (cGsdFileInfoEntity.bManufacturer_Specific_Alarm_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Extra_Alarm_SAP_supp
            //
            if (cGsdFileInfoEntity.bExtra_Alarm_SAP_supp_Exist)
            {
                strTemp += "Extra_Alarm_SAP_supp      =";
                if (cGsdFileInfoEntity.bExtra_Alarm_SAP_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //Alarm_Sequence_Mode_Count
            //
            if (cGsdFileInfoEntity.bAlarm_Sequence_Mode_Count_Exist)
            {
                strTemp += "Alarm_Sequence_Mode_Count = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucAlarm_Sequence_Mode_Count);
                strTemp += "\r\n";
            }
            //
            //Alarm_Type_Mode_supp
            //
            if (cGsdFileInfoEntity.bAlarm_Type_Mode_supp_Exist)
            {
                strTemp += "Alarm_Type_Mode_supp      =";
                if (cGsdFileInfoEntity.bAlarm_Type_Mode_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Diagnostic_Alarm_required
            //
            if (cGsdFileInfoEntity.bDiagnostic_Alarm_required_Exist)
            {
                strTemp += "Diagnostic_Alarm_required =";
                if (cGsdFileInfoEntity.bDiagnostic_Alarm_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Process_Alarm_required
            //
            if (cGsdFileInfoEntity.bProcess_Alarm_required_Exist)
            {
                strTemp += "Process_Alarm_required    =";
                if (cGsdFileInfoEntity.bProcess_Alarm_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Pull_Plug_Alarm_required
            //
            if (cGsdFileInfoEntity.bPull_Plug_Alarm_required_Exist)
            {
                strTemp += "Pull_Plug_Alarm_required  =";
                if (cGsdFileInfoEntity.bPull_Plug_Alarm_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Status_Alarm_required
            //
            if (cGsdFileInfoEntity.bStatus_Alarm_required_Exist)
            {
                strTemp += "Status_Alarm_required     =";
                if (cGsdFileInfoEntity.bStatus_Alarm_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Update_Alarm_required
            //
            if (cGsdFileInfoEntity.bUpdate_Alarm_required_Exist)
            {
                strTemp += "Update_Alarm_required     =";
                if (cGsdFileInfoEntity.bUpdate_Alarm_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Manufacturer_Specific_Alarm_required
            //
            if (cGsdFileInfoEntity.bManufacturer_Specific_Alarm_required_Exist)
            {
                strTemp += "Manufacturer_Specific_Alarm_required     =";
                if (cGsdFileInfoEntity.bManufacturer_Specific_Alarm_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //DPV1_Data_Types
            //
            if (cGsdFileInfoEntity.bDPV1_Data_Types_Exist)
            {
                strTemp += "DPV1_Data_Types           =";
                if (cGsdFileInfoEntity.bDPV1_Data_Types)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //WD_Base_1ms_supp
            //
            if (cGsdFileInfoEntity.bWD_Base_1ms_supp_Exist)
            {
                strTemp += "WD_Base_1ms_supp          =";
                if (cGsdFileInfoEntity.bWD_Base_1ms_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Check_Cfg_Mode
            //
            if (cGsdFileInfoEntity.bCheck_Cfg_Mode_Exist)
            {
                strTemp += "Check_Cfg_Mode            =";
                if (cGsdFileInfoEntity.bCheck_Cfg_Mode)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }

            //
            //4.4 Slave-related specifications
            //4.4.4 Slave related keywords for Data Exchange with Broadcast
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.4 Slave related keywords for Data Exchange with Broadcast";
            strTemp += "\r\n";

            //
            //Publisher_supp
            //
            if (cGsdFileInfoEntity.bPublisher_supp_Exist)
            {
                strTemp += "Publisher_supp            =";
                if (cGsdFileInfoEntity.bPublisher_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Subscriber_supp
            //
            if (cGsdFileInfoEntity.bSubscriber_supp_Exist)
            {
                strTemp += "Subscriber_supp           =";
                if (cGsdFileInfoEntity.bSubscriber_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //DXB_Max_Link_Count
            //
            if (cGsdFileInfoEntity.bDXB_Max_Link_Count_Exist)
            {
                strTemp += "DXB_Max_Link_Count        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucDXB_Max_Link_Count);
                strTemp += "\r\n";
            }
            //
            //DXB_Max_Data_Length
            //
            if (cGsdFileInfoEntity.bDXB_Max_Data_Length_Exist)
            {
                strTemp += "DXB_Max_Data_Length       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucDXB_Max_Data_Length);
                strTemp += "\r\n";
            }
            //
            //DXB_Subscribertable_Block_Location
            //
            if (cGsdFileInfoEntity.bDXB_Subscribertable_Block_Location_Exist)
            {
                strTemp += "DXB_Subscribertable_Block_Location       = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucDXB_Subscribertable_Block_Location);
                strTemp += "\r\n";
            }

            //
            //4.4 Slave-related specifications
            //4.4.5 Slave related keywords for Isochronous Mode
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.5 Slave related keywords for Isochronous Mode";
            strTemp += "\r\n";

            //
            //Isochron_Mode_supp
            //
            if (cGsdFileInfoEntity.bIsochron_Mode_supp_Exist)
            {
                strTemp += "Isochron_Mode_supp        =";
                if (cGsdFileInfoEntity.bIsochron_Mode_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //Isochron_Mode_required
            //
            if (cGsdFileInfoEntity.bIsochron_Mode_required_Exist)
            {
                strTemp += "Isochron_Mode_required    =";
                if (cGsdFileInfoEntity.bIsochron_Mode_required)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //TBASE_DP
            //
            if (cGsdFileInfoEntity.bTBASE_DP_Exist)
            {
                strTemp += "TBASE_DP             = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ulTBASE_DP);
                strTemp += "\r\n";
            }
            //
            //TDP_MIN
            //
            if (cGsdFileInfoEntity.bTDP_MIN_Exist)
            {
                strTemp += "TDP_MIN              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiTDP_MIN);
                strTemp += "\r\n";
            }
            //
            //TDP_MAX
            //
            if (cGsdFileInfoEntity.bTDP_MAXN_Exist)
            {
                strTemp += "TDP_MAX              = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiTDP_MAX);
                strTemp += "\r\n";
            }
            //
            //T_PLL_W_MAX
            //
            if (cGsdFileInfoEntity.bT_PLL_W_MAX_Exist)
            {
                strTemp += "T_PLL_W_MAX          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiT_PLL_W_MAX);
                strTemp += "\r\n";
            }
            //
            //TI_MIN
            //
            if (cGsdFileInfoEntity.bTI_MIN_Exist)
            {
                strTemp += "TI_MIN               = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiTI_MIN);
                strTemp += "\r\n";
            }
            //
            //TO_MIN
            //
            if (cGsdFileInfoEntity.bTO_MIN_Exist)
            {
                strTemp += "TO_MIN               = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiTO_MIN);
                strTemp += "\r\n";
            }

            //
            //4.4 Slave-related specifications
            //4.4.6 Slave related keywords for PROFIsafe Profile
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.6 Slave related keywords for PROFIsafe Profile";
            strTemp += "\r\n";

            //
            //F_ParamDescCRC
            //
            if (cGsdFileInfoEntity.bF_ParamDescCRC_Exist)
            {
                strTemp += "F_ParamDescCRC                 = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.uiF_ParamDescCRC);
                strTemp += "\r\n";
            }
            //
            //F_IO_StructureDescCRC
            //
            if (cGsdFileInfoEntity.bF_IO_StructureDescCRC_Exist)
            {
                strTemp += "F_IO_StructureDescCRC          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ulF_IO_StructureDescCRC);
                strTemp += "\r\n";
            }

            //F_Ext_User_Prm_Data_Ref
            //public List<CGSD_DATA_REF_INFO> F_Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();

            //F_Ext_User_Prm_Data_Const
            //public byte ucF_ExtModulePrmDataLen = 0;            //F_User_Prm_Data数据长度(1~237 )
            //public List<CGSD_DATA_CONST_INFO> F_Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();

            //
            //4.4 Slave-related specifications
            //4.4.7 Slave related keywords for extended parameterization
            //
            strTemp += "\r\n";
            strTemp += "*** 4.4 Slave-related specifications";
            strTemp += "\r\n";
            strTemp += "*** 4.4.7 Slave related keywords for extended parameterization";
            strTemp += "\r\n";

            //
            //X_Prm_SAP_supp
            //
            if (cGsdFileInfoEntity.bX_Prm_SAP_supp_Exist)
            {
                strTemp += "X_Prm_SAP_supp       =";
                if (cGsdFileInfoEntity.bX_Prm_SAP_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }
            //
            //X_Max_User_Prm_Data_Len
            //
            if (cGsdFileInfoEntity.bX_Max_User_Prm_Data_Len_Exist)
            {
                strTemp += "X_Max_User_Prm_Data_Len        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucX_Max_User_Prm_Data_Len);
                strTemp += "\r\n";
            }
            //
            //X_Ext_Module_Prm_Data_Len
            //
            if (cGsdFileInfoEntity.bX_Ext_Module_Prm_Data_Len_Exist)
            {
                strTemp += "X_Max_User_Prm_Data_Len        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucX_Ext_Module_Prm_Data_Len);
                strTemp += "\r\n";
            }

            //X_Ext_User_Prm_Data_Ref
            //X_Ext_User_Prm_Data_Const

            //
            //X_Prm_Block_Structure_supp
            //
            if (cGsdFileInfoEntity.bX_Prm_Block_Structure_supp_Exist)
            {
                strTemp += "X_Prm_Block_Structure_supp     =";
                if (cGsdFileInfoEntity.bX_Prm_Block_Structure_supp)
                {
                    strTemp += string.Format("{0:d}", 1);
                }
                else
                {
                    strTemp += string.Format("{0:d}", 0);
                }
                strTemp += "\r\n";
            }

            //
            //4.4 Slave-related specifications
            //4.4.8 Slave related keywords for subsystems
            //
            //
            //Subsys_Dir_Index
            //
            if (cGsdFileInfoEntity.bSubsys_Dir_Index_Exist)
            {
                strTemp += "Subsys_Dir_Index          = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucSubsys_Dir_Index);
                strTemp += "\r\n";
            }
            //
            //Subsys_Module_Dir_Index
            //
            if (cGsdFileInfoEntity.bSubsys_Module_Dir_Index_Exist)
            {
                strTemp += "Subsys_Module_Dir_Index        = ";
                strTemp += string.Format("{0:d}", cGsdFileInfoEntity.ucSubsys_Module_Dir_Index);
                strTemp += "\r\n";
            }


            //ttt




            //textBox2.Text = strTemp;

        }
        ///////////     END OF FILE     /////////////
    }


    //
    //GSD文件信息定义
    //
    public class CGSD_FILE_INFO
    {
        //
        //4.2 General specifications
        //4.2.1 General DP keywords
        //
        //GSD_Revision
        public bool bGSD_Revision_Exist = false;
        public byte ucGSD_Revision;

        //Vendor_Name
        public bool bVendor_Name_Exist = false;
        public string strVendor_Name;                       //Visible-String (32)

        //Model_Name
        public bool bModel_Name_Exist = false;
        public string strModel_Name;                        //Visible-String (32)

        //Revision
        public bool bRevision_Exist = false;
        public string strRevision;                          //Visible-String (32)

        //Revision_Number
        public bool bRevision_Number_Exist = false;
        public byte ucRevision_Number;                      //1 - 63

        //Ident_Number
        public bool bIdent_Number_Exist = false;
        public ushort uiIdent_Number;                       //0: PROFIBUS DP; 16 - 255: Manufacturer-specific

        //Protocol_Ident
        public bool bProtocol_Ident_Exist = false;
        public byte ucProtocol_Ident;                       //0: PROFIBUS DP; 16 - 255: Manufacturer-specific

        //Station_Type 
        public bool bStation_Type_Exist = false;
        public byte ucStation_Type;                         //0: DP Slave; 1: DP Master (Class 1)

        //FMS_supp 
        public bool bFMS_supp_Exist = false;
        public bool bFMS_supp;

        //Hardware_Release
        public bool bHardware_Release_Exist = false;
        public string strHardware_Release;

        //Software_Release
        public bool bSoftware_Release_Exist = false;
        public string strSoftware_Release;

        //9.6_supp 
        public bool bBaud_9_6_supp_Exist = false;
        public bool bBaud_9_6_supp;

        //19.2_supp 
        public bool bBaud_19_2_supp_Exist = false;
        public bool bBaud_19_2_supp;

        //45.45_supp 
        public bool bBaud_45_45_supp_Exist = false;
        public bool bBaud_45_45_supp;

        //93.75_supp 
        public bool bBaud_93_75_supp_Exist = false;
        public bool bBaud_93_75_supp;

        //187.5_supp 
        public bool bBaud_187_5_supp_Exist = false;
        public bool bBaud_187_5_supp;

        //500_supp 
        public bool bBaud_500_supp_Exist = false;
        public bool bBaud_500_supp;

        //1.5M_supp 
        public bool bBaud_1_5M_supp_Exist = false;
        public bool bBaud_1_5M_supp;

        //3M_supp 
        public bool bBaud_3M_supp_Exist = false;
        public bool bBaud_3M_supp;

        //6M_supp 
        public bool bBaud_6M_supp_Exist = false;
        public bool bBaud_6M_supp;

        //12M_supp 
        public bool bBaud_12M_supp_Exist = false;
        public bool bBaud_12M_supp;

        //MaxTsdr_9.6
        public bool bMaxTsdr_9_6_Exist = false;
        public ushort uiMaxTsdr_9_6;                            //Time base: Bit Time

        //MaxTsdr_19.2
        public bool bMaxTsdr_19_2_Exist = false;
        public ushort uiMaxTsdr_19_2;

        //MaxTsdr_45.45
        public bool bMaxTsdr_45_45_Exist = false;
        public ushort uiMaxTsdr_45_45;

        //MaxTsdr_93_75
        public bool bMaxTsdr_93_75_Exist = false;
        public ushort uiMaxTsdr_93_75;

        //MaxTsdr_187.5
        public bool bMaxTsdr_187_5_Exist = false;
        public ushort uiMaxTsdr_187_5;

        //MaxTsdr_500
        public bool bMaxTsdr_500_Exist = false;
        public ushort uiMaxTsdr_500;

        //MaxTsdr_1.5M
        public bool bMaxTsdr_1_5M_Exist = false;
        public ushort uiMaxTsdr_1_5M;

        //MaxTsdr_3M
        public bool bMaxTsdr_3M_Exist = false;
        public ushort uiMaxTsdr_3M;

        //MaxTsdr_6M
        public bool bMaxTsdr_6M_Exist = false;
        public ushort uiMaxTsdr_6M;

        //MaxTsdr_12M
        public bool bMaxTsdr_12M_Exist = false;
        public ushort uiMaxTsdr_12M;                                //Time base: Bit Time

        //Redundancy
        public bool bRedundancy_Exist = false;
        public bool bRedundancy;                                    //0: No, 1: Redundancy is supported

        //Repeater_Ctrl_Sig
        public bool bRepeater_Ctrl_Sig_Exist = false;
        public byte ucRepeater_Ctrl_Sig;                            //0: Not connected, 1: RS485, 2: TTL

        //24V_Pins
        public bool b24V_Pins_Exist = false;
        public byte uc24V_Pins;                                     //0: Not connected, 1: Input, 2: Output

        //Implementation_Type
        public bool bImplementation_Type_Exist = false;
        public string strImplementation_Type;                       //Visible-String (32), e.g. "SPC3"

        //Bitmap_Device
        public bool bBitmap_Device_Exist = false;
        public string strBitmap_Device;                             //Visible-String (8)

        //Bitmap_Diag
        public bool bBitmap_Diag_Exist = false;
        public string strBitmap_Diag;                                 //Visible-String (8)

        //Bitmap_SF
        public bool bBitmap_SF_Exist = false;
        public string strBitmap_SF;                                 //Visible-String (8); 70*40 pixels (width*height) in 16 colors.

        //OrderNumber
        public bool bOrderNumber_Exist = false;
        public string strOrderNumber;

        //
        //4.2 General specifications
        //4.2.2 dditional keywords for different physical interfaces
        //
        //暂不支持

        //
        //4.3 Master-related specifications
        //4.3.1 DP Master (Class 1) related keywords
        //
        //Master_Freeze_Mode_supp: (D starting with GSD_Revision 3)
        public bool bMaster_Freeze_Mode_supp_Exist = false;
        public bool bMaster_Freeze_Mode_supp;

        //Master_Sync_Mode_supp: (D starting with GSD_Revision 3)
        public bool bMaster_Sync_Mode_supp_Exist = false;
        public bool bMaster_Sync_Mode_supp;

        //Master_Fail_Safe_supp: (D starting with GSD_Revision 3)
        public bool bMaster_Fail_Safe_supp_Exist = false;
        public bool bMaster_Fail_Safe_supp;

        //Download_supp: (D)
        public bool bDownload_supp_Exist = false;
        public bool bDownload_supp;

        //Upload_supp: (D)
        public bool bUpload_supp_Exist = false;
        public bool bUpload_supp;

        //Act_Para_Brct_supp: (D)
        public bool bAct_Para_Brct_supp_Exist = false;
        public bool bAct_Para_Brct_supp;

        //Act_Param_supp: (D)
        public bool bAct_Param_supp_Exist = false;
        public bool bAct_Param_supp;

        //Max_MPS_Length: (D)
        public bool bMax_MPS_Length_Exist = false;
        public ulong ulMax_MPS_Length;

        //Max_Lsdu_MS: (D)
        public bool bMax_Lsdu_MS_Exist = false;
        public byte ucMax_Lsdu_MS;

        //Max_Lsdu_MM: (D)
        public bool bMax_Lsdu_MM_Exist = false;
        public byte ucMax_Lsdu_MM;

        //Min_Poll_Timeout: (D)
        public bool bMin_Poll_Timeout_Exist = false;
        public ushort uiMin_Poll_Timeout;                           //Time base: 10 ms

        //Trdy_9.6: (G)
        public bool bTrdy_9_6_Exist = false;
        public byte ucTrdy_9_6;                                     //Time base: Bit Time

        //Trdy_19.2: (G)
        public bool bTrdy_19_2_Exist = false;
        public byte ucTrdy_19_2;

        //Trdy_31.25: (G: starting with GSD_Revision 2)
        public bool bTrdy_31_25_Exist = false;
        public byte ucTrdy_31_25;

        //Trdy_45.45: (G: starting with GSD_Revision 2)
        public bool bTrdy_45_45_Exist = false;
        public byte ucTrdy_45_45;

        //Trdy_93.75: (G)
        public bool bTrdy_93_75_Exist = false;
        public byte ucTrdy_93_75;

        //Trdy_187.5: (G)
        public bool bTrdy_187_5_Exist = false;
        public byte ucTrdy_187_5;

        //Trdy_500: (G)
        public bool bTrdy_500_Exist = false;
        public byte ucTrdy_500;

        //Trdy_1.5M: (G)
        public bool bTrdy_1_5M_Exist = false;
        public byte ucTrdy_1_5M;

        //Trdy_3M: (G starting with GSD_Revision 1)
        public bool bTrdy_3M_Exist = false;
        public byte ucTrdy_3M;

        //Trdy_6M: (G starting with GSD_Revision 1)
        public bool bTrdy_6M_Exist = false;
        public byte ucTrdy_6M;

        //Trdy_12M: (G starting with GSD_Revision 1)
        public bool bTrdy_12M_Exist = false;
        public byte ucTrdy_12M;

        //Tqui_9.6: (G)
        public bool bTqui_9_6_Exist = false;
        public byte ucTqui_9_6;                                     //Time base: Bit Time

        //Tqui_19.2: (G)
        public bool bTqui_19_2_Exist = false;
        public byte ucTqui_19_2;

        //Tqui_31.25: (G)
        public bool bTqui_31_25_Exist = false;
        public byte ucTqui_31_25;

        //Tqui_45.45: (G)
        public bool bTqui_45_45_Exist = false;
        public byte ucTqui_45_45;

        //Tqui_93.75: (G)
        public bool bTqui_93_75_Exist = false;
        public byte ucTqui_93_95;

        //Tqui_183.5: (G)
        public bool bTqui_183_5_Exist = false;
        public byte ucTqui_183_5;

        //Tqui_500: (G)
        public bool bTqui_500_Exist = false;
        public byte ucTqui_500;

        //Tqui_1.5M: (G)
        public bool bTqui_1_5M_Exist = false;
        public byte ucTqui_1_5M;

        //Tqui_3M: (G)
        public bool bTqui_3M_Exist = false;
        public byte ucTqui_3M;

        //Tqui_6M: (G)
        public bool bTqui_6M_Exist = false;
        public byte ucTqui_6M;

        //Tqui_12M: (G)
        public bool bTqui_12M_Exist = false;
        public byte ucTqui_12M;

        //Tset_9.6: (G)
        public bool bTset_9_6_Exist = false;
        public byte ucTset_9_6;

        //Tset_19.2: (G)
        public bool bTset_19_2_Exist = false;
        public byte ucTset_19_2;

        //Tset_31.25: (G)
        public bool bTset_31_25_Exist = false;
        public byte ucTset_31_25;

        //Tset_45.45: (G)
        public bool bTset_45_45_Exist = false;
        public byte ucTset_45_45;

        //Tset_93.75: (G)
        public bool bTset_93_75_Exist = false;
        public byte ucTset_93_75;

        //Tset_187.5: (G)
        public bool bTset_187_5_Exist = false;
        public byte ucTset_187_5;

        //Tset_500: (G)
        public bool bTset_500_Exist = false;
        public byte ucTset_500;

        //Tset_1.5M: (G)
        public bool bTset_1_5M_Exist = false;
        public byte ucTset_1_5M;

        //Tset_3M: (G)
        public bool bTset_3M_Exist = false;
        public byte ucTset_3M;

        //Tset_6M: (G)
        public bool bTset_6M_Exist = false;
        public byte ucTset_6M;

        //Tset_12M: (G)
        public bool bTset_12M_Exist = false;
        public byte ucTset_12M;

        //LAS_Len
        public bool bLAS_Len_Exist = false;
        public byte ucLAS_Len;

        //Tsdi_9.6
        public bool bTsdi_9_6_Exist = false;
        public byte ucTsdi_9_6;

        //Tsdi_19.2
        public bool bTsdi_19_2_Exist = false;
        public byte ucTsdi_19_2;

        //Tsdi_31.25
        public bool bTsdi_31_25_Exist = false;
        public byte ucTsdi_31_25;

        //Tsdi_45.45
        public bool bTsdi_45_45_Exist = false;
        public byte ucTsdi_45_45;

        //Tsdi_93.75
        public bool bTsdi_93_75_Exist = false;
        public byte ucTsdi_93_75;

        //Tsdi_187.5
        public bool bTsdi_187_5_Exist = false;
        public byte ucTsdi_187_5;

        //Tsdi_500
        public bool bTsdi_500_Exist = false;
        public byte ucTsdi_500;

        //Tsdi_1.5M
        public bool bTsdi_1_5M_Exist = false;
        public byte ucTsdi_1_5M;

        //Tsdi_3M
        public bool bTsdi_3M_Exist = false;
        public byte ucTsdi_3M;

        //Tsdi_6M
        public bool bTsdi_6M_Exist = false;
        public byte ucTsdi_6M;

        //Tsdi_12M
        public bool bTsdi_12M_Exist = false;
        public byte ucTsdi_12M;

        //Max_Master_Input_Len
        public bool bMax_Master_Input_Len_Exist = false;
        public byte ucMax_Master_Input_Len;

        //Max_Master_Output_Len
        public bool bMax_Master_Output_Len_Exist = false;
        public byte ucMax_Master_Output_Len;


        //
        //4.3 Master-related specifications
        //4.3.2 Additional master related keywords for DP extensions
        //
        //DPV1_Master
        public bool bDPV1_Master_Exist = false;
        public bool bDPV1_Master;

        //C1_Master_Read_Write_supp
        public bool bC1_Master_Read_Write_supp_Exist = false;
        public bool bC1_Master_Read_Write_supp;

        //Master_DPV1_Alarm_supp
        public bool bMaster_DPV1_Alarm_supp_Exist = false;
        public bool bMaster_DPV1_Alarm_supp;

        //Master_Diagnostic_Alarm_supp
        public bool bMaster_Diagnostic_Alarm_supp_Exist = false;
        public bool bMaster_Diagnostic_Alarm_supp;

        //Master_Process_Alarm_supp
        public bool bMaster_Process_Alarm_supp_Exist = false;
        public bool bMaster_Process_Alarm_supp;

        //Master_Pull_Plug_Alarm_supp
        public bool bMaster_Pull_Plug_Alarm_supp__Exist = false;
        public bool bMaster_Pull_Plug_Alarm_supp;

        //Master_Status_Alarm_supp
        public bool bMaster_Status_Alarm_supp_Exist = false;
        public bool bMaster_Status_Alarm_supp;

        //Master_Update_Alarm_supp
        public bool bMaster_Update_Alarm_supp_Exist = false;
        public bool bMaster_Update_Alarm_supp;

        //Master_Manufacturer_Specific_Alarm_supp
        public bool bMaster_Manufacturer_Specific_Alarm_supp_Exist = false;
        public bool bMaster_Manufacturer_Specific_Alarm_supp;

        //Master_Extra_Alarm_SAP_supp
        public bool bMaster_Extra_Alarm_SAP_supp_Exist = false;
        public bool bMaster_Extra_Alarm_SAP_supp;

        //Master_Alarm_Sequence_Mode
        public bool bMaster_Alarm_Sequence_Mode_Exist = false;
        public byte ucMaster_Alarm_Sequence_Mode;                           //0: Sequence_Mode not supported; 1: 2 alarms in total; 2: 4 alarms in total
                                                                            //3: 8 alarms in total; 4: 12 alarms in total; 5: 16 alarms in total 
                                                                            //6: 24 alarms in total; 7: 32 alarms in total



        //
        //4.3 Master-related specifications
        //4.3.3 Additional master related keywords for DP-V2        
        //
        //Isochron_Mode_Synchronised
        public bool bIsochron_Mode_Synchronised_Exist = false;
        public byte ucIsochron_Mode_Synchronised;                           //0: Master device does not support the Isochron_Mode
                                                                            //1: Master device supports only the buffer synchronized Isochron_Mode (refer to IEC
                                                                            //61158-5:2003, 8.2.2.4.3.2)
                                                                            //2: Master device supports only the enhanced synchronized Isochron_Mode (refer to
                                                                            //IEC 61158-5:2003, 8.2.2.4.3.3)
                                                                            //3: Master device supports both, the buffer synchronized and the enhanced
                                                                            //synchronized Isochron_Mode.


        //DXB_Master_supp
        public bool bDXB_Master_supp_Exist = false;
        public bool bDXB_Master_supp;

        //X_Master_Prm_SAP_supp
        public bool bX_Master_Prm_SAP_supp_Exist = false;
        public bool bX_Master_Prm_SAP_supp;


        //
        //4.4 Slave-related specifications
        //4.4.1 Basic DP-Slave related keywords
        //
        //Freeze_Mode_supp
        public bool bFreeze_Mode_supp_Exist = false;
        public bool bFreeze_Mode_supp;

        //Sync_Mode_supp
        public bool bSync_Mode_supp_Exist = false;
        public bool bSync_Mode_supp;

        //Auto_Baud_supp
        public bool bAuto_Baud_supp_Exist = false;
        public bool bAuto_Baud_supp;

        //Set_Slave_Add_supp
        public bool bSet_Slave_Add_supp_Exist = false;
        public bool bSet_Slave_Add_supp;

        //User_Prm_Data_Len
        public bool bUser_Prm_Data_Len_Exist = false;
        public byte ucUser_Prm_Data_Len;

        //User_Prm_Data
        public bool bUser_Prm_Data_Exist = false;
        public string strUser_Prm_Data;
        public byte[] aucUser_Prm_Data = new byte[255];
        public byte ucUser_Prm_Data_LenL;

        //Min_Slave_Intervall
        public bool bMin_Slave_Intervall_Exist = false;
        public ushort uiMin_Slave_Intervallp;

        //Modular_Station
        public bool bModular_Station_Exist = false;
        public bool bModular_Station;                                   //0: compact device; 1: modular device

        //Max_Module
        public bool bMax_Module_Exist = false;
        public byte ucMax_Module;

        //Max_Input_Len
        public bool bMax_Input_Len_Exist = false;
        public byte ucMax_Input_Len;

        //Max_Output_Len
        public bool bMax_Output_Len_Exist = false;
        public byte ucMax_Output_Len;

        //Max_Data_Len
        public bool bMax_Data_Len_Exist = false;
        public ushort uiMax_Data_Len;

        //Unit_Diag_Bit
        public List<CUNIT_DIAG_BIT> Unit_Diag_Bit_List = new List<CUNIT_DIAG_BIT>();


        //X_Unit_Diag_Bit
        //(X_)Unit_Diag_Bit_Help
        //(X_)Unit_Diag_Not_Bit
        //(X_)Unit_Diag_Not_Bit_Help
        //(X_)Unit_Diag_Area
        //UnitDiagType

        //Module
        public List<CMODULE_INFO> asModuleInfoList = new List<CMODULE_INFO>();

        //Channel_Diag
        public List<CCHANNEL_DIAG> Channel_Diag_List = new List<CCHANNEL_DIAG>();

        //Channel_Diag_Help


        //Max_Diag_Data_Len
        public bool bMax_Diag_Data_Len_Exist = false;
        public byte ucMax_Diag_Data_Len;

        //Modul_Offset
        public bool bModul_Offset_Exist = false;
        public byte ucModul_Offset;

        //Slave_Family
        public bool bSlave_Family_Exist = false;
        public string strSlave_Family;                                      //0: General (can’t be assigned to the categories below)
                                                                            //1: Drives; 2: Switching devices; 3: I/O; 4: Valves; 5: Controllers; 6: HMI (MMI)
                                                                            //7: Encoders; 8: NC/RC; 9: Gateway; 10: Programmable Logic Controllers; 11: Ident systems
                                                                            //12: PROFIBUS PA Profile (independent of used Physical Layer); 13  255: reserved

        //Diag_Update_Delay
        public bool bDiag_Update_Delay_Exist = false;
        public byte ucDiag_Update_Delay;

        //Fail_Safe_required
        public bool bFail_Safe_required_Exist = false;
        public bool bFail_Safe_required;                                    //True: The device or a module requires the Fail_Safe mode for secure operation and is not optional.
                                                                            //False: The use of the Fail_Safe mode is optional.

        //Info_Text
        public bool bInfo_Text_Exist = false;
        public string strInfo_Text;                                         //Visible-String (256)
        public byte[] aucInfo_Text = new byte[256];

        //Max_User_Prm_Data_Len
        public bool bMax_User_Prm_Data_Len_Exist = false;
        public byte ucMax_User_Prm_Data_Len;                                //0  237

        //Ext_User_Prm_Data_Ref
        public List<CGSD_DATA_REF_INFO> Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();

        //Ext_User_Prm_Data_Const
        //public byte ucExtModulePrmDataLen = 0;              //User_Prm_Data数据长度
        public List<CGSD_DATA_CONST_INFO> Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();

        //ExtUserPrmData
        public List<CEXT_USER_PRM_DATA_INFO> ExtUserPrmData_List = new List<CEXT_USER_PRM_DATA_INFO>();

        //PrmText
        public List<CPRM_TEXT_INFO> PrmText_List = new List<CPRM_TEXT_INFO>();

        //Prm_Block_Structure_supp
        public bool bPrm_Block_Structure_supp_Exist = false;
        public bool bPrm_Block_Structure_supp;

        //Prm_Block_Structure_req
        public bool bPrm_Block_Structure_req_Exist = false;
        public bool bPrm_Block_Structure_req;


        //Jokerblock_Slot
        public bool bJokerblock_Slot_Exist = false;
        public byte ucJokerblock_Slot;

        //Jokerblock_Location
        public bool bJokerblock_Location_Exist = false;
        public byte ucJokerblock_Location;                                  //0: Prm-Telegram; 1: Prm-Telegram or Ext-Prm-Telegram; Only allowed, if X_Prm_SAP_supp = 1
                                                                            //2: Ext-Prm-Telegram; Only allowed, if X_Prm_SAP_supp = 1; 3 .. 255: Reserved                             
                                                                            //PrmCmd_supp
        public bool bPrmCmd_supp_Exist = false;
        public bool bPrmCmd_supp;


        //Slave_Max_Switch_Over_Time
        public bool bSlave_Max_Switch_Over_Time_Exist = false;
        public ushort uiSlave_Max_Switch_Over_Time;                         //Time base: 10 ms

        //Slave_Redundancy_supp
        public bool bSlave_Redundancy_supp_Exist = false;
        public byte ucSlave_Redundancy_supp;                                //0: not supported; 1: Slave is not redundant but can be connected to a flying master.
                                                                            //2 .. 7: Reserved; 8: Slave supports redundancy according [1].
                                                                            //9: Slave supports redundancy according [1] or can be connected to a flying
                                                                            //master. If connected to a flying master, the slave is used not redundant.

        //10 .. 255: Reserved

        //Ident_Maintenance_supp
        public bool bIdent_Maintenance_supp_Exist = false;
        public bool bIdent_Maintenance_supp;

        //Time_Sync_supp
        public bool bTime_Sync_supp_Exist = false;
        public bool bTime_Sync_supp;

        //Max_iParameter_Size
        public bool bMax_iParameter_Size_Exist = false;
        public ulong ulMax_iParameter_Size;

        //
        //4.4 Slave-related specifications
        //4.4.2 Additional keywords for module assignment
        //
        //SlotDefinition


        //
        //4.4 Slave-related specifications
        //4.4.3 Slave related keywords for DP extensions
        //PROFIBUS extensions mean the features of DP-V1 (see IEC 61784-1:2003 A3.1) and
        //list of options (see IEC 61784-1:2003 A3.1 and 7.2.3.2.5), compared to DP-V0.
        //
        //DPV1_Slave
        public bool bDPV1_Slave_Exist = false;
        public bool bDPV1_Slave;

        //C1_Read_Write_supp
        public bool bC1_Read_Write_supp_Exist = false;
        public bool bC1_Read_Write_supp;

        //C2_Read_Write_supp
        public bool bC2_Read_Write_supp_Exist = false;
        public bool bC2_Read_Write_supp;

        //C1_Max_Data_Len
        public bool bC1_Max_Data_Len_Exist = false;
        public byte ucC1_Max_Data_Len;                              //0 .. 240

        //C2_Max_Data_Len
        public bool bC2_Max_Data_Len_Exist = false;
        public byte ucC2_Max_Data_Len;                              //0,48 .. 240

        //C1_Response_Timeout
        public bool bC1_Response_Timeout_Exist = false;
        public ushort uiC1_Response_Timeout;                        //Time base: 10 ms

        //C2_Response_Timeout
        public bool bC2_Response_Timeout_Exist = false;
        public ushort uiC2_Response_Timeout;                        //Time base: 10 ms

        //C1_Read_Write_required
        public bool bC1_Read_Write_required_Exist = false;
        public bool bC1_Read_Write_required;

        //C2_Max_Count_Channels
        public bool bC2_Max_Count_Channels_Exist = false;
        public byte ucC2_Max_Count_Channels;                        //0 .. 49

        //Max_Initiate_PDU_Length
        public bool bMax_Initiate_PDU_Length_Exist = false;
        public byte ucMax_Initiate_PDU_Length;                      //0, 52 .. 244

        //Diagnostic_Alarm_supp
        public bool bDiagnostic_Alarm_supp_Exist = false;
        public bool bDiagnostic_Alarm_supp;

        //Process_Alarm_supp
        public bool bProcess_Alarm_supp_Exist = false;
        public bool bProcess_Alarm_supp;

        //Pull_Plug_Alarm_supp
        public bool bPull_Plug_Alarm_supp_Exist = false;
        public bool bPull_Plug_Alarm_supp;

        //Status_Alarm_supp
        public bool bStatus_Alarm_supp_Exist = false;
        public bool bStatus_Alarm_supp;

        //Update_Alarm_supp
        public bool bUpdate_Alarm_supp_Exist = false;
        public bool bUpdate_Alarm_supp;

        //Manufacturer_Specific_Alarm_supp
        public bool bManufacturer_Specific_Alarm_supp_Exist = false;
        public bool bManufacturer_Specific_Alarm_supp;

        //Extra_Alarm_SAP_supp
        public bool bExtra_Alarm_SAP_supp_Exist = false;
        public bool bExtra_Alarm_SAP_supp;

        //Alarm_Sequence_Mode_Count
        public bool bAlarm_Sequence_Mode_Count_Exist = false;
        public byte ucAlarm_Sequence_Mode_Count;                        //0, 2 .. 32

        //Alarm_Type_Mode_supp
        public bool bAlarm_Type_Mode_supp_Exist = false;
        public bool bAlarm_Type_Mode_supp;                              //shall always be set to 1: True

        //Diagnostic_Alarm_required
        public bool bDiagnostic_Alarm_required_Exist = false;
        public bool bDiagnostic_Alarm_required;

        //Process_Alarm_required
        public bool bProcess_Alarm_required_Exist = false;
        public bool bProcess_Alarm_required;

        //Pull_Plug_Alarm_required (D starting with GSD_Revision 3)
        public bool bPull_Plug_Alarm_required_Exist = false;
        public bool bPull_Plug_Alarm_required;

        //Status_Alarm_required (D starting with GSD_Revision 3)
        public bool bStatus_Alarm_required_Exist = false;
        public bool bStatus_Alarm_required;

        //Update_Alarm_required (D starting with GSD_Revision 3)
        public bool bUpdate_Alarm_required_Exist = false;
        public bool bUpdate_Alarm_required;

        //Manufacturer_Specific_Alarm_required (D starting with GSD_Revision 3)
        public bool bManufacturer_Specific_Alarm_required_Exist = false;
        public bool bManufacturer_Specific_Alarm_required;

        //DPV1_Data_Types (D starting with GSD_Revision 3)
        public bool bDPV1_Data_Types_Exist = false;
        public bool bDPV1_Data_Types;

        //WD_Base_1ms_supp (D starting with GSD_Revision 3)
        public bool bWD_Base_1ms_supp_Exist = false;
        public bool bWD_Base_1ms_supp;

        //Check_Cfg_Mode (D starting with GSD_Revision 3)
        public bool bCheck_Cfg_Mode_Exist = false;
        public bool bCheck_Cfg_Mode;

        //
        //4.4 Slave-related specifications
        //4.4.4 Slave related keywords for Data Exchange with Broadcast
        //
        //Publisher_supp
        public bool bPublisher_supp_Exist = false;
        public bool bPublisher_supp;

        //Subscriber_supp
        public bool bSubscriber_supp_Exist = false;
        public bool bSubscriber_supp;

        //DXB_Max_Link_Count
        public bool bDXB_Max_Link_Count_Exist = false;
        public byte ucDXB_Max_Link_Count;                                           //0  125

        //DXB_Max_Data_Length
        public bool bDXB_Max_Data_Length_Exist = false;
        public byte ucDXB_Max_Data_Length;                                          //0  244

        //DXB_Subscribertable_Block_Location
        public bool bDXB_Subscribertable_Block_Location_Exist = false;
        public byte ucDXB_Subscribertable_Block_Location;

        //
        //4.4 Slave-related specifications
        //4.4.5 Slave related keywords for Isochronous Mode
        //
        //Isochron_Mode_supp
        public bool bIsochron_Mode_supp_Exist = false;
        public bool bIsochron_Mode_supp;

        //Isochron_Mode_required
        public bool bIsochron_Mode_required_Exist = false;
        public bool bIsochron_Mode_required;

        //TBASE_DP
        public bool bTBASE_DP_Exist = false;
        public ulong ulTBASE_DP;                                                 //allowed values are 375, 750, 1500, 3000, 6000, 12000 which
                                                                                 //correspond to 31.25, 62.5, 125, 250, 500, 1000 μs respectively.       

        //TDP_MIN
        public bool bTDP_MIN_Exist = false;
        public ushort uiTDP_MIN;                                                //1  2的16次方-1

        //TDP_MAX
        public bool bTDP_MAXN_Exist = false;
        public ushort uiTDP_MAX;                                                //1  2的16次方-1

        //T_PLL_W_MAX
        public bool bT_PLL_W_MAX_Exist = false;
        public ushort uiT_PLL_W_MAX;                                            //12  2的16次方-1


        //TI_MIN
        public bool bTI_MIN_Exist = false;
        public ushort uiTI_MIN;                                                //1  2的16次方-1

        //TO_MIN
        public bool bTO_MIN_Exist = false;
        public ushort uiTO_MIN;

        //
        //4.4 Slave-related specifications
        //4.4.6 Slave related keywords for PROFIsafe Profile
        //
        //F_ParamDescCRC
        public bool bF_ParamDescCRC_Exist = false;
        public ushort uiF_ParamDescCRC;


        //F_IO_StructureDescCRC
        public bool bF_IO_StructureDescCRC_Exist = false;
        public ulong ulF_IO_StructureDescCRC;

        //F_Ext_User_Prm_Data_Ref
        public List<CGSD_DATA_REF_INFO> F_Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();

        //F_Ext_User_Prm_Data_Const
        public byte ucF_ExtModulePrmDataLen = 0;            //F_User_Prm_Data数据长度(1~237 )
        public List<CGSD_DATA_CONST_INFO> F_Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();

        //
        //4.4 Slave-related specifications
        //4.4.7 Slave related keywords for extended parameterization
        //
        //X_Prm_SAP_supp
        public bool bX_Prm_SAP_supp_Exist = false;
        public bool bX_Prm_SAP_supp;

        //X_Max_User_Prm_Data_Len
        public bool bX_Max_User_Prm_Data_Len_Exist = false;
        public byte ucX_Max_User_Prm_Data_Len;                                  //5  244

        //X_Ext_Module_Prm_Data_Len
        public bool bX_Ext_Module_Prm_Data_Len_Exist = false;
        public byte ucX_Ext_Module_Prm_Data_Len;                                //1  244

        //X_Ext_User_Prm_Data_Ref
        //X_Ext_User_Prm_Data_Const

        //X_Prm_Block_Structure_supp
        public bool bX_Prm_Block_Structure_supp_Exist = false;
        public bool bX_Prm_Block_Structure_supp;

        //
        //4.4 Slave-related specifications
        //4.4.8 Slave related keywords for subsystems
        //
        //Subsys_Dir_Index
        public bool bSubsys_Dir_Index_Exist = false;
        public byte ucSubsys_Dir_Index;                                         //1: Gateway capability according to [3] 
                                                                                //0, 2 .. 127: Reserved; 128 .. 255: User specific
                                                                                //Subsys_Module_Dir_Index
        public bool bSubsys_Module_Dir_Index_Exist = false;
        public byte ucSubsys_Module_Dir_Index;                                  //1: Gateway capability according to [3]
                                                                                //0, 2 .. 127: Reserved; 128 .. 255: User specific
                                                                                //
                                                                                //the others
                                                                                //



    };
    //
    //模块定义
    //
    public class CMODULE_INFO
    {
        public string strMod_Name;
        public string strConfig;

        public byte[] aucCfgData = new byte[128];   //最大长度暂时设为64
        public byte ucCfgLen;

        public bool bModule_Ref_Exist = false;
        public ushort uiModule_Ref = 0;

        public byte ucExt_Module_Prm_Data_Len = 0;              //the length of the associated User_Prm_Data is defined
        public byte ucX_Ext_Module_Prm_Data_Len = 0;            //1~244        
        public byte ucF_Ext_Module_Prm_Data_Len = 0;            //1  237

        //Data_Area
        public List<CDATA_AREA_INFO> asData_AreaList = new List<CDATA_AREA_INFO>();

        public bool bPreset_Exist = false;
        public byte ucPreset;

        public bool bF_IO_StructureDescCRC_Exist = false;
        public ulong ulF_IO_StructureDescCRC;

        public List<CGSD_DATA_CONST_INFO> Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();
        public List<CGSD_DATA_REF_INFO> Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();

        public List<CGSD_DATA_CONST_INFO> F_Ext_User_Prm_Data_Const_List = new List<CGSD_DATA_CONST_INFO>();
        public List<CGSD_DATA_REF_INFO> F_Ext_User_Prm_Data_Ref_List = new List<CGSD_DATA_REF_INFO>();

        public List<CCHANNEL_DIAG> Channel_Diag_List = new List<CCHANNEL_DIAG>();


        public bool bF_ParamDescCRC_Exist = false;
        public ushort uiF_ParamDescCRC;

    };
    //
    //定义CONST类型变量的类型
    //例如：Ext_User_Prm_Data_Const()
    //一般为HEX格式
    //
    public class CGSD_DATA_CONST_INFO
    {
        public byte ucRefBeg = 0;
        public byte ucDataLenL = 0;
        public byte[] aucData = new byte[255];//最大长度暂时定为128
    }
    //
    //定义REF类型变量的类型
    //例如：Ext_User_Prm_Data_Ref()
    //一般为十进制格式
    //
    public class CGSD_DATA_REF_INFO
    {
        public byte ucRefBeg = 0;
        public ushort uiData;
    }
    //
    //DATA_AREA
    //
    public class CDATA_AREA_INFO
    {
        public string strArea_Name;
        public byte ucRelated_CFG_Identifier;

        public bool bIO_Direction = false;
        public byte ucLength;
        public byte ucConsistency;
        public bool bPublisher_allowed = false;
        public bool bDP_Master_allowed = false;
        public byte ucData_Type;
    }
    //
    //PrmText项
    //
    public class CPRM_TEXT_ITEM_INFO
    {
        public ushort uiID;
        public string strText;
    }
    //
    //PrmText
    //
    public class CPRM_TEXT_INFO
    {
        public ushort uiReference_Number;
        public List<CPRM_TEXT_ITEM_INFO> asPrm_Text_Item_List = new List<CPRM_TEXT_ITEM_INFO>();
    }
    //
    //ExtUserPrmData
    //
    public class CEXT_USER_PRM_DATA_INFO
    {
        public ushort uiReference_Number;
        public string strExt_User_Prm_Data_Name;

        public string strData_Type_Name = "";                    //如Unsigned16，BitArea...

        public CDATA_TYPE_INFO_BIT cDataInfo_BIT = new CDATA_TYPE_INFO_BIT();
        public CDATA_TYPE_INFO_BITAREA cDataInfo_BITAREA = new CDATA_TYPE_INFO_BITAREA();
        public CDATA_TYPE_INFO_USIGN8 cDataInfo_USIGN8 = new CDATA_TYPE_INFO_USIGN8();

        public string strAllowed_Values = "";            //Data_Type_Array (16)

        public bool bPrm_Text_Ref_Exist = false;
        public ushort uiPrm_Text_Ref;

        public bool bChangeable_Exist = false;
        public bool bChangeable = false;

        public bool bVisible_Exist = false;
        public bool bVisible = false;
    }
    //
    //Data_Type相关信息
    //<Data_Type> = <Unsigned8>| <Unsigned16> | <Unsigned32>| <Signed8> | <Signed16>| <Signed32> | <Bit> 
    public class CDATA_TYPE_INFO_USIGN8
    {
        public byte ucDefault_Value;
        public byte ucMin_Value;
        public byte ucMax_Value;
        public byte[] aucAllowed_Values = new byte[16];

    }
    //
    //Data_Type相关信息
    //
    public class CDATA_TYPE_INFO_BITAREA
    {
        public byte ucFirstBit;
        public byte ucLastBit;

        public byte ucDefault_Value;

        public byte ucMin_Value;
        public byte ucMax_Value;
        public byte[] aucAllowed_Values = new byte[16];
    }
    //
    //Data_Type相关信息
    //
    public class CDATA_TYPE_INFO_BIT
    {
        public byte ucBitRef;

        public byte ucDefault_Value;

        public byte ucMin_Value;
        public byte ucMax_Value;
        public byte[] aucAllowed_Values = new byte[16];
    }
    //
    //Unit_Diag_Bit
    //
    public class CUNIT_DIAG_BIT
    {
        public ushort uiBit;
        public string strDiag_Text;
    }
    //
    //Channel_Diag:
    //
    public class CCHANNEL_DIAG
    {
        public byte ucError_Type;       //16 <= Error_Type <= 31
        public string strDiag_Text;
    }

}
