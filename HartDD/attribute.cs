using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class Attribute
    {
        /*Vibhor 230903: Adding these definitions for using in 
        parsing of TYPE_SIZE attribute of a var*/

        public const int DEFAULT_SIZE = 1;
        public const int BOOLEAN_SIZE = 1;
        public const int FLOAT_SIZE = 4;
        public const int DURATION_SIZE = 6;
        public const int TIME_SIZE = 6;
        public const int TIME_VALUE_SIZE = 4;       // timj 4jan08
        public const int DATE_AND_TIME_SIZE = 7;
        public const int DOUBLE_SIZE = 8;

        public const int DATE_SIZE = 3;

        public const int DDS_TYPE_UNUSED = 0;
        public const int INTEGER = 2;
        public const int UNSIGNED = 3;
        public const int FLOATG_PT = 4;
        public const int DOUBLE_FLOAT = 5;
        public const int ENUMERATED = 6;
        public const int BIT_ENUMERATED = 7;
        public const int INDEX = 8;
        public const int ASCII = 9;
        public const int PACKED_ASCII = 10;
        public const int PASSWORD = 11;
        public const int BITSTRING = 12;

        /* The following definition (HART_DATE_FORMAT) is only used by HART */

        public const int HART_DATE_FORMAT = 13;

#if VER_6
        public const int TIME = 14;
        public const int DATE_AND_TIME = 15;
        public const int DURATION = 16;
        public const int EUC = 17;
        public const int OCTETSTRING = 18;
        public const int VISIBLESTRING = 19;
#endif

        public const int TIME_VALUE = 20;
        public const int OBJECT_REF = 21;
        public const int BOOLEAN_T = 22;
        public const int MAX_TYPE1 = 23;    /* must be last in list PAW 03/03/09 added 1 to avoid clash with statreg.h*/


        public const int ROOT_MENU = 1000; /* the item id of the root menu */
        public const int VIEW_MENU = 1009;
        public const int OFFLINE_ROOT_MENU = 1002; // Added for Style Table Tree view, POB - 5/23/2014
        public const int DIAGNOSTIC_ROOT_MENU = 1018;  // Added for Style Table Tree view, POB - 5/23/2014
        public const int PROCESS_VARIABLES_ROOT_MENU = 1019;// Added for Style Table Tree view, POB - 5/23/2014
        public const int DEVICE_ROOT_MENU = 1020; // Added for Style Table Tree view, POB - 5/23/2014
        public const int MAINTENANCE_ROOT_MENU = 1026;  // Added for Style Table Tree view, POB - 5/23/2014


    }

    public class MIN_MAX
    {

        public uint which; /*which min/max value*/
        public bool isID;  // stevev 21aug07 - we gotta know which of the union is valid! 
                           //union
                           //{
        public uint id; /*ID of the variable in case of MIN / MAX -ID*/
        public ddpREFERENCE reff;/*Reference of the vaiable in case of MIN / MAX  - REF*/
                                       //}
                                       //variable;

        public void clear()
        {
            which = 0;
            reff = null;
            isID = false;
        }

        public MIN_MAX()
        {
            reff = new ddpREFERENCE();
            clear();
        }
        //public MIN_MAX(ref MIN_MAX s)
        //{ 
        //  reff = null;
        //operator = (s); 
        //}
        //MIN_MAX operator=(const MIN_MAX& s);

        public void Cleanup()
        {
            if ((!isID) && (reff != null))
            {// ddpREFERENCE has a destructor now....
                reff.Clear();
                reff = null;
            }
            which = 0;
            isID = false;
        }
        // special constructor for ids
        public MIN_MAX(uint w, uint symID)
        {
            which = w;
            isID = true;
            id = symID;
        }
    }


    public class DATA_ITEM_LIST : List<DATA_ITEM>
    {
        public DATA_ITEM_LIST()
        {
            Clear();
        }
        //public DATA_ITEM_LIST(ref DATA_ITEM_LIST src)        {operator= (src); };
        //DATA_ITEM_LIST& operator=(const DATA_ITEM_LIST& src);

        void Cleanup()
        {
            foreach (DATA_ITEM ddi in this)
            {
                ddi.Cleanup();   // PAw return parameter added 07/04/09
            }
        }
    }


    public class Element
    {

        const int VARREF_OPCODE = 27;
        const int MAXREF_OPCODE = 28;
        const int MINREF_OPCODE = 29;

        const int BLOCK_OPCODE = 30;
        const int BLOCKID_OPCODE = 31;
        const int BLOCKREF_OPCODE = 32;
        const int STRCST_OPCODE = 33;
        const int SYSTEMENUM_OPCODE = 34;



        //BYTE  byElemType ; /*1-21 =>OpCode ; 22 =>Int 23=>float 24-29 => ref_id */
        public byte byElemType;
        //union??? {
        //	BYTE			byOpCode;
        public byte byOpCode;
        public UInt64 ulConst; // from ulong 18jun08 stevev
        public float fConst;
        public List<ddpSTRING> pSTRCNST;
        public uint varId;  /* for VAR_ID*/
        public ddpREFERENCE varRef; /* fpr VAR_REF*/
        public MIN_MAX minMax; /* for MIN/MAX - ID & REF*/// made ptr stevev 21aug07 
                                                          //}
                                                          //elem;

        //Element(){byElemType = 0;  elem.pSTRCNST = NULL; elem.minMax.clear();};// it's a union sjv
        public Element()
        {
            byElemType = 0;
            ulConst = 0;
            pSTRCNST = new List<ddpSTRING>();
            varRef = new ddpREFERENCE();
            minMax = new MIN_MAX();
        }// minmax 2 ptr 21aug07 sjv

        /*
        Element(Element cconst)
        { 
            ulConst = 0;
            this = (cconst); 
        }
        */
        /*
        public static Element operator= (Element src)
        {

            Cleanup();// try to prevent memory leaks

            switch (byElemType)// stevev 8au807- then modify those that need to be
            {
                case VARREF_OPCODE:
                    if (src.elem.varRef != null)
                    {
                        elem.varRef = new ddpREFERENCE(*(src.elem.varRef));
                    }
                    break;
                case MAXREF_OPCODE:
                case MINREF_OPCODE:
                    if (src.elem.minMax != NULL)
                    {
                        elem.minMax = new MIN_MAX(*(src.elem.minMax));
                    }
                    break;
                case STRCST_OPCODE:
                    if (src.elem.pSTRCNST != NULL)
                    {
                        elem.pSTRCNST = new ddpSTRING(*(src.elem.pSTRCNST));
                    }
                    break;
                default:
                    memcpy(&elem, &(src.elem), sizeof(elem));// stevev 8au807-do the rest
                    break;
            }
            return *this;

        }
        //void clean(){ byElemType = 0;/* elem = {0}; / elem.minMax.clear();};
        //void clean(){ byElemType = 0; elem.minMax->clear();};
        */

        public void Cleanup()
        {
            switch (byElemType)
            {
                case VARREF_OPCODE:
                    if (varRef != null)
                    {// we gave ddpREFERENCE a destructor
                        varRef.Clear();
                        varRef = null;
                    }
                    break;
                case MAXREF_OPCODE:
                    if (minMax != null)
                    {
                        minMax.Cleanup();
                        minMax = null;
                    }
                    break;
                case MINREF_OPCODE:
                    if (minMax != null)
                    {
                        minMax.Cleanup();
                        minMax = null;
                    }
                    break;
                case STRCST_OPCODE:
                    {
                        pSTRCNST.Clear();
                        pSTRCNST = null;
                    }
                    break;
                default:
                    break;
            }
            byElemType = 0;
        }

        public unsafe static int parse_attr_type_size(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            //	byte  *leave_pointer;	/* chunk ptr for early exit */
            //int rc;
            //byte* chunkp1 = NULL;
            uint* length = null;
            uint tag, len; /* parsed tag */
                           //	uint   typesize;		/* size of tag item (2-byte int) */
            UInt64 LL;
            TYPE_SIZE vartype;

            //ASSERT_DBG(binChunk && size);

            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                length = &size;

                /*Parse the tag to know the type of the variable, as per the spec this can't be a conditional*/
                Common.DDL_PARSE_TAG(chunkp, length, &tag, &len);

                vartype.type = (ushort)tag;

                switch (tag)
                {
                    case Attribute.BOOLEAN_T:
                        vartype.size = (ushort)Attribute.BOOLEAN_SIZE;
                        break;
                    case Attribute.INTEGER:
                    case Attribute.UNSIGNED:

                        /* The length of an item is encoded
                         * with a first byte of zero, and then the following byte(s) encode the
                         * length.  If the first byte of the chunk is non-zero, then there is
                         * no length encoded, and the default value of 1 is returned.
                         */
                        if (len > *length)
                        {
                            return Common.DDL_ENCODING_ERROR;
                        }

                        *length -= len;

                        if ((0 == len) || (**chunkp != 0))
                        {
                            vartype.size = (ushort)Attribute.DEFAULT_SIZE;
                        }
                        else /*Parse the length*/
                        {
                            /*
                             * Skip the zero, and parse the size.
                             */

                            (*chunkp)++;
                            (len)--;

                            Common.DDL_PARSE_INTEGER(chunkp, &len, &LL);

                            vartype.size = (ushort)LL;
                        }
                        break;
                    case Attribute.FLOATG_PT:
                        if (len > *length)
                        {
                            return Common.DDL_ENCODING_ERROR;
                        }
                        *length -= len;

                        vartype.size = (ushort)Attribute.FLOAT_SIZE;

                        break;
                    case Attribute.DOUBLE_FLOAT:
                        if (len > *length)
                        {
                            return Common.DDL_ENCODING_ERROR;
                        }
                        *length -= len;
                        vartype.size = (ushort)Attribute.DOUBLE_SIZE;
                        break;
                    case Attribute.ENUMERATED:
                    case Attribute.BIT_ENUMERATED:
                    case Attribute.INDEX:
                        if (len > *length)
                        {
                            return Common.DDL_ENCODING_ERROR;
                        }
                        *length -= len;

                        if ((0 == len) || (**chunkp) != 0)
                        {
                            vartype.size = (ushort)Attribute.DEFAULT_SIZE;
                        }
                        else /*Parse the length*/
                        {
                            /*
                             * Skip the zero, and parse the size.
                             */

                            (*chunkp)++;
                            (len)--;

                            Common.DDL_PARSE_INTEGER(chunkp, &len, &LL);

                            vartype.size = (ushort)LL;
                        }
                        break;

                    case Attribute.ASCII:         /*These four cases don't have explicit length tag for size*/
                    case Attribute.PACKED_ASCII:
                    case Attribute.PASSWORD:
                    case Attribute.BITSTRING:

                        Common.DDL_PARSE_INTEGER(chunkp, length, &LL);
                        vartype.size = (ushort)LL;
                        break;
                    case Attribute.HART_DATE_FORMAT:
                        vartype.size = (ushort)Attribute.DATE_SIZE;
                        break;
                    //FF	case TIME:
                    //FF		vartype.size = (ushort) TIME_SIZE;
                    //FF		break;
                    case Attribute.TIME_VALUE:
                        vartype.size = (ushort)Attribute.TIME_VALUE_SIZE;
                        break;
                    //FF	case DATE_AND_TIME:
                    //FF		vartype.size = (ushort) DATE_AND_TIME_SIZE;
                    //FF		break;
                    //FF	case DURATION:		/* DURATION */
                    //FF		vartype.size = (ushort) DURATION_SIZE;
                    //FF		break;	
                    //FF	case EUC:

                    //FF		DDL_PARSE_INTEGER(chunkp,length,&typesize);
                    //FF		vartype.size = (ushort)typesize;

                    //FF		break;
                    //FF	case OCTETSTRING:	//TODO: These are undefined at this time
                    //FF	case VISIBLESTRING:
                    //	case BOOLEAN_T:
                    default:
                        return Common.DDL_ENCODING_ERROR;
                        //break;

                }/*End Switch tag*/

                pAttr.pVals = new VALUES();
                // PAW start debugging
                //  pAttr->pVals->typeSize = vartype;// PAW 20/03/09 revised to stop corruption
                pAttr.pVals.typeSize.size = (ushort)vartype.size;
                pAttr.pVals.typeSize.type = (ushort)vartype.type;
                // PAW 20/03/09 end
            }
            return Common.SUCCESS;

        } /*End parse_attr_type_size*/

    }

    public class DDlBlock : DDlBaseItem                /*Item Type == 12*/
    {

        public const int BLOCK_CHARACTERISTIC_ID = 0;
        public const int BLOCK_LABEL_ID = 1;
        public const int BLOCK_HELP_ID = 2;
        public const int BLOCK_PARAM_ID = 3;
        public const int BLOCK_MENU_ID = 4;
        public const int BLOCK_EDIT_DISP_ID = 5;
        public const int BLOCK_METHOD_ID = 6;
        public const int BLOCK_REFRESH_ID = 7;
        public const int BLOCK_UNIT_ID = 8;
        public const int BLOCK_WAO_ID = 9;
        public const int BLOCK_COLLECT_ID = 10;
        public const int BLOCK_ITEM_ARRAY_ID = 11;
        public const int BLOCK_PARAM_LIST_ID = 12;
        public const int MAX_BLOCK_ID = 13; /* must be last in list */
        public const int BLOCK_CHARACTERISTIC = (1 << BLOCK_CHARACTERISTIC_ID);
        public const int BLOCK_LABEL = (1 << BLOCK_LABEL_ID);
        public const int BLOCK_HELP = (1 << BLOCK_HELP_ID);
        public const int BLOCK_PARAM = (1 << BLOCK_PARAM_ID);

        public const int BLOCK_MENU = (1 << BLOCK_MENU_ID);
        public const int BLOCK_EDIT_DISP = (1 << BLOCK_EDIT_DISP_ID);
        public const int BLOCK_METHOD = (1 << BLOCK_METHOD_ID);
        public const int BLOCK_UNIT = (1 << BLOCK_UNIT_ID);

        public const int BLOCK_REFRESH = (1 << BLOCK_REFRESH_ID);
        public const int BLOCK_WAO = (1 << BLOCK_WAO_ID);
        public const int BLOCK_COLLECT = (1 << BLOCK_COLLECT_ID);
        public const int BLOCK_ITEM_ARRAY = (1 << BLOCK_ITEM_ARRAY_ID);

        public const int BLOCK_PARAM_LIST = (1 << BLOCK_PARAM_LIST_ID);

        //	void AllocBlockAttributes( uint ulBlockMask);
        public override void AllocAttributes(uint ulBlockMask)
        {
            DDlAttribute pDDlAttr = null;

            if ((ulBlockMask & BLOCK_PARAM) != 0)
            {
                pDDlAttr = new DDlAttribute("BlockParams", BLOCK_PARAM_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);

                attrList.Add(pDDlAttr);

            }
        }

    }

}