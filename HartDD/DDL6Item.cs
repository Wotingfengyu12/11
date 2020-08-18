using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace FieldIot.HARTDD
{
    public class DDL6Item
    {
        public struct DEPBIN
        {
            public uint bin_size;
            public byte[] bin_chunk;
            public uint bin_offset;
        }

        public struct FLAT_MASKS
        {
            public uint bin_exists;       /* There is binary for this attribute */
            public uint bin_hooked;       /* Binary is attached for this attribute */
            ///public uint attr_avail;       /* The attribute has been evaluated	 */
            ///public uint dynamic;      /* The attribute is dynamic */
        }
        public struct VAR_DEPBIN
        {
            public DEPBIN db_class;
            public DEPBIN db_handling;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_type_size;
            public DEPBIN db_display;
            public DEPBIN db_edit;
            public DEPBIN db_enums;
            public DEPBIN db_index_item_array;
            public DEPBIN db_resp_codes;
            public DEPBIN db_default_value;
        }

        public struct VAR_ACTIONS_DEPBIN
        {
            public DEPBIN db_pre_edit_act;
            public DEPBIN db_post_edit_act;
            public DEPBIN db_pre_read_act;
            public DEPBIN db_post_read_act;
            public DEPBIN db_pre_write_act;
            public DEPBIN db_post_write_act;
            public DEPBIN db_refresh_act;
            public DEPBIN db_post_rqst_act;
            public DEPBIN db_post_user_act;
        }

        public struct FLAT_VAR_ACTIONS
        {
            public VAR_ACTIONS_DEPBIN depbin;
        }

        public struct VAR_MISC_DEPBIN
        {
            public DEPBIN db_unit;
            public DEPBIN db_height;  //db_read_time_out;
            public DEPBIN db_width;   //db_write_time_out;
            public DEPBIN db_min_val;
            public DEPBIN db_max_val;
            public DEPBIN db_scale;
            public DEPBIN db_valid;
            public DEPBIN db_debug_info;
            public DEPBIN db_time_format;
            public DEPBIN db_time_scale;
        }

        public struct FLAT_VAR_MISC
        {
            public VAR_MISC_DEPBIN depbin;
        }

        public struct FLAT_VAR
        {
            public uint id;                /* Item ID for this item */
            public FLAT_MASKS masks;
            public VAR_DEPBIN depbin;
            public FLAT_VAR_ACTIONS actions;
            public FLAT_VAR_MISC misc;
        }

        public struct COMMAND_DEPBIN
        {
            public DEPBIN db_number;
            public DEPBIN db_oper;
            public DEPBIN db_trans;
            public DEPBIN db_resp_codes;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_COMMAND
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public COMMAND_DEPBIN depbin;
        }

        public struct MENU_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_items;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_style;/* TODO: add to clean_flat() */
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_MENU
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public MENU_DEPBIN depbin;
        }

        public struct EDIT_DISPLAY_DEPBIN
        {
            public DEPBIN db_disp_items;
            public DEPBIN db_edit_items;
            public DEPBIN db_label;
            public DEPBIN db_pre_edit_act;
            public DEPBIN db_post_edit_act;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_EDIT_DISPLAY
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public EDIT_DISPLAY_DEPBIN depbin;
        }

        public struct METHOD_DEPBIN
        {
            public DEPBIN db_class;
            public DEPBIN db_def;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_valid;
            public DEPBIN db_scope;
            public DEPBIN db_type;
            public DEPBIN db_params;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_METHOD
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public METHOD_DEPBIN depbin;
        }

        public struct ITEM_ARRAY_DEPBIN
        {
            public DEPBIN db_elements;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_valid;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_ITEM_ARRAY
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            //public ushort subtype;
            public ITEM_ARRAY_DEPBIN depbin;
        }

        public struct COLLECTION_DEPBIN
        {
            public DEPBIN db_members;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_label;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_COLLECTION
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public COLLECTION_DEPBIN depbin;
        }

        public struct REFRESH_DEPBIN
        {
            public DEPBIN db_items;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct UNIT_DEPBIN
        {
            public DEPBIN db_items;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_UNIT
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public UNIT_DEPBIN depbin;
        }

        public struct FLAT_REFRESH
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public REFRESH_DEPBIN depbin;
        }

        public struct WAO_DEPBIN
        {
            public DEPBIN db_items;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_WAO
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public WAO_DEPBIN depbin;
        }

        public struct RECORD_DEPBIN
        {
            public DEPBIN db_members;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_resp_codes;
        }

        public struct FLAT_RECORD
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public RECORD_DEPBIN depbin;
        }

        public struct BLOCK_DEPBIN
        {
            public DEPBIN db_characteristic;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_param;
            public DEPBIN db_param_list;
            public DEPBIN db_item_array;
            public DEPBIN db_collect;
            public DEPBIN db_menu;
            public DEPBIN db_edit_disp;
            public DEPBIN db_method;
            public DEPBIN db_unit;
            public DEPBIN db_refresh;
            public DEPBIN db_wao;
        }

        public struct ARRAY_DEPBIN
        {
            public DEPBIN db_num_of_elements;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_type;
            public DEPBIN db_valid;
            //public DEPBIN db_resp_codes;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }


        public struct FLAT_ARRAY
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public ARRAY_DEPBIN depbin;
        }

        public struct FLAT_BLOCK
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public BLOCK_DEPBIN depbin;
        }

        public struct FILE_DEPBIN
        {
            public DEPBIN db_members;
            public DEPBIN db_help;
            public DEPBIN db_label;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_FILE
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            //public ushort subtype;
            public FILE_DEPBIN depbin;
        }


        /*
         * CHART item
         */
        public struct CHART_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_height;
            public DEPBIN db_width;
            public DEPBIN db_type;
            public DEPBIN db_length;
            public DEPBIN db_cytime;
            public DEPBIN db_members;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_CHART
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            public CHART_DEPBIN depbin;
        }

        /*
         * GRAPH item
         */
        public struct GRAPH_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_height;
            public DEPBIN db_width;
            public DEPBIN db_x_axis;
            public DEPBIN db_members;
            public DEPBIN db_cytime;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_GRAPH
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // other stuff TODO - define this correctly
            public GRAPH_DEPBIN depbin;
        }

        /*
         * AXIS item
         */
        public struct AXIS_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_minval;
            public DEPBIN db_maxval;
            public DEPBIN db_scaling;
            public DEPBIN db_unit;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_AXIS
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // other stuff	TODO - define this correctly
            public AXIS_DEPBIN depbin;
        }

        /*
         * WAVEFORM item
         */
        public struct WAVEFORM_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_handling;
            public DEPBIN db_emphasis;
            public DEPBIN db_linetype;
            public DEPBIN db_linecolor;
            public DEPBIN db_y_axis;
            public DEPBIN db_x_keypts;
            public DEPBIN db_y_keypts;
            public DEPBIN db_type;
            public DEPBIN db_x_values;
            public DEPBIN db_y_values;
            public DEPBIN db_x_initial;
            public DEPBIN db_x_incr;
            public DEPBIN db_pt_count;
            public DEPBIN db_init_acts;
            public DEPBIN db_rfrsh_acts;
            public DEPBIN db_exit_acts;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
            public DEPBIN db_valid;       /* 23jan07 sjv - spec change */
        }

        public struct FLAT_WAVEFORM
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // TODO define the data instances correctly
            public WAVEFORM_DEPBIN depbin;
        }

        /*
         * SOURCE item
         */
        public struct SOURCE_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_emphasis;
            public DEPBIN db_linetype;
            public DEPBIN db_linecolor;
            public DEPBIN db_y_axis;
            public DEPBIN db_init_acts;
            public DEPBIN db_rfrsh_acts;
            public DEPBIN db_exit_acts;
            public DEPBIN db_members;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_SOURCE
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // TODO define the data instances correctly
            public SOURCE_DEPBIN depbin;
        }

        /*
         * LIST item
         */
        public struct LIST_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_type;
            public DEPBIN db_count;
            public DEPBIN db_capacity;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_LIST
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // TODO define the data instances correctly
            public LIST_DEPBIN depbin;
        }


        /*
         * GRID item
         */
        public struct GRID_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_height;
            public DEPBIN db_width;
            public DEPBIN db_orient;
            public DEPBIN db_handling;
            public DEPBIN db_members;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_GRID
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // other stuff TODO - define this correctly
            public GRID_DEPBIN depbin;
        }


        /*
         * IMAGE item
         */
        public struct IMAGE_DEPBIN
        {
            public DEPBIN db_label;
            public DEPBIN db_help;
            public DEPBIN db_valid;
            public DEPBIN db_link;
            public DEPBIN db_path;
            public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        }

        public struct FLAT_IMAGE
        {
            public uint id; /* Item ID for this item */
            public FLAT_MASKS masks;
            // TODO define the data instances correctly
            public IMAGE_DEPBIN depbin;
        }

        /*
         * BLOB item
         */
        //public struct BLOB_DEPBIN
        //{
        //    public DEPBIN db_label;
        //    public DEPBIN db_help;
        //    public DEPBIN db_handling;
        //    public DEPBIN db_identifier;
        //    public DEPBIN db_debug_info;/* TODO: add to clean_flat() */
        //}

        //public struct FLAT_BLOB
        //{
        //    public uint id; /* Item ID for this item */
        //    public FLAT_MASKS masks;
        //    // TODO define the data instances correctly
        //    public IMAGE_DEPBIN depbin;
        //}

        public struct FLAT_UNION_T//union
        {
            public FLAT_VAR fVar;
            public FLAT_COMMAND fCmd;
            public FLAT_MENU fMenu;
            public FLAT_EDIT_DISPLAY fEditDisp;
            public FLAT_METHOD fMethod;
            public FLAT_ITEM_ARRAY fIArr;
            public FLAT_COLLECTION fColl;
            public FLAT_REFRESH fRefresh;
            public FLAT_UNIT fUnit;
            public FLAT_WAO fWao;
            public FLAT_RECORD fRec;
            public FLAT_BLOCK fBlock;
            public FLAT_ARRAY fArr;
            //FLAT_PROGRAM		fProg;
            //FLAT_VAR_LIST		fVarList;
            //FLAT_RESP_CODE	fRespCd;
            //FLAT_DOMAIN		fDomain;
            public FLAT_FILE fFile;
            public FLAT_CHART fChart;
            public FLAT_GRAPH fGraph;
            public FLAT_AXIS fAxis;
            public FLAT_WAVEFORM fWaveFrm;
            public FLAT_SOURCE fSource;
            public FLAT_LIST fList;
            public FLAT_GRID fGrid;
            public FLAT_IMAGE fImage;
            //public FLAT_BLOB fBlob;
        }

    }

    public class DDL6BaseItem : DDlBaseItem
    {
        public static DDL6Item.FLAT_UNION_T glblFlats;// we only do one item at a time

        public int preFetchItem(ref DDlBaseItem pBaseItm, byte maskSize, ref byte[] pObjExtn, /*INT*/ref int rSize, ref uint pbyLocalAttrOffset)
        {
            Common.ITEM_EXTN pItmExtn = (Common.ITEM_EXTN)Common.BytesToStuct(pObjExtn, typeof(Common.ITEM_EXTN));
            //(ITEM_EXTN*)(*pObjExtn);
            bool byRetVal = Endian.read_dword(ref (pBaseItm.id), pItmExtn.byItemId, Endian.FORMAT_BIG_ENDIAN);
            if (!byRetVal)
                return (Common.DDL_ENCODING_ERROR);
            if (pBaseItm.id == 0L)            // no id for an external object (leave mask 0)
                return (Common.FETCH_EXTERNAL_OBJECT);// no need to go further

            int retVal = Common._preFetchItem(maskSize, ref pObjExtn, ref rSize, ref attrMask, ref pbyLocalAttrOffset);

            //memset((char*)(&glblFlats), 0, sizeof(FLAT_UNION_T));// clear the decks for action
            // note: there are a bunch of pointers in FLAT_UNION_T but no classes. memset OK

            return retVal;
        }

        public uint attrMask;

        public DDL6BaseItem()
        {
            attrMask = 0;
        }// pure virtual func

        public virtual int eval_attrs()// RPVFC( "DDL6BaseItem_B",0 );
        {
            return 0;
        }

        public virtual void clear_flat()// PVFC( "DDL6BaseItem_C" );
        {
            return;
        }

    }

    public class DDl6Variable : DDL6BaseItem      /*Item Type == 1*/
    {
        //FLAT_VAR* pVar;

        public const int VAR_ATTR_MASKS = 0x0fffbfff;//??????

        public const int VAR_CLASS_ID = 0;
        public const int VAR_HANDLING_ID = 1;
        public const int VAR_UNIT_ID = 2;
        public const int VAR_LABEL_ID = 3;
        public const int VAR_HELP_ID = 4;
        public const int VAR_WIDTHSIZE_ID = 5; /* number reuse - wap 11sep12 */
        public const int VAR_HEIGHTSIZE_ID = 6;    /* number reuse - wap 11sep12 */
        public const int VAR_VALID_ID = 7;
        public const int VAR_PRE_READ_ACT_ID = 8;
        public const int VAR_POST_READ_ACT_ID = 9;
        public const int VAR_PRE_WRITE_ACT_ID = 10;
        public const int VAR_POST_WRITE_ACT_ID = 11;
        public const int VAR_PRE_EDIT_ACT_ID = 12;
        public const int VAR_POST_EDIT_ACT_ID = 13;
        public const int VAR_RESP_CODES_ID = 14;
        public const int VAR_TYPE_SIZE_ID = 15;
        public const int VAR_DISPLAY_ID = 16;
        public const int VAR_EDIT_ID = 17;
        public const int VAR_MIN_VAL_ID = 18;
        public const int VAR_MAX_VAL_ID = 19;
        public const int VAR_SCALE_ID = 20;
        public const int VAR_ENUMS_ID = 21;
        public const int VAR_INDEX_ITEM_ARRAY_ID = 22;
        public const int VAR_DEFAULT_VALUE_ID = 23;
        public const int VAR_REFRESH_ACT_ID = 24;
        public const int VAR_DEBUG_ID = 25;

        public const int VAR_POST_RQST_ACT_ID = 26;
        public const int VAR_POST_USER_ACT_ID = 27;

        public const int VAR_TIME_FORMAT_ID = 28;
        public const int VAR_TIME_SCALE_ID = 29;
        public const int VAR_VISIBLE_ID = 30;
        public const int VAR_PRIVATE_ID = 31;
        public const int MAX_VAR_ID = 32;  /*must be last in list of 0 - 31 */

        public const uint VAR_CLASS = (1 << VAR_CLASS_ID);
        public const uint VAR_HANDLING = (1 << VAR_HANDLING_ID);
        public const uint VAR_UNIT = (1 << VAR_UNIT_ID);
        public const uint VAR_LABEL = (1 << VAR_LABEL_ID);
        public const uint VAR_HELP = (1 << VAR_HELP_ID);
        public const uint VAR_WIDTHSIZE = (1 << VAR_WIDTHSIZE_ID);
        public const uint VAR_HEIGHTSIZE = (1 << VAR_HEIGHTSIZE_ID);

        public const uint VAR_HEIGHT = (1 << VAR_HEIGHTSIZE_ID);
        public const uint VAR_WIDTH = (1 << VAR_WIDTHSIZE_ID);
        public const uint VAR_VALID = (1 << VAR_VALID_ID);

        public const uint VAR_PRE_READ_ACT = (1 << VAR_PRE_READ_ACT_ID);
        public const uint VAR_POST_READ_ACT = (1 << VAR_POST_READ_ACT_ID);
        public const uint VAR_PRE_WRITE_ACT = (1 << VAR_PRE_WRITE_ACT_ID);
        public const uint VAR_POST_WRITE_ACT = (1 << VAR_POST_WRITE_ACT_ID);

        public const uint VAR_PRE_EDIT_ACT = (1 << VAR_PRE_EDIT_ACT_ID);
        public const uint VAR_POST_EDIT_ACT = (1 << VAR_POST_EDIT_ACT_ID);
        public const uint VAR_RESP_CODES = (1 << VAR_RESP_CODES_ID);
        public const uint VAR_TYPE_SIZE = (1 << VAR_TYPE_SIZE_ID);

        public const uint VAR_DISPLAY = (1 << VAR_DISPLAY_ID);
        public const uint VAR_EDIT = (1 << VAR_EDIT_ID);
        public const uint VAR_MIN_VAL = (1 << VAR_MIN_VAL_ID);
        public const uint VAR_MAX_VAL = (1 << VAR_MAX_VAL_ID);

        public const uint VAR_SCALE = (1 << VAR_SCALE_ID);
        public const uint VAR_ENUMS = (1 << VAR_ENUMS_ID);
        public const uint VAR_INDEX_ITEM_ARRAY = (1 << VAR_INDEX_ITEM_ARRAY_ID);
        public const uint VAR_DEFAULT_VALUE = (1 << VAR_DEFAULT_VALUE_ID);
        public const uint VAR_REFRESH_ACT = (1 << VAR_REFRESH_ACT_ID);
        public const uint VAR_DEBUG = (1 << VAR_DEBUG_ID);

        public const uint VAR_POST_RQST_ACT = (1 << VAR_POST_RQST_ACT_ID);
        public const uint VAR_POST_USER_ACT = (1 << VAR_POST_USER_ACT_ID);

        public const uint VAR_TIME_FORMAT = (1 << VAR_TIME_FORMAT_ID);// timj 27dec07
        public const uint VAR_TIME_SCALE = (1 << VAR_TIME_SCALE_ID);    // timj 27dec07


        public const uint INVALID_VAR_TYPE_SUBATTR_MASK = ~(VAR_DISPLAY | VAR_EDIT | VAR_MIN_VAL | VAR_MAX_VAL | VAR_SCALE | VAR_ENUMS | VAR_INDEX_ITEM_ARRAY | VAR_TIME_FORMAT | VAR_TIME_SCALE);

        public const uint INVALID_ARITH_TYPE_SUBATTR_MASK = ~(VAR_ENUMS | VAR_INDEX_ITEM_ARRAY | VAR_TIME_FORMAT | VAR_TIME_SCALE);

        public const uint INVALID_ENUM_TYPE_SUBATTR_MASK = ~(VAR_DISPLAY | VAR_EDIT | VAR_MIN_VAL | VAR_MAX_VAL | VAR_SCALE | VAR_INDEX_ITEM_ARRAY | VAR_TIME_FORMAT | VAR_TIME_SCALE);

        public const uint INVALID_STRING_TYPE_SUBATTR_MASK = ~(VAR_DISPLAY | VAR_EDIT | VAR_MIN_VAL | VAR_MAX_VAL | VAR_SCALE | VAR_ENUMS | VAR_INDEX_ITEM_ARRAY | VAR_TIME_FORMAT | VAR_TIME_SCALE);

        public const uint INVALID_INDEX_TYPE_SUBATTR_MASK = ~(VAR_DISPLAY | VAR_EDIT | VAR_MIN_VAL | VAR_MAX_VAL | VAR_SCALE | VAR_ENUMS | VAR_TIME_FORMAT | VAR_TIME_SCALE);

        public const uint INVALID_DATE_TIME_TYPE_SUBATTR_MASK = ~(VAR_DISPLAY | VAR_EDIT | VAR_MIN_VAL | VAR_MAX_VAL | VAR_SCALE | VAR_ENUMS | VAR_INDEX_ITEM_ARRAY | VAR_TIME_FORMAT | VAR_TIME_SCALE);

        public const uint INVALID_TIME_VALUE_TYPE_SUBATTR_MASK = ~(VAR_MIN_VAL | VAR_MAX_VAL | VAR_SCALE | VAR_ENUMS | VAR_INDEX_ITEM_ARRAY);


        public DDl6Variable()
        {
            byItemType = VARIABLE_ITYPE;
            strItemName = "Variable";
            //pVar = &glblFlats.fVar; 
        }

        public override void AllocAttributes()//version 5 alloc
        {
            DDlAttribute pDDlAttr = new DDlAttribute("VarClass", VAR_CLASS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

            attrList.Add(pDDlAttr);

            /*	if(attrMask & VAR_TYPE_SIZE)
                { */

            /*Moved TYPE_SIZE attribute in to eval_variable*/
            /*		pDDlAttr = new DDlAttribute("attrVarTypeSize",
                                                    VAR_TYPE_SIZE_ID,
                                                    DDL_ATTR_DATA_TYPE_TYPE_SIZE,
                                                    false);

                    attrList.Add(pDDlAttr); */

            /*	} */

            /*	if(attrMask & VAR_CLASS)
                { */
            /*Type is a mandatory attribute and should be allocated anyway*/

            /*	} */

            /*Vibhor 141103 : Moving Handling to the beginning of the attribute list,
             So we will parse Handling in Eval as the first attribute*/
            /*	if(attrMask & VAR_HANDLING)
                {

                    pDDlAttr = new DDlAttribute("VarHandling",
                                                    VAR_HANDLING_ID,
                                                    DDL_ATTR_DATA_TYPE_BITSTRING,
                                                    false);

                    attrList.Add(pDDlAttr);

                }
            */

            if ((attrMask & VAR_LABEL) > 0)
            {

                pDDlAttr = new DDlAttribute("VarLabel", VAR_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }




            if ((attrMask & VAR_HELP) > 0)
            {

                pDDlAttr = new DDlAttribute("VarHelp", VAR_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_DISPLAY) > 0)
            {

                pDDlAttr = new DDlAttribute("VarDisplayFormat", VAR_DISPLAY_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_EDIT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarEditFormat", VAR_EDIT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_ENUMS) > 0)
            {

                pDDlAttr = new DDlAttribute("VarEnums", VAR_ENUMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ENUM_LIST, false);

                attrList.Add(pDDlAttr);

            }



            if ((attrMask & VAR_UNIT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarConstantUnit", VAR_UNIT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            /* removed 15oct12
                if((attrMask & VAR_WIDTHSIZE) > 0)// used to be read timeout
                {

                    pDDlAttr = new DDlAttribute("VarWidth", VAR_WIDTHSIZE_ID, DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);

                    attrList.Add(pDDlAttr);

                }

                if((attrMask & VAR_HEIGHTSIZE) > 0)
                {

                    pDDlAttr = new DDlAttribute("VarHeight", VAR_HEIGHTSIZE_ID, DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);

                    attrList.Add(pDDlAttr);

                }
            ****/

            /*	attrMask & VAR_RESP_CODES)
                {

                    pDDlAttr = new DDlAttribute("VarResponseCodes", VAR_RESP_CODES_ID, DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                    attrList.Add(pDDlAttr);

                }  */

            if ((attrMask & VAR_MIN_VAL) > 0)
            {

                pDDlAttr = new DDlAttribute("VarMinVal", VAR_MIN_VAL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MIN_MAX,/*This needs to be taken care of*/ false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_MAX_VAL) > 0)
            {

                pDDlAttr = new DDlAttribute("VarMaxVal", VAR_MAX_VAL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MIN_MAX,/*This needs to be taken care of*/ false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_SCALE) > 0)
            {

                pDDlAttr = new DDlAttribute("VarScalingFactor", VAR_SCALE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);

                attrList.Add(pDDlAttr);

            }



            if ((attrMask & VAR_INDEX_ITEM_ARRAY) > 0)
            {

                pDDlAttr = new DDlAttribute("VarIndexItemArrayName", VAR_INDEX_ITEM_ARRAY_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_PRE_READ_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPreReadActions", VAR_PRE_READ_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & VAR_POST_READ_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPostReadActions", VAR_POST_READ_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & VAR_PRE_WRITE_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPreWriteActions", VAR_PRE_WRITE_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_POST_WRITE_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPostWriteActions", VAR_POST_WRITE_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & VAR_PRE_EDIT_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPreEditActions", VAR_PRE_EDIT_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_POST_EDIT_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPostEditActions", VAR_POST_EDIT_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }


            /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
            /*After Defining all move this guy at the end*/
            /*We will allocate Validity if mask contains it;
            If not, then we will default it after parsing all other attributes in
            eval_variable*/

            if ((attrMask & VAR_VALID) > 0)
            {

                pDDlAttr = new DDlAttribute("VarValidity", VAR_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                /*	pDDlAttr.pVals.ulVal = 0x01L; /*Default Attribute*/

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_DEFAULT_VALUE) > 0)
            {

                pDDlAttr = new DDlAttribute("VarDefaultVal", VAR_DEFAULT_VALUE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);

                attrList.Add(pDDlAttr);

            }
            /*Vibhor 030904: End of Code*/

            /* stevev 10may05 */

            if ((attrMask & VAR_REFRESH_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarRefreshActions", VAR_REFRESH_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & VAR_DEBUG) > 0)
            {

                pDDlAttr = new DDlAttribute("VarDebugData", VAR_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);//  db_debug_info	4 + 1

                attrList.Add(pDDlAttr);

            }
            //#ifdef XMTR	
            if ((attrMask & VAR_POST_RQST_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPostRequestActions", VAR_POST_RQST_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & VAR_POST_USER_ACT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarPostUserChangeActions", VAR_POST_USER_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }
            //#endif

            /* end stevev 10may05 */


            if ((attrMask & VAR_TIME_FORMAT) > 0)
            {

                pDDlAttr = new DDlAttribute("VarTimeFormat", VAR_TIME_FORMAT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & VAR_TIME_SCALE) > 0)
            {

                pDDlAttr = new DDlAttribute("VarTimeScale", VAR_TIME_SCALE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

                attrList.Add(pDDlAttr);

            }

            /*Moved TYPE_SIZE attribute in to eval_variable*/

            /*Vibhor 141103 : Moving Handling to the beginning of the attribute list, So we will parse Handling in Eval as the first attribute*/


            return;
        }


        public override void AllocAttributes(uint attrMask)
        {
            ;
        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            //BYTE* pbyLocalAttrOffset;
            uint pbyLocalAttrOffset = 0;
            //BYTE* pbyItemExtn = pbyObjExtn;// internal iterator
            int iAttrLength = 0;
            DDlBaseItem di = this;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);
            id = di.id;

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            glblFlats.fVar.masks.bin_exists = attrMask & VAR_ATTR_MASKS;
            glblFlats.fVar.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        unsafe public static int attach_var_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_VAR var_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case VAR_UNIT_ID:
                /* removed 15oct12
                    case VAR_READ_TIME_OUT_ID:
                    case VAR_WRITE_TIME_OUT_ID:
                    ***/
                case VAR_WIDTHSIZE_ID:
                case VAR_HEIGHTSIZE_ID:

                case VAR_VALID_ID:
                case VAR_MIN_VAL_ID:
                case VAR_MAX_VAL_ID:
                case VAR_SCALE_ID:
                case VAR_DEBUG_ID:
                case VAR_TIME_FORMAT_ID:                                    // timj 4jan07 added
                case VAR_TIME_SCALE_ID:                                 // timj 4jan07 added
                                                                        //	ASSERT_RET(var_flat.misc, FETCH_NULL_POINTER);
                    if ((object)var_flat.misc == null)
                    {
                        var_flat.misc = new DDL6Item.FLAT_VAR_MISC();

                        /*
                         * Force the DEPBIN pointer array to all 0's
                         */
                    }

                    if ((object)var_flat.misc.depbin == null)
                    {

                        var_flat.misc.depbin = new DDL6Item.VAR_MISC_DEPBIN();

                        /*
                         * Force the DEPBIN pointer array to all 0's
                         */
                    }
                    break;

                case VAR_PRE_READ_ACT_ID:
                case VAR_POST_READ_ACT_ID:
                case VAR_PRE_WRITE_ACT_ID:
                case VAR_POST_WRITE_ACT_ID:
                case VAR_PRE_EDIT_ACT_ID:
                case VAR_POST_EDIT_ACT_ID:
                case VAR_REFRESH_ACT_ID:
                //#ifdef XMTR												// timj
                case VAR_POST_RQST_ACT_ID:
                case VAR_POST_USER_ACT_ID:
                    //#endif

                    //	ASSERT_RET(var_flat.actions, FETCH_NULL_POINTER);
                    if ((object)var_flat.actions == null)
                    {
                        var_flat.actions = new DDL6Item.FLAT_VAR_ACTIONS();

                        /*
                         * Force the actions pointer array to all 0's
                         */

                    }

                    if ((object)var_flat.actions.depbin == null)
                    {

                        var_flat.actions.depbin = new DDL6Item.VAR_ACTIONS_DEPBIN();

                        /*
                         * Force the DEPBIN pointer array to all 0's
                         */

                    }
                    break;

                default:
                    if ((object)(var_flat.depbin) == null)
                    {

                        var_flat.depbin = new DDL6Item.VAR_DEPBIN();

                    }
                    break;

            }

            /*
             * Select the type of attribute and attach the address in the scratchpad
             * to the corresponding DEPBIN pointer in the flat structure.  If the
             * DEPBIN structure pointer is null, reserve the space for it on the
             * scratchpad and set the pointer in the DEPBIN array.
             */

            switch (tag)
            {

                case VAR_CLASS_ID:
                    var_flat.depbin.db_class.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_class.bin_size = data_len;
                    var_flat.depbin.db_class.bin_offset = attr_offset;
                    break;
                case VAR_HANDLING_ID:
                    //depbin_ptr = var_flat.depbin.db_handling;
                    var_flat.depbin.db_handling.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_handling.bin_size = data_len;
                    var_flat.depbin.db_handling.bin_offset = attr_offset;
                    break;
                case VAR_UNIT_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_unit;
                    var_flat.misc.depbin.db_unit.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_unit.bin_size = data_len;
                    var_flat.misc.depbin.db_unit.bin_offset = attr_offset;
                    break;
                case VAR_LABEL_ID:
                    //depbin_ptr = var_flat.depbin.db_label;
                    var_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_label.bin_size = data_len;
                    var_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case VAR_HELP_ID:
                    //depbin_ptr = var_flat.depbin.db_help;
                    var_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_help.bin_size = data_len;
                    var_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                /*  removed 15oct12
                    case VAR_READ_TIME_OUT_ID:
                        depbin_ptr = var_flat.misc.depbin.db_read_time_out;
                        break;
                    case VAR_WRITE_TIME_OUT_ID:
                        depbin_ptr = var_flat.misc.depbin.db_write_time_out;
                        break;
                ***/
                case VAR_WIDTHSIZE_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_height;
                    var_flat.misc.depbin.db_height.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_height.bin_size = data_len;
                    var_flat.misc.depbin.db_height.bin_offset = attr_offset;
                    break;
                case VAR_HEIGHTSIZE_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_width;
                    var_flat.misc.depbin.db_width.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_width.bin_size = data_len;
                    var_flat.misc.depbin.db_width.bin_offset = attr_offset;
                    break;
                case VAR_VALID_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_valid;
                    var_flat.misc.depbin.db_valid.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_valid.bin_size = data_len;
                    var_flat.misc.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case VAR_PRE_READ_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_pre_read_act;
                    var_flat.actions.depbin.db_pre_read_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_pre_read_act.bin_size = data_len;
                    var_flat.actions.depbin.db_pre_read_act.bin_offset = attr_offset;
                    break;
                case VAR_POST_READ_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_post_read_act;
                    var_flat.actions.depbin.db_post_read_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_post_read_act.bin_size = data_len;
                    var_flat.actions.depbin.db_post_read_act.bin_offset = attr_offset;
                    break;
                case VAR_PRE_WRITE_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_pre_write_act;
                    var_flat.actions.depbin.db_pre_write_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_pre_write_act.bin_size = data_len;
                    var_flat.actions.depbin.db_pre_write_act.bin_offset = attr_offset;
                    break;
                case VAR_POST_WRITE_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_post_write_act;
                    var_flat.actions.depbin.db_post_write_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_post_write_act.bin_size = data_len;
                    var_flat.actions.depbin.db_post_write_act.bin_offset = attr_offset;
                    break;
                case VAR_PRE_EDIT_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_pre_edit_act;
                    var_flat.actions.depbin.db_pre_edit_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_pre_edit_act.bin_size = data_len;
                    var_flat.actions.depbin.db_pre_edit_act.bin_offset = attr_offset;
                    break;
                case VAR_POST_EDIT_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_post_edit_act;
                    var_flat.actions.depbin.db_post_edit_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_post_edit_act.bin_size = data_len;
                    var_flat.actions.depbin.db_post_edit_act.bin_offset = attr_offset;
                    break;
                case VAR_REFRESH_ACT_ID:
                    //depbin_ptr = var_flat.actions.depbin.db_refresh_act;
                    var_flat.actions.depbin.db_refresh_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_refresh_act.bin_size = data_len;
                    var_flat.actions.depbin.db_refresh_act.bin_offset = attr_offset;
                    break;
                case VAR_RESP_CODES_ID:
                    //depbin_ptr = var_flat.depbin.db_resp_codes;
                    var_flat.depbin.db_resp_codes.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_resp_codes.bin_size = data_len;
                    var_flat.depbin.db_resp_codes.bin_offset = attr_offset;
                    break;
                case VAR_TYPE_SIZE_ID:
                    //depbin_ptr = var_flat.depbin.db_type_size;
                    var_flat.depbin.db_type_size.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_type_size.bin_size = data_len;
                    var_flat.depbin.db_type_size.bin_offset = attr_offset;
                    break;
                case VAR_DISPLAY_ID:
                    //depbin_ptr = var_flat.depbin.db_display;
                    var_flat.depbin.db_display.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_display.bin_size = data_len;
                    var_flat.depbin.db_display.bin_offset = attr_offset;
                    break;
                case VAR_EDIT_ID:
                    //depbin_ptr = var_flat.depbin.db_edit;
                    var_flat.depbin.db_edit.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_edit.bin_size = data_len;
                    var_flat.depbin.db_edit.bin_offset = attr_offset;
                    break;
                case VAR_MIN_VAL_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_min_val;
                    var_flat.misc.depbin.db_min_val.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_min_val.bin_size = data_len;
                    var_flat.misc.depbin.db_min_val.bin_offset = attr_offset;
                    break;
                case VAR_MAX_VAL_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_max_val;
                    var_flat.misc.depbin.db_max_val.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_max_val.bin_size = data_len;
                    var_flat.misc.depbin.db_max_val.bin_offset = attr_offset;
                    break;
                case VAR_SCALE_ID:
                    //depbin_ptr = var_flat.misc.depbin.db_scale;
                    var_flat.misc.depbin.db_scale.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_scale.bin_size = data_len;
                    var_flat.misc.depbin.db_scale.bin_offset = attr_offset;
                    break;
                case VAR_ENUMS_ID:
                    //depbin_ptr = var_flat.depbin.db_enums;
                    var_flat.depbin.db_enums.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_enums.bin_size = data_len;
                    var_flat.depbin.db_enums.bin_offset = attr_offset;
                    break;
                case VAR_INDEX_ITEM_ARRAY_ID:
                    //depbin_ptr = var_flat.depbin.db_index_item_array;
                    var_flat.depbin.db_index_item_array.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_index_item_array.bin_size = data_len;
                    var_flat.depbin.db_index_item_array.bin_offset = attr_offset;
                    break;
                case VAR_DEFAULT_VALUE_ID:                          //Vibhor 280904: Added
                    //depbin_ptr = var_flat.depbin.db_default_value;
                    var_flat.depbin.db_default_value.bin_chunk = attr_data_ptr;
                    var_flat.depbin.db_default_value.bin_size = data_len;
                    var_flat.depbin.db_default_value.bin_offset = attr_offset;
                    break;
                //#ifdef XMTR
                case VAR_POST_RQST_ACT_ID:          //stevev 21feb05: Added  // timj 26dec07 no longer required
                    //depbin_ptr = var_flat.actions.depbin.db_post_rqst_act;
                    var_flat.actions.depbin.db_post_rqst_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_post_rqst_act.bin_size = data_len;
                    var_flat.actions.depbin.db_post_rqst_act.bin_offset = attr_offset;
                    break;
                case VAR_POST_USER_ACT_ID:                          //stevev 21feb05: Added
                    //depbin_ptr = var_flat.actions.depbin.db_post_user_act;
                    var_flat.actions.depbin.db_post_user_act.bin_chunk = attr_data_ptr;
                    var_flat.actions.depbin.db_post_user_act.bin_size = data_len;
                    var_flat.actions.depbin.db_post_user_act.bin_offset = attr_offset;
                    break;
                //#endif
                case VAR_DEBUG_ID:                                  // stevev 06may05: addedAdded
                    //depbin_ptr = var_flat.misc.depbin.db_debug_info;
                    var_flat.misc.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_debug_info.bin_size = data_len;
                    var_flat.misc.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                case VAR_TIME_FORMAT_ID:                                    // timj 26dec07 added
                    //depbin_ptr = var_flat.misc.depbin.db_time_format;
                    var_flat.misc.depbin.db_time_format.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_time_format.bin_size = data_len;
                    var_flat.misc.depbin.db_time_format.bin_offset = attr_offset;
                    break;
                case VAR_TIME_SCALE_ID:                                     // timj 26dec07 added
                    //depbin_ptr = var_flat.misc.depbin.db_time_scale;
                    var_flat.misc.depbin.db_time_scale.bin_chunk = attr_data_ptr;
                    var_flat.misc.depbin.db_time_scale.bin_size = data_len;
                    var_flat.misc.depbin.db_time_scale.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_VAR_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }           /* end switch */

            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            var_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }



        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_var_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fVar, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = attr_offset;//+ i 

                                rcode = attach_var_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fVar, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }
        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            DDlAttribute pAttribute = null;

            /*Vibhor 141103: Since HANDLING is not affected by TypeSize attribute & 
             for some efficiency we need it at the beginning of the list , so parsing
             it as the first attribute!!!*/

            if ((attrMask & VAR_HANDLING) > 0)
            {

                pAttribute = new DDlAttribute("VarHandling", VAR_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

                ulChunkSize = glblFlats.fVar.depbin.db_handling.bin_size;
                AttrChunkPtr = glblFlats.fVar.depbin.db_handling.bin_chunk;
                rc = Common.parse_attr_bitstring(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_handling.bin_offset);
                if (rc != Common.SUCCESS)
                    return rc;
                attrList.Add(pAttribute);

            }
            else
            {
                pAttribute = new DDlAttribute("VarHandling", VAR_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

                //pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = Common.READ_HANDLING | Common.WRITE_HANDLING; /*Default Attribute*/

                attrList.Add(pAttribute);

            }

            /*Since some attributes become invalid depending upon the "type" attribute
             of the variable , we will evaluate it first and restructure the attribute
             list , if required ( by deleting the invalid attributes*/

            /*The first attribute is always TYPE_SIZE*/
            //pAttribute = glblFlats.fVar.attrList.begin();

            pAttribute = new DDlAttribute("VarTypeSize", VAR_TYPE_SIZE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_TYPE_SIZE, false);

            AttrChunkPtr = glblFlats.fVar.depbin.db_type_size.bin_chunk;
            ulChunkSize = glblFlats.fVar.depbin.db_type_size.bin_size;

            rc = Element.parse_attr_type_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_type_size.bin_offset);

            if (rc != Common.SUCCESS)
                return rc;

            //uint masktemp = (uint)0xffffffff;

            switch (pAttribute.pVals.typeSize.type)
            {
                case Attribute.INTEGER:
                case Attribute.UNSIGNED:
                case Attribute.FLOATG_PT:
                case Attribute.DOUBLE_FLOAT:
                    attrMask &= DDl6Variable.INVALID_ARITH_TYPE_SUBATTR_MASK;
                    break;

                case Attribute.ENUMERATED:
                case Attribute.BIT_ENUMERATED:
                    attrMask &= DDl6Variable.INVALID_ENUM_TYPE_SUBATTR_MASK;
                    break;

                case Attribute.INDEX:
                    attrMask &= DDl6Variable.INVALID_INDEX_TYPE_SUBATTR_MASK;
                    break;

                //FF			case EUC:
                case Attribute.ASCII:
                case Attribute.PACKED_ASCII:
                case Attribute.PASSWORD:
                    //FF			case BITSTRING:
                    attrMask &= DDl6Variable.INVALID_STRING_TYPE_SUBATTR_MASK;
                    break;

                case Attribute.HART_DATE_FORMAT:
                    //FF			case TIME:
                    //FF			case DATE_AND_TIME:
                    //FF			case DURATION:
                    attrMask &= DDl6Variable.INVALID_DATE_TIME_TYPE_SUBATTR_MASK;
                    break;

                case Attribute.TIME_VALUE:    // timj 26dec07
                    attrMask &= DDl6Variable.INVALID_TIME_VALUE_TYPE_SUBATTR_MASK;
                    break;

                default:    /* should never happen */
                    attrMask &= DDl6Variable.INVALID_VAR_TYPE_SUBATTR_MASK;
                    break;
            } /*End Switch pAttribute.pVals.typeSize.type */

            //attrMask = (uint)masktemp;

            /*Now push the TYPE_SIZE attribute onto the Attribute List*/
            attrList.Add(pAttribute);

            /*Now pass the updated attribute mask to allocate the attributes*/
            AllocAttributes();

            /*Now we have the Attribute list for this variable ready !!!
             Just iterate through it and parse each individual attribute
             Note : We have already parsed the first attribute ie. TYPE_SIZE
             so we'll start from the second attribute*/

            //ItemAttrList::iterator p;

            //for (p = (attrList.begin()) + 1; p != attrList.end(); p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case VAR_CLASS_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_class.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_class.bin_size;
                            rc = Common.parse_attr_bitstring(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_class.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;
                    /*			case	VAR_HANDLING_ID:
                                    {
                                        AttrChunkPtr = glblFlats.fVar.depbin.db_handling.bin_chunk;
                                        ulChunkSize = glblFlats.fVar.depbin.db_handling.bin_size;
                                        rc = parse_attr_bitstring(ref pAttribute,AttrChunkPtr,ulChunkSize);
                                        if(rc != Common.SUCCESS)
                                            return rc;
                                    }
                                    break;
                    */
                    case VAR_UNIT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_unit.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_unit.bin_size;
                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.misc.depbin.db_unit.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;

                    case VAR_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_label.bin_size;
                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_label.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_help.bin_size;
                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_help.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    /*** height and width will n0t be in a version 6 device
                                case	VAR_READ_TIME_OUT_ID:
                                    {	
                                        AttrChunkPtr = glblFlats.fVar.misc.depbin.db_read_time_out.bin_chunk;
                                        ulChunkSize = glblFlats.fVar.misc.depbin.db_read_time_out.bin_size;
                                        //here we have to parse a constant integral value from an expression;
                                        //Need to define an API for this
                                        rc = parse_attr_expr(ref pAttribute,AttrChunkPtr,ulChunkSize);
                                        if(rc != Common.SUCCESS)
                                            return rc;					
                                    }
                                    break;

                                case	VAR_WRITE_TIME_OUT_ID:
                                    {	
                                        AttrChunkPtr = glblFlats.fVar.misc.depbin.db_write_time_out.bin_chunk;
                                        ulChunkSize = glblFlats.fVar.misc.depbin.db_write_time_out.bin_size;
                                        //here we have to parse a constant integral value from an expression;
                                        //Need to define an API for this
                                        rc = parse_attr_expr(ref pAttribute,AttrChunkPtr,ulChunkSize);
                                        if(rc != Common.SUCCESS)
                                            return rc; 

                                    }
                                    break;
                    ****/
                    case VAR_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_valid.bin_size;
                            rc = Common.parse_attr_int(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.misc.depbin.db_valid.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_PRE_READ_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_pre_read_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_pre_read_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_pre_read_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_POST_READ_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_post_read_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_post_read_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_post_read_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_PRE_WRITE_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_pre_write_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_pre_write_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_pre_write_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_POST_WRITE_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_post_write_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_post_write_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_post_write_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_PRE_EDIT_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_pre_edit_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_pre_edit_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_pre_edit_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_POST_EDIT_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_post_edit_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_post_edit_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_post_edit_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_REFRESH_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_refresh_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_refresh_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_refresh_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    //#ifdef XMTR
                    case VAR_POST_RQST_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_post_rqst_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_post_rqst_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_post_rqst_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_POST_USER_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.actions.depbin.db_post_user_act.bin_chunk;
                            ulChunkSize = glblFlats.fVar.actions.depbin.db_post_user_act.bin_size;
                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.actions.depbin.db_post_user_act.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    //#endif /*XMTR*/
                    /*			case	VAR_RESP_CODES_ID:
                                    {	
                                        AttrChunkPtr = glblFlats.fVar.depbin.db_resp_codes.bin_chunk;
                                        ulChunkSize = glblFlats.fVar.depbin.db_resp_codes.bin_size;
                                        rc = parse_attr_ulong(ref pAttribute,AttrChunkPtr,ulChunkSize);
                                        if(rc != Common.SUCCESS)
                                            return rc; 

                                    }
                                    break; */
                    case VAR_DISPLAY_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_display.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_display.bin_size;
                            rc = Common.parse_attr_disp_edit_format(ref pAttribute, ref AttrChunkPtr, ulChunkSize, Common.DISPLAY_FORMAT_TAG, glblFlats.fVar.depbin.db_display.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_EDIT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_edit.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_edit.bin_size;
                            rc = Common.parse_attr_disp_edit_format(ref pAttribute, ref AttrChunkPtr, ulChunkSize, Common.EDIT_FORMAT_TAG, glblFlats.fVar.depbin.db_edit.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;
                    case VAR_MIN_VAL_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_min_val.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_min_val.bin_size;
                            rc = Common.parse_attr_min_max_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, Common.MIN_VALUE_TAG, glblFlats.fVar.misc.depbin.db_min_val.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_MAX_VAL_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_max_val.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_max_val.bin_size;
                            rc = Common.parse_attr_min_max_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, Common.MAX_VALUE_TAG, glblFlats.fVar.misc.depbin.db_max_val.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_SCALE_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_scale.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_scale.bin_size;
                            rc = Common.parse_attr_scaling_factor(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.misc.depbin.db_scale.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;
                    case VAR_ENUMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_enums.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_enums.bin_size;
                            rc = Common.parse_attr_enum_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_enums.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_INDEX_ITEM_ARRAY_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_index_item_array.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_index_item_array.bin_size;
                            rc = Common.parse_attr_array_name(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_index_item_array.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case VAR_DEFAULT_VALUE_ID:      //Vibhor 030904: Start of Code
                        {
                            AttrChunkPtr = glblFlats.fVar.depbin.db_default_value.bin_chunk;
                            ulChunkSize = glblFlats.fVar.depbin.db_default_value.bin_size;
                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.depbin.db_default_value.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;
                    case VAR_DEBUG_ID:      //stevev 11may05
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_debug_info.bin_size;
                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.misc.depbin.db_debug_info.bin_offset);
                            if (rc != Common.SUCCESS)  // 4
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;  // here memory leak PAW 09/04/09
                        }
                        break;
                    /* handled elsewhere - but recognized */
                    case VAR_HANDLING_ID:
                    case VAR_TYPE_SIZE_ID:
                        break;

                    case VAR_TIME_FORMAT_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_time_format.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_time_format.bin_size;
                            rc = Common.parse_attr_disp_edit_format(ref pAttribute, ref AttrChunkPtr, ulChunkSize, Common.TIME_FORMAT_TAG, glblFlats.fVar.misc.depbin.db_time_format.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    case VAR_TIME_SCALE_ID:
                        {
                            AttrChunkPtr = glblFlats.fVar.misc.depbin.db_time_scale.bin_chunk;
                            ulChunkSize = glblFlats.fVar.misc.depbin.db_time_scale.bin_size;
                            rc = Common.parse_attr_time_scale(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fVar.misc.depbin.db_time_scale.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    default:
                        /*Should Never Reach here!!!!*/
                        break;
                }/*End Switch*/
                attrList[i] = pAttribute;
            }/*End for*/

            /*Vibhor 271003: Though Handling is an optional attribute, but wee need it for display processing & by definition a 
             variable without a Handling can be both read & written, so we will default this value to READ & WRITE if not already there*/

            /*	 if (!(attrMask & VAR_HANDLING))
                 {
                    pAttribute = new DDlAttribute("VarHandling", VAR_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

                    pAttribute.pVals = new VALUES;

                    pAttribute.pVals.ulVal = READ_HANDLING | WRITE_HANDLING; 

                    glblFlats.fVar.attrList.Add(pAttribute);
                 }
            */
            /*Just check if we got the validity attribute from the binary , if not then allocate it and default it to true & push it on the 
             attrList */

            if ((attrMask & VAR_VALID) == 0)
            {
                pAttribute = new DDlAttribute("VarValidity", VAR_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }

            // be sure they are set
            attrMask = attrMask | VAR_CLASS | VAR_TYPE_SIZE | VAR_HANDLING | VAR_VALID;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }
        public override void clear_flat()
        {
            ;
        }

    }


    public class DDl6Command : DDL6BaseItem            /*Item Type == 2*/
    {
        //DDL6Item.FLAT_COMMAND pCmd = glblFlats.fCmd;

        /* COMMAND attributes SIZE 1 */

        public const int COMMAND_NUMBER_ID = 0;
        public const int COMMAND_OPER_ID = 1;
        public const int COMMAND_TRANS_ID = 2;
        public const int COMMAND_RESP_CODES_ID = 3;
        public const int COMMAND_DEBUG_ID = 4;
        public const int MAX_COMMAND_ID = 5;   /* must be last in list */

        /* COMMAND attribute masks */

        public const int COMMAND_NUMBER = (1 << COMMAND_NUMBER_ID);
        public const int COMMAND_OPER = (1 << COMMAND_OPER_ID);
        public const int COMMAND_TRANS = (1 << COMMAND_TRANS_ID);
        public const int COMMAND_RESP_CODES = (1 << COMMAND_RESP_CODES_ID);
        public const int COMMAND_DEBUG = (1 << COMMAND_DEBUG_ID);

        public const int COMMAND_ATTR_MASKS = (COMMAND_NUMBER | COMMAND_OPER | COMMAND_TRANS | COMMAND_RESP_CODES | COMMAND_DEBUG);
        public DDl6Command()
        {
            byItemType = COMMAND_ITYPE;
            strItemName = "Command";
            //pCmd = &(glblFlats.fCmd); 
        }

        public override void AllocAttributes()
        {
            //Modified by Deepak
            DDlAttribute pDDlAttr;// = NULL;
            if ((attrMask & COMMAND_NUMBER) != 0)
            {

                pDDlAttr = new DDlAttribute("CmdNumber", COMMAND_NUMBER_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & COMMAND_OPER) != 0)
            {

                pDDlAttr = new DDlAttribute("CmdOperationType", COMMAND_OPER_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & COMMAND_TRANS) != 0)
            {

                pDDlAttr = new DDlAttribute("CmdTransaction", COMMAND_TRANS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_TRANSACTION_LIST, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & COMMAND_RESP_CODES) != 0)
            {
                pDDlAttr = new DDlAttribute("CmdResponseCodes", COMMAND_RESP_CODES_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_RESPONSE_CODE_LIST, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & COMMAND_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("CmdDebugData", COMMAND_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

        }

        public static int attach_command_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_COMMAND command_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case COMMAND_NUMBER_ID:
                    //depbin_ptr = &command_flat.depbin.db_number;
                    command_flat.depbin.db_number.bin_chunk = attr_data_ptr;
                    command_flat.depbin.db_number.bin_size = data_len;
                    command_flat.depbin.db_number.bin_offset = attr_offset;
                    break;
                case COMMAND_OPER_ID:
                    //depbin_ptr = &command_flat.depbin.db_oper;
                    command_flat.depbin.db_oper.bin_chunk = attr_data_ptr;
                    command_flat.depbin.db_oper.bin_size = data_len;
                    command_flat.depbin.db_oper.bin_offset = attr_offset;
                    break;

                case COMMAND_TRANS_ID:
                    //depbin_ptr = &command_flat.depbin.db_trans;
                    command_flat.depbin.db_trans.bin_chunk = attr_data_ptr;
                    command_flat.depbin.db_trans.bin_size = data_len;
                    command_flat.depbin.db_trans.bin_offset = attr_offset;
                    break;
                case COMMAND_RESP_CODES_ID:
                    //depbin_ptr = &command_flat.depbin.db_resp_codes;
                    command_flat.depbin.db_resp_codes.bin_chunk = attr_data_ptr;
                    command_flat.depbin.db_resp_codes.bin_size = data_len;
                    command_flat.depbin.db_resp_codes.bin_offset = attr_offset;
                    break;
                case COMMAND_DEBUG_ID:
                    //depbin_ptr = &command_flat.depbin.db_debug_info;
                    command_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    command_flat.depbin.db_debug_info.bin_size = data_len;
                    command_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_COMMAND_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            command_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_command_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fCmd, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = attr_offset;// + i

                                rcode = attach_command_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fCmd, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fCmd.masks.bin_exists = attrMask & COMMAND_ATTR_MASKS;
            glblFlats.fCmd.id = id;
            //iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, glblFlats.fCmd.masks.bin_hooked);
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr;
            uint ulChunkSize = 0;

            AllocAttributes();

            for (int i = 0; i < attrList.Count; i++)
            {// ptr2aPtr2a DDlAttribute 
                DDlAttribute pAt = attrList[i];
                switch (pAt.byAttrID)
                {
                    case COMMAND_NUMBER_ID:
                        {

                            AttrChunkPtr = glblFlats.fCmd.depbin.db_number.bin_chunk;
                            ulChunkSize = glblFlats.fCmd.depbin.db_number.bin_size;

                            rc = Common.parse_attr_int(ref pAt, ref AttrChunkPtr, ulChunkSize, glblFlats.fCmd.depbin.db_number.bin_offset);

                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;
                    case COMMAND_OPER_ID:
                        {
                            AttrChunkPtr = glblFlats.fCmd.depbin.db_oper.bin_chunk;
                            ulChunkSize = glblFlats.fCmd.depbin.db_oper.bin_size;

                            rc = Common.parse_attr_int(ref pAt, ref AttrChunkPtr, ulChunkSize, glblFlats.fCmd.depbin.db_oper.bin_offset);

                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;
                    case COMMAND_TRANS_ID:
                        {
                            AttrChunkPtr = glblFlats.fCmd.depbin.db_trans.bin_chunk;
                            ulChunkSize = glblFlats.fCmd.depbin.db_trans.bin_size;
                            rc = Common.parse_attr_transaction_list(ref pAt, ref AttrChunkPtr, ulChunkSize, glblFlats.fCmd.depbin.db_trans.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                            /** temp 
if(ref pAttribute.pVals.transList[0][0].post_rqst_rcv_act.size())
{
                                TRANSACTION *t = &(ref pAttribute.pVals.transList[0][0]);
                                LOGIT(CLOG_LOG,"Finished transactions: "<<ref pAttribute.pVals.transList[0][0].post_rqst_rcv_act.size() << " actions."<<endl;
}**/
                        }

                        break;
                    case COMMAND_RESP_CODES_ID:
                        {
                            AttrChunkPtr = glblFlats.fCmd.depbin.db_resp_codes.bin_chunk;
                            ulChunkSize = glblFlats.fCmd.depbin.db_resp_codes.bin_size;
                            rc = Common.parse_attr_resp_code_list(ref pAt, ref AttrChunkPtr, ulChunkSize, glblFlats.fCmd.depbin.db_resp_codes.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;

                        }
                        break;
                    case COMMAND_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fCmd.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fCmd.depbin.db_debug_info.bin_size;
                            rc = Common.parse_debug_info(ref pAt, ref AttrChunkPtr, ulChunkSize, glblFlats.fCmd.depbin.db_debug_info.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                            else
                                strItemName = pAt.pVals.debugInfo.symbol_name;

                        }
                        break;
                    default:
                        /*should never reach here*/
                        break;
                }
                attrList[i] = pAt;
            }/*End for */

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }
        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Menu : DDL6BaseItem               /*Item Type == 3*/
    {
        //DDL6Item.FLAT_MENU pMenu = glblFlats.fMenu;


        /* MENU attributes SIZE 1 */
        public const int MENU_LABEL_ID = 0;
        public const int MENU_ITEMS_ID = 1;
        public const int MENU_HELP_ID = 2;
        public const int MENU_VALID_ID = 3;
        public const int MENU_STYLE_ID = 4;
        public const int MENU_DEBUG_ID = 5;
        public const int MENU_VISIBLE_ID = 6;
        public const int MAX_MENU_ID = 7;  /* must be last in list */
        public const int MENU_LABEL = (1 << MENU_LABEL_ID);
        public const int MENU_ITEMS = (1 << MENU_ITEMS_ID);
        public const int MENU_HELP = (1 << MENU_HELP_ID);
        public const int MENU_VALID = (1 << MENU_VALID_ID);
        public const int MENU_STYLE = (1 << MENU_STYLE_ID);
        public const int MENU_DEBUG = (1 << MENU_DEBUG_ID);

        public const int MENU_ATTR_MASKS = (MENU_LABEL | MENU_ITEMS | MENU_HELP | MENU_VALID | MENU_STYLE | MENU_DEBUG);


        public DDl6Menu()
        {
            byItemType = MENU_ITYPE;
            strItemName = "Menu";
            //pMenu = &(glblFlats.fMenu); 
        }

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr;// = NULL;

            if ((attrMask & MENU_LABEL) > 0)
            {

                pDDlAttr = new DDlAttribute("MenuLabel", MENU_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & MENU_ITEMS) > 0)
            {

                pDDlAttr = new DDlAttribute("MenuItems", MENU_ITEMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MENU_ITEM_LIST, false);

                attrList.Add(pDDlAttr);

            }
            if ((attrMask & MENU_HELP) > 0)
            {

                pDDlAttr = new DDlAttribute("MenuHelp", MENU_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & MENU_VALID) > 0)
            {

                pDDlAttr = new DDlAttribute("MenuValidity", MENU_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & MENU_STYLE) > 0)
            {

                pDDlAttr = new DDlAttribute("MenuStyle", MENU_STYLE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MENU_STYLE, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & MENU_DEBUG) > 0)
            {
                pDDlAttr = new DDlAttribute("MenuDebugData", MENU_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_menu_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_MENU menu_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case MENU_LABEL_ID:
                    //depbin_ptr = &menu_flat.depbin.db_label;
                    //depbin_ptr = var_flat.depbin.db_index_item_array;
                    menu_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    menu_flat.depbin.db_label.bin_size = data_len;
                    menu_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case MENU_ITEMS_ID:
                    //depbin_ptr = &menu_flat.depbin.db_items;
                    menu_flat.depbin.db_items.bin_chunk = attr_data_ptr;
                    menu_flat.depbin.db_items.bin_size = data_len;
                    menu_flat.depbin.db_items.bin_offset = attr_offset;
                    break;

                case MENU_HELP_ID:
                    //depbin_ptr = &menu_flat.depbin.db_help;
                    menu_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    menu_flat.depbin.db_help.bin_size = data_len;
                    menu_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case MENU_VALID_ID:
                    //depbin_ptr = &menu_flat.depbin.db_valid;
                    menu_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    menu_flat.depbin.db_valid.bin_size = data_len;
                    menu_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case MENU_STYLE_ID:
                    //depbin_ptr = &menu_flat.depbin.db_style;
                    menu_flat.depbin.db_style.bin_chunk = attr_data_ptr;
                    menu_flat.depbin.db_style.bin_size = data_len;
                    menu_flat.depbin.db_style.bin_offset = attr_offset;
                    break;
                case MENU_DEBUG_ID:
                    //depbin_ptr = &menu_flat.depbin.db_debug_info;
                    menu_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    menu_flat.depbin.db_debug_info.bin_size = data_len;
                    menu_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_MENU_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            menu_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_menu_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fMenu, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_menu_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fMenu, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);


        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {

            //ASSERT_DBG(pbyObjExtn != NULL && glblFlats.fEditDisp != NULL);
            //ASSERT_DBG(pbyObjExtn[1] == byItemType);

            //BYTE* pbyLocalAttrOffset;
            //BYTE* pbyItemExtn = pbyObjExtn;// internal iterator
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fMenu.masks.bin_exists = attrMask & MENU_ATTR_MASKS;
            glblFlats.fMenu.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;

        }
        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr;// = NULL;
            uint ulChunkSize = 0;

            DDlAttribute pAttribute;// = NULL;

            AllocAttributes();

            //ItemAttrList::iterator p;

            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];
                switch (attrList[i].byAttrID)
                {
                    case MENU_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fMenu.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fMenu.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMenu.depbin.db_label.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    case MENU_ITEMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fMenu.depbin.db_items.bin_chunk;
                            ulChunkSize = glblFlats.fMenu.depbin.db_items.bin_size;

                            rc = Common.parse_attr_menu_item_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMenu.depbin.db_items.bin_offset);

                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    case MENU_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fMenu.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fMenu.depbin.db_help.bin_size;
                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMenu.depbin.db_help.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    case MENU_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fMenu.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fMenu.depbin.db_valid.bin_size;
                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMenu.depbin.db_valid.bin_offset);

                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    case MENU_STYLE_ID:
                        {
                            AttrChunkPtr = glblFlats.fMenu.depbin.db_style.bin_chunk;
                            ulChunkSize = glblFlats.fMenu.depbin.db_style.bin_size;
                            rc = Common.parse_attr_menu_style(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMenu.depbin.db_style.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                        }
                        break;

                    case MENU_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fMenu.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fMenu.depbin.db_debug_info.bin_size;
                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMenu.depbin.db_debug_info.bin_offset);
                            if (rc != Common.SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;

                    default:
                        /*should never reach here*/
                        break;

                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for*/

            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            if ((attrMask & MENU_VALID) == 0)
            {
                pAttribute = new DDlAttribute("MenuValidity", MENU_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }

            /*	if (!(attrMask & MENU_STYLE))
                 {
                     pAttribute = new DDlAttribute("MenuStyle",
                                                    MENU_STYLE_ID,
                                                    DDL_ATTR_DATA_TYPE_INTEGER,
                                                    false);


                    pAttribute.pVals = new VALUES;

                    pAttribute.pVals.iVal = 0; //Default Attribute

                    attrList.Add(pAttribute);
                 }
            */

            attrMask = attrMask | MENU_VALID;// |MENU_STYLE;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }
        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6EditDisplay : DDL6BaseItem/*Item Type == 4*/
    {
        //FLAT_EDIT_DISPLAY* glblFlats.fEditDisp;
        /* EDIT_DISPLAY attributes SIZE 2 */

        public const int EDIT_DISPLAY_LABEL_ID = 0;
        public const int EDIT_DISPLAY_EDIT_ITEMS_ID = 1;
        public const int EDIT_DISPLAY_DISP_ITEMS_ID = 2;
        public const int EDIT_DISPLAY_PRE_EDIT_ACT_ID = 3;
        public const int EDIT_DISPLAY_POST_EDIT_ACT_ID = 4;
        public const int EDIT_DISPLAY_HELP_ID = 5;/* EDDL*/
        public const int EDIT_DISPLAY_VALID_ID = 6;  /* EDDL*/
        public const int EDIT_DISPLAY_DEBUG_ID = 7;/* EDDL*/
        public const int EDIT_DISPLAY_VISIBLE_ID = 8;   /* EDDL*/
        public const int MAX_EDIT_DISPLAY_ID = 9;/* must be last in list */

        /* EDIT_DISPLAY attribute masks */

        public const int EDIT_DISPLAY_LABEL = (1 << EDIT_DISPLAY_LABEL_ID);
        public const int EDIT_DISPLAY_EDIT_ITEMS = (1 << EDIT_DISPLAY_EDIT_ITEMS_ID);
        public const int EDIT_DISPLAY_DISP_ITEMS = (1 << EDIT_DISPLAY_DISP_ITEMS_ID);
        public const int EDIT_DISPLAY_PRE_EDIT_ACT = (1 << EDIT_DISPLAY_PRE_EDIT_ACT_ID);
        public const int EDIT_DISPLAY_POST_EDIT_ACT = (1 << EDIT_DISPLAY_POST_EDIT_ACT_ID);
        public const int EDIT_DISPLAY_HELP = (1 << EDIT_DISPLAY_HELP_ID);
        public const int EDIT_DISPLAY_VALID = (1 << EDIT_DISPLAY_VALID_ID);
        public const int EDIT_DISPLAY_DEBUG = (1 << EDIT_DISPLAY_DEBUG_ID);

        public const int EDIT_DISP_ATTR_MASKS = (EDIT_DISPLAY_LABEL | EDIT_DISPLAY_EDIT_ITEMS | EDIT_DISPLAY_DISP_ITEMS | EDIT_DISPLAY_PRE_EDIT_ACT | EDIT_DISPLAY_POST_EDIT_ACT | EDIT_DISPLAY_HELP | EDIT_DISPLAY_VALID | EDIT_DISPLAY_DEBUG);
        public DDl6EditDisplay()
        {
            byItemType = EDIT_DISP_ITYPE;
            strItemName = "Edit Display";
            //glblFlats.fEditDisp = &(glblFlats.fEditDisp); 
        }

        //virtual ~DDl6EditDisplay(){}

        public override void AllocAttributes()
        {
            //Modified by Deepak
            DDlAttribute pDDlAttr;// = NULL;


            if ((attrMask & EDIT_DISPLAY_LABEL) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispLabel", EDIT_DISPLAY_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & EDIT_DISPLAY_EDIT_ITEMS) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispEditItems", EDIT_DISPLAY_EDIT_ITEMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & EDIT_DISPLAY_DISP_ITEMS) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispDisplayItems", EDIT_DISPLAY_DISP_ITEMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & EDIT_DISPLAY_PRE_EDIT_ACT) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispPreEditActions", EDIT_DISPLAY_PRE_EDIT_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & EDIT_DISPLAY_POST_EDIT_ACT) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispPostEditActions", EDIT_DISPLAY_POST_EDIT_ACT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & EDIT_DISPLAY_HELP) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispHelp", EDIT_DISPLAY_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & EDIT_DISPLAY_VALID) != 0)
            {

                pDDlAttr = new DDlAttribute("EditDispValidity", EDIT_DISPLAY_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & EDIT_DISPLAY_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("EditDispDebugData", EDIT_DISPLAY_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_edit_disp_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_EDIT_DISPLAY edit_disp_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case EDIT_DISPLAY_LABEL_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_label;
                    edit_disp_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_label.bin_size = data_len;
                    edit_disp_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case EDIT_DISPLAY_EDIT_ITEMS_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_edit_items;
                    edit_disp_flat.depbin.db_edit_items.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_edit_items.bin_size = data_len;
                    edit_disp_flat.depbin.db_edit_items.bin_offset = attr_offset;
                    break;
                case EDIT_DISPLAY_DISP_ITEMS_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_disp_items;
                    edit_disp_flat.depbin.db_disp_items.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_disp_items.bin_size = data_len;
                    edit_disp_flat.depbin.db_disp_items.bin_offset = attr_offset;
                    break;

                case EDIT_DISPLAY_PRE_EDIT_ACT_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_pre_edit_act;
                    edit_disp_flat.depbin.db_pre_edit_act.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_pre_edit_act.bin_size = data_len;
                    edit_disp_flat.depbin.db_pre_edit_act.bin_offset = attr_offset;
                    break;
                case EDIT_DISPLAY_POST_EDIT_ACT_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_post_edit_act;
                    edit_disp_flat.depbin.db_post_edit_act.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_post_edit_act.bin_size = data_len;
                    edit_disp_flat.depbin.db_post_edit_act.bin_offset = attr_offset;
                    break;
                case EDIT_DISPLAY_HELP_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_help;
                    edit_disp_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_help.bin_size = data_len;
                    edit_disp_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case EDIT_DISPLAY_VALID_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_valid;
                    edit_disp_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_valid.bin_size = data_len;
                    edit_disp_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case EDIT_DISPLAY_DEBUG_ID:
                    //depbin_ptr = &edit_disp_flat.depbin.db_debug_info;
                    edit_disp_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    edit_disp_flat.depbin.db_debug_info.bin_size = data_len;
                    edit_disp_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_EDIT_DISPLAY_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            edit_disp_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_edit_disp_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fEditDisp, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_edit_disp_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fEditDisp, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            //ASSERT_DBG(pbyObjExtn != NULL && glblFlats.fEditDisp != NULL);
            //ASSERT_DBG(pbyObjExtn[1] == byItemType);

            //BYTE* pbyLocalAttrOffset;
            //BYTE* pbyItemExtn = pbyObjExtn;// internal iterator
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fEditDisp.masks.bin_exists = attrMask & EDIT_DISP_ATTR_MASKS;
            glblFlats.fEditDisp.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            DDlAttribute pAttribute;// = NULL;

            AllocAttributes();

            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case EDIT_DISPLAY_LABEL_ID:
                        {

                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case EDIT_DISPLAY_EDIT_ITEMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_edit_items.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_edit_items.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_edit_items.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case EDIT_DISPLAY_DISP_ITEMS_ID:
                        {

                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_disp_items.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_disp_items.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_disp_items.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case EDIT_DISPLAY_PRE_EDIT_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_pre_edit_act.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_pre_edit_act.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_pre_edit_act.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case EDIT_DISPLAY_POST_EDIT_ACT_ID:
                        {
                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_post_edit_act.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_post_edit_act.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_post_edit_act.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case EDIT_DISPLAY_HELP_ID:
                        {

                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case EDIT_DISPLAY_VALID_ID:
                        {

                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case EDIT_DISPLAY_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fEditDisp.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fEditDisp.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fEditDisp.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/

            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            if ((attrMask & EDIT_DISPLAY_VALID) == 0)
            {
                pAttribute = new DDlAttribute("EditDisplayValidity", EDIT_DISPLAY_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }


            attrMask = attrMask | EDIT_DISPLAY_VALID;

            ulItemMasks = attrMask;

            return Common.SUCCESS;

        }


        public override void clear_flat()
        {
            ;
        }

    }


    public class DDl6Method : DDL6BaseItem     /*Item Type == 5*/
    {
        //FLAT_METHOD* pMthd;
        /* METHOD attributes SIZE 2 */

        public const int METHOD_CLASS_ID = 0;
        public const int METHOD_LABEL_ID = 1;
        public const int METHOD_HELP_ID = 2;
        public const int METHOD_DEF_ID = 3;
        public const int METHOD_VALID_ID = 4;
        public const int METHOD_SCOPE_ID = 5;
        public const int METHOD_TYPE_ID = 6;   /* stevev new 13apr05 */
        public const int METHOD_PARAMS_ID = 7;   /* stevev new 13apr05 */
        public const int METHOD_DEBUG_ID = 8;   /* stevev new 13apr05 */
        public const int METHOD_VISIBLE_ID = 9;    /* stevev new 13sep12 */
        public const int METHOD_PRIVATE_ID = 10;   /* stevev new 13sep12 */
        public const int MAX_METHOD_ID = 11;   /* must be last in list */
        /* METHOD attribute masks */

        public const int METHOD_CLASS = (1 << METHOD_CLASS_ID);
        public const int METHOD_LABEL = (1 << METHOD_LABEL_ID);
        public const int METHOD_HELP = (1 << METHOD_HELP_ID);
        public const int METHOD_DEF = (1 << METHOD_DEF_ID);
        public const int METHOD_VALID = (1 << METHOD_VALID_ID);
        public const int METHOD_SCOPE = (1 << METHOD_SCOPE_ID);
        public const int METHOD_TYPE = (1 << METHOD_TYPE_ID);
        public const int METHOD_PARAMS = (1 << METHOD_PARAMS_ID);
        public const int METHOD_DEBUG = (1 << METHOD_DEBUG_ID);
        public const int METHOD_VISIBLE = (1 << METHOD_VISIBLE_ID);

        public const int METHOD_ATTR_MASKS = (METHOD_CLASS | METHOD_LABEL | METHOD_HELP | METHOD_DEF | METHOD_VALID | METHOD_SCOPE | METHOD_TYPE | METHOD_PARAMS | METHOD_DEBUG);

        public DDl6Method()
        {
            byItemType = METHOD_ITYPE;
            strItemName = "Method";
            //pMthd = &(glblFlats.fMethod); 
        }

        //virtual ~DDl6Method(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr;// = NULL;

            if ((attrMask & METHOD_CLASS) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodClass", METHOD_CLASS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

                attrList.Add(pDDlAttr);
            }


            if ((attrMask & METHOD_LABEL) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodLabel", METHOD_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_HELP) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodHelp", METHOD_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_DEF) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodDefinition", METHOD_DEF_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEFINITION, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_VALID) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodValidity", METHOD_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_SCOPE) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodScope", METHOD_SCOPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_TYPE) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodType", METHOD_TYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_PARAM, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_PARAMS) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodParameters", METHOD_PARAMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_PARAM_LIST, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & METHOD_DEBUG) != 0)
            {

                pDDlAttr = new DDlAttribute("MethodDebugData", METHOD_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);

                attrList.Add(pDDlAttr);

            }
        }

        public static int attach_method_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_METHOD method_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case METHOD_CLASS_ID:
                    //depbin_ptr = &method_flat.depbin.db_class;
                    method_flat.depbin.db_class.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_class.bin_size = data_len;
                    method_flat.depbin.db_class.bin_offset = attr_offset;
                    break;
                case METHOD_LABEL_ID:
                    //depbin_ptr = &method_flat.depbin.db_label;
                    method_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_label.bin_size = data_len;
                    method_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case METHOD_HELP_ID:
                    //depbin_ptr = &method_flat.depbin.db_help;
                    method_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_help.bin_size = data_len;
                    method_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case METHOD_DEF_ID:
                    //depbin_ptr = &method_flat.depbin.db_def;
                    method_flat.depbin.db_def.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_def.bin_size = data_len;
                    method_flat.depbin.db_def.bin_offset = attr_offset;
                    break;

                case METHOD_SCOPE_ID:
                    //depbin_ptr = &method_flat.depbin.db_scope;
                    method_flat.depbin.db_scope.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_scope.bin_size = data_len;
                    method_flat.depbin.db_scope.bin_offset = attr_offset;
                    break;
                case METHOD_TYPE_ID:
                    //depbin_ptr = &method_flat.depbin.db_type;
                    method_flat.depbin.db_type.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_type.bin_size = data_len;
                    method_flat.depbin.db_type.bin_offset = attr_offset;
                    break;
                case METHOD_PARAMS_ID:
                    //depbin_ptr = &method_flat.depbin.db_params;
                    method_flat.depbin.db_params.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_params.bin_size = data_len;
                    method_flat.depbin.db_params.bin_offset = attr_offset;
                    break;
                case METHOD_VALID_ID:
                    //depbin_ptr = &method_flat.depbin.db_valid;
                    method_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_valid.bin_size = data_len;
                    method_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case METHOD_DEBUG_ID:
                    //depbin_ptr = &method_flat.depbin.db_debug_info;
                    method_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    method_flat.depbin.db_debug_info.bin_size = data_len;
                    method_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_METHOD_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            method_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_method_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fMethod, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_method_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fMethod, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {

            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fMethod.masks.bin_exists = attrMask & METHOD_ATTR_MASKS;
            glblFlats.fMethod.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;

        }


        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            DDlAttribute pAttribute;// = NULL;

            AllocAttributes();

            //ItemAttrList::iterator p;

            //for(p = pMeth.attrList.begin();p != pMeth.attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case METHOD_CLASS_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_class.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_class.bin_size;

                            rc = Common.parse_attr_bitstring(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_class.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case METHOD_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case METHOD_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case METHOD_DEF_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_def.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_def.bin_size;

                            rc = Common.parse_attr_definition(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_def.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case METHOD_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case METHOD_SCOPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_scope.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_scope.bin_size;

                            rc = Common.parse_attr_meth_scope(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_scope.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case METHOD_TYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_type.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_type.bin_size;

                            rc = Common.parse_attr_method_type(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_type.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case METHOD_PARAMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_params.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_params.bin_size;

                            rc = Common.parse_attr_param_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_params.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case METHOD_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fMethod.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fMethod.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fMethod.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;
                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End while */

            /*Vibhor 240204: Start of Code*/
            /*If a methode has no Help defined for it we'll have default string*/

            if ((attrMask & METHOD_HELP) == 0)
            {
                pAttribute = new DDlAttribute("MethodHelp", METHOD_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.strVal = new ddpSTRING();

                //pAttribute.pVals.strVal.str = new char[18];

                //strcpy(pAttribute.pVals.strVal.str, ""/*"No Help Available"*/);//Removed No Help Available, DDHost Test requires a blank label, POB - 12/2/2013
                pAttribute.pVals.strVal.str = "";

                pAttribute.pVals.strVal.flags = Common.FREE_STRING;

                pAttribute.pVals.strVal.strType = Common.DEV_SPEC_STRING_TAG; //This will ensure cleanup.

                attrList.Add(pAttribute);

                attrMask |= METHOD_HELP;

            }

            /*Vibhor 240204: End of Code*/

            if ((attrMask & METHOD_VALID) == 0)
            {
                pAttribute = new DDlAttribute("MethodValidity", METHOD_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);

                attrMask |= METHOD_VALID;
            }

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }


    public class DDl6Refresh : DDL6BaseItem        /*Item Type == 6*/
    {
        //FLAT_REFRESH* glblFlats.fRefresh;

        /* REFRESH attributes SIZE 1 */

        public const int REFRESH_ITEMS_ID = 0;
        public const int REFRESH_DEBUG_ID = 1;
        public const int MAX_REFRESH_ID = 2;   /* must be last in list - for backward compatability*/
        public const int REFRESH_UPDATE_LIST_ID = 3;   /* new format - 3 & 4 go together */
        public const int REFRESH_WATCH_LIST_ID = 4;/* new format - 3 & 4 go together */
        /* REFRESH attr7ibute masks */

        public const int REFRESH_ITEMS = (1 << REFRESH_ITEMS_ID);
        public const int REFRESH_DEBUG = (1 << REFRESH_DEBUG_ID);

        public const int REFRESH_ATTR_MASKS = (REFRESH_ITEMS | REFRESH_DEBUG);

        public DDl6Refresh()
        {
            byItemType = REFRESH_ITYPE;
            strItemName = "Refresh Relation";
            //glblFlats.fRefresh = &(glblFlats.fRefresh); 
        }

        //virtual ~DDl6Refresh(){}
        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & REFRESH_ITEMS) != 0)
            {
                pDDlAttr = new DDlAttribute("RefreshItems", REFRESH_ITEMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFRESH_RELATION, false);
                attrList.Add(pDDlAttr);
            }
            if ((attrMask & REFRESH_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("RefreshDebugData", REFRESH_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

            return;
        }

        public static int attach_refresh_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_REFRESH refresh_flat, ushort tag)
        {
            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case REFRESH_ITEMS_ID:
                    //depbin_ptr = &refresh_flat.depbin.db_items;		
                    refresh_flat.depbin.db_items.bin_chunk = attr_data_ptr;
                    refresh_flat.depbin.db_items.bin_size = data_len;
                    refresh_flat.depbin.db_items.bin_offset = attr_offset;
                    break;
                case REFRESH_DEBUG_ID:
                    //depbin_ptr = &refresh_flat.depbin.db_debug_info;
                    refresh_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    refresh_flat.depbin.db_debug_info.bin_size = data_len;
                    refresh_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_REFRESH_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            refresh_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_refresh_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fRefresh, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_refresh_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fRefresh, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {

            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fRefresh.masks.bin_exists = attrMask & REFRESH_ATTR_MASKS;
            glblFlats.fRefresh.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;

        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case REFRESH_ITEMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fRefresh.depbin.db_items.bin_chunk;
                            ulChunkSize = glblFlats.fRefresh.depbin.db_items.bin_size;

                            rc = Common.parse_attr_refresh_relation(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fRefresh.depbin.db_items.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case REFRESH_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fRefresh.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fRefresh.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fRefresh.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;

                        }
                        break;
                    default:
                        break;
                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for*/

            ulItemMasks = attrMask;

            return Common.SUCCESS;

        }

        public override void clear_flat()
        {
            ;
        }

    }

    public class DDl6Unit : DDL6BaseItem           /*Item Type == 7*/
    {
        //FLAT_UNIT* pUnit;
        /* UNIT attributes  SIZE 1 */

        public const int UNIT_ITEMS_ID = 0;
        public const int UNIT_DEBUG_ID = 1;
        public const int MAX_UNIT_ID = 2;  /* must be last in list - for backward compatability */
        public const int UNIT_VAR_ID = 3; /* new format - 4 is update list */
        public const int UNIT_UPDATE_LIST_ID = 4;/* new format - 3 is watch item */
        /* UNIT attribute masks */

        public const int UNIT_ITEMS = (1 << UNIT_ITEMS_ID);
        public const int UNIT_DEBUG = (1 << UNIT_DEBUG_ID);

        public const int UNIT_ATTR_MASKS = (UNIT_ITEMS | UNIT_DEBUG_ID);

        public DDl6Unit()
        {
            byItemType = UNIT_ITYPE;
            strItemName = "Unit Relation";
            //pUnit = &(glblFlats.fUnit); 
        }

        //virtual ~DDl6Unit(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & UNIT_ITEMS) != 0)
            {

                pDDlAttr = new DDlAttribute("UnitItems", UNIT_ITEMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNIT_RELATION, false);

                attrList.Add(pDDlAttr);

            }
            if ((attrMask & UNIT_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("UnitDebugData", UNIT_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

            return;
        }

        public static int attach_unit_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_UNIT unit_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case UNIT_ITEMS_ID:
                    //depbin_ptr = &unit_flat.depbin.db_items;
                    unit_flat.depbin.db_items.bin_chunk = attr_data_ptr;
                    unit_flat.depbin.db_items.bin_size = data_len;
                    unit_flat.depbin.db_items.bin_offset = attr_offset;
                    break;
                case UNIT_DEBUG_ID:
                    //depbin_ptr = &unit_flat.depbin.db_debug_info;
                    unit_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    unit_flat.depbin.db_debug_info.bin_size = data_len;
                    unit_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_UNIT_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            unit_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_unit_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fUnit, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_unit_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fUnit, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }



        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fUnit.masks.bin_exists = attrMask & UNIT_ATTR_MASKS;
            glblFlats.fUnit.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }
        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case UNIT_ITEMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fUnit.depbin.db_items.bin_chunk;
                            ulChunkSize = glblFlats.fUnit.depbin.db_items.bin_size;

                            rc = Common.parse_attr_unit_relation(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fUnit.depbin.db_items.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case UNIT_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fUnit.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fUnit.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fUnit.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;

                        }
                        break;
                    default:
                        break;
                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for*/

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }


    public class DDl6Wao : DDL6BaseItem                /*Item Type == 8*/
    {
        //FLAT_WAO* pWao;
        /* WRITE AS ONE attributes SIZE 1 */

        public const int WAO_ITEMS_ID = 0;
        public const int WAO_DEBUG_ID = 1;
        public const int MAX_WAO_ID = 2;    /* must be last in list */
        /* WRITE AS ONE attribute masks */

        public const int WAO_ITEMS = (1 << WAO_ITEMS_ID);
        public const int WAO_DEBUG = (1 << WAO_DEBUG_ID);

        public const int WAO_ATTR_MASKS = (WAO_ITEMS | WAO_DEBUG);

        public DDl6Wao()
        {
            byItemType = WAO_ITYPE;
            strItemName = "WAO Relation";
            //pWao = &(glblFlats.fWao); 
        }

        //virtual ~DDl6Wao(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & WAO_ITEMS) != 0)
            {

                pDDlAttr = new DDlAttribute("UnitItems", WAO_ITEMS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);

                attrList.Add(pDDlAttr);

            }
            if ((attrMask & WAO_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("UnitDebugData", WAO_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

            return;
        }

        public static int attach_wao_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_WAO wao_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case WAO_ITEMS_ID:
                    //depbin_ptr = &refresh_flat.depbin.db_items;		
                    wao_flat.depbin.db_items.bin_chunk = attr_data_ptr;
                    wao_flat.depbin.db_items.bin_size = data_len;
                    wao_flat.depbin.db_items.bin_offset = attr_offset;
                    break;
                case WAO_DEBUG_ID:
                    //depbin_ptr = &refresh_flat.depbin.db_debug_info;
                    wao_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    wao_flat.depbin.db_debug_info.bin_size = data_len;
                    wao_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_WAO_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            wao_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_wao_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fWao, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_wao_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fWao, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, 0, pbyLocalAttrOffset);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, 0, pbyLocalAttrOffset);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }


        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fWao.masks.bin_exists = attrMask & WAO_ATTR_MASKS;
            glblFlats.fWao.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case WAO_ITEMS_ID:
                        {
                            AttrChunkPtr = glblFlats.fWao.depbin.db_items.bin_chunk;
                            ulChunkSize = glblFlats.fWao.depbin.db_items.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWao.depbin.db_items.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case WAO_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fWao.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fWao.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWao.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;

                        }
                        break;
                    default:
                        break;
                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for*/

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }


    public class DDl6ItemArray : DDL6BaseItem      /*Item Type == 9*/
    {
        //FLAT_ITEM_ARRAY* pItmArr;
        /* ITEM_ARRAY attributes */

        public const int ITEM_ARRAY_ELEMENTS_ID = 0;
        public const int ITEM_ARRAY_LABEL_ID = 1;
        public const int ITEM_ARRAY_HELP_ID = 2;
        public const int ITEM_ARRAY_VALIDITY_ID = 3;
        public const int ITEM_ARRAY_DEBUG_ID = 4;
        public const int ITEM_ARRAY_PRIVATE_ID = 5;
        public const int MAX_ITEM_ARRAY_ID = 6;/* must be last in list */
        /* ITEM_ARRAY attribute masks */

        public const int ITEM_ARRAY_ELEMENTS = (1 << ITEM_ARRAY_ELEMENTS_ID);
        public const int ITEM_ARRAY_LABEL = (1 << ITEM_ARRAY_LABEL_ID);
        public const int ITEM_ARRAY_HELP = (1 << ITEM_ARRAY_HELP_ID);
        public const int ITEM_ARRAY_VALIDITY = (1 << ITEM_ARRAY_VALIDITY_ID);
        public const int ITEM_ARRAY_DEBUG = (1 << ITEM_ARRAY_DEBUG_ID);

        public const int ITEM_ARRAY_ATTR_MASKS = (ITEM_ARRAY_ELEMENTS | ITEM_ARRAY_LABEL | ITEM_ARRAY_HELP | ITEM_ARRAY_VALIDITY | ITEM_ARRAY_DEBUG);

        public DDl6ItemArray()
        {
            byItemType = ITEM_ARRAY_ITYPE;
            strItemName = "Item Array";
            //pItmArr = &(glblFlats.fIArr); 
        }

        //virtual ~DDl6ItemArray(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & ITEM_ARRAY_LABEL) != 0)
            {

                pDDlAttr = new DDlAttribute("ItemArrayLabel", ITEM_ARRAY_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & ITEM_ARRAY_HELP) != 0)
            {

                pDDlAttr = new DDlAttribute("ItemArrayHelp", ITEM_ARRAY_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & ITEM_ARRAY_ELEMENTS) != 0)
            {

                pDDlAttr = new DDlAttribute("ItemArrayElements", ITEM_ARRAY_ELEMENTS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ITEM_ARRAY_ELEMENT_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & ITEM_ARRAY_VALIDITY) != 0)
            {

                pDDlAttr = new DDlAttribute("ItemArrayValidity", ITEM_ARRAY_VALIDITY_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & ITEM_ARRAY_DEBUG) != 0)
            {

                pDDlAttr = new DDlAttribute("ItemArrayDebugData", ITEM_ARRAY_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);

                attrList.Add(pDDlAttr);

            }
        }

        public static int attach_item_array_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_ITEM_ARRAY item_array_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case ITEM_ARRAY_ELEMENTS_ID:
                    //		depbin_ptr = &item_array_flat.depbin.db_elements;
                    item_array_flat.depbin.db_elements.bin_chunk = attr_data_ptr;
                    item_array_flat.depbin.db_elements.bin_size = data_len;
                    item_array_flat.depbin.db_elements.bin_offset = attr_offset;
                    break;

                case ITEM_ARRAY_LABEL_ID:
                    //depbin_ptr = &item_array_flat.depbin.db_label;
                    item_array_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    item_array_flat.depbin.db_label.bin_size = data_len;
                    item_array_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case ITEM_ARRAY_HELP_ID:
                    //depbin_ptr = &item_array_flat.depbin.db_help;
                    item_array_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    item_array_flat.depbin.db_help.bin_size = data_len;
                    item_array_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case ITEM_ARRAY_VALIDITY_ID:
                    //depbin_ptr = &item_array_flat.depbin.db_valid;
                    item_array_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    item_array_flat.depbin.db_valid.bin_size = data_len;
                    item_array_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;

                case ITEM_ARRAY_DEBUG_ID:
                    //depbin_ptr = &item_array_flat.depbin.db_debug_info;
                    item_array_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    item_array_flat.depbin.db_debug_info.bin_size = data_len;
                    item_array_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_ITEM_ARRAY_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            item_array_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_item_array_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fIArr, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_item_array_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fIArr, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, pbyLocalAttrOffset);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, pbyLocalAttrOffset);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }


        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fIArr.masks.bin_exists = attrMask & ITEM_ARRAY_ATTR_MASKS;
            glblFlats.fIArr.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case ITEM_ARRAY_ELEMENTS_ID:
                        {
                            AttrChunkPtr = glblFlats.fIArr.depbin.db_elements.bin_chunk;
                            ulChunkSize = glblFlats.fIArr.depbin.db_elements.bin_size;

                            rc = Common.parse_attr_item_array_element_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fIArr.depbin.db_elements.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;
                    case ITEM_ARRAY_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fIArr.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fIArr.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fIArr.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case ITEM_ARRAY_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fIArr.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fIArr.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fIArr.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case ITEM_ARRAY_VALIDITY_ID:
                        {
                            AttrChunkPtr = glblFlats.fIArr.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fIArr.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fIArr.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case ITEM_ARRAY_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fIArr.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fIArr.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fIArr.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;
                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for */

            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            if ((attrMask & ITEM_ARRAY_VALIDITY) == 0)
            {
                pAttribute = new DDlAttribute("ItemArrayValidity", ITEM_ARRAY_VALIDITY_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }


            attrMask = attrMask | ITEM_ARRAY_VALIDITY;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }
        public override void clear_flat()
        {
            ;
        }
    }


    public class DDl6Collection : DDL6BaseItem /*Item Type == 10*/
    {
        //FLAT_COLLECTION* pColl;
        /* COLLECTION attributes SIZE 1 */

        public const int COLLECTION_MEMBERS_ID = 0;
        public const int COLLECTION_LABEL_ID = 1;
        public const int COLLECTION_HELP_ID = 2;
        public const int COLLECTION_VALID_ID = 3;
        public const int COLLECTION_DEBUG_ID = 4;
        public const int COLLECTION_VISIBLE_ID = 5;
        public const int COLLECTION_PRIVATE_ID = 6;
        public const int MAX_COLLECTION_ID = 7;/* must be last in list */
        /* COLLECTION attribute masks */

        public const int COLLECTION_MEMBERS = (1 << COLLECTION_MEMBERS_ID);
        public const int COLLECTION_LABEL = (1 << COLLECTION_LABEL_ID);
        public const int COLLECTION_HELP = (1 << COLLECTION_HELP_ID);
        public const int COLLECTION_VALIDITY = (1 << COLLECTION_VALID_ID);
        public const int COLLECTION_DEBUG = (1 << COLLECTION_DEBUG_ID);

        public const int COLLECTION_ATTR_MASKS = (COLLECTION_MEMBERS | COLLECTION_LABEL | COLLECTION_HELP | COLLECTION_VALIDITY | COLLECTION_DEBUG);
        public DDl6Collection()
        {
            byItemType = COLLECTION_ITYPE;
            strItemName = "Collection";
            //pColl = &(glblFlats.fColl); 
        }

        //virtual ~DDl6Collection(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & COLLECTION_LABEL) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionLabel", COLLECTION_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & COLLECTION_HELP) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionHelp", COLLECTION_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & COLLECTION_VALIDITY) != 0)
            {
                pDDlAttr = new DDlAttribute("CollectionValidity", COLLECTION_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & COLLECTION_MEMBERS) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionMembers", COLLECTION_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & COLLECTION_DEBUG) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionDebugData", COLLECTION_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);

                attrList.Add(pDDlAttr);

            }
        }

        public static int attach_collection_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_COLLECTION collection_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case COLLECTION_MEMBERS_ID:
                    //depbin_ptr = &collection_flat.depbin.db_members;
                    collection_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    collection_flat.depbin.db_members.bin_size = data_len;
                    collection_flat.depbin.db_members.bin_offset = attr_offset;
                    break;
                case COLLECTION_LABEL_ID:
                    //depbin_ptr = &collection_flat.depbin.db_label;
                    collection_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    collection_flat.depbin.db_label.bin_size = data_len;
                    collection_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case COLLECTION_HELP_ID:
                    //depbin_ptr = &collection_flat.depbin.db_help;
                    collection_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    collection_flat.depbin.db_help.bin_size = data_len;
                    collection_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case COLLECTION_VALID_ID:
                    //depbin_ptr = &collection_flat.depbin.db_valid;
                    collection_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    collection_flat.depbin.db_valid.bin_size = data_len;
                    collection_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case COLLECTION_DEBUG_ID:
                    //depbin_ptr = &collection_flat.depbin.db_debug_info;
                    collection_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    collection_flat.depbin.db_debug_info.bin_size = data_len;
                    collection_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_COLLECTION_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            collection_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_collection_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fColl, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_collection_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fColl, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }


        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fColl.masks.bin_exists = attrMask & COLLECTION_ATTR_MASKS;
            glblFlats.fColl.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case COLLECTION_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fColl.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fColl.depbin.db_members.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fColl.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case COLLECTION_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fColl.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fColl.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fColl.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case COLLECTION_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fColl.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fColl.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fColl.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case COLLECTION_VALID_ID:/* added 23jan06 stevev - spec change */
                        {
                            AttrChunkPtr = glblFlats.fColl.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fColl.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fColl.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case COLLECTION_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fColl.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fColl.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fColl.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;

                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for*/

            /* default attributes  - stevev 23jan07 */
            if ((attrMask & COLLECTION_VALIDITY) == 0)
            {
                pAttribute = new DDlAttribute("CollectionValidity", COLLECTION_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1;
                attrList.Add(pAttribute);
            }

            ulItemMasks = attrMask | COLLECTION_VALIDITY;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }


    public class DDl6Block : DDL6BaseItem              /*Item Type == 12*/
    {
        //FLAT_BLOCK* pBlk;
        /* BLOCK attributes */

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
        public const int MAX_BLOCK_ID = 13;/* must be last in list */
        /* BLOCK attribute masks */

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

        public const int BLOCK_ATTR_MASKS = (BLOCK_CHARACTERISTIC | BLOCK_LABEL | BLOCK_HELP | BLOCK_PARAM | BLOCK_MENU | BLOCK_EDIT_DISP | BLOCK_METHOD | BLOCK_UNIT | BLOCK_REFRESH | BLOCK_WAO | BLOCK_COLLECT | BLOCK_ITEM_ARRAY | BLOCK_PARAM_LIST);

        public DDl6Block()
        {
            byItemType = BLOCK_ITYPE;
            strItemName = "Block";
            //pBlk = &(glblFlats.fBlock); 
        }

        //virtual ~DDl6Block(){}

        public override void AllocAttributes()
        {
            /*	if(attrMask & BLOCK_CHARACTERISTIC)//block //??????
    {
        pDDlAttr = new DDlAttribute("BlockCharacteristic",
                                        BLOCK_CHARACTERISTIC_ID,
                                        DDL_ATTR_DATA_TYPE_REFERENCE,
                                        false);

        attrList.Add(pDDlAttr);
    }
*/
            DDlAttribute pDDlAttr = null;

            if ((attrMask & BLOCK_PARAM) != 0)
            {

                pDDlAttr = new DDlAttribute("BlockParams", BLOCK_PARAM_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);

                attrList.Add(pDDlAttr);

            }

        }

        public static int attach_block_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_BLOCK block_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case BLOCK_CHARACTERISTIC_ID:
                    //depbin_ptr = &block_flat.depbin.db_characteristic;
                    block_flat.depbin.db_characteristic.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_characteristic.bin_size = data_len;
                    block_flat.depbin.db_characteristic.bin_offset = attr_offset;
                    break;
                case BLOCK_LABEL_ID:
                    //depbin_ptr = &block_flat.depbin.db_label;
                    block_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_label.bin_size = data_len;
                    block_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case BLOCK_HELP_ID:
                    //depbin_ptr = &block_flat.depbin.db_help;
                    block_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_help.bin_size = data_len;
                    block_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case BLOCK_PARAM_ID:
                    //depbin_ptr = &block_flat.depbin.db_param;
                    block_flat.depbin.db_param.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_param.bin_size = data_len;
                    block_flat.depbin.db_param.bin_offset = attr_offset;
                    break;
                case BLOCK_MENU_ID:
                    //depbin_ptr = &block_flat.depbin.db_menu;
                    block_flat.depbin.db_menu.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_menu.bin_size = data_len;
                    block_flat.depbin.db_menu.bin_offset = attr_offset;
                    break;
                case BLOCK_EDIT_DISP_ID:
                    //depbin_ptr = &block_flat.depbin.db_edit_disp;
                    block_flat.depbin.db_edit_disp.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_edit_disp.bin_size = data_len;
                    block_flat.depbin.db_edit_disp.bin_offset = attr_offset;
                    break;
                case BLOCK_METHOD_ID:
                    //depbin_ptr = &block_flat.depbin.db_method;
                    block_flat.depbin.db_method.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_method.bin_size = data_len;
                    block_flat.depbin.db_method.bin_offset = attr_offset;
                    break;
                case BLOCK_UNIT_ID:
                    //depbin_ptr = &block_flat.depbin.db_unit;
                    block_flat.depbin.db_unit.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_unit.bin_size = data_len;
                    block_flat.depbin.db_unit.bin_offset = attr_offset;
                    break;
                case BLOCK_REFRESH_ID:
                    //depbin_ptr = &block_flat.depbin.db_refresh;
                    block_flat.depbin.db_refresh.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_refresh.bin_size = data_len;
                    block_flat.depbin.db_refresh.bin_offset = attr_offset;
                    break;
                case BLOCK_WAO_ID:
                    //depbin_ptr = &block_flat.depbin.db_wao;
                    block_flat.depbin.db_wao.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_wao.bin_size = data_len;
                    block_flat.depbin.db_wao.bin_offset = attr_offset;
                    break;
                case BLOCK_COLLECT_ID:
                    //depbin_ptr = &block_flat.depbin.db_collect;
                    block_flat.depbin.db_collect.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_collect.bin_size = data_len;
                    block_flat.depbin.db_collect.bin_offset = attr_offset;
                    break;
                case BLOCK_ITEM_ARRAY_ID:
                    //depbin_ptr = &block_flat.depbin.db_item_array;
                    block_flat.depbin.db_item_array.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_item_array.bin_size = data_len;
                    block_flat.depbin.db_item_array.bin_offset = attr_offset;
                    break;
                case BLOCK_PARAM_LIST_ID:
                    //depbin_ptr = &block_flat.depbin.db_param_list;
                    block_flat.depbin.db_param_list.bin_chunk = attr_data_ptr;
                    block_flat.depbin.db_param_list.bin_size = data_len;
                    block_flat.depbin.db_param_list.bin_offset = attr_offset;
                    break;

                default:
                    if (tag >= MAX_BLOCK_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            block_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_block_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fBlock, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_block_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fBlock, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }


        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fBlock.masks.bin_exists = attrMask & BLOCK_ATTR_MASKS;
            glblFlats.fBlock.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case BLOCK_CHARACTERISTIC_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_characteristic.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_characteristic.bin_size;
                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_characteristic.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_PARAM_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_param.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_param.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_param.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_MENU_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_menu.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_menu.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_menu.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_EDIT_DISP_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_edit_disp.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_edit_disp.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_edit_disp.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_METHOD_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_method.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_method.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_method.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_REFRESH_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_refresh.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_refresh.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_refresh.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_UNIT_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_unit.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_unit.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_unit.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_WAO_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_wao.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_wao.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_wao.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_COLLECT_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_collect.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_collect.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_collect.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_ITEM_ARRAY_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_item_array.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_item_array.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_item_array.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case BLOCK_PARAM_LIST_ID:
                        {
                            AttrChunkPtr = glblFlats.fBlock.depbin.db_param_list.bin_chunk;
                            ulChunkSize = glblFlats.fBlock.depbin.db_param_list.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fBlock.depbin.db_param_list.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    default:
                        break;


                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for*/

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    /* UNUSED: PROGRAM_ITYPE */

    public class DDl6Record : DDL6BaseItem         /*Item Type == 14*/
    {
        //FLAT_RECORD* pRec;
        /* RECORD attributes */

        public const int RECORD_MEMBERS_ID = 0;
        public const int RECORD_LABEL_ID = 1;
        public const int RECORD_HELP_ID = 2;
        public const int RECORD_RESP_CODES_ID = 3;
        public const int MAX_RECORD_ID = 4;/* must be last in list */
        /* RECORD attribute masks */

        public const int RECORD_MEMBERS = (1 << RECORD_MEMBERS_ID);
        public const int RECORD_LABEL = (1 << RECORD_LABEL_ID);
        public const int RECORD_HELP = (1 << RECORD_HELP_ID);
        public const int RECORD_RESP_CODES = (1 << RECORD_RESP_CODES_ID);

        public const int RECORD_ATTR_MASKS = (RECORD_MEMBERS | RECORD_LABEL | RECORD_HELP | RECORD_RESP_CODES);

        public DDl6Record()
        {
            byItemType = RECORD_ITYPE;
            strItemName = "Record";
            //pRec = &(glblFlats.fRec); 
        }

        //virtual ~DDl6Record(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & RECORD_LABEL) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionLabel", RECORD_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }


            if ((attrMask & RECORD_HELP) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionHelp", RECORD_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);

                attrList.Add(pDDlAttr);

            }

            if ((attrMask & RECORD_MEMBERS) != 0)
            {

                pDDlAttr = new DDlAttribute("CollectionMembers", RECORD_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);

                attrList.Add(pDDlAttr);

            }
        }

        public static int attach_record_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_RECORD record_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case RECORD_MEMBERS_ID:
                    //depbin_ptr = &record_flat.depbin.db_members;
                    record_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    record_flat.depbin.db_members.bin_size = data_len;
                    record_flat.depbin.db_members.bin_offset = attr_offset;
                    break;
                case RECORD_LABEL_ID:
                    //depbin_ptr = &record_flat.depbin.db_label;
                    record_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    record_flat.depbin.db_label.bin_size = data_len;
                    record_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case RECORD_HELP_ID:
                    //depbin_ptr = &record_flat.depbin.db_help;
                    record_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    record_flat.depbin.db_help.bin_size = data_len;
                    record_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case RECORD_RESP_CODES_ID:
                    //depbin_ptr = &record_flat.depbin.db_resp_codes;
                    record_flat.depbin.db_resp_codes.bin_chunk = attr_data_ptr;
                    record_flat.depbin.db_resp_codes.bin_size = data_len;
                    record_flat.depbin.db_resp_codes.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_RECORD_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            record_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_record_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fRec, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_record_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fRec, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fRec.masks.bin_exists = attrMask & RECORD_ATTR_MASKS;
            glblFlats.fRec.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case RECORD_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fRec.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fRec.depbin.db_members.bin_size;
                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fRec.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case RECORD_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fRec.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fRec.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fRec.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case RECORD_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fRec.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fRec.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fRec.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    /*Vibhor 311003: Commenting this one*/
                    /*			case	RECORD_RESP_CODES_ID:
                                    {
                                        AttrChunkPtr = glblFlats.fRec.depbin.db_resp_codes.bin_chunk;
                                        ulChunkSize  = glblFlats.fRec.depbin.db_resp_codes.bin_size;

                                        rc = parse_attr_reference((*p),AttrChunkPtr,ulChunkSize);
                                        if(rc != DDL_SUCCESS) 
                                            return rc;
                                    }
                    */
                    default:
                        break;

                }/*End switch*/
                attrList[i] = pAttribute;

            }/*End for */

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }


    /* * * * * DDL 6 Additions * * * * * */
    public class DDl6Array : DDL6BaseItem      /*Item Type == 15*/
    {
        //FLAT_ARRAY* pArr;

        /* ARRAY attributes - SIZE 2*/

        public const int ARRAY_LABEL_ID = 1;
        public const int ARRAY_HELP_ID = 2;
        public const int ARRAY_VALID_ID = 3;
        public const int ARRAY_TYPE_ID = 4;
        public const int ARRAY_NUM_OF_ELEMENTS_ID = 5;
        public const int ARRAY_DEBUG_ID = 6;
        public const int ARRAY_PRIVATE_ID = 7;
        public const int MAX_ARRAY_ID = 8; /* must be last in list */
        /* ARRAY attribute masks */

        public const int ARRAY_LABEL = (1 << ARRAY_LABEL_ID);
        public const int ARRAY_HELP = (1 << ARRAY_HELP_ID);
        public const int ARRAY_TYPE = (1 << ARRAY_TYPE_ID);
        public const int ARRAY_NUM_OF_ELEMENTS = (1 << ARRAY_NUM_OF_ELEMENTS_ID);
        public const int ARRAY_VALID = (1 << ARRAY_VALID_ID);
        public const int ARRAY_DEBUG = (1 << ARRAY_DEBUG_ID);

        public const int ARRAY_ATTR_MASKS = (ARRAY_LABEL | ARRAY_HELP | ARRAY_VALID | ARRAY_TYPE | ARRAY_NUM_OF_ELEMENTS | ARRAY_DEBUG);

        public override void AllocAttributes(uint attrMask)
        {
        }// to make base item happy

        public DDl6Array()
        {
            byItemType = ARRAY_ITYPE;
            strItemName = "Array";
            //pArr = &(glblFlats.fArr); 
        }

        //virtual ~DDl6Array(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & ARRAY_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("ArrayLabel", ARRAY_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & ARRAY_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("ArrayHelp", ARRAY_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & ARRAY_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("ArrayValidity", ARRAY_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & ARRAY_TYPE) != 0)
            {
                pDDlAttr = new DDlAttribute("ArrayType", ARRAY_TYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & ARRAY_NUM_OF_ELEMENTS) != 0)
            {
                pDDlAttr = new DDlAttribute("ArrayLength", ARRAY_NUM_OF_ELEMENTS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & ARRAY_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("ArrayDebugData", ARRAY_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_array_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_ARRAY array_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case ARRAY_LABEL_ID:
                    //depbin_ptr = &array_flat->depbin->db_label;
                    array_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    array_flat.depbin.db_label.bin_size = data_len;
                    array_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case ARRAY_HELP_ID:
                    //depbin_ptr = &array_flat->depbin->db_help;
                    array_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    array_flat.depbin.db_help.bin_size = data_len;
                    array_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case ARRAY_VALID_ID:                            //Vibhor 280904: Added	
                    //depbin_ptr = &array_flat->depbin->db_valid;
                    array_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    array_flat.depbin.db_valid.bin_size = data_len;
                    array_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case ARRAY_TYPE_ID:
                    //depbin_ptr = &array_flat->depbin->db_type;
                    array_flat.depbin.db_type.bin_chunk = attr_data_ptr;
                    array_flat.depbin.db_type.bin_size = data_len;
                    array_flat.depbin.db_type.bin_offset = attr_offset;
                    break;
                case ARRAY_NUM_OF_ELEMENTS_ID:
                    //depbin_ptr = &array_flat->depbin->db_num_of_elements;
                    array_flat.depbin.db_num_of_elements.bin_chunk = attr_data_ptr;
                    array_flat.depbin.db_num_of_elements.bin_size = data_len;
                    array_flat.depbin.db_num_of_elements.bin_offset = attr_offset;
                    break;
                case ARRAY_DEBUG_ID:
                    //depbin_ptr = &array_flat->depbin->db_debug_info;
                    array_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    array_flat.depbin.db_debug_info.bin_size = data_len;
                    array_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                //case ARRAY_RESP_CODES_ID:
                //    //depbin_ptr = &array_flat->depbin->db_resp_codes;
                //    array_flat.depbin.db_resp_codes.bin_chunk = attr_data_ptr;
                //    array_flat.depbin.db_resp_codes.bin_size = data_len;
                //    array_flat.depbin.db_resp_codes.bin_offset = attr_offset;
                //    break;
                default:
                    if (tag >= MAX_ARRAY_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            array_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_array_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fArr, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_array_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fArr, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fArr.masks.bin_exists = attrMask & ARRAY_ATTR_MASKS;
            glblFlats.fArr.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override unsafe int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;
            UInt64 tempLong;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case ARRAY_NUM_OF_ELEMENTS_ID:
                        {
                            AttrChunkPtr = glblFlats.fArr.depbin.db_num_of_elements.bin_chunk;
                            ulChunkSize = glblFlats.fArr.depbin.db_num_of_elements.bin_size;

                            pAttribute.pVals = new VALUES();

                            fixed (byte* chu = &AttrChunkPtr[glblFlats.fArr.depbin.db_num_of_elements.bin_offset])
                            {

                                Common.DDL_PARSE_INTEGER(&chu, &ulChunkSize, &tempLong);

                                if (DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT == pAttribute.attrDataType)
                                {
                                    pAttribute.pVals.llVal = (Int64)tempLong;
                                }
                                else /*DDL_ATTR_DATA_TYPE_UNSIGNED_LONG == pAttr.attrDataType*/
                                {
                                    pAttribute.pVals.ullVal = tempLong;
                                }

                                //				rc = Common.parse_attr_int(pAttribute,AttrChunkPtr,ulChunkSize);
                                //if (rc != Common.DDL_SUCCESS)
                                //  return rc;
                            }
                        }

                        break;
                    case ARRAY_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fArr.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fArr.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fArr.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case ARRAY_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fArr.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fArr.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fArr.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case ARRAY_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fArr.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fArr.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fArr.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case ARRAY_TYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fArr.depbin.db_type.bin_chunk;
                            ulChunkSize = glblFlats.fArr.depbin.db_type.bin_size;

                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fArr.depbin.db_type.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case ARRAY_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fArr.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fArr.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fArr.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;

                    default:
                        break;
                }/*End switch*/

                attrList[i] = pAttribute;//??????

            }/*End for */

            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            if ((attrMask & ARRAY_VALID) == 0)
            {
                pAttribute = new DDlAttribute("ArrayValidity", ARRAY_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }


            attrMask = attrMask | ARRAY_VALID;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    /* UNUSED:	VAR_LIST_ITYPE,
                RESP_CODES_ITYPE,
                DOMAIN_ITYPE,
                MEMBER_ITYPE    * */

    public class DDl6File : DDL6BaseItem   /*Item Type == 20*/
    {
        //FLAT_FILE* pFile;

        /* FILE attributes  SIZE 1 */

        public const int FILE_MEMBERS_ID = 0;
        public const int FILE_LABEL_ID = 1;
        public const int FILE_HELP_ID = 2;
        public const int FILE_NO_VALIDITY = 3;
        public const int FILE_DEBUG_ID = 4;
        public const int FILE_HANDLING_ID = 5; /* 5,6&7 added 17nov08 */
        public const int FILE_UPDATE_ACTIONS_ID = 6;
        public const int FILE_IDENTITY_ID = 7;
        public const int MAX_FILE_ID = 8;
        /* FILE attribute masks */

        public const int FILE_MEMBERS = (1 << FILE_MEMBERS_ID);
        public const int FILE_LABEL = (1 << FILE_LABEL_ID);
        public const int FILE_HELP = (1 << FILE_HELP_ID);
        public const int FILE_DEBUG = (1 << FILE_DEBUG_ID);
        public const int FILE_HANDLING = (1 << FILE_HANDLING_ID);
        public const int FILE_UPDATE_ACTIONS = (1 << FILE_UPDATE_ACTIONS_ID);
        public const int FILE_IDENTITY = (1 << FILE_IDENTITY_ID);

        public const int FILE_ATTR_MASKS = (FILE_MEMBERS | FILE_LABEL | FILE_HELP | FILE_DEBUG | FILE_HANDLING | FILE_UPDATE_ACTIONS | FILE_IDENTITY);

        public override void AllocAttributes(uint attrMask)
        {
        }// to make base item happy

        public DDl6File()
        {
            byItemType = FILE_ITYPE;
            strItemName = "File";
            //pFile = &(glblFlats.fFile); 
        }

        //virtual ~DDl6File(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & FILE_MEMBERS) != 0)
            {
                pDDlAttr = new DDlAttribute("FileMembers", FILE_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);
                attrList.Add(pDDlAttr);
            }



            if ((attrMask & FILE_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("FileLabel", FILE_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & FILE_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("FileHelp", FILE_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & FILE_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("FileDebugData", FILE_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_file_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_FILE file_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case FILE_MEMBERS_ID:
                    //depbin_ptr = &file_flat.depbin.db_members;		
                    file_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    file_flat.depbin.db_members.bin_size = data_len;
                    file_flat.depbin.db_members.bin_offset = attr_offset;
                    break;
                case FILE_LABEL_ID:
                    //depbin_ptr = &file_flat.depbin.db_label;		
                    file_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    file_flat.depbin.db_label.bin_size = data_len;
                    file_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case FILE_HELP_ID:
                    //depbin_ptr = &file_flat.depbin.db_help;		
                    file_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    file_flat.depbin.db_help.bin_size = data_len;
                    file_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case FILE_DEBUG_ID:
                    //depbin_ptr = &file_flat.depbin.db_debug_info;
                    file_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    file_flat.depbin.db_debug_info.bin_size = data_len;
                    file_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_FILE_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            file_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_file_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fFile, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_file_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fFile, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }


        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fFile.masks.bin_exists = attrMask & FILE_ATTR_MASKS;
            glblFlats.fFile.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case FILE_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fFile.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fFile.depbin.db_members.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fFile.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case FILE_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fFile.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fFile.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fFile.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case FILE_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fFile.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fFile.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fFile.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case FILE_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fFile.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fFile.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fFile.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/


            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Chart : DDL6BaseItem  /*Item Type == 21*/
    {
        //FLAT_CHART* pChart;

        /* CHART attributes  SIZE 2 */

        public const int CHART_LABEL_ID = 0;
        public const int CHART_HELP_ID = 1;
        public const int CHART_VALID_ID = 2;
        public const int CHART_HEIGHT_ID = 3;
        public const int CHART_WIDTH_ID = 4;
        public const int CHART_TYPE_ID = 5;
        public const int CHART_LENGTH_ID = 6;
        public const int CHART_CYCLETIME_ID = 7;
        public const int CHART_MEMBERS_ID = 8;/* sources */
        public const int CHART_DEBUG_ID = 9;
        public const int CHART_VISIBLE_ID = 10;
        public const int MAX_CHART_ID = 11;/* must be last in list */
        /* CHART attribute masks */

        public const int CHART_LABEL = (1 << CHART_LABEL_ID);
        public const int CHART_HELP = (1 << CHART_HELP_ID);
        public const int CHART_VALID = (1 << CHART_VALID_ID);
        public const int CHART_HEIGHT = (1 << CHART_HEIGHT_ID);
        public const int CHART_WIDTH = (1 << CHART_WIDTH_ID);
        public const int CHART_TYPE = (1 << CHART_TYPE_ID);
        public const int CHART_LENGTH = (1 << CHART_LENGTH_ID);
        public const int CHART_CYCLETIME = (1 << CHART_CYCLETIME_ID);
        public const int CHART_MEMBERS = (1 << CHART_MEMBERS_ID);
        public const int CHART_DEBUG = (1 << CHART_DEBUG_ID);

        public const int CHART_ATTR_MASKS = (CHART_LABEL | CHART_HELP | CHART_VALID | CHART_HEIGHT | CHART_WIDTH | CHART_TYPE | CHART_LENGTH | CHART_CYCLETIME | CHART_MEMBERS | CHART_DEBUG);
        public DDl6Chart()
        {
            byItemType = CHART_ITYPE;
            strItemName = "Chart";
            //pChart = &(glblFlats.fChart); 
        }

        //virtual ~DDl6Chart(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & CHART_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartLabel", CHART_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & CHART_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartHelp", CHART_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & CHART_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartValidity", CHART_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & CHART_HEIGHT) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartHeight", CHART_HEIGHT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & CHART_WIDTH) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartWidth", CHART_WIDTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & CHART_TYPE) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartType", CHART_TYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_CHART_TYPE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & CHART_LENGTH) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartLength", CHART_LENGTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & CHART_CYCLETIME) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartCycleTime", CHART_CYCLETIME_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & CHART_MEMBERS) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartMembers", CHART_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & CHART_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("ChartDebugData", CHART_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_chart_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_CHART chart_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {

                case CHART_LABEL_ID:
                    //depbin_ptr = &chart_flat.depbin.db_label;		
                    chart_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_label.bin_size = data_len;
                    chart_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case CHART_HELP_ID:
                    //depbin_ptr = &chart_flat.depbin.db_help;		
                    chart_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_help.bin_size = data_len;
                    chart_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case CHART_VALID_ID:
                    //depbin_ptr = &chart_flat.depbin.db_valid;		
                    chart_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_valid.bin_size = data_len;
                    chart_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case CHART_HEIGHT_ID:
                    //depbin_ptr = &chart_flat.depbin.db_height;		
                    chart_flat.depbin.db_height.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_height.bin_size = data_len;
                    chart_flat.depbin.db_height.bin_offset = attr_offset;
                    break;
                case CHART_WIDTH_ID:
                    //depbin_ptr = &chart_flat.depbin.db_width;		
                    chart_flat.depbin.db_width.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_width.bin_size = data_len;
                    chart_flat.depbin.db_width.bin_offset = attr_offset;
                    break;
                case CHART_TYPE_ID:
                    //depbin_ptr = &chart_flat.depbin.db_type;		
                    chart_flat.depbin.db_type.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_type.bin_size = data_len;
                    chart_flat.depbin.db_type.bin_offset = attr_offset;
                    break;
                case CHART_LENGTH_ID:
                    //depbin_ptr = &chart_flat.depbin.db_length;		
                    chart_flat.depbin.db_length.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_length.bin_size = data_len;
                    chart_flat.depbin.db_length.bin_offset = attr_offset;
                    break;
                case CHART_CYCLETIME_ID:
                    //depbin_ptr = &chart_flat.depbin.db_cytime;		
                    chart_flat.depbin.db_cytime.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_cytime.bin_size = data_len;
                    chart_flat.depbin.db_cytime.bin_offset = attr_offset;
                    break;
                case CHART_MEMBERS_ID:
                    //depbin_ptr = &chart_flat.depbin.db_members;		
                    chart_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_members.bin_size = data_len;
                    chart_flat.depbin.db_members.bin_offset = attr_offset;
                    break;
                case CHART_DEBUG_ID:
                    //depbin_ptr = &chart_flat.depbin.db_debug_info;
                    chart_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    chart_flat.depbin.db_debug_info.bin_size = data_len;
                    chart_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_CHART_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            chart_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {


            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();


            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_chart_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fChart, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_chart_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fChart, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fChart.masks.bin_exists = attrMask & CHART_ATTR_MASKS;
            glblFlats.fChart.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case CHART_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_members.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;
                    case CHART_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case CHART_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case CHART_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;

                    case CHART_HEIGHT_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_height.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_height.bin_size;

                            rc = Common.parse_attr_scope_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_height.bin_offset); //Vibhor 260804: Changed to int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;

                    case CHART_WIDTH_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_width.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_width.bin_size;

                            rc = Common.parse_attr_scope_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_width.bin_offset); //Vibhor 260804: Changed to int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;

                    case CHART_TYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_type.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_type.bin_size;

                            rc = Common.parse_attr_chart_type(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_type.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case CHART_LENGTH_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_length.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_length.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_length.bin_offset); //was Common.parse_attr_int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case CHART_CYCLETIME_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_cytime.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_cytime.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_cytime.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case CHART_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fChart.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fChart.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fChart.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;

                    default:
                        break;
                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for */

            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            if ((attrMask & CHART_VALID) == 0)
            {
                pAttribute = new DDlAttribute("ChartValidity", CHART_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }

            //Added By Anil October 25 2005--starts here
            //To make the Chart type as Strip chart if it is not defined
            if ((attrMask & CHART_TYPE) == 0)
            {
                pAttribute = new DDlAttribute("ChartType", CHART_TYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 4; /*Default Attribute Strip chart*/

                attrList.Add(pAttribute);
            }
            //Added By Anil October 25 2005 --Ends here

            //Added By Anil January 03 2006 on Chart Lenght

            if ((attrMask & CHART_LENGTH) == 0)
            {
                /* was ( pre 31jan06 ) ::		
                         pAttribute = new DDlAttribute("ChartLength", CHART_LENGTH_ID, DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                        pAttribute.pVals = new VALUES;
                        pAttribute.pVals.ulVal = 600000; /x*Default Attribute Length of chart*x/
                *** now::  */
                pAttribute = new DDlAttribute("ChartLength", CHART_LENGTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.pExpr = new ddpExpression();

                Element exprElem = new Element();

                exprElem.byElemType = Common.INTCST_OPCODE;
                exprElem.ulConst = 600000;
                pAttribute.pVals.pExpr.Add(exprElem);
                //exprElem.clean();
                exprElem.Cleanup();
                /* end new 31jan06 */

                attrList.Add(pAttribute);
            }

            //Anil 230506: Start of Code for Default value handling
            if ((attrMask & CHART_HEIGHT) == 0)
            {
                pAttribute = new DDlAttribute("ChartHeight", CHART_HEIGHT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.MEDIUM_DISPSIZE; /*Default Attribute for height is MEDIUM*/
                attrList.Add(pAttribute);
            }


            if ((attrMask & CHART_WIDTH) == 0)
            {
                pAttribute = new DDlAttribute("ChartWidth", CHART_WIDTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.MEDIUM_DISPSIZE; /*Default Attribute for Width is MEDIUM*/
                attrList.Add(pAttribute);
            }

            if ((attrMask & CHART_CYCLETIME) == 0)//expr
            {
                pAttribute = new DDlAttribute("ChartCycleTime", CHART_CYCLETIME_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.pExpr = new ddpExpression();

                Element exprElem = new Element();

                exprElem.byElemType = Common.INTCST_OPCODE;
                exprElem.ulConst = 1000;/*Default Attribute for Cycle time is 1 sec*/
                pAttribute.pVals.pExpr.Add(exprElem);
                //exprElem.clean();
                exprElem.Cleanup();
                attrList.Add(pAttribute);
            }

            attrMask = attrMask | CHART_VALID | CHART_TYPE | CHART_LENGTH | CHART_HEIGHT | CHART_WIDTH | CHART_CYCLETIME; //Added by Anil January 03 2006
                                                                                                                          //Anil 230506: End of Code for Default value handling
            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Graph : DDL6BaseItem  /*Item Type == 22*/
    {
        //FLAT_GRAPH* pGraph;

        /* GRAPH attributes SIZE 2 */

        public const int GRAPH_LABEL_ID = 0;
        public const int GRAPH_HELP_ID = 1;
        public const int GRAPH_VALID_ID = 2;
        public const int GRAPH_HEIGHT_ID = 3;
        public const int GRAPH_WIDTH_ID = 4;
        public const int GRAPH_XAXIS_ID = 5;
        public const int GRAPH_MEMBERS_ID = 6; /* Waveforms */
        public const int GRAPH_DEBUG_ID = 7;
        public const int GRAPH_CYCLETIME_ID = 8;   /* 22jan07 sjv - spec change */
        public const int GRAPH_VISIBLE_ID = 9;
        public const int MAX_GRAPH_ID = 10;/* must be last in list */
        /* GRAPH attribute masks */

        public const int GRAPH_LABEL = (1 << GRAPH_LABEL_ID);
        public const int GRAPH_HELP = (1 << GRAPH_HELP_ID);
        public const int GRAPH_VALID = (1 << GRAPH_VALID_ID);
        public const int GRAPH_HEIGHT = (1 << GRAPH_HEIGHT_ID);
        public const int GRAPH_WIDTH = (1 << GRAPH_WIDTH_ID);
        public const int GRAPH_XAXIS = (1 << GRAPH_XAXIS_ID);
        public const int GRAPH_MEMBERS = (1 << GRAPH_MEMBERS_ID);
        public const int GRAPH_DEBUG = (1 << GRAPH_DEBUG_ID);
        public const int GRAPH_CYCLETIME = (1 << GRAPH_CYCLETIME_ID); /* added 23jan07 stevev - spec change*/

        public const int GRAPH_ATTR_MASKS = (GRAPH_LABEL | GRAPH_HELP | GRAPH_VALID | GRAPH_HEIGHT | GRAPH_WIDTH | GRAPH_XAXIS | GRAPH_MEMBERS | GRAPH_CYCLETIME | GRAPH_DEBUG);

        public DDl6Graph()
        {
            byItemType = GRAPH_ITYPE;
            strItemName = "Graph";
            //pGraph = &(glblFlats.fGraph); 
        }

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & GRAPH_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphLabel", GRAPH_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphHelp", GRAPH_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphValidity", GRAPH_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_HEIGHT) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphHeight", GRAPH_HEIGHT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_WIDTH) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphWidth", GRAPH_WIDTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_XAXIS) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphXaxis", GRAPH_XAXIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_MEMBERS) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphMembers", GRAPH_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_CYCLETIME) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphCycleTime", GRAPH_CYCLETIME_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRAPH_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("GraphDebugData", GRAPH_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_graph_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_GRAPH graph_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case GRAPH_LABEL_ID:
                    //depbin_ptr = &graph_flat.depbin.db_label;		
                    graph_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_label.bin_size = data_len;
                    graph_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case GRAPH_HELP_ID:
                    //depbin_ptr = &graph_flat.depbin.db_help;		
                    graph_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_help.bin_size = data_len;
                    graph_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case GRAPH_VALID_ID:
                    //depbin_ptr = &graph_flat.depbin.db_valid;		
                    graph_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_valid.bin_size = data_len;
                    graph_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case GRAPH_HEIGHT_ID:
                    //depbin_ptr = &graph_flat.depbin.db_height;		
                    graph_flat.depbin.db_height.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_height.bin_size = data_len;
                    graph_flat.depbin.db_height.bin_offset = attr_offset;
                    break;
                case GRAPH_WIDTH_ID:
                    //depbin_ptr = &graph_flat.depbin.db_width;		
                    graph_flat.depbin.db_width.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_width.bin_size = data_len;
                    graph_flat.depbin.db_width.bin_offset = attr_offset;
                    break;
                case GRAPH_XAXIS_ID:
                    //depbin_ptr = &graph_flat.depbin.db_x_axis;		
                    graph_flat.depbin.db_x_axis.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_x_axis.bin_size = data_len;
                    graph_flat.depbin.db_x_axis.bin_offset = attr_offset;
                    break;
                case GRAPH_MEMBERS_ID:
                    //depbin_ptr = &graph_flat.depbin.db_members;		
                    graph_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_members.bin_size = data_len;
                    graph_flat.depbin.db_members.bin_offset = attr_offset;
                    break;

                case GRAPH_CYCLETIME_ID:
                    //depbin_ptr = &graph_flat.depbin.db_cytime;		
                    graph_flat.depbin.db_cytime.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_cytime.bin_size = data_len;
                    graph_flat.depbin.db_cytime.bin_offset = attr_offset;
                    break;
                case GRAPH_DEBUG_ID:
                    //depbin_ptr = &graph_flat.depbin.db_debug_info;
                    graph_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    graph_flat.depbin.db_debug_info.bin_size = data_len;
                    graph_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_GRAPH_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            graph_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_graph_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fGraph, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_graph_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fGraph, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fGraph.masks.bin_exists = attrMask & GRAPH_ATTR_MASKS;
            glblFlats.fGraph.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case GRAPH_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_members.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;
                    case GRAPH_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRAPH_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case GRAPH_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;

                    case GRAPH_HEIGHT_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_height.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_height.bin_size;

                            rc = Common.parse_attr_scope_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_height.bin_offset); //Vibhor 260804: Changed to int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;

                    case GRAPH_WIDTH_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_width.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_width.bin_size;

                            rc = Common.parse_attr_scope_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_width.bin_offset);    //Vibhor 260804: Changed to int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;

                    case GRAPH_XAXIS_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_x_axis.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_x_axis.bin_size;

                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_x_axis.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case GRAPH_CYCLETIME_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_cytime.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_cytime.bin_size;


                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_cytime.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case GRAPH_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fGraph.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fGraph.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGraph.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;

                    default:
                        break;
                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for */

            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            if ((attrMask & GRAPH_VALID) == 0)
            {
                pAttribute = new DDlAttribute("GraphValidity", GRAPH_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);


                pAttribute.pVals = new VALUES();

                pAttribute.pVals.ullVal = 1; /*Default Attribute*/

                attrList.Add(pAttribute);
            }
            //Anil 230506: Start of Code for Default value handling
            if ((attrMask & GRAPH_HEIGHT) == 0)
            {
                pAttribute = new DDlAttribute("GraphHeight", GRAPH_HEIGHT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.MEDIUM_DISPSIZE; /*Default Attribute for Graph height is MEDIUM*/
                attrList.Add(pAttribute);
            }


            if ((attrMask & GRAPH_WIDTH) == 0)
            {
                pAttribute = new DDlAttribute("GraphWidth", GRAPH_WIDTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.MEDIUM_DISPSIZE; /*Default Attribute for Graph is MEDIUM*/
                attrList.Add(pAttribute);
            }

            attrMask = attrMask | GRAPH_VALID | GRAPH_WIDTH | GRAPH_HEIGHT;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Axis : DDL6BaseItem   /*Item Type == 23*/
    {
        //FLAT_AXIS* pAxis;

        /* AXIS attributes - SIZE 2 */

        public const int AXIS_LABEL_ID = 0;
        public const int AXIS_HELP_ID = 1;
        public const int AXIS_VALID_ID = 2;
        public const int AXIS_MINVAL_ID = 3;
        public const int AXIS_MAXVAL_ID = 4;
        public const int AXIS_SCALING_ID = 5;
        public const int AXIS_CONSTUNIT_ID = 6;
        public const int AXIS_DEBUG_ID = 7;
        /* new 12apr05 */
        public const int AXIS_VIEW_MIN_VAL_ID = 8; /* NON-DD internal attribute */
        public const int AXIS_VIEW_MAX_VAL_ID = 9;/* NON-DD internal attribute */
        public const int MAX_AXIS_ID = 10;/* must be last in list */
        /* AXIS attribute masks */

        public const int AXIS_LABEL = (1 << AXIS_LABEL_ID);
        public const int AXIS_HELP = (1 << AXIS_HELP_ID);
        public const int AXIS_VALID = (1 << AXIS_VALID_ID);
        public const int AXIS_MINVAL = (1 << AXIS_MINVAL_ID);
        public const int AXIS_MAXVAL = (1 << AXIS_MAXVAL_ID);
        public const int AXIS_SCALING = (1 << AXIS_SCALING_ID);
        public const int AXIS_CONSTUNIT = (1 << AXIS_CONSTUNIT_ID);
        public const int AXIS_DEBUG = (1 << AXIS_DEBUG_ID);

        public const int AXIS_ATTR_MASKS = (AXIS_LABEL | AXIS_HELP | AXIS_VALID | AXIS_MINVAL | AXIS_MAXVAL | AXIS_SCALING | AXIS_CONSTUNIT | AXIS_DEBUG);

        public DDl6Axis()
        {
            byItemType = AXIS_ITYPE;
            strItemName = "Axis";
            //pAxis = &(glblFlats.fAxis); 
        }

        //virtual ~DDl6Axis(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & AXIS_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisLabel", AXIS_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & AXIS_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisHelp", AXIS_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & AXIS_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisValidity", AXIS_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & AXIS_MINVAL) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisMinVal", AXIS_MINVAL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & AXIS_MAXVAL) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisMaxVal", AXIS_MAXVAL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & AXIS_SCALING) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisScaling", AXIS_SCALING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & AXIS_CONSTUNIT) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisConstantUnit", AXIS_CONSTUNIT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & AXIS_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("AxisDebugData", AXIS_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_axis_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_AXIS axis_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case AXIS_LABEL_ID:
                    //depbin_ptr = &axis_flat.depbin.db_label;		
                    axis_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_label.bin_size = data_len;
                    axis_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case AXIS_HELP_ID:
                    //depbin_ptr = &axis_flat.depbin.db_help;		
                    axis_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_help.bin_size = data_len;
                    axis_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case AXIS_VALID_ID:
                    //depbin_ptr = &axis_flat.depbin.db_valid;		
                    axis_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_valid.bin_size = data_len;
                    axis_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case AXIS_MINVAL_ID:
                    //depbin_ptr = &axis_flat.depbin.db_minval;		
                    axis_flat.depbin.db_minval.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_minval.bin_size = data_len;
                    axis_flat.depbin.db_minval.bin_offset = attr_offset;
                    break;
                case AXIS_MAXVAL_ID:
                    //depbin_ptr = &axis_flat.depbin.db_maxval;		
                    axis_flat.depbin.db_maxval.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_maxval.bin_size = data_len;
                    axis_flat.depbin.db_maxval.bin_offset = attr_offset;
                    break;
                case AXIS_SCALING_ID:
                    //depbin_ptr = &axis_flat.depbin.db_scaling;		
                    axis_flat.depbin.db_scaling.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_scaling.bin_size = data_len;
                    axis_flat.depbin.db_scaling.bin_offset = attr_offset;
                    break;
                case AXIS_CONSTUNIT_ID:
                    //depbin_ptr = &axis_flat.depbin.db_unit;		
                    axis_flat.depbin.db_unit.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_unit.bin_size = data_len;
                    axis_flat.depbin.db_unit.bin_offset = attr_offset;
                    break;
                case AXIS_DEBUG_ID:
                    //depbin_ptr = &axis_flat.depbin.db_debug_info;
                    axis_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    axis_flat.depbin.db_debug_info.bin_size = data_len;
                    axis_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_AXIS_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            axis_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_axis_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fAxis, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_axis_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fAxis, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fAxis.masks.bin_exists = attrMask & AXIS_ATTR_MASKS;
            glblFlats.fAxis.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case AXIS_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;

                    case AXIS_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;
                    /***** axis no longer has validity - 23jan07 sjv - spec change ***
                    *			case	AXIS_VALID_ID:
                    *				{
                    *					AttrChunkPtr = glblFlats.fAxis.depbin.db_valid.bin_chunk;
                    *					ulChunkSize  = glblFlats.fAxis.depbin.db_valid.bin_size;
                    *
                    *					rc = Common.parse_attr_ulong((*p),AttrChunkPtr,ulChunkSize, glblFlats.fAxis.depbin.db_valid.bin_offset);
                    *					if(rc != Common.DDL_SUCCESS) 
                    *						return rc; 
                    *				}
                    *
                    *				break;
                    *******************/
                    case AXIS_MINVAL_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_minval.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_minval.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_minval.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;

                    case AXIS_MAXVAL_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_maxval.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_maxval.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_maxval.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;

                    case AXIS_SCALING_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_scaling.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_scaling.bin_size;

                            rc = Common.parse_attr_int(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_scaling.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;

                    case AXIS_CONSTUNIT_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_unit.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_unit.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_unit.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }

                        break;

                    case AXIS_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fAxis.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fAxis.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fAxis.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }

                        break;

                    default:
                        break;
                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/


            /*See if we didn't get validity, default it to true and push it onto the attribute List*/
            /** spec change: removed
            * 	if (!(attrMask & AXIS_VALID))
            *	 {
            *		 pAttribute = (DDlAttribute*)new DDlAttribute("AxisValidity",
            *										AXIS_VALID_ID,
            *										DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG,
            *										false);			
            *		pAttribute.pVals = new VALUES;		
            *		pAttribute.pVals.ulVal = 1; //Default Attribute
            *		attrList.Add(pAttribute);
            *	 }
            **** end spec change ***/
            //Anil 230506: Start of Code for Default value handling
            if ((attrMask & AXIS_SCALING) == 0)
            {
                pAttribute = new DDlAttribute("AxisScaling", AXIS_SCALING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.LINEAR_SCALE; /*Default Attribute*/
                attrList.Add(pAttribute);
            }
            attrMask = attrMask | AXIS_SCALING;// spec change 23jan07, sjv  | AXIS_VALID 

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Waveform : DDL6BaseItem   /*Item Type == 24*/
    {
        //FLAT_WAVEFORM* pWaveFrm;

        /* WAVEFORM attributes  SIZE 3 */

        public const int WAVEFORM_LABEL_ID = 0;
        public const int WAVEFORM_HELP_ID = 1;
        public const int WAVEFORM_HANDLING_ID = 2;
        public const int WAVEFORM_EMPHASIS_ID = 3;
        public const int WAVEFORM_LINETYPE_ID = 4;
        public const int WAVEFORM_LINECOLOR_ID = 5;
        public const int WAVEFORM_YAXIS_ID = 6;
        public const int WAVEFORM_KEYPTS_X_ID = 7;
        public const int WAVEFORM_KEYPTS_Y_ID = 8;
        public const int WAVEFORM_TYPE_ID = 9;
        public const int WAVEFORM_X_VALUES_ID = 10;
        public const int WAVEFORM_Y_VALUES_ID = 11;
        public const int WAVEFORM_X_INITIAL_ID = 12;
        public const int WAVEFORM_X_INCREMENT_ID = 13;
        public const int WAVEFORM_POINT_COUNT_ID = 14;
        public const int WAVEFORM_INIT_ACTIONS_ID = 15;
        public const int WAVEFORM_RFRSH_ACTIONS_ID = 16;
        public const int WAVEFORM_EXIT_ACTIONS_ID = 17;
        public const int WAVEFORM_DEBUG_ID = 18;
        public const int WAVEFORM_VALID_ID = 19;// added 22jan07 - sjv - spec change
        public const int MAX_WAVEFORM_ID = 20; /* must be last in list */
        /* WAVEFORM attribute masks */

        public const int WAVEFORM_LABEL = (1 << WAVEFORM_LABEL_ID);
        public const int WAVEFORM_HELP = (1 << WAVEFORM_HELP_ID);
        public const int WAVEFORM_HANDLING = (1 << WAVEFORM_HANDLING_ID);
        public const int WAVEFORM_EMPHASIS = (1 << WAVEFORM_EMPHASIS_ID);
        public const int WAVEFORM_LINETYPE = (1 << WAVEFORM_LINETYPE_ID);
        public const int WAVEFORM_LINECOLOR = (1 << WAVEFORM_LINECOLOR_ID);
        public const int WAVEFORM_YAXIS = (1 << WAVEFORM_YAXIS_ID);
        public const int WAVEFORM_KEYPTS_X = (1 << WAVEFORM_KEYPTS_X_ID);
        public const int WAVEFORM_KEYPTS_Y = (1 << WAVEFORM_KEYPTS_Y_ID);
        public const int WAVEFORM_TYPE = (1 << WAVEFORM_TYPE_ID);
        public const int WAVEFORM_X_VALUES = (1 << WAVEFORM_X_VALUES_ID);
        public const int WAVEFORM_Y_VALUES = (1 << WAVEFORM_Y_VALUES_ID);
        public const int WAVEFORM_X_INITIAL = (1 << WAVEFORM_X_INITIAL_ID);
        public const int WAVEFORM_X_INCREMENT = (1 << WAVEFORM_X_INCREMENT_ID);
        public const int WAVEFORM_POINT_COUNT = (1 << WAVEFORM_POINT_COUNT_ID);
        public const int WAVEFORM_INIT_ACTIONS = (1 << WAVEFORM_INIT_ACTIONS_ID);
        public const int WAVEFORM_RFRSH_ACTIONS = (1 << WAVEFORM_RFRSH_ACTIONS_ID);
        public const int WAVEFORM_EXIT_ACTIONS = (1 << WAVEFORM_EXIT_ACTIONS_ID);
        public const int WAVEFORM_DEBUG = (1 << WAVEFORM_DEBUG_ID);
        public const int WAVEFORM_VALID = (1 << WAVEFORM_VALID_ID);

        public const int WAVEFORM_ATTR_MASKS = (WAVEFORM_LABEL | WAVEFORM_HELP | WAVEFORM_HANDLING | WAVEFORM_EMPHASIS | WAVEFORM_LINETYPE | WAVEFORM_LINECOLOR | WAVEFORM_YAXIS | WAVEFORM_KEYPTS_X | WAVEFORM_KEYPTS_Y | WAVEFORM_TYPE | WAVEFORM_X_VALUES | WAVEFORM_Y_VALUES | WAVEFORM_X_INITIAL | WAVEFORM_X_INCREMENT | WAVEFORM_POINT_COUNT | WAVEFORM_INIT_ACTIONS | WAVEFORM_RFRSH_ACTIONS | WAVEFORM_EXIT_ACTIONS | WAVEFORM_VALID | WAVEFORM_DEBUG);
        public DDl6Waveform()
        {
            byItemType = WAVEFORM_ITYPE;
            strItemName = "Waveform";
            //pWaveFrm = &(glblFlats.fWaveFrm); 
        }

        //virtual ~DDl6Waveform(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & WAVEFORM_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformLabel", WAVEFORM_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformHelp", WAVEFORM_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_HANDLING) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformHandling", WAVEFORM_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_EMPHASIS) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformEmphasis", WAVEFORM_EMPHASIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, //Assuming bool will be encoded as int only
                                                false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_LINETYPE) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformLineType", WAVEFORM_LINETYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_LINE_TYPE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_LINECOLOR) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformLineColor", WAVEFORM_LINECOLOR_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_YAXIS) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformYAxis", WAVEFORM_YAXIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_KEYPTS_X) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformKeyPointsX", WAVEFORM_KEYPTS_X_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_KEYPTS_Y) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformKeyPointsY", WAVEFORM_KEYPTS_Y_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_TYPE) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformType", WAVEFORM_TYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_WAVEFORM_TYPE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_X_VALUES) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformXVals", WAVEFORM_X_VALUES_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_Y_VALUES) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformYVals", WAVEFORM_Y_VALUES_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_X_INITIAL) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformXInitial", WAVEFORM_X_INITIAL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_X_INCREMENT) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformXIncr", WAVEFORM_X_INCREMENT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_POINT_COUNT) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformPtCnt", WAVEFORM_POINT_COUNT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_INIT_ACTIONS) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformInitActions", WAVEFORM_INIT_ACTIONS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_RFRSH_ACTIONS) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformRfrshActions", WAVEFORM_RFRSH_ACTIONS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_EXIT_ACTIONS) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformExitActions", WAVEFORM_EXIT_ACTIONS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformDebugData", WAVEFORM_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & WAVEFORM_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("WaveformValidity", WAVEFORM_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_waveform_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_WAVEFORM wavfrm_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case WAVEFORM_LABEL_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_label;		
                    wavfrm_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_label.bin_size = data_len;
                    wavfrm_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case WAVEFORM_HELP_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_help;		
                    wavfrm_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_help.bin_size = data_len;
                    wavfrm_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case WAVEFORM_VALID_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_valid;		
                    wavfrm_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_valid.bin_size = data_len;
                    wavfrm_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case WAVEFORM_HANDLING_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_handling;		
                    wavfrm_flat.depbin.db_handling.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_handling.bin_size = data_len;
                    wavfrm_flat.depbin.db_handling.bin_offset = attr_offset;
                    break;
                case WAVEFORM_EMPHASIS_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_emphasis;		
                    wavfrm_flat.depbin.db_emphasis.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_emphasis.bin_size = data_len;
                    wavfrm_flat.depbin.db_emphasis.bin_offset = attr_offset;
                    break;
                case WAVEFORM_LINETYPE_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_linetype;		
                    wavfrm_flat.depbin.db_linetype.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_linetype.bin_size = data_len;
                    wavfrm_flat.depbin.db_linetype.bin_offset = attr_offset;
                    break;
                case WAVEFORM_LINECOLOR_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_linecolor;		
                    wavfrm_flat.depbin.db_linecolor.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_linecolor.bin_size = data_len;
                    wavfrm_flat.depbin.db_linecolor.bin_offset = attr_offset;
                    break;
                case WAVEFORM_YAXIS_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_y_axis;		
                    wavfrm_flat.depbin.db_y_axis.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_y_axis.bin_size = data_len;
                    wavfrm_flat.depbin.db_y_axis.bin_offset = attr_offset;
                    break;
                case WAVEFORM_KEYPTS_X_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_x_keypts;
                    wavfrm_flat.depbin.db_x_keypts.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_x_keypts.bin_size = data_len;
                    wavfrm_flat.depbin.db_x_keypts.bin_offset = attr_offset;
                    break;
                case WAVEFORM_KEYPTS_Y_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_y_keypts;
                    wavfrm_flat.depbin.db_y_keypts.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_y_keypts.bin_size = data_len;
                    wavfrm_flat.depbin.db_y_keypts.bin_offset = attr_offset;
                    break;
                case WAVEFORM_TYPE_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_type;
                    wavfrm_flat.depbin.db_type.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_type.bin_size = data_len;
                    wavfrm_flat.depbin.db_type.bin_offset = attr_offset;
                    break;
                case WAVEFORM_X_VALUES_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_x_values;
                    wavfrm_flat.depbin.db_x_values.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_x_values.bin_size = data_len;
                    wavfrm_flat.depbin.db_x_values.bin_offset = attr_offset;
                    break;
                case WAVEFORM_Y_VALUES_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_y_values;
                    wavfrm_flat.depbin.db_y_values.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_y_values.bin_size = data_len;
                    wavfrm_flat.depbin.db_y_values.bin_offset = attr_offset;
                    break;
                case WAVEFORM_X_INITIAL_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_x_initial;
                    wavfrm_flat.depbin.db_x_initial.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_x_initial.bin_size = data_len;
                    wavfrm_flat.depbin.db_x_initial.bin_offset = attr_offset;
                    break;
                case WAVEFORM_X_INCREMENT_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_x_incr;
                    wavfrm_flat.depbin.db_x_incr.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_x_incr.bin_size = data_len;
                    wavfrm_flat.depbin.db_x_incr.bin_offset = attr_offset;
                    break;
                case WAVEFORM_POINT_COUNT_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_pt_count;
                    wavfrm_flat.depbin.db_pt_count.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_pt_count.bin_size = data_len;
                    wavfrm_flat.depbin.db_pt_count.bin_offset = attr_offset;
                    break;
                case WAVEFORM_INIT_ACTIONS_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_init_acts;
                    wavfrm_flat.depbin.db_init_acts.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_init_acts.bin_size = data_len;
                    wavfrm_flat.depbin.db_init_acts.bin_offset = attr_offset;
                    break;
                case WAVEFORM_RFRSH_ACTIONS_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_rfrsh_acts;
                    wavfrm_flat.depbin.db_rfrsh_acts.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_rfrsh_acts.bin_size = data_len;
                    wavfrm_flat.depbin.db_rfrsh_acts.bin_offset = attr_offset;
                    break;
                case WAVEFORM_EXIT_ACTIONS_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_exit_acts;
                    wavfrm_flat.depbin.db_exit_acts.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_exit_acts.bin_size = data_len;
                    wavfrm_flat.depbin.db_exit_acts.bin_offset = attr_offset;
                    break;
                case WAVEFORM_DEBUG_ID:
                    //depbin_ptr = &wavfrm_flat.depbin.db_debug_info;
                    wavfrm_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    wavfrm_flat.depbin.db_debug_info.bin_size = data_len;
                    wavfrm_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_WAVEFORM_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            wavfrm_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_waveform_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fWaveFrm, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_waveform_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fWaveFrm, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fWaveFrm.masks.bin_exists = attrMask & WAVEFORM_ATTR_MASKS;
            glblFlats.fWaveFrm.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case WAVEFORM_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_HANDLING_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_handling.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_handling.bin_size;

                            rc = Common.parse_attr_bitstring(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_handling.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_EMPHASIS_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_emphasis.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_emphasis.bin_size;

                            rc = Common.parse_attr_int(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_emphasis.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_LINETYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_linetype.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_linetype.bin_size;

                            rc = Common.parse_attr_line_type(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_linetype.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_LINECOLOR_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_linecolor.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_linecolor.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_linecolor.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_YAXIS_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_y_axis.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_y_axis.bin_size;

                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_y_axis.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_KEYPTS_X_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_x_keypts.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_x_keypts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_x_keypts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_KEYPTS_Y_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_y_keypts.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_y_keypts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_y_keypts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_TYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_type.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_type.bin_size;

                            rc = Common.parse_attr_wavefrm_type(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_type.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_X_VALUES_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_x_values.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_x_values.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_x_values.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_Y_VALUES_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_y_values.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_y_values.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_y_values.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case WAVEFORM_X_INITIAL_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_x_initial.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_x_initial.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_x_initial.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_X_INCREMENT_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_x_incr.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_x_incr.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_x_incr.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_POINT_COUNT_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_pt_count.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_pt_count.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_pt_count.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_INIT_ACTIONS_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_init_acts.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_init_acts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_init_acts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_RFRSH_ACTIONS_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_rfrsh_acts.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_rfrsh_acts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_rfrsh_acts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_EXIT_ACTIONS_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_exit_acts.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_exit_acts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_exit_acts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case WAVEFORM_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    case WAVEFORM_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fWaveFrm.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fWaveFrm.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fWaveFrm.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/
            //Anil 230506: Start of Code for Default value handling

            if ((attrMask & WAVEFORM_VALID) == 0)   // added 23jan07 - sjv - spec change
            {
                pAttribute = new DDlAttribute("WaveformValidity", WAVEFORM_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1; //Default Attribute
                attrList.Add(pAttribute);
            }

            if ((attrMask & WAVEFORM_HANDLING) == 0)
            {
                pAttribute = new DDlAttribute("WaveformHandling", WAVEFORM_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.READ_HANDLING | Common.WRITE_HANDLING;
                attrList.Add(pAttribute);

            }

            if ((attrMask & WAVEFORM_EMPHASIS) == 0)
            {
                pAttribute = new DDlAttribute("WaveformEmphasis", WAVEFORM_EMPHASIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1;
                attrList.Add(pAttribute);
            }
            attrMask = attrMask | WAVEFORM_HANDLING | WAVEFORM_EMPHASIS;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Source : DDL6BaseItem /*Item Type == 25*/
    {
        //FLAT_SOURCE* pSource;

        /* SOURCE attributes SIZE 2 */

        public const int SOURCE_LABEL_ID = 0;
        public const int SOURCE_HELP_ID = 1;
        public const int SOURCE_VALID_ID = 2;
        public const int SOURCE_EMPHASIS_ID = 3;
        public const int SOURCE_LINETYPE_ID = 4;
        public const int SOURCE_LINECOLOR_ID = 5;
        public const int SOURCE_YAXIS_ID = 6;
        public const int SOURCE_MEMBERS_ID = 7;    /* variable/val-array-item */
        public const int SOURCE_DEBUG_ID = 8;
        public const int SOURCE_INIT_ACTIONS_ID = 9;   /* added 22jan07, sjv - spec change*/
        public const int SOURCE_RFRSH_ACTIONS_ID = 10;/* added 22jan07, sjv - spec change*/
        public const int SOURCE_EXIT_ACTIONS_ID = 11;/* added 22jan07, sjv - spec change*/
        public const int MAX_SOURCE_ID = 12;/* must be last in list */
        /* SOURCE attribute masks */

        public const int SOURCE_LABEL = (1 << SOURCE_LABEL_ID);
        public const int SOURCE_HELP = (1 << SOURCE_HELP_ID);
        public const int SOURCE_VALID = (1 << SOURCE_VALID_ID);
        public const int SOURCE_EMPHASIS = (1 << SOURCE_EMPHASIS_ID);
        public const int SOURCE_LINETYPE = (1 << SOURCE_LINETYPE_ID);
        public const int SOURCE_LINECOLOR = (1 << SOURCE_LINECOLOR_ID);
        public const int SOURCE_YAXIS = (1 << SOURCE_YAXIS_ID);
        public const int SOURCE_MEMBERS = (1 << SOURCE_MEMBERS_ID);
        public const int SOURCE_DEBUG = (1 << SOURCE_DEBUG_ID);
        public const int SOURCE_INIT_ACTIONS = (1 << SOURCE_INIT_ACTIONS_ID);
        public const int SOURCE_RFRSH_ACTIONS = (1 << SOURCE_RFRSH_ACTIONS_ID);
        public const int SOURCE_EXIT_ACTIONS = (1 << SOURCE_EXIT_ACTIONS_ID);

        public const int SOURCE_ATTR_MASKS = (SOURCE_LABEL | SOURCE_HELP | SOURCE_VALID | SOURCE_EMPHASIS | SOURCE_LINETYPE
                    | SOURCE_LINECOLOR | SOURCE_YAXIS | SOURCE_MEMBERS | SOURCE_INIT_ACTIONS | SOURCE_RFRSH_ACTIONS | SOURCE_EXIT_ACTIONS | SOURCE_DEBUG);
        public DDl6Source()
        {
            byItemType = SOURCE_ITYPE;
            strItemName = "Source";
            //pSource = &(glblFlats.fSource); 
        }

        //virtual ~DDl6Source(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & SOURCE_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceLabel", SOURCE_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceHelp", SOURCE_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceValidity", SOURCE_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_EMPHASIS) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceEmphasis", SOURCE_EMPHASIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_LINETYPE) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceLineType", SOURCE_LINETYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_LINE_TYPE, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & SOURCE_LINECOLOR) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceLineColor", SOURCE_LINECOLOR_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & SOURCE_YAXIS) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceYAxis", SOURCE_YAXIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_INIT_ACTIONS) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceInitActions", SOURCE_INIT_ACTIONS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_RFRSH_ACTIONS) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceRfrshActions", SOURCE_RFRSH_ACTIONS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_EXIT_ACTIONS) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceExitActions", SOURCE_EXIT_ACTIONS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_MEMBERS) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceMembers", SOURCE_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & SOURCE_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("SourceDebugData", SOURCE_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

        }

        public static int attach_source_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_SOURCE source_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case SOURCE_LABEL_ID:
                    //depbin_ptr = &source_flat.depbin.db_label;		
                    source_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_label.bin_size = data_len;
                    source_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case SOURCE_HELP_ID:
                    //depbin_ptr = &source_flat.depbin.db_help;		
                    source_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_help.bin_size = data_len;
                    source_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case SOURCE_VALID_ID:
                    //depbin_ptr = &source_flat.depbin.db_valid;		
                    source_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_valid.bin_size = data_len;
                    source_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case SOURCE_EMPHASIS_ID:
                    //depbin_ptr = &source_flat.depbin.db_emphasis;		
                    source_flat.depbin.db_emphasis.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_emphasis.bin_size = data_len;
                    source_flat.depbin.db_emphasis.bin_offset = attr_offset;
                    break;
                case SOURCE_LINETYPE_ID:
                    //depbin_ptr = &source_flat.depbin.db_linetype;		
                    source_flat.depbin.db_linetype.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_linetype.bin_size = data_len;
                    source_flat.depbin.db_linetype.bin_offset = attr_offset;
                    break;
                case SOURCE_LINECOLOR_ID:
                    //depbin_ptr = &source_flat.depbin.db_linecolor;		
                    source_flat.depbin.db_linecolor.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_linecolor.bin_size = data_len;
                    source_flat.depbin.db_linecolor.bin_offset = attr_offset;
                    break;
                case SOURCE_YAXIS_ID:
                    //depbin_ptr = &source_flat.depbin.db_y_axis;		
                    source_flat.depbin.db_y_axis.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_y_axis.bin_size = data_len;
                    source_flat.depbin.db_y_axis.bin_offset = attr_offset;
                    break;
                case SOURCE_INIT_ACTIONS_ID:
                    //depbin_ptr = &source_flat.depbin.db_init_acts;		
                    source_flat.depbin.db_init_acts.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_init_acts.bin_size = data_len;
                    source_flat.depbin.db_init_acts.bin_offset = attr_offset;
                    break;
                case SOURCE_RFRSH_ACTIONS_ID:
                    //depbin_ptr = &source_flat.depbin.db_rfrsh_acts;		
                    source_flat.depbin.db_rfrsh_acts.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_rfrsh_acts.bin_size = data_len;
                    source_flat.depbin.db_rfrsh_acts.bin_offset = attr_offset;
                    break;
                case SOURCE_EXIT_ACTIONS_ID:
                    //depbin_ptr = &source_flat.depbin.db_exit_acts;		
                    source_flat.depbin.db_exit_acts.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_exit_acts.bin_size = data_len;
                    source_flat.depbin.db_exit_acts.bin_offset = attr_offset;
                    break;
                case SOURCE_MEMBERS_ID:
                    //depbin_ptr = &source_flat.depbin.db_members;		
                    source_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_members.bin_size = data_len;
                    source_flat.depbin.db_members.bin_offset = attr_offset;
                    break;
                case SOURCE_DEBUG_ID:
                    //depbin_ptr = &source_flat.depbin.db_debug_info;
                    source_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    source_flat.depbin.db_debug_info.bin_size = data_len;
                    source_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_SOURCE_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            source_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_source_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fSource, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_source_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fSource, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fSource.masks.bin_exists = attrMask & SOURCE_ATTR_MASKS;
            glblFlats.fSource.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case SOURCE_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_EMPHASIS_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_emphasis.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_emphasis.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_emphasis.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_LINETYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_linetype.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_linetype.bin_size;

                            rc = Common.parse_attr_line_type(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_linetype.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_LINECOLOR_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_linecolor.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_linecolor.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_linecolor.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_YAXIS_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_y_axis.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_y_axis.bin_size;

                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_y_axis.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_INIT_ACTIONS_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_init_acts.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_init_acts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_init_acts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_RFRSH_ACTIONS_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_rfrsh_acts.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_rfrsh_acts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_rfrsh_acts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_EXIT_ACTIONS_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_exit_acts.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_exit_acts.bin_size;

                            rc = Common.parse_attr_reference_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_exit_acts.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case SOURCE_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_members.bin_size;

                            rc = Common.parse_attr_member_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case SOURCE_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fSource.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fSource.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fSource.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/
            //Anil 230506: Start of Code for Default value handling
            if ((attrMask & SOURCE_VALID) == 0)
            {
                pAttribute = new DDlAttribute("SourceValidity", SOURCE_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1;
                attrList.Add(pAttribute);
            }
            if ((attrMask & SOURCE_EMPHASIS) == 0)
            {
                pAttribute = new DDlAttribute("SourceEmphasis", SOURCE_EMPHASIS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1;
                attrList.Add(pAttribute);
            }

            attrMask = attrMask | SOURCE_VALID | SOURCE_EMPHASIS;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6List : DDL6BaseItem   /*Item Type == 26*/
    {
        //FLAT_LIST* pList;

        /* LIST attributes  SIZE 2 */

        public const int LIST_LABEL_ID = 0;
        public const int LIST_HELP_ID = 1;
        public const int LIST_VALID_ID = 2;
        public const int LIST_TYPE_ID = 3;
        public const int LIST_COUNT_ID = 4;
        public const int LIST_CAPACITY_ID = 5;
        public const int LIST_DEBUG_ID = 6;
        /* new 12apr05 */
        public const int LIST_FIRST_ID = 7;    /* NON-DD internal attribute */
        public const int LIST_LAST_ID = 8; /* NON-DD internal attribute */
        public const int LIST_PRIVATE_ID = 9;
        public const int MAX_LIST_ID = 10; /* must be last in list */
        /* LIST attribute masks */

        public const int LIST_LABEL = (1 << LIST_LABEL_ID);
        public const int LIST_HELP = (1 << LIST_HELP_ID);
        public const int LIST_VALID = (1 << LIST_VALID_ID);
        public const int LIST_TYPE = (1 << LIST_TYPE_ID);
        public const int LIST_COUNT = (1 << LIST_COUNT_ID);
        public const int LIST_CAPACITY = (1 << LIST_CAPACITY_ID);
        public const int LIST_DEBUG = (1 << LIST_DEBUG_ID);

        public const int LIST_ATTR_MASKS = (LIST_LABEL | LIST_HELP | LIST_VALID | LIST_TYPE | LIST_COUNT | LIST_CAPACITY | LIST_DEBUG);

        public DDl6List()
        {
            byItemType = LIST_ITYPE;
            strItemName = "List";
            //pList = &(glblFlats.fList); 
        }

        //virtual ~DDl6List(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & LIST_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("ListLabel", LIST_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & LIST_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("ListHelp", LIST_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & LIST_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("ListValidity", LIST_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & LIST_TYPE) != 0)
            {
                pDDlAttr = new DDlAttribute("ListType", LIST_TYPE_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & LIST_COUNT) != 0)
            {
                pDDlAttr = new DDlAttribute("ListCount", LIST_COUNT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & LIST_CAPACITY) != 0)
            {
                pDDlAttr = new DDlAttribute("ListCapacity", LIST_CAPACITY_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & LIST_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("ListDebugData", LIST_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }

        }

        public static int attach_list_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_LIST list_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case LIST_LABEL_ID:
                    //depbin_ptr = &list_flat.depbin.db_label;		
                    list_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_label.bin_size = data_len;
                    list_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case LIST_HELP_ID:
                    //depbin_ptr = &list_flat.depbin.db_help;		
                    list_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_help.bin_size = data_len;
                    list_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case LIST_VALID_ID:
                    //depbin_ptr = &list_flat.depbin.db_valid;		
                    list_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_valid.bin_size = data_len;
                    list_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case LIST_TYPE_ID:
                    //depbin_ptr = &list_flat.depbin.db_type;		
                    list_flat.depbin.db_type.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_type.bin_size = data_len;
                    list_flat.depbin.db_type.bin_offset = attr_offset;
                    break;
                case LIST_COUNT_ID:
                    //depbin_ptr = &list_flat.depbin.db_count;		
                    list_flat.depbin.db_count.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_count.bin_size = data_len;
                    list_flat.depbin.db_count.bin_offset = attr_offset;
                    break;
                case LIST_CAPACITY_ID:
                    //depbin_ptr = &list_flat.depbin.db_capacity;		
                    list_flat.depbin.db_capacity.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_capacity.bin_size = data_len;
                    list_flat.depbin.db_capacity.bin_offset = attr_offset;
                    break;
                case LIST_DEBUG_ID:
                    //depbin_ptr = &list_flat.depbin.db_debug_info;
                    list_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    list_flat.depbin.db_debug_info.bin_size = data_len;
                    list_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_LIST_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            list_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_list_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fList, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_list_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fList, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fList.masks.bin_exists = attrMask & LIST_ATTR_MASKS;
            glblFlats.fList.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case LIST_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case LIST_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case LIST_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case LIST_TYPE_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_type.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_type.bin_size;

                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_type.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case LIST_COUNT_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_count.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_count.bin_size;

                            rc = Common.parse_attr_expr(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_count.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case LIST_CAPACITY_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_capacity.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_capacity.bin_size;

                            rc = Common.parse_attr_int(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_capacity.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case LIST_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fList.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fList.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fList.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;

                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/

            if ((attrMask & LIST_VALID) == 0)   // added 23jan07 - sjv - spec change
            {
                pAttribute = new DDlAttribute("ListValidity", LIST_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);

                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1; //Default Attribute
                attrList.Add(pAttribute);
            }

            ulItemMasks = attrMask | LIST_VALID;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }

    }


    public class DDl6Grid : DDL6BaseItem   /*Item Type == 27*/
    {
        //FLAT_GRID* pGrid;

        /* GRID attributes SIZE 2 */

        public const int GRID_LABEL_ID = 0;
        public const int GRID_HELP_ID = 1;
        public const int GRID_VALID_ID = 2;
        public const int GRID_HEIGHT_ID = 3;
        public const int GRID_WIDTH_ID = 4;
        public const int GRID_ORIENT_ID = 5;
        public const int GRID_HANDLING_ID = 6;
        public const int GRID_MEMBERS_ID = 7;
        public const int GRID_DEBUG_ID = 8;
        public const int GRID_VISIBLE_ID = 9;
        public const int MAX_GRID_ID = 10; /* must be last in list */
        /* GRID attribute masks */

        public const int GRID_LABEL = (1 << GRID_LABEL_ID);
        public const int GRID_HELP = (1 << GRID_HELP_ID);
        public const int GRID_VALID = (1 << GRID_VALID_ID);
        public const int GRID_HEIGHT = (1 << GRID_HEIGHT_ID);
        public const int GRID_WIDTH = (1 << GRID_WIDTH_ID);
        public const int GRID_ORIENT = (1 << GRID_ORIENT_ID);
        public const int GRID_HANDLING = (1 << GRID_HANDLING_ID);
        public const int GRID_MEMBERS = (1 << GRID_MEMBERS_ID);
        public const int GRID_DEBUG = (1 << GRID_DEBUG_ID);

        public const int GRID_ATTR_MASKS = (GRID_LABEL | GRID_HELP | GRID_VALID | GRID_HEIGHT | GRID_WIDTH | GRID_ORIENT | GRID_HANDLING | GRID_MEMBERS | GRID_DEBUG);
        public DDl6Grid()
        {
            byItemType = GRID_ITYPE;
            strItemName = "Grid";
            //pGrid = &(glblFlats.fGrid); 
        }

        //virtual ~DDl6Grid(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & GRID_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("GridLabel", GRID_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRID_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("GridHelp", GRID_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRID_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("GridValidity", GRID_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRID_HEIGHT) != 0)
            {
                pDDlAttr = new DDlAttribute("GridHeight", GRID_HEIGHT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRID_WIDTH) != 0)
            {
                pDDlAttr = new DDlAttribute("GridWidth", GRID_WIDTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRID_ORIENT) != 0)
            {
                pDDlAttr = new DDlAttribute("GridOrientation", GRID_ORIENT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & GRID_HANDLING) != 0)
            {
                pDDlAttr = new DDlAttribute("GridHandling", GRID_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & GRID_MEMBERS) != 0)
            {
                pDDlAttr = new DDlAttribute("GridPath", GRID_MEMBERS_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_GRID_SET, false);
                attrList.Add(pDDlAttr);
            }


            if ((attrMask & GRID_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("GridDebugData", GRID_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_grid_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_GRID grid_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case GRID_LABEL_ID:
                    //depbin_ptr = &grid_flat.depbin.db_label;		
                    grid_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_label.bin_size = data_len;
                    grid_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case GRID_HELP_ID:
                    //depbin_ptr = &grid_flat.depbin.db_help;		
                    grid_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_help.bin_size = data_len;
                    grid_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case GRID_VALID_ID:
                    //depbin_ptr = &grid_flat.depbin.db_valid;		
                    grid_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_valid.bin_size = data_len;
                    grid_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case GRID_HEIGHT_ID:
                    //depbin_ptr = &grid_flat.depbin.db_height;		
                    grid_flat.depbin.db_height.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_height.bin_size = data_len;
                    grid_flat.depbin.db_height.bin_offset = attr_offset;
                    break;
                case GRID_WIDTH_ID:
                    //depbin_ptr = &grid_flat.depbin.db_width;		
                    grid_flat.depbin.db_width.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_width.bin_size = data_len;
                    grid_flat.depbin.db_width.bin_offset = attr_offset;
                    break;
                case GRID_ORIENT_ID:
                    //depbin_ptr = &grid_flat.depbin.db_orient;		
                    grid_flat.depbin.db_orient.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_orient.bin_size = data_len;
                    grid_flat.depbin.db_orient.bin_offset = attr_offset;
                    break;
                case GRID_HANDLING_ID:
                    //depbin_ptr = &grid_flat.depbin.db_handling;		
                    grid_flat.depbin.db_handling.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_handling.bin_size = data_len;
                    grid_flat.depbin.db_handling.bin_offset = attr_offset;
                    break;
                case GRID_MEMBERS_ID:
                    //depbin_ptr = &grid_flat.depbin.db_members;		
                    grid_flat.depbin.db_members.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_members.bin_size = data_len;
                    grid_flat.depbin.db_members.bin_offset = attr_offset;
                    break;
                case GRID_DEBUG_ID:
                    //depbin_ptr = &grid_flat.depbin.db_debug_info;
                    grid_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    grid_flat.depbin.db_debug_info.bin_size = data_len;
                    grid_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_GRID_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            grid_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_grid_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fGrid, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_grid_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fGrid, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fGrid.masks.bin_exists = attrMask & GRID_ATTR_MASKS;
            glblFlats.fGrid.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case GRID_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRID_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRID_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;

                    case GRID_HEIGHT_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_height.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_height.bin_size;

                            rc = Common.parse_attr_scope_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_height.bin_offset); //Vibhor 260804: Changed to int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case GRID_WIDTH_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_width.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_width.bin_size;

                            rc = Common.parse_attr_scope_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_width.bin_offset); //Vibhor 260804: Changed to int
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRID_ORIENT_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_orient.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_orient.bin_size;

                            rc = Common.parse_attr_orient_size(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_orient.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRID_HANDLING_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_handling.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_handling.bin_size;

                            rc = Common.parse_attr_bitstring(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_handling.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRID_MEMBERS_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_members.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_members.bin_size;

                            rc = Common.parse_gridmembers_list(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_members.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case GRID_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fGrid.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fGrid.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fGrid.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/

            //Anil 230506: Start of Code for Default value handling
            if ((attrMask & GRID_VALID) == 0)
            {
                pAttribute = new DDlAttribute("GridValidity", GRID_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1; /*Default Attribute for Validity is true*/
                attrList.Add(pAttribute);
            }
            if ((attrMask & GRID_HEIGHT) == 0)
            {
                pAttribute = new DDlAttribute("GridHeight", GRID_HEIGHT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);

                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.MEDIUM_DISPSIZE; /*Default Attribute for height is MEDIUM*/
                attrList.Add(pAttribute);
            }

            if ((attrMask & GRID_WIDTH) == 0)
            {
                pAttribute = new DDlAttribute("GridWidth", GRID_WIDTH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_SCOPE_SIZE, false);

                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.MEDIUM_DISPSIZE; /*Default Attribute for Width is MEDIUM*/
                attrList.Add(pAttribute);
            }

            if ((attrMask & GRID_ORIENT) == 0)
            {
                pAttribute = new DDlAttribute("GridOrientation", GRID_ORIENT_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT, false);

                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.ORIENT_VERT; /*Default Attribute for Grid Orientation VERTICAL*/
                attrList.Add(pAttribute);
            }

            if ((attrMask & GRID_HANDLING) == 0)
            {
                pAttribute = new DDlAttribute("GridHandling", GRID_HANDLING_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = Common.READ_HANDLING | Common.WRITE_HANDLING; /*Default Attribute for Grid handling is read write*/
                attrList.Add(pAttribute);
            }

            attrMask = attrMask | GRID_VALID | GRID_HEIGHT | GRID_WIDTH | GRID_ORIENT | GRID_HANDLING;

            ulItemMasks = attrMask;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    public class DDl6Image : DDL6BaseItem  /*Item Type == 28*/
    {
        //FLAT_IMAGE* pImage;
        /* IMAGE attributes SIZE 1 */

        public const int IMAGE_LABEL_ID = 0;
        public const int IMAGE_HELP_ID = 1;
        public const int IMAGE_VALID_ID = 2;
        public const int IMAGE_LINK_ID = 3;
        public const int IMAGE_PATH_ID = 4;
        public const int IMAGE_DEBUG_ID = 5;
        public const int IMAGE_VISIBLE_ID = 6;
        public const int MAX_IMAGE_ID = 7;/* must be last in list */
        /* IMAGE attribute masks */

        public const int IMAGE_LABEL = (1 << IMAGE_LABEL_ID);
        public const int IMAGE_HELP = (1 << IMAGE_HELP_ID);
        public const int IMAGE_VALID = (1 << IMAGE_VALID_ID);
        public const int IMAGE_LINK = (1 << IMAGE_LINK_ID);
        public const int IMAGE_PATH = (1 << IMAGE_PATH_ID);
        public const int IMAGE_DEBUG = (1 << IMAGE_DEBUG_ID);

        public const int IMAGE_ATTR_MASKS = (IMAGE_LABEL | IMAGE_HELP | IMAGE_VALID | IMAGE_LINK | IMAGE_PATH | IMAGE_DEBUG);

        public DDl6Image()
        {
            byItemType = IMAGE_ITYPE;
            strItemName = "Image";
            //pImage = &(glblFlats.fImage);
        }

        //virtual ~DDl6Image(){}

        public override void AllocAttributes()
        {
            DDlAttribute pDDlAttr = null;

            if ((attrMask & IMAGE_LABEL) != 0)
            {
                pDDlAttr = new DDlAttribute("ImageLabel", IMAGE_LABEL_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & IMAGE_HELP) != 0)
            {
                pDDlAttr = new DDlAttribute("ImageHelp", IMAGE_HELP_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & IMAGE_VALID) != 0)
            {
                pDDlAttr = new DDlAttribute("ImageValidity", IMAGE_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & IMAGE_LINK) != 0)
            {
                pDDlAttr = new DDlAttribute("ImageLink", IMAGE_LINK_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & IMAGE_PATH) != 0)
            {
                pDDlAttr = new DDlAttribute("ImagePath", IMAGE_PATH_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                attrList.Add(pDDlAttr);
            }

            if ((attrMask & IMAGE_DEBUG) != 0)
            {
                pDDlAttr = new DDlAttribute("ImageDebugData", IMAGE_DEBUG_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DEBUG_DATA, false);
                attrList.Add(pDDlAttr);
            }
        }

        public static int attach_image_data(byte[] attr_data_ptr, uint attr_offset, uint data_len, ref DDL6Item.FLAT_IMAGE image_flat, ushort tag)
        {

            //DDL6Item.FLAT_VAR* var_flat;
            //DDL6Item.DEPBIN depbin_ptr;

            //	int             rcode;

            //depbin_ptr = (DEPBIN**)0L;  /* Initialize the DEPBIN pointers */

            /*
             * Assign the appropriate flat structure pointer
             */

            //var_flat = (FLAT_VAR*)flats;

            /*
             * Check the flat structure for existence of the DEPBIN pointer arrays.
             * These must be reserved on the scratchpad before the DEPBIN
             * structures for each attribute can be created. Return if there is not
             * enough scratchpad memory to reserve the array.  For the Variables
             * flat structure only, the sub-structures var_flat.misc and
             * var_flat.actions must already exist.
             */

            switch (tag)
            {
                case IMAGE_LABEL_ID:
                    //depbin_ptr = &image_flat.depbin.db_label;		
                    image_flat.depbin.db_label.bin_chunk = attr_data_ptr;
                    image_flat.depbin.db_label.bin_size = data_len;
                    image_flat.depbin.db_label.bin_offset = attr_offset;
                    break;
                case IMAGE_HELP_ID:
                    //depbin_ptr = &image_flat.depbin.db_help;		
                    image_flat.depbin.db_help.bin_chunk = attr_data_ptr;
                    image_flat.depbin.db_help.bin_size = data_len;
                    image_flat.depbin.db_help.bin_offset = attr_offset;
                    break;
                case IMAGE_VALID_ID:
                    //depbin_ptr = &image_flat.depbin.db_valid;		
                    image_flat.depbin.db_valid.bin_chunk = attr_data_ptr;
                    image_flat.depbin.db_valid.bin_size = data_len;
                    image_flat.depbin.db_valid.bin_offset = attr_offset;
                    break;
                case IMAGE_LINK_ID:
                    //depbin_ptr = &image_flat.depbin.db_link;		
                    image_flat.depbin.db_link.bin_chunk = attr_data_ptr;
                    image_flat.depbin.db_link.bin_size = data_len;
                    image_flat.depbin.db_link.bin_offset = attr_offset;
                    break;
                case IMAGE_PATH_ID:
                    //depbin_ptr = &image_flat.depbin.db_path;		
                    image_flat.depbin.db_path.bin_chunk = attr_data_ptr;
                    image_flat.depbin.db_path.bin_size = data_len;
                    image_flat.depbin.db_path.bin_offset = attr_offset;
                    break;
                case IMAGE_DEBUG_ID:
                    //depbin_ptr = &image_flat.depbin.db_debug_info;
                    image_flat.depbin.db_debug_info.bin_chunk = attr_data_ptr;
                    image_flat.depbin.db_debug_info.bin_size = data_len;
                    image_flat.depbin.db_debug_info.bin_offset = attr_offset;
                    break;
                default:
                    if (tag >= MAX_IMAGE_ID)
                        return (FetchItem.FETCH_INVALID_ATTRIBUTE);
                    else
                        return Common.SUCCESS;

            }


            /*
             * Attach the data and the data length to the DEPBIN structure. It the
             * structure does not yet exist, reserve it on the scratchpad first.
             */

            //if ((object)(depbin_ptr) == null)
            //{

            //    depbin_ptr = new DDL6Item.DEPBIN();
            //    /*Put a check if malloc fails, return if yes!!*/

            //}
            //depbin_ptr.bin_chunk = attr_data_ptr;
            //depbin_ptr.bin_size = data_len;
            //depbin_ptr.bin_offset = attr_offset;

            /*
             * Set the .bin_hooked bit in the flat structure for the appropriate
             * attribute
             */

            image_flat.masks.bin_hooked |= (uint)(1L << tag);

            return (Common.SUCCESS);
        }

        public static int get_item_attr(ushort obj_index, uint obj_item_mask, int extn_attr_length, byte[] obj_ext_ptr, ushort itype, uint item_bin_hooked, uint pbyLocalAttrOffset)
        {

            uint local_data_ptr;
            uint obj_attr_ptr;    /* pointer to attributes in object
									 * extension */
            byte[] extern_obj_attr_ptr; /*pointer to attributes in external oject extn */
            byte extern_extn_attr_length; /*length of Extn data in external obj*/
            ushort curr_attr_RI;    /* RI for current attribute */
            ushort curr_attr_tag;   /* tag for current attribute */
            uint curr_attr_length; /* data length for current attribute */
            uint local_req_mask;   /* request mask for base or external
									 * objects */
            uint attr_mask_bit;    /* bit in item mask corresponding to
									 * current attribute */
            uint extern_attr_mask;     /* used for recursive call for
										 * External All objects */
            ushort extern_obj_index;    /* attribute data field for
										 * object index of External object */
            uint attr_offset = 0;  /* attribute data field for offset of //Vibhor 270904: Changed to long, see commments below
									 * data in local data area */
            int rcode;
            /*	BYTE byMaskSize=0; */

            /*
             * Check the validity of the pointer parameters
             */

            //ASSERT_RET(obj_ext_ptr, FETCH_INVALID_PARAM);

            /*
                 * Point to the first attribute in the object extension and begin
                 * extracting the attribute data
                 */

            local_req_mask = obj_item_mask;
            obj_attr_ptr = pbyLocalAttrOffset;

            uint i = 0;
            Common.ITEM_EXTN cItem = new Common.ITEM_EXTN();

            while ((obj_attr_ptr < extn_attr_length + pbyLocalAttrOffset) && local_req_mask > 0)
            {

                /*
                 * Retrieve the Attribute Identifier information from the
                 * leading attribute bytes.  The object extension pointer will
                 * point to the first byte after the Attribute ID, which will
                 * be Immediate data or will reference Local or External data.
                 */
                unsafe
                {
                    fixed (byte* bp = &obj_ext_ptr[0])
                    {
                        rcode = FetchItem.parse_attribute_id(bp, ref obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);
                    }
                }

                if (rcode != 0)
                {
                    return (rcode);
                }

                /*
                 * Confirm that the current attribute being is matched by a set
                 * bit in the Item Mask.  The mask or the attribute tag is
                 * incorrect if there is not a match.
                 */

                attr_mask_bit = (uint)(1L << curr_attr_tag);
                if ((obj_item_mask & attr_mask_bit) == 0)
                {
                    return (FetchItem.FETCH_ATTRIBUTE_NO_MASK_BIT);
                }

                /*
                 * Use the Tag field from the Attribute Identifier to determine
                 * if the attribute was requested or not (not applicable to
                 * External All data type).  For all data types except External
                 * All, skip to the next attribute in the extension if not
                 * requested.  Also, skip forward if the attribute has already
                 * been attached.
                 */

                switch (curr_attr_RI)
                {

                    case FetchItem.RI_IMMEDIATE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                rcode = attach_image_data(obj_ext_ptr, obj_attr_ptr, curr_attr_length, ref glblFlats.fImage, curr_attr_tag);

                                if (rcode != 0)
                                {
                                    return rcode;
                                }
                                else
                                {
                                    local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                }
                            }
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                            }
                        }
                        obj_attr_ptr += curr_attr_length;

                        /*
                         * Check for invalid length that would advance the
                         * object extension pointer past the end of the object
                         * extension
                         */

                        if (obj_attr_ptr > (extn_attr_length + pbyLocalAttrOffset))
                        {
                            return (FetchItem.FETCH_INVALID_ATTR_LENGTH);
                        }
                        break;

                    case FetchItem.RI_LOCAL:
                        /*This is reached in 2 cases:
                                    1- You access an attribute data thru an external object 
                                       ie. you come here as a consequence of a recursive call to get_item_attr 
                                       thru RI 2 or 3
                                    2- Some base object has the RI as 1 directly
                        */

                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*Vibhor 270904: Start of Code*/

                                /* attr_offset: used to be a ushort , now a ulong.
                                 In new tokenizer this is being encoded as a variable length integer
                                 so we'll parse this stuff accordingly.
                                */
                                /* Read encoded int */

                                attr_offset = 0;
                                do
                                {
                                    if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                    {
                                        return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                    }
                                    attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                }
                                while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) > 0);

                                /* end  Read encoded int */


                                /*Vibhor 270904: End of Code*/


                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == obj_index)
                                        break;
                                }

                                if ((DDlDevDescription.ObjectFixed[i].wDomainDataSize < (curr_attr_length + attr_offset)) &&
                                    (DDlDevDescription.ObjectFixed[i].wDomainDataSize != 0xffff))     // allow long attrs through #2500
                                    return -1; /*Data out of range*/

                                local_data_ptr = /*i + */attr_offset;

                                rcode = attach_image_data(DDlDevDescription.pbyObjectValue[i], local_data_ptr, curr_attr_length, ref glblFlats.fImage, curr_attr_tag);

                                if (rcode != Common.SUCCESS)
                                    return rcode;

                            }//endif item_bin_hooked...
                            else
                            {
                                local_req_mask &= ~attr_mask_bit;   /* clear bit */
                                {
                                    //read and discard the encoded integer, mainly to advance the object attribute pointer
                                    attr_offset = 0;
                                    do
                                    {
                                        if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                        {
                                            return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                        }
                                        attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                                    }
                                    while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                                }

                            }
                        }//endif local_req_mask...
                        else
                        {
                            //read and discard the encoded integer, mainly to advance the object attribute pointer
                            attr_offset = 0;
                            do
                            {
                                if ((attr_offset & FetchItem.MAX_LENGTH_MASK) != 0)
                                {
                                    return (FetchItem.FETCH_ATTR_LENGTH_OVERFLOW);
                                }
                                attr_offset = (attr_offset << FetchItem.LENGTH_SHIFT) | (uint)(FetchItem.LENGTH_MASK & obj_ext_ptr[obj_attr_ptr]);
                            }
                            while ((FetchItem.LENGTH_ENCODE_MASK & obj_ext_ptr[obj_attr_ptr++]) != 0);
                        }

                        break;

                    case FetchItem.RI_EXTERNAL_SINGLE:
                        if ((local_req_mask & attr_mask_bit) != 0)
                        {
                            if ((item_bin_hooked & attr_mask_bit) == 0)
                            {

                                /*
                                 * Set a request mask with a single attribute
                                 */

                                extern_attr_mask = attr_mask_bit;

                                extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                                /*Locate the external object */
                                for (i = 0; i < DDlDevDescription.uSODLength; i++)
                                {
                                    if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                        break;
                                }
                                /*Get extension length & the attribute offset*/

                                /*						switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                                        {
                                                        case VARIABLE_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        case BLOCK_ITYPE:
                                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                                            break;
                                                        default:
                                                            byMaskSize = MIN_ATTR_MASK_SIZE;

                                                        } 
                                */ /* Not required as the external object won't have any ID or a mask*/
                                extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                                //-byMaskSize;

                                extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                                //			+ byMaskSize;


                                /*Now make a recursive call to get the get_item_attr
                                  to get the attribute for this object*/

                                rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                                if (rcode != Common.SUCCESS)
                                    return (rcode);

                            }
                            local_req_mask &= ~attr_mask_bit;
                        }

                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;

                        break;

                    case FetchItem.RI_EXTERNAL_ALL:

                        extern_attr_mask = attr_mask_bit;

                        extern_obj_index = (ushort)((obj_ext_ptr[obj_attr_ptr] << 8) | obj_ext_ptr[obj_attr_ptr + 1]);

                        /*Locate the external object */
                        for (i = 0; i < DDlDevDescription.uSODLength; i++)
                        {
                            if (DDlDevDescription.ObjectFixed[i].index == extern_obj_index)
                                break;
                        }
                        /*Get extension length & the attribute offset*/
                        /*			switch(((ITEM_EXTN*)pbyExtensions[i]).byItemType)
                                        {
                                        case VARIABLE_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        case BLOCK_ITYPE:
                                            byMaskSize = VAR_ATTR_MASK_SIZE;
                                            break;
                                        default:
                                            byMaskSize = MIN_ATTR_MASK_SIZE;
                                        }
                        */ /* Not required as the external object won't have any ID or a mask*/

                        extern_extn_attr_length = (byte)(DDlDevDescription.byExtLengths[i] + DDlDevDescription.EXTEN_LENGTH_SIZE - Marshal.SizeOf(cItem));
                        //-byMaskSize;

                        extern_obj_attr_ptr = DDlDevDescription.pbyExtensions[i + Marshal.SizeOf(cItem)];
                        //			+ byMaskSize;


                        /*Now make a recursive call to get the get_item_attr
                          to get the attribute for this object*/

                        rcode = get_item_attr(extern_obj_index, obj_item_mask, extern_extn_attr_length, extern_obj_attr_ptr, itype, item_bin_hooked, 0);

                        /*
                         * Ignore FETCH_ATTRIBUTE_NOT_FOUND errors in this
                         * context since the request mask may point to
                         * attributes not contained in the external object.
                         */

                        if ((rcode != Common.SUCCESS) &&
                            (rcode != FetchItem.FETCH_ATTRIBUTE_NOT_FOUND))
                            return rcode;
                        /*
                         * Reduce attribute count by number of
                         * attributes attached from External object
                         */

                        local_req_mask &= ~extern_attr_mask;    /* clear attached bits */


                        obj_attr_ptr += FetchItem.EXTERNAL_REF_SIZE;
                        break;

                    default:
                        return (FetchItem.FETCH_INVALID_RI);
                }       /* end switch */

            }           /* end while */

            return (Common.SUCCESS);

        }

        public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
        {
            uint pbyLocalAttrOffset = 0;
            DDlBaseItem di = this;
            int iAttrLength = 0;
            int iRetVal = preFetchItem(ref di, maskSizes, ref pbyObjExtn, ref iAttrLength, ref pbyLocalAttrOffset);

            if (iRetVal != 0)
                return iRetVal;

            //pbyLocalAttrOffset = pbyItemExtn;

            id = di.id;

            glblFlats.fImage.masks.bin_exists = attrMask & IMAGE_ATTR_MASKS;
            glblFlats.fImage.id = id;
            iRetVal = get_item_attr(objIndex, attrMask, iAttrLength, pbyObjExtn, byItemType, 0, pbyLocalAttrOffset);

            return iRetVal;
        }

        public override int eval_attrs()
        {
            int rc;

            byte[] AttrChunkPtr = null;
            uint ulChunkSize = 0;

            AllocAttributes();

            DDlAttribute pAttribute;// = NULL;

            //for(p = attrList.begin();p != attrList.end();p++)
            for (int i = 0; i < attrList.Count; i++)
            {
                pAttribute = attrList[i];

                switch (pAttribute.byAttrID)
                {
                    case IMAGE_LABEL_ID:
                        {
                            AttrChunkPtr = glblFlats.fImage.depbin.db_label.bin_chunk;
                            ulChunkSize = glblFlats.fImage.depbin.db_label.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fImage.depbin.db_label.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case IMAGE_HELP_ID:
                        {
                            AttrChunkPtr = glblFlats.fImage.depbin.db_help.bin_chunk;
                            ulChunkSize = glblFlats.fImage.depbin.db_help.bin_size;

                            rc = Common.parse_attr_string(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fImage.depbin.db_help.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case IMAGE_VALID_ID:
                        {
                            AttrChunkPtr = glblFlats.fImage.depbin.db_valid.bin_chunk;
                            ulChunkSize = glblFlats.fImage.depbin.db_valid.bin_size;

                            rc = Common.parse_attr_ulong(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fImage.depbin.db_valid.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                        }
                        break;
                    case IMAGE_LINK_ID:
                        {
                            AttrChunkPtr = glblFlats.fImage.depbin.db_link.bin_chunk;
                            ulChunkSize = glblFlats.fImage.depbin.db_link.bin_size;

                            rc = Common.parse_attr_reference(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fImage.depbin.db_link.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case IMAGE_PATH_ID:
                        {
                            AttrChunkPtr = glblFlats.fImage.depbin.db_path.bin_chunk;
                            ulChunkSize = glblFlats.fImage.depbin.db_path.bin_size;

                            //rc = Common.parse_attr_expr((*p),AttrChunkPtr,ulChunkSize, glblFlats.fImage.depbin.db_label.bin_offset);
                            rc = Common.parse_attr_int(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fImage.depbin.db_path.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    case IMAGE_DEBUG_ID:
                        {
                            AttrChunkPtr = glblFlats.fImage.depbin.db_debug_info.bin_chunk;
                            ulChunkSize = glblFlats.fImage.depbin.db_debug_info.bin_size;

                            rc = Common.parse_debug_info(ref pAttribute, ref AttrChunkPtr, ulChunkSize, glblFlats.fImage.depbin.db_debug_info.bin_offset);
                            if (rc != Common.DDL_SUCCESS)
                                return rc;
                            else
                                strItemName = pAttribute.pVals.debugInfo.symbol_name;
                        }
                        break;
                    default:
                        break;

                }/*End switch*/

                attrList[i] = pAttribute;

            }/*End for*/

            if ((attrMask & IMAGE_VALID) == 0)
            {
                pAttribute = new DDlAttribute("SourceValidity", IMAGE_VALID_ID, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG, false);
                pAttribute.pVals = new VALUES();
                pAttribute.pVals.ullVal = 1;
                attrList.Add(pAttribute);
            }

            ulItemMasks = attrMask | IMAGE_VALID;

            return Common.SUCCESS;
        }

        public override void clear_flat()
        {
            ;
        }
    }

    //class DDl6Blob : DDL6BaseItem   /*Item Type == 29*/
    //{
    //    //FLAT_BLOB* pBlob;
    //    public DDl6Blob() 
    //    { 
    //        byItemType = BLOB_ITYPE; 
    //        strItemName = "Blob";
    //        //pBlob = &(glblFlats.fBlob); 
    //    }

    //    //virtual ~DDl6Blob(){}
    //    /*
    //    public override void AllocAttributes();
    //    void AllocAttributes(unsigned long attrMask) { AllocAttributes(); }// make base item happy
    //    public int fetch_item(ref byte[] pbyObjExtn, ushort objIndex)
    //    public override int eval_attrs();
    //    public override void clear_flat();
    //    */
    //}



}
