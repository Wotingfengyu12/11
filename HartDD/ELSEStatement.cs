using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CELSEStatement : CStatement
    {
        CStatement m_pStatement;

        public CELSEStatement()
        {

        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += "ELSEStatement";
            szData += ">";

            if (m_pStatement != null)
            {
                m_pStatement.Identify(ref szData);
            }
            szData += "</";
            szData += "ELSEStatement";
            szData += ">";

        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitELSEStatement(
                this,
                pSymbolTable,
                ref pvar,
                AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;

            if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                 || pToken == null || !pToken.IsELSEStatement())
            {
                //throw (C_UM_ERROR_INTERNALERR);
            }

            //Look for a statement
            CParserBuilder builder = new CParserBuilder();
            m_pStatement = (CStatement)builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_asic);
            if (null != (m_pStatement))
            {
                int i32Ret = m_pStatement.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                if (i32Ret == 0)
                {
                    //ADD_ERROR(C_ES_ERROR_MISSINGSTMT);
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        public override int GetLineNumber()
        {
            return m_pStatement.GetLineNumber();
        }

        public CStatement GetStatement()
        {
            return m_pStatement;
        }


    }
}