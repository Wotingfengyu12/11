using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace FieldIot.HARTDD
{
    public enum blockingType_t
    {
        bt_UNDEFINED,
        bt_ISBLOCKING,
        bt_NOTBLOCKING
    }

    public enum cmdOriginator_t
    {
        co_INITED = 0x00,
        co_READ_IMD = 0x01,     // From ReadImd (was co_DEV_LOADING)
        co_METHOD = 0x02,       // From sendMethCmd
        co_WRITEIMD = 0x04,     // From WriteImd (was co_WRITE)
        co_ASYNC_READ = 0x08,   // From Async Read (ServiceReads)
        co_ASYNC_WRITE = 0x10,  // From Async Write (ServiceWrites)
        co_TRANSPORT = 0x20,   // command 112 cycles
        co_UNDEFINED = 0x80      // I haven't a clue where I came from
    }

    public enum cmdDataItemType_t  /*  note that in the Interface Library 0,1,5 are only ones used */
    {
        cmdDataConst = 0,/*      integer constant - only (flags & width set to 0)            */
        cmdDataReference,       /* 1 reference <others are converted to this >                      */
        cmdDataFlags,           /* converted to (1) with non zero flags (width set to 0)            */
        cmdDataWidth,           /* converted to (1) with flags set to WIDTH_PRESENT (width set)     */
        cmdDataFlagWidth,       /* converted to (1) with flags set + flgWIDTH_PRESENT set(width set)*/
        cmdDataFloat            /* 5    float constant - only (flags & width set to 0)              */
    }                           /*    SEE cmdDataItemFlags_t below for valid flag values            */

    public class CDDLCmd : CDDLBase
    {
        uint uiCmdNum;
        cmdOperationType_t operation;
        hCRespCodeList cmdRespCodes;
        hCtransactionList transList;
        List<uint> cmdVarIDList;

        public CDDLCmd()
        {
            eType = nitype.nCmd;
            transList = new hCtransactionList();
            cmdRespCodes = new hCRespCodeList();
            transList.setCmdPtr(this);
            cmdVarIDList = new List<uint>();
        }

        public PackData getRequestData(int iTranNum, bool write = false)
        {
            PackData data = new PackData();
            hCtransaction ctransaction = null;

            foreach (hCtransaction tr in transList)
            {
                if (tr.number == (ulong)iTranNum)
                {
                    ctransaction = tr;
                    break;
                }

            }

            if (ctransaction == null)
            {
                return null;
            }

            byte index = 0;

            foreach (DATA_ITEM pDItm in ctransaction.request)
            {
                uint varID = Common.getVarID(pDev, pDItm);
                cmdItem ci = new cmdItem();
                ci.itemType = (cmdDataItemType_t)pDItm.type;
                ci.ucConst = (byte)pDItm.data.iconst;
                ci.fConst = pDItm.data.fconst;
                ci.uiVarId = varID;
                ci.flags = (cmdDataItemFlags_t)pDItm.flags;
                ci.ucMask = (byte)pDItm.mask;
                switch (ci.itemType)
                {
                    case cmdDataItemType_t.cmdDataConst:
                        data.Add(ci.ucConst);
                        break;

                    case cmdDataItemType_t.cmdDataFloat:
                        data.AddFloat(ci.fConst);
                        break;

                    case cmdDataItemType_t.cmdDataReference:
                        //add var
                        CDDLVar resVar;
                        CDDLBase ddb = new CDDLBase();
                        if (pDev.getItembyID(varID, ref ddb))
                        {
                            resVar = (CDDLVar)ddb;

                            if (write && resVar.getWriteStatus() == writestatus_t.waitforwrite)
                            {
                                resVar.setWriteStatus(writestatus_t.writting);
                                switch (resVar.VariableType())
                                {
                                    case variableType_t.vT_Ascii:
                                    case variableType_t.vT_Password:
                                        byte[] wridata =  Encoding.Default.GetBytes(resVar.wValueString);
                                        for(int i = 0; i < wridata.Length; i++)
                                        {
                                            data.Add(wridata[i]);
                                        }
                                        break;

                                    case variableType_t.vT_PackedAscii:
                                        //resVar.wValueString.ToUpper();
                                        resVar.wValueString = resVar.wValueString.PadRight((resVar.getSize() / 3) * 4, ' ');
                                        byte[] pdata = Encoding.Default.GetBytes(resVar.wValueString);
                                        byte[] wdata = new byte[resVar.getSize()];
                                        Common.Pack(ref wdata, pdata, pdata.Length);
                                        for (int i = 0; i < wdata.Length; i++)
                                        {
                                            data.Add(wdata[i]);
                                        }
                                        break;

                                    case variableType_t.vT_BitEnumerated:
                                    case variableType_t.vT_Enumerated:
                                        //resVar.getSize();
                                        data.Add((byte)resVar.wValueData);
                                        break;

                                    case variableType_t.vT_Boolean:
                                        break;

                                    case variableType_t.vT_FloatgPt:
                                        float fdata = Convert.ToSingle(resVar.wValueString);
                                        data.AddFloat(fdata);
                                        break;

                                    case variableType_t.vT_HartDate:
                                        DateTime dt = Convert.ToDateTime(resVar.wValueString);
                                        data.Add((byte)dt.Day);
                                        data.Add((byte)dt.Month);
                                        data.Add((byte)(dt.Year - 1900));
                                        break;

                                    case variableType_t.vT_Double:
                                    case variableType_t.vT_Duration:
                                    case variableType_t.vT_DateAndTime:
                                    case variableType_t.vT_EUC:
                                    case variableType_t.vT_Index:
                                    case variableType_t.vT_Integer:
                                    case variableType_t.vT_Time:
                                    case variableType_t.vT_TimeValue:
                                    case variableType_t.vT_Unsigned:
                                        break;

                                    case variableType_t.vT_BitString:
                                    case variableType_t.vT_OctetString:
                                    case variableType_t.vT_MaxType:
                                    case variableType_t.vT_undefined:
                                    case variableType_t.vT_unused:
                                    case variableType_t.vT_VisibleString:
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                CValueVarient cvalue = null;
                                cvalue = resVar.getRawDispValue();

                                switch (cvalue.vType)
                                {
                                    case valueType_t.isSymID:
                                        data.AddInt(cvalue.GetInt());
                                        break;

                                    case valueType_t.isFloatConst:
                                        data.AddFloat(cvalue.GetFloat());
                                        break;

                                    case valueType_t.isIntConst:
                                        if (cvalue.vSize == 1)
                                        {
                                            data.Add(cvalue.GetByte());
                                        }
                                        else if (cvalue.vSize == 2)
                                        {
                                            data.AddShort((short)cvalue.GetInt());
                                        }
                                        else
                                        {
                                            data.AddInt(cvalue.GetInt(), cvalue.vSize);
                                        }
                                        break;

                                    case valueType_t.isString:
                                        byte[] sdata = Encoding.ASCII.GetBytes(cvalue.GetString());

                                        foreach (byte uc in sdata)
                                        {
                                            data.Add(uc);
                                        }
                                        break;

                                    case valueType_t.isWideString:
                                        byte[] udata = Encoding.UTF8.GetBytes(cvalue.GetString());

                                        foreach (byte uc in udata)
                                        {
                                            data.Add(uc);
                                        }
                                        break;

                                    case valueType_t.isPackedString:
                                        //reVar.wValueString.ToUpper();
                                        string str = cvalue.GetString().PadRight((resVar.getSize() / 3) * 4, ' ');
                                        byte[] pdata = Encoding.Default.GetBytes(str);
                                        byte[] wdata = new byte[resVar.getSize()];
                                        Common.Pack(ref wdata, pdata, pdata.Length);
                                        for (int i = 0; i < wdata.Length; i++)
                                        {
                                            data.Add(wdata[i]);
                                        }

                                        break;

                                    default:
                                        break;
                                }
                            }

                            if (pDItm.mask == 0xff)
                            {
                                data.RemoveRange(0, data.Count - 1);
                            }
                            else if(pDItm.mask == 0xffff)
                            {
                                data.RemoveRange(0, data.Count - 2);
                            }

                        }
                        break;

                    case cmdDataItemType_t.cmdDataFlags:
                        if (ci.uiVarId != 0)
                        {
                            CDDLVar reVar;
                            CDDLBase db = new CDDLBase();
                            if (pDev.getItembyID(varID, ref db))
                            {
                                reVar = (CDDLVar)db;
                                if (write && reVar.getWriteStatus() == writestatus_t.writting)
                                {
                                    switch (reVar.VariableType())
                                    {
                                        case variableType_t.vT_Ascii:
                                        case variableType_t.vT_Password:
                                            byte[] wridata = Encoding.Default.GetBytes(reVar.wValueString);
                                            for (int i = 0; i < wridata.Length; i++)
                                            {
                                                data.Add(wridata[i]);
                                            }
                                            break;

                                        case variableType_t.vT_PackedAscii:
                                            //reVar.wValueString.ToUpper();
                                            reVar.wValueString = reVar.wValueString.PadRight((reVar.getSize() / 3) * 4, ' ');
                                            byte[] pdata = Encoding.Default.GetBytes(reVar.wValueString);
                                            byte[] wdata = new byte[reVar.getSize()];
                                            Common.Pack(ref wdata, pdata, pdata.Length);
                                            for (int i = 0; i < wdata.Length; i++)
                                            {
                                                data.Add(wdata[i]);
                                            }
                                            break;

                                        case variableType_t.vT_BitEnumerated:
                                        case variableType_t.vT_Enumerated:
                                            //resVar.getSize();
                                            data.Add((byte)reVar.wValueData);
                                            break;

                                        case variableType_t.vT_Boolean:
                                            break;

                                        case variableType_t.vT_FloatgPt:
                                            float fdata = Convert.ToSingle(reVar.wValueString);
                                            data.AddFloat(fdata);
                                            break;

                                        case variableType_t.vT_HartDate:
                                            DateTime dt = Convert.ToDateTime(reVar.wValueString);
                                            data.Add((byte)dt.Day);
                                            data.Add((byte)dt.Month);
                                            data.Add((byte)(dt.Year - 1900));
                                            break;

                                        case variableType_t.vT_Double:
                                        case variableType_t.vT_Duration:
                                        case variableType_t.vT_DateAndTime:
                                        case variableType_t.vT_EUC:
                                        case variableType_t.vT_Index:
                                        case variableType_t.vT_Integer:
                                        case variableType_t.vT_Time:
                                        case variableType_t.vT_TimeValue:
                                        case variableType_t.vT_Unsigned:
                                            break;

                                        case variableType_t.vT_BitString:
                                        case variableType_t.vT_OctetString:
                                        case variableType_t.vT_MaxType:
                                        case variableType_t.vT_undefined:
                                        case variableType_t.vT_unused:
                                        case variableType_t.vT_VisibleString:
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    CValueVarient cvalue = null;
                                    cvalue = reVar.getRawDispValue();

                                    switch (cvalue.vType)
                                    {
                                        case valueType_t.isSymID:
                                            data.AddInt(cvalue.GetInt());
                                            break;

                                        case valueType_t.isFloatConst:
                                            data.AddFloat(cvalue.GetFloat());
                                            break;

                                        case valueType_t.isIntConst:
                                            if (cvalue.vSize == 1)
                                            {
                                                data.Add(cvalue.GetByte());
                                            }
                                            else if (cvalue.vSize == 2)
                                            {
                                                data.AddShort((short)cvalue.GetInt());
                                            }
                                            else
                                            {
                                                data.AddInt(cvalue.GetInt());
                                            }
                                            break;

                                        case valueType_t.isString:
                                            byte[] sdata = Encoding.ASCII.GetBytes(cvalue.GetString());

                                            foreach (byte uc in sdata)
                                            {
                                                data.Add(uc);
                                            }
                                            break;

                                        case valueType_t.isWideString:
                                            byte[] udata = Encoding.UTF8.GetBytes(cvalue.GetString());

                                            foreach (byte uc in udata)
                                            {
                                                data.Add(uc);
                                            }
                                            break;

                                        case valueType_t.isPackedString:
                                            //reVar.wValueString.ToUpper();
                                            string str = cvalue.GetString().PadRight((reVar.getSize() / 3) * 4, ' ');
                                            byte[] pdata = Encoding.Default.GetBytes(str);
                                            byte[] wdata = new byte[reVar.getSize()];
                                            Common.Pack(ref wdata, pdata, pdata.Length);
                                            for (int i = 0; i < wdata.Length; i++)
                                            {
                                                data.Add(wdata[i]);
                                            }

                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        //add var
                        if ((ci.flags & cmdDataItemFlags_t.cmdDataItemFlg_Index) != 0)
                        {
                            data.Add(index);
                            CDDLVar reVar;
                            CDDLBase db = new CDDLBase();
                            if (pDev.getItembyID(varID, ref db))
                            {
                                reVar = (CDDLVar)db;
                                if(reVar.IsLocal())
                                {
                                    reVar.getDispValue().SetValue((int)index, valueType_t.isIntConst);
                                }
                            }
                            index++;
                        }

                        break;

                    default:
                        break;
                }
            }

            return data;
        }

        public bool setResponseData(int iTranNum, byte[] data, byte len)
        {
            hCtransaction ctransaction = null;

            foreach (hCtransaction tr in transList)
            {
                if (tr.number == (ulong)iTranNum)
                {
                    ctransaction = tr;
                    break;
                }
            }

            if (ctransaction == null)
            {
                return false;
            }
            byte offset = 0;
            byte index = 0;
            int mask = 0;
            bool next = true;

            foreach (DATA_ITEM pDItm in ctransaction.reply)
            {
                /*
                cmdItem ci = new cmdItem();

                ci.ucConst = (byte)pDItm.data.iconst;
                ci.fConst = pDItm.data.fconst;
                ci.uiVarId = varID;
                ci.flags = (cmdDataItemFlags_t)pDItm.flags;
                ci.ucMask = (byte)pDItm.mask;
                */

                ///flag,mask?

                switch ((cmdDataItemType_t)pDItm.type)
                {
                    case cmdDataItemType_t.cmdDataConst:
                        //data.Add(ci.ucConst);
                        offset++;
                        break;

                    case cmdDataItemType_t.cmdDataFloat:
                        //data.AddFloat(ci.fConst);
                        offset += 4;
                        break;

                    case cmdDataItemType_t.cmdDataFlags:
                        if ((pDItm.flags & (int)cmdDataItemFlags_t.cmdDataItemFlg_Index) != 0)
                        {
                            index = data[offset];
                        }
                        offset++;
                        break;

                    case cmdDataItemType_t.cmdDataReference:
                    case cmdDataItemType_t.cmdDataWidth:
                        //add var
                        uint varID = Common.getVarID(pDev, pDItm);

                        if (varID == 0x1f84)
                        {
                            ;
                        }

                        if(varID == 0x96 || varID == 0x97)
                        {
                            break;
                        }

                        CDDLVar resVar;
                        CDDLBase ddb = new CDDLBase();
                        if (pDev.getItembyID(varID, ref ddb))
                        {
                            resVar = (CDDLVar)ddb;

                            byte setsize;

                            if ((pDItm.mask & 0xff) > 0)//== 0xff)
                            {
                                setsize = 1;
                                mask |= (int)pDItm.mask;
                                if(mask != 0xff)
                                {
                                    next = false;
                                }
                                else
                                {
                                    next = true;
                                }
                            }

                            else if((pDItm.mask & 0xffff) > 0)// == 0xffff)
                            {
                                setsize = 2;
                                mask |= (int)pDItm.mask;
                                if (mask != 0xffff)
                                {
                                    next = false;
                                }
                                else
                                {
                                    next = true;
                                }
                            }

                            else
                            {
                                setsize = (byte)resVar.getSize();
                                next = true;
                            }

                            if (setsize + offset > len && offset <= 0xf0)
                            {
                                return false;
                            }
                            if(resVar.getWriteStatus() == writestatus_t.writting)
                            {
                                resVar.setWriteStatus(writestatus_t.none);
                                pDev.parentform.RemoveVarFromWriteList(resVar);
                            }
                            if(pDItm.mask != 0)
                            {
                                resVar.setValue(data, offset, len, setsize, (uint)pDItm.mask);
                            }
                            else
                            {
                                resVar.setValue(data, offset, len, setsize);
                            }
                            if (next)
                            {
                                offset += setsize;
                            }
                        }

                        break;

                    default:
                        break;
                }
            }

            return true;
        }
        
        public void insertWgtVisitor(hCcmdDispatcher pDispatch) // all transactions
        {// for each in transaction list: setVisitor(fpWgtCalcs);
            transList.setWgtVisitor(pDispatch);
        }

        public void addTransction(hCtransaction hct)
        {
            transList.Add(hct);
        }

        public void addRespCode(hCrespCode hcc)
        {
            cmdRespCodes.Add(hcc);
        }

        public uint getCmdNumber()// -1 on error
        {
            return uiCmdNum;
        }

        public void setCmdNumber(uint cmd)// -1 on error
        {
            uiCmdNum = cmd;
        }

        public override bool IsCmd()
        {
            return true;
        }

        public void setOperation(int op)
        {
            operation = (cmdOperationType_t)op;
        }

        public cmdOperationType_t getOperation()
        {
            return operation;
        }

        public int getRespCodes(ref hCRespCodeList rcList) // resolve and fill return list
        {// copy command level response codes into list
         //hCRespCodeList* pRespCdLst = cmdRespCodes.resolveCond(); 
            int rc = Common.SUCCESS;
            CDDLBase pItm = new CDDLBase();
            CDDLVar pInt = new CDDLVar();
            //hCenumList rcLst(devHndl());
            //vector<hCenumDesc>::iterator ied;
            //hCRespCodeList localRCList = new hCRespCodeList();

            // get the variable first then overlay the command's;  transaction will overlay that
            if (pDev.getItembyID(Common.RESPONSECODE_SYMID, ref pItm) && pItm != null)
            {// do response code
                pInt = (CDDLVar)pItm;
                //rc = pInt.procureList(rcLst);
                if (rc == Common.SUCCESS && pInt.enmList.Count > 0)
                {
                    foreach (EnumTriad_t enm in pInt.enmList)
                    {// ptr2a hCenumDesc
                     //works for vs6 #if _MSC_VER >= 1300  
                     // HOMZ-port to 2003,VS7>> error C2440: 'type case' cannot convert from 'std::vector<_Ty> ..etc
                        hCrespCode wrkingRC = new hCrespCode();
                        wrkingRC.val = enm.val;
                        wrkingRC.descS = enm.descS;
                        wrkingRC.helpS = enm.helpS;

                        wrkingRC.type = 0;//////??????

                        //#else
                        //				wrkingRC = *((hCenumDesc*)ied);
                        //#endif

                        rcList.Add(wrkingRC);
                        //wrkingRC.clear();
                    }
                }
            }

            //hCRespCodeList* pRespCdLst = cmdRespCodes.resolveCond();
            //rc = cmdRespCodes.resolveCond(&localRCList);
            //if (rc == Common.SUCCESS && localRCList.Count > 0)
            //{
            //rcList.append(&localRCList); // appends uniquely with insertion overwritting existing
            //localRCList.destroy();
            //}

            //rcList = (hCRespCodeList)rcList.Concat(cmdRespCodes);
            foreach(hCrespCode hcc in cmdRespCodes)
            {
                rcList.Add(hcc);
            }

            return Common.SUCCESS;
        }

        int updateVarCmdWeight(int baseWgt, int transNum, DATA_ITEM_LIST dataItmList/*, bool isWrite*/)
        {
            int rc = Common.SUCCESS;//, rb = Common.SUCCESS;
            //GroupItemList_t localVarGroup;// a list of hCGroupItemInf
            hCcommandDescriptor localDesc = new hCcommandDescriptor();    //int cmdNumber;	int transNumb;	int rd_wrWgt other;

            cmdVarIDList.Clear();

            localDesc.transNumb = transNum;
            localDesc.cmdNumber = (int)getCmdNumber();
            localDesc.rd_wrWgt = baseWgt;//stevev 3/25/04 noticed we were missing the base weight 
                                         //in the first iteration stevev 29jan09 - moved from below 4 lists
            localDesc.cmdTyp = operation;//to keep from looking it up all the time..

            foreach (DATA_ITEM pDItm in dataItmList)
            {
                uint varID = 0;
                if ((pDItm.flags & (int)cmdDataItemFlags_t.cmdDataItemFlg_Info) != (int)cmdDataItemFlags_t.cmdDataItemFlg_Info)
                {
                    varID = Common.getVarID(pDev, pDItm);
                }
                if (varID != 0 && !cmdVarIDList.Contains(varID))
                {
                    cmdVarIDList.Add(varID);
                }

                cmdItem ci = new cmdItem();
                ci.itemType = (cmdDataItemType_t)pDItm.type;
                ci.ucConst = (byte)pDItm.data.iconst;
                ci.fConst = pDItm.data.fconst;
                ci.uiVarId = varID;
                ci.flags = (cmdDataItemFlags_t)pDItm.flags;
                ci.ucMask = (byte)pDItm.mask;
                localDesc.cmdItemList.Add(ci);
            }// next dataitem
            //DEBUGLOG(CLOG_LOG, ">    Cmd: %d has %d Vars added in addAllVars.(dbg only msg)\n", getCmdNumber(), localVarGroup.size());
            //localDesc.idxList.Clear();
            int non_local_Index = 0;   // valid for the entire list

            CDDLVar pVar = null;

            foreach (uint ui in cmdVarIDList)
            {//itGIL is a ptr 2 hCGroupItemInfo of ONE possible var in this response
                CDDLBase db = new CDDLBase();
                if (pDev.getItembyID(ui, ref db))
                {
                    pVar = (CDDLVar)db;
                    if ((pVar.VariableType() == variableType_t.vT_Index || pVar.VariableType() == variableType_t.vT_Enumerated)
                        && (!pVar.IsLocal()))
                    {
                        non_local_Index = 100; // force it to be last considered
                    }   // used for all vars in the command

                    localDesc.rd_wrWgt -= non_local_Index;
                    if (operation == cmdOperationType_t.cmdOpWrite)
                    {
                        pVar.addWRdesc(localDesc);
                        /*
                        if LOGTHIS(LOGP_DD_INFO)
                        {
                            LOGIT(CLOG_LOG | COUT_LOG, "Item 0x%04x WRIT Desc:", pGIL.theVarID);
                            localDesc.dumpSelf();
                        }
                        */
                    }
                    else if (operation == cmdOperationType_t.cmdOpRead)
                    {
                        pVar.addRDdesc(localDesc);
                        /*
                        if LOGTHIS(LOGP_DD_INFO)
                        {
                            LOGIT(CLOG_LOG | COUT_LOG, "Item 0x%04x READ Desc:", pGIL.theVarID);
                            localDesc.dumpSelf();
                        }
                        //	if (localDesc.idxList.size() > 1)
                        //	{
                        //		LOGIT(CLOG_LOG,"WARNING: command %d dependent on %d local indexes.\n",
                        //										getCmdNumber(),localDesc.idxList.size() );
                        //	}
                        */
                    }
                }
                else // null in list or pointer not found for symbol
                {
                    ;
                    /*
                    if (&(*pGIL) == NULL)// PAW added &* 03/03/09
                    {
                        LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: resp groupItem was NULL from addAllVars"
        
                                                                    " in updateVarCommandUsage\n");
                    }
                    else // get var by pointer failed
                    {
                        
                        LOGIF(LOGP_NOT_TOK) (CERR_LOG, "ERROR: resp groupItem's getVarPtrBySymNumber failed "
        
                                " for Item 0x%04x (%s) in updateVarCommandUsage.\n", pGIL.theVarID,
                                (pGIL.theVarPtr) ? pGIL.theVarPtr.getName().c_str() : "");

                        hCitemBase* pItem = NULL;

                        if (devPtr().getItemBySymNumber(pGIL.theVarID, &pItem) == SUCCESS && pItem != NULL)
                        {
                            LOGIT(CERR_LOG, "     : is a %s item.", pItem.getTypeName());
                        }
                        else
                        {
                            LOGIT(CERR_LOG, "     : apparently does not exist in this Device.");
                        }
                        LOGIT(CERR_LOG, "(Used in command number %d (%s))\n", getCmdNumber(), getName().c_str());
                    } */
                }
                localDesc.rd_wrWgt = baseWgt;// reset the weighting for each var
                                             //localDesc.idxList.clear();
                                             //non_local_Index = 0;

            }// next group item
             //Memleak fix: DEEPAK 290105 
             //localVarGroup.clear();
             //END
            return rc;
        }

        int updateVarCmdCmd(int transNum, DATA_ITEM_LIST dataItmList, bool bWrite)
        {
            int rc = Common.SUCCESS;//, rb = Common.SUCCESS;
            hCcommandDescriptor localDesc = new hCcommandDescriptor();    //int cmdNumber;	int transNumb;	int rd_wrWgt other;
            cmdVarIDList.Clear();
            localDesc.transNumb = transNum;
            localDesc.cmdNumber = (int)getCmdNumber();
            localDesc.rd_wrWgt = 0;//stevev 3/25/04 noticed we were missing the base weight 
            localDesc.cmdTyp = operation;//to keep from looking it up all the time..

            foreach (DATA_ITEM pDItm in dataItmList)
            {
                uint varID = 0;
                if ((pDItm.flags & (int)cmdDataItemFlags_t.cmdDataItemFlg_Info) != (int)cmdDataItemFlags_t.cmdDataItemFlg_Info)
                {
                    varID = Common.getVarID(pDev, pDItm);
                }
                if (varID != 0 && !cmdVarIDList.Contains(varID))
                {
                    cmdVarIDList.Add(varID);
                }
                cmdItem ci = new cmdItem();
                ci.itemType = (cmdDataItemType_t)pDItm.type;
                ci.ucConst = (byte)pDItm.data.iconst;
                ci.fConst = pDItm.data.fconst;
                ci.uiVarId = varID;
                ci.flags = (cmdDataItemFlags_t)pDItm.flags;
                ci.ucMask = (byte)pDItm.mask;
                localDesc.cmdItemList.Add(ci);
            }// next dataitem
            int non_local_Index = 0;   // valid for the entire list

            CDDLVar pVar = null;

            foreach (uint ui in cmdVarIDList)
            {//itGIL is a ptr 2 hCGroupItemInfo of ONE possible var in this response
                CDDLBase db = new CDDLBase();
                if (pDev.getItembyID(ui, ref db))
                {
                    pVar = (CDDLVar)db;
                    if ((pVar.VariableType() == variableType_t.vT_Index || pVar.VariableType() == variableType_t.vT_Enumerated)
                        && (!pVar.IsLocal()))
                    {
                        non_local_Index = 100; // force it to be last considered
                    }   // used for all vars in the command

                    localDesc.rd_wrWgt -= non_local_Index;
                    if (bWrite)
                    {
                        pVar.addWRdesc(localDesc);
                    }
                    else
                    {
                        pVar.addRDdesc(localDesc);
                    }
                }
                else // null in list or pointer not found for symbol
                {
                    ;

                }
                localDesc.rd_wrWgt = 0;// reset the weighting for each var
            }
            return rc;
        }

        public int updateVarCommandUsage()
        {
            int rc = Common.SUCCESS;
            //cmdOperationType_t o = cmdOperationType_t.cmdOpNone,
            //O = cmdOperationType_t.cmdOpNone;
            //listOptrs2dataItemLists_t::iterator iP2DIL;
            CDDLBase pThisVarItem = new CDDLBase();
            int tNum = 0;


            // stevev 12feb09 - added cheap optimization..
            if (operation == cmdOperationType_t.cmdOpCmdCmd)
            {
                /*
                foreach (hCtransaction pTr in transList)
                {
                    if (pTr != null)// not the end of the list
                    {
                        tNum = (int)pTr.getTransNum();
                        // get response list
                        rc = updateVarCmdCmd(tNum, pTr.reply, false);
                        if(rc != Common.SUCCESS)
                        {
                            return rc;// we need go no further
                        }
                        rc = updateVarCmdCmd(tNum, pTr.request, true);
                        if (rc != Common.SUCCESS)
                        {
                            return rc;// we need go no further
                        }
                    }
                    // else trans ptr is null, end of transaction list, loop
                }// next transaction
                */
                return rc;// we need go no further
            }

            int holdBaseWgt = 0;

            foreach (hCtransaction pTr in transList)
            {
                if (pTr != null)// not the end of the list
                {
                    tNum = (int)pTr.getTransNum();

                    if (operation == cmdOperationType_t.cmdOpNone)
                    {
                        //LOGIT(CLOG_LOG, L"Command %d has NO operation type.\n", getCmdNumber());
                    }
                    else
                    {// it's a read || write - get both lists to get weight, use just one	

                        // get response list
                        if (operation == cmdOperationType_t.cmdOpRead)
                        {   // highest number wins:: extra data means try not to use it
                            if (getCmdNumber() == 1)
                            {
                                holdBaseWgt = 256;// only use this as a last resort
                            }
                            else
                            {
                                holdBaseWgt = 1024;// deductive weighting
                            }

                            holdBaseWgt -= (pTr.request.Count); //less request var cnt
                                                                // note that indexes should be exempt of this deduction
                            holdBaseWgt -= (pTr.reply.Count - 3);//rc,stat,thisitem

                            rc = updateVarCmdWeight(holdBaseWgt, tNum, pTr.reply);
                            if (rc != Common.SUCCESS/* && !pDev.getPolicy().isPartOfTok*/)
                            {
                                //LOGIT(CERR_LOG, "updateVarCmdWeight failed for read command\n");
                            }
                            // else just continue
                        }
                        else if (operation == cmdOperationType_t.cmdOpWrite)
                        {
                            holdBaseWgt = 1024;// deductive weighting
                            holdBaseWgt -= (pTr.request.Count); //less request var cnt
                                                                // note that indexes should be exempt of this deduction
                            holdBaseWgt -= (pTr.reply.Count - 3);//rc,stat,thisitem

                            rc = updateVarCmdWeight(holdBaseWgt, tNum, pTr.request);
                            if (rc != Common.SUCCESS)
                            {
                                //LOGIT(CERR_LOG, "updateVarCmdWeight failed for write command\n");
                            }
                            // else just continue
                        }// end elseif is write
                         // else none OR command-command <cmd 11,40,45,46 - do-not-dispatch>
                    }// endelse - has a command type
                }
                // else trans ptr is null, end of transaction list, loop
            }// next transaction
            return rc;
        }

        hCtransaction getTransactionByNumber(int tNumber)
        {
            return (transList.getTransactionByNumber(tNumber));
        }

        int getTheWgt(ref int transN, ref indexUseList_t useIdx, bool isRd)
        {
            int i, wgt = 0, cn = (int)getCmdNumber();//cn is for debugging
            hCtransaction pTrans = null;
            indexUseList_t idxUse = new indexUseList_t();
            useIdx.Clear();

            if (transN >= 0)// trans# have nothing to do w/ listSz.&& transN < (int)transList.size())
            {// weigh the spec'd transaction
                pTrans = getTransactionByNumber(transN);
                if (pTrans != null)
                {
                    wgt = pTrans.weigh(isRd, ref useIdx);// not a read
                                                         // transN stays valid - return weight
                }
                else
                {
                    //LOGIT(CERR_LOG, "ERROR: transaction list #%d could not be weighed in cmd %d\n", transN, getCmdNumber());
                    transN = -1;
                    wgt = 0;
                }
            }
            else
            {// scan all of 'em
                i = 0;
                foreach (hCtransaction iT in transList)
                {//iT is a ptr 2 a hCtransaction
                    i = iT.weigh(isRd, ref idxUse);
                    if (i > wgt)
                    {
                        wgt = i;
                        transN = (int)iT.getTransNum();
                        useIdx = idxUse;
                    }
                    //idxUse.clear(); // stevev 7jul06 - index from earlier trans being passed w/ later
                    // stevev 5may09 - moved outside of if so it is always clear going
                    //                 into weigh
                }
            }
            return wgt;
        }

        public int getWrWgt(ref int transN, ref indexUseList_t useIdx)
        {
            return (getTheWgt(ref transN, ref useIdx, false));
        }

        public int getRdWgt(ref int transN, ref indexUseList_t useIdx)
        {
            return (getTheWgt(ref transN, ref useIdx, true));
        }

    }

    public class CCmdList : List<CDDLCmd>
    {
#if _INVENSYS
        const int MAX_CMD_SKIP = 0;
#else
        const int MAX_CMD_SKIP = 2;
#endif
        HARTDevice device;

        //int skipCount;// dispatcher skip count
        //int lastBestCmdNum;
        //int lastBestTransN;
        indexUseList_t lastIndexSet;

        public CCmdList(HARTDevice hd)
        {
            device = hd;
            lastIndexSet = new indexUseList_t();
        }

        public CDDLCmd getCmdByNumber(int cmdNumber)// all commands
        {
            CDDLCmd pCRet = null;
            foreach (CDDLCmd cmd in this)
            {//iT is a ptr 2 a ptr 2 a hCcommand
                if (cmd.getCmdNumber() == cmdNumber)
                {
                    pCRet = cmd;
                    break; // out of for loop
                }
            }
            return pCRet;
        }

        public void insertWgtVisitor(hCcmdDispatcher pDispatch)// all commands
        {
            foreach (CDDLCmd iT in this)
            {//iT is a ptr 2 a ptr 2 a hCcommand
                iT.insertWgtVisitor(pDispatch);
            }
        }

        //public int getRead(ref CDDLCmd pCmd, ref int transNum, ref indexUseList_t useIdx, cmdDescList_t pList)

    }

    public class hIndexUse_t
    {
        public uint indexSymID;
        public int indexWrtStatus; // stevev added 5aug08 0 normal/1 user writen/2 was info
        //INSTANCE_DATA_STATE_T indexDataState;
        public CValueVarient indexDispValue;
        public CValueVarient indexRealValue;

        public uint devIdxSymID;
        //public uint devVarRequired;

        public hIndexUse_t()
        {
            indexDispValue = new CValueVarient();
            indexRealValue = new CValueVarient();
        }

        public bool isNOTempty()
        {
            return ((indexSymID > 0) || (devIdxSymID > 0));
        }
    }

    public class cmdDescList_t : List<hCcommandDescriptor>
    {

    }

    public class indexUseList_t : List<hIndexUse_t>
    {

    }

    /*
    public enum dispatchCondition_t // bit-enum so we can add different conditions later
    {
        dc_Nothing,     /* 0 /
        dc_Reads,       /* 1 /// Cleared at first weigh with nothing to do, set at something to do
    }
    */

    public class hCcmdDispatcher
    {
        const int WGT_CONSTANT_DATAITEM = 5;    /* we prefer constants  */
        const int WGT_PER_DATAITEM = (-1);/* the longer the worse */
        const int WGT_FOR_STALE = 50;   /* higher than most     */
        const int WGT_FOR_UNINIT = 5;       /* pretty low           */
        const int WGT_FOR_INVALID = 20;     /* has tried and failed */
        const int WGT_FOR_WRITE = 5000;/* puts it at pretty high priority */
        const int WGT_FOR_CMD_MATCH = 10;

        const int PREAMBLE_NUM = 5;

        List<int> vBlockingCmdStack;
        CCmdList pCmdList;
        CVarList pVarList;
        //hCTransferChannel port;
        HARTDevice deviceParent;
        CDDLVar pByteCntVar;
        uint idleRunCounter;
        bool appCommEnabled;
        //dispatchCondition_t dispatch_Status;

        ThreadUpdate tusend = new ThreadUpdate();

        public hCcmdDispatcher()
        {
            //port = new hCTransferChannel();
            vBlockingCmdStack = new List<int>();
            //pCmdList = new CCmdList();
            //pVarList = new CVarList();
        }

        public hCcmdDispatcher(HARTDevice device)
        {
            //port = new hCTransferChannel();
            vBlockingCmdStack = new List<int>();
            deviceParent = device;
            //pCmdList = new CCmdList();
            //pVarList = new CVarList();
        }

        public void initDispatch(HARTDevice device, bool bManCmd = false)
        {
            deviceParent = device;

            if (device != null)
            {
                //pVarList = (CVarList)deviceParent.getListPtr(itemType_t.iT_Variable);
                pCmdList = (CCmdList)deviceParent.getListPtr(itemType_t.iT_Command);//<hCcommand*>
                if (pCmdList != null)
                {
                    pCmdList.insertWgtVisitor(this);// into all transactions
                }
                else
                {
                    //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: no command list for the dispatcher to use.\n");
                    //rc = FAILURE;
                }

                pVarList = (CVarList)deviceParent.getListPtr(itemType_t.iT_Variable);
                if (pVarList == null)
                {
                    //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: no variable list for the dispatcher to use.\n");
                    //rc = FAILURE;
                }
            }
            else
            {
                //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: Device not found in dispatcher's initDispatch.\n");
                pCmdList = null;
                //rc = FAILURE;
            }

            /*
			if (rc == SUCCESS)
			{
				// generate background task to handle the async response codes asyncronously
				if (pAsyncRespCdEvent)
				{
					RegisterWaitForSingleObject(&hRegisteredAsyncTask,
									pAsyncRespCdEvent.getEventHandle(), gfWaitAndTimerAsyncCallback,
									(void*)this, INFINITE, WT_EXECUTEDEFAULT
							);
				}

				// generate background task to handle the command weighing while nothing else happening
				if (pIdleTriggerEvent)
				{
					RegisterWaitForSingleObject(&hRegisteredIdleTask,
									pIdleTriggerEvent.getEventHandle(), gfWaitAndTimerIdleCallback,
									(void*)this, INFINITE, WT_EXECUTEDEFAULT | WT_NORM_LESS_TWO
							//dbg			(void*)this,     INFINITE,     WT_EXECUTEDEFAULT | WT_NORMAL_PRI
							);
				}

			}
			*/
            /*
            Thread thread = new Thread(new ThreadStart(commIdleTask));
            thread.Start();
            */

            tusend.MainThread = new ThreadUpdate.InvokeSendThead(deviceParent.parentform.UpdataFormReq);
            tusend.RcvThread = new ThreadUpdate.InvokeRcvThead(deviceParent.parentform.procRcvData);
            tusend.VarThread = new ThreadUpdate.InvokeVarThead(deviceParent.parentform.UpdateData);
            tusend.LogThread = new ThreadUpdate.InvokeLog(deviceParent.parentform.Log);

            if (!bManCmd)
            {
                tusend.ReadThread = new Thread(new ThreadStart(commIdleTask));
                tusend.ReadThread.Start();
            }

            deviceParent.parentform.setThread(tusend);

            CDDLBase pItm = new CDDLBase();    // fill special symbol for testing (if it exists in the DD)
            if (deviceParent.getItembyID(1022, ref pItm) && pItm.IsVariable())
            {
                pByteCntVar = (CDDLVar)pItm;
            }// else leave it null

        }

        /*
        dispatchCondition_t getCond()
        {
            return dispatch_Status;
        }

        void setCond(dispatchCondition_t x)
        {
            dispatch_Status = (dispatchCondition_t)((int)dispatch_Status | (int)x);
        }

        void clrCond(dispatchCondition_t y)
        {
            dispatch_Status = (dispatchCondition_t)((int)dispatch_Status & (int)(~y));
        }
        */
        void commWriteTask()
        {
            bool bwrite = false;
            bwrite = ServiceWrites();
            //if (bwrite)
            {
                tusend.varThreadRes = deviceParent.parentform.BeginInvoke(tusend.VarThread);
            }
            tusend.ReadThread = new Thread(new ThreadStart(commIdleTask));
            tusend.ReadThread.Start();
            //Thread.Sleep(200);
        }

        public void startWrite()
        {
            tusend.WriteThread = new Thread(new ThreadStart(commWriteTask));
            tusend.WriteThread.Start();
            deviceParent.parentform.setThread(tusend);
        }

        public void SetAutoUpdate(bool bUp)
        {
            appCommEnabled = bUp;
        }

        public bool ServiceWrites()
        {
            // Anil Merger Activity From FDM to HCF November 2005 -End
            int rc = Common.SUCCESS;
            CDDLCmd pCmd = new CDDLCmd();
            int transNum = -1;// default to seach 'em all
            bool retVal = true;
            indexUseList_t localUseIdx = new indexUseList_t();
            cmdDescList_t pNeedy = new cmdDescList_t();

            if (pCmdList == null)
            {
                //LOGIF(LOGP_NOT_TOK)(CERR_LOG,"ERROR: No command list to service.\n");
                retVal = false;
            }
            else
            {
                pVarList = (CVarList)deviceParent.parentform.getVarListToWrite();
                if (pVarList != null && pVarList.Count != 0)
                {
                    pVarList.getNeededWrit(ref pNeedy);
                }
                else
                {
                    //retVal = false;
                    pNeedy = null; // do it the original way
                }
                // if index protection is found to be needed here - put it in calcWeight()
                //rc = pCmdList.getBestRead(ref pCmd, ref transNum, ref localUseIdx, pNeedy);

                if (rc != Common.SUCCESS || pCmd == null || pNeedy == null || pNeedy.Count == 0)// added a4aug06 try and stop shutdown crash
                {//		if No command needs sending 
                    retVal = false;// nothing needs work
                                   // set our status
                                   //clrCond(dispatchCondition_t.dc_Reads);
                }
                else
                {//	call SvcSend//cmd*/trans# the  non-blocking send -- Async Rd/Wr MUST enter here
                    //setCond(dispatchCondition_t.dc_Reads);
                    foreach (hCcommandDescriptor cds in pNeedy)
                    {

                        pCmd = pCmdList.getCmdByNumber(cds.cmdNumber);
                        transNum = cds.transNumb;
                        rc = Send(pCmd, transNum, true, blockingType_t.bt_NOTBLOCKING, true, cmdOriginator_t.co_ASYNC_READ, null);

                        // stevev - replace with send's counting mutex::
                        // rc = SvcSend(pCmd, transNum,localUseIdx);
                        if (rc != Common.SUCCESS || pCmd == null/* || !appCommEnabled*/)
                        {
                            return false; // break out of the loop
                        }
                    }
                    /*
                    if (pNeedy.Count > 0)
                    {
                        foreach (CDDLVar wvar in pVarList)
                        {
                            wvar.setWriteStatus(writestatus_t.writting);
                        }
                    }
                    */
                    /*
                    else if (ourPolicy.incrementalRead != 0)// incremental desired
                    {// return after one
                        return true;
                    }
                    else // non- incremental...do 'em all till we drop
                    {//	loop to scan for best write command
                     //	CVarList* pVarList = (CVarList*) devPtr().getListPtr(iT_Variable);
                     //	if ( pVarList != null && pVarList.isChanged())
                        if (pVarList != null && pVarList.isChanged())
                        {
                            ServiceWrites();
                        }
                    }// then check for reads again
                    */
                }
            }
            return retVal;
        }

        public bool ServiceReads()
        {
            // Anil Merger Activity From FDM to HCF November 2005 -End
            int rc = Common.SUCCESS;
            CDDLCmd pCmd = new CDDLCmd();
            int transNum = -1;// default to seach 'em all
            bool retVal = true;
            indexUseList_t localUseIdx = new indexUseList_t();
            cmdDescList_t pNeedy = new cmdDescList_t();

            if (pCmdList == null)
            {
                //LOGIF(LOGP_NOT_TOK)(CERR_LOG,"ERROR: No command list to service.\n");
                retVal = false;
            }
            else if (!appCommEnabled)
            {
                //LOGIT(CLOG_LOG, "WARNING: servicing reads with no appcomm.\n");
                retVal = false;
            }
            else
            {
                pVarList = (CVarList)deviceParent.parentform.getVarListToRefresh();
                if (pVarList != null && pVarList.Count != 0)
                {
                    pVarList.getNeededRead(ref pNeedy);
                }
                else
                {
                    //retVal = false;
                    pNeedy = null; // do it the original way
                }
                // if index protection is found to be needed here - put it in calcWeight()
                //rc = pCmdList.getBestRead(ref pCmd, ref transNum, ref localUseIdx, pNeedy);

                if (rc != Common.SUCCESS || pCmd == null || !appCommEnabled || pNeedy == null || pNeedy.Count == 0)// added a4aug06 try and stop shutdown crash
                {//		if No command needs sending 
                    retVal = false;// nothing needs work
                                   // set our status
                    //clrCond(dispatchCondition_t.dc_Reads);
                }
                else
                {//	call SvcSend//cmd*/trans# the  non-blocking send -- Async Rd/Wr MUST enter here
                    //setCond(dispatchCondition_t.dc_Reads);
                    foreach (hCcommandDescriptor cds in pNeedy)
                    {

                        pCmd = pCmdList.getCmdByNumber(cds.cmdNumber);
                        transNum = cds.transNumb;
                        rc = Send(pCmd, transNum, true, blockingType_t.bt_NOTBLOCKING, false, cmdOriginator_t.co_ASYNC_WRITE, null);

                        // stevev - replace with send's counting mutex::
                        // rc = SvcSend(pCmd, transNum,localUseIdx);
                        if (rc != Common.SUCCESS || pCmd == null || !appCommEnabled)
                        {
                            return false; // break out of the loop
                        }
                    }
                    /*
                    else if (ourPolicy.incrementalRead != 0)// incremental desired
                    {// return after one
                        return true;
                    }
                    else // non- incremental...do 'em all till we drop
                    {//	loop to scan for best write command
                     //	CVarList* pVarList = (CVarList*) devPtr().getListPtr(iT_Variable);
                     //	if ( pVarList != null && pVarList.isChanged())
                        if (pVarList != null && pVarList.isChanged())
                        {
                            ServiceWrites();
                        }
                    }// then check for reads again
                    */
                }
            }
            return retVal;
        }

        void commIdleTask()
        {
            bool bread = false;
            //	if ( (! isTimerExpired)      &&  dispatchEnabled && pCommInterface != null  )
            //   appCommEnabled does comm status & autoupdateDisabled
            /*if (appCommEnabled() && dispatchEnabled && pCommInterface != null && isTimerExpired != evt_lastcall)*/
            while (true)
            {
                if (/*port != null && */appCommEnabled)
                {// not in shutdown mode
                    if ((idleRunCounter % 2) == 0)
                    {
                        //bread = ServiceWrites();
                        //bread = false;
                    }
                    else if ((idleRunCounter % 1) == 0 && deviceParent.parentform.bafterSel)
                    {
                        bread = ServiceReads();
                    }
                    idleRunCounter++;
                }
                //deviceParent.parentform.UpdataData();
                if(bread)
                {
                    tusend.varThreadRes = deviceParent.parentform.BeginInvoke(tusend.VarThread);
                }
                //Thread.Sleep(200);
            }
        }

        /*
        public hCcmdDispatcher(System.IO.Ports.SerialPort serport)
        {
            port = new hCTransferChannel();
            port.pPortSupport = serport;
            vBlockingCmdStack = new List<int>();
            //pCmdList = new List<CDDLCmd>();
            //pVarList = new List<CDDLVar>();
        }

        public hCTransferChannel getPort()
        {
            return port;
        }
        */

        public int Send(CDDLCmd pCmd, int transNum, byte[] data, byte len, StreamWriter sw)//Vibhor 250304: Added the last parameter);	
        {
            byte ucPDULen = 0;
            byte[] ucSendData = new byte[255];

            for (int i = 0; i < PREAMBLE_NUM; i++)
            {
                ucSendData[i] = 0xff;
            }

            ucPDULen += PREAMBLE_NUM;

            if (pCmd.getCmdNumber() == 0)
            {
                ucSendData[ucPDULen] = 0x02;          // 定界符
                ucPDULen++;
                ucSendData[ucPDULen] = (byte)(deviceParent.sAddr | deviceParent.parentform.PRIMARY_HOST);
                ucPDULen++;
            }
            else
            {
                ucSendData[ucPDULen++] = 0x82;          // 定界符

                if (deviceParent.HartVer() >= 5)
                {
                    ucSendData[ucPDULen++] = (byte)(deviceParent.sLongAddr.ucMfgIDHostAddrBurst | deviceParent.parentform.PRIMARY_HOST);
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDevType;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Msb;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Mib;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Lsb;
                }
            }

            PackData actdata = pCmd.getRequestData(transNum);

            if (len != actdata.Count)
            {
                return Common.FAILURE;
            }

            ucSendData[ucPDULen++] = (byte)pCmd.getCmdNumber();             // 命令

            ucSendData[ucPDULen++] = (byte)len;             // 字节数
            for (int i = 0; i < len; i++)
            {
                ucSendData[ucPDULen++] = data[i];
            }
            ucSendData[ucPDULen++] = Common.CheckSums(ucSendData, ucPDULen, PREAMBLE_NUM); // 校验位

            while (deviceParent.parentform.gsRspInfo.ucSendState == 1) ;
            if (deviceParent.parentform.USART_Send(ucSendData, ucPDULen, 0) == returncode.eOk)
            {
                tusend.ucCmdSent = (byte)pCmd.getCmdNumber();
                deviceParent.parentform.BeginInvoke(tusend.LogThread, String.Format(Resource.CmdSentOk, tusend.ucCmdSent, transNum), LogType.Ok);
                tusend.ucTranNumSent = (byte)transNum;

                object[] param = new Object[] { ucSendData, ucPDULen, (byte)pCmd.getCmdNumber() };

                tusend.mainThreadRes = deviceParent.parentform.BeginInvoke(tusend.MainThread, param);

                returncode creply = deviceParent.parentform.ReData(sw);

                {
                    tusend.rcvThreadRes = deviceParent.parentform.BeginInvoke(tusend.RcvThread, creply, transNum, pCmd.getCmdNumber(), pCmd.getOperation());
                }
                //else
                {
                    //deviceParent.parentform.gsRspInfo.ucSendState = 0;
                }
            }

            else
            {
                return Common.FAILURE;

            }
            return Common.SUCCESS;
        }

        public int SendCmd(CDDLCmd pCmd, int transNum, PackData data = null, StreamWriter sw = null)
        {
            byte ucPDULen = 0;
            byte[] ucSendData = new byte[255];

            for (int i = 0; i < PREAMBLE_NUM; i++)
            {
                ucSendData[i] = 0xff;
            }

            ucPDULen += PREAMBLE_NUM;

            if (pCmd.getCmdNumber() == 0)
            {
                //gsSendInfo.ucFrameType = SHORT_FRAME;
                //gsSendInfo.ucPollingAdde = ucPollingAddr;
                ucSendData[ucPDULen] = 0x02;          // 定界符
                ucPDULen++;
                ucSendData[ucPDULen] = (byte)(deviceParent.sAddr | deviceParent.parentform.PRIMARY_HOST);// MASTER_ADDRESS;    // 短地址
                ucPDULen++;
            }
            else
            {
                //gsSendInfo.ucFrameType = LONG_FRAME;
                ucSendData[ucPDULen++] = 0x82;          // 定界符
                //memcpy(&ucSendData[ucPDULen], deviceParent.sLongAddr, 5);    // 长地址

                if (deviceParent.HartVer() >= 5)
                {
                    ucSendData[ucPDULen++] = (byte)(deviceParent.sLongAddr.ucMfgIDHostAddrBurst | deviceParent.parentform.PRIMARY_HOST);
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDevType;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Msb;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Mib;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Lsb;
                }
                //memcpy(&gsSendInfo.sLongAddr, deviceParent.sLongAddr, 5);
            }

            //PackData data = pCmd.getRequestData(transNum, bIsWriteImd);

            if(data == null)
            {
                data = pCmd.getRequestData(transNum, false);
            }

            ucSendData[ucPDULen++] = (byte)pCmd.getCmdNumber();             // 命令

            ucSendData[ucPDULen++] = (byte)data.Count;             // 字节数
            /*
            if (ucLen != 0 && pData != NULL)             // 数据段
            {
                memcpy(&ucSendData[ucPDULen], pData, ucLen);
                ucPDULen += ucLen;
            }
            */
            for (int i = 0; i < data.Count; i++)
            {
                ucSendData[ucPDULen++] = data[i];
            }
            ucSendData[ucPDULen++] = Common.CheckSums(ucSendData, ucPDULen, PREAMBLE_NUM); // 校验位
            //ucSendData[0] = ucPDULen - 1;               // 整个数据帧长度，即发送长度

            if (deviceParent.parentform.USART_Send(ucSendData, ucPDULen, 0, sw) == returncode.eOk)
            {
                return Common.SUCCESS;
            }
            else
            {
                return Common.FAILURE;
            }

        }

        public int Send(CDDLCmd pCmd, int transNum, bool actionsEn, blockingType_t isBlking,
                /*hCevent* peDone, ref indexUseList_t rIdxUse,*/ bool bIsWriteImd = false,
                cmdOriginator_t cmdOriginator = cmdOriginator_t.co_UNDEFINED, StreamWriter sw = null)//Vibhor 250304: Added the last parameter);	
        {
            byte ucPDULen = 0;
            byte[] ucSendData = new byte[255];

            for (int i = 0; i < PREAMBLE_NUM; i++)
            {
                ucSendData[i] = 0xff;
            }

            ucPDULen += PREAMBLE_NUM;

            if (pCmd.getCmdNumber() == 0)
            {
                //gsSendInfo.ucFrameType = SHORT_FRAME;
                //gsSendInfo.ucPollingAdde = ucPollingAddr;
                ucSendData[ucPDULen] = 0x02;          // 定界符
                ucPDULen++;
                ucSendData[ucPDULen] = (byte)(deviceParent.sAddr | deviceParent.parentform.PRIMARY_HOST);// MASTER_ADDRESS;    // 短地址
                ucPDULen++;
            }
            else
            {
                //gsSendInfo.ucFrameType = LONG_FRAME;
                ucSendData[ucPDULen++] = 0x82;          // 定界符
                //memcpy(&ucSendData[ucPDULen], deviceParent.sLongAddr, 5);    // 长地址

                if(deviceParent.HartVer() >= 5)
                {
                    ucSendData[ucPDULen++] = (byte)(deviceParent.sLongAddr.ucMfgIDHostAddrBurst | deviceParent.parentform.PRIMARY_HOST);
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDevType;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Msb;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Mib;
                    ucSendData[ucPDULen++] = deviceParent.sLongAddr.ucDeviceID_Lsb;
                }
                //memcpy(&gsSendInfo.sLongAddr, deviceParent.sLongAddr, 5);
            }

            PackData data = pCmd.getRequestData(transNum, bIsWriteImd);

            ucSendData[ucPDULen++] = (byte)pCmd.getCmdNumber();             // 命令

            ucSendData[ucPDULen++] = (byte)data.Count;             // 字节数
            /*
            if (ucLen != 0 && pData != NULL)             // 数据段
            {
                memcpy(&ucSendData[ucPDULen], pData, ucLen);
                ucPDULen += ucLen;
            }
            */
            for (int i = 0; i < data.Count; i++)
            {
                ucSendData[ucPDULen++] = data[i];
            }
            ucSendData[ucPDULen++] = Common.CheckSums(ucSendData, ucPDULen, PREAMBLE_NUM); // 校验位
            //ucSendData[0] = ucPDULen - 1;               // 整个数据帧长度，即发送长度

            while (deviceParent.parentform.gsRspInfo.ucSendState == 1) ;
            if (deviceParent.parentform.USART_Send(ucSendData, ucPDULen, 0) == returncode.eOk)
            {
                tusend.ucCmdSent = (byte)pCmd.getCmdNumber();
                deviceParent.parentform.BeginInvoke(tusend.LogThread, String.Format(Resource.CmdSentOk, tusend.ucCmdSent, transNum), LogType.Ok);
                tusend.ucTranNumSent = (byte)transNum;

                object[] param = new Object[] { ucSendData, ucPDULen, (byte)pCmd.getCmdNumber() };

                //tusend.MainThread(ucSendData, ucPDULen, (byte)pCmd.getCmdNumber());

                tusend.mainThreadRes = deviceParent.parentform.BeginInvoke(tusend.MainThread, param);

                returncode creply = deviceParent.parentform.ReData(sw);

                //if (creply == comcode.eOk)
                {
                    tusend.rcvThreadRes = deviceParent.parentform.BeginInvoke(tusend.RcvThread, creply, transNum, pCmd.getCmdNumber(), pCmd.getOperation());
                }
                //else
                {
                    //deviceParent.parentform.gsRspInfo.ucSendState = 0;
                }
            }

            else
            {
                return Common.FAILURE;

            }
            return Common.SUCCESS;
        }

        public int calcWeightOptimized(hCtransaction pThisTrans, ref indexUseList_t retIdx, bool isRead)
        {
            int rc = Common.FAILURE;
            List<DATA_ITEM> RslvdRqstItemList = new List<DATA_ITEM>();
            List<DATA_ITEM> RslvdRplyItemList = new List<DATA_ITEM>();
            CDDLVar pDataItemVar;
            int weight = 0;
            int idxWgt = 0;
            int reqstListSize = 0,
                 reqstlistValid = 0;
            int replyListSize = 0,
                 replyListValid = 0;

            // 7dec09 - moved from below to be usable throughout
            int cmdNum = (int)pThisTrans.getCmdPtr().getCmdNumber();
            int trnNum = (int)pThisTrans.getTransNum();

            try
            {
                if (pThisTrans == null || !appCommEnabled)// stevev 08apr10 bug 3034 subsequent
                {
                    return -1; // very little weight
                }
                rc = pThisTrans.getReqstDIlist(ref RslvdRqstItemList);// this merely resolves the cond list


                // extract the index&info variables from the request packet
                //int idxID = 0;
                //hCIndexValue2Variable pIV2V = null;
                //IdxVal2VarList_t transIdxNVarList;

                retIdx.Clear(); // we do it all, we are not additive

                foreach (DATA_ITEM pDI in RslvdRqstItemList)
                {// for each index variable in the request transaction				
                    pDataItemVar = Common.getVarPtr(deviceParent, pDI);
                    if (pDataItemVar != null && pDataItemVar.IsValid())
                    {
                        if ((pDI.flags & (int)(cmdDataItemFlags_t.cmdDataItemFlg_indexNinfo))
                        == (int)cmdDataItemFlags_t.cmdDataItemFlg_indexNinfo)
                        {// we only deal with info & index
                            //idxID = (int)pDataItemVar.getID();
                            hIndexUse_t ht = new hIndexUse_t();
                            ht.indexSymID = pDataItemVar.getID();

                            retIdx.Add(ht);
                            /*
                                pIV2V = pThisTrans.getMap4Index(idxID);
                                // we have a list of values & the variables each resolves
                                if (pIV2V == null)
                                {// error condition - not found in the map
                                    continue;// next index
                                }// else - process	

                                transIdxNVarList.Add(pIV2V);
                            */
                        }// else it is not an index, we don't consider it here
                    }// else - item is null (possible constant) or is invalid, do next dataitem
                }// next dataitem 

                // from that list of index&infos, get the best combination of indexes.
                //rc = findBestIdxList(retIdx, transIdxNVarList, isRead, cmdNum, trnNum);
                hIndexUse_t localIdxUse = new hIndexUse_t();//, pIdxUse = null;
                INSTANCE_DATA_STATE_T tDS;

                //get index mutex,
                //see line 2459 for re-resolution sequence
                //-- its like we're redoing the whole thing under the index mutex...???
                // with the best index(es) selected

                indexUseList_t requestIndexList = new indexUseList_t();
                //	hCdataitemList  RslvdRqstItemList(devHndl());
                //	hCdataitemList  RslvdRplyItemList(devHndl());

                hCcommandDescriptor cmdInfo;

                // we have to use a local flag to tell if we have acquired the indexMutex or not
                // The MEE can startup after we get the mutex, disable appComm and then pend on the mutex
                // we continue to the end but the appComm is disabled down there so we don't release the mutex
                // Setting the flag when we get it and testing the flag at the end stops this mutex loss
                bool haveIdxMutex = false;

                if (retIdx.Count > 0 && appCommEnabled)// stevev 08apr10 bug 3034 subsequent error
                {
                    CDDLBase pIB = new CDDLBase();
                    CDDLVar pII;
                    CValueVarient vv;
                    //localIdxUse.Clear();// we're about to reuse it

                    // store  the current value, set a new value
                    foreach (hIndexUse_t pIdxUse in retIdx)
                    {
                        if (deviceParent.getItembyID(pIdxUse.indexSymID, ref pIB))
                        {
                            localIdxUse.indexSymID = pIdxUse.indexSymID;
                            pII = (CDDLVar)pIB;
                            localIdxUse.indexDispValue = pII.getDispValue();
                            localIdxUse.indexWrtStatus = (int)pII.getWriteStatus();
                            pII.setWriteStatus(0);// set to NOT user written to avoid screen updations
                            requestIndexList.Add(localIdxUse);//entry values

                            if (pII.IsLocal())
                            {
                                vv = pIdxUse.indexDispValue;
                                pII.setDispValue(vv);    // set one to the selected value
                                pII.ApplyIt();                 // copy it to the other half
                                pII.markItemState(INSTANCE_DATA_STATE_T.IDS_CACHED); // force it good
                            }
                            else
                            {
                                //LOGIT(CLOG_LOG, " Info & Index Var NOT Set (is NOT Local) 0x%04x ", pIdxUse.indexSymID);
                            }
                            //localIdxUse.Clear();
                        }// not much we can do without an item pointer
                    }// next return index
                }
                else if (retIdx.Count > 0)
                {// we have indexes we need to set but appComm is disabled and we can't..no use continuing here
                    return -1;
                }

                // always re-resolve list
                //RslvdRplyItemList.clear();
                if (isRead)
                {
                    rc = pThisTrans.getReplyDIlist(ref RslvdRplyItemList);
                }
                else // isWrite
                {
                    rc = pThisTrans.getReqstDIlist(ref RslvdRplyItemList);
                }
                if (rc != Common.SUCCESS)
                {
                    //LOGIT(CLOG_LOG, "Indexed List did not re-resolve.\n");
                }

                reqstlistValid =        // we'll reduce by invalids...just in case it isRead
                reqstListSize = RslvdRplyItemList.Count;// stevev 2feb09

                // weigh the reply 4 read or request 4 write
                foreach (DATA_ITEM iT in RslvdRplyItemList)
                // stevev 08apr10 bug 3034 subsequent error, allow exit
                {// iT is a ptr 2 a hCdataitem	   
                    pDataItemVar = Common.getVarPtr(deviceParent, iT);

                    if (pDataItemVar != null && isRead && (!pDataItemVar.IsValid()))// isRead  AND Not Valid
                    {// this is the reply packet with an invalid item
                        reqstlistValid--;
                    }

                    /* stevev 12mar07 - we need to deal with the unsent write due to validity*/
                    if (pDataItemVar != null && (!isRead) && (!pDataItemVar.IsValid())) //isWrite AND Not Valid
                                                                                        //&& (compatability() != dm_275compatible))// and we ain't being lenient
                    {// this won't be sent
                        weight -= 50000;
                        continue;
                    }
                    /** end 12mar07 **/
                    if ((iT.flags & (int)cmdDataItemFlags_t.cmdDataItemFlg_Info) != 0 ||   // it is an info data Item
                          pDataItemVar == null ||
                        !pDataItemVar.IsValid() ||// stevev 23mar09 bug2596..added a couple )
                        pDataItemVar.getID() == Common.RESPONSECODE_SYMID ||
                        pDataItemVar.getID() == Common.DEVICESTATUS_SYMID)// stevev - skip weighing these
                    {                                           // includes index and info weighed above
                        continue;// skip it (add zero weight)
                    }

                    int tmpWgt = 0;

                    tDS = pDataItemVar.getDataState();

                    if (pDataItemVar.getDataStatus() == 0)// weigh if not zero < when reading only
                    {// do not weigh this item -  it has been marked as a problem variable
                    }
                    else if (isRead && tDS == INSTANCE_DATA_STATE_T.IDS_STALE)
                    {
                        tmpWgt = WGT_FOR_STALE;
                    }
                    else if (isRead && tDS == INSTANCE_DATA_STATE_T.IDS_UNINITIALIZED)// && !isDownloadMode())//downloadMode described
                                                                                      //                                                              at the top of this file
                    {
                        tmpWgt = WGT_FOR_UNINIT;
                    }
                    else if (isRead && tDS == INSTANCE_DATA_STATE_T.IDS_INVALID)// && !isDownloadMode())
                    {
                        tmpWgt = WGT_FOR_INVALID;
                    }
                    else if ((!isRead) && tDS == INSTANCE_DATA_STATE_T.IDS_NEEDS_WRITE)
                    {
                        tmpWgt = WGT_FOR_WRITE;
                    }
                    else
                    {
                        tmpWgt = 0;
                    }
                    int qcnt = (pDataItemVar.getQueryCnt() + 1);
                    tmpWgt *= (qcnt * qcnt);

                    if (tmpWgt != 0)
                    {
                        hCcommandDescriptor thisCmdDesc;
                        if (isRead)
                        {
                            thisCmdDesc = pDataItemVar.getRdCmd();
                            cmdInfo = pDataItemVar.getRdCmd(cmdNum);
                        }
                        else
                        {
                            thisCmdDesc = pDataItemVar.getWrCmd();
                            cmdInfo = pDataItemVar.getWrCmd(cmdNum);
                        }
                        if (cmdInfo.cmdNumber < 0xffff)
                        {
                            weight += tmpWgt;// will resolve a variable
                        }// else invalid cmd number

                        if (cmdInfo.cmdNumber == thisCmdDesc.cmdNumber)
                        {
                            weight += WGT_FOR_CMD_MATCH;// skew toward selected commands
                        }
                    }//no weight so far, we shouldn't add more
                }// next packet var
                if (weight > 0) // no reason to give weight to a weightless command
                {
                    weight += idxWgt; // mess with the request weight ( zero on write cmd )
                }


                /* stevev 21oct09 - this needs to be done while the indexes are still in place        */
                /* stevev  2feb09 - handle the invalid rules for the reply packet (request done above)*/
                if (!isRead && appCommEnabled) // we have to deal with the isWrite reply
                {                // stevev 08apr10 bug 3034 subsequent error, allow exit
                    //RslvdRqstItemList.clear(); // J.U. 28.04.11
                    rc = pThisTrans.getReplyDIlist(ref RslvdRqstItemList);// put reply into an empty list
                    replyListValid =        // we'll reduce by invalids...just in case it isRead
                            replyListSize = RslvdRqstItemList.Count;// stevev 2feb09

                    foreach (DATA_ITEM iT in RslvdRqstItemList)
                    {//  reduce by each invalid value
                        pDataItemVar = Common.getVarPtr(deviceParent, iT);
                        if (pDataItemVar != null && !pDataItemVar.IsValid())
                        {
                            replyListValid--;
                        }
                    }
                }
                //else the isRead reply is done

                /////  if (retIdx.size() > 0 && appCommEnabled())// stevev 08apr10 bug 3034 subsequent error
                if (retIdx.Count > 0 && haveIdxMutex)// stevev 23sep10 stop mee multitasking from losing index
                {
                    CDDLBase pIB = new CDDLBase();
                    CDDLVar pII;
                    CValueVarient vv;
                    // restore  the current value
                    foreach (hIndexUse_t itRet in requestIndexList)
                    {
                        if (deviceParent.getItembyID(itRet.indexSymID, ref pIB))
                        {
                            pII = (CDDLVar)pIB;
                            if (pII.IsLocal())
                            {
                                //LOGIT(CLOG_LOG," Idx 0x%04x to %d ",itRet.indexSymID,itRet.indexDispValue);
                                vv = itRet.indexDispValue;
                                pII.setDispValue(vv);    // set one
                                pII.ApplyIt();                 // copy it to the other
                                pII.markItemState(INSTANCE_DATA_STATE_T.IDS_CACHED); // force it good
                                pII.setWriteStatus(itRet.indexWrtStatus);// put it back like we found it
                            }
                            //else - NOT-Local...was logged when we stored it, don't do it again
                        }// not much we can do
                    }
                    /******INDEX MUTEX****/
                }
                else
                {
                }
                requestIndexList.Clear();


                // the reply rule is: if all in the reply packet are invalid, the command is not sent
                // but response code and device status don't count for this.  I assume that 100% of
                // tokenized reply packets have response-code & device-status variables.
                if ((!isRead && replyListValid <= 2 && replyListSize > 2) || //write reply pkt check 
                    (isRead && reqstlistValid <= 2 && reqstListSize > 2))  // read reply pkt check
                {// none valid & not a command-command
                 // invalid packet
                    RslvdRplyItemList.Clear();// in case some got filled
                    RslvdRqstItemList.Clear();
                    return -1;  // abort execution with very low weight
                }
                /* stevev end 2feb09 **/

                RslvdRplyItemList.Clear();// before destruction
                RslvdRqstItemList.Clear();

                return weight;


                /************************************************************/


            }// end try
            catch (Exception ex)
            {
                string s = ex.Message;
                //LOGIT(CERR_LOG | CLOG_LOG, "Calc weight optimized inside catch(...)\n");
                weight = WGT_FOR_STALE;
            }// end catch
            return weight;
        }
    }
}
