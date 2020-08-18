using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace FieldIot.HARTDD
{
    public class cfgInfo
    {
        public string ddRoot;
        public bool bOffline;
        public string defaultManufacturer;
        public string defaultDeviceType;

        public cfgInfo()
        {
            defaultManufacturer = "000026";
            defaultDeviceType = "0080";
        }

    }

    public enum LogType
    {
        Info,
        Error,
        Ok,
        Warning
    }

    public class ComboxItem
    {
        private string text;
        private int values;

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public int Values
        {
            get
            {
                return this.values;
            }
            set
            {
                this.values = value;
            }
        }

        public ComboxItem()
        {
        }

        public ComboxItem(string _Text, int _Values)
        {
            Text = _Text;
            Values = _Values;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public enum writestatus_t
    {
        none,
        waitforwrite,
        writting,
        writeerror,
        writedone,
        writecanceled
    }

    public enum returncode
    {
        eOk,
        eErr,
        eCommErr,
        eCloseErr,
        eDataErr,
        eTimeOutErr,
        eFileErr,
        eSerErr,
        eDicErr,
        eDDInitErr,
        eDDHeaderErr,
        eDDOdError,
        eSOdError,
        eObjectError,
        eAPP_DICTIONARY_MISSING
    }

    public enum updatetype
    {
        /* 06dec05 stevev - new notification system 
             // lHint value              pHint
              UPDATE_REBUILD_ALL     //  not used
           ,  UPDATE_REBUILD_LIST    //  pHdw Parent of list to rebuild
           ,  UPDATE_DELETE_BELOW    //  pHdw whose children will be deleted
           ,  UPDATE_REMOVE_ITEM     //  pHdw which will be deleted
           ,  UPDATE_ADD_ITEM        //  pHdw of item to add.
           ,  UPDATE_ITEM             //  pHdw of item to update.
           ,  UPDATE_WINDOW_ITEM     // similar to update item -usually a value-change item

           ,  window view will use most of these */
        UPDATE_T_DO_NOTHING,  //0 CView::OnInitialUpdate() does a OnUpdate(NULL, 0, NULL);  
        UPDATE_T_DRAW_HOLD    //1 set (tree) redraw off till release message
       , UPDATE_T_DRAW_DRAW    //2 set (tree) redraw back on
       /* new update plan */
       , UPDATE_STRUCTURE     // 3 for treeview - will send an update list view
       , UPDATE_ADD_DEVICE    // 4 for treeview - will send an update list view
       , UPDATE_LIST_VIEW     // 5 for listview to update as current selection
       , UPDATE_REMOVE_DEVICE // 6 for treeview  - will send an update list view
       , UPDATE_ONE_VALUE     // 7 for listview  - single value, label, unit updated
       , UPDATE_TREE_VIEW     // 8 works like add_device just for tree&list 
       , UPDATE_DELETE_ITEM   // 9 list item ceases to exist..we must strip tree of oointers to it
                              //   31jul07-crashed when root_menu both a table and a window & we get a struct change
    }

    public enum cmdDataItemFlags_t
    {
        cmdDataItemFlg_None = 0x00,
        cmdDataItemFlg_Info,        // 0x01
        cmdDataItemFlg_Index,       // 0x02
        cmdDataItemFlg_indexNinfo,  // 0x03
        cmdDataItemFlg_WidthPresent = 0x8000
    }

    public enum INSTANCE_DATA_STATE_T
    {
        IDS_UNINITIALIZED,
        IDS_CACHED, 
        IDS_STALE, 
        IDS_PENDING, 
        IDS_INVALID, 
        IDS_NEEDS_WRITE
    }

    public class Message
    {
        public const string M_UI_MT_STR = "";

        public const string M_UI_CONTINUE_QUEST = "\nContinue?";
        public const string M_UI_NO_RESP_CD = "No response code defined.";
        public const string M_UI_NO_RESP_CD_FORMAT = "  (Cmd: %d  RC: %d;";
        public const string M_UI_NO_RESP_CD_4_BIT = "Comm Error : No Description Available for bit value: 0x%02X for Command: %d";
        public const string M_UI_SEARCH_POLL_FAIL = "Connection to poll address (search; failed.";
        public const string M_UI_CONNECT_POLL_FAIL = "Connection to poll address %d failed.";
        public const string M_UI_NO_RESP_CONTINUE_QUEST = "No Response to resend after %d seconds. \n  Continue trying? ";
        public const string M_UI_DELAYED_RESP = "Delayed Response";
        public const string M_UI_COMM_ERROR = "Error In Communication with Device, Closing the Device...";

        public const string M_UI_UNABLE_2_INIT_COM = "Unable to initialize communications COM";
        public const string M_UI_NO_HART_UTIL = "Unable to get Hart Utilities dll";
        public const string M_UI_NO_MODEM_CONN = "Unable to get Hart Modem Connection";
        public const string M_UI_NO_HART_SERVER_CONN = "Unable to get Hart Server Connection";
        public const string M_UI_NO_HART_SELECT = "Unable to get Hart Select dll";
        public const string M_UI_UNK_DLL_EXCEPTION = "Unknown DLL exception.";
        public const string M_UI_NO_TIMER = "Cannot install timer.  Timers are being used by other applications";

        public const string M_UI_ARG_USAGE01 = "\nUsage: ";

        public const string M_UI_FORMAT_001 = "0x%c0%dX";

        public const string M_UI_NO_SAVE = "Saving a Device data snapshot is not yet supported.";
        public const string M_UI_NO_COMMS = "Communications have NOT been instantiated.";
        public const string M_UI_GENERIC = "This should not happen";
        public const string M_UI_SRC_CHANGED = "The DD file has changed on the disk. Reload?   ";

        public const string M_UI_CLOSE_DEVICE = "Close the current device?";
        public const string M_UI_NO_OPEN_SAVED = "Opening a previously saved Device is not yet supported.";
        public const string M_UI_NO_TABLE_STYLE = "The Table Style is not yet supported.";
        public const string M_UI_BAD_SN = "Invalid Serial Number";
        public const string M_UI_NOT_REGISTERED = "This application is not registered.   ";
        public const string M_UI_GENERATING = "Generating Device";
        public const string M_UI_PLEASE_RESTART = "Please restart the application to implement changes";
        public const string M_UI_POLLING_RNG = "Polling address must be a number from 0 to %d.";

        public const string M_UI_SCOPE_NO_CWND = "Could not instantiate the Cwnd object";
        public const string M_UI_SCOPE_CHART_FAIL = "Failed to instantiate Chart Control";
        public const string M_UI_SCOPE_STCHART_FAIL = "Failed to instantiate Strip Chart Control";
        public const string M_UI_SCOPE_SCCHART_FAIL = "Failed to instantiate Scope Chart Control";
        public const string M_UI_SCOPE_SWCHART_FAIL = "Failed to instantiate Sweep Chart Control";
        public const string M_UI_SCOPE_ANGUAGE_FAIL = "Failed to instantiate Angular Guage Control";
        public const string M_UI_SCOPE_LNGUAGE_FAIL = "Failed to instantiate Linear Guage Control";
        public const string M_UI_SCOPE_NO_IUNKNOWN = "Could not get IUnKnown pointer";
        public const string M_UI_SCOPE_LEGEND = "Legend";

        public const string M_UI_INVALID_ENTRY = "Value out of range.  Please enter a valid value";
        public const string M_UI_INVALID_TYPE = "Edit type is not recognized. Please notify the programmer.";
        public const string M_UI_NON_ASCII_STR = "ASCII type variables require ascii input strings.";
        public const string M_UI_PARSE_NUM_FAIL = "The string could not be parsed into a number.";
        public const string M_UI_INPUT_FAILURE = "The input failed for an unknown reason.";


        public const string M_UI_XMTR_NO_NUMBER = "FFFFFFFF";
        public const string M_UI_XMTR_BAD_INI = "INI file may be corrupt,Loading default values";
        public const string M_UI_XMTR_NO_DB = "No database specification found.";
        public const string M_UI_XMTR_ZERO_ILLEGAL = "Zero DD key is illegal.";
        public const string M_UI_XMTR_DB_NOT_AVAIL = "The Database is unavailable.";


        public const string M_METHOD_ABORTED = "Method aborted";
        public const string M_METHOD_SEND_ERROR = "Exiting Method: Send Error";
        public const string M_METHOD_RC_ABORT = "Aborting method due to response code or device status";

        public const string M_ARGS_NO_COMMAND = "no command name";

    }

    public enum timeScale_t
    {
        tsNo_Scale = 0x00,
        tsSecScale = 0x01,
        tsMinScale = 0x02,
        ts_MSScale = 0x03,    // 3 is Min & Sec
        tsHr_Scale = 0x04,
        tsH_SScale = 0x05,
        tsHM_Scale = 0x06,
        tsHMSScale = 0x07,
        ts_I_Scale = 0x08,    // Fixed defect #4511, POB - 4/21/2014
        ts_p_Scale = 0x10
    }

    public class CurrentTimeLocale_t
    {
        public string[] abday;
        public string[] day = new string[7];
        public string[] abmon = new string[13];
        public string[] mon = new string[13];
        public string[] am_pm = new string[2];

        public string d_t_fmt;
        public string t_fmt_ampm;
        public string t_fmt;
        public string d_fmt;

        public CurrentTimeLocale_t()
        {
            abday = new string[7] { " ", " ", " ", " ", " ", " ", " " };
            day = new string[7] { " ", " ", " ", " ", " ", " ", " " };
            abmon = new string[13] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
            mon = new string[13] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
            am_pm = new string[2] { " ", " " };
            d_t_fmt = " ";
            t_fmt_ampm = " ";
            t_fmt = " ";
            d_fmt = " ";
        }
    }

    public enum dtEditFmt_t
    {
        undefined,
        usFormat,   // mm/dd/yy
        invsFormat, // dd/mm/yy
        dotFormat   // dd.mm.yy
    }

    public class Common
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct ITEM_EXTN
        {

            public byte byLength;
            public byte byItemType;
            public byte bySubType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] byItemId;
        }

        public struct dmdate_t
        {
            public byte day;
            public byte mnth;
            public ushort year;

            public dmdate_t(byte d, byte m, ushort y)
            {
                day = d;
                mnth = m;
                year = y;
            }
        }

        public struct DEV_STRING_INFO
        {
            public uint id;
            public uint mfg;
            public ushort dev_type;
            public byte rev;
            public byte ddrev;
        }

        public const int BI_SUCCESS = 0;            /* task succeeded in intended task	  */
        public const int BI_ERROR = -1;     /* error occured in task			  */
        public const int BI_ABORT = -2;     /* user aborted task				  */
        public const int BI_NO_DEVICE = -3;     /* no device found on comm request	  */
        public const int BI_COMM_ERR = -4;      /* communications error				  */
        public const int BI_CONTINUE = -5;      /* continue */
        public const int BI_RETRY = -6; /* retry */
        public const int BI_PORT_IN_USE = -7;			/* block transfer port */

        /*
         * Grid Orientation.
         */

        public const int ORIENT_VERT = 1;
        public const int ORIENT_HORIZ = 2;

        /*
         * SCALE types.
        */

        public const int LINEAR_SCALE = 1;
        public const int LOG_SCALE = 2;

        public const int EXTEN_LENGTH_SIZE = 1;
        public const int DDL_SUCCESS = (0);
        public const int DDL_MEMORY_ERROR = (1500 + 1);
        public const int DDL_INSUFFICIENT_OCTETS = (1500 + 2);
        public const int DDL_SHORT_BUFFER = (1500 + 3);
        public const int DDL_ENCODING_ERROR = (1500 + 4);
        public const int DDL_LARGE_VALUE = (1500 + 5);
        public const int DDL_DIVIDE_BY_ZERO = (1500 + 6);
        const int DDL_BAD_VALUE_TYPE = (1500 + 7);
        const int DDL_SERVICE_ERROR = (1500 + 8);
        const int DDL_FILE_ERROR = (1500 + 9);
        const int DDL_BAD_FILE_TYPE = (1500 + 10);
        const int DDL_FILE_NOT_FOUND = (1500 + 11);
        const int DDL_OUT_OF_DATA = (1500 + 12);
        const int DDL_DEFAULT_ATTR = (1500 + 13);
        const int DDL_null_POINTER = (1500 + 14);
        const int DDL_INCORRECT_FILE_FORMAT = (1500 + 15);
        const int DDL_INVALID_PARAM = (1500 + 16);
        const int DDL_CHECK_RETURN_LIST = (1500 + 17);
        const int DDL_DEV_SPEC_STRING_NOT_FOUND = (1500 + 18);
        public const int DDL_DICT_STRING_NOT_FOUND = (1500 + 19);
        const int DDL_READ_VAR_VALUE_FAILED = (1500 + 20);
        public const int DDL_BINARY_REQUIRED = (1500 + 21);
        const int DDL_VAR_TYPE_NEEDED = (1500 + 22);
        const int DDL_EXPR_STACK_OVERFLOW = (1500 + 23);
        const int DDL_INVALID_TYPE_SUBATTR = (1500 + 24);
        const int DDL_BAD_FETCH_TYPE = (1500 + 25);
        const int DDL_RESOLVE_FETCH_FAIL = (1500 + 26);
        const int DDL_BLOCK_TABLES_NOT_LOADED = (1500 + 27);
        const int DDL_INSUFFICIENT_SIZE = (1500 + 28);  /* stevev 06may05 */
        const int DDL_UNHANDLED_STUFF_FAILURE = (1500 + 50); /*Vibhor 300903*/

        const int DDL_ERROR_END = (1500 + 99);
        const UInt64 DDL_UInt64_MAX = 0xffffffffffffffff;

        public const int DDS_SUCCESS = (0);
        public const int SUCCESS = (0);
        public const int FAILURE = (1);
        public const int FETCH_EXTERNAL_OBJECT = (1600 + 33);
        const int FETCH_ERROR_END = (1600 + 99);

        public const int IF_TAG = 0;
        public const int SELECT_TAG = 1;
        public const int CASE_TAG = 0;
        public const int DEFAULT_TAG = 1;
        public const int OBJECT_TAG = 2;
        public const int DEFAULT_TAG_VALUE = 0xff;


        /*
         * Miscellaneous tags.
         */

        public const int REFERENCE_SEQLIST_TAG = 75;
        public const int ENUMERATOR_SEQLIST_TAG = 76;
        public const int ENUMERATOR_TAG = 77;
        public const int DATA_ITEMS_SEQLIST_TAG = 78;
        public const int RSPCODES_SEQLIST_TAG = 79;
        public const int RESPONSE_CODE_TAG = 80;
        public const int MENU_ITEM_SEQLIST_TAG = 81;
        public const int ELEMENT_SEQLIST_TAG = 82;
        public const int ITEM_ARRAY_ELEMENT_TAG = 83; /*element*/
        public const int MEMBER_SEQLIST_TAG = 84;
        public const int MEMBER_TAG = 85;
        public const int EXPRESSION_TAG = 86;
        public const int RANGE_LIST_TAG = 87;
        public const int GRID_ELEMENT_TAG = 88;
        public const int GRID_SEQLIST_TAG = 89;
        public const int PARAMETER_TAG = 90;
        public const int PARAMETER_SEQLIST_TAG = 91; /* list of method params */
        public const int EXPRESSION_LIST_TAG = 92;
        /*
         * Import tags.
         */

        public const int IMPORT_TAG = 100;
        public const int ALL_IMPORT_TAG = 101;
        public const int TYPE_IMPORT_TAG = 102;
        public const int OBJECT_IMPORT_TAG = 103;
        public const int DELETE_TAG = 104;
        public const int REDEFINE_TAG = 105;
        public const int ADD_TAG = 106;

        public const int ATTR_DELETE_TAG = 0;
        public const int ATTR_REDEFINE_TAG = 1;
        public const int ATTR_ADD_TAG = 2;


        public const int NOT_OPCODE = 1;
        public const int NEG_OPCODE = 2;
        public const int BNEG_OPCODE = 3;
        public const int ADD_OPCODE = 4;
        public const int SUB_OPCODE = 5;
        public const int MUL_OPCODE = 6;
        public const int DIV_OPCODE = 7;
        public const int MOD_OPCODE = 8;
        public const int LSHIFT_OPCODE = 9;
        public const int RSHIFT_OPCODE = 10;
        public const int AND_OPCODE = 11;
        public const int OR_OPCODE = 12;
        public const int XOR_OPCODE = 13;
        public const int LAND_OPCODE = 14;
        public const int LOR_OPCODE = 15;
        public const int LT_OPCODE = 16;
        public const int GT_OPCODE = 17;
        public const int LE_OPCODE = 18;
        public const int GE_OPCODE = 19;
        public const int EQ_OPCODE = 20;
        public const int NEQ_OPCODE = 21;

        public const int INTCST_OPCODE = 22;
        public const int FPCST_OPCODE = 23;

        public const int VARID_OPCODE = 24;
        public const int MAXVAL_OPCODE = 25;/* now a reference */
        public const int MINVAL_OPCODE = 26;/* now a reference */

        public const int VARREF_OPCODE = 27;
        public const int MAXREF_OPCODE = 28;
        public const int MINREF_OPCODE = 29;

        public const int BLOCK_OPCODE = 30;
        public const int BLOCKID_OPCODE = 31;
        public const int BLOCKREF_OPCODE = 32;
        public const int STRCST_OPCODE = 33;
        public const int SYSTEMENUM_OPCODE = 34;

        /*
         * Chart sizes.
         */

        public const int XX_SMALL_DISPSIZE = 1;
        public const int X_SMALL_DISPSIZE = 2;
        public const int SMALL_DISPSIZE = 3;
        public const int MEDIUM_DISPSIZE = 4;
        public const int LARGE_DISPSIZE = 5;
        public const int X_LARGE_DISPSIZE = 6;
        public const int XX_LARGE_DISPSIZE = 7;
        public const int XXX_SMALL_DISPSIZE = 8;

        /*
         * 	STRING tags
         */
        public const int DONT_FREE_STRING = 0X00;
        public const int FREE_STRING = 0X01;
        public const int ISEMPTYSTRING = 0X10;

        public const int DEV_SPEC_STRING_TAG = 0;   /* string device specific id */
        public const int VARIABLE_STRING_TAG = 1;   /* string variable id */
        public const int ENUMERATION_STRING_TAG = 2;/* enumeration string information */
        public const int DICTIONARY_STRING_TAG = 3;/* dictionary string id */
        public const int VAR_REF_STRING_TAG = 4;/* variable_reference_string */
        public const int ENUM_REF_STRING_TAG = 5;	/* enumerated reference string */

        public static bool bTokRev6Flag = true;

        const uint DEFAULT_STD_DICT_STRING = (uint)((400 << 16) + 0);
        const uint DEFAULT_DEV_SPEC_STRING = (uint)((400 << 16) + 1);
        const uint DEFAULT_STD_DICT_HELP = (uint)((400 << 16) + 2);
        const uint DEFAULT_STD_DICT_LABEL = (uint)((400 << 16) + 3);
        const uint DEFAULT_STD_DICT_DESC = (uint)((400 << 16) + 4);
        const uint DEFAULT_STD_DICT_DISP_INT = (uint)((400 << 16) + 5);
        const uint DEFAULT_STD_DICT_DISP_UINT = (uint)((400 << 16) + 6);
        const uint DEFAULT_STD_DICT_DISP_FLOAT = (uint)((400 << 16) + 7);
        const uint DEFAULT_STD_DICT_DISP_DOUBLE = (uint)((400 << 16) + 8);
        const uint DEFAULT_STD_DICT_EDIT_INT = (uint)((400 << 16) + 9);
        const uint DEFAULT_STD_DICT_EDIT_UINT = (uint)((400 << 16) + 10);
        const uint DEFAULT_STD_DICT_EDIT_FLOAT = (uint)((400 << 16) + 11);
        const uint DEFAULT_STD_DICT_EDIT_DOUBLE = (uint)((400 << 16) + 12);

        /*
         * Item tags.
         */
        public const int VARIABLE_TAG = 1;
        public const int COMMAND_TAG = 2;
        public const int MENU_TAG = 3;
        public const int EDIT_DISPLAY_TAG = 4;
        public const int METHOD_TAG = 5;
        public const int REFRESH_TAG = 6;
        public const int UNIT_TAG = 7;
        public const int WRITE_AS_ONE_TAG = 8;
        public const int ITEM_ARRAY_TAG = 9;
        public const int COLLECTION_TAG = 0;
        public const int RELATION_TAG = 1;


        /*
         * Attribute tags.
         */

        public const int MIN_ATTR = (CLASS_TAG - 1);    /* This must be < Minimum Attr number */

        public const int CLASS_TAG = 20;
        public const int CONSTANT_UNIT_TAG = 21;
        public const int HANDLING_TAG = 22;
        public const int HELP_TAG = 23;
        public const int LABEL_TAG = 24;
        public const int PRE_EDIT_TAG = 25;
        public const int POST_EDIT_TAG = 26;
        public const int PRE_READ_TAG = 27;
        public const int POST_READ_TAG = 28;
        public const int PRE_WRITE_TAG = 29;
        public const int POST_WRITE_TAG = 30;
        public const int READ_TIMEOUT_TAG = 31;
        public const int WRITE_TIMEOUT_TAG = 32;
        public const int TYPE_TAG = 33;
        public const int DISPLAY_FORMAT_TAG = 34;
        public const int EDIT_FORMAT_TAG = 35;
        public const int MAX_VALUE_TAG = 36;
        public const int MIN_VALUE_TAG = 37;
        public const int SCALING_FACTOR_TAG = 38;
        public const int VALIDITY_TAG = 39;
        public const int NUMBER_TAG = 40;
        public const int OPERATION_TAG = 41;
        public const int TRANSACTION_TAG = 42;
        public const int RESPONSE_CODES_TAG = 43;
        public const int MENU_ITEMS_TAG = 44;
        public const int DISPLAY_ITEMS_TAG = 45;
        public const int EDIT_ITEMS_TAG = 46;
        public const int DEFINITION_TAG = 47;
        public const int ELEMENTS_TAG = 48;
        public const int MEMBERS_TAG = 49;
        public const int ITEM_ARRAYNAME_TAG = 50; /* array_name_specifier*/

        public const int CHART_TYPE_TAG = 51;
        public const int SCOPE_SIZE_TAG = 52;
        public const int MENU_STYLE_TAG = 53;
        public const int WAVE_TYPE_TAG = 54;

        /* stevev 22mar05 */
        public const int GRID_ORIENT_TAG = 55;
        public const int GRID_MEMBERS_TAG = 56;
        /* end stevev 22mar05 */
        /* stevev 04may05 */
        public const int NAME_STR_TAG = 57;
        public const int DEBUG_INFO_TAG = 58;
        public const int DEBUG_ATTR_INFO_TAG = 59;
        public const int DEBUG_ATTR_MEMBER_TAG = 60;
        /* 11dec07 timj */
        public const int TIME_FORMAT_TAG = 61;
        public const int TIME_SCALE_TAG = 62;
        // 4mar13 timj
        public const int UUID_TAG = 63;
        public const int IDENTITY_TAG = 64;

        public const int DEFAULT_VALUES_LIST_TAG = 65;
        public const int MAX_ATTR = (DEFAULT_VALUES_LIST_TAG + 1);/* This must be > Maximum Attr
						 * number*/

        /*
         * ALTERNATIVE output status classes.
         */

        public const int OC_NORMAL = 0; /* must be 0, see ddl_parse_one_enum() */

        /*
         * HART output status classes.
         */

        public const int OC_DV = 1;
        public const int OC_TV = 2;
        public const int OC_AO = 3;
        public const int OC_ALL = 4;

        // OC_AUTO is zero
        public const int OC_MANUAL = 0x01;
        // OC_GOOD is zero
        public const int OC_BAD = 0x02;
        public const int OC_MARGINAL = 0x04;

        /*
         * HART command operations.
         */

        public const int READ_OPERATION = 1;
        public const int WRITE_OPERATION = 2;
        public const int COMMAND_OPERATION = 3;

        /*
         *	Transaction tags
         */

        public const int TRANS_REQ_TAG = 0;
        public const int TRANS_REPLY_TAG = 1;
        public const int TRANS_RESP_CODES_TAG = 2;
        public const int TRANS_POSTRCV_ACTION_TAG = 3;


        /*
         * Handling of variables.
         */

        public const int READ_HANDLING = 0x01;
        public const int WRITE_HANDLING = 0x02;
        public const int CONFIG_HANDLING = 0x04;    /* XMTR */
        public const int NONE_HANDLING = 0x08;   /* designated as only None */

        public const int READ_ONLY_ITEM = 0x001;
        public const int DISPLAY_VALUE_ITEM = 0x002;
        public const int REVIEW_ITEM = 0x004;

        /*
         * Data item types.
         */

        public const int DATA_CONSTANT = 0;
        public const int DATA_REFERENCE = 1;

        public const int DATA_REF_FLAGS = 2;    /* HART */
        public const int DATA_REF_WIDTH = 3;    /* HART */
        public const int DATA_REF_FLAGS_WIDTH = 4;  /* HART */

        public const int DATA_FLOATING = 5;


        /* Masks for all members */

        public const int MEMBER_DESC_EVALED = 0X01;
        public const int MEMBER_HELP_EVALED = 0X02;
        public const int MEMBER_LOC_EVALED = 0X04;
        public const int MEMBER_REF_EVALED = 0X08;

        /* The masks for ITEM_ARRAY_ELEMENT */

        public const int IA_DESC_EVALED = MEMBER_DESC_EVALED;
        public const int IA_HELP_EVALED = MEMBER_HELP_EVALED;
        public const int IA_INDEX_EVALED = MEMBER_LOC_EVALED;
        public const int IA_REF_EVALED = MEMBER_REF_EVALED;

        /* The masks for MEMBER */

        public const int MEM_DESC_EVALED = MEMBER_DESC_EVALED;
        public const int MEM_HELP_EVALED = MEMBER_HELP_EVALED;
        public const int MEM_NAME_EVALED = MEMBER_LOC_EVALED;
        public const int MEM_REF_EVALED = MEMBER_REF_EVALED;


        /*
         * LINE types.
         */

        public const int DATA_LINETYPE = 1;
        public const int LOWLOW_LINETYPE = 2;
        public const int LOW_LINETYPE = 3;
        public const int HIGH_LINETYPE = 4;
        public const int HIGHHIGH_LINETYPE = 5;
        public const int TRANSPARENT_LINETYPE = 6;

        //const int MAX_FILE_NAME_PATH = 255;

        static DDlBlock pBlock = new DDlBlock(); /* Global DDlBlock pointer to resolve 'Parameter' references */

        //static Dictionary pPIdict = null;
        //static DDlDevDescription devDesc;

        public static string chInputFileName;

        static string[] abday = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        static string[] day = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        static string[] abmon = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        static string[] mon = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        static string[] am_pm = { "AM", "PM" };

        static string d_t_fmt = "%a %Ef %T %Y";
        static string t_fmt_ampm = "%I:%M:%S %p";
        static string t_fmt = "%H:%M:%S";
        static string d_fmt = "%m/%d/%y";

        static bool areSet = false;

        static CurrentTimeLocale_t CurrentTimeLocale = new CurrentTimeLocale_t();

        const int ALT_E = 0x01;
        const int ALT_O = 0x02;
        const int TM_YEAR_BASE = 1900;

        const int TS_SEC_PER_HR = 3600;
        const int TS_SEC_PER_MIN = 60;
        const int TS_MIN_PER_HR = 60;
        const int TS_HRS_PER_DAY = 24;
        const int TS__NO_SCALE_FACTOR = 1;
        const int TS_SEC_SCALE_FACTOR = 32000;
        const int TS_MIN_SCALE_FACTOR = 1920000;
        const int TS_HOURSCALE_FACTOR = 115200000;

        public const int SCHAR_MIN = (-128);
        public const int SCHAR_MAX = 127;
        public const int UCHAR_MAX = 0xff;
        public const int SHRT_MIN = (-32768);
        public const int SHRT_MAX = 32767;
        public const int USHRT_MAX = 0xffff;
        public const int INT_MIN = (-2147483647 - 1);
        public const int INT_MAX = 2147483647;
        public const uint UINT_MAX = 0xffffffff;
        public const int LONG_MIN = (-2147483647 - 1);
        public const int LONG_MAX = 2147483647;
        public const uint ULONG_MAX = 0xffffffff;
        public const Int64 LLONG_MAX = 9223372036854775807;
        public const Int64 LLONG_MIN = (-9223372036854775807 - 1);
        public const UInt64 ULLONG_MAX = 0xffffffffffffffff;

        public const int RESPONSECODE_SYMID = 150;
        public const int DEVICESTATUS_SYMID = 151;
        public const int COMMSTATUS_SYMID = 152;
        public const int MANUFACTURER_SYMID = 153;
        public const int DEVICETYPE_SYMID = 154;
        public const int UNIVERSALREV_SYMID = 156;
        public const int DEVICEID_SYMID = 161;
        public const int POLLINGADDRESS_SYMID = 162;

        public const int STATUS_SIZE = 3;   /* size of status array				  */
        public const int STATUS_RESPONSE_CODE = 0;
        public const int STATUS_COMM_STATUS = 1;
        public const int STATUS_DEVICE_STATUS = 2;

        public const int RESP_MASK_LEN = 16;    /* size of response code masks		*/
        public const int DATA_MASK_LEN = 25;    /* size of data masks				*/
        public const int MAX_XMTR_STATUS_LEN = DATA_MASK_LEN;

        public const int RESPONSE_BUFFER_LENGTH = 40;   /* size of buffer string to place resp*/
        public const int BI_DISP_STR_LEN = 126;/* size of # lines X # char/line in a response code display */

        public const int NORMAL_CMD = 0;
        public const int MORE_STATUS_CMD = 1;

        public static DateTime baseTime, lastTime, wrkTm;
        public static bool blogTime = false;

        public static void captureStartTime()
        {
            Common.baseTime = DateTime.Now;
            Common.lastTime = DateTime.Now;
        }

        public static byte CheckSums(byte[] pucCheck, byte ucLength, byte pre = 0)
        {
            byte i = 0;
            byte ucSum = 0;
            for (i = 0; i < ucLength - pre; i++)
            {
                ucSum ^= (pucCheck[pre + i]);//计算校验和
            }
            return ucSum;
        }

        public static void logTime()
        {
            TimeSpan ts;
            if (blogTime)
            {
                wrkTm = DateTime.Now;
                ts = wrkTm.Subtract(baseTime);
                double eMillitime = ts.TotalMilliseconds;// wrkTm.Millisecond - baseTime.Millisecond;
                ts = wrkTm.Subtract(lastTime);
                double sMillitime = ts.TotalMilliseconds;// wrkTm.Millisecond - baseTime.Millisecond;
                /*
                Int64 eTime = ;// wrkTm..time - baseTime.time;
                if (eMillitime < 0)
                {
                    eMillitime += 1000;
                    eTime -= 1;
                }

                int sMillitime = wrkTm.Millisecond - lastTime.Millisecond;
                Int64 sTime = wrkTm.time - lastTime.time;
                if (sMillitime < 0)
                {
                    sMillitime += 1000; 
                    sTime -= 1;
                }
                */

                //LOGIT(CLOG_LOG, " Elapsed time = %I64d.%d secs Split: %I64d.%d\n", eTime, eMillitime, sTime, sMillitime);
                lastTime = DateTime.Now;
            }
            // else it's an empty call
        }

        public static uint getVarIDItemArray(HARTDevice hartDev, colletion_member cmember, uint member)
        {
            uint id = 0;
            if (cmember.items.Count >= 2)//should be 2
            {
                CDDLBase cb2 = new CDDLBase();
                hartDev.getItembyID(cmember.items[cmember.items.Count - 1].uiID, ref cb2);
                CDDLItemArray cit2 = (CDDLItemArray)cb2;
                int index2 = 0xfff;//(int)cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f;

                for (int j = 0; j < cit2.arrayitems.Count; j++)
                {
                    uint uvarindex = 0;
                    CDDLBase dlbase = null;
                    if (cmember.items[cmember.items.Count - 2].index_s[0].byElemType == VARID_OPCODE)
                    {
                        if (hartDev.getItembyID(cmember.items[cmember.items.Count - 2].index_s[0].uiIndex, ref dlbase))
                        {
                            CDDLVar idVar = (CDDLVar)dlbase;
                            uvarindex = idVar.getDispValue().GetUInt();
                        }
                    }
                    else
                    {
                        uvarindex = cmember.items[cmember.items.Count - 2].index_s[0].uiIndex;//.member & 0x0f;
                    }
                    if (uvarindex  == cit2.arrayitems[j].uiIndex)
                    {
                        index2 = j;
                        break;
                    }
                }

                if (cit2.arrayitems[index2].items.Count == 1 && cit2.arrayitems[index2].items[0].uType == ddpREF.COLLECTION_ID_REF)
                {
                    CDDLBase ddbi3 = new CDDLBase();

                    if (hartDev.getItembyID(cit2.arrayitems[index2].items[0].uiID, ref ddbi3))
                    {
                        CDDLCollection collection2 = (CDDLCollection)ddbi3;
                        colletion_member cmember2 = null;
                        foreach (colletion_member cmem in collection2.collectionmembers)
                        {
                            if (cmem.uiName == member)
                            {
                                cmember2 = cmem;
                                break;
                            }
                        }
                        if (cmember2 != null)
                        {
                            menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                            switch(it2.uType)
                            {
                                case ddpREF.VARIABLE_ID_REF:
                                    {
                                        CDDLBase ddin2 = new CDDLBase();
                                        if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                        {
                                            id = it2.uiID;
                                        }
                                    }
                                    break;

                                case ddpREF.ITEM_ARRAY_ID_REF:
                                    //id = getVarIDItemArray(hartDev, cmember2, 0);
                                    break;

                                default:
                                    break;

                            }
                        }
                    }
                }
            }


            return id;
        }

        public static uint getVarIDItemArray(HARTDevice hartDev, ddpREFERENCE dref)
        {
            uint id = 0;
            CDDLBase pItem = new CDDLBase();

            if (hartDev.getItembyID(dref[dref.Count - 1].id, ref pItem))
            {
                if (pItem.IsItemArray() && dref.Count >= 2)//4???
                {
                    CDDLItemArray itemarray = (CDDLItemArray)pItem;
                    if (dref[dref.Count - 2].index.Count == 1 && dref[dref.Count - 2].type == ddpREF.VIA_ITEM_ARRAY_REF)
                    {
                        int index = 0;// xfff;// = (int)ml[ml.Count - 2].index_s[0].uiIndex & 0x0f;

                        CDDLBase dlbase = new CDDLBase();

                        for (int j = 0; j < itemarray.arrayitems.Count; j++)
                        {
                            uint uvarindex = 0;
                            if (dref[dref.Count - 2].index[0].byElemType == VARID_OPCODE)
                            {
                                if (hartDev.getItembyID(dref[dref.Count - 2].index[0].varId, ref dlbase))
                                {
                                    CDDLVar idVar = (CDDLVar)dlbase;
                                    uvarindex = idVar.getDispValue().GetUInt();
                                }
                            }
                            else
                            {
                                uvarindex = (uint)dref[dref.Count - 2].index[0].ulConst;//.member & 0x0f;
                            }

                            if (uvarindex == itemarray.arrayitems[j].uiIndex)
                            {
                                index = j;
                                break;
                            }
                        }

                        if (itemarray.arrayitems[index].items.Count == 1 && itemarray.arrayitems[index].items[0].uType == ddpREF.COLLECTION_ID_REF)
                        {
                            CDDLBase ddbi = new CDDLBase();

                            if (hartDev.getItembyID(itemarray.arrayitems[index].items[0].uiID, ref ddbi))
                            {
                                CDDLCollection collection = (CDDLCollection)ddbi;
                                colletion_member cmember = null;
                                foreach (colletion_member cmem in collection.collectionmembers)
                                {
                                    if (cmem.uiName == dref[dref.Count - 3].member)
                                    {
                                        cmember = cmem;
                                        break;
                                    }
                                }
                                if (cmember != null)
                                {
                                    menu_item it = cmember.items[cmember.items.Count - 1];
                                    CDDLBase ddin = new CDDLBase();
                                    switch (it.uType)
                                    {
                                        case ddpREF.ITEM_ARRAY_ID_REF:
                                            {
                                                id = getVarIDItemArray(hartDev, cmember, dref[dref.Count - 4].member);
                                            }
                                            break;

                                        case ddpREF.COLLECTION_ID_REF:
                                            if (cmember.items.Count == 1)//should be 1
                                            {
                                                CDDLBase ddbi3 = new CDDLBase();

                                                if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi3))
                                                {
                                                    CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                    colletion_member cmember2 = null;
                                                    foreach (colletion_member cmem in collection2.collectionmembers)
                                                    {
                                                        if (cmem.uiName == dref[dref.Count - 4].member)
                                                        {
                                                            cmember2 = cmem;
                                                            break;
                                                        }
                                                    }
                                                    if (cmember2 != null)
                                                    {
                                                        menu_item it2 = cmember2.items[cmember2.items.Count - 1];

                                                        switch (it2.uType)
                                                        {
                                                            case ddpREF.VARIABLE_ID_REF:
                                                                {
                                                                    CDDLBase ddin2 = new CDDLBase();
                                                                    if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                    {
                                                                        id = it2.uiID;
                                                                    }
                                                                }
                                                                break;

                                                            case ddpREF.ITEM_ARRAY_ID_REF:
                                                                id = getVarIDItemArray(hartDev, cmember2, cmember2.items[0].uiMember);
                                                                break;

                                                            default:
                                                                break;

                                                        }
                                                    }
                                                }
                                            }
                                            break;

                                        case ddpREF.VARIABLE_ID_REF:
                                            CDDLBase ddbi4 = new CDDLBase();

                                            if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi4))
                                            {
                                                id = cmember.items[0].uiID;
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return id;
        }

        public static string GetLangStr(string input)
        {
            if (input == null)
            {
                return "";
            }

            string output = input;
            string var = "";
            if (Regex.IsMatch(input, "|.*"))
            {
                byte[] info = Encoding.Default.GetBytes(input);
                input = Encoding.UTF8.GetString(info);
                string[] mull = input.Split('|');
                for (int i = 1; i < mull.Count(); i++)
                {
                    if (mull[i] == "en" || mull[i] == "en zz")
                    {
                        output = mull[i + 1];
                        var = output;
                    }

                    if (mull[i] == Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2))
                    {
                        output = mull[i + 1];
                        if (mull[i] == "zh")
                        {
                            switch (var)
                            {
                                case "SV is":
                                    output = "第2变量为";
                                    break;

                                case "TV is":
                                    output = "第3变量为";
                                    break;

                                case "QV is":
                                    output = "第4变量为";
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;
                    }
                }
                return output;
            }
            return output;
        }


        public static string doFormat(string formatStr, CValueVarient vValue)
        {
            string retStr = null;
            if (formatStr == null || formatStr == "")
            {
                /*
                string t = vValue.sStringVal;
                if ((int)t.Length < rsLen)
                {
                    retStr = t;
                }
                else
                {
                    //_tstrncpy(retStr, t.c_str(), rsLen - 1);
                    //retStr[rsLen - 1] = _T('\0');
                    retStr = t.Substring(0, rsLen - 1);
                }
                */
                return vValue.GetDispString();  // works good
            }
            // else we have some unique formatting, we'll have to deal with it

            //string pch = formatStr;//.Substring(formatStr.IndexOf('%'));// _tstrchar(formatStr, _T('%'));
                                                                     //string theChar = _T('\0');
            string newformat = null;
            char theChar = '\0';
            if (formatStr != null) // real formatting
            {//	no spaces allowed in formatting, so get the last char
                newformat = Common.GetLangStr(formatStr);
                theChar = formatStr[0];// start with the '%'
                foreach (char p in formatStr)
                {
                    //pch++;
                    if (p == ('\0') || p == (' ') || p == ('\t') || p == ('\n'))
                        break;
                    //else
                    theChar = p;// we want the last char
                }
            }// else - leave cahr at null to get to default


            switch (theChar)
            {
                case 'c':
                case 'C':
                    //_tsprintf(retStr, formatStr, (char)vValue);
                    retStr = String.Format(newformat, vValue.GetBool());
                    break;
                case 'd':
                case 'i':
                    //_tsprintf(retStr, formatStr, (int)vValue);
                    retStr = String.Format(newformat, vValue.GetInt());
                    break;
                case 'o':
                case 'u':
                case 'x':
                case 'X':
                    //_tsprintf(retStr, formatStr, (uint)vValue);
                    retStr = String.Format(newformat, vValue.GetUInt());
                    break;
                case 'e':
                case 'E':
                case 'f':
                case 'g':
                case 'G':
                    newformat = newformat.Substring(0, newformat.Length - 1);
                    string it = Regex.Match(newformat, @"\d+\.").Value;
                    if(it == null || it == "")
                    {
                        it = "0";
                    }
                    string dc = Regex.Match(newformat, @"\.\d+").Value;
                    if (dc != "")
                    {
                        dc = dc.Substring(1);
                    }
                    //newformat = "{" + it + ":" + dc + "F}";
                    newformat = "{0:F" + dc + "}";
                    if (vValue.vIsDouble)
                    {
                        double d = vValue.GetDouble();
                        //_tsprintf(retStr, formatStr, d);
                        retStr = String.Format(newformat, d);
                    }
                    else
                    {
                        float y = (float)vValue.GetFloat();
                        //_tsprintf(retStr, formatStr, y);
                        retStr = String.Format(newformat, y);
                    }
                    break;
                case 's':
                case 'S':
                    //_tsprintf(retStr, formatStr, ((string)vValue).c_str());
                    retStr = vValue.GetString();
                    break;
                case '%':
                default:
                    //_tsprintf(retStr, formatStr);
                    retStr = vValue.GetFloat().ToString();
                    break;
            }
            return retStr; // no error
        }

        public static uint getVarID(HARTDevice hartDev, DATA_ITEM di)
        {
            CDDLBase pItem = new CDDLBase();
            uint id = 0;

            if (di.type > Common.DATA_CONSTANT && di.type < Common.DATA_FLOATING)
            {
                switch (di.data.reff[di.data.reff.Count - 1].type)
                {
                    case ddpREF.ITEM_ARRAY_ID_REF:
                        {
                            id = getVarIDItemArray(hartDev, di.data.reff);
                        }
                        break;

                    case ddpREF.COLLECTION_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[di.data.reff.Count - 1].id, ref pItem))
                            {
                                CDDLCollection collection = (CDDLCollection)pItem;
                                colletion_member cmember = null;
                                foreach (colletion_member cmem in collection.collectionmembers)
                                {
                                    if (cmem.uiName == di.data.reff[di.data.reff.Count - 2].member)
                                    {
                                        cmember = cmem;
                                        break;
                                    }
                                }
                                if (cmember != null)
                                {
                                    menu_item it2 = cmember.items[cmember.items.Count - 1];
                                    if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                    {
                                        CDDLBase ddin2 = new CDDLBase();
                                        if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                        {
                                            id = it2.uiID;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.VARIABLE_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[0].id, ref pItem))
                            {
                                id = di.data.reff[0].id;
                            }
                        }
                        break;


                    default:
                        break;
                }

                //pV = (CDDLVar)pItem;
            }
            else
            {
                id = 0;
            }
            return id;
        }

        public static uint getVarID(HARTDevice hartDev, ddpREFERENCE reff)
        {
            CDDLBase pItem = new CDDLBase();
            uint id = 0;

            {
                switch (reff[reff.Count - 1].type)
                {
                    case ddpREF.ITEM_ARRAY_ID_REF:
                        {
                            if (hartDev.getItembyID(reff[reff.Count - 1].id, ref pItem))
                            {
                                if (pItem.IsItemArray() && reff.Count >= 2)//4???
                                {
                                    CDDLItemArray itemarray = (CDDLItemArray)pItem;
                                    if (reff[reff.Count - 2].index.Count == 1
                                        && reff[reff.Count - 2].type == ddpREF.VIA_ITEM_ARRAY_REF)
                                    {
                                        int index = 0xfff;// = (int)ml[ml.Count - 2].index_s[0].uiIndex & 0x0f;

                                        CDDLBase dlbase = new CDDLBase();

                                        for (int j = 0; j < itemarray.arrayitems.Count; j++)
                                        {
                                            uint uvarindex = 0;
                                            if (reff[reff.Count - 2].index[0].byElemType == VARID_OPCODE)
                                            {
                                                if (hartDev.getItembyID(reff[reff.Count - 2].index[0].varId, ref dlbase))
                                                {
                                                    CDDLVar idVar = (CDDLVar)dlbase;
                                                    uvarindex = idVar.getDispValue().GetUInt();
                                                }
                                            }
                                            else
                                            {
                                                uvarindex = (uint)reff[reff.Count - 2].index[0].ulConst;//.member & 0x0f;
                                            }

                                            if (uvarindex == itemarray.arrayitems[j].uiIndex)
                                            {
                                                index = j;
                                                break;
                                            }
                                        }

                                        if (itemarray.arrayitems[index].items.Count == 1
                                            && itemarray.arrayitems[index].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                        {
                                            CDDLBase ddbi = new CDDLBase();

                                            if (hartDev.getItembyID(itemarray.arrayitems[index].items[0].uiID, ref ddbi))
                                            {
                                                CDDLCollection collection = (CDDLCollection)ddbi;
                                                colletion_member cmember = null;
                                                foreach (colletion_member cmem in collection.collectionmembers)
                                                {
                                                    if (cmem.uiName == reff[reff.Count - 3].member)
                                                    {
                                                        cmember = cmem;
                                                        break;
                                                    }
                                                }
                                                if (cmember != null)
                                                {
                                                    menu_item it = cmember.items[cmember.items.Count - 1];
                                                    CDDLBase ddin = new CDDLBase();
                                                    switch (it.uType)
                                                    {
                                                        case ddpREF.ITEM_ARRAY_ID_REF:
                                                            if (cmember.items.Count == 2)//should be 2
                                                            {
                                                                CDDLBase cb2 = new CDDLBase();
                                                                hartDev.getItembyID(cmember.items[cmember.items.Count - 1].uiID, ref cb2);
                                                                CDDLItemArray cit2 = (CDDLItemArray)cb2;
                                                                int index2 = 0xfff;//(int)cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f;

                                                                for (int j = 0; j < cit2.arrayitems.Count; j++)
                                                                {
                                                                    if ((cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f) == cit2.arrayitems[j].uiIndex)
                                                                    {
                                                                        index2 = j;
                                                                        break;
                                                                    }
                                                                }

                                                                if (cit2.arrayitems[index2].items.Count == 1 && cit2.arrayitems[index2].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                                                {
                                                                    CDDLBase ddbi3 = new CDDLBase();

                                                                    if (hartDev.getItembyID(cit2.arrayitems[index].items[0].uiID, ref ddbi3))
                                                                    {
                                                                        CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                                        colletion_member cmember2 = null;
                                                                        foreach (colletion_member cmem in collection2.collectionmembers)
                                                                        {
                                                                            if (cmem.uiName == reff[reff.Count - 4].member)
                                                                            {
                                                                                cmember2 = cmem;
                                                                                break;
                                                                            }
                                                                        }
                                                                        if (cmember2 != null)
                                                                        {
                                                                            menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                                                                            if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                                                            {
                                                                                CDDLBase ddin2 = new CDDLBase();
                                                                                if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                                {
                                                                                    id = it2.uiID;
                                                                                }

                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        case ddpREF.COLLECTION_ID_REF:
                                                            if (cmember.items.Count == 1)//should be 1
                                                            {
                                                                CDDLBase ddbi3 = new CDDLBase();

                                                                if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi3))
                                                                {
                                                                    CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                                    colletion_member cmember2 = null;
                                                                    foreach (colletion_member cmem in collection2.collectionmembers)
                                                                    {
                                                                        if (cmem.uiName == reff[reff.Count - 4].member)
                                                                        {
                                                                            cmember2 = cmem;
                                                                            break;
                                                                        }
                                                                    }
                                                                    if (cmember2 != null)
                                                                    {
                                                                        menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                                                                        if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                                                        {
                                                                            CDDLBase ddin2 = new CDDLBase();
                                                                            if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                            {
                                                                                id = it2.uiID;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        case ddpREF.VARIABLE_ID_REF:
                                                            CDDLBase ddbi4 = new CDDLBase();

                                                            if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi4))
                                                            {
                                                                id = cmember.items[0].uiID;
                                                            }
                                                            break;

                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.COLLECTION_ID_REF:
                        {
                            if (hartDev.getItembyID(reff[reff.Count - 1].id, ref pItem))
                            {
                                CDDLCollection collection = (CDDLCollection)pItem;
                                colletion_member cmember = null;
                                foreach (colletion_member cmem in collection.collectionmembers)
                                {
                                    if (cmem.uiName == reff[reff.Count - 2].member)
                                    {
                                        cmember = cmem;
                                        break;
                                    }
                                }
                                if (cmember != null)
                                {
                                    menu_item it2 = cmember.items[cmember.items.Count - 1];
                                    if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                    {
                                        CDDLBase ddin2 = new CDDLBase();
                                        if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                        {
                                            id = it2.uiID;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.VARIABLE_ID_REF:
                        {
                            if (hartDev.getItembyID(reff[0].id, ref pItem))
                            {
                                id = reff[0].id;
                            }
                        }
                        break;


                    default:
                        break;
                }

                //pV = (CDDLVar)pItem;
            }
            return id;
        }

        public static void Pack(ref byte[] dest, byte[] source, int nChar)
        {
            byte[] s = source;
            byte[] d = dest; // - 5,
            byte i;
            uint p;
            byte soff = 0;
            byte doff = 0;
            for (; nChar > 0; nChar -= 4)
            {
                // fill p with the 4 packed ascii char (24bits into p)
                for (p = i = 0; i < 4; i++)
                {
                    p <<= 6;
                    p |= (uint)(s[soff] & 0x3f);
                    soff++;
                }
                // put the three bytes into the packed ascii destination
                doff += 2;
                for (i = 0; i < 3; i++)
                {
                    d[doff] = (byte)(p & 0xff);
                    doff--;
                    p >>= 8;
                }
                doff += 4;
            }
        }

        public static void UnPack(ref byte[] dest, byte[] source, int nChar)
        {

            byte[] s = source;
            byte[] d = dest; // - 5,
            byte oneChar,        // where the raw byte is placed to flip the bits
            i, z;
            byte soff = 0;
            byte doff = 0;
            byte byFirstTime = 0;

            uint p;

            for (; nChar > 0; nChar -= 4)
            {
                // fill p with the 4 packed ascii char 
                // (24bits into p from s)
                for (p = i = 0; i < 3; i++)
                {
                    p <<= 8;
                    p |= s[soff++];
                }

                // unpack p
                /* VMKP Modified on 090304 */
                if (byFirstTime == 0)
                {
                    doff += 4;
                    byFirstTime = 1;
                }
                else
                {
                    doff += 8;
                }

                for (i = 0; i < 4; i++)
                {
                    oneChar = (byte)(p & 0x3F);     // get the LS 6 bits
                    p >>= 6;
                    if ((oneChar & 0x20) != 0)
                    {
                        z = oneChar;
                    }

                    else
                    {
                        z = (byte)(oneChar | 0x40);
                    }
                    --doff;
                    d[doff] = z;
                }
            }
            //return dest;
        }

        public static uint getVarID(HARTDevice hartDev, DATA_ITEM di, int refindex)
        {
            CDDLBase pItem = new CDDLBase();
            uint id = 0;

            if (di.type == Common.DATA_REFERENCE || di.type == Common.DATA_REF_WIDTH)
            {
                switch (di.data.reff[di.data.reff.Count - 1].type)
                {
                    case ddpREF.ITEM_ARRAY_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[di.data.reff.Count - 1].id, ref pItem))
                            {
                                if (pItem.IsItemArray() && di.data.reff.Count >= 2)//4???
                                {
                                    CDDLItemArray itemarray = (CDDLItemArray)pItem;
                                    if (di.data.reff[di.data.reff.Count - 2].index.Count == 1
                                        && di.data.reff[di.data.reff.Count - 2].type == ddpREF.VIA_ITEM_ARRAY_REF)
                                    {
                                        int index = 0xfff;// = (int)ml[ml.Count - 2].index_s[0].uiIndex & 0x0f;

                                        for (int j = 0; j < itemarray.arrayitems.Count; j++)
                                        {
                                            if ((refindex & 0x0f) == itemarray.arrayitems[j].uiIndex)
                                            {
                                                index = j;
                                                break;
                                            }
                                        }

                                        if (itemarray.arrayitems[index].items.Count == 1
                                            && itemarray.arrayitems[index].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                        {
                                            CDDLBase ddbi = new CDDLBase();

                                            if (hartDev.getItembyID(itemarray.arrayitems[index].items[0].uiID, ref ddbi))
                                            {
                                                CDDLCollection collection = (CDDLCollection)ddbi;
                                                colletion_member cmember = null;
                                                foreach (colletion_member cmem in collection.collectionmembers)
                                                {
                                                    if (cmem.uiName == di.data.reff[di.data.reff.Count - 3].member)
                                                    {
                                                        cmember = cmem;
                                                        break;
                                                    }
                                                }
                                                if (cmember != null)
                                                {
                                                    menu_item it = cmember.items[cmember.items.Count - 1];
                                                    CDDLBase ddin = new CDDLBase();
                                                    switch (it.uType)
                                                    {
                                                        case ddpREF.ITEM_ARRAY_ID_REF:
                                                            if (cmember.items.Count == 2)//should be 2
                                                            {
                                                                CDDLBase cb2 = new CDDLBase();
                                                                hartDev.getItembyID(cmember.items[cmember.items.Count - 1].uiID, ref cb2);
                                                                CDDLItemArray cit2 = (CDDLItemArray)cb2;
                                                                int index2 = 0xfff;//(int)cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f;

                                                                for (int j = 0; j < cit2.arrayitems.Count; j++)
                                                                {
                                                                    if ((cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f) == cit2.arrayitems[j].uiIndex)
                                                                    {
                                                                        index2 = j;
                                                                        break;
                                                                    }
                                                                }

                                                                if (cit2.arrayitems[index2].items.Count == 1 && cit2.arrayitems[index2].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                                                {
                                                                    CDDLBase ddbi3 = new CDDLBase();

                                                                    if (hartDev.getItembyID(cit2.arrayitems[index].items[0].uiID, ref ddbi3))
                                                                    {
                                                                        CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                                        colletion_member cmember2 = null;
                                                                        foreach (colletion_member cmem in collection2.collectionmembers)
                                                                        {
                                                                            if (cmem.uiName == di.data.reff[di.data.reff.Count - 4].member)
                                                                            {
                                                                                cmember2 = cmem;
                                                                                break;
                                                                            }
                                                                        }
                                                                        if (cmember2 != null)
                                                                        {
                                                                            menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                                                                            if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                                                            {
                                                                                CDDLBase ddin2 = new CDDLBase();
                                                                                if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                                {
                                                                                    id = it2.uiID;
                                                                                }

                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        case ddpREF.COLLECTION_ID_REF:
                                                            if (cmember.items.Count == 1)//should be 1
                                                            {
                                                                CDDLBase ddbi3 = new CDDLBase();

                                                                if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi3))
                                                                {
                                                                    CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                                    colletion_member cmember2 = null;
                                                                    foreach (colletion_member cmem in collection2.collectionmembers)
                                                                    {
                                                                        if (cmem.uiName == di.data.reff[di.data.reff.Count - 4].member)
                                                                        {
                                                                            cmember2 = cmem;
                                                                            break;
                                                                        }
                                                                    }
                                                                    if (cmember2 != null)
                                                                    {
                                                                        menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                                                                        if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                                                        {
                                                                            CDDLBase ddin2 = new CDDLBase();
                                                                            if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                            {
                                                                                id = it2.uiID;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        case ddpREF.VARIABLE_ID_REF:
                                                            CDDLBase ddbi4 = new CDDLBase();

                                                            if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi4))
                                                            {
                                                                id = cmember.items[0].uiID;
                                                            }
                                                            break;

                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.COLLECTION_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[di.data.reff.Count - 1].id, ref pItem))
                            {
                                CDDLCollection collection = (CDDLCollection)pItem;
                                colletion_member cmember = null;
                                foreach (colletion_member cmem in collection.collectionmembers)
                                {
                                    if (cmem.uiName == di.data.reff[di.data.reff.Count - 2].member)
                                    {
                                        cmember = cmem;
                                        break;
                                    }
                                }
                                if (cmember != null)
                                {
                                    menu_item it2 = cmember.items[cmember.items.Count - 1];
                                    if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                    {
                                        CDDLBase ddin2 = new CDDLBase();
                                        if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                        {
                                            id = it2.uiID;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.VARIABLE_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[0].id, ref pItem))
                            {
                                id = di.data.reff[0].id;
                            }
                        }
                        break;


                    default:
                        break;
                }

                //pV = (CDDLVar)pItem;
            }
            else
            {
                id = 0;
            }
            return id;
        }

        public static CDDLVar getVarPtr(HARTDevice hartDev, DATA_ITEM di)
        {
            CDDLBase pItem = new CDDLBase();
            CDDLVar pV = null;

            if (di.type == Common.DATA_REFERENCE)
            {
                switch (di.data.reff[di.data.reff.Count - 1].type)
                {
                    case ddpREF.ITEM_ARRAY_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[di.data.reff.Count - 1].id, ref pItem))
                            {
                                if (pItem.IsItemArray() && di.data.reff.Count >= 2)//4???
                                {
                                    CDDLItemArray itemarray = (CDDLItemArray)pItem;
                                    if (di.data.reff[di.data.reff.Count - 2].index.Count == 1
                                        && di.data.reff[di.data.reff.Count - 2].type == ddpREF.VIA_ITEM_ARRAY_REF)
                                    {
                                        int index = 0xfff;// = (int)ml[ml.Count - 2].index_s[0].uiIndex & 0x0f;

                                        for (int j = 0; j < itemarray.arrayitems.Count; j++)
                                        {
                                            uint uvarindex = 0;
                                            CDDLBase dlbase = new CDDLBase();
                                            if (di.data.reff[di.data.reff.Count - 2].index[0].byElemType == VARID_OPCODE)
                                            {
                                                if (hartDev.getItembyID(di.data.reff[di.data.reff.Count - 2].index[0].varId, ref dlbase))
                                                {
                                                    CDDLVar idVar = (CDDLVar)dlbase;
                                                    uvarindex = idVar.getDispValue().GetUInt();
                                                }
                                            }
                                            else
                                            {
                                                uvarindex = (uint)di.data.reff[di.data.reff.Count - 2].index[0].ulConst;//.member & 0x0f;
                                            }

                                            if (uvarindex == itemarray.arrayitems[j].uiIndex)
                                            {
                                                index = j;
                                                break;
                                            }
                                        }

                                        if (itemarray.arrayitems[index].items.Count == 1
                                            && itemarray.arrayitems[index].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                        {
                                            CDDLBase ddbi = new CDDLBase();

                                            if (hartDev.getItembyID(itemarray.arrayitems[index].items[0].uiID, ref ddbi))
                                            {
                                                CDDLCollection collection = (CDDLCollection)ddbi;
                                                colletion_member cmember = null;
                                                foreach (colletion_member cmem in collection.collectionmembers)
                                                {
                                                    if (cmem.uiName == di.data.reff[di.data.reff.Count - 3].member)
                                                    {
                                                        cmember = cmem;
                                                        break;
                                                    }
                                                }
                                                if (cmember != null)
                                                {
                                                    menu_item it = cmember.items[cmember.items.Count - 1];
                                                    CDDLBase ddin = new CDDLBase();
                                                    switch (it.uType)
                                                    {
                                                        case ddpREF.ITEM_ARRAY_ID_REF:
                                                            if (cmember.items.Count == 2)//should be 2
                                                            {
                                                                CDDLBase cb2 = new CDDLBase();
                                                                hartDev.getItembyID(cmember.items[cmember.items.Count - 1].uiID, ref cb2);
                                                                CDDLItemArray cit2 = (CDDLItemArray)cb2;
                                                                int index2 = 0xfff;//(int)cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f;

                                                                for (int j = 0; j < cit2.arrayitems.Count; j++)
                                                                {
                                                                    if ((cmember.items[cmember.items.Count - 2].index_s[0].uiIndex & 0x0f) == cit2.arrayitems[j].uiIndex)
                                                                    {
                                                                        index2 = j;
                                                                        break;
                                                                    }
                                                                }

                                                                if (cit2.arrayitems[index2].items.Count == 1 && cit2.arrayitems[index2].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                                                {
                                                                    CDDLBase ddbi3 = new CDDLBase();

                                                                    if (hartDev.getItembyID(cit2.arrayitems[index].items[0].uiID, ref ddbi3))
                                                                    {
                                                                        CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                                        colletion_member cmember2 = null;
                                                                        foreach (colletion_member cmem in collection2.collectionmembers)
                                                                        {
                                                                            if (cmem.uiName == di.data.reff[di.data.reff.Count - 4].member)
                                                                            {
                                                                                cmember2 = cmem;
                                                                                break;
                                                                            }
                                                                        }
                                                                        if (cmember2 != null)
                                                                        {
                                                                            menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                                                                            if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                                                            {
                                                                                CDDLBase ddin2 = new CDDLBase();
                                                                                if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                                {
                                                                                    pV = (CDDLVar)ddin2;
                                                                                }

                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        case ddpREF.COLLECTION_ID_REF:
                                                            if (cmember.items.Count == 1)//should be 1
                                                            {
                                                                CDDLBase ddbi3 = new CDDLBase();

                                                                if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi3))
                                                                {
                                                                    CDDLCollection collection2 = (CDDLCollection)ddbi3;
                                                                    colletion_member cmember2 = null;
                                                                    foreach (colletion_member cmem in collection2.collectionmembers)
                                                                    {
                                                                        if (cmem.uiName == di.data.reff[di.data.reff.Count - 4].member)
                                                                        {
                                                                            cmember2 = cmem;
                                                                            break;
                                                                        }
                                                                    }
                                                                    if (cmember2 != null)
                                                                    {
                                                                        menu_item it2 = cmember2.items[cmember2.items.Count - 1];
                                                                        if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                                                        {
                                                                            CDDLBase ddin2 = new CDDLBase();
                                                                            if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                                                            {
                                                                                pV = (CDDLVar)ddin2;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        case ddpREF.VARIABLE_ID_REF:
                                                            CDDLBase ddbi4 = new CDDLBase();

                                                            if (hartDev.getItembyID(cmember.items[0].uiID, ref ddbi4))
                                                            {
                                                                pV = (CDDLVar)ddbi4;
                                                            }
                                                            break;

                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.COLLECTION_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[di.data.reff.Count - 1].id, ref pItem))
                            {
                                CDDLCollection collection = (CDDLCollection)pItem;
                                colletion_member cmember = null;
                                foreach (colletion_member cmem in collection.collectionmembers)
                                {
                                    if (cmem.uiName == di.data.reff[di.data.reff.Count - 2].member)
                                    {
                                        cmember = cmem;
                                        break;
                                    }
                                }
                                if (cmember != null)
                                {
                                    menu_item it2 = cmember.items[cmember.items.Count - 1];
                                    if (it2.uType == ddpREF.VARIABLE_ID_REF)
                                    {
                                        CDDLBase ddin2 = new CDDLBase();
                                        if (hartDev.getItembyID(it2.uiID, ref ddin2))
                                        {
                                            pV = (CDDLVar)ddin2;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ddpREF.VARIABLE_ID_REF:
                        {
                            if (hartDev.getItembyID(di.data.reff[0].id, ref pItem))
                            {
                                pV = (CDDLVar)pItem;
                            }
                        }
                        break;


                    default:
                        break;
                }

                //pV = (CDDLVar)pItem;
            }
            else
            {
                pV = null;
            }
            return pV;
        }

        static void fill_local()
        {
            // set the default then set locale so any error will leave the default
            int i;
            for (i = 0; i < 7; i++)
                CurrentTimeLocale.abday[i] = abday[i];
            for (i = 0; i < 7; i++)
                CurrentTimeLocale.day[i] = day[i];
            for (i = 0; i < 12; i++)
                CurrentTimeLocale.abmon[i] = abmon[i];
            for (i = 0; i < 12; i++)
                CurrentTimeLocale.mon[i] = mon[i];
            CurrentTimeLocale.am_pm[0] = am_pm[0];
            CurrentTimeLocale.am_pm[1] = am_pm[1];
            CurrentTimeLocale.d_t_fmt = d_t_fmt;
            CurrentTimeLocale.t_fmt_ampm = t_fmt_ampm;
            CurrentTimeLocale.t_fmt = t_fmt;
            CurrentTimeLocale.d_fmt = d_fmt;
            areSet = true;
            return;
        }

        static double roundHalfup(double value)
        {
            return Math.Floor(value + 0.5);
        }

        static double roundUp(double value)
        {
            double result = roundHalfup(Math.Abs(value));
            return (value < 0.0) ? -result : result;
        }

        public static double roundDbl(double toBrounded, int numberOfPlaces)
        {
            if (numberOfPlaces > 10 || numberOfPlaces < -10)
            {
                //LOGIT(CERR_LOG, "Error: round size out of range.\n");
                return toBrounded;
            }
            double left, right, wrk, ret, wwrk;
            double plier = Math.Pow(10.0, numberOfPlaces);
            if (numberOfPlaces < 0 && toBrounded < plier)
            {// too small to be rounded to that...
                return toBrounded;
            }

            //right = Math.modf(toBrounded, &left);

            left = Math.Truncate(toBrounded);

            if (toBrounded >= 0)
            {
                right = toBrounded - left;
            }
            else
            {
                right = left - toBrounded;
            }

            if (numberOfPlaces == 0)
            {
                ret = roundHalfup(toBrounded);
            }
            else
            if (numberOfPlaces > 0)
            {
                wrk = right * plier;
                wrk = roundUp(wrk);
                wwrk = wrk / plier;
                ret = wwrk * plier;
                ret = left + wwrk;

            }
            else // numberOfPlaces < 0
            {
                wrk = left * plier;//aka  left / abs(plier)
                wrk = roundUp(wrk);
                ret = (wrk / plier);
            }
            return ret;
        }

        public static ushort extractStruct(tm pTime)
        {
            ushort retVal = 0;
            ushort Min = (ushort)((pTime.tm_hour * TS_MIN_PER_HR) + pTime.tm_min);
            ushort Sec = (ushort)((Min * TS_SEC_PER_MIN) + pTime.tm_sec);
            retVal = (ushort)(Sec * TS_SEC_SCALE_FACTOR);
            return retVal;
        }

        public static void getActiveValues(string fmtStr, ref timeScale_t returnMask)
        {
            ushort x = 0, y = (ushort)returnMask;
            string locStr;
            int loc;
            char[] bc = { 'H', 'I', 'M', 'p', 'R', 'S', 'T' };

            if ((loc = fmtStr.IndexOfAny(bc)) != -1)
            {
                locStr = fmtStr.Substring(loc + 1); // post found letter
                getActiveValues(locStr, ref returnMask);// look in the rest of the string
                y = (ushort)returnMask;
                if (fmtStr[loc] == 'H' /*|| fmtStr[loc] == 'I'*/ )
                {
                    x = (ushort)timeScale_t.tsHr_Scale;
                }
                // Fixed defect #4511, 'I' now has its its own category, POB - 4/21/2014
                else if (fmtStr[loc] == 'I')
                {
                    x = (ushort)timeScale_t.ts_I_Scale;
                }
                else if (fmtStr[loc] == 'M')
                {
                    x = (ushort)timeScale_t.tsMinScale;
                }
                else if (fmtStr[loc] == 'S')
                {
                    x = (ushort)timeScale_t.tsSecScale;
                }
                else if (fmtStr[loc] == 'p')
                {
                    x = (ushort)timeScale_t.ts_p_Scale; // Fixed defect #4511, to determine if 'p' is present, POB - 4/21/2014
                }
                else
                {
                    //LOGIT(CLOG_LOG, "Warning: Format string %s has an R or T illegally.\n", fmtStr.c_str());
                }
            }
            //else we are the end of the string - no more to do
            y |= x;
            returnMask = (timeScale_t)y;
        }

        public static void fillStruct(ref tm pTime, ushort bgtm)
        {
            pTime.tm_isdst = -1;      // unknown if it is or not DST
            pTime.tm_mday =
            pTime.tm_mon =
            pTime.tm_year =
            pTime.tm_wday =
            pTime.tm_yday = 0;        // we are not doing date

            ushort s = (ushort)(bgtm / TS_SEC_SCALE_FACTOR);// total seconds
            ushort m = (ushort)(s / TS_SEC_PER_MIN);     // total minutes w/ tm_sec the fractional part
            pTime.tm_sec = s % TS_SEC_PER_MIN;     // seconds remaining
            ushort h = (ushort)(m / TS_MIN_PER_HR);      // hours
            pTime.tm_min = m % TS_MIN_PER_HR;      // minutes remaining
            pTime.tm_hour = h % TS_HRS_PER_DAY;     // hours remaining (it can roll over)
            return;
        }

        static int find_string(string bp, ref int tgt, string[] n1, string[] n2, int c)
        {
            int i;
            int len;

            /* check full name - then abbreviated ones */
            for (; n1 != null; n1 = n2, n2 = null)
            {
                for (i = 0; i < c; i++)
                {
                    len = n1[i].Length;
                    //if (strncasecmp(n1, bp, len) == 0)
                    if (n1[i] == bp.Substring(0, len))
                    {
                        tgt = i;
                        return len;
                    }
                }
            }

            /* Nothing matched */
            return 0;
        }

        static int conv_num(string buf, ref int dest, uint llim, uint ulim)
        {
            int result = 0;
            char ch;

            /* The limit also determines the number of valid digits. */
            uint rulim = ulim;

            int i = 0;
            ch = buf[i];
            if (ch < '0' || ch > '9')
                return 0;

            do
            {
                result *= 10;
                result += ch - '0';
                rulim /= 10;
                ch = buf[++i];

            }
            while ((result * 10 <= ulim) && rulim != 0 && ch >= '0' && ch <= '9');

            if (result < llim || result > ulim)
                return 0;

            dest = result;
            return i;
        }

        public static string strptime(string buf, string fmt, tm tm)
        {
            //char c;
            string bp;
            int alt_format = 0, i = 0, split_year = 0;
            string new_fmt;

            if (!areSet) fill_local();

            bp = buf;
            //int bp = 0;
            int ibp = 0;

            bool bfromstart = true;

            foreach (char c in fmt)//(bp != null && (c = *fmt++) != '\0')
            {
                bp = buf.Substring(ibp);
                if (bfromstart)
                {
                    /* Clear `alternate' modifier prior to new conversion. */
                    alt_format = 0;
                    i = 0;

                    /* Eat up white-space. */
                    if (Char.IsWhiteSpace(c))
                    {
                        while (Char.IsWhiteSpace(bp[ibp]))
                        {
                            ibp++;
                        }
                        continue;
                    }

                    if (c != '%')
                    {
                        if (c != bp[ibp++])
                            return null;
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;
                    }
                }
                bfromstart = true;
                switch (c)
                {
                    case '%':       /* "%%" is converted to "%". */
                        if (c != bp[ibp++])
                            return null;
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;

                    /*
                     * "Alternative" modifiers. Just set the appropriate flag
                     * and start over again.
                     */
                    case 'E':       /* "%E?" alternative conversion modifier. */
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        alt_format |= ALT_E;
                        bfromstart = false;
                        continue;

                    case 'O':       /* "%O?" alternative conversion modifier. */
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        alt_format |= ALT_O;
                        bfromstart = false;
                        continue;
                    //goto again;

                    /*
                     * "Complex" conversion rules, implemented through recursion.
                     */
                    case 'c':       /* Date and time, using the locale's format. */
                        new_fmt = CurrentTimeLocale.d_t_fmt;
                        goto recurse;

                    case 'D':       /* The date as "%m/%d/%y". */
                        new_fmt = "%m/%d/%y";
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        goto recurse;

                    case 'R':       /* The time as "%H:%M". */
                        new_fmt = "%H:%M";
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        goto recurse;

                    case 'r':       /* The time in 12-hour clock representation. */
                        new_fmt = CurrentTimeLocale.t_fmt_ampm;
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        goto recurse;

                    case 'T':       /* The time as "%H:%M:%S". */
                        new_fmt = "%H:%M:%S";
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        goto recurse;

                    case 'X':       /* The time, using the locale's format. */
                        new_fmt = CurrentTimeLocale.t_fmt;
                        goto recurse;

                    case 'x':       /* The date, using the locale's format. */
                        new_fmt = CurrentTimeLocale.d_fmt;
                    recurse:
                        bp = strptime(bp, new_fmt, tm);
                        if ((alt_format & ~(ALT_E)) != 0)
                            return null;
                        continue;

                    /*
                     * "Elementary" conversion rules.
                     */
                    case 'A':       /* The day of week, using the locale's form. */
                    case 'a':
                        ibp += find_string(bp, ref tm.tm_wday, CurrentTimeLocale.day, CurrentTimeLocale.abday, 7);
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;

                    case 'B':       /* The month, using the locale's form. */
                    case 'b':
                    case 'h':
                        ibp += find_string(bp, ref tm.tm_mon, CurrentTimeLocale.mon, CurrentTimeLocale.abmon, 12);
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;

                    case 'C':       /* The century number. */
                        i = 20;
                        ibp += conv_num(bp, ref i, 0, 99);

                        i = i * 100 - TM_YEAR_BASE;
                        if (split_year != 0)
                            i += tm.tm_year % 100;
                        split_year = 1;
                        tm.tm_year = i;
                        if ((alt_format & ~(ALT_E)) != 0)
                            return null;
                        continue;

                    case 'd':       /* The day of month. */
                    case 'e':
                        ibp += conv_num(bp, ref tm.tm_mday, 1, 31);
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'k':       /* The hour (24-hour clock representation). */
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;
                    /* FALLTHROUGH */
                    case 'H':
                        ibp += conv_num(bp, ref tm.tm_hour, 0, 23);
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'l':       /* The hour (12-hour clock representation). */
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;
                    /* FALLTHROUGH */
                    case 'I':
                        ibp += conv_num(bp, ref tm.tm_hour, 1, 12);
                        if (tm.tm_hour == 12)
                            tm.tm_hour = 0;
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'j':       /* The day of year. */
                        i = 1;
                        ibp += conv_num(bp, ref i, 1, 366);
                        tm.tm_yday = i - 1;
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;

                    case 'M':       /* The minute. */
                        ibp += conv_num(bp, ref tm.tm_min, 0, 59);
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'm':       /* The month. */
                        i = 1;
                        ibp += conv_num(bp, ref i, 1, 12);
                        tm.tm_mon = i - 1;
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'p':       /* The locale's equivalent of AM/PM. */
                        {
                            string[] targetStr = new string[2];
                            string tempStrAM = "", tempStrPM = "";
                            targetStr[0] = tempStrAM;
                            targetStr[1] = tempStrPM;
                            /*
                            if (GetLocaleInfo(LOCALE_SYSTEM_DEFAULT, LOCALE_S1159, tempStrAM, 12) |
                                 GetLocaleInfo(LOCALE_SYSTEM_DEFAULT, LOCALE_S2359, tempStrPM, 12))//success
                            {
                                ibp += find_string(bp, ref i, targetStr, null, 2);
                            }
                            else
                            */
                            {
                                ibp += find_string(bp, ref i, CurrentTimeLocale.am_pm, null, 2);
                            }
                            if (tm.tm_hour > 11)
                                return null;
                            tm.tm_hour += i * 12;
                            if ((alt_format & ~(0)) != 0)
                                return null;
                            continue;
                        }

                    case 'S':       /* The seconds. */
                        ibp += conv_num(bp, ref tm.tm_sec, 0, 59);  // fix Defect #4510, Change max value from 61 to 59, POB - 4/18/2014
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'U':       /* The week of year, beginning on sunday. */
                    case 'W':       /* The week of year, beginning on monday. */
                        /*
                         * XXX This is bogus, as we can not assume any valid
                         * information present in the tm structure at this
                         * point to calculate a real value, so just check the
                         * range for now.
                         */
                        ibp += conv_num(bp, ref i, 0, 53);
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'w':       /* The day of week, beginning on sunday. */
                        ibp += conv_num(bp, ref tm.tm_wday, 0, 6);
                        if ((alt_format & ~(ALT_O)) != 0)
                            return null;
                        continue;

                    case 'Y':       /* The year. */
                        i = TM_YEAR_BASE;       /* just for data sanity... */
                        ibp += conv_num(bp, ref i, 0, 9999);
                        tm.tm_year = i - TM_YEAR_BASE;
                        if ((alt_format & ~(ALT_E)) != 0)
                            return null;
                        continue;

                    case 'y':       /* The year within 100 years of the epoch. */
                        /* if ((alt_format & ~(ALT_E | ALT_O)) != 0)  return null; */
                        ibp += conv_num(bp, ref i, 0, 99);

                        if (split_year != 0)
                            /* preserve century */
                            i += (tm.tm_year / 100) * 100;
                        else
                        {
                            split_year = 1;
                            if (i <= 68)
                                i = i + 2000 - TM_YEAR_BASE;
                            else
                                i = i + 1900 - TM_YEAR_BASE;
                        }
                        tm.tm_year = i;
                        continue;

                    /*
                     * Miscellaneous conversions.
                     */
                    case 'n':       /* Any kind of white-space. */
                    case 't':
                        while (Char.IsWhiteSpace(bp[ibp]))
                        {
                            ibp++;
                        }
                        if ((alt_format & ~(0)) != 0)
                            return null;
                        continue;


                    default:        /* Unknown/unsupported conversion. */
                        return null;
                }
            }

            return bp;
        }

        public static uint datestr2int(string value, dtEditFmt_t editFormat)
        {
            uint LocVal;
            //UINT8 d=0,m=0,y=0;
            int d = 0, m = 0, y;
            int iy = 0, thisdec;//swscanf has to have %d var an int!(PaulW@GE)

            /*** stevev 6jul05 - modified all to support all dates intelligently ***/
            dmdate_t stDate = new dmdate_t(0, 0, 0);
            DateTime dt = DateTime.Now;
            //devPtr().getStartDate(stDate);
            stDate.year = (ushort)dt.Year;
            stDate.mnth = (byte)dt.Month;
            stDate.day = (byte)dt.Day;

            thisdec = ((int)(stDate.year / 100) * 100); // this decade

            ScanFormatted parser = new ScanFormatted();

            switch (editFormat)
            {
                case dtEditFmt_t.invsFormat:
                    {
                        parser.Parse(value, "%02d/%02d/%04d");
                        //if (swscanf(value.c_str(), "%02d/%02d/%04d", &d, &m, &iy) != 3)// dd/mm/yyyy
                        if (parser.Results.Count != 3)
                        {   // logit not enough digits
                            LocVal = 0;// leave d m y zero
                        }
                        else
                        {
                            d = (int)parser.Results[0];
                            m = (int)parser.Results[1];
                            iy = (int)parser.Results[2];
                        }
                        // else do the processing
                    }
                    break;
                case dtEditFmt_t.dotFormat:
                    {
                        parser.Parse(value, "%02d.%02d.%04d");
                        //if (swscanf(value.c_str(), "%02d.%02d.%04d", &d, &m, &iy) != 3)// dd.mm.yyyy
                        if (parser.Results.Count != 3)
                        {   // logit not enough digits
                            LocVal = 0;// leave d m y zero
                        }
                        else
                        {
                            d = (int)parser.Results[0];
                            m = (int)parser.Results[1];
                            iy = (int)parser.Results[2];
                        }
                        // else do the processing
                    }
                    break;
                case dtEditFmt_t.usFormat:
                    {   // start stevev 6jul05
                        parser.Parse(value, "%2d/%2d/%4d");
                        //if (swscanf(value.c_str(), "%2d/%2d/%4d", &m, &d, &iy) != 3)// mm/dd/yyyy
                        if (parser.Results.Count != 3)
                        {   // logit not enough digits
                            LocVal = 0;// leave d m y zero
                        }
                        else
                        {
                            m = (int)parser.Results[0];
                            d = (int)parser.Results[1];
                            iy = (int)parser.Results[2];
                        }
                        // else process it
                    }
                    break;
                default:
                    break;
            }// endswitch

            /* algorithm: force 2 digit year to be within +/- 50 years of SDC start date, 
                          convert to proper decade / millenia  or use 4 digits supplied  */
            if (iy < 100 && (m > 0 && d > 0))
            {
                iy += thisdec;// stevev 6jul05
                if ((iy - stDate.year) <= 50 && (iy - stDate.year) > -50)
                {
                    //iy = iy; // use as-is
                }
                else if ((iy - 100) < (stDate.year - 50))
                {
                    iy += 100;
                }
                else
                {
                    iy -= 100;
                }
                iy += 1900; // stevev 25jul07 - oops, missed a step.
            }
            else if (iy < 1900 || iy > 2155)
            {
                iy = 1900;
            }
            //else use it as - is
            // end  stevev 6jul05
            y = iy - 1900;

            LocVal = (uint)((d << 16) + (m << 8) + (y & 0xff));// all zeros on error

            //DEBUGLOG(CLOG_LOG, "Date conversion:|%s|  to 0x%04x\n", value.c_str(), LocVal);

            return LocVal;
        }

        public unsafe static int DDL_PARSE_INTEGER(byte** C, uint* L, UInt64* V)
        {
            if ((**(C) & 0x80) != 0)
            {
                int rc = ddl_parse_integer_func((C), (L), (V));
                if (rc != DDL_SUCCESS)
                    return DDL_ERROR_END; /*returns bool in sdc not::>rc;*/
            }
            else
            {
                if ((V) != null)
                {
                    *(V) = **(C);
                }
                ++(*(C));
                --(*(L));
            }
            return DDL_SUCCESS;
        }
        /*
        unsafe static public bool DDL_PARSE_INTEGER(byte** C, Int64* , Int64* V)
        {
            if ((**(C) & 0x80) != 0)
            {
                int rc = Common.ddl_parse_integer_func(C, (uint*), (UInt64*)V);
                if (rc != 0)
                    return true; /*returns bool in sdc not::>rc;/
            }
            else
            {
                if ((V) != null)
                {
                    *(V) = **(C);
                }
                ++(*(C));
                --(*());
            }
            return false;
        }
        */
        public static unsafe int DDL_PARSE_TAG(byte** C, uint* S, uint* T, uint* L)
        {
            if ((**(C) & 0x80) != 0 || ((**(C) & 0x7f) == 127))
            {
                int rc = ddl_parse_tag_func((C), (S), (T), (L));
                if (rc != DDL_SUCCESS)
                    return DDL_ERROR_END;
            }
            else
            {
                if ((L) != null)
                {
                    *L = 0;
                }
                if ((T) != null)
                {
                    *(T) = **(C);
                }
                //()? * = 0 : 0;	
                //(T)?* (T) = ** (C) : 0;	
                ++(*(C));
                --(*(S));
            }
            return DDL_SUCCESS;
        }

        public static int parse_attr_definition(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            pAttr.pVals = new VALUES();
            //(void)memset((char*)&pAttr.pVals.defData, 0, sizeof(DEFINITION));//ok

            return ddl_parse_definition(ref binChunk, size, ref pAttr.pVals.defData, uiOffset);
        }

        public static void add_optionList(ref ACTION_UI_DATA aud, string pListString)
        {
            string strList;
            int c, i, inLen = pListString.Length;

            if (pListString == null)
                return;

            if (inLen > 0)
            {
                strList = pListString;

                for (c = 0, i = 0; i < inLen; i++)
                {
                    if ((strList[i] == ';') || (strList[i] != ';' && strList[i + 1] == 0))
                    {
                        c++;
                    }
                }

                // CW fix begin 30nov11, email of 11/24/11
                aud.userInterfaceDataType = UI_DATA_TYPE.COMBO;
                // CW fix end
                aud.ComboBox.pchComboElementText = strList;
                aud.ComboBox.iNumberOfComboElements = c;

            }
            else
            {   //When no string is passed to combo box, make it blank by allocating 0ne byte
                aud.ComboBox.pchComboElementText = null;
                aud.ComboBox.iNumberOfComboElements = 0;
            }
        }

        public static void add_textMsg(ref ACTION_UI_DATA aud, string pMsg)
        {
            /*int len = pMsg.Length;
            
            if (aud.textMessage.pchTextMessage != null)
            {
                aud.textMessage.pchTextMessage = null;
            }
            aud.textMessage.iTextMessageLength = 0;
            */
            //	aud.userInterfaceDataType = TEXT_MESSAGE;// must be done in caller
            if (pMsg.Length > 0)
            {
                aud.textMessage.pchTextMessage = pMsg;
                aud.textMessage.iTextMessageLength = pMsg.Length;
            }
            else
            {
                aud.textMessage.pchTextMessage = null;
                aud.textMessage.iTextMessageLength = 0;
            }
        }
        public static int ddl_parse_definition(ref byte[] chunk, uint size, ref DEFINITION def, uint uiOffset)
        {

            //ASSERT_RET(chunk && size, DDL_INVALID_PARAM);

            def.size = size;

            /*def.data = new char[size];

            if (!def.data)
            {
                return DDL_MEMORY_ERROR;
            }
            ushort length = strlen((char*)chunk) + 1;
            if (def.size < length)
            {
                return FAILURE;
            }
            (void)strcpy((char*)def.data, (char*)chunk);*/

            byte[] data = new byte[def.size];

            for (uint i = 0; i < def.size; i++)
            {
                data[i] = chunk[uiOffset + i];
            }

            def.data = Encoding.Default.GetString(data);//Convert.ToString(data);//??????

            return DDL_SUCCESS;


        }/* End ddl_parse_definition*/


        public static unsafe int ddl_parse_tag_func(byte** chunkp, uint* size, uint* tagp, uint* lenp)
        {
            //ADDED By Deepak Initialize all vars
            int lenflag = 0;/* indicates implicit/explicit binary length */
            byte* chunk = null;    /* temp ptr to the binary chunk */
            uint cnt = 0;  /* temp ptr to the size of the binary chunk */
            byte c = 0;        /* current value of the char pointed at by chunk */
            //int rc = 0;     /* return code */
            uint tag = 0;  /* temp storage of parsed tag */
            uint length = 0;   /* temp storage of length of binary assoc with tag */
            UInt64 LL;

            //ASSERT_DBG(chunkp && *chunkp && size);

            chunk = *chunkp;
            cnt = *size;
            if (cnt == 0)
            {
                return DDL_INSUFFICIENT_OCTETS;
            }

            /*
            * Read the first character, and determine if there is an explicit
            * length specified (bit 7 == 1)
            */

            c = *chunk++;
            cnt--;
            lenflag = c & 0x80;
            tag = (uint)(c & 0x7f);

            /*
            * If the tag from the first character is <= 126, we are through. If
            * the tag is == 127, we need to build the tag ID from the following
            * characters.
            */

            if (tag == 127)
            {
                DDL_PARSE_INTEGER(&chunk, &cnt, &LL);
                tag = (uint)LL;
                tag += 126;
            }

            if (tagp != null)
            {
                *tagp = tag;
            }

            /*
            * If there is an explicit length, get it.
            */

            if (lenflag == 0)
            {
                length = 0;
            }
            else
            {
                DDL_PARSE_INTEGER(&chunk, &cnt, &LL);
                length = (uint)LL;
            }

            if (length > cnt)
            {
                return DDL_ENCODING_ERROR;
            }

            if (lenp != null)
            {
                *lenp = length;
            }
            *size = cnt;
            *chunkp = chunk;

            return DDL_SUCCESS;


        }/* End ddl_parse_tag_func */

        public static unsafe int parse_attr_meth_scope(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = 0;
            fixed (byte* chu = &binChunk[uiOffset])
            {
                fixed (ulong* pulV = &pAttr.pVals.ullVal)
                {
                    pAttr.pVals = new VALUES();

                    /*Simply parse the integer & return */
                    DDL_PARSE_INTEGER(&chu, &size, pulV);
                }
            }

            return rc;

        }/*End parse_attr_meth_scope*/

        public static unsafe int parse_attr_param(ref METHOD_PARAM pParam, byte** binChunk, ref uint size)
        {
            int rc = SUCCESS;
            uint tag, len;
            UInt64 LL;
            string sName = "";
            //ASSERT_DBG(pParam != null);
            fixed (uint* ipo = &size)
            {
                DDL_PARSE_TAG(binChunk, ipo, &tag, &len);

                if (tag != PARAMETER_TAG)
                    return DDL_ENCODING_ERROR;
                size -= len;// assume we will handle the whole thing

                //pAttr.pVals = new VALUES;
                // encoded integer var type

                DDL_PARSE_INTEGER(binChunk, &len, &LL);
                pParam.param_type = (int)LL;
                // type modifiers  bitstring
                rc = ddl_parse_bitstring(binChunk, &len, ref (pParam.param_modifiers));
                if (rc != SUCCESS)
                    return rc;
                //int (string& retStr, byte* binChunk, uint& size)
                if (len == 0)
                {
                    //LOGIT(CERR_LOG, "Param parsed with no name.\n");
                    //pParam.param_name = null;
                    return rc;
                }
                else
                {
                    rc = parse_ascii_string(ref sName, binChunk, ref len);
                    if (rc != SUCCESS)
                        return rc;
                    //pParam.param_name = new char[sName.length() + 1];
                    //strcpy(pParam.param_name, sName.c_str());
                    pParam.param_name = sName;
                }
                // exit
            }
            return rc;
        } /* end parse_attr_param */

        public static unsafe int parse_attr_param_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            uint tag, len;
            METHOD_PARAM wrkParam = new METHOD_PARAM();

            //ASSERT_DBG(pAttr != null);
            fixed (byte* chu = &binChunk[uiOffset])
            {

                // explicit tag PARAMETER_SEQLIST_TAG
                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (tag != PARAMETER_SEQLIST_TAG || len == 0)
                    return DDL_ENCODING_ERROR;

                size -= len;// assume we will handle the whole thing	
                //pAttr.pVals = new VALUES();
                //pAttr.pVals.paramList = new METHOD_PARAM_LIST;

                while (len != 0)
                {
                    rc = Common.parse_attr_param(ref wrkParam, &chu, ref len);
                    if (rc != SUCCESS)
                        return rc;
                    //else
                    pAttr.pVals.paramList.Add(wrkParam);
                    //RAZE_ARRAY(wrkParam.param_name);// from dd@fluke
                    ///wrkParam.Clear();
                }
            }
            // exit
            return rc;
        }/* end parse_attr_paramlist */

        public static unsafe int ddl_parse_float(byte** chunkp, uint* size, float* value)
        {
            //ADDED By Deepak initialize all vars
            float temp = 0; /* temporary storage */

            if (*size < (uint)sizeof(float))
            {
                return DDL_INSUFFICIENT_OCTETS;
            }

            /*
            * Move the four bytes into the float, taking care to handle the
            * different byte ordering.
            */

            /* Use LITTLE_ENDIAN byte order! */
            {
                uint i;
                byte* bytep;
                byte* chunk;   /* local pointer to binary */

                chunk = *chunkp;
                bytep = ((byte*)&temp) + (sizeof(float) - 1);
                for (i = 0; i < sizeof(float); ++i)
                {
                    *(bytep--) = *(chunk++);
                }
            }

            *chunkp += sizeof(float);
            *size -= (uint)sizeof(float);

            if (value != null)
            {
                *value = temp;
            }

            return DDL_SUCCESS;


        }/* End ddl_parse_float*/

        public static int app_func_get_dev_spec_string(ref DEV_STRING_INFO dev_str_info, ref ddpSTRING str)
        {

            /*	int             rs; */
            DDlDevDescription.STRING_TBL stringTbl = new DDlDevDescription.STRING_TBL();
            if (DDlDevDescription.pLitStringTable != null)
            {
            }
            /*
             * Find the pointer to the device specific table corresponding to block handle
             */
            /*Vibhor 090804: We have two possibilities:
              1. Device Directory == HART 5 Dev Dir
              2. Device Directory == HART 6 Dev Dir
              Since the design is such that only one of them would be allocated,
            */
            if (true == bTokRev6Flag)
            {
                stringTbl = DDlDevDescription.device_dir_6.string_tbl; //HART 6
            }
            else
            {

                stringTbl = DDlDevDescription.device_dir.string_tbl; //HART 5
            }

            if ((object)stringTbl == null)
            {
                //LOGIT(CERR_LOG, "\napp_func_get_dev_spec_string: Device specific string table not found\n");
                return DDL_DEV_SPEC_STRING_NOT_FOUND;
            }

            if (dev_str_info.id >= (uint)stringTbl.count)
            {   /*
		 * If string not found
		 */

                /*		fprintf(fout,"app_func_get_dev_spec_string: Device specific string of ID %d not found\n",
                            dev_str_info.id); */

                //DEBUGLOG(COUT_LOG, "\napp_func_get_dev_spec_string: Device specific string ID: %d is unusable.\n", dev_str_info.id);

                return DDL_DEV_SPEC_STRING_NOT_FOUND;
            }

            else
            {

                /*
                 * Retrieve the device specific string information.
                 */
                if (str.str == null)// the memory has been transfered
                {
                    if (DDlDevDescription.pLitStringTable == null)
                    {
                        //LOGIT(CERR_LOG, "\napp_func_get_dev_spec_string: Literal string table not found\n");
                        return DDL_DEV_SPEC_STRING_NOT_FOUND;
                    }// else get the string

                    str.flags = DONT_FREE_STRING;
                    str.str = DDlDevDescription.pLitStringTable.get_lit_string(dev_str_info.id);
                    if (str.str == null)
                    {
                        //LOGIT(CERR_LOG | CLOG_LOG, "\napp_func_get_dev_spec_string: Literal string number %d not found\n", dev_str_info.id);
                        return DDL_DEV_SPEC_STRING_NOT_FOUND;
                    }
                    str.len = (ushort)(str.str.Length);
                }
                else
                {
                    str.flags = DONT_FREE_STRING;
                    str.str = stringTbl.list[(int)dev_str_info.id].str;
                    str.len = (ushort)(str.str.Length);
                }
                return DDL_SUCCESS;
            }
        }

        //public static returncode getDevice(string chDbdir, DD_Key_t wrkingKey, ref aCdevice pAbstractDev, ref CDictionary dictionary, bool isInTokenizer) // recurse down the entire hierarchy
        //{
        //    if (dictionary != null)
        //    {
        //        pPIdict = dictionary;
        //    }
        //    else
        //    {
        //        pPIdict = null;
        //        return returncode.eAPP_DICTIONARY_MISSING;
        //    }
        //    // stevev 13mar08 - literal strings have to stay around to allow method string lookup
        //    // they now belong to the device and we are to only fill them in in this routine.
        //    // if they weren't passed in, this HAS TO BE a pre fm8 file load

        //    bool bRetVal;
        //    devDesc = new DDlDevDescription(chDbdir);
        //    bRetVal = devDesc.Initialize(chInputFileName, "|en|", dictionary/*, litStrings*/);
        //    /*Vibhor 291004: Start of Code*/
        //    /* Input file has a defualt extension of fm6 now, so if the initialize failed because there
        //       is no fm6 available, we'll try for a fms*/
        //    /* stevev - for better error reporting */
        //    string erInputFileName = chInputFileName;

        //    if (false == bRetVal)// can't be null... && null != chInputFileName)
        //    {
        //        string tmpFileName = chInputFileName;
        //        /*ushort filelen = strlen(chInputFileName) - 4;
        //        strncpy(erInputFileName, chInputFileName, filelen + 5);*//* stevev-for better reporting */
        //        /*strncpy(tmpFileName, chInputFileName, filelen);
        //        tmpFileName[filelen] = '\0';*/

        //        if (tmpFileName != null)
        //        {
        //            chInputFileName = tmpFileName + ".fms";
        //            bRetVal = devDesc.Initialize(chInputFileName, "|en|", dictionary);
        //        }
        //    }
        //    /*Vibhor 291004: End of Code*/
        //    if (!bRetVal)
        //    {
        //        // 22aug08  OUTPUT(CERR_LOG|UI_LOG,
        //        //LOGIT(CERR_LOG | UI_LOG, "ERROR: DD Initialize failed for '%s'\n  and '%s'", chInputFileName, erInputFileName);
        //        /* VMKP added on 030404*/
        //        //delete devDesc;
        //        /* VMKP added on 030404*/
        //        pPIdict = null;
        //        return returncode.eDDInitErr;
        //    }
        //    else
        //    {
        //        if (!isInTokenizer)
        //        //LOGIT(CERR_LOG | CLOG_LOG | UI_LOG, "Initialized using '%s'\n", chInputFileName);
        //        {

        //        }
        //    }

        //    int iDevItemListSize = devDesc.ItemsList.Count;

        //    //bRetVal = (devDesc.LoadDeviceDescription(isInTokenizer) == returncode.eOk);

        //    iDevItemListSize = devDesc.ItemsList.Count;

        //    if (devDesc.LoadDeviceDescription(isInTokenizer) != returncode.eOk)
        //    {
        //        // 22aug08  OUTPUT(CERR_LOG|UI_LOG,"ERROR: DD Load failed for '%s'",chInputFileName);
        //        //LOGIT(CERR_LOG | UI_LOG, "ERROR: DD Load failed for '%s'\n", chInputFileName);
        //        /* VMKP added on 030404*/
        //        //delete devDesc;
        //        /* VMKP added on 030404*/
        //        pPIdict = null;
        //        return returncode.eDataErr;
        //    }

        //    // stevev 5/11/04 - added for support of file sniffing
        //    /*
        //    WIN32_FIND_DATA FindFileData;
        //    USES_CONVERSION;
        //    CComBSTR combstr(chInputFileName);
        //    LPTSTR szTemp = OLE2T(combstr.m_str);
        //    HANDLE DirSearch =::FindFirstFile(szTemp, &FindFileData);
        //    if (DirSearch != INVALID_HANDLE_VALUE)
        //    {
        //        memcpy(&readFileTime, &(FindFileData.ftLastWriteTime), sizeof(FILETIME));


        //::FindClose(DirSearch);  // HOMZ - MEMORY LEAK :: RESOURCE / KERNEL32
        //    }
        //    */
        //    // end stevev 

        //    //vector<DDlBaseItem*>:: iterator p;
        //    //made global for debugging the catch...		DDlBaseItem *pBaseItem;

        //    /* Got the complete DD file data, Populate On to aCclasses */
        //    pAbstractDev.DDkey = wrkingKey;

        //    //for (p = devDesc.ItemsList.begin(); p != devDesc.ItemsList.end(); p++)
        //    foreach (DDlBaseItem pBaseItem in devDesc.ItemsList)
        //    {
        //        //pBaseItem = *p;

        //        aCitemBase tmpaCItemBase = new aCitemBase();

        //        tmpaCItemBase.itemId = pBaseItem.id;
        //        tmpaCItemBase.itemName = pBaseItem.strItemName;
        //        tmpaCItemBase.itemType = (uint)pBaseItem.byItemType;
        //        tmpaCItemBase.itemSubType = (uint)pBaseItem.byItemSubType;
        //        tmpaCItemBase.attrMask = pBaseItem.ulItemMasks;

        //        //DDlAttribute pAttr = null;
        //        switch (pBaseItem.byItemType)
        //        {
        //            case DDlBaseItem.VARIABLE_ITYPE:
        //                fill_var_item_attributes(tmpaCItemBase, ref ((DDlVariable)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.COMMAND_ITYPE:
        //                fill_command_attributes(tmpaCItemBase, ((DDlCommand*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.MENU_ITYPE:
        //                fill_menu_attributes(tmpaCItemBase, ((DDlMenu*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.EDIT_DISP_ITYPE:
        //                fill_edit_display_attributes(tmpaCItemBase, ((DDlEditDisplay*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.METHOD_ITYPE:
        //                fill_method_attributes(tmpaCItemBase, ((DDlMethod*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.REFRESH_ITYPE:
        //                fill_refresh_attributes(tmpaCItemBase, ((DDlRefresh*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.UNIT_ITYPE:
        //                fill_unit_relation(tmpaCItemBase, ((DDlUnit*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.WAO_ITYPE:
        //                fill_wao_relation(tmpaCItemBase, ((DDlWao*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.ITEM_ARRAY_ITYPE:
        //                fill_item_array_attributes(tmpaCItemBase, ((DDlItemArray*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.COLLECTION_ITYPE:
        //                fill_collection_attributes(tmpaCItemBase, ((DDlCollection*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.BLOCK_ITYPE:
        //            case DDlBaseItem.RECORD_ITYPE:
        //            case DDlBaseItem.PROGRAM_ITYPE:
        //            case DDlBaseItem.VAR_LIST_ITYPE:
        //            case DDlBaseItem.RESP_CODES_ITYPE:
        //            case DDlBaseItem.DOMAIN_ITYPE:
        //            case DDlBaseItem.MEMBER_ITYPE:
        //                //LOGIF(LOGP_MISC_CMTS)(CLOG_LOG, "Unknown item type coming out of JIT parser.\n");
        //                break;// unused
        //            case DDlBaseItem.ARRAY_ITYPE:
        //                fill_array_attributes(tmpaCItemBase, ref pBaseItem.attrList);
        //                break;
        //            case DDlBaseItem.FILE_ITYPE:
        //                fill_file_attributes(tmpaCItemBase, ((DDl6File*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.CHART_ITYPE:
        //                fill_chart_attributes(tmpaCItemBase, ((DDl6Chart*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.GRAPH_ITYPE:
        //                fill_graph_attributes(tmpaCItemBase, ((DDl6Graph*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.AXIS_ITYPE:
        //                fill_axis_attributes(tmpaCItemBase, ((DDl6Axis*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.WAVEFORM_ITYPE:
        //                fill_waveform_attributes(tmpaCItemBase, ((DDl6Waveform*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.SOURCE_ITYPE:
        //                fill_source_attributes(tmpaCItemBase, ((DDl6Source*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.LIST_ITYPE:
        //                fill_list_attributes(tmpaCItemBase, ((DDl6List*)pBaseItem).attrList);
        //                break;
        //            /* stevev 1apr05 - added new items */
        //            case DDlBaseItem.GRID_ITYPE:
        //                fill_grid_attributes(tmpaCItemBase, ((DDl6Grid*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.IMAGE_ITYPE:
        //                fill_image_attributes(tmpaCItemBase, ((DDl6Image*)pBaseItem).attrList);
        //                break;
        //            case DDlBaseItem.BLOB_ITYPE:
        //                //fill_blob_attributes(tmpaCItemBase, ((DDl6Blob*)pBaseItem).attrList);
        //                break;
        //            /* april fools */
        //            default:
        //                break;

        //        }/*End switch*/

        //        pAbstractDev.AitemPtrList.Add(tmpaCItemBase);
        //    }

        //    //for(q = devDesc.CriticalParamList.begin(); q != devDesc.CriticalParamList.end();q++)
        //    foreach (uint q in devDesc.CriticalParamList)
        //    {
        //        pAbstractDev.CritItemList.Add(q);
        //    }

        //    /* stevev 07Jan05 - add image transport */
        //    // NOTE: the memory passed WILL BE RETURNED
        //    //       the host is expected to read the data like a file, use the info and 
        //    //		 NOT delete the memory pointed to.
        //    BIimageList_it iK;
        //    BIframeList_it iF;

        //    imgframe_t ifi;
        //    AframeList_t ifL;

        //    for (iK = devDesc.ImagesList.begin(); iK != devDesc.ImagesList.end(); iK++)
        //    {
        //        for (iF = iK.begin(); iF < iK.end(); ++iF)
        //        {// iF isa ptr2a IMAGEFRAME_S
        //         //memcpy(&ifi,&(IMAGEFRAME_S *)iF, sizeof(imgframe_t));
        //            ifi.size = iF.ifs_size;
        //            ifi.pRawFrame = iF.ifs_pRawFrame;
        //            ifi.offset = iF.ifs_offset;
        //            memcpy(&(ifi.language[0]), &(iF.ifs_language[0]), 6);

        //            ifL.push_back(ifi);
        //        }
        //        pAbstractDev.AimageList.push_back(ifL);
        //        ifL.clear();
        //    }

        //    /* stevev end image transport addition */


        //    pPIdict = null;
        //    return Common.SUCCESS;
        //}

        //public static void fill_var_item_attributes(ref aCitemBase tmpaCitemBase, ref List<DDlAttribute> ddlList)
        //{

        //    ItemAttrList::iterator p;
        //    DDlAttribute* pAttr = null;

        //    for (p = ddlList.begin(); p != ddlList.end(); p++)
        //    {
        //        pAttr = (*p);
        //        aCattrBase* paAttr = null;

        //        /* VMKP added on 291203 */
        //        if ((pAttr.bIsAttributeConditional == TRUE) ||
        //            (pAttr.bIsAttributeConditionalList == TRUE))
        //        { tmpaCitemBase.isConditional = TRUE; }
        //        // next two lines are unique for var items and definitely a bug [5/5/2014 timj]
        //        //else
        //        //{	tmpaCitemBase.isConditional = FALSE; }
        //        /* VMKP added on 291203 */

        //        switch (pAttr.byAttrID)
        //        {

        //            case VAR_CLASS_ID:
        //                paAttr = fill_bitstring_attribute(pAttr, VAR_CLASS);
        //                break;

        //            case VAR_HANDLING_ID:
        //                paAttr = fill_bitstring_attribute(pAttr, VAR_HANDLING);
        //                break;

        //            case VAR_UNIT_ID:
        //                paAttr = fill_string_attribute(pAttr, VAR_UNIT);
        //                break;

        //            case VAR_LABEL_ID:
        //                paAttr = fill_string_attribute(pAttr, VAR_LABEL);
        //                break;

        //            case VAR_HELP_ID:
        //                paAttr = fill_string_attribute(pAttr, VAR_HELP);
        //                break;
        //            /* removed 15oct12
        //                        case	VAR_WIDTHSIZE_ID://	use aCattrCondLong	- as enum
        //                            paAttr =  fill_ulong_attribute(pAttr, VAR_WIDTH);
        //                            break;

        //                        case	VAR_HEIGHTSIZE_ID://	use aCattrCondLong	- as enum
        //                            paAttr =  fill_ulong_attribute(pAttr, VAR_HEIGHT);
        //                            break;
        //            ****/
        //            case VAR_WIDTHSIZE_ID:
        //                paAttr = fill_ulong_attribute(pAttr, VAR_WIDTH);
        //                break;

        //            case VAR_HEIGHTSIZE_ID:
        //                paAttr = fill_ulong_attribute(pAttr, VAR_HEIGHT);
        //                break;

        //            case VAR_VALID_ID:
        //                paAttr = fill_ulong_attribute(pAttr, VAR_VALID);
        //                break;

        //            case VAR_PRE_READ_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_PRE_READ_ACT);
        //                break;

        //            case VAR_POST_READ_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_POST_READ_ACT);
        //                break;

        //            case VAR_PRE_WRITE_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_PRE_WRITE_ACT);
        //                break;

        //            case VAR_POST_WRITE_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_POST_WRITE_ACT);
        //                break;

        //            case VAR_PRE_EDIT_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_PRE_EDIT_ACT);
        //                break;

        //            case VAR_POST_EDIT_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_POST_EDIT_ACT);
        //                break;

        //            case VAR_REFRESH_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_REFRESH_ACT);
        //                break;

        //            case VAR_DEBUG_ID:
        //                paAttr = fill_debug_attribute(pAttr, VAR_DEBUG);
        //                break;

        //            case VAR_POST_RQST_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_POST_RQST_ACT);
        //                break;

        //            case VAR_POST_USER_ACT_ID:
        //                paAttr = fill_actions_attribute(pAttr, VAR_POST_USER_ACT);
        //                break;

        //            case VAR_TYPE_SIZE_ID:
        //                {
        //                    paAttr = new aCattrTypeType;
        //                    tmpaCitemBase.itemSize = pAttr.pVals.typeSize.size;
        //                    ((aCattrTypeType*)paAttr).notCondTypeType.actualVarSize = pAttr.pVals.typeSize.size;
        //                    ((aCattrTypeType*)paAttr).notCondTypeType.actualVarType = pAttr.pVals.typeSize.type;
        //                    paAttr.attr_mask = VAR_TYPE_SIZE;
        //                }
        //                break;

        //            case VAR_DISPLAY_ID:
        //                paAttr = fill_string_attribute(pAttr, VAR_DISPLAY);
        //                break;

        //            case VAR_EDIT_ID:
        //                paAttr = fill_string_attribute(pAttr, VAR_EDIT);
        //                break;

        //            case VAR_MIN_VAL_ID:
        //                paAttr = fill_minmaxlist_attribute(pAttr, VAR_MIN_VAL);
        //                break;

        //            case VAR_MAX_VAL_ID:
        //                paAttr = fill_minmaxlist_attribute(pAttr, VAR_MAX_VAL);
        //                break;

        //            case VAR_SCALE_ID:
        //                paAttr = fill_expression_attribute(pAttr, VAR_SCALE);
        //                break;

        //            case VAR_ENUMS_ID:
        //                {
        //                    paAttr = new aCattrEnum;
        //                    ENUM_VALUE_LIST* enmList;

        //                    if (pAttr.bIsAttributeConditional == FALSE &&
        //                        pAttr.bIsAttributeConditionalList == FALSE)
        //                    {
        //                        enmList = pAttr.pVals.enmList;

        //                        ((aCattrEnum*)paAttr).condEnumListOlists.genericlist.clear();

        //                        aCgenericConditional tmpaCgenCond;
        //                        tmpaCgenCond.priExprType = (expressionType_e)eT_Direct;
        //                        tmpaCgenCond.destElements.clear();

        //                        aCgenericConditional::aCexpressDest tempaCDest;
        //                        tempaCDest.destType = eT_Direct;
        //                        tempaCDest.pPayload = new aCenumList;

        //                        fill_enumlist((aCenumList*)tempaCDest.pPayload, enmList);

        //                        tmpaCgenCond.destElements.push_back(tempaCDest);
        //                        ((aCattrEnum*)paAttr).condEnumListOlists.genericlist.push_back(tmpaCgenCond);
        //                        RAZE(tempaCDest.pPayload);// stevev 11sep09 - stop a major memory leak

        //                    }
        //                    else
        //                    if (pAttr.bIsAttributeConditional == TRUE &&
        //                        pAttr.bIsAttributeConditionalList == FALSE)
        //                    {
        //                        fill_conditional_attributes_list(&(((aCattrEnum*)paAttr).condEnumListOlists), pAttr.pCond);
        //                    }
        //                    else
        //                    if (pAttr.bIsAttributeConditional == FALSE &&
        //                        pAttr.bIsAttributeConditionalList == TRUE)
        //                    {
        //                        fill_conditional_chunks(&(((aCattrEnum*)paAttr).condEnumListOlists), pAttr);
        //                    }
        //                    else // TRUE TRUE
        //                    {
        //                        cerr << "ERROR:enumList flagged as both not-List and List." << endl;
        //                    }
        //                    // else - both - an error

        //                    paAttr.attr_mask = VAR_ENUMS;
        //                }
        //                break;
        //            case VAR_INDEX_ITEM_ARRAY_ID:
        //                paAttr = fill_reference_attribute(pAttr, VAR_INDEX_ITEM_ARRAY);
        //                break;

        //            case VAR_DEFAULT_VALUE_ID:
        //                paAttr = fill_expression_attribute(pAttr, VAR_DEFAULT_VALUE);
        //                break;
        //            /* stevev 30may08 added time attributes */
        //            case VAR_TIME_FORMAT_ID:
        //                paAttr = fill_string_attribute(pAttr, VAR_TIME_FORMAT);
        //                break;

        //            case VAR_TIME_SCALE_ID:
        //                paAttr = fill_bitstring_attribute(pAttr, VAR_TIME_SCALE);
        //                break;

        //            default:
        //                /*Should Never Reach here!!!!*/
        //                break;

        //        }

        //        tmpaCitemBase.attrLst.Add(paAttr);
        //    }/*End for p*/
        //}/*End fill_var_item_attributes */

        public unsafe static uint ddl_resolve_param_reference(uint paramName, ref ushort usType)
        {

            uint ulResolvedID = 0;

            //ASSERT_DBG(pBlock);

            if (pBlock.attrList.Count == 0)
                return ulResolvedID;

            DDlAttribute pAttr;

            foreach (DDlAttribute dl in pBlock.attrList)
            {
                if (dl.byAttrID == DDlBlock.BLOCK_PARAM_ID)
                {
                    pAttr = dl;
                    /*Now iterate thru the memberlist & find the value*/

                    /*Just return if Member List is empty*/

                    if (pAttr.pVals.memberList.Count == 0)
                        break;

                    MEMBER member;

                    foreach (MEMBER it in pAttr.pVals.memberList)
                    {
                        member = it;
                        if (member.name == paramName)
                        {
                            /*Assumption: This reference should be a direct reference
                            of type ID, coz here we can't handle a via Reference */
                            //??????for (ushort i = 0; i < member.item.Count; i++)
                            int i = 0;
                            {
                                switch (member.item[i].type)
                                {
                                    case ddpREF.ITEM_ID_REF:
                                    case ddpREF.ITEM_ARRAY_ID_REF:
                                    case ddpREF.COLLECTION_ID_REF:
                                    case ddpREF.BLOCK_ID_REF:
                                    case ddpREF.VARIABLE_ID_REF:
                                    case ddpREF.MENU_ID_REF:
                                    case ddpREF.EDIT_DISP_ID_REF:
                                    case ddpREF.METHOD_ID_REF:
                                    case ddpREF.REFRESH_ID_REF:
                                    case ddpREF.UNIT_ID_REF:
                                    case ddpREF.WAO_ID_REF:
                                    case ddpREF.RECORD_ID_REF:
                                        {
                                            ulResolvedID = member.item[i].id;
                                            usType = member.item[i].type;
                                        }
                                        break;
                                    default:
                                        /*We can't handle others here
                                         & ideally it shouldn't come here either!!!!*/
                                        /* Log this one!!!!*/
                                        break;

                                }/*End switch*/

                                //break; /*for i*/
                            }/*End for i*/
                            /*We are done !!! Just break out of this loop & return*/

                            break; /*for it*/

                        }/*Endif member.name*/

                    }/*End for it*/

                    break; /*for iterator p*/
                }/*End if (*p).byAttrID*/


            }/*End for p*/

            return ulResolvedID;
        }/*End ddl_resolve_param_reference*/

        public static unsafe int parse_attr_wavefrm_type(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            fixed (byte* chu = &binChunk[uiOffset])
            {
                uint tag, len;

                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (tag != WAVE_TYPE_TAG)
                {
                    return DDL_ENCODING_ERROR;
                }

                /*Now its just an integer */
                pAttr.pVals = new VALUES();

                /*Simply parse the integer & return */
                UInt64 ulTemp;
                DDL_PARSE_INTEGER(&chu, &size, &ulTemp);
                pAttr.pVals.ullVal = ulTemp;
            }
            return rc;

        }/*End parse_attr_wavefrm_type*/

        public static unsafe int parse_attr_line_type(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;
            fixed (byte* chu = &binChunk[uiOffset])
            {

                byte** chunkp = &chu;
                uint* length = &size;
                uint tag, tagp, len;

                ddpExpression tempExpr = new ddpExpression();

                LINE_TYPE tmpLineType = new LINE_TYPE();

                /*Parse the Tag to know if we have a conditional or a direct object*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                switch (tag)
                {

                    case IF_TAG: /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                            /*Now Parse the ddpExpression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, length, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                pAttr.pCond.byNumberOfSections++;

                            }

                        }
                        break; /*End IF_TAG*/

                    case SELECT_TAG: /*We have a Switch Case conditional*/
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, length, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (*length > 0)
                            {
                                DDL_PARSE_TAG(chunkp, length, &tagp, &len);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, length, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;

                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT///
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/

                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/


                        }
                        break; /*End SELECT_TAG*/

                    case OBJECT_TAG: /*We have a direct object*/
                        {

                            rc = ddl_parse_linetype(chunkp, length, ref tmpLineType);

                            if (rc != DDL_SUCCESS)
                                return rc;

                            pAttr.pVals = new VALUES();

                            pAttr.pVals.lineType = tmpLineType;

                        }
                        break; /*End OBJECT_TAG*/
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch tag*/
            }
            return SUCCESS;

        }/*End parse_attr_line_type*/

        public static unsafe int ddl_parse_ref(byte** chunkp, uint* size, ref ddpREFERENCE dstring)
        {
            int rc = 0; /*return code*/

            uint tag = 0, len = 0;/*tag & length of the parse binary*/
            //	ddpREF			tmpRef;/* temporary reference structure*/
            bool parse = true;
            UInt64 LL;

            //	ddpExpression tempExpr; /*temporary expression to parse the item_array index*/

            //ASSERT_DBG(chunkp && *chunkp && size);


            while (parse)
            {
                /*Parse the tag to find out the type of reference*/
                ddpREF tmpRef = new ddpREF();// was (void)memset((char*)&tmpRef,0,sizeof(ddpREF));

                DDL_PARSE_TAG(chunkp, size, &tag, &len);
                tmpRef.type = (ushort)tag;

                switch (tag)
                {
                    case ddpREF.ITEM_ID_REF:
                    case ddpREF.ITEM_ARRAY_ID_REF:
                    case ddpREF.COLLECTION_ID_REF:
                    case ddpREF.BLOCK_ID_REF:
                    case ddpREF.VARIABLE_ID_REF:
                    case ddpREF.MENU_ID_REF:
                    case ddpREF.EDIT_DISP_ID_REF:
                    case ddpREF.METHOD_ID_REF:
                    case ddpREF.REFRESH_ID_REF:
                    case ddpREF.UNIT_ID_REF:
                    case ddpREF.WAO_ID_REF:
                    case ddpREF.RECORD_ID_REF:
                    case ddpREF.ARRAY_ID_REF:
                    /*case VAR_LIST_ID_REF	
                      case PROGRAM_ID_REF	
                      case DOMAIN_ID_REF
                    */
                    case ddpREF.RESP_CODES_ID_REF:
                    case ddpREF.FILE_ID_REF:
                    case ddpREF.CHART_ID_REF:
                    case ddpREF.GRAPH_ID_REF:
                    case ddpREF.AXIS_ID_REF:
                    case ddpREF.WAVEFORM_ID_REF:
                    case ddpREF.SOURCE_ID_REF:
                    case ddpREF.LIST_ID_REF:
                    /* stevev 23mar05 - these are new items */
                    case ddpREF.IMAGE_ID_REF:
                    case ddpREF.GRID_ID_REF:
                    /* end stevev 23mar05 */
                    /* stevev 06feb13 - these are new items */
                    case ddpREF.BLOB_ID_REF:
                    case ddpREF.TEMPLATE_ID_REF:
                    case ddpREF.PLUGIN_ID_REF:
                        /* end stevev  06feb13*/

                        {
                            if (tag == ddpREF.RECORD_ID_REF)
                            {
                                tmpRef.type = ddpREF.COLLECTION_ID_REF;
                            }
                            // else leave it tag

                            /*Parse the ID*/
                            DDL_PARSE_INTEGER(chunkp, size, &LL);
                            tmpRef.id = (uint)LL;

                            if (dstring.Count != 0)
                            {
                                /*It comes here if we have a via.... reference in the 
                                 parent*/
                                if (tag == ddpREF.ITEM_ID_REF)
                                {
                                    if (dstring[dstring.Count - 1].type == ddpREF.VIA_COLLECTION_REF
                                     || dstring[dstring.Count - 1].type == ddpREF.VIA_RECORD_REF)
                                    {
                                        tmpRef.type = ddpREF.COLLECTION_ID_REF;
                                    }

                                    else if (dstring[dstring.Count - 1].type == ddpREF.VIA_ITEM_ARRAY_REF)
                                    {
                                        tmpRef.type = ddpREF.ITEM_ARRAY_ID_REF;
                                    }
                                    /*	else if(ref.at(ref.Count-1).type == VIA_RECORD_REF)
                                        {
                                            tmpRef.type = RECORD_ID_REF;
                                        } */
                                    else if (dstring[dstring.Count - 1].type == ddpREF.VIA_FILE_REF)
                                    {
                                        tmpRef.type = ddpREF.FILE_ID_REF;
                                    }

                                    else if (dstring[dstring.Count - 1].type == ddpREF.VIA_LIST_REF)
                                    {
                                        tmpRef.type = ddpREF.LIST_ID_REF;
                                    }

                                    else if (dstring[dstring.Count - 1].type == ddpREF.VIA_BITENUM_REF)
                                    {
                                        tmpRef.type = ddpREF.VARIABLE_ID_REF;
                                    }   // else - leave as is
                                }/*Endif tag*/

                            }/*Endif ref.Count> 0*/

                            //ref.Add(tmpRef);
                            dstring.Add(tmpRef);

                            parse = false;
                        }
                        break;

                    //			case IMAGE_ID_REF:/* if it walks like a duck ...  */ 
                    //							//handle as a expression  130904//do not handle as expression 7jan05
                    //				{
                    //					// get the integer
                    //					DDL_PARSE_INTEGER(chunkp,size,&(tmpRef.val.id));
                    //					// insert it as an expression in the reference
                    //					Expression* pE   =  new Expression;
                    //					Element		exprElem;
                    //					exprElem.byElemType   = INTCST_OPCODE;
                    //					exprElem.elem.ulConst = tmpRef.val.id; 
                    //
                    //					pE.Add(exprElem);
                    //					tmpRef.val.index = pE;	// replaces the id
                    //					// clear the reference id
                    //					// this will null the pointer...tmpRef.val.id = 0;
                    //					// save it					
                    //					ref.Add(tmpRef);			
                    //					
                    //					parse = false;
                    //				}
                    //				break;
                    case ddpREF.VIA_ARRAY_REF:
                    case ddpREF.VIA_ITEM_ARRAY_REF:
                    case ddpREF.VIA_LIST_REF:
                        {
                            tmpRef.index = new ddpExpression();
                            /*Parse the expression which follows, for a constant integer*/

                            rc = ddl_parse_expression(chunkp, size, ref tmpRef.index);
                            /*tmpRef.val.index = tempExpr.at(0).elem.ulConst; */

                            dstring.Add(tmpRef);
                        }
                        break;

                    case ddpREF.VIA_VAR_LIST_REF:
                    case ddpREF.VIA_COLLECTION_REF:
                    case ddpREF.VIA_RECORD_REF:
                    case ddpREF.VIA_FILE_REF:
                    case ddpREF.VIA_BITENUM_REF:
                    /* stevev 06may05 */
                    case ddpREF.VIA_CHART_REF:
                    case ddpREF.VIA_GRAPH_REF:
                    case ddpREF.VIA_SOURCE_REF:
                    case ddpREF.VIA_ATTR_REF:
                        /* end 06may06 */
                        /*case VIA_BLOB_REF: 06feb13 we may have to handle this later */
                        {
                            if (tag == ddpREF.VIA_RECORD_REF)
                                tmpRef.type = ddpREF.VIA_COLLECTION_REF;// translate munge

                            /*Parse the membername of the collection / record / varlist*/
                            DDL_PARSE_INTEGER(chunkp, size, &LL);
                            tmpRef.member = (uint)LL; //For VIA_BITENUM its the BitMask

                            dstring.Add(tmpRef);
                        }
                        break;
                    case ddpREF.VIA_PARAM_REF:
                    case ddpREF.VIA_PARAM_LIST_REF:
                        {
                            uint ulRefID;/*ID of the item referred by the member*/
                            ushort usRefType = 0; /*Type of the item*/
                            uint ulParamName;

                            /*Parse the name of param / param list*/

                            DDL_PARSE_INTEGER(chunkp, size, &LL);
                            tmpRef.member = (uint)LL;

                            ulParamName = tmpRef.member;
                            /*Now resolve the parameter reference from the HART block*/

                            ulRefID = ddl_resolve_param_reference(ulParamName, ref usRefType);

                            if (ulRefID == 0)
                            {/*It should not come here */

                                tmpRef.type = (ushort)tag;

                                dstring.Add(tmpRef);

                                parse = false;

                                break;
                            }
                            /*Else we have a resolved value & type */

                            tmpRef.type = usRefType;

                            tmpRef.id = ulRefID;

                            dstring.Add(tmpRef);

                            parse = false;

                        }
                        break;
                    case ddpREF.VIA_BLOCK_REF:
                        {
                            tmpRef.type = (ushort)tag;
                            /*Parse the name of the characteristic record*/

                            DDL_PARSE_INTEGER(chunkp, size, &LL);
                            tmpRef.id = (uint)LL;

                            dstring.Add(tmpRef);

                            parse = false;

                        }
                        break;
                    /* stevev 23mar05: try...*/
                    case ddpREF.ROWBREAK_REF:
                    case ddpREF.SEPARATOR_REF:
                        {
                            tmpRef.index = null; //Just defensive- val is a UNION, clear before setting!!!
                            tmpRef.id = tmpRef.member = 0;
                            dstring.Add(tmpRef);

                            parse = false;
                        }
                        break;
                    //case IMAGE_REF:
                    case ddpREF.CONSTANT_REF:
                        {
                            tmpRef.index = new ddpExpression();
                            //DDL_PARSE_TAG(chunkp, size, &tag, &length);
                            /*Parse the expression which follows, for a constant  */
                            rc = ddl_parse_expression(chunkp, size, ref tmpRef.index);

                            dstring.Add(tmpRef);

                            parse = false;
                        }
                        break;

                    default:
                        return DDL_ENCODING_ERROR;
                        //break;

                }/*End switch tag*/

            }/*End while parse*/

            return DDL_SUCCESS;

        }/*End ddl_parse_ref*/

        public unsafe static int ddl_parse_string(byte** chunkp, uint* size, ref ddpSTRING dstring)
        {
            //ADDED By Deepak initialize all  vars
            int rc = 0;             /* return code */
            DEV_STRING_INFO dev_str_info;   /* holds device specific string info */
            uint tag = 0;          /* temporary tag */
            uint temp = 0;     /* temporary storage for casting conversion */
            uint tag_len = 0;  /* length of binary assoc. w/ parsed tag */
            //uint enum_val = 0;     /* used for enumeration strings */
            UInt64 LL;

            /*Vibhor 201003: Adding following variables for string translation */

            //string pchTranslatedString = null;


            //ASSERT_DBG(chunkp && *chunkp && size);

            /*
            * initialize string struct
            */

            if (dstring != null)
            {
                dstring.flags = DONT_FREE_STRING;
                dstring.len = 0;
                dstring.str = null;
                dstring.strType = DICTIONARY_STRING_TAG; /*By default we will put a string to be of 
												  type DICTIONARY ddpSTRING*/
            }


            /*
            * Parse the tag to find out what kind of string it is.
            */

            DDL_PARSE_TAG(chunkp, size, &tag, &tag_len);

            switch (tag)
            {
                case DEV_SPEC_STRING_TAG:

                    /*
                    * device specific string (ie. string number). Parse the file
                    * information and string number.
                        */

                    if (dstring != null)
                    {
                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        dev_str_info.mfg = (uint)LL;

                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        dev_str_info.dev_type = (ushort)LL;

                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        dev_str_info.rev = (byte)LL;

                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        dev_str_info.ddrev = (byte)LL;

                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        dev_str_info.id = (uint)LL;

                        uint ddKey = (dev_str_info.mfg & 0xff) << 24;
                        ddKey += (uint)(dev_str_info.dev_type & 0xff) << 16;
                        ddKey += (uint)(dev_str_info.rev & 0xff) << 8;
                        ddKey += (uint)(dev_str_info.ddrev & 0xff);

                        rc = app_func_get_dev_spec_string(ref dev_str_info, ref dstring);

                        /*
                        * If a string was not found, get the default error string.
                        */

                        if (rc != DDL_SUCCESS)
                        {
                            //LOGIT(CERR_LOG, "Device specific string 0x%04x for device DDkey 0x%08x was not aquired.\n", temp, ddKey);
                            rc = DDlDevDescription.pGlobalDict.get_dictionary_string(DEFAULT_DEV_SPEC_STRING, ref dstring);
                            if (rc != DDL_SUCCESS)
                            {
                                //LOGIT(CERR_LOG, "Default device specific string was not aquired.\n");
                                return rc;
                            }
                        }
                    }

                    /* I donno when do we need to parse the string if we don't want to store it*/
                    else
                    {
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                    }

                    /*	timj 8jan08 - all languages now retained in the string, translation deferred to 
                                        copy_ddlstring in parserInfc.cpp

                            //Translate the string to the desired language as specified by the global langCode
                            //Vibhor 020104: If the DD has a null string ie "" then we will store a string which is
                            //nothing but null string ... 
                            if(dstring.len > 0)
                            {
                                pchTranslatedString = new char[dstring.len];

                                 //This call will translate the string with into the language with which the 
                                 //DDlDevdescription Class was initialized
                                rc = pGlobalDict.get_string_translation(dstring.str,pchTranslatedString,dstring.len);
                                if(rc == SUCCESS)
                                {//We have a translated string!!
                                    //dstring.str is holding a memory from Dictionary, we will allocate new memory to it
                                    //to store the translated string
                                    dstring.str = null;
                                    if(strlen(pchTranslatedString) > 0)
                                    {
                                        dstring.str = new char[strlen(pchTranslatedString)+1];
                                        strcpy(dstring.str,pchTranslatedString);
                                        dstring.len = strlen(dstring.str);
                                        dstring.flags = FREE_STRING;// stevev 7aug7
                                    }
                                    else
                                    {	
                                        dstring.str = new char[2];
                                        strcpy(dstring.str,"");
                                        dstring.len = 0;
                                        dstring.flags = FREE_STRING | ISEMPTYSTRING;// stevev 14nov05 set a flag for empty

                                    }
                                }
                                //free the memory allocated to the input & translated string 

                                delete [] pchTranslatedString; //Vibhor 120504 : changed to delete []

                                pchTranslatedString = null;
                            }
                            else
                     end timj */

                    if (dstring.len == 0)
                    {
                        dstring.str = "";
                        dstring.len = 0;
                        dstring.flags = FREE_STRING | ISEMPTYSTRING;// stevev 14nov05 set a flag for empty
                    }

                    dstring.strType = tag;
                    break;

                case VARIABLE_STRING_TAG:
                    DDL_PARSE_INTEGER(chunkp, size, &LL);
                    dstring.varId = (uint)LL;

                    dstring.strType = tag;
                    break;
                case VAR_REF_STRING_TAG: /*Undocumented: We have to verify if VAR_REF_STRING_TAG is really encountered*/
                    rc = ddl_parse_ref(chunkp, size, ref dstring.varRef);
                    dstring.strType = tag;
                    break;

                case ENUMERATION_STRING_TAG:
                case ENUM_REF_STRING_TAG:

                    /*
                    * enumeration/enumeration_reference string. Parse the
                    * enumeration ID, and the value within the enumeration.
                        */

                    if (dstring != null)
                    {

                        if (tag == ENUMERATION_STRING_TAG)
                        {
                            DDL_PARSE_INTEGER(chunkp, size, &LL);
                            dstring.enumStr.iD = (uint)LL;
                        }
                        else /*ENUM_REF_STRING_TAG*/
                        {
                            dstring.enumStr.reff = new ddpREFERENCE();
                            rc = ddl_parse_ref(chunkp, size, ref dstring.enumStr.reff);
                        }

                        dstring.strType = tag;

                        DDL_PARSE_INTEGER(chunkp, size, &LL);

                        dstring.enumStr.enumValue = (uint)LL;

                    }
                    else
                    {
                        if (tag == ENUMERATION_STRING_TAG)
                        {
                            DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                        }
                        else
                        {
                            /*
                            rc = ddl_parse_uint(chunkp, size, (uint *) null, depinfo,
                            var_needed);
                            if (rc != DDL_SUCCESS) {
                            return rc;
                            }
                                */
                        }
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                    }
                    dstring.strType = tag;
                    break;

                case DICTIONARY_STRING_TAG:

                    /*
                    * dictionary string. Parse the dictionary string number.
                        */

                    if (dstring != null)
                    {
                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        temp = (uint)LL;
                        // stevev 19dec13 - no label/help available is now blank
                        // version 8 dictionary doesn't include blank
                        //if ( temp == 0x1900002 || temp == 0x1900003 )
                        //{
                        //	temp = 0x1260009;
                        //}
                        if (temp == 0x1900002 || temp == 0x1900003)
                        {
                            dstring.str = "";
                            return DDL_SUCCESS;
                        }
                        rc = DDlDevDescription.pGlobalDict.get_dictionary_string(temp, ref dstring);

                        /*
                        * If a string was not found, get the default error string.
                        */

                        if (rc != DDL_SUCCESS)
                        {
                            rc = DDlDevDescription.pGlobalDict.get_dictionary_string(DEFAULT_STD_DICT_STRING, ref dstring);
                            if (rc != DDL_SUCCESS)
                            {
                                return rc;
                            }
                        }
                    }
                    else
                    {
                        DDL_PARSE_INTEGER(chunkp, size, (UInt64*)null);
                    }
                    dstring.strType = tag;
                    break;

                default:

                    /*
                    * Unknown tag. Default this string.
                    */

                    if (dstring != null)
                    {
                        rc = DDlDevDescription.pGlobalDict.get_dictionary_string(DEFAULT_STD_DICT_STRING, ref dstring);
                        if (rc != DDL_SUCCESS)
                        {
                            return rc;
                        }

                    }
                    *chunkp += tag_len;
                    *size -= tag_len;
                    break;
            }

            return DDL_SUCCESS;


        }/*End ddl_parse_string*/

        public unsafe static int ddl_parse_expression(byte** chunkp, uint* size, ref ddpExpression exprList)
        {
            int rc;

            //ADDED By Deepak initialize vars
            uint length = 0, tag = 0, len = 0;
            UInt64 LL;

            //	Element		exprElem;


            //ASSERT_DBG(chunkp && *chunkp && size);


            /*
            * Parse the tag, and make sure it is an EXPRESSION_TAG.
            */

            DDL_PARSE_TAG(chunkp, size, &tag, &length);

            if ((UInt64)EXPRESSION_TAG != tag)
            {
                return DDL_ENCODING_ERROR;
            }

            /*
            * Make sure the length makes sense.
            */

            if ((uint)0 == length || (uint)length > *size)
            {
                return DDL_ENCODING_ERROR;
            }


            *size -= length;


            while (length > 0)
            {

                /*
                * Parse the tag, and switch on the tag type
                    */
                Element exprElem = new Element();

                DDL_PARSE_TAG(chunkp, &length, &tag, &len);// NOTE : len is a placeholder
                //assert(tag < MAXIMUM_UCHAR);

                switch (tag)
                {
                    case INTCST_OPCODE: /* Integer constant opcode*/
                        {
                            UInt64 ullTemp;

                            DDL_PARSE_INTEGER(chunkp, &length, &ullTemp);
                            /* note:  The Tokenizer will insert a Unary NEG opcode for negative constants.
                            So, a -10 will be encoded as 0x0a, NEG_OPCODE, rest of expression...
                            >>>>expression handler: default constant type is int...not ushort.<<<<<<
                            *******/
                            exprElem.byElemType = (byte)tag;
                            exprElem.ulConst = ullTemp;
                        }
                        break;/*INTCST_OPCODE*/

                    case FPCST_OPCODE:
                        {
                            float fTemp;
                            rc = ddl_parse_float(chunkp, &length, &fTemp);
                            exprElem.byElemType = (byte)tag;
                            exprElem.fConst = fTemp;

                        }
                        break;/*FPCST_OPCODE*/
                    case STRCST_OPCODE:
                        {
                            ddpSTRING ddp = new ddpSTRING();
                            //exprElem.pSTRCNST = new ddpSTRING;
                            exprElem.byElemType = (byte)tag;
                            rc = ddl_parse_string(chunkp, &length, ref ddp);
                            exprElem.pSTRCNST.Add(ddp);
                        }
                        break;/*STRCST_OPCODE*/

                    case BLOCK_OPCODE:
                    case BLOCKID_OPCODE:
                    case BLOCKREF_OPCODE:
                    case SYSTEMENUM_OPCODE:
                        {
                            ;//error
                        }
                        break;

                    case VARID_OPCODE:
                    case MAXVAL_OPCODE:
                    case MINVAL_OPCODE:
                    case VARREF_OPCODE:
                    case MAXREF_OPCODE:
                    case MINREF_OPCODE:
                        {
                            uint whichTemp = 0;
                            uint tempID = 0;

                            if ((tag != VARID_OPCODE) && (tag != VARREF_OPCODE))
                            {   /*
					* A min or max value reference.  Parse which
					* value (0, 1, 2, ...),
					*/
                                DDL_PARSE_INTEGER(chunkp, &length, &LL); whichTemp = (uint)LL;
                            }/*Endif VARID_OPCODE......*/


                            switch (tag)
                            {
                                case VARID_OPCODE:
                                case MAXVAL_OPCODE:
                                case MINVAL_OPCODE:
                                    /*
                                     * Parse the variable ID.
                                     */
                                    DDL_PARSE_INTEGER(chunkp, &length, &LL);
                                    tempID = (uint)LL;
                                    if (tag != VARID_OPCODE)
                                    {
                                        exprElem.byElemType = (byte)tag;
                                        //exprElem.minMax.which = whichTemp;
                                        //exprElem.minMax.variable.id = tempID;
                                        //exprElem.minMax = new MIN_MAX(whichTemp, tempID);
                                    }/*End if*/
                                    else
                                    {
                                        exprElem.byElemType = (byte)tag;
                                        exprElem.varId = tempID;

                                    }/*End else*/

                                    break;

                                case VARREF_OPCODE:
                                    exprElem.byElemType = (byte)tag;
                                    /* Parse the reference of the variable */
                                    //exprElem.varRef = new ddpREFERENCE();
                                    rc = ddl_parse_ref(chunkp, &length, ref exprElem.varRef);

                                    if (rc != SUCCESS)
                                        return rc;
                                    break;

                                case MAXREF_OPCODE:
                                case MINREF_OPCODE:
                                    exprElem.byElemType = (byte)tag;

                                    //					exprElem.minMax.which = whichTemp; /*Set the which value */
                                    exprElem.minMax = new MIN_MAX();
                                    exprElem.minMax.which = whichTemp;
                                    //					exprElem.minMax.variable.ref = new ddpREFERENCE;
                                    ddpREFERENCE ddp = new ddpREFERENCE();
                                    //exprElem.minMax.reff = new ddpREFERENCE;
                                    exprElem.minMax.isID = false;
                                    //					rc = ddl_parse_ref(chunkp,&length,exprElem.minMax.variable.ref);
                                    rc = ddl_parse_ref(chunkp, &length, ref ddp);
                                    exprElem.minMax.reff = ddp;/////?????
                                    if (rc != SUCCESS)
                                        return rc;
                                    break;


                                default:    /* this cannot happen */
                                    //							CRASH_RET(DDL_SERVICE_ERROR);
                                    /* NOTREACHED */
                                    break;
                            }/*End switch nested*/

                        }
                        break;/*VARID_OPCODE,MAXVAL_OPCODE,MINVAL_OPCODE,VARREF_OPCODE,MAXREF_OPCODE */

                    /* these are now part of the reference by attribute *
                            case CNTREF_OPCODE:
                            case CAPREF_OPCODE:
                            case FSTREF_OPCODE:
                            case LSTREF_OPCODE:
                                {
                                    exprElem.byElemType = tag;
                                    /x* Parse the reference of the variable *x/
                                    exprElem.varRef = new ddpREFERENCE;
                                    rc = ddl_parse_ref(chunkp,&length,exprElem.varRef);
                                }
                                break;
                    ** end 21apr05 stevev */
                    default:
                        {

                            /* This has to be an opcode, simply push it on the list */
                            exprElem.byElemType = (byte)tag;
                            exprElem.byOpCode = (byte)tag;
                        }
                        break;

                }/* End (Element type) tag*/

                exprList.Add(exprElem);
                //exprElem.clean();
                ///exprElem.Cleanup();

            }/*End while*/

            return DDL_SUCCESS;

        }/* End ddl_parse_expression*/

        public static unsafe int ddl_parse_conditional(ref DDlConditional pConditional, byte** chunkp, uint* size)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint len = 0, lenp = 0; /*Length of the data associated with the Tag*/
            uint tag = 0, tagp = 0;/* The "tag" */
            //	uint temp;

            byte* leave_pointer = null; /* end-of-chunk pointer*/
            //bool bNestedConditional = false;

            VALUES tempVal = new VALUES();

            //ddpExpression tempExpr;

            DDlConditional pChild = null;

            //ASSERT_DBG(chunkp && *chunkp && size);

            /*
             * Parse the tag to find out if this is a SELECT statement, an if/else,
             * or a simple assignment.
             */

            DDL_PARSE_TAG(chunkp, size, &tag, &len);

            /*
             * Adjust size of remaining chunk.
             */

            *size -= len;

            /*
             * Calculate the return chunk pointer (we may be able to use it it later
             * for an early exit).
             */

            leave_pointer = *chunkp + len;

            switch (tag)
            {
                case IF_TAG:
                    {
                        /* If we reach here it means that there's a nested conditional*/
                        //bNestedConditional = true;
                        pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed

                        pConditional.Vals.Add(tempVal); /* Push a null value on the value list*/
                        pChild = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pConditional.attrDataType, 1);

                        /* Now Parse the ddpExpression associated with the IF block */

                        rc = ddl_parse_expression(chunkp, &len, ref pChild.expr);

                        if (rc != SUCCESS)
                            return rc; /* Return if not successful*/

                        /*otherwise Parse the value of the attribute associated with THEN clause*/

                        rc = ddl_parse_conditional(ref pChild, chunkp, &len);

                        if (rc != SUCCESS)
                            return rc; /* Return if not successful*/

                        /*Parse the ELSE portion if there's one*/
                        if (len > 0)
                        {

                            rc = ddl_parse_conditional(ref pChild, chunkp, &len);
                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/
                            pChild.byNumberOfSections++;

                        }

                        /*This has to be the last statement, in this case*/
                        pConditional.listOfChilds.Add(pChild);/*Push the child on the list*/
                    }
                    break; /*End case :IF_TAG*/

                case SELECT_TAG:
                    {

                        //bNestedConditional = true;
                        pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                        /*This guy will come in each case, not in select*/
                        pConditional.Vals.Add(tempVal);  /* Push a null value on the value list*/

                        pChild = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pConditional.attrDataType, 0);

                        /*Now Parse the ddpExpression Argument of the SELECT */

                        rc = ddl_parse_expression(chunkp, &len, ref pChild.expr);

                        if (rc != SUCCESS)
                            return rc;


                        /*otherwise Parse all the CASE branches and the DEFAULT */
                        while (len > 0)
                        {
                            ddpExpression tempExpr = new ddpExpression();

                            DDL_PARSE_TAG(chunkp, &len, &tagp, &lenp);

                            switch (tagp)
                            {
                                case CASE_TAG:
                                    {
                                        /*We are parsing the CASE constants as ddpExpression
                                        just bcoz of the spec. But it should be a constant 
                                        value , ie. an expression with just a  constant (integer)
                                        value*/

                                        rc = ddl_parse_expression(chunkp, &len, ref tempExpr);

                                        if (rc != SUCCESS)
                                            return rc;

                                        pChild.caseVals.Add(tempExpr);

                                        /*We have the case constant value 
                                        Now parse the attributre value from the 
                                        following chunk	*/

                                        rc = ddl_parse_conditional(ref pChild, chunkp, &len);
                                        if (rc != SUCCESS)
                                            return rc; /* Return if not successful*/

                                        pChild.byNumberOfSections++;

                                        ///tempExpr.Clear();
                                    }
                                    break;/*End CASE_TAG*/

                                case DEFAULT_TAG:
                                    {
                                        /*
                                                                    temp = DEFAULT_TAG_VALUE;
                                                                    pChild.caseVals.Add(temp);
                                        */
                                        tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                        pChild.caseVals.Add(tempExpr);


                                        pChild.byNumberOfSections++;

                                        rc = ddl_parse_conditional(ref pChild, chunkp, &len);
                                        if (rc != SUCCESS)
                                            return rc; /* Return if not successful*/
                                    }
                                    break;/*End DEFAULT_TAG*/
                                default:
                                    return DDL_ENCODING_ERROR;

                            }/*End Switch tagp*/
                        }/*End while*/

                        /*This has to be the last statement, in this case*/
                        pConditional.listOfChilds.Add(pChild);/*Push the child on the list*/
                    }
                    break;

                case OBJECT_TAG: /*We have a direct object, just parse it & return!!*/
                    {
                        //bNestedConditional = false;
                        pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);//Vibhor 200105: Changed
                        switch (pConditional.attrDataType)
                        {

                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT:
                                {
                                    Int64 iTemp;

                                    DDL_PARSE_INTEGER(chunkp, &len, (UInt64*)&iTemp);

                                    tempVal.llVal = iTemp;
                                    pConditional.Vals.Add(tempVal);
                                }
                                break;
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNSIGNED_LONG:
                                {
                                    UInt64 ulTemp;

                                    DDL_PARSE_INTEGER(chunkp, &len, &ulTemp);

                                    tempVal.ullVal = ulTemp;
                                    pConditional.Vals.Add(tempVal);

                                }
                                break;
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_FLOAT:
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_DOUBLE:
                                {
                                    float fTemp;

                                    rc = ddl_parse_float(chunkp, &len, &fTemp);
                                    if (DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_FLOAT == pConditional.attrDataType)
                                        tempVal.fVal = fTemp;
                                    else
                                        tempVal.dVal = (double)fTemp;

                                    pConditional.Vals.Add(tempVal);
                                }
                                break;
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_STRING:
                                {
                                    //ddpSTRING strTemp;
                                    tempVal.strVal = new ddpSTRING();
                                    rc = ddl_parse_string(chunkp, &len, ref tempVal.strVal);

                                    pConditional.Vals.Add(tempVal);

                                }
                                break;
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ITEM_ID:
                                {
                                    UInt64 idTemp;
                                    DDL_PARSE_INTEGER(chunkp, &len, &idTemp);

                                    tempVal.id = (uint)idTemp;
                                    pConditional.Vals.Add(tempVal);

                                }
                                break;
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_BITSTRING:
                                {
                                    uint ulTemp = 0;
                                    rc = ddl_parse_bitstring(chunkp, &len, ref ulTemp);
                                    tempVal.ullVal = ulTemp;
                                    pConditional.Vals.Add(tempVal);

                                }
                                break;

                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE:
                                {
                                    tempVal.reff = new ddpREFERENCE();

                                    rc = ddl_parse_ref(chunkp, &len, ref tempVal.reff);

                                    if (rc != DDL_SUCCESS)
                                        return rc;
                                    pConditional.Vals.Add(tempVal);

                                }
                                break;//Vibhor 200105: Added

                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION:
                                {
                                    tempVal.pExpr = new ddpExpression();

                                    rc = ddl_parse_expression(chunkp, &len, ref tempVal.pExpr);
                                    if (rc != DDL_SUCCESS)
                                        return rc;
                                    pConditional.Vals.Add(tempVal);
                                }
                                break;
                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MIN_MAX:
                                {
                                    /* not conditional
                                                        tempVal.minMaxList = new MIN_MAX_LIST;
                                                        MIN_MAX_VALUE tmpMinMax;
                                                        tmpMinMax.which = 0;
                                                        rc = ddl_parse_expression(chunkp,&len,&tmpMinMax.value);
                                                        if(rc != DDL_SUCCESS)
                                                            return rc;
                                                        tempVal.minMaxList.Add(tmpMinMax);

                                                        pConditional.Vals.Add(tempVal);
                                    **********/
                                }
                                break;

                            case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_LINE_TYPE:
                                {
                                    rc = ddl_parse_linetype(chunkp, &len, ref tempVal.lineType);
                                    if (rc != DDL_SUCCESS)
                                        return rc;
                                    pConditional.Vals.Add(tempVal);
                                }
                                break;

                            default:
                                break;
                        }

                        /*Following lists & compound data type will be handled 
                          separately*/

                        /*			case	DDL_ATTR_DATA_TYPE_ITEM_ID_LIST: 
                                                        break;
                                    case	DDL_ATTR_DATA_TYPE_ENUM_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_REFERENCE_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_TRANSACTION_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_RESPONSE_CODE_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_MENU_ITEM_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_OP_REF_TRAIL_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_DEFINITION:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_REFRESH_RELATION:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_UNIT_RELATION:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_ITEM_ARRAY_ELEMENT_LIST:
                                        break;
                                    case	DDL_ATTR_DATA_TYPE_MEMBER_LIST: 
                                        break; */

                    }/*end switch pConditional.attrDataType*/
                    break;
                default:
                    /*it should not come here*/

                    return DDL_ENCODING_ERROR;
                    //break;

            }/*End switch tag*/

            return SUCCESS;
        }/*end ddl_parse_conditional*/

        public static unsafe int ddl_parse_linetype(byte** chunkp, uint* size, ref LINE_TYPE lnType)
        {
            //int rc = 0;
            uint tag = 0, len = 0;
            //uint tmp = 0;
            UInt64 LL;

            //ASSERT_DBG(chunkp && *chunkp && *size);

            /*Parse the tag to find what type of line type is this*/

            DDL_PARSE_TAG(chunkp, size, &tag, &len);
            //assert(tag < _UI16_MAX);

            switch (tag)
            {

                case DATA_LINETYPE:
                    {
                        DDL_PARSE_INTEGER(chunkp, size, &LL);
                        lnType.qual = (int)LL;
                    }
                    break;
                case LOWLOW_LINETYPE:
                case LOW_LINETYPE:
                case HIGH_LINETYPE:
                case HIGHHIGH_LINETYPE:
                case TRANSPARENT_LINETYPE:
                    lnType.qual = 0;
                    break;

                default:
                    return DDL_ENCODING_ERROR;
                    //break;

            }/*End switch*/

            lnType.type = (ushort)tag;

            return DDL_SUCCESS;

        }/*End ddl_parse_linetype*/

        public static unsafe int ddl_parse_bitstring(byte** chunkp, uint* size, ref uint bitstring)
        {
            //ADDED By Deepak initialize all vars
            byte* chunk = null;        /* temp ptr to the binary chunk */
            uint read_mask = 0;    /* mask used to read bits from the binary */
            uint write_mask = 0;   /* mask used to store the bits  */
            uint value = 0;        /* temp storage of parsed bits */
            uint count = 0;        /* local value of binary size */
            int more_indicator = 0; /* indicators more bits to be read */
            int last_unused = 0;    /* # of bits to ignore in last octet */
            int bitlimit = 0;   /* loop counter */
            byte c = 0;

            //ASSERT_DBG(chunkp && *chunkp && size);

            /*
            * Read the first character
            */

            chunk = *chunkp;
            count = *size;
            value = 0;
            count--;
            c = *chunk++;

            /*
            * Special case the situation where there is no bitstring (that is, the
            * bit string is zero).
            */

            if (c == 0x80)
            {
                *size = count;
                *chunkp = chunk;

                //if (bitstring != 0)
                {
                    bitstring = value;
                }
                return DDL_SUCCESS;
            }

            /*
            * Pull out the number of bits to ignore in the last octet, and the
            * indicator of whether there are more octets.
            */

            more_indicator = c & 0x10;
            last_unused = (c & 0xE0) >> 5;
            c &= 0xF;

            /*
            * Build up the return value.  Note that in each octet, the highest
            * order bit corresponds to the lowest order bit in the returned bit
            * mask.  That is, in the first octet, bit 3 corresponds to bit 0, bit
            * 2 to bit 1, etc.  In the next octet, bit 6 corresponds to bit 4, bit
            * 5 to bit 5, bit 4 to bit 6, ...  In the next octet, bit 6
            * corresponds to bit 11, bit 5 to bit 12, ...
            */

            write_mask = 1;
            read_mask = 0x8;
            bitlimit = 4;

            while (more_indicator != 0)
            {

                if (count == 0)
                {

                    return DDL_INSUFFICIENT_OCTETS;
                }

                for (; bitlimit != 0; bitlimit--, read_mask >>= 1, write_mask <<= 1)
                {

                    if ((c & read_mask) != 0)
                    {

                        value |= write_mask;
                    }
                }

                /*
                * Read the next octet.
                */

                count--;
                c = *chunk++;
                more_indicator = c & 0x80;
                c &= 0x7F;
                read_mask = 0x40;
                bitlimit = 7;
            }

            /*
            * In the last octet, some of the bits may be ignored.  The number of
            * bits to ignore was specified in the very first octet, and remembered
            * in "last_unused".
            */

            bitlimit -= last_unused;

            if (bitlimit <= 0)
            {

                return DDL_ENCODING_ERROR;
            }

            for (; bitlimit != 0; bitlimit--, read_mask >>= 1, write_mask <<= 1)
            {

                if (write_mask == 0)
                {

                    return DDL_LARGE_VALUE;
                }
                if ((c & read_mask) != 0)
                {

                    value |= write_mask;
                }
            }

            *size = count;
            *chunkp = chunk;

            //if (bitstring != 0)
            {

                bitstring = value;
            }

            return DDL_SUCCESS;


        }/* End ddl_parse_bitstring */

        public unsafe static int parse_attr_string(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            fixed (byte* chu = &binChunk[uiOffset])
            {

                byte** chunkp = &chu;
                uint* length = null;
                uint tag, tagp, len;
                //	long temp;
                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);

                length = &size;

                /*Parse the Tag to know if we have a conditional or a direct object*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);


                switch (tag)
                {

                    case IF_TAG: /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                            /*Now Parse the ddpExpression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pCond.expr);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/



                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                pAttr.pCond.byNumberOfSections++;

                            }

                        }
                        break; /*End IF_TAG*/

                    case SELECT_TAG: /*We have a Switch Case conditional*/
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pCond.expr);

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (*length > 0)
                            {
                                DDL_PARSE_TAG(chunkp, length, &tagp, &len);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, length, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;
                                            /*
                                                                            ddpExpression :: iterator it;

                                                                            it = tempExpr.begin();

                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                {
                                            #ifdef _PARSER_DEBUG
                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_string()!!!");
                                                                                return DDL_UNHANDLED_STUFF_FAILURE;

                                                                                }
                                                                            temp = (long)it.elem.ulConst;
                                            */
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*										temp = DEFAULT_TAG_VALUE;
                                                                            pAttr.pCond.caseVals.Add(temp);
                                            */
                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/

                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/


                        }
                        break; /*End SELECT_TAG*/

                    case OBJECT_TAG: /*We have a direct object*/
                        {

                            pAttr.pVals = new VALUES();

                            pAttr.pVals.strVal = new ddpSTRING();

                            rc = ddl_parse_string(chunkp, length, ref pAttr.pVals.strVal);

                        }
                        break; /*End OBJECT_TAG*/
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch tag*/
                return SUCCESS;

            } /*End parse_attr_string*/
        }

        public unsafe static int parse_attr_bitstring(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            //byte** chunkp = null;
            uint* length = null;
            uint tag, tagp, len;
            //	long temp;
            uint tempLong = 0;
            ddpExpression tempExpr = new ddpExpression();

            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                length = &size;

                /*Parse the Tag to know if we have a conditional or a direct object*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                switch (tag)
                {

                    case IF_TAG: /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                            /*Now Parse the ddpExpression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pCond.expr);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/



                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                pAttr.pCond.byNumberOfSections++;

                            }

                        }
                        break; /*End IF_TAG*/

                    case SELECT_TAG: /*We have a Switch Case conditional*/
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pCond.expr);

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (*length > 0)
                            {
                                DDL_PARSE_TAG(chunkp, length, &tagp, &len);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, length, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;
                                            /*
                                                                                    ddpExpression :: iterator it;

                                                                                    it = tempExpr.begin();

                                                                                    if(it.byElemType != INTCST_OPCODE)
                                                                                        {
                                            #ifdef _PARSER_DEBUG
                                                            fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_bitstring!!!");
                                                                                        return DDL_UNHANDLED_STUFF_FAILURE;
                                            #endif											
                                                                                        }
                                                                                    temp = (long)it.ulConst;
                                            */
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*
                                                                                    temp = DEFAULT_TAG_VALUE;
                                                                                    pAttr.pCond.caseVals.Add(temp);
                                            */
                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/

                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/


                        }
                        break; /*End SELECT_TAG*/

                    case OBJECT_TAG: /*We have a direct object*/
                        {

                            pAttr.pVals = new VALUES();

                            rc = ddl_parse_bitstring(chunkp, length, ref tempLong);

                            if (rc != SUCCESS)
                                return rc;
                            /*Since bitstring value is a uint only, we'll store it as a uint itself*/
                            pAttr.pVals.ullVal = (uint)tempLong;

                        }
                        break; /*End OBJECT_TAG*/
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch tag*/
            }
            return SUCCESS;

        }/*End parse_attr_bitstring*/

        public unsafe static int ddl_parse_data_item(byte** chunkp, uint* size, ref DATA_ITEM dataItem)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint tag = 0, len = 0;
            uint tmp = 0;
            UInt64 LL;


            //ASSERT_DBG(chunkp && *chunkp && *size);

            /*Parse the tag to find what type of data item is this*/

            DDL_PARSE_TAG(chunkp, size, &tag, &len);
            //assert(tag < _UI16_MAX);

            switch (tag)
            {
                case DATA_CONSTANT:
                    /*Its an Integer constant!! Just parse it*/
                    DDL_PARSE_INTEGER(chunkp, size, &LL);
                    //assert(LL < _UI16_MAX);

                    dataItem.data.iconst = (ushort)LL;
                    dataItem.type = (ushort)tag;
                    dataItem.flags = 0;
                    //dataItem.width       = 0;
                    dataItem.mask = 0;
                    break;

                case DATA_REFERENCE:

                    /*Its a variable reference!!*/
                    dataItem.data.reff = new ddpREFERENCE();
                    rc = ddl_parse_ref(chunkp, size, ref dataItem.data.reff);
                    if (rc != DDL_SUCCESS)
                        return rc;
                    dataItem.type = (ushort)tag;
                    dataItem.flags = 0;
                    //dataItem.width = 0;
                    dataItem.mask = 0;
                    break;

                case DATA_REF_FLAGS:
                    /*
                     * A data reference with INFO or INDEX flag bits
                     */
                    dataItem.data.reff = new ddpREFERENCE();
                    rc = ddl_parse_ref(chunkp, size, ref dataItem.data.reff);
                    if (rc != DDL_SUCCESS)
                        return rc;

                    /*Now parse the flags*/
                    rc = ddl_parse_bitstring(chunkp, size, ref tmp);
                    if (rc != DDL_SUCCESS)
                        return rc;
                    dataItem.type = (ushort)tag;
                    dataItem.flags = (ushort)tmp;
                    //dataItem.width = 0;
                    dataItem.mask = 0;
                    break;

                case DATA_REF_WIDTH:
                    /*
                     * A data reference with a data mask.
                     */
                    dataItem.data.reff = new ddpREFERENCE();
                    rc = ddl_parse_ref(chunkp, size, ref dataItem.data.reff);
                    if (rc != DDL_SUCCESS)
                        return rc;

                    dataItem.type = (ushort)tag;

                    /*Parse the data mask*/

                    DDL_PARSE_INTEGER(chunkp, size, &LL);

                    // stevev - 18jun09 - not decoding properly (decoder assumes 8 bit masks)
                    //			dataItem.width = (ushort)ddl_mask_width((uint)LL);
                    // encoding this is ludicrous anyway
                    dataItem.mask = LL;
                    break;

                case DATA_REF_FLAGS_WIDTH:
                    /*
                     * A data reference INFO or INDEX flag bits and a data
                     * mask
                     */
                    dataItem.data.reff = new ddpREFERENCE();
                    rc = ddl_parse_ref(chunkp, size, ref dataItem.data.reff);
                    if (rc != DDL_SUCCESS)
                        return rc;

                    /*Parse the data mask*/

                    DDL_PARSE_INTEGER(chunkp, size, &LL);

                    // stevev - 18jun09 - not decoding properly (decoder assumes 8 bit masks)
                    //			dataItem.width = (ushort)ddl_mask_width((uint)LL);
                    // encoding this is ludicrous anyway
                    dataItem.mask = LL;

                    /*Parse the flags*/

                    rc = ddl_parse_bitstring(chunkp, size, ref tmp);
                    if (rc != DDL_SUCCESS)
                        return rc;

                    dataItem.flags = (ushort)tmp;
                    dataItem.type = (ushort)tag;
                    break;

                case DATA_FLOATING:
                    /*A floating point data item*/

                    fixed (float* fc = &dataItem.data.fconst)
                    {
                        rc = ddl_parse_float(chunkp, size, fc);

                        if (rc != SUCCESS)
                            return rc;
                    }

                    dataItem.type = (ushort)tag;
                    break;

                default:
                    return DDL_ENCODING_ERROR;
                    //break;

            }/*End switch*/

            return DDL_SUCCESS;

        }/*End ddl_parse_data_item*/


        public unsafe static int ddl_parse_dataitems(byte** chunkp, uint* size, ref DATA_ITEM_LIST dataItemList)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint len = 0, len1 = 0, tag = 0; /*length & tag of the parsed binary*/
            byte* leave_pointer = null; /*for safe & proper exit*/

            //	DATA_ITEM tmpDataItem;

            /*Parse the tag , it should be DATA_ITEMS_SEQLIST_TAG*/

            DDL_PARSE_TAG(chunkp, size, &tag, &len);

            if (DATA_ITEMS_SEQLIST_TAG != tag)
                return DDL_ENCODING_ERROR;

            *size -= len;

            leave_pointer = *chunkp + len;

            while (len > 0)
            {
                /*Now parse the tag to know whether its an IF / SELECT or a DIRECT tag*/

                DDL_PARSE_TAG(chunkp, &len, &tag, &len1);

                len -= len1;

                switch (tag)
                {
                    case IF_TAG:
                    case SELECT_TAG:
                        /*Not handled!!!!*/
                        return DDL_UNHANDLED_STUFF_FAILURE;
                    //break;
                    case OBJECT_TAG:
                        while (len1 > 0)
                        {
                            DATA_ITEM tmpDataItem = new DATA_ITEM(); //was (void)memset((char*)&tmpDataItem,0,sizeof(DATA_ITEM));

                            rc = ddl_parse_data_item(chunkp, &len1, ref tmpDataItem);

                            if (rc != DDL_SUCCESS)
                                return rc;

                            dataItemList.Add(tmpDataItem);
                        }/*End while*/

                        break;
                    default:
                        return DDL_ENCODING_ERROR;
                }/*End switch tag*/

            }

            /*This is a safe exit ....*/
            *chunkp = leave_pointer;

            return DDL_SUCCESS;

        }/*End ddl_parse_dataitems*/

        public unsafe static int ddl_parse_respcodes(byte** chunkp, uint* size, ref List<RESPONSE_CODE> respCodeList)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint tag = 0, len = 0;//, tmp = 0;
            UInt64 LL;

            //	RESPONSE_CODE tmpRespCode;

            /* this is crashing the system while trying to convert to wide char 
            ASSERT_DBG(chunkp && *chunkp && *size);
            */
            //if (!chunkp || !(*chunkp) || !(*size))
            //{
            //    LOGIT(CLOG_LOG | CERR_LOG, "ddl_parse_respcodes has a bad parameter!\n");
            //}

            while (*size > 0)
            {
                /*Parse the tag it should be RESPONSE_CODE_TAG*/

                DDL_PARSE_TAG(chunkp, size, &tag, &len);

                if (RESPONSE_CODE_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *size -= len;

                RESPONSE_CODE tmpRespCode = new RESPONSE_CODE(); // was (void)memset((char*)&tmpRespCode,0,sizeof(RESPONSE_CODE));

                /*Now parse the value , type, description & help of the response code*/

                DDL_PARSE_INTEGER(chunkp, &len, &LL);

                tmpRespCode.val = (ushort)LL;

                DDL_PARSE_INTEGER(chunkp, &len, &LL);

                tmpRespCode.type = (ushort)LL;

                rc = ddl_parse_string(chunkp, &len, ref tmpRespCode.desc);

                if (rc != DDL_SUCCESS)
                    return rc;

                tmpRespCode.evaled |= RESPONSE_CODE.RS_TYPE_EVALED | RESPONSE_CODE.RS_VAL_EVALED | RESPONSE_CODE.RS_DESC_EVALED;

                if (len > 0) /*If help is there...*/
                {   /*Parse it!!*/

                    rc = ddl_parse_string(chunkp, &len, ref tmpRespCode.help);

                    if (rc != DDL_SUCCESS)
                        return rc;

                    tmpRespCode.evaled |= RESPONSE_CODE.RS_HELP_EVALED;

                }/*Endif len*/

                /*push this value on to the list*/

                respCodeList.Add(tmpRespCode);

            }/*End while *size > 0*/

            return DDL_SUCCESS;

        }/*End ddl_parse_respcodes*/


        public unsafe static int ddl_parse_reflist(byte** chunkp, uint* size, ref List<ddpREFERENCE> refList)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            //	uint tag, len;/*tag & length of the parse binary*/

            //	ddpREFERENCE	tempRef; /*Reference to be parsed*/

            while (*size > 0)
            {
                ddpREFERENCE tempRef = new ddpREFERENCE();
                rc = ddl_parse_ref(chunkp, size, ref tempRef);
                if (rc != DDL_SUCCESS)
                    return rc;
                refList.Add(tempRef);

                ///tempRef.Clear();/*clean it for the next iteration*/
                                //		tempRef.erase(tempRef.begin(),tempRef.end());
            }/*End while */

            return DDL_SUCCESS;

        }/*End ddl_parse_reflist*/


        public unsafe static int parse_attr_transaction_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            uint tag, tag1, len, len1, len2;//,tmpInt;

            uint* length = null;

            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                length = &size;
                pAttr.pVals = new VALUES();

                UInt64 LL;

                //pAttr.pVals.transList = new TRANSACTION_LIST;

                while (*length > 0)
                {
                    //26aug10   was::> (void)memset((char*)&tmpTrans,0,sizeof(TRANSACTION));
                    //TRANSACTION tmpTrans;// 26aug10  try to stop the 
                    //  _SCL_SECURE_VALIDATE(this._Has_container());
                    // assertion.

                    TRANSACTION tmpTrans = new TRANSACTION();

                    /*Parse the tag it should be TRANSACTION_TAG*/

                    DDL_PARSE_TAG(chunkp, length, &tag, &len);

                    if (tag != TRANSACTION_TAG)
                        return DDL_ENCODING_ERROR;

                    *length -= len;

                    /*Parse the transaction number*/

                    DDL_PARSE_INTEGER(chunkp, &len, &LL);

                    tmpTrans.number = (uint)LL;

                    /*
                     * The rest of the TRANSACTION definition consists of a series of
                     * REQUEST, REPLY, and RESPONSE_CODE entries.  Parse the tag to
                     * determine which of the entries each one is, and then parse the entry.
                     */
                    while (len > 0)
                    {
                        DDL_PARSE_TAG(chunkp, &len, &tag, &len1);

                        switch (tag)
                        {
                            case TRANS_REQ_TAG:
                                rc = ddl_parse_dataitems(chunkp, &len, ref tmpTrans.request);
                                if (rc != DDL_SUCCESS)
                                    return rc;
                                break;

                            case TRANS_REPLY_TAG:
                                rc = ddl_parse_dataitems(chunkp, &len, ref tmpTrans.reply);
                                if (rc != DDL_SUCCESS)
                                    return rc;
                                break;
                            case TRANS_RESP_CODES_TAG:

                                DDL_PARSE_TAG(chunkp, &len, &tag1, &len1);

                                if (tag1 != RSPCODES_SEQLIST_TAG)
                                    return DDL_ENCODING_ERROR;

                                len -= len1;

                                /*Parse the tag to ascertain whether its a IF / SELECT or a DIRECT object*/
                                /*Its an assumption that we won't encounter conditional here*/


                                DDL_PARSE_TAG(chunkp, &len1, &tag1, &len2);

                                switch (tag1)
                                {
                                    case IF_TAG:
                                    case SELECT_TAG:
                                        return DDL_UNHANDLED_STUFF_FAILURE;
                                    //break;

                                    case OBJECT_TAG:

                                        rc = ddl_parse_respcodes(chunkp, &len1, ref tmpTrans.rcodes);
                                        if (rc != DDL_SUCCESS)
                                            return rc;
                                        break;
                                    default:
                                        return DDL_ENCODING_ERROR;
                                        //break;
                                }/*End switch*/
                                break;

                            case TRANS_POSTRCV_ACTION_TAG:

                                DDL_PARSE_TAG(chunkp, &len, &tag1, &len1);

                                if (tag1 != REFERENCE_SEQLIST_TAG)
                                    return DDL_ENCODING_ERROR;

                                len -= len1;

                                /*Parse the tag to ascertain whether its a IF / SELECT or a DIRECT object*/
                                /*Its an assumption that we won't encounter conditional here*/


                                DDL_PARSE_TAG(chunkp, &len1, &tag1, &len2);

                                switch (tag1)
                                {
                                    case IF_TAG:
                                    case SELECT_TAG:
                                        return DDL_UNHANDLED_STUFF_FAILURE;
                                    //break;

                                    case OBJECT_TAG:

                                        rc = ddl_parse_reflist(chunkp, &len2, ref tmpTrans.post_rqst_rcv_act);

                                        if (rc != DDL_SUCCESS)
                                            return rc;

                                        break;
                                    default:
                                        return DDL_ENCODING_ERROR;
                                        //break;
                                }/*End switch*/
                                break;


                            default:
                                return DDL_ENCODING_ERROR;
                        }/*switch tag*/

                    }/*End while len > 0*/

                    pAttr.pVals.transList.Add(tmpTrans);
                }/*End while*/
            }
            return SUCCESS;

        }/*End parse_attr_transaction_list*/
        public unsafe static int parse_attr_int(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            UInt64 LL;
            ddpExpression tempExpr = new ddpExpression();
            //byte** chunkp = null;
            uint* length = null;
            uint tag, tagp, len;
            byte** chunkp;
            //	long temp;
            //uint tempLong = 0;

            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {

                //ASSERT_DBG(binChunk && size);

                chunkp = &chunkp1;
                length = &size;

                /*Parse the Tag to know if we have a conditional or a direct object*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                switch (tag)
                {

                    case IF_TAG: /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                            /*Now Parse the ddpExpression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pCond.expr);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/



                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                pAttr.pCond.byNumberOfSections++;

                            }

                        }
                        break; /*End IF_TAG*/

                    case SELECT_TAG: /*We have a Switch Case conditional*/
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pCond.expr);

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (*length > 0)
                            {
                                DDL_PARSE_TAG(chunkp, length, &tagp, &len);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, length, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;

                                            /*										ddpExpression :: iterator it;

                                                                                    it = tempExpr.begin();

                                                                                    if(it.byElemType != INTCST_OPCODE)
                                                                                        {
                                            #ifdef _PARSER_DEBUG
                                                            fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_int()!!!");
                                                                                            return DDL_UNHANDLED_STUFF_FAILURE;
                                            #endif

                                                                                        }
                                                                                    temp = (long)it.elem.ulConst;
                                            */
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*										temp = DEFAULT_TAG_VALUE;
                                                                                    pAttr.pCond.caseVals.Add(temp);
                                            */

                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/

                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/


                        }
                        break; /*End SELECT_TAG*/

                    case OBJECT_TAG: /*We have a direct object*/
                        {

                            //??????pAttr.pVals = new VALUES();

                            DDL_PARSE_INTEGER(chunkp, length, &LL);

                            if (DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT == pAttr.attrDataType)
                            {
                                pAttr.pVals.llVal = (Int64)LL;
                            }
                            else /*DDL_ATTR_DATA_TYPE_UNSIGNED_LONG == pAttr.attrDataType*/
                            {
                                pAttr.pVals.ullVal = LL;
                            }

                        }
                        break; /*End OBJECT_TAG*/
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch tag*/
            }
            return SUCCESS;

        }/*End parse_attr_int*/

        public unsafe static int parse_attr_disp_edit_format(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, ushort tagExpected, uint uiOffset)
        {

            //int rc;
            fixed (byte* chu = &binChunk[uiOffset])
            {
                uint tag, len;
                byte* chub = chu;

                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (tag != tagExpected)
                    return DDL_ENCODING_ERROR;

                /*Now its just the string*/
                uiOffset += (uint)(chu - chub);
            }
            return parse_attr_string(ref pAttr, ref binChunk, size, uiOffset);

        }/* End parse_attr_disp_edit_format */

        public unsafe static int parse_attr_scaling_factor(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc = DDL_SUCCESS;
            uint tag, tagp, len, len1;

            fixed (byte* chu = &binChunk[uiOffset])
            {
                byte** chunkp = &chu;

                uint* length = &size;

                //	long temp;


                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);

                /*Parse the tag it should be either SCALING_FACTOR_TAG */


                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                if (SCALING_FACTOR_TAG != tag)
                    return DDL_ENCODING_ERROR;

                /*First parse the tag to ascertain whether its a IF / SELECT or a DIRECT tag*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                /*We won't loop here since it's not a list*/
                switch (tag)
                {
                    case IF_TAG:    /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF,
                                                                pAttr.attrDataType,
                                                                1);

                            /*Now Parse the ddpExpression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, &len, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, &len);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, &len);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/
                                pAttr.pCond.byNumberOfSections++;
                            }

                        }
                        break; /*End IF_TAG*/
                    case SELECT_TAG:
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, &len, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (len > 0)
                            {
                                DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);
                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/
                                            rc = ddl_parse_expression(chunkp, &len, ref tempExpr);
                                            if (rc != SUCCESS)
                                                return rc;
                                            /*
                                                                                ddpExpression :: iterator it;
                                                                                it = tempExpr.begin();

                                                                                if(it.byElemType != INTCST_OPCODE)
                                                                                {
                                            #ifdef _PARSER_DEBUG
                                                            fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_min_max_list()!!!");
                                                                                        return DDL_UNHANDLED_STUFF_FAILURE;
                                            #endif											
                                                                                }
                                                                                temp = (long)it.elem.ulConst;
                                            */
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, &len);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*
                                                                                temp = DEFAULT_TAG_VALUE;
                                                                                pAttr.pCond.caseVals.Add(temp);
                                            */
                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, &len);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/
                                    default:
                                        return DDL_ENCODING_ERROR;
                                }/*End Switch tagp*/


                            }/*End while*/

                        }/*End SELECT_TAG*/
                        break;
                    case OBJECT_TAG:
                        {
                            pAttr.pVals = new VALUES();

                            pAttr.pVals.pExpr = new ddpExpression();

                            rc = ddl_parse_expression(chunkp, &size, ref pAttr.pVals.pExpr);

                            if (rc != DDL_SUCCESS)
                                return rc;

                        }
                        break;
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch*/
            }

            return SUCCESS;

        }/*End parse_attr_scaling_factor*/

        /*This method calls ddl_parse_reference to parse the item array name to which a variable indexing*/

        public unsafe static int parse_attr_array_name(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            //int rc; /*return code*/
            uint tag, len; /*tag and length of the binary, we'll just neglect the len*/

            /*
             *  The first byte of a chunk will be a ITEM_ARRAYNAME_TAG
             *  Strip off the ITEM_ARRAYNAME_TAG.
             */
            fixed (byte* chu = &binChunk[uiOffset])
            {

                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (ITEM_ARRAYNAME_TAG != tag)
                    return DDL_ENCODING_ERROR;

                pAttr.pVals = new VALUES();

                pAttr.pVals.reff = new ddpREFERENCE();

                /*Call ddl_parse_ref to parse the array name reference*/

                return ddl_parse_ref(&chu, &size, ref pAttr.pVals.reff);
            }

        }/*End parse_attr_reference*/

        public unsafe static int parse_attr_expr(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;
            fixed (byte* chu = &binChunk[uiOffset])
            {
                byte** chunkp = &chu;
                uint* length = &size;
                uint tag, tagp, len;
                //	long temp;

                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);


                /*Parse the Tag to know if we have a conditional or a direct object*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                switch (tag)
                {

                    case IF_TAG: /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                            /*Now Parse the Expression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, length, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/



                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                pAttr.pCond.byNumberOfSections++;

                            }

                        }
                        break; /*End IF_TAG*/

                    case SELECT_TAG: /*We have a Switch Case conditional*/
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the Expression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, length, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (*length > 0)
                            {
                                DDL_PARSE_TAG(chunkp, length, &tagp, &len);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, length, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;
                                            /*
                                                                                    Expression :: iterator it;

                                                                                    it = tempExpr.begin();

                                                                                    if(it.byElemType != INTCST_OPCODE)
                                                                                        {
                                            #ifdef _PARSER_DEBUG
                                                            fprintf(ferr,"\n ddpExpression encountered in case tag parse_attr_expr()!!!");
                                                                                        return DDL_UNHANDLED_STUFF_FAILURE;
                                            #endif												
                                                                                        }
                                                                                    temp = (long)it.elem.ulConst;
                                            */
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*
                                                                                    temp = DEFAULT_TAG_VALUE;
                                                                                    pAttr.pCond.caseVals.Add(temp);
                                            */
                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/

                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/


                        }
                        break; /*End SELECT_TAG*/

                    case OBJECT_TAG: /*We have a direct object*/
                        {

                            pAttr.pVals = new VALUES();

                            pAttr.pVals.pExpr = new ddpExpression();

                            rc = ddl_parse_expression(chunkp, length, ref pAttr.pVals.pExpr);

                            if (rc != DDL_SUCCESS)
                                return rc;

                        }
                        break; /*End OBJECT_TAG*/
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch tag*/
            }
            return SUCCESS;


        }/*End parse_attr_expr*/

        public unsafe static int parse_attr_time_scale(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            uint tag, len;
            ulong ulval;

            fixed (byte* chu = &binChunk[uiOffset])
            {
                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (tag != TIME_SCALE_TAG)
                    return DDL_ENCODING_ERROR;

                /*Now its just an integer */
                pAttr.pVals = new VALUES();
                /*Simply parse the integer & return */
                DDL_PARSE_INTEGER(&chu, &size, &ulval);
            }

            pAttr.pVals.ullVal = ulval;

            return rc;


        }/*End parse_attr_time_scale*/

        public unsafe static int parse_attr_menu_item_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;

            fixed (byte* chu = &binChunk[uiOffset])
            {

                byte** chunkp;// = null;

                chunkp = &chu;


                uint* lengthp = &size;
                uint tag, tagp, length, len, len1;
                //	long temp;

                DDlConditional tempPtrToCond = null;

                List<MENU_ITEM> tempPtrToMenuItemList;// = null;

                MENU_ITEM tmpMenuItem;

                VALUES tempVal = new VALUES();

                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);

                /*The first tag should be a MENU_ITEM_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (MENU_ITEM_SEQLIST_TAG != tag)
                {
                    return DDL_ENCODING_ERROR;
                }

                //*lengthp -= length;// never used, apparently for debugging

                /*Parse the Tag to know if we have a conditional or a direct object*/

                while (length > 0)
                {
                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {
                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != Common.SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/

                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MENU_ITEM_SEQLIST_TAG);

                                if (rc != Common.SUCCESS)
                                    return rc; /* Return if not successful*/



                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MENU_ITEM_SEQLIST_TAG);
                                    if (rc != Common.SUCCESS)
                                        return rc; /* Return if not successful*/

                                    tempPtrToCond.byNumberOfSections++;

                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression Argument of the SELECT */

                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != Common.SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    //sjv 18apr06								DDL_PARSE_TAG(chunkp,&length,&tagp,&len1);
                                    DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);
                                    //sjv 18apr06-comment 29jun06 
                                    //			 - using len in the parses within this while loop has nothing to do with Implicit
                                    //			   it is equivelent to doing a 'len -= len1;' here ( and using len1 there ), 
                                    //			   but doing the subtraction a little at a time
                                    switch (tagp)
                                    {
                                        case CASE_TAG:
                                            {
                                                /*We are parsing the CASE constants as expression
                                                just bcoz of the spec. But it should be a constant 
                                                value , ie. an expression with just a  constant (integer)
                                                value*/

                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);// Implicit tag, do not use len1

                                                if (rc != Common.SUCCESS)
                                                    return rc;
                                                /*
                                                                            ddpExpression :: iterator it;

                                                                            it = tempExpr.begin();

                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                {
                                                #ifdef _PARSER_DEBUG
                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_menu_item_list() !!!");
                                                                                    return DDL_UNHANDLED_STUFF_FAILURE;
                                                #endif												
                                                                                }
                                                                            temp = (long)it.elem.ulConst;
                                                */
                                                tempPtrToCond.caseVals.Add(tempExpr);

                                                /*We have the case constant value 
                                                Now parse the attributre value from the 
                                                following chunk	*/

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MENU_ITEM_SEQLIST_TAG);// Implicit tag, do not use len1
                                                if (rc != Common.SUCCESS)
                                                    return rc; /* Return if not successful*/

                                                tempPtrToCond.byNumberOfSections++;

                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                /*
                                                                            temp = DEFAULT_TAG_VALUE;
                                                                            pAttr.pCond.caseVals.Add(temp);
                                                */
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                tempPtrToCond.caseVals.Add(tempExpr);  // timj repaired during tokenizer V&V 16jul2014

                                                tempPtrToCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MENU_ITEM_SEQLIST_TAG);// Implicit tag, do not use len1
                                                if (rc != Common.SUCCESS)
                                                    return rc; /* Return if not successful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/


                                }/*End while*/

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    //pAttr.pVals = new VALUES;

                                    //pAttr.pVals.menuItemsList = new MENU_ITEM_LIST;

                                    rc = ddl_parse_menuitems(chunkp, &len, ref pAttr.pVals.menuItemsList);

                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.byNumOfChunks++;
                                    break;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.menuItemsList = new MENU_ITEM_LIST;

                                    rc = ddl_parse_menuitems(chunkp, &len, ref tempVal.menuItemsList);

                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);//Vibhor 200105: Changed


                                    pAttr.byNumOfChunks++;


                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals, we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }
                                break;

                            }
                        /*End OBJECT_TAG*/
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/
                }/*End while length > 0 */

                if (/*pAttr.pVals == null
                    &&*/ pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    //pAttr.pVals = new VALUES;
                    //pAttr.pVals.menuItemsList = new MENU_ITEM_LIST;
                    for (ushort i = 0; i < pAttr.directVals.Count; i++)
                    {
                        tempPtrToMenuItemList = pAttr.directVals[i].menuItemsList;//??????//??????
                        for (ushort j = 0; j < tempPtrToMenuItemList.Count; j++)
                        {
                            tmpMenuItem = tempPtrToMenuItemList[j];
                            pAttr.pVals.menuItemsList.Add(tmpMenuItem);
                            ///tmpMenuItem.Cleanup();

                        }/*Endfor j*/

                        /*Just Clear this list, its not required any more*/
                        tempPtrToMenuItemList.Clear();

                        //stevev - merge - 20feb07 - make it always #if _MSC_VER >= 1300  // HOMZ - port to 2003, VS7 - Memory Leak fix
                        //#endif
                    }/*Endfor i*/

                    /*Now Clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;
                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                {
                    pAttr.bIsAttributeConditional = false;
                }

                /*Vibhor 180105: End of Code*/
            }

            return SUCCESS;


        }/*End parse_attr_menu_item_list*/

        public static int parse_attr_ulong(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            return parse_attr_int(ref pAttr, ref binChunk, size, uiOffset);

        }/*End parse_attr_int*/


        public unsafe static int parse_attr_menu_style(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            uint tag, len;
            fixed (byte* chu = &binChunk[uiOffset])
            {
                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (tag != MENU_STYLE_TAG)
                {
                    return DDL_ENCODING_ERROR;
                }

                /*Now its just an integer */
                //pAttr.pVals = new VALUES;

                /*Simply parse the integer & return */
                UInt64 ulTemp = pAttr.pVals.ullVal;

                DDL_PARSE_INTEGER(&chu, &size, &ulTemp);

                pAttr.pVals.ullVal = ulTemp;
            }

            //rc = parse_attr_int(pAttr,binChunk,size);

            return rc;

        }/*End parse_attr_menu_style*/


        public unsafe static int parse_attr_enum_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            List<ENUM_VALUE> tempPtrToEnumList;// = null;
            fixed (byte* chu = &binChunk[uiOffset])
            {
                byte** chunkp = &chu;
                uint* lengthp = &size;
                uint tag, tagp, length, len, len1;


                //	DDlConditional *tempPtrToCond = null;


                //ENUM_VALUE tmpEnm;

                VALUES tempVal = new VALUES();


                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);
                /*The first tag should be a ENUMERATOR_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (ENUMERATOR_SEQLIST_TAG != tag)
                {
                    return DDL_ENCODING_ERROR;
                }

                *lengthp -= length;


                while (length > 0)
                {
                    //		DDlConditional *tempPtrToCond = null;

                    /*Parse the Tag to know if we have a conditional or a direct object*/

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {

                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                DDlConditional tempPtrToCond = null;

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }


                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/

                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ENUMERATOR_SEQLIST_TAG);

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/



                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ENUMERATOR_SEQLIST_TAG);
                                    if (rc != SUCCESS)
                                        return rc; /* Return if not successful*/

                                    tempPtrToCond.byNumberOfSections++;

                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {
                                DDlConditional tempPtrToCond = null;

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression Argument of the SELECT */

                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);

                                    switch (tagp)
                                    {
                                        case CASE_TAG:
                                            {
                                                /*We are parsing the CASE constants as expression
                                                just bcoz of the spec. But it should be a constant 
                                                value , ie. an expression with just a  constant (integer)
                                                value*/

                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);

                                                if (rc != SUCCESS)
                                                    return rc;
                                                /*
                                                                                            ddpExpression :: iterator it;

                                                                                            it = tempExpr.begin();

                                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                                {
                                                    #ifdef _PARSER_DEBUG
                                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_enum_list!!!");
                                                                                                    return DDL_UNHANDLED_STUFF_FAILURE;
                                                    #endif												
                                                                                                }
                                                                                            temp = (long)it.elem.ulConst;
                                                */
                                                tempPtrToCond.caseVals.Add(tempExpr);

                                                /*We have the case constant value 
                                                Now parse the attributre value from the 
                                                following chunk	*/

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ENUMERATOR_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/

                                                tempPtrToCond.byNumberOfSections++;

                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                /*
                                                                                            temp = DEFAULT_TAG_VALUE;
                                                                                            pAttr.pCond.caseVals.Add(temp);
                                                */
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                tempPtrToCond.caseVals.Add(tempExpr);
                                                // note that tempPtrToCond == pAttr.pCond in a non-list


                                                tempPtrToCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ENUMERATOR_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/


                                }/*End while*/

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }


                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    pAttr.pVals = new VALUES();

                                    //pAttr.pVals.enmList = new ENUM_VALUE_LIST;

                                    rc = ddl_parse_enums(chunkp, &len, ref pAttr.pVals.enmList);

                                    if (rc != DDL_SUCCESS)

                                        return rc;

                                    pAttr.byNumOfChunks++;
                                    break;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.enmList = new ENUM_VALUE_LIST;

                                    rc = ddl_parse_enums(chunkp, &len, ref tempVal.enmList);

                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);//Vibhor 200105: Changed

                                    //							pAttr.bIsAttributeConditionalList = true;


                                    pAttr.byNumOfChunks++;

                                    ///tempVal.enmList.clear();


                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals,
                                    we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }/*Parse the enumerations*/
                                break;
                            }
                        /*End OBJECT_TAG*/
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/


                }/*End while length > 0*/

                if (/*pAttr.pVals == null
                    &&*/ pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    pAttr.pVals = new VALUES();
                    //pAttr.pVals.enmList = new ENUM_VALUE_LIST;

                    VALUES ittVALS;
                    //for (int i = 0; i < pAttr.directVals.Count; i++)// a ValueList  aka vector  <VALUES>    
                    //for (ittVALS = pAttr.directVals.begin(); ittVALS != pAttr.directVals.end(); ++ittVALS)
                    for (int i = 0; i < pAttr.directVals.Count; i++)
                    {//a ptr 2a VALUES
                        ittVALS = pAttr.directVals[i];
                        //tempPtrToEnumList = pAttr.directVals.at(i).enmList;
                        // 			tempPtrToEnumList = ((VALUES*) ittVALS).enmList; // a ENUM_VALUE_LIST* PAW see below 03/03/09
                        tempPtrToEnumList = ittVALS.enmList; // a ENUM_VALUE_LIST* see above PAW
                        if (tempPtrToEnumList == null)
                            continue; // skip nulls
                        //ENUM_VALUE_LIST::iterator ENUM_VALUE; // a ENUM_VALUE*
                        //ENUM_VALUE ENUM_VALUE;
                        //for (int j =0; j < tempPtrToEnumList.Count; j++)
                        //for (ittENMVAL = tempPtrToEnumList.begin(); ittENMVAL != tempPtrToEnumList.end(); ++ittENMVAL)
                        for (int j = 0; j < tempPtrToEnumList.Count(); j++)
                        {// a ptr 2a ENUM_VALUE
                         //tmpEnm = tempPtrToEnumList.at(j);
                         //pAttr.pVals.enmList.Add(tmpEnm);
                         //				peV = (ENUM_VALUE*) ittENMVAL;//PAW see below 03/03/09
                         //peV = (ENUM_VALUE*)&(*ittENMVAL);//PAW
                            pAttr.pVals.enmList.Add(tempPtrToEnumList[j]);
                            //peV.Cleanup();
                        }/*Endfor j*/

                        /*Just clear this list, its not required any more*/
                        tempPtrToEnumList.Clear();
                        //tempPtrToEnumList.erase(tempPtrToEnumList.begin(),tempPtrToEnumList.end());
                        //			((VALUES*) ittVALS).enmList = null;//PAW see below 03/03/09
                        //((VALUES*)&(*ittVALS)).enmList = null;//PAW see above
                        //pAttr.directVals[i].enmList = null;//??????
                    }/*Endfor i*/

                    /*Now clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                    pAttr.bIsAttributeConditional = false;

                /*Vibhor 180105: End of Code*/
            }

            return SUCCESS;

        }/*End parse_attr_enum_list*/

        public unsafe static int parse_attr_orient_size(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            uint tag, len;
            fixed (byte* chu = &binChunk[uiOffset])
            {

                DDL_PARSE_TAG(&chu, &size, &tag, &len);

                if (tag != GRID_ORIENT_TAG)
                {
                    return DDL_ENCODING_ERROR;
                }

                /*Now its just an integer */
                pAttr.pVals = new VALUES();

                /*Simply parse the integer & return */
                UInt64 ulTemp;
                DDL_PARSE_INTEGER(&chu, &size, &ulTemp);
                pAttr.pVals.ullVal = ulTemp;
            }

            return rc;

        }/*End parse_attr_orient_size*/

        public unsafe static int parse_gridmembers_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            byte** chunkp = null;
            uint* lengthp = null;

            uint tag, len, length;
            uint tag1, len1;

            fixed (byte* chu = &binChunk[uiOffset])
            {
                DDlConditional tempPtrToCond = null;
                List<GRID_SET> tp_2gridSetList = null;

                //GRID_SET tmpGridSet;
                ddpExpression tempExpr = new ddpExpression();
                VALUES tempVal = new VALUES();

                chunkp = &chu;
                lengthp = &size;

                /*The first tag should be a GRID_SEQLIST_TAG if not then return error*/
                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (tag != GRID_SEQLIST_TAG)
                    return DDL_ENCODING_ERROR;

                *lengthp -= length;// get finished length (for return value?)

                /*Parse the Tag to know if we have a conditional or a direct object*/

                while (length > 0)
                {
                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {
                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/
                                    pAttr.pCond = tempPtrToCond;
                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);
                                }
                                pAttr.byNumOfChunks++;

                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/
                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, GRID_SEQLIST_TAG);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/


                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, GRID_SEQLIST_TAG);
                                    if (rc != SUCCESS)
                                        return rc; /* Return if not successful*/

                                    tempPtrToCond.byNumberOfSections++;
                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/
                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }
                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {
                                tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/
                                    pAttr.pCond = tempPtrToCond;
                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);
                                }
                                pAttr.byNumOfChunks++;

                                /*Now Parse the ddpExpression Argument of the SELECT */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));
                                if (rc != SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    DDL_PARSE_TAG(chunkp, &length, &tag1, &len1);

                                    switch (tag1)
                                    {
                                        case CASE_TAG:
                                            {/* DD CASE's are expressions (variables or calc'd values)*/
                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);
                                                if (rc != SUCCESS)
                                                    return rc;

                                                tempPtrToCond.caseVals.Add(tempExpr);

                                                /*We have the case value 
                                                  Now parse the attributre value from the following chunk	*/
                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, GRID_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/

                                                tempPtrToCond.byNumberOfSections++;
                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                pAttr.pCond.caseVals.Add(tempExpr);

                                                tempPtrToCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, GRID_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/

                                }/* wend len */

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/
                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }
                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    pAttr.pVals = new VALUES();

                                    //pAttr.pVals.gridMemList = new GRID_SET_LIST;

                                    rc = ddl_parse_gridMembers(chunkp, &len, ref pAttr.pVals.gridMemList);
                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.gridMemList = new GRID_SET_LIST;

                                    rc = ddl_parse_gridMembers(chunkp, &len, ref tempVal.gridMemList);
                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);

                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals,
                                    we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }
                                pAttr.byNumOfChunks++;
                            }
                            break;/*End OBJECT_TAG*/

                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/
                }/*wend length > 0 */

                if (/*pAttr.pVals == null
                    &&*/ pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    pAttr.pVals = new VALUES();
                    //pAttr.pVals.gridMemList = new GRID_SET_LIST;

                    //GRID_SET_LIST::iterator itGridSet; // a ptr 2a GRID_SET
                    // = null;

                    for (ushort i = 0; i < pAttr.directVals.Count; i++)// vector of VALUES 
                    {
                        tp_2gridSetList = pAttr.directVals[i].gridMemList;
                        // was:for (int j =0; j < tp_2gridSetList.Count; j++)
                        foreach (GRID_SET pGridSet in tp_2gridSetList)// stevev 06aug07
                        {
                            //tmpGridSet = tp_2gridSetList.at(j);
                            //pAttr.pVals.gridMemList.Add(tmpGridSet);
                            //					 pGridSet = (GRID_SET*)itGridSet;	see below
                            //pGridSet = (GRID_SET*)&(*itGridSet);// PAW changed from above 03/03/09
                            pAttr.pVals.gridMemList.Add(pGridSet);
                            //pGridSet.Cleanup();

                        }/*next j*/

                        /*Just clear this list, it's not required any more*/
                        ///tp_2gridSetList.Clear();
                        //pAttr.directVals[i].gridMemList = null;
                    }/*next i*/

                    /*Now clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                {
                    pAttr.bIsAttributeConditional = false;
                }
            }
            return SUCCESS;

        }/*End parse_gridmembers_list*/

        public unsafe static int parse_attr_min_max_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, ushort tagExpected, uint uiOffset)
        {

            int rc;
            uint tag, /*unused tagp,*/ len;/*unused ,len1; */

            fixed (byte* chu = &binChunk[uiOffset])
            {

                byte** chunkp;// = null;

                uint length;
                UInt64 LL;

                //DDlConditional tempPtrToCond = null;

                List<MIN_MAX_VALUE> tempPtrToMinMaxList;// = null;

                MIN_MAX_VALUE tmpMinMax = new MIN_MAX_VALUE();

                // unused	VALUES		tempVal;

                // unused	bool bRetVal;

                //ddpExpression tempExpr;

                //ASSERT_DBG(binChunk && size);

                chunkp = &chu;

                length = size;


                /*Parse the tag it should be either MIN_VALUE_TAG or MAX_VALUE_TAG*/

                DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                if (tag != tagExpected)
                    return DDL_ENCODING_ERROR;


                while (length > 0)
                {                                       // assume pointer was transfered in the Add
                                                        //(void)memset((char*)&tmpMinMax, 0, sizeof(MIN_MAX_VALUE));

                    /*Parse the index of the min / max value*/
                    DDL_PARSE_INTEGER(chunkp, &length, &LL);

                    tmpMinMax.which = (uint)LL;

                    /*parse the Min - max value expression*/

                    tmpMinMax.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_UNDEFINED, DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_EXPRESSION, 1);// number of sections
                    rc = ddl_parse_conditional(ref tmpMinMax.pCond, chunkp, &length);

                    // the list itself is not conditional
                    if ((object)pAttr.pVals == null)
                    {
                        pAttr.pVals = new VALUES();
                        //pAttr.pVals.minMaxList = null;
                    }
                    //if (pAttr.pVals.minMaxList == null)
                    {
                        pAttr.pVals.minMaxList = new List<MIN_MAX_VALUE>();
                    }
                    pAttr.pVals.minMaxList.Add(tmpMinMax);

                }/*End while*/

                if (//pAttr.pVals == null && 
                    pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    pAttr.pVals = new VALUES();
                    //pAttr.pVals.minMaxList = new MIN_MAX_LIST();
                    for (ushort i = 0; i < pAttr.directVals.Count; i++)
                    {
                        tempPtrToMinMaxList = pAttr.directVals[i].minMaxList;
                        for (ushort j = 0; j < tempPtrToMinMaxList.Count; j++)
                        {
                            tmpMinMax = tempPtrToMinMaxList[j];
                            pAttr.pVals.minMaxList.Add(tmpMinMax);

                        }/*Endfor j*/

                        /*Just clear this list, its not required any more*/
                        ///tempPtrToMinMaxList.Clear();
                    }/*Endfor i*/

                    /*Now clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                {
                    pAttr.bIsAttributeConditional = false;

                }

                /*Vibhor 180105: End of Code*/
            }

            return SUCCESS;

        }/* End parse_attr_min_max_list*/

        public unsafe static int parse_ascii_string(ref string retStr, byte** binChunk, ref uint rsize)
        {
            int rc = SUCCESS;
            uint tag, totLen = 0, len;
            UInt64 LL;
            uint size = rsize;

            DDL_PARSE_TAG(binChunk, &size, &tag, &totLen);// implicit, no len

            if (tag != NAME_STR_TAG)
                return DDL_ENCODING_ERROR;


            DDL_PARSE_INTEGER(binChunk, &size, &LL); len = (uint)LL;
            if (len == 0 || len > 0x7ff)// aribitrary maximum len
                return DDL_LARGE_VALUE;

            string rawStr = "";// = new char[len + 1];// +1 just in case this is start of memory leak "Ametek_model_cod" PAW 08/04/09
            //memcpy(rawStr, *binChunk, len);
            size -= len;
            for (int i = 0; i < len; i++)
            {
                rawStr += (char)(*(*binChunk)++);
            }
            //*binChunk += len;
            //if (strlen(rawStr) > len)
            //return DDL_SHORT_BUFFER;

            retStr = rawStr.TrimEnd('\0');
            rsize = size;
            //delete[] rawStr;
            return rc;
        }

        public unsafe static int parse_member_debug_info(ref MEMBER_DEBUG_T mem, byte* binChunk, ref uint rsize)
        {
            int rc = SUCCESS;
            uint tag, len = 0;
            UInt64 LL = 0;
            uint size = rsize;

            DDL_PARSE_TAG(&binChunk, &size, &tag, &len);// explict, len will be returned
            if (tag != DEBUG_ATTR_MEMBER_TAG)
                return DDL_ENCODING_ERROR;

            //symbol name
            if (len > 2)
            {
                size -= len; // assume all will be successful
                rc = parse_ascii_string(ref mem.symbol_name, &binChunk, ref len);
            }
            else
            {
                size = 0;
                rc = DDL_INSUFFICIENT_SIZE;
            }
            // flags
            if (rc == SUCCESS)
                rc = ddl_parse_bitstring(&binChunk, &len, ref (mem.flags));
            //value
            if (rc == SUCCESS)
                DDL_PARSE_INTEGER(&binChunk, &size, &LL);
            mem.member_value = (uint)LL;
            rsize = size;
            return rc;
        }

        public unsafe static int parse_attr_debug_info(ref ATTR_DEBUG_INFO_T pADI, byte* binChunk, ref uint rsize)
        {
            int rc = SUCCESS;
            uint tag, len;
            UInt64 LL = 0;
            //string wrkStr;
            uint size = rsize;

            MEMBER_DEBUG_T wrkMember = new MEMBER_DEBUG_T();
            byte* bc = binChunk;

            DDL_PARSE_TAG(&bc, &size, &tag, &len);// explicit - has a length 

            if (tag != DEBUG_ATTR_INFO_TAG)
                return DDL_ENCODING_ERROR;

            // attr-tag-number
            if (len > 2)
            {
                size -= len; // assume all will be successful
                DDL_PARSE_INTEGER(&bc, &len, &LL);
                pADI.attr_tag = (uint)LL;
            }
            else
            {
                size = 0;
                rc = DDL_INSUFFICIENT_SIZE;
            }

            // lineno
            if (rc == SUCCESS && len > 0)
                DDL_PARSE_INTEGER(&bc, &len, &LL); pADI.attr_lineNo = (uint)LL;
            // filename
            if (rc == SUCCESS && len > 2)
            {
                rc = ddl_parse_string(&bc, &len, ref (pADI.attr_filename));
            }
            // members
            while (len > 2 && rc == SUCCESS)
            {
                rc = parse_member_debug_info(ref wrkMember, binChunk, ref len);
                if (rc == SUCCESS)
                {
                    pADI.attr_member_list.Add(wrkMember);
                }
                wrkMember.symbol_name = null;
                wrkMember.flags = wrkMember.member_value = 0;
            }

            rsize = size;
            return rc;
        }

        public unsafe static int parse_debug_info(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {/* this the item level debug info */
            int rc = SUCCESS;
            uint tag, len;
            UInt64 LL = 0;    // PAW prevents uninitialised variable error 03/03/09
            //string wrkStr;
            //ATTR_DEBUG_INFO_T wrkAttr;

            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;

                DDL_PARSE_TAG(chunkp, &size, &tag, &len);// explicit

                if (tag != DEBUG_INFO_TAG)
                    return DDL_ENCODING_ERROR;

                pAttr.pVals = new VALUES();
                pAttr.pVals.debugInfo = new ITEM_DEBUG_INFO();

                /* symbol name */
                if (len > 2)
                {
                    size -= len; // assume all will be successful
                    rc = parse_ascii_string(ref pAttr.pVals.debugInfo.symbol_name, chunkp, ref len);// PAW memory leak started here 09/04/09
                }
                else
                {
                    size = 0;
                    rc = DDL_INSUFFICIENT_SIZE;
                }


                // file name
                if (rc == SUCCESS && len > 2)
                {
                    rc = ddl_parse_string(chunkp, &len, ref pAttr.pVals.debugInfo.file_name);
                }
                // line number

                if (rc == SUCCESS && len > 0)
                    DDL_PARSE_INTEGER(chunkp, &len, &LL); pAttr.pVals.debugInfo.lineNo = (uint)LL;

                // flags
                if (rc == SUCCESS && len > 0)
                    rc = ddl_parse_bitstring(chunkp, &len, ref pAttr.pVals.debugInfo.flags);

                // if more len
                ATTR_DEBUG_INFO_T pADI;
                while (len > 2 && rc == SUCCESS)
                {
                    // attributes
                    pADI = new ATTR_DEBUG_INFO_T();

                    rc = parse_attr_debug_info(ref pADI, chunkp1, ref len);
                    if (rc == SUCCESS)
                    {
                        pAttr.pVals.debugInfo.attr_list.Add(pADI);
                    }
                }
            }
            return rc;

        }/*End parse_attr_debug_info*/
        /* end  stevev 06may05 */

        public unsafe static int parse_attr_unit_relation(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                uint* lengthp = &size;
                uint tag, length, len;

                /*A unit relation consists of a reference to a unit variable & a list of 
                 references to vars which have that varible as a unit */

                /*Since we can't handle conditionals as subattribute we r expecting direct objects*/

                pAttr.pVals = new VALUES();

                //pAttr.pVals.unitReln = new UNIT_RELATION;

                /*Parse the unit var*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                *lengthp -= length;

                switch (tag)
                {
                    case IF_TAG:
                    case SELECT_TAG:
                        /*Log this one!!!!*/
                        return DDL_UNHANDLED_STUFF_FAILURE;
                    //break;

                    case OBJECT_TAG:

                        rc = ddl_parse_ref(chunkp, &length, ref pAttr.pVals.unitReln.unit_var);
                        if (rc != DDL_SUCCESS)
                            return rc;
                        break;
                    default:
                        return DDL_ENCODING_ERROR;
                }/*End switch*/


                /*Now parse the var list*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (REFERENCE_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *lengthp -= length;

                while (length > 0)
                {

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {
                        case IF_TAG:
                        case SELECT_TAG:
                            /*Log this one!!!!*/
                            return DDL_UNHANDLED_STUFF_FAILURE;
                        //break;
                        case OBJECT_TAG:
                            {

                                rc = ddl_parse_reflist(chunkp, &len, ref pAttr.pVals.unitReln.var_units);
                                if (rc != DDL_SUCCESS)
                                    return rc;

                            }
                            break;
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch*/

                }/*End while*/
            }
            return SUCCESS;

        }

        public unsafe static int parse_attr_refresh_relation(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                uint* lengthp = &size;
                uint tag, length, len;

                /*A refresh relation consists of a list of watch items and a list of update items
                both are reference lists*/

                /*The first tag should be a REFERENCE_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (REFERENCE_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *lengthp -= length; /*The remaining length should contain the update list!!*/

                pAttr.pVals = new VALUES();

                pAttr.pVals.refrshReln = new REFRESH_RELATION();


                /*ASSUMPTION: Since we are parsing a reference list as a subattribute 
                we r not expecting conditionals */

                /*Parse the watch list*/
                while (length > 0)
                {

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {
                        case IF_TAG:
                        case SELECT_TAG:
                            /*Log this one!!!!*/
                            return DDL_UNHANDLED_STUFF_FAILURE;

                        //break;
                        case OBJECT_TAG:
                            {

                                rc = ddl_parse_reflist(chunkp, &len, ref pAttr.pVals.refrshReln.watch_list);
                                if (rc != DDL_SUCCESS)
                                    return rc;

                            }
                            break;

                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch*/

                }/*End while*/

                /*Parse the update list*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (REFERENCE_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;


                while (length > 0)
                {

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {
                        case IF_TAG:
                        case SELECT_TAG:
                            /*Log this one!!!!*/
                            return DDL_UNHANDLED_STUFF_FAILURE;
                        //break;
                        case OBJECT_TAG:
                            {

                                rc = ddl_parse_reflist(chunkp, &len, ref pAttr.pVals.refrshReln.update_list);
                                if (rc != DDL_SUCCESS)
                                    return rc;

                            }
                            break;
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch*/

                }/*End while */
            }

            return SUCCESS;

        }

        public unsafe static int parse_attr_item_array_element_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                uint* lengthp = &size;
                uint tag, tagp, length, len, len1;
                //	long temp;

                //	DDlConditional *tempPtrToCond = null;

                List<ITEM_ARRAY_ELEMENT> tempPtrToElementList;// = null;

                ITEM_ARRAY_ELEMENT tmpElement;

                VALUES tempVal = new VALUES();

                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);

                /*The first tag should be a MENU_ITEM_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (ELEMENT_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *lengthp -= length;

                while (length > 0)
                {
                    DDlConditional tempPtrToCond = null;

                    /*Parse the Tag to know if we have a conditional or a direct object*/

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {

                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != Common.SUCCESS)
                                    return rc; /* Return if not Common.SUCCESSful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/

                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ELEMENT_SEQLIST_TAG);

                                if (rc != Common.SUCCESS)
                                    return rc; /* Return if not Common.SUCCESSful*/



                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ELEMENT_SEQLIST_TAG);
                                    if (rc != Common.SUCCESS)
                                        return rc; /* Return if not Common.SUCCESSful*/

                                    tempPtrToCond.byNumberOfSections++;

                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }


                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression Argument of the SELECT */

                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != Common.SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);

                                    switch (tagp)
                                    {
                                        case CASE_TAG:
                                            {
                                                /*We are parsing the CASE constants as expression
                                                just bcoz of the spec. But it should be a constant 
                                                value , ie. an expression with just a  constant (integer)
                                                value*/

                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);

                                                if (rc != Common.SUCCESS)
                                                    return rc;
                                                /*
                                                                                            ddpExpression :: iterator it;

                                                                                            it = tempExpr.begin();

                                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                                {
                                                    #ifdef _PARSER_DEBUG
                                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_item_array_element_list()!!!");\
                                                                                                return DDL_UNHANDLED_STUFF_FAILURE;
                                                    #endif												
                                                                                                }
                                                                                            temp = (long)it.elem.ulConst;
                                                */
                                                pAttr.pCond.caseVals.Add(tempExpr);

                                                /*We have the case constant value 
                                                Now parse the attributre value from the 
                                                following chunk	*/

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ELEMENT_SEQLIST_TAG);
                                                if (rc != Common.SUCCESS)
                                                    return rc; /* Return if not Common.SUCCESSful*/

                                                tempPtrToCond.byNumberOfSections++;

                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                /*
                                                                                            temp = DEFAULT_TAG_VALUE;
                                                                                            pAttr.pCond.caseVals.Add(temp);
                                                */
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                pAttr.pCond.caseVals.Add(tempExpr);

                                                tempPtrToCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, ELEMENT_SEQLIST_TAG);
                                                if (rc != Common.SUCCESS)
                                                    return rc; /* Return if not Common.SUCCESSful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/


                                }/*End while*/

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    pAttr.pVals = new VALUES();

                                    //pAttr.pVals.itemArrElmnts = new ITEM_ARRAY_ELEMENT_LIST;

                                    rc = ddl_parse_itemarray(chunkp, &len, ref pAttr.pVals.itemArrElmnts);

                                    if (rc != Common.DDL_SUCCESS)

                                        return rc;

                                    pAttr.byNumOfChunks++;
                                    break;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.itemArrElmnts = new ITEM_ARRAY_ELEMENT_LIST;

                                    rc = ddl_parse_itemarray(chunkp, &len, ref tempVal.itemArrElmnts);

                                    if (rc != Common.DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);

                                    //							pAttr.bIsAttributeConditionalList = true;


                                    pAttr.byNumOfChunks++;

                                    //tempVal.enmList.clear();


                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals, we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }
                                break;
                            }
                        /*End OBJECT_TAG*/
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/

                }/*End while length >0 */

                if (/*pAttr.pVals == null
                    && */pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    pAttr.pVals = new VALUES();
                    //pAttr.pVals.itemArrElmnts = new ITEM_ARRAY_ELEMENT_LIST;
                    for (ushort i = 0; i < pAttr.directVals.Count; i++)
                    {
                        tempPtrToElementList = pAttr.directVals[i].itemArrElmnts;
                        for (ushort j = 0; j < tempPtrToElementList.Count; j++)
                        {
                            tmpElement = tempPtrToElementList[j];
                            pAttr.pVals.itemArrElmnts.Add(tmpElement);

                        }/*Endfor j*/

                        /*Just clear this list, its not required any more*/
                        ///tempPtrToElementList.Clear();
                    }/*Endfor i*/

                    /*Now clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                    pAttr.bIsAttributeConditional = false;

                /*Vibhor 180105: End of Code*/
            }

            return Common.SUCCESS;

        }/*End parse_attr_item_array_element_list*/

        public unsafe static int ddl_parse_one_enum(byte** chunkp, uint* size, ref ENUM_VALUE enmVal)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint tag = 0, len = 0;
            uint kind_o_class = 0, which = 0;
            UInt64 LL;
            //	OUTPUT_STATUS tmpStatus;

            while (*size > 0)
            {


                DDL_PARSE_TAG(chunkp, size, &tag, &len);

                switch (tag)
                {
                    case ENUM_VALUE.ENUM_VALUE_TAG:
                        {
                            /*Parse the value of the enumeration*/

                            DDL_PARSE_INTEGER(chunkp, size, &LL);
                            enmVal.val = (uint)LL;

                            enmVal.evaled |= ENUM_VALUE.ENUM_VAL_EVALED;

                        }
                        break;
                    case ENUM_VALUE.ENUM_STATUS_TAG:
                        {
                            /*
                            * Parse the status class information for the enumeration.
                            */

                            *size -= len;

                            /*
                             * Parse the class, and the encoding of the kind and
                             * output class (in one byte).
                             */

                            DDL_PARSE_INTEGER(chunkp, &len, &LL); enmVal.status.status_class = (uint)LL;

                            while (len > 0)
                            {
                                OUTPUT_STATUS tmpStatus;

                                DDL_PARSE_INTEGER(chunkp, &len, &LL); kind_o_class = (uint)LL;

                                if (kind_o_class == OC_NORMAL)
                                {

                                    /*
                                     * 	There should be nothing more to parse
                                     */
                                    /*I donno if this thing needs to be pushed onto the list,
                                     because we are actualyy parsing it & its actually OC_NORMAL
                                    */

                                    if (len > 0)
                                        len = 0;

                                    break;  /* get out of this while loop */
                                }/*endif kind_o_class*/
                                else
                                {

                                    /*
                                     *	In a HART binary, KIND and OUTPUT_CLASS are
                                     *	combined in the same binary byte.  KIND is 
                                     *	stored in bits 0-2 and OUTPUT_CLASS is stored 
                                     *	in bits 3-7 of "kind_oclass".
                                     */



                                    //tmpStatus = (OUTPUT_STATUS*)&enmVal.status.oclasses.list[enmVal.status.oclasses.count++];

                                    tmpStatus.kind = (ushort)(kind_o_class & 0x07);

                                    /*Vibhor 071003: Since OUTPUT_CLASS is actually stored in bits 3 & 4 only
                                    I'm stripping off the rest unused bits, See Spec 504
                                    This further helps in checking the valid combinations of the output classes
                                    against status.oclass as 
                                        AUTO & GOOD 	(0x00)
                                        MANUAL & GOOD	(0x01)
                                        AUTO & BAD      (0x10)
                                        MANUAL & BAD    (0x11)
                                    */
                                    /* stevev 12/7/04 marginal uses another bit*/
                                    tmpStatus.oclass = (ushort)((kind_o_class >> 03) & 0x07);


                                    /*
                                     *	There are 4 types of KIND.
                                     *		DV = dynamic variable.
                                     *		TV = transmitter variable.
                                     *		AO = analog output
                                     *		ALL = all outputs
                                     *
                                     *	DV, TV and AO are followed by the integer "which" which
                                     *	indicates the variable (or output) the output_class is 
                                     *	associated with.
                                     */

                                    if (tmpStatus.kind == OC_DV || tmpStatus.kind == OC_TV || tmpStatus.kind == OC_AO)
                                    {
                                        DDL_PARSE_INTEGER(chunkp, &len, &LL);
                                        which = (uint)LL;
                                        tmpStatus.which = (ushort)which;
                                        //enmVal.status.oclasses.Add(tmpStatus);		//  [6/3/2014 timj]										
                                    }
                                    else
                                    {
                                        tmpStatus.which = 0;
                                    }
                                    enmVal.status.oclasses.Add(tmpStatus);   // make 8 vs 10 data match [6/3/2014 timj]											

                                }/*End Else*/

                            }/*End while len > 0*/

                            enmVal.evaled |= ENUM_VALUE.ENUM_STATUS_EVALED;

                        }
                        break;
                    case ENUM_VALUE.ENUM_ACTIONS_TAG:
                        {
                            /*This is an ID of a Method.... though coded as a reference
                              ie.  the reference type has to be a method ID*/
                            /*so skipping the first byte ... the REFERENCE_TAG 
                             and parsing the Method ID*/
                            DDL_PARSE_TAG(chunkp, size, &which, &len);

                            if (ddpREF.METHOD_ID_REF != which)
                            {
                                /*log an error!!!*/
                                /*myprintf(fout,"Invalid reference type in Enum Actions!!!!");*/
                            }

                            /*Parse the method ID!!*/

                            DDL_PARSE_INTEGER(chunkp, size, &LL); enmVal.actions = (uint)LL;
                            enmVal.evaled |= ENUM_VALUE.ENUM_ACTIONS_EVALED;
                        }
                        break;
                    case ENUM_VALUE.ENUM_DESC_TAG:
                        {
                            rc = ddl_parse_string(chunkp, size, ref enmVal.desc);
                            if (rc != DDL_SUCCESS)
                                return rc;
                            enmVal.evaled |= ENUM_VALUE.ENUM_DESC_EVALED;

                        }
                        break;
                    case ENUM_VALUE.ENUM_HELP_TAG:
                        {
                            rc = ddl_parse_string(chunkp, size, ref enmVal.help);
                            if (rc != DDL_SUCCESS)
                                return rc;
                            enmVal.evaled |= ENUM_VALUE.ENUM_HELP_EVALED;

                        }
                        break;
                    case ENUM_VALUE.ENUM_CLASS_TAG:
                        {
                            rc = ddl_parse_bitstring(chunkp, size, ref enmVal.func_class);
                            if (rc != DDL_SUCCESS)
                                return rc;

                            enmVal.evaled |= ENUM_VALUE.ENUM_CLASS_EVALED;
                        }
                        break;
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;

                }/*End switch tag*/

            }/*End while *size >0 */
            /*** these are unspeced string additions  stevev 01feb12
                if(!(enmVal.evaled & ENUM_DESC_EVALED))
                {
                    enmVal.desc.str = new char[18];
                    strcpy(enmVal.desc.str,"No Desc Available");
                    enmVal.desc.strType = DEV_SPEC_STRING_TAG;
                    enmVal.desc.flags   = FREE_STRING;
                    enmVal.desc.len = strlen(enmVal.desc.str);
                    enmVal.evaled |= ENUM_DESC_EVALED;
                }

                if(!(enmVal.evaled & ENUM_HELP_EVALED))
                {
                    enmVal.help.str = new char[18];
                    strcpy(enmVal.help.str,"No Help Available");
                    enmVal.help.strType = DEV_SPEC_STRING_TAG;
                    enmVal.help.flags   = FREE_STRING;
                    enmVal.help.len = strlen(enmVal.help.str);
                    enmVal.evaled |= ENUM_HELP_EVALED;
                }
            ******/
            return DDL_SUCCESS;
        }

        public unsafe static int parse_attr_member_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint Count, uint uiOffset)
        {

            int rc;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                uint* lengthp = &Count;
                uint tag, tagp, length, len, len1;

                DDlConditional tempPtrToCond = null;

                List<MEMBER> tempPtrToMemberList = null;

                MEMBER tmpMember = new MEMBER();

                VALUES tempVal = new VALUES();

                ddpExpression tempExpr = new ddpExpression();

                /*The first tag should be a MENU_ITEM_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (MEMBER_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *lengthp -= length;


                while (length > 0)
                {

                    /*Parse the Tag to know if we have a conditional or a direct object*/

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {

                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/

                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MEMBER_SEQLIST_TAG);

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/



                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MEMBER_SEQLIST_TAG);
                                    if (rc != SUCCESS)
                                        return rc; /* Return if not successful*/

                                    tempPtrToCond.byNumberOfSections++;

                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }
                                /*Now Parse the ddpExpression Argument of the SELECT */

                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);

                                    switch (tagp)
                                    {
                                        case CASE_TAG:
                                            {
                                                /*We are parsing the CASE constants as expression
                                                just bcoz of the spec. But it should be a constant 
                                                value , ie. an expression with just a  constant (integer)
                                                value*/

                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);

                                                if (rc != SUCCESS)
                                                    return rc;
                                                /*
                                                                                            ddpExpression :: iterator it;

                                                                                            it = tempExpr.begin();

                                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                                {
                                                    #ifdef _PARSER_DEBUG
                                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_member_list()!!!");
                                                                                                    return DDL_UNHANDLED_STUFF_FAILURE;
                                                    #endif												
                                                                                                }
                                                                                            temp = (long)it.elem.ulConst;
                                                */
                                                tempPtrToCond.caseVals.Add(tempExpr);

                                                /*We have the case constant value 
                                                Now parse the attributre value from the 
                                                following chunk	*/

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MEMBER_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/

                                                tempPtrToCond.byNumberOfSections++;

                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                /*
                                                                                            temp = DEFAULT_TAG_VALUE;
                                                                                            pAttr.pCond.caseVals.Add(temp);
                                                */
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                pAttr.pCond.caseVals.Add(tempExpr);

                                                tempPtrToCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, MEMBER_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/


                                }/*End while*/

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }


                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    pAttr.pVals = new VALUES();

                                    //pAttr.pVals.memberList = new MEMBER_LIST;

                                    //ADDED By Deepak
                                    ///(pAttr.pVals.memberList).Clear();
                                    //END

                                    rc = ddl_parse_members(chunkp, &len, ref pAttr.pVals.memberList);

                                    if (rc != DDL_SUCCESS)

                                        return rc;

                                    pAttr.byNumOfChunks++;
                                    break;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.memberList = new MEMBER_LIST;

                                    rc = ddl_parse_members(chunkp, &len, ref tempVal.memberList);

                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);//Vibhor 200105: Changed


                                    pAttr.byNumOfChunks++;


                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals, we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }
                                break;

                            }
                        /*End OBJECT_TAG*/
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/

                }/*End while*/


                if (/*pAttr.pVals == null
                    && */pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count() == 0)
                    && (pAttr.directVals.Count() > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    pAttr.pVals = new VALUES();
                    //pAttr.pVals.memberList = new MEMBER_LIST;
                    for (ushort i = 0; i < pAttr.directVals.Count(); i++)
                    {
                        tempPtrToMemberList = pAttr.directVals[i].memberList;
                        for (ushort j = 0; j < tempPtrToMemberList.Count(); j++)
                        {
                            tmpMember = tempPtrToMemberList[j];
                            pAttr.pVals.memberList.Add(tmpMember);

                        }/*Endfor j*/

                        /*Just Clear this list, its not required any more*/
                        ///tempPtrToMemberList.Clear();
                    }/*Endfor i*/

                    /*Now Clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                    pAttr.bIsAttributeConditional = false;

                /*Vibhor 180105: End of Code*/
            }
            return SUCCESS;

        }

        public unsafe static int parse_attr_method_type(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            //ASSERT_DBG(pAttr != null);

            pAttr.pVals = new VALUES();
            pAttr.pVals.methodType = new METHOD_PARAM();
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                return (parse_attr_param(ref pAttr.pVals.methodType, &chunkp1, ref size));
            }
        } /* end parse_attr_method_type*/

        public unsafe static int parse_attr_chart_type(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                uint tag, len;

                DDL_PARSE_TAG(&chunkp1, &size, &tag, &len);

                if (tag != CHART_TYPE_TAG)
                {
                    return DDL_ENCODING_ERROR;
                }

                /*Now its just an integer */
                pAttr.pVals = new VALUES();

                /*Simply parse the integer & return */
                UInt64 ulTemp;
                DDL_PARSE_INTEGER(&chunkp1, &size, &ulTemp);
                pAttr.pVals.ullVal = ulTemp;
            }

            return rc;


        }/*End parse_attr_chart_type*/

        public unsafe static int parse_attr_reference(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                uint* length = &size;
                uint tag, tagp, len;
                //	long temp;

                ddpExpression tempExpr = new ddpExpression();

                /*Parse the Tag to know if we have a conditional or a direct object*/

                DDL_PARSE_TAG(chunkp, length, &tag, &len);

                switch (tag)
                {

                    case IF_TAG: /*We have an IF THEN ELSE conditional*/
                        {
                            pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                            /*Now Parse the ddpExpression associated with the IF block */
                            rc = ddl_parse_expression(chunkp, length, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/



                            /*Parse the ELSE portion if there's one*/
                            if (*length > 0)
                            {
                                rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                pAttr.pCond.byNumberOfSections++;

                            }

                        }
                        break; /*End IF_TAG*/

                    case SELECT_TAG: /*We have a Switch Case conditional*/
                        {
                            pAttr.bIsAttributeConditional = true;
                            pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, length, ref (pAttr.pCond.expr));

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (*length > 0)
                            {
                                DDL_PARSE_TAG(chunkp, length, &tagp, &len);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, length, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;
                                            /*
                                                                                    ddpExpression :: iterator it;

                                                                                    it = tempExpr.begin();

                                                                                    if(it.byElemType != INTCST_OPCODE)
                                                                                        {
                                            #ifdef _PARSER_DEBUG
                                                            fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_reference()!!!");
                                                                                        return DDL_UNHANDLED_STUFF_FAILURE;
                                            #endif												
                                                                                        }
                                                                                    temp = (long)it.elem.ulConst;
                                            */
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pAttr.pCond.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*										
                                                                                    temp = DEFAULT_TAG_VALUE;
                                                                                    pAttr.pCond.caseVals.Add(temp);
                                            */
                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pAttr.pCond.caseVals.Add(tempExpr);

                                            pAttr.pCond.byNumberOfSections++;

                                            rc = ddl_parse_conditional(ref pAttr.pCond, chunkp, length);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/

                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/


                        }
                        break; /*End SELECT_TAG*/

                    case OBJECT_TAG: /*We have a direct object*/
                        {

                            pAttr.pVals = new VALUES();

                            pAttr.pVals.reff = new ddpREFERENCE();

                            rc = ddl_parse_ref(chunkp, length, ref pAttr.pVals.reff);

                            if (rc != DDL_SUCCESS)
                                return rc;

                        }
                        break; /*End OBJECT_TAG*/
                    default:
                        return DDL_ENCODING_ERROR;
                        //break;
                }/*End switch tag*/
            }
            return SUCCESS;

        }/*End parse_attr_reference*/


        public unsafe static int parse_attr_scope_size(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc = SUCCESS;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                uint tag, len;

                DDL_PARSE_TAG(&chunkp1, &size, &tag, &len);

                if (tag != SCOPE_SIZE_TAG)
                    return DDL_ENCODING_ERROR;

                /*Now its just an integer */
                pAttr.pVals = new VALUES();
                UInt64 ulTemp;
                /*Simply parse the integer & return */
                DDL_PARSE_INTEGER(&chunkp1, &size, &ulTemp);
                pAttr.pVals.ullVal = ulTemp;
            }

            return rc;

        }/*End parse_attr_scope_size*/

        public unsafe static int ddl_parse_enums(byte** chunkp, uint* size, ref List<ENUM_VALUE> pEnmList)
        {
            int rc = 0;
            uint len = 0, tag = 0; /*length & tag of the parsed binary*/

            //ENUM_VALUE tempEnm; /*Enum value to be parsed*/


            while (*size > 0)
            {

                ENUM_VALUE tempEnm = new ENUM_VALUE(); //was (void)memset((char *) &tempEnm, 0, sizeof(ENUM_VALUE));
                /*
                 * Parse the tag, and make sure it is an ENUMERATOR_TAG
                 */

                DDL_PARSE_TAG(chunkp, size, &tag, &len);

                if (tag != ENUMERATOR_TAG)
                {
                    return DDL_ENCODING_ERROR;
                }

                *size -= len;

                rc = ddl_parse_one_enum(chunkp, &len, ref tempEnm);

                if (rc != DDL_SUCCESS)
                    return rc;

                pEnmList.Add(tempEnm);

                ///tempEnm.Cleanup();

            }/*End while *size > 0*/

            return DDL_SUCCESS;

        }

        public unsafe static int ddl_parse_menuitems(byte** chunkp, uint* size, ref List<MENU_ITEM> pMenuItems)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint tmpQual = 0;

            //	MENU_ITEM tmpMenuItem;

            while (*size > 0)
            {
                /*Parse the Item Reference*/

                MENU_ITEM tmpMenuItem = new MENU_ITEM(); // was(void)memset((char*)&tmpMenuItem,0,sizeof(MENU_ITEM));

                rc = ddl_parse_ref(chunkp, size, ref tmpMenuItem.item);
                if (rc != DDL_SUCCESS)
                    return rc;


                /*Parse the Item Qualifier*/

                rc = ddl_parse_bitstring(chunkp, size, ref tmpQual);

                if (rc != DDL_SUCCESS)
                    return rc;

                tmpMenuItem.qual = (ushort)tmpQual;

                pMenuItems.Add(tmpMenuItem);

                //tmpMenuItem.Cleanup();

            }/*End while *size > 0*/

            return DDL_SUCCESS;

        }/*End ddl_parse_menuitems*/

        public unsafe static int ddl_parse_conditional_list(ref DDlConditional pConditional, byte** chunkp, uint* size, uint tagExpected)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint len = 0, len1 = 0, lenp = 0; /*Length of the data associated with the Tag*/
            uint tag = 0, tagp = 0;/* The "tag" */
            //	uint temp;

            byte* leave_pointer = null; /* end-of-chunk pointer*/
            //bool bNestedConditional = false;

            bool bCondSectInitialized = false; //Vibhor 200105: Added

            VALUES tempVal = new VALUES();

            //	ddpExpression tempExpr;

            DDlConditional pChild = null; /*This guy goes on ChildList of the conditional
								     or the ConditionalList of the Chunk*/

            DDlSectionChunks pSecChunks = null;//Vibhor 200105: Added//This guy is initialized only once in a single call

            //ASSERT_DBG(chunkp && *chunkp && size);


            DDL_PARSE_TAG(chunkp, size, &tag, &len);

            if (tag != tagExpected)
            {
                return DDL_ENCODING_ERROR;
            }

            *size -= len;

            while (len > 0)
            {
                /*
                 * Parse the tag to find out if this is a SELECT statement, an if/else,
                 * or a simple assignment.
                 */

                DDL_PARSE_TAG(chunkp, &len, &tag, &len1);

                /*
                 * Adjust size of remaining chunk.
                 */

                len -= len1;

                /*
                 * Calculate the return chunk pointer (we may be able to use it it later
                 * for an early exit).
                 */

                leave_pointer = *chunkp + len;

                /*See if we have something left over in our binary chunk, if yes, we have
                a multichunklist and so lets initialize our DDlSectionChunks ptr*/
                if (len > 0 && null == pSecChunks)
                {
                    pSecChunks = new DDlSectionChunks(pConditional.attrDataType);
                }

                switch (tag)
                {
                    case IF_TAG:
                        {

                            if (null == pSecChunks && len == 0) //double check
                            {
                                /* If we reach here it means that there's a pure nested conditional*/
                                pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);
                                pConditional.Vals.Add(tempVal); /* Push a null value on the value list*/

                            }
                            else if (null != pSecChunks)
                            {
                                /* We have a possible multichunkList for this section*/
                                if (bCondSectInitialized == false)
                                {
                                    pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CHUNKS);
                                    pConditional.Vals.Add(tempVal); /* Push a null value on the value list*/
                                    bCondSectInitialized = true;
                                }
                                pSecChunks.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);
                                pSecChunks.byNumOfChunks++;
                            }


                            pChild = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pConditional.attrDataType, 1);

                            /* Now Parse the ddpExpression associated with the IF block */

                            rc = ddl_parse_expression(chunkp, &len1, ref pChild.expr);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*otherwise Parse the value of the attribute associated with THEN clause*/

                            rc = ddl_parse_conditional_list(ref pChild, chunkp, &len1, tagExpected);

                            if (rc != SUCCESS)
                                return rc; /* Return if not successful*/

                            /*Parse the ELSE portion if there's one*/
                            if (len1 > 0)
                            {

                                rc = ddl_parse_conditional_list(ref pChild, chunkp, &len1, tagExpected);
                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/
                                pChild.byNumberOfSections++;

                            }

                            /*We got one IF..ELSE block, now push it either on Conditionals Child List
                             or on SectionChunk's Conditional List, as the case may be*/
                            if (null == pSecChunks)
                            {//this guy belongs to the conditional
                                pConditional.listOfChilds.Add(pChild);/*Push the child on the list*/
                            }
                            else
                            {//this guy is part of the chunk list
                                pSecChunks.conditionalVals.Add(pChild);
                            }
                        }
                        break; /*End case :IF_TAG*/

                    case SELECT_TAG:
                        {

                            if (null == pSecChunks && len == 0) //double check
                            {
                                pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);
                                /*This guy will come in each case, not in select*/
                                pConditional.Vals.Add(tempVal);  /* Push a null value on the value list*/
                            }
                            else if (null != pSecChunks)
                            {
                                if (bCondSectInitialized == false)
                                {
                                    pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CHUNKS);
                                    pConditional.Vals.Add(tempVal); /* Push a null value on the value list*/
                                    bCondSectInitialized = true;
                                }
                                pSecChunks.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);
                                pSecChunks.byNumOfChunks++;
                            }

                            pChild = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pConditional.attrDataType, 0);


                            /*Now Parse the ddpExpression Argument of the SELECT */

                            rc = ddl_parse_expression(chunkp, &len1, ref pChild.expr);

                            if (rc != SUCCESS)
                                return rc;

                            /*otherwise Parse all the CASE branches and the DEFAULT */
                            while (len1 > 0)
                            {
                                ddpExpression tempExpr = new ddpExpression();

                                DDL_PARSE_TAG(chunkp, &len1, &tagp, &lenp);

                                switch (tagp)
                                {
                                    case CASE_TAG:
                                        {
                                            /*We are parsing the CASE constants as expression
                                            just bcoz of the spec. But it should be a constant 
                                            value , ie. an expression with just a  constant (integer)
                                            value*/

                                            rc = ddl_parse_expression(chunkp, &len1, ref tempExpr);

                                            if (rc != SUCCESS)
                                                return rc;

                                            pChild.caseVals.Add(tempExpr);

                                            /*We have the case constant value 
                                            Now parse the attributre value from the 
                                            following chunk	*/

                                            rc = ddl_parse_conditional_list(ref pChild, chunkp, &len1, tagExpected);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/

                                            pChild.byNumberOfSections++;

                                            ///tempExpr.Clear();
                                        }
                                        break;/*End CASE_TAG*/

                                    case DEFAULT_TAG:
                                        {
                                            /*
                                                                            temp = DEFAULT_TAG_VALUE;
                                                                            pChild.caseVals.Add(temp);
                                            */

                                            tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                            pChild.caseVals.Add(tempExpr);

                                            pChild.byNumberOfSections++;

                                            rc = ddl_parse_conditional_list(ref pChild, chunkp, &len1, tagExpected);
                                            if (rc != SUCCESS)
                                                return rc; /* Return if not successful*/
                                        }
                                        break;/*End DEFAULT_TAG*/
                                    default:
                                        return DDL_ENCODING_ERROR;

                                }/*End Switch tagp*/


                            }/*End while*/



                            /*We got one SELECT block, now push it either on Conditionals Child List
                            or on SectionChunk's Conditional List, as the case may be*/
                            if (null == pSecChunks)
                            {//this guy belongs to the conditional
                                pConditional.listOfChilds.Add(pChild);/*Push the child on the list*/
                            }
                            else
                            {//this guy is part of the chunk list
                                pSecChunks.conditionalVals.Add(pChild);
                            }
                        }
                        break;

                    case OBJECT_TAG: /*We have a direct object, just parse it & return!!*/
                        {
                            if (null == pSecChunks && len == 0) //double check
                            {
                                pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);
                                /*We have SINGLE a chunk which has the desired "list" of values!!
                                  switch on attDataType and parse the appropriate value */
                            }
                            else if (null != pSecChunks)
                            { //We have a possible combination of directs and conditionals
                                if (bCondSectInitialized == false)
                                {
                                    pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CHUNKS);
                                    pConditional.Vals.Add(tempVal); /* Push a null value on the value list*/
                                    bCondSectInitialized = true;
                                }
                                pSecChunks.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);
                                pSecChunks.byNumOfChunks++;
                            }

                            VALUES tmpVal = new VALUES();

                            switch (pConditional.attrDataType)
                            {
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ENUM_LIST:
                                    {
                                        //tmpVal.enmList = new ENUM_VALUE_LIST;

                                        if (tmpVal.enmList != null) // HOMZ
                                        {
                                            rc = ddl_parse_enums(chunkp, &len1, ref tmpVal.enmList);
                                        }
                                        else
                                        {
                                            rc = DDL_MEMORY_ERROR;
                                        }

                                        if (rc != DDL_SUCCESS)
                                            return rc;
                                    }

                                    break;
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST:
                                    {

                                        //tmpVal.refList = new REFERENCE_LIST;

                                        rc = ddl_parse_reflist(chunkp, &len1, ref tmpVal.refList);

                                        if (rc != DDL_SUCCESS)
                                            return rc;


                                    }
                                    break;
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_TRANSACTION_LIST:
                                    {

                                    }
                                    break;
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_RESPONSE_CODE_LIST:
                                    {

                                        //tmpVal.respCdList = new RESPONSE_CODE_LIST;

                                        rc = ddl_parse_respcodes(chunkp, &len1, ref tmpVal.respCdList);

                                        if (rc != DDL_SUCCESS)
                                            return rc;

                                    }

                                    break;
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MENU_ITEM_LIST:
                                    {
                                        /*!!!! Ideally the control should not come here!!!! */


                                        //tmpVal.menuItemsList = new MENU_ITEM_LIST;

                                        rc = ddl_parse_menuitems(chunkp, &len1, ref tmpVal.menuItemsList);
                                        if (rc != DDL_SUCCESS)
                                            return rc;

                                    }
                                    break;
                                /*				case	DDL_ATTR_DATA_TYPE_REFRESH_RELATION:
                                                    break;
                                                case	DDL_ATTR_DATA_TYPE_UNIT_RELATION:
                                                    break; */
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ITEM_ARRAY_ELEMENT_LIST:
                                    {

                                        //tmpVal.itemArrElmnts = new ITEM_ARRAY_ELEMENT_LIST;

                                        rc = ddl_parse_itemarray(chunkp, &len1, ref tmpVal.itemArrElmnts);

                                        if (rc != DDL_SUCCESS)
                                            return rc;

                                    }
                                    break;
                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST:
                                    {

                                        //tmpVal.memberList = new MEMBER_LIST;

                                        rc = ddl_parse_members(chunkp, &len1, ref tmpVal.memberList);
                                        if (rc != DDL_SUCCESS)
                                            return rc;

                                    }
                                    break;

                                case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_GRID_SET:
                                    {

                                        //tmpVal.gridMemList = new GRID_SET_LIST;

                                        rc = ddl_parse_gridMembers(chunkp, &len1, ref tmpVal.gridMemList);
                                        if (rc != DDL_SUCCESS)
                                            return rc;

                                    }
                                    break;

                                default:
                                    /*it should not come here*/

                                    return DDL_ENCODING_ERROR;
                                    //break;

                            }/*end switch pConditional.attrDataType*/

                            if (null == pSecChunks)
                            {//this guy belongs to the conditional
                                pConditional.Vals.Add(tmpVal);/*Push the value on the direct vals list*/
                            }
                            else
                            {//this guy is part of the chunk list
                                pSecChunks.directVals.Add(tmpVal);
                            }

                        }
                        break;
                }/*End switch tag*/
            }/*End While*/

            /*See if we had a multichunk conditional section, just push it onto the conditional*/
            if (null != pSecChunks)
            {
                /*We may have parsed a direct list in multiple chunks !!
                 If that's the case just empty it out from the pSecChunk to the 
                 pConditional*/
                ushort i, j = 0;
                if (pSecChunks.conditionalVals.Count == 0)
                {
                    switch (pConditional.attrDataType)
                    {
                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ENUM_LIST:
                            {

                                //tempVal.enmList = new ENUM_VALUE_LIST;

                                List<ENUM_VALUE> pTmpEnmList;// = null;
                                ENUM_VALUE tmpEnm = new ENUM_VALUE();

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpEnmList = pSecChunks.directVals[i].enmList;
                                    for (j = 0; j < pTmpEnmList.Count; j++)
                                    {
                                        tmpEnm = pTmpEnmList[j];
                                        tempVal.enmList.Add(tmpEnm);
                                    }
                                }

                            }

                            break;
                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_REFERENCE_LIST:
                            {

                                //tempVal.refList = new REFERENCE_LIST;
                                List<ddpREFERENCE> pTmpRefList;// = null;
                                ddpREFERENCE tmpRef;

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpRefList = pSecChunks.directVals[i].refList;
                                    for (j = 0; j < pTmpRefList.Count; j++)
                                    {
                                        tmpRef = pTmpRefList[j];
                                        tempVal.refList.Add(tmpRef);
                                    }
                                }
                            }
                            break;
                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_RESPONSE_CODE_LIST:
                            {

                                //tempVal.respCdList = new RESPONSE_CODE_LIST;
                                List<RESPONSE_CODE> pTmpRspCdList;// = null;
                                RESPONSE_CODE tmpRsp;

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpRspCdList = pSecChunks.directVals[i].respCdList;
                                    for (j = 0; j < pTmpRspCdList.Count; j++)
                                    {
                                        tmpRsp = pTmpRspCdList[j];
                                        tempVal.respCdList.Add(tmpRsp);
                                    }
                                }

                            }
                            break;
                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MENU_ITEM_LIST:
                            {

                                //tempVal.menuItemsList = new MENU_ITEM_LIST;
                                List<MENU_ITEM> pTmpMenuItmLst;// = null;
                                MENU_ITEM tmpMnItm;

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpMenuItmLst = pSecChunks.directVals[i].menuItemsList;
                                    for (j = 0; j < pTmpMenuItmLst.Count; j++)
                                    {
                                        tmpMnItm = pTmpMenuItmLst[j];
                                        tempVal.menuItemsList.Add(tmpMnItm);
                                    }
                                }

                            }
                            break;

                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_ITEM_ARRAY_ELEMENT_LIST:
                            {

                                //tempVal.itemArrElmnts = new ITEM_ARRAY_ELEMENT_LIST;
                                List<ITEM_ARRAY_ELEMENT> pTmpIAElmLst;// = null;
                                ITEM_ARRAY_ELEMENT tmpElm;

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpIAElmLst = pSecChunks.directVals[i].itemArrElmnts;
                                    for (j = 0; j < pTmpIAElmLst.Count; j++)
                                    {
                                        tmpElm = pTmpIAElmLst[j];
                                        tempVal.itemArrElmnts.Add(tmpElm);
                                    }
                                }


                            }
                            break;
                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_MEMBER_LIST:
                            {

                                //tempVal.memberList = new MEMBER_LIST;
                                List<MEMBER> pTmpMemList;// = null;
                                MEMBER tmpMmbr;

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpMemList = pSecChunks.directVals[i].memberList;
                                    for (j = 0; j < pTmpMemList.Count; j++)
                                    {
                                        tmpMmbr = pTmpMemList[j];
                                        tempVal.memberList.Add(tmpMmbr);
                                    }
                                }


                            }
                            break;
                        /* stevev 25mar05 */
                        case DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_GRID_SET:
                            {

                                //tempVal.gridMemList = new GRID_SET_LIST;
                                List<GRID_SET> pTmpGridSetLst;// = null;
                                GRID_SET tmpGridSet;

                                for (i = 0; i < pSecChunks.directVals.Count; i++)
                                {
                                    pTmpGridSetLst = pSecChunks.directVals[i].gridMemList;
                                    for (j = 0; j < pTmpGridSetLst.Count; j++)
                                    {
                                        tmpGridSet = pTmpGridSetLst[i];
                                        tempVal.gridMemList.Add(tmpGridSet);
                                        ///tmpGridSet.Cleanup();

                                    }
                                }
                            }
                            break;
                        /* end stevev 25mar05 */
                        default:
                            /*it should not come here*/

                            return DDL_ENCODING_ERROR;
                            //break;

                    }/*end switch pConditional.attrDataType*/

                    pConditional.isSectionConditionalList.RemoveAt(pConditional.isSectionConditionalList.Count - 1);
                    pConditional.isSectionConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);
                    pConditional.Vals.RemoveAt(pConditional.Vals.Count - 1);
                    pConditional.Vals.Add(tempVal);

                    /*We don't need the pSecChunk any more, so delete it*/
                    // fluke 25jul14 - allow these to remain and let the destructor handle the memory
                    //pSecChunks.directVals.erase(pSecChunks.directVals.begin(),pSecChunks.directVals.end());
                    //pSecChunks.directVals.clear();
                    ///pSecChunks.isChunkConditionalList.Clear();

                }
                else
                {
                    pConditional.listOfChunks.Add(pSecChunks);
                }
            }/*Endif null != pSecChunks*/

            return SUCCESS;

        }/*End ddl_parse_conditional_list*/

        public static unsafe int ddl_parse_gridMembers(byte** chunkp, uint* size, ref List<GRID_SET> pGridSets)
        {
            int rc = 0;
            //uint tmpQual = 0;
            uint tag, len;
            uint tag1, len1;

            GRID_SET tmpGridSet = new GRID_SET();

            while (*size > 0)// a sequence of grid members
            {
                /* verify the tag */
                DDL_PARSE_TAG(chunkp, size, &tag, &len);

                if (tag != GRID_ELEMENT_TAG)
                    return DDL_ENCODING_ERROR;

                /* assume we will parse the whole thing */
                *size -= len;

                if (len > 0)// grid element contents
                {
                    /*Parse the String*/
                    rc = ddl_parse_string(chunkp, &len, ref tmpGridSet.desc);
                    if (rc != DDL_SUCCESS)
                        return rc;

                    DDL_PARSE_TAG(chunkp, &len, &tag1, &len1);

                    if (tag1 != GRID_MEMBERS_TAG)
                        return DDL_ENCODING_ERROR;

                    rc = ddl_parse_reflist(chunkp, &len1, ref tmpGridSet.values);
                    if (rc != DDL_SUCCESS)
                        return rc;
                }

                pGridSets.Add(tmpGridSet);
                // trashes the string ptrs:::tmpGridSet.Cleanup();
                /*
                REFERENCE_LIST::iterator it;

                ddpREFERENCE::iterator it1;

                for (it = tmpGridSet.values.begin(); it != tmpGridSet.values.end(); it++)
                {
                    for (it1 = (*it).begin(); it1 != (*it).end(); it1++)
                    {
                        (*it1).Cleanup();

                    }
                    (*it).clear();
                }
                */
                ///tmpGridSet.Cleanup();
                ///tmpGridSet.values.Clear();


            }/* wend members are left to process */
            return DDL_SUCCESS;

        }/*End ddl_parse_gridMembers*/

        public static unsafe int ddl_parse_members(byte** chunkp, uint* size, ref List<MEMBER> memberList)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint tag = 0, len = 0;//, tmp=0;
            UInt64 LL;

            //	MEMBER tmpMember;

            //ADDED By Deepak 
            if (null == memberList)
                return DDL_ENCODING_ERROR;

            //ASSERT_DBG(chunkp && *chunkp && *size);

            while (*size > 0)
            {
                /*Parse the tag , it should be MEMBER_TAG*/

                DDL_PARSE_TAG(chunkp, size, &tag, &len);

                if (MEMBER_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *size -= len;

                MEMBER tmpMember = new MEMBER(); // was (void)memset((char*)&tmpMember,0,sizeof(MEMBER));

                /* Parse the name*/
                DDL_PARSE_INTEGER(chunkp, &len, &LL);

                tmpMember.name = (uint)LL;

                /*Parse the item*/

                rc = ddl_parse_ref(chunkp, &len, ref tmpMember.item);
                if (rc != DDL_SUCCESS)
                    return rc;

                tmpMember.evaled |= MEM_NAME_EVALED | MEM_REF_EVALED;

                if (len > 0) /*If description & help is there*/
                {
                    if (**chunkp == 57)
                    {// member name - do before optional desc&help

                        rc = parse_ascii_string(ref tmpMember.member_name, chunkp, ref len);
                        if (rc != DDL_SUCCESS)
                            return rc;
                    }
                    if (len > 0) /*If description & help is there*/
                    {
                        /*Parse description*/
                        rc = ddl_parse_string(chunkp, &len, ref tmpMember.desc);
                        if (rc != DDL_SUCCESS)
                            return rc;

                        tmpMember.evaled |= MEM_DESC_EVALED;
                        if (len > 0)
                        {
                            rc = ddl_parse_string(chunkp, &len, ref tmpMember.help);
                            if (rc != DDL_SUCCESS)
                                return rc;
                            tmpMember.evaled |= MEM_HELP_EVALED;
                        }/*Endif len nested*/
                    }//endif - len after member name

                }/*Endif len*/

                memberList.Add(tmpMember);

            }/*End while *size > 0*/

            return DDL_SUCCESS;

        }/*End ddl_parse_members*/

        public static unsafe int ddl_parse_itemarray(byte** chunkp, uint* size, ref List<ITEM_ARRAY_ELEMENT> itemArray)
        {
            //ADDED By Deepak initialize vars
            int rc = 0;
            uint tag = 0, len = 0;
            UInt64 LL;

            //	ITEM_ARRAY_ELEMENT tmpElement;

            //ASSERT_DBG(chunkp && *chunkp && *size);

            while (*size > 0)
            {
                /*Parse the tag , it should be ITEM_ARRAY_ELEMENT_TAG*/

                DDL_PARSE_TAG(chunkp, size, &tag, &len);

                if (ITEM_ARRAY_ELEMENT_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *size -= len;

                ITEM_ARRAY_ELEMENT tmpElement = new ITEM_ARRAY_ELEMENT(); //was (void)memset((char*)&tmpElement,0,sizeof(ITEM_ARRAY_ELEMENT));

                /* Parse the index*/
                DDL_PARSE_INTEGER(chunkp, &len, &LL);

                tmpElement.index = (ushort)LL;

                /*Parse the item*/

                rc = ddl_parse_ref(chunkp, &len, ref tmpElement.item);
                if (rc != DDL_SUCCESS)
                    return rc;

                tmpElement.evaled |= IA_INDEX_EVALED | IA_REF_EVALED;

                if (len > 0) /*If description & help is there*/
                {
                    /*Parse description*/
                    rc = ddl_parse_string(chunkp, &len, ref tmpElement.desc);
                    if (rc != DDL_SUCCESS)
                        return rc;

                    tmpElement.evaled |= IA_DESC_EVALED;
                    if (len > 0)
                    {
                        rc = ddl_parse_string(chunkp, &len, ref tmpElement.help);
                        if (rc != DDL_SUCCESS)
                            return rc;
                        tmpElement.evaled |= IA_HELP_EVALED;
                    }/*Endif len nested*/

                }/*Endif len*/

                itemArray.Add(tmpElement);

            }/*End while *size > 0*/

            return DDL_SUCCESS;

        }/*End ddl_parse_itemarray*/

        public static unsafe int parse_attr_resp_code_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {

            int rc;
            fixed (byte* chunkp1 = &binChunk[uiOffset])
            {
                byte** chunkp = &chunkp1;
                uint* lengthp = &size;
                uint tag, tagp, length, len, len1;
                //	long temp;

                DDlConditional tempPtrToCond = null;

                List<RESPONSE_CODE> tempPtrToRespCodeList;

                RESPONSE_CODE tmpRespCode;

                VALUES tempVal = new VALUES();

                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);

                /*The first tag should be a RSPCODES_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (RSPCODES_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;

                //*lengthp -= length;// never used, apparently for debugging

                while (length > 0)
                {
                    /*Parse the Tag to know if we have a conditional or a direct object*/

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {
                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref tempPtrToCond.expr);

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/

                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, RSPCODES_SEQLIST_TAG);

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, RSPCODES_SEQLIST_TAG);
                                    if (rc != SUCCESS)
                                        return rc; /* Return if not successful*/

                                    tempPtrToCond.byNumberOfSections++;

                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression Argument of the SELECT */

                                rc = ddl_parse_expression(chunkp, &len, ref tempPtrToCond.expr);

                                if (rc != SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    //sjv 18apr06								DDL_PARSE_TAG(chunkp,&length,&tagp,&len1);
                                    DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);
                                    //sjv 29jun06- using len in the parses within this while loop has nothing to do with Implicit
                                    //			   it is equivelent to doing a 'len -= len1;' here, but doing it a little at a time
                                    switch (tagp)
                                    {
                                        case CASE_TAG:
                                            {
                                                /*We are parsing the CASE constants as expression
                                                just bcoz of the spec. But it should be a constant 
                                                value , ie. an expression with just a  constant (integer)
                                                value*/

                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);

                                                if (rc != SUCCESS)
                                                    return rc;
                                                /*
                                                                            ddpExpression :: iterator it;

                                                                            it = tempExpr.begin();

                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                {
                                                #ifdef _PARSER_DEBUG
                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_resp_code_list()!!!");
                                                                                return DDL_UNHANDLED_STUFF_FAILURE;
                                                #endif												
                                                                                }
                                                                            temp = (long)it.elem.ulConst;
                                                */
                                                tempPtrToCond.caseVals.Add(tempExpr);

                                                /*We have the case constant value 
                                                Now parse the attributre value from the 
                                                following chunk	*/

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, RSPCODES_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/

                                                tempPtrToCond.byNumberOfSections++;

                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                /*
                                                                            temp = DEFAULT_TAG_VALUE;
                                                                            pAttr.pCond.caseVals.Add(temp);
                                                */
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                pAttr.pCond.caseVals.Add(tempExpr);

                                                tempPtrToCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, RSPCODES_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/


                                }/*End while*/

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    pAttr.pVals = new VALUES();//??????

                                    //pAttr.pVals.respCdList; = new RESPONSE_CODE_LIST;

                                    rc = ddl_parse_respcodes(chunkp, &len, ref pAttr.pVals.respCdList);

                                    if (rc != DDL_SUCCESS)

                                        return rc;

                                    pAttr.byNumOfChunks++;
                                    break;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.respCdList = new RESPONSE_CODE_LIST;

                                    rc = ddl_parse_respcodes(chunkp, &len, ref tempVal.respCdList);

                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);//Vibhor 200105: Changed

                                    pAttr.byNumOfChunks++;

                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals,
                                    we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }
                                break;

                            }
                        /*End OBJECT_TAG*/
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/

                }/*End while*/

                if ((object)pAttr.pVals == null
                    && pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    pAttr.pVals = new VALUES();
                    //pAttr.pVals.respCdList = new RESPONSE_CODE_LIST;

                    for (ushort i = 0; i < pAttr.directVals.Count; i++)
                    {
                        tempPtrToRespCodeList = pAttr.directVals[i].respCdList;
                        for (ushort j = 0; j < tempPtrToRespCodeList.Count; j++)
                        {
                            tmpRespCode = tempPtrToRespCodeList[j];
                            pAttr.pVals.respCdList.Add(tmpRespCode);

                        }/*Endfor j*/

                        /*Just clear this list, its not required any more*/
                        ///tempPtrToRespCodeList.Clear();
                    }/*Endfor i*/

                    /*Now clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                    pAttr.bIsAttributeConditional = false;

                /*Vibhor 180105: End of Code*/
            }

            return SUCCESS;

        }/*End parse_attr_resp_code_list*/

        public static unsafe int _preFetchItem(byte maskSize, ref byte[] pObjExtn, /*INT*/ref int rSize, ref uint _attrMask, ref uint pbyLocalAttrOffset)
        {
            int retVal = 0;// success
                           //fixed(ITEM_EXTN* pItmExtn = pObjExtn);
                           //ITEM_EXTN pItmExtn = (ITEM_EXTN*)pObjExtn;
            ITEM_EXTN pItmExtn = (ITEM_EXTN)Common.BytesToStuct(pObjExtn, typeof(Common.ITEM_EXTN));

            _attrMask = 0;

            uint uiExtLength = pItmExtn.byLength;

            //(*pObjExtn) += sizeof(ITEM_EXTN); /*Point to the masks skip>len,type,subtype,id<*/

            for (int j = 0; j < maskSize; j++)
            {
                //_attrMask = (_attrMask << 8) | (uint)*((*pObjExtn)++);
                _attrMask = (_attrMask << 8) | (uint)pObjExtn[Marshal.SizeOf(pItmExtn) + j];
            }

            pbyLocalAttrOffset = (uint)(Marshal.SizeOf(pItmExtn) + maskSize);

            rSize = (int)uiExtLength - Marshal.SizeOf(pItmExtn) + EXTEN_LENGTH_SIZE - maskSize;
            /*If no attributes or the mask value is zero,means the DD is corrupt!! */
            if (rSize <= 0 || _attrMask == 0)
                return (DDL_ENCODING_ERROR);

            return retVal;
        }

        public static object BytesToStuct(byte[] bytes, Type type)
        {
            //得到结构体的大小
            int size = System.Runtime.InteropServices.Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = System.Runtime.InteropServices.Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            System.Runtime.InteropServices.Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }

        public static unsafe int ddl_parse_integer_func(byte** chunkp, uint* size, UInt64* value)
        {

            uint cnt = 0;  /* temp ptr to size of binary */
            UInt64 val = 0; /* temp storage for parsed integer */
            byte* chunk = null;    /* temp ptr to binary */
            byte c = 0;        /* temp ptr to binary */
            int more_indicator = 0; /* need to parse another byte */

            //ASSERT_DBG(chunkp && *chunkp && size);

            /*
            * Read each character, building the uint until the high order bit is
            * not set
            */

            val = 0;
            chunk = *chunkp;
            cnt = *size;

            do
            {
                if (cnt == 0)
                {
                    return DDL_INSUFFICIENT_OCTETS;
                }

                c = *chunk++;
                more_indicator = c & 0x80;
                c &= 0x7f;

                if (val > (DDL_UInt64_MAX >> 7))
                {
                    return DDL_LARGE_VALUE;
                }

                val <<= 7;
                if (val > DDL_UInt64_MAX - c)
                {
                    return DDL_LARGE_VALUE;
                }

                val |= c;
                --cnt;
            } while (more_indicator != 0);

            /*
            * Update the pointer and size, and return the value
            */

            *size = cnt;
            *chunkp = chunk;

            if (value != null)
            {
                *value = val;
            }

            return DDL_SUCCESS;

            //ADDED By Deepak Initialize all vars


        }/*End ddl_parse_integer_func*/

        public static unsafe int parse_attr_reference_list(ref DDlAttribute pAttr, ref byte[] binChunk, uint size, uint uiOffset)
        {
            int rc;
            //byte** chunkp = null;
            fixed (byte* chu = &binChunk[uiOffset])
            {

                byte** chunkp = &chu;
                uint* lengthp = null;
                uint tag, tagp, length, len, len1;
                //	long temp;

                DDlConditional tempPtrToCond = null;

                List<ddpREFERENCE> tempPtrToReferenceList;// = null;

                ddpREFERENCE tmpRef = new ddpREFERENCE();

                VALUES tempVal = new VALUES();

                ddpExpression tempExpr = new ddpExpression();

                //ASSERT_DBG(binChunk && size);

                lengthp = &size;

                /*The first tag should be a REFERENCE_SEQLIST_TAG if not then return error*/

                DDL_PARSE_TAG(chunkp, lengthp, &tag, &length);

                if (REFERENCE_SEQLIST_TAG != tag)
                    return DDL_ENCODING_ERROR;

                *lengthp -= length;


                while (length > 0)
                {
                    /*Parse the Tag to know if we have a conditional or a direct object*/

                    DDL_PARSE_TAG(chunkp, &length, &tag, &len);

                    length -= len;

                    switch (tag)
                    {

                        case IF_TAG: /*We have an IF THEN ELSE conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_IF, pAttr.attrDataType, 1);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression associated with the IF block */
                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/

                                /*otherwise Parse the value of the attribute associated with THEN clause*/

                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, REFERENCE_SEQLIST_TAG);

                                if (rc != SUCCESS)
                                    return rc; /* Return if not successful*/



                                /*Parse the ELSE portion if there's one*/
                                if (len > 0)
                                {
                                    rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, REFERENCE_SEQLIST_TAG);
                                    if (rc != SUCCESS)
                                        return rc; /* Return if not successful*/

                                    tempPtrToCond.byNumberOfSections++;

                                }

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End IF_TAG*/

                        case SELECT_TAG: /*We have a Switch Case conditional*/
                            {
                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                {/*We have a conditional in single chunk*/

                                    pAttr.bIsAttributeConditional = true; /*This guy is a conditional*/
                                    pAttr.pCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);
                                    tempPtrToCond = pAttr.pCond;
                                    pAttr.byNumOfChunks++;
                                }
                                else
                                {/*We have a multichunk list which has conditionals*/
                                    tempPtrToCond = new DDlConditional(DDL_COND_TYPE.DDL_COND_TYPE_SELECT, pAttr.attrDataType, 0);

                                    pAttr.bIsAttributeConditionalList = true;
                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_CONDNL);//Vibhor 200105: Changed
                                    pAttr.byNumOfChunks++;
                                }

                                /*Now Parse the ddpExpression Argument of the SELECT */

                                rc = ddl_parse_expression(chunkp, &len, ref (tempPtrToCond.expr));

                                if (rc != SUCCESS)
                                    return rc;

                                /*otherwise Parse all the CASE branches and the DEFAULT */
                                while (len > 0)
                                {
                                    DDL_PARSE_TAG(chunkp, &len, &tagp, &len1);

                                    switch (tagp)
                                    {
                                        case CASE_TAG:
                                            {
                                                /*We are parsing the CASE constants as expression
                                                just bcoz of the spec. But it should be a constant 
                                                value , ie. an expression with just a  constant (integer)
                                                value*/

                                                rc = ddl_parse_expression(chunkp, &len, ref tempExpr);

                                                if (rc != SUCCESS)
                                                    return rc;
                                                /*
                                                                                            ddpExpression :: iterator it;

                                                                                            it = tempExpr.begin();

                                                                                            if(it.byElemType != INTCST_OPCODE)
                                                                                                {
                                                    #ifdef _PARSER_DEBUG
                                                                    fprintf(ferr,"\n ddpExpression encountered in case tag in parse_attr_reference_list()!!!");
                                                                                                    return DDL_UNHANDLED_STUFF_FAILURE;
                                                    #endif									
                                                                                                }
                                                                                            temp = (long)it.elem.ulConst;
                                                */
                                                tempPtrToCond.caseVals.Add(tempExpr);

                                                /*We have the case constant value 
                                                Now parse the attributre value from the 
                                                following chunk	*/

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, REFERENCE_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/

                                                tempPtrToCond.byNumberOfSections++;

                                                ///tempExpr.Clear();
                                            }
                                            break;/*End CASE_TAG*/

                                        case DEFAULT_TAG:
                                            {
                                                /*
                                                                                            temp = DEFAULT_TAG_VALUE;
                                                                                            pAttr.pCond.caseVals.Add(temp);
                                                */
                                                tempExpr.Clear();// use an empty expression to indicate DEFAULT
                                                pAttr.pCond.caseVals.Add(tempExpr);

                                                pAttr.pCond.byNumberOfSections++;

                                                rc = ddl_parse_conditional_list(ref tempPtrToCond, chunkp, &len, REFERENCE_SEQLIST_TAG);
                                                if (rc != SUCCESS)
                                                    return rc; /* Return if not successful*/
                                            }
                                            break;/*End DEFAULT_TAG*/

                                        default:
                                            return DDL_ENCODING_ERROR;

                                    }/*End Switch tagp*/


                                }/*End while*/

                                if (pAttr.bIsAttributeConditionalList == true)
                                {/*then we have to push this conditional value on conditionalVals*/

                                    pAttr.conditionalVals.Add(tempPtrToCond);
                                }

                            }
                            break; /*End SELECT_TAG*/

                        case OBJECT_TAG: /*We have a direct object*/
                            {

                                if ((pAttr.byNumOfChunks == 0) && length == 0)
                                { /*We have a direct list in a single chunk*/
                                    pAttr.pVals = new VALUES();

                                    pAttr.pVals.refList = new List<ddpREFERENCE>();

                                    rc = ddl_parse_reflist(chunkp, &len, ref pAttr.pVals.refList);

                                    if (rc != DDL_SUCCESS)

                                        return rc;

                                    pAttr.byNumOfChunks++;
                                    break;

                                }
                                else /*We are having a  possible combination of direct & conditional chunks*/
                                { /*Spl case of all chunks having direct Values , we'll handle after looping */

                                    //tempVal.refList = new REFERENCE_LIST;

                                    rc = ddl_parse_reflist(chunkp, &len, ref tempVal.refList);

                                    if (rc != DDL_SUCCESS)
                                        return rc;

                                    pAttr.directVals.Add(tempVal);

                                    pAttr.isChunkConditionalList.Add(DDL_COND_SECTION_TYPE.DDL_SECT_TYPE_DIRECT);//Vibhor 200105: Changed


                                    pAttr.byNumOfChunks++;


                                    /*Just set the conditional flag every time we come here irrespective of 
                                     whether or not its set earlier, If we have chunks of non conditionals,
                                    we will reset this flag later*/
                                    pAttr.bIsAttributeConditional = true;

                                }
                                break;

                            }
                        /*End OBJECT_TAG*/
                        default:
                            return DDL_ENCODING_ERROR;
                            //break;
                    }/*End switch tag*/
                }/*End while*/

                if ((object)pAttr.pVals == null
                    && pAttr.bIsAttributeConditionalList == false
                    && (pAttr.conditionalVals.Count == 0)
                    && (pAttr.directVals.Count > 0)) /*The last one is a double check*/
                {
                    /*We have a Direct list in more than one chunk!!!!
                     So we will copy the same to pAttr.Vals.enmList*/
                    //??????pAttr.pVals = new VALUES();
                    //pAttr.pVals.refList = new REFERENCE_LIST;
                    for (ushort i = 0; i < pAttr.directVals.Count; i++)
                    {
                        tempPtrToReferenceList = pAttr.directVals[i].refList;
                        for (ushort j = 0; j < tempPtrToReferenceList.Count; j++)
                        {
                            tmpRef = tempPtrToReferenceList[j];
                            pAttr.pVals.refList.Add(tmpRef);

                        }/*Endfor j*/

                        /*Just clear this list, its not required any more*/
                        ///tempPtrToReferenceList.Clear();
                    }/*Endfor i*/

                    /*Now clear the directVals list too*/

                    pAttr.directVals.Clear();

                    /*Reset the bIsAttributeConditional flag*/
                    pAttr.bIsAttributeConditional = false;

                }/*Endif*/

                /*Vibhor 180105: Start of Code*/

                /*If due to some combination both Conditional & ConditionalList Flags are set, 
                 Reset the bIsAttributeConditional */
                if (pAttr.bIsAttributeConditional == true && pAttr.bIsAttributeConditionalList == true)
                    pAttr.bIsAttributeConditional = false;

                /*Vibhor 180105: End of Code*/
            }
            return SUCCESS;

        }/*End parse_attr_reference_list*/


        public static int get_item_name(uint ui, ref string item_name, string symFilePath)
        {
            StreamReader br;


            string buffer;
            string iname;
            uint id = 0;

            item_name = "no_symbol_name";
            br = new StreamReader(new FileStream(symFilePath, FileMode.Open));
            if (br != null)
            {
                while ((buffer = br.ReadLine()) != null)
                {
                    List<string> item = new List<string>();

                    string[] sArray = buffer.Split(' ');

                    for (int i = 0; i < sArray.Length; i++)
                    {
                        if (sArray[i] != "")
                        {
                            item.Add(sArray[i]);
                        }
                    }

                    iname = item[1];

                    if (Regex.IsMatch(item[item.Count - 1], @"^[+-]?\d*[.]?\d*$"))
                    {
                        id = Convert.ToUInt16(item[item.Count - 1], 10);
                    }

                    else if (Regex.IsMatch(item[item.Count - 2], @"^[+-]?\d*[.]?\d*$"))
                    {
                        id = Convert.ToUInt16(item[item.Count - 2], 10);
                    }

                    if (id == ui)
                    {
                        item_name = iname;
                        br.Close();
                        return 0;
                    }

                }
                br.Close();
                return 1;//item not found

            }
            else
                return -1;//file open error

        }

        public static ITEM_EXTN byteToStructure<ITEM_EXTN>(byte[] dataBuffer)
        {
            object structure = null;
            int size = Marshal.SizeOf(typeof(ITEM_EXTN));
            IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(ITEM_EXTN));
            }
            finally
            {
                Marshal.FreeHGlobal(allocIntPtr);
            }
            return (ITEM_EXTN)structure;
        }

        public static int _preFetchItem(byte maskSize, byte[][] pObjExtn, /*INT*/ ref int rSize, ref uint _attrMask, int iOffset)
        {
            int retVal = 0;// success

            ITEM_EXTN pItmExtn;
            pItmExtn = byteToStructure<ITEM_EXTN>(pObjExtn[iOffset]);
            _attrMask = 0;


            uint uiExtLength = (uint)pItmExtn.byLength;

            //(*pObjExtn) += sizeof(ITEM_EXTN); /*Point to the masks skip>len,type,subtype,id<*/

            for (int j = 0; j < maskSize; j++)
            {
                //_attrMask = (_attrMask << 8) | (uint)*((*pObjExtn)++);
                _attrMask = (_attrMask << 8) | (uint)(pObjExtn[iOffset + 1][j]);
            }

            rSize = (int)uiExtLength - Marshal.SizeOf(typeof(ITEM_EXTN)) + EXTEN_LENGTH_SIZE - maskSize;
            /*If no attributes or the mask value is zero,means the DD is corrupt!! */
            if (rSize <= 0 || _attrMask == 0)
            {
                return (DDL_ENCODING_ERROR);
            }

            return retVal;
        }
    }

    public class Endian
    {
        public const int FORMAT_BIG_ENDIAN = 1;

        const int FORMAT_LITTLE_ENDIAN = 2;
        /*
        public static bool write_word(void* dest, WORD* source, int format)
        {
            ;
        }
        public static bool write_dword(void* dest, DWORD* source, int format)
        {
            ;
        }
        public static bool write_float(void* dest, FLOAT* source, int format)
        {
            ;
        }
        */
        //tmp = (short)((source & 0xff) << 8 + (source >> 8) & 0xff);
        public static bool read_word(ref ushort dest, ushort source, int format)
        {
            //ADDED By Deepak initializing the variables
            byte[] source_ptr = new byte[2];
            ushort tmp = 0;

            switch (format)
            {
                case FORMAT_BIG_ENDIAN:
                    source_ptr[0] = (byte)(source & 0xff);
                    source_ptr[1] = (byte)((source >> 8) & 0xff);
                    tmp = (ushort)(source_ptr[0] * 256 + source_ptr[1]);
                    break;

                case FORMAT_LITTLE_ENDIAN:
                    source_ptr[1] = (byte)(source & 0xff);
                    source_ptr[0] = (byte)((source >> 8) & 0xff);
                    tmp = (ushort)(source_ptr[0] * 256 + source_ptr[1]);
                    break;

                default:
                    return false;
            }

            dest = tmp;

            return true;
        }

        public static bool read_dword(ref uint dest, uint source, int format)
        {
            byte[] source_ptr = new byte[4];
            //uint tmp = 0;

            switch (format)
            {
                case FORMAT_BIG_ENDIAN:
                    source_ptr[0] = (byte)(source & 0xff);
                    source_ptr[1] = (byte)((source >> 8) & 0xff);
                    source_ptr[2] = (byte)((source >> 16) & 0xff);
                    source_ptr[3] = (byte)((source >> 24) & 0xff);
                    break;

                case FORMAT_LITTLE_ENDIAN:
                    source_ptr[3] = (byte)(source & 0xff);
                    source_ptr[2] = (byte)((source >> 8) & 0xff);
                    source_ptr[1] = (byte)((source >> 16) & 0xff);
                    source_ptr[0] = (byte)((source >> 24) & 0xff);
                    break;

                default:
                    return false;
            }

            uint t0 = ((uint)source_ptr[0]) << 24;
            uint t1 = ((uint)source_ptr[1]) << 16;
            uint t2 = ((uint)source_ptr[2]) << 8;
            uint t3 = (uint)source_ptr[3];

            //tmp = (uint)(source_ptr[0] >> 24 + source_ptr[1] >> 16 + source_ptr[2] >> 16 + source_ptr[3]);
            dest = t0 + t1 + t2 + t3;

            return true;
        }

        public static bool read_dword(ref uint dest, byte[] source, int format)
        {
            byte[] source_ptr = new byte[4];

            switch (format)
            {
                case FORMAT_BIG_ENDIAN:
                    source_ptr[0] = (byte)(source[0]);
                    source_ptr[1] = (byte)(source[1]);
                    source_ptr[2] = (byte)(source[2]);
                    source_ptr[3] = (byte)(source[3]);
                    break;

                case FORMAT_LITTLE_ENDIAN:
                    source_ptr[3] = (byte)(source[0]);
                    source_ptr[2] = (byte)((source[1]) & 0xff);
                    source_ptr[1] = (byte)((source[2]) & 0xff);
                    source_ptr[0] = (byte)((source[3]) & 0xff);
                    break;

                default:
                    return false;
            }

            uint t0 = ((uint)source_ptr[0]) << 24;
            uint t1 = ((uint)source_ptr[1]) << 16;
            uint t2 = ((uint)source_ptr[2]) << 8;
            uint t3 = (uint)source_ptr[3];

            //tmp = (uint)(source_ptr[0] >> 24 + source_ptr[1] >> 16 + source_ptr[2] >> 16 + source_ptr[3]);
            dest = t0 + t1 + t2 + t3;

            return true;
        }

        public static bool read_dword_spl(ref uint dest, byte[] source_ptr, int size, int format)
        {
            int i;
            uint tmp = 0;

            switch (format)
            {
                case FORMAT_BIG_ENDIAN:
                    for (i = 0; i < size; i++)
                    {
                        tmp <<= 8;
                        tmp |= source_ptr[i];
                    }
                    break;

                case FORMAT_LITTLE_ENDIAN:
                    for (i = size - 1; i >= 0; i--)
                    {
                        tmp <<= 8;
                        tmp |= source_ptr[i];
                    }
                    break;

                default:
                    return false;
            }

            dest = tmp;

            return true;
        }
        /*
        public static bool read_float(FLOAT* dest, void* source, int format)
        {
            ;
        }*/

        /***************快速排序功能****************/
        public delegate int QSortCompareFunction<T>(T a, T b);
        public static void QSort<T>(T[] array, uint len, QSortCompareFunction<T> compareFunc)
        {
            //排序算法： 插入排序
            for (int i = 1; i < len; i++)
            {
                T t = array[i];
                int j = i;
                while ((j > 0) && compareFunc(array[j - 1], t) > 0)
                {
                    array[j] = array[j - 1];
                    --j;
                }
                array[j] = t;
            }
        }
        /************快速排序功能结束***************/

    }

}