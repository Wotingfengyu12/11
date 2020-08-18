using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CCompoundExpression : CExpression
    {
        CExpression m_pFirstExp;
        CExpression m_pSecondExp;
        RUL_TOKEN_SUBTYPE m_Operator;

        public CCompoundExpression()
        {
            m_pFirstExp = null;
            m_pSecondExp = null;
            m_Operator = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;

        }

        public CCompoundExpression(CExpression f, CExpression s, RUL_TOKEN_SUBTYPE Op)
        {
            m_pFirstExp = f;
            m_pSecondExp = s;
            m_Operator = Op;
        }

        public override void Identify(ref string szData)
        {
            string sz1 = "BRACK";
            string sz2;
            if (m_Operator == RUL_TOKEN_SUBTYPE.RUL_RPAREN)
                sz2 = sz1;
            else
                sz2 = szTokenSubstrings[(int)m_Operator];

            szData += "<";
            szData += sz2;
            szData += ">";
            if (m_pFirstExp != null)
            {
                m_pFirstExp.Identify(ref szData);
            }
            if (m_pSecondExp != null)
            {
                m_pSecondExp.Identify(ref szData);
            }
            szData += "</";
            szData += sz2;
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 For handling DD variable and Expression
        {
            return pVisitor.visitCompoundExpression(
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
            return m_pSecondExp.GetLineNumber();
        }

        public CExpression GetFirstExpression()
        {
            return m_pFirstExp;
        }

        public CExpression GetSecondExpression()
        {
            return m_pSecondExp;
        }

        public RUL_TOKEN_SUBTYPE GetOperator()
        {
            return m_Operator;
        }


    }

}
