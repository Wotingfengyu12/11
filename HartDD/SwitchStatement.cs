using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CSwitchStatement : CStatement
	{
		CExpression m_pExpression;
		CAssignmentStatement m_pExpressionStatement;

		//CStatement*		m_pExpStatement; Commented by TSRPrasad 09MAR2004 to fix memory leaks
		CStatement m_pStatement;
		CCASEStatement[] m_pCase = new CCASEStatement[MAX_CASE_STATEMENTS];
		CCASEStatement m_pDefaultCase;
		int m_iNumberOfCasesPresent;
		bool m_bIsDefaultPresent;

		GRAMMAR_NODE_TYPE expressionNodeType;

		public CSwitchStatement()
		{
			;
		}


		public override void Identify(ref string szData)
		{
			szData += "<";
			szData += "SWITCHStatement";
			szData += ">";

			szData += "</";
			szData += "SWITCHStatement";
			szData += ">";
		}

		//	Allow Visitors to do different operations on the node.
		public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
			ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
		{
			return pVisitor.visitSwitchStatement(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
		}

		//	Create as much of the parse tree as possible.
		public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
		{
			CToken pToken = null;
			//try
			{
				//Munch a <SWITCH>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_SWITCH))
				{
					//throw (C_UM_ERROR_INTERNALERR);
				}

				//Munch a <(>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_LPAREN))
				{
					//ADD_ERROR(C_IF_ERROR_MISSINGLP);
					plexAnal.UnGetToken();
				}

				//Munch & Parse the expression.
				//we got to give the expression string to the expression parser.
				//	
				CParserBuilder builder2 = new CParserBuilder();
				CExpParser expParser = new CExpParser();
				CGrammarNode pNode = null;

				pNode = builder2.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);

				m_pExpression = null;
				m_pExpressionStatement = null;
				if (null != pNode)
				{
					expressionNodeType = pNode.GetNodeType();
					if (expressionNodeType == GRAMMAR_NODE_TYPE.NODE_TYPE_ASSIGN)
					{
						expressionNodeType = GRAMMAR_NODE_TYPE.NODE_TYPE_ASSIGN;
						m_pExpressionStatement = (CAssignmentStatement)pNode;

						m_pExpressionStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable, STATEMENT_TYPE.STMT_ASSIGNMENT_FOR);
					}
					else if (expressionNodeType == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
					{
						expressionNodeType = GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION;
						m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_FOR);

						if (m_pExpression == null)
						{
							//ADD_ERROR(C_WHILE_ERROR_MISSINGEXP);
						}
					}
				}
				else
				{
					//ADD_ERROR(C_WHILE_ERROR_MISSINGSTMT);
				}

				//Munch a <)>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RPAREN))
				{
					//ADD_ERROR(C_IF_ERROR_MISSINGRP);
					plexAnal.UnGetToken();
				}

				//Munch a <{>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_LBRACK))
				{
					//ADD_ERROR(C_IF_ERROR_MISSINGRP);
					plexAnal.UnGetToken();
				}

				//See if you can snatch a "case"
				CParserBuilder builder = new CParserBuilder();
				int iNumberOfCaseStatements = 0;
				m_pCase[iNumberOfCaseStatements] = (CCASEStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_SELECTION);
				while(null != (m_pCase[iNumberOfCaseStatements]))
				{
					m_pCase[iNumberOfCaseStatements].CreateParseSubTree(ref plexAnal, ref pSymbolTable);
					if (m_pCase[iNumberOfCaseStatements].IsDefaultStatement())
					{
						m_bIsDefaultPresent = true;
						m_pDefaultCase = m_pCase[iNumberOfCaseStatements];
					}
					else
					{
						iNumberOfCaseStatements++;
					}
					m_pCase[iNumberOfCaseStatements] = (CCASEStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_SELECTION);
				}
				m_iNumberOfCasesPresent = iNumberOfCaseStatements;

				//Munch a <}>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RBRACK))
				{
					//ADD_ERROR(C_IF_ERROR_MISSINGRP);
					plexAnal.UnGetToken();
				}

				/* VMKP Commented on 140404,  For INOR device 
					Sensor selection method is crashing with this Fix */
				/*	if (pNode)	//TSRPRASAD 09MAR2004 Fix the memory leaks
					{
						delete pNode;
						pNode = null;
					}*/
				/* VMKP Commented on 140404 */

				return 1;
			}
			/*
			catch (CRIDEError perr)
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
			return m_pStatement.GetLineNumber();
		}


		public CExpression GetExpression()
		{
			return m_pExpression;
		}

		public CAssignmentStatement GetExpressionStatement()
		{
			return m_pExpressionStatement;
		}

		public GRAMMAR_NODE_TYPE GetExpressionNodeType()
		{
			return expressionNodeType;
		}

		public CStatement GetStatement()
		{
			return m_pStatement;
		}

		public CCASEStatement GetCaseStatement(int iIndex)
		{
			if (iIndex > m_iNumberOfCasesPresent - 1)
			{
				return null;
			}
			else
			{
				return m_pCase[iIndex];
			}
		}

		public int GetNumberOfCaseStatements()
		{
			return m_iNumberOfCasesPresent;
		}

		public bool IsDefaultPresent()
		{
			return m_bIsDefaultPresent;
		}

		public CCASEStatement GetDefaultStatement()
		{
			return m_pDefaultCase;
		}


    }
}
