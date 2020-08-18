using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CTypeCheckVisitor : CGrammarNodeVisitor
    {
		const int MAX_BYTE = 0xff;
		//typedef int(CTypeCheckVisitor::* PFN_TYPECHECKER)(INTER_VARIANT&,INTER_VARIANT&,INTER_VARIANT&);
		delegate int PFN_TYPECHECKER(ref INTER_VARIANT V1, ref INTER_VARIANT V2, ref INTER_VARIANT V3);

		VARIANT_TYPE[] m_TypeMapper = new VARIANT_TYPE[MAX_BYTE];
		string m_pszRuleName;
		PFN_TYPECHECKER[] m_fnTypeCheckTable = new PFN_TYPECHECKER[MAX_BYTE];

		const int TYPE_SUCCESS = 1;
		const int TYPE_FAILURE = 0;

		public CTypeCheckVisitor()
        {
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_INT_CONSTANT] = VARIANT_TYPE.RUL_INT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT] = VARIANT_TYPE.RUL_CHAR;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL] = VARIANT_TYPE.RUL_UINT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL] = VARIANT_TYPE.RUL_INT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_LONG_DECL] = VARIANT_TYPE.RUL_INT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL] = VARIANT_TYPE.RUL_LONGLONG;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL] = VARIANT_TYPE.RUL_USHORT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL] = VARIANT_TYPE.RUL_SHORT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_REAL_CONSTANT] = VARIANT_TYPE.RUL_FLOAT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_REAL_DECL] = VARIANT_TYPE.RUL_FLOAT;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL] = VARIANT_TYPE.RUL_DOUBLE;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_BOOL_CONSTANT] = VARIANT_TYPE.RUL_BOOL;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL] = VARIANT_TYPE.RUL_BOOL;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT] = VARIANT_TYPE.RUL_CHARPTR;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_STRING_DECL] = VARIANT_TYPE.RUL_CHARPTR;
			m_TypeMapper[(int)RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL] = VARIANT_TYPE.RUL_UNSIGNED_CHAR;

			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_UPLUS] = new PFN_TYPECHECKER(tc_uplus);   // added &CTypeCheckVIsitor:: PAW 03/03/09
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_UMINUS] = new PFN_TYPECHECKER(tc_uminus);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS] = new PFN_TYPECHECKER(tc_add);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS] = new PFN_TYPECHECKER(tc_sub);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_MUL] = new PFN_TYPECHECKER(tc_mul);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_DIV] = new PFN_TYPECHECKER(tc_div);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_MOD] = new PFN_TYPECHECKER(tc_mod);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_EXP] = new PFN_TYPECHECKER(tc_exp);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_NOT_EQ] = new PFN_TYPECHECKER(tc_neq);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_LT] = new PFN_TYPECHECKER(tc_lt);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_GT] = new PFN_TYPECHECKER(tc_gt);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_EQ] = new PFN_TYPECHECKER(tc_eq);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_GE] = new PFN_TYPECHECKER(tc_ge);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_LE] = new PFN_TYPECHECKER(tc_le);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_AND] = new PFN_TYPECHECKER(tc_land);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_OR] = new PFN_TYPECHECKER(tc_lor);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_NOT] = new PFN_TYPECHECKER(tc_lnot);// added new PFN_TYPECHECKER( PAW
			m_fnTypeCheckTable[(int)RUL_TOKEN_SUBTYPE.RUL_RPAREN] = new PFN_TYPECHECKER(tc_rparen);// added new PFN_TYPECHECKER( PAW
		}

		int tc_uplus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_uminus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_add(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_sub(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_mul(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_div(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_mod(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_exp(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_neq(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_lt(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_gt(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_eq(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_ge(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_le(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_land(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_lor(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_lnot(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		int tc_rparen(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
		{
			return TYPE_SUCCESS;
		}

		public override int visitArrayExpression(CArrayExpression pArrExp, CSymbolTable pSymbolTable,
						ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType)
		{
			//try
			{
				int i32Idx = pArrExp.GetToken().GetSymbolTableIndex();
				CVariable pVariable = pSymbolTable.GetAt(i32Idx);
				INTER_SAFEARRAY prgsa = pVariable.GetValue().GetValue().prgsa;

				//evaluate the expressions...
				EXPR_VECTOR pvecExpressions = pArrExp.GetExpressions();
				List<int> vecDims = new List<int>();
				prgsa.GetDims(ref vecDims);

				if (vecDims.Count != pvecExpressions.Count)
				{
					//throw (C_TC_ERROR_DIM_MISMATCH, pArrExp);
				}
				byte[] by;
				switch (prgsa.Type())
				{
					case VARIANT_TYPE.RUL_CHAR:
						by = new byte[1];
						//by[0] = ' ';
						by[0] = 0;
						pvar.SetValue(by, 0, prgsa.Type());
						break;
					case VARIANT_TYPE.RUL_INT:
						by = new byte[4];
						by = BitConverter.GetBytes((int)0);
						pvar.SetValue(by, 0, prgsa.Type());
						break;
					case VARIANT_TYPE.RUL_BOOL:
						by = new byte[1];
						by[0] = 0;
						pvar.SetValue(by, 0, prgsa.Type());
						break;
					case VARIANT_TYPE.RUL_FLOAT:
						by = new byte[4];
						by = BitConverter.GetBytes((float)0.0);
						pvar.SetValue(by, 0, prgsa.Type());
						break;
					case VARIANT_TYPE.RUL_DOUBLE:
						by = new byte[4];
						by = BitConverter.GetBytes((double)0.0);
						pvar.SetValue(by, 0, prgsa.Type());
						break;
					case VARIANT_TYPE.RUL_CHARPTR:
					case VARIANT_TYPE.RUL_SAFEARRAY:
					case VARIANT_TYPE.RUL_DD_STRING:
					case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
						by = new byte[1];
						by[0] = 0;
						pvar.SetValue(by, 0, prgsa.Type());
						break;
				}
				return TYPE_SUCCESS;
			}
			/*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
			}
			catch (...)
			{
				throw (C_UM_ERROR_UNKNOWNERROR, pArrExp);
			}
			return TYPE_FAILURE;
			*/
			}






		}
	}
