using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FieldIot.HARTDD
{
    public struct tm
    {
        public int tm_sec;   // seconds after the minute - [0, 60] including leap second
        public int tm_min;   // minutes after the hour - [0, 59]
        public int tm_hour;  // hours since midnight - [0, 23]
        public int tm_mday;  // day of the month - [1, 31]
        public int tm_mon;   // months since January - [0, 11]
        public int tm_year;  // years since 1900
        public int tm_wday;  // days since Sunday - [0, 6]
        public int tm_yday;  // days since January 1 - [0, 365]
        public int tm_isdst; // daylight savings time flag
    }

    public struct VarIndex
    {
        public uint refType;
        public uint refId;
        //public ushort curVal;
    }

    public class CVarList : List<CDDLVar>
    {
        public int bumpAllVars()
        {
            int c = 0;
            //Deepak : 112504
            DATA_QUALITY_T tds;
            bool bIsValid = false;

            try
            {
                foreach (CDDLVar pV in this)
                {// ptr2aPtr2a hCVar
                    if (pV == null)
                    {
                        //DEBUGLOG(CERR_LOG, "ERROR: BumpAllVars found a NULL pointer in the VAR list!(debug only)\n");
                        continue;
                    }
                    // add a filter (or 2 or 3...)
                    tds = pV.getDataQuality();

                    try
                    {
                        bIsValid = pV.IsValid();
                    }
                    catch (Exception ex)
                    {
                        String str = "";
                        str += ex.Message + "\n";
                        //MessageBox.Show(str, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bIsValid = true;
                    }

                    if ((!pV.IsWriteOnly()) && // handling test
                        (!pV.IsLocal()) && // locality test
                        (bIsValid/*(*iT).IsValidTest()*/      ) && // validity test /*DEEPAK 101804 changed to isvalid to isvalidtest()*/
                        (pV.HasReadCmd()) && // readablity test
                                             //((*iT).getDataQuality() == DA_NOT_VALID || (*iT).getDataQuality() == DA_STALEUNK)
                        (tds != DATA_QUALITY_T.DA_HAVE_DATA))/* sjv04dec06-must bump all stales or they may never get read */

                    {
                        //devPtr().aquireItemMutex();
                        //bAquiredMutex = true;
                        pV.bumpQueryCnt();
                        if (tds != DATA_QUALITY_T.DA_STALE_OK)
                        {
                            pV.bumpQueryCnt();/*sjv04dec06-doublebump all but OK*/
                        }
                        //devPtr().returnItemMutex();
                        //bAquiredMutex = false;
                    }
                    c++;
                }// next

            }//try

            catch (Exception ex)
            {
                String str = "";
                str += ex.Message + "\n";
                //MessageBox.Show(str, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //if (bAquiredMutex)
                //    devPtr().returnItemMutex();
                //LOGIT(CERR_LOG, "Inside bumpAllVars catch(...)\n");
            }
            //END changes 112504

            return c;
        }

        public int getNeededRead(ref cmdDescList_t cmdDescLst)
        {
            return getNeededList(ref cmdDescLst, false);
        }

        public int getNeededWrit(ref cmdDescList_t cmdDescLst)
        {
            return getNeededList(ref cmdDescLst, true);
        }

        public int getNeededList(ref cmdDescList_t cmdDescLst, bool isWrt)
        {
            cmdDescLst.Clear();

            cmdDescList_t varList = new cmdDescList_t();
            foreach (CDDLVar pVr in this)
            {// isa ptr 2a ptr 2a hCVar
                if (pVr == null || (!pVr.IsValid()))
                {
                    continue;// skip this entry
                }
                uint thisItmID = pVr.getID();// also good for debugging
                if (thisItmID == Common.RESPONSECODE_SYMID || thisItmID == Common.DEVICESTATUS_SYMID)
                {
                    continue;// we don't weigh these
                }

                if (pVr.getDataStatus() == 0)
                {// do not weigh this item -  it has been marked as a problem variable
                    continue;
                }

                INSTANCE_DATA_STATE_T tDS = pVr.getDataState();
                if (/*tDS == INSTANCE_DATA_STATE_T.IDS_NEEDS_WRITE && */isWrt)
                {// deal with write command list
                    varList = pVr.getWrCmdList();
                }
                else if ((!isWrt) && (tDS == INSTANCE_DATA_STATE_T.IDS_STALE
                    || tDS == INSTANCE_DATA_STATE_T.IDS_UNINITIALIZED || tDS == INSTANCE_DATA_STATE_T.IDS_INVALID))
                {// deal with read command list
                    varList = pVr.getRdCmdList();
                }
                //else // (tDS == IDS_CACHED || tDS == IDS_PENDING ) leave the list clear

                // this is append unique for cmdDescList_t
                // for each var command
                //cmdDescListIT varIT, outIT;
                //hCcommandDescriptor pVardesc, *pOutdesc;
                bool foundMatch = false;
                //foreach (hCcommandDescriptor pVardesc in varList)
                //for(int i = 0; i < varList.Count; i++)
                if (varList.Count > 0)
                {// for each read or write command that could service this var...
                    hCcommandDescriptor pVardesc = varList.ElementAt(0);
                    //pVardesc = &(*varIT);
                    foreach (hCcommandDescriptor pOutdesc in cmdDescLst)
                    {//check if its already there
                        //pOutdesc = &(*outIT);
                        if (pVardesc.cmdNumber == pOutdesc.cmdNumber && pVardesc.transNumb == pOutdesc.transNumb)
                        {// we aren't going to compare indexes in this first cut
                            foundMatch = true;// already in the list, we can skip adding it
                            //DEBUGLOG(CLOG_LOG, "-", pVr.getID());
                            break; // out of inner loop
                        }
                        // else keep looking
                    }// next unique output value
                    if (!foundMatch)// we need to add it since it isn't there
                    {
                        cmdDescLst.Add(pVardesc);
                        //DEBUGLOG(CLOG_LOG, "+", pVr.getID());
                    }
                    // else skip it and go on to the next var list entry
                }// next var command

            }// next var

            return Common.SUCCESS;
        }

    }

    public class CDDLVar : CDDLBase
    {
        string unit;
        bool bWritable;
        bool bWriteOnly = false;

        //public float value;

        /*integer type*/
        //UInt64 Value;   /* device value */
        //UInt64 Wrt_Val; /* display value */
        //UInt64 cacheVal;
        bool isSigned = false;

        int varclass;
        int Handling;

        /*index type*/
        public EnumList enmList;
        public VarIndex vIndex;

        CValueVarient vValue;//current value
        CValueVarient Wrt_Val;//value to write
        //CValueVarient cacheVal;//cached value

        public Element VarMin;
        public Element VarMax;

        float scaleFactor = 0;
        bool bscaled = false;

        public uint ScaleID = 0;//0 means no scaleID
        public uint TimeScaleID = 0;//0 means no timescaleID
        public string TimeScaleFormat = null;

        variableType_t datatype;
        //public uint size;
        //ushort itemSize;
        public uint unitrelationID;
        public List<uint> relationVarList;
        public Dictionary<uint, string> unitlist;
        public Dictionary<uint, uint> unitidlist;
        //variableType_t varType;
        uint varSize;
        INSTANCE_DATA_STATE_T dataState;
        INSTANCE_DATA_STATE_T previousDataState;
        DATA_QUALITY_T dataQuality;

        hCcommandDescriptor readCommand;    // 0xff == NONE 
        hCcommandDescriptor writeCommand;   // 0xff == NONE.

        cmdDescList_t rdCmdList;
        cmdDescList_t wrCmdList;

        string DispFormat;
        string EditFormat;
        string returnString;

        byte[] digits4bytes = { 0, 4, 6, 9, 11, 14, 16, 18, 20 };

        writestatus_t writeStatus;
        int dataStatus;

        int queryCount;// a weighting multiplier

        string wValStr;
        int wValue;

        public string wValueString
        {
            get
            {
                return wValStr;
            }
            set
            {
                wValStr = value;
            }
        }

        public int wValueData
        {
            get
            {
                return wValue;
            }
            set
            {
                wValue = value;
            }
        }

        public CDDLVar()
        {
            dataStatus = 1;

            unitlist = null;
            unitidlist = null;
            eType = nitype.nVar;
            vValue = new CValueVarient();
            Wrt_Val = new CValueVarient();
            VarMin = new Element();
            VarMax = new Element();
            enmList = new EnumList();
            vIndex = new VarIndex();
            readCommand = new hCcommandDescriptor();
            writeCommand = new hCcommandDescriptor();
            rdCmdList = new cmdDescList_t();
            wrCmdList = new cmdDescList_t();
            relationVarList = new List<uint>();
        }

        public void setScaleFactor(bool bfact, float factor)
        {
            bscaled = true;
            scaleFactor = factor;
        }

        public bool getScaleFactor(ref float factor)
        {
            factor = scaleFactor;
            return bscaled;
        }

        public INSTANCE_DATA_STATE_T getDataState()
        {
            return (dataState);
        }
        public DATA_QUALITY_T getDataQuality()
        {
            return (dataQuality);
        }

        public CValueVarient getDispValue()//////??????
        {
            return vValue;
        }

        public CValueVarient getWriteValue()//////??????
        {
            return Wrt_Val;
        }

        public cmdDescList_t getRdCmdList()
        {
            return rdCmdList;
        }

        public cmdDescList_t getWrCmdList()
        {
            return wrCmdList;
        }

        public void setWriteOnly(bool b)
        {
            bWriteOnly = b;
        }

        public void setHandling(int h)
        {
            Handling = h;
        }

        public void setClass(int h)
        {
            varclass = h;
        }

        public bool IsWriteOnly()
        {
            return bWriteOnly;

        }/*End IsWriteOnly*/

        public override bool IsReadOnly()
        {
            if ((Handling & Common.WRITE_HANDLING) != 0) // if not writable then read only
            {
                return false;
            }
            else
            {
                return true;
            }
        }/*End IsReadOnly*/

        public void bumpQueryCnt()
        {
            queryCount += pDev.queryWeight();
        }
        public int getQueryCnt()
        {
            return (queryCount);
        }

        public void addRDdesc(hCcommandDescriptor rdD)
        {
            rdCmdList.Add(rdD);// insert the entire command
        }

        public void addWRdesc(hCcommandDescriptor wrD)
        {
            wrCmdList.Add(wrD);
        }


        public void SetDispFormat(string df)
        {
            DispFormat = df;
        }

        public string GetDispFormat()
        {
            return DispFormat;
        }

        public void updateEnumList()
        {
            if (datatype == variableType_t.vT_Enumerated)
            {
                EnumList newEnuL = new EnumList();
                foreach (EnumTriad_t en in enmList)
                {
                    EnumTriad_t nen = en;
                    if (en.descS == null)
                    {
                        CDDLVar enVar = null;
                        //nen.descS = nen.enumStr;
                        if (pDev.getVarbyID(nen.enumStr.iD, ref enVar))
                        {
                            nen.descS = enVar.GetEnumString(nen.enumStr.enumValue);
                        }
                    }
                    newEnuL.Add(nen);
                }
                enmList = newEnuL;
            }
        }

        public List<ComboxItem> getEnumList()
        {
            List<ComboxItem> enumlist = new List<ComboxItem>();
            if (datatype == variableType_t.vT_Enumerated)
            {
                foreach (EnumTriad_t en in enmList)
                {
                    ComboxItem item = new ComboxItem(Common.GetLangStr(en.descS), (int)en.val);
                    enumlist.Add(item);
                }
                return enumlist;
            }
            return null;
        }

        public string GetEnumString(uint val)
        {
            if (datatype == variableType_t.vT_Enumerated)
            {
                foreach (EnumTriad_t en in enmList)
                {
                    if (en.val == val)
                    {
                        return Common.GetLangStr(en.descS);
                    }
                }
            }
            return null;
        }

        public string GetDispString()
        {
            string retStr = null;
            if (DispFormat == null || DispFormat == "")
            {
                /*
                string t = vValue.sStringVal;
                if ((int)t.Length < rsLen)
                {
                    retStr = t;
                }
                else
                {
                    //_tstrncpy(retStr, t.c_str(), rsLen - 1);
                    //retStr[rsLen - 1] = _T('\0');
                    retStr = t.Substring(0, rsLen - 1);
                }
                */
                if (datatype == variableType_t.vT_Enumerated)
                {
                    foreach (EnumTriad_t en in enmList)
                    {
                        if (en.val == vValue.GetInt())
                        {
                            return Common.GetLangStr(en.descS);
                        }
                    }
                }
                else if (datatype == variableType_t.vT_HartDate)
                {
                    int data = vValue.GetInt();
                    int day, month, year;
                    if (data != 0)
                    {
                        day = data >> 16;
                        month = (data & 0xffff) >> 8;
                        year = (data & 0xff) + 1900;
                    }
                    else
                    {
                        day = 1;
                        month = 1;
                        year = 1900;
                    }
                    DateTime dt = new DateTime(year, month, day);
                    return dt.ToString("d");
                }
                else
                {
                    if (bscaled)
                    {
                        return vValue.GetDispString(scaleFactor);
                    }
                    else
                    {
                        return vValue.GetDispString();  // works good
                    }
                }

            }
            // else we have some unique formatting, we'll have to deal with it

            //string pch = formatStr;//.Substring(formatStr.IndexOf('%'));// _tstrchar(formatStr, _T('%'));
            //string theChar = _T('\0');
            string newformat = null;
            char theChar = '\0';
            if (DispFormat != null) // real formatting
            {//	no spaces allowed in formatting, so get the last char
                newformat = Common.GetLangStr(DispFormat);
                theChar = DispFormat[0];// start with the '%'
                foreach (char p in DispFormat)
                {
                    //pch++;
                    if (p == ('\0') || p == (' ') || p == ('\t') || p == ('\n'))
                        break;
                    //else
                    theChar = p;// we want the last char
                }
            }// else - leave cahr at null to get to default

            switch (theChar)
            {
                case 'c':
                case 'C':
                    //_tsprintf(retStr, formatStr, (char)vValue);
                    retStr = String.Format(newformat, vValue.GetBool());
                    break;
                case 'd':
                case 'i':
                    //_tsprintf(retStr, formatStr, (int)vValue);
                    retStr = String.Format(newformat, vValue.GetInt());
                    break;
                case 'o':
                case 'u':
                case 'x':
                case 'X':
                    //_tsprintf(retStr, formatStr, (uint)vValue);
                    retStr = String.Format(newformat, vValue.GetUInt());
                    break;
                case 'e':
                case 'E':
                case 'f':
                case 'g':
                case 'G':
                    newformat = newformat.Substring(0, newformat.Length - 1);
                    string it = Regex.Match(newformat, @"\d+\.").Value;
                    if (it == null || it == "")
                    {
                        it = "0";
                    }
                    string dc = Regex.Match(newformat, @"\.\d+").Value;
                    if (dc != "")
                    {
                        dc = dc.Substring(1);
                    }
                    //newformat = "{" + it + ":" + dc + "F}";
                    float factor = 1;
                    if (bscaled)
                    {
                        factor = scaleFactor;
                    }
                    newformat = "{0:F" + dc + "}";
                    if (vValue.vIsDouble)
                    {
                        double d = vValue.GetDouble() * factor;
                        //_tsprintf(retStr, formatStr, d);
                        retStr = String.Format(newformat, d);
                    }
                    else
                    {
                        float y = (float)vValue.GetFloat() * factor;
                        //_tsprintf(retStr, formatStr, y);
                        retStr = String.Format(newformat, y);
                    }
                    break;
                case 's':
                case 'S':
                    //_tsprintf(retStr, formatStr, ((string)vValue).c_str());
                    retStr = vValue.GetString();
                    break;
                case '%':
                default:
                    //_tsprintf(retStr, formatStr);
                    retStr = vValue.GetFloat().ToString();
                    break;
            }
            return retStr; // no error
        }

        public void SetEditFormat(string ef)
        {
            EditFormat = ef;
        }

        public string GetEditFormat()
        {
            return EditFormat;
        }

        public CValueVarient getRawDispValue()//////??????
        {
            vValue.vSize = getSize();
            return vValue;
        }

        public void getFmtStr(ref string retStr)
        {
            retStr = TimeScaleFormat;
            if (retStr == null)
            {
                retStr = "H:M:S";//%T
            }
            if (retStr == "%T" || retStr == "T")
            {
                retStr = "H:M:S";
            }
            if (retStr == "%R" || retStr == "R")
            {
                retStr = "H:M";
            }
        }

        public bool IsNumeric()
        {
            if (datatype == variableType_t.vT_Integer || datatype == variableType_t.vT_FloatgPt || datatype == variableType_t.vT_Double)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setDataStatus(int newStatus)
        {
            dataStatus = newStatus;
        }// 1 normal, 0 stop weighing

        public int getDataStatus()
        {
            return dataStatus;
        }

        public void setWriteStatus(writestatus_t newStatus)
        {
            writeStatus = newStatus;
        }// 0 normal, 1 user written

        public void setWriteStatus(int newStatus)
        {
            writeStatus = (writestatus_t)newStatus;
        }// 0 normal, 1 user written

        public writestatus_t getWriteStatus()
        {
            return writeStatus;
        }

        public void setDispValue(CValueVarient cv)//////??????
        {
            vValue = cv;
        }

        public void setValue(byte[] data, byte offset, byte len, byte size = 0, uint mask = 0xffffffff)
        {
            //switch(datatype)
            byte[] vda = new byte[len - offset];
            for (int i = 0; i < len - offset; i++)
            {
                vda[i] = data[offset + i];
            }

            byte setsize;
            if (size != 0)
            {
                setsize = size;
            }
            else
            {
                setsize = (byte)varSize;
            }


            vValue.SetVarValue(vda, datatype, setsize, mask);
        }

        public void ApplyIt()
        {
            Wrt_Val = vValue;
        }

        public uint getUnitVarId()
        {
            return unitrelationID;
        }

        public override bool IsValid()
        {
            return true;
        }

        public void setUnitFromList()
        {
            if (unitlist == null)
            {
                if (unitidlist != null)
                {
                    foreach (KeyValuePair<uint, uint> kvp in unitidlist)
                    {

                    }
                }
                else
                {

                }
            }
            else
            {
                //unitl
            }
        }

        public INSTANCE_DATA_STATE_T markItemState(INSTANCE_DATA_STATE_T newSt)
        {
            INSTANCE_DATA_STATE_T r = dataState;
            // stevev 17apr07 :: now on a case-by-case basis::> dataState = newSt;

            switch (newSt)
            {
                case INSTANCE_DATA_STATE_T.IDS_STALE:
                    if (r == INSTANCE_DATA_STATE_T.IDS_NEEDS_WRITE)//stevev 17apr07-transition restriction
                    {
                        break;  // - skip any state change
                    }
                    if (dataQuality == DATA_QUALITY_T.DA_STALE_OK || dataQuality == DATA_QUALITY_T.DA_HAVE_DATA)
                    {
                        dataQuality = DATA_QUALITY_T.DA_STALE_OK;
                    }
                    else // notvalid or staleunk
                    {
                        dataQuality = DATA_QUALITY_T.DA_STALEUNK;
                    }
                    /* for the removal of encoded state in the values (ie -1) 05jul05*/
                    if (r == INSTANCE_DATA_STATE_T.IDS_UNINITIALIZED || r == INSTANCE_DATA_STATE_T.IDS_INVALID)
                    {
                        previousDataState = r;// this is cleared in extract
                    }// else leave it alone
                    dataState = newSt;// stevev 17apr07
                    break;
                case INSTANCE_DATA_STATE_T.IDS_NEEDS_WRITE:
                case INSTANCE_DATA_STATE_T.IDS_PENDING:
                    if (dataQuality == DATA_QUALITY_T.DA_STALE_OK || dataQuality == DATA_QUALITY_T.DA_HAVE_DATA)
                    {
                        dataQuality = DATA_QUALITY_T.DA_STALE_OK;
                    }
                    else // notvalid or staleunk
                    {
                        dataQuality = DATA_QUALITY_T.DA_STALEUNK;
                    }
                    /* for the removal of encoded state in the values (ie -1) 05jul05*/
                    if (r == INSTANCE_DATA_STATE_T.IDS_UNINITIALIZED || r == INSTANCE_DATA_STATE_T.IDS_INVALID)
                    {
                        previousDataState = r;// this is cleared in extract
                    }// else leave it alone
                    dataState = newSt;// stevev 17apr07
                    break;
                case INSTANCE_DATA_STATE_T.IDS_CACHED:
                    if (dataState != newSt)
                    {
                        pDev.notifyVarUpdate((int)getID(), NUA_t.STAT_changed);    // J.U. 17.02.11
                    }
                    if (r == INSTANCE_DATA_STATE_T.IDS_NEEDS_WRITE)//stevev 17apr07-transition restriction
                        break; // - skip any state change
                    dataQuality = DATA_QUALITY_T.DA_HAVE_DATA;
                    queryCount = 0;
                    dataState = newSt;// stevev 17apr07
                    break;
                case INSTANCE_DATA_STATE_T.IDS_INVALID:
                    if (dataState != newSt)
                    {
                        pDev.notifyVarUpdate((int)getID(), NUA_t.STAT_changed);    // J.U. 22.03.11
                    }
                    dataQuality = DATA_QUALITY_T.DA_NOT_VALID;
                    dataState = newSt;// stevev 17apr07
                    break;
                case INSTANCE_DATA_STATE_T.IDS_UNINITIALIZED:
                default:
                    dataQuality = DATA_QUALITY_T.DA_NOT_VALID;
                    dataState = newSt;// stevev 17apr07
                    break;
            }// endswitch
            return r;
        }

        public void addPercent(ref string fmtStr)
        {
            string locStr;
            int loc;
            char[] ch = { 'H', 'I', 'M', 'p', 'R', 'S', 'T' };

            if ((loc = fmtStr.IndexOfAny(ch)) != -1)
            {
                locStr = fmtStr.Substring(loc + 1); // post found letter
                addPercent(ref locStr);// add percent to the rest of the string
                if (loc == 0)
                {
                    fmtStr = "%" + fmtStr.Substring(loc, 1) + locStr;
                }
                else if (loc > 0 && fmtStr[loc - 1] != '%')
                {
                    fmtStr = fmtStr.Substring(0, loc) + "%" + fmtStr.Substring(loc, 1) + locStr;
                }
                else
                {
                    fmtStr = fmtStr.Substring(0, loc) + fmtStr.Substring(loc, 1) + locStr;
                }
            }
            //else we are the end of the string - no more to do
        }

        public void setRawDispValue(CValueVarient newValue)
        {
            vValue = newValue;
        }

        /*
        public void setRawDispValue(CValueVarient newValue, )
        {
            d_WrtVal = ((double)newValue);
            //if (IsLocal())
            //{
            //  ApplyIt();
            //}
            if (IsLocal())
            {// go ahead and apply it
                if ((dValue != d_WrtVal)) // stevev 02jan07 
                                          // - locals must be updated reguardless of criticality
                {// needs notification /
                    hCmsgList noteMsgs;// moved 04dec06
                                       //setDependentsUNK();
                                       //NUA_t p = IS_changed;
                                       // stevev 12apr07 - modified 2b like setDispVal() as per Vibhor
                    if (
                    ApplyIt()//;//Value = Wrt_Val;  
                    && !devPtr().isInMethod()
                    )
                    {
                        noteMsgs.insertUnique(getID(), mt_Val, 0);// we forgot to add ourselves
                                                                  //devPtr().notifyVarUpdate(getID(),p);

                        notify(noteMsgs);
                        notifyUpdate(noteMsgs); // send 'em now
                    }
                }
                else
                {
                    ApplyIt();//Value = Wrt_Val;
                }
            }
        }
        */

        bool validateTrim(ref string inputStr)
        {
            string locStr;
            int loc;
            //bool isNegative = false;
            variableType_t vt = VariableType();

            // Capture a copy of the user input string
            locStr = inputStr;

            // Check to determine if empty string

            if (locStr.Length <= 0)
            {
                /*
                 * Nothing has been entered.
                 * This is a parsing error!
                 */
                return false;
            }

            // Currently, we are only validating on Integers here
            // Floats and Doubles will be handled differently
            // in hCNumeric::getValueFromStr().
            if (vt == variableType_t.vT_Unsigned || vt == variableType_t.vT_Integer || vt == variableType_t.vT_Index)
            {
                /*
                 * Trim any leading spaces
                 *
                 * NOTE:  This code may never be used if the user interface
                 * has already performed a trim left on the input string, but
                 * better be certain it the spaces are removed.
                 */
                loc = locStr.IndexOf(" ");
                // We found the position of the first non-space value
                if (loc != -1)  // loc = 0 means there are no "leading" spaces
                {
                    locStr = locStr.Substring(loc); // update the input string without the leading zeros            
                }

                // Check to see if there is a hexadecimal sign in the value
                char[] bc = { 'x', 'X' };
                loc = locStr.IndexOfAny(bc);
                if (loc != -1)
                {
                    /* 
                     * We found a hexadecimal sign!
                     */

                    //update the locStr without the hexadecimal sign
                    locStr = locStr.Substring(loc + 1);
                }

                // Check to see if there is a negative character in the value
                loc = locStr.IndexOf("-");
                if ((loc) != -1)
                {
                    /* 
                     * We found a negative character!
                     */


                    // Check to determine if empty string
                    if (locStr.Length <= 0)
                    {
                        /*
                         * There is no value after the negative character.
                         * This is a parsing error!
                         */
                        return false;
                    }
                    //isNegative = true;
                }

                // Trim any leading zeros ( and after any minus sign)
                loc = locStr.IndexOf("0");
                // We found the position of the first non-zero value
                if (loc != -1)  // loc = 0 means there are no "leading" zeros
                {
                    locStr = locStr.Substring(loc); // update the input string without the leading zeros
                }
            }

            // Any change to the locStr must be assigned back to the inputStr
            inputStr = locStr;

            return true;
        }

        CValueVarient getValueFromStr(string inputStr)
        {
            CValueVarient retVal = new CValueVarient();
            string wrkStr, editFormat = "", lenMod;
            variableType_t vt;
            int dp, prec, h, ft;
            int maxChars = 0, rightOdec = 0, fmtWidth = 0;
            //bool isInt = true;
            //wchar_t fmt = 0xffff, precStr[];
            char fmt;
            char[] precStr = new char[64];

            // Ensure that the input string is a valid numerical
            // string, POB - 4/28/2014
            if (validateTrim(ref inputStr))
            {
                /*
                 * This is a parsing error!
                 */
                retVal.vIsValid = false;
                return retVal;
            }

            //editFormat = getEditFormat();
            string dsp = "", scn = "";
            ft = getFormatInfo(ref fmtWidth, ref rightOdec, ref maxChars, ref editFormat, ref scn, ref dsp);
            vt = VariableType();
            if (editFormat.Length <= 0 || ft < 0) // a getFormatInfo error
            {// bad or missing edit format      
                editFormat = "";
                rightOdec = -1;// non existent
                if (vt == variableType_t.vT_Integer)
                {
                    fmt = 'd';
                    lenMod = "I64";
                    maxChars = digits4bytes[VariableSize()];
                }
                else if (vt == variableType_t.vT_Unsigned || vt == variableType_t.vT_Index)
                {
                    fmt = 'u';
                    lenMod = "I64";
                    maxChars = digits4bytes[VariableSize()] - 1;// neg sign uneeded
                }
                else if (vt == variableType_t.vT_FloatgPt)
                {
                    fmt = 'f';// edit format desired
                    lenMod = "";
                    rightOdec = 6;
                    maxChars = 12;
                }
                else if (vt == variableType_t.vT_Double)
                {
                    fmt = 'f';// edit format desired
                    lenMod = "";
                    rightOdec = 8;
                    maxChars = 18;
                }
                else // unsupported
                {
                    retVal.vIsValid = false;
                    return retVal;
                }
                //char[] buffer = new char[65];

                editFormat = "%";
                editFormat += maxChars.ToString();// _itow(maxChars, buffer, 10);// max-fieldWidth
                editFormat += lenMod;// length modifier (of receiving var)
                editFormat += fmt;// conversion type
                ft = fmt;
            }
            else
                fmt = (char)ft;//////

            wrkStr = scn;
            //transform(wrkStr.begin(), wrkStr.end(), wrkStr.begin(), tolower);
            wrkStr = wrkStr.ToLower();

            /*  // can't happen...filtered above
                if ( ft < 0 )
                {
                    retLoc = wrkStr.find_last_of("diouxefg");// last instance of any of the letters
                    if (retLoc == string::npos)
                    {
                        retVal.vIsValid = false;// error return <no legal formatting characters>
                        return retVal;
                    }
                    else
                        fmt = wrkStr[retLoc];// lower case version
                }
                // fmt is OK
            **/
            if (fmt == 'e' || fmt == 'f' || fmt == 'g' ||
                fmt == 'E' || fmt == 'F' || fmt == 'G')
            {// float format--- measure input-string's precision
                //isInt = false;
                dp = inputStr.IndexOf('.');
                prec = 0;
                if (dp != -1)
                {
                    for (prec = 0, h = dp + 1; h < (int)(inputStr.Length) && prec < 64; h++)
                    {
                        if (char.IsDigit(inputStr[h]))//////
                        {
                            precStr[prec++] = inputStr[h];
                            precStr[prec] = '\0';
                        }
                        else
                            break; // out of for-loop on any non-digit
                    }
                    // prec is string-length of digits to right of DP
                }
                //else -- no dp means no precision - leave prec zero

                if (prec > rightOdec && dp != -1 && rightOdec >= 0)
                {// so limit the precision to format amount
                    wrkStr = inputStr.Substring(0, dp + 1);
                    wrkStr += inputStr.Substring(dp + 1, rightOdec);
                    inputStr = wrkStr;
                }

                // stevev 28aug08 - apparent typo....if ( editFormat.length() > maxChars )
                if ((ushort)maxChars > 0 && inputStr.Length > maxChars)
                {
                    retVal.vIsValid = false;// error return <bigger than format>
                    return retVal;
                }

                // else - process the string
                double ffL = 0.0;
                //string endPtr = null;
                string strtPtr = inputStr;

                // scanf only goes into floats.  widestring to double

                try
                {
                    ffL = Convert.ToDouble(strtPtr);// wcstod(strtPtr, &endPtr);// handles 'dDeE' as exponent delimiter
                }

                catch
                {
                    retVal.vIsValid = false;
                }

                // stevev 26apr11 round to the rightOdec location
                if (rightOdec >= 0)
                {

                    // note - we must round to edit format and then again if we convert to int
                    ffL = Common.roundDbl(ffL, rightOdec);
                }

                //if (inputStr.c_str() == endPtr || ffL == HUGE_VAL || ffL == -HUGE_VAL || *endPtr != 0)
                if (/*strtPtr == endPtr || */!(ffL >= -double.MaxValue && ffL <= double.MaxValue)/* || endPtr != null*/)
                {                                                    // J.U. to more strict invalidate
                    //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: getValueFromStr did not decode.|%s|\n", inputStr.c_str());
                    retVal.vIsValid = false;
                }
                else
                {
                    retVal.SetValue((double)ffL, valueType_t.isFloatConst);
                    retVal.vIsValid = true;
                }
            }
            else // not a float edit format
            {
                Int64 sfL = 0;

                //verify the I64 is in the non=float versions
                int loc = scn.IndexOf("I64");
                if (loc == -1)
                {
                    //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: getValueFromStr (i) needs to add I64\n");
                    retVal.vIsValid = false;
                }

                /*  stevev 07feb11 - scanf rolls over when a number larger than I64_MAX is typed in.
                    eg '9223372036854775809' comes out as 1 (_I64_MAX = 9223372036854775807)
                    This is adequate for the host test because "the value can抰 be changed to that" 
                    too large number.
                */
                ScanFormatted sf = new ScanFormatted();
                sf.Parse(inputStr, scn);

                if (sf.Results.Count == 0)
                {
                    //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: getValueFromStr (i) did not decode.|%s|\n", inputStr.c_str());
                    retVal.vIsValid = false;
                }
                else // success
                {
                    //char[] str64Val = new char[21];
                    string wsVal;
                    UInt64 u64Val = 0;
                    sfL = (Int64)sf.Results[0];
                    // conversion from signed to unsigned storage
                    if (vt == variableType_t.vT_Unsigned || vt == variableType_t.vT_Index)
                    {
                        retVal.SetValue(sfL, valueType_t.isIntConst);
                        retVal.vIsUnsigned = true;

                        u64Val = (UInt64)sfL;
                        //_ui64tow(u64Val, str64Val, 10);
                        //wsVal = new string(str64Val);
                        wsVal = u64Val.ToString();
                    }
                    else
                    {
                        retVal.vIsUnsigned = false;
                        retVal.SetValue(sfL, valueType_t.isIntConst);

                        //_i64tow(retVal, str64Val, 10);
                        //wsVal = new string(str64Val);
                        wsVal = retVal.GetInt().ToString();
                    }
                    char[] bc = { 'x', 'X' };
                    if ((scn.IndexOfAny(bc)) != -1)
                    {
                        /*
                         * We found a hexidecimal formatted string.
                         * This format could be applied to either an integer
                         * or an unsigned.
                         *
                         * For now, we are not evaluating this for swscanf
                         * overflow
                         */
                        //swprintf(str64Val,"%I64x",(int)retVal);
                        wsVal = inputStr;
                    }

                    /*
                     * Compare the input string entered by the user with the actual value.
                     * A difference will likely indicate a scanf rolls over.
                     * For DD Host Testing, we need to indicate to the user that it is out
                     * of range when the string entered by the user does not match the
                     * modified value.  Fixed defect #4502 and #4503, POB - 4/17/2014
                     */
                    if (wsVal == inputStr)
                    {
                        retVal.vIsValid = false;
                    }
                    else
                    {
                        retVal.vIsValid = true;
                    }
                }
            }
            return retVal;
        }

        void setDisplayValue(CValueVarient newValue)// from UI w/ rangetest 
        {
            if (isInRange(newValue))
            {
                setDispValue(newValue);
            }
        }

        public void setDisplayValueString(string s)// from UI with range test
        {
            CValueVarient aValue;
            aValue = getValueFromStr(s);
            if (aValue.vIsValid)
            {
                setDisplayValue(aValue);
            }
            else
            {
                if (aValue.vType == valueType_t.isVeryLong)
                {
                    /*
                     * Values were assigned so there must have been
                     * an overflow issue. Inform the user of an out
                     * of range issue.
                     * Fixed defect #4502 and #4503, POB - 4/17/2014
                     */
                    //return APP_OUT_OF_RANGE_ERR;
                }
                else
                {
                    //return APP_PARSE_FAILURE;
                }
            }
        }

        public void setTimeDisplayValueString(CValueVarient cv)
        {
            CValueVarient aValue = new CValueVarient();
            string s = cv.GetString();
            if (cv.GetString().Length <= 0)
            {
                /*
                 * Blank values
                 * We do not nofity the user of an issue, we simply restore the previous value
                 * This is now consistent with how the floats and integers are being handled, POB - 4/18/2014
                 */
            }
            if (TimeScaleID > 0)
            {
                setDisplayValueString(s);
            }
            else
            {
                string fmt = "";
                string ex = "";
                string wsRemaining;
                string pRet;


                if (TimeScaleFormat != null)
                {
                    getFmtStr(ref ex);
                    if (ex.Length > 0)
                    {
                        fmt = ex;  // we already did it all
                    }
                }
                else
                {
                    fmt = "H:M:S";// MS doesn't support this ..."%T";//stevev 27jan10
                }

                ushort curVal = (ushort)vValue.GetUInt();// (ushort)Value;  // was Wrt_Val;before but roll-overs were cumulative
                ushort remainingValue;
                tm tS = new tm(), tR = new tm();
                addPercent(ref fmt);

                /*
                 * Return wchar_t *
                 *  NULL: Did not complete the parsing format, out of range value or bad user 
                 *        input string
                 *  string: Completed the parsing of the applied format.
                 * Capture the results, POB - 4/18/2014
                 */
                pRet = Common.strptime(s, fmt, tS);// char to time...compiled in UI

                if (pRet != null)
                {
                    /* 
                     * We have a success!
                     * Parsing has completed on the applied format, POB - 4/18/2014
                     */
                    wsRemaining = pRet;

                    if (wsRemaining != "")
                    {
                        /* 
                         * The string is not empty...
                         * If it was truly a succcess, the string will be empty. 
                         * This likely indicates that there is a problem with the
                         * last time value as since there are left over characters.
                         * We need to indicate a failure, POB - 4/18/2014
                         */
                        pRet = null;
                    }
                }

                if (pRet != null)
                {
                    timeScale_t fmtMsk = timeScale_t.tsNo_Scale;
                    //ex can be MT:: getActiveValues(ex, fmtMsk);
                    Common.getActiveValues(fmt, ref fmtMsk);//test the same format as we used to extract

                    Common.fillStruct(ref tR, curVal);
                    remainingValue = (ushort)(curVal - Common.extractStruct(tR));

                    // was::> locVar = (unsigned)( ((((tS.tm_hour*60)+tS.tm_min)*60)+tS.tm_sec)*32000 );
                    if ((fmtMsk & timeScale_t.tsHr_Scale) != 0)
                    {
                        tR.tm_hour = tS.tm_hour;
                    }
                    if ((fmtMsk & timeScale_t.ts_I_Scale) != 0)
                    {
                        if (((fmtMsk & timeScale_t.ts_p_Scale) == 0) && tR.tm_hour >= 12) // Check current value when %p is absent
                        {
                            /* 
                             * strptime() will return values 0 - 11 when user enters 12
                             * for %I.  Ensure that PM does not get changed to AM in the
                             * process.  Fixed defect #4511, POB - 4/21/2014
                             */
                            tR.tm_hour = tS.tm_hour + 12;
                        }
                        else
                        {
                            tR.tm_hour = tS.tm_hour;
                        }
                    }
                    if ((fmtMsk & timeScale_t.tsMinScale) != 0)
                    {
                        tR.tm_min = tS.tm_min;
                    }
                    if ((fmtMsk & timeScale_t.tsSecScale) != 0)
                    {
                        tR.tm_sec = tS.tm_sec;
                    }

                    aValue.SetValue((ushort)(remainingValue + Common.extractStruct(tR)), valueType_t.isIntConst);

                    // Parsing Success
                    setDispValue(aValue);
                }
                else
                {
                    // Parsing Failure
                    aValue.vIsValid = false;
                }
            }

        }

        public void setDateDisplayValueString(CValueVarient cv)
        {
            uint loc = Common.datestr2int(cv.GetString(), dtEditFmt_t.usFormat);
            CValueVarient vv = new CValueVarient();
            vv.SetValue(loc, valueType_t.isIntConst);

            if (cv.GetString().Length <= 0)
            {
                /*
                 * Blank values
                 * We do not nofity the user of an issue, we simply restore the previous value
                 * This is now consistent with how the floats and integers are being handled, POB - 4/18/2014
                 */
            }

            if (isInRange(vv))
            {
                setDispValue(vv);// will deal with locals and their notification
            }
        }

        public bool isInRange(CValueVarient cv)//////??????
        {
            if (vValue.isInteger() && VarMin.byOpCode == Common.INTCST_OPCODE)
            {
                if (vValue.GetUInt() >= VarMin.ulConst && vValue.GetUInt() <= VarMax.ulConst)
                {
                    return true;
                }
            }
            else if (vValue.isFloat() && VarMin.byOpCode == Common.FPCST_OPCODE)
            {
                if (vValue.GetFloat() >= VarMin.fConst && vValue.GetFloat() <= VarMax.fConst)
                {
                    return true;
                }
            }

            return false;
        }

        public hCcommandDescriptor getRdCmd(int cNum)
        {
            hCcommandDescriptor retDesc = new hCcommandDescriptor();
            retDesc.cmdNumber = 0xffff; // the error return
            foreach (hCcommandDescriptor iT in rdCmdList)
            {
                if (iT.cmdNumber == cNum)
                {
                    retDesc = iT;
                    break; // out of for
                }
            }
            return retDesc;
        }

        public hCcommandDescriptor getWrCmd(int cNum)
        {
            hCcommandDescriptor retDesc = new hCcommandDescriptor();
            retDesc.cmdNumber = 0xffff; // the error return
            foreach (hCcommandDescriptor iT in wrCmdList)
            {
                if (iT.cmdNumber == cNum)
                {
                    retDesc = iT;
                    break; // out of for
                }
            }
            return retDesc;
        }


        public hCcommandDescriptor getRdCmd()
        {
            return (readCommand);
        }
        public hCcommandDescriptor getWrCmd()
        {
            return (writeCommand);
        }

        public void SetReadCommand(hCcommandDescriptor c)
        {
            readCommand = c;
        }

        public void SetWriteCommand(hCcommandDescriptor c)
        {
            writeCommand = c;
        }

        public void setWritable(bool b)
        {
            bWritable = b;
        }

        public bool isWritable()
        {
            return bWritable;
        }

        public bool IsLocal()
        {
            if ((varclass & (int)maskClass_t.maskLocal) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasReadCmd()
        {
            if (readCommand.cmdNumber >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setunit(string u)
        {
            unit = u;
        }

        public string getUnitStr(uint value)
        {
            switch (datatype)
            {
                case variableType_t.vT_Ascii:
                    unit = vValue.GetString();
                    break;

                case variableType_t.vT_Enumerated:
                    string ustr;
                    if (unitlist != null && unitlist.Count > 0)
                    {
                        if (unitlist.TryGetValue(value, out ustr))
                        {
                            unit = ustr;
                        }
                    }
                    else if (unitidlist != null && unitidlist.Count > 0)
                    {
                        uint unid;
                        CDDLVar unVar = null;
                        if (unitidlist.TryGetValue(value, out unid))
                        {
                            if (pDev.getVarbyID(unid, ref unVar))
                            {
                                unit = unVar.getUnitStr(value);
                            }
                        }
                    }

                    break;

                default:
                    break;
            }

            return unit;
        }

        public string getUnitStr()
        {
            switch (datatype)
            {
                case variableType_t.vT_Ascii:
                    unit = vValue.GetString();
                    break;

                case variableType_t.vT_Enumerated:
                    uint ui = vValue.GetUInt();
                    string ustr;
                    if (unitlist != null && unitlist.Count > 0)
                    {
                        if (unitlist.TryGetValue(ui, out ustr))
                        {
                            unit = ustr;
                        }
                    }
                    else if (unitidlist != null && unitidlist.Count > 0)
                    {
                        uint unid;
                        CDDLVar unVar = null;
                        if (unitidlist.TryGetValue(ui, out unid))
                        {
                            if (pDev.getVarbyID(unid, ref unVar))
                            {
                                unit = unVar.getUnitStr(ui);
                            }
                        }
                    }

                    break;

                default:
                    break;
            }

            unit = Common.GetLangStr(unit);

            return unit;
        }

        public string getunit()
        {
            return unit;
        }

        public int getEditFormatInfo(ref int fmtWidth, ref int rightOdec, ref string edt)
        {
            int mx = 0, rr = 0;
            string scanf = "", dispf = "";
            rr = getFormatInfo(ref fmtWidth, ref rightOdec, ref mx, ref edt, ref scanf, ref dispf);
            return rr;
        }

        public int getDispFormatInfo(ref int fmtWidth, ref int rightOdec, ref string dft)
        {
            int mx = 0, rr = 0;
            string scanf = "", editf = "";
            rr = getFormatInfo(ref fmtWidth, ref rightOdec, ref mx, ref editf, ref scanf, ref dft);
            return rr;
        }

        int getMethodList(varAttrType_t vat, ref List<hCmethodCall> methList)
        {
            int rc = Common.SUCCESS;

            List<CDDLBase> localItmList = new List<CDDLBase>();
            hCmethodCall wrkMthCall = new hCmethodCall();
            CValueVarient retVal = new CValueVarient();
            retVal.SetValue(getID(), valueType_t.isSymID);    // we use it first for the self parameter
            methodCallSource_t srcTyp = methodCallSource_t.msrc_ACTION; // sjv 06mar07

            //hCattrBase* pAB = getaAttr(vat);
            //if (pAB != NULL)
            {// we have an attribute

                switch (vat)
                {
                    case varAttrType_t.varAttrPreReadAct:
                        {
                            //hCattrVarPreRdAct* pActions = (hCattrVarPreRdAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                            srcTyp = methodCallSource_t.msrc_CMD_ACT;
                        }
                        break;
                    case varAttrType_t.varAttrPostReadAct:        // count = 0
                        {
                            //hCattrVarPostRdAct* pActions = (hCattrVarPostRdAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                            srcTyp = methodCallSource_t.msrc_CMD_ACT;
                        }
                        break;
                    case varAttrType_t.varAttrPreWriteAct:    //10// count = 0
                        {
                            //hCattrVarPreWrAct* pActions = (hCattrVarPreWrAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                            srcTyp = methodCallSource_t.msrc_CMD_ACT;
                        }
                        break;
                    case varAttrType_t.varAttrPostWriteAct:       // count = 0
                        {
                            //hCattrVarPostWrAct* pActions = (hCattrVarPostWrAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                            srcTyp = methodCallSource_t.msrc_CMD_ACT;
                        }
                        break;
                    case varAttrType_t.varAttrPreEditAct:         // count = 0
                        {
                            //hCattrVarPreEditAct* pActions = (hCattrVarPreEditAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                        }
                        break;
                    case varAttrType_t.varAttrPostEditAct:        // count = 0
                        {
                            //hCattrVarPostEditAct* pActions = (hCattrVarPostEditAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                        }
                        break;
                    case varAttrType_t.varAttrRefreshAct:     // count = 0
                        {
                            //hCattrVarRefreshAct* pActions = (hCattrVarRefreshAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                        }
                        break;
                    case varAttrType_t.varAttrPostRequestAct:
                        {
                            //hCattrVarPostRequestAct* pActions = (hCattrVarPostRequestAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                        }
                        break;
                    case varAttrType_t.varAttrPostUserAct:
                        {
                            //hCattrVarPostUserAct* pActions = (hCattrVarPostUserAct*)pAB;
                            //rc = pActions.getItemPtrs(localItmList);
                        }
                        break;

                    default:
                        {
                            //LOGIT(CERR_LOG, "getMethodList: has UNKNOWN %s attribute.\n", varAttrStrings[vat]);
                            rc = Common.FAILURE;
                        }
                        break;
                }// end switch       


                if (rc == Common.SUCCESS && localItmList.Count > 0)
                {//for each in the list
                    foreach (CDDLBase iT in localItmList)
                    {//iT isa ptr 2a ptr 2a hCitemBase
                        if (iT.eType != nitype.nMethod)
                        {
                            //LOGIF(LOGP_NOT_TOK)(CERR_LOG, "ERROR: non-Method type in a Pre/Post action list.\n");
                        }
                        else
                        {// we be good
                         //  fill a method call with varID, msrc_ACTION,methodID  
                            wrkMthCall.methodID = (iT).getID();
                            wrkMthCall.source = srcTyp;  // sjv 06mar07 - usually:  msrc_ACTION;
                            wrkMthCall.paramList.Add(retVal);
                            methList.Add(wrkMthCall);
                            //wrkMthCall.clear();
                        }
                    }// next method
                }// else return the error

            }// else - no actions, just return SUCCESS
            return rc;
        }

        public void getActionList(varAttrType_t actionType, ref List<int> actionList)
        {
            int rc;
            List<hCmethodCall> methList = new List<hCmethodCall>();

            //vector<hCmethodCall> :: iterator it;

            rc = getMethodList(actionType, ref methList);

            if (rc == Common.SUCCESS && methList.Count > 0)
            {
                foreach (hCmethodCall it in methList)
                {
                    int actionID = 0;

                    actionID = (int)it.methodID;

                    actionList.Add(actionID);
                }
            }
        }

        public override string GetUnitStr()
        {
            return unit;
        }

        public variableType_t VariableType()
        {
            return (datatype);
        }//variableType_t

        public void SetVariableType(variableType_t vt)
        {
            datatype = vt;
            switch (datatype)
            {
                case variableType_t.vT_Ascii:
                case variableType_t.vT_Password:
                    vValue.vType = valueType_t.isWideString;
                    break;

                case variableType_t.vT_PackedAscii:
                    vValue.vType = valueType_t.isPackedString;
                    break;

                case variableType_t.vT_BitEnumerated:
                case variableType_t.vT_Enumerated:
                    vValue.vType = valueType_t.isIntConst;
                    break;

                case variableType_t.vT_Boolean:
                    vValue.vType = valueType_t.isBool;
                    break;

                case variableType_t.vT_FloatgPt:
                case variableType_t.vT_Double:
                    vValue.vType = valueType_t.isFloatConst;
                    break;

                case variableType_t.vT_Duration:
                case variableType_t.vT_DateAndTime:
                case variableType_t.vT_EUC:
                case variableType_t.vT_HartDate:
                case variableType_t.vT_Index:
                case variableType_t.vT_Integer:
                case variableType_t.vT_Time:
                case variableType_t.vT_TimeValue:
                case variableType_t.vT_Unsigned:
                    vValue.vType = valueType_t.isIntConst;
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
        }//variableType_t

        public void setVarsize(uint size)
        {
            varSize = size;
            vValue.vSize = (int)varSize;
        }

        public uint VariableSize()
        {
            return varSize;
        }

        public override bool IsVariable()
        {
            return true;
        }

        public int getSize()
        {
            if (datatype == variableType_t.vT_PackedAscii)
            {
                return (int)varSize / 4 * 3;
            }
            else
            {
                return (int)varSize;
            }
        }

        public bool IsDynamic()//the only class the counts          
        {// this is a PROCURE call - any unresolved conditional will return default false

            if ((varclass & (int)maskClass_t.maskDynamic) != 0)
            {
                return true;
            }
            else
            {
                return false;//01dec06-was 'return true;' found by Vibhor
            }
            //else - class has output an error, just return the default;
        }

        public CValueVarient getRealValue()
        {
            CValueVarient retVal = new CValueVarient();
            retVal.vType = valueType_t.isFloatConst;
            //retVal.vValue.fFloatConst = fValue;
            retVal.vSize = getSize();
            //if (IsValidTest()) 
            if (true)
            {
                retVal.vIsValid = true;
            }
            else
            {
                //retVal.vIsValid = false;
            }
            return retVal;
        }

        public string formatValue(CValueVarient incoming, string format)
        {
            string fchar; // 1.79e305 makes for a long string
            if (format == "")
            {
                returnString = "";
                return returnString;
            }

            //int w,p,m;
            //string fmtStr, fIStr;

            //int t = getFormatInfo(w,p,m, NULL,NULL, &fIStr); //we only need display here

            // determine int or double output required
            char[] ba = { 'e', 'E', 'f', 'F', 'g', 'G' };
            if (format.IndexOfAny(ba) != -1)
            {// it's a float output
             // bug 2527 - stack blown (a LOT) with swprintf()
             //int r = _snwprintf(fchar, 350, format.c_str(), (double)incoming);
                fchar = String.Format(format, (double)incoming.GetDouble());
                if (fchar.Length > 350 || fchar.Length < 0)
                {
                    //_snwprintf(fchar, 64, "%g", (double)incoming);
                    fchar = String.Format("%g", (double)incoming.GetDouble());
                }
            }
            else
            {// it's a (long long) int format
                if (incoming.vIsUnsigned)
                //swprintf(fchar, format.c_str(), (UINT64)incoming);
                {
                    fchar = String.Format(format, (UInt64)incoming.GetUInt());
                }
                else
                //swprintf(fchar, format.c_str(), (INT64)incoming);
                {
                    fchar = String.Format(format, (Int64)incoming.GetInt());
                }
            }
            returnString = fchar;
            return returnString;
        }

        public int getFormatInfo(ref int fmtWidth, ref int rightOdec, ref int maxChars,
            ref string pEditFmtStr, ref string pScanFmtStr, ref string pDispFmtStr)
        {
            int retVal = -1;

            int dwidth = 0, dprecision = 0;
            string dflags = "", dLmod = "";
            char dconvert = '\0';

            int ewidth = 0, eprecision = 0;
            string eflags = "", eLmod = "";
            char econvert = '\0';

            string dfmt, efmt, d1fmt;// has to be different so any use of 'efmt = dfmt;' won't crash
                                     // parseFormat(efmt...) <the I^$ in the format gives crash>
            {
                if (VariableType() != variableType_t.vT_Enumerated && VariableType() != variableType_t.vT_BitEnumerated)
                {
                    ;
                }
                uint vs = VariableSize();
                switch (VariableType())
                {
                    case variableType_t.vT_Integer:
                        {
                            if (vs == 1)
                                dfmt = "4d";
                            else if (vs == 2)
                                dfmt = "6d";
                            else if (vs > 2 && vs <= 4)
                                dfmt = "11d";
                            else
                                dfmt = "20d";
                        }
                        break;
                    case variableType_t.vT_Unsigned:
                    case variableType_t.vT_Enumerated:
                    case variableType_t.vT_BitEnumerated:
                    case variableType_t.vT_Index:
                        {
                            if (vs == 1)
                                dfmt = "4u";
                            else if (vs == 2)
                                dfmt = "6u";
                            else if (vs > 2 && vs <= 4)
                                dfmt = "11u";
                            else
                                dfmt = "20u";
                        }
                        break;
                    case variableType_t.vT_FloatgPt:
                        {
                            dfmt = "12.5g";
                        }
                        break;
                    case variableType_t.vT_Double:
                        {
                            dfmt = "18.8g";
                        }
                        break;
                    default:
                        {
                            dfmt = "";
                        }
                        break;
                }//endswitch
            }

            d1fmt = parseFormat(dfmt, ref dflags, ref dwidth, ref dprecision, ref dLmod, ref dconvert);
            if (pDispFmtStr != null)
                pDispFmtStr = "%" + d1fmt;
            // else the caller doesn't want the info

            efmt = dfmt;// else value
            if (VariableType() != variableType_t.vT_Enumerated && VariableType() != variableType_t.vT_BitEnumerated)
            {
                ;
            }

            efmt = parseFormat(efmt, ref eflags, ref ewidth, ref eprecision, ref eLmod, ref econvert);
            if (pEditFmtStr != null)
                pEditFmtStr = "%" + efmt;
            // else the caller doesn't want the info

            CValueVarient hi = new CValueVarient(), lo = new CValueVarient();
            highest(ref hi);//natural default range
            lowest(ref lo);

            string hiStr, loStr;

            //isDisplay - calculate the max characters
            if (pEditFmtStr != null)
            {
                hiStr = formatValue(hi, pEditFmtStr);
                loStr = formatValue(lo, pEditFmtStr);
                maxChars = Math.Max(hiStr.Length, loStr.Length);//longest natural value disp formt'd
                fmtWidth = ewidth;
                rightOdec = eprecision;
                retVal = econvert;
                if (pScanFmtStr != null)
                {// % optionalwidth optional length-modifier conversion-spec
                    pScanFmtStr = "%";// start
                    if (ewidth > 0)
                    {
                        //wchar_t number[64]; memset(number, 0, sizeof(wchar_t) * 64);
                        //_itow(ewidth, number, 10);// optional max width
                        string number = ewidth.ToString();
                        pScanFmtStr += number;
                    }// otherwise empty
                    pScanFmtStr += eLmod;   // length-modifier...usually empty
                    pScanFmtStr += econvert;// the conversion type specifier
                }
            }
            else // edit format is null
            if (pDispFmtStr != null)
            {
                hiStr = formatValue(hi, pDispFmtStr);
                loStr = formatValue(lo, pDispFmtStr);
                maxChars = Math.Max(hiStr.Length, loStr.Length);//longest natural value disp formt'd
                fmtWidth = dwidth;
                rightOdec = dprecision;
                retVal = dconvert;
            }
            else// we have neither format
            {
                fmtWidth = 0;
                rightOdec = -1;
                retVal = -1;
            }
            return retVal;
        }
        //string			processFormat(string fmtStr);// helper for getFormatInfo

        void highest(ref CValueVarient retVal)
        {
            int s = (int)VariableSize();// assume 1 - 8 for integers
            if (isSigned)
            {
                retVal.SetValue((Int64)(0xffffffffffffffff >> (((8 - s) * 8) + 1)), valueType_t.isIntConst);
            }
            else
            {
                retVal.SetValue((Int64)(0xffffffffffffffff >> (((8 - s) * 8))), valueType_t.isIntConst);
            }
        }

        void lowest(ref CValueVarient retVal)
        {
            int s = (int)VariableSize();// assume 1 - 8 for integers
            if (isSigned)
            {
                retVal.SetValue((Int64)(0xffffffffffffffff << ((s * 8) - 1)), valueType_t.isIntConst);//  most negative number available
            }
            else
            {
                retVal.SetValue((Int64)(0), valueType_t.isIntConst);// smallest possible number
            }
        }

        string parseFormat(string fmtStr,
            /*outputs::*/           ref string flgs, ref int wid, ref int prec, ref string lm, ref char type)
        {
            string numeric = "";
            int z, strt = 0;

            string fIStr = fmtStr;
            if (fmtStr.Length <= 0)
            {
                //DEBUGLOG(CERR_LOG, "ERROR: process format called with an empty format string.\n");
                fIStr = "";
                return fIStr;
            }
            variableType_t vt = (variableType_t)VariableType();

            // deal with percent sign..if it was included
            while (true) // use break and continue
            {
                strt = fmtStr.IndexOf('%');
                if (strt != -1)// no %
                {
                    if (fmtStr.Length > 0)
                    {
                        fmtStr = '%' + fmtStr;
                        strt = 1;
                        break;// we're done here
                    }
                    else // the entire string was illegal(never a % in an empty string)
                    {
                        fIStr = "";
                        return fIStr;// not much to do
                    }
                }
                else // there is a %
                {
                    if (strt > 0)// past leading stuff before '%'
                    {
                        fmtStr = fmtStr.Substring(strt);// % to end
                        strt = 1;// point to location after %
                                 //  retLoc -= (fmt.length() - wrkStr.length());// compensate 
                    }
                    else // strt == 0...starts with a '%'
                    {
                        strt = 1;
                    }
                    if (fmtStr[strt] == '%')// embedded %%...strings aren't allowed here
                    {
                        strt++;// get past second percent
                        fmtStr = fmtStr.Substring(strt);// past second % to end
                                                        // loop to see if we have a real percent
                        strt = 0;
                    }
                    else
                    {
                        break; // we have what we need
                    }
                }
            }//wend
             // we should have a format string with a prepended % and strt pointing behind it

            string wrkStr = fmtStr;// get a working copy

            int dLoc, fLoc, useLoc;
            int XPos = -1;

            // Check string for upper case 'X'
            // Fixed defect #4505 and #4506, POB - 4/24/2014
            dLoc = wrkStr.IndexOf("X");
            if (dLoc != -1)
            {
                // We found an upper case 'X' in the format
                XPos = dLoc;
            }

            //transform(wrkStr.begin(), wrkStr.end(), wrkStr.begin(), tolower);// standard format
            wrkStr = wrkStr.ToLower();

            char[] ba = { 'd', 'i', 'o', 'u', 'x' };
            dLoc = wrkStr.LastIndexOfAny(ba);// last instance of any integer type
            char[] bb = { 'e', 'f', 'g' };
            fLoc = wrkStr.LastIndexOfAny(bb); // last instance of any floating type<a not allowed>
            if (dLoc == -1 && fLoc == -1)
            {
                //DEBUGLOG(CLOG_LOG, "ERROR:  trying to parse a format with no legal type conversion.\n");
                fIStr = "";
                return fIStr;// error return <no legal formatting characters>
            }
            else if (fLoc == -1)// it's a integer type character only
            {
                useLoc = dLoc;

                // Fixed defect #4505 and #4506, POB - 4/24/2014
                if (XPos > 0)
                {
                    // Restore the upper case 'X' in the format
                    //wrkStr.Replace(XPos, 1, "X");
                    wrkStr = wrkStr.Remove(XPos, 1);
                    wrkStr = wrkStr.Insert(XPos, "X");
                }

                type = wrkStr[dLoc];// tolower should never change the length
            }
            else// has to be float or both float and integer (defaults to float)
            {
                useLoc = fLoc;
                type = wrkStr[fLoc];// tolower should never change the length
            }

            string flagList = " +-0#";// all the legal flag characters
            //string lenModLst = "hljzt";
            prec = -1;// default value
            wid = 0;// ditto
                    // scan from left to right, filling in the piece-parts
            int state = 1;// flags (skip percent sign)
            z = 0;
            while (strt <= useLoc)
            {
                if (state == 1 && strt == useLoc)
                {
                    break; // possible flags & type only
                }

                if (state == 1)// we are looking for flags
                {
                    int loc = flagList.IndexOf(wrkStr[strt]);
                    if (loc != -1)// we found a flag
                    {
                        flgs = flgs + wrkStr[strt++];// keep looking
                    }
                    else // not flags
                    {
                        state = 2;// then fall thru to deal with it
                    }
                }

                if (state == 2 && strt == useLoc) // looking for width
                {
                    if (z > 0)
                    {
                        wid = Convert.ToInt32(numeric);//_wtoi(numeric);
                        z = 0;
                    }// else no width
                    break;// no more to do
                }

                if (state == 2 && strt < useLoc) // looking for width
                {
                    if (Char.IsDigit(wrkStr[strt]))
                    {
                        //numeric[z++] = wrkStr[strt++];
                        wrkStr = wrkStr.Remove(z);
                        wrkStr = wrkStr.Insert(z - 1, wrkStr[strt++].ToString());
                        z++;
                    }
                    else// can't be width anymore
                    if (wrkStr[strt] == '.')
                    {
                        state = 3;// precision
                        strt++;
                    }
                    else
                    {// leave prec default -1
                        state = 4;// length modifier, fall thru
                    }
                    if (state > 2 && z > 0)
                    {
                        wid = Convert.ToInt32(numeric);//_wtoi(numeric);
                        z = 0;
                    }// else it's still getting numbers or there is no width
                }

                if (state == 3 && strt == useLoc) // looking for precision
                {
                    if (z > 0)
                    {
                        prec = Convert.ToInt32(numeric);//_wtoi(numeric);
                        z = 0;
                    }// else there is no precision
                    break;// nothing else to do
                }

                if (state == 3 && strt < useLoc) // looking for precision
                {
                    if (Char.IsDigit(wrkStr[strt]))
                    {
                        //numeric[z++] = wrkStr[strt++];
                        //numeric[z] = '\0';
                        wrkStr = wrkStr.Remove(z);
                        wrkStr = wrkStr.Insert(z - 1, wrkStr[strt++].ToString());
                        z++;
                    }
                    else
                    {
                        state = 4;// length modifier or type
                    }
                    if (state > 3 && z > 0)
                    {
                        prec = Convert.ToInt32(numeric);//_wtoi(numeric);
                        z = 0;
                    }// else it's still getting numbers or there is no precision
                }
                if (state == 4 && strt == useLoc) // looking for length modifiers - not legal
                {
                    break;// we're done
                }

                if (state == 4 && strt < useLoc) // looking for length modifiers - not legal
                {// from here to useLoc (the type value) all illegal
                 // illegal, discard
                    char loc = wrkStr[strt];
                    // note that %c in a wide version printf is supposed to represent a wchar_t
                    string tmps = wrkStr.Substring(strt + 1);
                    wrkStr = wrkStr.Substring(0, strt);
                    wrkStr = wrkStr + tmps;// character removed
                                           // do the actual return value too
                    tmps = fIStr.Substring(strt + 1);
                    fIStr = fIStr.Substring(0, strt);
                    fIStr = fIStr + tmps;// character removed
                    useLoc--;// it just moved left one...strt stays the same but its on the next char
                }

            }//wend

            if ((vt == variableType_t.vT_Integer || vt == variableType_t.vT_Unsigned || vt == variableType_t.vT_Index ||
                   vt == variableType_t.vT_Enumerated || vt == variableType_t.vT_BitEnumerated)//dloux
                                                                                               // it's an int variable and NOT a float format
               && !(type == 'f' || type == 'e' || type == 'g'))
            {
                // we are going to assume there is nothing but the format in the string -stevev 19jun08
                fIStr = fIStr.Substring(0, fIStr.Length - 1);// put everything but type into main string
                fIStr += "I64";                            // say we're passing in a long long
                fIStr += type;          // tack the type back on the end   
                lm = "I64";
            }
            else
            {
                ;// lm.erase();
            }

            return fIStr;
        }


        /*
        CValueVarient getDispValue(bool iHaveMutex = false)
        {
            int w, p, m;
            string fmtStr;
            int t = getFormatInfo(w, p, m, null, null, &fmtStr);

            CValueVarient retVal = new CValueVarient();
            retVal.vType = valueType_t.isFloatConst;
            retVal.vValue.fFloatConst = f_WrtVal;
            retVal.vSize = getSize();
            retVal = scaleValue(retVal, fmtStr);
            //if (IsValid())
            if(true)
                retVal.vIsValid = true;
            else
                //retVal.vIsValid = false;
            return retVal;
        }
        */
    }

    public class hCgrpItmBasedClass : CDDLBase /* class for collections,itemarrays, & files */
    {

    }


}
