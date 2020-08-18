using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FieldIot.HARTDD
{
    public class HARTRSPINFO
    {
        const byte DATA_MAX_NUM = 64;

        public byte ucState;
        public byte ucDelimiter;
        public byte ucByteCount;
        public byte ucPreamNum;
        public byte ucAddrType;
        public byte ucHostAddr;
        public byte ucPollAddr;
        public byte ucShortAddr;
        public byte[] aucLongAddr = new byte[5];
        public byte[] aucRspCode = new byte[2];
        public byte ucCmd;
        //public byte ucErr;
        public byte ucSendState;
        public byte ucBitErr;//奇校验错，0－checkbyte 1-data 2-bytecount 3-command 4-address 5-delimiter
        public byte ucCheckSum;
        public byte[] aucBuf = new byte[DATA_MAX_NUM];

        public void clear()
        {

        }
    }

    public class ThreadUpdate
    {
        public delegate void InvokeSendThead(byte[] data, byte len, byte cmdNum, byte transNum);//委托
        public delegate void InvokeRcvThead(returncode rc, int trannum, uint cmdnum, cmdOperationType_t operation);
        public delegate void InvokeVarThead();

        public delegate void InvokeLog(string loginfo, LogType type);

        public InvokeSendThead MainThread = null;//事件
        public InvokeRcvThead RcvThread = null;//事件
        public InvokeVarThead VarThread = null;//事件
        public InvokeLog LogThread = null;
        public Thread ReadThread = null;
        public Thread WriteThread = null;

        public IAsyncResult mainThreadRes;
        public IAsyncResult rcvThreadRes;
        public IAsyncResult varThreadRes;

        public byte ucCmdSent = 0xff;
        public byte ucTranNumSent = 0xff;

        public ThreadUpdate()
        {
            //MainThread = new InvokeOtherThead(byte[] data, byte len);
        }
        
        /*
        public void WaitMoreTime()
        {
            runtask();
            MainThread(i);//调用事件
        }
        */
    }

}
