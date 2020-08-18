using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CReturnStatement : CStatement
    {
        int i32LineNumber;
        CExpression m_pExpression;//Anil Octobet 5 2005 for handling Method Calling Method

        public CReturnStatement()
        {

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
            //Anil Octobet 5 2005 for handling Method Calling Method
            return pVisitor.visitReturnStatement(
                this,
                pSymbolTable,
                ref pvar,
                AssignType);
            //	return VISIT_RETURN;
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                //Munch a <return>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || !pToken.IsRETURNStatement())
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }
                //Munch a <;>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON)//Anil Octobet 5 2005 for handling Method Calling Method
                {
                    //throw(C_UM_ERROR_INTERNALERR)//Commented by anil
                    //this is the case of Void return so no need to parse the futher satement			
                    m_pExpression = null;//Anil Octobet 5 2005 for handling Method Calling Method
                    return 1;

                }
                plexAnal.UnGetToken();

                //Added Anil Octobet 5 2005 for handling Method Calling Method
                //Return statement may be a Expression So Do Parse those Expression and Push it on to m_pExpression
                CExpParser expParser = new CExpParser();
                //try
                {
                    m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN);

                    if (m_pExpression == null)
                    {
                        //throw (C_AP_ERROR_MISSINGEXP);
                    }
                }
                /*
				catch (CRIDEError* perr)
				{
					pvecErrors.push_back(perr);
					plexAnal.SynchronizeTo(EXPRESSION, pSymbolTable);
				}
				*/
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

        public CExpression GetExpression()//Anil Octobet 5 2005 for handling Method Calling Method
        {
            return m_pExpression;
        }

        //This returns the last line in which this node has a presence...

    }
}
