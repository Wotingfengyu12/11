using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace FieldIot.HARTDD
{
    public class HartDictionary
    {
        public const int TOK_ERROR = 0;
        public const int TOK_DONE = 1;
        public const int TOK_NUMBER = 2;
        public const int TOK_NAME = 3;
        public const int TOK_TEXT = 4;
        public const int TOK_COMMA = 5;

        //MODIFIED: Deepak changed size to 10
        string languageCode;       //         wchar_t languageCode[10];       // timj 7jan08
        //static int dicterrs;
        //static int dictline;
        //static int dict_limit = 0;      /* Size of dict_array */
        //static int dict_count = 0;      /* Number of entries in dict_array */
        //static DICT_ENT[] dict_array;// = new DICT_ENT();	/* Array of DICT_ENTs */

        const string high_text = "|en|@XX@Highest Offset";

        static string dictfile = null;

        //static string tokbase = null;
        //static string tokptr = null;
        //static string toklim = null;


        // PAW 02/06/09 place these outside the class as new and malloc are tramping all over them ! */
        public struct DICT_TABLE_ENTRY
        {
            public UInt32 ulref;
            public string name;        // timj 13dec07
            public ushort len;
            public string str;
            public bool used;      // timj 14jan08
        };

        //public struct DICT_ENT
        //{
        //    public UInt32 section;  /* The entry section number*/
        //    public UInt32 offset;       /* The entry number within the section */
        //    public UInt32 value;        /* The value is computed from section and offset*/
        //    public string name;     /* The name of the entry*/
        //    public string dict_text;    /* The text for the entry*/
        //};

        const int MAX_DICT_TABLE_SIZE = 0x4000;
        const string DEF__LANG__CTRY = "|en|";
        DICT_TABLE_ENTRY[] dict_table; /*This array holds the dictionary*/

        byte[] pchDictData;
        //int iIndex;

        UInt32 num_dict_table_entries;
        public HartDictionary()
        {
            dict_table = new DICT_TABLE_ENTRY[MAX_DICT_TABLE_SIZE];
        }

        public HartDictionary(string pchLangCode)
        {

            dict_table = new DICT_TABLE_ENTRY[MAX_DICT_TABLE_SIZE];
            //MODIFIED: Deepak Added if condition
            if (pchLangCode != "")
            {
                languageCode = pchLangCode;
            }
            else
            {
                languageCode = DEF__LANG__CTRY;
            }
            /*
            for (i = 0; i < MAX_DICT_TABLE_SIZE; i++)
            {
                dict_table[i].str = "";
                //MODIFIED by Deepak, Added two member initialization
                dict_table[i].ulref = 0;
                dict_table[i].len = 0;
                dict_table[i].name = "";  // J.U. this is checked in the destructor and have to be initialized
                dict_table[i].used = false; // J.U.
            }
            */
            //MODIFIED by Deepak, Added two member initialization
            //pchDictData = "";
            //iIndex = 0;

            num_dict_table_entries = 0;
        }

        public int get_string_translation(string instr, ref string outstr)
        {
            if (instr == null || instr == "")
            {
                return 1;
            }

            if(instr[0] != '|')
            {
                outstr = instr;
            }
            else
            {
                string[] mull = instr.Split('|');
                for (int i = 1; i < mull.Count(); i++)
                {
                    if (mull[i] == "en" || mull[i] == "en zz")
                    {
                        outstr = mull[i + 1];
                    }

                    if (mull[i] == Thread.CurrentThread.CurrentUICulture.Name)
                    {
                        outstr = mull[i + 1];
                        break;
                    }
                }
            }
            return 0;
        }

        public int get_dictionary_string(uint index, ref ddpSTRING str)
        {
            //ADDED By Deepak , initializing all vars
            DICT_TABLE_ENTRY found_ptr = new DICT_TABLE_ENTRY();
            bool bf = false;
            //DICT_TABLE_ENTRY key;

            //key.ulref = index;

            /*
             * Perform a binary search on the standard dictionary table to find the
             * entry we're looking for.
             

            found_ptr = (DICT_TABLE_ENTRY*)B_SEARCH((char*)&key,
                (char*)dict_table, (unsigned)num_dict_table_entries,
                sizeof(DICT_TABLE_ENTRY), dict_compare);*/

            foreach (DICT_TABLE_ENTRY de in dict_table)
            {
                if (de.ulref == index)
                {
                    found_ptr = de;
                    bf = true;
                    break;
                }
            }

            if (!bf)
            {
                return Common.DDL_DICT_STRING_NOT_FOUND;
            }
            else
            {
                /*
                 * Retrieve the information
                 */
                str.flags = Common.DONT_FREE_STRING;
                str.len = found_ptr.len;
                str.str = found_ptr.str;
                return Common.DDL_SUCCESS;
            }
        }

        void dict_table_install(uint reff, string name, string str)
        {
            dict_table[num_dict_table_entries].ulref = reff;
            dict_table[num_dict_table_entries].len = (ushort)str.Length;
            dict_table[num_dict_table_entries].str = str;
            dict_table[num_dict_table_entries].name = name;
            dict_table[num_dict_table_entries].used = true; // timj 14jan08

            num_dict_table_entries++;
            // stevev 20aug07 - the computer knows it has an issue, it needs to tell about it.
            if (num_dict_table_entries >= MAX_DICT_TABLE_SIZE)
            {
                //LOGIT(CERR_LOG | UI_LOG, "ERROR: Dictionaries too big.\n");
                num_dict_table_entries--;// force a write-over to prevent writing into un-alloc'd memory
            }
        }

        public int makedict(string file, string[] addnl_file_array)
        {
            //ADDED By Deepak initialized the variables
            int iDictDataSize = 0;
            int iAddnlFileIndex;
            /*
             * Initialize the globals.
             */

            //dicterrs = 0;
            //dictline = 1;

            /*
             *	Create the DICT_ENT pointer array.
             */

            //dict_limit = 200; 
            /*Vibhor 141003: Increasing the dict limit to 500 as we are supporting upto 
                             4 additional dictionaries 	*/
            //dict_limit = MAX_DICT_TABLE_SIZE;
            //dict_count = 0;
            /* DEEPAK : Since we need to use realloc we shall use malloc and free
            dict_array = (DICT_ENT *)new DICT_ENT[((unsigned)(dict_limit))];
            */
            //dict_array = new DICT_ENT[dict_limit];

            dictfile = file;

            iAddnlFileIndex = 0;

            while (dictfile != null && dictfile != "")
            {
                /*
		         * Open the file.
		         */

                BinaryReader dictfp;

                dictfp = new BinaryReader(new FileStream(dictfile, FileMode.Open, FileAccess.Read));
                FileInfo fin = new FileInfo(dictfile);
                iDictDataSize = (int)fin.Length;
                if (dictfp != null)
                {
                    pchDictData = new byte[iDictDataSize];

                    dictfp.Read(pchDictData, 0, iDictDataSize);

                    /*Terminate the buffer with NULL character*/
                    /*
                     * Read it.
                     */
                    //iIndex = 0;
                    /*
                     *	Sort the entries from the files
                     */
                    //ADDED by Deepak
                    //if (dict_array != null)
                    //{
                    //    Endian.QSort<DICT_ENT>(dict_array, compdict);
                    //}
                    //string dctstring = Encoding.Default.GetString(pchDictData);
                    //string dctstring = Encoding.GetEncoding("Windows-1252").GetString(pchDictData);
                    string dctstring = Encoding.UTF8.GetString(pchDictData);

                    string dctnocomm = Regex.Replace(dctstring, @"/\*(.|\r\n)*?\*/", "");


                    string[] sections;// = new string[MAX_DICT_TABLE_SIZE];
                    sections = dctnocomm.Split('[');

                    //for(int i = 0; i < sections.Count(); i++)
                    foreach (string st in sections)
                    {
                        if (Regex.IsMatch(st, @"^[0-9].*"))
                        {
                            string[] scn = st.Split("\r\n".ToCharArray());
                            List<string> scnlist = new List<string>();
                            foreach (string ss in scn)
                            {
                                if (ss != "")
                                {
                                    scnlist.Add(ss);
                                }
                            }


                            string a = Regex.Match(scnlist[0], @"^\d+,").Value;
                            a = a.Substring(0, a.Length - 1);
                            uint section = Convert.ToUInt32(a);
                            string b = Regex.Match(scnlist[0], @",\d+\]").Value;
                            b = b.Substring(1, b.Length - 2);
                            uint offset = Convert.ToUInt32(b);
                            string name = Regex.Match(scnlist[0], @"\].+").Value;
                            //name = name.Substring(3, name.Length - 3);
                            name = Regex.Replace(name, "\".+\"", "");
                            name = name.Replace("]", "");
                            name = name.Replace(" ", "");
                            string dict_text = Regex.Match(scnlist[0], "\".+\"").Value;
                            if (dict_text != "")
                            {
                                dict_text = dict_text.Substring(1, dict_text.Length - 2);
                                if (!Regex.IsMatch(dict_text, "^\\|.*"))
                                {
                                    dict_text = "|en|" + dict_text;
                                }
                            }
                            if (scnlist.Count > 1)//xxx_highestoffset            "@XX@Highest Offset"
                            {
                                string dict_start = Regex.Match(scnlist[1], "\".+\"").Value;
                                if (dict_start != "")
                                {
                                    dict_start = dict_start.Substring(1, dict_start.Length - 2);
                                    dict_text = dict_text + dict_start;
                                }
                                //if (dict_text != "")
                                {
                                }
                                if (!Regex.IsMatch(dict_text, "^\\|.*"))
                                {
                                    dict_text = "|en|" + dict_text;
                                }
                                if (scnlist.Count > 2)
                                {
                                    for (int i = 2; i < scnlist.Count; i++)
                                    {
                                        string lang = Regex.Match(scnlist[i], "\".+\"").Value;
                                        if (lang != "")
                                        {
                                            lang = lang.Substring(1, lang.Length - 2);
                                            dict_text += lang;

                                        }
                                    }
                                }
                            }
                            if (!Regex.IsMatch(name, "xxx_highestoffset.*"))
                            {
                                dict_table_install(section * 65536 + offset, name, dict_text);
                            }
                            /*
                            if (scnlist.Count > 2 && scnlist.Count != 5)
                            {
                                scnlist.Add("aaa");
                            }
                            */
                        }

                    }
                    //string a = Regex.Match(tokbase, @"\[\d+,").Value;

                    dictfp.Close();

                    //ADDED By Deepak
                    pchDictData = null;
                }// endif not failure

                if (dictfile == addnl_file_array[iAddnlFileIndex])
                {
                    dictfile = null;
                }
                else
                {
                    dictfile = addnl_file_array[iAddnlFileIndex++];
                }
            }// wend dictfile not null

            /*
             * Return number of errors seen.
             */
            return 0;
        }

        public int makedict(ref DDlDevDescription.DICT_REF_TBL dict_ref_tbl)
        {
            ushort i;
            //DEBUGLOG(CLOG_LOG, "DICTIONARY::: makedict from table at %p\n", this);

            if ((object)dict_ref_tbl.name == null && (object)dict_ref_tbl.text == null)
            {
                // timj  14jan08
                // name and text are not present. therefore dictionary just came in
                // from an fm6 binary.  also, dictionary has already been installed
                // from .dct file with name and text.  

                // Each entry with a ref in dict_ref_tbl will be marked as used,
                // others are marked as unused

                // first mark all entries as being unused
                for (i = 0; i < num_dict_table_entries; i++)
                {
                    dict_table[i].used = false;
                }

                // then mark ref's in dict_ref_tbl as being used
                for (i = 0; i < dict_ref_tbl.count; i++)
                {
                    DICT_TABLE_ENTRY found_ptr = new DICT_TABLE_ENTRY();
                    foreach (DICT_TABLE_ENTRY de in dict_table)
                    {
                        if (de.ulref == dict_ref_tbl.list[i])
                        {
                            found_ptr = de;
                            found_ptr.used = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                // timj 14jan08
                // dictionary just came in from binary (fm8) or from .dct file (fm6)
                // build the dictionary, marking all entries as used
                for (i = 0; i < dict_ref_tbl.count; i++)
                {
                    dict_table_install(dict_ref_tbl.list[i], dict_ref_tbl.name[i].str, dict_ref_tbl.text[i].str);//??????
                    // stevev - 28sep11 - mark as deletable to try and stop mem leak
                    dict_ref_tbl.name[i].flags = Common.FREE_STRING;
                    dict_ref_tbl.text[i].flags = Common.FREE_STRING;
                }

                // sort the dictionary entries
                Endian.QSort<DICT_TABLE_ENTRY>(dict_table, num_dict_table_entries, DictableSortBy);
                //(void)qsort(dict_table, num_dict_table_entries, sizeof(DICT_TABLE_ENTRY), dict_compare);

            }
            return 0;   // dicterrs
        }


        static int DictableSortBy(DICT_TABLE_ENTRY a, DICT_TABLE_ENTRY b)
        {
            if (a.ulref > b.ulref)
                return 1;
            if (a.ulref < b.ulref)
                return -1;
            return 0;
        }

        /*
        static int compdict(DICT_ENT a, DICT_ENT b)
        {
            if (a.value > b.value)
                return 1;
            if (a.value < b.value)
                return -1;
            return 0;
        }
        */

    }
    public class ddpREFERENCE : List<ddpREF>
    {
        public ddpREFERENCE()
        {
            Clear();
        }
        //ddpREFERENCE&  operator= (const ddpREFERENCE& src);
        //ddpREFERENCE(const ddpREFERENCE& r) { operator= (r); };

        public void Cleanup()     // PAW return parameter added 07/04/09
        {

            foreach (ddpREF iRef in this)
            {
                iRef.Cleanup();
            }

            Clear();
        }

        //	void* First(void) { return ((void*)_First);};removed for debug PAW 07/04/09
        // left out, apparently not needed.  10aug10 stevev
    }
    
    public class ddbENUM_REF
    {

        //union{
        public uint iD;  /*valid if ENUMERATION_STRING_TAG*/
        public ddpREFERENCE reff; /*valid if ENUM_REF_STRING_TAG */ //Vibhor 051103
                                                                    //}
                                                                    //enmVar;
        public uint enumValue;
        public ddbENUM_REF()
        {
            reff = null;
            iD = 0;
            enumValue = 0xffffffff;
        }
    };
    public class ddpExpression : List<Element>
    {

        public ddpExpression()
        {
            Clear();
        }
        //ddpExpression(const ddpExpression& src) { operator= (src); };
        //ddpExpression& operator=(const ddpExpression& src);
    };

    public class ddpREF
    {
        public const int DEFAULT_TYPE_REF = -1;
        public const int ITEM_ID_REF = 0;
        public const int ITEM_ARRAY_ID_REF = 1;
        public const int COLLECTION_ID_REF = 2;

        public const int VIA_ITEM_ARRAY_REF = 3;
        public const int VIA_COLLECTION_REF = 4;
        public const int VIA_RECORD_REF = 5;
        public const int VIA_ARRAY_REF = 6;
        public const int VIA_VAR_LIST_REF = 7;
        public const int VIA_PARAM_REF = 8;
        public const int VIA_PARAM_LIST_REF = 9;
        public const int VIA_BLOCK_REF = 10;
        public const int BLOCK_ID_REF = 11;
        public const int VARIABLE_ID_REF = 12;
        public const int MENU_ID_REF = 13;
        public const int EDIT_DISP_ID_REF = 14;
        public const int METHOD_ID_REF = 15;
        public const int REFRESH_ID_REF = 16;
        public const int UNIT_ID_REF = 17;
        public const int WAO_ID_REF = 18;
        public const int RECORD_ID_REF = 19;
        public const int ARRAY_ID_REF = 20;
        public const int VAR_LIST_ID_REF = 21;
        public const int PROGRAM_ID_REF = 22;
        public const int DOMAIN_ID_REF = 23;
        public const int RESP_CODES_ID_REF = 24;

        public const int FILE_ID_REF = 25;
        public const int CHART_ID_REF = 26;
        public const int GRAPH_ID_REF = 27;
        public const int AXIS_ID_REF = 28;
        public const int WAVEFORM_ID_REF = 29;
        public const int SOURCE_ID_REF = 30;
        public const int LIST_ID_REF = 31;

        public const int IMAGE_ID_REF = 32;
        public const int SEPARATOR_REF = 33;/* colbreak*/
        public const int CONSTANT_REF = 34;/* see Expression opcodes for type */

        public const int VIA_FILE_REF = 35;
        public const int VIA_LIST_REF = 36;
        public const int VIA_BITENUM_REF = 37;

        public const int GRID_ID_REF = 38; /* added at end to be backwards compatable */
        public const int ROWBREAK_REF = 39;
        /* 25apr05 - add access to members */
        public const int VIA_CHART_REF = 40;
        public const int VIA_GRAPH_REF = 41;
        public const int VIA_SOURCE_REF = 42;
        /*           and attribute reference*/
        public const int VIA_ATTR_REF = 43;
        public const int BLOB_ID_REF = 44; /* new 14nov08 experimental at this time. */
        public const int VIA_BLOB_REF = 45;    /* to get to attribute references */
        public const int TEMPLATE_ID_REF = 46; /* new - has no VIA capability 19oct12 */
        public const int PLUGIN_ID_REF = 47;/* new - has no VIA capability 19oct12 */
        public const int MAX_REFTYPE = 48;

        public ushort type;

        //union 
        //{
        public uint id;     // image number too  
        public ddpExpression index;
        public uint member;   // member name id 

        //}
        //val;

        public ddpREF()
        {
            type = 0xffff;
            index = null;
            id = 0;
        }
        //ddpREF(const ddpREF& r) :type(0) { val.index = null; operator= (r); };  // copy constructor for vector pushes
        //~ddpREF() { Cleanup(); };

        public void Cleanup()//??????
        {
            if (type == VIA_ITEM_ARRAY_REF ||
               /* VIA_COLLECTION_REF || VIA_FILE_REF || VIA_BITENUM_REF || VIA_CHART_REF || 
                  VIA_GRAPH_REF || VIA_SOURCE_REF || VIA_ATTR_REF
               these are supposed to be member names, not expression lists */
               type == VIA_ARRAY_REF ||
               type == VIA_LIST_REF ||
               type == CONSTANT_REF)/* the other 7 via_xxx are made to be constant member or id inparse_base*/
            {
                if (index != null && index.Count > 0)
                {
                    if (index.Count > 10000)
                    {
                        //LOGIT(CLOG_LOG, "ddpREF has an issue while trying to Cleanup().\n");
                        //RAZE(val.index);
                    }
                    else
                    {

                        foreach (Element el in index)
                        {
                            el.Cleanup();
                        }
                        index.Clear();
                    }
                }
                type = 0;
                member = 0;// will cover val.index,val.id both        }

                //ddpREF& operator= (const ddpREF& src);

            };

        }
    }
    public class ddpSTRING
    {

        public string str;  /* the pointer to the string */
        public ushort len; /* the length of the string */
        public ushort flags;   /* memory allocation flags */
        public uint strType; /*Simply store the tag of string type*/

        /*The above 3 guys would be Null in the following cases*/

        public ddbENUM_REF enumStr; /*reference to an enumerated varible*/
        public uint varId; /*id of a var of type string to be entered by the user @ runtime*/
        public ddpREFERENCE varRef; /* reference to a variable of type string
							   if present, this value has to be 
								entered by the user @ runtime!!!*/
        //ddpSTRING();
        //           ddpSTRING& operator=(const ddpSTRING& srcStr );
        //ddpSTRING( const ddpSTRING& s) : str(null),len(0),flags(0),strType(0),varId(0)
        //           {  operator= (s); };
        //           ~ddpSTRING() { Cleanup(); };

        public ddpSTRING()
        {
            enumStr = new ddbENUM_REF();
            varRef = new ddpREFERENCE();
        }

        public void Cleanup()
        {
            ;
        }
    }


}
