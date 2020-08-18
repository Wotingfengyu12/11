using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldIot.HARTDD;
using FieldIot.ProfibusDP;
using System.IO;

namespace CQC.ConTest
{    
    delegate void testmodule(parameters paras, out results res);
    
    class TestFunc
    {
        public testmodule func;
        public string name;
        parameters para;
        results res;

        public TestFunc(testmodule dg)
        {
            func = new testmodule(dg);
        }

        public results getResults(parameters pa)
        {
            para = pa;
            func(para, out res);
            return res;
        }
    }

    class HartTestFuncs : List<TestFunc>
    {
        HARTDevice hartDev;

        public TestFunc this[string name]
        {
            get
            {
                foreach(TestFunc tc in this)
                {
                    if (tc.name == name)
                    {
                        return tc;
                    }
                }
                return null;
            }
        }

        public HartTestFuncs()
        {
            hartDev = new HARTDevice(null);
            initFuncs();
            logSw = null;
        }

        StreamWriter logSw;

        public void setLogsw(StreamWriter sw)
        {
            logSw = sw;
        }

        void saveLogfile(string format, params object[] args)
        {
            if (logSw != null)
            {
                logSw.WriteLine(String.Format(format, args));
            }
        }

        public HartTestFuncs(StreamWriter sw)
        {
            hartDev = new HARTDevice(null);
            initFuncs();
            logSw = sw;
        }



        public HARTDevice HartDev
        {
            get
            {
                return hartDev;
            }
            set
            {
                hartDev = value;
            }
        }

        public HartTestFuncs(HARTDevice hdev)
        {
            hartDev = hdev;
            initFuncs();
        }

        void initFuncs()
        {
            TestFunc tf = new TestFunc(GetDDInfo);
            tf.name = "GetDDInfo";
            Add(tf);

            tf = new TestFunc(ReadVariable);
            tf.name = "ReadVariable";
            Add(tf);

            tf = new TestFunc(WriteVariable);
            tf.name = "WriteVariable";
            Add(tf);

            tf = new TestFunc(SendCommand);
            tf.name = "SendCommand";
            Add(tf);

            tf = new TestFunc(GetDeviceInfo);
            tf.name = "GetDeviceInfo";
            Add(tf);

        }

        void GetDDInfo(parameters paras, out results rets)
        {
            saveLogfile("Getting Varible list from DD file");
            rets = new results();
            result rst = new result();
            rst.name = "NumOfVars";
            rst.value = hartDev.Vars.Count;
            rets.Add(rst);
            saveLogfile("Getting command list from DD file");
            rst = new result();
            rst.name = "NumOfCmds";
            rst.value = hartDev.Cmds.Count;
            rets.Add(rst);
            saveLogfile("Getting collection list from DD file");
            rst = new result();
            rst.name = "NumOfColletions";
            rst.value = hartDev.Collections.Count;
            rets.Add(rst);
            rets.response = rspCode.positive;
        }

        void ReadVariable(parameters paras, out results rets)
        {
            rets = new results();
            result rst = new result();
            ThreadUpdate tusend = new ThreadUpdate();
            if (paras.Count != 1)
            {
                rets.response = rspCode.negitive;
                rets.resDesc = "More than 1 variable to read.";
                saveLogfile("More than 1 variable to read.");
            }
            else
            {
                CDDLBase ddb = new CDDLBase();
                if (hartDev.getItembyName((string)paras[0].value, ref ddb))
                {
                    if (ddb.GetType() == typeof(CDDLVar))
                    {
                        CDDLVar vartoread = (CDDLVar)ddb;
                        hCcommandDescriptor pCmdDesc = vartoread.getRdCmdList().ElementAt(0);
                        CDDLCmd pCmd = hartDev.Cmds.getCmdByNumber(pCmdDesc.cmdNumber);
                        if (hartDev.pCmdDispatch.SendCmd(pCmd, pCmdDesc.transNumb, null, logSw) == Common.SUCCESS)
                        {
                            saveLogfile("Command {0}, transaction {1} sent.", pCmdDesc.cmdNumber, pCmdDesc.transNumb);
                            returncode creply = hartDev.parentform.ReData(null);
                            hartDev.parentform.setThread(tusend);
                            tusend.ucTranNumSent = (byte)pCmdDesc.transNumb;
                            tusend.ucCmdSent = (byte)pCmdDesc.cmdNumber;
                            hartDev.parentform.procRcvData(creply, pCmdDesc.transNumb, pCmd.getCmdNumber(), pCmd.getOperation());
                            rets.response = (hartDev.parentform.getCmdRes() as results).response;//getCmdRes
                            rst.name = (string)paras[0].value;
                            //rst.rtype = resultDataType.floatpoint;
                            rst.value = vartoread.GetDispString();
                            rets.Add(rst);
                        }
                        else
                        {
                            rets.response = rspCode.negitive;
                            rets.resDesc = String.Format("The Command {0}, transaction {1} cannot be sent.", pCmdDesc.cmdNumber, pCmdDesc.transNumb);
                            saveLogfile(rets.resDesc);
                        }
                    }
                    else
                    {
                        rets.response = rspCode.negitive;
                        rets.resDesc = "The item is not a variable.";
                        saveLogfile(rets.resDesc);
                    }
                }
                else
                {
                    rets.response = rspCode.negitive;
                    rets.resDesc = "The variable name is not valid.";
                    saveLogfile("The variable name is not valid.");
                }
            }
        }

