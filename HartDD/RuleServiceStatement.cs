using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CRuleServiceStatement : CStatement
    {
        CToken m_pRuleName;

        public CRuleServiceStatement()
        {

        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<";
            szData += szTokenSubstrings[(int)RUL_TOKEN_SUBTYPE.RUL_RULE_ENGINE];
            szData += ">";

            if (m_pRuleName != null)
                m_pRuleName.Identify(ref szData);

            szData += "</";
            szData += szTokenSubstrings[(int)RUL_TOKEN_SUBTYPE.RUL_RULE_ENGINE];
            szData += ">";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitRuleService(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = null;
            //try
            {
                //Munch a <RuleEngine>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_TYPE.RUL_KEYWORD != pToken.GetType())
                    || (RUL_TOKEN_SUBTYPE.RUL_RULE_ENGINE != pToken.GetSubType()))
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }

                //Munch a <::>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType())
                    || RUL_TOKEN_SUBTYPE.RUL_SCOPE != pToken.GetSubType())
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGSCOPE);
                    plexAnal.UnGetToken();
                }

                //Munch a <Invoke>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_TYPE.RUL_KEYWORD != pToken.GetType())
                    || (RUL_TOKEN_SUBTYPE.RUL_INVOKE != pToken.GetSubType()))
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGINVOKE);
                    plexAnal.UnGetToken();
                }

                //Munch a <(>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_TYPE.RUL_SYMBOL != pToken.GetType())
                    || RUL_TOKEN_SUBTYPE.RUL_LPAREN != pToken.GetSubType())
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGLPAREN);
                    plexAnal.UnGetToken();
                }

                //Munch a <RuleName> -- this is a string
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT != pToken.GetSubType()
                        && RUL_TOKEN_SUBTYPE.RUL_STRING_DECL != pToken.GetSubType()))
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGRNAME);
                    plexAnal.UnGetToken();
                }
                m_pRuleName = pToken;
                pToken = null;

                //Munch a <)>
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || RUL_TOKEN_SUBTYPE.RUL_RPAREN != pToken.GetSubType())
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGRPAREN);
                    plexAnal.UnGetToken();
                }

                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable))
                    || pToken == null || (RUL_TOKEN_SUBTYPE.RUL_SEMICOLON != pToken.GetSubType()))
                {
                    //ADD_ERROR(C_RS_ERROR_MISSINGSC);
                    plexAnal.UnGetToken();
                }
                return 1;
            }
            /*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
				plexAnal.MovePast(
					RUL_TOKEN_TYPE.RUL_SYMBOL,
					RUL_TOKEN_TYPE.RUL_SEMICOLON,
					pSymbolTable);
			}
			catch (...)
			{
				//throw (C_UM_ERROR_UNKNOWNERROR);
			}
			return PARSE_FAIL;
			*/
        }

        public override int GetLineNumber()
        {
            return m_pRuleName.GetLineNumber();
        }

        public CToken GetRuleToken()
        {
            return m_pRuleName;
        }

        public void GetRuleName(ref string szData)
        {
            szData += m_pRuleName.GetLexeme();
        }

    }

}
