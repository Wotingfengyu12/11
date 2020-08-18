using FieldIot.HARTDD;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace CQC.ConTest
{
    class mainform : mainpage
    {
        frmMain form;

        public mainform(frmMain f)
        {
            form = f;
            cmdRes = form.cmdRes;
        }

        public override object getCmdRes()
        {
            return form.cmdRes;
        }

        public override void Update(int type, uint SymID)
        {
            //form.Update((updatetype)type, SymID);
        }

        public override returncode USART_Send(byte[] data, byte len, byte add = 0, StreamWriter sw = null)
        {
            return form.USART_Send(data, len, add, sw);
            //return returncode.eOk;
        }

        public override void procRcvData(returncode rc, int trannum, uint cmdnum, cmdOperationType_t operation)
        {
            form.procRcvHartData(rc, trannum, cmdnum, operation);
        }

        public override void UpdataFormReq(byte[] data, byte len, byte cmdNum, byte transNum)
        {
            //form.UpdataFormReq(data, len, cmdNum, transNum);
        }

        public override returncode ReData(StreamWriter sw)
        {
            //return form.ReData();
            return form.RecvHartData(sw);
        }

        public override returncode RecvData(StreamWriter sw)
        {
            return form.RecvHartData(sw);
        }

        public override void dispMsgRcv(string msgRcv)
        {
            //form.dispMsgRcv(msgRcv);
        }

        public override returncode GetIdentity(byte pollAddr, StreamWriter sw)
        {
            //pollAddr,  chksum
            return form.GetIdentity(pollAddr, sw);
        }

        public override void UpdateData()
        {
            //form.UpdateData();
        }

        public override void Log(string info, int logtype)
        {
            form.Log(info, (LogType)logtype);
        }

        public override bool baftSel()
        {
            return form.bafterSel;
        }

        public override IAsyncResult BeginInvoke(Delegate method)
        {
            return form.BeginInvoke(method);
        }

        public override IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            return form.BeginInvoke(method, args);
        }


    }
}
