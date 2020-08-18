using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CStatement : CGrammarNode
    {
        public const int MAX_CASE_STATEMENTS = 255;

        public CStatement()
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
            return 0;
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            return 0;
        }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return -1;
        }

    }

    public class CStatementList : CGrammarNode
    {
        List<CStatement> m_stmtList;

        public CStatementList()
        {
            m_stmtList = new List<CStatement>();

        }
        //	Identify self

        public override void Identify(ref string szData)
        {
            string szNum1;
            string szNum2;
            szData += "<StatementList>";
            for (int i = 0; i < m_stmtList.Count; i++)
            {
                szNum1 = String.Format("<Statement%02d>", i);
                szNum2 = String.Format("</Statement%02d>", i);
                szData += szNum1;
                m_stmtList[i].Identify(ref szData);
                szData += szNum2;
            }
            szData += "</StatementList>";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitStatementList(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CParserBuilder builder = new CParserBuilder();
            CGrammarNode pStmt = null;

            int i = 0;

            //try
            {
                while (true)
                {
                    i++;

                    if(i == 0xb)
                    {
                        ;
                    }

                    pStmt = builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);
                    if (pStmt == null)
                    {
                        if (plexAnal.IsEndOfSource())
                        {
                            return 1;
                        }
                        else
                        {
                            CToken pToken = null;

                            if ((CLexicalAnalyzer.LEX_FAIL != plexAnal.GetNextToken(ref pToken, ref pSymbolTable)) && pToken != null)
                            {
                                if (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON)
                                {
                                    continue;
                                }
                                else
                                {
                                    plexAnal.UnGetToken();
                                }
                            }

                            return 0;
                        }
                    }
                    pStmt.SetScopeIndex(plexAnal.GetSymbolTableScopeIndex());         //SCR26200 Felix
                    m_stmtList.Add((CStatement)pStmt);
                    pStmt.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                }//end of while loop

                //return 1;
            }
            /*
			catch (...)
			{
				return PARSE_FAIL;
			}
			*/
        }

        public override int GetLineNumber()
        {
            return m_stmtList[m_stmtList.Count - 1].GetLineNumber();
        }

        public List<CStatement> GetStmtList()
        {
            return m_stmtList;
        }

        public bool AddStatement(CGrammarNode pStmt)
        {
            m_stmtList.Add((CStatement)pStmt);
            return true;
        }



    }

}
