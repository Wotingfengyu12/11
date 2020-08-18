using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CCompoundStatement : CStatement
	{
		CStatementList m_pStmtList;

		public CCompoundStatement()
		{

		}

		//	Identify self
		public override void Identify(ref string szData)
		{
			szData += "<";
			szData += "CompoundStatement";
			szData += ">";

			if (m_pStmtList != null)
				m_pStmtList.Identify(ref szData);

			szData += "</";
			szData += "CompoundStatement";
			szData += ">";

		}

		//	Allow Visitors to do different operations on the node.
		public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
			ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
		{
			return pVisitor.visitCompoundStatement(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
		}

		//	Create as much of the parse tree as possible.
		public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
		{
			CToken pToken = null;

			//try
			{
				//Munch a <{>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_LBRACK))
				{
					if (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_COLON)
					{
						//throw (C_UM_ERROR_INTERNALERR);
					}
				}

				//Munch List of statments...
				//try
				{
					m_pStmtList = new CStatementList();
					m_pStmtList.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
				}
				/*
				catch (CRIDEError* perr)
				{
					pvecErrors.push_back(perr);
					plexAnal.MoveTo(
						RUL_SYMBOL,
						RUL_RBRACK,
						pSymbolTable);
				}
				*/
				//Munch a <}>
				if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
					|| pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RBRACK))
				{
					//ADD_ERROR(C_CS_ERROR_MISSINGRBRACK);
					plexAnal.UnGetToken();
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
			return m_pStmtList.GetLineNumber();
		}

		public CStatementList GetStatementList()
		{
			return m_pStmtList;
		}


	}
}
