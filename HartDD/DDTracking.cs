using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{

    public enum cmdOperationType_t
    {
        cmdOpNone = 0,
        cmdOpRead,
        cmdOpWrite,
        cmdOpCmdCmd     // 3
    }

    public class cmdItem
    {
        public cmdDataItemType_t itemType;
        public byte ucConst;
        public float fConst;
        public byte ucMask;
        public uint uiVarId;
        public cmdDataItemFlags_t flags;
    }

    public class hCcommandDescriptor
    {
        public int cmdNumber;
        public int transNumb;
        public int rd_wrWgt;
        //	the following may need to become a list of hCvarSrcTrail as tokenizer improves
        //int				srcIdxVal;	// index value for this resolution - -1 @ none needed
        public int srcIdxVar; //enabled stevev-30dec08, return value from commandAllLists
        //public indexUseList_t idxList;
        public cmdOperationType_t cmdTyp;
        //public List<byte> cmdData;
        public List<cmdItem> cmdItemList;

        public hCcommandDescriptor()
        {
            //idxList = new indexUseList_t();
            clear();
            //cmdData = new List<byte>();
            cmdItemList = new List<cmdItem>();
        }

        //hCcommandDescriptor(const hCcommandDescriptor& cd) {operator=(cd);};
        void clear()
        {
            cmdNumber = transNumb = -1;
            rd_wrWgt = 0;
            srcIdxVar = 0;
            cmdTyp = cmdOperationType_t.cmdOpNone;
            //idxList.Clear();
        }

        /*
        //hCcommandDescriptor& operator=(const hCcommandDescriptor& cd);
        void dumpSelf()
        {
            ;
        }

        hIndexUse_t indexValue(int idxItm)
        {
            hIndexUse_t retType = new hIndexUse_t();
            foreach (hIndexUse_t pIu in idxList)
            {
                if (pIu.indexSymID == idxItm)
                    retType = pIu;
            }
            return retType;
        }
        */
    }
}
