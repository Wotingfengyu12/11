using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CEmptyStatement : CStatement
    {
        public CEmptyStatement()
        {

        }

        //	Identify self
        public override void Identify(ref string szData)
        {

        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            if ((object)pvar != null)   // stevev 19nov09 - make 'for(;;)' statement work
            {
                byte[] b = { 1 };
                pvar.SetValue(b, 0, VARIANT_TYPE.RUL_UNSIGNED_CHAR);      // stevev 19nov09 - make 'for(;;)' statement work
            }

            return VISIT_NORMAL;
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_SUBTYPE.RUL_SEMICOLON != pToken.GetSubType()))
                {
                    return 0;
                }
                return 1;
            }
            /*
			catch (CRIDEError* perr)
			{
				pvecErrors->push_back(perr);
				plexAnal->SynchronizeTo(EXPRESSION, pSymbolTable);
			}
			catch (...)
			{
				throw (C_UM_ERROR_UNKNOWNERROR);
			}
			return PARSE_FAIL;
			*/
        }

        public override int GetLineNumber()
        {
            return 0;
        }
    }

}
