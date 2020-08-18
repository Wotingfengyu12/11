using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace FieldIot.HARTDD
{
    public class EnumTriad_t
    {
        public uint val;
        public string descS;
        public string helpS;
        public ddbENUM_REF enumStr; /*reference to an enumerated varible*/
    }

    public class EnumList : List<EnumTriad_t>
    {
        public uint maxDescLen()
        {
            uint len = 0;
            foreach (EnumTriad_t ent in this)
            {
                if (len < ent.descS.Length)
                {
                    len = (uint)ent.descS.Length;
                }
            }
            return len;
        }
    }

    public enum actionType_t
    {
        eT_actionInit = 0,
        eT_actionRefresh,
        eT_actionExit
    }

    public enum updatePermission_t
    {
        up_DONOT_UPDATE,    // 0
        up_UPDATE_STD_DYN = 1,  // eg in DISPLAY builtin, do all %{dyn} && %#ofDyn
        up_UPDATE_SPEC_DYN = 2      // update special dynamics - all %#ofDyn
    }

    public enum devMode_t
    {
        dm_Standard,
        dm_275compatible
        /* more later */
    }

    public enum VARIANT_TYPE
    {
        RUL_NULL = 0,
        /************numeric types********/
        RUL_BOOL,
        RUL_CHAR,
        RUL_UNSIGNED_CHAR,
        RUL_INT,
        RUL_UINT,
        RUL_SHORT,
        RUL_USHORT, /* 16 bit char */
        RUL_LONGLONG,
        RUL_ULONGLONG,
        RUL_FLOAT,
        RUL_DOUBLE,
        /**************** string types ****/
        RUL_CHARPTR,
        RUL_WIDECHARPTR,/* aka ushortPTR */
        RUL_BYTE_STRING,
        RUL_DD_STRING,
        RUL_SAFEARRAY
    }

    public struct _BYTE_STRING
    {
        public byte[] bs;
        public int bsLen;
        //struct byteString_s() {bs = 0;bsLen = 0;};
        //struct byteString_s(_UINT32 Len) {if (Len){bs = new byte[Len];bsLen=Len;}\
        //	else{bs = 0;bsLen = 0;}    };
        //~byteString_s() {if ( bs ) { delete[] bs;bsLen = 0;} };
    }

    public struct __VAL
    {
        public bool bValue;
        public byte cValue;
        public byte ucValue;
        public short sValue;
        public ushort usValue;
        public int nValue
        {
            get
            {
                return (int)lValue;
            }
            set 
            {
                lValue = value;
            }
        }
        public uint unValue
        {
            get
            {
                return (uint)lValue;
            }
            set
            {
                lValue = value;
            }
        }
        public UInt64 ulValue
        {
            get
            {
                return (UInt64)lValue;
            }
            set
            {
                lValue = (Int64)value;
            }
        }
        public Int64 lValue;
        public float fValue
        {
            get
            {
                return (float)dValue;
            }
            set
            {
                dValue = value;
            }

        }
        public double dValue;

        public string pszValue;  // both wide and dd_string
        public string pzcVal;
        public INTER_SAFEARRAY prgsa;
        public _BYTE_STRING bString;// int bsLen; and byte* bs;	
    }

    public enum BUILTIN_NAME
    {
        BUILTIN_delay = 0,
        BUILTIN_DELAY = 1,
        BUILTIN_DELAY_TIME = 2,
        BUILTIN_BUILD_MESSAGE = 3,
        BUILTIN_PUT_MESSAGE,
        BUILTIN_put_message,
        BUILTIN_ACKNOWLEDGE,
        BUILTIN_acknowledge,
        BUILTIN__get_dev_var_value,
        BUILTIN__get_local_var_value,
        BUILTIN__display_xmtr_status,
        BUILTIN_display_response_status,
        BUILTIN_display,
        BUILTIN_SELECT_FROM_LIST,
        BUILTIN_select_from_list,
        BUILTIN__vassign,
        BUILTIN__dassign,
        BUILTIN__fassign,
        BUILTIN__lassign,
        BUILTIN__iassign,
        BUILTIN__fvar_value,
        BUILTIN__ivar_value,
        BUILTIN__lvar_value,
        BUILTIN_svar_value,
        BUILTIN_sassign,
        BUILTIN_save_values,
        BUILTIN_get_more_status,
        BUILTIN__get_status_code_string,
        BUILTIN_get_enum_string,
        BUILTIN__get_dictionary_string,
        //Anil 22 December 2005 for dictionary_string built in
        BUILTIN__dictionary_string,
        BUILTIN_resolve_array_ref,
        BUILTIN_resolve_record_ref,
        BUILTIN_resolve_param_ref,
        BUILTIN_resolve_local_ref,
        BUILTIN_rspcode_string,
        BUILTIN__set_comm_status,
        BUILTIN__set_device_status,
        BUILTIN__set_resp_code,
        BUILTIN__set_all_resp_code,
        BUILTIN__set_no_device,
        BUILTIN_SET_NUMBER_OF_RETRIES,
        BUILTIN__set_xmtr_comm_status,
        BUILTIN__set_xmtr_device_status,
        BUILTIN__set_xmtr_resp_code,
        BUILTIN__set_xmtr_all_resp_code,
        BUILTIN__set_xmtr_no_device,
        BUILTIN__set_xmtr_all_data,
        BUILTIN__set_xmtr_data,
        BUILTIN_abort,
        BUILTIN_process_abort,
        BUILTIN__add_abort_method,
        BUILTIN__remove_abort_method,
        BUILTIN_remove_all_abort,
        BUILTIN_push_abort_method,
        BUILTIN_pop_abort_method,
        BUILTIN_NaN_value,
        BUILTIN_isetval,
        BUILTIN_lsetval,
        BUILTIN_fsetval,
        BUILTIN_dsetval,
        BUILTIN_igetvalue,
        BUILTIN_igetval,
        BUILTIN_lgetval,
        BUILTIN_fgetval,
        BUILTIN_dgetval,
        BUILTIN_sgetval,
        BUILTIN_ssetval,
        BUILTIN_send,
        BUILTIN_send_command,
        BUILTIN_send_command_trans,
        BUILTIN_send_trans,
        BUILTIN_ext_send_command,
        BUILTIN_ext_send_command_trans,
        BUILTIN_tsend_command,
        BUILTIN_tsend_command_trans,
        BUILTIN_abs,
        BUILTIN_acos,
        BUILTIN_asin,
        BUILTIN_atan,
        BUILTIN_cbrt,
        BUILTIN_ceil,
        BUILTIN_cos,
        BUILTIN_cosh,
        BUILTIN_exp,
        BUILTIN_floor,
        BUILTIN_fmod,
        BUILTIN_frand,
        BUILTIN_log,
        BUILTIN_log10,
        BUILTIN_log2,
        BUILTIN_pow,
        BUILTIN_round,
        BUILTIN_sin,
        BUILTIN_sinh,
        BUILTIN_sqrt,
        BUILTIN_tan,
        BUILTIN_tanh,
        BUILTIN_trunc,
        BUILTIN_atof,
        BUILTIN_atoi,
        BUILTIN_itoa,
        BUILTIN_YearMonthDay_to_Date,
        BUILTIN_Date_to_Year,
        BUILTIN_Date_to_Month,
        BUILTIN_Date_to_DayOfMonth,
        BUILTIN_GetCurrentDate,
        BUILTIN_GetCurrentTime,
        BUILTIN_GetCurrentDateAndTime,
        BUILTIN_To_Date_and_Time,
        BUILTIN_strstr,
        BUILTIN_strupr,
        BUILTIN_strlwr,
        BUILTIN_strlen,
        BUILTIN_strcmp,
        BUILTIN_strtrim,
        BUILTIN_strmid,
        BUILTIN_discard_on_exit,
        BUILTIN__ListInsert,               //Vibhor 200905: Added List Builtins,
        BUILTIN__ListDeleteElementAt,
        BUILTIN__MenuDisplay,//Anil September 26 2005 added MenuDisplay,
        BUILTIN_remove_all_abort_methods,
        BUILTIN_DiffTime,
        BUILTIN_AddTime,
        BUILTIN_Make_Time,
        BUILTIN_To_Time,
        BUILTIN_Date_To_Time,
        BUILTIN_To_Date,
        BUILTIN_Time_To_Date,
        BUILTIN_DATE_to_days,// stevev 16jul14 - rest of the time builtins,
        BUILTIN_days_to_DATE,// stevev 16jul14 ,
        BUILTIN_From_DATE_AND_TIME_VALUE,// stevev 16jul14 ,
        BUILTIN_From_TIME_VALUE,// stevev 16jul14 ,
        BUILTIN_TIME_VALUE_to_seconds,// stevev 16jul14 ,
        BUILTIN_TIME_VALUE_to_Hour,// stevev 16jul14 ,
        BUILTIN_TIME_VALUE_to_Minute,// stevev 16jul14 ,
        BUILTIN_TIME_VALUE_to_Second,// stevev 16jul14 ,
        BUILTIN_seconds_to_TIME_VALUE,// stevev 16jul14 ,
        BUILTIN_DATE_AND_TIME_VALUE_to_string,// stevev 16jul14 ,
        BUILTIN_DATE_to_string,// stevev 16jul14 ,
        BUILTIN_TIME_VALUE_to_string,// stevev 16jul14 ,
        BUILTIN_timet_to_string,// stevev 16jul14 ,
        BUILTIN_timet_to_TIME_VALUE,// stevev 16jul14 ,
        BUILTIN_To_TIME_VALUE,// stevev 16jul14 ,
        BUILTIN_fpclassify,
        BUILTIN_nanf,
        BUILTIN_nan,// stevev 25jun07,
        BUILTIN_literal_string,// stevev 29jan08,
        BUILTIN_openTransferPort,// stevev 24nov08 - block transfer,
        BUILTIN_closeTransferPort,// stevev 24nov08,
        BUILTIN_abortTransferPort,// stevev 24nov08,
        BUILTIN_writeItem2Port,// stevev 24nov08,
        BUILTIN_readItemfromPort,// stevev 24nov08,
        BUILTIN_getTransferStatus,// stevev 24nov08,
        BUILTIN__ERROR,// stevev 16jul14 -  debugging,
        BUILTIN__TRACE,// stevev 16jul14,
        BUILTIN__WARNING,// stevev 16jul14,
    }

    public class CHart_Builtins
    {
        public const int MAX_DD_STRING = 1024;  /*stevev 20may07 - not everywhere yet */
        public const int MAX_LEN_ALLOC = MAX_DD_STRING;
        public const int BI_SUCCESS = 0;
        public const int BLTIN_SUCCESS = 0;
        public const int BLTIN_FAILURE = 1;
        public const int BUILTIN_SUCCESS = 0;
        public const int BUILTIN_ABORT = 1;
        public const int METHOD_ABORTED = 0xf3;
        public const int BLTIN_NO_MEMORY = (1800 + 1);
        public const int BLTIN_VAR_NOT_FOUND = (1800 + 2);
        public const int BLTIN_BAD_ID = (1800 + 3);
        public const int BLTIN_NO_DATA_TO_SEND = (1800 + 4);
        public const int BLTIN_WRONG_DATA_TYPE = (1800 + 5);
        public const int BLTIN_NO_RESP_CODES = (1800 + 6);
        public const int BLTIN_BAD_METHOD_ID = (1800 + 7);
        public const int BLTIN_BUFFER_TOO_SMALL = (1800 + 8);
        public const int BLTIN_CANNOT_READ_VARIABLE = (1800 + 9);
        public const int BLTIN_INVALID_PROMPT = (1800 + 10);
        public const int BLTIN_NO_LANGUAGE_STRING = (1800 + 11);
        public const int BLTIN_DDS_ERROR = (1800 + 12);
        public const int BLTIN_FAIL_RESPONSE_CODE = (1800 + 13);
        public const int BLTIN_FAIL_COMM_ERROR = (1800 + 14);
        public const int BLTIN_NOT_IMPLEMENTED = (1800 + 15);
        public const int BLTIN_BAD_ITEM_TYPE = (1800 + 16);
        public const int BLTIN_VALUE_NOT_SET = (1800 + 17);
        public const int BLTIN_BAD_POINTER = (1800 + 18);
        public const int BLTN_ERROR_END = (1800 + 99);

        //public const int BI_SUCCESS = 0;        /* task succeeded in intended task	  */
        public const int BI_ERROR = -1;     /* error occured in task			  */
        public const int BI_ABORT = -2;     /* user aborted task				  */
        public const int BI_NO_DEVICE = -3;     /* no device found on comm request	  */
        public const int BI_COMM_ERR = -4;      /* communications error				  */
        public const int BI_CONTINUE = -5;      /* continue */
        public const int BI_RETRY = -6; /* retry */
        public const int BI_PORT_IN_USE = -7;           /* block transfer port */

        public const int INTERNAL_BUFFER_SIZE = 1024 * 3;
        public const int DEFAULT_TRANSACTION_NUMBER = -1;

        public const updatePermission_t UPDATE_NORMAL = updatePermission_t.up_UPDATE_STD_DYN;
        public const updatePermission_t UPDATE_ALL = updatePermission_t.up_UPDATE_STD_DYN | updatePermission_t.up_UPDATE_SPEC_DYN;

        public const string mt_String = "";

        public const int PUT_MESSAGE_SLEEP_TIME = 2000;

        const int __IGNORE__ = 0;
        const int __ABORT__ = 1;
        const int __RETRY__ = 2;

        const int RESP_MASK_LEN = 16;   /* size of response code masks		*/
        const int DATA_MASK_LEN = 25;   /* size of data masks				*/
        const int MAX_XMTR_STATUS_LEN = DATA_MASK_LEN;

        const int RESPONSE_BUFFER_LENGTH = 40;  /* size of buffer string to place resp*/
        const int BI_DISP_STR_LEN = 126;/* size of # lines X # char/line in a response code display */

        const long randomKey = 0;

        DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        //string[] vtype_strings = {"<null>", "B", "%s","%s","%d", "%u","%d","%u","%I64d","%I64u", "%f","%lf", "%s", "%ws", "%s", "%s", " <out-of-bounds>"};
        string[] vtype_strings = { "<null>", "B", "%s", "%s", "%d", "%u", "%d", "%u", "d", "u", "%f", "%lf", "%s", "%ws", "%s", "%s", " <out-of-bounds>" };

        private HARTDevice m_pDevice;
        CInterpreter m_pInterpreter;
        OneMeth m_pMeth;
        Dictionary<string, BUILTIN_NAME> m_MapBuiltinFunNameToEnum;
        //Added By Anil June 27 2005 --starts here
        //MAPBUILTINNAMETOENUM m_MapBuiltinFunNameToEnum;
        //MAPBUILTINNAMETOENUM::iterator m_MapBuiltinFunNameToEnumIter;
        void MapBuiltinFunNameToEnum()
        {
            m_MapBuiltinFunNameToEnum["delay"] = BUILTIN_NAME.BUILTIN_delay;
            m_MapBuiltinFunNameToEnum["DELAY"] = BUILTIN_NAME.BUILTIN_DELAY;

            m_MapBuiltinFunNameToEnum["DELAY_TIME"] = BUILTIN_NAME.BUILTIN_DELAY_TIME;

            m_MapBuiltinFunNameToEnum["BUILD_MESSAGE"] = BUILTIN_NAME.BUILTIN_BUILD_MESSAGE;

            m_MapBuiltinFunNameToEnum["PUT_MESSAGE"] = BUILTIN_NAME.BUILTIN_PUT_MESSAGE;

            m_MapBuiltinFunNameToEnum["put_message"] = BUILTIN_NAME.BUILTIN_put_message;

            m_MapBuiltinFunNameToEnum["ACKNOWLEDGE"] = BUILTIN_NAME.BUILTIN_ACKNOWLEDGE;

            m_MapBuiltinFunNameToEnum["acknowledge"] = BUILTIN_NAME.BUILTIN_acknowledge;

            m_MapBuiltinFunNameToEnum["_get_dev_var_value"] = BUILTIN_NAME.BUILTIN__get_dev_var_value;

            m_MapBuiltinFunNameToEnum["_get_local_var_value"] = BUILTIN_NAME.BUILTIN__get_local_var_value;

            m_MapBuiltinFunNameToEnum["_display_xmtr_status"] = BUILTIN_NAME.BUILTIN__display_xmtr_status;

            m_MapBuiltinFunNameToEnum["display_response_status"] = BUILTIN_NAME.BUILTIN_display_response_status;

            m_MapBuiltinFunNameToEnum["display"] = BUILTIN_NAME.BUILTIN_display;

            m_MapBuiltinFunNameToEnum["SELECT_FROM_LIST"] = BUILTIN_NAME.BUILTIN_SELECT_FROM_LIST;

            m_MapBuiltinFunNameToEnum["select_from_list"] = BUILTIN_NAME.BUILTIN_select_from_list;

            m_MapBuiltinFunNameToEnum["_vassign"] = BUILTIN_NAME.BUILTIN__vassign;

            m_MapBuiltinFunNameToEnum["_dassign"] = BUILTIN_NAME.BUILTIN__dassign;

            m_MapBuiltinFunNameToEnum["_fassign"] = BUILTIN_NAME.BUILTIN__fassign;

            m_MapBuiltinFunNameToEnum["_lassign"] = BUILTIN_NAME.BUILTIN__lassign;

            m_MapBuiltinFunNameToEnum["_iassign"] = BUILTIN_NAME.BUILTIN__iassign;

            m_MapBuiltinFunNameToEnum["_fvar_value"] = BUILTIN_NAME.BUILTIN__fvar_value;

            m_MapBuiltinFunNameToEnum["_ivar_value"] = BUILTIN_NAME.BUILTIN__ivar_value;

            m_MapBuiltinFunNameToEnum["_lvar_value"] = BUILTIN_NAME.BUILTIN__lvar_value;

            m_MapBuiltinFunNameToEnum["svar_value"] = BUILTIN_NAME.BUILTIN_svar_value;

            m_MapBuiltinFunNameToEnum["sassign"] = BUILTIN_NAME.BUILTIN_sassign;

            m_MapBuiltinFunNameToEnum["save_values"] = BUILTIN_NAME.BUILTIN_save_values;

            m_MapBuiltinFunNameToEnum["get_more_status"] = BUILTIN_NAME.BUILTIN_get_more_status;

            m_MapBuiltinFunNameToEnum["_get_status_code_string"] = BUILTIN_NAME.BUILTIN__get_status_code_string;

            // made it into _get.. 25jul07 -stevev- methods.h added METHODID() and the underbar
            m_MapBuiltinFunNameToEnum["_get_enum_string"] = BUILTIN_NAME.BUILTIN_get_enum_string;

            m_MapBuiltinFunNameToEnum["_get_dictionary_string"] = BUILTIN_NAME.BUILTIN__get_dictionary_string;

            //Anil 22 December 2005 for dictionary_string built in
            m_MapBuiltinFunNameToEnum["_dictionary_string"] = BUILTIN_NAME.BUILTIN__dictionary_string;

            m_MapBuiltinFunNameToEnum["resolve_array_ref"] = BUILTIN_NAME.BUILTIN_resolve_array_ref;

            m_MapBuiltinFunNameToEnum["resolve_record_ref"] = BUILTIN_NAME.BUILTIN_resolve_record_ref;

            m_MapBuiltinFunNameToEnum["resolve_param_ref"] = BUILTIN_NAME.BUILTIN_resolve_param_ref;

            m_MapBuiltinFunNameToEnum["resolve_local_ref"] = BUILTIN_NAME.BUILTIN_resolve_local_ref;

            m_MapBuiltinFunNameToEnum["rspcode_string"] = BUILTIN_NAME.BUILTIN_rspcode_string;

            m_MapBuiltinFunNameToEnum["_set_comm_status"] = BUILTIN_NAME.BUILTIN__set_comm_status;

            m_MapBuiltinFunNameToEnum["_set_device_status"] = BUILTIN_NAME.BUILTIN__set_device_status;

            m_MapBuiltinFunNameToEnum["_set_resp_code"] = BUILTIN_NAME.BUILTIN__set_resp_code;

            m_MapBuiltinFunNameToEnum["_set_all_resp_code"] = BUILTIN_NAME.BUILTIN__set_all_resp_code;

            m_MapBuiltinFunNameToEnum["_set_no_device"] = BUILTIN_NAME.BUILTIN__set_no_device;

            m_MapBuiltinFunNameToEnum["SET_NUMBER_OF_RETRIES"] = BUILTIN_NAME.BUILTIN_SET_NUMBER_OF_RETRIES;

            m_MapBuiltinFunNameToEnum["_set_xmtr_comm_status"] = BUILTIN_NAME.BUILTIN__set_xmtr_comm_status;

            m_MapBuiltinFunNameToEnum["_set_xmtr_device_status"] = BUILTIN_NAME.BUILTIN__set_xmtr_device_status;

            m_MapBuiltinFunNameToEnum["_set_xmtr_resp_code"] = BUILTIN_NAME.BUILTIN__set_xmtr_resp_code;

            m_MapBuiltinFunNameToEnum["_set_xmtr_all_resp_code"] = BUILTIN_NAME.BUILTIN__set_xmtr_all_resp_code;

            m_MapBuiltinFunNameToEnum["_set_xmtr_no_device"] = BUILTIN_NAME.BUILTIN__set_xmtr_no_device;

            m_MapBuiltinFunNameToEnum["_set_xmtr_all_data"] = BUILTIN_NAME.BUILTIN__set_xmtr_all_data;

            m_MapBuiltinFunNameToEnum["_set_xmtr_data"] = BUILTIN_NAME.BUILTIN__set_xmtr_data;

            m_MapBuiltinFunNameToEnum["abort"] = BUILTIN_NAME.BUILTIN_abort;

            m_MapBuiltinFunNameToEnum["process_abort"] = BUILTIN_NAME.BUILTIN_process_abort;

            m_MapBuiltinFunNameToEnum["_add_abort_method"] = BUILTIN_NAME.BUILTIN__add_abort_method;

            m_MapBuiltinFunNameToEnum["_remove_abort_method"] = BUILTIN_NAME.BUILTIN__remove_abort_method;

            m_MapBuiltinFunNameToEnum["remove_all_abort"] = BUILTIN_NAME.BUILTIN_remove_all_abort;

            m_MapBuiltinFunNameToEnum["_push_abort_method"] = BUILTIN_NAME.BUILTIN_push_abort_method;/*stevev4waltS 11oct07 - match methods.h*/

            m_MapBuiltinFunNameToEnum["pop_abort_method"] = BUILTIN_NAME.BUILTIN_pop_abort_method;

            m_MapBuiltinFunNameToEnum["NaN_value"] = BUILTIN_NAME.BUILTIN_NaN_value;

            m_MapBuiltinFunNameToEnum["isetval"] = BUILTIN_NAME.BUILTIN_isetval;

            m_MapBuiltinFunNameToEnum["lsetval"] = BUILTIN_NAME.BUILTIN_lsetval;

            m_MapBuiltinFunNameToEnum["fsetval"] = BUILTIN_NAME.BUILTIN_fsetval;

            m_MapBuiltinFunNameToEnum["dsetval"] = BUILTIN_NAME.BUILTIN_dsetval;

            m_MapBuiltinFunNameToEnum["igetvalue"] = BUILTIN_NAME.BUILTIN_igetvalue;

            m_MapBuiltinFunNameToEnum["igetval"] = BUILTIN_NAME.BUILTIN_igetval;

            m_MapBuiltinFunNameToEnum["lgetval"] = BUILTIN_NAME.BUILTIN_lgetval;

            m_MapBuiltinFunNameToEnum["fgetval"] = BUILTIN_NAME.BUILTIN_fgetval;

            m_MapBuiltinFunNameToEnum["dgetval"] = BUILTIN_NAME.BUILTIN_dgetval;

            m_MapBuiltinFunNameToEnum["sgetval"] = BUILTIN_NAME.BUILTIN_sgetval;

            m_MapBuiltinFunNameToEnum["ssetval"] = BUILTIN_NAME.BUILTIN_ssetval;

            m_MapBuiltinFunNameToEnum["send"] = BUILTIN_NAME.BUILTIN_send;

            m_MapBuiltinFunNameToEnum["send_command"] = BUILTIN_NAME.BUILTIN_send_command;

            m_MapBuiltinFunNameToEnum["send_command_trans"] = BUILTIN_NAME.BUILTIN_send_command_trans;

            m_MapBuiltinFunNameToEnum["send_trans"] = BUILTIN_NAME.BUILTIN_send_trans;

            m_MapBuiltinFunNameToEnum["ext_send_command"] = BUILTIN_NAME.BUILTIN_ext_send_command;

            m_MapBuiltinFunNameToEnum["ext_send_command_trans"] = BUILTIN_NAME.BUILTIN_ext_send_command_trans;

            m_MapBuiltinFunNameToEnum["tsend_command"] = BUILTIN_NAME.BUILTIN_tsend_command;

            m_MapBuiltinFunNameToEnum["tsend_command_trans"] = BUILTIN_NAME.BUILTIN_tsend_command_trans;

            m_MapBuiltinFunNameToEnum["abs"] = BUILTIN_NAME.BUILTIN_abs;

            m_MapBuiltinFunNameToEnum["acos"] = BUILTIN_NAME.BUILTIN_acos;

            m_MapBuiltinFunNameToEnum["asin"] = BUILTIN_NAME.BUILTIN_asin;

            m_MapBuiltinFunNameToEnum["atan"] = BUILTIN_NAME.BUILTIN_atan;

            m_MapBuiltinFunNameToEnum["cbrt"] = BUILTIN_NAME.BUILTIN_cbrt;

            m_MapBuiltinFunNameToEnum["ceil"] = BUILTIN_NAME.BUILTIN_ceil;

            m_MapBuiltinFunNameToEnum["cos"] = BUILTIN_NAME.BUILTIN_cos;

            m_MapBuiltinFunNameToEnum["cosh"] = BUILTIN_NAME.BUILTIN_cosh;

            m_MapBuiltinFunNameToEnum["exp"] = BUILTIN_NAME.BUILTIN_exp;

            m_MapBuiltinFunNameToEnum["floor"] = BUILTIN_NAME.BUILTIN_floor;

            m_MapBuiltinFunNameToEnum["fmod"] = BUILTIN_NAME.BUILTIN_fmod;

            m_MapBuiltinFunNameToEnum["frand"] = BUILTIN_NAME.BUILTIN_frand;

            m_MapBuiltinFunNameToEnum["log"] = BUILTIN_NAME.BUILTIN_log;

            m_MapBuiltinFunNameToEnum["log10"] = BUILTIN_NAME.BUILTIN_log10;

            m_MapBuiltinFunNameToEnum["log2"] = BUILTIN_NAME.BUILTIN_log2;

            m_MapBuiltinFunNameToEnum["pow"] = BUILTIN_NAME.BUILTIN_pow;

            m_MapBuiltinFunNameToEnum["round"] = BUILTIN_NAME.BUILTIN_round;

            m_MapBuiltinFunNameToEnum["sin"] = BUILTIN_NAME.BUILTIN_sin;

            m_MapBuiltinFunNameToEnum["sinh"] = BUILTIN_NAME.BUILTIN_sinh;

            m_MapBuiltinFunNameToEnum["sqrt"] = BUILTIN_NAME.BUILTIN_sqrt;

            m_MapBuiltinFunNameToEnum["tan"] = BUILTIN_NAME.BUILTIN_tan;

            m_MapBuiltinFunNameToEnum["tanh"] = BUILTIN_NAME.BUILTIN_tanh;

            m_MapBuiltinFunNameToEnum["trunc"] = BUILTIN_NAME.BUILTIN_trunc;

            m_MapBuiltinFunNameToEnum["atof"] = BUILTIN_NAME.BUILTIN_atof;

            m_MapBuiltinFunNameToEnum["atoi"] = BUILTIN_NAME.BUILTIN_atoi;

            m_MapBuiltinFunNameToEnum["itoa"] = BUILTIN_NAME.BUILTIN_itoa;

            //	m_MapBuiltinFunNameToEnum["YearMonthDay_to_Date"]					= BUILTIN_NAME.BUILTIN_YearMonthDay_to_Date;//WS:EPM Not a builtin 25jun07

            m_MapBuiltinFunNameToEnum["Date_to_Year"] = BUILTIN_NAME.BUILTIN_Date_to_Year;

            m_MapBuiltinFunNameToEnum["Date_to_Month"] = BUILTIN_NAME.BUILTIN_Date_to_Month;

            m_MapBuiltinFunNameToEnum["Date_to_DayOfMonth"] = BUILTIN_NAME.BUILTIN_Date_to_DayOfMonth;

            //	m_MapBuiltinFunNameToEnum["GetCurrentDate"]							= BUILTIN_NAME.BUILTIN_GetCurrentDate;//WS:EPM Not a builtin 25jun07

            m_MapBuiltinFunNameToEnum["GetCurrentTime"] = BUILTIN_NAME.BUILTIN_GetCurrentTime;

            //	m_MapBuiltinFunNameToEnum["GetCurrentDateAndTime"]					= BUILTIN_NAME.BUILTIN_GetCurrentDateAndTime;//WS:EPM Not a builtin 25jun07

            //	m_MapBuiltinFunNameToEnum["To_Date_and_Time"]						= BUILTIN_NAME.BUILTIN_To_Date_and_Time;//WS:EPM Not a builtin 25jun07

            m_MapBuiltinFunNameToEnum["strstr"] = BUILTIN_NAME.BUILTIN_strstr;

            m_MapBuiltinFunNameToEnum["strupr"] = BUILTIN_NAME.BUILTIN_strupr;

            m_MapBuiltinFunNameToEnum["strlwr"] = BUILTIN_NAME.BUILTIN_strlwr;

            m_MapBuiltinFunNameToEnum["strlen"] = BUILTIN_NAME.BUILTIN_strlen;

            m_MapBuiltinFunNameToEnum["strcmp"] = BUILTIN_NAME.BUILTIN_strcmp;

            m_MapBuiltinFunNameToEnum["strtrim"] = BUILTIN_NAME.BUILTIN_strtrim;

            m_MapBuiltinFunNameToEnum["strmid"] = BUILTIN_NAME.BUILTIN_strmid;

            //Added By Anil July 01 2005 --starts here
            m_MapBuiltinFunNameToEnum["discard_on_exit"] = BUILTIN_NAME.BUILTIN_discard_on_exit;
            //Added By Anil July 01 2005 --Ends here

            //Vibhor 200905: Added
            m_MapBuiltinFunNameToEnum["_ListInsert"] = BUILTIN_NAME.BUILTIN__ListInsert;            //Vibhor 130705: Added

            m_MapBuiltinFunNameToEnum["_ListDeleteElementAt"] = BUILTIN_NAME.BUILTIN__ListDeleteElementAt;  //Vibhor 130705: Added

            m_MapBuiltinFunNameToEnum["_MenuDisplay"] = BUILTIN_NAME.BUILTIN__MenuDisplay;  //Anil September 26 2005 added MenuDisplay

            m_MapBuiltinFunNameToEnum["remove_all_abort_methods"] = BUILTIN_NAME.BUILTIN_remove_all_abort_methods;

            m_MapBuiltinFunNameToEnum["DiffTime"] = BUILTIN_NAME.BUILTIN_DiffTime;

            m_MapBuiltinFunNameToEnum["AddTime"] = BUILTIN_NAME.BUILTIN_AddTime;

            m_MapBuiltinFunNameToEnum["Make_Time"] = BUILTIN_NAME.BUILTIN_Make_Time;

            m_MapBuiltinFunNameToEnum["To_Time"] = BUILTIN_NAME.BUILTIN_To_Time;

            m_MapBuiltinFunNameToEnum["Date_To_Time"] = BUILTIN_NAME.BUILTIN_Date_To_Time;

            m_MapBuiltinFunNameToEnum["To_Date"] = BUILTIN_NAME.BUILTIN_To_Date;

            m_MapBuiltinFunNameToEnum["Time_To_Date"] = BUILTIN_NAME.BUILTIN_Time_To_Date;

            // added 16jul14------------------------------------------------------------------------------
            m_MapBuiltinFunNameToEnum["DATE_to_days"] = BUILTIN_NAME.BUILTIN_DATE_to_days;

            m_MapBuiltinFunNameToEnum["days_to_DATE"] = BUILTIN_NAME.BUILTIN_days_to_DATE;

            m_MapBuiltinFunNameToEnum["From_DATE_AND_TIME_VALUE"] = BUILTIN_NAME.BUILTIN_From_DATE_AND_TIME_VALUE;

            m_MapBuiltinFunNameToEnum["From_TIME_VALUE"] = BUILTIN_NAME.BUILTIN_From_TIME_VALUE;

            m_MapBuiltinFunNameToEnum["TIME_VALUE_to_seconds"] = BUILTIN_NAME.BUILTIN_TIME_VALUE_to_seconds;

            m_MapBuiltinFunNameToEnum["TIME_VALUE_to_Hour"] = BUILTIN_NAME.BUILTIN_TIME_VALUE_to_Hour;

            m_MapBuiltinFunNameToEnum["TIME_VALUE_to_Minute"] = BUILTIN_NAME.BUILTIN_TIME_VALUE_to_Minute;

            m_MapBuiltinFunNameToEnum["TIME_VALUE_to_Second"] = BUILTIN_NAME.BUILTIN_TIME_VALUE_to_Second;

            m_MapBuiltinFunNameToEnum["seconds_to_TIME_VALUE"] = BUILTIN_NAME.BUILTIN_seconds_to_TIME_VALUE;

            m_MapBuiltinFunNameToEnum["DATE_AND_TIME_VALUE_to_string"] = BUILTIN_NAME.BUILTIN_DATE_AND_TIME_VALUE_to_string;

            m_MapBuiltinFunNameToEnum["DATE_to_string"] = BUILTIN_NAME.BUILTIN_DATE_to_string;

            m_MapBuiltinFunNameToEnum["TIME_VALUE_to_string"] = BUILTIN_NAME.BUILTIN_TIME_VALUE_to_string;

            m_MapBuiltinFunNameToEnum["timet_to_string"] = BUILTIN_NAME.BUILTIN_timet_to_string;

            m_MapBuiltinFunNameToEnum["timet_To_TIME_VALUE"] = BUILTIN_NAME.BUILTIN_timet_to_TIME_VALUE;

            m_MapBuiltinFunNameToEnum["To_TIME_VALUE"] = BUILTIN_NAME.BUILTIN_To_TIME_VALUE;
            // end 16jul14 addition

            m_MapBuiltinFunNameToEnum["fpclassify"] = BUILTIN_NAME.BUILTIN_fpclassify;

            m_MapBuiltinFunNameToEnum["nanf"] = BUILTIN_NAME.BUILTIN_nanf;

            m_MapBuiltinFunNameToEnum["nan"] = BUILTIN_NAME.BUILTIN_nan;// stevev - added 25jun07
                                                                        //stevev 29jan08 to look up method's literal string
            m_MapBuiltinFunNameToEnum["literal_string"] = BUILTIN_NAME.BUILTIN_literal_string;
            // stevev 24nov08 - add block transfer functions
            m_MapBuiltinFunNameToEnum["openTransferPort"] = BUILTIN_NAME.BUILTIN_openTransferPort;

            m_MapBuiltinFunNameToEnum["closeTransferPort"] = BUILTIN_NAME.BUILTIN_closeTransferPort;

            m_MapBuiltinFunNameToEnum["abortTransferPort"] = BUILTIN_NAME.BUILTIN_abortTransferPort;

            m_MapBuiltinFunNameToEnum["_writeItemToDevice"] = BUILTIN_NAME.BUILTIN_writeItem2Port;

            m_MapBuiltinFunNameToEnum["_readItemFromDevice"] = BUILTIN_NAME.BUILTIN_readItemfromPort;

            m_MapBuiltinFunNameToEnum["get_transfer_status"] = BUILTIN_NAME.BUILTIN_getTransferStatus;
            // end transfer functions
            // added 16jul14 - stevev ------------------------------------------------------------------------------
            m_MapBuiltinFunNameToEnum["_ERROR"] = BUILTIN_NAME.BUILTIN__ERROR;

            m_MapBuiltinFunNameToEnum["_TRACE"] = BUILTIN_NAME.BUILTIN__TRACE;

            m_MapBuiltinFunNameToEnum["_WARNING"] = BUILTIN_NAME.BUILTIN__WARNING;
        }
        //Added By Anil June 27 2005 --Ends here

        int lPre_postItemID;

        public int SEND_COMMAND
            (
            int iCommandNumber,
            int iTransNumber,
            ref byte[] pchResponseStatus,
            ref byte[] pchMoreDataStatus,
            ref byte[] pchMoreDataInfo,
            int iCmdType,
            bool bMoreDataFlag,
            ref int iMoreInfoSize
            )
        {
            return 0;
        }
        /*
		void PackedASCIIToASCII(
									byte* pbyPackedASCII
									, ushort short wPackedASCIISize
									, char* pchASCII
								);
		void ASCIIToPackedASCII(
									char* pchASCII
									  , byte* pbyPackedASCIIOutput
									   , ushort short* pwPackedASCIISize
								   );*/
        /*Vibhor 081204: Start of Code*/
        //Adding this function to execute the actions on waveforms. Should only be called from PlotBuiltins

        public CHart_Builtins()
        {
            m_MapBuiltinFunNameToEnum = new Dictionary<string, BUILTIN_NAME>();
            MapBuiltinFunNameToEnum();
        }


        //prashant 05/04/04
        public bool m_AbortInProgress;

        //operations
        public bool Initialise(HARTDevice pDevice, CInterpreter pInterpreter, OneMeth/*MEE*/ pMeth)
        {
            if ((pDevice == null) || (pInterpreter == null) || (pMeth == null))
            {
                return false;
            }
            m_pDevice = pDevice;
            m_pInterpreter = pInterpreter;
            m_pMeth = pMeth;

            return true;
        }

        public bool Initialise(HARTDevice pDevice, CInterpreter pInterpreter, int lItemId)
        {
            if ((pDevice == null) || (pInterpreter == null))
            {
                return false;
            }
            m_pDevice = pDevice;
            m_pInterpreter = pInterpreter;

            lPre_postItemID = lItemId;

            return true;
        }

        bool GetStringParam(ref string retString, ref INTER_VARIANT[] pParamArray, int paramNumber)
        {
            //string pC = retString;
            //bool ret = true;
            /* 11 feb08 - most of this processing is now done in the INTER_VARIENT */
            string pRet = "";

            pParamArray[paramNumber].GetStringValue(ref pRet);
            if (pRet != null)// allocated in GetString
            {
                //retStringLen = min(wcslen(pRet), MAX_DD_STRING) + 1;
                //wcsncpy(retString, pRet, retStringLen);
                retString = pRet;
            }
            /* instead of the following...
				if ((pParamArray[paramNumber].GetVarType() == RUL_CHARPTR)  || 
					(pParamArray[paramNumber].GetVarType() == RUL_UNSIGNED_CHAR)  )
				{
					maxLength = min((retStringLen*sizeof(string)),MAX_DD_STRING);
					strLength = strlen((char*)(pParamArray[paramNumber]));
					if (strLength > maxLength)
					{
						strncpy(pC,pParamArray[paramNumber],maxLength-1);
						retString[maxLength-1] = '\0';
					}
					else
						strcpy(pC,pParamArray[paramNumber]);
				}
				else
				if ((pParamArray[paramNumber].GetVarType() == RUL_WIDECHARPTR)  || 
					(pParamArray[paramNumber].GetVarType() == RUL_DD_STRING)  )
				{
					maxLength = min(retStringLen,MAX_DD_STRING);
					strLength = _tstrlen(pParamArray[paramNumber]);
					if (strLength > maxLength)
					{
						_tstrncpy(retString,pParamArray[paramNumber],maxLength-1);
						retString[maxLength-1] = _T('\0');
					}
					else
						_tstrcpy(retString,pParamArray[paramNumber]);
				}
				else 
				if (pParamArray[paramNumber].GetVarType() == RUL_SAFEARRAY)
				{
					GetWCharArray(pParamArray[paramNumber], retString, strLength);
				}
				else
				{
					return false;
				}
			***** end replaced code *****/
            return true;
        }

        bool GetLongArray(INTER_VARIANT varValue, int[] plongArray, ref int iArraySize)
        {
            INTER_SAFEARRAY sa = null;
            if (varValue.GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
            {
                sa = varValue.GetSafeArray();

                List<int> il = null;
                int idims = sa.GetDims(ref il);

                if (idims == 1)
                {
                    INTER_VARIANT vValue = new INTER_VARIANT();
                    for (int i = 0, index = 0; index < sa.GetNumberOfElements(); i += sa.GetElementSize(), index++)
                    {
                        sa.GetElement((uint)i, ref vValue);
                        if (vValue.GetVarType() != VARIANT_TYPE.RUL_INT)
                        {
                            iArraySize = 0;
                            return false;
                        }
                        plongArray[index] = vValue.GetVarInt();
                    }
                    iArraySize = sa.GetNumberOfElements();
                    return true;
                }
                else
                {
                    iArraySize = 0;
                    return false;
                }
            }
            else
            {
                iArraySize = 0;
                return false;
            }
        }

        int CopyToOutbuf(ref string dest, ref int availChars, string source, int srcLen = -1)//-1 is copy till '\0'
        {
            int r = BLTIN_SUCCESS;

            if (dest == null || source == null)
            {
                //SDCLOG(CERR_LOG, "Method Format Error: CopyToOutbuf: Empty Parameter!\n");
                return BLTIN_INVALID_PROMPT;
            }

            if (srcLen < 0)
            {
                srcLen = source.Length;
            }
            if (srcLen > availChars)
            {// buffer full
                r = BLTIN_BUFFER_TOO_SMALL;
            }
            else
            {//	leave r = SUCCESS-copy over terminating null
                /*
				(void)_tstrncpy(dest, source, srcLen); 
				dest[srcLen] = _T('\0');
				dest += (srcLen);
				*/
                dest += source.Substring(0, srcLen);
                availChars -= srcLen;
            }

            return r;
        }

        int bltin_format_string(ref string out_buf, int max_length, updatePermission_t updateLevel, string passed_prompt, int[] glob_var_ids, int nCnt_Ids,
                     ref CValueVarient[] pDynamicVarValues, ref bool bDyanmicVarvalChanged)
        {
            int retCode = BLTIN_SUCCESS;
            int remainingOut = max_length, copy_length = 0, paramIdx = -1;
            //bool isSpecialDyn = false;// tells the one we are working with is a special %n
            string prompt_buf = null;
            string prompt = null;
            string curr_ptr = null;

            string temp_buf = "";// bigger to handle multibyte chars
            string curr_format;
            string curr_param;
            char prompt_char = '\0';

            CDDLBase pIB = null;
            CDDLVar pVar = null;
            CValueVarient ReturnedDataItem = new CValueVarient();
            INTER_VARIANT VariantVarVal = new INTER_VARIANT();

            bool updateDynamic = false;//tells if we are in the update state(==enabledynamic)
            string curr_label;
            //, tempValue;
            int nDynVarCntr = (pDynamicVarValues != null) ? 0 : -1;// add to prevent filling an empty

            bDyanmicVarvalChanged = false;

            //#update_dynamics - passed in (depends on builtin function at this time)
            //#find_lang_string()	
            //Find the appropriate country code string in the prompt string.

            /*
			prompt_buf = (string)malloc((_tstrlen(passed_prompt) + 1) * 2);

			if (prompt_buf == null)
			{
				out_buf = '\0';
				return BLTIN_NO_MEMORY; //#exit if null returned - nothing to work with -	
			}
			DEBUGLOG(CLOG_LOG, _T(">>>|%s|<<< Entry\n"), passed_prompt);
			*/
            //(void)_tstrcpy(prompt_buf, passed_prompt);
            prompt_buf = passed_prompt;
            //prompt = find_lang_string (prompt_buf);// copies the string to prompt_buf from internal loc
            // use the dictionary version... no need for 5 versions of this
            // stevev 13mar14 - change the parameters to match find_lang_string
            prompt = prompt_buf;
            int rc = DDlDevDescription.pGlobalDict.get_string_translation(prompt_buf, ref prompt);
            if (rc != Common.SUCCESS)
            //if (prompt == null)
            {
                //free(prompt_buf);
                out_buf = "";
                return BLTIN_NO_LANGUAGE_STRING;    //#exit if null returned - nothing to work with -
            }

            //#for - ever	
            while (retCode == BLTIN_SUCCESS)// stop on error code
            {
                //#	clear vars as required for the loop	 strings					
                //temp_buf[0] = _T('\0');
                //isSpecialDyn = false;
                updateDynamic = false;
                paramIdx = -1;
                //curr_format[0] =
                //curr_param[0] = _T('\0'); // to make Walt happy..

                //#		move short circuit inside loop so we get the language string reguardless
                //curr_ptr = _tstrchar(prompt, _T('%'));
                if (prompt.Contains('%'))
                {
                    int st = prompt.IndexOf('%');
                    curr_ptr = prompt.Substring(st, prompt.Length - st);
                }

                //#	if no more '%'	
                //if (curr_ptr == null)
                else
                {
                    //#		if  !  Copy2Output()	--- no more %, copy the rest to output and leave
                    //strcat(out_buf,prompt);
                    retCode = CopyToOutbuf(ref out_buf, ref remainingOut, prompt);
                    // retCode determines success or failure, we're leaving reguardless
                    break;
                }// else we have a '%' - so process it												
                 //#
                 //#	if ! Copy2Output(  current location, to the '%' char )	
                 //	assert("*curr_ptr == '%'");
                copy_length = prompt.Length - curr_ptr.Length;// inherently copies upto but not including '%'

                if (copy_length > 0)
                {
                    if (CopyToOutbuf(ref out_buf, ref remainingOut, prompt, copy_length) != 0)//testing retcode
                    {// probable full buffer
                        break;// error exit
                    }
                }
                prompt = curr_ptr.Substring(1);//////
                prompt_char = prompt[0];

                /*
				 *	Handle the formatting.  The formatting consists of:
				 *
				 *	%[format]{param ID array index} for a device param
				 *	%[format]{method-local param name} for a local paramparam
				 *	%[format]{complex-Var-reference} for a attributes and the like
				 *  %[format]n  when n is 0 to 9 id index (update dynamic if allowed)
				 *
				 *	The format is optional, and consists of standard scanf
				 *	format string, with the addition of 'L', which specifies
				 *	the label of the param and 'U' which is the Units 
				 * stevev 23feb11                                   and ',D' for isDynamic
				 */
                //# acquire the formatting

                if (prompt_char == '[') //#	if nextChar == '[' // whitespace not allowed	
                {   /**	Capture the format string.	 */

                    //prompt++; // skip the '['
                    //#		get following ']'		
                    //curr_ptr = _tstrchar(prompt, _T(']'));
                    if (prompt.Contains(']'))
                    {
                        int st = prompt.IndexOf(']');
                        curr_ptr = prompt.Substring(st, prompt.Length - st);
                    }

                    if (curr_ptr == null || curr_ptr == prompt)
                    {
                        //#		exit if not found		
                        retCode = BLTIN_INVALID_PROMPT;
                        //SDCLOG(CERR_LOG, "Method Format Error: format's closing ']' not found.\n");
                        break; // exit 
                    }
                    //copy_length = curr_ptr - prompt;
                    //#		curr_format  =  "%"  + string between '[' & ']'			
                    curr_format = "%";
                    //(void)_tstrncpy((curr_format[1]), prompt, copy_length);

                    //curr_format[copy_length + 1] = _T('\0');
                    curr_format = prompt.Substring(1, prompt.IndexOf(']') - 1);//////
                    prompt = curr_ptr + 1;
                    prompt_char = prompt[0];
                }
                else
                {
                    //#	else curr_format == MT string		
                    curr_format = "";
                }

                //#
                //#	if nextChar == '{'  // whitespace not allowed			
                if (prompt_char == '{')
                {/**	Get the param string. */
                    //prompt++;       // skip the '{'
                    //#		get following '}'		
                    //curr_ptr = _tstrchar(prompt, _T('}'));
                    if (prompt.Contains('}'))
                    {
                        int st = prompt.IndexOf('}');
                        curr_ptr = prompt.Substring(st, prompt.Length - st);
                    }
                    if (curr_ptr == null)
                    {
                        //#		exit if not found				
                        retCode = BLTIN_INVALID_PROMPT;
                        //SDCLOG(CERR_LOG, "Method Format Error: param's closing '}' not found.\n");
                        break; // leave the loop & return
                    }
                    //copy_length = curr_ptr - prompt;
                    //#		curr_param  =  string between '{' & '}'		
                    //(void)_tstrncpy(curr_param, prompt, copy_length);
                    //curr_param[copy_length] = _T('\0');
                    curr_param = prompt.Substring(1, prompt.IndexOf('}') - 1);//////

                    prompt = curr_ptr + 1;
                    prompt_char = prompt[0];
                }
                //#	else														
                //#	if nextChar == numeric  // whitspace not allowed - single digit only
                else if (Char.IsDigit(prompt_char))
                {   /**	Special case of param string... %X is same as %{X}
			 	where X is a single digit number.		 */
                    //#		curr_param = one digit + '\0'		
                    curr_param = prompt_char.ToString();
                    //curr_param[1] = 0;
                    prompt = prompt.Substring(1);
                    prompt_char = prompt[0];
                    //isSpecialDyn = true;// this is a special that is eligible for dynamic updates
                    // it will ONLY be updated if it is Dynamic and up_UPDATE_SPEC_DYN has been set
                }
                //#	else														
                //#	if got no format string	
                else if (curr_format == "")
                {
                    //#		copy '%' to out_buf   - sometimes a percent is just a percent					
                    retCode = CopyToOutbuf(ref out_buf, ref remainingOut, "%", 1);
                    continue;// will exit if there was an error above...
                }
                else
                //#	else		// '%' [ stuff ] nothing
                {
                    retCode = BLTIN_INVALID_PROMPT;
                    //SDCLOG(CERR_LOG, "Method Format Error:No parameter after format.""(whitespace is not allowed)\n");
                    //#		error - exit	
                    break;
                }
                //#
                //#	// we have captured 'em both. Now the parameter MUST resolve to an hCVar*  
                //# //		                                            OR a method-local-variable		
                //#
                //#	if curr_par == isdigit	
                if (Char.IsDigit((curr_param[0])))
                {
                    paramIdx = Convert.ToInt32(curr_param);
                    //#		do range check - exit on error	
                    if (glob_var_ids == null || paramIdx > (nCnt_Ids - 1))//zero based glob_var_ids array
                    {
                        retCode = BLTIN_VAR_NOT_FOUND;
                        //SDCLOG(CERR_LOG, "Method Format Error:Param Index not found in Var-ID array.\n");
                        //#		error - exit	
                        break;
                    }// else - OK, so process it
                     //#		lookup itemid in idarray
                    int itemID = (glob_var_ids[paramIdx]);
                    //#		VarPtr = getItemBySymbolNumber()	/* if device param */
                    if (m_pDevice.getItembyID((uint)itemID, ref pIB) && pIB != null
                        //#		verify it's a var - exit on error	
                        && pIB.IsVariable())
                    {
                        pVar = (CDDLVar)pIB;
                        if (m_pDevice.whatCompatability() == devMode_t.dm_Standard)// NOT lenient mode
                        {
                            if (!pVar.IsValid())
                            {
                                retCode = BLTIN_CANNOT_READ_VARIABLE;
                                //SDCLOG(CERR_LOG, "Method Format Error:Trying to display an ""Invalid Variable\n");
                                break;// error exit
                            }// else it's OK, continue
                        }// else - lenient, we don't care
                    }
                    else // unfound item
                    {
                        if (passed_prompt == null)
                        {
                            passed_prompt = "";
                        }
                        retCode = BLTIN_VAR_NOT_FOUND;
                        //SDCLOG(CERR_LOG, L"Method Format Error:ID array # %d was not found in the DD\n"L"             Prompt:'%s'\n", paramIdx, passed_prompt);
                        //#		error - exit	
                        break;
                    }
                }
                //#	else    // not a digit, it's a (possibly complex) variable reference	
                else
                {
                    paramIdx = -1;
                    //#		interp-- GetVarPtr	
                    //VariantVarVal.Clear();
                    pVar = null;
                    //string wS(curr_param);
                    //string S;
                    //S = TStr2AStr(wS);

                    //retCode = m_pInterpreter.GetVariableValue(curr_param, ref VariantVarVal, ref pVar);
                    //if (retCode == 0)// false on error - weird but...
                    if (!m_pInterpreter.GetVariableValue(curr_param, ref VariantVarVal, ref pVar))
                    {
                        //SDCLOG(CERR_LOG, "Method Format Error:Failed to find Variable Value.'%s'\n", (char*)curr_param);
                        //#		error - exit	
                        break;
                    }
                    //#		if it's a DD-var, hCVar* has value	
                    //#		else will use INTER_VARIENT to hold the method-local variable's value	
                }

                //# # # # # # # # # # we have the format string and the variable/value # # # # # # # # #

                //#	if  curr_format '%L' || '%U' 
                //#			value_string <<= label or unit	
                if (/*curr_format[0] != 0 && */(curr_format == "%L" || curr_format == "%U"))
                {   /*	Print the label of the param. or flag dynamic*/
                    if (pVar != null)
                    {
                        //#		if a DD-variable
                        if (curr_format == "%L")
                        {
                            //pVar.Label(curr_label);
                            curr_label = pVar.GetName();
                            //(void)_tsprintf(temp_buf, _T("%s"), curr_label.c_str());
                            temp_buf = curr_label;
                        }
                        else // must be U
                        {
                            //pVar.getUnitString(curr_label);
                            curr_label = pVar.GetUnitStr();//////
                                                           //(void)_tsprintf(temp_buf, _T("%s"), curr_label.c_str());
                            temp_buf = curr_label;
                        }
                    }
                    else // %L & %U are only valid on DD variables
                    {
                        //SDCLOG(CERR_LOG, "Method Format Error:%%[L],%%[U] & %%[D] are only valid on DD variables\n");
                        break;// nothing to display
                    }
                }
                else
                //#	else     // not L or U  format
                {
                    if (curr_format != "" && curr_format != null)
                    {
                        if (curr_format == "%D")// check for a D alone
                        {
                            updateDynamic = true; // we'll check for an actual dynamic later
                            curr_format = "";// no need to do anymore formatting
                        }
                        else// check for a ,D in the format
                        {
                            //curr_ptr = _tstrchar (prompt, _T(','));//comma illegal except 4 ',D'
                            //if (curr_ptr != null && curr_ptr != prompt)
                            curr_ptr = curr_format.Substring(curr_format.IndexOf(','));//comma illegal except 4 ',D'
                            if (curr_ptr != null && curr_ptr != curr_format)
                            {//		found
                             //*curr_ptr++ = 0;// get rid of comma and whatever follows
                                if (curr_ptr[0] == 'D')// if it was ',D'
                                {
                                    updateDynamic = true;
                                }
                                else// otherwise an error - no whitspace allowed
                                {
                                    //SDCLOG(CERR_LOG, "Method Format Error:comma is only valid for ',D' formatting\n");
                                    // fall thru to try and display something
                                }
                            }
                            // else - at not found, do nothing.
                        }
                    }// endif we have a format so check for a D


                    /* modified 30may14 - stevev - 
					 * up_UPDATE_SPEC_DYN is only used for display - where any and all dynamics are updated
					 * up_UPDATE_STD_DYN  is normal - only update if dynamic and has a 'D' format
					 * up_DONOT_UPDATE    is for a couple of builtins that NEVER update 
					 *                    - these will have a null pDynamicVarValues
					 */
                    if (pVar != null)// we have a DD variable
                    {// do the value
                     // isSpecialDyn usage is not defined at this time.
                     //#			if  var.isDynamic && ( isSpecialDyn || update_Dynamic )	
                        if (pVar.IsDynamic() // we only update dynamic variables, ever (wap 10aug07)
                             &&
                             pDynamicVarValues != null  // we were passed in (if not then NEVER update)
                             &&
                             (updateDynamic || // set via the 'D' option above
                                               //OR - if it is an 'auto update' built-in function (DISPLAY or display)
                                ((updateLevel & updatePermission_t.up_UPDATE_SPEC_DYN) == updatePermission_t.up_UPDATE_SPEC_DYN)
                             )
                           )
                        {// we are GO to update dynamics
                            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay =
                            updateDynamic = true;
                            //#				get command,  send it the first time
                            hCcommandDescriptor rdCmd = pVar.getRdCmd();
                            if (rdCmd.cmdNumber > -1)
                            {   // get cmd ptr
                                /* stevev 20Jul05 this needs the entire descriptor 
								 ** including indexes ***
								 for now we'll just give the right transaction????????? */
                                m_pDevice.sendMethodCmd(rdCmd.cmdNumber, rdCmd.transNumb);//stevev 30nov11 add indexes
                                /* end stevev 20Jul05 */
                            }// else - just get the current display value.
                        }
                        else
                        {
                            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay =
                            updateDynamic = false;
                        }// endif dynamic update - send command

                        //#				getDispValue to CurrValueVarient (dynamic & non-dynamic the same)
                        ReturnedDataItem = pVar.getDispValue();// we won't use it in getDisplayString 
                                                               //#				put value into dynamic array  &&  set ischanged
                        if (updateDynamic)
                        {
                            if (!(pDynamicVarValues[nDynVarCntr] == ReturnedDataItem))
                            {
                                bDyanmicVarvalChanged = true;
                                pDynamicVarValues[nDynVarCntr] = ReturnedDataItem;
                            }
                            nDynVarCntr++;
                        }// else there is a reason this is not supposed to be updated		
                    }//endif we have a DD variable 
                    else   // not a DD-variable;  MUST be a method-local variable so do INTER_VARIENT
                    {
                        if (updateDynamic)// we have a 'D' format
                        {
                            //SDCLOG(CERR_LOG, "Method Format Error:'D' formatting is illegal for method-local variables.\n");
                        }
                        //#			convert intervarient to CurrValueVarient
                        //if (inter2hcVARIANT(ReturnedDataItem, VariantVarVal))// true on error
                        {
                            //SDCLOG(CERR_LOG, "Method Format Error: " "Method-Local to Varient conversion failed.\n");
                            //break;
                        }
                        //#			if curr_format == null - ie isEmpty()	
                        if (curr_format == "" || curr_format == null)
                        {
                            //#				copy default format string into curr_format	
                            //_tstrcpy(curr_format,vtype_strings[VariantVarVal.GetVarType()]);
                            string s = vtype_strings[(int)VariantVarVal.GetVarType()];
                            /*
							string ws;
							ws = AStr2TStr(s);
							_tstrcpy(curr_format, ws.c_str());
							*/
                            curr_format = s;
                        }
                        //#			//else -- we'll use the curr_format and currvalvarient to get the string later
                    }// end else parameter must be a method-local

                    //#		if  curr_format == null && VarPtr != null	
                    /*
					if (curr_format[0] == '\0' && pVar != null)
					{// we need to handle the formatting exceptions first
						string tmp;

						//#			tmp = getDisplayString()	
						switch (pVar.VariableType())
						{
							case variableType_t.vT_Enumerated:
								{
									if (((hCEnum*)pVar).procureString((ushort)ReturnedDataItem, tmp))
									{
										tmp = "";
									}
								}
								break;
							case variableType_t.vT_BitEnumerated:
								{
									if (((hCBitEnum*)pVar).procureString((ushort)ReturnedDataItem, tmp))
									{
										tmp = "";
									}
								}
								break;
							//AOEP35746: VT_PASSWORD DETECTION TO HIDE THE PASSWORD WS_08sep10
							case variableType_t.vT_Password:
								{
									string strPassword = pVar.getDispValueString();
									int nSize = strPassword.length();
									string tempString[100] = { 0 };
									for (int i = 0; i < nSize; i++)
									{
										tempString[i] = '*';
									}
									tmp = tempString;
								}
								break;
							//END AOEP35746
							default: // all other types...
									 // vT_Index does a string substitution...
								tmp = pVar.getDispValueString();   //varient unused here
								break;
						}// endswitch vartype
						 //#				value_string <<= tmp					
						temp_buf = tmp;
					}
					else// - all the rest...
					*/
                    {   // have a user_format with or without a pVar 
                        // OR no  user_format without pVar (curr_format has default formatting)
                        if (doFormat(curr_format, /*pVar, ref */ReturnedDataItem, ref temp_buf, INTERNAL_BUFFER_SIZE))
                        {// was an error
                            temp_buf = "<Formatting Error>";
                        }
                        //#			switch on varient data type													
                        //#				value_string from CurrValueVarient using curr_format
                    }
                }// end-else  (not L or U or D)									
                 //#	
                 //#	Copy2Output(  value_string  )
                if (CopyToOutbuf(ref out_buf, ref remainingOut, temp_buf, -1) != 0)//testing retcode
                {// probable full buffer
                    break;// error exit
                }
                //#	Loop to the forever
            }// loop for ever
             //DEBUGLOG(CLOG_LOG, L">>>|%s|<<<\n", out_buf);

            return retCode;
        }// end of bltin_format_string()

        bool doFormat(string formatStr, /*CDDLVar pV, ref */CValueVarient vValue, ref string retStr, int rsLen)
        {
            if (retStr == null)
            {
                return true;
            }
            if (formatStr == null || formatStr == "")
            {
                string t = vValue.GetDispString();
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
                return false; // works good
            }
            // else we have some unique formatting, we'll have to deal with it

            string pch = formatStr.Substring(formatStr.IndexOf('%'));// _tstrchar(formatStr, _T('%'));
                                                                     //string theChar = _T('\0');
            char theChar = '\0';
            if (pch != null) // real formatting
            {//	no spaces allowed in formatting, so get the last char
                theChar = pch[0];// start with the '%'
                foreach (char p in pch)
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
                    retStr = String.Format(formatStr, vValue.GetBool());
                    break;
                case 'd':
                case 'i':
                    //_tsprintf(retStr, formatStr, (int)vValue);
                    retStr = String.Format(formatStr, vValue.GetInt());
                    break;
                case 'o':
                case 'u':
                case 'x':
                case 'X':
                    //_tsprintf(retStr, formatStr, (uint)vValue);
                    retStr = String.Format(formatStr, vValue.GetUInt());
                    break;
                case 'e':
                case 'E':
                case 'f':
                case 'g':
                case 'G':
                    if (vValue.vIsDouble)
                    {
                        double d = vValue.GetDouble();
                        //_tsprintf(retStr, formatStr, d);
                        retStr = String.Format(formatStr, d);
                    }
                    else
                    {
                        float y = (float)vValue.GetFloat();
                        //_tsprintf(retStr, formatStr, y);
                        retStr = String.Format(formatStr, y);
                    }
                    break;
                case 's':
                case 'S':
                    //_tsprintf(retStr, formatStr, ((string)vValue).c_str());
                    retStr = String.Format(formatStr, vValue.GetString());
                    break;
                case '%':
                default:
                    //_tsprintf(retStr, formatStr);
                    retStr = formatStr;
                    break;
            }
            return false; // no error
        }

        int DELAY(int iTimeInSeconds, string prompt)
        {

            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = "";//[MAX_LEN_ALLOC] = { 0 };
            string out_prompt = "";//[MAX_LEN_ALLOC] = { 0 };
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50];  //16APR2004Added by ANOOP 
                                                                  //clearVarientArray(pDynVarVals);
            bool bDynVarValsChanged = false;
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */


            int rc = DDlDevDescription.pGlobalDict.get_string_translation(prompt, ref out_prompt);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                                   out_prompt, null, 0, ref pDynVarVals, ref bDynVarValsChanged);

            if (structUIData.textMessage.pchTextMessage != null)
            {
                structUIData.textMessage.pchTextMessage = null;
            }
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;
            if (out_buf.Length > 0)
            {
                structUIData.textMessage.pchTextMessage = out_buf;
                structUIData.textMessage.iTextMessageLength = out_buf.Length;
            }
            else
            {
                structUIData.textMessage.iTextMessageLength = 0;
            }

            structUIData.bUserAcknowledge = false;
            /*Vibhor 030304: Start of Code*/
            if (!(this.m_AbortInProgress))
                structUIData.bEnableAbortOnly = true;
            else
            {
                structUIData.bEnableAbortOnly = false;
            }
            // Need the abort button
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = (ushort)(iTimeInSeconds * 1000);
            /*Vibhor 040304: End of Code*/

            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }

            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;  //Added by ANOOP 20FEB2004
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            /*Vibhor 040304: Start of Code*/
            /*	else // Now this is handled in MethSupport.cpp
				{
					Sleep((uint)iTimeInSeconds * 1000);
				}
			*/
            /*Vibhor 040304: End of Code*/

            /*<START>Added by ANOOP 20APR2004 Delay Builtin shud support dynamic vars	*/
            if (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
            {
                long dwStartTime = DateTime.Now.Ticks;//ddbGetTickCount();//GetTickCount();
                long dwEndTime = dwStartTime;

                out_buf = mt_String;
                while ((dwEndTime - dwStartTime) < (structUIData.uDelayTime) &&
                         true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynVarValsChanged = false;
                    out_buf = "";
                    retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, out_prompt,
                                                    null, 0, ref pDynVarVals, ref bDynVarValsChanged);
                    if (true == bDynVarValsChanged)
                    {
                        Common.add_textMsg(ref structUIData, out_buf);
                    }
                    //=============================================================================================
                    // has to be after the format so bEnableDynamicDisplay is preserved 4 while test
                    if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    //=============================================================================================
                    {
                        bSetAbortFlag = true;// must be after the bltin_format_string is called or they will be reset
                        break;
                    }
                    //systemSleep(100); // use 100 due to timing - will give 100 mS time jitter
                    Thread.Sleep(100);
                    dwEndTime = DateTime.Now.Ticks;
                }//wend till disabled
            }
            /*<END>Added by ANOOP 20APR2004 Delay Builtin shud support dynamic vars	*/

            if (null != structUIData.textMessage.pchTextMessage)
            {
                structUIData.textMessage.pchTextMessage = null;
            }

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int delay(int iTimeInSeconds, string pchDisplayString, int[] lItemId, int iNumberOfItemIds)
        {

            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = null;//[MAX_LEN_ALLOC] = { 0 };
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50];  //16APR2004Added by ANOOP 
                                                                  //clearVarientArray(pDynVarVals);
            bool bDynVarValsChanged = false;
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */
            // stevev 26dec07 - the prompt string may be more than one language (not a dictStr)
            //					as such, one translation will alwauys be shorter than the whole.
            DDlDevDescription.pGlobalDict.get_string_translation(pchDisplayString, ref pchDisplayString);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                            lItemId, iNumberOfItemIds, ref pDynVarVals, ref bDynVarValsChanged);

            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;

            structUIData.bUserAcknowledge = false;
            /*Vibhor 030304: Start of Code*/
            if (!(this.m_AbortInProgress))
            {
                structUIData.bEnableAbortOnly = true; // Need the abort button
            }
            else
            {
                structUIData.bEnableAbortOnly = false;
            }
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = (ushort)(iTimeInSeconds * 1000);
            /*Vibhor 040304: End of Code*/

            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }

            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;	//Added by ANOOP 20FEB2004

            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }

            /*<START>Added by ANOOP 20APR2004 Delay Builtin shud support dynamic vars	*/
            if (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
            {

                long dwStartTime = DateTime.Now.Ticks;//ddbGetTickCount();//**** This will wrap every 47 days that the computer runs!!!
                long dwEndTime = dwStartTime;

                while (structUIData.uDelayTime > (dwEndTime - dwStartTime) &&
                       true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynVarValsChanged = false;
                    out_buf = "";
                    retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                                        pchDisplayString, lItemId, iNumberOfItemIds, ref pDynVarVals,
                                        ref bDynVarValsChanged);

                    if (true == bDynVarValsChanged)
                    {
                        Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code			
                    }
                    if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    //systemSleep(100);// we need to give some time to the winMain to update its display
                    Thread.Sleep(100);
                    dwEndTime = DateTime.Now.Ticks; //ddbGetTickCount();
                }//wend till disabled
            }
            /*<END>Added by ANOOP 20APR2004 Delay Builtin shud support dynamic vars	*/


            if (null != structUIData.textMessage.pchTextMessage)
            {
                //delete[] structUIData.textMessage.pchTextMessage;
                structUIData.textMessage.pchTextMessage = null;
            }

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int ACKNOWLEDGE(string message)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = "";
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            bool bDynaVarValChanged = false;
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */

            int rc = DDlDevDescription.pGlobalDict.get_string_translation(message, ref message);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                                        message, null, 0, ref pDynVarVals, ref bDynaVarValChanged);

            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;

            //Anil September 23 2005 has added this as return value was not checked,
            if (retval == BI_ERROR)
            {
                //TODO what to return as 
                //     this Function is not handling any return value other than METHOD_ABORTED
            }

            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/
            /*Vibhor 040304: Comment: Added the second condition below*/
            //                                                 always true
            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;	//Added by ANOOP 20FEB2004
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            {
                out_buf = mt_String;
                while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynaVarValChanged = false;
                    retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                                            message, null, 0, ref pDynVarVals, ref bDynaVarValChanged);

                    if (true == bDynaVarValChanged)
                    {
                        Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code		
                    }
                    //=============================================================================================
                    // has to be after the format so bEnableDynamicDisplay is preserved 4 while test
                    if (false ==
                       m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    //=============================================================================================
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    Thread.Sleep(600);
                }//wend till disabled
            }

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            structUIData.bDisplayDynamic = false;  //Added by ANOOP 200204

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int acknowledge(string message, int[] glob_var_ids, int iNumberOfItemIds)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string disp_msg = "";
            bool bSetAbortFlag = false;
            /*<START>Added by ANOOP for dynamic vars %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            //bool bRefreshDynamicVars = true;
            bool bDynaVarValChanged = false;

            // stevev 02Jun14 - now set in bltin_format_string
            // m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay=false;
            /*<END>Added by ANOOP for dynamic vars %0 */

            int rc = DDlDevDescription.pGlobalDict.get_string_translation(message, ref message);
            int retval = bltin_format_string(ref disp_msg, MAX_LEN_ALLOC, UPDATE_NORMAL, message,
                            glob_var_ids, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);

            Common.add_textMsg(ref structUIData, disp_msg);    // stevev 26dec07 - common code
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;

            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/

            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;  //Added by ANOOP 200204
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            {
                disp_msg = mt_String;
                while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynaVarValChanged = false;
                    retval = bltin_format_string(ref disp_msg, MAX_LEN_ALLOC, UPDATE_NORMAL, message,
                                glob_var_ids, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);
                    if (true == bDynaVarValChanged)
                    {
                        Common.add_textMsg(ref structUIData, disp_msg);    // stevev 26dec07 - common code		
                    }
                    if (false ==
                        m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    Thread.Sleep(600);
                }//wend till disabled
            }
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            structUIData.bDisplayDynamic = false;  //Added by ANOOP 200204

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int put_message(string message, int[] glob_var_ids, int iNumberOfItemIds)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = "";
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50];  //16APR2004Added by ANOOP 
                                                                  //clearVarientArray(pDynVarVals);
            bool bDynVarValsChanged = false;
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */

            int rc = DDlDevDescription.pGlobalDict.get_string_translation(message, ref message);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                            message, glob_var_ids, iNumberOfItemIds, ref pDynVarVals, ref bDynVarValsChanged);

            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;

            structUIData.bUserAcknowledge = false;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/
            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            structUIData.bDisplayDynamic = false;   //Added by ANOOP 20FEB2004
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            //Sleep(1000);
            //m_pDevice.m_pMethSupportInterface.SleepWithMessageLoop(1000);		
            Thread.Sleep(1000);

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int Read(uint item_id, ref CValueVarient ppReturnedDataItem, bool bScalingReqd)
        {
            CDDLBase pIB = null;
            CDDLVar pVar = null;
            CDDLCmd pC = null;
            CCmdList pCmdList = null;

            if (m_pDevice.getItembyID(item_id, ref pIB))
            {
                if (null != pIB)
                {
                    pVar = (CDDLVar)pIB;

                    INSTANCE_DATA_STATE_T ids = pVar.getDataState();
                    DATA_QUALITY_T dq = pVar.getDataQuality();

                    if (dq == DATA_QUALITY_T.DA_NOT_VALID || dq == DATA_QUALITY_T.DA_STALEUNK
                            || ids == INSTANCE_DATA_STATE_T.IDS_UNINITIALIZED || ids == INSTANCE_DATA_STATE_T.IDS_INVALID
                            || (ids == INSTANCE_DATA_STATE_T.IDS_STALE && dq == DATA_QUALITY_T.DA_STALEUNK))
                    {

                        hCcommandDescriptor rdc = pVar.getRdCmd();
                        //				localIdxUse =  rdc;
                        if (rdc.cmdNumber < 0xFFFF /*0xFF Commented by Anil October 25 2005 and Changed to 0xFFFF PAR 5539 */
                            && rdc.cmdNumber > -1)
                        {
                            pCmdList = (CCmdList)m_pDevice.getListPtr(itemType_t.iT_Command);//<hCcommand*> 				

                            pC = pCmdList.getCmdByNumber(rdc.cmdNumber);
                            /* stevev 16apr07 the above is easier to maintain...
							// get cmd ptr
							for (CCmdList::iterator iCT = pCmdList.begin(); 
								 iCT < pCmdList.end();                 iCT++ )
							{// iCT is ptr 2 ptr to hCcommand
								if ( rdc.cmdNumber == (*iCT).getCmdNumber() )
								{
									pC = (hCcommand*)(*iCT);
									break; // out of for loop
								}
							}// next command
							....***/
                            if (pC != null)// command supported
                            {
                                /*Vibhor 220204: Start of Code*/
                                // was	int retVal=m_pDevice.sendMethodCmd( rdc.cmdNumber,0);
                                // stevev 30nov11 - added index list
                                int retVal = (int)m_pDevice.sendMethodCmd(rdc.cmdNumber, DEFAULT_TRANSACTION_NUMBER);
                                /*Vibhor 220204: End of Code*/
                                if (retVal != BI_SUCCESS)
                                {
                                    //call abort method
                                    return BI_ERROR; // Vibhor 220204
                                }
                            }
                        }// if there's no read Command for the Var, just fall through...
                    }
                    /*Vibhor 270204: Start of Code*/
                    if (bScalingReqd)
                    {
                        ppReturnedDataItem = pVar.getDispValue(); /* VMKP removed repetative code from two 
													 places above and put it in common place 
													on 200204 */
                    }
                    else
                    {//Scaling not required !!
                        ppReturnedDataItem = pVar.getRawDispValue();
                    }
                    /*Vibhor 270204: End of Code*/
                }
                return BI_SUCCESS;
            }
            else
            {
                return BI_ERROR;

            }

        }



        int Write(uint item_id, CValueVarient ppReturnedDataItem)
        {
            CDDLBase pIB = null;
            CDDLVar pVar = null;
            CDDLCmd pC = null;
            CCmdList pCmdList = null;

            if (m_pDevice.getItembyID(item_id, ref pIB))
            {
                if (null != pIB)
                {
                    pVar = (CDDLVar)pIB;

                    INSTANCE_DATA_STATE_T ids = pVar.getDataState();
                    DATA_QUALITY_T dq = pVar.getDataQuality();

                    if (ids != INSTANCE_DATA_STATE_T.IDS_CACHED)
                    {
                        /* stevev 01oct08 -- the assumption here, that the command will be successful, has no basis.
						   we will leave the command dispatcher to decide if the value becomes cached
						pVar.markItemState(IDS_CACHED);
						   note that this may be an issue for write commands that don't 'echo-back' their values.
						   If this occurs, we will need to determine a better solution
						***/

                        hCcommandDescriptor rdc = pVar.getWrCmd();
                        if (rdc.cmdNumber < 0xFFFF /* stevev 16apr07 caught in walk thru 0xFF*/ && rdc.cmdNumber > -1)
                        {
                            pCmdList = (CCmdList)m_pDevice.getListPtr(itemType_t.iT_Command);//<hCcommand*> 

                            pC = pCmdList.getCmdByNumber(rdc.cmdNumber);
                            /* stevev 16apr07 the above is easier to maintain...				
							// get cmd ptr
							for (CCmdList::iterator iCT = pCmdList.begin(); 
								 iCT < pCmdList.end();                iCT++ )
							{// iCT is ptr 2 ptr to hCcommand
								if ( rdc.cmdNumber == (*iCT).getCmdNumber() )
								{
									pC = (hCcommand*)(*iCT);
									break; // out of for loop
								}
							}// next command
							.../***/
                            if (pC != null)// command supported
                            {// stevev 30nov11 - change transaction to default, add index list
                                int retVal = (int)m_pDevice.sendMethodCmd(rdc.cmdNumber, DEFAULT_TRANSACTION_NUMBER);
                                if (retVal != BI_SUCCESS)
                                {
                                    //call abort method
                                }

                            }
                        }

                    }
                    pVar.setDispValue(ppReturnedDataItem);
                }
                return BI_SUCCESS;
            }
            else
            {
                return BI_ERROR;
            }

        }

        public int _get_dev_var_value(string pchDisplayString, int[] lItemId1, int iNumberOfItemIds, uint lItemId)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = "";
            CDDLBase pIB = null;
            CValueVarient vvReturnedDataItem = new CValueVarient();
            EnumTriad_t localData = new EnumTriad_t();
            //int valLen;
            string finalstr = "";
            //bool memAlloc_Enum = false;//, memAlloc_String = false;
            uint nCntr = 0;
            CDDLVar pVar = null;
            //	hCenumList eList;
            bool bSetAbortFlag = false;
            /*<START>Added by ANOOP for dynamci vars %0  */
            CValueVarient[] pDynVarVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            bool bDynaVarValChanged = false;
            List<uint> uintList = new List<uint>();        //Vibhor 200605: added
            List<uint> uValidIndxList = new List<uint>(); //Vibhor 200605: added

            // memsets are not needed due to ctors in structures.

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>Added by ANOOP for dynamci vars %0  */
            int rc = DDlDevDescription.pGlobalDict.get_string_translation(pchDisplayString, ref pchDisplayString);

            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                           lItemId1, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);

            //	Go to dev obj and get to know the type of the variable as well as the min and max value.	

            if (m_pDevice.getItembyID(lItemId, ref pIB) && pIB.IsVariable())
            {
                pVar = (CDDLVar)pIB;
                // stevev 08feb11 - get all formatting information at once
                int rgt = 0;
                int max = 0;
                string edt = "";//, dsp, scn;
                pVar.getEditFormatInfo(ref max, ref rgt, ref edt);//////to be continued

                //Anil Done changes for the Pre Edit aActions from the method 21 December 2005
                List<int> actionList = new List<int>();
                varAttrType_t actionType = varAttrType_t.varAttrPreEditAct;

                //Get all the pre edit actions for this variable
                pVar.getActionList(actionType, ref actionList);

                //Go through each of Pre edit actions and Execute it						
                int iReturnVal = m_pMeth.ExecuteActionsInMethod(actionList);
                if (0 != iReturnVal)
                {
                    return BI_ERROR;
                }
                structUIData.DDitemId = lItemId;
                structUIData.pVar4ItemID = pVar;

                // stevev 22may07 fix the index variable editting in method dialogs
                variableType_t locType = pVar.VariableType();
                // treat non-item arrays as integers
                /*
				if (locType == variableType_t.vT_Index)
				{
					CDDLArray pIndexedItem = null;
					
					//pIB = null;
					if (((hCindex)pVar).pIndexed.getArrayPointer(pIB) != SUCCESS ||
						pIB == null || (pIB.eType != nitype.nItemArray &&
										 pIB.eType != nitype.nArray &&
										 pIB.eType != nitype.nList))
					{
						locType = variableType_t.vT_Unsigned;// treat it like an int
					}// else just keep going
					else // is one of three indexed types
					if (pIB.eType == nitype.nItemArray)
					{// index must display descriptions
					 // leave locType as vT_Index
						pIndexedItem = (hCitemArray)pIB;// a no-op
					}
					else
					{//iT_Array,iT_List - index shouldn't try to display descriptions
						locType = variableType_t.vT_Unsigned;// treat it like an int
						pIndexedItem = null;
					}
				}
				// end stevev 22may07 
				*/
                switch (locType)
                {
                    case variableType_t.vT_Enumerated:
                        {
                            structUIData.userInterfaceDataType = UI_DATA_TYPE.COMBO;
                            structUIData.ComboBox.comboBoxType = COMBO_BOX_TYPE.COMBO_BOX_TYPE_SINGLE_SELECT;

                            /*
							hCEnum pEn = null;
							pEn = (hCEnum)pIB;

							hCenumList eList(pEn.devHndl());
							uValidIndxList.Clear();
							if (pEn.procureList(eList) != SUCCESS)
							{
								return BI_ERROR;
							}
							else
							*/
                            {
                                CValueVarient cvIndexValue = new CValueVarient();
                                //cvIndexValue = pEn.getDispValue();

                                if (Read(lItemId, ref cvIndexValue, true) != BI_SUCCESS)
                                {
                                    return BI_ERROR;
                                }
                                finalstr = mt_String;

                                structUIData.EditBox.nSize = pVar.enmList.maxDescLen();// eList.maxDescLen();// stevev 08feb11     pEn.VariableSize();

                                uint nCurrentValue = cvIndexValue.GetUInt();
                                nCntr = 0; // yes, it duplicates the original clear

                                structUIData.ComboBox.nCurrentIndex = 0xffffffff;// -1 into unsigned 
                                foreach (EnumTriad_t iT in pVar.enmList)
                                {//iT isa ptr 2 hCenumDesc
                                    localData = iT;                    // a EnumTriad_t
                                    uint nPosition = localData.val;

                                    if (finalstr == "")
                                    {
                                        finalstr = localData.descS;
                                    }
                                    else
                                    {
                                        finalstr += ";";
                                        finalstr += localData.descS;
                                    }
                                    //stevev - these are not required to be contiguous
                                    uValidIndxList.Add(nPosition);

                                    if (nCurrentValue == nPosition)
                                    {
                                        structUIData.ComboBox.nCurrentIndex = nCntr;
                                    }
                                    nCntr++;
                                }// next enumerated value

                                if (structUIData.ComboBox.nCurrentIndex == 0xffffffff)
                                {// we never found a match <current value is not a valid one>
                                    string tmp = String.Format("0x%02x NoEnumeration", nCurrentValue);
                                    finalstr += ";";
                                    finalstr += tmp;

                                    uValidIndxList.Add(nCurrentValue);

                                    structUIData.ComboBox.nCurrentIndex = nCntr;
                                    nCntr++;
                                }

                            }// endelse we got the list of triads					 

                            if (finalstr != "")
                            {
                                //memAlloc_Enum = true;
                                structUIData.ComboBox.pchComboElementText = finalstr;
                                //stevev 23oct09 - copy the trailing null too
                                structUIData.ComboBox.iNumberOfComboElements = (int)nCntr;
                                // done at match...structUIData.ComboBox.nCurrentIndex = nIndex;
                            }
                            break;
                        }
                    case variableType_t.vT_BitEnumerated:
                        {
                            structUIData.userInterfaceDataType = UI_DATA_TYPE.COMBO;
                            structUIData.ComboBox.comboBoxType = COMBO_BOX_TYPE.COMBO_BOX_TYPE_MULTI_SELECT;

                            /*
							hCBitEnum* pEn = null;
							pEn = (hCBitEnum*)pIB;

							hCenumList eList(pEn.devHndl());
							uValidIndxList.Clear();
							if (pEn.procureList(eList) != SUCCESS)
							{
								return BI_ERROR;
							}
							else
							*/
                            {
                                CValueVarient cvIndexValue = new CValueVarient();//WS:EPM 30apr07
                                                                                 //cvIndexValue = pEn.getRawDispValue();//WS:EPM 30apr07

                                if (Read(lItemId, ref cvIndexValue, true) != BI_SUCCESS)
                                {
                                    return BI_ERROR;
                                }
                                finalstr = mt_String;

                                structUIData.EditBox.nSize = pVar.enmList.maxDescLen();// stevev 08feb11     pEn.VariableSize();

                                uint nCurrentValue = cvIndexValue.GetUInt();
                                nCntr = 0;

                                structUIData.ComboBox.nCurrentIndex = 0xffffffff;// -1 into unsigned 
                                foreach (EnumTriad_t iT in pVar.enmList)
                                {//iT isa ptr 2 hCenumDesc
                                    localData = iT;                    // a EnumTriad_t
                                    uint nPosition = localData.val;

                                    //WS:EPM 30apr07 - start section
                                    //if (nCntr >= (valLen * 8))
                                    //{// stevev-21may07-from::  MAXIMUM_NUMBER_OF_BITS_IN_BITENUM )
                                    //break;//if there is more than 32 bits here;  we have problems.
                                    //}
                                    //WS:EPM 30apr07 end section
                                    //do we have a description
                                    string ss = localData.descS;
                                    if (nCntr == 0) // stevev-21may07-from::   finalstr.empty() )
                                    {
                                        finalstr = ss;
                                    }
                                    else
                                    {
                                        finalstr += ";";
                                        finalstr += ss;
                                    }
                                    /* the current value probably has several bits set, just send it
									uValidIndxList.Add((UINT32)nPosition);//stevev - these are not required to be contiguous

									if(nCurrentValue == nPosition)
									{
										structUIData.ComboBox.nCurrentIndex = nCntr;
									}
									*/

                                    structUIData.ComboBox.m_lBitValues[nCntr] = nPosition;
                                    //stevev 22may07..changed from::>  localData.val;//WS:EPM 30apr07
                                    nCntr++;
                                }// next desc
                                 // if no match,
                                structUIData.ComboBox.nCurrentIndex = nCurrentValue; //several bits set
                            }

                            if (finalstr != null)
                            {
                                //memAlloc_Enum = true;
                                structUIData.ComboBox.pchComboElementText = finalstr;
                                structUIData.ComboBox.iNumberOfComboElements = (int)nCntr;
                            }
                            break;
                        }
                    /*
					case vT_Password:
					{
						structUIData.userInterfaceDataType= EDIT;	
						structUIData.EditBox.editBoxType  = EDIT_BOX_TYPE_PASSWORD;
						if( Read(lItemId,structUIData.EditBox.editBoxValue,true) == BI_SUCCESS )
						{	
							//if (structUIData.EditBox.editBoxValue.vSize > 0)
							//{
							//	memAlloc_String = true;
							//	structUIData.EditBox.pchDefaultValue = new char[ structUIData.EditBox.editBoxValue.vSize +1 ];
							//	strcpy(structUIData.EditBox.pchDefaultValue,
							//		  ((string)structUIData.EditBox.editBoxValue).c_str());
							//	structUIData.EditBox.iDefaultStringLength = nLen_str;
							//}
							//structUIData.EditBox.iMaxStringLength=pVar.VariableSize(); //WS:EPM 30apr07	

							structUIData.EditBox.nSize = pVar.VariableSize(); 
						}
						else
						{
							return BI_ERROR;
						}	
						break;				
					}
					*/
                    case variableType_t.vT_Password:
                    case variableType_t.vT_Ascii:
                    case variableType_t.vT_PackedAscii:
                        {
                            /*<START>Commented by ANOOP 25MAR2004 No need of any conversions from packed ascii to ascii
							char pch_PackedAscii[1024]={0};
							int nLen_PackedAscii;
							<END>Commented by ANOOP 25MAR2004	*/
                            //char pch_Ascii[MAX_LEN_ALLOC]={0};				

                            structUIData.userInterfaceDataType = UI_DATA_TYPE.EDIT;

                            if (locType == variableType_t.vT_Password)
                                structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_PASSWORD;
                            else
                                structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_STRING;

                            if (Read(lItemId, ref structUIData.EditBox.editBoxValue, true) == BI_SUCCESS)
                            {   /*			
					// we wouldn't be here if we weren't one of these...
					//if( pVar.VariableType() == vT_PackedAscii || pVar.VariableType() == vT_Ascii)
					//{
						if( vvReturnedDataItem.vType == vvReturnedDataItem.isWideString )
						{
							wcstombs( pch_Ascii, vvReturnedDataItem.sWideStringVal.c_str(), sizeof(pch_Ascii) );
						}
						else 
						if( vvReturnedDataItem.vType == vvReturnedDataItem.isString )
						{
							strcpy(pch_Ascii,vvReturnedDataItem.sStringVal.c_str()  );
						}
					//}
					
					if( strlen(pch_Ascii) > 0)
					{
						memAlloc_String=true;
						structUIData.EditBox.pchDefaultValue =new char[strlen(pch_Ascii) +1];
						strcpy(structUIData.EditBox.pchDefaultValue,pch_Ascii); 
						structUIData.EditBox.iDefaultStringLength=strlen(pch_Ascii);
					}
					else
					{
						structUIData.EditBox.pchDefaultValue = null; 
						structUIData.EditBox.iDefaultStringLength=0;
					}	
					structUIData.EditBox.iMaxStringLength=pVar.VariableSize();
					*/
                                structUIData.EditBox.nSize = (uint)max + 1;// stevev 08feb11 
                            }
                            else
                            {
                                return BI_ERROR;
                            }

                            break;
                        }
                    case variableType_t.vT_TimeValue:
                        {
                            //string sTmp;

                            structUIData.userInterfaceDataType = UI_DATA_TYPE.TIME;
                            structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_TIME;

                            if (Read(lItemId, ref structUIData.EditBox.editBoxValue, true) == BI_SUCCESS)
                            {
                                /*
								hCTimeValue	*pTime=(hCTimeValue	*)pVar;	
								sTmp = pTime.getDispValueString(); 
								int	nTmplen = sTmp.length();
								if( nTmplen >0 )
								{
									structUIData.datetime.pchHartDate=new string[nTmplen +1];
									_tstrcpy(structUIData.datetime.pchHartDate,sTmp.c_str()); 
									structUIData.EditBox.iDefaultStringLength = nTmplen +1;
								}
								else
								{
									structUIData.datetime.pchHartDate =new string[8];//MAX_LEN_ALLOC];
									_tstrcpy(structUserInput.datetime.pchHartDate,mt_String); 
								}
								structUIData.EditBox.iMaxStringLength=MAX_LEN_ALLOC -1;
								*/
                                /* stevev 21jul09 - bogus assumption::>
								structUIData.EditBox.nSize = structUIData.EditBox.editBoxValue.vSize; 
								<<:: replace with the following ::>>*/
                                structUIData.EditBox.nSize = (uint)max + 1;

                            }
                            else
                            {
                                return BI_ERROR;
                            }
                        }
                        break;


                    case variableType_t.vT_Integer:
                    case variableType_t.vT_Unsigned:
                    case variableType_t.vT_FloatgPt:
                    case variableType_t.vT_Double:
                        //case  vT_BitString:
                        //case  vT_VisibleString:
                        {
                            structUIData.userInterfaceDataType = UI_DATA_TYPE.EDIT;
                            structUIData.EditBox.nSize = pVar.VariableSize();
                            if (Read(lItemId, ref structUIData.EditBox.editBoxValue, true) == BI_SUCCESS)
                            {
                                if (structUIData.EditBox.editBoxValue.vType == valueType_t.isIntConst ||
                                structUIData.EditBox.editBoxValue.vType == valueType_t.isVeryLong)
                                {
                                    structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                                    //structUIData.EditBox.iValue=(__Int64)vvReturnedDataItem;
                                    //varient will coerce ints into __Int64
                                    //structUIData.EditBox.editBoxValue = vvReturnedDataItem;

                                    //string tmpStr;
                                    //((hCNumeric *) pVar).ReadForEdit(structUIData.EditBox.strEdtFormat);
                                    structUIData.EditBox.strEdtFormat = edt;
                                    //structUIData.EditBox.strEdtFormat = tmpStr;


                                    /*<START>Added by ANOOP to validata a list of ranges */

                                    /* stevev 28 may 09 - we are going to use the variable's range checking to deal with this.

															hCRangeList retList;
															MinMaxVal tmpMinMaxVal = new MinMaxVal();
															RangeList_t::iterator mmFnd;

															((hCinteger *)pVar).getMinMaxList(retList);

															for (mmFnd = retList.begin(); mmFnd != retList.end(); mmFnd++)
															{
																tmpMinMaxVal.IntMinMaxVal.iMinval = mmFnd.second.minVal;
																tmpMinMaxVal.IntMinMaxVal.iMaxval = mmFnd.second.maxVal;
																structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
															}
									//<END>Added by ANOOP to validate a list of ranges 
															end stevev 29 may09 */

                                    /*<START> Commented by ANOOP to validating a list of ranges
															structUIData.EditBox.iMinValue=INT_MIN;
															structUIData.EditBox. iMaxValue=INT_MAX;
									<END> Commented by ANOOP to validating a list of ranges	*/

                                }
                                else if (structUIData.EditBox.editBoxValue.vType == valueType_t.isFloatConst)
                                {// could be an int w/ float format	
                                    structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_FLOAT;
                                    //structUIData.EditBox.fValue=(float)vvReturnedDataItem;
                                    //structUIData.EditBox.editBoxValue = vvReturnedDataItem;
                                    //tstring tmpStr;
                                    //((hCFloat *) pVar).ReadForEdit(structUIData.EditBox.strEdtFormat);
                                    structUIData.EditBox.strEdtFormat = edt;
                                    //structUIData.EditBox.strEdtFormat=tmpStr;

                                    /* stevev 28 may 09 - we are going to use the variable's range checking to deal with this.
															hCRangeList retList;
															MinMaxVal tmpMinMaxVal = new MinMaxVal();
															RangeList_t::iterator mmFnd;


															((hCFloat *)pVar).getMinMaxList(retList);
															for (mmFnd = retList.begin(); mmFnd != retList.end(); mmFnd++)
															{ 
																tmpMinMaxVal.FloatMinMaxVal.fMinval   = mmFnd.second.minVal;
																tmpMinMaxVal.FloatMinMaxVal.fMaxval   = mmFnd.second.maxVal;
																structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
															}
															end stevev 29 may09 */
                                    /*<START> Commented by ANOOP to validating a list of ranges
															structUIData.EditBox.fMinValue=FLT_MIN;
															structUIData.EditBox.fMaxValue=FLT_MAX;
									<END> Commented by ANOOP to validating a list of ranges	*/
                                }
                                else if (structUIData.EditBox.editBoxValue.vType == valueType_t.isString)
                                {
                                    structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_STRING;
                                    //structUIData.EditBox.editBoxValue = structUIData.EditBox.editBoxValue;

                                    //char pchStrval[MAX_LEN_ALLOC]={0};

                                    //strcpy(pchStrval,structUIData.EditBox.editBoxValue.sStringVal.c_str() );
                                    //if( strlen(pchStrval) > 0)
                                    //{
                                    //	memAlloc_String=true;
                                    //	structUIData.EditBox.pchDefaultValue =new char[strlen(pchStrval) +1];
                                    //	strcpy(structUIData.EditBox.pchDefaultValue,pchStrval); 
                                    //	structUIData.EditBox.iDefaultStringLength=strlen(pchStrval);
                                    //}
                                    //else
                                    //{
                                    //	strcpy(structUIData.EditBox.pchDefaultValue,""); 
                                    //	structUIData.EditBox.iDefaultStringLength=0;
                                    //	structUIData.EditBox.iMaxStringLength=pVar.VariableSize();
                                    //}
                                    //structUIData.EditBox.iMaxStringLength=pVar.VariableSize();

                                    //structUserInput.EditBox.pchDefaultValue =new char[MAX_LEN_ALLOC];
                                    //strcpy(structUserInput.EditBox.pchDefaultValue,""); 
                                    //structUserInput.EditBox.iDefaultStringLength=MAX_LEN_ALLOC -1;
                                }
                                else if (structUIData.EditBox.editBoxValue.vType == valueType_t.isWideString)
                                {
                                    structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_STRING;
                                    // leave value as read above
                                }
                                // else all other types are discarded
                            }
                            else // Read() returned an error
                            {
                                return BI_ERROR;
                            }
                            break;
                        }
                    case variableType_t.vT_HartDate:
                        {
                            structUIData.userInterfaceDataType = UI_DATA_TYPE.HARTDATE;
                            structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_DATE;

                            if (Read(lItemId, ref structUIData.EditBox.editBoxValue, true) == BI_SUCCESS)
                            {
                                /*
								hChartDate	*pDate=(hChartDate	*)pVar;	
								pDate.Read(sTmpdate); 
								int		nTmplen=sTmpdate.length();
								if( nTmplen >0 )
								{
									structUIData.datetime.pchHartDate=new string[nTmplen +1];
									_tstrcpy(structUIData.datetime.pchHartDate,sTmpdate.c_str()); 
								}
								else
								{
								//	_tstrcpy(structUIData.datetime.pchHartDate,_T(""));
								//	structUserInput.datetime.pchHartDate =new string[MAX_LEN_ALLOC];
									structUIData.datetime.pchHartDate =new string[MAX_LEN_ALLOC];
								//	_tstrcpy(structUserInput.datetime.pchHartDate,mt_String);
									_tstrcpy(structUIData.datetime.pchHartDate,mt_String);
								}
								*/
                                structUIData.EditBox.nSize = (uint)max + 1;// stevev 08feb11
                                                                           // was structUIData.EditBox.editBoxValue.vSize; 
                            }
                            else
                            {
                                return BI_ERROR;
                            }

                            //structUserInput.datetime.pchHartDate =new string[MAX_LEN_ALLOC];
                            //_tstrcpy(structUserInput.datetime.pchHartDate,mt_String); 
                            //				structUserInput.datetime.pchHartDate=MAX_LEN_ALLOC -1;
                            break;
                        }
                    //PARFIX: 5544, the case for Index type variables was not there at all !!!
                    // stevev - 22may07 :: indexes may be item arrays, lists or value arrays
                    case variableType_t.vT_Index:  // only for item arrays, see filter before switch
                        {
                            structUIData.userInterfaceDataType = UI_DATA_TYPE.COMBO;
                            structUIData.ComboBox.comboBoxType = COMBO_BOX_TYPE.COMBO_BOX_TYPE_SINGLE_SELECT;

                            /*
							hCindex* pIndx = null;
							pIndx = (hCindex*)pIB;

							hCitemArray* pIndxdArr = null;
							hCgroupItemDescriptor* pGID = null;
							//CDDLBase pIndexedItem = null;// stevev - 22may07
							*/
                            uValidIndxList.Clear();
                            pIB = null;

                            // stevev - 22may07-if(	pIndx.pIndexed.getArrayPointer(pIndxdArr) ==
                            //											 SUCCESS && pIndxdArr != null)
                            if (pVar.VariableType() == variableType_t.vT_Index)//iT_Array,iT_List
                            {
                                CDDLBase ddi = new CDDLBase();
                                m_pDevice.getItembyID(pVar.vIndex.refId, ref ddi);
                                CValueVarient cvIndexValue = new CValueVarient();
                                if (Read(lItemId, ref cvIndexValue, true) != BI_SUCCESS)
                                {
                                    return BI_ERROR;
                                }
                                finalstr = mt_String;
                                structUIData.EditBox.nSize = pVar.VariableSize();
                                structUIData.ComboBox.nCurrentIndex = 0xffffffff;
                                uint nCurrentValue = (uint)cvIndexValue.GetUInt();
                                nCntr = 0; // yes, it duplicates the original clear

                                switch (ddi.eType)//////other types??????
                                {
                                    case nitype.nItemArray:
                                        CDDLItemArray itemarray = (CDDLItemArray)ddi;
                                        foreach (item_array it in itemarray.arrayitems)
                                        {
                                            uint uIndxVal = it.uiIndex;
                                            string tmpstr = it.desc;
                                            if (tmpstr == "")
                                            {//empty
                                                tmpstr = String.Format("%d", uIndxVal);
                                            }
                                            if (finalstr == "")
                                            {
                                                finalstr += tmpstr;
                                            }
                                            else
                                            {
                                                finalstr += ";";
                                                finalstr += tmpstr;
                                            }
                                            uValidIndxList.Add(uIndxVal);
                                            /* * *  WS:EPM 10aug07 * * */
                                            if (uIndxVal == nCurrentValue)
                                            {
                                                structUIData.ComboBox.nCurrentIndex = nCntr;
                                            }
                                            nCntr++;
                                            /* * end -- WS:EPM 10aug07 * * */
                                            // else getbyindex failed - skip it silently
                                            // stevev via HOMZ 21feb07 - pGID alloc'd in getbyindex
                                        }// next index
                                        break;

                                    case nitype.nList:
                                        CDDLList List = (CDDLList)ddi;
                                        /*
                                         * 
                                         * 
                                         */

                                        break;

                                    default:
                                        break;
                                }
                                if (structUIData.ComboBox.nCurrentIndex == 0xffffffff)
                                {// we never found a match <current value is not a valid one>
                                    string tmp = String.Format("0x%02x Nonexistent", nCurrentValue);
                                    finalstr += ";";
                                    finalstr += tmp;

                                    uValidIndxList.Add(nCurrentValue);

                                    structUIData.ComboBox.nCurrentIndex = nCntr;
                                    nCntr++;
                                }

                            }
                            else // iT_Array,iT_List  stevev - 22may07
                            {// treat it like an integer - should have been done before we got here
                                break;
                            }

                            if (finalstr != "")
                            {
                                //memAlloc_Enum = true;
                                structUIData.ComboBox.pchComboElementText = finalstr;
                                structUIData.ComboBox.iNumberOfComboElements = (int)nCntr;
                            }
                            break;
                        }

                    default:
                        break;
                }// end of switch case 


                UI_DATA_TYPE h = structUIData.userInterfaceDataType;
                Common.add_textMsg(ref structUIData, out_buf);     // stevev 26dec07 - common code
                structUIData.userInterfaceDataType = h; ;// sjv 19feb08 - restore held value

                structUIData.bUserAcknowledge = true;
                /*Vibhor 030304: Start of Code*/
                structUIData.bEnableAbortOnly = false; // just defensive
                /*Vibhor 030304: End of Code*/
                /*Vibhor 040304: Start of Code*/
                structUIData.uDelayTime = 0;// just defensive
                /*Vibhor 040304: End of Code*/
                /*Vibhor 040304: Comment: Added the second condition below*/

                if (m_pMeth.GetMethodAbortStatus())// always true,you just set it...&& (structUIData.bEnableAbortOnly == false))
                {
                    structUIData.bMethodAbortedSignalToUI = true;
                }
                else
                {
                    structUIData.bMethodAbortedSignalToUI = false;// defensive only
                }
                //=============================================================================================
                if (false ==
                       m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                //=============================================================================================
                {
                    bSetAbortFlag = true;
                }
                else
                {   //Anil Done changes for the post Edit aActions from the method 21 December 2005
                    actionList = new List<int>(); ;
                    actionType = varAttrType_t.varAttrPostEditAct;

                    //Get all the pre post actions for this variable
                    pVar.getActionList(actionType, ref actionList);

                    out_buf = mt_String;
                    while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                    {
                        bDynaVarValChanged = false;
                        bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                                                        pchDisplayString, lItemId1, iNumberOfItemIds,
                                                        ref pDynVarVals, ref bDynaVarValChanged);

                        if (true == bDynaVarValChanged)
                        {
                            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
                        }
                        //=============================================================================================
                        // has to be after the format so bEnableDynamicDisplay is preserved 4 while test
                        if (false ==
                           m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                        //=============================================================================================
                        {
                            bSetAbortFlag = true;
                            break;
                        }
                        Thread.Sleep(600);
                    }//wend till disabled

                    structUIData.bDisplayDynamic = false;   //Added by ANOOP 200204

                    // stevev - moved POST edit actions till the edit interface is completely back
                    //Go through each of post edit actions and Execute it							
                    iReturnVal = m_pMeth.ExecuteActionsInMethod(actionList);
                    if (0 != iReturnVal)
                    {
                        return BI_ERROR;
                    }

                    // Set user entered display value
                    switch (structUserInput.userInterfaceDataType)
                    {
                        case UI_DATA_TYPE.COMBO:
                            {
                                uint nSelect = structUserInput.nComboSelection;// stevev 22may07 -1;	
                                switch (pVar.VariableType())
                                {
                                    case variableType_t.vT_Enumerated:
                                        {
                                            nSelect -= 1;// index is zero based here stevev 22may07
                                            vvReturnedDataItem.SetValue(uValidIndxList[(int)nSelect], valueType_t.isIntConst);
                                            pVar.setDispValue(vvReturnedDataItem);
                                            break;
                                        }
                                    case variableType_t.vT_BitEnumerated:
                                        {
                                            // stevev 22may07 - bit-enum returns value of all bits set (not an index)
                                            vvReturnedDataItem.SetValue(uValidIndxList[(int)nSelect], valueType_t.isIntConst);
                                            pVar.setDispValue(vvReturnedDataItem);
                                            /* stevev 22may07 -  was::>   
											hCEnum* pEn = null;
											pEn = (hCBitEnum*)pIB;
											hCenumList eList(pEn.devHndl());
											if ( pEn.procureList(eList) != SUCCESS )
											{
												return BI_ERROR;
											}
											else
											{
												if( ((hCBitEnum*)pVar).procureList(eList)== SUCCESS && eList.size() > 0 ) 
												{
														vvReturnedDataItem = (int) eList[nSelect].val;
												}
												pVar.setDispValue(vvReturnedDataItem);	
											}
											** end was **/
                                            break;
                                        }

                                    //PARFIX: 5544, the case for Index type variables was not there at all !!!
                                    case variableType_t.vT_Index:
                                        {
                                            nSelect -= 1;
                                            vvReturnedDataItem.SetValue(uValidIndxList[(int)nSelect], valueType_t.isIntConst);
                                            pVar.setDispValue(vvReturnedDataItem);
                                        }
                                        break;

                                }//end switch varType

                                break;
                            }
                        case UI_DATA_TYPE.EDIT:
                            {
                                switch (structUserInput.EditBox.editBoxType)
                                {
                                    case EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER:
                                        {
                                            //vvReturnedDataItem = (Int64)structUserInput.EditBox.iValue;
                                            //vvReturnedDataItem = structUserInput.EditBox.editBoxValue;
                                            // stevev 27feb06
                                            if (!pVar.isInRange(structUserInput.EditBox.editBoxValue))
                                            {
                                                //TODO: now what
                                            }

                                            pVar.setDispValue(structUserInput.EditBox.editBoxValue);
                                            break;
                                        }
                                    case EDIT_BOX_TYPE.EDIT_BOX_TYPE_FLOAT:
                                        {
                                            //vvReturnedDataItem = (float)structUserInput.EditBox.fValue;
                                            //vvReturnedDataItem = structUserInput.EditBox.editBoxValue;
                                            /*m_pDevice.WriteImd (lItemId,&vvReturnedDataItem);*/
                                            /*if (Write(lItemId,vvReturnedDataItem) != SUCCESS)
											{
												return BI_ERROR;
											}*/
                                            pVar.setDispValue(structUserInput.EditBox.editBoxValue);
                                            break;
                                        }
                                    case EDIT_BOX_TYPE.EDIT_BOX_TYPE_STRING:
                                    case EDIT_BOX_TYPE.EDIT_BOX_TYPE_PASSWORD:
                                        {
                                            //vvReturnedDataItem = 
                                            //				(char *)structUserInput.EditBox.pchDefaultValue;
                                            pVar.setDispValue(structUserInput.EditBox.editBoxValue);
                                            break;
                                        }
                                    case EDIT_BOX_TYPE.UNKNOWN_EDIT_BOX_TYPE:
                                        {
                                            break;
                                        }
                                }
                                break;

                            }
                        case UI_DATA_TYPE.HARTDATE:
                            {
                                //hChartDate* pDate = (hChartDate*)pVar;
                                //vvReturnedDataItem = (string *)structUserInput.datetime.pchHartDate;
                                //pDate.setDispValue(vvReturnedDataItem);
                                pVar.setDateDisplayValueString(structUserInput.EditBox.editBoxValue);
                                //if(structUserInput.datetime.pchHartDate)
                                //{
                                //	delete[] structUserInput.datetime.pchHartDate;
                                //	if(structUIData.datetime.pchHartDate)
                                //	{
                                //		delete[]  structUIData.datetime.pchHartDate;
                                //	}
                                //}
                                break;
                            }
                        case UI_DATA_TYPE.TIME:
                            {
                                //hCTimeValue* pTime = (hCTimeValue*)pVar;
                                //string tmpStr;
                                //tmpStr = (string *)structUserInput.datetime.pchHartDate;
                                //was vvReturnedDataItem = (string *)structUserInput.datetime.pchHartDate;
                                //pTime.setDispValue(vvReturnedDataItem);
                                pVar.setTimeDisplayValueString(structUserInput.EditBox.editBoxValue);
                                /*
								if(structUserInput.datetime.pchHartDate)
								{
									delete[] structUserInput.datetime.pchHartDate;
									structUserInput.datetime.pchHartDate = null;
									if(structUIData.datetime.pchHartDate)
									{
										delete[]  structUIData.datetime.pchHartDate;
										structUIData.datetime.pchHartDate = null;
									}
								}*/
                            }
                            break;
                    }// end switch	userInterfaceDataType
                }//end else methodDisplay was success
            }//endif getbysymbolnumber success   AND  IsVariable.......else - falls thru to do nothing

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }// end _get_dev_var_value

        int PUT_MESSAGE(string message)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = "";
            //int iTimeInSeconds = PUT_MESSAGE_SLEEP_TIME;  //preset Time for the message to be displayed
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50];  //16APR2004Added by ANOOP 
                                                                  //clearVarientArray(pDynVarVals);
            bool bDynVarValsChanged = false;
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */

            int rc = DDlDevDescription.pGlobalDict.get_string_translation(message, ref message);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC/*was::>strlen(message)*/,
                             updatePermission_t.up_DONOT_UPDATE, message, null, 0, ref pDynVarVals, ref bDynVarValsChanged);

            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;

            structUIData.bUserAcknowledge = false;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/

            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            structUIData.bDisplayDynamic = false;// stevev always...	//Added by ANOOP 20FEB2004
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            //Sleep(1000);
            //m_pDevice.m_pMethSupportInterface.SleepWithMessageLoop(1000);		
            Thread.Sleep(1000);

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }

        }

        int _get_local_var_value(string pchDisplayString, int[] plItemIds, int iNumberOfItemIds, string pchVariableName)
        {

            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string out_buf = "";
            //string strVar_name = "";
            //string curr_ptr = null;
            //CDDLBase pIB = null;
            CValueVarient vvReturnedDataItem = new CValueVarient();
            INTER_VARIANT varVal = new INTER_VARIANT();
            VARIANT_TYPE varVariantType; // we have to remember this across display execution
            bool bSetAbortFlag = false;
            /*<START>Added by ANOOP for dynamci vars %0  */
            CValueVarient[] pDynVarVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            bool bDynaVarValChanged = false;

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>Added by ANOOP for dynamci vars %0  */

            Common.captureStartTime();

            int rc = DDlDevDescription.pGlobalDict.get_string_translation(pchDisplayString, ref pchDisplayString);

            //DEBUGLOG(CLOG_LOG, L"_get_dev_var_value:: prompt>  '%s'\n", pchDisplayString);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                plItemIds, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);
            // WS:EPM 17jul07 checkin - start
            // remove trailing whitespace from the char* (could be in InterpretedVisitor.cpp in visitFunctionExpression(...)) SF:EPM
            string sTrim = pchVariableName;
            int nLastPosition = sTrim.Length - 1;         // get the last position of the array
                                                          // stevev 26oct10 - deal with empty strings and the like...
            while (sTrim.Length > 0 && nLastPosition >= 0 && Char.IsWhiteSpace(sTrim[nLastPosition]))
            {
                sTrim.Remove(nLastPosition);                 // remove the whitespace
                pchVariableName = sTrim;
                nLastPosition = sTrim.Length - 1;         // get the new last position of the array
            }
            // WS:EPM 17jul07 checkin - end
            //	Get the value of the variable as well as the min and max value.	
            CDDLVar tmp = null;
            m_pInterpreter.GetVariableValue(pchVariableName, ref varVal, ref tmp);

            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
                                                           //add_textMsg sets the structUIData.userInterfaceDataType == TEXT_MESSAGE

            structUIData.userInterfaceDataType = UI_DATA_TYPE.EDIT;
            varVariantType = varVal.GetVarType();

            switch (varVariantType)
            {
                case VARIANT_TYPE.RUL_CHAR:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(int)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((byte)varVal.GetVarInt(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 1;
                        /*<START>Added by stevev 27feb06 for validating the list of  ranges */
                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = Common.SCHAR_MIN;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Common.SCHAR_MAX;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                        /*<END>Added by stevev for validating the list of  ranges */
                    }
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(int)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((byte)varVal.GetVarByte(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 1;

                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = 0;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Common.UCHAR_MAX;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                    }
                    break;
                case VARIANT_TYPE.RUL_INT:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(int)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((int)varVal.GetVarInt(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 4;
                        /*<START>Added by ANOOP for validating the list of  ranges */
                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = Common.INT_MIN;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Common.INT_MAX;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);

                        /*			structUIData.EditBox.iMinValue=INT_MIN;
                                    structUIData.EditBox.iMaxValue=INT_MAX;
                        /*<END>Added by ANOOP for validating the list of  ranges */
                    }
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(int)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((uint)varVal.GetVarUInt(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 4;

                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = 0;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Common.UINT_MAX;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                    }
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(int)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((short)varVal.GetVarInt(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 2;

                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = Common.SHRT_MIN;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Common.SHRT_MAX;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                    }
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(int)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((ushort)varVal.GetVarUInt(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 2;

                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = 0;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Common.USHRT_MAX;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                    }
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(Int64)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((Int64)varVal.GetVarInt64(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 8;

                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = Int64.MinValue;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Int64.MaxValue;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                    }
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER;
                        //structUIData.EditBox.iValue=(Int64)varVal;
                        structUIData.EditBox.editBoxValue.SetValue((UInt64)varVal.GetVarInt64(), valueType_t.isIntConst);
                        structUIData.EditBox.nSize = 8;

                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.IntMinMaxVal.iMinval = 0;
                        tmpMinMaxVal.IntMinMaxVal.iMaxval = Int64.MaxValue;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);
                    }
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_FLOAT;
                        //structUIData.EditBox.fValue=(float)varVal; 
                        structUIData.EditBox.editBoxValue.SetValue((float)varVal.GetVarFloat(), valueType_t.isIntConst);
                        /*<START>Added by ANOOP for validating the list of  ranges */
                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.FloatMinMaxVal.fMinval = float.MinValue;  //Code corrected in the code review 
                        tmpMinMaxVal.FloatMinMaxVal.fMaxval = float.MaxValue;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);

                        /*			structUIData.EditBox.fMinValue=FLT_MIN;
                                    structUIData.EditBox.fMaxValue=FLT_MAX;
                        /*<END>Added by ANOOP for validating the list of  ranges */
                    }
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    {
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_FLOAT;
                        //structUIData.EditBox.fValue=(float)((double)varVal);  // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast> 
                        // stevev - merge 19feb07 - added par 750, editbox needs a double.
                        structUIData.EditBox.editBoxValue.SetValue((double)varVal.GetVarDouble(), valueType_t.isIntConst);
                        /*<START>Added by ANOOP for validating the list of  ranges */
                        MinMaxVal tmpMinMaxVal = new MinMaxVal();
                        tmpMinMaxVal.FloatMinMaxVal.fMinval = double.MinValue;  //Code corrected in the code review 
                        tmpMinMaxVal.FloatMinMaxVal.fMaxval = double.MaxValue;
                        structUIData.EditBox.MinMaxVal.Add(tmpMinMaxVal);

                        /*			structUIData.EditBox.fMinValue=FLT_MIN;
                                    structUIData.EditBox.fMaxValue=FLT_MAX;
                        /*<END>Added by ANOOP for validating the list of  ranges */
                    }
                    break;


                /******* assumption: the UI will only allow the editing of wide characters, all strings will 
                                        go to/from wide char to be edited.                        ***************/
                case VARIANT_TYPE.RUL_CHARPTR:
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                case VARIANT_TYPE.RUL_DD_STRING:     // SF:EPM:  ADDED RUL_DD_STRING. This was not being handled !
                case VARIANT_TYPE.RUL_SAFEARRAY:// only of type (one of three above)
                    {
                        //			structUserInput.EditBox.iDefaultStringLength =
                        //			structUserInput.EditBox.iMaxStringLength     = MAX_DD_STRING;
                        //			structUserInput.EditBox.pchDefaultValue = null;  // we do wide chars
                        //			structUserInput.EditBox.pwcDefaultValue = new wchar_t[MAX_DD_STRING+1];
                        //			memset(structUserInput.EditBox.pwcDefaultValue,0,sizeof(wchar_t)*MAX_DD_STRING);
                        /* stevev 28may09 - use hart varient for easier handling....
                        structUIData.EditBox.editBoxType=EDIT_BOX_TYPE_STRING;
                        structUIData.EditBox.iMaxStringLength=MAX_DD_STRING;
                        structUIData.EditBox.iDefaultStringLength = 0;
                        varVal.GetStringValue(structUIData.EditBox.pwcDefaultValue);
                        if( structUIData.EditBox.pwcDefaultValue )
                        {
                            structUIData.EditBox.iDefaultStringLength = wcslen(structUIData.EditBox.pwcDefaultValue);
                        }
                        */
                        structUIData.EditBox.editBoxType = EDIT_BOX_TYPE.EDIT_BOX_TYPE_STRING;
                        string varstring = "";
                        varVal.GetStringValue(ref varstring);
                        structUIData.EditBox.editBoxValue.SetValue(varstring, valueType_t.isWideString);//converts from narrow
                        structUIData.EditBox.nSize = MAX_LEN_ALLOC;// stevev 14apr10 - found while doing 2493
                    }
                    break;

                default: // and RUL_NULL & RUL_SAFEARRAY & RUL_BOOL &  RUL_BYTE_STRING
                    break;//no-op
            }

            //	add_textMsg(structUIData,out_buf);	// stevev 26dec07 - common code
            //I have commented out the previous line because if it is called here it overwrites important 
            //     information from the strctIUIData. It is already called above.  This call is redundant.
            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/
            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus())// always true, you  just set it.. && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            else
            {
                structUIData.bMethodAbortedSignalToUI = false;// defensive only
            }
            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;	//Added by ANOOP 200204
            //===============================================================================================
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            //=============================================================================================
            {
                bSetAbortFlag = true;
            }
            else
            {// stevev 30may14 - add dynamic update loop to the get local var value for dynamic in prompt
                out_buf = mt_String;
                while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynaVarValChanged = false;
                    retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                        plItemIds, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);

                    if (true == bDynaVarValChanged)
                    {
                        Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
                    }
                    //=============================================================================================
                    if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    //=============================================================================================
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    Thread.Sleep(600);
                }// loop till not enabled

                Common.logTime();// isa logif start_stop (has newline)
                structUIData.bDisplayDynamic = false;   //Added by ANOOP 200204

                switch (structUserInput.userInterfaceDataType)
                {
                    case UI_DATA_TYPE.EDIT:
                        {
                            switch (structUserInput.EditBox.editBoxType)
                            {
                                case EDIT_BOX_TYPE.EDIT_BOX_TYPE_INTEGER:
                                    {
                                        switch (varVariantType)
                                        {
                                            case VARIANT_TYPE.RUL_CHAR:
                                                varVal.SetValue((byte)structUserInput.EditBox.editBoxValue.GetByte(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                                                varVal.SetValue((byte)structUserInput.EditBox.editBoxValue.GetByte(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_INT:
                                                varVal.SetValue((int)structUserInput.EditBox.editBoxValue.GetInt(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_UINT:
                                                varVal.SetValue((uint)structUserInput.EditBox.editBoxValue.GetUInt(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_SHORT:
                                                varVal.SetValue((short)structUserInput.EditBox.editBoxValue.GetInt(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_USHORT:
                                                varVal.SetValue((ushort)structUserInput.EditBox.editBoxValue.GetInt(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_LONGLONG:
                                                varVal.SetValue((Int64)structUserInput.EditBox.editBoxValue.GetInt64(), varVariantType);
                                                break;
                                            case VARIANT_TYPE.RUL_ULONGLONG:
                                                varVal.SetValue((UInt64)structUserInput.EditBox.editBoxValue.GetUInt64(), varVariantType);
                                                break;
                                            default:
                                                //varVal.clear();
                                                break;
                                        }
                                        //varVal = (Int64)structUserInput.EditBox.iValue;
                                        m_pInterpreter.SetVariableValue(pchVariableName, varVal);
                                        break;
                                    }
                                case EDIT_BOX_TYPE.EDIT_BOX_TYPE_FLOAT:
                                    {
                                        if (varVariantType == VARIANT_TYPE.RUL_FLOAT)
                                        {
                                            varVal.SetValue((float)structUserInput.EditBox.editBoxValue.GetFloat(), varVariantType);
                                        }
                                        else
                                        if (varVariantType == VARIANT_TYPE.RUL_DOUBLE)
                                        {
                                            varVal.SetValue((double)structUserInput.EditBox.editBoxValue.GetDouble(), varVariantType);
                                        }
                                        else
                                        {
                                            //varVal.Clear();
                                        }
                                        //if( varVal.GetVarType() == RUL_FLOAT )//WS:EPM 10aug07
                                        //{
                                        //varVal  = (float)structUserInput.EditBox.fValue;
                                        //}//WS:EPM 10aug07
                                        //else if( varVal.GetVarType() == RUL_DOUBLE )//WS:EPM 10aug07
                                        //{
                                        //	varVal  = (double)structUserInput.EditBox.fValue;//WS:EPM 10aug07
                                        //}
                                        m_pInterpreter.SetVariableValue(pchVariableName, varVal);
                                        break;
                                    }
                                case EDIT_BOX_TYPE.EDIT_BOX_TYPE_STRING:
                                    {
                                        varVal.SetValue(structUserInput.EditBox.editBoxValue.GetString(), varVariantType);
                                        m_pInterpreter.SetVariableValue(pchVariableName, varVal);
                                        break;
                                    }
                                case EDIT_BOX_TYPE.UNKNOWN_EDIT_BOX_TYPE:
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                        //break;
                }

            }

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }// end _get_local_var_value

        int _display_xmtr_status(uint lItemId, int iStatusValue)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            string status_str = "";
            CDDLBase pIB = null;
            bool bSetAbortFlag = false;

            //I am being very defensive.  I dont want the uDelayTime to be set to some inifinite number
            // because it is not set in the following code.
            structUIData.bUserAcknowledge = true;
            structUIData.bEnableAbortOnly = false; // just defensive
            structUIData.uDelayTime = 0;// just defensive

            if (m_pDevice.getItembyID(lItemId, ref pIB))
            {
                CDDLVar pVar = (CDDLVar)pIB;
                switch (pVar.VariableType())
                {
                    case variableType_t.vT_Enumerated:
                        {
                            if (status_str.Length > 0)
                            {
                                Common.add_textMsg(ref structUIData, status_str);  // stevev 26dec07 - common code
                                structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;
                            }
                            else
                            {
                                //do something?
                            }
                            break;
                        }

                    case variableType_t.vT_BitEnumerated:
                        {
                            if (status_str.Length > 0)
                            {
                                Common.add_textMsg(ref structUIData, status_str);  // stevev 26dec07 - common code
                                structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;
                            }
                            else
                            {
                                //do something?
                            }
                            break;
                        }
                    default:
                        //do something?
                        break;

                }
                /*Vibhor 040304: Comment: Added the second condition below*/

                if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
                {
                    structUIData.bMethodAbortedSignalToUI = true;
                }
                structUIData.bDisplayDynamic = false;   //Added by ANOOP 200204
                structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;  //Added by ANOOP 05FEB2004
                if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                {
                    bSetAbortFlag = true;
                }

            }

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }

        }

        int rspcode_string(int iCommandNumber, int iResponseCode, ref string pchResponseCodeString)
        {
            List<CDDLVar> itemList = new List<CDDLVar>();
            //int nHandle = 0;        // Used for optimisation, 0 means resolve it otherwise use the existing resolved list.
            int retVal = BI_SUCCESS;

            //hCrespCode	rspCode(m_pDevice.devHndl());

            hCRespCodeList tmprespCodeList = new hCRespCodeList();// treat as a vector of <hCrespCode>

            pchResponseCodeString = "";

            CCmdList ptrCmndList = (CCmdList)m_pDevice.getListPtr(itemType_t.iT_Command);
            if (ptrCmndList != null)
            {
                CDDLCmd pCommand = ptrCmndList.getCmdByNumber(iCommandNumber);
                if (pCommand != null)
                {
                    pCommand.getRespCodes(ref tmprespCodeList);
                    foreach (hCrespCode pRspCode in tmprespCodeList)
                    {
                        //rspCode=(*iT);
                        //if( rspCode.getVal() == iResponseCode)
                        if (pRspCode.getVal() == iResponseCode)
                        {
                            //wstring strtmp=rspCode.getDescStr().s; 
                            string strtmp = pRspCode.getDescStr();

                            pchResponseCodeString = strtmp;
                            break;
                        }// else keep looking	
                    }// next
                    if (pchResponseCodeString == "")// not found
                    {
                        retVal = BI_ERROR;// bad response code value
                    }
                }
                else
                {
                    retVal = BI_ERROR;// bad command number
                }
            }
            else
            {
                retVal = BI_ERROR;// no commands in device - we really should bail out now :}
            }

            return retVal;

        }

        int display_response_status(int lCommandNumber, int iStatusValue)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            List<CDDLVar> itemList = new List<CDDLVar>();
            //int nHandle = 0;        // Used for optimisation, 0 means resolve it otherwise use the existing resolved list.
            string str_desc;
            bool bSetAbortFlag = false;

            //responseCodeList_t respCodeList = new responseCodeList_t();

            //hCrespCode	rspCode(m_pDevice.devHndl());

            hCRespCodeList respCodeList;// treat as a vector of <hCrespCode>

            respCodeList = new hCRespCodeList();


            CCmdList ptrCmndList = (CCmdList)m_pDevice.getListPtr(itemType_t.iT_Command);
            if (ptrCmndList != null)
            {
                CDDLCmd pCommand = ptrCmndList.getCmdByNumber(lCommandNumber);
                if (pCommand != null)
                {
                    pCommand.getRespCodes(ref respCodeList);
                    foreach (hCrespCode pRspCode in respCodeList)
                    {
                        //rspCode=(*iT);
                        //if( rspCode.getVal() == iStatusValue)
                        if (pRspCode.getVal() == iStatusValue)
                        {
                            int str_len;
                            //str_desc=rspCode.getDescStr(); 
                            str_desc = pRspCode.getDescStr();

                            str_len = str_desc.Length;
                            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;
                            if (str_len > 0)
                            {
                                structUIData.textMessage.pchTextMessage = str_desc;
                                structUIData.textMessage.iTextMessageLength = str_len;
                                //_tstrcpy(structUIData.textMessage.pchTextMessage, str_desc.c_str());
                            }

                            structUIData.bUserAcknowledge = true;
                            /*Vibhor 030304: Start of Code*/
                            structUIData.bEnableAbortOnly = false; // just defensive
                            /*Vibhor 030304: End of Code*/
                            /*Vibhor 040304: Start of Code*/
                            structUIData.uDelayTime = 0;// just defensive
                            /*Vibhor 040304: End of Code*/

                            /*Vibhor 040304: Comment: Added the second condition below*/

                            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
                            {
                                structUIData.bMethodAbortedSignalToUI = true;
                            }
                            structUIData.bDisplayDynamic = false; //Added by Prashant 20FEB2004
                            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                            {
                                bSetAbortFlag = true;
                            }

                        }

                    }
                }
            }

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int display(string message, int[] glob_var_ids, int iNumberOfItemIds)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            //tchar disp_msg[MAX_LEN_ALLOC] = { 0 };
            string disp_msg = "";
            bool bSetAbortFlag = false;
            /*<START>Added by ANOOP for dynamci vars %0  */
            CValueVarient[] pDynVarVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            bool bDynaVarValChanged = false;
            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>Added by ANOOP for dynamci vars %0  */
            int rc = DDlDevDescription.pGlobalDict.get_string_translation(message, ref message);

            int retval = bltin_format_string(ref disp_msg, MAX_LEN_ALLOC, UPDATE_ALL, message,
                            glob_var_ids, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);

            Common.add_textMsg(ref structUIData, disp_msg);    // stevev 26dec07 - common code
            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;

            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/
            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus())// always true, you  just set it..  && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;	//Added by ANOOP 200204
            if (false ==
                     m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            {
                disp_msg = mt_String;
                while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynaVarValChanged = false;
                    retval = bltin_format_string(ref disp_msg, MAX_LEN_ALLOC, UPDATE_ALL, message,
                             glob_var_ids, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);

                    if (true == bDynaVarValChanged)
                    {
                        Common.add_textMsg(ref structUIData, disp_msg);    // stevev 26dec07 - common code
                    }
                    //=============================================================================================
                    // has to be after the format so bEnableDynamicDisplay is preserved 4 while test
                    if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    //=============================================================================================
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    Thread.Sleep(600);
                }//wend till disabled
            }

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            structUIData.bDisplayDynamic = false;  //Added by ANOOP 200204

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
        }

        int select_from_list(string pchDisplayString, int[] lItemId, int iNumberOfItemIds, string pchList)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            //tchar out_buf[MAX_LEN_ALLOC] = { 0 };
            //tchar lst_buf[MAX_LEN_ALLOC] = { 0 };
            string out_buf = "";
            string lst_buf = "";

            //unint iTimeInSeconds = 2000;  //preset Time for the message to be displayed
            //int nCntr = 0, pos = 0, rc;
            int rc;
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50], pDynListVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            //clearVarientArray(pDynListVals);
            bool bDynaVarValChanged = false;

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */
            rc = DDlDevDescription.pGlobalDict.get_string_translation(pchDisplayString, ref pchDisplayString);

            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                            lItemId, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);
            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code

            // we need to save it because the next call will probably change it
            bool valueWasUpdate = m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay;

            //*** CHECK ****
            //Each element in the ';' sep'd list may have multi languages AND variable names as PUT_MESSAGE
            rc = DDlDevDescription.pGlobalDict.get_string_translation(pchList, ref pchList);
            retval = bltin_format_string(ref lst_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchList,
                                                    null, 0, ref pDynListVals, ref bDynaVarValChanged);
            Common.add_optionList(ref structUIData, lst_buf);

            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/

            if (valueWasUpdate && !m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
            {
                m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay =
                structUIData.bDisplayDynamic = true;
            }

            structUIData.userInterfaceDataType = UI_DATA_TYPE.COMBO;
            structUIData.ComboBox.comboBoxType = COMBO_BOX_TYPE.COMBO_BOX_TYPE_SINGLE_SELECT;// WS:EPM 30apr07

            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus())// always true, you  just set it..  && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            // stevev 02Jun14 - now set in bltin_format_string 
            // structUIData.bDisplayDynamic = false;	//Added by ANOOP 200204
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            {
                while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynaVarValChanged = false;

                    retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                                  lItemId, iNumberOfItemIds, ref pDynVarVals, ref bDynaVarValChanged);

                    if (true == bDynaVarValChanged)
                    {
                        Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
                    }
                    // save it to combine with next
                    valueWasUpdate = m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay;

                    // now do the selection update
                    bDynaVarValChanged = false;
                    retval = bltin_format_string(ref lst_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchList,
                                                   null, 0, ref pDynListVals, ref bDynaVarValChanged);
                    if (true == bDynaVarValChanged)
                    {
                        Common.add_optionList(ref structUIData, lst_buf);
                    }

                    if (valueWasUpdate || m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                    {
                        m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay =
                        structUIData.bDisplayDynamic = true;
                    }
                    //=============================================================================================
                    // has to be after the format so bEnableDynamicDisplay is preserved 4 while test
                    if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    //=============================================================================================
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    Thread.Sleep(600);
                }//wend till disabled
            }

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            structUIData.bDisplayDynamic = false;   //Added by ANOOP 200204

            /*<END>14FEB04  Added by ANOOP for dyanmaic variables %0 */

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                if (structUserInput.userInterfaceDataType == UI_DATA_TYPE.COMBO)
                {
                    //WS:EPM 30apr07- start section
                    if (structUserInput.ComboBox.comboBoxType == COMBO_BOX_TYPE.COMBO_BOX_TYPE_SINGLE_SELECT)
                    {
                        return (int)(structUserInput.nComboSelection - 1);
                    }
                    else if (structUserInput.ComboBox.comboBoxType == COMBO_BOX_TYPE.COMBO_BOX_TYPE_MULTI_SELECT)//WS:EPM 30apr07
                    {
                        return (int)structUserInput.nComboSelection;
                    }
                    else
                    {
                        return BI_ERROR;
                    }//WS:EPM 30apr07 - end section
                }
                else
                {
                    return BI_ERROR;
                }
            }

        }

        int _iassign(uint item_id, Int64 new_value)
        {
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID(item_id, ref pIB) && null != pIB)
            {
                CDDLVar pVarDest = (CDDLVar)pIB;

                if (true == pVarDest.IsNumeric())
                {/* Ideally we should have checked for IsFloat, but there are DDs 
			which assign one numeric type to other */
                    CValueVarient tempValue = new CValueVarient();
                    tempValue.SetValue(new_value, valueType_t.isIntConst);
                    pVarDest.setRawDispValue(tempValue);           //Set the raw value.Handles Locals
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                //                                    dynamic comands
                    return BI_SUCCESS;
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int _lassign(uint item_id, Int64 new_value)
        {
            int nRetVal = BI_ERROR;
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID(item_id, ref pIB) && null != pIB)
            {
                CDDLVar pVarDest = (CDDLVar)pIB;

                if (true == pVarDest.IsNumeric())
                {
                    CValueVarient tempValue = new CValueVarient();
                    tempValue.SetValue(new_value, valueType_t.isIntConst);
                    pVarDest.setRawDispValue(tempValue);           //Set the raw value.Handles Locals
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                //                                    dynamic comands
                    nRetVal = BI_SUCCESS;//mark as having completed successfully
                }
            }
            return nRetVal;//single exit point
        }


        int _fassign(uint item_id, float new_value)
        {
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID(item_id, ref pIB) && null != pIB)
            {
                CDDLVar pVarDest = (CDDLVar)pIB;

                if (true == pVarDest.IsNumeric())
                {/* Ideally we should have checked for IsFloat, but there are DDs 
			which assign one numeric type to other */
                    CValueVarient tempValue = new CValueVarient();
                    tempValue.SetValue(new_value, valueType_t.isFloatConst);
                    pVarDest.setRawDispValue(tempValue);           //Set the raw value.Handles Locals
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                //                                    dynamic comands
                    return BI_SUCCESS;
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int _dassign(uint item_id, double new_value)
        {
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID(item_id, ref pIB) && null != pIB)
            {
                CDDLVar pVarDest = (CDDLVar)pIB;

                if (true == pVarDest.IsNumeric())
                {/* Ideally we should have checked for IsDouble, but there are DDs 
			which assign one numeric type to other */
                    CValueVarient tempValue = new CValueVarient();
                    tempValue.SetValue(new_value, valueType_t.isFloatConst);
                    pVarDest.setRawDispValue(tempValue);           //Set the raw value.Handles Locals
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                //                                    dynamic comands
                    return BI_SUCCESS;
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int _vassign(uint item_id_dest, uint item_id_source)
        {
            CValueVarient Var_src;
            CDDLBase pIB = new CDDLBase();
            CDDLVar pVarDest = new CDDLVar();

            if (m_pDevice.getItembyID(item_id_source, ref pIB) && null != pIB)
            {
                CDDLVar pVarSrc = (CDDLVar)pIB;
                Var_src = pVarSrc.getRawDispValue();//was getDispValue();		
                //pIB = null;
                if (m_pDevice.getItembyID(item_id_dest, ref pIB) && null != pIB)
                {
                    pVarDest = (CDDLVar)pIB;
                    if ((pVarDest.VariableType() == pVarSrc.VariableType()) &&
                         (pVarDest.VariableSize() >= pVarSrc.VariableSize()))
                    {
                        pVarDest.setRawDispValue(Var_src);         //Set the raw value.Handles Locals	
                        pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                    //                                    dynamic comands					
                        return BI_SUCCESS;
                    }
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int SELECT_FROM_LIST(string pchDisplayString, string pchList)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            //tchar out_buf[MAX_LEN_ALLOC] = { 0 };
            //tchar lst_buf[MAX_LEN_ALLOC] = { 0 };
            string out_buf = "";
            string lst_buf = "";
            int rc;
            bool bSetAbortFlag = false;
            /*<START>14FEB04  Added by ANOOP for dynamic variables %0 */
            CValueVarient[] pDynVarVals = new CValueVarient[50], pDynListVals = new CValueVarient[50];
            //clearVarientArray(pDynVarVals);
            //clearVarientArray(pDynListVals);
            bool bDynaVarValChanged = false;

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            /*<END>14FEB04  Added by ANOOP for dynamic variables %0 */

            rc = DDlDevDescription.pGlobalDict.get_string_translation(pchDisplayString, ref pchDisplayString);
            int retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                                              null, 0, ref pDynVarVals, ref bDynaVarValChanged);
            Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code

            //*** CHECK *****
            //Each element in the ';' sep'd list may have multi languages AND variable names as PUT_MESSAGE
            // stevev 30may14 - yes that is true, the prompt string and all the select strings.
            rc = DDlDevDescription.pGlobalDict.get_string_translation(pchList, ref pchList);
            retval = bltin_format_string(ref lst_buf, MAX_LEN_ALLOC, UPDATE_NORMAL,
                                            pchList, null, 0, ref pDynListVals, ref bDynaVarValChanged);
            Common.add_optionList(ref structUIData, lst_buf);


            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/

            structUIData.userInterfaceDataType = UI_DATA_TYPE.COMBO;
            structUIData.ComboBox.comboBoxType = COMBO_BOX_TYPE.COMBO_BOX_TYPE_SINGLE_SELECT;//WS:EPM 30apr07

            /*Vibhor 040304: Comment: Added the second condition below*/
            if (m_pMeth.GetMethodAbortStatus())// always true, you  just set it.. && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            // stevev 02Jun14 - now set in bltin_format_string
            // structUIData.bDisplayDynamic = false;	//Added by ANOOP 200204
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            {
                while (true == m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay)
                {
                    bDynaVarValChanged = false;

                    retval = bltin_format_string(ref out_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchDisplayString,
                                                   null, 0, ref pDynVarVals, ref bDynaVarValChanged);
                    if (true == bDynaVarValChanged)
                    {
                        Common.add_textMsg(ref structUIData, out_buf); // stevev 26dec07 - common code
                    }

                    bDynaVarValChanged = false;
                    retval = bltin_format_string(ref lst_buf, MAX_LEN_ALLOC, UPDATE_NORMAL, pchList,
                                                        null, 0, ref pDynListVals, ref bDynaVarValChanged);
                    if (true == bDynaVarValChanged)
                    {
                        Common.add_optionList(ref structUIData, lst_buf);
                    }
                    //=============================================================================================
                    // has to be after the format so bEnableDynamicDisplay is preserved 4 while test
                    if (false ==
                       m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
                    //=============================================================================================
                    {
                        bSetAbortFlag = true;
                        break;
                    }
                    Thread.Sleep(600);
                }//wend till disabled
            }

            m_pDevice.m_pMethSupportInterface.bEnableDynamicDisplay = false;
            structUIData.bDisplayDynamic = false;   //Added by ANOOP 200204
            /*<END>14FEB04 Added by ANOOP for dynamic variables  %0	*/

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                if (structUserInput.userInterfaceDataType == UI_DATA_TYPE.COMBO)
                {//WS:EPM 30apr07 - start section
                    if (structUserInput.ComboBox.comboBoxType == COMBO_BOX_TYPE.COMBO_BOX_TYPE_SINGLE_SELECT)
                    {
                        return (int)(structUserInput.nComboSelection - 1);
                    }
                    else if (structUserInput.ComboBox.comboBoxType == COMBO_BOX_TYPE.COMBO_BOX_TYPE_MULTI_SELECT)
                    {
                        return (int)structUserInput.nComboSelection;
                    }
                    else
                    {
                        return BI_ERROR;// was structUserInput.nComboSelection - 1;
                    }
                }//WS:EPM 30apr07 - end section
                else
                {
                    return BI_ERROR;
                }
            }

        }

        void GetLanguageCode(ref string szString, ref string szLanguageCode, ref bool bLangCodePresent)
        {
            if (szString != null)
            {
                bLangCodePresent = false;
                if (szString[0] == '|' && szString[3] == '|')
                {
                    bLangCodePresent = true;
                    /*int count, itemp = szString.Length;// WS - 9apr07 - 2005 checkin
                    for (count = 4; count < itemp; count++)// WS - 9apr07 - 2005 checkin
                    {
                        if (count < 8)
                        {
                            szLanguageCode[count - 4] = szString[count - 4];
                        }
                        szString[count - 4] = szString[count];
                    }
                    */
                    szString = szString.Substring(4);
                    szLanguageCode = szString;
                }
            }
            return;

        }

        bool SetStringParam(FunctionExpression pFuncExp, ref INTER_VARIANT[] pParamArray, int paramNumber, string paramString)
        {
            //	char* pC = (char*) paramString;
            bool ret = true;

            if ((pParamArray[paramNumber].GetVarType() == VARIANT_TYPE.RUL_CHARPTR) ||
                (pParamArray[paramNumber].GetVarType() == VARIANT_TYPE.RUL_WIDECHARPTR) ||
                (pParamArray[paramNumber].GetVarType() == VARIANT_TYPE.RUL_DD_STRING) ||
                (pParamArray[paramNumber].GetVarType() == VARIANT_TYPE.RUL_BYTE_STRING) ||
                (pParamArray[paramNumber].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                pParamArray[paramNumber].SetValue(paramString); //will convert to pParamArray type
                ret = OutputParameterValue(pFuncExp, paramNumber, pParamArray[paramNumber]);
                // added WS:EPM 17jul07
            }
            else
            {
                ret = false;// we don't do string to numeric here
            }
            return ret;
        }

        bool SetCharStringParam(FunctionExpression pFuncExp, ref INTER_VARIANT[] pParamArray, int paramNumber, string paramString)
        {
            bool ret = true;
            INTER_VARIANT pParam = pParamArray[paramNumber];
            if ((pParam.GetVarType() == VARIANT_TYPE.RUL_CHARPTR) ||    /* needs L */
                (pParam.GetVarType() == VARIANT_TYPE.RUL_WIDECHARPTR) ||    /* needs L */
                (pParam.GetVarType() == VARIANT_TYPE.RUL_DD_STRING) ||  /* needs L */
                (pParam.GetVarType() == VARIANT_TYPE.RUL_BYTE_STRING) ||
                (pParam.GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                pParamArray[paramNumber].SetValue(paramString);//will convert destination type as required
                ret = OutputParameterValue(pFuncExp, paramNumber, pParamArray[paramNumber]);// added WS:EPM 17jul07
            }
            else
            {// string to numeric unsupported
                ret = false;
            }
            return ret;
        }

        bool OutputParameterValue(FunctionExpression pFuncExp, int nParamNumber, INTER_VARIANT NewVarValue)
        {
            bool bRetVal = true;
            //char szLocalVarName[MAX_PATH] = { 0 };
            string szLocalVarName = "";

            //////??????CComplexDDExpression pExp = (CComplexDDExpression)pFuncExp.GetExpParameter(nParamNumber);
            CPrimaryExpression pExp = (CPrimaryExpression)pFuncExp.GetExpParameter(nParamNumber);
            if (pExp == null)
            {
                bRetVal = false;
            }

            if (bRetVal)
            {
                CToken pToken = pExp.GetToken();
                if (pToken != null)
                {
                    szLocalVarName = pToken.GetLexeme();
                }
                else
                {
                    bRetVal = false;
                }
            }

            if (bRetVal)
            {
                string szLang = "";
                bool bLangPresent = false;
                //		Remove the Language code , if it was appended <a tokenizer bug>
                GetLanguageCode(ref szLocalVarName, ref szLang, ref bLangPresent);
                //		Update the DD local var szLocaVarName with the value lselection
                m_pInterpreter.SetVariableValue(szLocalVarName, NewVarValue);
            }

            return bRetVal;
        }

        int _GetCurrentTime()
        {
            TimeSpan ts = new TimeSpan();
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            ts = DateTime.Now - dt;
            return (int)ts.TotalSeconds;
        }

        int getTransferStatus(int iDirection, ref int[] pLongItemIds, int iNumberOfItemIds)
        {// full-duplex ports are not currently supported
         // we assume that the transport layer fills the port info on each transaction
         // we assume that any sending of command 111 also fills the port info
         // we only report the port info from here
         //int rc = Common.FAILURE;

            /* hCTransferChannel pChannel = m_pDevice.pCmdDispatch.getPort();
             // we do not own pChannel memory - do not delete
             if (pChannel == null)
             {
                 //LOGIT(CERR_LOG | UI_LOG, "Memory error: port %d could not be opened\n", iportNumber);
                 return BI_ERROR;
             }
             else
             if (pChannel.Session_State == sessionState_t.ss_Undefined || pChannel.Session_State == sessionState_t.ss_Closed)
             {
                 //LOGIT(CERR_LOG | UI_LOG, "Error: port %d not opened\n", iportNumber);
                 return BI_ERROR;
             }
             else
             {

                 if (iNumberOfItemIds > 0)
                     pLongItemIds[0] = pChannel.respCd;
                 if (iNumberOfItemIds > 1)
                     pLongItemIds[1] = pChannel.funcCd;
                 if (iNumberOfItemIds > 2)
                     pLongItemIds[2] = pChannel.masterSync;
                 if (iNumberOfItemIds > 3)
                     pLongItemIds[3] = pChannel.slave_Sync;

                 return BI_SUCCESS;
             }
             */
            return BI_SUCCESS;

        }

        float _fvar_value(int lItemId)
        {
            float fretVal;
            CValueVarient ppReturnedDataItem = new CValueVarient();

            if (Read((uint)lItemId, ref ppReturnedDataItem, false) == BI_SUCCESS)
            {
                fretVal = ppReturnedDataItem.GetFloat();
                return (fretVal);
            }
            else
            {
                return BI_ERROR;
            }

        }

        Int64 _ivar_value(int lItemId)
        {
            Int64 iretVal = 0;
            CValueVarient ppReturnedDataItem = new CValueVarient();

            if (Read((uint)lItemId, ref ppReturnedDataItem, false) == BI_SUCCESS)
            {
                iretVal = ppReturnedDataItem.GetInt64();
                return (iretVal);
            }
            else
            {
                return BI_ERROR;
            }

        }

        Int64 _lvar_value(int lItemId)
        {
            Int64 lretVal = 0;
            CValueVarient ppReturnedDataItem = new CValueVarient();

            if (Read((uint)lItemId, ref ppReturnedDataItem, false) == BI_SUCCESS)
            {
                lretVal = ppReturnedDataItem.GetInt64();
                return (lretVal);
            }
            else
            {
                return BI_ERROR;
            }

        }

        int isetval(Int64 iValue)
        {
            int nRetVal = BI_ERROR;//start in failed state
            CDDLBase pIB = null;

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                CDDLVar pVarDest = (CDDLVar)pIB;
                CValueVarient tempValue = new CValueVarient();
                if (true == pVarDest.IsNumeric())
                {
                    tempValue.SetValue((Int64)iValue, valueType_t.isIntConst);
                    pVarDest.setRawDispValue(tempValue);//Set the raw value.Handles Locals
                    //pVarDest.ApplyIt();         // see Note 1 at top
                    //pVarDest.cacheValue();      // display => cache (scaling function)

                    /* see 14aug14 not at the top of the file
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                 //                                    dynamic comands
                    **/
                    // stevev 26sep08 .. a method can't change this state!....    
                    //                    pVarDest.markItemState(IDS_CACHED);
                    // stevev 26sep08 we have to notify that there has been a change -scaling-
                    /*
                    hCmsgList msgs;
                    msgs.insertUnique(pVarDest.getID(), mt_Mth, 0);
                    pVarDest.notifyUpdate(msgs);
                    */
                    nRetVal = BI_SUCCESS;//we have successfully executed
                }
            }
            return nRetVal;//single exit point
        }

        /*
        int openPort()
        {
            //int rc = Common.FAILURE;
            hCcmdDispatcher pCmdDispatch = m_pDevice.pCmdDispatch;

            hCTransferChannel pChannel = pCmdDispatch.getPort();
            // we do not own pChannel memory - do not delete
            if (pChannel == null)
            {
                //LOGIT(CERR_LOG | UI_LOG, "Memory error: port %d could not be opened\n", iportNumber);
                return BI_ERROR;
            }
            else if (pChannel.Session_State != sessionState_t.ss_Undefined && pChannel.Session_State != sessionState_t.ss_Closed)
            {
                //LOGIT(CERR_LOG | UI_LOG, "Error: port %d already opened\n", iportNumber);
                return BI_PORT_IN_USE;
            }
            else
            {
                //assert(pChannel.portNumber == iportNumber);

                pChannel.Session_State = sessionState_t.ss_Opening;

                pChannel.max_segment_len = hCTransferChannel.MAX_SEG_LEN;
                pChannel.masterSync = pChannel.startMasterSync = (int)hCTransferChannel.STARTING_MASTER_SYNC;
                pChannel.maserSyncRolloverCnt = 0;

                /*
                if ((rc = pChannel.setupCmd111(transferFunc_t.tf_OpenPort)) != BI_SUCCESS)
                    return rc;

                if ((rc = send_command(111)) != BI_SUCCESS)////////////////////////////////////
                {// exit on command error
                    pChannel.Session_State = sessionState_t.ss_Closed;
                    return rc;
                }   ///////////////////////////////////////////////////////////////////////////////

                //  fill in port info;
                pChannel.fillPortInfo(111);

                if (pChannel.respCd != 0 && pChannel.respCd != 8)
                {
                    pChannel.Session_State = sessionState_t.ss_Closed;
                    rc = BI_ERROR;
                    if (pChannel.respCd == 10)
                    {
                        rc = BI_PORT_IN_USE;
                    }
                    return rc;
                }
                
                pChannel.OpenPort();
                if (pChannel.funcCd != (int)transferFunc_t.tf_OpenPort)
                {
                    pChannel.Session_State = sessionState_t.ss_Closed;
                    return BI_ERROR;
                }

                if (pChannel.max_segment_len > hCTransferChannel.MAX_SEG_LEN)
                {// must be <= master_max_seg_len
                    //LOGIT(CERR_LOG | UI_LOG, "Device Error: Device segment length too big.\n");
                    pChannel.max_segment_len = hCTransferChannel.MAX_SEG_LEN;
                }
                //else leave it lay

                if (pChannel.masterSync != hCTransferChannel.STARTING_MASTER_SYNC)
                {
                    //LOGIT(CERR_LOG | UI_LOG, "Error:Cmd 111 did not use the sent master sync.\n");
                    closePort();
                    return BI_ERROR;
                }// else, leave it
                pChannel.Session_State = sessionState_t.ss_Opened;

                return BI_SUCCESS;
            }
            //return BI_ERROR;
        }
        */

            /*
        int closePort()
        {
            hCcmdDispatcher pCmdDispatch = m_pDevice.pCmdDispatch;

            hCTransferChannel pChannel = pCmdDispatch.getPort();
            // we do not own pChannel memory - do not delete
            if (pChannel == null)
            {
                //LOGIT(CERR_LOG | UI_LOG, "Memory error: port %d could not be opened\n", iportNumber);
                return BI_ERROR;
            }
            else if (pChannel.Session_State == sessionState_t.ss_Undefined && pChannel.Session_State == sessionState_t.ss_Closed)
            {
                //LOGIT(CERR_LOG | UI_LOG, "Error: port %d not opened\n", iportNumber);
                return BI_ERROR;
            }
            else//opened,opening,closing
            {
                //assert(pChannel.portNumber == iportNumber);

                sessionState_t hldSt = pChannel.Session_State;
                pChannel.Session_State = sessionState_t.ss_Closing;

                /*
                if ((rc = pChannel.setupCmd111(tf_Ready2Close)) != BI_SUCCESS)
                {
                    pChannel.Session_State = hldSt;
                    return rc;
                }

                if ((rc = send_command(111)) != BI_SUCCESS)////////////////////////////////////
                {// exit on command error
                    pChannel.Session_State = hldSt;
                    return rc;
                }   ///////////////////////////////////////////////////////////////////////////////

                //  fill in port info;
                //should be set by the send_command...pChannel.fillPortInfo(111);
                /
                pChannel.ClosePort();
                //  reply of 'Port Closed' is success..BI_SUCCESS
                if (pChannel.funcCd == (int)transferFunc_t.tf_PortClosed)
                {
                    pChannel.Session_State = sessionState_t.ss_Closed;
                    return BI_SUCCESS;
                }
                else if (pChannel.funcCd == (int)transferFunc_t.tf_Transfer)
                {
                    pChannel.Session_State = hldSt;
                    return BI_PORT_IN_USE;
                }
                else
                // everything else is BI_ERROR
                {
                    pChannel.Session_State = hldSt;
                    return BI_ERROR;
                }
            }
        }
        */

        int abort()
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            bool bSetAbortFlag = false;

            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;
            /* Walt 01may07 - buffer doesn't always exist.***
                strcpy(structUIData.textMessage.pchTextMessage,"Method aborted");
             ****/
            /* stevev 01may07 - modified Walt's Solution **/
            structUIData.textMessage.iTextMessageLength = Message.M_METHOD_ABORTED.Length;
            structUIData.textMessage.pchTextMessage = Message.M_METHOD_ABORTED;

            structUIData.bUserAcknowledge = true;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/

            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/

            /*Vibhor 040304: Comment: Added the second condition below*/

            if (m_pMeth.GetMethodAbortStatus() && (structUIData.bEnableAbortOnly == false))
            {
                structUIData.bMethodAbortedSignalToUI = true;
            }
            structUIData.bDisplayDynamic = false; //Added by Prashant 20FEB2004
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput))
            {
                bSetAbortFlag = true;
            }
            else
            {
                m_pMeth.abort();
            }

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
            }
            return (METHOD_ABORTED);
        }

        int process_abort()
        {
            m_pMeth.process_abort();
            return (METHOD_ABORTED);
        }

        int _add_abort_method(int lMethodId)
        {
            int nRetVal = BI_SUCCESS;

            nRetVal = m_pMeth._add_abort_method(lMethodId);

            return nRetVal;
        }

        int _remove_abort_method(int lMethodId)
        {
            int nRetVal = BI_SUCCESS;

            nRetVal = m_pMeth._remove_abort_method(lMethodId);

            return nRetVal;
        }

        int remove_all_abort()
        {
            int nRetVal = BI_SUCCESS;

            nRetVal = m_pMeth.remove_all_abort();

            return (nRetVal);
        }

        /*Arun 190505 Start of code*/

        int push_abort_method(int lMethodId)
        {
            int nRetVal = BI_SUCCESS;

            nRetVal = m_pMeth._push_abort_method(lMethodId);

            return (nRetVal);
        }

        int pop_abort_method()
        {
            int nRetVal = BI_SUCCESS;

            nRetVal = m_pMeth._pop_abort_method();

            return (nRetVal);
        }

        int send(int cmd_number, ref byte[] cmd_status)
        {
            byte[] more_status = new byte[Common.STATUS_SIZE];
            byte[] more_status_data = new byte[Common.MAX_XMTR_STATUS_LEN];
            int info_size = 0;

            return SEND_COMMAND(cmd_number, DEFAULT_TRANSACTION_NUMBER, ref cmd_status,
                ref more_status, ref more_status_data, Common.NORMAL_CMD, false, ref info_size);
        }

        int send_command(int cmd_number)
        {
            byte[] cmd_status = new byte[Common.STATUS_SIZE];
            byte[] more_status = new byte[Common.STATUS_SIZE];
            byte[] more_status_data = new byte[Common.MAX_XMTR_STATUS_LEN];
            int info_size = 0;

            return SEND_COMMAND(cmd_number, DEFAULT_TRANSACTION_NUMBER, ref cmd_status,
                ref more_status, ref more_status_data, Common.NORMAL_CMD, true, ref info_size);
        }

        int send_command_trans(int cmd_number, int iTransNumber)
        {
            byte[] cmd_status = new byte[Common.STATUS_SIZE];
            byte[] more_status = new byte[Common.STATUS_SIZE];
            byte[] more_status_data = new byte[Common.MAX_XMTR_STATUS_LEN];
            int info_size = 0;

            //return SEND_COMMAND(cmd_number,iTransNumber,cmd_status,more_status,more_status_data,NORMAL_CMD,true);
            return SEND_COMMAND(cmd_number, iTransNumber, ref cmd_status,
                ref more_status, ref more_status_data, Common.NORMAL_CMD, false, ref info_size);
        }


        int send_trans(int cmd_number, int iTransNumber, ref byte[] pchResponseStatus)
        {
            byte[] more_status = new byte[Common.STATUS_SIZE];
            byte[] more_status_data = new byte[Common.MAX_XMTR_STATUS_LEN];
            int info_size = 0;

            //return SEND_COMMAND(cmd_number,iTransNumber,pchResponseStatus,more_status,more_status_data,NORMAL_CMD,false);
            return SEND_COMMAND(cmd_number, iTransNumber, ref pchResponseStatus, ref more_status, ref more_status_data,
                                                                             Common.NORMAL_CMD, true, ref info_size);

        }

        int ext_send_command(int cmd_number, byte[] pchResponseStatus,
            byte[] pchMoreDataStatus, byte[] pchMoreDataInfo, ref int moreInfoSize)
        {

            return SEND_COMMAND(cmd_number, DEFAULT_TRANSACTION_NUMBER, ref pchResponseStatus, ref pchMoreDataStatus,
                ref pchMoreDataInfo, Common.NORMAL_CMD, true, ref moreInfoSize);

        }

        int ext_send_command_trans(int iCommandNumber, int iTransNumber, ref byte[] pchResponseStatus,
            ref byte[] pchMoreDataStatus, ref byte[] pchMoreDataInfo, ref int moreInfoSize)
        {

            return SEND_COMMAND(iCommandNumber, DEFAULT_TRANSACTION_NUMBER, ref pchResponseStatus,
                                    ref pchMoreDataStatus, ref pchMoreDataInfo, Common.NORMAL_CMD, true, ref moreInfoSize);

        }

        int tsend_command(int iCommandNumber)
        {
            byte[] cmd_status = new byte[Common.STATUS_SIZE];
            byte[] more_status = new byte[Common.STATUS_SIZE];
            byte[] more_status_data = new byte[Common.MAX_XMTR_STATUS_LEN];
            int info_size = 0;

            return SEND_COMMAND(iCommandNumber, DEFAULT_TRANSACTION_NUMBER, ref cmd_status,
                ref more_status, ref more_status_data, Common.NORMAL_CMD, false, ref info_size);
        }

        int tsend_command_trans(int iCommandNumber, int iTransNumber)
        {
            byte[] cmd_status = new byte[Common.STATUS_SIZE];
            byte[] more_status = new byte[Common.STATUS_SIZE];
            byte[] more_status_data = new byte[Common.MAX_XMTR_STATUS_LEN];
            int info_size = 0;

            return SEND_COMMAND(iCommandNumber, iTransNumber, ref cmd_status,
                ref more_status, ref more_status_data, Common.NORMAL_CMD, false, ref info_size);
        }

        int get_more_status(byte[] more_data_status, byte[] more_data_info, ref int moreInfoSize)
        {
            byte[] cmd_status = new byte[Common.STATUS_SIZE];

            return SEND_COMMAND(0, DEFAULT_TRANSACTION_NUMBER, ref cmd_status,
                ref more_data_status, ref more_data_info, Common.MORE_STATUS_CMD, true, ref moreInfoSize);
        }

        bool SetByteStringParam(FunctionExpression pFuncExp, INTER_VARIANT[] pParamArray, int paramNumber, ref _BYTE_STRING bsS)
        {// must handle nulls in the byte string!!
            //	char* pC = (char*) paramString;
            bool ret = true;
            INTER_VARIANT pParam = pParamArray[paramNumber];

            if ((pParam.GetVarType() == VARIANT_TYPE.RUL_CHARPTR) ||    /* needs L */
                (pParam.GetVarType() == VARIANT_TYPE.RUL_WIDECHARPTR) ||    /* needs L */
                (pParam.GetVarType() == VARIANT_TYPE.RUL_DD_STRING) ||  /* needs L */
                (pParam.GetVarType() == VARIANT_TYPE.RUL_BYTE_STRING) ||
                (pParam.GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                pParamArray[paramNumber].SetValue(bsS);//will convert destination type as required
                ret = OutputParameterValue(pFuncExp, paramNumber, pParamArray[paramNumber]);// added WS:EPM 17jul07
            }
            else
            {// string to numeric unsupported
                ret = false;
            }
            return ret;
        }

        bool GetCharStringParam(ref string retString, INTER_VARIANT[] pParamArray, int paramNumber)
        {
            string pRet = null;

            pParamArray[paramNumber].GetStringValue(ref pRet);
            if (pRet != null)// allocated in GetString
            {
                //wcstombs(retString, pRet, retStringLen);
                retString = pRet;
            }

            return true;
        }

        string svar_value(uint lItemId)
        {

            CValueVarient ppReturnedDataItem = new CValueVarient();

            if (Read(lItemId, ref ppReturnedDataItem, false) == BI_SUCCESS)
            {
                string szReturnValue = ppReturnedDataItem.GetString();
                return szReturnValue;
            }
            else
            {
                return null;
            }
        }

        int sassign(uint lItemId, string new_value)
        {
            CDDLBase pIB = new CDDLBase();

            string S = new_value;// narrow string	

            if (m_pDevice.getItembyID(lItemId, ref pIB))
            {
                CDDLVar pVarDest = (CDDLVar)pIB;
                CValueVarient tempValue = new CValueVarient();
                tempValue.SetValue(S, valueType_t.isString);

                variableType_t varType = pVarDest.VariableType();

                if (varType == variableType_t.vT_Ascii || varType == variableType_t.vT_PackedAscii || varType == variableType_t.vT_Password)
                {
                    pVarDest.setRawDispValue(tempValue);           //Set the raw value.Handles Locals
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                //                                    dynamic comands
                    return BI_SUCCESS;
                }
            }
            return BI_ERROR;
        }

        int _get_status_code_string(uint lItemId, int iStatusCode, ref string pchStatusString, int iMaxStringLength)
        {
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID(lItemId, ref pIB))
            {
                CDDLVar pVar = (CDDLVar)pIB;

                switch (pVar.VariableType())
                {
                    case variableType_t.vT_Enumerated:
                    case variableType_t.vT_BitEnumerated:
                        //pchStatusString = pVar.vValue.GetString().Substring(0, iMaxStringLength);//////
                        pchStatusString = pVar.enmList[iStatusCode].descS.Substring(0, iMaxStringLength);
                        return BI_SUCCESS;

                    default:
                        break;
                }
                return BI_ERROR;
            }
            else
            {
                return BI_ERROR;
            }
        }


        int _get_dictionary_string(uint lItemId, ref string pchDictionaryString, int iMaxStringLength)
        {
            string /*str = "", */newstr = "";
            ddpSTRING ddp = new ddpSTRING();
            DDlDevDescription.pGlobalDict.get_dictionary_string(lItemId, ref ddp);
            newstr = ddp.str;
            //DDlDevDescription.pGlobalDict.get_string_translation(newstr, ref str);
            pchDictionaryString = Common.GetLangStr(newstr);
            //pchDictionaryString = str.Substring(0, iMaxStringLength);//////

            return (BI_SUCCESS);
        }

        //Anil 22 December 2005 for dictionary_string built in
        int _dictionary_string(uint lItemId, ref string pchDictionaryString)
        {
            string str = "";
            ddpSTRING ddp = new ddpSTRING();
            int r = DDlDevDescription.pGlobalDict.get_dictionary_string(lItemId, ref ddp);
            str = ddp.str;

            if (str != "")
            {
                pchDictionaryString = str;
            }
            if (r != Common.DDL_SUCCESS)
                return (BI_ERROR);
            else
                return (BI_SUCCESS);
        }

        // stevev 29jan08
        int literal_string(uint lItemId, ref string pchLiteralString)
        {
            string str, ostr = "";
            if (DDlDevDescription.pLitStringTable == null)
                return (BI_ERROR);
            str = DDlDevDescription.pLitStringTable.get_lit_string(lItemId);

            if (str != null)
            {// stevev 21sep10-literal string function must return a single language
                int rc = DDlDevDescription.pGlobalDict.get_string_translation(str, ref ostr);
                if (rc == Common.DDS_SUCCESS)
                {
                    pchLiteralString = ostr;
                    return (BI_SUCCESS);
                }
                else
                {
                    return (BI_ERROR);
                }
            }
            else
                return (BI_ERROR);
        }

        /*Arun 190505 Start of code*/

        int get_enum_string(uint lItemId, int iStatusCode, ref string pchStatusString, int iStatusStringLength)
        {
            int iReturnValue;
            iReturnValue = _get_status_code_string(lItemId, iStatusCode, ref pchStatusString, iStatusStringLength);
            return iReturnValue;
        }

        int resolve_group_reference(uint dwItemId, int dwIndex, itemType_t typeCheck)
        {
            CDDLBase pItm = new CDDLBase();
            //hCgroupItemDescriptor* pGID = null;

            int iId = 0;
            /* stevev 25jan07 - expand to handle all group item types */
            bool paramsOK = (typeCheck == itemType_t.iT_ItemArray || // sjv 25jan07
                             typeCheck == itemType_t.iT_Collection ||
                             typeCheck == itemType_t.iT_File ||
                             typeCheck == itemType_t.iT_Array ||
                             typeCheck == itemType_t.iT_List);

            //m_pDevice.getItembyID((uint)dwItemId, ref pItm);
            if (m_pDevice.getItembyID(dwItemId, ref pItm) && paramsOK)
            {
                /* stevev 25jan07 replaced the following */
                /*Vibhor 120204: Start of Code*/
                //				if(typeCheck == iT_Collection)
                //				{
                //					rc = ((hCcollection *)pItm).getByIndex(dwIndex, &pGID );
                //				}
                //				else if(typeCheck == iT_ItemArray)
                //				{
                //					rc = ((hCitemArray *)pItm).getByIndex(dwIndex, &pGID );
                //				}
                /*Vibhor 120204: End of Code*/
                /* stevev 18feb08 - additional types with unique acess techniques */

                switch (pItm.eType)
                {
                    case nitype.nItemArray:
                        CDDLItemArray ita = (CDDLItemArray)pItm;
                        //iId = (int)ita.arrayitems[dwIndex].items[0].uiID;
                        foreach (item_array ia in ita.arrayitems)
                        {
                            if (dwIndex == ia.uiIndex)
                            {
                                iId = (int)ia.items[0].uiID;
                            }
                        }
                        break;

                    case nitype.nCollection:
                        CDDLCollection cle = (CDDLCollection)pItm;
                        foreach (colletion_member cm in cle.collectionmembers)
                        {
                            if (dwIndex == cm.uiName)
                            {
                                iId = (int)cm.items[0].uiID;
                            }
                        }
                        break;

                    /*
                case nitype.nFile:
                    CDDLCollection cle = (CDDLCollection)pItm;
                    foreach (colletion_member cm in cle.collectionmembers)
                    {
                        if (dwIndex == cm.uiName)
                        {
                            iId = (int)cm.items[0].uiID;
                        }
                    }
                    break;
                    */

                    case nitype.nArray:
                        CDDLArray arr = (CDDLArray)pItm;
                        iId = (int)arr.uiRefID;
                        break;

                    case nitype.nList:
                        CDDLList list = (CDDLList)pItm;
                        iId = (int)list.uiRefID;
                        break;

                    default:
                        break;
                }
            }
            return (int)iId;
        }

        int resolve_array_ref(uint lItemId, int iIndex)
        {
            int lRetVal;
            lRetVal = resolve_group_reference(lItemId, iIndex, itemType_t.iT_ItemArray);
            return lRetVal;
        }/*End resolve_array_ref*/

        int resolve_record_ref(uint lItemId, int iIndex)
        {

            int lRetVal;

            lRetVal = (int)resolve_group_reference(lItemId, iIndex, itemType_t.iT_Collection);

            return lRetVal;
        }/*End resolve_record_ref*/

        int resolve_param_ref(uint lItemId)
        {

            if (lItemId < 0xC0009F00 || lItemId > 0xC0009f62)
            {
                /* error handling */
                return 0;
            }
            else
            {
                int dwItemValue, dwTemp;
                dwItemValue = (int)lItemId;
                dwTemp = dwItemValue - 0x3fff9fef;
                return (int)dwTemp;
            }
        }
        /***** Start XMTR ABORT, IGNORE, RETRY builtins *****/
        int _set_xmtr_comm_status(int iCommStatus, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byXmtrCommAbortMask |= (byte)iCommStatus;
                    m_pMeth.m_byXmtrCommRetryMask &= (byte)~iCommStatus;
                    break;

                case __RETRY__:
                    m_pMeth.m_byXmtrCommAbortMask &= (byte)~iCommStatus;
                    m_pMeth.m_byXmtrCommRetryMask |= (byte)iCommStatus;
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byXmtrCommAbortMask &= (byte)~iCommStatus;
                    m_pMeth.m_byXmtrCommRetryMask &= (byte)~iCommStatus;
                    break;
            }
            return BI_SUCCESS;
        }

        int _set_xmtr_device_status(int iDeviceStatus, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byXmtrStatusAbortMask |= (byte)iDeviceStatus;
                    m_pMeth.m_byXmtrStatusRetryMask &= (byte)~iDeviceStatus;
                    break;

                case __RETRY__:
                    m_pMeth.m_byXmtrStatusAbortMask &= (byte)~iDeviceStatus;
                    m_pMeth.m_byXmtrStatusRetryMask |= (byte)iDeviceStatus;
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byXmtrStatusAbortMask &= (byte)~iDeviceStatus;
                    m_pMeth.m_byXmtrStatusRetryMask &= (byte)~iDeviceStatus;
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_xmtr_device_status*/

        int _set_xmtr_resp_code(int iResponseCode, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byXmtrRespAbortMask[((int)((iResponseCode) / 8))] |= (byte)(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    m_pMeth.m_byXmtrRespRetryMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    break;

                case __RETRY__:
                    m_pMeth.m_byXmtrRespAbortMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    m_pMeth.m_byXmtrRespRetryMask[((int)((iResponseCode) / 8))] |= (byte)(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byXmtrRespAbortMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    m_pMeth.m_byXmtrRespRetryMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    break;
            }

            return BI_SUCCESS;


        }/*End _set_xmtr_resp_code*/


        int _set_xmtr_all_resp_code(int iAbortIgnoreRetry)
        {
            int i;

            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:

                    m_pMeth.m_byXmtrRespAbortMask[0] = 0xFE;
                    m_pMeth.m_byXmtrRespRetryMask[0] = 0;
                    for (i = 1; i < RESP_MASK_LEN; i++)
                    {
                        m_pMeth.m_byXmtrRespAbortMask[i] = 0xFF;
                        m_pMeth.m_byXmtrRespRetryMask[i] = 0;
                    }
                    break;

                case __RETRY__:
                    m_pMeth.m_byXmtrRespAbortMask[0] = 0;
                    m_pMeth.m_byXmtrRespRetryMask[0] = 0xFE;
                    for (i = 1; i < RESP_MASK_LEN; i++)
                    {
                        m_pMeth.m_byXmtrRespAbortMask[i] = 0;
                        m_pMeth.m_byXmtrRespRetryMask[i] = 0xFF;
                    }
                    break;

                default:        /* __IGNORE__ */
                    for (i = 0; i < RESP_MASK_LEN; i++)
                    {
                        m_pMeth.m_byXmtrRespAbortMask[i] = 0;
                        m_pMeth.m_byXmtrRespRetryMask[i] = 0;
                    }
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_xmtr_all_resp_code*/

        int _set_xmtr_no_device(int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byXmtrReturnNodevAbortMask |= (byte)1;//TRUE
                    m_pMeth.m_byXmtrReturnNodevRetryMask &= (~1);
                    break;

                case __RETRY__:
                    m_pMeth.m_byXmtrReturnNodevAbortMask &= ~1;
                    m_pMeth.m_byXmtrReturnNodevRetryMask |= (byte)1;
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byXmtrReturnNodevAbortMask &= ~1;
                    m_pMeth.m_byXmtrReturnNodevRetryMask &= ~1;
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_xmtr_no_device*/

        int _set_xmtr_all_data(int iAbortIgnoreRetry)
        {
            int i;
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:

                    for (i = 0; i < DATA_MASK_LEN; i++)
                    {
                        m_pMeth.m_byXmtrDataAbortMask[i] = 0xFF;
                        m_pMeth.m_byXmtrDataRetryMask[i] = 0;
                    }
                    break;

                case __RETRY__:

                    for (i = 0; i < DATA_MASK_LEN; i++)
                    {
                        m_pMeth.m_byXmtrDataAbortMask[i] = 0;
                        m_pMeth.m_byXmtrDataRetryMask[i] = 0xFF;
                    }
                    break;

                default:        /* __IGNORE__ */

                    for (i = 0; i < DATA_MASK_LEN; i++)
                    {
                        m_pMeth.m_byXmtrDataAbortMask[i] = 0;
                        m_pMeth.m_byXmtrDataRetryMask[i] = 0;
                    }
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_xmtr_all_data*/


        int _set_xmtr_data(int iByteCode, int iBitMask, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byXmtrDataAbortMask[((int)((iByteCode) / 8))] |= (byte)(1 << ((iBitMask) - (((int)((iBitMask) / 8)) * 8)));
                    m_pMeth.m_byXmtrDataRetryMask[((int)((iByteCode) / 8))] &= (byte)~(1 << ((iBitMask) - (((int)((iBitMask) / 8)) * 8)));
                    break;

                case __RETRY__:
                    m_pMeth.m_byXmtrDataAbortMask[((int)((iByteCode) / 8))] &= (byte)~(1 << ((iBitMask) - (((int)((iBitMask) / 8)) * 8)));
                    m_pMeth.m_byXmtrDataRetryMask[((int)((iByteCode) / 8))] |= (byte)(1 << ((iBitMask) - (((int)((iBitMask) / 8)) * 8)));
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byXmtrDataAbortMask[((int)((iByteCode) / 8))] &= (byte)~(1 << ((iBitMask) - (((int)((iBitMask) / 8)) * 8)));
                    m_pMeth.m_byXmtrDataRetryMask[((int)((iByteCode) / 8))] &= (byte)~(1 << ((iBitMask) - (((int)((iBitMask) / 8)) * 8)));
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_xmtr_data*/

        string STRSTR(string string_var, string substring_to_find)
        {
            string szRetVal = null;
            //if (string_var && substring_to_find)
            {
                int i = string_var.IndexOf(substring_to_find);
                szRetVal = string_var.Substring(i);
            }
            return szRetVal;
        }

        string STRUPR(string string_var)
        {
            string szRetVal = null;
            //if (string_var)
            {
                szRetVal = string_var.ToUpper();
            }
            return szRetVal;

        }

        string STRLWR(string string_var)
        {
            string szRetVal = null;
            //if (string_var)
            {
                szRetVal = string_var.ToLower();
            }
            return szRetVal;
        }

        int STRLEN(string string_var)
        {
            int nRetVal = 0;
            //if (string_var)
            {
                nRetVal = string_var.Length;
            }
            return nRetVal;
        }

        int STRCMP(string string_var1, string string_var2)
        {
            int nRetVal = -1;
            if (string_var1 == null && string_var2 == null)//beware of being passed null pointers
            {
                nRetVal = 0;//if they are both null (empty) then say they match.
            }
            else if (string_var1 == null || string_var2 == null)//beware of being passed null pointers
            {
                nRetVal = -1;
            }
            else
            {
                nRetVal = (string_var1 == string_var2) ? 0 : -1;
            }
            return nRetVal;
        }

        string STRTRIM(string string_var)
        {
            if (string_var != null) //check for null pointer
            {
                char[] rm = { ' ', '\t', '\r', '\n' };
                string_var.Trim(rm);

                /*
                int istrLen = string_var.Length;
                int iNoOfspace = 0;
                int icount;// PAW see below 03/03/09
                for (icount = 0; icount < (istrLen - iNoOfspace); icount++)
                {
                    if ((string_var[icount] == ' ') || (string_var[icount] == '\t') || (string_var[icount] == '\r') || (string_var[icount] == '\n'))
                    {
                        //for (int iRef = icount; iRef < (istrLen - iNoOfspace); iRef++)
                        {
                            //string_var[iRef] = string_var[iRef + 1];
                        }
                        string_var = string_var.Remove(icount, 1);
                        iNoOfspace++;
                        icount--;
                    }
                    else
                    {
                        break;
                    }
                }
                istrLen = string_var.Length;
                for (icount = istrLen; icount > 0; icount--)
                {
                    if ((string_var[icount - 1] == ' ') || (string_var[icount - 1] == '\t') || (string_var[icount - 1] == '\r') || (string_var[icount - 1] == '\n'))
                    {
                        //string_var[icount - 1] = '\0';
                        string_var.Remove(icount - 1, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                */
            }
            return string_var;
        }

        //Get a substring from a string specifying starting position and length.
        string STRMID(string string_var, int start, int len)
        {
            string szReturnValue = null;
            if (string_var != null)
            {
                szReturnValue = string_var.Substring(start, len);
                /*
                int nSourceStringLength = wcslen(string_var);

                if ((start < nSourceStringLength) && (len <= nSourceStringLength) &&
                    ((start + len) <= nSourceStringLength))
                {
                    for (int nIndex = 0; nIndex < len; nIndex++)
                    {
                        szReturnValue[nIndex] = string_var[start + nIndex];
                        szReturnValue[nIndex + 1] = 0;
                    }
                }
                */
            }
            return szReturnValue;
        }

        int _set_comm_status(int iCommStatus, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byCommAbortMask |= (byte)iCommStatus;
                    m_pMeth.m_byCommRetryMask &= (byte)~iCommStatus;
                    break;

                case __RETRY__:
                    m_pMeth.m_byCommAbortMask &= (byte)~iCommStatus;
                    m_pMeth.m_byCommRetryMask |= (byte)iCommStatus;
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byCommAbortMask &= (byte)~iCommStatus;
                    m_pMeth.m_byCommRetryMask &= (byte)~iCommStatus;
                    break;
            }

            return BI_SUCCESS;
        }/*End _set_comm_status*/

        int _set_device_status(int iDeviceStatus, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byStatusAbortMask |= (byte)iDeviceStatus;
                    m_pMeth.m_byStatusRetryMask &= (byte)~iDeviceStatus;
                    break;

                case __RETRY__:
                    m_pMeth.m_byStatusAbortMask &= (byte)~iDeviceStatus;
                    m_pMeth.m_byStatusRetryMask |= (byte)iDeviceStatus;
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byStatusAbortMask &= (byte)~iDeviceStatus;
                    m_pMeth.m_byStatusRetryMask &= (byte)~iDeviceStatus;
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_device_status*/

        int _set_resp_code(int iResponseCode, int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byRespAbortMask[((int)((iResponseCode) / 8))] |= (byte)(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    m_pMeth.m_byRespRetryMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    break;

                case __RETRY__:
                    m_pMeth.m_byRespAbortMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    m_pMeth.m_byRespRetryMask[((int)((iResponseCode) / 8))] |= (byte)(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byRespAbortMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    m_pMeth.m_byRespRetryMask[((int)((iResponseCode) / 8))] &= (byte)~(1 << ((iResponseCode) - (((int)((iResponseCode) / 8)) * 8)));
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_device_status*/


        int _set_all_resp_code(int iAbortIgnoreRetry)
        {
            int i;

            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:

                    m_pMeth.m_byRespAbortMask[0] = 0xFE;
                    m_pMeth.m_byRespRetryMask[0] = 0;
                    for (i = 1; i < RESP_MASK_LEN; i++)
                    {
                        m_pMeth.m_byRespAbortMask[i] = 0xFF;
                        m_pMeth.m_byRespRetryMask[i] = 0;
                    }
                    break;

                case __RETRY__:
                    m_pMeth.m_byRespAbortMask[0] = 0;
                    m_pMeth.m_byRespRetryMask[0] = 0xFE;
                    for (i = 1; i < RESP_MASK_LEN; i++)
                    {
                        m_pMeth.m_byRespAbortMask[i] = 0;
                        m_pMeth.m_byRespRetryMask[i] = 0xFF;
                    }
                    break;

                default:        /* __IGNORE__ */
                    for (i = 0; i < RESP_MASK_LEN; i++)
                    {
                        m_pMeth.m_byRespAbortMask[i] = 0;
                        m_pMeth.m_byRespRetryMask[i] = 0;
                    }
                    break;
            }

            return BI_SUCCESS;

        }/*End _set_all_resp_code*/

        int _set_no_device(int iAbortIgnoreRetry)
        {
            switch (iAbortIgnoreRetry)
            {
                case __ABORT__:
                    m_pMeth.m_byReturnNodevAbortMask |= 1;//TRUE
                    m_pMeth.m_byReturnNodevRetryMask &= ~1;
                    break;

                case __RETRY__:
                    m_pMeth.m_byReturnNodevAbortMask &= ~1;
                    m_pMeth.m_byReturnNodevRetryMask |= 1;
                    break;

                default:        /* __IGNORE__ */
                    m_pMeth.m_byReturnNodevAbortMask &= ~1;
                    m_pMeth.m_byReturnNodevRetryMask &= ~1;
                    break;
            }

            return BI_SUCCESS;


        }/*End _set_no_device*/

        Int64 igetvalue()
        {
            Int64 iretVal = BI_ERROR;
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                CDDLVar pVar = (CDDLVar)pIB;
                if (true == pVar.IsNumeric())
                {
                    CValueVarient ppReturnedDataItem;
                    /* Ideally we should have checked for IsInt, but there are DDs 
                    which assign one numeric type to other */
                    ppReturnedDataItem = pVar.getRawDispValue(); //don't check what we got
                    iretVal = ppReturnedDataItem.GetInt64();
                }// else type error
            }// else parameter error
            return iretVal;// single point return
        }

        Int64 igetval()
        {
            return igetvalue();
        }

        float fgetval()
        {
            float fretVal;
            CValueVarient ppReturnedDataItem = new CValueVarient();
            CDDLVar pVar = new CDDLVar();
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                pVar = (CDDLVar)pIB;
                if (true == pVar.IsNumeric())
                {/* Ideally we should have checked for IsInt, but there are DDs 
			    which assign one numeric type to other */
                    ppReturnedDataItem = pVar.getRawDispValue(); //don't check what we got
                    fretVal = ppReturnedDataItem.GetFloat();
                    return (fretVal);
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        double dgetval()
        {
            double dretVal;
            CValueVarient ppReturnedDataItem = new CValueVarient();
            CDDLVar pVar = new CDDLVar();
            CDDLBase pIB = new CDDLBase();

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                pVar = (CDDLVar)pIB;
                if (true == pVar.IsNumeric())
                {/* Ideally we should have checked for IsInt, but there are DDs 
			    which assign one numeric type to other */
                    ppReturnedDataItem = pVar.getRawDispValue(); //don't check what we got
                    dretVal = ppReturnedDataItem.GetDouble();
                    return (dretVal);
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int lgetval()
        {
            return (int)igetvalue();
        }

        int fsetval(float fValue)
        {
            CDDLBase pIB = new CDDLBase();
            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                CDDLVar pVarDest = (CDDLVar)pIB;
                CValueVarient tempValue = new CValueVarient();
                if (true == pVarDest.IsNumeric())
                {/* Ideally we should have checked for IsDouble, but there are DDs 
			which assign one numeric type to other */
                    tempValue.SetValue(fValue, valueType_t.isFloatConst);
                    pVarDest.setRawDispValue(tempValue);//Set the raw value.Handles Locals
                                                        //pVarDest.ApplyIt();         // see Note 1 at top
                                                        //pVarDest.cacheValue();      // display => cache (scaling function)
                    /* see 14aug14 not at the top of the file
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                 //                                    dynamic comands
                    **/
                    // stevev 26sep08 .. a method can't change this state!....    
                    //                    pVarDest.markItemState(IDS_CACHED);
                    // stevev 26sep08 we have to notify that there has been a change			
                    //hCmsgList msgs;
                    //msgs.insertUnique(pVarDest.getID(), mt_Mth, 0);
                    //pVarDest.notifyUpdate(msgs);
                    return BI_SUCCESS;
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int lsetval(Int64 lValue)
        {// stevev 26sep08 - this seems a little ridiculous...
            return isetval(lValue);
        }
        /*  too much cut n paste....
            int nRetVal = BI_ERROR;//start in failed state
            hCitemBase* pIB = null;

            if(m_pDevice.getItemBySymNumber(lPre_postItemID,&pIB) == SUCCESS && pIB != null)
            {
                CValueVarient tempValue;
                hCVar *pVarDest = (hCVar*)pIB;

                if(true == pVarDest.IsNumeric())
                {
                    tempValue = (Int64)lValue;
                    pVarDest.setRawDispValue(tempValue);//was setDispValue(tempValue);
                    pVarDest.ApplyIt();
                    pVarDest.cacheValue();
                    pVarDest.markItemState(IDS_CACHED);
                    nRetVal = BI_SUCCESS;
                }	
            }
            return nRetVal;//single exit point
        }
        */
        int dsetval(double dValue)
        {
            CDDLBase pIB = null;

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                CDDLVar pVarDest = (CDDLVar)pIB;
                CValueVarient tempValue = new CValueVarient();
                if (true == pVarDest.IsNumeric())
                {/* Ideally we should have checked for IsDouble, but there are DDs 
			which assign one numeric type to other */
                    tempValue.SetValue(dValue, valueType_t.isFloatConst);
                    pVarDest.setRawDispValue(tempValue);//Set the raw value.Handles Locals
                    //pVarDest.ApplyIt();         // see Note 1 at top
                    //pVarDest.cacheValue();      // display => cache (scaling function)
                    /* see 14aug14 not at the top of the file
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                 //                                    dynamic comands
                    **/
                    // stevev 26sep08 .. a method can't change this state!....    
                    //                    pVarDest.markItemState(IDS_CACHED);
                    // stevev 26sep08 we have to notify that there has been a change			
                    //hCmsgList msgs;
                    //msgs.insertUnique(pVarDest.getID(), mt_Mth, 0);
                    //pVarDest.notifyUpdate(msgs);
                    return BI_SUCCESS;
                }
            }
            // We come here ONLY if we fell through one of the above conditions....
            return BI_ERROR;
        }

        int save_values()
        {
            //	bSaveValuesCalled = true; 	
            m_pMeth.save_values();
            return BI_SUCCESS;
        }

        //Added By Anil July 01 2005 --starts here
        int discard_on_exit()
        {
            m_pMeth.discard_on_exit();
            return BI_SUCCESS;
        }

        int sgetval(string stringvalue, int length)
        {
            CValueVarient ppReturnedDataItem;

            CDDLBase pIB = new CDDLBase();
            if (stringvalue == null || length < 1)
            {
                return BI_ERROR;
            }

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                if (null != pIB)
                {
                    CDDLVar pVar = (CDDLVar)pIB;
                    ppReturnedDataItem = pVar.getRawDispValue(); //don't check the rc or what we got

                    string stemp;
                    stemp = ppReturnedDataItem.sStringVal;
                    /* incorrect treatment- stevev 30may07
                    if(stemp.length() <= length)
                        return BI_ERROR;
                    else
                        strcpy(stringvalue,stemp.c_str());
                    */
                    stringvalue = stemp;

                    return BI_SUCCESS;
                }
                //else fall thru to error return
            }
            // else fall thru to error return

            return BI_ERROR;

        }

        // stevev 27dec07 - only wide strings are available from the methods
        //					BUILTIN_ssetval in BuiltinInvoke converts to string
        //					only normal ascii may be set to a dd variable 
        string ssetval(string value)
        {

            CValueVarient tempValue = new CValueVarient();
            CDDLBase pIB = new CDDLBase();

            string S = value;

            if (m_pDevice.getItembyID((uint)lPre_postItemID, ref pIB))
            {
                CDDLVar pVarDest = (CDDLVar)pIB;

                if (pVarDest.VariableType() == variableType_t.vT_Ascii || pVarDest.VariableType() == variableType_t.vT_PackedAscii
            || pVarDest.VariableType() == variableType_t.vT_Password /*|| pVarDest.VariableType() == vT_BitString*/)
                {
                    tempValue.SetValue(S, valueType_t.isString);// to a short string
                    pVarDest.setRawDispValue(tempValue);//Set the raw value.Handles Locals
                                                        //pVarDest.ApplyIt();         // see Note 1 at top
                                                        //pVarDest.cacheValue();      // display => cache (scaling function)
                    /* see 14aug14 not at the top of the file
                    pVarDest.setWriteStatus(1); // stevev 26sep08 - these were getting overwritten by 
                                                 //                                    dynamic comands
                    **/
                    // stevev 26sep08 .. a method can't change this state!....    
                    //                    pVarDest.markItemState(IDS_CACHED);	
                    // stevev 26sep08 we have to notify that there has been a change -scaling-		
                    //hCmsgList msgs;
                    //msgs.insertUnique(pVarDest.getID(), mt_Mth, 0);
                    //pVarDest.notifyUpdate(msgs);
                }
            }
            return value;
        }

        int _ListInsert(uint lListId, int iIndex, uint lItemId)
        {
            int iRetCode;
            CDDLBase pIB = new CDDLBase();

            if ((0 == lListId) || (-1 == iIndex) || (0 == lItemId))
            {
                return BI_ERROR;
            }
            /*Check if List exists & is Valid*/
            if (m_pDevice.getItembyID((uint)lListId, ref pIB))
            {
                if ((pIB.eType == nitype.nList) && (pIB.IsValidTest()))
                {
                    CDDLList pList = (CDDLList)pIB;

                    /*Now check if item to be inserted exists and is Valid*/

                    if (m_pDevice.getItembyID((uint)lItemId, ref pIB) && (pIB.IsValidTest()))
                    {
                        /*Rest of the sanity checking is done in the hClist::insert itself
                          So just call and return accordingly*/
                        iRetCode = pList.Insert(pIB, iIndex);
                        if (Common.SUCCESS != iRetCode)
                        {
                            return BI_ERROR;
                        }
                    }
                    else
                    {
                        return BI_ERROR;
                    }
                }
            }
            else
            {
                return BI_ERROR;
            }

            return BI_SUCCESS;

        }/*End _ListInsert()*/


        int _ListDeleteElementAt(long lListId, int iIndex)
        {
            CDDLBase pIB = new CDDLBase();
            int iRetCode = Common.SUCCESS;

            if ((0 == lListId) || (-1 == iIndex))
            {
                return BI_ERROR;
            }
            /*Check is List exists & is Valid*/
            if (m_pDevice.getItembyID((uint)lListId, ref pIB))
            {
                if ((pIB.eType == nitype.nList) && (pIB.IsValidTest()))
                {
                    CDDLList pList = (CDDLList)pIB;
                    /*Just calll the hClist::Remove() and return accordingly*/

                    iRetCode = pList.Remove(iIndex);

                    if (Common.SUCCESS != iRetCode)
                    {
                        return BI_ERROR;
                    }
                }
            }
            else
            {
                return BI_ERROR;
            }

            return BI_SUCCESS;

        }/*End _ListDeleteElementAt()*/

        int Date_to_Year(int hart_date)//WS 25jun07 - converted to HARTdate to year
        {
            int nYear = hart_date & 0xFF;
            nYear += 1900;
            return nYear;
        }

        int Date_to_Month(int hart_date)//WS 25jun07 - converted to HARTdate to month
        {
            int nMonth = ((hart_date >> 8) & 0xFF) + 1;
            return nMonth;
        }

        int Date_to_DayOfMonth(int hart_date)//WS 25jun07 - converted to HARTdate to DOM
        {
            int nDay = (hart_date >> 16) & 0xFF;
            return nDay;
        }

        double DiffTime(long time_t1, long time_t0)
        {// modified to decode 25jun07 - stevev
            DateTime _t1 = new DateTime(time_t1 * 10000000 + dt1970.Ticks);
            DateTime _t0 = new DateTime(time_t0 * 10000000 + dt1970.Ticks);
            TimeSpan ts = _t1 - _t0;
            return ts.Ticks;
        }

        long get_time(long htimeT)
        {// use randomKey to decode the HART-time_t to our internal time_t
         // temporary algorithm	
            long rVal = htimeT ^ randomKey;
            return rVal;
        }

        long make_time_t(long timeIn)
        {
            long val = (long)timeIn; // we cast the (possibly) __int64 to a long, 
                                     // but we will loose resolution (i.e. won't work after year 2038)
                                     // temporary algorithm							 
            val ^= randomKey;
            return val;
        }

        // WS 25jun07-changed 2nd param type- it is cast from numeric in Invoke
        long AddTime(long time_t1, long lseconds)
        {// modified to decode/encode 25jun07 - stevev
            long _t1 = get_time(time_t1);
            return make_time_t(_t1 + lseconds);
        }

        long mktime(tm time)
        {
            DateTime dt = new DateTime(time.tm_yday, time.tm_mon, time.tm_mday, time.tm_hour, time.tm_min, time.tm_sec);
            TimeSpan ts = dt - dt1970;
            return (long)ts.TotalSeconds;
        }

        long Make_Time(int year, int month, int dayOfMonth, int hour, int minute, int second, int isDST)
        {
            tm StDateNTime;

            //do some range checking
            if (year >= 1900)
            {
                year = year - 1900;
            }
            if (year > 255)
            {
                year = 255;
            }

            if (month > 0)//assume that the users pass in Jan == 1 not Jan == 0
            {
                month--;
            }
            if (month > 11)//No months are greater than Dec.
            {
                month = 11;
            }

            if (dayOfMonth > 31)//No day of months > 31
            {
                dayOfMonth = 31; //Dont bother checking for Feb 31... This is already an error condition.
            }

            StDateNTime.tm_wday = 0;
            StDateNTime.tm_yday = 0;
            StDateNTime.tm_year = year;
            StDateNTime.tm_mon = month;
            StDateNTime.tm_mday = dayOfMonth;
            StDateNTime.tm_hour = hour;
            StDateNTime.tm_min = minute;
            StDateNTime.tm_sec = second;
            StDateNTime.tm_isdst = isDST;
            return make_time_t(mktime(StDateNTime));// encode stevev 25jun07

        }

        long To_Time(int date, int hour, int minute, int second, int isDST)
        {
            tm StDateNTime;

            StDateNTime.tm_wday = 0;
            StDateNTime.tm_yday = 0;
            StDateNTime.tm_year = date & 0xFF;// WS 25jun07 fixed via normalization
            StDateNTime.tm_mon = (date >> 8) & 0xFF;// WS 25jun07 fixed via normalization
            StDateNTime.tm_mday = (date >> 16) & 0xFF;// WS 25jun07 fixed via normalization
            StDateNTime.tm_hour = hour;
            StDateNTime.tm_min = minute;
            StDateNTime.tm_sec = second;
            StDateNTime.tm_isdst = isDST;

            return make_time_t(mktime(StDateNTime));// encode stevev 25jun07


        }

        long Date_To_Time(int date)
        {
            tm StDateNTime;

            StDateNTime.tm_wday = 0;
            StDateNTime.tm_yday = 0;
            StDateNTime.tm_year = date & 0xFF;// WS 25jun07 fixed via normalization
            StDateNTime.tm_mon = (date >> 8) & 0xFF;// WS 25jun07 fixed via normalization
            StDateNTime.tm_mday = (date >> 16) & 0xFF;// WS 25jun07 fixed via normalization
            StDateNTime.tm_hour = 0;
            StDateNTime.tm_min = 0;
            StDateNTime.tm_sec = 0;
            StDateNTime.tm_isdst = -1;//As per Wally's comment

            return make_time_t(mktime(StDateNTime));// encode stevev 25jun07
        }

        int get_tm(ref tm pstTM, int date)
        {

            pstTM.tm_wday = 0;
            pstTM.tm_yday = 0;
            pstTM.tm_year = date & 0x0000FF;
            pstTM.tm_mon = ((date & 0x00FF00) >> 8) - 1;
            pstTM.tm_mday = (date & 0xFF0000) >> 16;
            pstTM.tm_hour = 0;
            pstTM.tm_min = 0;
            pstTM.tm_sec = 0;
            pstTM.tm_isdst = -1;    //the C run-time library code compute whether standard time or daylight saving time is in effect.

            //check input limits
            if (pstTM.tm_mon < 0)
            {
                return BI_ERROR;
            }
            else if (pstTM.tm_mon > 11)
            {
                return BI_ERROR;
            }
            if (pstTM.tm_mday < 1)
            {
                return BI_ERROR;
            }
            else if (pstTM.tm_mday > 31)
            {
                return BI_ERROR;
            }

            //(void) mktime(pstTM);
            return BI_SUCCESS;
        }

        int To_Date(int year, int month, int dayOfMonth)// WS 25jun07 - return a HART DATE
        {
            int hart_date = 0;
            //do some range checking
            if (year >= 1900)
            {
                year = year - 1900;
            }
            if (year > 255)
            {
                year = 255;
            }

            if (month > 0)//assume that the users pass in Jan == 1 not Jan == 0
            {
                month--;
            }
            if (month > 11)//No months are greater than Dec.
            {
                month = 11;
            }

            if (dayOfMonth > 31)//No day of months > 31
            {
                dayOfMonth = 31; //Dont bother checking for Feb 31... This is already an error condition.
            }

            hart_date = year;
            hart_date |= (month << 8);
            hart_date |= (dayOfMonth << 16);

            return hart_date;
        }

        int Time_To_Date(int time_t1)// WS 25jun07 - return a HART DATE
        {// stevev 25jun07 - decode time_t coming in
            DateTime tempTime = new DateTime(get_time(time_t1) * 10000000 + dt1970.Ticks);  // copy our data from a long into a time_t (which is defined as an __int64 in VS2005)
            //tm StDateNTime = gmtime(tempTime);// WS - 9apr07 - 2005 checkin
            int hart_date = tempTime.Year;
            hart_date |= (tempTime.Month << 8);
            hart_date |= (tempTime.Day << 16);

            return hart_date;
        }

        //================= DATE & TIME functions added 16jul14 stevev from emerson =======================================================
        // This function assumes the inputs are DATE types in 4 bytes.
        // | days of month ([1,31] in 2 bytes) | month ([1,12] in 1 byte) | year ([0, 255] in 1 byte) |
        long DATE_to_days(int date1, int date0)
        {

            tm StDateNTime = new tm();
            if (get_tm(ref StDateNTime, date1) == BI_ERROR)
            {
                return BI_ERROR;
            }

            long time1 = mktime(StDateNTime);

            tm StDateNTime0 = new tm();
            if (get_tm(ref StDateNTime0, date0) == BI_ERROR)
            {
                return BI_ERROR;
            }

            long time0 = mktime(StDateNTime0);

            double dDiffTime = time1 - time0;  //in seconds

            if ((date1 != date0) && (dDiffTime == 0))
            {
                return (BI_ERROR);
            }
            else
            {
                long lDiffTime = (long)(dDiffTime / (24 * 60 * 60));    //in days
                return (lDiffTime);
            }
        }

        // This function assumes the second input is DATE types in 4 bytes.
        // | days of month ([1,31] in 2 bytes) | month ([1,12] in 1 byte) | year ([0, 255] in 1 byte) |
        int days_to_DATE(int days, int date0)
        {

            tm StDateNTime = new tm();
            if (get_tm(ref StDateNTime, date0) == BI_ERROR)
            {
                return BI_ERROR;
            }
            StDateNTime.tm_mday += days;

            long time0 = mktime(StDateNTime);

            //convert time_t structure back to tm structure in local time zone
            DateTime StDateNTime0 = new DateTime(time0 * 10000000 + dt1970.Ticks);

            int futureDate = (int)((StDateNTime0.Day << 16) & 0xFFFF0000) | (((StDateNTime0.Month + 1) << 8) & 0xFF00) | (StDateNTime0.Year & 0xFF);
            return futureDate;
        }

        //This builtin creates a time_t value from the DATE and TIME_VALUE
        int From_DATE_AND_TIME_VALUE(uint date, uint time_value)
        {

            tm timeinfo = new tm();
            if (get_tm(ref timeinfo, (int)date) == BI_ERROR)
            {
                return BI_ERROR;
            }
            // Fill tm structure  
            int sec = (int)time_value / 32000;
            timeinfo.tm_sec = sec;

            // call mktime to create time_t type  
            long returnedVal;
            returnedVal = mktime(timeinfo);
            return (int)make_time_t(returnedVal);
        }

        //This builtin creates a time_t from the TIME_VALUE
        long From_TIME_VALUE(uint time_value)
        {
            long returnedVal;

            //get current time
            //time(returnedVal);
            returnedVal = _GetCurrentTime();
            returnedVal += (time_value / 32000);
            return make_time_t(returnedVal);
        }

        uint seconds_to_TIME_VALUE(double seconds)
        {
            return ((uint)(seconds * 32000));
        }
        double TIME_VALUE_to_seconds(uint time_value)
        {
            return ((double)time_value / 32000);
        }

        int TIME_VALUE_to_Hour(uint time_value)
        {
            int returnVal;

            returnVal = (int)((float)time_value / (60 * 60 * 32000));
            return (returnVal);
        }
        int TIME_VALUE_to_Minute(uint time_value)
        {
            int returnVal;

            returnVal = (int)((float)time_value / (60 * 32000));
            return (returnVal);
        }
        int TIME_VALUE_to_Second(ulong time_value)
        {
            int returnVal;

            returnVal = (int)((float)time_value / 32000);
            return (returnVal);
        }

        int DATE_AND_TIME_VALUE_to_string(ref string output_str, string format, int date, long time_value)
        {

            tm StDateNTime = new tm();
            if (get_tm(ref StDateNTime, date) == BI_ERROR)
            {
                return BI_ERROR;
            }

            StDateNTime.tm_sec = (int)((time_value) / 32000);

            //update tm in stantdard format for display
            long seconds = mktime(StDateNTime);
            //int returnVal = wcsftime(output_str, MAX_DD_STRING, format, StDateNTime);
            DateTime dt = new DateTime(seconds * 10000000);

            //int returnVal = wcsftime(timet_str, MAX_DD_STRING, format, StDateNTime);
            output_str = dt.ToString(format);

            return (BI_SUCCESS);
        }

        int DATE_to_string(ref string output_str, string format, int date)
        {

            tm StDateNTime = new tm();
            if (get_tm(ref StDateNTime, date) == BI_ERROR)
            {
                return BI_ERROR;
            }

            //int returnVal = wcsftime(output_str, MAX_DD_STRING, format, StDateNTime);
            long seconds = mktime(StDateNTime);
            //int returnVal = wcsftime(output_str, MAX_DD_STRING, format, StDateNTime);
            DateTime dt = new DateTime(seconds * 10000000);
            output_str = dt.ToString(format);

            return (BI_SUCCESS);
        }

        int TIME_VALUE_to_string(ref string time_value_str, string format, long time_value)
        {
            /*
            tm StDateNTime;

            StDateNTime.tm_wday = 0;
            StDateNTime.tm_yday = 0;
            StDateNTime.tm_year = 72;   //Jan. 1, 1972
            StDateNTime.tm_mon = 0;
            StDateNTime.tm_mday = 1;
            StDateNTime.tm_hour = 0;
            StDateNTime.tm_min = 0;
            StDateNTime.tm_sec = (int)((time_value) / 32000);
            StDateNTime.tm_isdst = -1;  //daylight saving time is unknown
            */
            //update tm in stantdard format for display
            //mktime(StDateNTime);
            //int returnVal = wcsftime(time_value_str, MAX_DD_STRING, format, &StDateNTime);
            DateTime dt = new DateTime(time_value * 10000000 + dt1970.Ticks);
            time_value_str = dt.ToString(format);
            return (BI_SUCCESS);
        }
        int timet_to_string(ref string timet_str, string format, long timet_value)
        {
            long time_t_val = get_time(timet_value);

            //get the time in tm struct
            //tm StDateNTime = gmtime(time_t_val);
            DateTime dt = new DateTime(time_t_val * 10000000 + dt1970.Ticks);

            //int returnVal = wcsftime(timet_str, MAX_DD_STRING, format, StDateNTime);
            timet_str = dt.ToString(format);

            return (BI_SUCCESS);
        }

        // The Builtin timet_to_TIME_VALUE converts the time of day part of a time_t to a TIME_VALUE(4).
        uint timet_to_TIME_VALUE(long timet_value)
        {
            //convert time_t structure to tm structure in local time zone
            long inputTime = get_time(timet_value);

            DateTime dt = new DateTime(timet_value * 10000000 + dt1970.Ticks);

            //output is number of 1/32 ms since midnight
            uint time_value = (uint)(32000 * ((dt.Hour * 60 + dt.Minute) * 60 + dt.Second));

            return (time_value);
        }

        uint To_TIME_VALUE(int hours, int minutes, int seconds)
        {
            uint time_value = (uint)(32000 * (60 * (60 * hours + minutes) + seconds));

            return (time_value);
        }

        int _MenuDisplay(uint lMenuId, string pchOptionList, ref int lselection)
        {
            //Fill the structure required for the Menu display
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();
            ACTION_USER_INPUT_DATA structUserInputData = new ACTION_USER_INPUT_DATA();
            structUIData.userInterfaceDataType = UI_DATA_TYPE.MENU;
            structUIData.DDitemId = lMenuId;

            //**** CHECK ****
            //Each element in the ';' sep'd list may have multi languages but MAY NOT have any variables
            int rc = DDlDevDescription.pGlobalDict.get_string_translation(pchOptionList, ref pchOptionList);
            // stevev 04oct05 - add extra buttons so dialog can tell it's a display menu
            string szTemp = "Abort;";
            szTemp += pchOptionList;
            int c, i;
            for (c = 0, i = 0; i < (int)szTemp.Length; i++) // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
            {
                if ((szTemp[i] == ';') || (szTemp[i] != ';' && szTemp[i + 1] == 0))
                {
                    c++;
                }
            }
            structUIData.ComboBox.pchComboElementText = szTemp;
            structUIData.ComboBox.iNumberOfComboElements = c;
            // end stevev 04oct05
            bool bSetAbortFlag = false;
            //As of today this functionality is not complete as Steve has to make changes in the MethodDisplay code
            //Any attempt to call this builtin will crash the SDC
            if (false == m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInputData))
            {
                bSetAbortFlag = true;
            }

            lselection = (int)structUserInputData.nComboSelection;
            // stevev added 4oct07

            if (bSetAbortFlag)
            {
                m_pMeth.process_abort();
                return (METHOD_ABORTED);
            }
            else
            {
                return (BI_SUCCESS);
            }
            //return BI_SUCCESS;

        }//End of MenuDisplay

        public bool InvokeFunction(string pchFunctionName, int iNumberOfParameters,
                    INTER_VARIANT[] pVarParameters, ref INTER_VARIANT pVarReturnValue,
                    ref int pBuiltinReturnCode, FunctionExpression pFuncExp)
        {

            string strFunName = pchFunctionName;
            if (!m_MapBuiltinFunNameToEnum.ContainsKey(strFunName))
            {
                return false;
            }

            BUILTIN_NAME enumBuiltinFunValue = m_MapBuiltinFunNameToEnum[strFunName];

            switch (enumBuiltinFunValue)
            {
                //if (strcmp("delay",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_delay:
                    {
                        int iTimeinSecs = 0;
                        string pchString = null;
                        //int pSize = MAX_DD_STRING;

                        if (pVarParameters[0].isNumeric())
                        {
                            iTimeinSecs = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (!GetStringParam(ref pchString, ref pVarParameters, 1))
                        {
                            return false;
                        }

                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;

                        if (pVarParameters[2].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[2], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }


                        int iReturnValue = delay(iTimeinSecs, pchString, pLongItemIds, iNumberOfItemIds);
                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            byte[] by = new byte[4];
                            by = BitConverter.GetBytes(iReturnValue);
                            pVarReturnValue.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("DELAY",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_DELAY:
                    {
                        string pchString = null;
                        //int pSize = MAX_DD_STRING;
                        int TimeInSecs = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            TimeInSecs = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (!GetStringParam(ref pchString, ref pVarParameters, 1))
                        {
                            return false;
                        }

                        int iReturnValue = DELAY(TimeInSecs, pchString);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            byte[] by = new byte[4];
                            by = BitConverter.GetBytes(iReturnValue);
                            pVarReturnValue.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("DELAY_TIME",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_DELAY_TIME:
                    {
                        int TimeInSecs = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            TimeInSecs = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        //int iReturnValue = DELAY_TIME(TimeInSecs);

                        Thread.Sleep(TimeInSecs);
                        int iReturnValue = BI_SUCCESS;
                        byte[] by = new byte[4];
                        by = BitConverter.GetBytes(iReturnValue);
                        pVarReturnValue.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                /*Arun 200505 Start of code*/
                //break;
                //else
                //if (strcmp("BUILD_MESSAGE",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_BUILD_MESSAGE:
                    {
                        string pchDestString = "";
                        string pchMessageString = "";
                        // notUsed		int iReturnValue;			

                        if (!GetStringParam(ref pchMessageString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        CValueVarient[] cv = null;
                        DDlDevDescription.pGlobalDict.get_string_translation(pchMessageString, ref pchMessageString);
                        bool bl = true;
                        bltin_format_string(ref pchDestString, MAX_LEN_ALLOC, updatePermission_t.up_DONOT_UPDATE, pchMessageString,
                                             null, 0, ref cv, ref bl);
                        //BUILD_MESSAGE(pchDestString, pchMessageString);
                        if (pchDestString == null)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(pchDestString, VARIANT_TYPE.RUL_DD_STRING);
                        }
                        return true;
                    }
                /*End of code*/
                //break;
                //else
                //if (strcmp("PUT_MESSAGE",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_PUT_MESSAGE:
                    {
                        string pchString = "";

                        if (GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int iReturnValue = PUT_MESSAGE(pchString);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            byte[] by = new byte[4];
                            by = BitConverter.GetBytes(iReturnValue);
                            pVarReturnValue.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("put_message",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_put_message:
                    {
                        string pchString = "";

                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;
                        if (pVarParameters[1].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[1], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }


                        int iReturnValue = put_message(pchString, pLongItemIds, iNumberOfItemIds);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("ACKNOWLEDGE",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_ACKNOWLEDGE:
                    {
                        string pchString = "";

                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int iReturnValue = ACKNOWLEDGE(pchString);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("acknowledge",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_acknowledge:
                    {
                        string pchString = "";

                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;
                        if (pVarParameters[1].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[1], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }

                        int iReturnValue = acknowledge(pchString, pLongItemIds, iNumberOfItemIds);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_get_dev_var_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__get_dev_var_value:
                    {
                        string pchString = "";

                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;
                        uint lItemId = 0;

                        if (pVarParameters[1].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[1], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }

                        if (pVarParameters[2].isNumeric())
                        {
                            lItemId = (uint)pVarParameters[2].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }


                        int iReturnValue = _get_dev_var_value(pchString, pLongItemIds, iNumberOfItemIds, lItemId);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_get_local_var_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__get_local_var_value:
                    {
                        string pchString = "";
                        string wide_var_name = "";
                        string var_name = "";
                        string xlated_var_name = "";

                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;

                        if (pVarParameters[1].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[1], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }
                        //                     use an alias, should be type char so it'll go in
                        if (!GetStringParam(ref wide_var_name, ref pVarParameters, 2))
                        {
                            return false;
                        }
                        //wcstombs(var_name, wide_var_name, MAX_DD_STRING);
                        var_name = wide_var_name;
                        DDlDevDescription.pGlobalDict.get_string_translation(var_name, ref xlated_var_name);

                        int iReturnValue = _get_local_var_value(pchString, pLongItemIds, iNumberOfItemIds, xlated_var_name);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }

                        return true;
                    }
                //break;
                //else
                //if( strcmp("_display_xmtr_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__display_xmtr_status:
                    {
                        uint lItemId = 0;
                        int iStatusval = 0;

                        if (pVarParameters[0].isNumeric()) // was '=' stevev 30may07
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric()) // was '=' stevev 30may07
                        {
                            iStatusval = pVarParameters[1].GetVarInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }

                        int iReturnValue = _display_xmtr_status(lItemId, iStatusval);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;

                    }
                //break;
                //else
                //if( strcmp("display_response_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_display_response_status:
                    {
                        int lCommandVal = 0;
                        int iStatusval = 0;

                        if (pVarParameters[0].isNumeric())// was '=' stevev 30may07
                        {
                            lCommandVal = pVarParameters[0].GetVarInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }

                        // changed 05jun07;;if (pVarParameters[1].varType == RUL_INT)// was '=' stevev 30may07
                        //{if (! pVarParameters[1].isNumeric()) return false;};
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        iStatusval = (int)pVarParameters[1].GetVarInt();

                        int iReturnValue = display_response_status(lCommandVal, iStatusval);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }

                        return true;


                    }
                //break;
                //else
                //if (strcmp("display",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_display:
                    {
                        string pchString = "";

                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;

                        if (pVarParameters[1].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[1], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }


                        int iReturnValue = display(pchString, pLongItemIds, iNumberOfItemIds);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }

                        return true;
                    }
                //break;
                //else
                //if (strcmp("SELECT_FROM_LIST",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_SELECT_FROM_LIST:
                    {
                        string pchString_01 = "", pchString_02 = "";

                        if (!GetStringParam(ref pchString_01, ref pVarParameters, 0))
                        {
                            return false;
                        }

                        if (!GetStringParam(ref pchString_02, ref pVarParameters, 1))
                        {
                            return false;
                        }

                        int iReturnValue = SELECT_FROM_LIST(pchString_01, pchString_02);

                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("select_from_list",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_select_from_list:
                    {
                        string pchString_01 = "", pchString_02 = "";

                        if (!GetStringParam(ref pchString_01, ref pVarParameters, 0))
                        {
                            return false;
                        }


                        int[] pLongItemIds = new int[100];
                        int iNumberOfItemIds = 0;

                        if (pVarParameters[1].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[1], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }

                        if (!GetStringParam(ref pchString_02, ref pVarParameters, 2))
                        {
                            return false;
                        }

                        int iReturnValue = select_from_list(pchString_01, pLongItemIds, iNumberOfItemIds, pchString_02);
                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {
                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_vassign",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__vassign:
                    {
                        int lItemIdDest = 0;
                        int lItemIdSrc = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemIdDest = pVarParameters[0].GetVarInt();
                        }
                        /*Vibhor 230204: Start of Code*/
                        else
                        {
                            return false;
                        }
                        /*Vibhor 230204: End of Code*/

                        if (pVarParameters[1].isNumeric())
                        {
                            lItemIdSrc = (int)pVarParameters[1].GetVarInt();
                        }
                        /*Vibhor 230204: Start of Code*/
                        else
                        {
                            return false;
                        }
                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = _vassign((uint)lItemIdDest, (uint)lItemIdSrc);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_dassign",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__dassign:
                    {
                        int lItemId = 0;
                        double dVal = 0.0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        /*Vibhor 230204: Start of Code*/
                        else
                        {
                            return false;
                        }

                        //{if (! pVarParameters[1].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        dVal = pVarParameters[1].GetVarDouble();

                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = _dassign((uint)lItemId, dVal);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_fassign",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__fassign:
                    {
                        int lItemId = 0;
                        float fVal = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        /*Vibhor 230204: Start of Code*/
                        else
                        {
                            return false;
                        }

                        //{if (! pVarParameters[1].isNumeric()) return false;};// stevev added 18feb08
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }

                        fVal = (float)pVarParameters[1].GetVarFloat();

                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = _fassign((uint)lItemId, fVal);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_lassign",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__lassign:
                    {
                        int lItemId = 0;
                        Int64 lVal = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        /*Vibhor 230204: Start of Code*/
                        else
                        {
                            return false;
                        }

                        //{if (! pVarParameters[1].isNumeric()) return false;};// stevev added 18feb08
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }

                        lVal = (Int64)pVarParameters[1].GetVarInt64();

                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = _lassign((uint)lItemId, lVal);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_iassign",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__iassign:
                    {
                        int lItemId = 0;
                        Int64 iVal = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        /*Vibhor 230204: Start of Code*/
                        else
                        {
                            return false;
                        }

                        //{if (! pVarParameters[1].isNumeric()) return false;};// stevev added 18feb08
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }

                        iVal = (Int64)pVarParameters[1].GetVarInt64();

                        /*Vibhor 230204: End of Code*/
                        int iReturnValue = _iassign((uint)lItemId, iVal);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_fvar_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__fvar_value:
                    {
                        int lItemId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }

                        float fReturnValue = _fvar_value(lItemId);

                        pVarReturnValue.SetValue(fReturnValue, VARIANT_TYPE.RUL_FLOAT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_ivar_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__ivar_value:
                    {
                        int lItemId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }

                        Int64 iReturnValue = _ivar_value(lItemId);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_LONGLONG);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_lvar_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__lvar_value:
                    {
                        int lItemId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }


                        Int64 iReturnValue = _lvar_value(lItemId);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_LONGLONG);
                        return true;
                    }
                //break;
                //Added By Anil June 20 2005 --starts here
                //else
                //if (strcmp("svar_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_svar_value:
                    {
                        uint lItemId = 0;
                        string string_return_var = null;
                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }

                        string_return_var = svar_value(lItemId);

                        pVarReturnValue.SetValue(string_return_var, VARIANT_TYPE.RUL_DD_STRING);

                        if (string_return_var != null)
                        {
                            string_return_var = null;
                        }

                        return true;
                    }
                //break;
                //else
                //if (strcmp("sassign",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_sassign:
                    {
                        int iReturnValue = BI_ERROR;//default to failure
                        uint lItemId = 0;
                        string szValue = null;

                        if (pVarParameters[0].isNumeric())//do we have an ItemID?
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }
                        else// added else - stevev 30may07
                        {
                            return false;
                        }

                        //Get the String value 
                        pVarParameters[1].GetStringValue(ref szValue, VARIANT_TYPE.RUL_DD_STRING);

                        if (szValue != null)//do we have a valid string?
                        {
                            string szLang = "";
                            bool bLangPresent = false;
                            //		Remove the Language code , if it was prepended <a tokenizer bug>
                            GetLanguageCode(ref szValue, ref szLang, ref bLangPresent);//remove language code if present

                            iReturnValue = sassign(lItemId, szValue);//do operation and save return value.

                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);//set return value

                        return true;
                    }
                //break;
                //Added By Anil June 20 2005 --Ends here

                //else
                //if (strcmp("save_values",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_save_values:
                    {
                        m_pMeth.save_values();//save_values();

                        pVarReturnValue.SetValue(BI_SUCCESS, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                //Added By Anil July 01 2005 --starts here
                case BUILTIN_NAME.BUILTIN_discard_on_exit:
                    {
                        m_pMeth.discard_on_exit();// discard_on_exit();
                        pVarReturnValue.SetValue(BI_SUCCESS, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //Added By Anil July 01 2005 --Ends here
                //else
                //if (strcmp("get_more_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_get_more_status:
                    {// stevev 25dec07 - this returns the raw bytes - NOT strings!
                     //uchar pch_RespCode[STATUS_SIZE]={0};
                     //uchar pch_MoreStatusCode[MAX_XMTR_STATUS_LEN]={0};
                        int iReturnValue, moreInfoSize = Common.MAX_XMTR_STATUS_LEN;
                        _BYTE_STRING status;
                        status.bs = new byte[Common.STATUS_SIZE];
                        status.bsLen = Common.STATUS_SIZE;
                        _BYTE_STRING info;
                        info.bs = new byte[Common.MAX_XMTR_STATUS_LEN];
                        info.bsLen = Common.MAX_XMTR_STATUS_LEN;

                        //iReturnValue = get_more_status(pch_RespCode,pch_MoreStatusCode,moreInfoSize);
                        iReturnValue = get_more_status(status.bs, info.bs, ref moreInfoSize);
                        info.bsLen = moreInfoSize;

                        //if ( ! SetByteStringParam(pFuncExp, pVarParameters, 0, pch_RespCode, STATUS_SIZE) )//more_data_status
                        if (!SetByteStringParam(pFuncExp, pVarParameters, 0, ref status))
                        {
                            return false;
                        }

                        //if ( ! SetByteStringParam(pFuncExp, pVarParameters, 1, pch_MoreStatusCode, moreInfoSize) )//more_data_info
                        if (!SetByteStringParam(pFuncExp, pVarParameters, 1, ref info))//more_data_info
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_get_status_code_string",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__get_status_code_string:
                    {
                        uint lItemId = 0;
                        string pchString = "";

                        int iStatusCode = 0;
                        int iStatusStringlength = MAX_DD_STRING;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iStatusCode = (int)pVarParameters[1].GetVarInt();
                        }

                        if (pVarParameters[3].isNumeric())
                        {
                            iStatusStringlength = (int)pVarParameters[3].GetVarInt();
                        }

                        int iReturnValue = _get_status_code_string(lItemId, iStatusCode, ref pchString, iStatusStringlength);
                        if (!SetStringParam(pFuncExp, ref pVarParameters, 2, pchString))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                /*Arun 200505 Start of code*/
                //else
                //if (strcmp("get_enum_string",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_get_enum_string:
                    {
                        uint lItemId = 0;
                        int iVariableValue = 0;
                        int iMaxStringLength = MAX_DD_STRING;
                        string pchString = "";
                        int iReturnValue = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iVariableValue = (int)pVarParameters[1].GetVarInt();
                        }

                        //if (pVarParameters[3].varType == RUL_INT)//get_enum_string only takes 3 arguments WHS
                        //{
                        //	iMaxStringLength=(int)pVarParameters[3];
                        //}

                        iReturnValue = get_enum_string(lItemId, iVariableValue, ref pchString, iMaxStringLength);
                        //get_enum_string already truncated to iMaxStringLength

                        if (!SetStringParam(pFuncExp, ref pVarParameters, 2, pchString))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                /*End of code*/
                //else
                //if (strcmp("_get_dictionary_string",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__get_dictionary_string:
                    {
                        string pchString = "";
                        uint lItemId = 0;
                        int iMaxStringLength = MAX_DD_STRING;

                        if (iNumberOfParameters != 3)
                        {
                            return false;
                        }

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }

                        if (pVarParameters[2].isNumeric())
                        {
                            iMaxStringLength = (int)pVarParameters[2].GetVarInt();
                        }

                        int iReturnValue = _get_dictionary_string(lItemId, ref pchString, iMaxStringLength);
                        //_get_dictionary_string already truncated to iMaxStringLength

                        if (!SetStringParam(pFuncExp, ref pVarParameters, 1, pchString))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //Anil 22 December 2005 for dictionary_string built in
                case BUILTIN_NAME.BUILTIN__dictionary_string:
                    {
                        string pchString = null;
                        uint lItemId = 0;   //WS:EPM 24may07	

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }

                        int iReturnValue = _dictionary_string(lItemId, ref pchString);

                        if (pchString != null)
                        {
                            pVarReturnValue.SetValue(pchString, VARIANT_TYPE.RUL_DD_STRING);//WS:EPM 24may07
                            pchString = null;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("resolve_array_ref",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_resolve_array_ref:
                    {
                        uint lItemId = 0;
                        int iIndex = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iIndex = pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = resolve_array_ref(lItemId, iIndex);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("resolve_record_ref",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_resolve_record_ref:
                    {
                        uint lItemId = 0;
                        int iIndex = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iIndex = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = resolve_record_ref(lItemId, iIndex);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("resolve_param_ref",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_resolve_param_ref:
                    {
                        uint lItemId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = resolve_param_ref(lItemId);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_resolve_local_ref:
                    {
                        int lItemId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(lItemId, VARIANT_TYPE.RUL_INT);

                        return true;
                    }
                //break;
                //else
                //if (strcmp("rspcode_string",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_rspcode_string:
                    {
                        int iCmdNumber = -1;
                        int iRespCode = 0;
                        string szwLocaVarName = "";
                        string szLocaVarName = "";
                        string pchString = "";
                        int iRespCodeLength = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmdNumber = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iRespCode = pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[3].isNumeric())
                        {
                            iRespCodeLength = (int)pVarParameters[3].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = rspcode_string(iCmdNumber, iRespCode, ref pchString);
                        // GetStringParam is overloaded to get a char string if type is RUL_CHARPTR
                        if (!GetStringParam(ref szwLocaVarName, ref pVarParameters, 2))
                        {
                            return false;
                        }
                        //wcstombs(szLocaVarName, szwLocaVarName, MAX_DD_STRING);
                        szLocaVarName = szwLocaVarName;

                        INTER_VARIANT varTemp = new INTER_VARIANT();
                        string szLang = "";
                        bool bLangPresent = false;
                        //		Remove the Language code , if it was appended <a tokenizer bug>
                        GetLanguageCode(ref szLocaVarName, ref szLang, ref bLangPresent);
                        varTemp.SetValue(pchString, VARIANT_TYPE.RUL_DD_STRING);
                        //		Update the DD local var szLocaVarName with the value lselection
                        m_pInterpreter.SetVariableValue(szLocaVarName, varTemp);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_comm_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_comm_status:
                    {
                        int icomm_status = 0;
                        int iAbortIgnoreRetry = 0;

                        // stevev 11feb08  if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            icomm_status = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08  if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iAbortIgnoreRetry = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_comm_status(icomm_status, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_device_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_device_status:
                    {
                        int idev_status = 0;
                        int iAbortIgnoreRetry = 0;

                        //stevev 11feb08 if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            idev_status = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        //stevev 11feb08 if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_device_status(idev_status, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_resp_code",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_resp_code:
                    {
                        int iResp_code;
                        int iAbortIgnoreRetry;

                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iResp_code = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08   if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_resp_code(iResp_code, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_all_resp_code",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_all_resp_code:
                    {
                        int iAbortIgnoreRetry = 0;

                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_all_resp_code(iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_no_device",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_no_device:
                    {
                        int iAbortIgnoreRetry = 0;

                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_no_device(iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("SET_NUMBER_OF_RETRIES",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_SET_NUMBER_OF_RETRIES:
                    {
                        int iNo_of_retries = 0;

                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iNo_of_retries = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        m_pMeth.m_iAutoRetryLimit = iNo_of_retries;

                        pVarReturnValue.SetValue(BI_SUCCESS, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_comm_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_comm_status:
                    {
                        int iCommStatus = 0;
                        int iAbortIgnoreRetry = 0;

                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iCommStatus = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08   if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iAbortIgnoreRetry = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_comm_status(iCommStatus, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_device_status",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_device_status:
                    {
                        int iDeviceStatus = 0;
                        int iAbortIgnoreRetry = 0;


                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iDeviceStatus = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08   if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iAbortIgnoreRetry = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_device_status(iDeviceStatus, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_resp_code",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_resp_code:
                    {
                        int iRespCode = 0;
                        int iAbortIgnoreRetry = 0;

                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iRespCode = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08   if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iAbortIgnoreRetry = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_resp_code(iRespCode, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_all_resp_code",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_all_resp_code:
                    {
                        int iAbortIgnoreRetry = 0;


                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_all_resp_code(iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_no_device",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_no_device:
                    {
                        int iAbortIgnoreRetry = 0;


                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_no_device(iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_all_data",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_all_data:
                    {
                        int iAbortIgnoreRetry = 0;


                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iAbortIgnoreRetry = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_all_data(iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_set_xmtr_data",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__set_xmtr_data:
                    {
                        int iByteCode = 0;
                        int iBitMask = 0;
                        int iAbortIgnoreRetry = 0;


                        // stevev 11feb08   if (pVarParameters[0].GetVarType() == RUL_INT)
                        if (pVarParameters[0].isNumeric())
                        {
                            iByteCode = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08   if (pVarParameters[1].GetVarType() == RUL_INT)
                        if (pVarParameters[1].isNumeric())
                        {
                            iBitMask = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        // stevev 11feb08   if (pVarParameters[2].GetVarType() == RUL_INT)
                        if (pVarParameters[2].isNumeric())
                        {
                            iAbortIgnoreRetry = (int)pVarParameters[2].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _set_xmtr_data(iByteCode, iBitMask, iAbortIgnoreRetry);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("abort",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_abort:
                    {
                        int iReturnValue = abort();
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("process_abort",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_process_abort:
                    {
                        int iReturnValue = process_abort();
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_add_abort_method",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__add_abort_method:
                    {
                        int lMethodId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lMethodId = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _add_abort_method(lMethodId);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("_remove_abort_method",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN__remove_abort_method:
                    {
                        int lMethodId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lMethodId = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = _remove_abort_method(lMethodId);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("remove_all_abort",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_remove_all_abort:
                    {
                        int iReturnValue = remove_all_abort();
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                /*Arun 190505 Start of code*/
                //break;
                //else
                //if (strcmp("push_abort_method",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_push_abort_method:
                    {
                        int lMethodId = 0;

                        if (pVarParameters[0].isNumeric())
                        {
                            lMethodId = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = push_abort_method(lMethodId);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("pop_abort_method",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_pop_abort_method:
                    {
                        int iReturnValue = pop_abort_method();
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                /*End of code*/

                //else
                //if (strcmp("NaN_value",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_NaN_value:
                    {
                        uint fReturnValue = 0x7f800000 | 0x00200000;// NaN_value();
                        pVarReturnValue.SetValue(fReturnValue, VARIANT_TYPE.RUL_FLOAT);
                        return true;
                    }

                //break;
                /* stevev - 26jun07 - add stub outs for required builtins */
                case BUILTIN_NAME.BUILTIN_nan:
                    {
                        /*	char pchString[MAX_DD_STRING]={0};
                            int  pSize   = MAX_DD_STRING;

                            if ( ! GetStringParam(pchString, pVarParameters, 0) )
                            {
                                return false;
                            }

                            double dReturnValue = nan(pchString);

                            pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);

                            return true;
                        */
                        return false;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_nanf:
                    {
                        /*	char pchString[MAX_DD_STRING]={0};
                            int  pSize   = MAX_DD_STRING;

                            if ( ! GetStringParam(pchString, pVarParameters, 0) )
                            {
                                return false;
                            }

                            float fReturnValue = nanf(pchString);

                            pVarReturnValue.SetValue(fReturnValue, VARIANT_TYPE.RUL_FLOAT);

                            return true;
                        */
                        return false;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_fpclassify:
                    {
                        /*	float fValue  = 0.0;
                            double dValue = 0.0;
                            int    retVal = 0;

                            if (pVarParameters[0].varType == RUL_FLOAT)
                            {
                                fValue=(float)pVarParameters[0].GetVarInt();
                                retVal = _fpclassifyf(fValue);	
                            }
                            else
                            if (pVarParameters[0].varType == RUL_DOUBLE)
                            {
                                dValue=pVarParameters[0].GetVarInt();
                                retVal = _fpclassifyd(dValue);	
                            }
                            else
                            {
                                return false;
                            }
                            pVarReturnValue.SetValue(retVal, VARIANT_TYPE.RUL_INT);
                            return true;
                        ***/
                        return false;
                    }
                //break;
                /* stevev 25jun07 - end stubouts ***/
                //else
                //if (strcmp("isetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_isetval:
                    {
                        Int64 iValue = 0;

                        /*Vibhor 230204: Start of Code*/

                        if (pVarParameters[0].isNumeric())
                        {
                            iValue = (Int64)pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = isetval(iValue);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("lsetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_lsetval:
                    {
                        int lValue = 0;

                        /*Vibhor 230204: Start of Code*/

                        if (pVarParameters[0].isNumeric())
                        {
                            lValue = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        /*Vibhor 230204: End of Code*/
                        int iReturnValue = lValue;//lsetval(lValue);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("fsetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_fsetval:
                    {
                        float fValue = 0;

                        /*Vibhor 230204: Start of Code*/

                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }

                        fValue = (float)pVarParameters[0].GetVarInt();

                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = fsetval(fValue);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("dsetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_dsetval:
                    {
                        double dValue = 0.0;

                        /*Vibhor 230204: Start of Code*/

                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }

                        dValue = pVarParameters[0].GetVarInt();

                        /*Vibhor 230204: End of Code*/

                        int iReturnValue = dsetval(dValue);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("igetvalue",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_igetvalue:
                    {
                        Int64 iReturnValue = igetvalue();
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_LONGLONG);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("igetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_igetval:
                    {
                        Int64 iReturnValue = igetval();
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_LONGLONG);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("lgetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_lgetval:
                    {
                        Int64 lReturnValue = lgetval();
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_LONGLONG);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("fgetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_fgetval:
                    {
                        float fReturnValue = fgetval();
                        pVarReturnValue.SetValue(fReturnValue, VARIANT_TYPE.RUL_FLOAT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("dgetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_dgetval:
                    {
                        double dReturnValue = (float)dgetval();
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                /*Arun 200505 Start of code */
                //else
                //if (strcmp("sgetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_sgetval:
                    {
                        string pchString = "";
                        //	int iStringLength;
                        int iReturnValue = 0;

                        /* stevev 30may07 - sgetval only has one parameter
                        if (pVarParameters[1].varType == RUL_INT)
                        {
                            iStringLength= pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }
                        */

                        iReturnValue = sgetval(pchString, MAX_DD_STRING);
                        if (!SetCharStringParam(pFuncExp, ref pVarParameters, 0, pchString))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("ssetval",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_ssetval:
                    {
                        string pchString = "";
                        string pchReturnValue = null;
                        // only wide char is available from method literal strings
                        if (!GetStringParam(ref pchString, ref pVarParameters, 0))
                        {
                            return false;
                        }
                        /*
                        string lW(pchString);
                        string lS;
                        lS = TStr2AStr(lW);
                        pchReturnValue = ssetval(lS.c_str());
                        */
                        pchReturnValue = ssetval(pchString);

                        pVarReturnValue.SetValue(pchReturnValue, VARIANT_TYPE.RUL_CHARPTR);

                        return true;
                    }
                //break;
                /*End of code*/
                //else
                //if (strcmp("send",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_send:
                    {
                        int iCmd_no = -1;
                        //uchar pchString[MAX_DD_STRING]={0};
                        _BYTE_STRING byteString;
                        byteString.bs = new byte[MAX_DD_STRING];
                        byteString.bsLen = MAX_DD_STRING;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        //int iReturnValue = send(iCmd_no,pchString);
                        int iReturnValue = send(iCmd_no, ref byteString.bs);
                        byteString.bsLen = Common.STATUS_SIZE;// adjust to onlly the desired bytes

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 1, ref byteString))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("send_command",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_send_command:
                    {
                        int iCmd_no;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }


                        int iReturnValue = send_command(iCmd_no);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("send_command_trans",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_send_command_trans:
                    {
                        int iCmd_no = -1;
                        int iTrans_no = -1;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iTrans_no = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = send_command_trans(iCmd_no, iTrans_no);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("send_trans",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_send_trans:
                    {
                        int iCmd_no = -1;
                        int iTrans_no = -1;
                        //uchar pchString[MAX_DD_STRING]={0};
                        _BYTE_STRING byteString;
                        byteString.bs = new byte[MAX_DD_STRING];
                        byteString.bsLen = MAX_DD_STRING;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iTrans_no = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }


                        int iReturnValue = send_trans(iCmd_no, iTrans_no, ref byteString.bs);//was  pchString);
                        byteString.bsLen = Common.STATUS_SIZE;// adjust to onlly the desired bytes

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 2, ref byteString))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("ext_send_command",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_ext_send_command:
                    {
                        int iCmd_no = -1;
                        //uchar pchString_RespStatus[MAX_DD_STRING]={0};
                        //uchar pchString_MoreDataStatus[MAX_DD_STRING]={0};
                        //uchar pchString_MoreDataInfo[MAX_DD_STRING]={0};
                        _BYTE_STRING status;
                        status.bs = new byte[MAX_DD_STRING];
                        status.bsLen = MAX_DD_STRING;
                        _BYTE_STRING morestatus;
                        morestatus.bs = new byte[MAX_DD_STRING];
                        morestatus.bsLen = MAX_DD_STRING;
                        _BYTE_STRING info;
                        info.bs = new byte[MAX_DD_STRING];
                        info.bsLen = MAX_DD_STRING;

                        int InfoSize = 0;


                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }


                        //int iReturnValue = ext_send_command(iCmd_no, pchString_RespStatus, pchString_MoreDataStatus,
                        //													 pchString_MoreDataInfo,moreInfoSize);
                        int iReturnValue = ext_send_command(iCmd_no, status.bs, morestatus.bs, info.bs, ref InfoSize);
                        info.bsLen = InfoSize;

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 1, ref status))
                        {
                            return false;
                        }

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 2, ref morestatus))
                        {
                            return false;
                        }

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 3, ref info))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("ext_send_command_trans",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_ext_send_command_trans:
                    {
                        int iCmd_no = -1;
                        int iTrans_no = -1;
                        //int pSizeInfo = 0;
                        //uchar pchString_RespStatus[MAX_DD_STRING]={0};
                        //uchar pchString_MoreDataStatus[MAX_DD_STRING]={0};
                        //uchar pchString_MoreDataInfo[MAX_DD_STRING]={0};
                        _BYTE_STRING status;
                        status.bs = new byte[MAX_DD_STRING];
                        status.bsLen = MAX_DD_STRING;
                        _BYTE_STRING morestatus;
                        morestatus.bs = new byte[MAX_DD_STRING];
                        morestatus.bsLen = MAX_DD_STRING;
                        _BYTE_STRING info;
                        info.bs = new byte[MAX_DD_STRING];
                        info.bsLen = MAX_DD_STRING;

                        int InfoSize = 0;



                        if (pVarParameters[0].isNumeric())// cmd number
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())// transaction number
                        {
                            iTrans_no = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }


                        //int iReturnValue = ext_send_command_trans(iCmd_no,iTrans_no,pchString_RespStatus,
                        //	                                                        pchString_MoreDataStatus,
                        //															pchString_MoreDataInfo,pSizeInfo);
                        int iReturnValue = ext_send_command_trans(iCmd_no, iTrans_no, ref status.bs,
                            ref morestatus.bs, ref info.bs, ref InfoSize);
                        info.bsLen = InfoSize;

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 2, ref status))
                        {
                            return false;
                        }

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 3, ref morestatus))
                        {
                            return false;
                        }

                        if (!SetByteStringParam(pFuncExp, pVarParameters, 4, ref info))
                        {
                            return false;
                        }

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("tsend_command",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_tsend_command:
                    {
                        int iCmd_no = -1;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }


                        int iReturnValue = tsend_command(iCmd_no);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;
                //else
                //if (strcmp("tsend_command_trans",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_tsend_command_trans:
                    {
                        int iCmd_no = -1;
                        int iTrans_no = -1;

                        if (pVarParameters[0].isNumeric())
                        {
                            iCmd_no = pVarParameters[0].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        if (pVarParameters[1].isNumeric())
                        {
                            iTrans_no = (int)pVarParameters[1].GetVarInt();
                        }
                        else
                        {
                            return false;
                        }

                        int iReturnValue = tsend_command_trans(iCmd_no, iTrans_no);
                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        if ((iReturnValue == BI_ABORT)
                            || (iReturnValue == BI_NO_DEVICE)
                            || (iReturnValue == BI_COMM_ERR))
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        return true;
                    }
                //break;

                // Anil December 16 2005 deleted the Plot builtins case

                /*Arun 110505 Start of code*/
                /*****************************************Math Builtions (eDDL) ****************************/
                //else
                //if (strcmp("abs",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_abs:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }

                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Abs(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("acos",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_acos:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Acos(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("asin",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_asin:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Asin(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("atan",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_atan:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Atan(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("cbrt",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_cbrt:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Pow(dValue, 1.0 / 3.0);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("ceil",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_ceil:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Ceiling(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("cos",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_cos:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Cos(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("cosh",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_cosh:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Cosh(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("exp",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_exp:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Exp(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("floor",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_floor:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = System.Math.Floor(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("fmod",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_fmod:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValueX = pVarParameters[0].GetVarDouble();
                        //{if (! pVarParameters[1].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        double dValueY = pVarParameters[1].GetVarDouble();
                        double dReturnValue = dValueX - (int)(dValueX / dValueY) * dValueY;// fmod(dValueX, dValueY);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_frand:
                    {// returns a double between zero and 1
                        Random ra = new Random();
                        double dReturnValue = ra.NextDouble();
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("log",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_log:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Log(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("log10",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_log10:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Log10(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("log2",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_log2:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Log(dValue, 2);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("pow",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_pow:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValueX = pVarParameters[0].GetVarDouble();
                        //{if (! pVarParameters[1].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        double dValueY = pVarParameters[1].GetVarDouble();
                        double dReturnValue = Math.Pow(dValueX, dValueY);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("round",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_round:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Round(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("sin",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_sin:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Sin(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("sinh",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_sinh:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Sinh(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("sqrt",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_sqrt:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Sqrt(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("tan",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_tan:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Tan(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("tanh",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_tanh:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Tanh(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("trunc",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_trunc:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double dValue = pVarParameters[0].GetVarDouble();
                        double dReturnValue = Math.Truncate(dValue);
                        pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("atof",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_atof:
                    {// stevev 11feb08 -results of inter_varient rework
                     //	char* dValue=(char*)pVarParameters[0].GetVarInt();	
                        string dValue = null;
                        pVarParameters[0].GetStringValue(ref dValue);
                        if (dValue != null)
                        {
                            double dReturnValue = Convert.ToDouble(dValue);//atof(dValue);
                            pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_DOUBLE);
                            return true;
                        }
                        return false;
                    }
                //break;
                //else
                //if (strcmp("atoi",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_atoi:
                    {// stevev 11feb08 -results of inter_varient rework
                     //	char* dValue=(char*)pVarParameters[0].GetVarInt();	
                        string dValue = null;
                        pVarParameters[0].GetStringValue(ref dValue);
                        if (dValue != null)
                        {
                            int dReturnValue = Convert.ToInt32(dValue);// atoi(dValue);
                            pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_INT);
                            return true;
                        }
                        return false;
                    }
                //break;
                //else
                //if (strcmp("itoa",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_itoa:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int dValue1 = pVarParameters[0].GetVarInt();
                        string dValue2 = "";// NOT Unicode
                                            //{if (! pVarParameters[2].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        int dValue3 = (int)pVarParameters[2].GetVarInt();
                        //string dReturnValue = itoa(dValue1, dValue2, dValue3);

                        dValue2 = Convert.ToString(dValue1, dValue3);

                        //Anil 250407 The variable is the out Param so we should get the LoiacalVariable name and 
                        //then update this value through interpreter pointer 
                        //Method.h should have public const int itoa (a,b,c)          itoa((a), LOCALVAR (b), (c))
                        string szLocaVarName = "";

                        if (!GetCharStringParam(ref szLocaVarName, pVarParameters, 1))
                        {
                            return false;
                        }

                        INTER_VARIANT varTemp = new INTER_VARIANT();
                        string szLang = "";
                        bool bLangPresent = false;
                        //Remove the Language code , if it was apended <a tokenizer bug>
                        GetLanguageCode(ref szLocaVarName, ref szLang, ref bLangPresent);
                        varTemp.SetValue(dValue2, VARIANT_TYPE.RUL_DD_STRING);
                        //Update the DD local var szLocaVarName with the value lselection
                        m_pInterpreter.SetVariableValue(szLocaVarName, varTemp);

                        //anil 250407 Return value shoul be set to dValue2 and not dReturnValue
                        pVarReturnValue.SetValue(dValue2, VARIANT_TYPE.RUL_DD_STRING);

                        return true;
                    }
                //break;
                /*****************************************End of Math Builtins (eDDL) *********************/

                /*End of code*/

                /* Arun 160505 Start of code */

                /****************************************Date Time Builtins (eDDL)*************************/
                //else
                //if (strcmp("YearMonthDay_to_Date",pchFunctionName)==0)
                //	case BUILTIN_NAME.BUILTIN_YearMonthDay_to_Date:/* WS:EPM Not a builtin-25jun07 */
                //	{
                //		int dValue1=pVarParameters[0].GetVarInt();	
                //		int dValue2=(int)pVarParameters[1].GetVarInt();	
                //		int dValue3=(int)pVarParameters[2].GetVarInt();	
                //		long dReturnValue =YearMonthDay_to_Date(dValue1,dValue2,dValue3);
                //		pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_INT);
                //		return true;	
                //	}
                //	//break;
                //else
                //if (strcmp("Date_to_Year",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_Date_to_Year:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int lValue = pVarParameters[0].GetVarInt();   /* WS:EPM-changed types-25jun07*/
                        int lReturnValue = Date_to_Year(lValue);// WS - 9apr07 - 2005 checkin
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("Date_to_Month",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_Date_to_Month:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int lValue = pVarParameters[0].GetVarInt();   /* WS:EPM-changed types-25jun07*/
                        int lReturnValue = Date_to_Month(lValue); // WS - 9apr07 - 2005 checkin
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("Date_to_DayOfMonth",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_Date_to_DayOfMonth:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int lValue = pVarParameters[0].GetVarInt();   /* WS:EPM-changed types-25jun07*/
                        int lReturnValue = Date_to_DayOfMonth(lValue); // WS - 9apr07 - 2005 checkin
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("GetCurrentDate",pchFunctionName)==0)
                //case BUILTIN_NAME.BUILTIN_GetCurrentDate:/* WS:EPM Not a builtin-25jun07 */
                //{
                //	long dReturnValue =GetCurrentDate();
                //	pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_INT);
                //	return true;	
                //}
                //break;
                //else
                //if (strcmp("GetCurrentTime",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_GetCurrentTime:
                    {
                        int lReturnValue = _GetCurrentTime();/* WS:EPM-changed types-25jun07*/
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                //else
                //if (strcmp("GetCurrentDateAndTime",pchFunctionName)==0)
                //case BUILTIN_NAME.BUILTIN_GetCurrentDateAndTime:/* WS:EPM Not a builtin-25jun07 */
                //{
                //	float dReturnValue =GetCurrentDateAndTime();
                //	pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_FLOAT);
                //	return true;	
                //}
                ////break;
                //else
                //if (strcmp("To_Date_and_Time",pchFunctionName)==0)
                //case BUILTIN_NAME.BUILTIN_To_Date_and_Time:/* WS:EPM Not a builtin-25jun07 */
                //{
                //	int dValue1=pVarParameters[0].GetVarInt();	
                //	int dValue2=pVarParameters[0].GetVarInt();	
                //	int dValue3=pVarParameters[0].GetVarInt();	
                //	int dValue4=pVarParameters[0].GetVarInt();	
                //	int dValue5=pVarParameters[0].GetVarInt();	
                //	float dReturnValue =To_Date_and_Time(dValue1,dValue2,dValue3,dValue4,dValue5);
                //	pVarReturnValue.SetValue(dReturnValue, VARIANT_TYPE.RUL_FLOAT);
                //	return true;	
                //}
                ////break;

                /***************************************Date Time Builtins (eDDL)**************************/


                /****************************Start of DD_STRING  Builtins  (eDDL) ********************/
                //Added By Anil June 17 2005 --starts here
                //else
                //if (strcmp("STRSTR",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strstr:
                    {
                        string string_var = null;
                        string substring_to_find = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var, VARIANT_TYPE.RUL_DD_STRING);
                        pVarParameters[1].GetStringValue(ref substring_to_find, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck

                        GetLanguageCode(ref substring_to_find, ref szLangCode, ref bLanCodePrese);
                        //Append the Language code  When ur Returning
                        GetLanguageCode(ref string_var, ref szLangCode, ref bLanCodePrese);

                        string szTemp = STRSTR(string_var, substring_to_find);
                        //As per Vibhor's Suggestion if the string Not Found this is made ad terminated string
                        if (szTemp == null)
                        {
                            pVarReturnValue.SetValue("\0", VARIANT_TYPE.RUL_DD_STRING);
                        }
                        else
                        {
                            if (bLanCodePrese == true)

                            {
                                string szTempValue = szLangCode;
                                szTempValue += szTemp;
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTempValue, VARIANT_TYPE.RUL_DD_STRING);
                                if (szTempValue != null)
                                {
                                    szTempValue = null;
                                }
                            }
                            else
                            {
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTemp, VARIANT_TYPE.RUL_DD_STRING);

                            }
                        }
                        //Delete all the Memory Allocated;
                        if (string_var != null)
                        {
                            string_var = null;
                        }
                        if (substring_to_find != null)
                        {
                            substring_to_find = null;
                        }
                        return true;
                    }
                //break;

                //else
                //if (strcmp("STRUPR",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strupr:
                    {
                        string string_var = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck

                        //Append the Language code  When ur Returning
                        GetLanguageCode(ref string_var, ref szLangCode, ref bLanCodePrese);

                        string szTemp = STRUPR(string_var);
                        //As per Vibhor's Suggestion if the string Not Found this is made ad terminated string
                        if (szTemp == null)
                        {
                            pVarReturnValue.SetValue("\0", VARIANT_TYPE.RUL_DD_STRING);
                        }
                        else
                        {
                            if (bLanCodePrese == true)
                            {
                                string szTempValue = szLangCode + szTemp;
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTempValue, VARIANT_TYPE.RUL_DD_STRING);
                            }
                            else
                            {
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTemp, VARIANT_TYPE.RUL_DD_STRING);

                            }

                        }
                        //Delete all the Memory Allocated;
                        return true;
                    }
                //break;

                //else
                //if (strcmp("STRLWR",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strlwr:
                    {
                        string string_var = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck

                        //Append the Language code  When ur Returning
                        GetLanguageCode(ref string_var, ref szLangCode, ref bLanCodePrese);

                        string szTemp = STRLWR(string_var);
                        //As per Vibhor's Suggestion if the string Not Found this is made ad terminated string
                        if (szTemp == null)
                        {
                            pVarReturnValue.SetValue("\0", VARIANT_TYPE.RUL_DD_STRING);
                        }
                        else
                        {
                            if (bLanCodePrese == true)

                            {
                                string szTempValue = szLangCode + szTemp;
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTempValue, VARIANT_TYPE.RUL_DD_STRING);

                            }
                            else
                            {
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTemp, VARIANT_TYPE.RUL_DD_STRING);

                            }

                        }
                        //Delete all the Memory Allocated;
                        return true;
                    }
                //break;

                //else
                //if (strcmp("STRLEN",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strlen:
                    {
                        string string_var = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck

                        //Append the Language code  When ur Returning
                        GetLanguageCode(ref string_var, ref szLangCode, ref bLanCodePrese);

                        int istrLen = STRLEN(string_var);
                        pVarReturnValue.SetValue(istrLen, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;


                //else
                //if (strcmp("STRCMP",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strcmp:
                    {
                        string string_var1 = null;
                        string string_var2 = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var1, VARIANT_TYPE.RUL_DD_STRING);
                        pVarParameters[1].GetStringValue(ref string_var2, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck

                        GetLanguageCode(ref string_var1, ref szLangCode, ref bLanCodePrese);
                        GetLanguageCode(ref string_var2, ref szLangCode, ref bLanCodePrese);

                        int iCmp = STRCMP(string_var1, string_var2);
                        pVarReturnValue.SetValue(iCmp, VARIANT_TYPE.RUL_INT);
                        //Delete all the Memory Allocated;
                        return true;
                    }
                //break;

                //else
                //if (strcmp("STRTRIM",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strtrim:
                    {
                        string string_var = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck

                        //Append the Language code  When ur Returning
                        GetLanguageCode(ref string_var, ref szLangCode, ref bLanCodePrese);

                        string szTemp = STRTRIM(string_var);
                        //As per Vibhor's Suggestion if the string Not Found this is made ad terminated string
                        if (szTemp == null)
                        {
                            pVarReturnValue.SetValue("\0", VARIANT_TYPE.RUL_DD_STRING);
                        }
                        else
                        {
                            if (bLanCodePrese == true)

                            {
                                string szTempValue = szLangCode + szTemp;
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTempValue, VARIANT_TYPE.RUL_DD_STRING);

                            }
                            else
                            {
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTemp, VARIANT_TYPE.RUL_DD_STRING);

                            }

                        }
                        //Delete all the Memory Allocated;
                        return true;
                    }
                //break;

                //else
                //if (strcmp("STRMID",pchFunctionName)==0)
                case BUILTIN_NAME.BUILTIN_strmid:
                    {
                        string string_var = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[0].GetStringValue(ref string_var, VARIANT_TYPE.RUL_DD_STRING);

                        //Append the Language code  When ur Returning
                        GetLanguageCode(ref string_var, ref szLangCode, ref bLanCodePrese);
                        //{if (! pVarParameters[1].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        int iStrat = (int)pVarParameters[1].GetVarInt();
                        //{if (! pVarParameters[2].isNumeric()) return false;};// added stevev 18feb08		
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        int iLen = (int)pVarParameters[2].GetVarInt();

                        string szTemp = STRMID(string_var, iStrat, iLen);
                        //As per Vibhor's Suggestion if the string Not Found this is made ad terminated string
                        if (szTemp == null)
                        {
                            pVarReturnValue.SetValue("\0", VARIANT_TYPE.RUL_DD_STRING);
                        }
                        else
                        {
                            if (bLanCodePrese == true)

                            {
                                string szTempValue = szLangCode + szTemp;
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTempValue, VARIANT_TYPE.RUL_DD_STRING);
                            }
                            else
                            {
                                //Set this vlue in the variant
                                pVarReturnValue.SetValue(szTemp, VARIANT_TYPE.RUL_DD_STRING);

                            }

                        }

                        //Delete all the Memory Allocated;
                        return true;
                    }
                //break;
                /*Vibhor 200905: Start of Code*/
                case BUILTIN_NAME.BUILTIN__ListInsert:
                    {
                        uint lListId = 0;
                        int iIndx = -1;
                        uint lItemId = 0;
                        //Get the List Id
                        if (pVarParameters[0].isNumeric())
                        {
                            lListId = pVarParameters[0].GetVarUInt();
                        }

                        //Get the Index at which the insertion is required
                        if (pVarParameters[1].isNumeric())
                        {
                            iIndx = (int)pVarParameters[1].GetVarInt();
                        }

                        //Get the Id of the item which needs to be inserted in the list
                        if (pVarParameters[2].isNumeric())
                        {
                            lItemId = pVarParameters[2].GetVarUInt();
                        }

                        int iReturnValue = _ListInsert(lListId, iIndx, lItemId);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);

                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN__ListDeleteElementAt:
                    {
                        long lListId = 0;
                        int iIndx = -1;

                        //Get the List Id
                        if (pVarParameters[0].isNumeric())
                        {
                            lListId = pVarParameters[0].GetVarInt();
                        }

                        //Get the Index of the element which needs to be deleted.
                        if (pVarParameters[1].isNumeric())
                        {
                            iIndx = (int)pVarParameters[1].GetVarInt();
                        }

                        int iReturnValue = _ListDeleteElementAt(lListId, iIndx);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);

                        return true;
                    }
                //break;
                /*Vibhor 200905: End of Code*/

                //Anil September 26 2005 added MenuDisplay Start of Code
                case BUILTIN_NAME.BUILTIN__MenuDisplay:
                    {
                        string szOptionList = null;
                        uint lMenuId = 0;
                        string szLocaVarName = null;
                        int lselection = 0;

                        //Get the Menu Id
                        if (pVarParameters[0].isNumeric())
                        {
                            lMenuId = pVarParameters[0].GetVarUInt();
                        }
                        if (!GetStringParam(ref szOptionList, ref pVarParameters, 1))
                        {
                            return false;
                        }

                        //This is the DD Local Varible which is passes by reference
                        //So when we go out of this function, We need to update this value with the out param of
                        //_MenuDisplay below
                        if (!GetCharStringParam(ref szLocaVarName, pVarParameters, 2))
                        {
                            return false;
                        }

                        int iReturnValue = _MenuDisplay(lMenuId, szOptionList, ref lselection);

                        //Vibhor 221106: 
                        if (iReturnValue == METHOD_ABORTED)
                        {
                            pBuiltinReturnCode = BUILTIN_ABORT;
                        }
                        else
                        {   //We got the selction which,actually has to be stored in szLocaVarName
                            //Hence the below additional code
                            //Create a inter varinat with value equal to lselection and data type as RUL_INT
                            INTER_VARIANT varTemp = new INTER_VARIANT();
                            string szLang = "";
                            bool bLangPresent = false;
                            //		Remove the Language code , if it was apended
                            GetLanguageCode(ref szLocaVarName, ref szLang, ref bLangPresent);
                            varTemp.SetValue(lselection, VARIANT_TYPE.RUL_INT);
                            //		Update the DD local var szLocaVarName with the value lselection
                            m_pInterpreter.SetVariableValue(szLocaVarName, varTemp);

                            pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        }
                        return true;
                    }
                //break;
                //Anil September 26 2005 added MenuDisplay End of Code

                case BUILTIN_NAME.BUILTIN_DiffTime:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[1].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        // WS - 25jun07 - changed data types, fixed index //
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        long time_t1 = pVarParameters[0].GetVarInt();// WS - 9apr07 - 2005 checkin	
                        long time_t0 = (int)pVarParameters[1].GetVarInt();// WS - 9apr07 - 2005 checkin	

                        double dDiffTime = DiffTime(time_t1, time_t0);// WS - 9apr07 - 2005 checkin

                        pVarReturnValue.SetValue(dDiffTime, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_AddTime:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[1].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        // WS - 25jun07 - changed data types //
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        long time_t1 = pVarParameters[0].GetVarInt();// WS - 9apr07 - 2005 checkin	
                        long lseconds = (int)pVarParameters[1].GetVarInt();
                        long lAddedTime = AddTime(time_t1, lseconds);
                        pVarReturnValue.SetValue(lAddedTime, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_Make_Time:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[1].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[2].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[3].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[4].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[5].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[3].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[4].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[5].isNumeric())
                        {
                            return false;
                        }
                        int year = pVarParameters[0].GetVarInt();
                        int month = (int)pVarParameters[1].GetVarInt();
                        int dayofmonth = (int)pVarParameters[2].GetVarInt();
                        int hour = (int)pVarParameters[3].GetVarInt();
                        int minute = (int)pVarParameters[4].GetVarInt();
                        int second = (int)pVarParameters[5].GetVarInt();
                        int isDST = (int)pVarParameters[6].GetVarInt();

                        long lConvTime = Make_Time(year, month, dayofmonth, hour, minute, second, isDST);

                        pVarReturnValue.SetValue(lConvTime, VARIANT_TYPE.RUL_INT);//WS-fixed return type 25jun07
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_To_Time:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[1].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[2].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[3].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[4].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[3].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[4].isNumeric())
                        {
                            return false;
                        }
                        int date = pVarParameters[0].GetVarInt();// WS - 9apr07 - 2005 checkin 
                        int hour = (int)pVarParameters[1].GetVarInt();
                        int minute = (int)pVarParameters[2].GetVarInt();
                        int second = (int)pVarParameters[3].GetVarInt();
                        int isDST = (int)pVarParameters[4].GetVarInt();

                        long lConvTime = To_Time(date, hour, minute, second, isDST);
                        pVarReturnValue.SetValue(lConvTime, VARIANT_TYPE.RUL_INT);//WS-fixed return type 25jun07
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_Date_To_Time:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int date = pVarParameters[0].GetVarInt();// WS - 9apr07 - 2005 checkin 

                        long lConvTime = Date_To_Time(date);
                        pVarReturnValue.SetValue(lConvTime, VARIANT_TYPE.RUL_INT);//WS-fixed return type 25jun07
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_To_Date:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[1].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        //{if (! pVarParameters[2].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        int Year = pVarParameters[0].GetVarInt();
                        int month = (int)pVarParameters[1].GetVarInt();
                        int DayOfMonth = (int)pVarParameters[2].GetVarInt();

                        long lConvDate = To_Date(Year, month, DayOfMonth);
                        pVarReturnValue.SetValue(lConvDate, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_Time_To_Date:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};/* stevev-added check-25jun07*/
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int time_t1 = pVarParameters[0].GetVarInt();// WS - 9apr07 - 2005 checkin	

                        long lConvDate = Time_To_Date(time_t1);
                        pVarReturnValue.SetValue(lConvDate, VARIANT_TYPE.RUL_INT);// WS-fixed return type 25jun07
                        return true;
                    }
                //break;
                /*=========================== date/time functions - 16jul14 =============================================*/

                case BUILTIN_NAME.BUILTIN_From_DATE_AND_TIME_VALUE:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        //{if (! pVarParameters[1].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        uint lValue = pVarParameters[0].GetVarUInt();
                        uint ulValue = (uint)pVarParameters[1].GetVarUInt();
                        long lReturnValue = From_DATE_AND_TIME_VALUE(lValue, ulValue);
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_From_TIME_VALUE:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        uint ulValue = (uint)pVarParameters[0].GetVarInt();
                        long lReturnValue = From_TIME_VALUE(ulValue);
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_DATE_to_days:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        //{if (! pVarParameters[1].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        int lValue0 = pVarParameters[0].GetVarInt();
                        int lValue1 = (int)pVarParameters[1].GetVarInt();
                        long lReturnValue = DATE_to_days(lValue0, lValue1);
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_days_to_DATE:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        //{if (! pVarParameters[1].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        int lValue0 = pVarParameters[0].GetVarInt();
                        int lValue1 = (int)pVarParameters[1].GetVarInt();
                        long lReturnValue = days_to_DATE(lValue0, lValue1);
                        pVarReturnValue.SetValue(lReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_seconds_to_TIME_VALUE:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        double seconds = pVarParameters[0].GetVarDouble();

                        uint time_value = seconds_to_TIME_VALUE(seconds);
                        pVarReturnValue.SetValue(time_value, VARIANT_TYPE.RUL_UINT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_TIME_VALUE_to_seconds:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        uint time_value = (uint)pVarParameters[0].GetVarUInt();

                        double seconds = TIME_VALUE_to_seconds(time_value);
                        pVarReturnValue.SetValue(seconds, VARIANT_TYPE.RUL_DOUBLE);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_TIME_VALUE_to_Hour:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        uint time_value = (uint)pVarParameters[0].GetVarUInt();

                        int hour = TIME_VALUE_to_Hour(time_value);
                        pVarReturnValue.SetValue(hour, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_TIME_VALUE_to_Minute:
                    {
                        {
                            if (!pVarParameters[0].isNumeric())
                            {
                                return false;
                            }
                        }
                        uint time_value = (uint)pVarParameters[0].GetVarUInt();

                        int min = TIME_VALUE_to_Minute(time_value);
                        pVarReturnValue.SetValue(min, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_TIME_VALUE_to_Second:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        uint time_value = (uint)pVarParameters[0].GetVarUInt();

                        int second = TIME_VALUE_to_Second(time_value);
                        pVarReturnValue.SetValue(second, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                case BUILTIN_NAME.BUILTIN_DATE_AND_TIME_VALUE_to_string:
                    {
                        string format = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[1].GetStringValue(ref format, VARIANT_TYPE.RUL_DD_STRING);
                        //For Second string strip off the Language Code. 
                        GetLanguageCode(ref format, ref szLangCode, ref bLanCodePrese);

                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;

                        }
                        int date = (int)pVarParameters[2].GetVarInt();
                        if (!pVarParameters[3].isNumeric())
                        {
                            return false;
                        }
                        uint time_value = (uint)pVarParameters[3].GetVarUInt();


                        string output_str = "";
                        int iSize = 0;
                        iSize = DATE_AND_TIME_VALUE_to_string(ref output_str, format, date, time_value);

                        //Update the output_str to user
                        if (!SetStringParam(pFuncExp, ref pVarParameters, 0, output_str))
                        {
                            return false;
                        }
                        pVarReturnValue.SetValue(iSize, VARIANT_TYPE.RUL_INT);

                        //Delete all the Memory Allocated;
                        if (format != null)
                        {
                            format = null;
                        }
                        return true;
                    }
                ////break;

                case BUILTIN_NAME.BUILTIN_DATE_to_string:
                    {
                        string format = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;
                        string date_str = "";
                        int iSize = 0;

                        //{if (! pVarParameters[2].isNumeric()) return false;};
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        int date = pVarParameters[2].GetVarInt();

                        //Get the String value 
                        pVarParameters[1].GetStringValue(ref format, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck
                        //Remove the Language code from the first argument to the second argument
                        GetLanguageCode(ref format, ref szLangCode, ref bLanCodePrese);

                        iSize = DATE_to_string(ref date_str, format, date);

                        //Update the time_value_str to user
                        if (!SetStringParam(pFuncExp, ref pVarParameters, 0, date_str))
                        {
                            return false;
                        }
                        pVarReturnValue.SetValue(iSize, VARIANT_TYPE.RUL_INT);

                        //Delete all the Memory Allocated;
                        if (format != null)
                        {
                            format = null;
                        }
                        return true;
                    }
                ////break;

                case BUILTIN_NAME.BUILTIN_TIME_VALUE_to_string:
                    {
                        string format = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;

                        //Get the String value 
                        pVarParameters[1].GetStringValue(ref format, VARIANT_TYPE.RUL_DD_STRING);
                        //For Second string strip off the Language Code. 
                        GetLanguageCode(ref format, ref szLangCode, ref bLanCodePrese);

                        //{if (! pVarParameters[2].isNumeric()) return false;};
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        uint time_value = (uint)pVarParameters[2].GetVarInt();


                        string time_value_str = "";
                        int iSize = 0;
                        iSize = TIME_VALUE_to_string(ref time_value_str, format, time_value);

                        //Update the time_value_str to user
                        if (!SetStringParam(pFuncExp, ref pVarParameters, 0, time_value_str))
                        {
                            return false;
                        }
                        pVarReturnValue.SetValue(iSize, VARIANT_TYPE.RUL_INT);

                        //Delete all the Memory Allocated;
                        if (format != null)
                        {
                            format = null;
                        }
                        return true;
                    }
                ////break;

                case BUILTIN_NAME.BUILTIN_timet_to_string:
                    {
                        string format = null;
                        string szLangCode = "";
                        bool bLanCodePrese = false;
                        string time_t_str = "";
                        int iSize = 0;

                        //{if (! pVarParameters[2].isNumeric()) return false;};
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        long time_t = (int)pVarParameters[2].GetVarInt();

                        //Get the String value 
                        pVarParameters[1].GetStringValue(ref format, VARIANT_TYPE.RUL_DD_STRING);

                        //For Second string strip off the Language Code. Ideally SPeaking For first also we need to strip off and then Campare and then APend it banck
                        //Remove the Language code from the first argument to the second argument
                        GetLanguageCode(ref format, ref szLangCode, ref bLanCodePrese);

                        iSize = timet_to_string(ref time_t_str, format, time_t);

                        //Update the time_value_str to user
                        if (!SetStringParam(pFuncExp, ref pVarParameters, 0, time_t_str))
                        {
                            return false;
                        }
                        pVarReturnValue.SetValue(iSize, VARIANT_TYPE.RUL_INT);

                        //Delete all the Memory Allocated;
                        if (format != null)
                        {
                            format = null;
                        }
                        return true;
                    }
                ////break;

                case BUILTIN_NAME.BUILTIN_timet_to_TIME_VALUE:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        long timet_value = pVarParameters[0].GetVarInt();

                        uint time_value = timet_to_TIME_VALUE(timet_value);
                        pVarReturnValue.SetValue(time_value, VARIANT_TYPE.RUL_UINT);
                        return true;
                    }
                ////break;
                case BUILTIN_NAME.BUILTIN_To_TIME_VALUE:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};
                        //{if (! pVarParameters[1].isNumeric()) return false;};
                        //{if (! pVarParameters[2].isNumeric()) return false;};
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        if (!pVarParameters[2].isNumeric())
                        {
                            return false;
                        }
                        int hours = pVarParameters[0].GetVarInt();
                        int minutes = (int)pVarParameters[1].GetVarInt();
                        int seconds = (int)pVarParameters[2].GetVarInt();

                        uint time_value = To_TIME_VALUE(hours, minutes, seconds);
                        pVarReturnValue.SetValue(time_value, VARIANT_TYPE.RUL_UINT);
                        return true;
                    }
                ////break;
                /*=============================================end date/time 16jul14 ===========================*/

                //stevev 29jan08 for literal strings in the methods
                case BUILTIN_NAME.BUILTIN_literal_string:
                    {
                        string pchString = null;
                        uint lItemId = 0;   //WS:EPM 24may07	

                        if (pVarParameters[0].isNumeric())
                        {
                            lItemId = pVarParameters[0].GetVarUInt();
                        }

                        int iReturnValue = literal_string(lItemId, ref pchString);

                        if (pchString != null)
                        {
                            if((object)pVarReturnValue == null)
                            {
                                pVarReturnValue = new INTER_VARIANT();
                            }
                            pVarReturnValue.SetValue(pchString, VARIANT_TYPE.RUL_DD_STRING);
                            //pchString = null;
                        }
                        return true;
                    }
                ////break;
                /**************************** Begin Block Transfer Builtins (eDDL) ********************/
                case BUILTIN_NAME.BUILTIN_openTransferPort:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// port number is only parameter	
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        //int iportNumber = pVarParameters[0].GetVarInt();

                        int iReturnValue = Common.SUCCESS;//openPort();

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                ////break;
                case BUILTIN_NAME.BUILTIN_closeTransferPort:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// port number is only parameter	
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        //int iportNumber = pVarParameters[0].GetVarInt();

                        int iReturnValue = Common.SUCCESS;// closePort();

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                ////break;
                case BUILTIN_NAME.BUILTIN_abortTransferPort:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// port number is only parameter	
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int iportNumber = pVarParameters[0].GetVarInt();

                        int iReturnValue = Common.SUCCESS;// closePort();// abortPort(iportNumber);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                ////break;
                ////////////////////////////////////////////////////////////////////////////////////
                case BUILTIN_NAME.BUILTIN_writeItem2Port:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// port number is first parameter	
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int iportNumber = pVarParameters[0].GetVarInt();
                        //{if (! pVarParameters[1].isNumeric()) return false;};// item number is last parameter	
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        ushort iItemNumber = (ushort)pVarParameters[1].GetVarInt();

                        int iReturnValue = BI_SUCCESS;// write2Port(iportNumber, iItemNumber);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_readItemfromPort:
                    {
                        //{if (! pVarParameters[0].isNumeric()) return false;};// port number is first parameter	
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }

                        int iportNumber = pVarParameters[0].GetVarInt();
                        //{if (! pVarParameters[1].isNumeric()) return false;};// item number is last parameter	
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        ushort iItemNumber = (ushort)pVarParameters[1].GetVarInt();

                        int iReturnValue = BI_SUCCESS;//readFromPort(iportNumber, iItemNumber);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN_getTransferStatus:
                    {
                        if (!pVarParameters[0].isNumeric())
                        {
                            return false;
                        }
                        int iportNumber = pVarParameters[0].GetVarInt();
                        if (!pVarParameters[1].isNumeric())
                        {
                            return false;
                        }
                        int iDirection = (int)pVarParameters[1].GetVarInt();

                        int[] pLongItemIds = new int[10];
                        int iNumberOfItemIds = 0;

                        if (pVarParameters[2].GetVarType() == VARIANT_TYPE.RUL_SAFEARRAY)
                        {
                            GetLongArray(pVarParameters[2], pLongItemIds, ref iNumberOfItemIds);
                        }
                        else
                        {
                            iNumberOfItemIds = 0;
                        }

                        int iReturnValue = getTransferStatus(iportNumber, ref pLongItemIds, iNumberOfItemIds);

                        pVarReturnValue.SetValue(iReturnValue, VARIANT_TYPE.RUL_INT);
                        return true;
                    }
                //break;

                /****************************  End Block Transfer Builtins (eDDL) *********************/
                /****************************  Debug builtins (eddl) from emerson 16jul14 *************/


                case BUILTIN_NAME.BUILTIN__ERROR:
                    {
                        string pchInputString = "ERROR:";
                        string strpI = "";
                        if (!GetStringParam(ref strpI, ref pVarParameters, 0))
                        {
                            //return false;
                        }

                        //LOGIT(CERR_LOG, pchInputString);
                        pchInputString += strpI;//record this??????//////

                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN__WARNING:
                    {
                        string pchInputString = "WARNING:";
                        string strpI = "";
                        if (!GetStringParam(ref strpI, ref pVarParameters, 0))
                        {
                            //return false;
                        }

                        //LOGIT(CERR_LOG, pchInputString);
                        pchInputString += strpI;

                        return true;
                    }
                //break;
                case BUILTIN_NAME.BUILTIN__TRACE:
                    {
                        string pchInputString = "TRACE:";
                        string strpI = "";
                        if (!GetStringParam(ref strpI, ref pVarParameters, 0))
                        {
                            //return false;
                        }

                        //LOGIT(CLOG_LOG, pchInputString);
                        pchInputString += strpI;

                        return true;
                    }
                //break;

                default:
                    break;

                    //Added By Anil June 17 2005 --Ends here
                    /****************************End of DD_STRING  Builtins (eDDL) ********************/
            }


            /* End of code */

            return false;
        }

    }

    public class INTER_VARIANT
    {
        public __VAL val;
        VARIANT_TYPE varType;
        //string charout;// for conversion return value from wide to standard

        public INTER_VARIANT()
        {
            val = new __VAL();
            varType = VARIANT_TYPE.RUL_BOOL;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void SetValue(INTER_SAFEARRAY sa)
        {
            varType = VARIANT_TYPE.RUL_SAFEARRAY;
            int aryTyp = (int)sa.Type();
            int arySiz = sa.GetNumberOfElements();
            int i;
            int elemSz = sa.GetElementSize();
            INTER_VARIANT localIV = new INTER_VARIANT();

            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                case VARIANT_TYPE.RUL_CHAR:
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                case VARIANT_TYPE.RUL_SHORT:
                case VARIANT_TYPE.RUL_USHORT:
                case VARIANT_TYPE.RUL_INT:
                case VARIANT_TYPE.RUL_UINT:
                case VARIANT_TYPE.RUL_LONGLONG:
                case VARIANT_TYPE.RUL_ULONGLONG:
                case VARIANT_TYPE.RUL_FLOAT:
                case VARIANT_TYPE.RUL_DOUBLE:
                    /* error --- attempt to set a numeric from an array */
                    break;//return cleared

                /*  safearray to string is currently only allowed for matching types 
                    eg array of wide chars to widecharptr  **/

                case VARIANT_TYPE.RUL_CHARPTR:
                    if (aryTyp == (int)VARIANT_TYPE.RUL_CHAR)
                    {
                        sa.GetElement(0, ref localIV);

                        localIV.GetStringValue(ref val.pzcVal);

                        /*
                        val.pzcVal = ;// new byte[arySiz + 1];
                        for (i = 0; i < arySiz; i++)
                        {
                            sa.GetElement(i, ref localIV);
                            val.pzcVal[i] = (char)localIV;
                            localIV.Clear();
                        }
                        val.pzcVal[arySiz] = 0;
                        */
                    }
                    // else conversion desired... not supported at this time
                    break;
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                case VARIANT_TYPE.RUL_DD_STRING:
                    if (aryTyp == (int)VARIANT_TYPE.RUL_USHORT)
                    {
                        sa.GetElement(0, ref localIV);

                        localIV.GetStringValue(ref val.pzcVal);

                        /*
                        val.pszValue = sa.;// new wchar_t[arySiz + 1];
                        /*
                        int offset = 0;
                        for (i = 0; i < arySiz; offset += elemSz, i++)
                        {
                            sa.GetElement((uint)offset, ref localIV);
                            val.pszValue[i] = (wchar_t)localIV;
                            localIV.Clear();
                        }
                        */
                    }
                    // else conversion desired... not supported at this time
                    break;
                case VARIANT_TYPE.RUL_BYTE_STRING:
                    if (aryTyp == (int)VARIANT_TYPE.RUL_UNSIGNED_CHAR)
                    {
                        val.bString.bsLen = arySiz;
                        val.bString.bs = new byte[arySiz];
                        for (i = 0; i < arySiz; i++)
                        {
                            sa.GetElement((uint)i, ref localIV);
                            val.bString.bs[i] = localIV.GetVarByte();
                            localIV.Clear();
                        }
                        val.bString.bs[arySiz] = 0;
                    }
                    // else conversion desired... not supported at this time
                    break;

                case VARIANT_TYPE.RUL_SAFEARRAY:
                    if (val.prgsa == null)
                    {
                        val.prgsa = new INTER_SAFEARRAY();//allocate new safe array
                        val.prgsa = sa;
                    }
                    else
                    {
                        (val.prgsa) = sa;
                    }
                    break;
                case VARIANT_TYPE.RUL_NULL:
                default:
                    /* throw an error - this is impossible to get into */
                    break;
            }
        }

        public void SetValue(_BYTE_STRING f)
        {
            int inLen = f.bsLen;
            int storeLen = inLen + 1;

            //CLEAR_DATA(RUL_BYTE_STRING);

            if (varType == VARIANT_TYPE.RUL_CHARPTR)
            {// byte to string conversion
                if (f.bs != null)
                {
                    val.pzcVal = Encoding.Default.GetString(f.bs);                  // force string termination if not-a-string
                }
                else
                {
                    val.pzcVal = "\0";
                }
            }
            else
            if (varType == VARIANT_TYPE.RUL_WIDECHARPTR || varType == VARIANT_TYPE.RUL_DD_STRING)
            {// byte to wide
                val.pszValue = Encoding.Unicode.GetString(f.bs);// force string termination if not-a-string//////
            }
            else
            if (varType == VARIANT_TYPE.RUL_BYTE_STRING)
            {
                val.bString.bs = f.bs;
                val.bString.bsLen = inLen;
                //memcpy(val.bString.bs, (byte *)f.bs,inLen);
            }
            else
            if (varType == VARIANT_TYPE.RUL_SAFEARRAY)
            {
                //if (val.prgsa == null)
                val.prgsa = new INTER_SAFEARRAY(f);
                //else
                //val.prgsa = f;
            }
            // else throw an error - its a string to numeric conversion attempt!!!!!!!!!!!!!!!

        }

        public void SetValue(object value, VARIANT_TYPE type)
        {
            varType = type;
            switch (type)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    val.bValue = (bool)value;
                    break;

                case VARIANT_TYPE.RUL_BYTE_STRING:
                    val.bString = (_BYTE_STRING)value;
                    break;

                case VARIANT_TYPE.RUL_CHAR:
                    val.cValue = (byte)value;
                    break;

                case VARIANT_TYPE.RUL_CHARPTR:
                    val.pzcVal = (string)value;
                    break;

                case VARIANT_TYPE.RUL_DD_STRING:
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                    val.pszValue = (string)value;
                    break;

                case VARIANT_TYPE.RUL_DOUBLE:
                    val.dValue = (double)value;
                    break;

                case VARIANT_TYPE.RUL_FLOAT:
                    val.fValue = (float)value;
                    break;

                case VARIANT_TYPE.RUL_INT:
                    val.nValue = (int)value;
                    break;

                case VARIANT_TYPE.RUL_LONGLONG:
                    val.lValue = (Int64)value;
                    break;

                case VARIANT_TYPE.RUL_SAFEARRAY:
                    val.prgsa = (INTER_SAFEARRAY)value;
                    break;

                case VARIANT_TYPE.RUL_SHORT:
                    val.sValue = (short)value;
                    break;

                case VARIANT_TYPE.RUL_UINT:
                    val.unValue = (uint)value;
                    break;

                case VARIANT_TYPE.RUL_ULONGLONG:
                    val.ulValue = (UInt64)value;
                    break;

                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    val.ucValue = (byte)value;
                    break;

                case VARIANT_TYPE.RUL_USHORT:
                    val.usValue = (ushort)value;
                    break;

                default:
                    break;
            }
        }

        int narrowStr2number(string pStr)
        {
            int nLen = 0;
            //int nVal = 0;
            Int64 lVal = 0;
            UInt64 ulVal = 0;
            int i = 0;
            double fVal = 0;
            //string pEnd = null;
            bool bIsFloat = false;
            bool bIsHex = false, bIsOctal = false, bIsNeg = false;
            //int iBase = 10;

            if (pStr == null || (nLen = pStr.Length) == 0)
            {
                return -1; // failure
            }

            /************************* reworked by stevev 10oct05 *********************************/
            if (nLen >= 2)
            {
                bIsNeg = (pStr[0] == '-');
                if (pStr[0] == '0')
                {// has to be octal || hex
                    if ((pStr[1] == 'x') || (pStr[1] == 'X'))
                    {
                        bIsHex = true;
                        //iBase = 16;
                        //sscanf(pStr, "%I64x", &ulVal);
                        ScanFormatted parser = new ScanFormatted();
                        parser.Parse(pStr, "%x");
                        ulVal = Convert.ToUInt64(parser.Results[0]);//(uint)((int)parser.Results[0]);
                    }
                    else if (pStr[1] >= '0' && pStr[1] < '8')
                    {// octal
                        bIsOctal = true;
                        //iBase = 8;
                        //sscanf(pStr, "%I64o", &ulVal);
                        ScanFormatted parser = new ScanFormatted();
                        parser.Parse(pStr, "%0");
                        ulVal = Convert.ToUInt64(parser.Results[0]);//(uint)((int)parser.Results[0]);
                    }
                    else
                    {// float or decimal - actually an error....
                        for (i = 1; i < nLen; i++)// we'll try to recover via float
                        {
                            if (pStr[i] == '.' || pStr[i] == 'E' || pStr[i] == 'e')
                            {
                                bIsFloat = true;
                                break;
                            }
                        }
                        if (!bIsFloat)
                        {// if all still false then its decimal eg 0999 - actually an error
                         // throw error
                            return -2;
                        }
                    }//end else			
                }
                else
                {// starts with a non-zero [1-9+\-]
                 // Walt EPM - 17oct08- make '.025' work as well as '0.025'
                    for (i = 0; i < nLen; i++)// we'll try to recover via float/
                    {
                        if (pStr[i] == '.' || pStr[i] == 'E' || pStr[i] == 'e')
                        {
                            bIsFloat = true;
                            break;
                        }
                    }
                }// endelse a decimal/float
            }
            // else: length == 1, can't be octal or hex or float, must be decimal...process as such

            if (bIsFloat)
            {
                //retVal.fValue = (float)atof(pStr);
                val.dValue = Convert.ToDouble(pStr);//strtod(pStr, &pEnd);
                if ((val.dValue <= float.MaxValue && val.dValue >= float.MinValue) ||
                     (val.dValue > (-float.MaxValue) && val.dValue < (-float.MinValue)))
                {
                    fVal = val.dValue;
                    val.fValue = (float)fVal;
                    varType = VARIANT_TYPE.RUL_FLOAT;
                }
                else
                {
                    varType = VARIANT_TYPE.RUL_DOUBLE;
                }
            }
            else
            {// decimal is the only option left
             //		we just gotta figure out how big
                if ((!bIsHex) && (!bIsOctal))
                {// we haven't scanned it yet, do it now
                 //iBase = 10;
                    if (bIsNeg)
                    {
                        //sscanf(pStr, "%I64d", &lVal);
                        ScanFormatted parser = new ScanFormatted();
                        parser.Parse(pStr, "%d");
                        //lVal = (int)parser.Results[0];
                        lVal = Convert.ToInt64(parser.Results[0]);
                    }
                    else// go ushort until proven otherwise
                    {
                        //sscanf(pStr, "%I64d", &ulVal);
                        ScanFormatted parser = new ScanFormatted();
                        parser.Parse(pStr, "%d");
                        //ulVal = (uint)((int)parser.Results[0]);
                        ulVal = Convert.ToUInt64(parser.Results[0]);
                    }
                }

                if (bIsNeg)
                {
                    val.lValue = lVal;
                    varType = VARIANT_TYPE.RUL_LONGLONG;
                }
                else
                {// we are non-negative
                    if (ulVal <= Int64.MaxValue)//WHS EP June17-2008 dont constrain constants to short/char/int - default to natural size.
                    {
                        val.lValue = (Int64)ulVal;
                        varType = VARIANT_TYPE.RUL_LONGLONG;
                    }
                    else
                    {
                        val.ulValue = ulVal;
                        varType = VARIANT_TYPE.RUL_ULONGLONG;
                    }
                }
            }

            return 0; // SUCCESS
        }

        public INTER_VARIANT(bool bIsNumber, string szNumber)
        {
            varType = VARIANT_TYPE.RUL_NULL;
            if (bIsNumber && szNumber != null)
            {
                if (narrowStr2number(szNumber) != Common.SUCCESS)
                {// throw error
                    varType = VARIANT_TYPE.RUL_NULL;
                }
                // else - return what we have
            }
            else
            {// NaN
             // leave clear and null
            }
        }

        public INTER_SAFEARRAY GetSafeArray()
        {
            if (varType == VARIANT_TYPE.RUL_SAFEARRAY)
            {
                return val.prgsa;
            }
            else
            {
                return null;
            }
        }

        public int XMLize(ref string szData)
        {
            string str = "";
            switch (varType)
            {
                case VARIANT_TYPE.RUL_NULL:
                    //sprintf(str, "%d,", 0);
                    str = String.Format("%d,", 0);
                    break;
                case VARIANT_TYPE.RUL_BOOL:
                    str = String.Format("%d,", val.bValue ? 1 : 0);
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    str = String.Format("%c,", val.cValue);
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    str = String.Format("%u,", (uint)val.ucValue);
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    str = String.Format("%hd,", val.sValue);
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    str = String.Format("%hu,", val.usValue);
                    break;
                case VARIANT_TYPE.RUL_INT:
                    str = String.Format("%d,", val.nValue);
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    str = String.Format("%u,", val.unValue);
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    str = String.Format("%I64d,", val.lValue);
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    str = String.Format("%I64u,", val.ulValue);
                    break;

                case VARIANT_TYPE.RUL_FLOAT:
                    str = String.Format("%.5f,", val.fValue);
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    str = String.Format("%.12g,", val.dValue);
                    break;

                case VARIANT_TYPE.RUL_CHARPTR:
                    str = String.Format("%s,", val.pzcVal);
                    break;
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                    str = String.Format("%S,", val.pszValue);
                    break;
                case VARIANT_TYPE.RUL_DD_STRING:
                    str = String.Format("%S,", val.pszValue);
                    break;
                case VARIANT_TYPE.RUL_BYTE_STRING:
                    {
                        str = String.Format("%d:", val.bString.bsLen);
                        for (int j = 0; j < (int)val.bString.bsLen; j++)
                        {
                            string num = String.Format("%02x ", val.bString.bs[j]);
                            str += num;
                        }
                        str += ";";
                    }
                    break;
                case VARIANT_TYPE.RUL_SAFEARRAY:
                    (val.prgsa).XMLize(ref szData);
                    break;
                default:
                    break;
            }
            szData += str;
            return 0;
        }

        void Clear()// releases memory, sets to RUL_NULL
        {
            switch (varType)
            {
                case VARIANT_TYPE.RUL_NULL:
                    break;
                case VARIANT_TYPE.RUL_BOOL:
                case VARIANT_TYPE.RUL_CHAR:
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                case VARIANT_TYPE.RUL_INT:
                case VARIANT_TYPE.RUL_SHORT:
                case VARIANT_TYPE.RUL_UINT:
                case VARIANT_TYPE.RUL_USHORT:
                case VARIANT_TYPE.RUL_LONGLONG:
                case VARIANT_TYPE.RUL_ULONGLONG:
                case VARIANT_TYPE.RUL_FLOAT:
                case VARIANT_TYPE.RUL_DOUBLE:
                    varType = VARIANT_TYPE.RUL_NULL;
                    val = new __VAL();
                    break;
                case VARIANT_TYPE.RUL_CHARPTR:
                    varType = VARIANT_TYPE.RUL_NULL;
                    if (val.pzcVal != null)
                    {
                        val.pzcVal = null;
                    }
                    //memset(val, 0, sizeof(val));
                    val = new __VAL();
                    break;
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                case VARIANT_TYPE.RUL_DD_STRING:
                    varType = VARIANT_TYPE.RUL_NULL;
                    if (val.pszValue != null)
                    {
                        val.pszValue = null;
                    }
                    //memset(val, 0, sizeof(val));
                    val = new __VAL();
                    break;
                case VARIANT_TYPE.RUL_BYTE_STRING:
                    varType = VARIANT_TYPE.RUL_NULL;
                    if (val.bString.bs != null)
                    {
                        val.bString.bs = null;
                    }
                    val = new __VAL();
                    break;
                case VARIANT_TYPE.RUL_SAFEARRAY:
                    varType = VARIANT_TYPE.RUL_NULL;
                    //Anil 250407 I am wondering why this is here from such a long time.
                    //This memory gets allocated during the Declaration List execution(ie char sztemp[100];). 
                    //This should not be deleted afterwards.<were does it go?...sjv 01jun07??>
                    //Hence Commenting
                    if (val.prgsa != null)// uncommented WS:EPM 17jul07 checkin
                    {
                        val.prgsa = null;
                    }
                    val = new __VAL();
                    break;
            }
        }

        public void SetValue(string psz, VARIANT_TYPE vt)
        {
            varType = vt;
            SetValue(psz);
        }

        public void SetValue(double input, VARIANT_TYPE vt)
        {
            varType = vt;
            byte[] by = new byte[8];
            by = BitConverter.GetBytes(input);
            SetValue(by, 0, vt);
        }

        public void SetValue(int input, VARIANT_TYPE vt)
        {
            varType = vt;
            byte[] by = new byte[4];
            by = BitConverter.GetBytes(input);
            SetValue(by, 0, vt);
        }

        public void SetValue(string psz)
        {
            int inLen = psz.Length;
            int storeLen = inLen + 1;

            if (varType == VARIANT_TYPE.RUL_CHARPTR)
            {
                val.pzcVal = psz;
            }
            else
            if (varType == VARIANT_TYPE.RUL_WIDECHARPTR || varType == VARIANT_TYPE.RUL_DD_STRING)
            {// narrow to wide conversion (destination never changes in operator=)
                val.pszValue = psz;
            }
            else if (varType == VARIANT_TYPE.RUL_BYTE_STRING)
            {// converted to ushort
                val.bString.bs = Encoding.Default.GetBytes(psz);
            }
            else if (varType == VARIANT_TYPE.RUL_SAFEARRAY)
            {
                if (val.prgsa == null)
                {
                    val.prgsa = new INTER_SAFEARRAY();
                }
                val.prgsa.m_data.pvData = Encoding.Default.GetBytes(psz);//////
                                                                         //else
                                                                         //    *(val.prgsa) = psz;
            }
        }
        //public void SetValue(bool bv, VARIANT_TYPE vt = VARIANT_TYPE.RUL_BOOL)

        public void SetValue(byte[] pmem, uint uiOff, VARIANT_TYPE vt)
        {
            Clear();
            val = new __VAL();
            varType = vt;
            //uint uiOff = 0;//////
            if (pmem == null) //* throw an error /
                return;
            //int L = 0;
            switch (vt)
            {
                case VARIANT_TYPE.RUL_NULL:
                    //throw error
                    break;
                case VARIANT_TYPE.RUL_BOOL:
                    //memcpy((val.bValue), pmem, sizeof(val.bValue));
                    if (pmem[uiOff] > 0)
                    {
                        val.bValue = true;
                    }
                    else
                    {
                        val.bValue = false;
                    }
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    //memcpy((val.ucValue), pmem, sizeof(val.ucValue));
                    val.ucValue = pmem[uiOff];
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    val.cValue = pmem[uiOff];
                    //memcpy((val.cValue), pmem, sizeof(val.cValue));
                    break;
                case VARIANT_TYPE.RUL_INT:
                    //memcpy((val.nValue), pmem, sizeof(val.nValue));
                    val.nValue = (pmem[uiOff + 3] << 24) + (pmem[uiOff + 2] << 16) + (pmem[uiOff + 1] << 8) + pmem[uiOff];
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    val.sValue = (short)((pmem[uiOff + 1] << 8) + pmem[uiOff]);
                    //memcpy((val.sValue), pmem, sizeof(val.sValue));
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    //memcpy((val.unValue), pmem, sizeof(val.unValue));
                    val.unValue = (uint)((pmem[uiOff + 3] << 24) + (pmem[uiOff + 2] << 16) + (pmem[uiOff + 1] << 8) + pmem[uiOff]);
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    val.usValue = (ushort)((pmem[uiOff + 1] << 8) + pmem[uiOff]);
                    //memcpy((val.usValue), pmem, sizeof(val.usValue));
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    //memcpy((val.lValue), pmem, sizeof(val.lValue));
                    //val.lValue = pmem[uiOff + 7] >> 56 + pmem[uiOff + 2] >> 48 + pmem[uiOff + 1] >> 32 + pmem[uiOff] + pmem[uiOff + 3] >> 24 + pmem[uiOff + 2] >> 16 + pmem[uiOff + 1] >> 8 + pmem[uiOff];
                    val.lValue = BitConverter.ToInt64(pmem, (int)uiOff);
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    //memcpy((val.ulValue), pmem, sizeof(val.ulValue));
                    //val.ulValue = pmem[uiOff + 7] >> 56 + pmem[uiOff + 2] >> 48 + pmem[uiOff + 1] >> 32 + pmem[uiOff] + pmem[uiOff + 3] >> 24 + pmem[uiOff + 2] >> 16 + pmem[uiOff + 1] >> 8 + pmem[uiOff];
                    val.ulValue = BitConverter.ToUInt64(pmem, (int)uiOff);
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    //memcpy((val.fValue), pmem, sizeof(val.fValue));
                    val.fValue = BitConverter.ToSingle(pmem, (int)uiOff);
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    //memcpy((val.dValue), pmem, sizeof(val.dValue));
                    val.dValue = BitConverter.ToDouble(pmem, (int)uiOff);
                    break;
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                case VARIANT_TYPE.RUL_DD_STRING:
                    /*
                    L = wcslen((string)pmem);
                    // stevev 24sep10 - this doesn't handle empty strings ("")
                    //if ( L > 0 && L <= MAX_DD_STRING)
                    if (L <= CHart_Builtins.MAX_DD_STRING)
                    {// including zero length - the empty string
                        val.pszValue = new string[L + 1];
                        wcscpy(val.pszValue, (string)pmem);
                    }
                    else
                    {// just use what we can
                        val.pszValue = new string[MAX_DD_STRING + 1];
                        wcsncpy(val.pszValue, (string)pmem, MAX_DD_STRING);
                        val.pszValue[MAX_DD_STRING] = 0;
                    }*/

                    val.pszValue = Encoding.Default.GetString(pmem, (int)uiOff, (int)(pmem.Length - uiOff));
                    break;
                case VARIANT_TYPE.RUL_CHARPTR:
                    /*
                    L = strlen((char*)pmem);
                    if (L > 0 && L <= MAX_DD_STRING)
                    {
                        val.pzcVal = new char[L + 1];
                        strcpy(val.pzcVal, (char*)pmem);
                    }// else leave it empty
                    */
                    val.pzcVal = Encoding.Default.GetString(pmem, (int)uiOff, (int)(pmem.Length - uiOff));
                    break;
                case VARIANT_TYPE.RUL_SAFEARRAY:
                    //val.prgsa = new INTER_SAFEARRAY(*((INTER_SAFEARRAY*)pmem));
                    byte[] dat = new byte[(int)(pmem.Length - uiOff)];
                    for (int i = 0; i < dat.Length; i++)
                    {
                        dat[i] = pmem[uiOff + i];
                    }
                    using (MemoryStream ms = new MemoryStream(dat))
                    {
                        IFormatter iFormatter = new BinaryFormatter();
                        val.prgsa = (INTER_SAFEARRAY)iFormatter.Deserialize(ms);
                    }
                    break;
                case VARIANT_TYPE.RUL_BYTE_STRING:
                    /*
                    L = ((_BYTE_STRING*)pmem).bsLen;
                    if (L > 0 && L <= MAX_DD_STRING)// arbitrary max
                    {
                        val.bString.bs = new byte[L];
                        memcpy(val.bString.bs, (byte*)pmem, L);
                    }// else leave it empty
                    */
                    val.bString.bs = new byte[(int)(pmem.Length - uiOff)];
                    for (int i = 0; i < val.bString.bs.Length; i++)
                    {
                        val.bString.bs[i] = pmem[uiOff + i];
                    }
                    break;
                default:
                    varType = VARIANT_TYPE.RUL_NULL;
                    break;
            }
        }

        public void GetValue(ref byte[] pme, uint uiOff, VARIANT_TYPE vt)
        {
            byte[] pmem;
            if (isNumeric())//else, string conversion is not supported
            {
                switch (vt)
                {
                    case VARIANT_TYPE.RUL_BOOL:
                        //v.bValue = (bool)(*this);
                        //memcpy(pmem, &(v.bValue), sizeof(bool));
                        if (val.bValue)
                        {
                            pme[uiOff] = (byte)1;
                        }
                        else
                        {
                            pme[uiOff] = (byte)0;
                        }
                        break;
                    case VARIANT_TYPE.RUL_CHAR:
                        //v.cValue = (char)(*this);
                        //memcpy(pmem, &(v.cValue), sizeof(char));
                        pme[uiOff] = (byte)val.cValue;
                        break;
                    case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                        //v.ucValue = (ushort char)(*this);
                        //memcpy(pmem, &v.ucValue, sizeof(ushort char));
                        pme[uiOff] = (byte)val.ucValue;
                        break;
                    case VARIANT_TYPE.RUL_SHORT:
                        //v.sValue = (short)(*this);
                        //memcpy(pmem, &(v.sValue), sizeof(short));
                        pmem = BitConverter.GetBytes(val.sValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_USHORT:
                        //v.usValue = (ushort short)(*this);
                        //memcpy(pmem, &(v.usValue), sizeof(ushort short));
                        pmem = BitConverter.GetBytes(val.usValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_INT:
                        //v.nValue = (int)(*this);
                        //memcpy(pmem, &(v.nValue), sizeof(int));
                        pmem = BitConverter.GetBytes(val.nValue);
                        for (int i = 0; i < 4/*pme.Length*/; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_UINT:
                        //v.unValue = (uint)(*this);
                        //memcpy(pmem, &(v.unValue), sizeof(uint));
                        pmem = BitConverter.GetBytes(val.unValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_LONGLONG:
                        //v.lValue = (__Int64)(*this);
                        //memcpy(pmem, &(v.lValue), sizeof(__Int64));
                        pmem = BitConverter.GetBytes(val.lValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_ULONGLONG:
                        //v.ulValue = (ushort __Int64)(*this);
                        //memcpy(pmem, &(v.ulValue), sizeof(ushort __Int64));
                        pmem = BitConverter.GetBytes(val.ulValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_FLOAT:
                        //v.fValue = (float)*this;
                        //memcpy(pmem, &(v.fValue), sizeof(float));
                        pmem = BitConverter.GetBytes(val.fValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    case VARIANT_TYPE.RUL_DOUBLE:
                        //v.dValue = *this;
                        //memcpy(pmem, &(v.dValue), sizeof);
                        pmem = BitConverter.GetBytes(val.dValue);
                        for (int i = 0; i < pme.Length; i++)
                        {
                            pme[uiOff + i] = pmem[i];
                        }
                        break;
                    default:
                        // unsupported conversion error 
                        break;
                }
            }
            else
            {// NON numeric
                INTER_VARIANT localVar = new INTER_VARIANT();
                //localVar = vt;// set the type
                //localVar = (*this);// converts self to desired type via operator equal
                switch (vt)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        {
                            //char* pchar = localVar.GetValue().pzcVal;
                            //memcpy(pmem, pchar, strlen(pchar) + 1);
                        }
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        {
                            //string pchar = localVar.GetValue().pszValue;
                            //memcpy(pmem, pchar, wcslen(pchar) + 1);
                            Encoding.Default.GetBytes(val.pszValue);
                        }
                        break;
                    case VARIANT_TYPE.RUL_BYTE_STRING:
                        {
                            //ushort char* pchar = localVar.GetValue().bString.bs;
                            //memcpy(pmem, pchar, localVar.GetValue().bString.bsLen);
                            Encoding.Default.GetBytes(val.pzcVal);
                        }
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        {
                            /*
                            INTER_SAFEARRAY* pchar = localVar.GetSafeArray();
                            if (pchar)
                                //	memcpy(pmem,pchar.getDataPtr(),pchar.MemoryAllocated());
                                memcpy(pmem, (char*)pchar, pchar.MemoryAllocated());
                                */
                            using (MemoryStream ms = new MemoryStream())
                            {
                                IFormatter iFormatter = new BinaryFormatter();
                                iFormatter.Serialize(ms, val.prgsa);
                                pmem = ms.GetBuffer();
                            }
                        }
                        break;
                    case VARIANT_TYPE.RUL_NULL:
                    default:    // throw an error this can't happen /
                        break;
                }
            }// end else numeric or not
        }

        public static INTER_VARIANT operator +(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue + temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue + temp.val.fValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue + temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue + temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue + temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue + temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue + temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue + temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue + temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue + temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;
            }
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal + var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator -(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue - temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue - temp.val.fValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue - temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue - temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue - temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue - temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue - temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue - temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue - temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue - temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator *(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue * temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue * temp.val.fValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue * temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue * temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue * temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue * temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue * temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue * temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue * temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue * temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator /(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue / temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue / temp.val.fValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue / temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue / temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue / temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue / temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue / temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue / temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue / temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue / temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public uint GetVarUInt()
        {
            uint retVal = 0;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    if (val.bValue)
                    {
                        retVal = (uint)1;
                    }
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = (uint)val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = (uint)val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = (uint)val.sValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = (uint)val.usValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = (uint)val.nValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = (uint)val.unValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = (uint)val.lValue;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = (uint)val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = (uint)val.fValue;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = (uint)val.dValue;
                    break;

                /* for now, all strings will NOT cast to a const */
                default:
                    retVal = (uint)0;
                    break;
            }// end switch
            return retVal;
        }

        public byte GetVarByte()
        {
            byte retVal = 0;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    if (val.bValue)
                    {
                        retVal = (byte)1;
                    }
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = (byte)val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = (byte)val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = (byte)val.sValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = (byte)val.usValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = (byte)val.nValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = (byte)val.unValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = (byte)val.lValue;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = (byte)val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = (byte)val.fValue;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = (byte)val.dValue;
                    break;

                /* for now, all strings will NOT cast to a const */
                default:
                    retVal = (byte)0;
                    break;
            }// end switch
            return retVal;
        }

        public int GetVarInt()
        {
            int retVal = 0;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    if (val.bValue)
                    {
                        retVal = (int)1;
                    }
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = (int)val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = (int)val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = (int)val.sValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = (int)val.usValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = (int)val.nValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = (int)val.unValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = (int)val.lValue;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = (int)val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = (int)val.fValue;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = (int)val.dValue;
                    break;

                /* for now, all strings will NOT cast to a const */
                default:
                    retVal = (int)0;
                    break;
            }// end switch
            return retVal;
        }

        public Int64 GetVarInt64()
        {
            Int64 retVal = 0;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    if (val.bValue)
                    {
                        retVal = (Int64)1;
                    }
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = (Int64)val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = (Int64)val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = (Int64)val.sValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = (Int64)val.usValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = (Int64)val.nValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = (Int64)val.unValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = (Int64)val.lValue;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = (Int64)val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = (Int64)val.fValue;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = (Int64)val.dValue;
                    break;

                /* for now, all strings will NOT cast to a const */
                default:
                    retVal = (Int64)0;
                    break;
            }// end switch
            return retVal;
        }

        public double GetVarDouble()
        {
            double retVal = 0.0;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    if (val.bValue)
                    {
                        retVal = 1.0;
                    }
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = val.sValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = val.usValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = val.nValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = val.unValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = val.lValue;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = val.fValue;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = val.dValue;
                    break;

                /* for now, all strings will NOT cast to a const */
                default:
                    retVal = 0;
                    break;
            }// end switch
            return retVal;
        }

        public float GetVarFloat()
        {
            float retVal = 0;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    if (val.bValue)
                    {
                        retVal = (float)1;
                    }
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = (float)val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = (float)val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = (float)val.sValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = (float)val.usValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = (float)val.nValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = (float)val.unValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = (float)val.lValue;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = (float)val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = (float)val.fValue;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = (float)val.dValue;
                    break;

                /* for now, all strings will NOT cast to a const */
                default:
                    retVal = (float)0;
                    break;
            }// end switch
            return retVal;
        }

        public static INTER_VARIANT operator %(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue % temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue % temp.val.fValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue % temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue % temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue % temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue % temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue % temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue % temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue % temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue % temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator &(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            /*
            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue & temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue & temp.val.fValue;
            }
            else */
            if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue & temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue & temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue & temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue & temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue & temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue & temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue & temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue & temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator |(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            /*
            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue | temp.val.dValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue | temp.val.fValue;
            }
            else*/
            if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue | temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue | temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue | temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue | temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue | temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue | temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue | temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue | temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator ^(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            /*
            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue | temp.val.dValue;
            }
            else 
            if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue | temp.val.fValue;
            }
            else*/
            if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue ^ temp.val.ulValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue ^ temp.val.lValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue ^ temp.val.unValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue ^ temp.val.nValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue ^ temp.val.usValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue ^ temp.val.sValue);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue ^ temp.val.ucValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue ^ temp.val.cValue);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }
            /*
            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator >>(INTER_VARIANT var1, int temp)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            //int temp = var;

            /*
            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue | temp.val.dValue;
            }
            else 
            if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue | temp.val.fValue;
            }
            else*/
            if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue >> temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue >> temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue >> temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue >> temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue >> temp);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue >> temp);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue >> temp);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue >> temp);
            }
            /*
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }

            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator <<(INTER_VARIANT var1, int temp)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            //int temp = var;

            /*
            if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
            {
                retValue.varType = VARIANT_TYPE.RUL_DOUBLE;
                retValue.val.dValue = var1.val.dValue | temp.val.dValue;
            }
            else 
            if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
            {
                retValue.varType = VARIANT_TYPE.RUL_FLOAT;
                retValue.val.fValue = var1.val.fValue | temp.val.fValue;
            }
            else*/
            if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_ULONGLONG;
                retValue.val.ulValue = var1.val.ulValue << temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG))
            {
                retValue.varType = VARIANT_TYPE.RUL_LONGLONG;
                retValue.val.lValue = var1.val.lValue << temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_UINT))
            {
                retValue.varType = VARIANT_TYPE.RUL_UINT;
                retValue.val.unValue = var1.val.unValue << temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_INT))
            {
                retValue.varType = VARIANT_TYPE.RUL_INT;
                retValue.val.nValue = var1.val.nValue << temp;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_USHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_USHORT;
                retValue.val.usValue = (ushort)(var1.val.usValue << temp);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_SHORT))
            {
                retValue.varType = VARIANT_TYPE.RUL_SHORT;
                retValue.val.sValue = (short)(var1.val.sValue << temp);
            }// end switch
            else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
            {
                retValue.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                retValue.val.ucValue = (byte)(var1.val.ucValue << temp);
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHAR))
            {// not both - we can't do var1...
                retValue.varType = VARIANT_TYPE.RUL_CHAR;
                retValue.val.cValue = (byte)(var1.val.cValue << temp);
            }
            /*
            else if ((var1.varType == VARIANT_TYPE.RUL_BOOL))
            {
                retValue.varType = VARIANT_TYPE.RUL_BOOL;
                retValue.val.bValue = var1.val.bValue & temp.val.bValue;//////
            }

            else if ((((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var.varType == VARIANT_TYPE.RUL_CHARPTR)
                || (((var.varType == VARIANT_TYPE.RUL_DD_STRING) || (var.varType == VARIANT_TYPE.RUL_WIDECHARPTR)) && var1.varType == VARIANT_TYPE.RUL_CHARPTR))
            {// narrow to wide conversion
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            //Added By Stevev 20dec07 --starts here
            else if (((var1.varType == VARIANT_TYPE.RUL_DD_STRING) || (var1.varType == VARIANT_TYPE.RUL_WIDECHARPTR))
                    && ((var.varType == VARIANT_TYPE.RUL_WIDECHARPTR) || (var.varType == VARIANT_TYPE.RUL_DD_STRING)))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue - var.val.pszValue;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_CHARPTR) && (var.varType == VARIANT_TYPE.RUL_CHARPTR))
            {
                retValue.varType = VARIANT_TYPE.RUL_CHARPTR;
                retValue.val.pzcVal = var1.val.pzcVal - var.val.pzcVal;
            }
            else if ((var1.varType == VARIANT_TYPE.RUL_BYTE_STRING) && (var.varType == VARIANT_TYPE.RUL_BYTE_STRING))
            {
                retValue.varType = VARIANT_TYPE.RUL_BYTE_STRING;
                retValue.val.bString.bsLen = var1.val.bString.bsLen + var.val.bString.bsLen;
                retValue.val.bString.bs = new byte[retValue.val.bString.bsLen];

                for (int i = 0; i < var1.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i] = var1.val.bString.bs[i];
                }

                for (int i = 0; i < var.val.bString.bsLen; i++)
                {
                    retValue.val.bString.bs[i + var1.val.bString.bsLen] = var.val.bString.bs[i];
                }
            }
            // Walt EPM - 05sep08 - add
            else if ((var1.varType == VARIANT_TYPE.RUL_DD_STRING) && (var.varType == VARIANT_TYPE.RUL_SAFEARRAY))
            {
                retValue.varType = VARIANT_TYPE.RUL_DD_STRING;
                retValue.val.pszValue = var1.val.pszValue + Encoding.Default.GetString(var.val.prgsa.m_data.pvData);
            }
            */
            // Walt EPM - 05sep08 - end
            return retValue;

        }

        public static INTER_VARIANT operator ~(INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();

            switch (var.varType)
            {
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retValue.val.ulValue = ~var.val.ulValue;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retValue.val.ulValue = (UInt64)~var.val.lValue;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retValue.val.ulValue = (uint)~var.val.unValue;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retValue.val.ulValue = (uint)~var.val.nValue;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retValue.val.ulValue = (uint)~var.val.usValue;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retValue.val.ulValue = (uint)~var.val.sValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retValue.val.ulValue = (uint)~var.val.ucValue;
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retValue.val.ulValue = (uint)~var.val.cValue;
                    break;
                case VARIANT_TYPE.RUL_BOOL:
                    if (!var.val.bValue)
                    {
                        retValue.val.ulValue = 1;
                    }
                    else
                    {
                        retValue.val.ulValue = 0;
                    }

                    break;
            }

            return retValue;
        }

        public static INTER_VARIANT operator ==(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;

            if (var1.isNumeric() && temp.isNumeric())
            {
                if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
                {
                    retValue.val.bValue = (bool)(var.val.dValue == temp.val.dValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
                {
                    retValue.val.bValue = (bool)((float)var.val.fValue == (float)temp.val.fValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.ulValue == temp.val.ulValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.lValue == temp.val.lValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
                {
                    retValue.val.bValue = (bool)(var1.val.unValue == temp.val.unValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
                {
                    retValue.val.bValue = (bool)(var1.val.nValue == temp.val.nValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.usValue == temp.val.usValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.sValue == temp.val.sValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.ucValue == temp.val.ucValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.cValue == temp.val.cValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
                {
                    retValue.val.bValue = (bool)(var1.val.bValue == temp.val.bValue) ? true : false;
                }
            }
            else//todo string compares
            {
                // Walt EPM - 05sep08 - add
                string str1 = null;
                string str2 = null;
                switch (var1.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str1 = var1.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str1 = var1.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str1 = BitConverter.ToString(var1.val.prgsa.m_data.pvData);
                        break;
                    default:
                        break;
                }
                switch (var.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str2 = var.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str2 = var.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str2 = BitConverter.ToString(var.val.prgsa.m_data.pvData);
                        break;
                    default:
                        break;
                }
                if (str1 == str2)
                {
                    retValue.val.bValue = (bool)true;
                }
                else
                {
                    retValue.val.bValue = (bool)false;
                }
                // Walt EPM - 05sep08 - end
            }
            return retValue;
        }

        public static INTER_VARIANT operator !=(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;
            if (var1.isNumeric() && temp.isNumeric())
            {
                if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
                {
                    retValue.val.bValue = (bool)(var1.val.dValue != temp.val.dValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
                {
                    retValue.val.bValue = (bool)(var1.val.fValue != temp.val.fValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.ulValue != temp.val.ulValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.lValue != temp.val.lValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
                {
                    retValue.val.bValue = (bool)(var1.val.unValue != temp.val.unValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
                {
                    retValue.val.bValue = (bool)(var1.val.nValue != temp.val.nValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.usValue != temp.val.usValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.sValue != temp.val.sValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.ucValue != temp.val.ucValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.cValue != temp.val.cValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
                {
                    retValue.val.bValue = (bool)((bool)var1.val.bValue != (bool)temp.val.bValue) ? true : false;
                }
            }
            // Walt EPM - 05sep08 - add
            else //string compares
            {
                string str1 = null;
                string str2 = null;
                switch (var1.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str1 = var1.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str1 = var1.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str1 = BitConverter.ToString(var1.val.prgsa.m_data.pvData);
                        break;
                }
                switch (var.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str2 = var.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str2 = var.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str2 = BitConverter.ToString(var.val.prgsa.m_data.pvData);
                        break;
                }
                if (str1 == str2)
                {
                    retValue.val.bValue = (bool)false;
                }
                else
                {
                    retValue.val.bValue = (bool)true;
                }
            }

            // Walt EPM - 05sep08 - end
            return retValue;
        }

        public static INTER_VARIANT operator >=(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;
            if (var1.isNumeric() && temp.isNumeric())
            {
                if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
                {
                    retValue.val.bValue = (bool)(var1.val.dValue >= temp.val.dValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
                {
                    retValue.val.bValue = (bool)(var1.val.fValue >= temp.val.fValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.ulValue >= temp.val.ulValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.lValue >= temp.val.lValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
                {
                    retValue.val.bValue = (bool)(var1.val.unValue >= temp.val.unValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
                {
                    retValue.val.bValue = (bool)(var1.val.nValue >= temp.val.nValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.usValue >= temp.val.usValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.sValue >= temp.val.sValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.ucValue >= temp.val.ucValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.cValue >= temp.val.cValue) ? true : false;
                }
                /*
                else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
                {
                    retValue.val.bValue = (bool)((bool)var1.val.bValue >= (bool)temp.val.bValue) ? true : false;
                }
                */
            }
            // Walt EPM - 05sep08 - add
            else //string compares
            {
                string str1 = null;
                string str2 = null;
                switch (var1.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str1 = var1.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str1 = var1.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str1 = BitConverter.ToString(var1.val.prgsa.m_data.pvData);
                        break;
                }
                switch (var.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str2 = var.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str2 = var.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str2 = BitConverter.ToString(var.val.prgsa.m_data.pvData);
                        break;
                }
                if (str1 == str2)
                {
                    retValue.val.bValue = (bool)true;
                }
                else
                {
                    retValue.val.bValue = (bool)false;
                }
            }

            // Walt EPM - 05sep08 - end
            return retValue;
        }

        public static INTER_VARIANT operator <=(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;
            if (var1.isNumeric() && temp.isNumeric())
            {
                if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
                {
                    retValue.val.bValue = (bool)(var1.val.dValue <= temp.val.dValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
                {
                    retValue.val.bValue = (bool)(var1.val.fValue <= temp.val.fValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.ulValue <= temp.val.ulValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.lValue <= temp.val.lValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
                {
                    retValue.val.bValue = (bool)(var1.val.unValue <= temp.val.unValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
                {
                    retValue.val.bValue = (bool)(var1.val.nValue <= temp.val.nValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.usValue <= temp.val.usValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.sValue <= temp.val.sValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.ucValue <= temp.val.ucValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.cValue <= temp.val.cValue) ? true : false;
                }
                /*
                else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
                {
                    retValue.val.bValue = (bool)((bool)var1.val.bValue <= (bool)temp.val.bValue) ? true : false;
                }
                */
            }
            // Walt EPM - 05sep08 - add
            else //string compares
            {
                string str1 = null;
                string str2 = null;
                switch (var1.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str1 = var1.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str1 = var1.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str1 = BitConverter.ToString(var1.val.prgsa.m_data.pvData);
                        break;
                }
                switch (var.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str2 = var.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str2 = var.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str2 = BitConverter.ToString(var.val.prgsa.m_data.pvData);
                        break;
                }
                if (str1 == str2)
                {
                    retValue.val.bValue = (bool)true;
                }
                else
                {
                    retValue.val.bValue = (bool)false;
                }
            }

            // Walt EPM - 05sep08 - end
            return retValue;
        }

        public static INTER_VARIANT operator >(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;
            if (var1.isNumeric() && temp.isNumeric())
            {
                if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
                {
                    retValue.val.bValue = (bool)(var1.val.dValue > temp.val.dValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
                {
                    retValue.val.bValue = (bool)(var1.val.fValue > temp.val.fValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.ulValue > temp.val.ulValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.lValue > temp.val.lValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
                {
                    retValue.val.bValue = (bool)(var1.val.unValue > temp.val.unValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
                {
                    retValue.val.bValue = (bool)(var1.val.nValue > temp.val.nValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.usValue > temp.val.usValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.sValue > temp.val.sValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.ucValue > temp.val.ucValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.cValue > temp.val.cValue) ? true : false;
                }
                /*
                else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
                {
                    retValue.val.bValue = (bool)((bool)var1.val.bValue > (bool)temp.val.bValue) ? true : false;
                }
                */
            }
            // Walt EPM - 05sep08 - add
            else //string compares
            {
                string str1 = null;
                string str2 = null;
                switch (var1.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str1 = var1.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str1 = var1.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str1 = BitConverter.ToString(var1.val.prgsa.m_data.pvData);
                        break;
                }
                switch (var.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str2 = var.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str2 = var.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str2 = BitConverter.ToString(var.val.prgsa.m_data.pvData);
                        break;
                }
                if (str1 == str2)
                {
                    retValue.val.bValue = (bool)true;
                }
                else
                {
                    retValue.val.bValue = (bool)false;
                }
            }

            // Walt EPM - 05sep08 - end
            return retValue;
        }

        public static INTER_VARIANT operator <(INTER_VARIANT var1, INTER_VARIANT var)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();
            INTER_VARIANT temp = var;
            if (var1.isNumeric() && temp.isNumeric())
            {
                if ((var1.varType == VARIANT_TYPE.RUL_DOUBLE) || (var.varType == VARIANT_TYPE.RUL_DOUBLE))
                {
                    retValue.val.bValue = (bool)(var1.val.dValue < temp.val.dValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_FLOAT) || (var.varType == VARIANT_TYPE.RUL_FLOAT))
                {
                    retValue.val.bValue = (bool)(var1.val.fValue < temp.val.fValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_ULONGLONG) || (var.varType == VARIANT_TYPE.RUL_ULONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.ulValue < temp.val.ulValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_LONGLONG) || (var.varType == VARIANT_TYPE.RUL_LONGLONG))
                {
                    retValue.val.bValue = (bool)(var1.val.lValue < temp.val.lValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UINT) || (var.varType == VARIANT_TYPE.RUL_UINT))
                {
                    retValue.val.bValue = (bool)(var1.val.unValue < temp.val.unValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_INT) || (var.varType == VARIANT_TYPE.RUL_INT))
                {
                    retValue.val.bValue = (bool)(var1.val.nValue < temp.val.nValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_USHORT) || (var.varType == VARIANT_TYPE.RUL_USHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.usValue < temp.val.usValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_SHORT) || (var.varType == VARIANT_TYPE.RUL_SHORT))
                {
                    retValue.val.bValue = (bool)(var1.val.sValue < temp.val.sValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR) || (var.varType == VARIANT_TYPE.RUL_UNSIGNED_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.ucValue < temp.val.ucValue) ? true : false;
                }
                else if ((var1.varType == VARIANT_TYPE.RUL_CHAR) || (var.varType == VARIANT_TYPE.RUL_CHAR))
                {
                    retValue.val.bValue = (bool)(var1.val.cValue < temp.val.cValue) ? true : false;
                }
                /*
                else if ((var1.varType == VARIANT_TYPE.RUL_BOOL) || (var.varType == VARIANT_TYPE.RUL_BOOL))
                {
                    retValue.val.bValue = (bool)((bool)var1.val.bValue < (bool)temp.val.bValue) ? true : false;
                }
                */
            }
            // Walt EPM - 05sep08 - add
            else //string compares
            {
                string str1 = null;
                string str2 = null;
                switch (var1.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str1 = var1.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str1 = var1.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str1 = BitConverter.ToString(var1.val.prgsa.m_data.pvData);
                        break;
                }
                switch (var.varType)
                {
                    case VARIANT_TYPE.RUL_CHARPTR:
                        str2 = var.val.pzcVal;
                        break;
                    case VARIANT_TYPE.RUL_WIDECHARPTR:
                    case VARIANT_TYPE.RUL_DD_STRING:
                        str2 = var.val.pszValue;
                        break;
                    case VARIANT_TYPE.RUL_SAFEARRAY:
                        str2 = BitConverter.ToString(var.val.prgsa.m_data.pvData);
                        break;
                }
                if (str1 == str2)
                {
                    retValue.val.bValue = (bool)true;
                }
                else
                {
                    retValue.val.bValue = (bool)false;
                }
            }

            // Walt EPM - 05sep08 - end
            return retValue;
        }

        public bool GetBoolVal()
        {
            bool retVal = false;
            switch (varType)
            {
                case VARIANT_TYPE.RUL_BOOL:
                    retVal = val.bValue;
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    retVal = (bool)(val.cValue != 0);
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    retVal = (bool)(val.ucValue != 0);
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    retVal = (bool)(val.sValue != 0);
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    retVal = (bool)(val.usValue != 0);
                    break;
                case VARIANT_TYPE.RUL_INT:
                    retVal = (bool)(val.nValue != 0);
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    retVal = (bool)(val.unValue != 0);
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    retVal = (bool)(val.lValue != 0);
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    retVal = (bool)(val.ulValue != 0);
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    retVal = (bool)(val.fValue != 0.0);
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    retVal = (bool)(val.dValue != 0.0);
                    break;
                default:
                    retVal = false;
                    break;
            }// end switch

            return retVal;
        }

        public static INTER_VARIANT operator !(INTER_VARIANT iv)
        {
            INTER_VARIANT retValue = new INTER_VARIANT();

            if (iv.isNumeric())
            {
                retValue.val.bValue = !iv.GetBoolVal();
            }

            return retValue;
        }

        public static int VariantSize(VARIANT_TYPE vt)
        {
            int r = 0;
            switch (vt)
            {
                case VARIANT_TYPE.RUL_NULL:
                    r = 0;
                    break;
                case VARIANT_TYPE.RUL_BOOL:
                    r = 1;
                    break;
                case VARIANT_TYPE.RUL_CHAR:
                    r = 1;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    r = 1;
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    r = 2;
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    r = 2;
                    break;
                case VARIANT_TYPE.RUL_INT:
                    r = 4;
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    r = 4;
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    r = 8;
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    r = 8;
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    r = 4;
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    r = 8;
                    break;
                case VARIANT_TYPE.RUL_CHARPTR:
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                case VARIANT_TYPE.RUL_DD_STRING:
                    r = 4;
                    break;
                case VARIANT_TYPE.RUL_BYTE_STRING:
                    r = 8;
                    break;
                case VARIANT_TYPE.RUL_SAFEARRAY:
                    r = 4;
                    break;
                default:
                    r = 0;
                    break;
            }// end switch

            return r;
        }

        public void GetStringValue(ref string pmem, VARIANT_TYPE vt = VARIANT_TYPE.RUL_CHARPTR)
        {
            if (pmem == null)
                return;

            if (varType == VARIANT_TYPE.RUL_SAFEARRAY)
            {
                INTER_VARIANT lIV = new INTER_VARIANT();
                int cnt = 0;

                INTER_SAFEARRAY pSA = GetSafeArray();
                if (pSA != null && (cnt = pSA.GetNumberOfElements()) > 0)
                {
                    if (pSA.Type() == VARIANT_TYPE.RUL_CHARPTR || pSA.Type() == VARIANT_TYPE.RUL_CHAR)// emerson checkin april2013
                    {
                        pmem = Encoding.Default.GetString(pSA.m_data.pvData);
                        /*
                        for (int i = 0; i < cnt; i++)
                        {
                            pSA.GetElement(i, ref lIV);
                            pmem += lIV;
                            lIV.Clear();
                        }
                        */
                    }
                    else if (pSA.Type() == VARIANT_TYPE.RUL_WIDECHARPTR || pSA.Type() == VARIANT_TYPE.RUL_DD_STRING)
                    {
                        /*
                        for (int i = 0; i < cnt; i++)
                        {
                            pSA.GetElement(i, ref lIV);
                            pmem += lIV;
                            lIV.Clear();
                        }
                        */
                        pmem = Encoding.Default.GetString(pSA.m_data.pvData);
                    }
                    else if (pSA.Type() == VARIANT_TYPE.RUL_SAFEARRAY)
                    {
                        ;// multi-dimensional strings are not supported
                    }
                    //else - numeric conversion not supported
                }
            }

            if (varType == VARIANT_TYPE.RUL_CHARPTR)
            {
                if (val.pzcVal != null)
                {
                    pmem = val.pzcVal;
                }
            }

            if (varType == VARIANT_TYPE.RUL_WIDECHARPTR || varType == VARIANT_TYPE.RUL_DD_STRING)
            {
                if (val.pszValue != null)
                {
                    pmem = val.pszValue;
                }
            }
        }

        // stevev 05jun07 - used to detect if promotion is possible
        public bool isNumeric()
        {
            switch (GetVarType())
            {
                case VARIANT_TYPE.RUL_BOOL:
                case VARIANT_TYPE.RUL_CHAR:
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                case VARIANT_TYPE.RUL_SHORT:
                case VARIANT_TYPE.RUL_USHORT:
                case VARIANT_TYPE.RUL_INT:
                case VARIANT_TYPE.RUL_UINT:
                case VARIANT_TYPE.RUL_LONGLONG:
                case VARIANT_TYPE.RUL_ULONGLONG:
                case VARIANT_TYPE.RUL_FLOAT:
                case VARIANT_TYPE.RUL_DOUBLE:
                    return true;
                case VARIANT_TYPE.RUL_CHARPTR:
                case VARIANT_TYPE.RUL_WIDECHARPTR:
                case VARIANT_TYPE.RUL_DD_STRING:
                case VARIANT_TYPE.RUL_BYTE_STRING:
                case VARIANT_TYPE.RUL_SAFEARRAY:
                    return false;
                case VARIANT_TYPE.RUL_NULL:
                default:
                    /* throw an error */
                    return false;
            }
            //return false;
        }

        public VARIANT_TYPE GetVarType()
        {
            return varType;
        }

        public int narrowStr2number(ref __VAL retVal, ref VARIANT_TYPE retType, string pStr)
        {
            /*
            int nLen = 0;
            int nVal = 0;
            Int64 lVal = 0;
            UInt64 ulVal = 0;
            int i = 0;
            double fVal = 0;
            string pEnd = null;
            bool bIsFloat = false;
            bool bIsHex = false, bIsOctal = false, bIsNeg = false;
            int iBase = 10;

            if (pStr == null || pStr == "")
            {
                return -1; // failure
            }

            //************************* reworked by stevev 10oct05 *********************************
            if (nLen >= 2)
            {
                bIsNeg = (pStr[0] == '-');
                if (pStr[0] == '0')
                {// has to be octal || hex
                    if ((pStr[1] == 'x') || (pStr[1] == 'X'))
                    {
                        bIsHex = true;
                        iBase = 16;
                        sscanf(pStr, "%I64x", &ulVal);
                    }
                    else
                    if (pStr[1] >= '0' && pStr[1] < '8')
                    {// octal
                        bIsOctal = true;
                        iBase = 8;
                        sscanf(pStr, "%I64o", &ulVal);
                    }
                    else
                    {// float or decimal - actually an error....
                        for (i = 1; i < nLen; i++)// we'll try to recover via float
                        {
                            if (pStr[i] == '.' || pStr[i] == 'E' || pStr[i] == 'e')
                            {
                                bIsFloat = true;
                                break;
                            }
                        }
                        if (!bIsFloat)
                        {// if all still false then its decimal eg 0999 - actually an error
                         // throw error
                            return -2;
                        }
                    }//end else			
                }
                else
                {// starts with a non-zero [1-9+\-]
                 // Walt EPM - 17oct08- make '.025' work as well as '0.025'
                    for (i = 0; i < nLen; i++)// we'll try to recover via float/
                    {
                        if (pStr[i] == '.' || pStr[i] == 'E' || pStr[i] == 'e')
                        {
                            bIsFloat = true;
                            break;
                        }
                    }
                }// endelse a decimal/float
            }
            // else: length == 1, can't be octal or hex or float, must be decimal...process as such

            if (bIsFloat)
            {
                //retVal.fValue = (float)atof(pStr);
                retVal.dValue = strtod(pStr, &pEnd);
                //if ((retVal.dValue <= FLT_MAX && retVal.dValue >= FLT_MIN) || (retVal.dValue > (-FLT_MAX) && retVal.dValue < (-FLT_MIN)))
                {
                    fVal = retVal.dValue;
                    retVal.fValue = (float)fVal;
                    retType = VARIANT_TYPE.RUL_FLOAT;
                }
                else
                {
                    retType = VARIANT_TYPE.RUL_DOUBLE;
                }
            }
            else
            {// decimal is the only option left
             //		we just gotta figure out how big
                if ((!bIsHex) && (!bIsOctal))
                {// we haven't scanned it yet, do it now
                    iBase = 10;
                    if (bIsNeg)
                    {
                        sscanf(pStr, "%I64d", &lVal);
                    }
                    else// go ushort until proven otherwise
                    {
                        sscanf(pStr, "%I64d", &ulVal);
                    }
                }

                if (bIsNeg)
                {
                    retVal.lValue = lVal;
                    retType = RUL_LONGLONG;
                }
                else
                {// we are non-negative
                    if (ulVal <= _I64_MAX)//WHS EP June17-2008 dont constrain constants to short/char/int - default to natural size.
                    {
                        retVal.lValue = (Int64)ulVal;
                        retType = RUL_LONGLONG;
                    }
                    else

                    {
                        retVal.ulValue = ulVal;
                        retType = RUL_ULONGLONG;
                    }
                }
            }
            */
            return 0;
        }

        public static void StripLangCode(ref string szString, ref string szLangCode, ref bool bLangCodePresent)
        {
            //if (bLangCodePresent)
            {
                bLangCodePresent = false;
            }
            if (szString.Length > 3)// emerson checkin april2013
            {
                if ((szString[0] == ('|')) && (szString[3] == ('|')))
                {
                    //if (bLangCodePresent)
                    {
                        bLangCodePresent = true;
                    }
                    if (szLangCode != null)
                    {
                        szLangCode = szString.Substring(1, 2);
                    }
                    szString = szString.Substring(4);
                }
            }
        }

        public __VAL GetValue()
        {
            return val;
        }
    }

    public struct INTER_SAFEARRAYBOUND
    {
        public uint cElements;
    }

    public class INTER_SAFEARRAY_DATA
    {   /*
    Note: Please make sure that the new member added is initialized in ctor. 
	ctor added by emerson due to 2010 iterator issue(crash on clear())
	*/

        public ushort cDims;      // Count of dimensions in this array. 
        //public ushort fFeatures;  // Flags used by the SafeArray routines.
        public ushort cbElements; // Size of an element of the array. 
                                  //         Does not include size of pointed-to //data.
        public byte[] pvData;     // Void pointer to the data. 
        public VARIANT_TYPE varType;
        public List<INTER_SAFEARRAYBOUND> vecBounds;  // Vector can handle its own initialization.

        public INTER_SAFEARRAY_DATA()
        {
            vecBounds = new List<INTER_SAFEARRAYBOUND>();
        }
    }

    public class INTER_SAFEARRAY
    {
        public INTER_SAFEARRAY_DATA m_data;
        Int32 m_i32mem;
        //private string m_wcharPtr;//this is intended to avoid memory leaks when casting strings between narrow and wide.

        public INTER_SAFEARRAY()
        {
            m_data = new INTER_SAFEARRAY_DATA();
        }

        public INTER_SAFEARRAY(_BYTE_STRING bsValue)
        {
            m_data = new INTER_SAFEARRAY_DATA();
            //m_wcharPtr = null;
            m_i32mem = bsValue.bsLen;  // actual size
            m_data.cbElements = 1;
            m_data.pvData = bsValue.bs;// new byte[m_i32mem + 1];// store an extra null top be kind
            m_data.varType = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
            m_data.cDims = 1;
            m_data.pvData[m_i32mem] = 0;// just in case...
        }

        public Int32 Allocate()
        {
            Int32 i32Size = m_data.vecBounds.Count;
            if ((m_data.cDims > 0) && (i32Size > 0))
            {
                //Walt:EPM-24aug07
                if (m_data.pvData != null)//double check that we do not leak memory
                {
                    m_data.pvData = null;
                }

                Int32 i32mem = m_data.cbElements;
                for (int i = 0; i < i32Size; i++)
                {
                    i32mem *= (int)m_data.vecBounds[i].cElements;
                }

                m_data.pvData = new byte[i32mem];
                m_i32mem = i32mem;
            }
            return 1;
        }

        void makeEmpty() // makes all elements zero(preserves type & length)
        {
            Int32 i32Size = m_data.vecBounds.Count;
            if ((m_data.cDims > 0) && (i32Size > 0))
            {
                Int32 i32mem = 1;
                for (Int32 i = 0; i < i32Size; i++)
                {
                    i32mem *= (int)m_data.vecBounds[i].cElements;
                }
                i32mem *= m_data.cbElements;
            }
        }

        /*//////
        public void GetElement(int i32Idx, ref INTER_VARIANT pvar)// pvar must be a passed in varient to be filled
        {
            pvar.SetValue(m_data.pvData, (uint)i32Idx, m_data.varType);

        }
        */

        public ushort GetDims(ref List<int> pvecDims)
        {
            if (pvecDims != null)
            {
                int i32Size = m_data.vecBounds.Count;
                for (int i = 0; i < i32Size; i++)
                {
                    pvecDims.Add((int)m_data.vecBounds[i].cElements);
                }
            }
            return m_data.cDims;
        }

        public int XMLize(ref string szData)
        {
            INTER_VARIANT temp = new INTER_VARIANT();
            for (uint i = 0; i < m_i32mem; i += m_data.cbElements)
            {
                byte[] data = new byte[m_data.cbElements];
                for (int j = 0; j < m_data.cbElements; j++)
                {
                    data[j] = m_data.pvData[i + j];
                }

                temp.SetValue(data, 0, m_data.varType);
                temp.XMLize(ref szData);
            }
            return 0;
        }

        public void AddDim(INTER_SAFEARRAYBOUND prgsaBound)
        {
            m_data.cDims++;
            m_data.vecBounds.Add(prgsaBound);
        }

        void AddDim(int prgsaBound)
        {
            INTER_SAFEARRAYBOUND local = new INTER_SAFEARRAYBOUND();
            local.cElements = (uint)prgsaBound;
            AddDim(local);
        }

        public Int32 MemoryAllocated()
        {
            return m_i32mem;
        }

        public void SetAllocationParameters(VARIANT_TYPE vt, ushort cDims, INTER_SAFEARRAYBOUND prgsaBound)
        {
            m_data.cDims = cDims;
            m_data.cbElements = (ushort)INTER_VARIANT.VariantSize(vt);
            m_data.varType = vt;
            m_data.vecBounds.Add(prgsaBound);

        }
        void SetAllocationParameters(VARIANT_TYPE vt, ushort cDims, int prgsaBound)
        {
            INTER_SAFEARRAYBOUND local = new INTER_SAFEARRAYBOUND();
            local.cElements = (uint)prgsaBound;
            SetAllocationParameters(vt, cDims, local);
        }

        public int GetNumberOfElements()    // aka .size()
        {// stevev 31may07 - avoid divide by zero
            if (m_data.cbElements != 0)
            {
                return (m_i32mem / m_data.cbElements);
            }
            else
            {
                return 0;
            }
        }

        public int GetElementSize()
        {
            return (m_data.cbElements);
        }

        public void GetElement(uint i32Idx, ref INTER_VARIANT pvar)
        {// set return pvar value from array data                       
            pvar.SetValue(m_data.pvData, i32Idx, m_data.varType);
        }

        public void SetElement(uint i32Idx, INTER_VARIANT pvar)
        {
            pvar.GetValue(ref m_data.pvData, i32Idx, m_data.varType);
        }

        public VARIANT_TYPE Type()
        {
            return m_data.varType;
        }

    }
}
