using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CProgram : CGrammarNode
    {
        CDeclarations m_pDeclarations;
        CStatementList m_pStmtList;

        public CProgram()
        {

        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<Program>";
            m_pDeclarations.Identify(ref szData);
            m_pStmtList.Identify(ref szData);
            szData += "</Program>";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            int nReturn = pVisitor.visitProgram(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
            /*
            if (nReturn == VISIT_ERROR)// emerson april2013
            {
                throw (C_UM_ERROR_INTERNALERR);
            }
            */
            return nReturn;
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            int i32Ret = 0;
            CToken pToken = null;
            //try
            {
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_LBRACK))
                {
                    //DELETE_PTR(pToken);//clean up memory even on errors
                    //throw (C_UM_ERROR_INTERNALERR);
                }

                m_pDeclarations = new CDeclarations();
                i32Ret = m_pDeclarations.CreateParseSubTree(ref plexAnal, ref pSymbolTable);

                m_pStmtList = new CStatementList();
                i32Ret = m_pStmtList.CreateParseSubTree(ref plexAnal, ref pSymbolTable);

                if (0 == i32Ret)
                {
                    if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                        || pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RBRACK))
                    {

                        //DELETE_PTR(pToken);//clean up memory, even on errors
                        //throw (C_UM_ERROR_INTERNALERR);
                        return i32Ret;
                    }
                    else
                    {
                        return 1;
                    }
                }
                return 0;
            }
            /*
			catch (CRIDEError* perr)
			{
				//if nobody has bothered to catch the error till now,
				//what else can be done but to just eat it and keep quiet...
				pvecErrors.push_back(perr);
			}
			catch (...)
			{
				throw (C_UM_ERROR_UNKNOWNERROR);
			}
			return i32Ret;
			*/
            }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return m_pStmtList.GetLineNumber();
        }

        public CDeclarations GetDeclarations()
        {
            return m_pDeclarations;
        }

        public CStatementList GetStatementList()
        {
            return m_pStmtList;
        }

	}
}
