using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CAssignmentStatement : CStatement
    {
        CToken m_pVariable;
        CExpression m_pArrayExp;
        COMServiceExpression m_pOMExp;
        bool m_bLvalueIsArray;
        RUL_TOKEN_SUBTYPE m_AssignType;
        CExpression m_pComplexDDExp;//Added By Anil August 23 2005
        bool m_bLvalueIsComplexDD;//23 2005 Handling DD variable and Expression
        CExpression m_pExpression;

        public CAssignmentStatement()
        {
            SetNodeType(GRAMMAR_NODE_TYPE.NODE_TYPE_ASSIGN);
            m_pExpression = new CExpression();
            m_pComplexDDExp = new CExpression();
            m_pArrayExp = new CExpression();
            m_pOMExp = new COMServiceExpression();
            m_pVariable = new CToken();
        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += szTokenSubstrings[(int)RUL_TOKEN_SUBTYPE.RUL_ASSIGN];
            szData += ">";
            if (m_pVariable != null)
                m_pVariable.Identify(ref szData);
            else if (m_pArrayExp != null)
                m_pArrayExp.Identify(ref szData);
            else if (m_pOMExp != null)
                m_pOMExp.Identify(ref szData);
            m_pExpression.Identify(ref szData);
            szData += "</";
            szData += szTokenSubstrings[(int)RUL_TOKEN_SUBTYPE.RUL_ASSIGN];
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitAssignment(
                this,
                pSymbolTable,
                ref pvar,
                AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                CExpParser expParser;
                //Munch a <Var>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable)) || pToken == null)
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }
                if (pToken.IsArrayVar())
                {
                    plexAnal.UnGetToken();
                    expParser = new CExpParser();
                    m_pArrayExp = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_LVALUE);
                    m_bLvalueIsArray = true;
                    m_pVariable = null;
                    m_pOMExp = null;
                    m_pComplexDDExp = null;//Added By Anil August 23 2005

                }
                //Added By Anil August 4 2005 --starts here
                //For Handlin the DD variable and Expressions
                else if (pToken.IsDDItem())
                {
                    plexAnal.UnGetToken();
                    expParser = new CExpParser();
                    m_pComplexDDExp = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_LVALUE);
                    m_bLvalueIsComplexDD = true;
                    m_pArrayExp = null;
                    m_pVariable = null;
                    m_pOMExp = null;

                }
                //Added By Anil August 4 2005 --Ends here
                else if (pToken.IsVariable())
                {
                    m_pVariable = pToken;
                    m_pArrayExp = null;
                    m_pOMExp = null;
                    m_pComplexDDExp = null;//Anil August 23 2005

                    //DELETE_PTR(pToken);
                    //todo walter
                }
                else if (pToken.IsOMToken())
                {
                    //do something...
                    plexAnal.UnGetToken();
                    m_pOMExp = new COMServiceExpression();
                    m_pOMExp.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                    m_pVariable = null;
                    m_pArrayExp = null;
                    m_pComplexDDExp = null;//Anil August 23 2005

                }
                else
                {
                    //throw (C_AP_ERROR_LVALUE);
                }

                //DELETE_PTR(pToken);  //todo walter
                pToken = null;
                //Munch a <=> or <*=>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null
                    || !pToken.IsAssignOp())
                {
                    //throw (C_AP_ERROR_MISSINGEQ);
                }
                m_AssignType = pToken.GetSubType();

                //Munch & Parse the expression.
                //we got to give the expression string to the expression parser.
                expParser = new CExpParser();
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
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_SUBTYPE.RUL_SEMICOLON != pToken.GetSubType()))
                {
                    //throw (C_AP_ERROR_MISSINGSC);
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

        public int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable, STATEMENT_TYPE stmt_type)
        {
            CToken pToken = null;
            //try
            {
                CExpParser expParser = new CExpParser();
                //Munch a <Var>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable)) || pToken == null)
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }
                if (pToken.IsVariable())
                {
                    m_pVariable = pToken;
                    m_pArrayExp = null;
                    m_pOMExp = null;
                    m_pComplexDDExp = null;//Anil August 23 2005
                }
                else if (pToken.IsArrayVar())
                {
                    plexAnal.UnGetToken();
                    expParser = new CExpParser();
                    m_pArrayExp = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_LVALUE);
                    m_bLvalueIsArray = true;
                    m_pVariable = null;
                    m_pOMExp = null;
                    m_pComplexDDExp = null;//Anil August 23 2005

                }
                //Added By Anil August 4 2005 --starts here
                //Handling DD variable and Expressions
                else if (pToken.IsDDItem())
                {
                    plexAnal.UnGetToken();
                    expParser = new CExpParser();
                    m_pComplexDDExp = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_LVALUE);
                    m_bLvalueIsArray = true;
                    m_pVariable = null;
                    m_pOMExp = null;
                    m_pArrayExp = null;
                }
                //Added By Anil August 4 2005 --Ends here
                else if (pToken.IsOMToken())
                {
                    //do something...
                    plexAnal.UnGetToken();
                    m_pOMExp = new COMServiceExpression();
                    m_pOMExp.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                    m_pVariable = null;
                    m_pArrayExp = null;
                }
                else
                {
                    //throw (C_AP_ERROR_LVALUE);
                }

                pToken = null;
                //Munch a <=> or <*=>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || !pToken.IsAssignOp())
                {
                    //throw (C_AP_ERROR_MISSINGEQ);
                }
                m_AssignType = pToken.GetSubType();

                //Munch & Parse the expression.
                //we got to give the expression string to the expression parser.
                expParser = new CExpParser();
                //try
                {
                    if (stmt_type == STATEMENT_TYPE.STMT_ASSIGNMENT_FOR)
                    {
                        m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_WHILE);
                    }
                    else
                    {
                        m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN);
                    }

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
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_SUBTYPE.RUL_SEMICOLON != pToken.GetSubType()))
                {
                    if ((RUL_TOKEN_SUBTYPE.RUL_RBRACK != pToken.GetSubType()) && (stmt_type != STATEMENT_TYPE.STMT_ASSIGNMENT_FOR))
                    {
                        //throw (C_AP_ERROR_MISSINGSC);
                    }
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

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            int i32LineNumber = 0;
            if (m_pArrayExp != null)
            {
                i32LineNumber = m_pArrayExp.GetLineNumber();
            }
            else if (m_pOMExp != null)
            {
                i32LineNumber = m_pOMExp.GetLineNumber();
            }
            else if (m_pExpression != null)
            {
                i32LineNumber = m_pExpression.GetLineNumber();
            }

            return i32LineNumber;
        }

        public CToken GetVariable()
        {
            return m_pVariable;
        }

        public CExpression GetArrayExp()
        {
            return m_pArrayExp;
        }

        public CExpression GetExpression()
        {
            return m_pExpression;
        }

        public CExpression GetOMExpression()
        {
            return m_pOMExp;
        }

        public CExpression GetComplexDDExp()//Anil August 23 2005 for Handling DD variable and Expression
        {
            return m_pComplexDDExp;
        }

        public RUL_TOKEN_SUBTYPE GetAssignmentType()
        {
            return m_AssignType;
        }
    }
}
