using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public enum PRODUCTION
    {
        EXPRESSION = 0,
        DECLARATION = 1,
        ASSIGNMENT = 2,
        SELECTION = 3,
        ITERATION = 4,
        COMPOUND_STMT = 5,
        STMT_LIST = 6
    }

    public enum DD_ITEM_TYPE
    {
        DD_ITEM_VAR = 0,
        DD_ITEM_NONVAR = 1,
        DD_ITEM_METHOD = 2
    }

    public struct DFA_State
    {
        public string szWord;
        public RUL_TOKEN_TYPE Type;
        public RUL_TOKEN_SUBTYPE SubType;

        public DFA_State(string s, RUL_TOKEN_TYPE t, RUL_TOKEN_SUBTYPE st)
        {
            szWord = s;
            Type = t;
            SubType = st;
        }
    }

    public class FOLLOW_ELEMENT
    {
        public RUL_TOKEN_TYPE Type;
        public RUL_TOKEN_SUBTYPE SubType;

        public FOLLOW_ELEMENT(RUL_TOKEN_TYPE rhsType, RUL_TOKEN_SUBTYPE rhsSubType)
        {
            Type = rhsType;
            SubType = rhsSubType;
        }
    }

    public class FOLLOWS
    {
        public PRODUCTION production;
        public List<FOLLOW_ELEMENT> set;

        public FOLLOWS()
        {
            set = new List<FOLLOW_ELEMENT>();
        }
    }

    public class FOLLOW_SET
    {
        public const int PRODUCTION_COUNT = 7;
        protected FOLLOWS[] follows = new FOLLOWS[PRODUCTION_COUNT];

        public FOLLOW_SET()
        {
            for (int i = 0; i < PRODUCTION_COUNT; i++)
            {
                follows[i] = new FOLLOWS();
            }

            follows[(int)PRODUCTION.EXPRESSION].production = PRODUCTION.EXPRESSION;
            follows[(int)PRODUCTION.EXPRESSION].set.Add(new FOLLOW_ELEMENT(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_SEMICOLON));
            follows[(int)PRODUCTION.EXPRESSION].set.Add(new FOLLOW_ELEMENT(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_RPAREN));
            follows[(int)PRODUCTION.EXPRESSION].set.Add(new FOLLOW_ELEMENT(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_LBRACK));
            follows[(int)PRODUCTION.EXPRESSION].set.Add(new FOLLOW_ELEMENT(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_RBRACK));

            follows[(int)PRODUCTION.EXPRESSION].production = PRODUCTION.DECLARATION;
            follows[(int)PRODUCTION.EXPRESSION].set.Add(new FOLLOW_ELEMENT(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_COMMA));
            follows[(int)PRODUCTION.EXPRESSION].set.Add(new FOLLOW_ELEMENT(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_SEMICOLON));
        }

        private FOLLOW_ELEMENT FOLLOW_ELEMENT(RUL_TOKEN_TYPE rUL_SYMBOL, RUL_TOKEN_SUBTYPE rUL_SEMICOLON)
        {
            throw new NotImplementedException();
        }

        public bool IsPresent(PRODUCTION production, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType)
        {
            for (int i = 0; i < PRODUCTION_COUNT; i++)
            {
                if (follows[i].production == production)
                {
                    int i32SetSize = follows[i].set.Count;
                    for (int j = 0; j < i32SetSize; j++)
                    {
                        if ((follows[i].set[j].Type == Type) && (follows[i].set[j].SubType == SubType))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    public class CLexicalAnalyzer
    {
        DFA_State[] State;

        DFA_State[] OM_Service;

        public const int LEX_SUCCESS = 1;
        public const int LEX_FAIL = 0;
        public const int C_UM_ERROR_UNKNOWNERROR = 0x00000001;
        public const int C_UM_ERROR_LOWMEMORY = 0x00000002;
        public const int C_UM_ERROR_INTERNALERR = 0x00000003;
        public const int BUFFER_SIZE = 1000;

        public const char LCUR = '{';
        public const char RCUR = '}';
        public const char LBOX = '[';

        Exception ep = new Exception();

        //	Data Members...
        int m_i32CurrentPos;
        int m_i32PrevPos;
        //int m_i32LAPosition;
        int m_i32LineNo;
        int m_i32CurLineNo;
        int m_i32PrevLineNo;
        int m_nPrevSymbolTableScopeIndex;//stevev 25apr13
        int m_nSymbolTableScopeIndex; //SCR26200 Felix
        int m_nLastSymbolTableScopeIndex; //SCR26200 Felix for handling Nested Depth of Symbol

        string m_pszSource;/////
        //	_UCHAR			m_szRuleName[RULENAMELEN];

        CToken m_PrevToken;
        CToken m_CurToken;
        //ERROR_VEC m_pvecErr;
        /*Vibhor 010705: Start of Code*/
        // MEE member pointer : Path to Global (DD) Data 
        MEE m_pMEE;
        static FOLLOW_SET g_follow_set;

        public CLexicalAnalyzer()
        {
            m_i32CurrentPos = 0;
            m_i32PrevPos = 0;
            //m_i32LAPosition = 0;
            m_i32LineNo = 1;
            //m_pvecErr = 0;
            m_i32CurLineNo = 1;
            m_i32PrevLineNo = 1;
            m_pMEE = null;
            m_nSymbolTableScopeIndex = 0;//SCR26200 Felix
            m_nLastSymbolTableScopeIndex = 0;
            m_nPrevSymbolTableScopeIndex = 0;
            g_follow_set = new FOLLOW_SET();
            m_CurToken = new CToken();
            m_PrevToken = new CToken();
            initstate();
        }

        public bool InitMeeInterface(MEE pMEE)
        {
            if (pMEE != null)
            {
                m_pMEE = pMEE;
                return true;
            }
            return false;
        }

        public MEE GetMEEInterface()
        {
            if (m_pMEE != null)
                return m_pMEE;
            else
                return null;
        }

        public void initstate()
        {
            OM_Service = new DFA_State[1];
            OM_Service[0] = new DFA_State("Service", RUL_TOKEN_TYPE.RUL_SERVICE, RUL_TOKEN_SUBTYPE.RUL_SERVICE_ATTRIBUTE);

            State = new DFA_State[223];

            State[0] = new DFA_State("//", RUL_TOKEN_TYPE.RUL_COMMENT, RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE);
            State[1] = new DFA_State("+=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN);
            State[2] = new DFA_State("-=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN);
            State[3] = new DFA_State("/=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN);
            State[4] = new DFA_State("%=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN);
            State[5] = new DFA_State("*=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN);
            State[6] = new DFA_State("!=", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_NOT_EQ);
            State[7] = new DFA_State("&=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN);
            State[8] = new DFA_State("|=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN);
            State[9] = new DFA_State("^=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN);
            State[10] = new DFA_State(">>=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN);
            State[11] = new DFA_State("<<=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN);
            State[12] = new DFA_State("++", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_PLUS_PLUS);
            State[13] = new DFA_State("--", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MINUS_MINUS);
            State[14] = new DFA_State("+", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_PLUS);
            State[15] = new DFA_State("-", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MINUS);
            State[16] = new DFA_State("*", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MUL);
            State[17] = new DFA_State("/", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_DIV);
            State[18] = new DFA_State("%", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MOD);
            State[19] = new DFA_State("&&", RUL_TOKEN_TYPE.RUL_LOGICAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_LOGIC_AND);
            State[20] = new DFA_State("||", RUL_TOKEN_TYPE.RUL_LOGICAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_LOGIC_OR);
            State[21] = new DFA_State("!", RUL_TOKEN_TYPE.RUL_LOGICAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_LOGIC_NOT);
            State[22] = new DFA_State("&", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_AND);
            State[23] = new DFA_State("|", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_OR);
            State[24] = new DFA_State("^", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_XOR);
            State[25] = new DFA_State("~", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_NOT);
            State[26] = new DFA_State(">>", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT);
            State[27] = new DFA_State("<<", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT);
            //							State[0] = new DFA_State("!", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_NOT);
            State[28] = new DFA_State("<=", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_LE);
            State[29] = new DFA_State(">=", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_GE);
            //							State[0] = new DFA_State("**", RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_EXP);
            //							State[0] = new DFA_State("<>", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_NOT_EQ);
            State[30] = new DFA_State("<", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_LT);
            State[31] = new DFA_State(">", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_GT);
            State[32] = new DFA_State("==", RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_EQ);
            State[33] = new DFA_State("=", RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_ASSIGN);
            State[34] = new DFA_State("{", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_LBRACK);
            State[35] = new DFA_State("}", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_RBRACK);
            State[36] = new DFA_State("(", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_LPAREN);
            State[37] = new DFA_State(")", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_RPAREN);
            State[38] = new DFA_State("[", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_LBOX);
            State[39] = new DFA_State("]", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_RBOX);
            State[40] = new DFA_State(";", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_SEMICOLON);
            State[41] = new DFA_State(":", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_COLON);
            State[42] = new DFA_State(",", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_COMMA);
            State[43] = new DFA_State(".", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_DOT);
            State[44] = new DFA_State("?", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_QMARK);
            //							State[0] = new DFA_State("::", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_SCOPE);
            State[45] = new DFA_State("if", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_IF);
            State[46] = new DFA_State("else", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_ELSE);
            State[47] = new DFA_State("switch", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_SWITCH);
            State[48] = new DFA_State("case", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_CASE);
            State[49] = new DFA_State("default", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_DEFAULT);
            State[50] = new DFA_State("while", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_WHILE);
            State[51] = new DFA_State("for", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FOR);
            State[52] = new DFA_State("do", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_DO);
            State[53] = new DFA_State("unsigned int", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL);
            State[54] = new DFA_State("int", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL);
            State[55] = new DFA_State("unsigned long long", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL);
            State[56] = new DFA_State("long long", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL);
            State[57] = new DFA_State("unsigned long", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL);
            State[58] = new DFA_State("long", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_LONG_DECL);
            State[59] = new DFA_State("unsigned short", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL);
            State[60] = new DFA_State("short", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL);
            State[61] = new DFA_State("float", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_REAL_DECL);
            State[62] = new DFA_State("double", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL);
            State[63] = new DFA_State("char", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL);
            State[64] = new DFA_State("break", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_BREAK);
            State[65] = new DFA_State("continue", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_CONTINUE);
            State[66] = new DFA_State("return", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_RETURN);
            //Added By Anil June 14 2005 --starts here
            State[67] = new DFA_State("DD_STRING", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL);
            //Added By Anil June 14 2005 --Ends here
            State[68] = new DFA_State("unsigned char", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL);
            //							State[0] = new DFA_State("bool", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL);
            //							State[0] = new DFA_State("true", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_BOOL_CONSTANT);
            //							State[0] = new DFA_State("false", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_BOOL_CONSTANT);
            //							State[0] = new DFA_State("string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_STRING_DECL);
            //							State[0] = new DFA_State("Invoke", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_INVOKE);
            //							{"RuleEngine",RUL_KEYWORD,RUL_RULE_ENGINE}//,
            //							{"ObjectManager",RUL_KEYWORD,RUL_OM}
            /* Start of Function signatures */
            State[69] = new DFA_State("DELAY_TIME", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[70] = new DFA_State("DELAY", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[71] = new DFA_State("delay", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[72] = new DFA_State("process_abort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[73] = new DFA_State("_add_abort_method", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[74] = new DFA_State("_remove_abort_method", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[75] = new DFA_State("remove_all_abort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[76] = new DFA_State("abort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*Arun 190505 Start of code*/
            State[77] = new DFA_State("_push_abort_method", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);/*stevev4waltS - 11oct07*/
            State[78] = new DFA_State("pop_abort_method", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*End of code*/
            State[79] = new DFA_State("remove_all_abort_methods", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*Arun 200505 Start of code*/
            State[80] = new DFA_State("BUILD_MESSAGE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*End of code*/
            State[81] = new DFA_State("PUT_MESSAGE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[82] = new DFA_State("put_message", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[83] = new DFA_State("ACKNOWLEDGE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[84] = new DFA_State("acknowledge", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[85] = new DFA_State("_get_dev_var_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[86] = new DFA_State("_get_local_var_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[87] = new DFA_State("_display_xmtr_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[88] = new DFA_State("display_response_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[89] = new DFA_State("display", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[90] = new DFA_State("SELECT_FROM_LIST", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[91] = new DFA_State("select_from_list", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[92] = new DFA_State("_vassign", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[93] = new DFA_State("_dassign", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[94] = new DFA_State("_fassign", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[95] = new DFA_State("_iassign", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[96] = new DFA_State("_lassign", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[97] = new DFA_State("_fvar_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[98] = new DFA_State("_ivar_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[99] = new DFA_State("_lvar_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[100] = new DFA_State("save_values", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //Added By Anil July 01 2005 --starts here
            State[101] = new DFA_State("discard_on_exit", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //Added By Anil July 01 2005 --Ends here
            //Added By Anil June 20 2005 --starts here
            State[102] = new DFA_State("svar_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[103] = new DFA_State("sassign", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //Added By Anil June 20 2005 --Ends here
            State[104] = new DFA_State("get_more_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[105] = new DFA_State("_get_status_code_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*Arun 200505 Start of code*/
            // stevev - made it '_get_enum..' from 'get_enum..' 25jul07-sjv
            State[106] = new DFA_State("_get_enum_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*End of code*/
            State[107] = new DFA_State("_get_dictionary_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //Anil 22 December 2005 for dictionary_string built in
            State[108] = new DFA_State("_dictionary_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //stevev 29jan08
            State[109] = new DFA_State("literal_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[110] = new DFA_State("resolve_param_ref", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[111] = new DFA_State("resolve_array_ref", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[112] = new DFA_State("resolve_record_ref", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[113] = new DFA_State("resolve_local_ref", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[114] = new DFA_State("rspcode_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[115] = new DFA_State("_set_comm_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[116] = new DFA_State("_set_device_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[117] = new DFA_State("_set_resp_code", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[118] = new DFA_State("_set_all_resp_code", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[119] = new DFA_State("_set_no_device", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[120] = new DFA_State("SET_NUMBER_OF_RETRIES", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[121] = new DFA_State("_set_xmtr_comm_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[122] = new DFA_State("_set_xmtr_device_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[123] = new DFA_State("_set_xmtr_resp_code", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[124] = new DFA_State("_set_xmtr_all_resp_code", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[125] = new DFA_State("_set_xmtr_no_device", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[126] = new DFA_State("_set_xmtr_all_data", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[127] = new DFA_State("_set_xmtr_data", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[128] = new DFA_State("NaN_value", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[129] = new DFA_State("fpclassify", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[130] = new DFA_State("nanf", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[131] = new DFA_State("nan", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);/*stevev added 25jun07*/
            State[132] = new DFA_State("isetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[133] = new DFA_State("fsetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[134] = new DFA_State("lsetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[135] = new DFA_State("dsetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[136] = new DFA_State("igetvalue", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[137] = new DFA_State("igetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[138] = new DFA_State("fgetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[139] = new DFA_State("lgetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[140] = new DFA_State("dgetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*Arun 200505 Start of code*/
            State[141] = new DFA_State("sgetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[142] = new DFA_State("ssetval", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*End of code*/
            State[143] = new DFA_State("send_command_trans", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[144] = new DFA_State("send_command", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[145] = new DFA_State("ext_send_command_trans", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[146] = new DFA_State("ext_send_command", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[147] = new DFA_State("tsend_command_trans", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[148] = new DFA_State("tsend_command", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[149] = new DFA_State("send_trans", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[150] = new DFA_State("send", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[151] = new DFA_State("process_abort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /*Arun 110505 Start of code*/
            /***********************Math Builtins (eDDL)**********************/
            State[152] = new DFA_State("abs", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[153] = new DFA_State("acos", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[154] = new DFA_State("asin", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[155] = new DFA_State("atan", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[156] = new DFA_State("cbrt", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[157] = new DFA_State("ceil", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[158] = new DFA_State("cos", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[159] = new DFA_State("cosh", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[160] = new DFA_State("exp", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[161] = new DFA_State("floor", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[162] = new DFA_State("fmod", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[163] = new DFA_State("frand", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[164] = new DFA_State("log", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[165] = new DFA_State("log10", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[166] = new DFA_State("log2", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[167] = new DFA_State("pow", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[168] = new DFA_State("round", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[169] = new DFA_State("sin", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[170] = new DFA_State("sinh", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[171] = new DFA_State("sqrt", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[172] = new DFA_State("tan", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);    /* inserted 14feb07 stevev */
            State[173] = new DFA_State("tanh", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[174] = new DFA_State("trunc", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[175] = new DFA_State("atof", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[176] = new DFA_State("atoi", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[177] = new DFA_State("itoa", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /**********************End of Math Builtins (eDDL)***************/
            /*End of code*/
            /*Arun 160505 Start of code */
            /***********************Date Time Builtins (eDDL)*******************/
            //							State[0] = new DFA_State("YearMonthDay_to_Date", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);-WS:EPM Not a builtin-25jun07
            State[178] = new DFA_State("Date_to_Year", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[179] = new DFA_State("Date_to_Month", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[180] = new DFA_State("Date_to_DayOfMonth", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //							State[0] = new DFA_State("GetCurrentDate", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);-WS:EPM Not a builtin-25jun07
            State[181] = new DFA_State("GetCurrentTime", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            //							State[0] = new DFA_State("GetCurrentDateAndTime", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);-WS:EPM Not a builtin-25jun07
            //							State[0] = new DFA_State("To_Date_and_Time", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);-WS:EPM Not a builtin-25jun07
            State[182] = new DFA_State("DiffTime", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[183] = new DFA_State("AddTime", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[184] = new DFA_State("Make_Time", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[185] = new DFA_State("To_Time", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[186] = new DFA_State("Date_To_Time", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[187] = new DFA_State("To_Date", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[188] = new DFA_State("Time_To_Date", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            // added 16jul14 -----------------------------------------------------
            State[189] = new DFA_State("DATE_to_days", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[190] = new DFA_State("days_to_DATE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[191] = new DFA_State("From_DATE_AND_TIME_VALUE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[192] = new DFA_State("From_TIME_VALUE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[193] = new DFA_State("TIME_VALUE_to_seconds", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[194] = new DFA_State("TIME_VALUE_to_Hour", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[195] = new DFA_State("TIME_VALUE_to_Minute", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[196] = new DFA_State("TIME_VALUE_to_Second", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[197] = new DFA_State("seconds_to_TIME_VALUE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[198] = new DFA_State("DATE_AND_TIME_VALUE_to_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[199] = new DFA_State("DATE_to_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[200] = new DFA_State("TIME_VALUE_to_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[201] = new DFA_State("timet_to_string", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[202] = new DFA_State("timet_To_TIME_VALUE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[203] = new DFA_State("To_TIME_VALUE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /**********************End of Date Time Builtins (eDDL)************/
            //Added By Anil June 17 2005 --starts here
            /****************************Start of DD_STRING  Builtins  (eDDL) ********************/
            State[204] = new DFA_State("strstr", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            State[205] = new DFA_State("strupr", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            State[206] = new DFA_State("strlwr", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            State[207] = new DFA_State("strlen", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            State[208] = new DFA_State("strcmp", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            State[209] = new DFA_State("strtrim", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            State[210] = new DFA_State("strmid", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);//Anil changed to lower case 29th November 2005
            /****************************End of DD_STRING  Builtins (eDDL) ********************/
            //Added By Anil June 17 2005 --Ends here
            /* End of code */
            /*Vibhor 200905: Start of Code*/
            /****************************Start List Manipulation Builtins (eDDL) ********************/
            State[211] = new DFA_State("_ListInsert", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[212] = new DFA_State("_ListDeleteElementAt", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            /****************************End List Manipulation Builtins (eDDL) ********************/
            /*Vibhor 200905: End of Code*/
            //Anil September 26 2005 added MenuDisplay
            State[213] = new DFA_State("_MenuDisplay", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            // stevev 18feb09 - add transfer functions
            State[214] = new DFA_State("openTransferPort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[215] = new DFA_State("closeTransferPort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[216] = new DFA_State("abortTransferPort", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[217] = new DFA_State("_writeItemToDevice", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[218] = new DFA_State("_readItemFromDevice", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[219] = new DFA_State("get_transfer_status", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            // stevev 16jul14 - add debug functions
            State[220] = new DFA_State("_ERROR", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[221] = new DFA_State("_TRACE", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);
            State[222] = new DFA_State("_WARNING", RUL_TOKEN_TYPE.RUL_KEYWORD, RUL_TOKEN_SUBTYPE.RUL_FUNCTION);

        }

        public int nextToken(ref CToken ppToken, ref int i32NewPos, ref string pszBuffer, bool isLookAhead = false)// stevev 25apr13
        {
            string pszSource = m_pszSource;
            RUL_TOKEN_TYPE Type = RUL_TOKEN_TYPE.RUL_EOF;
            RUL_TOKEN_SUBTYPE SubType = RUL_TOKEN_SUBTYPE.RUL_ARRAY_DECL;
            bool bIsFound = false;
            bool bIsObject_or_Service = false;
            int i = 0;
            COMPOUND_DATA CmpData = new COMPOUND_DATA();

            //try
            {
                for (i = m_i32CurrentPos; /*(pszSource[i] != 0)*/ pszSource.Length > i && !bIsFound;)
                {
                    //munch all prepended white space of a token...
                    if (isSpace(pszSource, ref i))
                    {
                        ;
                    }
                    else if (isTerminal(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                    {
                        if (Type == RUL_TOKEN_TYPE.RUL_COMMENT)
                        {
                            while (pszSource[i] != 0 && pszSource[i++] != '\n')
                                ;
                            continue;
                        }

                        string pchBuffer1 = pszBuffer;

                        if ((Type == RUL_TOKEN_TYPE.RUL_SYMBOL) && (SubType == RUL_TOKEN_SUBTYPE.RUL_DOT))
                        {
                            if (isNumber(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                            {
                                //Get the numeric constant...
                                pchBuffer1 += pszBuffer;
                                pszBuffer = pchBuffer1;
                                bIsFound = true;
                                break;
                            }
                        }
                        //SCR26200 Felix
                        else if (pszBuffer[0] == LCUR)
                        {
                            if (!isLookAhead)// stevev 25apr13
                            {
                                this.m_nSymbolTableScopeIndex++;
                            }
                            if (pchBuffer1.Length > 0)
                            {
                                pchBuffer1 = pchBuffer1.Remove(0, 1);
                            }
                            pchBuffer1 = pchBuffer1.Insert(0, " ");
                        }
                        else if (pszBuffer[0] == RCUR)
                        {
                            if (this.m_nSymbolTableScopeIndex >= 1 && !isLookAhead)
                            {// we need to unscope variables at this.m_nSymbolTableScopeIndex
                                this.m_nSymbolTableScopeIndex--;
                            }
                            //pchBuffer1[0] = ' ';
                            if (pchBuffer1.Length > 0)
                            {
                                pchBuffer1 = pchBuffer1.Remove(0, 1);
                            }
                            pchBuffer1 = pchBuffer1.Insert(0, " ");
                        }

                        bIsFound = true;
                        //The next operator is Unary + or - operator if the current Token is 
                        //a.	Operator
                        //b.	Symbol
                        //c.	None
                        if ((m_CurToken.GetType() == RUL_TOKEN_TYPE.RUL_TYPE_NONE)
                            || (m_CurToken.IsOperator())
                            || (m_CurToken.IsSymbol() && m_CurToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RPAREN && m_CurToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RBOX)
                            || (m_CurToken.GetType() == RUL_TOKEN_TYPE.RUL_KEYWORD)
                            )
                        {
                            switch (SubType)
                            {
                                case RUL_TOKEN_SUBTYPE.RUL_PLUS:
                                    SubType = RUL_TOKEN_SUBTYPE.RUL_UPLUS;
                                    bIsFound = true;
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_MINUS:
                                    SubType = RUL_TOKEN_SUBTYPE.RUL_UMINUS;
                                    bIsFound = true;
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_PLUS_PLUS:
                                    SubType = RUL_TOKEN_SUBTYPE.RUL_PRE_PLUS_PLUS;
                                    bIsFound = true;
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_MINUS_MINUS:
                                    SubType = RUL_TOKEN_SUBTYPE.RUL_PRE_MINUS_MINUS;
                                    bIsFound = true;
                                    break;
                            }
                        }
                    }
                    else if (isNumber(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                    {
                        //Get the numeric constant...
                        bIsFound = true;
                    }
                    else if (isString(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                    {
                        //Get the string constant...
                        bIsFound = true;
                    }
                    else if (isChar(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                    {
                        //Get the char constant...
                        bIsFound = true;
                    }
                    else if (isService(pszSource, ref i, ref Type, ref SubType, ref pszBuffer, ref CmpData))
                    {
                        bIsFound = true;
                        bIsObject_or_Service = true;
                    }
                    else if (isIdentifier(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                    {
                        if (pszSource.Length > i && pszSource[i] == LBOX)
                        {
                            Type = RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE;
                        }
                        else
                        {
                            int iSavedPos = i;
                            while (pszSource.Length > iSavedPos && pszSource[iSavedPos] == ' ')
                            {
                                iSavedPos++;
                            }
                            if (pszSource.Length > iSavedPos && pszSource[iSavedPos] == LBOX)
                            {
                                Type = RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE;
                                i = iSavedPos;
                            }
                        }
                        bIsFound = true;
                    }
                    else
                    {
                        i++;
                        bIsFound = true;
                        Type = RUL_TOKEN_TYPE.RUL_TYPE_ERROR;
                        SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_ERROR;

                        throw (ep);
                    }
                }
                if (bIsFound)
                {
                    i32NewPos = i;
                    if (bIsObject_or_Service)
                    {
                        return Tokenize(i, Type, SubType, ref ppToken, ref pszBuffer, CmpData);
                    }
                    else
                    {
                        return Tokenize(i, Type, SubType, ref ppToken, ref pszBuffer);
                    }
                }
                return LEX_FAIL;
            }
            /*
            catch (CRIDEError* perr)
            {
                i32NewPos = i;
                *ppToken = 0;
                throw perr;
            }
            catch (...)	
	        {
                i32NewPos = i;
                throw (C_UM_ERROR_UNKNOWNERROR);
            }
            return LEX_FAIL;
            */
        }

        int nextAnyToken(CToken ppToken, int i32NewPos, string pszBuffer)
        {
            string pszSource = m_pszSource;
            RUL_TOKEN_TYPE Type = RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR;
            RUL_TOKEN_SUBTYPE SubType = RUL_TOKEN_SUBTYPE.RUL_ARRAY_DECL;
            bool bIsFound = false;
            bool bIsObject_or_Service = false;
            int i = 0;
            COMPOUND_DATA CmpData = new COMPOUND_DATA();

            //try
            {
                for (i = m_i32CurrentPos; (pszSource[i] != 0) && !bIsFound;)
                {
                    //munch all prepended white space of a token...
                    if (isSpace(pszSource, ref i))
                    {
                        ;
                    }
                    else if (isTerminal(pszSource, ref i, ref Type, ref SubType, ref pszBuffer))
                    {
                        bIsFound = true;
                    }
                    else
                    {
                        i++;
                        bIsFound = true;
                        Type = RUL_TOKEN_TYPE.RUL_TYPE_ERROR;
                        SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_ERROR;
                    }
                }
                if (bIsFound)
                {
                    i32NewPos = i;
                    if (bIsObject_or_Service)
                    {
                        return Tokenize(i, Type, SubType, ref ppToken, ref pszBuffer, CmpData);
                    }
                    else
                    {
                        return Tokenize(i, Type, SubType, ref ppToken, ref pszBuffer);
                    }
                }
                return LEX_FAIL;
            }
            /*
                catch (CRIDEError* perr)
                {
                    i32NewPos = i;
                    *ppToken = 0;
                    throw perr;
                }
                catch (...)	
	            {
                    i32NewPos = i;
                    throw (C_UM_ERROR_UNKNOWNERROR);
                }

            return LEX_FAIL;
            */
        }

        public int LookAheadToken(ref CToken ppToken)
        {
            //try
            {
                int i32NewPos = 0;
                string szBuffer = "";

                int i32Ret = nextToken(ref ppToken, ref i32NewPos, ref szBuffer, true);//stevev 25apr13
                m_i32LineNo = m_i32CurLineNo;

                return i32Ret;
            }
            /*
            catch (CRIDEError* perr)
            {
                m_pvecErr.Add(perr);
            }
            catch (...)	
	        {
                throw (C_UM_ERROR_UNKNOWNERROR);
            }

            return LEX_FAIL;
            */
        }

        public bool ScanLineForToken(RUL_TOKEN_TYPE tokenType, RUL_TOKEN_SUBTYPE tokenSubType, ref CToken ppToken)
        {
            int i32NewPos = 0, i32CurrentPos;
            int iPreviousCurrentPos = m_i32CurrentPos;

            string pszRulString = "";

            /* Get the rule string */
            if (!GetRulString(tokenType, tokenSubType, ref pszRulString))
            {
                return false;
            }

            i32CurrentPos = i32NewPos;


            //string pSubStringStartAddress = strstr(m_pszSource[m_i32CurrentPos], pszRulString);
            string SourceSubString = m_pszSource.Substring(m_i32CurrentPos);
            int l = SourceSubString.IndexOf(pszRulString);
            string pSubStringStartAddress = null;
            if (l > 0)
            {
                pSubStringStartAddress = SourceSubString.Substring(l);
            }

            /* Check if it is a == operator */
            if ((tokenSubType == RUL_TOKEN_SUBTYPE.RUL_ASSIGN) && (pSubStringStartAddress != null))
            {
                if (pSubStringStartAddress[0] == '=' && pSubStringStartAddress[1] == '=')
                {
                    pSubStringStartAddress = null;
                }
                else
                if (pSubStringStartAddress[0] == '=' && SourceSubString[l - 1] == '>')
                {
                    pSubStringStartAddress = null;
                }
                else
                if (pSubStringStartAddress[0] == '=' && SourceSubString[l - 1] == '<')
                {
                    pSubStringStartAddress = null;
                }
                else
                if (pSubStringStartAddress[0] == '=' && SourceSubString[l - 1] == '!')
                {
                    pSubStringStartAddress = null;
                }
            }

            if (pSubStringStartAddress == null)
            {
                return false;
            }


            string szSource = m_pszSource.Substring(m_i32CurrentPos);

            string pSubEOSStartAddress = null;

            //pSubEOSStartAddress =  (string)strstr((string)&m_pszSource[m_i32CurrentPos], ")");
            int o = szSource.IndexOf(")");
            if (o > 0)
            {
                pSubEOSStartAddress = szSource.Substring(o);
            }

            if (pSubEOSStartAddress != null)
            {
                //string pSubEOSStartAddressOpenBracket = (string)strstr((string)&m_pszSource[m_i32CurrentPos], "(");
                string pSubEOSStartAddressOpenBracket = null;
                int t = szSource.IndexOf("(");
                if (t > 0)
                {
                    pSubEOSStartAddressOpenBracket = szSource.Substring(t);
                }

                if (pSubEOSStartAddressOpenBracket != null)
                {
                    //if (pSubEOSStartAddressOpenBracket < pSubEOSStartAddress)
                    if (t < o)
                    {
                        pSubEOSStartAddress = null;
                    }
                }
            }

            string pSubSemiColonStartAddress = null;

            int n = szSource.IndexOf(";");
            //pSubSemiColonStartAddress = (string)strstr((string)&m_pszSource[m_i32CurrentPos], ";");
            if (n > 0)
            {
                pSubSemiColonStartAddress = szSource.Substring(n);
            }

            if (pSubSemiColonStartAddress != null)
            {
                if (pSubEOSStartAddress != null)
                {
                    if (n < o)
                    {
                        pSubEOSStartAddress = pSubSemiColonStartAddress;
                    }
                }
                else
                {
                    pSubEOSStartAddress = pSubSemiColonStartAddress;
                }
            }

            if (pSubEOSStartAddress != null)
            {
                if (l > n)
                {
                    return false;
                }
            }

            string pSubDoubleQuoteStartAddress = null;
            int k = szSource.IndexOf("\"");
            if (k > 0)
            {
                pSubDoubleQuoteStartAddress = szSource.Substring(k);
            }
            //pSubDoubleQuoteStartAddress = (string)strstr((string)&m_pszSource[m_i32CurrentPos], "\"");
            if ((pSubDoubleQuoteStartAddress != null) && (k < l))
            {
                //string pSubDoubleQuoteEndAddress = null;
                int m = pSubDoubleQuoteStartAddress.IndexOf("\"");
                //pSubDoubleQuoteEndAddress = (string)strstr((string)pSubDoubleQuoteStartAddress + 1, "\"");
                if (m + k > l)
                {
                    /* The symbol is within a string */
                    return false;
                }
            }

            if (pSubStringStartAddress != null)
            {
                int iReturnValue = TokenizeWithoutSave(0, tokenType, tokenSubType, ref ppToken, ref pszRulString);

                if (iReturnValue != 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public int GetNextToken(ref CToken ppToken, ref CSymbolTable pSymbolTable)
        {
            int i32NewPos = 0;
            //try
            {
                string szBuffer = null;
                if (LEX_FAIL != nextToken(ref ppToken, ref i32NewPos, ref szBuffer))
                {
                    SaveState(i32NewPos);

                    //Before u exit, make an entry in the Symbol Table
                    if (ppToken.IsVariable() || ppToken.IsArrayVar())
                    {
                        /*Vibhor 010705: Start of Code*/
                        /*Following change will prevent an undeclared variable to be used in a method
                        & consequently the Interpreter will throw a parsing error.

                        If we come here with a Variable token, and the same is not available in symbol Table
                        we possibly have a Global (DD) Variable. In that case, search it in the device and add
                        it to the Global symbol table. If its not found there too send a LEX_FAIL to the caller.

                        */
                        //int i32Idx = pSymbolTable.Insert(**ppToken);
                        //int i32Idx = pSymbolTable.GetIndex(ppToken.GetLexeme()); 
                        int i32Idx = pSymbolTable.GetIndex(ppToken.GetLexeme(), this.GetSymbolTableScopeIndex()); //SCR26200 Felix
                        if (-1 == i32Idx)
                        {
                            //Anil August 26 2005 For handling DD variable and Expression
                            //bool bIsVariableItem = true; //default is variable
                                                         //Anil Octobet 5 2005 for handling Method Calling Method
                            DD_ITEM_TYPE DDitemType = DD_ITEM_TYPE.DD_ITEM_METHOD;
                            i32Idx = m_pMEE.FindGlobalToken(ppToken.GetLexeme(), GetLineNumber(), ref DDitemType);
                            if (-1 != i32Idx)
                            {
                                UnGetToken();
                                ppToken.m_bIsGlobal = true;
                                ppToken.SetSymbolTableIndex(i32Idx);
                                ppToken.SetType(m_pMEE.m_GlobalSymTable.GetAt(i32Idx).Token.GetType());
                                ppToken.SetSubType(m_pMEE.m_GlobalSymTable.GetAt(i32Idx).Token.GetSubType());
                                string szDotExpression = null;
                                // stevev 16mar09 - method calling item other than method....  eg    menuItem();// makes a mess
                                if (m_pMEE.m_GlobalSymTable.GetAt(i32Idx).Token.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_DD_METHOD 
                                    && m_pszSource[i32NewPos] == '(')
                                {
                                    ppToken.SetType(RUL_TOKEN_TYPE.RUL_TYPE_ERROR);
                                    ppToken.SetSubType(RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_ERROR);

                                    //	throw(C_LEX_ERROR_ILLEGALITEM);
                                }
                                else
                                //Anil Octobet 5 2005 for handling Method Calling Method
                                if (GetComplexDotExp((i32NewPos), ref szDotExpression, DDitemType))
                                {
                                    string szFullLexeme = null;
                                    bool bResetGlobalSymbolTable = true;// emerson checkin april2013
                                    if (szDotExpression != null)
                                    {
                                        /*
                                        i32NewPos += strlen(szDotExpression);
                                        szFullLexeme = new char[strlen(ppToken.GetLexeme()) + strlen(szDotExpression) + 1];
                                        strcpy(szFullLexeme, ppToken.GetLexeme());
                                        strcat(szFullLexeme, szDotExpression);
                                        delete[] szDotExpression;      //WHS - June5-2007 - plug memory leak

                                        szDotExpression = null;         //WHS - June5-2007 - plug memory leak
                                        */
                                        szFullLexeme = ppToken.GetLexeme() + szDotExpression;
                                        ppToken.SetDDItemName(ppToken.GetLexeme());
                                        ppToken.SetLexeme(szFullLexeme);


                                        if (DDitemType == DD_ITEM_TYPE.DD_ITEM_VAR)// emerson checkin april2013
                                        {
                                            bResetGlobalSymbolTable = false; // do reset the global symbols table for this
                                        }
                                    }
                                    else
                                    {
                                        ppToken.SetDDItemName(ppToken.GetLexeme());

                                    }
                                    // bResetGlobalSymbolTable qualifiers //adde @ emerson checkin april2013
                                    if (ppToken.GetLexeme() != null && bResetGlobalSymbolTable)
                                    {
                                        m_pMEE.m_GlobalSymTable.GetAt(i32Idx).Token.SetLexeme(ppToken.GetLexeme());
                                    }
                                    if (ppToken.GetDDItemName() != null && bResetGlobalSymbolTable)
                                    {
                                        m_pMEE.m_GlobalSymTable.GetAt(i32Idx).Token.SetDDItemName(ppToken.GetDDItemName());
                                    }
                                    SaveState(i32NewPos);
                                    CToken pTokenTemp = null;
                                    //Anil Octobet 5 2005 for handling Method Calling Method
                                    //Changed the Condition for DD var and DD method
                                    string str = ppToken.GetLexeme();
                                    if (DDitemType == DD_ITEM_TYPE.DD_ITEM_VAR)
                                    {
                                        Tokenize(i32NewPos, RUL_TOKEN_TYPE.RUL_DD_ITEM, RUL_TOKEN_SUBTYPE.RUL_DD_SIMPLE, ref pTokenTemp, ref str);
                                    }
                                    //Anil Octobet 5 2005 for handling Method Calling Method
                                    else if (DDitemType == DD_ITEM_TYPE.DD_ITEM_NONVAR)
                                    {
                                        Tokenize(i32NewPos, RUL_TOKEN_TYPE.RUL_DD_ITEM, RUL_TOKEN_SUBTYPE.RUL_DD_COMPLEX, ref pTokenTemp, ref str);
                                    }
                                    else if (DDitemType == DD_ITEM_TYPE.DD_ITEM_METHOD)
                                    {
                                        Tokenize(i32NewPos, RUL_TOKEN_TYPE.RUL_DD_ITEM, RUL_TOKEN_SUBTYPE.RUL_DD_METHOD, ref pTokenTemp, ref str);
                                    }
                                }//get complex dot was false
                            }
                            else
                            {
                                return LEX_FAIL;
                            }

                        }
                        else// global token not found i32Idx == -1
                        {
                            //SCR26200 Felix
                            ppToken.SetSymbolTableIndex(i32Idx);
                            ppToken.SetSymbolTableScopeIndex(this.GetSymbolTableScopeIndex());     //SCR26200 Felix
                            ppToken.SetSubType(pSymbolTable.GetAt(i32Idx).Token.GetSubType());
                        }
                        /*Vibhor 010705: End of Code*/
                    }
                    else if (RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT == ppToken.GetSubType())
                    {
                        int i32Idx = pSymbolTable.InsertConstant(ppToken);
                        if (i32Idx >= 0)// emerson checkin april2013
                        {
                            ppToken.SetConstantIndex(i32Idx);
                        }
                        else
                        {
                            //LOGIT(CERR_LOG, "Unable to insert string constant into symbol table\n");
                        }
                    }
                    else if (RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT == ppToken.GetSubType())
                    {
                        int i32Idx = pSymbolTable.InsertConstant(ppToken);
                        if (i32Idx >= 0)// emerson checkin april2013
                        {
                            ppToken.SetConstantIndex(i32Idx);
                        }
                        else
                        {
                        }
                    }
                    return LEX_SUCCESS;
                }
            }
            /*
            catch (CRIDEError* perr)
            {
                SaveState(i32NewPos);
                m_pvecErr.Add(perr);
            }
            catch (...)
	        {
                SaveState(i32NewPos);
                throw (C_UM_ERROR_UNKNOWNERROR);
            }
            return LEX_FAIL;
            */
            return LEX_FAIL;
        }

        public int GetNextVarToken(ref CToken ppToken, ref CSymbolTable pSymbolTable, RUL_TOKEN_SUBTYPE SubType)
        {
            int i32NewPos = 0;
            //try
            {
                string szBuffer = null;
                if (LEX_FAIL != nextToken(ref ppToken, ref i32NewPos, ref szBuffer))
                {
                    SaveState(i32NewPos);

                    //Before u exit, make an entry in the Symbol Table
                    if (ppToken.IsVariable())
                    {
                        ppToken.SetSubType(SubType);
                        int i32Idx = pSymbolTable.Insert(ppToken, this.GetSymbolTableScopeIndex());
                        if (i32Idx >= 0)//check if a valid index before crashing.
                        {
                            ppToken.SetSymbolTableIndex(i32Idx);
                            ppToken.SetSymbolTableScopeIndex(this.GetSymbolTableScopeIndex());
                            ppToken.SetSubType(pSymbolTable.GetAt(i32Idx).Token.GetSubType());
                        }
                        else
                        {
                            //LOGIT(CERR_LOG, "Unable to insert variable into symbol table\n");
                        }
                    }
                    else
                    if (ppToken.IsArrayVar())
                    {
                        ppToken.SetSubType(RUL_TOKEN_SUBTYPE.RUL_ARRAY_DECL);
                        int i32Idx = pSymbolTable.Insert(ppToken, this.GetSymbolTableScopeIndex());
                        if (i32Idx >= 0)// emerson checkin april2013
                        {
                            ppToken.SetSymbolTableIndex(i32Idx);
                            ppToken.SetSymbolTableScopeIndex(this.GetSymbolTableScopeIndex());
                            ppToken.SetSubType(pSymbolTable.GetAt(i32Idx).Token.GetSubType());
                        }
                        else
                        {
                            //LOGIT(CERR_LOG, "Unable to insert array into symbol table\n");
                        }
                    }
                    else if (RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT == ppToken.GetSubType())
                    {
                        int i32Idx = pSymbolTable.InsertConstant(ppToken);
                        if (i32Idx >= 0)// emerson checkin april2013
                        {
                            ppToken.SetConstantIndex(i32Idx);
                        }
                        else
                        {
                            //LOGIT(CERR_LOG, "Unable to insert string constant into symbol table\n");
                        }

                    }
                    else if (RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT == ppToken.GetSubType())
                    {
                        int i32Idx = pSymbolTable.InsertConstant(ppToken);
                        if (i32Idx >= 0)// emerson checkin april2013
                        {
                            ppToken.SetConstantIndex(i32Idx);
                        }
                        else
                        {
                        }

                    }
                    return LEX_SUCCESS;
                }
            }
            /*
            catch (CRIDEError* perr)
            {
                SaveState(i32NewPos);
                m_pvecErr.Add(perr);
            }
            catch (...)
            {
                SaveState(i32NewPos);
                throw (C_UM_ERROR_UNKNOWNERROR);
            }
            return LEX_FAIL;
            */
            return LEX_FAIL;
        }

        public int UnGetToken()
        {
            m_CurToken = m_PrevToken;
            m_i32CurrentPos = m_i32PrevPos;

            m_i32LineNo = m_i32PrevLineNo;
            m_i32CurLineNo = m_i32PrevLineNo;
            m_nLastSymbolTableScopeIndex = m_nPrevSymbolTableScopeIndex;//stevev 25apr13
            m_nSymbolTableScopeIndex = m_nPrevSymbolTableScopeIndex;
            return LEX_SUCCESS;
        }

        public int Load(string pszRule, string pszRuleName/*, ERROR_VEC* pvecErrors*/)
        {
            m_pszSource = pszRule;
            //m_pvecErr = pvecErrors;
            return LEX_SUCCESS;
        }

        int SaveState(int i32CurState)
        {
            m_i32PrevPos = m_i32CurrentPos;
            m_i32CurrentPos = i32CurState;
            m_i32PrevLineNo = m_i32CurLineNo;
            m_i32CurLineNo = m_i32LineNo;
            m_nPrevSymbolTableScopeIndex = m_nLastSymbolTableScopeIndex;
            m_nLastSymbolTableScopeIndex = m_nSymbolTableScopeIndex;//stevev 25apr13(reused LastSymbol)
            return LEX_SUCCESS;
        }

        public bool IsEndOfSource()
        {
            // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
            return (m_i32CurrentPos >= m_pszSource.Length) ? true : false;
        }

        public bool isSpace(string pszSource, ref int i)
        {
            if (pszSource[i] == ' ' || pszSource[i] == '\t' || pszSource[i] == '\r')
            {
                //munch all prepended white space of a token...
                i++;
            }
            else if (pszSource[i] == '\n')
            {
                m_i32LineNo++;
                i++;
            }
            else
            {
                return false;
            }

            return true;
        }

        //match for the regular expression 
        //	digit	-.		[0-9]
        //	Number	-.		digit+ (.digit+)
        bool isNumber(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            pszBuffer = "";
            if (Char.IsDigit(pszSource[i32CurPos]) || (pszSource[i32CurPos] == '.'))
            {
                int i = 0;
                bool bIsFloat = false;
                bool bIsHex = false;
                for (; pszSource[i32CurPos + i] != 0; i++)
                {
                    if (i == 0)
                    {
                        if (Char.IsDigit(pszSource[i32CurPos + i]))
                        {
                            if (pszBuffer.Length > i)
                            {
                                pszBuffer = pszBuffer.Remove(i, 1);
                            }
                            pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                            //pszBuffer[i] = pszSource[i32CurPos + i];
                            //pszBuffer[i + 1] = 0;
                            if (pszBuffer[0] == '0' && ((pszSource[i32CurPos + i + 1] == 'x') || (pszSource[i32CurPos + i + 1] == 'X')))
                            {
                                //pszBuffer = pszBuffer.Remove(i + 1, 1);
                                pszBuffer = pszBuffer.Insert(i + 1, pszSource[i32CurPos + i + 1].ToString());
                                //pszBuffer[i + 1] = pszSource[i32CurPos + i + 1];
                                bIsHex = true;
                                i++;
                            }
                            continue;
                        }
                    }
                    if (Char.IsDigit(pszSource[i32CurPos + i]))
                    {
                        //pszBuffer[i] = pszSource[i32CurPos + i];
                        //pszBuffer = pszBuffer.Remove(i, 1);
                        if (pszBuffer.Length > i)
                        {
                            pszBuffer = pszBuffer.Remove(i, 1);
                        }
                        pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                        continue;
                    }
                    else if (('.' == pszSource[i32CurPos + i]) && !bIsFloat)
                    {
                        //pszBuffer[i] = pszSource[i32CurPos + i];
                        if (pszBuffer.Length > i)
                        {
                            pszBuffer = pszBuffer.Remove(i, 1);
                        }
                        pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                        bIsFloat = true;
                        continue;
                    }
                    else
                    if (bIsFloat &&
                         (('e' == pszSource[i32CurPos + i]) ||
                           ('E' == pszSource[i32CurPos + i])
                         ))
                    {
                        if (('+' == pszSource[i32CurPos + i + 1]) || ('-' == pszSource[i32CurPos + i + 1]) ||
                             Char.IsDigit(pszSource[i32CurPos + i + 1]))//WHS EP June17-2008 support implicit + sign for scientific notation
                        {
                            //pszBuffer[i] = pszSource[i32CurPos + i];
                            //pszBuffer[i + 1] = pszSource[i32CurPos + i + 1];
                            if (pszBuffer.Length > i)
                            {
                                pszBuffer = pszBuffer.Remove(i, 1);
                            }
                            pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                            if (pszBuffer.Length > i + 1)
                            {
                                pszBuffer = pszBuffer.Remove(i + 1, 1);
                            }
                            pszBuffer = pszBuffer.Insert(i + 1, pszSource[i32CurPos + i + 1].ToString());
                            i++;
                            continue;
                        }
                    }
                    else if (bIsHex)
                    {
                        bool bUnknownSymbolFound = false;
                        switch (pszSource[i32CurPos + i])
                        {
                            case 'a':
                            case 'b':
                            case 'c':
                            case 'd':
                            case 'e':
                            case 'f':
                            case 'A':
                            case 'B':
                            case 'C':
                            case 'D':
                            case 'E':
                            case 'F':
                                {
                                    //pszBuffer[i] = pszSource[i32CurPos + i];
                                    if (pszBuffer.Length > i)
                                    {
                                        pszBuffer = pszBuffer.Remove(i, 1);
                                    }
                                    pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                                    continue;
                                }
                            default:
                                {
                                    bUnknownSymbolFound = true;
                                    break;
                                }
                        }
                        if (bUnknownSymbolFound)
                            break;
                    }
                    else
                    {
                        if (!bIsHex && !bIsFloat)
                        {
                            if (
                            ('e' == pszSource[i32CurPos + i])
                            || ('E' == pszSource[i32CurPos + i])
                            )
                            {
                                bIsFloat = true;
                                //pszBuffer[i] = pszSource[i32CurPos + i];
                                if (pszBuffer.Length > i)
                                {
                                    pszBuffer = pszBuffer.Remove(i, 1);
                                }
                                pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                                if (('+' == pszSource[i32CurPos + i + 1]) || ('-' == pszSource[i32CurPos + i + 1]) ||
                                     Char.IsDigit(pszSource[i32CurPos + i + 1]))//WHS EP June17-2008 support implicit + sign for scientific notation
                                {
                                    //pszBuffer[i + 1] = pszSource[i32CurPos + i + 1];
                                    //pszBuffer = pszBuffer.Remove(i + 1, 1);
                                    pszBuffer = pszBuffer.Insert(i + 1, pszSource[i32CurPos + i + 1].ToString());
                                    i++;
                                }
                                continue;
                            }

                        }
                        break;
                    }
                }
                if (i >= 0) //if found a number
                {
                    i32CurPos += i;
                    Type = RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT;
                    SubType = bIsFloat ? RUL_TOKEN_SUBTYPE.RUL_REAL_CONSTANT : RUL_TOKEN_SUBTYPE.RUL_INT_CONSTANT;
                    return true;
                }

            }
            return false;
        }

        bool isString(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            //memset(pszBuffer, 0, BUFFER_SIZE);
            pszBuffer = "";
            if ('"' == pszSource[i32CurPos])
            {
                int i = 1;
                for (; '"' != pszSource[i32CurPos + i]; i++)
                {
                    //pszBuffer[i - 1] = pszSource[i32CurPos + i];
                    //pszBuffer = pszBuffer.Remove(i - 1, 1);
                    pszBuffer = pszBuffer.Insert(i - 1, pszSource[i32CurPos + i].ToString());
                    if ((pszSource[i32CurPos + i] == '\\') && (pszSource[i32CurPos + i + 1] == '"')
                        && (pszSource[i32CurPos + i - 1] != '\\'))
                    {
                        //pszBuffer[i - 1 + 1] = pszSource[i32CurPos + i + 1];
                        if (pszBuffer.Length > i)
                        {
                            pszBuffer = pszBuffer.Remove(i, 1);
                        }
                        pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i + 1].ToString());
                        i++;
                    }
                }
                if ('"' == pszSource[i32CurPos + i])
                {
                    i++;
                    i32CurPos += i;
                    Type = RUL_TOKEN_TYPE.RUL_STR_CONSTANT;
                    SubType = RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT;
                    return true;
                }
            }
            return false;
        }

        bool isChar(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            //memset(pszBuffer, 0, BUFFER_SIZE);
            pszBuffer = "";
            if ('\'' == pszSource[i32CurPos])
            {
                int i = 1;
                for (; '\'' != pszSource[i32CurPos + i]; i++)
                {
                    //pszBuffer[i - 1] = pszSource[i32CurPos + i];
                    //pszBuffer = pszBuffer.Remove(i - 1, 1);
                    pszBuffer = pszBuffer.Insert(i - 1, pszSource[i32CurPos + i].ToString());
                }
                if ('\'' == pszSource[i32CurPos + i])
                {
                    i++;
                    i32CurPos += i;
                    Type = RUL_TOKEN_TYPE.RUL_CHR_CONSTANT;
                    SubType = RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT;
                    return true;
                }
            }
            return false;
        }

        //Store the previous token for identifying the 
        //unary- operator

        int Tokenize(int i32CurState, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, ref CToken ppToken, ref string pszBuffer)
        {
            //try
            {
                ppToken = new CToken(pszBuffer, Type, SubType, GetLineNumber());
                if (null == ppToken)
                {
                    throw (ep);
                }
                m_PrevToken = m_CurToken;
                m_CurToken = ppToken;

                if (Type == RUL_TOKEN_TYPE.RUL_TYPE_ERROR)
                {
                    pszBuffer = "Error in Lexical Analysis";
                }

                return LEX_SUCCESS;
            }

            /*
            catch (CRIDEError* perr)
            {
                DELETE_PTRppToken;
                throw perr;
            }
            catch ()	
            {
                DELETE_PTRppToken;
                throw (C_UM_ERROR_UNKNOWNERROR);
            }
            */
        }

        //Store the previous token for identifying the 
        //unary- operator

        int Tokenize(int i32CurState, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, ref CToken ppToken, ref string pszBuffer, COMPOUND_DATA cmpData)
        {
            //try
            {
                //memset(pszBuffer, 0, BUFFER_SIZE);
                //strcat(pszBuffer, cmpData.m_szName);

                pszBuffer = cmpData.m_szName;

                ppToken = new CToken(pszBuffer, Type, SubType, cmpData, GetLineNumber());
                if (null == ppToken)
                {
                    throw (ep);
                }
                m_PrevToken = m_CurToken;
                m_CurToken = ppToken;

                if (Type == RUL_TOKEN_TYPE.RUL_TYPE_ERROR)
                {
                    pszBuffer = "Error in Lexical Analysis";
                }

                return LEX_SUCCESS;
            }
            /*
            catch (CRIDEError* perr)
            {
                DELETE_PTRppToken;
                throw perr;
            }
            catch (...)	
            {
                DELETE_PTRppToken;
                throw (C_UM_ERROR_UNKNOWNERROR);
            }
            return LEX_FAIL;
            */
        }

        int TokenizeWithoutSave(int i32CurState, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, ref CToken ppToken, ref string pszBuffer)
        {
            //try
            {
                ppToken = new CToken(pszBuffer, Type, SubType, GetLineNumber());
                if (null == ppToken)
                {
                    throw (ep);
                }

                if (Type == RUL_TOKEN_TYPE.RUL_TYPE_ERROR)
                {
                    pszBuffer = "Error in Lexical Analysis";
                }

                return LEX_SUCCESS;
            }
            /*
            catch (CRIDEError* perr)
            {
                DELETE_PTRppToken;
                throw perr;
            }
            catch (...)	
	        {
                DELETE_PTRppToken;
                throw (C_UM_ERROR_UNKNOWNERROR);
            }
        return LEX_FAIL;
            */
        }

        bool MatchGrammarTerminals(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            //int i32Size = sizeof(State) / sizeof(DFA_State);
            int i32Size = State.Count();
            int i = 0, j = 0;

            for (i = 0; i < i32Size; i++)
            {
                int i32Len = State[i].szWord.Length;//strlen(State[i].szWord);
                pszBuffer = "";
                for (j = 0; /*(pszSource[i32CurPos + j] != 0) &&*/ (pszSource.Length > i32CurPos + j) && (j < i32Len); j++)
                {
                    //pszBuffer = pszBuffer.Remove(j, 1);
                    pszBuffer = pszBuffer.Insert(j, pszSource[i32CurPos + j].ToString());

                    if (State[i].szWord[j] != pszSource[i32CurPos + j])
                        break;
                }

                if ((j == i32Len) && (pszSource.Length > i32CurPos + j) && (pszSource[i32CurPos + j] == '_') && (State[i].Type == RUL_TOKEN_TYPE.RUL_KEYWORD))
                {
                    continue;
                }

                if ((j == i32Len) && ((State[i].Type != RUL_TOKEN_TYPE.RUL_KEYWORD) || ((State[i].Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && !Char.IsLetterOrDigit(pszSource[i32CurPos + j]))))    //match found
                {
                    i32CurPos += j;
                    Type = State[i].Type;
                    SubType = State[i].SubType;
                    return true;
                }
            }

            return false;
        }

        public bool MatchOMService(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            //int nSize = OM_Service) / sizeof(DFA_State);
            int nSize = OM_Service.Count();
            int i = 0, j = 0;

            for (i = 0; i < nSize; i++)
            {
                int nLen = OM_Service[i].szWord.Length;//strlen(OM_Service[i].szWord);
                //memset(pszBuffer, 0, BUFFER_SIZE);
                pszBuffer = "";
                for (j = 0; (pszSource[i32CurPos + j] != 0) && (j < nLen); j++)
                {
                    //pszBuffer[j] = pszSource[i32CurPos + j];

                    //pszBuffer = pszBuffer.Remove(j, 1);
                    pszBuffer = pszBuffer.Insert(j, pszSource[i32CurPos + j].ToString());

                    if (OM_Service[i].szWord[j] != pszSource[i32CurPos + j])
                        break;
                }
                if ((j == nLen) && ((OM_Service[i].Type != RUL_TOKEN_TYPE.RUL_KEYWORD) || ((OM_Service[i].Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && !Char.IsLetterOrDigit(pszSource[i32CurPos + j]))))    //match found
                {
                    i32CurPos += j;
                    Type = OM_Service[i].Type;
                    SubType = OM_Service[i].SubType;
                    return true;
                }
            }

            return false;
        }

        public bool isTerminal(string szSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            return MatchGrammarTerminals(szSource, ref i32CurPos, ref Type, ref SubType, ref pszBuffer);
        }

        bool isPointChar(char ch)
        {
            return (Char.IsLetterOrDigit(ch) || ch == '.');
        }

        //	match for the regular expression 
        //	letter		-. [a-zA-Z]
        //	digit		-. [0-9]
        //	identifier	-.	letter(letter|digit)*
        bool isIdentifier(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer)
        {
            //try
            {
                if (Char.IsLetter(pszSource[i32CurPos]) || (pszSource[i32CurPos] == '_'))
                {
                    //Get the Identifier
                    int i = 0;
                    pszBuffer = "";
                    for (i = 0; (pszSource.Length > i32CurPos + i) && (Char.IsLetterOrDigit(pszSource[i32CurPos + i]) || pszSource[i32CurPos + i] == '_') && (i < BUFFER_SIZE); i++)
                    {
                        //pszBuffer[i] = pszSource[i32CurPos + i];
                        if (pszBuffer.Length > i)
                        {
                            pszBuffer = pszBuffer.Remove(i, 1);
                        }
                        pszBuffer = pszBuffer.Insert(i, pszSource[i32CurPos + i].ToString());
                    }
                    //Variable -- pszBuffer -- It's got to be a variable now.
                    i32CurPos += i;
                    Type = RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE;//Keywords[i].Type;
                    SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;//Keywords[i].SubType;
                    if (i >= BUFFER_SIZE)
                    {
                        throw (ep);
                    }
                    return true;
                }
                return false;
            }
            /*
            catch(CRIDEError* perr)
            {					
                UNUSED_LOCAL(perr);		
                throw;	
            }
            catch(...)
            {								
                throw(C_UM_ERROR_UNKNOWNERROR);
            }
            return LEX_FAIL;
            */
        }

        int GetLineNumber()
        {
            return m_i32CurLineNo;
        }

        //The Object access is of the form
        // <ObjectManager::><Id(.Id)*>
        bool isObject(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, string pszBuffer, ref COMPOUND_DATA cmpData)
        {
            int i32Temp = i32CurPos;
            return false;
        }

        bool isService(string pszSource, ref int i32CurPos, ref RUL_TOKEN_TYPE Type, ref RUL_TOKEN_SUBTYPE SubType, ref string pszBuffer, ref COMPOUND_DATA cmpData)
        {
            int i32Temp = i32CurPos;
            if (MatchOMService(pszSource, ref i32Temp, ref Type, ref SubType, ref pszBuffer) && (Type == RUL_TOKEN_TYPE.RUL_SERVICE))
            {
                i32CurPos = i32Temp;    //if the token is a desired one, then update the cur pointer.
                RUL_TOKEN_TYPE newType = RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR;
                RUL_TOKEN_SUBTYPE newSubType = RUL_TOKEN_SUBTYPE.RUL_ARRAY_DECL;
                cmpData = new COMPOUND_DATA();
                int i = i32CurPos;

                while (isSpace(pszSource, ref i));
                //if (isTerminal(pszBuffer, ref i, ref newType, ref newSubType, ref pszBuffer) && (newSubType == RUL_TOKEN_SUBTYPE.RUL_SCOPE))
                if (isTerminal(pszSource, ref i, ref newType, ref newSubType, ref pszBuffer) && (newSubType == RUL_TOKEN_SUBTYPE.RUL_SCOPE))
                {
                    while (isSpace(pszSource, ref i)) ;
                    if (isIdentifier(pszSource, ref i, ref newType, ref newSubType, ref pszBuffer))
                    {
                        //Now go on looking for (.Id)*
                        //1.	Copy the buffer into the Compound Data.
                        //2.	Go on looking for <alphanum> and <.>
                        i32CurPos = i;
                        //int nLen = strlen(pszBuffer);
                        //memset(cmpData.m_szName, 0, nLen + 1);// this was cleared earlier
                        //memcpy(cmpData.m_szName, pszBuffer, nLen);
                        cmpData.m_szName = pszBuffer;

                        if (isTerminal(pszSource, ref i, ref newType, ref newSubType, ref pszBuffer) && (newSubType == RUL_TOKEN_SUBTYPE.RUL_DOT))
                        {
                            if (isIdentifier(pszSource, ref i, ref newType, ref newSubType, ref pszBuffer))
                            {
                                //strcat(cmpData.m_szAttribute, ".");
                                //strcat(cmpData.m_szAttribute, pszBuffer);
                                cmpData.m_szAttribute += "." + pszBuffer;
                                i32CurPos = i;
                                if (isTerminal(pszSource, ref i, ref newType, ref newSubType, ref pszBuffer) && (newSubType == RUL_TOKEN_SUBTYPE.RUL_LPAREN))
                                {
                                    if (isTerminal(pszSource, ref i, ref newType, ref newSubType, ref pszBuffer) && (newSubType == RUL_TOKEN_SUBTYPE.RUL_RPAREN))
                                    {
                                        SubType = RUL_TOKEN_SUBTYPE.RUL_SERVICE_INVOKE;
                                        i32CurPos = i;
                                    }
                                    else
                                    {
                                        //error -- mismatched parenthesis
                                        return false;
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                //error -- no identifier after <.>
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //error --  no identifier after <::>
                        return false;
                    }
                }
            }
            return false;
        }

        string GetRuleName()
        {
            //	return (string)m_szRuleName;
            return null;
        }

        int MoveTo(RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            int i32Ret = LEX_FAIL;
            bool bIsFound = false;
            while (LEX_SUCCESS == (i32Ret = GetNextToken(ref pToken, ref pSymbolTable)) && pToken != null)
            {
                RUL_TOKEN_TYPE newType = pToken.GetType();
                RUL_TOKEN_SUBTYPE newSubType = pToken.GetSubType();

                if (newType == Type && newSubType == SubType)
                {
                    bIsFound = true;
                    break;
                }
            }
            if (bIsFound)
            {
                UnGetToken();
            }
            return i32Ret;
        }

        public int MovePast(RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            int i32Ret = LEX_FAIL;
            while (LEX_SUCCESS == (i32Ret = GetNextToken(ref pToken, ref pSymbolTable)) && pToken != null)
            {
                RUL_TOKEN_TYPE newType = pToken.GetType();
                RUL_TOKEN_SUBTYPE newSubType = pToken.GetSubType();

                if (newType == Type && newSubType == SubType)
                {
                    break;
                }
            }
            return i32Ret;
        }

        public int SynchronizeTo(PRODUCTION production, CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            int i32Ret = LEX_FAIL;
            bool bIsFound = false;

            while (LEX_SUCCESS == (i32Ret = GetNextToken(ref pToken, ref pSymbolTable)) && pToken != null)
            {
                RUL_TOKEN_TYPE newType = pToken.GetType();
                RUL_TOKEN_SUBTYPE newSubType = pToken.GetSubType();

                if (g_follow_set.IsPresent(production, newType, newSubType))
                {
                    bIsFound = true;
                    break;
                }
            }
            if (bIsFound)
            {
                UnGetToken();
            }

            return i32Ret;
        }

        bool GetRulString(RUL_TOKEN_TYPE tokenType, RUL_TOKEN_SUBTYPE tokenSubType, ref string pszRulString)
        {
            //int i32Size = sizeof(State) / sizeof(DFA_State);
            int i32Size = State.Length;

            for (int iLoopVar = 0; iLoopVar < i32Size; iLoopVar++)
            {
                if ((State[iLoopVar].Type == tokenType) && (State[iLoopVar].SubType == tokenSubType))
                {
                    //strcpy(pszRulString, State[iLoopVar].szWord);
                    pszRulString = State[iLoopVar].szWord;
                    return true;
                }
            }
            return false;
        }


        //This fuction gets the Expression followed by the Firs dot Operatot which we
        //May need to resolve while executing the statement
        //Anil changed Function Prototype Octobet 5 2005 for handling Method Calling Method
        public bool GetComplexDotExp(int iPosOfDot, ref string szDotExpression, DD_ITEM_TYPE DDitemType)//Changed the Function type Anil September 16 2005
        {

            //If it is not variable type then Do the below chwcking
            bool bIsVariableItem = true;
            if (DDitemType == DD_ITEM_TYPE.DD_ITEM_NONVAR)
            {
                bIsVariableItem = false;
            }

            if (bIsVariableItem == false)
            {
                if (!((m_pszSource[iPosOfDot] == '.') || (m_pszSource[iPosOfDot] == '[')))
                {
                    return false;
                }                    
            }

            //Get the Expression followe by first dot (.)
            bool bIsEnd = false;
            string pszSource = m_pszSource;
            int iLeftBrackCount = 0;
            //RUL_TOKEN_TYPE Type;
            //RUL_TOKEN_SUBTYPE SubType;
            //string pszBuffer = null;
            int lCount = 0;
            bool nNonSpacePresent = false;

            //Added to take care of Methos DD item Anil September 16 2005
            if (DDitemType == DD_ITEM_TYPE.DD_ITEM_METHOD)
            {
                int i = iPosOfDot;
                while (isSpace(pszSource, ref i)) ;
                if (pszSource[i] != '(')
                {
                    return false;
                }
                int iLeftBrace = 0;
                i = iPosOfDot;
                for (; (pszSource[i] != 0) && !bIsEnd;)
                {
                    if (isSpace(pszSource, ref i))
                    {
                        lCount++;
                        continue;
                    }
                    if ((iLeftBrace == 0) && IsEndofComDotOp(pszSource[i], i))
                    {
                        break;
                    }
                    if (pszSource[i] == '(')
                    {
                        iLeftBrace++;

                    }
                    if (pszSource[i] == ')')
                    {
                        iLeftBrace--;

                    }
                    lCount++;
                    i++;
                }

                if (iLeftBrace != 0)
                {
                    return false;
                }

                if (lCount > 0)
                {
                    int iTemp = lCount;
                    //This is because , it may happen Space follwed by the Comple DD expression is present, Just remove that
                    for (i = iPosOfDot + iTemp - 1; i > iPosOfDot; i--)
                    {

                        int lTemp = i;
                        if (isSpace(pszSource, ref lTemp))
                        {
                            lCount--;
                            continue;
                        }
                        break;
                    }
                    //*szDotExpression = new char[lCount + 1];
                    //strncpy(*szDotExpression, (string)&m_pszSource[iPosOfDot], lCount);
                    //(*szDotExpression)[lCount] = '\0';
                    szDotExpression = m_pszSource.Substring(iPosOfDot);
                }
                return true;
            }
            //Loop through and get the total count of the Expression
            for (int i = iPosOfDot; (pszSource[i] != 0) && !bIsEnd;)
            {
                if (isSpace(pszSource, ref i))
                {
                    lCount++;
                    continue;
                }
                if ((iLeftBrackCount == 0) && IsEndofComDotOp(pszSource[i], i))
                {
                    break;
                }
                nNonSpacePresent = true;
                if (pszSource[i] == '[')
                {
                    iLeftBrackCount++;

                }
                if (pszSource[i] == ']')
                {
                    iLeftBrackCount--;

                }
                lCount++;
                i++;
            }

            if (bIsVariableItem == false && lCount == 0)
            {
                return false;
            }

            if ((lCount > 0) && (nNonSpacePresent == true))
            {
                int iTemp = lCount;
                //This is because , it may happen Space follwed by the Comple DD expression is present, Just remove that
                for (int i = iPosOfDot + iTemp - 1; i > iPosOfDot; i--)
                {

                    int lTemp = i;
                    if (isSpace(pszSource, ref lTemp))
                    {
                        lCount--;
                        continue;
                    }
                    break;
                }
                //*szDotExpression = new char[lCount + 1];
                //strncpy(*szDotExpression, (string)&m_pszSource[iPosOfDot], lCount);
                //(*szDotExpression)[lCount] = '\0';
                szDotExpression = m_pszSource.Substring(iPosOfDot);
            }
            return true;
        }
        //Helper Function to know whether DD Express is terminated
        bool IsEndofComDotOp(char ch, int i)
        {
            if ((m_pszSource[i] == '+') ||
                (m_pszSource[i] == '-') ||
                (m_pszSource[i] == '/') ||
                (m_pszSource[i] == '*') ||
                (m_pszSource[i] == ';') ||
                (m_pszSource[i] == '=') ||
                (m_pszSource[i] == '>') ||  /* added as per Anil 24oct05 */
                (m_pszSource[i] == '<') ||  /* added as per Anil 24oct05 */
                (m_pszSource[i] == '!') ||  /* added as per Anil 24oct05 */
                (m_pszSource[i] == '&') ||  /* added as per Anil 24oct05 */
                (m_pszSource[i] == '|') ||  /* added as per Anil 24oct05 */
                (m_pszSource[i] == '^') ||  /* added as per Anil 24oct05 */
                (m_pszSource[i] == ',') ||
                //Anil 040806 Bug fix 562 
                //If the DD expression follwed by ")", which is the case in if statement
                (m_pszSource[i] == ')')
                )
            {

                return true;
            }
            return false;
        }

        //SCR26200 Felix
        void SetSymbolTableScopeIndex(int nSymTblScpIdx)
        {
            m_nSymbolTableScopeIndex = nSymTblScpIdx;
        }

        public int GetSymbolTableScopeIndex() //SCR26200 Felix
        {
            return m_nSymbolTableScopeIndex;
        }

    }

}
