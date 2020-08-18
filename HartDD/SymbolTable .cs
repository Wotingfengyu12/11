using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    /*
    public struct CONSTANT_POOL_UTF8
    {
        public byte tag;
        public ushort length;
        public byte[] pBytes; // this is of length bytes
    }
    */

    public class CSymbolTable
    {
        protected List<CVariable> m_symbol_table;
        //List<CONSTANT_POOL_UTF8> m_constant_pool_table;
        public int m_nCurrentScope;         //SCR26200 Felix

        public CSymbolTable()
        {
            m_symbol_table = new List<CVariable>();
            //m_constant_pool_table = new List<CONSTANT_POOL_UTF8>();
        }

        public CVariable GetAt(int nIdx)
        {
            if (((int)nIdx < m_symbol_table.Count) && (nIdx >= 0))
            {
                return m_symbol_table[nIdx];
            }
            else
            {
                return null;
            }
        }

         public bool SetAt(int nIdx, CVariable cv)
        {
            if (((int)nIdx < m_symbol_table.Count) && (nIdx >= 0))
            {
                m_symbol_table[nIdx] = cv;
                return true;
            }
            else
            {
                return false;
            }
        }

       public void SetValue(int nIdx, INTER_VARIANT cv)
        {
            m_symbol_table[nIdx].SetValue(cv);
        }

        public CVariable GetConstantAt(int nIdx)
        {
            return null;
        }

        public int Insert(CToken token)
        {
            int nIdx = GetIndex(token.GetLexeme());
            if (-1 == nIdx)
            {
                CVariable pNewToken = new CVariable();
                pNewToken.setCVariable(token);
                pNewToken.SetSymbolTableIndex(m_symbol_table.Count);
                m_symbol_table.Add(pNewToken);

                return m_symbol_table.Count - 1;
            }
            return nIdx;
        }

        public int Insert(CToken token, int m_ScopeIndex)
        {
            int nIdx = -1;
            if ((nIdx = Find(token.GetLexeme(), m_ScopeIndex)) == 0)
            {
                CVariable pNewToken = new CVariable();
                pNewToken.setCVariable(token);
                pNewToken.SetSymbolTableIndex(m_symbol_table.Count);
                pNewToken.Token.SetSymbolTableScopeIndex(m_ScopeIndex);
                m_symbol_table.Add(pNewToken);

                return m_symbol_table.Count - 1;
            }
            return nIdx;// return what Find found
        }

        public int Delete(string pszTokenName)
        {
            int index = 0;
            if (-1 != (index = GetIndex(pszTokenName)))
            {
                m_symbol_table.RemoveAt(index);//.erase(m_symbol_table.begin() + index);
                return index;
            }
            return -1;
        }

        public CVariable Find(string pszTokenName, ref int index)
        {
            int nSize = m_symbol_table.Count;
            CVariable pToken = null;
            CVariable pToken2 = null;
            for (int i = 0; i < nSize; i++)
            {
                pToken = null;
                pToken = m_symbol_table[i];
                if (m_nCurrentScope == 0)                                       //SCR26200 Felix
                {
                    if (pToken.Token.GetLexeme() == pszTokenName)
                    {
                        //return i;
                        return pToken;
                    }
                }
                else
                {
                    //Return values from method calling methods.  Arguments to methods calling methods.  Device Variables.
                    if (pToken.Token.m_bIsReturnToken || pToken.Token.m_bIsRoutineToken || pToken.Token.m_bIsGlobal)
                    {
                        if (pToken.Token.GetLexeme() == pszTokenName)
                        {
                            index = i;
                            return pToken;
                        }
                    }
                    else
                    {
                        if ((pToken.Token.GetLexeme() == pszTokenName) && (pToken.Token.GetSymbolTableScopeIndex() < m_nCurrentScope))
                        {
                            if (pToken2 != null)
                            {
                                if (pToken2.Token.GetSymbolTableScopeIndex() < pToken.Token.GetSymbolTableScopeIndex())
                                {
                                    index = i;
                                    pToken2 = pToken;
                                }
                                // else pToken2 is the closest, leave it be
                            }
                            else
                            {
                                index = i;
                                pToken2 = pToken;
                            }
                        }
                        if ((pToken.Token.GetLexeme() == pszTokenName) && (pToken.Token.GetSymbolTableScopeIndex() == m_nCurrentScope))
                        {
                            index = i;
                            return pToken;
                        }
                    }
                }
            }
            index = nSize;
            return pToken2;
            //return ret;
        }

        public int Find(string pszTokenName, int m_nSymbolTableScopeIndex) //SCR26200 Felix
        {
            int nSize = m_symbol_table.Count;
            CVariable pToken = null;
            for (int i = 0; i < nSize; i++)
            {
                pToken = null;
                pToken = m_symbol_table[i];
                if ((pToken.Token.GetLexeme() == pszTokenName) && (pToken.Token.GetSymbolTableScopeIndex() == m_nSymbolTableScopeIndex))
                {
                    // stevev 25apr13  return pToken;
                    return i;
                }
            }
            return 0;
        }

        public int GetSymbTableSize()
        {
            return m_symbol_table.Count;
        }

        public int TraceDump(ref string szDumpFile)
        {
            int nSize = m_symbol_table.Count;
            //INTER_VARIANT var;
            szDumpFile += "<";
            szDumpFile += "SymbolTable";
            szDumpFile += ">";
            for (int i = 0; i < nSize; i++)
            {
                szDumpFile += "<";
                szDumpFile += m_symbol_table[i].Token.GetLexeme();
                szDumpFile += ">";
                m_symbol_table[i].GetValue().XMLize(ref szDumpFile);
                szDumpFile += "</";
                szDumpFile += m_symbol_table[i].Token.GetLexeme();
                szDumpFile += ">";
            }
            szDumpFile += "</";
            szDumpFile += "SymbolTable";
            szDumpFile += ">";

            return 1;
        }

        public int GetCount()
        {
            return ((int)m_symbol_table.Count);// WS - 9apr07 - VS2005 checkin
        }

        public int InsertConstant(CToken token)
        {
            /*
            CONSTANT_POOL_UTF8 pconst_entry = new CONSTANT_POOL_UTF8();
            m_constant_pool_table.Add(pconst_entry);
            pconst_entry.tag = CONSTANT_Utf8;

            short i16Count = (short)token.GetLexeme().Length;
            pconst_entry.length = _MSB_INT16(i16Count);
            pconst_entry.length <<= 8;
            pconst_entry.length |= _LSB_INT16(i16Count);
            pconst_entry.pBytes = new byte[pconst_entry.length + 1];
            //memset(pconst_entry.pBytes, 0, pconst_entry.length + 1);
            memcpy(pconst_entry.pBytes, token.GetLexeme(), pconst_entry.length);

            return m_constant_pool_table.Count - 1;
            */
            return 0;
        }

        int InsertOMConstant(byte[] pchOID_AID, byte uchType)
        {
            /*
            CONSTANT_POOL_UTF8 pconst_entry = null;
            pconst_entry = new CONSTANT_POOL_UTF8();
            m_constant_pool_table.Add(pconst_entry);
            pconst_entry.tag = uchType;

            pconst_entry.length = 9;
            pconst_entry.pBytes = new _UCHAR[pconst_entry.length + 1];
            memcpy(pconst_entry.pBytes, pchOID_AID, pconst_entry.length);

            return m_constant_pool_table.Count - 1;
            */
            return 0;
        }

        public int GetIndex(string pszTokenName)  //Vibhor 010705: Made Public
        {
            int nSize = m_symbol_table.Count;
            for (int i = 0; i < nSize; i++)
            {
                CToken pToken = m_symbol_table[i].Token;
                if (pToken.GetLexeme() == pszTokenName)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetIndex(string pszTokenName, int m_nSymbolTableScopeIndex) //SCR26200 Felix
        {
            int nSize = m_symbol_table.Count;
            for (int i = 0; i < nSize; i++)
            {
                CToken pToken = m_symbol_table[i].Token;
                if (pToken.GetLexeme() == pszTokenName && (pToken.GetSymbolTableScopeIndex() == m_nSymbolTableScopeIndex))
                {
                    return i;
                }
            }

            for (int i = 0; i < nSize; i++)
            {
                CToken pToken = m_symbol_table[i].Token;
                if (pToken.GetLexeme() == pszTokenName)
                {
                    return i;
                }
            }
            return -1;
        }
        // stevev 25apr13  CVariable* Find(

        //void Remove(int m_ScopeIndex) // 24apr13 stevev

    }

    public class CVariable// : CToken
    {
        protected INTER_VARIANT m_Value;

        public CToken Token;

        public CVariable()
        {
            m_Value = new INTER_VARIANT();
            Token = new CToken();
        }

        public void setCVariable(CToken pToken)
        {
            Token = pToken;
            m_Value = new INTER_VARIANT();
            RUL_TOKEN_TYPE Type = pToken.GetType();
            RUL_TOKEN_SUBTYPE SubType = pToken.GetSubType();

            if (Type == RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE)
            {
                byte[] data;
                switch (SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL:
                        data = BitConverter.GetBytes(' ');
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_CHAR);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL:
                        //m_Value = (Int64)0;
                        data = BitConverter.GetBytes((Int64)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_LONGLONG);// = (char)' ';
                        break;
                    // Walt EPM 08sep08 - added
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL:
                        //m_Value = (ushort)0;
                        data = BitConverter.GetBytes((ushort)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_USHORT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL:
                        //m_Value = (short)0;
                        data = BitConverter.GetBytes((short)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_SHORT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL:
                        //m_Value = (uint)0;
                        data = BitConverter.GetBytes((uint)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_UINT);// = (char)' ';
                        break;
                    // Walt EPM 08sep08 - end added
                    case RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_DECL:
                        //m_Value = (long)0;
                        data = BitConverter.GetBytes((int)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_INT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL:
                        //m_Value = (bool)false;
                        data = BitConverter.GetBytes(false);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_BOOL);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_REAL_DECL:
                        //m_Value = (float)0.0;
                        data = BitConverter.GetBytes((float)0.0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_FLOAT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL:
                        //m_Value = (double)0.0;//WS:EPM 10aug07
                        data = BitConverter.GetBytes((double)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_DOUBLE);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_STRING_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL: //Added By Anil July 07 2005
                        //m_Value = (string)"";
                        data = System.Text.Encoding.Default.GetBytes("");
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_DD_STRING);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL:
                        //m_Value = (byte)' ';//WHS EP June17-2008 have changed this to make sure that it works for all data types
                        data = BitConverter.GetBytes(' ');
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_UNSIGNED_CHAR);// = (char)' ';
                        break;
                }
            }
        }

        public void SetSymbolTableIndex(int nSymTblIdx)
        {
            Token.SetSymbolTableIndex(nSymTblIdx);
        }


        public CVariable(string szLexeme, RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType)
        {
            ;
        }

        //	Identify self
        public void Identify(string szData)
        {
            szData += "<";
            szData += Token.m_pszLexeme;
            szData += ">";

            if (Token.GetCompoundData() != null)
            {
                szData += Token.GetCompoundData().m_szName;
                szData += ",";
                szData += Token.GetCompoundData().m_szAttribute;
            }

            szData += "</";
            szData += Token.m_pszLexeme;
            szData += ">";
        }

        public INTER_VARIANT GetValue()
        {
            return m_Value;
        }

        public void SetValue(INTER_VARIANT iv)
        {
            m_Value = iv;
        }

        void SetVarType(RUL_TOKEN_TYPE Type, RUL_TOKEN_SUBTYPE SubType)//Added By Anil August 5 2005 For handling DD variable and Expression
        {
            byte[] data;
            if (Type == RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE)
            {
                switch (SubType)
                {
                    case RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL:
                        data = BitConverter.GetBytes(' ');
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_CHAR);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL:
                        //m_Value = (INT64)0;
                        data = BitConverter.GetBytes((Int64)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_LONGLONG);// = (char)' ';
                        break;
                    // Walt EPM 08sep08 - added
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL:
                        //m_Value = (unsigned short)0;
                        data = BitConverter.GetBytes((ushort)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_USHORT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL:
                        //m_Value = (short)0;
                        data = BitConverter.GetBytes((short)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_SHORT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL:
                        data = BitConverter.GetBytes((uint)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_UINT);// = (char)' ';
                        break;
                    // Walt EPM 08sep08 - end added
                    case RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_DECL:
                        data = BitConverter.GetBytes((int)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_INT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL:
                        data = BitConverter.GetBytes(false);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_BOOL);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_REAL_DECL:
                        data = BitConverter.GetBytes((float)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_FLOAT);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL:
                        data = BitConverter.GetBytes((double)0);
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_DOUBLE);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_STRING_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL: //Added By Anil July 07 2005
                        data = System.Text.Encoding.Default.GetBytes("");
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_DD_STRING);// = (char)' ';
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL:
                        data = BitConverter.GetBytes((byte)' ');
                        m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_UNSIGNED_CHAR);// = (char)' ';
                        break;

                }
            }
        }
        /*Vibhor 070705: Start of Code*/
        //Adding following overloaded fns for setting the initial
        //values of Global (DD) variables .

        void SetValue(int iValue)
        {
            byte[] data = BitConverter.GetBytes(iValue);
            m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_INT);// = (int)iValue;
        }

        void SetValue(float fValue)
        {
            byte[] data = BitConverter.GetBytes(fValue);
            m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_FLOAT);// = (int)iValue;
        }

        void SetValue(double dValue)
        {
            byte[] data = BitConverter.GetBytes(dValue);
            m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_DOUBLE);// = (int)iValue;
        }

        void SetValue(string pszValue)
        {
            byte[] data = Encoding.Default.GetBytes(pszValue);
            m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_DD_STRING);// = (int)iValue;
        }

        void SetValue(bool bValue)
        {
            byte[] data = BitConverter.GetBytes(bValue);
            m_Value.SetValue(data, 0, VARIANT_TYPE.RUL_BOOL);// = (int)iValue;
        }
        /*Vibhor 070705: End of Code*/
    }

}

