using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CPrimaryExpression : CExpression
    {
        CToken m_pToken;
        public CPrimaryExpression()
        {
            m_pToken = new CToken();
        }

        public CPrimaryExpression(CToken pToken)
        {
            m_pToken = pToken;
        }

        public override void Identify(ref string szData)
        {
            szData += "<";
            if (!m_pToken.IsNumeric())
            {
                szData += m_pToken.GetLexeme();
            }
            else
            {
                szData += "NUM_";
                szData += m_pToken.GetLexeme();
            }
            szData += ">";

            if (m_pToken.GetCompoundData() != null)
            {
                szData += m_pToken.GetCompoundData().m_szName;
                szData += ",";
                szData += m_pToken.GetCompoundData().m_szAttribute;
            }

            szData += "</";
            if (!m_pToken.IsNumeric())
            {
                szData += m_pToken.GetLexeme();
            }
            else
            {
                szData += "NUM_";
                szData += m_pToken.GetLexeme();
            }
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 For handling DD variable and Expression
        {
            return pVisitor.visitPrimaryExpression(
                    this,
                    pSymbolTable,
                    ref pvar,
                    AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            return 0;
        }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return m_pToken.GetLineNumber();
        }

        public CToken GetToken()
        {
            return m_pToken;
        }

    }

}
