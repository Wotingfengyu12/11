using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CExpression : CStatement
    {
        protected CExpression m_pExpression;
        public CExpression()
        {
            SetNodeType(GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION);
            m_pExpression = null;   //TSRPRASAD 09MAR2004 Fix the memory leaks
        }
        //	Identify self
        public override void Identify(ref string szData)
        {
            ;
        }

        //	Allo\w Visitors to do different operations on the node.
        public override int Execute(
            CGrammarNodeVisitor pVisitor,
            CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar,
            RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitExpression(this, pSymbolTable, ref pvar, AssignType);
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CExpParser expParser = new CExpParser();
            CToken pToken = null;
            //try
            {
                m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN);
                if (m_pExpression == null)
                {
                    //ADD_ERROR(C_IF_ERROR_MISSINGEXP);
                }

                //Munch a <;> 
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null
                    || pToken.IsEOS() == false)
                {
                    //throw (C_RS_ERROR_MISSINGSC);
                }


                return 0;
            }
            /*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
				plexAnal.SynchronizeTo(EXPRESSION, pSymbolTable);
			}
			return 0;
			*/
        }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return -1;
        }

        public CExpression GetExpression()
        {
            return m_pExpression;
        }

    }

}