        void WriteVariable(parameters paras, out results rets)
        {
            rets = new results();
            result rst = new result();
            ThreadUpdate tusend = new ThreadUpdate();
            if (paras.Count != 1)
            {
                rets.response = rspCode.negitive;
                rets.resDesc = "More than 1 variable to read.";
                saveLogfile(rets.resDesc);
            }
            else
            {
                CDDLBase ddb = new CDDLBase();
                if (hartDev.getItembyName((string)paras[0].value, ref ddb))
                {
                    if (ddb.GetType() == typeof(CDDLVar))
                    {
                        CDDLVar vartowrite = (CDDLVar)ddb;
                        hCcommandDescriptor pCmdDesc = vartowrite.getWrCmdList().ElementAt(0);
                        CDDLCmd pCmd = hartDev.Cmds.getCmdByNumber(pCmdDesc.cmdNumber);
                        if (hartDev.pCmdDispatch.SendCmd(pCmd, pCmdDesc.transNumb, null, logSw) == Common.SUCCESS)
                        {
                            saveLogfile("Command {0}, transaction {1} sent.", pCmdDesc.cmdNumber, pCmdDesc.transNumb);
                            returncode creply = hartDev.parentform.ReData(null);
                            hartDev.parentform.setThread(tusend);
                            tusend.ucTranNumSent = (byte)pCmdDesc.transNumb;
                            tusend.ucCmdSent = (byte)pCmdDesc.cmdNumber;
                            hartDev.parentform.procRcvData(creply, pCmdDesc.transNumb, pCmd.getCmdNumber(), pCmd.getOperation());
                            rets.response = (hartDev.parentform.getCmdRes() as results).response;//getCmdRes

                            //rst.name = (string)paras[0].value;
                            //rst.rtype = resultDataType.floatpoint;
                            //rst.value = vartowrite.GetDispString();

                            rets.Add(rst);
                        }
                        else
                        {
                            rets.response = rspCode.negitive;
                            rets.resDesc = String.Format("The Command {0}, transaction {1} cannot be sent.", pCmdDesc.cmdNumber, pCmdDesc.transNumb);
                            saveLogfile(rets.resDesc);
                        }
                    }
                    else
                    {
                        rets.response = rspCode.negitive;
                        rets.resDesc = "The item is not a variable.";
                        saveLogfile(rets.resDesc);
                    }
                }
                else
                {
                    rets.response = rspCode.negitive;
                    rets.resDesc = "The variable name is not valid.";
                    saveLogfile(rets.resDesc);
                }
            }
        }

        void SendCommand(parameters paras, out results rets)
        {
            rets = new results();
            result rst = new result();
            ThreadUpdate tusend = new ThreadUpdate();
            if (paras.Count != 1)
            {
                rets.response = rspCode.negitive;
                rets.resDesc = "More than 1 command to send.";
                saveLogfile(rets.resDesc);
            }
            else
            {
                CDDLBase ddb = new CDDLBase();
                if (hartDev.getItembyName((string)paras[0].value, ref ddb))
                {
                    if (ddb.GetType() == typeof(CDDLCmd))
                    {
                        CDDLCmd pCmd = (CDDLCmd)ddb;
                        if (hartDev.pCmdDispatch.SendCmd(pCmd, 0, null, logSw) == Common.SUCCESS)
                        {
                            saveLogfile("Command {0}, transaction {1} sent.", pCmd.getCmdNumber(), 0);
                            returncode creply = hartDev.parentform.ReData(null);
                            hartDev.parentform.setThread(tusend);
                            tusend.ucTranNumSent = (byte)0;
                            tusend.ucCmdSent = (byte)pCmd.getCmdNumber();
                            hartDev.parentform.procRcvData(creply, 0, pCmd.getCmdNumber(), pCmd.getOperation());
                            rets.response = (hartDev.parentform.getCmdRes() as results).response;//getCmdRes

                            //rst.name = (string)paras[0].value;
                            //rst.rtype = resultDataType.floatpoint;
                            //rst.value = vartowrite.GetDispString();

                            rets.Add(rst);
                        }
                        else
                        {
                            rets.response = rspCode.negitive;
                            rets.resDesc = String.Format("The Command {0}, transaction {1} cannot be sent.", pCmd.getCmdNumber(), 0);
                            saveLogfile(rets.resDesc);
                        }
                    }
                    else
                    {
                        rets.response = rspCode.negitive;
                        rets.resDesc = "The item is not a command.";
                        saveLogfile(rets.resDesc);
                    }

                }
                else
                {
                    rets.response = rspCode.negitive;
                    rets.resDesc = "The variable name is not valid.";
                    saveLogfile(rets.resDesc);
                }
            }
        }

