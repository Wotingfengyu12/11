using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class hCtransaction : TRANSACTION
    {
        //public int Number;// transaction number
        //public short size;  // data packet size in bytes
        ///*
        //short firstReqByte; // -1 means NOT constant, else value
        //*/
        //public short constCnt;  // 0 means none, else value is number of constants
        CDDLCmd pCmd;//cmd parent

        hCcmdDispatcher pDispatch; // which instance to use the pointer into

        public hCtransaction()
        {
        }

        public void setCmdPtr(CDDLCmd pCMD)
        {
            pCmd = pCMD;
        }

        public void setWgtTrans(hCcmdDispatcher pDpch)
        {
            pDispatch = pDpch;
        }

        public CDDLCmd getCmdPtr()
        {
            return (pCmd);
        }

        public ulong getTransNum()
        {
            return number;
        }

        public int weigh(bool isRead, ref indexUseList_t useIdx)
        {
            int retInt = 0;

            if (pDispatch == null)
            {
                //LOGIT(CERR_LOG, L"ERROR: dispatch/function null in hCTransaction:weigh.\n");
                retInt = -1;
            }
            else
            {
                retInt = pDispatch.calcWeightOptimized(this, ref useIdx, isRead);
            }
            return retInt;
        }

        public int getReqstDIlist(ref List<DATA_ITEM> pDIlist)
        {
            if (request.Count > 0)
            {
                pDIlist = request;
                return Common.SUCCESS;
            }
            else
            {
                return Common.FAILURE;
            }
        }

        public int getReplyDIlist(ref List<DATA_ITEM> pDIlist)
        {
            if (reply.Count > 0)
            {
                pDIlist = reply;
                return Common.SUCCESS;
            }
            else
            {
                return Common.FAILURE;
            }
        }
    }

    public class hCtransactionList : List<hCtransaction>
    {
        CDDLCmd pCmd;
        hCcmdDispatcher pDispatcher;

        public hCtransactionList()
        {
        }

        public void setCmdPtr(CDDLCmd pCMD)
        {
            pCmd = pCMD;
        }

        public void setWgtVisitor(hCcmdDispatcher pDispatch)
        {
            pDispatcher = pDispatch;
            foreach (hCtransaction iT in this)
            {
                iT.setWgtTrans(pDispatch);
            }
        }

        public hCtransaction getTransactionByNumber(int tNumber)
        {
            int tlsz = Count;
            hCtransaction pRetPtr = null;

            if (tlsz > 0 /*&& tNumber <= tlsz */) // transaction numbers start at 0?
            {
                if (tNumber >= 0)
                {
                    foreach (hCtransaction iT in this)
                    {// iT = ptr 2 hCtransaction
                        if (iT.getTransNum() == (ulong)tNumber)
                        {
                            //works for vs6 #if _MSC_VER >= 1300  // HOMZ - port to 2003, VS7
                            pRetPtr = iT;
                            //#else
                            //					pRetPtr = iT;
                            //#endif
                            break; // out of for...
                        }
                    }
                }
                else // get lowest transaction number
                {
                    tNumber = 255;
                    foreach (hCtransaction iT in this)
                    {// iT = ptr 2 hCtransaction
                        if (iT.getTransNum() < (ulong)tNumber)
                        {
                            tNumber = (int)iT.getTransNum();
                            //works for vs6 #if _MSC_VER >= 1300  // HOMZ - port to 2003, VS7
                            pRetPtr = iT;
                            //#else
                            //					pRetPtr = iT;
                            //#endif
                        }
                    }
                }
                if (pRetPtr == null)
                {
                    ;
                }
            }
            else
            {
                //		cerr< <"ERROR: Transaction number "< < tNumber
                //								< < " out of range for " < < tlsz < < " transactions."< <endl;
                //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: Empty transaction list.\n");
            }
            return pRetPtr;
        }

    }
}
