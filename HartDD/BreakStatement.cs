using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CBreakStatement : CStatement
    {
        int i32LineNumber;

        public CBreakStatement()
        {
            i32LineNumber = 0;
        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += "BREAK";
            szData += ">";
            szData += "</";
            szData += "BREAK";
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_BREAK;
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                //Munch a <break>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || !pToken.IsBREAKStatement())
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }
                //Munch a <;>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_SEMICOLON)
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }
                return 1;
            }
            /*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
				plexAnal.SynchronizeTo(EXPRESSION, pSymbolTable);
			}
			catch (...)
			{
				throw (C_UM_ERROR_UNKNOWNERROR);
			}
			return PARSE_FAIL;
			*/
        }

        public int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, CSymbolTable pSymbolTable, STATEMENT_TYPE stmt_type)
        {
            return 1;
        }

        public override int GetLineNumber()
        {
            return i32LineNumber;
        }

    }

}