        void GetDeviceInfo(parameters paras, out results rets)
        {
            rets = new results();
            result rst = new result();
            if (paras.Count != 0)
            {
                rets.response = rspCode.negitive;
                rets.resDesc = "No paramater is allowed for this function.";
                saveLogfile(rets.resDesc);
            }
            else
            {
                DeviceDetails dev = new DeviceDetails();
                List<string> devInfo = dev.getDevDetails(hartDev);

                if (devInfo.Count != 4)
                {
                    rets.response = rspCode.negitive;
                    rets.resDesc = "No paramater is allowed for this function.";
                    saveLogfile(rets.resDesc);
                }
                else
                {
                    saveLogfile("Getting device info.");
                    rst = new result();
                    rst.name = "manuID";
                    rst.value = devInfo[0];
                    rets.Add(rst);
                    rst = new result();
                    rst.name = "devType";
                    rst.value = devInfo[1];
                    rets.Add(rst);
                    rst = new result();
                    rst.name = "devRev";
                    rst.value = devInfo[2];
                    rets.Add(rst);
                    rst = new result();
                    rst.name = "ddRev";
                    rst.value = devInfo[3];
                    rets.Add(rst);
                    rets.response = rspCode.positive;
                }
            }

        }

    }

    class DPTestFuncs : List<TestFunc>
    {
        DPComm dpDev;
        StreamWriter logSw;

        public void setLogsw(StreamWriter sw)
        {
            logSw = sw;
        }

        void saveLogfile(string format, params object[] args)
        {
            if (logSw != null)
            {
                logSw.WriteLine(String.Format(format, args));
            }
        }

        public TestFunc this[string name]
        {
            get
            {
                foreach (TestFunc tc in this)
                {
                    if (tc.name == name)
                    {
                        return tc;
                    }
                }
                return null;
            }
        }

        public DPTestFuncs(DPComm dev)
        {
            dpDev = dev;
            initFuncs();
        }

        public DPComm DPDev
        {
            get
            {
                return dpDev;
            }
            set
            {
                dpDev = value;
            }
        }

        /*
        public DPTestFuncs(DPComm dev)
        {
            dpDev = dev;
            initFuncs();
        }
        */

        void initFuncs()
        {
            TestFunc tf = new TestFunc(GetGSDInfo);
            tf.name = "GetGSDInfo";
            Add(tf);

            tf = new TestFunc(DiagnoseDev);
            tf.name = "DiagnoseDev";
            Add(tf);

            tf = new TestFunc(SetPRM);
            tf.name = "SetPRM";
            Add(tf);

            tf = new TestFunc(ChkCFG);
            tf.name = "ChkCFG";
            Add(tf);

            tf = new TestFunc(DataExchange);
            tf.name = "DataExchange";
            Add(tf);

        }

        void GetGSDInfo(parameters paras, out results rets)
        {
            DPDev.parseGSD(0, logSw);
            //将信息写入到文本框中
            rets = new results();
            result rst = new result();
            rst.name = "Vendor_Name";
            rst.value = DPDev.pGSD.cGsdFileInfoEntity.strVendor_Name;
            rets.Add(rst);
            rst = new result();
            rst.name = "Model_Name";
            rst.value = DPDev.pGSD.cGsdFileInfoEntity.strModel_Name;
            rets.Add(rst);
            rst = new result();
            rst.name = "Ident_Number";
            rst.value = (int)DPDev.pGSD.cGsdFileInfoEntity.uiIdent_Number;
            rets.Add(rst);
            rets.response = rspCode.positive;
        }

