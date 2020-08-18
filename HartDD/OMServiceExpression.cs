using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class COMServiceExpression : CExpression
    {
		string m_pucObjectName;
		List<string> m_attribList;
		int m_i32constant_pool_idx;

		public COMServiceExpression()
		{
			m_pucObjectName = null;
			m_i32constant_pool_idx = -1;
			m_attribList = new List<string>();
		}

		//	Identify self
		public void Identify(string szData)//override?
		{
			;
		}

		//	Allow Visitors to do different operations on the node.
		public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
			ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
		{
			return pVisitor.visitOMExpression(this, pSymbolTable, ref pvar, AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
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

		string GetObjectName()
		{
			return m_pucObjectName;
		}

		int GetAttibuteCount()
		{
			return m_attribList.Count;
		}

		string GetAttributeName(int i32Idx)
		{
			return m_attribList[i32Idx];
		}

		int GetConstantPoolIdx()
		{
			return m_i32constant_pool_idx;
		}

		//protected int MakeConstantPoolEntry(CLexicalAnalyzer* plexAnal, CSymbolTable* pSymbolTable);

	}
}
