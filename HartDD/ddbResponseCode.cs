using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class hCrespCode
    {
        const int SUCCESS_RSPCODE = 1;
        const int MISC_WARNING_RSPCODE = 2;
        const int DATA_ENTRY_WARNING_RSPCODE = 3;
        const int DATA_ENTRY_ERROR_RSPCODE = 4;
        const int MODE_ERROR_RSPCODE = 5;
        const int PROCESS_ERROR_RSPCODE = 6;
        const int MISC_ERROR_RSPCODE = 7;
        const int INVALID_RSPCODE = 8;

        public uint val;
        public uint type;
        public string descS;
        public string helpS;
        //string retString; // stevev - WIDE2NARROW char interface

        public hCrespCode()
        {

        }

        /*
        public static hCrespCode operator ~(hCrespCode src)
        {
            hCrespCode hcc = new hCrespCode();
            hcc.val = src.val;
            hcc.type = 0;
            hcc.descS = src.descS;
            hcc.helpS = src.helpS;
            return hcc;
        }
        */

        public uint getVal()
        {
            return val;
        }

        public string getDescStr()
        {
            return descS;
        }
        // stevev - WIDE2NARROW char interface \/ \/ \/
        // stevev - always wide... string       get_DescStr(void) {return ((string) descS);};
        public uint getType()
        {
            return type;
        }

        //static
        public uint getType(byte c) /* stevev 7mar05 - make translation available to all */
        {
            if ((c >= 0 && c <= 7) || (c >= 16 && c <= 23) || (c >= 32 && c <= 64)
              || (c >= 9 && c <= 13) || (c >= 65 && c <= 95) || (c == 15)
              || (c == 28) || (c == 29))
            {/* it is an error */
                return MISC_ERROR_RSPCODE;
            }
            else
            {/* it is a warning */
                return MISC_WARNING_RSPCODE;
            }
        }

        public void setEqual(hCrespCode pArc)
        {
            val = pArc.val;
            type = pArc.type;
            descS = pArc.descS;
            helpS = pArc.helpS;
        }

        public void setDuplicate(hCrespCode pSrc)
        {
            val = pSrc.val;
            type = pSrc.type;
            descS = pSrc.descS;
            helpS = pSrc.helpS;
        }
        // duplicate strings with any replicate, they don't use it

        //public void dumpSelf(int indent = 0, string typeName = null);

    }

    /*
    public class responseCodeList_t : List<hCrespCode>
    {

    }
    */

    public class hCRespCodeList : List<hCrespCode>
    {

    }

}
