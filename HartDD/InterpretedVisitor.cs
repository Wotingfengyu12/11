using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CInterpretedVisitor : CGrammarNodeVisitor
    {
        public const int VISIT_ERROR = 0;
        public const int VISIT_BREAK = 1;
        public const int VISIT_CONTINUE = 2;
        public const int VISIT_RETURN = 3;
        public const int VISIT_NORMAL = 4;
        public const int VISIT_SCOPE_VAR = 5;

        public const int MAX_INT_DIGITS = 10;
        public const int MAX_LOOPS = 0xffff;
        public const int MAX_NUMBER_OF_FUNCTION_PARAMETERS = 10;

        delegate int PFN_INTERPRETER(ref INTER_VARIANT V1, ref INTER_VARIANT V2, ref INTER_VARIANT V3);
        PFN_INTERPRETER[] m_fnTable = new PFN_INTERPRETER[255];

        bool m_IsLValue;
        CHart_Builtins m_pBuiltInLib;

        MEE m_pMEE; //Vibhor 070705: Added
        bool m_bIsRoutine;//Anil Octobet 5 2005 for handling Method Calling Method

        public CInterpretedVisitor()
        {
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS_PLUS] = new PFN_INTERPRETER(uplusplus);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS_MINUS] = new PFN_INTERPRETER(uminusminus);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_PRE_PLUS_PLUS] = new PFN_INTERPRETER(upreplusplus);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_PRE_MINUS_MINUS] = new PFN_INTERPRETER(upreminusminus);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_UPLUS] = new PFN_INTERPRETER(uplus);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_UMINUS] = new PFN_INTERPRETER(uminus);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_AND] = new PFN_INTERPRETER(bitand);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_OR] = new PFN_INTERPRETER(bitor);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_XOR] = new PFN_INTERPRETER(bitxor);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_NOT] = new PFN_INTERPRETER(bitnot);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT] = new PFN_INTERPRETER(bitrshift);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT] = new PFN_INTERPRETER(bitlshift);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS] = new PFN_INTERPRETER(add);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS] = new PFN_INTERPRETER(sub);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MUL] = new PFN_INTERPRETER(mul);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_DIV] = new PFN_INTERPRETER(div);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MOD] = new PFN_INTERPRETER(mod);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_EXP] = new PFN_INTERPRETER(exp);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_NOT_EQ] = new PFN_INTERPRETER(neq);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_LT] = new PFN_INTERPRETER(lt);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_GT] = new PFN_INTERPRETER(gt);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_EQ] = new PFN_INTERPRETER(eq);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_GE] = new PFN_INTERPRETER(ge);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_LE] = new PFN_INTERPRETER(le);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_AND] = new PFN_INTERPRETER(land);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_OR] = new PFN_INTERPRETER(lor);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_LOGIC_NOT] = new PFN_INTERPRETER(lnot);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_RPAREN] = new PFN_INTERPRETER(rparen);

            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_ASSIGN] = new PFN_INTERPRETER(assign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN] = new PFN_INTERPRETER(plusassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN] = new PFN_INTERPRETER(minusassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN] = new PFN_INTERPRETER(divassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN] = new PFN_INTERPRETER(modassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN] = new PFN_INTERPRETER(mulassign);

            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN] = new PFN_INTERPRETER(bitandassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN] = new PFN_INTERPRETER(bitorassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN] = new PFN_INTERPRETER(bitxorassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN] = new PFN_INTERPRETER(rshiftassign);
            m_fnTable[(int)RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN] = new PFN_INTERPRETER(lshiftassign);

        }
        public void Initialize(CHart_Builtins pBuiltInLib, MEE pMEE) //Vibhor 070705: Modified
        {
            m_pBuiltInLib = pBuiltInLib;
            m_pMEE = pMEE;
        }

        public override int visitArrayExpression(CArrayExpression pArrExp, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            int i32Idx = pArrExp.GetToken().GetSymbolTableIndex();
            CVariable pVariable = pSymbolTable.GetAt(i32Idx);
            INTER_SAFEARRAY prgsa = pVariable.GetValue().GetValue().prgsa;

            //evaluate the expressions...
            EXPR_VECTOR pvecExpressions = pArrExp.GetExpressions();
            int i32Count = pvecExpressions.Count;
            INTER_VARIANT var = new INTER_VARIANT();
            List<int> vecDims = new List<int>();


            prgsa.GetDims(ref vecDims);
            int i32mem = prgsa.MemoryAllocated();
            int i32loc = 0;

            for (int i = 0; i < i32Count; i++)
            {
                if ((int)vecDims.Count >= i + 1)// WS:EPM 17jul07
                {
                    i32mem = i32mem / vecDims[i];
                }
                //var.Clear();
                pvecExpressions[i].Execute(this, pSymbolTable, ref var);
                //i32loc = i32loc + i32mem * (int)var;
                i32loc = i32loc + i32mem * var.val.nValue;
            }

            if (m_IsLValue)
            {
                m_IsLValue = false;
                //lvalue
                prgsa.SetElement((uint)i32loc, pvar);
            }
            else
            {
                //rvalue
                //pvar.Clear();
                prgsa.GetElement((uint)i32loc, ref pvar);
            }
            return VISIT_NORMAL;
        }

        public override int visitComplexDDExpression(CComplexDDExpression pArrExp, CSymbolTable pSymbolTable, 
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            int i32Idx = pArrExp.GetToken().GetSymbolTableIndex();
            CVariable pVariable = m_pMEE.m_GlobalSymTable.GetAt(i32Idx);

            //evaluate the expressions...
            EXPR_VECTOR pvecExpressions = pArrExp.GetExpressions();
            int i32Count = pvecExpressions.Count;
            INTER_VARIANT var = new INTER_VARIANT();

            //Added:Anil Octobet 5 2005 for handling Method Calling Method
            //Added the code for the DD Methos Execution
            if (RUL_TOKEN_SUBTYPE.RUL_DD_METHOD == pVariable.Token.GetSubType())
            {
                // Fill the current values of all the Method Agrument
                List<INTER_VARIANT> vectInterVar = new List<INTER_VARIANT>();
                int i = 0;   // WS - 9apr07 - 2005 checkin
                for (i = 0; i < i32Count; i++)// WS - 9apr07 - 2005 checkin
                {
                    //var.Clear();
                    pvecExpressions[i].Execute(this, pSymbolTable, ref var);
                    vectInterVar.Add(var);
                }

                //Fill all the methos Agrument info like pchCallerArgName and ..._TYPE and ..._SUBTYPE
                int iParamCount = 0;
                string szDDitemName;
                string pszComplexDDExpre;
                szDDitemName = pVariable.Token.GetDDItemName();
                pszComplexDDExpre = pVariable.Token.GetLexeme();
                ushort NoOfParams = 0;

                METHOD_ARG_INFO_VECTOR vectMethArgInfo = new METHOD_ARG_INFO_VECTOR();
                //From here just extrac the Arg list and fill it out
                {
                    int iLeftPeranthis = 0;
                    bool bValidMethodCall = false;
                    i = szDDitemName.Length;
                    //Checkk for the Valid Method call, 
                    //ie Method call should Start and end with open and Close Parenthesis respectively
                    for (; i < pszComplexDDExpre.Length; i++)  // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
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
                        return VISIT_ERROR;
                    }
                    //Now strat extracting the each Argument name and push it in the strvCallerArgList vector
                    int iNoOfchar = 0;
                    int lstlen = pszComplexDDExpre.Length;
                    for (; i < (lstlen); i++)
                    {
                        //Look for the space and do not count
                        if ((' ' != pszComplexDDExpre[i]))
                        {
                            iNoOfchar++;
                        }
                        //If u find ')', reduce the iLeftPeranthis and 
                        //when u go out of this loop iLeftPeranthis dhould be zero		
                        if (')' == pszComplexDDExpre[i])
                        {
                            iLeftPeranthis--;

                        }
                        //If u find '(', increase the iLeftPeranthis and 
                        //when u go out of this loop iLeftPeranthis dhould be zero		
                        if ('(' == pszComplexDDExpre[i])
                        {
                            iLeftPeranthis++;

                        }
                        //if it is ',' or last ")", AND there is an arg name (no args, skip to else)
                        if (((pszComplexDDExpre[i] == ',') || (0 == iLeftPeranthis)) && (iNoOfchar > 1))
                        {
                            //do insert here, Get the start pos of the arg name
                            iParamCount++;
                            //do insert here				
                            int istartPosOfPassedItem = 0;
                            iNoOfchar--;//Because ; or ) is included
                            int iNoOfSpaces = 0;
                            //It may so happen that for the arg there are space before , or ')'
                            //EG: ( ArgnameOne                  ,   argName2        ), thats why this below loop			
                            for (int x = i - 1; ; x--)
                            {
                                if (' ' == pszComplexDDExpre[x])
                                {
                                    iNoOfSpaces++;

                                }
                                else
                                {
                                    break;
                                }

                            }
                            //Get the strating position and Char count
                            istartPosOfPassedItem = i - iNoOfchar - iNoOfSpaces;
                            int iCount = iNoOfchar + 1;
                            string pchDecSource;// +1 for Null Char +1 for ; -1 for as it had counted ]
                            //strncpy(pchDecSource = &pszComplexDDExpre[istartPosOfPassedItem],iNoOfchar);
                            pchDecSource = pszComplexDDExpre.Substring(istartPosOfPassedItem, iNoOfchar);
                            //Find it in Synbol table??
                            //I got the Arg name from here So find it in Symbol table,
                            //If it is DD item, or DD expressiom then pCVariable will be null 
                            //TODO handle the DD item case
                            int idex = 0;
                            CVariable pCVariable = pSymbolTable.Find(pchDecSource, ref idex);
                            METHOD_ARG_INFO stMethArg = new METHOD_ARG_INFO();
                            //stMethArg
                            if ((pCVariable != null) && ( RUL_TOKEN_TYPE.RUL_ARRAY_VARIABLE == pCVariable.Token.GetType()))// || (RUL_SAFEARRAY == pCVariable.GetType() )))
                            {
                                pvar = pCVariable.GetValue();
                                vectInterVar[NoOfParams] = pCVariable.GetValue();

                            }
                            if (pCVariable == null)
                            {
                                //This can be a case of Constant passed or from the DD item
                                //So have to Diifferenrialte between two
                                //Call mee to know wether it is DD item
                                //bool bIsComplex = false;

                                // warning C4288: nonstandard extension used : 'iCount' : loop control 
                                //  variable declared in the for-loop is used outside the for-loop scope;
                                //  it conflicts with the declaration in the outer scope
                                // HOMZ - solution: Move int iCount outside the loop...
                                // stevev..already defined higher up.... int iCount = 0;
                                int acharCnt = 0;// this could identify a float constant, preclude that
                                for (iCount = 0; iCount < pchDecSource.Length; iCount++)
                                // warning C4018: '>=' : signed/unsigned mismatch <HOMZ: added cast>
                                {
                                    // stevev 20feb09 - preclude float strings
                                    //if (!strchr("0123456789+-eE.", pchDecSource[iCount]))
                                    string fl = "0123456789+-eE.";
                                    int t = fl.IndexOf(pchDecSource[iCount]);
                                    if(t == 0)
                                    {// non float format char
                                        acharCnt++;
                                    }
                                    if (('.' == pchDecSource[iCount] && acharCnt > 0) || ('[' == pchDecSource[iCount]))
                                    {
                                        //bIsComplex = true;
                                        break;
                                    }
                                }
                                //string szDDitemName = new char[iCount + 1];
                                //strncpy(szDDitemName, &pchDecSource[0], iCount);
                                //szDDitemName[iCount] = '\0';
                                szDDitemName = pchDecSource.Substring(0, iCount);

                                //Check for the case of DD item...
                                if (m_pMEE.IsDDItem(szDDitemName))
                                {
                                    stMethArg.SetCallerArgName(pchDecSource);
                                    stMethArg.SetType(RUL_TOKEN_TYPE.RUL_DD_ITEM);
                                    stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_DD_COMPLEX);
                                    vectMethArgInfo.Add(stMethArg);
                                    //it may so happen that  after resolving this DD expressionn it is 
                                    //          again a Constant parameter
                                    /*
                                    pvar = new INTER_VARIANT();
                                    int iReturnValue = m_pMEE.ResolveDDExp(pchDecSource, szDDitemName, ref pvar);
                                    if (iReturnValue == Common.FAILURE)
                                    {
                                        //  it is Surely an DD item reference
                                    }
                                    else
                                    {
                                        //it may or may not DD item, it may be Constant also...refere below Example
                                        vectInterVar[NoOfParams] = pvar;
                                    }
                                    */
                                }
                                else
                                {
                                    //This is surely a Constant Passed to Method
                                    //RUL_NUMERIC_CONSTANT, 
                                    //RUL_STR_CONSTANT,
                                    //RUL_CHR_CONSTANT,	
                                    if (NoOfParams < vectInterVar.Count)//dont go beyond the end of the array- Walt EPM 08sep08
                                    {
                                        INTER_VARIANT varTemp = vectInterVar[NoOfParams];
                                        stMethArg.SetCallerArgName("PassedByConstant");
                                        stMethArg.SetType(RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE);
                                        stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE);
                                        switch (varTemp.GetVarType())
                                        {
                                            case VARIANT_TYPE.RUL_CHAR:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL);
                                                break;
                                            case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL);
                                                break;
                                            case VARIANT_TYPE.RUL_USHORT:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL);
                                                break;
                                            case VARIANT_TYPE.RUL_SHORT:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL);
                                                break;
                                            case VARIANT_TYPE.RUL_INT:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL);
                                                break;
                                            case VARIANT_TYPE.RUL_UINT:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL);
                                                break;
                                            case VARIANT_TYPE.RUL_LONGLONG:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL);
                                                break;

                                            case VARIANT_TYPE.RUL_BOOL:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL);
                                                break;

                                            case VARIANT_TYPE.RUL_FLOAT:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_REAL_DECL);
                                                break;

                                            case VARIANT_TYPE.RUL_ULONGLONG:
                                            case VARIANT_TYPE.RUL_DOUBLE:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL);
                                                break;

                                            case VARIANT_TYPE.RUL_CHARPTR:
                                                stMethArg.SetSubType(RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL);
                                                break;
                                            default:
                                                return Common.FAILURE; //we made a bad assumption above.

                                        }
                                        vectMethArgInfo.Add(stMethArg);
                                    }
                                    else
                                    {
                                        return Common.FAILURE; //we made a bad assumption above.
                                    }
                                }

                                if (szDDitemName != null)// memory leak plug emerson checkin april2013
                                {
                                    szDDitemName = null;
                                }
                            }
                            else
                            {
                                stMethArg.SetCallerArgName(pchDecSource);
                                stMethArg.SetType(pCVariable.Token.GetType());
                                stMethArg.SetSubType(pCVariable.Token.GetSubType());
                                vectMethArgInfo.Add(stMethArg);
                            }
                            NoOfParams++;
                            if (pchDecSource != null)
                            {
                                pchDecSource = null;
                            }
                            iNoOfchar = 0;

                        }
                        //else (not ',' and not last ')') or no chars encountered so far <eg meth(,)>
                        if (0 == iLeftPeranthis)// no more parameters <final ')' found>..get out
                            break;
                    }// next character (i) in parameter list string
                }// end of block

                //When I come here, vectMethArgInfo is filled with the Parameter that are passed to 
                //the called method  and  vectInterVar is filled with its corresponding values
                /*
                RETURNCODE iReturnValue = m_pMEE.ResolveMethodExp(pVariable.GetLexeme(), pVariable.GetDDItemName(), pvar, &vectInterVar, &vectMethArgInfo);
                if (iReturnValue == FAILURE)
                {
                    return FAILURE;
                }
                */
                //Once we Execute this method, We need to again fill the values of those parameter, 
                //	which are passed by reference
                /*
                int iNoOfArgs = vectMethArgInfo.Count;
                for (i = 0; i < iNoOfArgs; i++)
                {
                    //Check for the Parameter which are passed by reference other than return 
                    //For return it is self generated parameter for temporory purpose which are
                    //	always passed by reference
                    if ((vectMethArgInfo[i].ePassedType == DD_METH_AGR_PASSED_BYREFERENCE) &&
                         !(vectMethArgInfo[i].m_IsReturnVar))
                    {
                        //Check for Simple var , in which case it is direct value assgnment
                        if (RUL_SIMPLE_VARIABLE == vectMethArgInfo[i].GetType())
                        {
                            CVariable pStore = null;
                            pStore = pSymbolTable.Find(vectMethArgInfo[i].GetCallerArgName());
                            INTER_VARIANT fdftemp = vectInterVar[i];
                            pStore.GetValue() = vectInterVar[i];
                        }//End of Simple var
                         //Check for array, where ass is not direct...Assignment is by index basis
                        else
                        if (RUL_ARRAY_VARIABLE == vectMethArgInfo[i].GetType())
                        {

                            CVariable* pVariable =
                                pSymbolTable.Find(vectMethArgInfo[i].GetCallerArgName());

                            INTER_VARIANT* vartemp = &vectInterVar[i];

                            INTER_SAFEARRAY* prgsaCalled = vartemp.GetValue().prgsa;

                            //Extract the dimention and assign each of them
                            vector<int> vecDims;
                            prgsaCalled.GetDims(&vecDims);
                            int i32mem = prgsaCalled.MemoryAllocated();
                            int iMemsize = i32mem / vecDims[0];
                            int iArraysize = vecDims[0];


                            //Get the called and caller and assign individually
                            INTER_SAFEARRAY* prgsaCaller = pVariable.GetValue().GetValue().prgsa;

                            for (int iCount = 0; iCount < iArraysize; iCount++)
                            {
                                INTER_VARIANT VarTemp;
                                VarTemp.Clear();
                                prgsaCalled.GetElement(iMemsize * iCount, &VarTemp);
                                prgsaCaller.SetElement(iMemsize * iCount, &VarTemp);

                            }

                        }//end of Array var		

                    }//End of Non Return bar

                    //Check for the return type var, Exclude the return void Statement
                    if ((vectMethArgInfo[i].ePassedType == DD_METH_AGR_PASSED_BYREFERENCE) &&
                        (vectMethArgInfo[i].m_IsReturnVar) &&
                        (vectMethArgInfo[i].GetSubType() != RUL_SUBTYPE_NONE)
                        )
                    {

                        INTER_VARIANT vaTem = vectInterVar[i];
                        // removed WS:EPM 17jul07  pvar.varType  = vaTem.varType;
                        *pvar = vaTem;
                    }//End of return var			
                }
            }//End of Method RUL_DD_METHOD == pVariable.GetSubType()
            else
            {
                int* lTempArray = new int[i32Count];


                int i = 0;// WS - 9apr07 - 2005 checkin
                for (i = 0; i < i32Count; i++)// WS - 9apr07 - 2005 checkin
                {
                    var.Clear();
                    ((*pvecExpressions)[i]).Execute(this, pSymbolTable, var);
                    lTempArray[i] = (int)var;
                }
                // emerson april2013 uses pARRExp.GetToken() for its pointer
                // historically pVariable was used
                //#define ComplexExprHolder   pArrExp.GetToken() // /* historically pVariable 
                int lStrlen = strlen(ComplexExprHolder.GetLexeme());

                char* szTempLexeme = new char[lStrlen + 1];
                memset(szTempLexeme, 0, lStrlen + 1);

                //stevev 04jan07 - overrun if > 1 digit index
                // change char* szActualstring = new char[lStrlen+1];	
                // change memset(szActualstring,0,lStrlen + 1);
                //Anil: 050107  I would prefer declaring it as string with dynamic allocation:
                string szActualstring = "";
                //char* szActualstring = new char[lStrlen + MAX_INT_DIGITS + 1];	
                //memset(szActualstring,0,lStrlen + MAX_INT_DIGITS + 1);		

                //Here is slight Confusion ,
                //Funda: Complex DD Expression is Actually stored in m_pszLexeme which is got by 
                //	pVariable.GetLexeme().  Where as Actual token is Stored in m_pszComplexDDExpre 
                //	which i got by pVariable.GetDDItemName()	

                strcpy(szTempLexeme, ComplexExprHolder.GetLexeme());
                long int lCout = 0;
                int iNoOfBrackExpre = 0;

                for (long int iTemp = 0; iTemp < lStrlen; iTemp++)
                {
                    if (szTempLexeme[iTemp] == '[')
                    {

                        //szActualstring[lCout++] = szTempLexeme[iTemp];
                        szActualstring += szTempLexeme[iTemp];
                        iTemp++;
                        char szBuf[MAX_INT_DIGITS + 1];
                        _itoa(lTempArray[iNoOfBrackExpre], szBuf, 10);
                        //strcat(szActualstring,szBuf);
                        szActualstring += szBuf;
                        //strcat(szActualstring,"]");
                        szActualstring += "]";
                        lCout += strlen(szBuf) + 1;

                        iNoOfBrackExpre++;
                        long int iPos = i;
                        int iLeftBrackCount = 1;
                        long int iCount = 0;
                        while ((iLeftBrackCount != 0) && (iTemp < lStrlen))
                        {
                            if (szTempLexeme[iTemp] == '[')
                                iLeftBrackCount++;
                            if (szTempLexeme[iTemp] == ']')
                                iLeftBrackCount--;
                            iTemp++;
                        }
                        iTemp--;
                    }
                    else
                    {
                        szActualstring += szTempLexeme[iTemp];
                        //szActualstring[lCout++] = szTempLexeme[iTemp];
                    }
                }
                // added WS:EPM 17jul07
                if (lTempArray)
                {
                    delete[] lTempArray;
                    lTempArray = null;
                }


                //szActualstring[lCout] = '\0';
                if (szTempLexeme)
                {
                    delete[] szTempLexeme;
                    szTempLexeme = null;
                }


                //Here is slight Confusion ,
                //Funda: Comple DD Expression is Actually stored in m_pszLexeme which is got by 
                //	pVariable.GetLexeme().  Where as Actual token is Stored in m_pszComplexDDExpre 
                //	which i got by pVariable.GetDDItemName()

                if (m_IsLValue)
                {
                    m_IsLValue = false;
                    RETURNCODE iReturnValue = m_pMEE.ResolveNUpdateDDExp(szActualstring.c_str(), ComplexExprHolder.GetDDItemName(), pvar, AssignType);
                    if (iReturnValue == FAILURE)
                    {
                        return VISIT_ERROR;
                    }
                }
                else
                {
                    RETURNCODE iReturnValue = m_pMEE.ResolveDDExp(szActualstring.c_str(), ComplexExprHolder.GetDDItemName(), pvar);
                    if (iReturnValue == FAILURE)
                    {
                        return VISIT_ERROR;
                    }
                }
            */

            }//end of Else for Checking for method type
            return VISIT_NORMAL;
        }

        public override int visitAssignment(CAssignmentStatement pAssStmt, CSymbolTable pSymbolTable, 
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CToken pVariable = null;
            CExpression pExp = null;
            CExpression pArray = null;
            CExpression pComplexDDExp = null;//Added By Anil August 23 2005
            int iRetValue = VISIT_NORMAL;

            if (pAssStmt != null)
            {
                pVariable = pAssStmt.GetVariable();
                pExp = pAssStmt.GetExpression();
                pArray = pAssStmt.GetArrayExp();
                pComplexDDExp = pAssStmt.GetComplexDDExp();//Added By Anil August 23 2005
            }
            else
            {
                //error -- no assign statement
                return VISIT_ERROR;
            }

            INTER_VARIANT var = new INTER_VARIANT();

            /*Vibhor 110205: Start of Code*/
            if (pVariable != null)
            {
                byte[] by;
                switch (pVariable.GetSubType())
                {
                    case RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL:
                        {
                            //			var = (char)' ';  //WS:EPM 10aug07::Leave this as RUL_NULL because it could be a RUL_CHAR or a SafeArray of RUL_CHAR's
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL:
                        {
                            by = new byte[8];
                            by = BitConverter.GetBytes((Int64)0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_LONGLONG);
                        }
                        break;
                    // Walt EPM 08sep08 - start insert
                    case RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL:
                        {
                            by = new byte[2];
                            by = BitConverter.GetBytes((short)0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_SHORT);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL:
                        {
                            by = new byte[2];
                            by = BitConverter.GetBytes((ushort)0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_USHORT);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL:
                        {
                            by = new byte[4];
                            by = BitConverter.GetBytes((uint)0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_UINT);
                        }
                        break;
                    // Walt EPM 08sep08 - end insert
                    case RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL:
                    case RUL_TOKEN_SUBTYPE.RUL_LONG_DECL:
                        {
                            by = new byte[4];
                            by = BitConverter.GetBytes((int)0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_REAL_DECL:
                        {
                            by = new byte[4];
                            by = BitConverter.GetBytes((float)0.0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_FLOAT);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL:
                        {
                            by = new byte[8];
                            by = BitConverter.GetBytes((double)0.0);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_DOUBLE);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL:
                        {
                            by = new byte[1];
                            by = BitConverter.GetBytes(false);
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_BOOL);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_STRING_DECL:
                        {
                            //by = new byte[1];
                            by = Encoding.Default.GetBytes("");
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_DD_STRING);
                        }
                        break;
                    case RUL_TOKEN_SUBTYPE.RUL_ARRAY_DECL:
                        {
                            //by = new byte[1];
                            by = Encoding.Default.GetBytes("");
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_DD_STRING);
                        }
                        break;
                    //Added By Anil June 15 2005 --starts here
                    case RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL:
                        {
                            //by = new byte[1];
                            by = Encoding.Default.GetBytes("");
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_DD_STRING);
                        }
                        break;
                    //Added By Anil June 15 2005 --Ends here
                    case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_CHAR_DECL:
                        {
                            //var = (char *)"";  //WS:EPM 10aug07::Leave this as RUL_NULL because it could be a RUL_UNSIGNED_CHAR or a SafeArray of RUL_UNSIGNED_CHAR's
                            by = new byte[1];
                            by[0] = 0;
                            var.SetValue(by, 0, VARIANT_TYPE.RUL_UNSIGNED_CHAR);
                        }
                        break;

                    default:
                        break; //var.varType == RUL_NULL;
                }
            }
            /*Vibhor 110205: End of Code*/
            if (pExp != null)
            {
                iRetValue = pExp.Execute(this, pSymbolTable, ref var);
                if (iRetValue == VISIT_RETURN || iRetValue == VISIT_ERROR)// sded error checkin april2013
                {
                    return iRetValue;
                }
            }
            else
            {
                //error -- no expression in assingment statement
                return VISIT_ERROR;
            }

            if (pVariable != null)
            {
                int nIdx = -1;
                CVariable pStore = null;

                nIdx = pVariable.GetSymbolTableIndex();
                // commented out code removed 17jul07 
                pStore = pSymbolTable.GetAt(nIdx);
                if (pStore != null)
                {
                    switch (pAssStmt.GetAssignmentType())
                    {
                        case RUL_TOKEN_SUBTYPE.RUL_ASSIGN:
                            {
                                pStore.SetValue(var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN:
                            {
                                //pStore.GetValue() = pStore.GetValue() + var;
                                pStore.SetValue(pStore.GetValue() + var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN:
                            {
                                //pStore.GetValue() = pStore.GetValue() - var;
                                pStore.SetValue(pStore.GetValue() - var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() / var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() * var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() % var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() & var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() | var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() ^ var);
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() >> var.GetVarInt());
                                break;
                            }
                        case RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN:
                            {
                                pStore.SetValue(pStore.GetValue() << var.GetVarInt());
                                break;
                            }

                    }//end switch
                }//end of if statement
                 // else - null pStore - error?? logit???

                // Set the display Value of the variable if its a Global (DD Variable)
                //Anil August 26 2005 Comented By Anil as DD item is No more Variable
                /*	if(pVariable.m_bIsGlobal)
                    {
                        switch(pStore.GetSubType())
                        {
                            case	RUL_INTEGER_DECL:
                                    m_pMEE.SetVariableValue(pStore.GetLexeme(),(int)(pStore.GetValue()));
                                    break;
                            case	RUL_REAL_DECL:
                                    m_pMEE.SetVariableValue(pStore.GetLexeme(),(float)(pStore.GetValue()));
                                    break;
                            case	RUL_DOUBLE_DECL:
                                    m_pMEE.SetVariableValue(pStore.GetLexeme(),(double)(pStore.GetValue()));
                                    break;
                            case	RUL_STRING_DECL:
                                    m_pMEE.SetVariableValue(pStore.GetLexeme(),(char*)(pStore.GetValue()));
                                    break;
                            case	RUL_BOOLEAN_DECL:
                                    m_pMEE.SetVariableValue(pStore.GetLexeme(),(bool)(pStore.GetValue()));
                                    break;
                    }

                    }*/
            }//endif pVariable
            else if (pArray != null)
            {
                m_IsLValue = true;
                pArray.Execute(this, pSymbolTable, ref var, pAssStmt.GetAssignmentType()); //Anil August 26 2005 //for Fixing a[10] += 5;
            }
            //Added By Anil August 23 2005 --starts here 
            //This below is handle if it is of type	ComplexDD expression
            else if (pComplexDDExp != null)
            {
                m_IsLValue = true;
                int iTemp = pComplexDDExp.Execute(this, pSymbolTable, ref var, pAssStmt.GetAssignmentType());
                if (iTemp == VISIT_ERROR)
                    return VISIT_ERROR;
            }
            //Added By Anil August 23 2005 --Ends here
            else
            {
                //error -- no variable in assingment statement
                return VISIT_ERROR;
            }
            pvar = var;
            return VISIT_NORMAL;
        }

        public override int visitBreakStatement(CBreakStatement pItnStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_BREAK;
        }

        public override int visitReturnStatement(CReturnStatement pItnStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            //Added:Anil Octobet 5 2005 for handling Method Calling Method
            //this is a Double check that Return statement is only from the Called method..
            //ie  method which does not sit on the Menu
            if (false == m_bIsRoutine)
            {
                //If so return ; ie Do not execute the return statement
                return VISIT_RETURN;
            }
            CExpression pExp = null;
            pExp = pItnStmt.GetExpression();
            //if pExp, then it is return Void statement ..so no need to Execute this
            if (null == pExp)
            {
                return VISIT_RETURN;//This is the case of Void return statement ie return;
            }
            //Other wise we may need to Execute this
            else
            {
                int iSizeSymbTa = pSymbolTable.GetSymbTableSize();
                INTER_VARIANT var = new INTER_VARIANT();
                // removed WS:EPM 17jul07 var.varType = RUL_NULL;
                int iRetValue = VISIT_NORMAL;
                //bool bRetValFound = false;
                int iRetVarIndex = 0;
                //Now Loop through the Symbol table and Get the variable 
                //which we pushed as the Return variable in the starting
                //This variable will have m_bIsReturnToken as true..
                //None other var in symbol table should have this flag set

                for (int iCount = 0; iCount < iSizeSymbTa; iCount++)
                {
                    CVariable pCVariable = pSymbolTable.GetAt(iCount);
                    if (null != pCVariable)
                    {
                        if (pCVariable.Token.m_bIsReturnToken == true)
                        {
                            byte[] by;
                            //Once we get that Fill its variable type depending on the Return type declared in the method
                            iRetVarIndex = iCount;
                            //bRetValFound = true;
                            switch (pCVariable.Token.GetSubType())
                            {
                                case RUL_TOKEN_SUBTYPE.RUL_CHAR_DECL:
                                    {
                                        //var = (char)' ';
                                        by = new byte[1];
                                        by[0] = 0;
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_CHAR);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_LONG_LONG_DECL:
                                    {
                                        //var = (Int64)0;
                                        by = new byte[8];
                                        by = BitConverter.GetBytes((Int64)0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_LONGLONG);
                                    }
                                    break;
                                // Walt EPM 08sep08 - start insert
                                case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_SHORT_INTEGER_DECL:
                                    {
                                        //var = 0;
                                        by = new byte[2];
                                        by = BitConverter.GetBytes((ushort)0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_USHORT);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_SHORT_INTEGER_DECL:
                                    {
                                        //var = (short)0;
                                        by = new byte[2];
                                        by = BitConverter.GetBytes((short)0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_SHORT);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_UNSIGNED_INTEGER_DECL:
                                    {
                                        //var = 0;
                                        by = new byte[4];
                                        by = BitConverter.GetBytes((uint)0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_UINT);
                                    }
                                    break;
                                // Walt EPM 08sep08 - end insert
                                case RUL_TOKEN_SUBTYPE.RUL_INTEGER_DECL:
                                case RUL_TOKEN_SUBTYPE.RUL_LONG_DECL:
                                    {
                                        //var = (long)0;
                                        by = new byte[4];
                                        by = BitConverter.GetBytes((int)0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_REAL_DECL:
                                    {
                                        //var = (float)0.0;
                                        by = new byte[4];
                                        by = BitConverter.GetBytes((float)0.0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_FLOAT);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_DOUBLE_DECL:
                                    {
                                        //var = (double)0.0;//WS:EPM 10aug07
                                        by = new byte[8];
                                        by = BitConverter.GetBytes((double)0.0);
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_DOUBLE);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_BOOLEAN_DECL:
                                    {
                                        //var = (bool)false;
                                        by = new byte[1];
                                        by[0] = 0;
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_BOOL);
                                    }
                                    break;
                                case RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL:
                                    {
                                        //var = (char*)"";
                                        by = new byte[1];
                                        by[0] = 0;
                                        var.SetValue(by, 0, VARIANT_TYPE.RUL_DD_STRING);
                                    }
                                    break;

                                default:
                                    break; //var.varType == RUL_NULL;
                            } //End of switch
                            break;

                        }//End of pCVariable.m_bIsReturnToken == true

                    }//end of null != pCVariable 
                }//End of for loop	


                if (pExp != null)
                {
                    //Execute this return statement and get the value out of that and then store that in the Return variable
                    iRetValue = pExp.Execute(this, pSymbolTable, ref var);
                    CVariable pCVariable = pSymbolTable.GetAt(iRetVarIndex);
                    //pCVariable.GetValue() = var;
                    pCVariable.SetValue(var);
                    return VISIT_RETURN;
                }
                else
                {
                    //error -- no expression in assingment statement
                    return VISIT_ERROR;
                }
            }
            //return VISIT_RETURN;

        }
        
        public override int visitContinueStatement(CContinueStatement pItnStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_CONTINUE;
        }

        public override int visitCompoundStatement(CCompoundStatement pCompStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CStatementList pStmtLst = null;

            if (pCompStmt != null)
            {
                pStmtLst = pCompStmt.GetStatementList();
            }
            else
            {
                //error -- no compound statement.
                return VISIT_ERROR;
            }

            return visitStatementList(pStmtLst, pSymbolTable, ref pvar);
        }

        public override int visitIterationStatement(CIterationStatement pItnStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pExp = null;
            CAssignmentStatement pExpStmt = null;
            CStatement pStmt = null;

            if (pItnStmt != null)
            {
                if (pItnStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                {
                    pExp = pItnStmt.GetExpression();
                }
                else
                {
                    pExpStmt = pItnStmt.GetExpressionStatement();
                }
                pStmt = pItnStmt.GetStatement();
            }
            else
            {
                //error -- no Iteration statement.
                return VISIT_ERROR;
            }

            INTER_VARIANT var1 = new INTER_VARIANT(), var2 = new INTER_VARIANT();
            if (pExp != null || pExpStmt != null)
            {
                ulong ulongLoopCount = 0;
                while (true)
                {
                    ulongLoopCount++;
                    if (ulongLoopCount >= MAX_LOOPS)
                    {
                        break;
                    }

                    int iRetValue;

                    if (pItnStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                    {
                        iRetValue = pExp.Execute(this, pSymbolTable, ref var1);
                    }
                    else
                    {
                        iRetValue = pExpStmt.Execute(this, pSymbolTable, ref var1);
                    }

                    if (iRetValue == VISIT_RETURN)
                    {
                        return iRetValue;
                    }

                    if (!((iRetValue != 0) && ((object)var1 != null) && pStmt != null))
                    {
                        break;
                    }

                    //var1.Clear();
                    int iVisitReturnType = pStmt.Execute(this, pSymbolTable, ref var2);
                    switch (iVisitReturnType)
                    {
                        case VISIT_BREAK:
                            return VISIT_NORMAL;
                        case VISIT_CONTINUE:
                            continue;
                        case VISIT_RETURN:
                            return VISIT_RETURN;
                    }
                    //var2.Clear();
                }
            }
            else
            {
                //error -- no expression in while statement
                return VISIT_ERROR;
            }
            return VISIT_NORMAL;
        }

        public override int visitIterationStatement(CIterationDoWhileStatement pItnStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN) //Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pExp = null;
            CAssignmentStatement pExpStmt = null;
            CStatement pStmt = null;

            if (pItnStmt != null)
            {
                if (pItnStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                {
                    pExp = pItnStmt.GetExpression();
                }
                else
                {
                    pExpStmt = pItnStmt.GetExpressionStatement();
                }

                pStmt = pItnStmt.GetStatement();
            }
            else
            {
                //error -- no Iteration statement.
                return VISIT_ERROR;
            }

            INTER_VARIANT var1 = new INTER_VARIANT(), var2 = new INTER_VARIANT();
            if (pExp != null || pExpStmt != null)
            {
                ulong ulongLoopCount = 0;
                do
                {
                    ulongLoopCount++;
                    if (ulongLoopCount >= MAX_LOOPS)
                    {
                        break;
                    }

                    //var1.Clear();
                    int iVisitReturnType = pStmt.Execute(this, pSymbolTable, ref var2);
                    switch (iVisitReturnType)
                    {
                        case VISIT_BREAK:
                            return VISIT_NORMAL;
                        case VISIT_CONTINUE:
                            continue;
                        case VISIT_RETURN:
                            return VISIT_RETURN;
                    }
                    //var2.Clear();

                    int iRetValue;

                    if (pItnStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                    {
                        iRetValue = pExp.Execute(this, pSymbolTable, ref var1);
                    }
                    else
                    {
                        iRetValue = pExpStmt.Execute(this, pSymbolTable, ref var1);
                    }

                    if (iRetValue == VISIT_RETURN)
                    {
                        return iRetValue;
                    }

                    if (iRetValue != 0 && (object)var1 != null && pStmt != null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
            }
            else
            {
                //error -- no expression in while statement
                return VISIT_ERROR;
            }
            return VISIT_NORMAL;
        }

        public override int visitIterationStatement(CIterationForStatement pItnStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pExp = null;
            CAssignmentStatement pExpStmt = null;
            CStatement pStmt = null;

            if (pItnStmt != null)
            {
                if (pItnStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                {
                    pExp = pItnStmt.GetExpression();
                }
                else
                {
                    pExpStmt = pItnStmt.GetExpressionStatement();
                }
                pStmt = pItnStmt.GetStatement();
            }
            else
            {
                //error -- no Iteration statement.
                return VISIT_ERROR;
            }

            INTER_VARIANT var1 = new INTER_VARIANT(), var2 = new INTER_VARIANT();
            if (pExp != null || pExpStmt != null)
            {
                ulong ulongLoopCount = 0;
                while (true)
                {
                    ulongLoopCount++;
                    if (ulongLoopCount >= MAX_LOOPS)
                    {
                        break;
                    }

                    int iRetValue;

                    if (pItnStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                    {
                        iRetValue = pExp.Execute(this, pSymbolTable, ref var1);
                    }
                    else
                    {
                        iRetValue = pExpStmt.Execute(this, pSymbolTable, ref var1);
                    }

                    if (iRetValue == VISIT_RETURN)
                    {
                        return iRetValue;
                    }

                    if (!(iRetValue != 0 && (object)var1 != null && pStmt != null))//////var1.val.lvalue == 0???
                    {
                        break;
                    }

                    //var1.Clear();
                    int iVisitReturnType = pStmt.Execute(this, pSymbolTable, ref var2);
                    switch (iVisitReturnType)
                    {
                        case VISIT_BREAK:
                            return VISIT_NORMAL;
                        case VISIT_CONTINUE:
                            continue;
                        case VISIT_RETURN:
                            return VISIT_RETURN;
                    }
                    //var2.Clear();
                }
            }
            else
            {
                //error -- no expression in while statement
                return VISIT_ERROR;
            }
            return VISIT_NORMAL;
        }

        public override int visitSelectionStatement(CSelectionStatement pSelStmt, CSymbolTable pSymbolTable, 
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pExp = null;
            CStatement pStmt = null;
            CELSEStatement pElse = null;

            if (pSelStmt != null)
            {
                pExp = pSelStmt.GetExpression();
                pStmt = pSelStmt.GetStatement();
                pElse = pSelStmt.GetELSEStatement();
            }
            else
            {
                //error -- no selection statement..
                return VISIT_ERROR;
            }

            INTER_VARIANT var1 = new INTER_VARIANT();
            INTER_VARIANT var2 = new INTER_VARIANT();
            if (pExp != null)
            {
                int iRetValue;

                // Gowtham 260306: Start of Code Modifications
                // Split the if condition to see if the return value is VISIT_RETURN.
                iRetValue = pExp.Execute(this, pSymbolTable, ref var1);
                if (iRetValue == VISIT_RETURN)
                {
                    return iRetValue;
                }
                //if (iRetValue && (bool)var1 && pStmt)
                if (iRetValue != 0 && var1.val.bValue && pStmt != null)
                {
                    // Gowtham 260306: End of Code Modifications
                    if (iRetValue == VISIT_RETURN)
                    {
                        return iRetValue;
                    }

                    //var1.Clear();
                    int iVisitReturnType = pStmt.Execute(this, pSymbolTable, ref var2);
                    switch (iVisitReturnType)
                    {
                        case VISIT_BREAK:
                            return VISIT_BREAK;
                        case VISIT_CONTINUE:
                            return VISIT_CONTINUE;
                        case VISIT_RETURN:
                            return VISIT_RETURN;
                    }
                    //var2.Clear();
                }
                else if (false == var1.val.bValue)
                {
                    if (pElse != null)
                    {
                        int iVisitReturnType = pElse.Execute(this, pSymbolTable, ref var2);
                        switch (iVisitReturnType)
                        {
                            case VISIT_BREAK:
                                return VISIT_BREAK;
                            case VISIT_CONTINUE:
                                return VISIT_CONTINUE;
                            case VISIT_RETURN:
                                return VISIT_RETURN;
                        }
                    }
                    //var2.Clear();
                }
                else
                {
                    //either there is no statement or execute failed.
                    return VISIT_ERROR;
                }
            }
            else
            {
                //error -- no expression in if statement...
            }
            return VISIT_NORMAL;
        }

        public override int visitSwitchStatement(CSwitchStatement pSelStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pExp = null;
            CAssignmentStatement pExpStmt = null;
            CStatement pStmt = null;
            CCASEStatement pCaseStatement = null;
            int iNumberOfCasesPresent = 0;
            bool bIsDefaultPresent = false;

            if (pSelStmt != null)
            {
                if (pSelStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                {
                    pExp = pSelStmt.GetExpression();
                }
                else
                {
                    pExpStmt = pSelStmt.GetExpressionStatement();
                }
                pStmt = pSelStmt.GetStatement();
                iNumberOfCasesPresent = pSelStmt.GetNumberOfCaseStatements();
                bIsDefaultPresent = pSelStmt.IsDefaultPresent();
            }
            else
            {
                //error -- no selection statement..
                return VISIT_ERROR;
            }

            if ((iNumberOfCasesPresent <= 0) && (bIsDefaultPresent == false))
            {
                return VISIT_NORMAL;

            }

            INTER_VARIANT var1 = new INTER_VARIANT(), var2 = new INTER_VARIANT(), var3 = new INTER_VARIANT();
            if (pExp != null || pExpStmt != null)
            {
                int iRetValue;
                if (pSelStmt.GetExpressionNodeType() == GRAMMAR_NODE_TYPE.NODE_TYPE_EXPRESSION)
                {
                    iRetValue = pExp.Execute(this, pSymbolTable, ref var1);
                }
                else
                {
                    iRetValue = pExpStmt.Execute(this, pSymbolTable, ref var1);
                }

                if (iRetValue == VISIT_RETURN)
                {
                    return iRetValue;
                }

                bool bMatchFound = false;

                for (int iLoopVar = 0; iLoopVar < iNumberOfCasesPresent; iLoopVar++)
                {
                    pCaseStatement = pSelStmt.GetCaseStatement(iLoopVar);

                    CExpression pCaseExp = pCaseStatement.GetExpression();

                    pCaseExp.Execute(this, pSymbolTable, ref var2);

                    if ((var1.val.nValue == var2.val.nValue) || bMatchFound)
                    {
                        bMatchFound = true;
                        int iVisitReturnType = pCaseStatement.Execute(this, pSymbolTable, ref var3);
                        switch (iVisitReturnType)
                        {
                            case VISIT_BREAK:
                                return VISIT_NORMAL;
                            case VISIT_CONTINUE:
                                return VISIT_CONTINUE;
                            case VISIT_RETURN:
                                return VISIT_RETURN;
                        }
                    }
                    //var2.Clear();
                }
                if (bIsDefaultPresent)
                {
                    pCaseStatement = pSelStmt.GetDefaultStatement();

                    int iVisitReturnType = pCaseStatement.Execute(this, pSymbolTable, ref var3);
                    switch (iVisitReturnType)
                    {
                        case VISIT_BREAK:
                            return VISIT_NORMAL;
                        case VISIT_CONTINUE:
                            return VISIT_CONTINUE;
                        case VISIT_RETURN:
                            return VISIT_RETURN;
                    }
                    //var2.Clear();
                }
                return VISIT_NORMAL;
            }
            else
            {
                //error -- no expression in if statement...
            }
            return VISIT_NORMAL;
        }

        public override int visitELSEStatement(CELSEStatement pELSE, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CStatement pStmt = null;
            INTER_VARIANT var = new INTER_VARIANT();
            if (pELSE != null)
            {
                pStmt = pELSE.GetStatement();
            }
            else
            {
                //error -- no else statement.
                return VISIT_ERROR;
            }
            if (pStmt != null)
            {
                return pStmt.Execute(this, pSymbolTable, ref var);
            }
            else
            {
                //error -- no Statement in Else
                return VISIT_ERROR;
            }
            //return VISIT_NORMAL;
        }

        public override int visitCASEStatement(CCASEStatement pCase, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CStatementList pStmtList = null;
            INTER_VARIANT var = new INTER_VARIANT();
            if (pCase != null)
            {
                pStmtList = pCase.GetStatement();
            }
            else
            {
                //error -- no else statement.
                return CGrammarNode.VISIT_ERROR;
            }

            if (pStmtList != null)
            {
                return pStmtList.Execute(this, pSymbolTable, ref var);
            }
            else
            {
                //error -- no Statement in Else
                return CGrammarNode.VISIT_ERROR;
            }
            //return VISIT_NORMAL;
        }

        public override int visitCompoundExpression(CCompoundExpression pCompStmt, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pFirstExp = null;
            CExpression pSecondExp = null;
            RUL_TOKEN_SUBTYPE Operator = RUL_TOKEN_SUBTYPE.RUL_SUBTYPE_NONE;
            INTER_VARIANT var1 = new INTER_VARIANT();
            INTER_VARIANT var2 = new INTER_VARIANT();

            if (pCompStmt != null)
            {
                pFirstExp = pCompStmt.GetFirstExpression();
                pSecondExp = pCompStmt.GetSecondExpression();
                Operator = pCompStmt.GetOperator();
            }
            else
            {
                //error
                return VISIT_ERROR;
            }

            if (pFirstExp != null)
            {
                int iVisitReturnType = pFirstExp.Execute(this, pSymbolTable, ref var1);
                switch (iVisitReturnType)
                {
                    case VISIT_BREAK:
                        return VISIT_BREAK;
                    case VISIT_CONTINUE:
                        return VISIT_NORMAL;
                    case VISIT_RETURN:
                        return VISIT_RETURN;
                }
            }
            else
            {
                //error -- no first operand
                return VISIT_ERROR;
            }
            // emerson checkin april2013
            // evaluate OR and AND for the first expression
            // For OR, if the first expression is true, we are done and return true
            // For AND, if the first expression is false, we are done and return false
            if (Operator == RUL_TOKEN_SUBTYPE.RUL_LOGIC_AND)
            {
                if ((bool)var1.val.bValue == false)
                {
                    //pvar.Clear();
                    pvar.val.bValue = false;
                    return VISIT_NORMAL;
                }
            }
            else if (Operator == RUL_TOKEN_SUBTYPE.RUL_LOGIC_OR)
            {
                if ((bool)var1.val.bValue == true)
                {
                    //pvar.Clear();
                    pvar.val.bValue = true;
                    return VISIT_NORMAL;
                }
            }

            if (pSecondExp != null)
            {
                int iVisitReturnType = pSecondExp.Execute(this, pSymbolTable, ref var2);
                switch (iVisitReturnType)
                {
                    case VISIT_BREAK:
                        return VISIT_BREAK;
                    case VISIT_CONTINUE:
                        return VISIT_NORMAL;
                    case VISIT_RETURN:
                        return VISIT_RETURN;
                }
            }

            //pvar.Clear();
            if (m_fnTable[(int)Operator] != null)
            {
                if ((Operator == RUL_TOKEN_SUBTYPE.RUL_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN)
                    || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN)
                )
                {
                    m_fnTable[(int)Operator](ref var2, ref var1, ref pvar);
                }
                else
                {
                    m_fnTable[(int)Operator](ref var1, ref var2, ref pvar);
                    //(this.* m_fnTable[Operator])(var1, var2, *pvar);
                }
            }
            if ((Operator == RUL_TOKEN_SUBTYPE.RUL_PLUS_PLUS)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_MINUS_MINUS)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_PRE_PLUS_PLUS)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_PRE_MINUS_MINUS)
                )
            {
                if (pFirstExp != null)
                {
                    CToken pExpToken = ((CPrimaryExpression)pFirstExp).GetToken();
                    CVariable pStore = null; //Anil Moved above
                    if (pExpToken != null && pExpToken.IsVariable())
                    {
                        int nIdx = pExpToken.GetSymbolTableIndex();
                        pStore = pSymbolTable.GetAt(nIdx);

                        if (pStore != null)
                            pStore.SetValue(var1);
                    }
                    //Added By Anil August 25 2005 --starts here
                    //Check whether it is of type DD item
                    if (pExpToken != null && pExpToken.IsDDItem())
                    {
                        int nIdx = pExpToken.GetSymbolTableIndex();
                        pStore = m_pMEE.m_GlobalSymTable.GetAt(nIdx);
                        if (pStore != null)
                            pStore.SetValue(var1);
                        int iReturnValue = m_pMEE.ResolveNUpdateDDExp(pExpToken.GetLexeme(), pExpToken.GetDDItemName(), ref var1);
                        if (iReturnValue == Common.FAILURE)
                        {
                            return VISIT_ERROR;
                        }
                    }
                    //Added By Anil August 25 2005 --Ends here
                }
            }

            if (
                (Operator == RUL_TOKEN_SUBTYPE.RUL_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN)
                || (Operator == RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN)
                )
            {
                if (pFirstExp != null)
                {
                    CToken pExpToken = ((CPrimaryExpression)pFirstExp).GetToken();
                    CVariable pStore = null;//Anil Moved up
                    if (pExpToken != null && pExpToken.IsVariable())
                    {
                        int nIdx = pExpToken.GetSymbolTableIndex();
                        pStore = pSymbolTable.GetAt(nIdx);
                        if (pStore != null)
                            pStore.SetValue(pvar);
                    }
                    //Added By Anil August 25 2005 --starts here
                    //Check whether it is of type DD item
                    if (pExpToken != null && pExpToken.IsDDItem())
                    {
                        int nIdx = pExpToken.GetSymbolTableIndex();
                        pStore = m_pMEE.m_GlobalSymTable.GetAt(nIdx);
                        if (pStore != null)
                            pStore.SetValue(var1);
                        int iReturnValue = m_pMEE.ResolveNUpdateDDExp(pExpToken.GetLexeme(), pExpToken.GetDDItemName(), ref pvar);
                        if (iReturnValue == Common.FAILURE)
                        {
                            return VISIT_ERROR;
                        }
                    }
                    //Added By Anil August 25 2005 --Ends here
                }
            }
            return VISIT_NORMAL;
        }

        public override int visitPrimaryExpression(CPrimaryExpression pPrimStmt, CSymbolTable pSymbolTable, 
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CToken pToken = null;
            if (pPrimStmt != null)
            {
                pToken = pPrimStmt.GetToken();
            }
            else
            {
                //error -- no primary statement...
                return VISIT_ERROR;
            }
            if (pToken != null && pToken.IsVariable())
            {
                int nIdx = pToken.GetSymbolTableIndex();
                CVariable pStore = null;
                if (nIdx >= 0)
                {
                    pStore = pSymbolTable.GetAt(nIdx);

                    if (pStore != null)
                        pvar = pStore.GetValue();
                }

            }
            else if (pToken.IsNumeric())
            {
                INTER_VARIANT temp = new INTER_VARIANT(true, pToken.GetLexeme());
                pvar = temp;
            }
            else if (pToken.IsConstant())
            {
                //Got to fill it this up...
                if (RUL_TOKEN_SUBTYPE.RUL_STRING_CONSTANT == pToken.GetSubType())
                {
                    pvar.SetValue(pToken.GetLexeme());

                }
                else if (RUL_TOKEN_SUBTYPE.RUL_CHAR_CONSTANT == pToken.GetSubType())
                {
                    string pchChar = pToken.GetLexeme();
                    pvar.val.cValue = (byte)pchChar[0];//////
                }
            }
            else
            {
                //error -- no token in a primary statement.
                return VISIT_ERROR;
            }
            return VISIT_NORMAL;
        }

        public override int visitProgram(CProgram pProgram, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CDeclarations pDecl = null;
            CStatementList pStmtList = null;

            if (pProgram != null)
            {
                pDecl = pProgram.GetDeclarations();
                pStmtList = pProgram.GetStatementList();
            }
            else
            {
                //error -- no program to execute.
                return VISIT_ERROR;
            }
            if (pDecl != null)
            {
                INTER_VARIANT iv = null;
                pDecl.Execute(this, pSymbolTable, ref iv);
            }
            else
            {
                //		return VISIT_ERROR;
            }
            if (pStmtList != null)
            {
                INTER_VARIANT iv = null;
                return pStmtList.Execute(this, pSymbolTable, ref iv);
            }
            else
            {
                //error -- no statements in the program...
                return VISIT_ERROR;
            }

            //return 1;
        }

        public override int visitExpression(CExpression pExpression, CSymbolTable pSymbolTable, 
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            CExpression pExp = null;

            if (pExpression != null)
            {
                pExp = pExpression.GetExpression();
            }
            else
            {
                return VISIT_ERROR;
            }

            if (null == pExp)
            {
                return VISIT_ERROR;
            }
            INTER_VARIANT var1 = new INTER_VARIANT();

            return pExp.Execute(this, pSymbolTable, ref var1);//////
        }

        public override int visitStatement(CStatement pStatement, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_ERROR;
        }

        public override int visitStatementList(CStatementList pStmtList, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            List<CStatement> pStmtCol = null;
            CStatement pStmt = null;

            if (pStmtList != null)
            {
                pStmtCol = pStmtList.GetStmtList();
                int nSize = pStmtCol.Count;
                for (int i = 0; i < nSize; i++)
                {
                    pStmt = pStmtCol[i];
                    pSymbolTable.m_nCurrentScope = pStmt.GetScopeIndex();         //SCR26200 Felix
                    INTER_VARIANT iv = null;
                    int iVisitReturnType = pStmt.Execute(this, pSymbolTable, ref iv);//stevev 19nov09 -force it null in release mode too
                    switch (iVisitReturnType)
                    {
                        case VISIT_BREAK:
                            return VISIT_BREAK;
                        case VISIT_CONTINUE:
                            return VISIT_CONTINUE;
                        case VISIT_RETURN:
                            return VISIT_RETURN;
                    }
                    //Anil 180107 if VISIT_SCOPE_VAR == iVisitReturnType means  we are executing 
                    //the Statement list which has declaration
                    //This is a bug fix to get rid of the variable which is declared within the scope
                    //the below DD Method code was not handled
                    /*MethodDefination 
					{
						int x;
						x = 0;
						if(int x == 0)
						{
							int y; 
							ACKNOWLEDGE("This was not executing");
						}


					}*/
                    if (  VISIT_SCOPE_VAR == iVisitReturnType)
                    {
                        //Anil 240107 Fool the interpreter that you have executed this statement( which is declaration)
                        continue;
                    }
                    if (iVisitReturnType == VISIT_ERROR)//Anil Added September 12 2005
                        return VISIT_ERROR;
                }
            }
            else
            {
                //error -- no statements in the list.
                return VISIT_ERROR;
            }
            return VISIT_NORMAL;
        }

        public override int visitDeclarations(CDeclarations pDeclarations, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {

            int i32Count = pSymbolTable.GetCount();
            CVariable pVariable = null;

            for (int i = 0; i < i32Count; i++)
            {
                pVariable = pSymbolTable.GetAt(i);
                INTER_VARIANT var = pVariable.GetValue();

                //Bug Fix for PAR 570. Initilize DD_STRING to null character during the declaration execution
                //Below conditio gives it is DD_STRING
                if (
                    (RUL_TOKEN_SUBTYPE.RUL_DD_STRING_DECL == pVariable.Token.GetSubType()) &&
                    (RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE == pVariable.Token.GetType()) &&
                    // feb08	(RUL_CHARPTR == var.GetVarType())&&
                    (VARIANT_TYPE.RUL_WIDECHARPTR == var.GetVarType()) &&
                    !(pVariable.Token.m_bIsRoutineToken)
                  )
                {
                    string tmpStr = "";
                    var.SetValue(tmpStr, VARIANT_TYPE.RUL_DD_STRING);

                }

                if (VARIANT_TYPE.RUL_SAFEARRAY == var.GetVarType() && !(pVariable.Token.m_bIsRoutineToken))
                {
                    (var.GetValue().prgsa).Allocate();
                }
            }
            return VISIT_NORMAL;
        }

        public override int visitRuleService(CRuleServiceStatement pStatement, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_ERROR;
        }

        public override int visitOMExpression(COMServiceExpression pExpression, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_NORMAL;
        }

        public override int visitFunctionExpression(FunctionExpression pFuncExp, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            if (null == pFuncExp)
            {
                return VISIT_ERROR;
            }

            INTER_VARIANT[] pVarParams = new INTER_VARIANT[MAX_NUMBER_OF_FUNCTION_PARAMETERS];

            for (int iLoopVar = 0; iLoopVar < pFuncExp.GetParameterCount(); iLoopVar++)
            {
                if(pFuncExp.GetFunctionName() == "_get_dictionary_string")
                {
                    ;
                }
                if (pFuncExp.GetParameterType(iLoopVar) != RUL_TOKEN_TYPE.RUL_STR_CONSTANT)
                {
                    CExpression pExp = pFuncExp.GetExpParameter(iLoopVar);
                    if (null == pExp)
                    {// message added emerson checkin april2013
                        //char szMessage[1024] = { 0 };
                        //sprintf(szMessage, "***FAILURE*** Missing Argument %d of %s\n", iLoopVar + 1, pFuncExp.GetFunctionName());
                        //LOGIT(COUT_LOG, szMessage);

                        return VISIT_ERROR;
                    }
                    int iRetValue;

                    iRetValue = pExp.Execute(this, pSymbolTable, ref pVarParams[iLoopVar]);
                    if (iRetValue == VISIT_RETURN)
                    {
                        return iRetValue;
                    }
                }
                else
                {
                    CToken pToken = pFuncExp.GetConstantParameter(iLoopVar);
                    //pVarParams[iLoopVar].SetValue((void *)pToken.GetLexeme(), RUL_CHARPTR);
                    if (pToken != null)  //WaltS - 04may07 this check for null pointer 
                    {
                        pVarParams[iLoopVar].SetValue(pToken.GetLexeme());
                    }
                    else   //WaltS - 04may07
                    {
                        // expressiona as parameters added in emerson checkin april2013
                        CExpression pExp = pFuncExp.GetExpParameter(iLoopVar);
                        if (null == pExp)
                        {
                            //char szMessage[1024] = { 0 };
                            //sprintf(szMessage, "***FAILURE*** Missing Argument %d of %s\n", iLoopVar + 1, pFuncExp.GetFunctionName());
                            //LOGIT(COUT_LOG, szMessage);

                            return VISIT_ERROR;
                        }
                        int iRetValue;

                        iRetValue = pExp.Execute(this, pSymbolTable, ref pVarParams[iLoopVar]);
                        if (iRetValue == VISIT_RETURN)
                        {
                            return iRetValue;
                        }
                    }
                }
            }

            int iReturnStatus = 0;
            bool bRetValue = m_pBuiltInLib.InvokeFunction(pFuncExp.GetFunctionName(), pFuncExp.GetParameterCount(),
                pVarParams, ref pvar, ref iReturnStatus, pFuncExp);          // added WS:EPM 17jul07
            if (bRetValue)
            {
                if (iReturnStatus == 1)
                {
                    return VISIT_RETURN;
                }
                else
                {
                    int idex = 0;
                    CVariable p_bi_rc = pSymbolTable.Find("_bi_rc", ref idex);
                    if (p_bi_rc != null)
                    {
                        //p_bi_rc.GetValue() = pvar;
                        p_bi_rc.SetValue(pvar);
                    }

                    return VISIT_NORMAL;
                }
            }
            else
            {
                return VISIT_ERROR;
            }
        }

        public override int visitIFExpression(IFExpression pIfExp, CSymbolTable pSymbolTable, ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            if (pIfExp == null)
            {
                return VISIT_ERROR;
            }
            CExpression pExp = null;
            CExpression pTrueExp = null;
            CExpression pFalseExp = null;

            pIfExp.GetExpressions(ref pExp, ref pTrueExp, ref pFalseExp);

            if (pExp != null)
            {
                INTER_VARIANT Var = new INTER_VARIANT();
                int iRetValue;

                iRetValue = pExp.Execute(this, pSymbolTable, ref Var);
                if (iRetValue == VISIT_RETURN)
                {
                    return iRetValue;
                }

                //if ((bool)Var)
                if (Var.val.bValue)
                {
                    if (pTrueExp != null)
                    {
                        iRetValue = pTrueExp.Execute(this, pSymbolTable, ref Var);
                        if (iRetValue == VISIT_RETURN)
                        {
                            return iRetValue;
                        }
                        pvar = Var;
                    }
                    else
                    {
                        return VISIT_ERROR;
                    }
                }
                else
                {
                    if (pFalseExp != null)
                    {
                        iRetValue = pFalseExp.Execute(this, pSymbolTable, ref Var);
                        if (iRetValue == VISIT_RETURN)
                        {
                            return iRetValue;
                        }
                        pvar = Var;
                    }
                    else
                    {
                        return VISIT_ERROR;
                    }
                }
                return VISIT_NORMAL;
            }
            else
            {
                return VISIT_ERROR;
            }
        }

        public void SetIsRoutineFlag(bool bisRoutine)//Anil Octobet 5 2005 for handling Method Calling Method
        {
            m_bIsRoutine = bisRoutine;
            return;
        }

        public bool GetIsRoutineFlag()//Anil Octobet 5 2005 for handling Method Calling Method
        {
            return m_bIsRoutine;
        }

        public int uplusplus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            INTER_VARIANT temp = new INTER_VARIANT();
            int i = 1;
            byte[] by = BitConverter.GetBytes(i);
            temp.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
            v3 = v1;
            v1 = v1 + temp;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int uminusminus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            INTER_VARIANT temp = new INTER_VARIANT();
            int i = 1;
            byte[] by = BitConverter.GetBytes(i);
            temp.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
            v3 = v1;
            v1 = v1 - temp;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int upreplusplus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            INTER_VARIANT temp = new INTER_VARIANT();
            int i = 1;
            byte[] by = BitConverter.GetBytes(i);
            temp.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
            v3 = v1 + temp;
            v1 = v1 + temp;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int upreminusminus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            INTER_VARIANT temp = new INTER_VARIANT();
            int i = 1;
            byte[] by = BitConverter.GetBytes(i);
            temp.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
            v3 = v1 - temp;
            v1 = v1 - temp;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int uplus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            /*
            byte[] by;
            switch (v1.GetVarType())
            {
                case VARIANT_TYPE.RUL_CHAR:
                    by = new byte[1]; 
                    v1.GetValue(ref by, 0, VARIANT_TYPE.RUL_CHAR);
                    //by[0] = +by[0];
                    v3.SetValue(by, 0, VARIANT_TYPE.RUL_CHAR);
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR:
                    {
                        by = new byte[1];
                        v1.GetValue(ref by, 0, VARIANT_TYPE.RUL_UNSIGNED_CHAR);
                        by[0] += by[0];
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_UNSIGNED_CHAR);
                    }
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    {
                        by = new byte[2];
                        ushort i;
                        i = v1.val.usValue;
                        i = +(i);
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_SHORT);
                    }
                    break;
                case VARIANT_TYPE.RUL_USHORT:
                    {
                        by = new byte[2];
                        v1.GetValue(ref by, 0, VARIANT_TYPE.RUL_USHORT);
                        i = +(i);
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_USHORT);
                    }
                    break;
                case VARIANT_TYPE.RUL_INT:
                    {
                        int i;
                        v1.GetValue(by, 0, VARIANT_TYPE.RUL_INT);
                        i = +(i);
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_INT);
                    }
                    break;
                case VARIANT_TYPE.RUL_UINT:
                    {
                        ulong i = 0;
                        v1.GetValue(by, 0, VARIANT_TYPE.RUL_UINT);
                        i = +(i);
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_UINT);
                    }
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    {
                        __int64 i = 0;
                        v1.GetValue(by, 0, VARIANT_TYPE.RUL_LONGLONG);
                        i = +(i);
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_LONGLONG);
                    }
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG:
                    {
                        unsigned __int64 i = 0;
                        v1.GetValue(by, 0, VARIANT_TYPE.RUL_ULONGLONG);
                        i = +(i);
                        v3.SetValue(by, 0, VARIANT_TYPE.RUL_ULONGLONG);
                    }
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    float f;
                    v1.GetValue(by, 0, , VARIANT_TYPE.RUL_FLOAT);
                    f = +(f);
                    v3.SetValue(by, 0, , VARIANT_TYPE.RUL_FLOAT);
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    double d;
                    v1.GetValue(by, 0,  VARIANT_TYPE.RUL_DOUBLE);
                    d = +(d);
                    v3.SetValue(by, 0,  VARIANT_TYPE.RUL_DOUBLE);
                    break;
            }
            */
            v3 = v1;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int uminus(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            switch (v1.GetVarType())
            {
                case VARIANT_TYPE.RUL_CHAR:
                    v3.val.cValue = (byte)-v1.val.cValue;
                    break;
                case VARIANT_TYPE.RUL_UNSIGNED_CHAR://negative of an unsigned... we need to promote this to an integer
                    {
                        v3.val.nValue = -v1.val.ucValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_SHORT:
                    {
                        v3.val.sValue = (short)-v1.val.sValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_USHORT://negative of an unsigned... we need to promote this to an integer
                    {
                        v3.val.nValue = -v1.val.usValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_INT:
                    {
                        v3.val.nValue = -v1.val.nValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_UINT://negative of an unsigned long... we need to promote this to a long long
                    {
                        v3.val.lValue = -v1.val.unValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_LONGLONG:
                    {
                        v3.val.lValue = -v1.val.lValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_ULONGLONG://negative of an unsigned long long... we need to promote this to a double
                    {
                        /*
                        unsigned __int64 i = 0;
                        v1.GetValue(by, RUL_ULONGLONG);
                        double j = -(double)(__int64)(i);
                        v3.SetValue((void*)&j, RUL_DOUBLE);
                        */
                        v3.val.dValue = -v1.val.lValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_FLOAT:
                    {
                        v3.val.fValue = -v1.val.fValue;
                    }
                    break;
                case VARIANT_TYPE.RUL_DOUBLE:
                    {
                        v3.val.dValue = -v1.val.dValue;
                    }
                    break;
            }
            //v3 = -(v1);
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitand(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 & v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitor(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 | v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitxor(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 ^ v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitnot(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = ~v1;
            return CGrammarNode.VISIT_NORMAL;
        }

        int bitrshift(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 >> v2.GetVarInt();
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitlshift(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 << v2.GetVarInt();
            return CGrammarNode.VISIT_NORMAL;
        }

        int add(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 + v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int sub(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 - v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        int mul(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 * v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int div(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 / v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        int mod(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 % v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int exp(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 ^ v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int neq(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = (v1 != v2);
            return CGrammarNode.VISIT_NORMAL;
        }

        public int lt(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = (v1 < v2);
            return CGrammarNode.VISIT_NORMAL;
        }

        int gt(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = (v1 > v2);
            return CGrammarNode.VISIT_NORMAL;
        }

        public int eq(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 == v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int ge(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = (v1 >= v2);
            return CGrammarNode.VISIT_NORMAL;
        }

        int le(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 <= v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int land(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            //////v3 = v1 && v2;
            v3 = v1 & v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int lor(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            //v3 = v1 || v2;
            v3 = v1 | v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int lnot(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = !v1;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int rparen(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1;
            return CGrammarNode.VISIT_NORMAL;
        }

        int assign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int plusassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 + v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int minusassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 - v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        int divassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 / v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int modassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 % v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        int mulassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 * v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitandassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 & v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitorassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 | v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int bitxorassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 ^ v2;
            return CGrammarNode.VISIT_NORMAL;
        }

        public int rshiftassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 >> v2.GetVarInt();
            return CGrammarNode.VISIT_NORMAL;
        }

        public int lshiftassign(ref INTER_VARIANT v1, ref INTER_VARIANT v2, ref INTER_VARIANT v3)
        {
            v3 = v1 << v2.GetVarInt();
            return CGrammarNode.VISIT_NORMAL;
        }

    }


}
