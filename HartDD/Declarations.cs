using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class CDeclaration : CGrammarNode
    {
        public CDeclaration()
        {

        }
        //	Identify self
        public override void Identify(ref string szData)
        {
            ;
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return VISIT_SCOPE_VAR;
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CToken pToken = new CToken();
            CToken pSymToken = null;

            //try
            {
                if ((CLexicalAnalyzer.LEX_FAIL == plexAnal.GetNextToken(ref pToken, ref pSymbolTable)) || pToken == null)
                {
                    //throw (C_UM_ERROR_INTERNALERR);
                }

                RUL_TOKEN_SUBTYPE SubType = pToken.GetSubType();

                bool lboxState = false;
                bool rboxState = false;
                bool numState = false;
                bool idState = true;
                bool commaState = false;
                CToken pArrToken = null;
                int dimCnt = 0; // stevev 25apr13 - we have to reuse bracket scoped arrays

                while ((CLexicalAnalyzer.LEX_FAIL != plexAnal.GetNextVarToken(ref pToken, ref pSymbolTable, SubType))
                        && pToken != null && (!pToken.IsEOS()))
                {
                    int idex = 0;
                    CVariable cv = pSymbolTable.Find(pToken.GetLexeme(), ref idex);
                    if (cv != null)
                    {
                        pSymToken = cv.Token;
                    }
                    if ((!pToken.IsSymbol() && pSymToken != null)
                         || (RUL_TOKEN_SUBTYPE.RUL_LBOX == pToken.GetSubType())
                        || (RUL_TOKEN_SUBTYPE.RUL_RBOX == pToken.GetSubType())
                        || (pToken.IsNumeric()))
                    {
                        if (pToken.IsArrayVar())
                        {
                            lboxState = true;
                            rboxState = false;
                            numState = false;
                            idState = false;
                            commaState = false;

                            pSymToken.SetSubType(SubType);
                            //Make a copy of the array Token
                            pArrToken = new CToken(pToken);
                        }
                        else if (lboxState)
                        {
                            if (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_LBOX)
                            {
                                //ADD_ERROR(C_DECL_ERROR_LBOX);
                                plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                            }
                            lboxState = false;
                            rboxState = true;
                            numState = false;
                            idState = false;
                            commaState = false;
                        }
                        else if (rboxState)
                        {
                            if ((null == pArrToken) || !pToken.IsNumeric())
                            {
                                //ADD_ERROR(C_DECL_ERROR_NUM);
                                plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                            }

                            lboxState = false;
                            rboxState = false;
                            numState = true;
                            idState = false;
                            commaState = false;
                            //This is a number and with pArrToken get the symbol table token 

                            int i32Idx = pArrToken.GetSymbolTableIndex();
                            CVariable pVar = pSymbolTable.GetAt(i32Idx);
                            //INTER_VARIANT varArray = pVar.GetValue();
                            VARIANT_TYPE vtSafeArray = new VARIANT_TYPE();

                            //increment the dimension and set the limit of that dimension
                            if (pVar.GetValue().GetVarType() != VARIANT_TYPE.RUL_SAFEARRAY || dimCnt == 0)// stevev 25apr13
                            {
                                // WS:EMP-17jul07:varArray.Clear();
                                // WS:EMP-17jul07:varArray.varType = RUL_SAFEARRAY;
                                // WS:EMP-17jul07:__VAL& val = (__VAL&)varArray.GetValue();
                                // stevev-14feb08:make it more flexible...INTER_SAFEARRAYBOUND rgbound[1] = {atoi(pToken.GetLexeme())};
                                INTER_SAFEARRAYBOUND rgbound = new INTER_SAFEARRAYBOUND();
                                rgbound.cElements = Convert.ToUInt32(pToken.GetLexeme());
                                //INTER_SAFEARRAYBOUND rgbound[1] = { strtoul(pToken->GetLexeme(), NULL, 0) };
                                INTER_SAFEARRAY sa = new INTER_SAFEARRAY(); // WS:EMP-17jul07 was::>val.prgsa = new INTER_SAFEARRAY();
                                ushort cDims = 0;  // WS:EMP-17jul07 was::>_USHORT cDims = (val.prgsa).GetDims();

                                TokenType_to_VariantType(pToken.GetType(), SubType, ref vtSafeArray);

                                // WS:EMP-17jul07 was::>(val.prgsa).SetAllocationParameters(vtSafeArray, ++cDims, rgbound);
                                sa.SetAllocationParameters(vtSafeArray, ++cDims, rgbound);
                                sa.Allocate(); // stevev 11jun09 - get rid of error message, destructor will deallocate
                                //varArray = sa; // added WS:EMP-17jul07
                                pVar.GetValue().SetValue(sa);
                                pSymbolTable.SetAt(i32Idx, pVar);
                                dimCnt = 1;
                            }
                            else// isa RUL_SAFEARRAY && dimCnt > 0
                            {
                                __VAL val = pVar.GetValue().GetValue();
                                //INTER_SAFEARRAYBOUND rgbound[1] = { strtoul(pToken.GetLexeme(), NULL, 0) };
                                INTER_SAFEARRAYBOUND rgbound = new INTER_SAFEARRAYBOUND();
                                rgbound.cElements = 3;
                                (val.prgsa).AddDim(rgbound);
                                dimCnt++;
                            }

                        }
                        else if (numState)
                        {
                            if (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_RBOX)
                            {
                                //ADD_ERROR(C_DECL_ERROR_RBOX);
                                plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                            }

                            //accept a Right box.
                            lboxState = true;
                            rboxState = false;
                            numState = false;
                            commaState = true;
                            idState = false;
                        }
                        else
                        {
                            if (idState)
                            {
                                if (pToken.GetType() != RUL_TOKEN_TYPE.RUL_SIMPLE_VARIABLE)
                                {
                                    //ADD_ERROR(C_DECL_ERROR_IDMISSING);
                                    plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                                }
                                pSymToken.SetSubType(SubType);
                            }
                            else
                            {
                                //ADD_ERROR(C_DECL_ERROR_COMMAMISSING);
                                plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                            }
                            lboxState = false;
                            rboxState = false;
                            numState = false;
                            idState = false;
                            commaState = true;
                        }
                    }
                    else if (commaState)
                    {
                        if (pToken.GetSubType() != RUL_TOKEN_SUBTYPE.RUL_COMMA)
                        {
                            //ADD_ERROR(C_DECL_ERROR_COMMAMISSING);
                            plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                        }

                        idState = true;
                        commaState = false;
                        lboxState = false;
                        rboxState = false;
                        numState = false;
                    }
                    else
                    {
                        //Of course, this is a problem case. 
                        //Unfortunately, expressions in the declarations are not handled
                        //ADD_ERROR(C_DECL_ERROR_EXPRESSION);
                        plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);

                        //accept a Right box. //VMKP added on 030404
                        /*  Synchronizing was not proper when an expression
						 present in the variable declaration, With that the 
						 below lines one declaration next to the expression
						 declaration is skipping */
                        lboxState = true;
                        rboxState = false;
                        numState = false;
                        commaState = true;
                        idState = false;
                    }
                }//end of while loop

                //Validate the exit criteria...
                if (!(rboxState == numState == idState == false) || !(commaState == true))
                {
                    //ADD_ERROR(C_DECL_ERROR_UNKNOWN);
                    plexAnal.SynchronizeTo(PRODUCTION.DECLARATION, pSymbolTable);
                }

                return 1;
            }
            /*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
				plexAnal.MovePast(
					RUL_SYMBOL,
					RUL_SEMICOLON,
					pSymbolTable);
			}
			catch (...)
			{
				throw (C_UM_ERROR_UNKNOWNERROR);
			}

			return PARSE_FAIL;
			*/
        }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return -1;
        }

    }

    public class CDeclarations : CGrammarNode
    {
        protected List<CDeclaration> m_declList;

        public CDeclarations()
        {
            m_declList = new List<CDeclaration>();
        }

        //	Identify self
        public override void Identify(ref string szData)
        {
            szData += "<Declarations>";
            szData += "</Declarations>";
        }

        //	Allow Visitors to do different operations on the node.
        public override int Execute(CGrammarNodeVisitor pVisitor, CSymbolTable pSymbolTable,
            ref INTER_VARIANT pvar, RUL_TOKEN_SUBTYPE AssignType = RUL_TOKEN_SUBTYPE.RUL_ASSIGN)//Anil August 26 2005 to Fix a[exp1] += exp2
        {
            return pVisitor.visitDeclarations(
        this,
        pSymbolTable,
        ref pvar,
        AssignType);//Anil August 26 2005 to Fix a[exp1] += exp2
        }

        //	Create as much of the parse tree as possible.
        public override int CreateParseSubTree(ref CLexicalAnalyzer plexAnal, ref CSymbolTable pSymbolTable)
        {
            CParserBuilder builder = new CParserBuilder();
            CGrammarNode pDecl = null;

            //try
            {
                while (null != (pDecl = builder.CreateParser(ref plexAnal, STATEMENT_TYPE.STMT_DECL)))
                {                    
                    m_declList.Add((CDeclaration)pDecl);
                    pDecl.CreateParseSubTree(ref plexAnal, ref pSymbolTable);
                }
                return 1;
            }
            /*
			catch (CRIDEError* perr)
			{
				pvecErrors.push_back(perr);
				//just skip a few tokens and hope everything becomes alright.
				plexAnal.MovePast(
					RUL_SYMBOL,
					RUL_SEMICOLON,
					pSymbolTable);
			}
			catch (...)
			{
				throw (C_UM_ERROR_UNKNOWNERROR);
			}
			return PARSE_FAIL;
			*/
        }

        //This returns the last line in which this node has a presence...
        public override int GetLineNumber()
        {
            return -1;
        }

        List<CDeclaration> GetDeclarations()
        {
            return m_declList;
        }
    }

    public class CParserBuilder
    {
        public CGrammarNode CreateParser(ref CLexicalAnalyzer plexAnal, STATEMENT_TYPE stmt_type)
        {
            CGrammarNode pNode = null;
            CToken pToken = null;

            if ((CLexicalAnalyzer.LEX_FAIL != (plexAnal.LookAheadToken(ref pToken))) && pToken != null)
            {
                if (((stmt_type == STATEMENT_TYPE.STMT_DECL) || (stmt_type == STATEMENT_TYPE.STMT_asic))
                    && ((pToken.GetType() == RUL_TOKEN_TYPE.RUL_SYMBOL) && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SEMICOLON)))
                {
                    pNode = new CEmptyStatement();
                }
                else if (
                    ((stmt_type == STATEMENT_TYPE.STMT_DECL) || (stmt_type == STATEMENT_TYPE.STMT_asic))
                    && ((pToken.GetType() == RUL_TOKEN_TYPE.RUL_SYMBOL) && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_LPAREN))
                    )
                {
                    pNode = new CExpression();
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_DECL) || (stmt_type == STATEMENT_TYPE.STMT_asic)) && pToken.IsDeclaration())          //Declaration Statement
                {
                    pNode = new CDeclaration();
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_asic))
                        && (RUL_TOKEN_TYPE.RUL_KEYWORD == pToken.GetType())
                        && !pToken.IsIteration()
                        && !pToken.IsFunctionToken())
                {
                    if (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_IF)// IF Selection Statement
                    {
                        pNode = new CSelectionStatement();
                    }
                    else if (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_SWITCH)//SWITCH Statement
                    {
                        pNode = new CSwitchStatement();
                    }
                    else if (RUL_TOKEN_TYPE.RUL_KEYWORD == pToken.GetType()
                        && RUL_TOKEN_SUBTYPE.RUL_RULE_ENGINE == pToken.GetSubType())     //Rule Statement
                    {
                        pNode = new CRuleServiceStatement();
                    }
                    else if (RUL_TOKEN_TYPE.RUL_KEYWORD == pToken.GetType()
                        && RUL_TOKEN_SUBTYPE.RUL_BREAK == pToken.GetSubType())       //break Statement
                    {
                        pNode = new CBreakStatement();
                    }
                    else if (RUL_TOKEN_TYPE.RUL_KEYWORD == pToken.GetType()
                        && RUL_TOKEN_SUBTYPE.RUL_CONTINUE == pToken.GetSubType())        //continue Statement
                    {
                        pNode = new CContinueStatement();
                    }
                    else if (RUL_TOKEN_TYPE.RUL_KEYWORD == pToken.GetType()
                        && RUL_TOKEN_SUBTYPE.RUL_RETURN == pToken.GetSubType())      //continue Statement
                    {
                        pNode = new CReturnStatement();
                    }
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_ITERATION) || (stmt_type == STATEMENT_TYPE.STMT_asic)) && pToken.IsIteration())                               //Iteration Statement
                {
                    if (pToken.IsWHILEStatement())
                    {
                        pNode = new CIterationStatement();
                    }
                    else if (pToken.IsDOStatement())
                    {
                        pNode = new CIterationDoWhileStatement();
                    }
                    else if (pToken.IsFORStatement())
                    {
                        pNode = new CIterationForStatement();
                    }
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_ASSIGNMENT) || (stmt_type == STATEMENT_TYPE.STMT_asic)) && (pToken.IsFunctionToken()))
                //Assignment Statement
                {
                    pNode = new CExpression();
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_ASSIGNMENT) || (stmt_type == STATEMENT_TYPE.STMT_asic))
                        && (pToken.IsVariable() || pToken.IsArrayVar() || pToken.IsDDItem()//Added By Anil August 4 2005
                            || pToken.IsOMToken() || pToken.IsNumeric()
                            || pToken.IsConstant() || pToken.IsFunctionToken() || pToken.IsOperator()
                            || ((pToken.GetType() == RUL_TOKEN_TYPE.RUL_SYMBOL) && (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_LPAREN))
                            ))
                //Assignment Statement
                {
                    CToken pNewToken = null;  //TSRPRASAD 09MAR2004 Fix the memory leaks	*/

                    bool bLineIsAssignment = false;
                    if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ARITHMETIC_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_PLUS_ASSIGN, ref pNewToken))   //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MINUS_ASSIGN, ref pNewToken))   //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_DIV_ASSIGN, ref pNewToken))     //TSRPRASAD 09MAR2004 Fix the memory leaks	*/			
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MOD_ASSIGN, ref pNewToken))   //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }

                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_MUL_ASSIGN, ref pNewToken))    //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_AND_ASSIGN, ref pNewToken)) //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_OR_ASSIGN, ref pNewToken))  //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_XOR_ASSIGN, ref pNewToken)) //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_RSHIFT_ASSIGN, ref pNewToken))  //TSRPRASAD 09MAR2004 Fix the memory leaks	*/
                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_BIT_LSHIFT_ASSIGN, ref pNewToken)) //TSRPRASAD 09MAR2004 Fix the memory leaks	*/

                    {
                        bLineIsAssignment = true;
                    }
                    else if (plexAnal.ScanLineForToken(RUL_TOKEN_TYPE.RUL_ASSIGNMENT_OPERATOR, RUL_TOKEN_SUBTYPE.RUL_ASSIGN, ref pNewToken)) //TSRPRASAD 09MAR2004 Fix the memory leaks	*/

                    {
                        bLineIsAssignment = true;
                    }

                    if (bLineIsAssignment)
                    {
                        pNode = new CAssignmentStatement();
                    }
                    else
                    {
                        pNode = new CExpression();
                    }

                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_ASSIGNMENT) || (stmt_type == STATEMENT_TYPE.STMT_asic)) && pToken.IsFunctionToken())                           //Assignment Statement
                {
                    pNode = new CAssignmentStatement();
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_COMPOUND) || (stmt_type == STATEMENT_TYPE.STMT_asic)) && pToken.IsCompound())                                //Compound Statement
                {
                    pNode = new CCompoundStatement();
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_SERVICE) || (stmt_type == STATEMENT_TYPE.STMT_asic) || (stmt_type == STATEMENT_TYPE.STMT_ASSIGNMENT_FOR))
                        && pToken.IsService())                             //Service Statement
                {
                    pNode = new CServiceStatement();
                }
                else if (((stmt_type == STATEMENT_TYPE.STMT_SELECTION)) && (RUL_TOKEN_TYPE.RUL_KEYWORD == pToken.GetType()))
                {
                    if ((pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_CASE) || (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_DEFAULT))//CASE or DEFAULT Statement
                    {
                        pNode = new CCASEStatement();
                    }
                    else if (pToken.GetSubType() == RUL_TOKEN_SUBTYPE.RUL_ELSE)//ELSE Statement
                    {
                        pNode = new CELSEStatement();
                    }
                }
                else
                {
                    //error
                    //the natural control flow is allowed to take care of this 
                    //erroneous condition.
                }
            }
            return pNode;
        }
    }

}
