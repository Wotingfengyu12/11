using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    /*
    public enum transferFunc_t
    {
        tf_Transfer,    // 0
        tf_OpenPort,
        tf_ResetPort,
        tf_Ready2Close,
        tf_PortClosed
    }

    public class bufChunk_t
    {
        byte len;
        ushort masterStart;  // master's count of first byte
        ushort slaveStart;   // my count of first byte
        byte[] data = new byte[0xff];

    }

    public enum transferState_t
    {
        ts_Undefined // others to be determined

    }
    */

    public enum sessionState_t
    {
        ss_Undefined,   /* closed, has never been opened */
        ss_Opening,     /* transitory while command is In-Air */
        ss_Opened,      /* ready to transfer */
        ss_Closing,     /* transitory while command is In-Air */
        ss_Closed       /* no more transmission or stream filling will take place */
    }

    /*
    public class hCTransferChannel
    {
        public const int MAX_SEG_LEN = 230;
        public const int STARTING_MASTER_SYNC = 0;

        //public HARTDevice pDevice;

        public System.IO.Ports.SerialPort pPortSupport;

        public sessionState_t Session_State;

        // for getTransferStatus
        //public byte respCd;
        //public byte devStat;// unused at this time
        public byte funcCd;
        // for port function
        //public byte portNumber;
        public byte max_segment_len;  // as negotiated

        public ushort startMasterSync;  // where massa started
        public ushort masterSync;        // where the massa is
        public ushort maserSyncRolloverCnt;    // location extension (will be 1 too hi)

        public ushort startSlave_Sync;  // where massa started
        public ushort slave_Sync;        // where the slave is
        public ushort slaveSyncRolloverCnt;    // location extension (will be 1 too hi)

        public List<bufChunk_t> recv_Buffer; // vector of packet specifics
        //public uint activeRecvChunk;      // the index of the active buffChunk
        //public transferState_t recvState;

        //public List<bufChunk_t> xmit_Buffer; // vector of packet specifics
        //public uint activeXmitChunk;      // the index of the active buffChunk
        //public transferState_t xmitState;

        public hCTransferChannel()
        {
            pPortSupport = new System.IO.Ports.SerialPort();
            //xmit_Buffer = new List<bufChunk_t>();
            recv_Buffer = new List<bufChunk_t>();
        }

        public void OpenPort()
        {
            if (pPortSupport.IsOpen)
            {
                funcCd = 0;// (int)transferFunc_t.tf_OpenPort;
            }
            else
            {
                pPortSupport.Open();
                funcCd = (int)transferFunc_t.tf_OpenPort;
            }
        }

        public void ClosePort()
        {
            if(pPortSupport.IsOpen)
            {
                pPortSupport.Close();
                if (!pPortSupport.IsOpen)
                {
                    funcCd = (int)transferFunc_t.tf_PortClosed;
                }
            }
            else
            {
                funcCd = 0;// (int)transferFunc_t.tf_OpenPort;
            }
        }

    }
    */

}