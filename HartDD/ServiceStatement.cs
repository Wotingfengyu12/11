using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CServiceStatement : CStatement
	{
		//CToken m_pToken;   //this contains the service.

		public CServiceStatement()
		{
			//m_pToken = new CToken();
		}

		//	Identify self
		public override void Identify(ref string szData)
		{
			return;
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

		public override int GetLineNumber()
		{
			return -1;
		}


	}
}