        void DiagnoseDev(parameters paras, out results rets)
        {
            rets = new results();
            result rst = new result();
            ThreadUpdate tusend = new ThreadUpdate();
            if (paras.Count != 1)
            {
                rets.response = rspCode.negitive;
                rets.resDesc = "More than 1 variable to read.";
            }
            else
            {
                CFRAME_PARSE_NODE cFrameParse = new CFRAME_PARSE_NODE();

                if (DPDev.diagDevice(0, ref cFrameParse))
                {
                    //if (hartDev.pCmdDispatch.SendCmd(pCmd, pCmdDesc.transNumb) == Common.SUCCESS)
                    {
                        /*
                        rets.response = (hartDev.parentform.getCmdRes() as results).response;//getCmdRes
                        rst.name = (string)paras[0].value;
                        //rst.rtype = resultDataType.floatpoint;
                        rst.value = vartoread.GetDispString();
                        rets.Add(rst);
                        */
                        string slaveState = DPDev.parseSlaveStateByDiag(cFrameParse.aucData, cFrameParse.ucDataLen);
                        rst = new result();
                        rst.name = "SlaveState";
                        rst.value = slaveState;
                        rets.Add(rst);

                        string DiagnoseInfo = "";
                        DPDev.parseDiagnosticsInfo(cFrameParse.aucData, cFrameParse.ucDataLen, ref DiagnoseInfo);
                        rst = new result();
                        rst.name = "DiagnoseInfo";
                        rst.value = DiagnoseInfo;
                        rets.Add(rst);

                    }
                    //else
                    {
                        rets.response = rspCode.positive;
                        //rets.resDesc = String.Format("The Command {0}, transaction {1} cannot be sent.", pCmdDesc.cmdNumber, pCmdDesc.transNumb);
                    }
                }
                else
                {
                    rets.response = rspCode.negitive;
                    rets.resDesc = "The Diagnose is failed.";
                }
            }
        }

        void SetPRM(parameters paras, out results rets)
        {
            rets = new results();
            //result rst = new result();
            //rst.name = "NumOfVars";
            //rst.value = hartDev.Vars.Count;
            //rets.Add(rst);

            if (DPDev.asLocalDevCfgList[0].strWK_State == "WAIT_PRM")
            {
                DPDev.SetPRM();
                //生成SET_PRM报文中数据
                CFRAME_PARSE_NODE cFrameParse = new CFRAME_PARSE_NODE();

                if (DPDev.diagDevice(0, ref cFrameParse))
                {
                    string slaveState = DPDev.parseSlaveStateByDiag(cFrameParse.aucData, cFrameParse.ucDataLen);
                    if(slaveState == "WAIT_CFG")
                    {
                        rets.response = rspCode.positive;
                    }

                }
                else
                {
                    rets.response = rspCode.negitive;
                }
            }
            else
            {
                rets.response = rspCode.negitive;
            }

        }

        void ChkCFG(parameters paras, out results rets)
        {
            rets = new results();
            //result rst = new result();
            //rst.name = "NumOfVars";
            //rst.value = hartDev.Vars.Count;
            //rets.Add(rst);

            if (DPDev.asLocalDevCfgList[0].strWK_State == "WAIT_CFG")
            {
                DPDev.ChkCFG();
                //生成SET_PRM报文中数据
                CFRAME_PARSE_NODE cFrameParse = new CFRAME_PARSE_NODE();

                if (DPDev.diagDevice(0, ref cFrameParse))
                {
                    string slaveState = DPDev.parseSlaveStateByDiag(cFrameParse.aucData, cFrameParse.ucDataLen);
                    if (slaveState == "DATA_EXCHANGE")
                    {
                        rets.response = rspCode.positive;
                    }

                }
                else
                {
                    rets.response = rspCode.negitive;
                }
            }
            else
            {
                rets.response = rspCode.negitive;
            }

        }

        void DataExchange(parameters paras, out results rets)
        {
            rets = new results();
            //result rst = new result();
            //rst.name = "NumOfVars";
            //rst.value = hartDev.Vars.Count;
            //rets.Add(rst);
            if (paras.Count != 2)
            {
                rets.response = rspCode.negitive;
                rets.resDesc = "More than 1 variable to read.";
                saveLogfile("More than 1 variable to read.");
            }
            else
            {
                if (DPDev.asLocalDevCfgList[0].strWK_State == "DATA_EXCHANGE")
                {
                    byte[] output = new byte[255];
                    string outputstr = (string)paras[0].value;
                    byte length = (byte)paras[1].value;
                    if (length * 2 == outputstr.Length)
                    {
                        for (int i = 0; i < outputstr.Length / 2; i++)
                        {
                            output[i] = Convert.ToByte(outputstr.Substring(i, 2));
                        }
                        DPDev.dataExchange(output, length);

                        CFRAME_PARSE_NODE cFrameParse = new CFRAME_PARSE_NODE();

                        if (DPDev.diagDevice(0, ref cFrameParse))
                        {
                            string slaveState = DPDev.parseSlaveStateByDiag(cFrameParse.aucData, cFrameParse.ucDataLen);
                            if (slaveState == "DATA_EXCHANGE")
                            {
                                rets.response = rspCode.positive;
                            }

                        }
                        else
                        {
                            rets.response = rspCode.negitive;
                        }
                    }
                    else
                    {
                        rets.response = rspCode.negitive;
                    }
                }
                else
                {
                    rets.response = rspCode.negitive;
                }
            }
        }


    }

}
