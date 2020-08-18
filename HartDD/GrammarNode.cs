using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public enum GRAMMAR_NODE_TYPE
    {
        NODE_TYPE_INVALID
        , NODE_TYPE_ASSIGN
        , NODE_TYPE_EXPRESSION
    }

    public class CGrammarNode
    {
        private GRAMMAR_NODE_TYPE m_NodeType;
        int m_CurrentScopeIndex;//SCR26200 Felix

        // The return codes for the Execute function calls
        public const int VISIT_ERROR = 0;
        public const int VISIT_BREAK = 1;
        public const int VISIT_CONTINUE = 2;
        public const int VISIT_RETURN = 3;
        public const int VISIT_NORMAL = 4;

        //Anil 240107 Defined this when declaration come after some statement list
        //Basically for the scope of the variable
        public const int VISIT_SCOPE_VAR = 5;


        public static string[] szTokenSubstrings =
        {
                                    "SUBTYPE_NONE",
									//INT Operators
									"UPLUS",
                                    "UMINUS",
                                    "PLUS",
                                    "MINUS",
                                    "MUL",
                                    "DIV",
                                    "MOD",
                                    "EXP",
                                    "NOT_EQ",
                                    "LT",
                                    "GT",
                                    "EQ",
                                    "GE",
                                    "LE",
                                    "LOGIC_AND",
                                    "LOGIC_OR",
                                    "LOGIC_NOT",
                                    "ASSIGN",
									//FLOAT Operators
									"FUPLUS",
                                    "FUMINUS",
                                    "FPLUS",
                                    "FMINUS",
                                    "FMUL",
                                    "FDIV",
                                    "FMOD",
                                    "FRAND",
                                    "FEXP",
                                    "I2F",
                                    "F2I",
                                    "NOT_FEQ",
                                    "FLT",
                                    "FGT",
                                    "FEQ",
                                    "FGE",
                                    "FLE",
									//String Operators
									"SPLUS",
                                    "SEQ",
                                    "NOT_SEQ",
									//Keywords
									"IF",
                                    "ELSE",
                                    "WHILE",
                                    "CHAR_DECL",
                                    "INTEGER_DECL",
                                    "REAL_DECL",
                                    "BOOLEAN_DECL",
                                    "STRING_DECL",	
									//Symbols
									"LPAREN",
                                    "RPAREN",
                                    "LBRACK",
                                    "RBRACK",
                                    "LBOX",
                                    "RBOX",
                                    "SEMICOLON",
                                    "COMMA",
                                    "DOT",
                                    "SCOPE",
									//Constants
									"CHAR_CONSTANT",
                                    "INT_CONSTANT",
                                    "REAL_CONSTANT",
                                    "BOOL_CONSTANT",
                                    "STRING_CONSTANT",
									//Service SubTypes
									"SERVICE_INVOKE",
                                    "SERVICE_ATTRIBUTE",
									//Rule Self Invoke
									"RULE_ENGINE",
                                    "INVOKE",
									//Object manager
									"OM",
									//General
									"DOLLAR",
                                    "SUBTYPE_ERROR"
                                };


        public CGrammarNode()
        {
            SetNodeType(GRAMMAR_NODE_TYPE.NODE_TYPE_INVALID);
            m_CurrentScopeIndex = 0;
        }

        //	Identify self
        public virtual void Identify(ref string szData)
        {
            ;
        }

        public virtual int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar,
            RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            return 0;
        }

        public virtual int GetLineNumber()
        {
            return 0;
        }

        public void SetNodeType(GRAMMAR_NODE_TYPE nodeType)
        {
            m_NodeType = nodeType;

        }

        public GRAMMAR_NODE_TYPE GetNodeType()
        {
            return m_NodeType;
        }

        //This will return the nested depth of the symbols---Felix
        public int GetScopeIndex()
        {
            return m_CurrentScopeIndex;

        }

        public void SetScopeIndex(int nSymTblScpIdx)
        {
            m_CurrentScopeIndex = nSymTblScpIdx;
        }
        public void TokenType_to_VariantType(RUL_TOKEN_TYPE token, RUL_TOKEN_SUBTYPE subtoken, ref VARIANT_TYPE vt)
        {
            switch (subtoken)
            {
                case RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL:
                    vt = VARIANT_TYPE.RUL_CHAR;
                    break;
                case RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL:
                    vt = VARIANT_TYPE.RUL_LONGLONG;
                    break;
                // Walt EPM 08sep08 - added
                case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL:
                    vt = VARIANT_TYPE.RUL_USHORT;
                    break;
                case RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL:
                    vt = VARIANT_TYPE.RUL_SHORT;
                    break;
                case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL:
                    vt = VARIANT_TYPE.RUL_UINT;
                    break;
                // end add  Walt EPM 08sep08
                case RUL_TOKEN_SUBTYPE.RUL_INT_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL:
                case RUL_TOKEN_SUBTYPE.RUL_LONG_DECL:
                    vt = VARIANT_TYPE.RUL_INT;
                    break;
                case RUL_TOKEN_SUBTYPE.RUL_REAL_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_REAL_DECL:
                    vt = VARIANT_TYPE.RUL_FLOAT;
                    break;
                case RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL:
                    vt = VARIANT_TYPE.RUL_DOUBLE;
                    break;
                case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL:
                    vt = VARIANT_TYPE.RUL_UNSIGNED_CHAR;
                    break;
            }
        }


    }

    public class CGrammarNodeVisitor
    {
        public virtual int visitCompoundExpression(CCompoundExpression pStatement, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitPrimaryExpression(CPrimaryExpression pStatement, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }


        public virtual int visitExpression(CExpression pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }


        public virtual int visitArrayExpression(CArrayExpression pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitComplexDDExpression(CComplexDDExpression pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitAssignment(CAssignmentStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitCompoundStatement(CCompoundStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitIterationStatement(CIterationStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitIterationStatement(CIterationDoWhileStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitIterationStatement(CIterationForStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitSelectionStatement(CSelectionStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitSwitchStatement(CSwitchStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitELSEStatement(CELSEStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitCASEStatement(CCASEStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitProgram(CProgram pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitStatement(CStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitBreakStatement(CBreakStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitReturnStatement(CReturnStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }


        public virtual int visitContinueStatement(CContinueStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitStatementList(CStatementList pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitDeclarations(CDeclarations pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitRuleService(CRuleServiceStatement pStatement, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitOMExpression(COMServiceExpression pExpression, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitFunctionExpression(FunctionExpression pExpression, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }

        public virtual int visitIFExpression(IFExpression pExpression, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)    //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return 0;
        }


    }
}
