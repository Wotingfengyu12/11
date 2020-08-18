using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace FieldIot.HARTDD
{
    public enum NUA_t
    {
        NO_change,  /* 0 :: 18nov05 - no longer a valid input (filtered out) */
        IS_changed, /* 18nov05 - now defined as value changed (not structure */
        STR_changed,/* added 18nov05 - generate a structure changed message  */
        STRT_actPkt,
        END_actPkt,
        STRT_methPkt,/* added 24jan06 - to restrict notifications from methods*/
        END_methPkt  /*               - ditto                                 */
       , METH_changed /* sjv 6jun07-we must preclude copying in notifyAppVarChange()*/
       , STAT_changed   // J.U. State is changed 17.02.11
       , ABORT_dd    /* stevev 11apr11 - UI has to kill dev obj so we have to tell */
       , DEL_element  /* stevev 23may11 - list element no longer exists             */
    }
    
    /*
    public enum VarType
    {
        TYPE_NONE = 0,
        TYPE_DUMMY,
        TYPE_INTEGER,   // 2
        TYPE_UNSIGN_INT,
        TYPE_FLT,
        TYPE_DBL,       // 5
        TYPE_ENUM,
        TYPE_BITENUM,
        TYPE_INDEX,
        TYPE_STRING,    // packed_ascii
        TYPE_DUMMY02,   //10 is packed ascii in the rest of the program
        TYPE_PASSWORD,
        TYPE_DUMMY03,   // FF type BITSTRING
        TYPE_DATE,
        TYPE_TIMEVALUE = 20,
        TYPE_BIT        // single bit from bit-enum +stevev 17may11
    }
    */

    public enum dState_t
    {
        ds_OffLine,     /* 0 */
        ds_OnLine,      /* 1 */
        ds_OffIniting,  /* 2 - offline & Initializing */
        ds_OnIniting,   /* 3 -  online & Initializing */
        ds_Closing      /* 4 - Set in CloseDevice()   */
    }

    public struct LongAddr
    {
        //设备的地址
        public byte ucMfgIDHostAddrBurst;       //制造商号+master address+burst mode
        public byte ucDevType;      //设备类型
        public byte ucDeviceID_Msb; //标识号高位
        public byte ucDeviceID_Mib; //标识号中位
        public byte ucDeviceID_Lsb; //标识号低位 
    }

    public class HARTDevice
    {
        public byte sAddr;
        public LongAddr sLongAddr;
        public UInt64 devDDkey;
        public Identity_s ddbDeviceID;
        //public fileInfo_s devFileInfo;
        //public string localdir;

        public CVarList Vars;
        public CMenuList Menus;
        public CCollectionList Collections;
        public CItemArrayList ItemArrays;
        public CImageList Images;
        public CChartList Charts;
        public CUnitList Units;
        public CMethodList Methods;
        public CCmdList Cmds;
        public CEditDisplayList EditDisplays;
        public CWaoList Waos;
        public CRefresheList Refreshes;
        public CArrayList Arrays;
        public CFileList Files;
        public CGraphList Graphs;
        public CAxiseList Axises;
        public CWaveformList Waveforms;
        public CSourceList Sources;
        public CListList Lists;
        public CGridList Grids;

        Dictionary<uint, CDDLBase> mapIDitem;
        Dictionary<string, CDDLBase> mapNameitem;

        byte hartVer;

        //public HartDictionary dictionary;

        //public litstringtable literalStringTbl;


        public bool devIsExecuting; // stevev 04apr10 - added to know when we are in a method
        //public dState_t devState;       // stevev 12jul07 made it public


        public MEE pMEE;
        public CMethodSupport m_pMethSupportInterface;
        public hCcmdDispatcher pCmdDispatch;
        devMode_t compatabilityMode;
        int weightPerQuery;

        public mainpage parentform;//parent form

        public HARTDevice(mainpage form)
        {
            parentform = form;
            Vars = new CVarList();
            Menus = new CMenuList();
            Collections = new CCollectionList();
            ItemArrays = new CItemArrayList();
            Images = new CImageList();
            Charts = new CChartList();
            Units = new CUnitList();
            Methods = new CMethodList();
            Cmds = new CCmdList(this);
            EditDisplays = new CEditDisplayList();
            Waos = new CWaoList();
            Arrays = new CArrayList();
            Files = new CFileList();
            Graphs = new CGraphList();
            Axises = new CAxiseList();
            Waveforms = new CWaveformList();
            Sources = new CSourceList();
            Lists = new CListList();
            Grids = new CGridList();
            Refreshes = new CRefresheList();

            mapIDitem = new Dictionary<uint, CDDLBase>();
            mapNameitem = new Dictionary<string, CDDLBase>();

            pMEE = new MEE();
            pMEE.m_pDevice = this;

            sLongAddr = new LongAddr();

            sAddr = 0;
            //dictionary = DDlDevDescription.pGlobalDict;// new HartDictionary();
            //literalStringTbl = new litstringtable();

            devDDkey = 0;
            ddbDeviceID = new Identity_s();
            ddbDeviceID.clear();
            m_pMethSupportInterface = new CMethodSupport();

            compatabilityMode = devMode_t.dm_Standard;
        }

        /*
        public void instantiate(System.IO.Ports.SerialPort port, byte poolingAddr, bool bAuto)
        {
            mapIDitem.Clear();
            mapNameitem.Clear();
            m_pMethSupportInterface = new CMethodSupport();
            
            ConvertItemsToList(Form1.devDesc.ItemsList);
            pCmdDispatch = new hCcmdDispatcher();
            pCmdDispatch.initDispatch(this);
            pCmdDispatch.SetAutoUpdate(bAuto);
            sAddr = poolingAddr;
        }
        */

        public void instantiate(byte poolingAddr, bool bAuto)
        {
            mapIDitem.Clear();
            mapNameitem.Clear();
            m_pMethSupportInterface = new CMethodSupport();

            ConvertItemsToList(mainpage.devDesc.ItemsList);
            pCmdDispatch = new hCcmdDispatcher();
            pCmdDispatch.initDispatch(this);
            pCmdDispatch.SetAutoUpdate(bAuto);
            sAddr = poolingAddr;
            hartVer = 5;
        }

        public void instantiate(byte poolingAddr)
        {
            mapIDitem.Clear();
            mapNameitem.Clear();
            m_pMethSupportInterface = new CMethodSupport();
            ConvertItemsToList(mainpage.devDesc.ItemsList);
            pCmdDispatch = new hCcmdDispatcher(this);
            sAddr = poolingAddr;
            hartVer = 5;
        }

        public int queryWeight()
        { 
            return weightPerQuery; 
        }

        public byte HartVer()
        {
            return hartVer;
        }

        public void setVer(byte v)
        {
            hartVer = v;
        }

        public object getListPtr(itemType_t it)
        {
            switch (it)
            {
                case itemType_t.iT_Variable:
                    return Vars;
                case itemType_t.iT_Command:
                    return Cmds;
                case itemType_t.iT_Menu:
                    return Menus;
                case itemType_t.iT_EditDisplay:
                    return EditDisplays;
                case itemType_t.iT_Method:
                    return Methods;
                case itemType_t.iT_Refresh:/*relation*/
                    return Refreshes;
                case itemType_t.iT_Unit:/*relation*/
                    return Units;
                case itemType_t.iT_WaO:
                    return Waos;
                case itemType_t.iT_ItemArray:
                    return ItemArrays;
                case itemType_t.iT_Collection:
                    return Collections;

                case itemType_t.iT_ReservedOne:/*special*/
                    return Images; // raw images

                case itemType_t.iT_Array:
                    return Arrays;
                case itemType_t.iT_File:
                    return Files;
                case itemType_t.iT_Chart:
                    return Charts;
                case itemType_t.iT_Graph:
                    return Graphs;
                case itemType_t.iT_Axis:
                    return Axises;
                case itemType_t.iT_Waveform:
                    return Waveforms;
                case itemType_t.iT_Source:
                    return Sources;
                case itemType_t.iT_List:
                    return Lists;
                case itemType_t.iT_Grid:
                    return Grids;
                case itemType_t.iT_Image:
                    return Images;  // image items

                case itemType_t.iT_Critical_Item:
                    {
                        List<int> r = null;
                        //////m_criticalItems.getCriticalIDs(ref  r);
                        return r;// be sure to cast this correctly!
                    }

                default:
                    return null;
            }// end switch
        }

        public devMode_t whatCompatability()
        {
            return compatabilityMode;
        }

        public void setCompatability(devMode_t newMode)
        {
            compatabilityMode = newMode;
        }

        public bool getVarbyID(uint itemID, ref CDDLVar var)
        {
            CDDLBase db = null;
            if(getItembyID(itemID, ref db))
            {
                var = (CDDLVar)db;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool getItembyID(uint itemID, ref CDDLBase item)
        {
            return mapIDitem.TryGetValue(itemID, out item);
        }

        public bool getItembyName(string itemname, ref CDDLBase item)
        {
            return mapNameitem.TryGetValue(itemname, out item);
        }

        private string GetLangStr(string input)
        {
            string output = input;
            if (Regex.IsMatch(input, "|.*"))
            {
                string[] mull = input.Split('|');
                for (int i = 1; i < mull.Count(); i++)
                {
                    if (mull[i] == "en")
                    {
                        output = mull[i + 1];
                    }

                    if (mull[i] == Thread.CurrentThread.CurrentUICulture.Name)
                    {
                        output = mull[i + 1];
                        break;
                    }
                }
                return output;
            }
            return output;
        }

        public returncode sendMethodCmd(int commandNumber, int transactionNumber = 0/*, indexUseList_t pI = null*/)// stevev added indexes 29nov11
        {
            returncode rc = returncode.eOk;
            if (pCmdDispatch == null)
            {
                //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: sendMethodCmd with no dispatcher present.");
                rc = returncode.eErr;//DEVICE_NO_DISPATCH | HOST_INTERNALERR;
            }
            else
            {
                CDDLCmd pC = null;// = cmdInstanceLst.getCmdByNumber(commandNumber);
                foreach (CDDLCmd cmd in Cmds)
                {
                    if (commandNumber == cmd.getCmdNumber())
                    {
                        pC = cmd;
                        break;
                    }
                }
                if (pC == null || pC.eType != nitype.nCmd)
                {
                    //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: No command class for cmd# %d.", commandNumber);
                    rc = returncode.eErr;//DEVICE_NO_COMMANDCLASS | HOST_INTERNALERR;
                }
                else
                {
                    //if (pI == null)
                        //pI = new indexUseList_t();// an empty list

                    int ret = pCmdDispatch.Send(pC, transactionNumber,
                        true,               // actions are enabled
                        blockingType_t.bt_ISBLOCKING,      // wait for response complete
                                                           //null,               // no event required
                        //ref pI,                // indexes are up to the caller...good luck
                        false,              // we are not in write-imd	stevev 25may07
                        cmdOriginator_t.co_METHOD, null);         // origin is method			stevev 25may07

                    ///hrc = pCmdDispatch.getHostErrorStatus();
                    /**
                    if (rc == SUCCESS) // a good host status hid the command failure
                    {
                        rc = hrc;
                    }   // found by honeywell 29aug11
                    */
                }
            }

            return rc;// returns host error - hi byte non zero means internal error
        }

        public void doExecute(hCmethodCall oneMethod, ref CValueVarient returnValue, bool isMultipleEntry)
        {
            returncode rc = 0;
            if (pCmdDispatch != null)
            {
                // stevev 13aug13 - this duplicates the activity in PreExecute
                //		pCmdDispatch->disableAppCommRequests();
                //			DEBUGLOG(CLOG_LOG,"Device executeMethod disables appComm\n");
                //?/?/?pCmdDispatch.notifyVarUpdate(0, STRT_methPkt);// stevev 23jan06
            }
            /*
            if (oneMethod.methodID > 0)
            {
                CDDLBase pItemBase = new CDDLBase();
                getItembyID(oneMethod.methodID, ref pItemBase);
                oneMethod.m_pMeth = (CDDLMethod)pItemBase;
            }
            */
            /* end sjv 16may07 */

            if (!isMultipleEntry) // then we are the first entry of a call stack
            {
                devIsExecuting = true;  // stevev 06apr10
                                        //pMEE.m_bSaveValues = false; // clear save value flag
                                        //cacheDispValues();           // cache display values
                                        // @ debug verify there are no displays to push
            }
            /***else - done in method support where the dialogs are controlled
                // push active displays & clear display ptrs
                // verify stack and method count match
            ***/
            //m_pMethSupportInterface.PreExecute(PPARM); // doesn't do much of anything right now
            oneMethod.m_MethState = mState_t.mc_Initing;
            rc = m_pMethSupportInterface.DoMethodSupportExecute(oneMethod, this);// handles pushing & poping the dialogs
            //m_pMethSupportInterface.PostExecute(PPARM);// doesn't do much of anything right now

            if (!isMultipleEntry)
            {
                // @ debug verify there are no displays to pop
                // un-cache display values as required		
                //uncacheDispVals(pMEE.m_bSaveValues);
                //pMEE.m_bSaveValues = false;
                // clear save values flag
                devIsExecuting = false;  // stevev 06apr10
            }
        }

        public void ConvertItemsToList(List<DDlBaseItem> ItemsList)
        {
            foreach (DDlBaseItem ddb in ItemsList)
            {
                switch (ddb.byItemType)
                {
                    case DDlBaseItem.VARIABLE_ITYPE:
                        {
                            CDDLVar var = new CDDLVar();
                            var.pDev = this;
                            var.SetID(ddb.id);
                            var.SetSourceName(ddb.strItemName);

                            if (ddb.id == 0x235e)//fordebug
                            {
                                ;
                            }

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Variable.VAR_CLASS_ID:
                                        var.setClass((int)ddattr.pVals.ullVal);
                                        break;

                                    case DDl6Variable.VAR_HANDLING_ID:
                                        var.setHandling((int)ddattr.pVals.ullVal);
                                        if ((ddattr.pVals.ullVal & Common.WRITE_HANDLING) != 0)
                                        {
                                            var.setWritable(true);
                                            if (((ddattr.pVals.ullVal & Common.READ_HANDLING) != 0))
                                            {
                                                var.setWriteOnly(true);
                                            }
                                            else
                                            {
                                                var.setWriteOnly(false);
                                            }
                                        }
                                        else
                                        {
                                            var.setWritable(false);
                                        }
                                        break;

                                    case DDl6Variable.VAR_UNIT_ID:
                                        var.setunit(ddattr.pVals.strVal.str);
                                        break;

                                    case DDl6Variable.VAR_LABEL_ID:
                                        var.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Variable.VAR_HELP_ID:
                                        var.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Variable.VAR_WIDTHSIZE_ID:
                                        break;

                                    case DDl6Variable.VAR_HEIGHTSIZE_ID:
                                        break;

                                    case DDl6Variable.VAR_VALID_ID:
                                        break;

                                    case DDl6Variable.VAR_PRE_READ_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_POST_READ_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_PRE_WRITE_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_POST_WRITE_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_PRE_EDIT_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_POST_EDIT_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_RESP_CODES_ID:
                                        break;

                                    case DDl6Variable.VAR_TYPE_SIZE_ID:
                                        var.SetVariableType((variableType_t)ddattr.pVals.typeSize.type);
                                        //var.SetVariableType((variableType_t)var.datatype);//////
                                        //var.size = ddattr.pVals.typeSize.size;
                                        var.setVarsize((uint)ddattr.pVals.typeSize.size);
                                        break;

                                    case DDl6Variable.VAR_DISPLAY_ID:
                                        var.SetDispFormat(ddattr.pVals.strVal.str);
                                        break;

                                    case DDl6Variable.VAR_EDIT_ID:
                                        var.SetEditFormat(ddattr.pVals.strVal.str);
                                        break;

                                    case DDl6Variable.VAR_MIN_VAL_ID:
                                        var.VarMin = ddattr.pVals.minMaxList[0].pCond.Vals[0].pExpr[0];
                                        break;

                                    case DDl6Variable.VAR_MAX_VAL_ID:
                                        var.VarMax = ddattr.pVals.minMaxList[0].pCond.Vals[0].pExpr[0];
                                        break;

                                    case DDl6Variable.VAR_SCALE_ID:
                                        var.ScaleID = (uint)ddattr.pVals.ullVal;
                                        if (ddattr.pVals.pExpr != null && ddattr.pVals.pExpr.Count > 0)
                                        {
                                            if (ddattr.pVals.pExpr[0].byElemType == Common.FPCST_OPCODE)
                                            {
                                                var.setScaleFactor(true, ddattr.pVals.pExpr[0].fConst);
                                            }
                                        }
                                        break;

                                    case DDl6Variable.VAR_ENUMS_ID:
                                        //var.eType = nitype.nBitEnum;
                                        //if (ddattr.pVals.enmList[0].desc.str != null)
                                        {
                                            var.unitlist = new Dictionary<uint, string>();
                                        }
                                        //else
                                        {
                                            var.unitidlist = new Dictionary<uint, uint>();
                                        }
                                        foreach (ENUM_VALUE ev in ddattr.pVals.enmList)
                                        {
                                            EnumTriad_t ent = new EnumTriad_t();
                                            ent.val = ev.val;
                                            ent.descS = ev.desc.str;
                                            ent.helpS = ev.help.str;
                                            ent.enumStr = ev.desc.enumStr;
                                            var.enmList.Add(ent);

                                            if (ev.desc.str != null)
                                            {
                                                var.unitlist.Add(ev.val, ev.desc.str);
                                            }
                                            else
                                            {
                                                if (ev.desc.enumStr.enumValue != 0xffffffff)
                                                {
                                                    var.unitidlist.Add(ev.desc.enumStr.enumValue, ev.desc.enumStr.iD);
                                                }
                                                else
                                                {
                                                    //var.unitidlist.Add();
                                                    var.unitidlist.Add(ev.val, ev.desc.varId);
                                                }
                                            }
                                        }
                                        break;

                                    case DDl6Variable.VAR_INDEX_ITEM_ARRAY_ID:
                                        var.vIndex.refId = ddattr.pVals.reff[0].id;
                                        var.vIndex.refType = ddattr.pVals.reff[0].type;
                                        break;

                                    case DDl6Variable.VAR_DEFAULT_VALUE_ID:
                                        break;

                                    case DDl6Variable.VAR_REFRESH_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_DEBUG_ID:
                                        break;

                                    case DDl6Variable.VAR_POST_RQST_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_POST_USER_ACT_ID:
                                        break;

                                    case DDl6Variable.VAR_TIME_FORMAT_ID:
                                        var.TimeScaleFormat = ddattr.pVals.strVal.str;
                                        break;

                                    case DDl6Variable.VAR_TIME_SCALE_ID:
                                        var.TimeScaleID = (uint)ddattr.pVals.ullVal;
                                        break;

                                    case DDl6Variable.VAR_VISIBLE_ID:
                                        break;

                                    case DDl6Variable.VAR_PRIVATE_ID:
                                        break;

                                    default:
                                        break;

                                }

                            }
                            Vars.Add(var);
                            mapIDitem.Add(ddb.id, var);
                            mapNameitem.Add(ddb.strItemName, var);
                        }
                        break;

                    case DDlBaseItem.COMMAND_ITYPE:
                        {
                            CDDLCmd cmd = new CDDLCmd();
                            cmd.pDev = this;
                            cmd.SetID(ddb.id);
                            cmd.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Command.COMMAND_NUMBER_ID:
                                        cmd.setCmdNumber((uint)ddattr.pVals.ullVal);
                                        break;

                                    case DDl6Command.COMMAND_OPER_ID:
                                        cmd.setOperation((int)ddattr.pVals.ullVal);
                                        break;

                                    case DDl6Command.COMMAND_TRANS_ID:
                                        foreach (TRANSACTION tsc in ddattr.pVals.transList)
                                        {
                                            //hCtransaction ts = (hCtransaction)tsc;
                                            hCtransaction ts = new hCtransaction();
                                            ts.number = tsc.number;
                                            ts.request = tsc.request;
                                            ts.reply = tsc.reply;
                                            ts.rcodes = tsc.rcodes;
                                            ts.post_rqst_rcv_act = tsc.post_rqst_rcv_act;
                                            ts.setCmdPtr(cmd);
                                            cmd.addTransction(ts);
                                        }

                                        break;

                                    case DDl6Command.COMMAND_RESP_CODES_ID:                                        
                                        foreach(RESPONSE_CODE rcode in ddattr.pVals.respCdList)
                                        {
                                            hCrespCode rc = new hCrespCode();
                                            rc.val = rcode.val;
                                            rc.type = rcode.type;
                                            rc.descS = Common.GetLangStr(rcode.desc.str);
                                            rc.helpS = Common.GetLangStr(rcode.help.str);
                                            cmd.addRespCode(rc);
                                        }

                                        break;

                                    default:
                                        break;

                                }
                            }

                            Cmds.Add(cmd);
                            mapIDitem.Add(ddb.id, cmd);
                            mapNameitem.Add(ddb.strItemName, cmd);
                        }
                        break;

                    case DDlBaseItem.MENU_ITYPE:
                        {
                            CDDLMenu menu = new CDDLMenu();
                            menu.pDev = this;
                            menu.SetID(ddb.id);
                            menu.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Menu.MENU_LABEL_ID:
                                        menu.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Menu.MENU_ITEMS_ID:
                                        foreach (MENU_ITEM mi in ddattr.pVals.menuItemsList)
                                        {
                                            menulist menulists = new menulist();
                                            menulists.qual = mi.qual;
                                            foreach (ddpREF dpref in mi.item)
                                            {
                                                menu_item item = new menu_item();
                                                item.uType = dpref.type;
                                                item.uiID = dpref.id;
                                                item.uiMember = dpref.member;
                                                if (dpref.index != null)
                                                {
                                                    if (dpref.index.Count > 1)//debuginfo
                                                    {
                                                        ;
                                                    }

                                                    foreach (Element ele in dpref.index)
                                                    {
                                                        Index_t it = new Index_t();
                                                        it.byElemType = ele.byElemType;
                                                        if (ele.byElemType == Common.INTCST_OPCODE)
                                                        {
                                                            it.uiIndex = (uint)ele.ulConst;
                                                        }
                                                        else if (ele.byElemType == Common.VARID_OPCODE)
                                                        {
                                                            it.uiIndex = ele.varId;//??????
                                                        }
                                                        else
                                                        {
                                                            it.uiIndex = 0xffff;
                                                        }
                                                        item.index_s.Add(it);
                                                    }

                                                }

                                                if (!menulists.Contains(item) || item.uType == 0x21)
                                                {
                                                    menulists.Add(item);
                                                }

                                            }
                                            menu.menuItems.Add(menulists);
                                        }

                                        break;

                                    case DDl6Menu.MENU_HELP_ID:
                                        menu.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Menu.MENU_VALID_ID:
                                        break;

                                    case DDl6Menu.MENU_STYLE_ID:
                                        break;

                                    case DDl6Menu.MENU_DEBUG_ID:
                                        break;

                                    case DDl6Menu.MENU_VISIBLE_ID:
                                        break;

                                    default:
                                        break;

                                }
                            }
                            Menus.Add(menu);
                            mapIDitem.Add(ddb.id, menu);
                            mapNameitem.Add(ddb.strItemName, menu);
                        }
                        break;

                    case DDlBaseItem.EDIT_DISP_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.METHOD_ITYPE:
                        {
                            CDDLMethod method = new CDDLMethod();
                            method.pDev = this;
                            method.SetID(ddb.id);
                            method.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Method.METHOD_CLASS_ID:
                                        break;

                                    case DDl6Method.METHOD_LABEL_ID:
                                        if (ddattr.pVals.strVal.str != null)
                                        {
                                            method.SetLabel(ddattr.pVals.strVal.str);//
                                        }
                                        break;

                                    case DDl6Method.METHOD_HELP_ID:
                                        method.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Method.METHOD_DEF_ID:
                                        method.defData = ddattr.pVals.defData.data;
                                        break;

                                    case DDl6Method.METHOD_VALID_ID:
                                        break;

                                    case DDl6Method.METHOD_SCOPE_ID:
                                        break;

                                    case DDl6Method.METHOD_TYPE_ID:
                                        break;

                                    case DDl6Method.METHOD_PARAMS_ID:
                                        break;

                                    case DDl6Method.METHOD_DEBUG_ID:
                                        break;

                                    case DDl6Method.METHOD_VISIBLE_ID:
                                        break;

                                    case DDl6Method.METHOD_PRIVATE_ID:
                                        break;

                                    default:
                                        break;

                                }
                            }

                            Methods.Add(method);
                            mapIDitem.Add(ddb.id, method);
                            mapNameitem.Add(ddb.strItemName, method);
                        }
                        break;

                    case DDlBaseItem.REFRESH_ITYPE:
                        {
                            CDDLRefresh refresh = new CDDLRefresh();
                            refresh.pDev = this;
                            refresh.SetID(ddb.id);
                            refresh.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Refresh.REFRESH_ITEMS_ID:
                                        foreach(ddpREFERENCE drefl in ddattr.pVals.refrshReln.watch_list)
                                        {
                                            uint uid = Common.getVarID(this, drefl);
                                            refresh.watchVarList.Add(uid);
                                        }
                                        foreach (ddpREFERENCE drefl in ddattr.pVals.refrshReln.update_list)
                                        {
                                            uint uid = Common.getVarID(this, drefl);
                                            refresh.updateVarList.Add(uid);
                                        }
                                        break;

                                    case DDl6Refresh.REFRESH_WATCH_LIST_ID:
                                        ;
                                        break;

                                    case DDl6Refresh.REFRESH_UPDATE_LIST_ID:
                                        ;
                                        break;

                                    case DDl6Refresh.REFRESH_DEBUG_ID:
                                        break;

                                    default:
                                        break;
                                }
                            }
                            Refreshes.Add(refresh);
                            mapIDitem.Add(ddb.id, refresh);
                            mapNameitem.Add(ddb.strItemName, refresh);
                        }
                        break;

                    case DDlBaseItem.UNIT_ITYPE:
                        {
                            CDDLUnit unit = new CDDLUnit();
                            unit.pDev = this;
                            unit.SetID(ddb.id);
                            unit.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Unit.UNIT_ITEMS_ID:
                                        CDDLBase itb = new CDDLBase();
                                        uint vid = Common.getVarID(this, ddattr.pVals.unitReln.unit_var);
                                        getItembyID(vid, ref itb);
                                        CDDLVar itvar;// = (CDDLVar)itb;
                                        //if (ddattr.pVals.unitReln.unit_var.Count > 1)///debug
                                        if (itb != null && itb.eType == nitype.nVar)
                                        {
                                            itvar = (CDDLVar)itb;
                                            //itvar.unitrelationID = unit.GetID();
                                            unit.unitVarId = itvar.getID();
                                            if (itvar.VariableType() == variableType_t.vT_Ascii)
                                            {
                                                //unit.
                                            }
                                            else if(itvar.VariableType() == variableType_t.vT_Enumerated)
                                            {
                                                unit.enumlist = itvar.unitlist;
                                                unit.enumIDlist = itvar.unitidlist;
                                            }
                                        }

                                        else if (ddattr.pVals.unitReln.unit_var[ddattr.pVals.unitReln.unit_var.Count - 1].type == ddpREF.ITEM_ARRAY_ID_REF)
                                        {
                                            ddpREFERENCE iaddp = ddattr.pVals.unitReln.unit_var;
                                            CDDLBase ddi = new CDDLBase();
                                            CDDLItemArray itemarray;
                                            if (getItembyID(iaddp[iaddp.Count - 1].id, ref ddi))
                                            {
                                                itemarray = (CDDLItemArray)ddi;
                                                int index = 0xfff;// = (int)ml[ml.Count - 2].index_s[0].uiIndex & 0x0f;

                                                for (int j = 0; j < itemarray.arrayitems.Count; j++)
                                                {
                                                    if ((iaddp[iaddp.Count - 2].index[0].ulConst & 0x0f) == itemarray.arrayitems[j].uiIndex)
                                                    {
                                                        index = j;
                                                        break;
                                                    }
                                                }
                                                if (itemarray.arrayitems[index].items.Count == 1
                                                    && itemarray.arrayitems[index].items[0].uType == ddpREF.COLLECTION_ID_REF)
                                                {
                                                    CDDLBase ddbi = new CDDLBase();

                                                    if (getItembyID(itemarray.arrayitems[index].items[0].uiID, ref ddbi))
                                                    {
                                                        CDDLCollection collection = (CDDLCollection)ddbi;
                                                        colletion_member cmember = null;
                                                        foreach (colletion_member cmem in collection.collectionmembers)
                                                        {
                                                            if (cmem.uiName == iaddp[iaddp.Count - 3].member)
                                                            {
                                                                cmember = cmem;
                                                                break;
                                                            }
                                                        }
                                                        if (cmember != null)
                                                        {
                                                            menu_item it = cmember.items[cmember.items.Count - 1];
                                                            CDDLBase ddin = new CDDLBase();
                                                            switch (it.uType)
                                                            {
                                                                case ddpREF.VARIABLE_ID_REF:
                                                                    CDDLBase ddbi4 = new CDDLBase();

                                                                    if (getItembyID(cmember.items[0].uiID, ref ddbi4))
                                                                    {
                                                                        itvar = (CDDLVar)ddbi4;
                                                                        //itvar.unitrelationID = unit.GetID();
                                                                        unit.enumlist = itvar.unitlist;
                                                                        unit.enumIDlist = itvar.unitidlist;
                                                                    }
                                                                    break;

                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        foreach (ddpREFERENCE drefl in ddattr.pVals.unitReln.var_units)
                                        {
                                            uint uid = Common.getVarID(this, drefl);
                                            unit.VarIdList.Add(uid);
                                            /*foreach (ddpREF dref in drefl)
                                            {
                                                {
                                                    //unit.enumlist.Add(dref.)
                                                    getItembyID(dref.id, ref itb);
                                                    itvar = (CDDLVar)itb;
                                                    itvar.unitlist = unit.enumlist;
                                                    itvar.unitidlist = unit.enumIDlist;
                                                }
                                            }
                                            */
                                        }

                                        break;

                                    case DDl6Unit.UNIT_DEBUG_ID:
                                        break;

                                    default:
                                        break;

                                }
                                break;
                            }

                            Units.Add(unit);
                            mapIDitem.Add(ddb.id, unit);
                            mapNameitem.Add(ddb.strItemName, unit);
                        }
                        break;

                    case DDlBaseItem.WAO_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.ITEM_ARRAY_ITYPE:
                        {
                            CDDLItemArray itemarray = new CDDLItemArray();
                            itemarray.pDev = this;
                            itemarray.SetID(ddb.id);
                            itemarray.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6ItemArray.ITEM_ARRAY_ELEMENTS_ID:
                                        foreach (ITEM_ARRAY_ELEMENT iae in ddattr.pVals.itemArrElmnts)
                                        {
                                            item_array ita = new item_array();
                                            ita.uiElv = iae.evaled;
                                            ita.uiIndex = iae.index;
                                            ita.desc = iae.desc.str;
                                            ita.help = iae.desc.str;

                                            foreach (ddpREF dpref in iae.item)
                                            {
                                                menu_item item = new menu_item();
                                                item.uType = dpref.type;
                                                item.uiID = dpref.id;

                                                if (dpref.index != null)
                                                {
                                                    foreach (Element ele in dpref.index)
                                                    {
                                                        Index_t it = new Index_t();
                                                        it.byElemType = ele.byElemType;
                                                        if (ele.byElemType == Common.INTCST_OPCODE)
                                                        {
                                                            it.uiIndex = (uint)ele.ulConst;
                                                        }
                                                        else if (ele.byElemType == Common.VARID_OPCODE)
                                                        {
                                                            it.uiIndex = ele.varId;//??????
                                                        }
                                                        else
                                                        {
                                                            it.uiIndex = 0xffff;
                                                        }
                                                        item.index_s.Add(it);
                                                    }
                                                }
                                                ita.items.Add(item);
                                            }
                                            if ((!itemarray.arrayitems.Contains(ita)))
                                            {
                                                itemarray.arrayitems.Add(ita);
                                            }
                                        }

                                        break;

                                    case DDl6ItemArray.ITEM_ARRAY_LABEL_ID:
                                        itemarray.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6ItemArray.ITEM_ARRAY_HELP_ID:
                                        itemarray.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6ItemArray.ITEM_ARRAY_VALIDITY_ID:
                                        break;

                                    case DDl6ItemArray.ITEM_ARRAY_DEBUG_ID:
                                        break;

                                    case DDl6ItemArray.ITEM_ARRAY_PRIVATE_ID:
                                        break;

                                    default:
                                        break;
                                }
                            }
                            ItemArrays.Add(itemarray);
                            mapIDitem.Add(ddb.id, itemarray);
                            mapNameitem.Add(ddb.strItemName, itemarray);
                        }
                        break;

                    case DDlBaseItem.COLLECTION_ITYPE:
                        {
                            CDDLCollection collection = new CDDLCollection();
                            collection.pDev = this;
                            collection.SetID(ddb.id);
                            collection.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Collection.COLLECTION_MEMBERS_ID:
                                        foreach (MEMBER mi in ddattr.pVals.memberList)
                                        {
                                            colletion_member memb = new colletion_member();
                                            memb.uiElv = mi.evaled;
                                            memb.uiName = mi.name;
                                            memb.desc = mi.desc.str;
                                            memb.help = mi.desc.str;
                                            foreach (ddpREF dpref in mi.item)
                                            {
                                                menu_item item = new menu_item();
                                                item.uType = dpref.type;
                                                item.uiID = dpref.id;

                                                if (dpref.index != null)
                                                {
                                                    foreach (Element ele in dpref.index)
                                                    {
                                                        Index_t it = new Index_t();
                                                        it.byElemType = ele.byElemType;
                                                        if (ele.byElemType == Common.INTCST_OPCODE)
                                                        {
                                                            it.uiIndex = (uint)ele.ulConst;
                                                        }
                                                        else if (ele.byElemType == Common.VARID_OPCODE)
                                                        {
                                                            it.uiIndex = ele.varId;//??????
                                                        }
                                                        else
                                                        {
                                                            it.uiIndex = 0xffff;
                                                        }
                                                        item.index_s.Add(it);
                                                    }
                                                }
                                                else
                                                {
                                                    item.uiMember = dpref.member;
                                                }
                                                memb.items.Add(item);
                                            }
                                            if ((!collection.collectionmembers.Contains(memb)))
                                            {
                                                collection.collectionmembers.Add(memb);
                                            }
                                        }

                                        break;

                                    case DDl6Collection.COLLECTION_LABEL_ID:
                                        collection.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Collection.COLLECTION_HELP_ID:
                                        collection.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Collection.COLLECTION_VALID_ID:
                                        break;

                                    case DDl6Collection.COLLECTION_DEBUG_ID:
                                        break;

                                    case DDl6Collection.COLLECTION_VISIBLE_ID:
                                        break;

                                    case DDl6Collection.COLLECTION_PRIVATE_ID:
                                        break;

                                    default:
                                        break;

                                }
                            }
                            Collections.Add(collection);
                            mapIDitem.Add(ddb.id, collection);
                            mapNameitem.Add(ddb.strItemName, collection);
                        }
                        break;

                    case DDlBaseItem.BLOCK_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.PROGRAM_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.RECORD_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.ARRAY_ITYPE:
                        {
                            CDDLArray arr = new CDDLArray();
                            arr.pDev = this;
                            arr.SetID(ddb.id);
                            arr.SetSourceName(ddb.strItemName);
                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Array.ARRAY_NUM_OF_ELEMENTS_ID:
                                        if (DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_INT == ddattr.attrDataType)
                                        {
                                            arr.uiEleNum = (uint)ddattr.pVals.llVal;
                                        }
                                        else
                                        {
                                            arr.uiEleNum = (uint)ddattr.pVals.ullVal;
                                        }
                                        break;

                                    case DDl6Array.ARRAY_TYPE_ID:
                                        arr.uiRefID = ddattr.pVals.reff[0].id;

                                        break;

                                    case DDl6Array.ARRAY_LABEL_ID:
                                        arr.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Array.ARRAY_HELP_ID:
                                        arr.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Array.ARRAY_VALID_ID:
                                        break;

                                    case DDl6Array.ARRAY_DEBUG_ID:
                                        break;

                                    default:
                                        break;
                                }
                            }
                            Arrays.Add(arr);
                            mapIDitem.Add(ddb.id, arr);
                            mapNameitem.Add(ddb.strItemName, arr);
                        }
                        break;

                    case DDlBaseItem.VAR_LIST_ITYPE:
                        {
                            CDDLList lst = new CDDLList();
                            lst.pDev = this;
                            lst.SetID(ddb.id);
                            lst.SetSourceName(ddb.strItemName);
                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6List.LIST_COUNT_ID:
                                        lst.uiCount = (uint)ddattr.pVals.llVal;
                                        break;

                                    case DDl6List.LIST_LABEL_ID:
                                        lst.SetLabel(ddattr.pVals.strVal.str);
                                        break;

                                    case DDl6Array.ARRAY_HELP_ID:
                                        lst.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Array.ARRAY_VALID_ID:
                                        break;

                                    case DDl6Array.ARRAY_DEBUG_ID:
                                        break;


                                    default:
                                        break;
                                }
                            }
                            Lists.Add(lst);
                            mapIDitem.Add(ddb.id, lst);
                            mapNameitem.Add(ddb.strItemName, lst);
                        }
                        break;

                    case DDlBaseItem.RESP_CODES_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.DOMAIN_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.MEMBER_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.FILE_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.CHART_ITYPE:
                        {
                            CDDLChart chart = new CDDLChart();
                            chart.pDev = this;
                            chart.SetID(ddb.id);
                            chart.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Chart.CHART_LABEL_ID:
                                        chart.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Chart.CHART_HELP_ID:
                                        chart.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Chart.CHART_VALID_ID:
                                        break;

                                    case DDl6Chart.CHART_HEIGHT_ID:
                                        break;

                                    case DDl6Chart.CHART_WIDTH_ID:
                                        break;

                                    case DDl6Chart.CHART_TYPE_ID:
                                        break;

                                    case DDl6Chart.CHART_LENGTH_ID:
                                        break;

                                    case DDl6Chart.CHART_CYCLETIME_ID:
                                        break;

                                    case DDl6Chart.CHART_MEMBERS_ID:
                                        break;

                                    case DDl6Chart.CHART_DEBUG_ID:
                                        break;

                                    case DDl6Chart.CHART_VISIBLE_ID:
                                        break;

                                    default:
                                        break;

                                }

                            }
                            Charts.Add(chart);
                            mapIDitem.Add(ddb.id, chart);
                            mapNameitem.Add(ddb.strItemName, chart);
                        }
                        break;

                    case DDlBaseItem.GRAPH_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.AXIS_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.WAVEFORM_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.SOURCE_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.LIST_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.GRID_ITYPE:
                        {

                        }
                        break;

                    case DDlBaseItem.IMAGE_ITYPE:
                        {
                            CDDLImage image = new CDDLImage();
                            image.pDev = this;
                            image.SetID(ddb.id);
                            image.SetSourceName(ddb.strItemName);

                            foreach (DDlAttribute ddattr in ddb.attrList)
                            {
                                switch (ddattr.byAttrID)
                                {
                                    case DDl6Image.IMAGE_LABEL_ID:
                                        image.SetLabel(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Image.IMAGE_HELP_ID:
                                        image.SetHelp(ddattr.pVals.strVal.str);//
                                        break;

                                    case DDl6Image.IMAGE_VALID_ID:
                                        break;

                                    case DDl6Image.IMAGE_LINK_ID:
                                        break;

                                    case DDl6Image.IMAGE_PATH_ID:
                                        break;

                                    case DDl6Image.IMAGE_DEBUG_ID:
                                        break;

                                    case DDl6Image.IMAGE_VISIBLE_ID:
                                        break;

                                    default:
                                        break;

                                }

                            }
                            Images.Add(image);
                            mapIDitem.Add(ddb.id, image);
                            mapNameitem.Add(ddb.strItemName, image);
                        }
                        break;

                    case DDlBaseItem.BLOB_ITYPE:
                        {

                        }
                        break;

                    default:
                        break;
                }
            }

            //post process after all type converted

            foreach (CDDLArray arr in Arrays)
            {
                CDDLBase db = new CDDLBase();
                if (getItembyID(arr.uiRefID, ref db))
                {
                    arr.rType = db.eType;
                    switch (db.eType)
                    {
                        case nitype.nVar:
                            arr.items = new CDDLVar[arr.uiEleNum];
                            break;

                        case nitype.nCollection:
                            arr.items = new CDDLCollection[arr.uiEleNum];
                            break;

                        case nitype.nAxis:
                            arr.items = new CDDLAxis[arr.uiEleNum];
                            break;

                        default:
                            break;

                    }
                }
            }

            foreach (CDDLList list in Lists)
            {
                CDDLBase db = new CDDLBase();
                if (getItembyID(list.uiRefID, ref db))
                {
                    list.rType = db.eType;
                }
            }

            foreach(CDDLCmd cmd in Cmds)
            {
                //add cmd to var/?/?/?
                cmd.updateVarCommandUsage();
            }

            foreach(CDDLUnit unit in Units)
            {
                unit.updateVarUnit();
            }

            foreach(CDDLRefresh refresh in Refreshes)
            {
                refresh.updateVarRelation();
            }

            foreach (CDDLVar vara in Vars)
            {
                vara.updateEnumList();
            }

        }

        public void notifyVarUpdate(int changedItemNumber, NUA_t isChanged)
        {
            switch (isChanged)
            {
                case NUA_t.METH_changed:/* sjv 6jun07 new one to preclude the above copy, all else is the same */
                case NUA_t.IS_changed:
                    {
                        parentform.OnUpdate(mainpage.WM_SDC_VAR_CHANGEDVALUE, (uint)changedItemNumber);
                    }
                    break;
                case NUA_t.STR_changed:
                    {
                        parentform.OnUpdate(mainpage.WM_SDC_STRUCTURE_CHANGED, (uint)changedItemNumber);
                    }
                    break;
                case NUA_t.STRT_actPkt:
                    {
                    }
                    break;
                case NUA_t.END_actPkt:
                    {
                    }
                    break;
                case NUA_t.STRT_methPkt:
                    {
                    }
                    break;
                case NUA_t.END_methPkt:
                    {
                    }
                    break;

                case NUA_t.ABORT_dd:      /* stevev 11apr11 - UI has to kill dev obj so obj has to tell */
                    {// no questions asked... just kill it		
                        //Close Device Here?/?/?/?
                    }
                    break;
                case NUA_t.DEL_element:
                    {// added notify item disappearance 23may11
                        {
                            //element deleted here/?/?/?

                        }// else - nop
                    }
                    break;
                case NUA_t.STAT_changed:  // J.U. State is changed 17.02.11 - used when offline
                case NUA_t.NO_change:  /* 18nov05 - no longer a legal input. */
                default:
                    {// nothing is done (the message is consumed)
                     // too many...TRACE(_T("    NotifyAppVarChanged----(0x%x)  NO CHANGE Notification\n"),GetCurrentThreadId());	
                     // debug
                        //state = 0x12;
                        //LOGIF(LOGP_NOTIFY)(CLOG_LOG, "No change call in Notify: 0x%04x\n", changedItemNumber);
                    }
                    break;
            }// endswitxh
        }
    }

}
