using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CCASEStatement : CStatement
	{
		CExpression m_pExpression;
		CStatementList m_pStatementList;
	bool m_bIsDefaultCase;

		public CCASEStatement()
		{
			m_pStatementList = new CStatementList();
		}

		//	Identify self
		public override void Identify(ref string szData)
		{
			if (m_bIsDefaultCase)
			{
				szData += "<";
				szData += "DEFAULT";
				szData += ">";
			}
			else
			{
				szData += "<";
				szData += "CASEStatement";
				szData += ">";
			}


			if (m_pExpression != null)
			{
				m_pExpression.Identify(ref szData);
			}

			if (m_pStatementList != null)
			{
				m_pStatementList.Identify(ref szData);
			}

			if (m_bIsDefaultCase)
			{
				szData += "</";
				szData += "DEFAULT";
				szData += ">";
			}
			else
			{
				szData += "</";
				szData += "CASEStatement";
				szData += ">";
			}
		}

		//	Allow Visitors to do different operations on the node.
		public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
			ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
		{
			return pVisitor.visitCASEStatement(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
		}

		//	Create as much of the parse tree as possible.
		public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
		{
			//Eat a Statement...
			CToken pToken = null;
			//try
			{
				//Munch a <CASE>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_CASE)
					)
				{
					if (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_DEFAULT)
					{
						//throw (C_UM_ERROR_INTERNALERR);
					}
					else
					{
						m_bIsDefaultCase = true;
					}
				}

				//Munch & Parse the expression.
				//we got to give the expression string to the expression parser.
				//	
				if (m_bIsDefaultCase != true)
				{
					CExpParser expParser = new CExpParser();
					//try
					{
						m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_CASE);
						if (m_pExpression == null)
						{
							//ADD_ERROR(C_IF_ERROR_MISSINGEXP);
						}
					}
					/*
					catch (CRIDEError* perr)
					{
						pvecErrors.push_back(perr);
						plexAnal.SynchronizeTo(EXPRESSION, ref pSymbolTable);
					}
					*/
				}

				//Munch a <:>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_COLON))
				{
					//throw (C_UM_ERROR_INTERNALERR);
				}

				if ((CLexicalAnalyzer.LEX_FAIL != plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					&& pToken != null && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_CASE))
				{
					plexAnal.UnGetToken();
					m_pStatementList = null;
					return 1;
				}
				plexAnal.UnGetToken();

				//Look for a statement
				m_pStatementList = new CStatementList();
				/*		if((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken,pSymbolTable)) 
							|| !pToken
							|| (pToken.GetSubType() != RUL_LBRACK)
							)
						{
							plexAnal.UnGetToken();
						}
						else
						{
							bCompoundStatement = true;
						}
						DELETE_PTR(pToken);*/

				bool bCompoundStatement = false;
				int iBrackCount = 0;
				while (true)
				{
					if (CLexicalAnalyzer.LEX_FAIL != plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					{
						if ((pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_LBRACK) && (bCompoundStatement))
						{
							bCompoundStatement = true;
							iBrackCount++;

						}
						if ((pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_RBRACK) && (bCompoundStatement))
						{
							if (bCompoundStatement == true)
							{
								iBrackCount--;
								if (iBrackCount == 0)
								{
									bCompoundStatement = false;
								}
							}
						}

						if ((pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_CASE) || (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_DEFAULT))
						{
							plexAnal.UnGetToken();
							break;
						}

						plexAnal.UnGetToken();
					}

					CGrammarNode pStmt = null;
					CParserBuilder builder = new CParserBuilder();

					pStmt = builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);
					if (pStmt == null)
					{
						if (plexAnal.IsEndOfSource())
						{
							return 1;
						}
						else
						{
							return 0;
						}
					}
					m_pStatementList.AddStatement((CStatement)pStmt);
					int i32Ret = pStmt.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
					if (i32Ret == 0)
					{
						//ADD_ERROR(C_ES_ERROR_MISSINGSTMT);
					}
				}
				return 1;

			}
			/*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
				plexAnal.MovePast(
					RUL_SYMBOL,
					RUL_SEMICOLON,
					pSymbolTable);
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
			return m_pStatementList.GetLineNumber();
		}

		public CStatementList GetStatement()
		{
			return m_pStatementList;
		}

		public CExpression GetExpression()
		{
			return m_pExpression;
		}

		public bool IsDefaultStatement()
		{
			return m_bIsDefaultCase;
		}
	}
}
