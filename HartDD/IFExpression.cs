using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class IFExpression : CExpression
	{
		CExpression m_pIfExpression;
		CExpression m_pTrueExpression;
		CExpression m_pFalseExpression;
		
		//	Identify self
		public override void Identify(ref string szData)
		{
			;
		}

		//	Allow Visitors to do different operations on the node.
		public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
			ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
		{
			return pVisitor.visitIFExpression(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
		}

		//	Create as much of the parse tree as possible.
		public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
		{
			CExpParser expParser = new CExpParser();
			CToken pToken = null;
			//try
			{
				m_pIfExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN, false);
				if (m_pIfExpression == null)
				{
					//throw (C_IF_ERROR_MISSINGEXP);
				}

				//Munch a <?> 
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable)) || pToken == null
					|| !((pToken.GetType() == RUL_TOKEN_TYPE.RUL_SYMBOL) && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_QMARK)))
				{
					//throw (C_RS_ERROR_MISSINGSC);
				}

				m_pTrueExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN, false);
				if (m_pTrueExpression == null)
				{
					//throw (C_RS_ERROR_MISSINGSC);
				}

				//Munch a <:> 
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable)) || pToken == null
					|| !((pToken.GetType() == RUL_TOKEN_TYPE.RUL_SYMBOL) && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COLON)))
				{
					//throw (C_RS_ERROR_MISSINGSC);
				}

				m_pFalseExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN, false);//Vibhor 110205: Changed from EXPR_FOR
				if (m_pFalseExpression == null)
				{
					//throw (C_RS_ERROR_MISSINGSC);
				}

				//Munch a <;> 
				/*		if((LEX_FAIL == plexAnal.GetNextToken(&pToken,pSymbolTable)) 
							|| !pToken
							|| !pToken.IsEOS())
						{
							throw(C_RS_ERROR_MISSINGSC);
						}*/

				return 0;
			}
			/*
			catch (...)
			{
				return PARSE_FAIL;
			}
			return 0;
			*/

			}

		//This returns the last line in which this node has a presence...
		public new virtual int GetLineNumber()
		{
			return -1;
		}

		public void GetExpressions(ref CExpression pIfExpression, ref CExpression pTrueExpression, ref CExpression pFalseExpression )
		{
			pIfExpression = m_pIfExpression;
			pTrueExpression = m_pTrueExpression;
			pFalseExpression = m_pFalseExpression;
		}


	}

}
