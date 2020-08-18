using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public struct Function_Signatures
    {
        public string szWord;
        public RUL_TOKEN_TYPE functionReturnType;
        public int iNumberOfParameters;
        public RUL_TOKEN_TYPE[] piParameterType;// = new RUL_TOKEN_TYPE[FunctionExpression.MAX_NUMBER_OF_FUNCTION_PARAMETERS];
        public Function_Signatures(string str, RUL_TOKEN_TYPE type, int num, RUL_TOKEN_TYPE[] pi)
        {
            szWord = str;
            functionReturnType = type;
            iNumberOfParameters = num;
            piParameterType = pi;
        }
    }

    public class FunctionExpression : CExpression
    {
        public const int MAX_NUMBER_OF_FUNCTION_PARAMETERS = 10;
        string m_pchFunctionName;
        int m_i32ParameterCount;
        new CExpression[] m_pExpression;
        //new CExpression[] m_pExpression;
        RUL_TOKEN_TYPE[] m_pTokenType;
        CToken[] m_pConstantTokens;

        Function_Signatures[] functionsDefs =
            {
                new Function_Signatures("delay", RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT }),

                new Function_Signatures("DELAY",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("DELAY_TIME",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("abort",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("process_abort",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("_add_abort_method",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_remove_abort_method",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("remove_all_abort",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                /*Arun 190505 Start of code */
                new Function_Signatures("_push_abort_method",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),/*stevev4waltS - 11oct07*/

                new Function_Signatures("pop_abort_method",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                /*End of code */
                /*Arun 200505 Start of code*/
                new Function_Signatures("BUILD_MESSAGE",  RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT }), // modified 30may07 per WS:EPM
                /*End of code*/

                new Function_Signatures("remove_all_abort_methods",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("PUT_MESSAGE",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("put_message",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("ACKNOWLEDGE",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("acknowledge",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_get_dev_var_value",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_get_local_var_value",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("_display_xmtr_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("display_response_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("display",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("SELECT_FROM_LIST",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("select_from_list",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT,RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("_vassign",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_dassign",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_fassign",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_iassign",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_lassign",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_fvar_value",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_ivar_value",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_lvar_value",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("save_values",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                //Added By Anil July 01 2005 --starts here
                new Function_Signatures("discard_on_exit",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                //Added By Anil July 01 2005 --Ends here
                //Added By Anil June 20 2005 --starts here
                new Function_Signatures("svar_value",  RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("sassign",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),	

                //Added By Anil June 20 2005 --Ends here

                new Function_Signatures("get_more_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("_get_status_code_string",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 4,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                /*Arun 200505 Start of code*/
                //WHS 2007 get_enum_string takes 3 arguments not 4.
                // stevev - made it '_get_enum..' from 'get_enum..' 25jul07-sjv
                new Function_Signatures("_get_enum_string",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),
                /*End of code*/

                new Function_Signatures("_get_dictionary_string",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                //Anil 22 December 2005 for dictionary_string built in
                new Function_Signatures("_dictionary_string",  RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                //stevev 29dec08
                new Function_Signatures("literal_string",  RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("resolve_param_ref",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("resolve_array_ref",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("resolve_record_ref",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("resolve_local_ref",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_DD_ITEM}),/* stevev 19feb09 - try DD_Item */

                new Function_Signatures("rspcode_string",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 4,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_comm_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_device_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_resp_code",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_all_resp_code",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_no_device",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("SET_NUMBER_OF_RETRIES",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_comm_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_device_status",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_resp_code",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_all_resp_code",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_no_device",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_all_data",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_set_xmtr_data",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("NaN_value",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("fpclassify",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("nanf",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("nan",   RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT}), /*stevev added 25jun07*/

                new Function_Signatures("isetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("fsetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("lsetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("dsetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("igetvalue",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("igetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("fgetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("lgetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                new Function_Signatures("dgetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),

                /*Arun 200505 Start of code*/
                new Function_Signatures("sgetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT,RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("ssetval",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),
                /*End of code*/

                new Function_Signatures("ext_send_command_trans",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 5,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("ext_send_command",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 4,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("tsend_command_trans",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("tsend_command",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("send_command_trans",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("send_command",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("send_trans",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("send",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                /*Arun 110505 Start of code*/
                /*********************Math Builtins (eDDL) ***********************/
                new Function_Signatures("abs",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("acos",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("asin",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("atan",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("cbrt",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("ceil",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("cos",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("cosh",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("exp",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("floor",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("fmod",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("frand",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("log",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("log10",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("log2",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("pow",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("round",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("sin",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("sinh",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("sqrt",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("tan",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("tanh",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("trunc",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("atof",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("atoi",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("itoa",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),
                /*********************End of Math Builtins (eDDL)******************/
                /* End of code*/

                /* Arun 160505 Start of code*/
                /*********************Date Time Builtins(eDDL)********************/
                //	new Function_Signatures("YearMonthDay_to_Date",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,  
                //new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),/* not in spec */-WS:EPM removed-25jun07
                new Function_Signatures("Date_to_Year",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("Date_to_Month",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("Date_to_DayOfMonth",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                //	new Function_Signatures("GetCurrentDate",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),							/* not in spec */-WS:EPM removed-25jun07
                new Function_Signatures("GetCurrentTime",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),
                //	new Function_Signatures("GetCurrentDateAndTime",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 0, null),						/* not in spec */-WS:EPM removed-25jun07
                //	new Function_Signatures("To_Date_and_Time",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 5,  
                //new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),	/* not in spec */-WS:EPM removed-25jun07
                //Anil added these Time related function 28t November
                new Function_Signatures("DiffTime",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("AddTime",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("Make_Time",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 7,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("To_Time",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 5,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("Date_To_Time",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("To_Date",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("Time_To_Date",  RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                // added 16jul14 -----------------------------------------------------
                new Function_Signatures("DATE_to_days",                 RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("days_to_DATE",                 RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("From_DATE_AND_TIME_VALUE",     RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("From_TIME_VALUE",          RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("TIME_VALUE_to_seconds",        RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("TIME_VALUE_to_Hour",       RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("TIME_VALUE_to_Minute",         RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("TIME_VALUE_to_Second",         RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("seconds_to_TIME_VALUE",        RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("DATE_AND_TIME_VALUE_to_string",    RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 4,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("DATE_to_string",           RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("TIME_VALUE_to_string",         RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("timet_to_string",          RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_STR_CONSTANT, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("timet_To_TIME_VALUE",      RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                new Function_Signatures("To_TIME_VALUE",                RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),

                /*********************End of Date Time Builtins (eDDL) **************/

                //Added By Anil June 17 2005 --starts here
                /****************************Start of DD_STRING  Builtins  (eDDL) ********************/

                new Function_Signatures("strstr",   RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 2,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT,RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),//Anil changed to lower case 29th November 2005

                new Function_Signatures("strupr",   RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),//Anil changed to lower case 29th November 2005

                new Function_Signatures("strlwr",   RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),//Anil changed to lower case 29th November 2005

                new Function_Signatures("strlen",   RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),//Anil changed to lower case 29th November 2005

                new Function_Signatures("strcmp",   RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT,RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),//Anil changed to lower case 29th November 2005

                new Function_Signatures("strtrim",  RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),//Anil changed to lower case 29th November 2005

                new Function_Signatures("strmid",   RUL_TOKEN_TYPE.RUL_STR_CONSTANT, 3,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE,RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE}),//Anil changed to lower case 29th November 2005

                /****************************End of DD_STRING  Builtins (eDDL) ********************/
                //Added By Anil June 17 2005 --Ends here

                /*End of code*/

                /*Vibhor 200905: Start of Code*/
                /****************************Start List Manipulation Builtins (eDDL) ********************/

                new Function_Signatures("_ListInsert",          RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT,RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT,RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_ListDeleteElementAt",   RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT,RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                /****************************End List Manipulation Builtins (eDDL) ********************/

                /*Vibhor 200905: End of Code*/
                //Anil September 26 2005 added Menudisplay
                new Function_Signatures("_MenuDisplay", RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT,RUL_TOKEN_TYPE.RUL_STR_CONSTANT,RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                /* stevev 18feb09 - add transfer builtins */
                new Function_Signatures("openTransferPort"  , RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("closeTransferPort" , RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("abortTransferPort" , RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_writeItemToDevice"    , RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("_readItemFromDevice"   , RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 2,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                new Function_Signatures("get_transfer_status"   , RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 3,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT, RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT}),

                /* end stevev transfer builtins */
                /* add debug builtins 16jul14 */
                new Function_Signatures("_ERROR",               RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("_TRACE",               RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT}),

                new Function_Signatures("_WARNING",             RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE, 1,
                new RUL_TOKEN_TYPE[]{ RUL_TOKEN_TYPE.RUL_STR_CONSTANT})
            };

        public FunctionExpression()
        {
            m_pExpression = new CExpression[MAX_NUMBER_OF_FUNCTION_PARAMETERS];
            m_pTokenType = new RUL_TOKEN_TYPE[MAX_NUMBER_OF_FUNCTION_PARAMETERS];
            m_pConstantTokens = new CToken[MAX_NUMBER_OF_FUNCTION_PARAMETERS];
        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += m_pchFunctionName;
            szData += ">";

            szData += "</";
            szData += m_pchFunctionName;
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable, 
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitFunctionExpression(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                //Munch a <FUNCTION NAME>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || !pToken.IsFunctionToken())
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }

                m_pchFunctionName = pToken.GetLexeme();

                /* Now get the details of the function */
                Function_Signatures Func = new Function_Signatures();
                if (GetFunctionDetails(pToken, ref Func) == 0)
                {
                    return 0;
                }

                //Munch a <(>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType()) || RUL_TOKEN_SUBTYPE.RUL_LPAREN != pToken.GetSubType())
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGLPAREN);
                    plexAnal.UnGetToken();
                }

                /* Now get the parameters */
                m_i32ParameterCount = Func.iNumberOfParameters;

                for (int iLoopVar = 0; iLoopVar < (int)Func.iNumberOfParameters; iLoopVar++)  // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
                {
                    m_pTokenType[iLoopVar] = Func.piParameterType[iLoopVar];
                    switch (Func.piParameterType[iLoopVar])
                    {
                        case RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT:
                        case RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE:
                        case RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE:
                        case RUL_TOKEN_TYPE.RUL_DD_ITEM:           //Vibhor 140705: Added
                            {
                                bool bParenPresent = false;
                                /* Check if there is a <(>*/
                                /*if((LEX_FAIL != plexAnal.GetNextToken(&pToken,pSymbolTable)) 
                                    && pToken
                                    && RUL_SYMBOL == pToken.GetType()
                                    && RUL_LPAREN == pToken.GetSubType())
                                {
                                    bParenPresent = true;
                                }
                                else
                                {
                                    plexAnal.UnGetToken();
                                }
                                DELETE_PTR(pToken);*/

                                CExpParser expParser = new CExpParser();
                                m_pExpression[iLoopVar] = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_WHILE);
                                if (m_pExpression[iLoopVar] == null)
                                {
                                    //ADD_ERROR(C_WHILE_ERROR_MISSINGEXP);
                                }

                                /*if <(> was present, check for <)>*/
                                if (bParenPresent)
                                {
                                    /* Check if there is a <(>*/
                                    if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                                        || pToken == null || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType()) || RUL_TOKEN_SUBTYPE.RUL_RPAREN != pToken.GetSubType())
                                    {
                                        plexAnal.UnGetToken();
                                    }
                                }

                                break;
                            }
                        case RUL_TOKEN_TYPE.RUL_STR_CONSTANT:
                            {
                                bool bParenPresent = false;
                                bool bEnterWhile = true;
                                int iCountLeftParenthisis = 0;
                                /* Check if there is a <(>*/
                                //Anil 16 November 2005
                                //This is for handling the Multiplle left parathissi come oin the Built in calls
                                while (bEnterWhile)
                                {
                                    if ((CLexicalAnalyzer.LEX_FAIL != plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                                        && pToken != null
                                        && RUL_TOKEN_TYPE.RUL_SYMBOL == pToken.GetType()
                                        && RUL_TOKEN_SUBTYPE.RUL_LPAREN == pToken.GetSubType())
                                    {
                                        bParenPresent = true;
                                        iCountLeftParenthisis++;
                                        bEnterWhile = true;
                                    }
                                    else
                                    {
                                        plexAnal.UnGetToken();
                                        bEnterWhile = false;
                                    }
                                }

                                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                                    || pToken != null || !pToken.IsConstant())
                                {
                                    if (pToken.IsVariable())
                                    {
                                        plexAnal.UnGetToken();
                                        m_pTokenType[iLoopVar] = RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE;
                                        CExpParser expParser = new CExpParser();
                                        m_pExpression[iLoopVar] = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_WHILE);
                                        if (m_pExpression[iLoopVar] == null)
                                        {
                                            //ADD_ERROR(C_WHILE_ERROR_MISSINGEXP);
                                        }
                                    }
                                    // stevev 30jan08 - added to handle function-as-string
                                    else if (pToken.IsFunctionToken())
                                    {
                                        plexAnal.UnGetToken();
                                        m_pTokenType[iLoopVar] = RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE;// may need RUL_STR_CONSTANT
                                        CExpParser expParser = new CExpParser();
                                        m_pExpression[iLoopVar] = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_WHILE);
                                        if (m_pExpression[iLoopVar] != null)
                                        {
                                            //ADD_ERROR(C_WHILE_ERROR_MISSINGEXP);
                                        }
                                    }
                                    // else - we don't handle other possibilities...
                                }
                                else
                                {
                                    //m_pConstantTokens[iLoopVar] = new CToken;
                                    m_pConstantTokens[iLoopVar] = pToken;
                                }

                                /*if <(> was present, check for <)>*/
                                if (bParenPresent)
                                {
                                    /* Check if there is a <(>*/
                                    //Anil 16 November 2005						
                                    //This is for handling the Multiplle left parathissi come oin the Built in calls					
                                    for (int iCount = 0; iCount < iCountLeftParenthisis; iCount++)
                                    {
                                        if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                                            || pToken == null
                                            || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType())
                                            || RUL_TOKEN_SUBTYPE.RUL_RPAREN != pToken.GetSubType())
                                        {
                                            plexAnal.UnGetToken();
                                        }
                                    }
                                }
                                break;
                            }
                        default:
                            return 0;
                    }
                    //Munch a <,>
                    if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                        || pToken == null
                        || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType())
                        || RUL_TOKEN_SUBTYPE.RUL_COMMA != pToken.GetSubType())
                    {
                        //ADD_ERROR(C_RS_ERROR_MISSINGLPAREN);
                        plexAnal.UnGetToken();
                    }
                }// next function parameter

                //Munch a <)>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null
                    || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType())
                    || RUL_TOKEN_SUBTYPE.RUL_RPAREN != pToken.GetSubType())
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGLPAREN);
                    plexAnal.UnGetToken();
                }

                /*		if((LEX_FAIL == plexAnal.GetNextToken(&pToken,pSymbolTable)) 
                            || !pToken
                            || (RUL_SYMBOL != pToken.GetType())
                            || RUL_SEMICOLON != pToken.GetSubType())
                        {
                            //ADD_ERROR(C_RS_ERROR_MISSINGLPAREN);
                            plexAnal.UnGetToken();
                        }
                        DELETE_PTR(pToken);*/

                return 1;

            }
            }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return -1;
        }

        public string GetFunctionName()
        {
            return m_pchFunctionName;
        }

        public int GetParameterCount()
        {
            return m_i32ParameterCount;
        }

        public CExpression GetExpParameter(int iParameterIndex)
        {
            return m_pExpression[iParameterIndex];
        }

        public RUL_TOKEN_TYPE GetParameterType(int iParameterIndex)
        {
            return m_pTokenType[iParameterIndex];
        }

        public CToken GetConstantParameter(int iParameterIndex)
        {
            return m_pConstantTokens[iParameterIndex];
        }

        public int GetFunctionDetails(CToken pToken, ref Function_Signatures pFunc)
        {
            int iNumberOfFunctions = functionsDefs.Length;

            for (int iLoopVar = 0; iLoopVar < iNumberOfFunctions; iLoopVar++)
            {
                if (pToken.GetLexeme() == functionsDefs[iLoopVar].szWord)
                {
                    pFunc = functionsDefs[iLoopVar];
                    return 1;
                }
            }
            return 0;
        }


    }
}
