using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class COMPOUND_DATA
    {
        public string m_szName;
        public string m_szAttribute;
    }

    public class CToken
    {
        protected RUL_TOKEN_TYPE m_Type;
        RUL_TOKEN_SUBTYPE m_SubType;
        public string m_pszLexeme;
        int m_nSymbolTableIndex;
        int m_i32constant_pool_idx;
        COMPOUND_DATA m_pCompound;
        int m_i32LineNo;
        string m_pszDDItemName;//Anil August 26 2005 For handling DD variable and Expression

        public bool m_bIsGlobal;   //whether the symbol is a DD item (not a method local)

        public bool m_bIsRoutineToken;//Anil Octobet 5 2005 for handling Method Calling Method
                               //To know whthet is routine ie, if this parameter has come when one method calls other
                               //So that need not  Execute the declaration list

        public bool m_bIsReturnToken; //Anil Octobet 5 2005 for handling Method Calling Method
                               //Just to make sure the the variable is reuturn toke
        int m_nSymbolTableScopeIndex; //SCR26200 Felix for handling Nested Depth of Symbol

        public CToken()
        {
            m_pszLexeme = null;
            m_pszDDItemName = null;//Anil August 26 2005 For handling DD variable and Expression
            ////m_pCompound = null;
            m_Type = RUL_TOKEN_TYPE.RUL_TYPE_NONE;
            m_SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;
            m_nSymbolTableIndex = -1;
            m_i32constant_pool_idx = -1;
            m_i32LineNo = -1;
            m_bIsGlobal = false;  //Vibhor 070705: Added
            m_bIsRoutineToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_bIsReturnToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_nSymbolTableScopeIndex = 0; ///SCR26200 /Felix for handling Nested Depth of Symbol
            m_pCompound = new COMPOUND_DATA();

        }

        public CToken(string szLexeme)
        {
            m_pszLexeme = szLexeme;
            m_pszDDItemName = null;//Added By Anil August 22 2005
            m_pCompound = null;
            m_Type = RUL_TOKEN_TYPE.RUL_TYPE_NONE;
            m_SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;
            m_nSymbolTableIndex = -1;
            m_i32constant_pool_idx = -1;
            m_i32LineNo = -1;
            m_bIsGlobal = false;  //Vibhor 070705: Added
            m_bIsRoutineToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_bIsReturnToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_nSymbolTableScopeIndex = 0; //SCR26200 Felix for handling Nested Depth of Symbol
        }

        public CToken(string szLexeme, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, int i32LineNo)
        {
            m_pszLexeme = szLexeme;
            m_pszDDItemName = null;//Added By Anil August 22 2005
            m_pCompound = null;

            m_Type = Type;
            m_SubType = SubType;
            m_nSymbolTableIndex = -1;
            m_i32constant_pool_idx = -1;
            m_i32LineNo = i32LineNo;
            m_bIsGlobal = false;  //Vibhor 070705: Added
            m_bIsRoutineToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_bIsReturnToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_nSymbolTableScopeIndex = 0; //SCR26200 Felix for handling Nested Depth of Symbol

        }

        public CToken(string szLexeme, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType, COMPOUND_DATA cmpData, int i32LineNo)
        {

            m_pszLexeme = szLexeme;
            m_pszDDItemName = null;//Added By Anil August 22 2005

            m_Type = Type;
            m_SubType = SubType;
            m_nSymbolTableIndex = -1;
            m_i32constant_pool_idx = -1;
            m_i32LineNo = i32LineNo;
            m_bIsGlobal = false;  //Vibhor 070705: Added

            m_pCompound = new COMPOUND_DATA();
            m_pCompound.m_szName = cmpData.m_szName;
            m_pCompound.m_szAttribute = cmpData.m_szAttribute;
            m_bIsReturnToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_bIsRoutineToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_nSymbolTableScopeIndex = 0; //SCR26200 Felix for handling Nested Depth of Symbol

        }

        public CToken(CToken token)
        {
            m_Type = token.m_Type;
            m_SubType = token.m_SubType;
            m_pszLexeme = token.m_pszLexeme;
            m_pszDDItemName = null;
            //Anil August 26 2005 For handling DD variable and Expression
            if (token.m_pszDDItemName != null)
            {
                m_pszDDItemName = token.m_pszDDItemName;
            }

            m_nSymbolTableIndex = token.m_nSymbolTableIndex;
            m_i32constant_pool_idx = token.m_i32constant_pool_idx;
            m_i32LineNo = token.m_i32LineNo;
            m_bIsGlobal = token.m_bIsGlobal;  //Vibhor 070705: Added
            m_bIsRoutineToken = false;//Anil Octobet 5 2005 for handling Method Calling Method
            m_bIsReturnToken = false;//Anil Octobet 5 2005 for handling Method Calling Method

            m_nSymbolTableScopeIndex = 0; //SCR26200 Felix for handling Nested Depth of Symbol

            if (token.m_pCompound != null)
            {
                m_pCompound = new COMPOUND_DATA();
                m_pCompound = token.m_pCompound;

            }

        }

        new public RUL_TOKEN_TYPE GetType()
        {
            return m_Type;
        }

        public RUL_TOKEN_SUBTYPE GetSubType()
        {
            return m_SubType;
        }

        public bool IsOperator()
        {
            if ((m_Type == RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR)
                || (m_Type == RUL_TOKEN_TYPE.RUL_LOGICAL_OPERATOR)
                || (m_Type == RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR)
                || (m_Type == RUL_TOKEN_TYPE.RUL_RELATIONAL_OPERATOR)
                )
            {
                return true;
            }
            return false;
        }

        public bool IsNumeric()
        {
            return (m_Type == RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT) ? true : false;
        }

        public bool IsConstant()
        {
            switch (m_SubType)
            {
                case RUL_TOKEN_SUBTYPE.RUL_REAL_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_BOOL_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_INT_CONSTANT:
                case RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT:
                    return true;
            }
            return false;
        }

        public bool IsVariable()
        {
            return ((m_Type == RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE)//||(m_Type == RUL_OBJECT_VARIABLE)
                    || (m_SubType == RUL_TOKEN_SUBTYPE.RUL_SERVICE_ATTRIBUTE)) ? true : false;
        }

        public bool IsAssignOp()
        {
            return (m_Type == RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR) ? true : false;
        }

        public bool IsWHILEStatement()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_WHILE:
                        return true;
                }
            }
            return false;
        }

        public bool IsDOStatement()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_DO:
                        return true;
                }
            }
            return false;
        }

        public bool IsFORStatement()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_FOR:
                        return true;
                }
            }
            return false;
        }

        public bool IsDeclaration()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_REAL_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_STRING_DECL:
                    //Added By Anil June 14 2005 --starts here
                    case RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL:
                    //Added By Anil June 14 2005 --Ends here
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL:
                        return true;

                }
            }
            return false;

        }

        public bool IsSelection()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_IF:
                    case RUL_TOKEN_SUBTYPE.RUL_ELSE:
                    case RUL_TOKEN_SUBTYPE.RUL_SWITCH:
                    case RUL_TOKEN_SUBTYPE.RUL_CASE:
                    case RUL_TOKEN_SUBTYPE.RUL_DEFAULT:
                        return true;
                }
            }
            return false;
        }

        public bool IsIFStatement()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_IF:
                        return true;
                }
            }
            return false;
        }

        public bool IsELSEStatement()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_ELSE:
                        return true;
                }
            }
            return false;
        }

        public bool IsIteration()
        {
            if (RUL_TOKEN_TYPE.RUL_KEYWORD == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_WHILE:
                    case RUL_TOKEN_SUBTYPE.RUL_FOR:
                    case RUL_TOKEN_SUBTYPE.RUL_DO:
                        return true;
                }
            }
            return false;
        }

        public bool IsCompound()
        {
            if (RUL_TOKEN_TYPE.RUL_SYMBOL == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_LBRACK:
                    case RUL_TOKEN_SUBTYPE.RUL_COLON:// For handling case
                        return true;
                }
            }
            return false;
        }

        public bool IsService()
        {
            if (RUL_TOKEN_TYPE.RUL_SERVICE == m_Type)
            {
                switch (m_SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_SERVICE_INVOKE:
                        return true;
                }
            }
            return false;
        }

        public string GetLexeme()
        {
            return m_pszLexeme;
        }

        public bool IsEOS()
        {
            if (m_SubType == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON)
                return true;
            return false;
        }

        public void SetSubType(RUL_TOKEN_SUBTYPE SubType)
        {
            m_SubType = SubType;
        }

        public bool IsSymbol()
        {
            if (m_Type == RUL_TOKEN_TYPE.RUL_SYMBOL)
                return true;
            return false;
        }

        public int GetSymbolTableIndex()
        {
            return m_nSymbolTableIndex;
        }

        public COMPOUND_DATA GetCompoundData()
        {
            return m_pCompound;
        }

        public void Identify(ref string szData)
        {
            szData += "<";
            szData += m_pszLexeme;
            szData += ">";
            COMPOUND_DATA cda = GetCompoundData();
            if (cda != null)
            {
                szData += cda.m_szName;
                szData += ",";
                szData += cda.m_szAttribute;

            }
            szData += "</";
            szData += m_pszLexeme;
            szData += ">";
        }

        public bool IsArrayVar()
        {
            if (m_Type == RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE)
            {
                return true;
            }
            return false;
        }

        public bool IsOMToken()
        {
            if ((m_Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && (m_SubType == RUL_TOKEN_SUBTYPE.RUL_OM))
            {
                return true;
            }
            return false;
        }

        public bool IsFunctionToken()
        {
            if ((m_Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && (m_SubType == RUL_TOKEN_SUBTYPE.RUL_FUNCTION))
            {
                return true;
            }
            return false;
        }

        public bool IsBREAKStatement()
        {
            if ((m_Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && (m_SubType == RUL_TOKEN_SUBTYPE.RUL_BREAK))
            {
                return true;
            }
            return false;
        }

        public bool IsCONTINUEStatement()
        {
            if ((m_Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && (m_SubType == RUL_TOKEN_SUBTYPE.RUL_CONTINUE))
            {
                return true;
            }
            return false;
        }

        public bool IsRETURNStatement()
        {
            if ((m_Type == RUL_TOKEN_TYPE.RUL_KEYWORD) && (m_SubType == RUL_TOKEN_SUBTYPE.RUL_RETURN))
            {
                return true;
            }
            return false;
        }

        public int GetLineNumber()
        {
            return m_i32LineNo;
        }

        /*Vibhor 140705: Start of Code*/

        public bool IsDDItem()
        {
            return ((m_Type == RUL_TOKEN_TYPE.RUL_DD_ITEM) ? true : false);
        }

        public void SetType(RUL_TOKEN_TYPE type)
        {
            m_Type = type;
        }

        /*Vibhor 140705: End of Code*/


        //Added By Anil July 28 2005 --starts here
        //Added for the Global Symbol takel Though it is not that Clan Solution
        public void SetLexeme(string szLexeme)
        {
            m_pszLexeme = szLexeme;
        }

        public void SetDDItemName(string szComplexDDExpre)
        {
            m_pszDDItemName = szComplexDDExpre;
        }

        public string GetDDItemName()
        {
            return m_pszDDItemName;
        }

        public void SetSymbolTableIndex(int nSymTblIdx)
        {
            m_nSymbolTableIndex = nSymTblIdx;
        }

        public void SetSymbolTableScopeIndex(int nSymTblScpIdx)
        {
            m_nSymbolTableScopeIndex = nSymTblScpIdx;
        }

        public int GetSymbolTableScopeIndex()   //SCR26200 Felix
        {
            return m_nSymbolTableScopeIndex;
        }

        public int GetConstantIndex()
        {
            return m_i32constant_pool_idx;
        }

        public void SetConstantIndex(int i32constant_pool_idx)
        {
            m_i32constant_pool_idx = i32constant_pool_idx;
        }

    }
}
