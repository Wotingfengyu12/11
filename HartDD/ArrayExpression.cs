using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class EXPR_VECTOR : List<CExpression>
    {

    }

    public class CArrayExpression : CExpression
    {
        CToken m_pToken;
        EXPR_VECTOR m_vecExpressions;   //each dim in actual array corresponds 
        public CArrayExpression()
        {
            m_pToken = new CToken();
            m_vecExpressions = new EXPR_VECTOR();
        }

        public CArrayExpression(CToken pToken)
        {
            m_pToken = pToken;
            m_vecExpressions = new EXPR_VECTOR();
        }

        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += m_pToken.GetLexeme();
            szData += ">";

            for (int i = 0; i < m_vecExpressions.Count; i++)
            {
                m_vecExpressions[i].Identify(ref szData);
            }

            szData += "</";
            szData += m_pToken.GetLexeme();
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
        ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitArrayExpression(
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
            return m_vecExpressions[m_vecExpressions.Count - 1].GetLineNumber();
        }

        public CToken GetToken()
        {
            return m_pToken;
        }

        public EXPR_VECTOR GetExpressions()
        {
            return m_vecExpressions;
        }

        public void AddDimensionExpr(CExpression pExpr)
        {
            m_vecExpressions.Add(pExpr);

        }
        //to one elt in the vector
    }

}
