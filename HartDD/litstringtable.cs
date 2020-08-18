using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class litstringtable
    {

        Dictionary<uint, string> table;// = new Dictionary<uint, string>();

        public litstringtable()
        {
            table = new Dictionary<uint, string>();
        }

        public void install(uint index, string s)
        {
            table.Add(index, s);
        }        

        //wstring get_lit_string(unsigned long index);
        public string get_lit_string(uint index)
        {
            if (table.ContainsKey(index))
                return table[index];
            else
                return "";// otherwise it would make a record.
        }
        void dump()
        {
            foreach (KeyValuePair<uint, string> kvp in table)
            {
                string litstr = get_lit_string(kvp.Key);

            }
        }

        public int makelit(DDlDevDescription.STRING_TBL string_tbl, bool isLatin1)
        {
            //List<ddpSTRING> dstring = null;      /* temp pointer for the list */
            //List<ddpSTRING> end_string = null;   /* end pointer for the list */

            uint index = 0;
            foreach (ddpSTRING dstring in string_tbl.list)
            {// a list of ddpSTRING
                /*
                string hld;
                hld = dstring.str;
                string lit;
                dstring.str = null;// stevev 28sep11 - take ownership of the memory
                if (isLatin1)
                {
                    //int iAllocLength = latin2utf8size(string->str) + 1;
                    int iAllocLength = hld.Length + 1;
                        //latin2utf8size(hld) + 1;
                    lit = new char[iAllocLength];
                    //if (lit == (char*)0)
                    {
                        //LOGIT(CERR_LOG, L"Memory exhausted.\n");
                        //exit(-1);
                    }

                    //latin2utf8(string->str, lit, iAllocLength);
                    latin2utf8(hld, lit, iAllocLength);
                    //delete[] hld;
                }
                else
                {
                    lit = hld;  // before this change, valid utf8 strings caused a 
                                // NULL to install in the table [2/27/2014 timj]
                }
                */
                install(index++, dstring.str);
            }

            return 0;	// dicterrs            ;
        }
        //int makelit(FMx_Dict* fmxdict)
        //{
        //    ;
        //}

    }
}
