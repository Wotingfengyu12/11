using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CSelectionStatement : CStatement
    {
        CExpression m_pExpression;
        CStatement m_pStatement;
        CELSEStatement m_pElse;

        public CSelectionStatement()
        {

        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += "IFStatement";
            szData += ">";

            szData += "<";
            szData += "Expression";
            szData += ">";
            if (m_pExpression != null)
                m_pExpression.Identify(ref szData);
            szData += "</";
            szData += "Expression";
            szData += ">";
            if (m_pStatement != null)
                m_pStatement.Identify(ref szData);

            szData += "</";
            szData += "IFStatement";
            szData += ">";

            if (m_pElse != null)
            {
                m_pElse.Identify(ref szData);
            }
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitSelectionStatement(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            {
                //Munch a <IF>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || !pToken.IsIFStatement())
                {
                    //throw(C_UM_ERROR_INTERNALERR);
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
                CExpParser expParser = new CExpParser();
                //try
                {
                    m_pExpression = expParser.ParseExpression(ref plexAnal, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_IF);
                    if (m_pExpression == null)
                    {
                        //ADD_ERROR(C_IF_ERROR_MISSINGEXP);
                    }
                }

                //Munch a <)>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RPAREN))
                {
                    //ADD_ERROR(C_IF_ERROR_MISSINGRP);
                    plexAnal.UnGetToken();
                }

                //Eat a Statement...
                CParserBuilder builder = new CParserBuilder();
                m_pStatement = (CStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);
                if (null != m_pStatement)
                {
                    int i32Ret = m_pStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                    if (i32Ret == 0)
                    {
                        //ADD_ERROR(C_IF_ERROR_MISSINGSTMT);
                    }
                }
                else
                {
                    //ADD_ERROR(C_IF_ERROR_MISSINGSTMT);
                }

                //See if you can snatch a "else"
                try
                {
                    m_pElse = (CELSEStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_SELECTION);
                    if (null != m_pElse)
                    {
                        m_pElse.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                    }
                }
                catch (Exception ex)
                {
                    string exinfo = ex.Message;
                    //return 0;
                }

                return 1;
            }

        }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return m_pStatement.GetLineNumber();
        }

        public CExpression GetExpression()
        {
            return m_pExpression;
        }

        public CStatement GetStatement()
        {
            return m_pStatement;
        }

        public CELSEStatement GetELSEStatement()
        {
            return m_pElse;
        }


    }


}
