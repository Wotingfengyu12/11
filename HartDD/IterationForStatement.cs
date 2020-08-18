using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CIterationForStatement : CStatement
    {
        CExpression m_pExpression;
        CAssignmentStatement m_pExpressionStatement;

        CAssignmentStatement m_pInitializationStatement;
        CAssignmentStatement m_pIncrementStatement;
        CExpression m_pIncrementExpression;
        CStatement m_pStatement;

        GRAMMAR_NODE_TYPE incrementNodeType;
        GRAMMAR_NODE_TYPE expressionNodeType;

        public CIterationForStatement()
        {

        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += "FORStatement";
            szData += ">";

            if (m_pStatement != null)
                m_pStatement.Identify(ref szData);

            szData += "<";
            szData += "Expression";
            szData += ">";
            if (m_pExpression != null)
                m_pExpression.Identify(ref szData);
            szData += "</";
            szData += "Expression";
            szData += ">";
            szData += "</";

            szData += "FORStatement";
            szData += ">";

        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitIterationStatement(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                //Munch a <FOR>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || !pToken.IsFORStatement())
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }

                //Munch a <(>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || !(pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_LPAREN))
                {
                    //ADD_ERROR(C_WHILE_ERROR_MISSINGLP);
                    plexAnal.UnGetToken();
                }

                //Munch a Initialization Statement...
                CParserBuilder builder = new CParserBuilder();
                m_pInitializationStatement = (CAssignmentStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);
                if (null != (m_pInitializationStatement))
                {
                    m_pInitializationStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                }
                else
                {
                    //Munch a <;>
                    if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                        || pToken == null || !(pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON))
                    {
                        //ADD_ERROR(C_WHILE_ERROR_MISSINGLP);
                        plexAnal.UnGetToken();
                    }
                }

                //Munch & Parse the expression.
                //we got to give the expression string to the expression parser.
                //	
                CParserBuilder builder2 = new CParserBuilder();
                CExpParser expParser = new CExpParser();
                CGrammarNode pNode;

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

                        m_pExpressionStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable, STATEMENT_TYPE.STMT_asic);
                    }
                    else if (expressionNodeType == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                    {
                        expressionNodeType = GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION;
                        m_pExpression
                            = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_FOR);

                        if (m_pExpression == null)
                        {
                            //ADD_ERROR(C_WHILE_ERROR_MISSINGEXP);
                        }
                    }
                    else if (expressionNodeType == GRAMMAR_NODE_TYPE.NODE_TYPE_INVALID)
                    {
                        pToken = null;

                        //try
                        {
                            if ((CLexicalAnalyzer.LEX_FAIL != (plexAnal.LookAheadToken(ref pToken))) && pToken != null)
                            {
                                if ((pToken.GetType() == RUL_TOKEN_TYPE.RUL_SYMBOL) && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON))
                                {
                                    // This code is to handle for(;;) for Yokagawa EJX
                                    expressionNodeType = GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION;
                                    CToken pToken2 = new CToken("1");
                                    pToken2.SetType(RUL_TOKEN_TYPE.RUL_NUMERIC_CONSTANT);
                                    m_pExpression = new CPrimaryExpression(pToken2);
                                }
                            }
                        }
                        /*
						catch (...)
						{
						}
						*/
                    }
                }
                else
                {
                    //ADD_ERROR(C_WHILE_ERROR_MISSINGSTMT);
                }

                //Munch a <;>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || !(pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON))
                {
                    //ADD_ERROR(C_WHILE_ERROR_MISSINGLP);
                    plexAnal.UnGetToken();
                }

                //Munch a Increment Statement...
                pNode = builder2.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);

                m_pIncrementExpression = null;
                m_pIncrementStatement = null;
                if (null != pNode)
                {
                    incrementNodeType = pNode.GetNodeType();
                    if (incrementNodeType == GRAMMAR_NODE_TYPE.NODE_TYPE_ASSIGN)
                    {
                        incrementNodeType = GRAMMAR_NODE_TYPE.NODE_TYPE_ASSIGN;
                        m_pIncrementStatement = (CAssignmentStatement)pNode;

                        m_pIncrementStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable, STATEMENT_TYPE.STMT_ASSIGNMENT_FOR);
                    }
                    else if (incrementNodeType == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                    {
                        incrementNodeType = GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION;
                        m_pIncrementExpression
                            = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_FOR);

                        if (m_pIncrementExpression == null)
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
                    || pToken == null || !(pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_RPAREN))
                {
                    //ADD_ERROR(C_WHILE_ERROR_MISSINGLP);
                    plexAnal.UnGetToken();
                }

                //Munch the statement
                CParserBuilder builder3 = new CParserBuilder();
                m_pStatement = (CStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);
                if (null != (m_pStatement))
                {
                    m_pStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                }
                else
                {
                    //ADD_ERROR(C_WHILE_ERROR_MISSINGSTMT);
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

        public CStatement GetStatement()
        {
            return m_pStatement;
        }

        public CAssignmentStatement GetInitializationStatement()
        {
            return m_pInitializationStatement;
        }

        public CAssignmentStatement GetIncrementStatement()
        {
            return m_pIncrementStatement;
        }

        public CExpression GetIncrementExpression()
        {
            return m_pIncrementExpression;
        }

        public GRAMMAR_NODE_TYPE GetIncrementNodeType()
        {
            return incrementNodeType;
        }

        public GRAMMAR_NODE_TYPE GetExpressionNodeType()
        {
            return expressionNodeType;
        }

    }
}
