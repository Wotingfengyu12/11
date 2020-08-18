using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FieldIot.HARTDD
{
    public enum methodCallSource_t
    {
        msrc_UNKNOWN,   // 0
        msrc_ACTION,    // called from a pre/post action
        msrc_EXTERN,    // called from an external location(usually the UI)
        msrc_METHOD,     // future method calling a method
        msrc_CMD_ACT   // action from inside send()/srvcRcvPkt() ie pre/post read/writes
    }/*typedef*/


    public enum mState_t
    {
        mc_NoTExist,    /* 0 */
        mc_Initing,     /* 1 */
        mc_Running,     /* 2 */
        mc_Closing      /* 3 */
    }

    public enum valueType_t
    {
        invalid,
        isBool,
        isOpcode,
        isIntConst,
        isFloatConst,
        isDepIndex,
        isSymID,
        isVeryLong,
        isString,
        isWideString,
        isPackedString
    }

    public enum expressElemType_t
    {/* note that these are equivalent to the tag values */
        eeT_Unknown = 0,
        eet_NOT,            //_OPCODE 1         bool
        eet_NEG,            //_OPCODE 2         Value
        eet_BNEG,           //_OPCODE 3         Value
        eet_ADD,            //_OPCODE 4         Value
        eet_SUB,            //_OPCODE 5         Value
        eet_MUL,            //_OPCODE 6         Value
        eet_DIV,            //_OPCODE 7         Value
        eet_MOD,            //_OPCODE 8         Value
        eet_LSHIFT,         //_OPCODE 9         Value
        eet_RSHIFT,         //_OPCODE 10        Value
        eet_AND,            //_OPCODE 11        Value
        eet_OR,             //_OPCODE 12        Value
        eet_XOR,            //_OPCODE 13        bool
        eet_LAND,           //_OPCODE 14        bool
        eet_LOR,            //_OPCODE 15        bool
        eet_LT,             //_OPCODE 16        bool
        eet_GT,             //_OPCODE 17        bool
        eet_LE,             //_OPCODE 18        bool
        eet_GE,             //_OPCODE 19        bool
        eet_EQ,             //_OPCODE 20        bool
        eet_NEQ,            //_OPCODE 21        bool

        eet_INTCST,         //_CONST  22		Value
        eet_FPCST,          //_CONST  23        Value

        eet_VARID,          //_VARVAL 24    isRef = F
        eet_MAXVAL,         //_VARVAL 25    isRef = F,isMax = T
        eet_MINVAL,         //_VARVAL 26    isRef = F,isMax = F

        eet_VARREF,         //_VARVAL 27    isRef = T
        eet_MAXREF,         //_VARVAL 28    isRef = T,isMax = T
        eet_MINREF,          //_VARVAL 29    isRef = T,isMax = F

        //#ifdef NOTUSED4HART
        eet_BLOCK,          //_VARVAL 30    isRef = F ????
        eet_BLOCKID,        //_VARVAL 31    isRef = F
        eet_BLOCKREF,       //_VARVAL 32    isRef = T
                            //#endif

        eet_STRCONST,       //STRCST_OPCODE  33
        eet_SYSENUM,        //SYSTEMENUM_OPCODE 34
        eet_COUNTREF,       //CNTREF_OPCODE 35
        eet_CAPACITYREF,    //CAPREF_OPCODE 36
        eet_FIRSTREF,       //FSTREF_OPCODE 37 // stevev 17aug06 - this is a VARREF
        eet_LASTREF,         //LSTREF_OPCODE 38 // stevev 17aug06 - this is a VARREF
        /* stevev 17aug06 - rest of legal types - not in Tokenizer yet all reference.xxx*/
        eet_DFLT_VALREF,   //DFLTVAL_OPCODE 39
        eet_VIEW_MINREF,   //VMIN_OPCODE    40
        eet_VIEW_MAXREF,    //VMAX_OPCODE    41

        eet_INVALID = -1
    }

    public struct vValue_t
    {
        public bool bIsTrue;    // constant T/F
        public expressElemType_t iOpCode;  // type 1 - 21
        public int iIntConst;  // type 22
        public double dFloatConst;// type 23 - float
        public float fFloatConst;// type 23 - float
        public int depIndex;   // type 24 through 29
        public uint varSymbolID;// other uses like index into dependency list
        public Int64 longlongVal;// undefined in DDs for now
    }

    public class hCmethodCall
    {
        public uint methodThreadID;

        public uint methodID;
        public methodCallSource_t source;  // what kind of call this is
        public List<CValueVarient> paramList;
        // stevev 27jan06 - track the used dialogs on the stack

        public mState_t m_MethState;

        public CDDLMethod m_pMeth = null;

        public object m_pMethodDlg;
        public object m_pMenuDlg;
    }

    public class methodCallList_t : List<hCmethodCall>
    {

    }
    /*
    public struct INTEGER_RANK
    {
        public int rank;
        public bool is_unsigned;
    }
    */

    public class CValueVarient
    {
        //byte[] tmpBuf = new byte[40];// for weird float conversions//////

        /* union */
        public string sStringVal;
        string sWideStringVal;
        public valueType_t vType;
        public int vSize;
        //uint vIndex;   /* only for bit-enum'd bit reference resolution */
        /* also holds 'which' for attribute id resolution*/
        public bool vIsValid;
        public bool vIsUnsigned;
        public bool vIsDouble;
        bool vIsBit;    /* set when bit-Enum SymbolID:: vIndex is Valid */
        public vValue_t vValue;
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        public CValueVarient()
        {
            vIsDouble = false;
        }

        public bool valueIsZero()
        {
            if (vType == valueType_t.isBool)
                return !vValue.bIsTrue;
            else if (vType == valueType_t.isIntConst)
                return ((vValue.iIntConst == 0) ? true : false);
            else if (vType == valueType_t.isVeryLong)
                return ((vValue.longlongVal == 0) ? true : false);
            else if (vType == valueType_t.isFloatConst)
                return (vValue.fFloatConst < 0.0001 && vValue.fFloatConst > -0.0001) ? true : false;
            else if (vType == valueType_t.isSymID)
                return ((vValue.varSymbolID == 0) ? true : false);
            else
                return (true);
        }

        public bool isNumeric()
        {
            if ((vType == valueType_t.isBool) || (vType == valueType_t.isIntConst) || (vType == valueType_t.isFloatConst) || (vType == valueType_t.isVeryLong))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isInteger()
        {
            if ((vType == valueType_t.isIntConst) || (vType == valueType_t.isVeryLong))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isFloat()
        {
            if (vType == valueType_t.isFloatConst)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetValue(object value, valueType_t type)
        {
            vType = type;
            switch (type)
            {
                case valueType_t.isSymID:
                    vValue.varSymbolID = (uint)value;
                    break;

                case valueType_t.isIntConst:
                    vValue.iIntConst = (int)(value);
                    break;

                case valueType_t.isFloatConst:
                    vValue.fFloatConst = (float)value;
                    break;

                case valueType_t.isWideString:
                    sWideStringVal = (string)value;
                    break;

                case valueType_t.isString:
                    sStringVal = (string)value;
                    break;

                default:
                    break;
            }
            if(value.GetType() == typeof(double))
            {
                vIsDouble = true;
            }
        }

        public void SetVarValue(byte[] value, variableType_t type, byte size, uint mask)
        {
            switch(type)
            {
                case variableType_t.vT_Ascii:
                case variableType_t.vT_Duration:
                case variableType_t.vT_EUC:
                case variableType_t.vT_OctetString:
                case variableType_t.vT_Password:
                case variableType_t.vT_DateAndTime:
                case variableType_t.vT_TimeValue:
                    vType = valueType_t.isString;
                    break;

                case variableType_t.vT_PackedAscii:
                    vType = valueType_t.isPackedString;
                    break;

                case variableType_t.vT_HartDate:
                case variableType_t.vT_Time:
                case variableType_t.vT_BitEnumerated:
                case variableType_t.vT_BitString:
                case variableType_t.vT_Enumerated:
                case variableType_t.vT_Boolean:
                case variableType_t.vT_Index:
                case variableType_t.vT_Unsigned:
                case variableType_t.vT_Integer:
                    vType = valueType_t.isIntConst;
                    break;

                case variableType_t.vT_FloatgPt:
                case variableType_t.vT_Double:
                    vType = valueType_t.isFloatConst;
                    break;

                case variableType_t.vT_VisibleString:
                    vType = valueType_t.isWideString;
                    break;

                case variableType_t.vT_undefined:
                case variableType_t.vT_unused:
                default:
                    vType = valueType_t.invalid;
                    break;
            }

            SetValue(value, size, valueType_t.invalid, mask);
        }

        public void SetValue(byte[] value, byte size = 1, valueType_t type = valueType_t.invalid, uint mask = 0xffffffff)
        {
            if (type != valueType_t.invalid)
            {
                vType = type;
            }
            switch (vType)
            {
                case valueType_t.isSymID:
                    vValue.varSymbolID = (UInt32)(BitConverter.ToUInt32(value, 0) & mask);
                    break;

                case valueType_t.isIntConst:
                    if (size == 1)
                    {
                        vValue.iIntConst = (byte)(value[0] & mask);
                    }
                    else if (size == 2)
                    {
                        byte[] sl = new byte[2];
                        for (int i = 0; i < 2; i++)
                        {
                            sl[i] = value[i];
                        }
                        sl = sl.Reverse().ToArray();
                        vValue.iIntConst = (Int16)(BitConverter.ToInt16(sl, 0) & mask);
                    }
                    else if (size == 3)//DATE TYPE?
                    {
                        byte[] dl = new byte[4];
                        dl[0] = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            dl[i + 1] = value[i];
                        }
                        dl = dl.Reverse().ToArray();
                        vValue.iIntConst = (int)(BitConverter.ToInt32(dl, 0) & mask);
                    }
                    else if (size == 4)
                    {
                        byte[] il = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            il[i] = value[i];
                        }
                        il = il.Reverse().ToArray();
                        vValue.iIntConst = (int)(BitConverter.ToInt32(il, 0) & mask);
                    }
                    break;

                case valueType_t.isFloatConst:
                    byte[] fl = new byte[4];
                    for(int i = 0; i < 4; i++)
                    {
                        fl[i] = value[i];
                    }
                    fl = fl.Reverse().ToArray();

                    vValue.fFloatConst = BitConverter.ToSingle(fl, 0);
                    break;

                case valueType_t.isWideString:
                    sWideStringVal = Encoding.UTF8.GetString(value, 0, size);// BitConverter.ToString(value);// (string)value;
                    break;

                case valueType_t.isString:
                    sStringVal = Encoding.ASCII.GetString(value, 0, size); ;// BitConverter.ToString(value);// (string)value;
                    break;

                case valueType_t.isPackedString:
                    byte[] data = new byte[size/3*4];
                    Common.UnPack(ref data, value, data.Length);
                    sStringVal = Encoding.Default.GetString(data, 0, data.Length);// BitConverter.ToString(value);// (string)value;
                    break;

                default:
                    break;
            }
            if (value.GetType() == typeof(double))
            {
                vIsDouble = true;
            }
        }

        public bool GetBool()
        {
            {
                bool bVal = false;

                switch (vType)
                {
                    case valueType_t.isBool:
                        {
                            bVal = vValue.bIsTrue;
                        }
                        break;
                    case valueType_t.isIntConst:
                        {
                            bVal = ((vValue.iIntConst > 0) ? true : false);
                        }
                        break;
                    case valueType_t.isVeryLong:
                        {
                            bVal = ((vValue.longlongVal > 0) ? true : false);
                        }
                        break;
                    case valueType_t.isFloatConst:
                        {
                            bVal = ((vValue.fFloatConst > 0) ? true : false);
                        }
                        break;
                    case valueType_t.isString:
                        {
                            bVal = ((sStringVal.Length > 0) ? true : false);
                        }
                        break;
                    case valueType_t.isWideString:
                        {
                            bVal = (sWideStringVal.Length > 0) ? true : false;
                        }
                        break;
                    case valueType_t.isSymID: /* aka unsigned long */
                        {
                            bVal = ((vValue.varSymbolID > 0) ? true : false);
                        }
                        break;
                    case valueType_t.invalid:
                    case valueType_t.isOpcode:
                    case valueType_t.isDepIndex:
                        {
                            bVal = false; // error.
                        }
                        break;
                    default:
                        {
                            bVal = false; // error.
                        }
                        break;
                }
                return bVal;
            }
        }

        public int GetInt()
        {
            {
                int iVal = 0;

                switch (vType)
                {
                    case valueType_t.isBool:
                        {
                            iVal = ((vValue.bIsTrue) ? 1 : 0);
                        }
                        break;
                    case valueType_t.isIntConst:
                        if (vIsUnsigned)
                        {
                            iVal = (int)vValue.iIntConst;
                        }
                        else
                        {
                            iVal = vValue.iIntConst;
                        }
                        break;
                    case valueType_t.isVeryLong:
                        {
                            if (vIsUnsigned)
                            {
                                iVal = (int)vValue.longlongVal;
                            }
                            else
                            {
                                iVal = (int)vValue.longlongVal;
                            }
                        }
                        break;
                    case valueType_t.isSymID: /* aka unsigned long */
                        {
                            iVal = (int)vValue.varSymbolID;
                        }
                        break;
                    case valueType_t.isFloatConst:
                        {
                            iVal = (int)vValue.fFloatConst;
                        }
                        break;
                    case valueType_t.isString:
                        {
                            ScanFormatted sc = new ScanFormatted();
                            //if ( SCAN_FUNC(sStringVal.c_str(), "%d", &iVal) <= 0)
                            if (sc.Parse(sStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (int)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.isWideString:
                        {
                            ScanFormatted sc = new ScanFormatted();
                            //if (wscanf(sWideStringVal.c_str(), L"%d", &iVal) <= 0)
                            if (sc.Parse(sWideStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (int)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.invalid:
                    case valueType_t.isOpcode:
                    case valueType_t.isDepIndex:
                        {
                            iVal = 0;
                        }
                        break;
                    default:
                        {
                            iVal = 0;
                        }
                        break;
                }
                return iVal;
            }
        }

        public uint GetUInt()
        {
            {
                uint iVal = 0;

                switch (vType)
                {
                    case valueType_t.isBool:
                        {
                            iVal = (uint)((vValue.bIsTrue) ? 1 : 0);
                        }
                        break;
                    case valueType_t.isIntConst:
                        if (vIsUnsigned)
                        {
                            iVal = (uint)vValue.iIntConst;
                        }
                        else
                        {
                            iVal = (uint)vValue.iIntConst;
                        }
                        break;
                    case valueType_t.isVeryLong:
                        {
                            if (vIsUnsigned)
                            {
                                iVal = (uint)vValue.longlongVal;
                            }
                            else
                            {
                                iVal = (uint)vValue.longlongVal;
                            }
                        }
                        break;
                    case valueType_t.isSymID: /* aka unsigned long */
                        {
                            iVal = (uint)vValue.varSymbolID;
                        }
                        break;
                    case valueType_t.isFloatConst:
                        {
                            iVal = (uint)vValue.fFloatConst;
                        }
                        break;
                    case valueType_t.isString:
                        {
                            //if ( SCAN_FUNC(sStringVal.c_str(), "%d", &iVal) <= 0)
                            ScanFormatted sc = new ScanFormatted();
                            if (sc.Parse(sStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (uint)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.isWideString:
                        {
                            ScanFormatted sc = new ScanFormatted();
                            //if (wscanf(sWideStringVal.c_str(), L"%d", &iVal) <= 0)
                            if (sc.Parse(sWideStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (uint)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.invalid:
                    case valueType_t.isOpcode:
                    case valueType_t.isDepIndex:
                        {
                            iVal = 0;
                        }
                        break;
                    default:
                        {
                            iVal = 0;
                        }
                        break;
                }
                return iVal;
            }
        }

        public Int64 GetInt64()
        {
            {
                Int64 iVal = 0;

                switch (vType)
                {
                    case valueType_t.isBool:
                        {
                            iVal = ((vValue.bIsTrue) ? 1 : 0);
                        }
                        break;
                    case valueType_t.isIntConst:
                        if (vIsUnsigned)
                        {
                            iVal = (int)vValue.iIntConst;
                        }
                        else
                        {
                            iVal = vValue.iIntConst;
                        }
                        break;
                    case valueType_t.isVeryLong:
                        {
                            if (vIsUnsigned)
                            {
                                iVal = (Int64)vValue.longlongVal;
                            }
                            else
                            {
                                iVal = (Int64)vValue.longlongVal;
                            }
                        }
                        break;
                    case valueType_t.isSymID: /* aka unsigned long */
                        {
                            iVal = (Int64)vValue.varSymbolID;
                        }
                        break;
                    case valueType_t.isFloatConst:
                        {
                            iVal = (Int64)vValue.fFloatConst;
                        }
                        break;
                    case valueType_t.isString:
                        {
                            ScanFormatted sc = new ScanFormatted();
                            //if ( SCAN_FUNC(sStringVal.c_str(), "%d", &iVal) <= 0)
                            if (sc.Parse(sStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (Int64)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.isWideString:
                        {
                            ScanFormatted sc = new ScanFormatted();
                            //if (wscanf(sWideStringVal.c_str(), L"%d", &iVal) <= 0)
                            if (sc.Parse(sWideStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (Int64)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.invalid:
                    case valueType_t.isOpcode:
                    case valueType_t.isDepIndex:
                        {
                            iVal = 0;
                        }
                        break;
                    default:
                        {
                            iVal = 0;
                        }
                        break;
                }
                return iVal;
            }
        }

        public UInt64 GetUInt64()
        {
            {
                UInt64 iVal = 0;

                switch (vType)
                {
                    case valueType_t.isBool:
                        {
                            iVal = (UInt64)((vValue.bIsTrue) ? 1 : 0);
                        }
                        break;
                    case valueType_t.isIntConst:
                        if (vIsUnsigned)
                        {
                            iVal = (UInt64)vValue.iIntConst;
                        }
                        else
                        {
                            iVal = (UInt64)vValue.iIntConst;
                        }
                        break;
                    case valueType_t.isVeryLong:
                        {
                            if (vIsUnsigned)
                            {
                                iVal = (UInt64)vValue.longlongVal;
                            }
                            else
                            {
                                iVal = (UInt64)vValue.longlongVal;
                            }
                        }
                        break;
                    case valueType_t.isSymID: /* aka unsigned long */
                        {
                            iVal = (UInt64)vValue.varSymbolID;
                        }
                        break;
                    case valueType_t.isFloatConst:
                        {
                            iVal = (UInt64)vValue.fFloatConst;
                        }
                        break;
                    case valueType_t.isString:
                        {
                            //if ( SCAN_FUNC(sStringVal.c_str(), "%d", &iVal) <= 0)
                            ScanFormatted sc = new ScanFormatted();
                            if (sc.Parse(sStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (UInt64)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.isWideString:
                        {
                            ScanFormatted sc = new ScanFormatted();
                            //if (wscanf(sWideStringVal.c_str(), L"%d", &iVal) <= 0)
                            if (sc.Parse(sWideStringVal, "%d") <= 0)
                            {
                                iVal = 0;
                            }
                            else
                            {
                                iVal = (UInt64)sc.Results[0];
                            }
                        }
                        break;
                    case valueType_t.invalid:
                    case valueType_t.isOpcode:
                    case valueType_t.isDepIndex:
                        {
                            iVal = 0;
                        }
                        break;
                    default:
                        {
                            iVal = 0;
                        }
                        break;
                }
                return iVal;
            }
        }

        public byte GetByte()
        {
            byte bVal;

            switch (vType)
            {
                case valueType_t.isBool:
                    {
                        bVal = (byte)((vValue.bIsTrue) ? 1 : 0);
                    }
                    break;
                case valueType_t.isIntConst:
                    {
                        if (vIsUnsigned)
                        {
                            bVal = (byte)(vValue.iIntConst);
                        }
                        else
                        {
                            bVal = (byte)vValue.iIntConst;
                        }
                    }
                    break;
                case valueType_t.isVeryLong:
                    {
                        if (vIsUnsigned)
                        { // bill doesn't support unsigned long long to double conversion (in VS6)
                            bVal = (byte)((vValue.longlongVal & 0x7fffffffffffffff));
                        }
                        else
                        {
                            bVal = (byte)vValue.longlongVal;
                        }
                    }
                    break;
                case valueType_t.isFloatConst:
                    {
                        bVal = (byte)vValue.fFloatConst;
                    }
                    break;
                case valueType_t.isString:
                    {
                        //if (SCAN_FUNC(sStringVal.c_str(), "%lf", &fVal) <= 0)
                        ScanFormatted sc = new ScanFormatted();
                        if (sc.Parse(sStringVal, "%f") <= 0)
                        {
                            bVal = 0;
                        }
                        else
                        {
                            bVal = (byte)sc.Results[0];
                        }
                    }
                    break;
                case valueType_t.isWideString:
                    {
                        //if (wscanf(sWideStringVal.c_str(), L"%f", &fVal) <= 0)
                        ScanFormatted sc = new ScanFormatted();
                        if (sc.Parse(sWideStringVal, "%f") <= 0)
                        {
                            bVal = 0;
                        }
                        else
                        {
                            bVal = (byte)sc.Results[0];
                        }
                    }
                    break;

                case valueType_t.invalid:
                case valueType_t.isOpcode:
                case valueType_t.isDepIndex:
                case valueType_t.isSymID:
                default:
                    {
                        bVal = 0; // error.
                    }
                    break;
            }
            return bVal;
        }

        public double GetDouble()
        {
            double fVal;

            switch (vType)
            {
                case valueType_t.isBool:
                    {
                        fVal = ((vValue.bIsTrue) ? 1.0 : 0.0);
                    }
                    break;
                case valueType_t.isIntConst:
                    {
                        if (vIsUnsigned)
                        {
                            fVal = (double)(vValue.iIntConst);
                        }
                        else
                        {
                            fVal = (double)vValue.iIntConst;
                        }
                    }
                    break;
                case valueType_t.isVeryLong:
                    {
                        if (vIsUnsigned)
                        { // bill doesn't support unsigned long long to double conversion (in VS6)
                            fVal = (double)((vValue.longlongVal & 0x7fffffffffffffff));


                            if (((UInt64)vValue.longlongVal & 0x8000000000000000) != 0)
                            {
                                fVal += 0x7fffffffffffffff; // highest positive value
                                fVal += 1;                  // but we need another...
                            }

                        }
                        else
                        {
                            fVal = (double)vValue.longlongVal;
                        }
                    }
                    break;
                case valueType_t.isFloatConst:
                    {
                        fVal = vValue.fFloatConst;
                    }
                    break;
                case valueType_t.isString:
                    {
                        //if (SCAN_FUNC(sStringVal.c_str(), "%lf", &fVal) <= 0)
                        ScanFormatted sc = new ScanFormatted();
                        if (sc.Parse(sStringVal, "%f") <= 0)
                        {
                            fVal = 0.0;
                        }
                        else
                        {
                            fVal = (double)sc.Results[0];
                        }
                    }
                    break;
                case valueType_t.isWideString:
                    {
                        //if (wscanf(sWideStringVal.c_str(), L"%f", &fVal) <= 0)
                        ScanFormatted sc = new ScanFormatted();
                        if (sc.Parse(sWideStringVal, "%f") <= 0)
                        {
                            fVal = 0.0;
                        }
                        else
                        {
                            fVal = (double)sc.Results[0];
                        }
                    }
                    break;

                case valueType_t.invalid:
                case valueType_t.isOpcode:
                case valueType_t.isDepIndex:
                case valueType_t.isSymID:
                default:
                    {
                        fVal = 0.0; // error.
                    }
                    break;
            }
            return fVal;
        }

        public float GetFloat()
        {
            float fVal;

            switch (vType)
            {
                case valueType_t.isBool:
                    {
                        fVal = ((vValue.bIsTrue) ? 1 : 0);
                    }
                    break;
                case valueType_t.isIntConst:
                    {
                        if (vIsUnsigned)
                        {
                            fVal = (float)((uint)vValue.iIntConst);
                        }
                        else
                        {
                            fVal = (float)vValue.iIntConst;
                        }
                    }
                    break;
                case valueType_t.isVeryLong:
                    {
                        if (vIsUnsigned)
                        { // bill doesn't support unsigned long long to double conversion (in VS6)
                            fVal = (float)((vValue.longlongVal & 0x7fffffffffffffff));


                            if (((UInt64)vValue.longlongVal & 0x8000000000000000) != 0)
                            {
                                fVal += 0x7fffffffffffffff; // highest positive value
                                fVal += 1;                  // but we need another...
                            }

                        }
                        else
                        {
                            fVal = (float)vValue.longlongVal;
                        }
                    }
                    break;
                case valueType_t.isFloatConst:
                    {
                        fVal = (float)vValue.fFloatConst;
                    }
                    break;
                case valueType_t.isString:
                    {
                        //if (SCAN_FUNC(sStringVal.c_str(), "%lf", &fVal) <= 0)
                        ScanFormatted sc = new ScanFormatted();
                        if (sc.Parse(sStringVal, "%f") <= 0)
                        {
                            fVal = 0;
                        }
                        else
                        {
                            fVal = (float)sc.Results[0];
                        }
                    }
                    break;
                case valueType_t.isWideString:
                    {
                        //if (wscanf(sWideStringVal.c_str(), L"%f", &fVal) <= 0)
                        ScanFormatted sc = new ScanFormatted();
                        if (sc.Parse(sWideStringVal, "%f") <= 0)
                        {
                            fVal = 0;
                        }
                        else
                        {
                            fVal = (float)sc.Results[0];
                        }
                    }
                    break;

                case valueType_t.invalid:
                case valueType_t.isOpcode:
                case valueType_t.isDepIndex:
                case valueType_t.isSymID:
                default:
                    {
                        fVal = 0; // error.
                    }
                    break;
            }
            return fVal;
        }

        public valueType_t GetValueType()
        {
            return vType;
        }

        public object GetValue(ref valueType_t type)
        {
            object ret;
            type = vType;
            switch (vType)
            {
                case valueType_t.isBool:
                    {

                        ret = (vValue.bIsTrue) ? true : false;
                    }
                    break;
                case valueType_t.isIntConst:
                    {
                        if (vIsUnsigned)
                        {
                            ret = vValue.iIntConst;
                        }
                        else
                        {
                            ret = vValue.iIntConst;
                        }
                    }
                    break;
                case valueType_t.isVeryLong:
                    {
                        Int64 data;
                        if (vIsUnsigned)
                        { // bill doesn't support unsigned long long to double conversion (in VS6)
                            data = vValue.longlongVal & 0x7fffffffffffffff;


                            if (((UInt64)vValue.longlongVal & 0x8000000000000000) != 0)
                            {
                                data += 0x7fffffffffffffff; // highest positive value
                                data += 1;                  // but we need another...
                            }
                            ret = data;
                        }
                        else
                        {
                            ret = vValue.longlongVal;
                        }
                    }
                    break;
                case valueType_t.isFloatConst:
                    {
                        ret = vValue.fFloatConst;
                    }
                    break;
                case valueType_t.isString:
                    {
                        //if (SCAN_FUNC(sStringVal.c_str(), "%lf", &fVal) <= 0)
                        ret = sStringVal;
                    }
                    break;
                case valueType_t.isWideString:
                    {
                        //if (wscanf(sWideStringVal.c_str(), L"%f", &fVal) <= 0)
                        ret = sWideStringVal;
                    }
                    break;

                case valueType_t.invalid:
                case valueType_t.isOpcode:
                case valueType_t.isDepIndex:
                case valueType_t.isSymID:
                default:
                    {
                        ret = 0; // error.
                    }
                    break;
            }
            return ret;
        }

        public string GetDispString(float factor)
        {
            string sVal = "";
            switch (vType)
            {
                case valueType_t.isWideString:
                    {
                        sVal = sWideStringVal;
                        // we won't deal with converting numerics to strings right now
                    }
                    break;

                case valueType_t.isString:
                case valueType_t.isPackedString:
                    {
                        sVal = sStringVal;
                        // we won't deal with converting numerics to strings right now
                    }
                    break;

                default:
                    sVal = (GetFloat() * factor).ToString();
                    break;
            }

            return sVal;
        }

        public string GetDispString()
        {
            string sVal = "";
            switch (vType)
            {
                case valueType_t.isWideString:
                    {
                        sVal = sWideStringVal;
                        // we won't deal with converting numerics to strings right now
                    }
                    break;

                case valueType_t.isString:
                case valueType_t.isPackedString:
                    {
                        sVal = sStringVal;
                        // we won't deal with converting numerics to strings right now
                    }
                    break;

                default:
                    sVal = GetFloat().ToString();
                    break;
            }

            return sVal;
        }

        public string GetString()
        {
            string sVal = "";
            if (vType == valueType_t.isWideString)
            {
                sVal = sWideStringVal;
                // we won't deal with converting numerics to strings right now
            }
            else if (vType == valueType_t.isString)
            {
                sVal = sStringVal; // UTF8 to Unicode conversion
                                   // we won't deal with converting numerics to strings right now
            }
            else if (vType == valueType_t.isPackedString)
            {
                sVal = sStringVal; // UTF8 to Unicode conversion
                                   // we won't deal with converting numerics to strings right now
            }
            else
            {
                sVal = getAsWString();
            }
            return sVal;
        }

        void clear()
        {
            vType = valueType_t.invalid;
            vValue.fFloatConst = 0;
            vSize = 0;
            vIsValid = false;
            vIsUnsigned = false;
            vIsDouble = false;
            //vIndex = 0;
            vIsBit = false;
            // erasing empty strings cause crashes
            sWideStringVal = "";
            sStringVal = "";
        }

        // for internal use only

        //static valueType_t promote(CValueVarient inOne, CValueVarient outOne, ref CValueVarient inTwo, ref CValueVarient outTwo)
        //{
        //    valueType_t retType = valueType_t.invalid;
        //    CValueVarient local = new CValueVarient();

        //    outOne = inOne;
        //    outTwo = inTwo;

        //    // they both have to be numeric to be here
        //    if ((!inOne.isNumeric()) || (!inTwo.isNumeric()))
        //    {
        //        return retType; // an error
        //    }

        //    INTEGER_RANK oneRank = new INTEGER_RANK(), twoRank = new INTEGER_RANK();
        //    INTEGER_RANK oneCnvt = new INTEGER_RANK(), twoCnvt = new INTEGER_RANK();

        //    /*	First, if the corresponding real type of either operand is long double, the other
        //    operand is converted, without change of type domain, to a type whose
        //    corresponding real type is long double.   */
        //    /*
        //    --- We don't support long double at this time
        //    */
        //    /*  Otherwise, if the corresponding real type of either operand is double, the other
        //    operand is converted, to a double. */
        //    if ((outOne.vType == valueType_t.isFloatConst && outOne.vIsDouble) &&
        //        (outTwo.vType != valueType_t.isFloatConst || !outTwo.vIsDouble))
        //    {
        //        local.clear();
        //        local = (double)outTwo;
        //        outTwo.clear();
        //        outTwo = local;
        //        retType = valueType_t.isFloatConst;
        //    }
        //    else if ((outOne.vType != valueType_t.isFloatConst || !outOne.vIsDouble) &&
        //             (outTwo.vType == valueType_t.isFloatConst && outTwo.vIsDouble))
        //    {
        //        local.clear();
        //        local = (double)outOne;
        //        outOne.clear();
        //        outOne = local;
        //        retType = valueType_t.isFloatConst;
        //    }
        //    else if ((outOne.vType == valueType_t.isFloatConst && outOne.vIsDouble) &&
        //             (outTwo.vType == valueType_t.isFloatConst && outTwo.vIsDouble))
        //    {
        //        local.clear();
        //        local = (double)outOne;
        //        outOne.clear();
        //        outOne = local;
        //        local.clear();
        //        local = (double)outTwo;
        //        outTwo.clear();
        //        outTwo = local;
        //        retType = isFloatConst;
        //    }

        //    /*  Otherwise, if the corresponding real type of either operand is float, the other
        //    operand is converted to a float.  */
        //    else if ((outOne.vType == valueType_t.isFloatConst && !outOne.vIsDouble) &&
        //             (outTwo.vType != valueType_t.isFloatConst || outTwo.vIsDouble))
        //    {
        //        local.clear();
        //        local = (float)outTwo;
        //        outTwo.clear();
        //        outTwo = local;
        //        retType = valueType_t.isFloatConst;
        //    }
        //    else if ((outOne.vType != valueType_t.isFloatConst || outOne.vIsDouble) &&
        //             (outTwo.vType == valueType_t.isFloatConst && !outTwo.vIsDouble))
        //    {
        //        local.clear();
        //        local = (float)outOne;
        //        outOne.clear();
        //        outOne = local;
        //        retType = valueType_t.isFloatConst;
        //    }
        //    else if ((outOne.vType == valueType_t.isFloatConst && !outOne.vIsDouble) &&
        //             (outTwo.vType == valueType_t.isFloatConst && !outTwo.vIsDouble))
        //    {
        //        local.clear();
        //        local = outOne;
        //        outOne.clear();
        //        outOne = local;
        //        local.clear();
        //        local = outTwo;
        //        outTwo.clear();
        //        outTwo = local;
        //        retType = valueType_t.isFloatConst;
        //    }
        //    // else: neither is double nor float so fall thru to int handling

        //    /*  Otherwise, the integer promotions are performed on both operands. Then the
        //following rules are applied to the promoted operands:   */
        //    if (retType == valueType_t.invalid) // no float types...
        //    {
        //        if (outOne.vType == valueType_t.isBool)
        //        {
        //            oneRank.is_unsigned = false;
        //            oneRank.rank = 1;
        //        }
        //        else if (outOne.vType == valueType_t.isIntConst)
        //        {
        //            if (outOne.vIsUnsigned)
        //                oneRank.is_unsigned = true;
        //            else // not unsigned => is signed
        //                oneRank.is_unsigned = false;

        //            switch (outOne.vSize)
        //            {
        //                case 1: // char & byte
        //                    oneRank.rank = 2;
        //                    break;
        //                case 2: // short & unsigned short
        //                    oneRank.rank = 3;
        //                    break;
        //                case 4: // int & unsigned int
        //                    oneRank.rank = 4;
        //                    break;
        //                default:            //0,3
        //                    return valueType_t.invalid; // error return
        //                    //break;
        //            }
        //        }
        //        else if (outOne.vType == valueType_t.isVeryLong)
        //        {
        //            if (outOne.vIsUnsigned)
        //                oneRank.is_unsigned = true;
        //            else // not unsigned => is signed
        //                oneRank.is_unsigned = false;

        //            if (outOne.vSize == 8)
        //            {
        //                oneRank.rank = 5;
        //            }
        //            else // size 5,6,7 & > 8
        //            {
        //                return valueType_t.invalid; // error return
        //            }
        //        }
        //        else
        //        {
        //            return valueType_t.invalid; // error return
        //        }

        //        if (outTwo.vType == valueType_t.isBool)
        //        {
        //            twoRank.is_unsigned = false;
        //            twoRank.rank = 1;
        //        }
        //        else if (outTwo.vType == valueType_t.isIntConst)
        //        {
        //            if (outTwo.vIsUnsigned)
        //                twoRank.is_unsigned = true;
        //            else // not unsigned => is signed
        //                twoRank.is_unsigned = false;

        //            switch (outTwo.vSize)
        //            {
        //                case 1: // char & byte
        //                    twoRank.rank = 2;
        //                    break;
        //                case 2: // short & unsigned short
        //                    twoRank.rank = 3;
        //                    break;
        //                case 4: // int & unsigned int
        //                    twoRank.rank = 4;
        //                    break;
        //                default:            //0,3
        //                    return valueType_t.invalid; // error return
        //                    //break;
        //            }
        //        }
        //        else if (outTwo.vType == valueType_t.isVeryLong)
        //        {
        //            if (outTwo.vIsUnsigned)
        //                twoRank.is_unsigned = true;
        //            else // not unsigned => is signed
        //                twoRank.is_unsigned = false;

        //            if (outTwo.vSize == 8)
        //            {
        //                twoRank.rank = 5;
        //            }
        //            else // size 5,6,7 & > 8
        //            {
        //                return valueType_t.invalid; // error return
        //            }
        //        }
        //        else
        //        {
        //            return valueType_t.invalid; // error return
        //        }

        //        /* If both operands have the same type, then no further conversion is needed. */
        //        if (outOne.vType == outTwo.vType &&
        //            outOne.vIsUnsigned == outTwo.vIsUnsigned &&
        //            outOne.vIsDouble == outTwo.vIsDouble)
        //        {
        //            return outTwo.vType; // done
        //        }

        //        /* Otherwise, if both operands have signed integer types or both have unsigned
        //        integer types, the operand with the type of lesser integer conversion rank is
        //        converted to the type of the operand with greater rank. */
        //        if (((oneRank.is_unsigned) && (twoRank.is_unsigned)) ||
        //            ((!oneRank.is_unsigned) && (!twoRank.is_unsigned)))
        //        { // lower to higher
        //          //oneCnvt,twoCnvt;
        //            if (oneRank.rank > twoRank.rank)
        //            {
        //                twoCnvt = oneRank; // other stays empty
        //            }
        //            else
        //            {
        //                oneCnvt = twoRank; // other stays empty
        //            }
        //        }
        //        else // one is signed, the other is unsigned
        //            /* Otherwise, if the operand that has unsigned integer type has rank greater or
        //   equal to the rank of the type of the other operand, then the operand with
        //   signed integer type is converted to the type of the operand with unsigned
        //   integer type.*/
        //            if (oneRank.is_unsigned && oneRank.rank >= twoRank.rank)
        //        {                      // two converted to one's type
        //            twoCnvt = oneRank; // other stays empty
        //        }
        //        else if (twoRank.is_unsigned && twoRank.rank >= oneRank.rank)
        //        {                      // one converted to two's type
        //            oneCnvt = twoRank; // other stays empty
        //        }
        //        else
        //            /* Otherwise, if the type of the operand with signed integer type can represent
        //                    all of the values of the type of the operand with unsigned integer type, then
        //                    the operand with unsigned integer type is converted to the type of the
        //                    operand with signed integer type.  */
        //            if ((!oneRank.is_unsigned) && oneRank.rank > twoRank.rank)
        //        {                      //two converted to one's type
        //            twoCnvt = oneRank; // other stays empty
        //        }
        //        else if ((!twoRank.is_unsigned) && twoRank.rank > oneRank.rank)
        //        {                      // one converted to two's type
        //            oneCnvt = twoRank; // other stays empty
        //        }
        //        else
        //            /* Otherwise, both operands are converted to the unsigned integer type
        //                            corresponding to the type of the operand with signed integer type.	*/
        //            if (oneRank.is_unsigned)     // two is SIGNED
        //        {                                //both to twoRank.rank and unsigned
        //            twoCnvt = oneCnvt = twoRank; // other stays empty
        //            twoCnvt.is_unsigned = true;
        //            oneCnvt.is_unsigned = true;
        //        }
        //        else                             // one is SIGNED
        //        {                                //both to oneRank.rank and unsigned
        //            twoCnvt = oneCnvt = oneRank; // other stays empty
        //            twoCnvt.is_unsigned = true;
        //            oneCnvt.is_unsigned = true;
        //        }

        //        // do the conversion(s)
        //        if (oneCnvt.rank > 0)
        //        { // convert oneOut to oneCnvt type
        //            switch (oneCnvt.rank)
        //            {
        //                case 1: // bool
        //                    {
        //                        local.clear();
        //                        local = (bool)outOne;
        //                        outOne.clear();
        //                        outOne = local;
        //                        retType = isBool;
        //                    }
        //                    break;
        //                case 2: // char
        //                    {
        //                        if (oneCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (byte)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isIntConst; //RUL_UNSIGNED_CHAR;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (char)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isIntConst; //RUL_CHAR;
        //                        }
        //                    }
        //                    break;
        //                case 3: // short
        //                    {
        //                        if (oneCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (unsigned short)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isIntConst; //RUL_USHORT;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (short)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isIntConst; //RUL_SHORT;
        //                        }
        //                    }
        //                    break;
        //                case 4: // int
        //                    {
        //                        if (oneCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (unsigned int)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isIntConst; //RUL_UINT;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (int)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isIntConst; //RUL_INT;
        //                        }
        //                    }
        //                    break;
        //                case 5: // long long
        //                    {
        //                        if (oneCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (UINT64)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isVeryLong; //RUL_ULONGLONG;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (INT64)outOne;
        //                            outOne.clear();
        //                            outOne = local;
        //                            retType = valueType_t.isVeryLong; //RUL_LONGLONG;
        //                        }
        //                    }
        //                    break;
        //                default:
        //                    outOne.clear();    // error
        //                    retType = valueType_t.invalid; //RUL_null;
        //                    break;
        //            } // endswitch
        //        }     // else no conversion on one

        //        if (twoCnvt.rank > 0)
        //        { // convert twoOut to twoCnvt type
        //            switch (twoCnvt.rank)
        //            {
        //                case 1: // bool
        //                    {
        //                        local.clear();
        //                        local = (bool)outTwo;
        //                        outTwo.clear();
        //                        outTwo = local;
        //                        retType = valueType_t.isBool; //RUL_BOOL;
        //                    }
        //                    break;
        //                case 2: // char
        //                    {
        //                        if (twoCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (byte)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isIntConst; //RUL_UNSIGNED_CHAR;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (char)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isIntConst; //RUL_CHAR;
        //                        }
        //                    }
        //                    break;
        //                case 3: // short
        //                    {
        //                        if (twoCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (unsigned short)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isIntConst; //RUL_USHORT;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (short)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isIntConst; //RUL_SHORT;
        //                        }
        //                    }
        //                    break;
        //                case 4: // int
        //                    {
        //                        if (twoCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (unsigned int)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isIntConst; //RUL_UINT;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (int)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isIntConst; //RUL_INT;
        //                        }
        //                    }
        //                    break;
        //                case 5: // long long
        //                    {
        //                        if (twoCnvt.is_unsigned)
        //                        {
        //                            local.clear();
        //                            local = (UINT64)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isVeryLong; //RUL_ULONGLONG;
        //                        }
        //                        else //signed
        //                        {
        //                            local.clear();
        //                            local = (INT64)outTwo;
        //                            outTwo.clear();
        //                            outTwo = local;
        //                            retType = valueType_t.isVeryLong; //RUL_LONGLONG;
        //                        }
        //                    }
        //                    break;
        //                default:
        //                    outTwo.clear();    // error
        //                    retType = valueType_t.invalid; //RUL_null;
        //                    break;
        //            } // endswitch
        //        }     // else no conversion on two

        //    } //else let the float types stay
        //    return retType;
        //}

        string getAsWString()
        {
            string wS;

            if (vIsValid)
            {
                switch (vType)
                {
                    case valueType_t.invalid:
                        wS = "*INVALID*";
                        break;
                    case valueType_t.isBool:
                        wS = (vValue.bIsTrue) ? "TRUE" : "FALSE";
                        break;
                    case valueType_t.isOpcode:
                        {
                            switch ((expressElemType_t)(vValue.iOpCode))
                            {
                                case expressElemType_t.eet_NOT: //_OPCODE 1
                                    wS = "NOT";
                                    break;
                                case expressElemType_t.eet_NEG: //_OPCODE 2
                                    wS = "NEG";
                                    break;
                                case expressElemType_t.eet_BNEG: //_OPCODE 3
                                    wS = "BNEG";
                                    break;
                                case expressElemType_t.eet_ADD: //_OPCODE 4
                                    wS = "ADD";
                                    break;
                                case expressElemType_t.eet_SUB: //_OPCODE 5
                                    wS = "SUB";
                                    break;
                                case expressElemType_t.eet_MUL: //_OPCODE 6
                                    wS = "MU";
                                    break;
                                case expressElemType_t.eet_DIV: //_OPCODE 7
                                    wS = "DIV";
                                    break;
                                case expressElemType_t.eet_MOD: //_OPCODE 8
                                    wS = "MOD";
                                    break;
                                case expressElemType_t.eet_LSHIFT: //_OPCODE 9
                                    wS = "LSHIFT";
                                    break;
                                case expressElemType_t.eet_RSHIFT: //_OPCODE 10
                                    wS = "RSHIFT";
                                    break;
                                case expressElemType_t.eet_AND: //_OPCODE 11
                                    wS = "AND";
                                    break;
                                case expressElemType_t.eet_OR: //_OPCODE 12
                                    wS = "OR";
                                    break;
                                case expressElemType_t.eet_XOR: //_OPCODE 13
                                    wS = "XOR";
                                    break;
                                case expressElemType_t.eet_LAND: //_OPCODE 14
                                    wS = "LOGICALAND";
                                    break;
                                case expressElemType_t.eet_LOR: //_OPCODE 15
                                    wS = "LOGICALOR";
                                    break;
                                case expressElemType_t.eet_LT: //_OPCODE 16
                                    wS = "LESSTHAN";
                                    break;
                                case expressElemType_t.eet_GT: //_OPCODE 17
                                    wS = "GREATERTHAN";
                                    break;
                                case expressElemType_t.eet_LE: //_OPCODE 18
                                    wS = "LESSTHANOREQUA";
                                    break;
                                case expressElemType_t.eet_GE: //_OPCODE 19
                                    wS = "GREATERTHANOREQUA";
                                    break;
                                case expressElemType_t.eet_EQ: //_OPCODE 20
                                    wS = "EQUA";
                                    break;
                                case expressElemType_t.eet_NEQ: //_OPCODE 21
                                    wS = "NOTEQUA";
                                    break;
                                default:
                                    //LOGIT(CERR_LOG, "VARIENT: Tried to output an unknown OpCode: %d\n", vValue.iOpCode);
                                    wS = "*UNKNOWN_OPCODE*";
                                    break;
                            }
                        }
                        break;
                    case valueType_t.isIntConst:
                        wS = String.Format("const %d (0x%02x)", vValue.iIntConst, vValue.iIntConst);
                        break;
                    case valueType_t.isFloatConst:
                        //swprintf(tmpBuff, 68, "const %f (0x%02x)", vValue.fFloatConst, vValue.iIntConst);
                        wS = String.Format("const %f (0x%02x)", vValue.fFloatConst, vValue.iIntConst);
                        break;
                    case valueType_t.isDepIndex:
                        //swprintf(tmpBuff, 68, "*DEPIDX( %d (0x%02x))*", vValue.depIndex, vValue.iIntConst);
                        wS = String.Format("*DEPIDX( %d (0x%02x))*", vValue.depIndex, vValue.iIntConst);
                        break;
                    case valueType_t.isString:
                        wS = "String const |";
                        wS += sStringVal;
                        wS += "|";
                        break;
                    case valueType_t.isSymID:
                        //sprintf(tmpBuff, 68, "Symbol(0x%04x)", vValue.varSymbolID);
                        wS = String.Format("Symbol(0x%04x)", vValue.varSymbolID);
                        if (vIsBit)
                        {
                            //swprintf(tmpBuff, 68, "[0x%02x]", vValue.varSymbolID);
                            wS += String.Format("[0x%02x]", vValue.varSymbolID);
                        }
                        break;
                    case valueType_t.isVeryLong: // added 2sept08 stevev
                        //swprintf(tmpBuff, 68, "const %I64d(0x%04I64x)", vValue.longlongVal, vValue.longlongVal);
                        wS = String.Format("const %I64d(0x%04I64x)", vValue.longlongVal, vValue.longlongVal);
                        break;
                    case valueType_t.isWideString: // added 2sept08 stevev
                                                   // wouldn't take a straight wstring, We'll just output the first char for now
                        wS = "WideString const |";
                        wS += sWideStringVal;
                        wS += "|";
                        break;
                    default:
                        //LOGIT(CERR_LOG, "VARIENT: Tried to output an unknown type:%d\n", vType);
                        wS = "*UNKNOWN_TYPE*";
                        break;
                }
            }
            else
            {
                wS = "*FLAGGED_INVALID*";
            }
            return wS;
        }
    }

    public enum UI_DATA_TYPE
    {
        UNKNOWN_UI_DATA_TYPE = 0,
        TEXT_MESSAGE,
        EDIT,
        COMBO,
        HARTDATE,
        VARIABLES_CHANGED_MSG,
        PLOT,    //Vibhor 071204: Chart / Graph
        MENU,   // stevev 09aug05 - displayMenu() - button Names in combo list
        TIME,   // TIME_VALUE could be int,float, or tod
    }

    public class CExpParser
    {
        CToken m_Dollar;
        Stack<CToken> m_Terminals;
        Stack<CExpression> m_NonTerminals;
        public static Dictionary<int, Precedence> s_PrecedenceTable = new Dictionary<int, Precedence>();
        public const int C_EP_ERROR_ILLEGALOP = 0x03000001;

        public CExpParser()
        {
            m_Dollar = new CToken("$", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_DOLLAR, -1);
            CToken pToken = new CToken("$", RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_DOLLAR, -1);

            m_Terminals = new Stack<CToken>();
            m_NonTerminals = new Stack<CExpression>();

            m_Terminals.Push(pToken);
        }

        CToken CGetActualToken(ref CLexicalAnalyzer plexAnal, CSymbolTable pSymbolTable, STMT_EXPR_TYPE expr, int i32BrackCount, CToken ppNextToken, ref bool bIsShiftOver)
        {
            CToken pToken = null;
            if (((expr == STMT_EXPR_TYPE.EXPR_LVALUE) && (ppNextToken).IsAssignOp())
                || ((expr == STMT_EXPR_TYPE.EXPR_ASSIGN) && (ppNextToken).IsEOS())
                || ((expr == STMT_EXPR_TYPE.EXPR_IF) && i32BrackCount == 0)
                || ((expr == STMT_EXPR_TYPE.EXPR_WHILE) && i32BrackCount == 0)
                || ((expr == STMT_EXPR_TYPE.EXPR_WHILE) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COMMA))
                || ((expr == STMT_EXPR_TYPE.EXPR_ASSIGN) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_QMARK))
                || ((expr == STMT_EXPR_TYPE.EXPR_ASSIGN) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COLON))
                || ((expr == STMT_EXPR_TYPE.EXPR_FOR) && i32BrackCount == 0)
                || ((expr == STMT_EXPR_TYPE.EXPR_FOR) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON))
                || ((expr == STMT_EXPR_TYPE.EXPR_CASE) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COLON)))
            {
                bIsShiftOver = true;
                pToken = m_Dollar;
            }
            else
            {

                //If the expression contains an OM expression, then
                //1.	Parse it using the COMServiceExpression
                //2.	Push it into NT stack(ie we are by-passing the reduxion to Expression).
                if ((ppNextToken).IsOMToken())
                {
                    plexAnal.UnGetToken();

                    COMServiceExpression pExpression = new COMServiceExpression();
                    pExpression.CreateParseSubTree(ref plexAnal, ref pSymbolTable);

                    m_NonTerminals.Push(pExpression);

                }
                else if ((ppNextToken).IsFunctionToken())
                {
                    plexAnal.UnGetToken();

                    FunctionExpression pExpression = new FunctionExpression();
                    pExpression.CreateParseSubTree(ref plexAnal, ref pSymbolTable);

                    m_NonTerminals.Push(pExpression);
                }
                else
                {
                    pToken = ppNextToken;
                }
            }
            return pToken;
        }


        public CExpression ParseExpression(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable, STMT_EXPR_TYPE expr, bool bLookForQMark = true)
        {
            CToken pToken = null;
            CToken pNextToken = null;
            bool bProcessingQMark = false;

            if (bLookForQMark && !bProcessingQMark)
            {
                if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_QMARK, ref pToken))
                {
                    if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_SYMBOL, RUL_TOKEN_SUBTYPE.RUL_COLON, ref pToken))
                    {
                        IFExpression pIfExpression = new IFExpression();
                        bProcessingQMark = true;
                        pIfExpression.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                        bProcessingQMark = false;
                        return (CExpression)pIfExpression;
                    }
                }
            }

            int i32BrackCount = 0;
            bool bIsShiftOver = false;
            int nLastTokenState = CLexicalAnalyzer.LEX_FAIL;  // ** Walt EPM 08sep08

            //try
            {
                if ((expr == STMT_EXPR_TYPE.EXPR_IF) || (expr == STMT_EXPR_TYPE.EXPR_WHILE) || (expr == STMT_EXPR_TYPE.EXPR_FOR))
                {
                    i32BrackCount++; //we count the lparen now;
                }
                while ((!bIsShiftOver || (m_Terminals.Count > 1))
                    && (CLexicalAnalyzer.LEX_FAIL != (nLastTokenState = plexAnal.GetNextToken(ref pNextToken, ref pSymbolTable)))// ** Walt EPM 08sep08
                                                                                                                             //&& (LEX_FAIL != plexAnal.GetNextToken(&pNextToken,pSymbolTable))
                    && pNextToken != null)
                {
                    if (pNextToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_LPAREN)
                    {
                        i32BrackCount++;
                    }
                    else if (pNextToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_RPAREN)
                    {
                        i32BrackCount--;
                    }
                    /*			if (pNextToken.GetSubType() == RUL_COLON)
                                {
                                    while (!m_Terminals.empty())
                                        m_Terminals.pop();
                                    break;
                                }*/

                    pToken = GetActualToken(ref plexAnal, ref pSymbolTable, expr, i32BrackCount, pNextToken, bIsShiftOver);
                    if (null == (pToken))
                    {
                        continue;
                    }
                    // OK. we are in the loop only if
                    //a.	the expr is an assign and the Token is not a ;
                    //b.	the expr is an if and the the paren count is non-zero
                    //c.	the expr is an while and the the paren count is non-zero
                    if ((pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_DOLLAR)
                        && (m_Terminals.Peek().GetSubType() == RUL_TOKEN_SUBTYPE.RUL_DOLLAR))
                    {
                        break;
                    }

                    /* VMKP added on 030404 */
                    /* Fixed the problem in Low Pressure and High Pressure
                      issue in 0x3e Manufacturer DD */
                    if ((pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_LPAREN)
                        //				&& (m_NonTerminals.top().GetSubType() == RUL_LPAREN)
                        && (m_NonTerminals.Count > 0)
                        && (m_Terminals.Count < 2)
                        && i32BrackCount == 2)
                    {
                        break;
                    }
                    /* VMKP added on 030404 */
                    if (IncomingTokenPrecedence(pToken.GetSubType()) >= StackTopTokenPrecedence(m_Terminals.Peek().GetSubType()))
                    {
                        m_Terminals.Push(pToken);
                    }
                    else// if(IncomingTokenPrecedence(pToken.GetSubType()) < StackTopTokenPrecedence((m_Terminals.top()).GetSubType()))
                    {
                        RUL_TOKEN_SUBTYPE SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;

                        Reduce(SubType, ref pSymbolTable, ref plexAnal);//Anil August 26 2005 For handling DD variable and Expression
                        while (IncomingTokenPrecedence(SubType) <= StackTopTokenPrecedence((m_Terminals.Peek()).GetSubType()))
                        {
                            if (m_Terminals.Peek().GetSubType() == RUL_TOKEN_SUBTYPE.RUL_DOLLAR)
                            {
                                break;
                            }
                            SubType = m_Terminals.Peek().GetSubType();
                            //delete m_Terminals.top();
                            m_Terminals.Pop();
                        }

                        if (pNextToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_RPAREN)
                        {
                            i32BrackCount++;
                        }
                        plexAnal.UnGetToken();
                    }
                    pNextToken = null;
                }

                plexAnal.UnGetToken();
                if (nLastTokenState == CLexicalAnalyzer.LEX_FAIL)    // ** Walt EPM 08sep08
                {
                    ;//throw (uint)C_EP_ERROR_LEXERROR;
                }

                if (m_Terminals.Count > 1)
                {
                    ;//throw (uint)C_EP_ERROR_MISSINGSC;
                }

                if ((expr != STMT_EXPR_TYPE.EXPR_IF) && (expr != STMT_EXPR_TYPE.EXPR_WHILE) && (expr != STMT_EXPR_TYPE.EXPR_FOR))
                {
                    if (i32BrackCount != 0)
                    {
                        ;//throw (uint)C_EP_ERROR_MISSINGPAREN;
                    }
                }

                /*	Walt:EPM when m_NonTerminals[0] == 0  then the call m_NonTerminals.top() will blow up;
                        if(m_NonTerminals.top())
                        {
                            if((m_NonTerminals.Count > 1) && (expr != EXPR_FOR) )
                                if (expr != EXPR_ASSIGN)
                                    throw (uint)C_EP_ERROR_MISSINGOP;

                            return m_NonTerminals.top();
                        }
                    Replace with below::>
                */
                if (m_NonTerminals.Count > 0)
                {
                    if (m_NonTerminals.Peek() != null)
                    {
                        if ((m_NonTerminals.Count > 1) && (expr != STMT_EXPR_TYPE.EXPR_FOR))
                        {
                            if ((expr != STMT_EXPR_TYPE.EXPR_ASSIGN) && (expr != STMT_EXPR_TYPE.EXPR_WHILE))// emerson checkin april2013
                            {
                                //throw (uint)C_EP_ERROR_MISSINGOP;
                            }
                        }

                        return m_NonTerminals.Peek();//.top?
                    }
                }
                else
                {
                    ;//throw (uint)C_EP_ERROR_MISSINGPAREN;
                }
            }
            /*
            catch (uint error)
            {
                if (error == C_EP_ERROR_LEXERROR)// ** Walt EPM 08sep08
                {
                }
                else
                {
                    //clean up the terminal stack and the non-terminal stack
                    plexAnal.UnGetToken();
                    EmptyStacks(true, true);
                    DELETE_PTR(pNextToken);
                }
                throw (error);
            }
            catch (...)
	        {
                //clean up the terminal stack and the non-terminal stack
                EmptyStacks(true, true);
                DELETE_PTR(pNextToken);
                throw (C_EP_ERROR_UNKNOWN);
            }
            */
            return null;
        }

        public void Reduce(RUL_TOKEN_SUBTYPE SubType, ref CSymbolTable pSymbolTable, ref CLexicalAnalyzer plexAnal)//Anil August 26 2005 for //Handling DD variable and Expression. to get the MEE ptr throgh plexAnal
        {
            CExpression pExpression = null;
            CToken pToken = m_Terminals.Peek();
            int nNonTerminals = s_PrecedenceTable[(int)pToken.GetSubType()].n;

            //try
            {
                SubType = pToken.GetSubType();
                m_Terminals.Pop();

                if (pToken.IsVariable() || pToken.IsNumeric() || pToken.IsConstant())
                {
                    pExpression = new CPrimaryExpression(pToken);
                }

                else if (pToken.IsArrayVar())
                {
                    pExpression = new CArrayExpression(pToken);
                }
                //Added By Anil August 4 2005 --starts here
                //If it is DD item then Form the Expression as ComplexDDExpression
                else if (pToken.IsDDItem())
                {
                    pExpression = new CComplexDDExpression(pToken);
                    InsertDDExpr(pToken.GetLexeme(), pExpression, ref pSymbolTable, ref plexAnal, pToken);//Anil Octobet 5 2005 for handling Method Calling Method
                }
                //Added By Anil August 4 2005 --Ends here
                else    //operator
                {
                    CExpression pNTExpression1 = null;
                    CExpression pNTExpression2 = null;
                    switch (nNonTerminals)
                    {
                        case 2:
                            pNTExpression1 = m_NonTerminals.Peek();
                            m_NonTerminals.Pop();
                            pNTExpression2 = m_NonTerminals.Peek();
                            m_NonTerminals.Pop();

                            pExpression = new CCompoundExpression(pNTExpression2, pNTExpression1, pToken.GetSubType());
                            break;
                        case 1:
                            if (RUL_TOKEN_SUBTYPE.RUL_RBOX != pToken.GetSubType())
                            {
                                pNTExpression1 = m_NonTerminals.Peek();
                                m_NonTerminals.Pop();

                                pExpression = new CCompoundExpression(pNTExpression1, null, pToken.GetSubType());
                            }
                            else
                            {
                                pNTExpression1 = m_NonTerminals.Peek();  //array dim expr
                                m_NonTerminals.Pop();

                                pNTExpression2 = m_NonTerminals.Peek();  //array identifier
                                m_NonTerminals.Pop();

                                ((CArrayExpression)pNTExpression2).AddDimensionExpr(pNTExpression1);
                                pExpression = pNTExpression2;
                            }
                            break;
                        case 0:
                            pExpression = null;
                            break;
                    }

                    if (s_PrecedenceTable[(int)(m_Terminals.Peek().GetSubType())].t != 0)
                    {   //for the moment this is '(' or '['
                        pToken = m_Terminals.Peek();
                        SubType = pToken.GetSubType();
                        m_Terminals.Pop();
                    }
                }

                if (pExpression != null)
                {
                    m_NonTerminals.Push(pExpression);
                }
            }
            /*
            catch (...)
	        {
                DELETE_PTR(pExpression);
                throw;
            }
            */
        }

        public void EmptyStacks(bool IsTerminal, bool IsNonTerminal)
        {
            if (IsTerminal)
            {
                m_Terminals.Clear();
                /*
                CToken pToken = null;
                while (!m_Terminals.empty())
                {
                    pToken = m_Terminals.Peek();
                    delete pToken;
                    pToken = 0;

                    m_Terminals.Pop();
                }
                */
            }
            if (IsNonTerminal)
            {
                m_NonTerminals.Clear();
                /*
                //CExpression pExp = null;
                while (!m_NonTerminals.empty())
                {
                    pExp = m_NonTerminals.Peek();
                    delete pExp;
                    pExp = 0;

                    m_NonTerminals.Pop();
                }
                */
            }
        }

        public int IncomingTokenPrecedence(RUL_TOKEN_SUBTYPE SubType)
        {
            /*
            PRECEDENCE_TABLE::iterator myIt;
            myIt = s_PrecedenceTable.Find(SubType);
            if (myIt == s_PrecedenceTable.end())
                throw (_UINT32)C_EP_ERROR_ILLEGALOP;
                */
            if (s_PrecedenceTable.ContainsKey((int)SubType))
            {
                return s_PrecedenceTable[(int)SubType].Incoming;
            }
            else
            {
                return C_EP_ERROR_ILLEGALOP;
            }
        }

        public int StackTopTokenPrecedence(RUL_TOKEN_SUBTYPE SubType)
        {
            if (s_PrecedenceTable.ContainsKey((int)SubType))
            {
                return s_PrecedenceTable[(int)SubType].StackTop;
            }
            else
            {
                return C_EP_ERROR_ILLEGALOP;
            }
        }

        public CToken GetActualToken(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable, STMT_EXPR_TYPE expr,
            int i32BrackCount, CToken ppNextToken, bool bIsShiftOver)
        {
            CToken pToken = null;
            if (((expr == STMT_EXPR_TYPE.EXPR_LVALUE) && (ppNextToken).IsAssignOp())
                || ((expr == STMT_EXPR_TYPE.EXPR_ASSIGN) && (ppNextToken).IsEOS())
                || ((expr == STMT_EXPR_TYPE.EXPR_IF) && i32BrackCount == 0)
                || ((expr == STMT_EXPR_TYPE.EXPR_WHILE) && i32BrackCount == 0)
                || ((expr == STMT_EXPR_TYPE.EXPR_WHILE) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COMMA))
                || ((expr == STMT_EXPR_TYPE.EXPR_ASSIGN) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_QMARK))
                || ((expr == STMT_EXPR_TYPE.EXPR_ASSIGN) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COLON))
                || ((expr == STMT_EXPR_TYPE.EXPR_FOR) && i32BrackCount == 0)
                || ((expr == STMT_EXPR_TYPE.EXPR_FOR) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON))
                || ((expr == STMT_EXPR_TYPE.EXPR_CASE) && ((ppNextToken).GetSubType() == RUL_TOKEN_SUBTYPE.RUL_COLON))
                )
            {
                bIsShiftOver = true;
                pToken = m_Dollar;
            }
            else
            {

                //If the expression contains an OM expression, then
                //1.	Parse it using the COMServiceExpression
                //2.	Push it into NT stack(ie we are by-passing the reduxion to Expression).
                if ((ppNextToken).IsOMToken())
                {
                    plexAnal.UnGetToken();

                    COMServiceExpression pExpression = null;
                    pExpression = new COMServiceExpression();
                    pExpression.CreateParseSubTree(ref plexAnal, ref pSymbolTable);

                    m_NonTerminals.Push(pExpression);

                }
                else
                if ((ppNextToken).IsFunctionToken())
                {
                    plexAnal.UnGetToken();

                    FunctionExpression pExpression = null;
                    pExpression = new FunctionExpression();
                    pExpression.CreateParseSubTree(ref plexAnal, ref pSymbolTable);

                    m_NonTerminals.Push(pExpression);
                }
                else
                {
                    pToken = ppNextToken;
                }
            }
            return pToken;
        }

        //Anil August 26 2005
        //for handling DD variable and Expression
        //This is to get any expression within [] and to isert in to DDcomplexExp class
        public bool InsertDDExpr(string pszComplexDDExpre, CExpression DDArrayExpression, ref CSymbolTable pSymbolTable,ref CLexicalAnalyzer plexAnal, CToken pToken)
        {
            CExpParser expParser = new CExpParser();

            if ((RUL_TOKEN_TYPE.RUL_DD_ITEM == pToken.GetType()) && (RUL_TOKEN_SUBTYPE.RUL_DD_METHOD == pToken.GetSubType()))
            {
                // ** Walt EPM 08sep08
                int nSizeOfBuffer = pszComplexDDExpre.Length + 1;
                string pchDecSource = ""; //make sure to clear it
                                          // ** end  Walt EPM 08sep08
                int iLeftPeranthis = 0;
                bool bValidMethodCall = false;
                int i = pToken.GetDDItemName().Length;

                for (; i < pszComplexDDExpre.Length; i++) // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>

                {
                    if (pszComplexDDExpre[i] == '(')
                    {
                        bValidMethodCall = true;
                        iLeftPeranthis = 1;
                        i++;
                        break;
                    }
                }
                if (bValidMethodCall == false)
                {
                    return false;
                }

                int iNoOfchar = 0;
                int lstlen = pszComplexDDExpre.Length;
                for (; i < lstlen; i++)
                {
                    if ((' ' != pszComplexDDExpre[i]))
                    {
                        //pchDecSource[iNoOfchar] = pszComplexDDExpre[i];// ** Walt EPM 08sep08
                        //pchDecSource = pchDecSource.Remove(iNoOfchar, 1);//dont need remove
                        //pchDecSource = pchDecSource.Insert(iNoOfchar, pszComplexDDExpre[i].ToString());
                        iNoOfchar++;
                        pchDecSource += pszComplexDDExpre[i].ToString();
                    }
                    if (')' == pszComplexDDExpre[i])
                    {
                        iLeftPeranthis--;

                    }
                    if ('(' == pszComplexDDExpre[i])
                    {
                        iLeftPeranthis++;

                    }
                    if (((pszComplexDDExpre[i] == ',') || (0 == iLeftPeranthis)) && (iNoOfchar > 1))
                    {
                        //do insert here
                        /*** Walt EPM 08sep08				
                        int istartPosOfPassedItem = 0 ;
                        iNoOfchar--;//Because ; or ) is included
                        int iNoOfSpaces = 0;
                        for(int x = i-1; ;x--)
                        {
                            if(' ' == pszComplexDDExpre[x])
                            {
                                iNoOfSpaces++;

                            }
                            else
                            {
                                break;
                            }

                        }
                        istartPosOfPassedItem = i - iNoOfchar - iNoOfSpaces ;

                        int iCount = iNoOfchar + 1 + 1;
                        char* pchDecSource = new char[ iCount ];// +1 for Null Char +1 for ; -1 for as it had counted ]
                        memset(pchDecSource,0,iCount);				
                        strncpy(pchDecSource,(const char*)&pszComplexDDExpre[istartPosOfPassedItem],iNoOfchar);
                        strcat(pchDecSource, ";");
                        pchDecSource[iCount - 1] = '\0';
                        *** end Walt EPM 08sep08 */

                        //pchDecSource[iNoOfchar - 1] = ';';//last character may be a ")" or a ";" // Walt EPM 08sep08
                        pchDecSource += ";";

                        //Form this as new Lexical, and load it in to it
                        CLexicalAnalyzer clexAnalTemp = new CLexicalAnalyzer();
                        clexAnalTemp.InitMeeInterface(plexAnal.GetMEEInterface());
                        clexAnalTemp.Load(pchDecSource, "test");
                        //Call Parse Expression to get the Expression class
                        CExpression pExpression = expParser.ParseExpression(ref clexAnalTemp, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN);
                        //Insert this exp in to the main DDComplex expression class
                        if (pExpression != null)
                        {
                            ((CComplexDDExpression)DDArrayExpression).AddDimensionExpr(pExpression);
                            /* Walt EPM 08sep08 - moved below
                            if(pchDecSource)
                            {
                                delete[] pchDecSource;
                                pchDecSource = null;
                            }
                            *** end  Walt EPM 08sep08 - moved */
                            iNoOfchar = 0;
                        }

                    }
                    if (0 == iLeftPeranthis)
                    {
                        break;
                    }
                }//end of for loop
                 // ** Walt EPM 08sep08 moved to here

            }
            else
            {

                for (int i = 0; i < pszComplexDDExpre.Length; i++)  // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
                {
                    if (pszComplexDDExpre[i] == '[')
                    {
                        i++;
                        int iPos = i;
                        int iLeftBrackCount = 1;
                        int iCount = 0;
                        while ((iLeftBrackCount != 0) && (i < pszComplexDDExpre.Length))  // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
                        {
                            if (pszComplexDDExpre[i] == '[')
                                iLeftBrackCount++;
                            if (pszComplexDDExpre[i] == ']')
                                iLeftBrackCount--;
                            i++;
                            iCount++;
                        }
                        i--; // stevev - 9-22-10 - it is missing the second(and subsequent)'[' in array[2][3]
                             //Get the Expression within the []
                        if (iCount > 0)
                        {
                            string pchDecSource;// +1 for Null Char +1 for ; -1 for as it had counted ]
                            //strncpy(pchDecSource, (const char*)&pszComplexDDExpre[iPos],iCount - 1);
                            //strcat(pchDecSource, ";");
                            pchDecSource = pszComplexDDExpre.Substring(iPos, iCount - 1);
                            pchDecSource += ";";
                            //Form this as new Lexical, and load it in to it
                            CLexicalAnalyzer clexAnalTemp = new CLexicalAnalyzer();
                            clexAnalTemp.InitMeeInterface(plexAnal.GetMEEInterface());
                            clexAnalTemp.Load(pchDecSource, "test");
                            //Call Parse Expression to get the Expression class
                            CExpression pExpression = expParser.ParseExpression(ref clexAnalTemp, ref pSymbolTable, STMT_EXPR_TYPE.EXPR_ASSIGN);
                            //Insert this exp in to the main DDComplex expression class
                            ((CComplexDDExpression)DDArrayExpression).AddDimensionExpr(pExpression);
                        }
                    }
                }
            }

            return true;

        }
    }

    public class UI_DATA_TYPE_TEXT_MESSAGE
    {
        public int iTextMessageLength;
        public string pchTextMessage;
    }

    public enum EDIT_BOX_TYPE
    {
        UNKNOWN_EDIT_BOX_TYPE,
        EDIT_BOX_TYPE_INTEGER,
        EDIT_BOX_TYPE_FLOAT,
        EDIT_BOX_TYPE_STRING,
        EDIT_BOX_TYPE_PASSWORD,
        EDIT_BOX_TYPE_TIME,
        EDIT_BOX_TYPE_DATE,
    }

    public struct IntMinMaxVal_t
    {
        public Int64 iMinval;
        public Int64 iMaxval;
    }

    public struct FloatMinMaxVal_t
    {
        public double fMinval;
        public double fMaxval;
    }

    public struct MinMaxVal
    {
        public IntMinMaxVal_t IntMinMaxVal;
        public FloatMinMaxVal_t FloatMinMaxVal;
    }

    public class UI_DATA_EDIT_BOX
    {
        public EDIT_BOX_TYPE editBoxType;

        public CValueVarient editBoxValue;
        //__int64		iValue;
        //float	fValue;
        public uint nSize;
        /*<START>06/01/2004 ANOOP for validating the list of ranges*/
        /*	int		iValue, iMinValue, iMaxValue;
            float	fValue, fMinValue, fMaxValue;	*/
        public List<MinMaxVal> MinMaxVal;// = new List<MinMaxVal>();
        public string strEdtFormat;
        public UI_DATA_EDIT_BOX()
        {
            //editBoxType = new EDIT_BOX_TYPE();
            editBoxValue = new CValueVarient();
            MinMaxVal = new List<MinMaxVal>();
        }
    }
    /*This is the structure used to send the display info and values from the builtin(device object)
 to the SDC methods UI.
*/
    public class ACTION_UI_DATA
    {
        public UI_DATA_TYPE userInterfaceDataType;

        public UI_DATA_TYPE_TEXT_MESSAGE textMessage;// has its own ctor
        public UI_DATA_TYPE_COMBO ComboBox;   // has its own ctor
        public UI_DATA_EDIT_BOX EditBox;
        //	UI_DATA_TYPE_DATETIME		datetime;

        public bool bMethodAbortedSignalToUI;
        public bool bUserAcknowledge;
        public bool bDisplayDynamic;   //Added by Prashant 20FEB2004

        // There are certain cases where we need just the abort button enabled
        public bool bEnableAbortOnly;  // Vibhor 030304: 
        public ushort uDelayTime;            // in MilliSecs. Vibhor 040304:
        public uint DDitemId;          //Added By Anil September 26 2005 as suggested by Steve
        public CDDLVar pVar4ItemID;     // just save the re-lookup

        public ACTION_UI_DATA()
        {
            textMessage = new UI_DATA_TYPE_TEXT_MESSAGE();
            ComboBox = new UI_DATA_TYPE_COMBO();
            EditBox = new UI_DATA_EDIT_BOX();
        }
        /* stevev 26jan06 - constructor */
    }

    public enum COMBO_BOX_TYPE
    {
        UNKNOWN_COMBO_BOX_TYPE,
        COMBO_BOX_TYPE_SINGLE_SELECT,
        COMBO_BOX_TYPE_MULTI_SELECT
    }

    public class UI_DATA_TYPE_COMBO
    {
        public const int MAXIMUM_NUMBER_OF_BITS_IN_BITENUM = 32;
        public COMBO_BOX_TYPE comboBoxType;//WS:EPM 30apr07 
        public int iNumberOfComboElements;
        public string pchComboElementText; //This will be a list of options separated by semi colon
        public uint[] m_lBitValues = new uint[MAXIMUM_NUMBER_OF_BITS_IN_BITENUM];
        //each selection in a bit enum has a value and a string associated with it. 
        //These values are not necessarily in binary order and values may be skipped 
        //so for each string in pchComboElementText, we will keep a bit value in m_lBitValues
        // The list should be in the order the DD has them in - NOT reordered to be numerically 
        //	(on value) or aphabetically (on string) in order.

        public uint nCurrentIndex; //Added By Anil October 25 2005 for fixing the PAR 5436
    }

    /*This is the structure used to send the user input from the SDC methods UI to the device object
    (and then to the builtins)*/
    public class ACTION_USER_INPUT_DATA
    {
        public UI_DATA_TYPE userInterfaceDataType;
        public UI_DATA_TYPE_COMBO ComboBox;//WS:EPM 30apr07 
        public UI_DATA_EDIT_BOX EditBox;
        //UI_DATA_TYPE_DATETIME		datetime; 

        public uint nComboSelection;

        public ACTION_USER_INPUT_DATA()
        {
            ComboBox = new UI_DATA_TYPE_COMBO();
            EditBox = new UI_DATA_EDIT_BOX();
        }
    }

    public class CMethodSupport
    {

        // DEVICE STATUS - Second byte after a response code <non-comm status>
        public const int DS_DEVMALFUNCTION = 0x80;// bit 7
        public const int DS_CONFIGCHANGED = 0x40;   // bit 6
        public const int DS_COLDSTART = 0x20;// bit 5
        public const int DS_MORESTATUSAVAIL = 0x10;// bit 4
        public const int DS_OUTCURRENTFIXED = 0x08; // bit 3
        public const int DS_ANALGOUTSATURATE = 0x04;    // bit 2
        public const int DS_SNSRVAROUTOFRNG = 0x02;// bit 1
        public const int DS_SNSRPRIVAROUTRNG = 0x01;    // bit 0
        public const int DS_NO_STATUS = 0x00;

        public bool bEnableDynamicDisplay;  //Added by Prashant 17FEB2004 for notification of dynamic variables in a method

        hCmethodCall pCurrentMethodCall;

        ACTION_UI_DATA pActUIdata; // for access to method display info in display menu
        bool m_CommErrorOccured;
        int m_buttonSelection;
        int clientCount;
        List<hCmethodCall> methodStack;

        public CMethodSupport()
        {
            methodStack = new List<hCmethodCall>();
        }
        //function to be implemented in derived class that handles initializations to be 
        //done before Method execution begins
        public virtual void PreExecute()
        {

        }

        //function to be implemented in derived class that handles cleanup to be done
        public virtual void PostExecute() //PVFC( "hCMethSupport_3" )
        {
            ;
        }

        //function to be implemented in derived class that actually calls the method
        //execution functions
        public virtual returncode DoMethodSupportExecute(hCmethodCall MethCall, HARTDevice pDev)// RPVFC( "hCMethSupport_2",0 )
        {
            bool bReturnSuccess = false;
            returncode rc = returncode.eOk;

            CDDLBase pIB = null;
            CDDLVar pVar = null;
            CValueVarient tempVariant;
            int iCurrDevStat;
            bool isAction = (MethCall.source == methodCallSource_t.msrc_ACTION || MethCall.source == methodCallSource_t.msrc_CMD_ACT);

            hCmethodCall pThisMethCall = MethCall;// we need a stack variable to keep this thread's
                                                  // info from being changed in another thread.
                                                  //	CValueVarient retVal;
                                                  //if (pDoc == null)
                                                  //return FAILURE;
                                                  //hCddbDevice pDev = pDoc.pCurDev;// we'll need this
                                                  //unsigned  int meeCnt = pDev.getMEEdepth();

            // commented out code removed 07mar07
            if (MethCall.m_pMeth == null)
                return returncode.eErr;

            string wMethNm;
            wMethNm = MethCall.m_pMeth.GetName();

            // stevev 17jan11 - add dialog pointer protection
            //hCmutex::hCmutexCntrl* pDlgMutex = m_DialogsMtx.getMutexControl("MethodSupportExecute");
            //if (!isAction)// 01aug12 - let actions slide
            //pDlgMutex.aquireMutex();// waits forever

            uint myThreadID = (uint)Thread.CurrentThread.ManagedThreadId;// GetCurrentThreadId();
            pushMethod(ref pThisMethCall);      // put it in the library...now does this uniquely

            /*
            if (meeCnt > 0)// we are not the first ones in
            {
                if (pCurrentMethodCall == null)
                {
                    LOGIT(CERR_LOG, "Method Stack Error: %d MEEs with NO currently running method.\n",
                                                                                               meeCnt);
                    TRACE(L"Method Stack Error: <<<< %d MEEs with NO currently running method.\n",
                                                                                            meeCnt);
                    if (!isAction)// 01aug12 - let actions slide	
                        pDlgMutex.relinquishMutex();
                    m_DialogsMtx.returnMutexControl(pDlgMutex);

                    return false;
                }
                // else somebody was running and all is well (we have the mutex)

                pCurrentMethodCall = pThisMethCall;// we have a new current
                DEBUGLOG(CLOG_LOG, L"*** DoMethodExecute: current method set to %s (%#x)\n",
                                                                        wMethNm.c_str(), myThreadID);
                if (pCurrentMethodCall.m_pMenuDlg != null || pCurrentMethodCall.m_pMethodDlg != null)
                {
                    LOGIT(CERR_LOG | CLOG_LOG, "Warning:Method execution start with active dialog(s).\n");
                }
            }
            else
            */
            {// we are the first one in
                pCurrentMethodCall = pThisMethCall;// we have a new current
            }

            if (!isAction)// 01aug12 - let actions slide	
            {
                //DEBUGLOG(CLOG_LOG, L"*** DoMethodExecute: relinquish Mutex for thread(%#x).\n", myThreadID);
                //pDlgMutex.relinquishMutex();// don't hold it during execution
            }

            // stevev 24jan11 - DlgMutex protects pCurrentMethodCall.... we don't have the mutex
            //			so we can't use the pointer!
            //if called from an action
            // stevev 24jan11 
            //if(pCurrentMethodCall.source == msrc_ACTION||pCurrentMethodCall.source == msrc_CMD_ACT)
            if (pThisMethCall.source == methodCallSource_t.msrc_ACTION || pThisMethCall.source == methodCallSource_t.msrc_CMD_ACT)
            {
                // stevev 24jan11 if (pCurrentMethodCall.paramList.Count != 1 )
                if (pThisMethCall.paramList.Count != 1)
                {
                    rc = returncode.eObjectError;
                }
                else
                {
                    // stevev 24jan11 pCurrentMethodCall.m_MethState = mc_Running;
                    pThisMethCall.m_MethState = mState_t.mc_Running;
                    //first element in the parameter list is the variable item ID of interest
                    // stevev 24jan11 bReturnSuccess = pDev.pMEE.ExecuteMethod(pDev,pCurrentMethodCall.methodID,
                    // stevev 24jan11 	                          pCurrentMethodCall.paramList[0].vValue.varSymbolID);
                    bReturnSuccess = pDev.pMEE.ExecuteMethod(pDev, (int)pThisMethCall.methodID, (int)pThisMethCall.paramList[0].vValue.varSymbolID);
                }
            }
            else
            // stevev 24jan11if (pCurrentMethodCall.source == msrc_EXTERN)
            if (pThisMethCall.source == methodCallSource_t.msrc_EXTERN)
            {
                // stevev 24jan11pCurrentMethodCall.m_MethState = mc_Running;
                pThisMethCall.m_MethState = mState_t.mc_Running;
                // stevev 24jan11 remove:
                //       ReturnSuccess = pDev.pMEE.ExecuteMethod(pDev, pCurrentMethodCall.methodID);
                bReturnSuccess = pDev.pMEE.ExecuteMethod(pDev, (int)pThisMethCall.methodID);
            }
            else
            {
                rc = returncode.eErr;
            }

            if (!isAction)// 01aug12 - let actions slide
            {
                //pDlgMutex.aquireMutex(hCevent::FOREVER);// waits forever
            }
            hCmethodCall pMC = retrieveMethod(myThreadID);// resets the pCurrentMethod to this thread's

            if (pMC == null || pCurrentMethodCall == null)
            {
                /*
                if (!isAction)// 01aug12 - let actions slide
                    pDlgMutex.relinquishMutex();
                m_DialogsMtx.returnMutexControl(pDlgMutex);
                */
                return returncode.eErr;
            }

            // verify it's still my method
            if (pCurrentMethodCall.methodThreadID != myThreadID)
            {
                pCurrentMethodCall = retrieveMethod(myThreadID);
                if (pCurrentMethodCall == null)// we've been removed...should never happen
                {
                    /*
                    if (!isAction)// 01aug12 - let actions slide
                        pDlgMutex.relinquishMutex();
                    m_DialogsMtx.returnMutexControl(pDlgMutex);
                    MethCall.m_MethState = mState_t.mc_NoTExist;
                    */
                    return returncode.eErr; // error return
                }
                else
                {
                }

                /*
                // this is post execute activity::>
                if (pCurrentMethodCall.m_pMethodDlg)
                { //close the methods dialog after the method execution is over	

                    // we need to have a counter of dialog users so we close the dialog when everybody is done
                    pCurrentMethodCall.m_MethState = mState_t.mc_Closing;
                    //TRACE(L"*** DoMethodExecute:'%s' send Method-Dialog close message.\n", wMethNm.c_str());
                    pCurrentMethodCall.m_pMethodDlg.SendMessage(WM_CLOSE);// we'll check it in a bit
                }

                if (pCurrentMethodCall.m_pMenuDlg)
                {//close the menu dialog after the method execution is over
                 // no need to count this one, kill it when you leave(if we aren't action)
                    pCurrentMethodCall.m_pMenuDlg.SendClose();// we'll check it in a bit
                }
                */

                if (pCurrentMethodCall.source != methodCallSource_t.msrc_CMD_ACT)
                {// @ Not cmd action - handle command 38
                    bool rb = pDev.getItembyID(HartSupport.DEVICE_COMM48_STATUS, ref pIB);
                    if (rb && pIB != null)
                    {
                        pVar = (CDDLVar)pIB;
                        // modified for KROHNE device error 10/18/04 stevev
                        tempVariant = pVar.getRealValue();
                        iCurrDevStat = (int)tempVariant.vValue.iIntConst;
                        if ((iCurrDevStat & DS_CONFIGCHANGED) != 0)
                        {// It's assumed that Command 38 is being supported by the device
                            rc = pDev.sendMethodCmd(38, -1);
                        }
                    }
                    else
                    {
                    }
                }// else command activity will deal with response code and status

                // kill any menu/method displays left over :: we told 'em to die - now check it out
                //int z;// PAW 03/03/09	
                //for ( /*int*/ z = 0; z < 30; z++)
                //{
                //    // Wait for complete deletion - just a ! ready will have a cross-thread pointer failure
                //    //if ( (m_pMethodDlg != null && m_pMethodDlg.m_dlgReady)   // have a method dialog
                //    //   ||(m_pMenuDlg   != null && m_pMenuDlg.m_dlgReady)    )//have a menuDisplay dialog
                //    //if (m_pMethodDlg != null || m_pMenuDlg   != null )
                //    if (pCurrentMethodCall.m_pMethodDlg != null || pCurrentMethodCall.m_pMenuDlg != null)
                //    {
                //        systemSleep(200);
                //    }
                //    else
                //    {
                //        break; // out of for loop
                //    }
                //}// next

                //if (z >= 30)
                //{
                //}

                //stevev: The thread alloc'd it, it should delete it:: do  it there 	RAZE(m_pMenuDlg);
                //stevev: The thread alloc'd it, it should delete it:: do  it there 	RAZE(m_pMethodDlg);
                // this is actually being done below

                if (rc == returncode.eOk && !bReturnSuccess)
                {
                    rc = returncode.eErr;
                    //DEBUGLOG(CLOG_LOG, "MethodSupport has Execution Failed but all Returns successful.\n");
                }// if both success, leave it, if rc has an error code, leave it

                if (methodStack.Count > 0)// we pushed ' em on entry
                {
                    pCurrentMethodCall = removeMethod((ushort)myThreadID);
                    //LOGIF(LOGP_DD_INFO)(CLOG_LOG, L"*** DoMethodExecute: '%s' remove method from the stack.\n", wMethNm.c_str());
                    if (MethCall == pCurrentMethodCall)
                    {
                        pCurrentMethodCall = popMethod(); // we be done with the old, put previous on
                    }
                    else
                    {
                        pCurrentMethodCall = null; // we be done
                                                   //TRACE(L"*** DoMethodExecute: '%s' Nothing On the stack: null method anyway.\n", wMethNm.c_str());
                    }

                    MethCall.m_MethState = mState_t.mc_NoTExist;
                    if (rc != returncode.eOk)
                    {   //m_ctlStat.SetWindowText("Method Execution Failed");
                        if (pDev.pMEE.latestMethodStatus == methErrors_t.me_Aborted)
                        {
                        }
                        else
                        {
                            // return rc;   
                            // Used to return false; false == zero and SUCCESS == 0.  This is a defect.  
                            // This means that it always returned success even when there was a problem or abort in
                            //  the method.  This needs to return FAILURE. Methods that fail should not allow the
                            //  next method to run in a list or allow an event (i.e., Edit) to occur if there is an
                            //  abort or method
                            // failure, POB - 14 Aug 2008
                        }
                    }
                    //CHKSTK;
                    // after latest status

                    //if (!isAction)// 01aug12 - let actions slide
                    //pDlgMutex.relinquishMutex();
                    //m_DialogsMtx.returnMutexControl(pDlgMutex);

                    if (m_CommErrorOccured == true)
                    {
                        return returncode.eErr;
                    }
                    else
                    {
                        // Used to return the previous method code instead of the current return code. 
                        // This does make sense; it shows an error for a previous method execution when the
                        // current method does not have one. Now, we return the current "return code" instead,
                        // POB - 14 Aug 2008.
                        return rc; //MethCall.m_pMeth.GetLastMethodsReturnCode();  
                    }
                }
            }
            return rc;
        }

        // true when method(s) running
        public virtual bool hasClients() //RPVFC( "hCMethSupport_4",false );
        {
            return true;
        }

        public hCmethodCall currentMethod()
        {
            uint thisThreadID = (uint)Thread.CurrentThread.ManagedThreadId;
            if (pCurrentMethodCall == null || pCurrentMethodCall.methodThreadID != thisThreadID)
            {
                pCurrentMethodCall = retrieveMethod(thisThreadID);
                if (pCurrentMethodCall == null)// we've been removed...should never happen
                {
                    return null;
                }
            }
            return pCurrentMethodCall;
        }

        //function to be implemented in derived class that handles the display of methods UI 
        public virtual bool MethodDisplay(ACTION_UI_DATA structMethodsUIData, ref ACTION_USER_INPUT_DATA structUserInputData)// RPVFC( "hCMethSupport_5",false );
        {
            //hCmethodCall pMC = null;

            ///* stevev - changed the message box so it is independent of the method dialog: error freeze-up
            //    freeze-up puts up a method UI screen with no buttons enabled,does the message box & exits.
            //    the method UI screen stays forever - have to kill sdc ***/
            //if (structMethodsUIData.userInterfaceDataType == UI_DATA_TYPE.VARIABLES_CHANGED_MSG)
            //{// short circuit dialog interaction		
            //    //display the message indicating that some variable values have changed during 
            //    // method execution and the user has to commit or cancel these changes once he exits 
            //    // the methods dialog.

            //    //string strMsg;
            //    //strMsg = Resource.IDS_VAR_VALS_CHANGED;
            //    //MessageBox.Show(strMsg);
            //    return true;
            //}

            ////hCmutex::hCmutexCntrl* pDlgMutex = m_DialogsMtx.getMutexControl("MethodDisplay");
            ////pDlgMutex.aquireMutex();// waits forever

            //pMC = currentMethod();// resets the pCurrentMethod to this thread's

            //if (pMC == null || pMC.m_pMeth == null)
            //{
            //    //pDlgMutex.relinquishMutex();
            //    //m_DialogsMtx.returnMutexControl(pDlgMutex);
            //    return false;
            //}

            //if (structMethodsUIData.userInterfaceDataType != UI_DATA_TYPE.MENU)
            //{// do standard method display
            //    if (pMC.m_pMethodDlg == null)
            //    {// generate a new one
            //     //AfxBeginThread(StartMethodDisplayProc, (LPVOID)this);

            //        pMC.m_pMethodDlg = new MethodForm(pMC, structMethodsUIData, ref structUserInputData);
            //    }
            //    else  // we already have a method dialog up
            //    {
            //    }
            //}
            //else // request is type menu - do displayMenu()
            //{
            //    pActUIdata = structMethodsUIData;
            //    if (pMC.m_pMenuDlg == null)
            //    {// generate a new one in a 'User-Interface Thread'
            //     // temp to check param
            //     //pMS = this;

            //        //AfxBeginThread(StartDisplayMenuProc, (LPVOID)this);
            //        pMC.m_pMenuDlg = new MenuForm();
            //        pMC.m_pMenuDlg.ShowDialog();

            //        // pend on dialog constructed

            //    }
            //    // stevev 26jan06 - return error at second displaymenu
            //    else
            //    {
            //        pMC.m_pMenuDlg.ShowDialog();
            //        //return false;
            //    }
            //}

            ////call function in MethodsDlg to display the information got from builtins
            //bool bReturn = true;
            //if (structMethodsUIData.userInterfaceDataType != UI_DATA_TYPE.MENU)
            //{
            //    if (pMC.m_pMethodDlg == null)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        pMC.m_pMethodDlg.DrawFormUI(structMethodsUIData);// send message
            //        //systemSleep(300);// try to get the dialog to pump some messages
            //    }
            //    /*  stevev 26jul05 - the related states vs actions of the 3 parameters:
            //                        bUserAcknowledge, bDisplayDynamic, and bEnableAbortOnly 
            //                        and the internal  bEnableDynamicDisplay are unclear at best.
            //        // reworked below ///////////////////////////////////////////////////////////
            //    Removed the old, commented out version 01nov06, 
            //        you can view an earlier version if the contents are needed
            //    ***** end //  stevev 26jul05  / / / ** */

            //    /*  NOTES stevev 26jul05 -----------------
            //        see excel sheet: tasks_Methods.xls::MethSupportStates. NEW table
            //        summary::>				
            //        Next			Abort		loc.Dyn	NxtBtn	AbtBtn	str.Dyn	BtnEvt	retVal	GetUserInput
            //        don't care		true		clear	clear	clear	clear	clear	false	
            //        true			false		clear	clear			clear	clear	true	Execute
            //        don't care		true		clear	clear	clear	clear	clear	false	
            //        true			false				clear					clear	true	Execute
            //     -----------------------end Notes 26jul05 */
            //    if (bEnableDynamicDisplay)
            //    {// poll UI, don't pend on it!!!
            //        if (pMC.m_pMethodDlg.m_MethodAborted)
            //        {
            //            bEnableDynamicDisplay = false;
            //            pMC.m_pMethodDlg.m_bNextBtnClicked = false;
            //            pMC.m_pMethodDlg.m_bAbortBtnClicked = false;
            //            // stevev 27jul05 ResetEvent(m_pMethodDlg.m_hUserAckEvent);
            //            //pMC.m_pMethodDlg.p_hCevtCntl.clearEvent();
            //            structMethodsUIData.bDisplayDynamic = false;
            //            // we leave m_MethodAborted for future calls
            //            bReturn = false;
            //        }
            //        else if (pMC.m_pMethodDlg.m_NextButtonSet)
            //        {
            //            bEnableDynamicDisplay = false;
            //            pMC.m_pMethodDlg.m_bNextBtnClicked = false;
            //            structMethodsUIData.bDisplayDynamic = false;
            //            // stevev 27jul05 ResetEvent(m_pMethodDlg.m_hUserAckEvent);
            //            //pMC.m_pMethodDlg.p_hCevtCntl.clearEvent();
            //            pMC.m_pMethodDlg.m_NextButtonSet = false;
            //            bReturn = true;
            //            pMC.m_pMethodDlg.GetMethodsUserInputData(ref structUserInputData);
            //        }
            //        // else no-op just return
            //        else
            //        {
            //            bEnableDynamicDisplay = false;
            //            pMC.m_pMethodDlg.m_bNextBtnClicked = false;
            //            pMC.m_pMethodDlg.m_bAbortBtnClicked = false;
            //            // stevev 27jul05 ResetEvent(m_pMethodDlg.m_hUserAckEvent);
            //            //pMC.m_pMethodDlg.p_hCevtCntl.clearEvent();
            //            structMethodsUIData.bDisplayDynamic = false;
            //            // we leave m_MethodAborted for future calls
            //            bReturn = false;
            //        }
            //    }
            //    else
            //    {// pend and then act on it
            //        bReturn = true;
            //        if (structMethodsUIData.bUserAcknowledge && structMethodsUIData.uDelayTime == 0)
            //        {
            //            //structMethodsUIData.uDelayTime = hCevent::FOREVER;
            //        }

            //        //pMC.m_pMethodDlg.p_hCevtCntl.pendOnEvent(structMethodsUIData.uDelayTime);

            //        pMC.m_pMethodDlg.Show();

            //        if (pMC.m_pMethodDlg.m_bAbortBtnClicked)// stevev 29may14 - breturn is always true
            //        {
            //            bEnableDynamicDisplay = false;
            //            pMC.m_pMethodDlg.m_bNextBtnClicked = false;
            //            pMC.m_pMethodDlg.m_bAbortBtnClicked = false;
            //            //ResetEvent(m_pMethodDlg.m_hUserAckEvent);
            //            structMethodsUIData.bDisplayDynamic = false;
            //            pMC.m_pMethodDlg.m_NextButtonSet = false;
            //            // we leave method aborted
            //            bReturn = false;
            //        }
            //        else if (pMC.m_pMethodDlg.m_bNextBtnClicked)
            //        {
            //            // not this state    bEnableDynamicDisplay = false;
            //            pMC.m_pMethodDlg.m_bNextBtnClicked = false;
            //            pMC.m_pMethodDlg.m_NextButtonSet = false;
            //            // not this state    structMethodsUIData.bDisplayDynamic = false;	
            //            //ResetEvent(m_pMethodDlg.m_hUserAckEvent);
            //            bReturn = true;
            //            pMC.m_pMethodDlg.GetMethodsUserInputData(ref structUserInputData);
            //        }
            //        // else - (almost) 
            //        //        impossible to get an event w/ no error and have both of these false
            //        // we won't log or anything for now
            //        else
            //        {
            //        }
            //    }
            //}
            //else // a menu
            //{
            //    // pend on event!!!!!!!!!!!!!!!!
            //    bReturn = true;
            //    m_buttonSelection = -100; // no button selected yet
            //    if (structMethodsUIData.bUserAcknowledge && structMethodsUIData.uDelayTime == 0)
            //    {
            //        //structMethodsUIData.uDelayTime = hCevent::FOREVER;
            //    }
            //}
            //return bReturn;
            return true;
        }

        /* added 07mar07 by stevev */
        // function to be implemented in derived class that returns the number of methods currently running
        public virtual int numberMethodsRunning() //RPVFC( "hCMethSupport_6",0 ); // should check that the number is correct as well
        {
            return 0;
        }

        /* 01nov06 - tack-on some UI interfaces needed for layout calcs */
        //   image index, UI looks up image(it knows what lang), converts pixels X pixels to rows X cols & returns
        public virtual bool UIimageSize(int imgIndex, ref ushort xCols, ref ushort yRows, bool isInline) //RPVFC( "hCMethSupport_7",0 );
        {
            return true;
        }
        //   string ptr, UI extracts the string(it knows what lang), converts lines X chars then to rows X cols & returns
        public virtual bool UIstringSize(ref string c_str, ref ushort xCols, ref ushort yRows, int heightLimit, bool isDialog = false) //RPVFC( "hCMethSupport_8",0 );
        {
            return true;
        }

        void pushMethod(ref hCmethodCall pNewMeth)
        {//deque<hCmethodCall*> methodStack;
         //19jan11 - stevev - made this push unique so switching tasks won't push multiple copies
            bool found = false;
            //deque<hCmethodCall*>::iterator dqMIT;
            //vector<hCmethodCall*>::iterator dqMIT;
            //for (dqMIT = methodStack.begin(); dqMIT != methodStack.end(); ++dqMIT)
            foreach (hCmethodCall pMC in methodStack)
            {// is ptr2ptr 2 methodcall
                //hCmethodCall* pMC = (hCmethodCall*)(*dqMIT);
                if (pMC.methodThreadID == 0)
                {
                    ;// _CrtDbgBreak();
                }
                if (pMC.methodThreadID == pNewMeth.methodThreadID)
                {
                    found = true;
                    break;
                    // keep going to test all of it for zero thread....break; // out of for loop
                }
            }
            if (!found)
            {
                pNewMeth.methodThreadID = (uint)Thread.CurrentThread.ManagedThreadId; //GetCurrentThreadId();
                methodStack.Insert(0, pNewMeth);
                //methodStack.push_back(pNewMeth);	
                //TRACE(_T("    Pushed %#x the on stack...now has %d entries.\n"), pNewMeth.methodThreadID, methodStack.Count);
                //test
                int y = methodStack.Count;
                if ((methodStack[y - 1].methodThreadID == pNewMeth.methodThreadID) && (pNewMeth.methodThreadID != 0))
                {// all is well
                }
                else
                {
                    //TRACE(L"We gotta mess here\n");
                }
                // end test
            }
            else
            {
                //test
                int y = methodStack.Count;
                if ((methodStack[y - 1].methodThreadID == pNewMeth.methodThreadID) && (pNewMeth.methodThreadID != 0))
                {// all is well
                }
                else
                {
                    //TRACE(L"We gotta mess here\n");
                }
                // end test
            }
        }
        public hCmethodCall popMethod()
        {
            hCmethodCall r = null;
            if (methodStack.Count > 0)
            {
                r = methodStack[0];
                //r = methodStack.front();
                // don't remove it....methodStack.pop_front();
            }
            // else - return the null	
            if (r == null)
            {
                //TRACE(_T("Failed to pop a thread off the stack of %u\n"), methodStack.Count);
            }
            else
            {
                //TRACE(_T("    Popped Thrd %#x off stack...has %u\n"), r.methodThreadID, methodStack.Count);
            }

            return r;
        }

        public hCmethodCall retrieveMethod(uint thrdID)
        {
            hCmethodCall r = null;
            if (methodStack.Count > 0)
            {
                //r = methodStack.front();
                //methodStack.pop_front();

                foreach (hCmethodCall dqMIT in methodStack)
                {// is ptr2ptr 2 methodcall
                    hCmethodCall pMC = dqMIT;
                    if (pMC.methodThreadID == 0)
                    {
                        //_CrtDbgBreak();// debugging
                    }
                    if (pMC.methodThreadID == thrdID)
                    {
                        r = pMC;
                        //dqMIT = null;
                        break; // out of for loop///?????
                    }
                }
            }
            // else - return the null	
            if (r == null)
            {
                //TRACE(_T("Failed to retrieve thread %#x off the stack\n"), thrdID);
            }

            return r;
        }
        public hCmethodCall removeMethod(ushort thrdID)// does not delete, returns ptr
        {
            hCmethodCall r = null;
            if (methodStack.Count > 0)
            {
                foreach (hCmethodCall dqMIT in methodStack)
                {// is ptr2ptr 2 methodcall
                    hCmethodCall pMC = dqMIT;
                    if (pMC.methodThreadID == 0)
                    {
                        //_CrtDbgBreak();// debugging
                    }
                    if (pMC.methodThreadID == thrdID)
                    {
                        r = pMC;
                        //dqMIT = null;
                        methodStack.Remove(dqMIT);
                        break; // out of for loop
                    }
                }
                //CHKSTK;
            }
            // else - return the null	
            if (r == null)
            {
                //TRACE(_T("Failed to remove thread %u from the stack. Stack still has %d.\n"), thrdID, methodStack.Count);
            }

            return r;
        }

    }

    public class DOT_OP_ATTR
    {
        public string strDotOpName;
        uint uiUniqueID;
        uint uiVarType;
        
        public DOT_OP_ATTR()
        {
            uiUniqueID = 0;
            uiVarType = 0;
        }

        public DOT_OP_ATTR(string strTemp, uint uiTemp1, uint uiTemp2)
        {
            strDotOpName = strTemp;
            uiUniqueID = uiTemp1;
            uiVarType = uiTemp2;
        }
    }

    public class DOT_OP_LIST_t : List<int>
    {

    }

    public class MEE
    {
        const byte DOT_ID_NONE = 0;
        const byte DOT_ID_LABEL = 1;
        const byte DOT_ID_HELP = 2;
        const byte DOT_ID_MIN_VALUE = 3;
        const byte DOT_ID_MAX_VALUE = 4;
        const byte DOT_ID_DFLT_VALUE = 5;
        const byte DOT_ID_VIEW_MIN = 6;
        const byte DOT_ID_VIEW_MAX = 7;
        const byte DOT_ID_COUNT = 8;
        const byte DOT_ID_CAPACITY = 9;
        const byte DOT_ID_FIRST = 10;
        const byte DOT_ID_LAST = 11;

        const byte DOT_TYPE_INTEGER = 1;
        const byte DOT_TYPE_REAL = 2;
        const byte DOT_TYPE_DOUBLE = 3;
        const byte DOT_TYPE_STRING = 4;
        const byte DOT_TYPE_BOOLEAN = 5;
        const byte DOT_TYPE_LIST = 6;
        public HARTDevice m_pDevice;
        /* The Item Id of the method being executed */
        //int m_lMethodItemId;
        uint m_NoOneMethInstance;

        public CSymbolTable m_GlobalSymTable; //Vibhor 010705: Added for DD Variables
        List<string> m_svSecondaryAtt;

        //	vector <OneMeth*> MethStack; //To support methods calling methods

        public string methodNameString;// stevev 21sep07 - for help in debugging
        public methErrors_t latestMethodStatus;
        public bool m_bSaveValues;

        Dictionary<string, DOT_OP_ATTR> m_MapDotOpNameToAttr;
        DOT_OP_ATTR[] DotOpAttr;
        DOT_OP_LIST_t[] AvailAttrOP;

        public MEE()
        {
            m_GlobalSymTable = new CSymbolTable();
            m_svSecondaryAtt = new List<string>();
            DotOpAttr = new DOT_OP_ATTR[12];
            AvailAttrOP = new DOT_OP_LIST_t[(int)itemType_t.iT_MaxType];
            for (int i = 0; i < AvailAttrOP.Length; i++)
            {
                AvailAttrOP[i] = new DOT_OP_LIST_t();
            }
            m_MapDotOpNameToAttr = new Dictionary<string, DOT_OP_ATTR>();
            BuildDotOpNameToAttrMap();
        }

        public uint IncreaseOneMethRefNo()
        {
            return (++m_NoOneMethInstance);
        }
        public uint DecreaseOneMethRefNo()
        {
            return (--m_NoOneMethInstance);

        }

        public uint GetOneMethRefNo()
        {
            return m_NoOneMethInstance;
        }

        public bool ExecuteMethod(HARTDevice pDevice, int lMethodItemId)
        {
            m_pDevice = pDevice; //Vibhor 070705: Added, Sorry I missed this one earlier !!!

            OneMeth pMeth = new OneMeth();
            IncreaseOneMethRefNo();
            latestMethodStatus = methErrors_t.me_NoError;

            bool bRet = pMeth.ExecuteMethod(this, pDevice, lMethodItemId);
            DecreaseOneMethRefNo();

            return bRet;
        }

        public void BuildDotOpNameToAttrMap()
        {
            //                         non-aggregates cannot be initialized with initializer list
            DotOpAttr[DOT_ID_NONE] = new DOT_OP_ATTR("NONE", DOT_ID_NONE, DOT_TYPE_STRING);
            DotOpAttr[DOT_ID_LABEL] = new DOT_OP_ATTR("LABEL", DOT_ID_LABEL, DOT_TYPE_STRING);
            DotOpAttr[DOT_ID_HELP] = new DOT_OP_ATTR("HELP", DOT_ID_HELP, DOT_TYPE_STRING);
            DotOpAttr[DOT_ID_MIN_VALUE] = new DOT_OP_ATTR("MIN_VALUE", DOT_ID_MIN_VALUE, DOT_TYPE_REAL);
            DotOpAttr[DOT_ID_MAX_VALUE] = new DOT_OP_ATTR("MAX_VALUE", DOT_ID_MAX_VALUE, DOT_TYPE_REAL);
            DotOpAttr[DOT_ID_DFLT_VALUE] = new DOT_OP_ATTR("DEFAULT_VALUE", DOT_ID_DFLT_VALUE, DOT_TYPE_REAL);
            DotOpAttr[DOT_ID_VIEW_MIN] = new DOT_OP_ATTR("VIEW_MIN", DOT_ID_VIEW_MIN, DOT_TYPE_REAL);
            DotOpAttr[DOT_ID_VIEW_MAX] = new DOT_OP_ATTR("VIEW_MAX", DOT_ID_VIEW_MAX, DOT_TYPE_REAL);
            DotOpAttr[DOT_ID_COUNT] = new DOT_OP_ATTR("COUNT", DOT_ID_COUNT, DOT_TYPE_INTEGER);
            DotOpAttr[DOT_ID_CAPACITY] = new DOT_OP_ATTR("CAPACITY", DOT_ID_CAPACITY, DOT_TYPE_INTEGER);
            DotOpAttr[DOT_ID_FIRST] = new DOT_OP_ATTR("FIRST", DOT_ID_FIRST, DOT_TYPE_INTEGER);
            DotOpAttr[DOT_ID_LAST] = new DOT_OP_ATTR("LAST", DOT_ID_LAST, DOT_TYPE_INTEGER);

            int i; // declare out here to (legally) use in both loops
            for (i = 0; i < DotOpAttr.Length; i++)
            {
                //m_MapDotOpNameToAttr[DotOpAttr[i].strDotOpName] = DotOpAttr[i];
                m_MapDotOpNameToAttr.Add(DotOpAttr[i].strDotOpName, DotOpAttr[i]);
            }

            AvailAttrOP[(int)itemType_t.iT_Variable].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Variable].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_Variable].Add(DOT_ID_MIN_VALUE);
            AvailAttrOP[(int)itemType_t.iT_Variable].Add(DOT_ID_MAX_VALUE);
            AvailAttrOP[(int)itemType_t.iT_Variable].Add(DOT_ID_DFLT_VALUE);

            AvailAttrOP[(int)itemType_t.iT_Axis].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Axis].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_Axis].Add(DOT_ID_MIN_VALUE);
            AvailAttrOP[(int)itemType_t.iT_Axis].Add(DOT_ID_MAX_VALUE);
            AvailAttrOP[(int)itemType_t.iT_Axis].Add(DOT_ID_VIEW_MIN);
            AvailAttrOP[(int)itemType_t.iT_Axis].Add(DOT_ID_VIEW_MAX);

            AvailAttrOP[(int)itemType_t.iT_List].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_List].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_List].Add(DOT_ID_COUNT);
            AvailAttrOP[(int)itemType_t.iT_List].Add(DOT_ID_CAPACITY);
            AvailAttrOP[(int)itemType_t.iT_List].Add(DOT_ID_FIRST);
            AvailAttrOP[(int)itemType_t.iT_List].Add(DOT_ID_LAST);

            AvailAttrOP[(int)itemType_t.iT_Menu].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Menu].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_Grid].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Grid].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_Image].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Image].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_EditDisplay].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_EditDisplay].Add(DOT_ID_HELP);
            AvailAttrOP[(int)itemType_t.iT_Method].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Method].Add(DOT_ID_HELP);

            AvailAttrOP[(int)itemType_t.iT_Collection].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Collection].Add(DOT_ID_HELP);//members
            AvailAttrOP[(int)itemType_t.iT_File].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_File].Add(DOT_ID_HELP);//members

            AvailAttrOP[(int)itemType_t.iT_Chart].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Chart].Add(DOT_ID_HELP);//members-source
            AvailAttrOP[(int)itemType_t.iT_Graph].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Graph].Add(DOT_ID_HELP);//members-waveform
            AvailAttrOP[(int)itemType_t.iT_Source].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Source].Add(DOT_ID_HELP);//members-numeric
            AvailAttrOP[(int)itemType_t.iT_Waveform].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Waveform].Add(DOT_ID_HELP);

            AvailAttrOP[(int)itemType_t.iT_ItemArray].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_ItemArray].Add(DOT_ID_HELP);//elements
            AvailAttrOP[(int)itemType_t.iT_Array].Add(DOT_ID_LABEL);
            AvailAttrOP[(int)itemType_t.iT_Array].Add(DOT_ID_HELP);//elements

        }

        public int ValidateExp(ref string szComplexDDExpre, ref string szLastAttr, ref bool bPrimaryAttr)
        {
            if (szComplexDDExpre == null)
            {
                return Common.FAILURE;
            }

            int iDDExpreLen = szComplexDDExpre.Length;
            int iDotCount = 0;
            int iLastDotPos = 0;
            //Get the position of last dot operator , if it is present
            for (int i = 0; i < iDDExpreLen; i++)
            {
                if (szComplexDDExpre[i] == '.')
                {
                    iDotCount++;
                    iLastDotPos = i;
                }
            }

            //Get the Expression followed by Last dot	
            if (iDotCount != 0)
            {

                szLastAttr = szComplexDDExpre.Substring(iLastDotPos + 1, iDDExpreLen - iLastDotPos - 1);

                //if Expression followed by Last dot is secondary attribute , then it is not valid !! 
                //So return FAILURE
                string strDotExpression = szLastAttr;
                for (int i = 0; i < m_svSecondaryAtt.Count; i++)    // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
                {
                    if (strDotExpression == m_svSecondaryAtt[i])
                    {
                        return Common.FAILURE;
                    }
                }
                //Before Checking it against primary attribute, because it may happen that it is MAX_VALUEn or MIN_VALUEn
                if (szLastAttr.IndexOf("MAX_VALUE") > 0)//////
                {
                    strDotExpression = "MAX_VALUE";
                }
                else if (szLastAttr.IndexOf("MIN_VALUE") > 0)
                {
                    strDotExpression = "MIN_VALUE";
                }
                //Check whether it is primary attribute
                if (m_MapDotOpNameToAttr.ContainsKey(strDotExpression))
                {
                    //it is not a primary attribute. Good!! Reduces my work of resolution in latter part
                    bPrimaryAttr = true;
                }
                else
                {
                    //it is a primary attribute . Not bad!! I need to take care of this in latter part
                    bPrimaryAttr = false;
                }
            }
            return Common.SUCCESS;
        }

        public int ResolveExp(string szDDitemName, string szComplexDDExpre, uint iEndofEpression, ref CDDLBase pIBFinal)
        {
            return Common.SUCCESS;
        }

        public int ResolveNUpdateDDExp(string szComplexDDExpre, string szTokenName, ref INTER_VARIANT ivVariableValue,
            RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)
        {
            if (szComplexDDExpre == null || szTokenName == null)
                return Common.FAILURE;

            string szLastAttr = null;
            bool bPrimaryAttr = false;

            if (Common.FAILURE == ValidateExp(ref szComplexDDExpre, ref szLastAttr, ref bPrimaryAttr))
            {
                if (szLastAttr != null)
                {
                    szLastAttr = null;
                }
                return Common.FAILURE;
            }

            int iEndofEpression = szComplexDDExpre.Length;
            if (true == bPrimaryAttr)
            {
                iEndofEpression = szComplexDDExpre.Length - szLastAttr.Length - 1; //WS:EPM 10aug07
            }

            if (szLastAttr != null)
            {
                szLastAttr = null;
            }

            /*WS:EPM 10aug07 - moved it up
            if( true == bPrimaryAttr )
            {
                return FAILURE;
            }  end  WS:EPM 10aug07 */


            //No we need to resolve the Coplex Expression in to an Var ptr which is pIBFinal
            //WS:EPM 10aug07 - moved it up
            //unsigned long iEndofEpression = strlen(szComplexDDExpre);;
            CDDLBase pIBFinal = new CDDLBase();
            if (Common.FAILURE == ResolveExp(szTokenName, szComplexDDExpre, (uint)iEndofEpression, ref pIBFinal))
            {
                return Common.FAILURE;
            }

            //now we got the final ptr to the variable
            //Cross check whether it is really a variable
            if ((null == pIBFinal) || (!(pIBFinal.IsVariable())))
            {
                return Common.FAILURE;
            }

            CValueVarient tmpCV = new CValueVarient();
            CDDLVar pVar = (CDDLVar)pIBFinal;
            if (Common.FAILURE == GetDDVarValNEvalAssType(pVar, ivVariableValue, tmpCV, AssignType))
            {
                return Common.FAILURE;
            }
            //stevev 18oct07 - scaling is ONLY for display....pVar.setDispValue(tmpCV);
            //////pVar.setRawDispValue(tmpCV);

            return Common.SUCCESS;

        }

        public int GetDDVarValNEvalAssType(CDDLVar pIBFinal, INTER_VARIANT ivVariableValue, CValueVarient tmpCV, RUL_TOKEN_SUBTYPE AssignType)
        {

            if (null == pIBFinal)
            {
                return Common.FAILURE;
            }

            return Common.SUCCESS;
        }

        public int FindGlobalToken(string pszTokenName, int lLineNum, ref DD_ITEM_TYPE DDitemType)//changed the Function Type Anil September 16 2005
        {
            RUL_TOKEN_TYPE tokenType = RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE;
            RUL_TOKEN_SUBTYPE tokenSubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;

            //	bIsVariableItem = true;
            DDitemType = DD_ITEM_TYPE.DD_ITEM_VAR;
            int nIndx = -1;

            nIndx = m_GlobalSymTable.GetIndex(pszTokenName);

            //if not already there in the GlobalSymbolTable, then search the device
            if (-1 == nIndx)
            {
                CDDLBase pIB = null;
                string strTokenName = pszTokenName;
                if (m_pDevice.getItembyName(strTokenName, ref pIB) && null != pIB/* && pIB.IsValid()*/)
                {
                    nitype itemtype = pIB.eType;
                    //Added to check for method also Anil September 16 2005
                    if (itemtype == nitype.nVar)
                    {
                        //	bIsVariableItem = true;
                        DDitemType = DD_ITEM_TYPE.DD_ITEM_VAR;
                        tokenSubType = RUL_TOKEN_SUBTYPE.RUL_DD_SIMPLE;
                    }
                    else if (itemtype == nitype.nMethod)
                    {
                        tokenSubType = RUL_TOKEN_SUBTYPE.RUL_DD_METHOD;
                        DDitemType = DD_ITEM_TYPE.DD_ITEM_METHOD;
                    }
                    else
                    {   //its a non-variable DD item
                        //bIsVariableItem = false;
                        DDitemType = DD_ITEM_TYPE.DD_ITEM_NONVAR;
                        tokenSubType = RUL_TOKEN_SUBTYPE.RUL_DD_COMPLEX;
                    }
                    tokenType = RUL_TOKEN_TYPE.RUL_DD_ITEM;
                    CToken localToken = new CToken(pszTokenName, tokenType, tokenSubType, lLineNum);
                    localToken.m_bIsGlobal = true;

                    nIndx = m_GlobalSymTable.Insert(localToken);//SymbolTable::Insert makes a copy of the token

                }//end if SUCCESS

            }//endif -1
            return nIndx;

        }

        public bool ExecuteMethod(HARTDevice pDevice, int lMethodItemId, int lVarItemId)
        {
            m_pDevice = pDevice; //Anil 12oct05: Vibhor, you missed this one too !!!

            OneMeth pMeth = new OneMeth();
            IncreaseOneMethRefNo();
            latestMethodStatus = methErrors_t.me_NoError;

            bool bRet = pMeth.ExecuteMethod(this, pDevice, lMethodItemId, lVarItemId);
            DecreaseOneMethRefNo();
            return bRet;
        }/*End ExecuteMethod*/

        public bool IsDDItem(string pszDDItemName)
        {
            CDDLBase pIB = new CDDLBase();
            string strDDItemName = pszDDItemName;
            //Get the Item base ptr, if it is success , then it is a DD item
            if (m_pDevice.getItembyName(strDDItemName, ref pIB) && (null != pIB))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    public enum methErrors_t
    {
        me_NoError,
        me_Aborted
    }

    public enum INTERPRETER_STATUS
    {
        INTERPRETER_STATUS_INVALID,
        INTERPRETER_STATUS_OK,
        INTERPRETER_STATUS_PARSE_ERROR,
        INTERPRETER_STATUS_EXECUTION_ERROR,
        INTERPRETER_STATUS_UNKNOWN_ERROR
    }

    public struct Precedence
    {
        public int StackTop;    //used by function f
        public int Incoming;    //used by function g
        public int n;   //number of operands.
        public int t;   //number of terminals to be lifted.
    }

    public class CParser
    {
        public const int PARSE_SUCCESS = 1;
        public const int PARSE_FAIL = 0;

        public const int TOKEN_SUCCESS = 1;
        public const int TOKEN_FAILURE = 0;

        public const int TYPE_SUCCESS = 1;
        public const int TYPE_FAILURE = 0;

        private CHart_Builtins pBuiltInLib;
        bool m_bIsRoutine;//Anil Octobet 5 2005 for handling Method Calling Method
        //Dictionary<int, Precedence> s_PrecedenceTable;

        CLexicalAnalyzer lexAnal;
        CSymbolTable SymbolTable;
        CProgram pgm;
        CInterpretedVisitor interpretor;
        CTypeCheckVisitor typeChecker;
        MEE m_pMEE; //Vibhor 010705: Added

        public CParser()
        {
            pBuiltInLib = new CHart_Builtins();
            //s_PrecedenceTable = new Dictionary<int, Precedence>();
            pgm = new CProgram();
            SymbolTable = new CSymbolTable();
            typeChecker = new CTypeCheckVisitor();
            interpretor = new CInterpretedVisitor();
            lexAnal = new CLexicalAnalyzer();
        }

        public bool Initialize(CHart_Builtins pBuiltInLibParam, MEE pMEE) //Vibhor 010705: Modified
        {
            if (pBuiltInLibParam != null)
            {
                pBuiltInLib = pBuiltInLibParam;
                m_pMEE = pMEE; //Vibhor 010705: Added
                interpretor.Initialize(pBuiltInLibParam, m_pMEE);
                //Anil Octobet 5 2005 for handling Method Calling Method
                //This is required to differentite whether it is called method or it is a method called from menu
                interpretor.SetIsRoutineFlag(m_bIsRoutine);
                if (lexAnal.InitMeeInterface(m_pMEE))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public INTERPRETER_STATUS Execute(string pszSource, string pszRuleName, string szData, string szSymbolTable)
        {
            InitializePrecedenceTable();

            lexAnal.Load(pszSource, pszRuleName);

            CLexicalAnalyzer lexAnalPredefinedDeclarations = new CLexicalAnalyzer();
            string pchDecSource = "{int _bi_rc;}";

            lexAnalPredefinedDeclarations.Load(pchDecSource, pszRuleName);
            //try
            {
                /*<START>TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                CProgram pgm1 = new CProgram();
                int iRet32 = pgm1.CreateParseSubTree(ref lexAnalPredefinedDeclarations, ref SymbolTable);
                /*<END>TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                if (iRet32 == PARSE_FAIL)
                {
                    return INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR;
                }

                iRet32 = pgm.CreateParseSubTree(ref lexAnal, ref SymbolTable);
                if (iRet32 == PARSE_FAIL)
                {
                    return INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR;
                }
            }
            /*
            catch ()
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR;
            }
            */
            // The following code visits all the nodes in the Parse tree
            // and tries to interpret it...

            //try
            {
                INTER_VARIANT iv = null;
                pgm.Execute(interpretor, SymbolTable, ref iv);
            }
            /*
            catch ()
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
            }
            */
            return INTERPRETER_STATUS.INTERPRETER_STATUS_OK;
        }

        public void InitializePrecedenceTable()
        {
            Precedence pre = new Precedence();
            pre.StackTop = -1;
            pre.Incoming = -1;
            pre.n = 0;

            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DOLLAR] = pre;

            // =, +=, -=, *=, /=, %=, &=, ^=, |=, <<=, >>=
            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN] = pre;

            pre.StackTop = 12;
            pre.Incoming = 11;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN] = pre;

            /*	pre.StackTop				= 13;
                pre.Incoming				= 14;
                s_PrecedenceTable[RUL_EXP].n					= 2;*/

            // ?, :
            pre.StackTop = 14;
            pre.Incoming = 13;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_QMARK] = pre;

            pre.StackTop = 14;
            pre.Incoming = 13;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_COLON] = pre;

            // ||	
            pre.StackTop = 16;
            pre.Incoming = 15;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_OR] = pre;


            // &&
            pre.StackTop = 18;
            pre.Incoming = 17;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_AND] = pre;

            // |
            pre.StackTop = 20;
            pre.Incoming = 19;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_OR] = pre;


            // ^
            pre.StackTop = 22;
            pre.Incoming = 21;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_XOR] = pre;


            // &
            pre.StackTop = 24;
            pre.Incoming = 23;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_AND] = pre;


            // ==, !=
            pre.StackTop = 26;
            pre.Incoming = 25;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_NOT_EQ] = pre;

            pre.StackTop = 26;
            pre.Incoming = 25;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_EQ] = pre;


            // >, <, >=, <=
            pre.StackTop = 28;
            pre.Incoming = 27;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LT] = pre;

            pre.StackTop = 28;
            pre.Incoming = 27;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_GT] = pre;

            pre.StackTop = 28;
            pre.Incoming = 27;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_GE] = pre;

            pre.StackTop = 28;
            pre.Incoming = 27;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LE] = pre;


            // >>, <<
            pre.StackTop = 30;
            pre.Incoming = 29;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT] = pre;

            pre.StackTop = 30;
            pre.Incoming = 29;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT] = pre;


            // +, -
            pre.StackTop = 32;
            pre.Incoming = 31;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS] = pre;

            pre.StackTop = 32;
            pre.Incoming = 31;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS] = pre;


            // *, /, %
            pre.StackTop = 34;
            pre.Incoming = 33;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MUL] = pre;

            pre.StackTop = 34;
            pre.Incoming = 33;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DIV] = pre;

            pre.StackTop = 34;
            pre.Incoming = 33;
            pre.n = 2;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MOD] = pre;

            // +, -, ++, --, !, ~
            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_UPLUS] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_UMINUS] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS_PLUS] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS_MINUS] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_PRE_PLUS_PLUS] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_PRE_MINUS_MINUS] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_NOT] = pre;

            pre.StackTop = 35;
            pre.Incoming = 36;
            pre.n = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_NOT] = pre;


            // Parenthesis and Brackets
            pre.StackTop = 0;
            pre.Incoming = 37;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LPAREN] = pre;

            pre.StackTop = 38;
            pre.Incoming = 0;
            pre.n = 1;
            pre.t = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_RPAREN] = pre;

            pre.StackTop = 0;
            pre.Incoming = 37;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LBOX] = pre;

            pre.StackTop = 38;
            pre.Incoming = 0;
            pre.n = 1;
            pre.t = 1;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_RBOX] = pre;

            // Declarations
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL] = pre;
            // ** Walt EPM 08sep08
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL] = pre;
            // ** end Walt EPM 08sep08
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LONG_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL] = pre;
            // ** Walt EPM 08sep08
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL] = pre;
            // ** end Walt EPM 08sep08
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_REAL_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_STRING_DECL] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_INT_CONSTANT] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_BOOL_CONSTANT] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_REAL_CONSTANT] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_SERVICE_ATTRIBUTE] = pre;

            //Added By Anil June 16 2005 --starts here
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL] = pre;
            //Added By Anil June 16 2005 --Ends here

            //Anil August 26 2005 For handling DD variable and Expression
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DD_SIMPLE] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DD_COMPLEX] = pre;

            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_DD_METHOD] = pre;

            //Added By Anil June 16 2005 --starts here
            pre.StackTop = 40;
            pre.Incoming = 39;
            pre.n = 0;
            CExpParser.s_PrecedenceTable[(int)RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL] = pre;
            //Added By Anil June 16 2005 --Ends here
        }

        //int BuildParseTree(string pszSource, string pszRuleName, string szData, string szSymbolTable);

        int Compile(string pszSource, string pszRuleName, string ppszbyteCode, ref int pi32byteCodeSize, int i32Console)
        {
            InitializePrecedenceTable();
            return PARSE_SUCCESS;
        }

        public bool GetVariableValue(string pchVariableName, ref INTER_VARIANT varValue)
        {/* note: stevev 31may07 - array access does not work.
            a) preprocessor does no substitution inside a format string (ie %{cmd_status[STATUS_COMM_STATUS]}) 
	        b) we need a way to execute the expression inside the '[' ']' 
	        c) we need a way to look up that element inside the variable array
	        none of which are immediatly available ***/
            int index = 0;
            CVariable pVar = SymbolTable.Find(pchVariableName, ref index);
            if (pVar == null)
            {
                return false;
            }
            else
            {
                varValue = pVar.GetValue();
                return true;
            }
        }

        public bool SetVariableValue(string pchVariableName, INTER_VARIANT varValue)
        {
            int index = 0;
            CVariable pVar = SymbolTable.Find(pchVariableName, ref index);
            //int i = SymbolTable.Find(pchVariableName, );
            if (pVar == null)
            {
                return false;
            }
            else
            {
                //SymbolTable.SetValue(index, varValue);
                pVar.SetValue(varValue);
                return true;
            }
        }

        //Anil Octobet 5 2005 for handling Method Calling Method
        //Added an overloaded function to handle the methods calling methods
        //public int Execute(string pszSource, string pszRuleName, string szData, string szSymbolTable, METHOD_ARG_INFO_VECTOR vectMethArg, List<INTER_VARIANT> vectInterVar);

        void SetIsRoutineFlag(bool bIsRoutine)
        {
            m_bIsRoutine = bIsRoutine;
            return;
        }

        bool GetIsRoutineFlag()
        {
            return m_bIsRoutine;
        }

    }

    public class CInterpreter
    {
        private bool m_bInitialized;

        /* Status of the interpreter */
        INTERPRETER_STATUS m_intStatus;

        /* Pointer to the source as passed by the Client */
        string m_pchSourceCode;

        /* The pointer to the parser object */
        CParser m_pParser;

        MEE m_pMEE; //Vibhor 010705 Added : Required for access to "Global" (DD) Data

        public INTERPRETER_STATUS ExecuteCode
                                (
                                ref CHart_Builtins pBuiltInLibParam
                                , string pchSource
                                    , string pchSourceName
                                    , string pchCodeData
                                    , string pchSymbolDump
                                    , ref MEE pMEE         //Vibhor 010705: Added
                                )
        {
            if (m_bInitialized == true)
            {
                m_bInitialized = false;

                m_intStatus = INTERPRETER_STATUS.INTERPRETER_STATUS_INVALID;

                //if (m_pParser)
                //{
                //delete m_pParser;
                //}
            }

            if (pchSource == null)
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR;
            }
            else
            {
                m_pParser = new CParser();
                if (null == m_pParser)
                {
                    return INTERPRETER_STATUS.INTERPRETER_STATUS_UNKNOWN_ERROR;
                }

                m_pMEE = pMEE;  //Vibhor 010705: Added

                if (false == m_pParser.Initialize(pBuiltInLibParam, m_pMEE))
                {
                    return INTERPRETER_STATUS.INTERPRETER_STATUS_UNKNOWN_ERROR;
                }

                m_pchSourceCode = pchSource;
                m_bInitialized = true;

                m_intStatus = (INTERPRETER_STATUS)m_pParser.Execute(pchSource, pchSourceName, pchCodeData, pchSymbolDump);
                return m_intStatus;
            }
        }

        /*
        bool GetVariableValue
                    (
                        string pchVariableName
                        , INTER_VARIANT &varValue , hCVar** ppDevObjVar = null
                    );
                    */
        public bool SetVariableValue(string pchVariableName, INTER_VARIANT varValue)
        {
            return m_pParser.SetVariableValue(pchVariableName, varValue);
        }
        //Anil Octobet 5 2005 for handling Method Calling Method

        public bool GetVariableValue(string pchVariableName, ref INTER_VARIANT varValue, ref CDDLVar ppDevObjVar)
        {
            if (false == m_pParser.GetVariableValue(pchVariableName, ref varValue))
            //actually 'GetLocalVariableValue'
            {
                /*
                int lCount = 0;
                int lCurrentPos = 0;
                while ((pchVariableName[lCount] != '[') &&
                       (pchVariableName[lCount] != '.') &&
                       (pchVariableName[lCount] != 0))
                {
                    lCount++;
                }

                int iLen = lCount - lCurrentPos + 1;

                //allocate string
                string szTokenName = pchVariableName;
                CDDLBase pIB = new CDDLBase();
                int iretCode = m_pMEE.ResolveDDExpForBuiltin(pchVariableName, szTokenName, ref varValue, ref pIB);
                if (ppDevObjVar != null)
                {
                    if (pIB != null && pIB.IsVariable())
                    {
                        ppDevObjVar = (CDDLVar)pIB;
                    }
                    else
                    {
                        ppDevObjVar = null;
                    }
                }
                
                //Now release the string. 

                if (iretCode == Common.SUCCESS)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                */
                return false;
            }
            else
            {// use the varValue that determined by the GetLocalVariableValue
                if (ppDevObjVar != null)// we were given a pointer to fill
                {
                    ppDevObjVar = null;// tell it we have no DeviceObjectPointer
                }
            }
            return true;
            //	return m_pParser.GetVariableValue(pchVariableName,varValue);
        }

        public INTERPRETER_STATUS ExecuteCode
                                (
                                CHart_Builtins pBuiltInLibParam
                                , string pchSource
                                    , string pchSourceName
                                    , string pchCodeData
                                    , string pchSymbolDump
                                    , MEE pMEE
                                    , METHOD_ARG_INFO_VECTOR vectMethArg
                                    , List<INTER_VARIANT> vectInterVar
                                    )
        {
            return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
        }

    }

    public enum RUL_TOKEN_TYPE
    {
        RUL_TYPE_NONE = 0,
        RUL_KEYWORD,
        RUL_SYMBOL,
        RUL_ARITHMETIC_OPERATOR,
        RUL_ASSIGNMENT_OPERATOR,
        RUL_RELATIONAL_OPERATOR,
        RUL_LOGICAL_OPERATOR,
        RUL_NUMERIC_CONSTANT,
        RUL_STR_CONSTANT,
        RUL_CHR_CONSTANT,
        RUL_SIMPLE_VARIABLE,
        RUL_ARRAY_VARIABLE,
        RUL_SERVICE,
        RUL_TYPE_ERROR,
        RUL_COMMENT,
        RUL_EOF,
        RUL_DD_ITEM     //Vibhor 140705: Added
    }

    public enum RUL_TOKEN_SUBTYPE
    {
        RUL_SUBTYPE_NONE = 0,
        //INT Operators
        RUL_UPLUS,
        RUL_UMINUS,
        RUL_PLUS,
        RUL_MINUS,
        RUL_MUL,
        RUL_DIV,
        RUL_MOD,
        RUL_EXP,
        RUL_NOT_EQ,
        RUL_LT,
        RUL_GT,
        RUL_EQ,
        RUL_GE,
        RUL_LE,
        RUL_LOGIC_AND,
        RUL_LOGIC_OR,
        RUL_LOGIC_NOT,
        RUL_ASSIGN,
        RUL_PLUS_ASSIGN,
        RUL_MINUS_ASSIGN,
        RUL_DIV_ASSIGN,
        RUL_MOD_ASSIGN,
        RUL_MUL_ASSIGN,
        RUL_PLUS_PLUS,
        RUL_MINUS_MINUS,
        RUL_PRE_PLUS_PLUS,
        RUL_PRE_MINUS_MINUS,
        RUL_BIT_AND_ASSIGN,
        RUL_BIT_OR_ASSIGN,
        RUL_BIT_XOR_ASSIGN,
        RUL_BIT_RSHIFT_ASSIGN,
        RUL_BIT_LSHIFT_ASSIGN,
        //FLOAT Operators
        RUL_FUPLUS,
        RUL_FUMINUS,
        RUL_FPLUS,
        RUL_FMINUS,
        RUL_FMUL,
        RUL_FDIV,
        RUL_FMOD,
        RUL_FRAND,
        RUL_FEXP,
        RUL_I2F,
        RUL_F2I,
        RUL_NOT_FEQ,
        RUL_FLT,
        RUL_FGT,
        RUL_FEQ,
        RUL_FGE,
        RUL_FLE,
        //DOUBLE Operators
        RUL_DUPLUS,
        RUL_DUMINUS,
        RUL_DPLUS,
        RUL_DMINUS,
        RUL_DMUL,
        RUL_DDIV,
        RUL_DMOD,
        RUL_DEXP,
        RUL_I2D,
        RUL_D2I,
        RUL_F2D,
        RUL_D2F,
        RUL_NOT_DEQ,
        RUL_DLT,
        RUL_DGT,
        RUL_DEQ,
        RUL_DGE,
        RUL_DLE,
        //String Operators
        RUL_SPLUS,
        RUL_SEQ,
        RUL_NOT_SEQ,
        //Keywords
        RUL_IF,
        RUL_ELSE,
        RUL_SWITCH,
        RUL_CASE,
        RUL_DEFAULT,
        RUL_WHILE,
        RUL_FOR,
        RUL_DO,
        RUL_CHAR_DECL,
        RUL_UNSIGNED_INTEGER_DECL,
        RUL_INTEGER_DECL,
        RUL_LONG_LONG_DECL,//WaltSigtermans March 14 2008 Added 8 byte integer
        RUL_LONG_DECL,
        RUL_UNSIGNED_SHORT_INTEGER_DECL,
        RUL_SHORT_INTEGER_DECL,
        RUL_REAL_DECL,
        RUL_DOUBLE_DECL,
        RUL_BOOLEAN_DECL,
        RUL_STRING_DECL,
        RUL_ARRAY_DECL,
        //Added By Anil June 14 2005 --starts here
        RUL_DD_STRING_DECL,
        //Added By Anil June 14 2005 --Ends here
        RUL_UNSIGNED_CHAR_DECL,
        //Symbols
        RUL_LPAREN,
        RUL_RPAREN,
        RUL_LBRACK,
        RUL_RBRACK,
        RUL_LBOX,
        RUL_RBOX,
        RUL_SEMICOLON,
        RUL_COLON,
        RUL_COMMA,
        RUL_DOT,
        RUL_QMARK,
        RUL_SCOPE,
        //Constants
        RUL_CHAR_CONSTANT,
        RUL_INT_CONSTANT,
        RUL_REAL_CONSTANT,
        RUL_BOOL_CONSTANT,
        RUL_STRING_CONSTANT,
        //Service SubTypes
        RUL_SERVICE_INVOKE,
        RUL_SERVICE_ATTRIBUTE,
        //Rule Self Invoke
        RUL_RULE_ENGINE,
        RUL_INVOKE,
        //Object manager
        RUL_OM,
        //General
        RUL_DOLLAR,
        RUL_SUBTYPE_ERROR,
        // Jump statements
        RUL_BREAK,
        RUL_CONTINUE,
        RUL_RETURN,
        // Bit wise operators
        RUL_BIT_AND,
        RUL_BIT_OR,
        RUL_BIT_XOR,
        RUL_BIT_NOT,
        RUL_BIT_RSHIFT,
        RUL_BIT_LSHIFT,
        // Function
        RUL_FUNCTION,
        // DD (Global) Identifier
        RUL_DD_SIMPLE,//Anil August 26 2005 For handling DD variable
        RUL_DD_COMPLEX,//Anil August 26 2005 For handling DD variable
        RUL_DD_METHOD//Anil Octobet 5 2005 for handling Method Calling Method
    }

    public enum STATEMENT_TYPE
    {
        STMT_DECL = 0,
        STMT_ASSIGNMENT,
        STMT_SELECTION,
        STMT_ELSE,
        STMT_ITERATION,
        STMT_COMPOUND,
        STMT_OPTIONAL,
        STMT_EXPRESSION,
        STMT_SERVICE,
        STMT_RUL_INVOKE,
        STMT_asic,
        STMT_ASSIGNMENT_FOR,
        STMT_BREAK,
        STMT_CONTINUE,
        STMT_RETURN,
        STMT_CASE
    }

    public enum STMT_EXPR_TYPE
    {
        EXPR_NONE = 0,
        EXPR_ASSIGN,
        EXPR_IF,
        EXPR_WHILE,
        EXPR_FOR,
        EXPR_LVALUE,
        EXPR_CASE
    }

    public enum DD_METH_AGR_PASSED_TYPE
    {
        DD_METH_AGR_PASSED_UNKNOWN = 0,
        DD_METH_AGR_PASSED_BYVALUE = 1,
        DD_METH_AGR_PASSED_BYREFERENCE = 2
    }

    public class METHOD_ARG_INFO_VECTOR : List<METHOD_ARG_INFO>
    {

    }

    public class METHOD_ARG_INFO
    {
        string m_pchCalledArgName;
        string m_pchCallerArgName;
        RUL_TOKEN_TYPE m_Type;
        RUL_TOKEN_SUBTYPE m_SubType;

        public bool m_IsReturnVar;
        DD_METH_AGR_PASSED_TYPE ePassedType;
        UInt64 ulDDItemId;

        public METHOD_ARG_INFO()
        {
            m_pchCalledArgName = null;
            ePassedType = DD_METH_AGR_PASSED_TYPE.DD_METH_AGR_PASSED_UNKNOWN;
            m_Type = RUL_TOKEN_TYPE.RUL_TYPE_NONE;
            m_SubType = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;
            ulDDItemId = 0;
            m_IsReturnVar = false;
        }

        public void SetCalledArgName(string pCalledArgName)
        {
            //Assgn the vlues
            m_pchCalledArgName = pCalledArgName;
        }
        public void SetCallerArgName(string pCallerArgName)
        {
            m_pchCallerArgName = pCallerArgName;
            //Fille the Caller Arg name also

        }

        public void SetType(RUL_TOKEN_TYPE eType)
        {
            m_Type = eType;
        }
        public void SetSubType(RUL_TOKEN_SUBTYPE eSubType)
        {
            m_SubType = eSubType;
        }

        new public RUL_TOKEN_TYPE GetType()
        {
            return m_Type;
        }
        RUL_TOKEN_SUBTYPE GetSubType()
        {
            return m_SubType;
        }

        string GetCalledArgName()
        {
            return m_pchCalledArgName;

        }

        string GetCallerArgName()
        {
            return m_pchCallerArgName;

        }

    }

    public class OneMeth
    {
        public const int RESP_MASK_LEN = 16;/* size of response code masks		*/
        public const int DATA_MASK_LEN = 25;    /* size of data masks				*/

        /*device status codes used in defaults*/
        public const int DEVICE_MALFUNCTION = 0x80;
        public const int CMD_NOT_IMPLIMENTED = 0x40;
        public const int ACCESS_RESTRICTED = 0x10;

        public const int BI_SUCCESS = 0;    /* task succeeded in intended task	  */
        public const int BI_ERROR = -1;     /* error occured in task			  */
        public const int BI_ABORT = -2;/* user aborted task				  */
        public const int BI_NO_DEVICE = -3;             /* no device found on comm request	  */
        public const int BI_COMM_ERR = -4;          /* communications error				  */
        public const int BI_CONTINUE = -5;      /* continue */
        public const int BI_RETRY = -6; /* retry */
        public const int BI_PORT_IN_USE = -7;		/* block transfer port */

        public const int MAX_METHOD_LIST = 20;

        //methErrors_t latestMethodStatus;

        CInterpreter m_pInterpreter;
        CHart_Builtins m_pBuiltinLib;

        /* The Item Id of the method being executed */
        int m_lMethodItemId;

        //string m_pchMethodSourceCode;

        /* Item id of the Item for which Pre/post action is being executed */
        int m_lPrePostItemId;

        /* List that holds the abort methods */
        List<int> abortMethodList;
        //ITEMID_LIST::iterator ListIterator;

        /* Flag is set to true if any of the ABORT builtins are called */
        bool m_bMethodAborted;

        /* Flag that is set when save_values is called */
        //bool	m_bSaveValues;

        public int m_iAutoRetryLimit;

        public byte m_byCommAbortMask;
        public byte m_byCommRetryMask;
        public byte m_byStatusAbortMask;
        public byte m_byStatusRetryMask;
        public byte[] m_byRespAbortMask = new byte[RESP_MASK_LEN];
        public byte[] m_byRespRetryMask = new byte[RESP_MASK_LEN];
        public short m_byReturnNodevAbortMask;
        public short m_byReturnNodevRetryMask;

        public byte m_byXmtrCommAbortMask;
        public byte m_byXmtrCommRetryMask;
        public byte m_byXmtrStatusAbortMask;
        public byte m_byXmtrStatusRetryMask;
        public byte[] m_byXmtrRespAbortMask = new byte[RESP_MASK_LEN];
        public byte[] m_byXmtrRespRetryMask = new byte[RESP_MASK_LEN];
        public short m_byXmtrReturnNodevAbortMask;
        public short m_byXmtrReturnNodevRetryMask;
        public byte[] m_byXmtrDataAbortMask = new byte[DATA_MASK_LEN];
        public byte[] m_byXmtrDataRetryMask = new byte[DATA_MASK_LEN];

        HARTDevice m_pDevice;

        MEE m_pMEE;

        public OneMeth()
        {
            abortMethodList = new List<int>();

            m_iAutoRetryLimit = 0;

            m_byCommAbortMask = 0;
            m_byCommRetryMask = 0x7F;
            m_byStatusAbortMask = DEVICE_MALFUNCTION;
            m_byStatusRetryMask = 0;
            m_byReturnNodevAbortMask = 1;
            m_byReturnNodevRetryMask = 0;

            m_byXmtrCommAbortMask = 0; ;
            m_byXmtrCommRetryMask = 0x7F;
            m_byXmtrStatusAbortMask = DEVICE_MALFUNCTION;
            m_byXmtrStatusRetryMask = 0;
            m_byXmtrReturnNodevAbortMask = 1;
            m_byXmtrReturnNodevRetryMask = 0;

            int i = 0;
            for (i = 0; i < RESP_MASK_LEN; i++)
            {
                m_byRespAbortMask[i] = 0;
                m_byRespRetryMask[i] = 0;
                m_byXmtrRespAbortMask[i] = 0;
                m_byXmtrRespRetryMask[i] = 0;
            }

            m_byRespAbortMask[2] = 0x01;        /*ACCESS_RESTRICTED*/
            m_byRespAbortMask[8] = 0x01;        /*CMD_NOT_IMPLIMENTED*/
            m_byXmtrRespAbortMask[2] = 0x01;    /*ACCESS_RESTRICTED*/
            m_byXmtrRespAbortMask[8] = 0x01;    /*CMD_NOT_IMPLIMENTED*/

            for (i = 0; i < DATA_MASK_LEN; i++)
            {
                m_byXmtrDataAbortMask[i] = 0;
                m_byXmtrDataRetryMask[i] = 0;
            }
        }

        public int process_abort()
        {
            m_bMethodAborted = true;
            return 0;
        }

        public int ExecuteActionsInMethod(List<int> actionList)
        {

            for (int iCount = 0; iCount < (int)actionList.Count; iCount++)     // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
            {
                int lMethodItemId = actionList[iCount];
                bool bRet = m_pMEE.ExecuteMethod(m_pDevice, lMethodItemId);
                if (false == bRet)
                {
                    return Common.FAILURE;
                }
            }

            return Common.SUCCESS;
        }

        public bool ExecuteMethod(MEE pMEE, HARTDevice pDevice, int lMethodItemId, int lVarItemId = 0)
        {
            if (pDevice == null)
            {
                return false;
            }

            if (0 != lVarItemId)
            {
                m_lPrePostItemId = lVarItemId;
            }

            m_pDevice = pDevice;
            m_pMEE = pMEE;///

            //	pDevice.cacheDispValues(); 

            INTERPRETER_STATUS intStatus = ExecMethod(lMethodItemId);

            if (intStatus != INTERPRETER_STATUS.INTERPRETER_STATUS_OK)
            {
                //m_bSaveValues = false;
                // replaced by below----HandleSaveValues();
                //pDevice.uncacheDispVals(m_bSaveValues);

                return false;
            }

            if (m_bMethodAborted == true || m_pMEE.latestMethodStatus == methErrors_t.me_Aborted)
            {// stevev - 14jun13 - this code is never called in an abort sequence.
                foreach (int lQueueMethodId in abortMethodList)
                {
                    intStatus = ExecMethod(lQueueMethodId);
                    if (intStatus != INTERPRETER_STATUS.INTERPRETER_STATUS_OK)
                    {
                        //m_bSaveValues = false;
                        // replaced by below----HandleSaveValues();
                        //pDevice.uncacheDispVals(m_bSaveValues);
                        return false;
                    }
                    if (m_pMEE != null)// Copied from the Method-calling-Method version below
                    {//                     specifically to stop a simple error return on methods that
                     //                     end via ProcessAbort.
                        m_pMEE.latestMethodStatus = methErrors_t.me_Aborted;
                    }
                }
            }

            // replaced by below----HandleSaveValues();
            //pDevice.uncacheDispVals(m_bSaveValues);
            if (m_bMethodAborted == true)
                return false;

            return true;
        }
        //Anil Octobet 5 2005 for handling Method Calling Method
        //Added Overloaded function 
        public bool ExecuteMethod(MEE pMEE, ref HARTDevice pDevice, int lMethodItemId, ref METHOD_ARG_INFO_VECTOR vectMethArg, List<INTER_VARIANT> vectInterVar, int lVarItemId = 0)
        {
            if (pDevice == null)
            {
                return false;
            }

            if (0 != lVarItemId)
            {
                m_lPrePostItemId = lVarItemId;
            }

            m_pMEE = pMEE; //Initialize the MEE pointer;

            m_pDevice = pDevice;

            //	pDevice.cacheDispValues(); 

            INTERPRETER_STATUS intStatus = ExecMethod(lMethodItemId, vectMethArg, vectInterVar);

            if (intStatus != INTERPRETER_STATUS.INTERPRETER_STATUS_OK)
            {
                //		m_bSaveValues = false;
                // replaced by below----HandleSaveValues();
                //		pDevice.uncacheDispVals(m_bSaveValues);

                return false;
            }

            if (m_bMethodAborted == true || m_pMEE.latestMethodStatus == methErrors_t.me_Aborted)
            {
                foreach (int lQueueMethodId in abortMethodList)
                {
                    intStatus = ExecMethod(lQueueMethodId);
                    if (intStatus != INTERPRETER_STATUS.INTERPRETER_STATUS_OK)
                    {
                        //m_bSaveValues = false;
                        // replaced by below----HandleSaveValues();
                        //pDevice.uncacheDispVals(m_bSaveValues);
                        return false;
                    }
                }
                if (m_pMEE != null)
                {
                    m_pMEE.latestMethodStatus = methErrors_t.me_Aborted;
                }
            }

            // replaced by below----HandleSaveValues();
            //pDevice.uncacheDispVals(m_bSaveValues);
            if (m_bMethodAborted == true)
                return false;

            return true;
        }

        private INTERPRETER_STATUS ExecMethod(int lMethodItemId)
        {
            CDDLBase p_ib = null;
            CDDLMethod p_m = null;
            bool rc = m_pDevice.getItembyID((uint)lMethodItemId, ref p_ib);
            if (!(rc && p_ib != null && p_ib.eType == nitype.nMethod))
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
            }

            m_lMethodItemId = lMethodItemId;

            p_m = (CDDLMethod)p_ib;
            string pS = p_m.getDef();

            if (pS == null)
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
            }

            string pbyData = "";//[10000] = "";
            string pbySymbolTable = "";

            AllocLibrary();
            m_pBuiltinLib.Initialise(m_pDevice, m_pInterpreter, this);
            m_pBuiltinLib.Initialise(m_pDevice, m_pInterpreter, m_lPrePostItemId); /* VMKP added on 200204 */
            //	char pchCode[] = {"{float tmp;long lValue[3];tmp=2.8;lValue[0] = 150;lValue[1] = 151;lValue[2] = 152;acknowledge(\"Test string%{tmp} and val is %{0}\", lValue);}"};

            if (m_bMethodAborted == true)
            {
                this.m_pBuiltinLib.m_AbortInProgress = true;
            }
            m_pMEE.methodNameString = p_m.GetName();

            INTERPRETER_STATUS intStatus = m_pInterpreter.ExecuteCode(ref m_pBuiltinLib, pS, "Test", pbyData, pbySymbolTable, ref m_pMEE);
            if (intStatus != INTERPRETER_STATUS.INTERPRETER_STATUS_OK)
            {
                switch (intStatus)
                {
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_INVALID:
                        DisplayMessage("**** Internal Error: ****\nInterpreter State Unknown !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_INVALID;
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR:
                        DisplayMessage("**** Internal Error: ****\nMethod code has parsing errors !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR;
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR:
                        DisplayMessage("**** Internal Error: ****\nError in method execution !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_UNKNOWN_ERROR:
                        DisplayMessage("**** Internal Error: ****\nInterpreter State Unknown !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_UNKNOWN_ERROR;
                }
            }
            else if (m_pMEE != null && m_pMEE.latestMethodStatus == methErrors_t.me_Aborted)
            {
                m_bMethodAborted = true;
            }

            //commented by prashant 230204
            //DisplayMessage("", false);
            //prashant 230204 end
            FreeLibrary();
            return intStatus;
        }

        //Anil Octobet 5 2005 for handling Method Calling Method
        //Added an OverLoaded function
        INTERPRETER_STATUS ExecMethod(int lMethodItemId, METHOD_ARG_INFO_VECTOR vectMethArg, List<INTER_VARIANT> vectInterVar)
        {
            CDDLBase p_ib = null;
            CDDLMethod p_m = null;
            bool rc = m_pDevice.getItembyID((uint)lMethodItemId, ref p_ib);
            if (!(rc && p_ib != null && p_ib.eType == nitype.nMethod))
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
            }

            m_lMethodItemId = lMethodItemId;

            p_m = (CDDLMethod)p_ib;
            string pS = p_m.GetName();//.getDef(sLen);

            if (pS == null)
            {
                return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
            }

            string pbyData = "";//[10000] = "";
            string pbySymbolTable = "";

            AllocLibrary();
            m_pBuiltinLib.Initialise(m_pDevice, m_pInterpreter, this);
            m_pBuiltinLib.Initialise(m_pDevice, m_pInterpreter, m_lPrePostItemId); /* VMKP added on 200204 */
            //	char pchCode[] = {"{float tmp;long lValue[3];tmp=2.8;lValue[0] = 150;lValue[1] = 151;lValue[2] = 152;acknowledge(\"Test string%{tmp} and val is %{0}\", lValue);}"};

            if (m_bMethodAborted == true)
            {
                this.m_pBuiltinLib.m_AbortInProgress = true;
            }
            m_pMEE.methodNameString = p_m.GetName();

            INTERPRETER_STATUS intStatus = m_pInterpreter.ExecuteCode(m_pBuiltinLib, pS, "Test", pbyData, pbySymbolTable, m_pMEE, vectMethArg, vectInterVar);
            if (intStatus != INTERPRETER_STATUS.INTERPRETER_STATUS_OK)
            {
                switch (intStatus)
                {
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_INVALID:
                        DisplayMessage("**** Internal Error: ****\nInterpreter State Unknown !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_INVALID;
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR:
                        DisplayMessage("**** Internal Error: ****\nMethod code has parsing errors !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_PARSE_ERROR;
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR:
                        DisplayMessage("**** Internal Error: ****\nError in method execution !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_EXECUTION_ERROR;
                    case INTERPRETER_STATUS.INTERPRETER_STATUS_UNKNOWN_ERROR:
                        DisplayMessage("**** Internal Error: ****\nInterpreter State Unknown !");
                        FreeLibrary();
                        return INTERPRETER_STATUS.INTERPRETER_STATUS_UNKNOWN_ERROR;
                }
            }
            else if (m_pMEE != null && m_pMEE.latestMethodStatus == methErrors_t.me_Aborted)
            {
                m_bMethodAborted = true;
            }

            //commented by prashant 230204
            //DisplayMessage("", false);
            //prashant 230204 end
            FreeLibrary();
            return intStatus;
        }

        public void abort()
        {
            m_bMethodAborted = true;
        }

        public int _add_abort_method(int lMethodId)
        {
            int nRetVal = BI_ERROR;

            //AOEP35747
            if (abortMethodList.Count < MAX_METHOD_LIST)
            {
                abortMethodList.Add(lMethodId);
                nRetVal = BI_SUCCESS;
            }
            return nRetVal;
        }

        public int _remove_abort_method(int lMethodId)
        {
            int nRetVal = BI_ERROR;

            if (!GetMethodAbortStatus())
            {
                abortMethodList.Remove(lMethodId);
                nRetVal = BI_SUCCESS;
                /*
                ListIterator = abortMethodList.begin();
                while (ListIterator != abortMethodList.end())
                {
                    long lQueueMethodId = *ListIterator;
                    if (lQueueMethodId == lMethodId)
                    {
                        abortMethodList.erase(ListIterator);
                        ListIterator = abortMethodList.begin();
                        nRetVal = BI_SUCCESS;
                        break;/*Vibhor 030305: Bug Fix : As per the definition in the spec it should only 
					remove the first occurence of a method in the abort list
			        So,loop no further after the first one !!
                    }
                    else
                    {
                        ListIterator++;
                    }
                }
                */
            }
            return nRetVal;
        }

        public int remove_all_abort()
        {
            int nRetVal = BI_ERROR;

            if (!GetMethodAbortStatus())
            {
                abortMethodList.Clear();
                nRetVal = BI_SUCCESS;
            }

            return nRetVal;
        }

        public void save_values()
        {
            m_pMEE.m_bSaveValues = true;//m_bSaveValues = true;
        }

        //Added By Anil July 01 2005 --starts here
        public void discard_on_exit()
        {
            if (m_pMEE.m_bSaveValues == true)
            {
                m_pMEE.m_bSaveValues = false;
            }
        }
        //Added By Anil July 01 2005 --Ends here

        public bool GetMethodAbortStatus()
        {
            return m_bMethodAborted;
        }

        //bool RefreshWaveform(long lRfrshMethID);

        //int ExecuteActions(ddbItemList_t actionList);

        /*Arun 190505 Start of code */

        public int _push_abort_method(int lMethodId)
        {
            int nRetVal = BI_ERROR;

            if (abortMethodList.Count < MAX_METHOD_LIST)
            {
                abortMethodList.Insert(0, lMethodId);
                nRetVal = BI_SUCCESS;
            }

            return nRetVal;
        }

        public int _pop_abort_method()
        {
            int nRetVal = BI_ERROR;
            if (!GetMethodAbortStatus())
            {
                if (abortMethodList.Count != 0)
                {
                    abortMethodList.RemoveAt(0);
                    nRetVal = BI_SUCCESS;
                }
            }
            return nRetVal;
        }

        /*End of code*/

        //returncode ExecuteActionsInMethod(List<uint> actionList);

        void AllocLibrary()
        {
            m_pInterpreter = new CInterpreter();
            m_pBuiltinLib = new CHart_Builtins();

        }

        void FreeLibrary()
        {
            m_pInterpreter = null;
            m_pBuiltinLib = null;
        }

        void DisplayMessage(string pchMessage, bool bUserAcknowledgeValue = true)
        {
            ACTION_USER_INPUT_DATA structUserInput = new ACTION_USER_INPUT_DATA();
            ACTION_UI_DATA structUIData = new ACTION_UI_DATA();

            structUIData.userInterfaceDataType = UI_DATA_TYPE.TEXT_MESSAGE;
            structUIData.bUserAcknowledge = bUserAcknowledgeValue;
            /*Vibhor 030304: Start of Code*/
            structUIData.bEnableAbortOnly = false; // just defensive
            /*Vibhor 030304: End of Code*/
            /*Vibhor 040304: Start of Code*/
            structUIData.uDelayTime = 0;// just defensive
            /*Vibhor 040304: End of Code*/

            structUIData.textMessage.pchTextMessage = pchMessage;
            structUIData.bDisplayDynamic = false;   //Added by Prashant 20FEB2004
            m_pDevice.m_pMethSupportInterface.MethodDisplay(structUIData, ref structUserInput);

            return;
        }

        //public HARTDevice m_pDev; //Vibhor 010705: Made it public to expose global symbol table

    }

}
