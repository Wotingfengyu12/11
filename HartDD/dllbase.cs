using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

namespace FieldIot.HARTDD
{
    public enum DATA_QUALITY_T
    {
        DA_HAVE_DATA, 
        DA_NOT_VALID, 
        DA_STALE_OK, 
        DA_STALEUNK
    }

    public enum itemType_t
    {
        iT_ReservedZeta,        // 0
        iT_Variable,            // 1
        iT_Command,             // 2
        iT_Menu,                // 3
        iT_EditDisplay,         // 4
        iT_Method,              // 5
        iT_Refresh,/*relation*/ // 6
        iT_Unit,/*relation*/    // 7
        iT_WaO,                 // 8
        iT_ItemArray,           // 9
        iT_Collection,          //10
        iT_ReservedOne,             // specially used to get the image list
        iT_Block,               //12
        iT_Program,             //13
        iT_Record,              //14
        iT_Array,               //15
        iT_VariableList,        //16
        iT_ResponseCodes,       //17
        iT_Domain,              //18
        iT_Member,              //19
        iT_File,                //20
        iT_Chart,               //21
        iT_Graph,               //22
        iT_Axis,                //23
        iT_Waveform,            //24
        iT_Source,              //25
        iT_List,                //26
        iT_Grid,                //27
        iT_Image,               //28
        iT_Blob,                //29
        iT_PlugIn,              //30
        iT_Template,            //31
        iT_ReservedTwo,         //30
        iT_NotHartOne,
        iT_NotHartTwo,
        iT_NotHartThre,
        iT_NotHartFour,
        iT_ReservedThre,        //37
        iT_MaxType,             //  38
        /*
         *  Special object type values
         */
        iT_FormatObject = 128,
        iT_DeviceDirectory,     //129
        iT_BlockDirectory,      //130
        /* these additional ITYPES are used by DDS
         *       'resolve' function to build resolve trails 
         */
        iT_Parameter = 200,
        iT_ParameterList,       //201
        iT_BlockCharacteristic, //202

        iT_NotAnItemType = 255, // probably a constant
        iT_Critical_Item = 256

    }

    public enum varAttrType_t
    {                                      // default values
        varAttrClass = 0,  //*- none - required??
        varAttrHandling,           //*READ_HANDLING | WRITE_HANDLING
        varAttrConstUnit,          //*len=0, flags=0
        varAttrLabel,              //*BASE:  DEFAULT_STD_DICT_LABEL
        varAttrHelp,               //*BASE:  DEFAULT_STD_DICT_HELP
        varAttrWidth,   // 5	// 0 
        varAttrHeight,          // 0
        varAttrValidity,           //*BASE:  TRUE
        varAttrPreReadAct,         // count = 0
        varAttrPostReadAct,        // count = 0
        varAttrPreWriteAct,    //10// count = 0
        varAttrPostWriteAct,       // count = 0
        varAttrPreEditAct,         // count = 0
        varAttrPostEditAct,        // count = 0
        varAttrResponseCodes,       // 0---not supported by hart
        varAttrTypeSize,      //15 //*- none - required
        varAttrDisplay,            //*DEFAULT_STD_DICT_DISP_INT
                                   // DEFAULT_STD_DICT_DISP_UINT
                                   // DEFAULT_STD_DICT_DISP_FLOAT
                                   // DEFAULT_STD_DICT_DISP_DOUBLE
        varAttrEdit,               //*DEFAULT_STD_DICT_EDIT_INT
                                   // DEFAULT_STD_DICT_EDIT_UINT
                                   // DEFAULT_STD_DICT_EDIT_FLOAT
                                   // DEFAULT_STD_DICT_EDIT_DOUBLE
        varAttrMinValue,           // count = 0
        varAttrMaxValue,           // count = 0
        varAttrScaling,        //20//*1 byte int with value of 1
        varAttrEnums,              // count = 0
        varAttrIndexItemArray,     // 22
        varAttrDefaultValue,
        varAttrRefreshAct,
        varAttrDebugInfo,          // 25

        varAttrPostRequestAct,
        varAttrPostUserAct,

        varAttrTimeFormat,          // added 02jun08
        varAttrTimeScale,
        varAttrPrivate,
        varAttrVisibility,
        varAttrLastvarAttr  //30     /* must be last in list for scanning*/
    }

    public enum nitype
    {
        nNull = 0,
        nVar,
        nMenu,
        nCollection,
        nItemArray,
        nArray,
        nWao,
        nCmd,
        nEditDisp,
        nMethod,
        nRefresh,
        nUnit,
        nBlock,
        nChart,
        nSource,
        nImage,
        nList,
        nFile,
        nGrid,
        nAxis,
        nGraph/*,
        nEnum,
        nBitEnum*/
    }

    public class CDDLBase
    {
        //hCitemBase* m_pItemBase;
        //hCreference* m_pReference;
        //short m_IndexNumber; // -1 if unused, usually used when m_pItemBase is an array
        // or a bit
        uint m_lItemId;/* VMKP added on 210104 *///stevev 15may13 make public 4 debug
                                                 //protected int m_nImage;      // image id number, move to protected, POB - 5/16/2014
                                                 //HTREEITEM m_hTreeItem;   // value returned when this was put into tree
        bool m_bIsValid;
        CDDLBase m_pParent;     // Points to CDDLBase derived class that owns this
        string m_strLabel;     // Name used in the tree list and for sorting
                              //bool m_strNmValid;  // true if strName correct (even if empty)
        string m_strHelp;
        //bool m_classExpanded;
        //Vibhor 1405 : Start of Code
        string m_strSourceName;
        //Vibhor 1405 : End of Code
        //CTypedPtrList<CObList, CDDLBase*> m_ChildList;         // List of children owned by this
        public List<CDDLBase> m_ChildList;

        public nitype eType;

        public HARTDevice pDev = null;

        //itemType_t itmTyVal;


        public virtual void SetType()
        {
            eType = nitype.nNull;
            //itmTyVal = itemType_t.iT_NotAnItemType;
        }

        //uint uItemType;

        // Construction
        public CDDLBase()
        {
            m_pParent = null;
            m_bIsValid = true;
            //m_IndexNumber = -1; //unused
            m_ChildList = new List<CDDLBase>();
            m_strLabel = "";
        }
        //CDDLBase(hCitemBase* pIB = null);
        //CDDLBase(hCreference* pIR);
        //CDDLBase(const CDDLBase & Base); // Copy Constructor

        public bool IsValidTest()  /*returns true if valid and CACHED (does not read to reolve */
        {/* * * see notes in IsValid() below * * */
            return true; // default is valid
        }

        public bool isValid()
        {
            return true;
        }

        // Operations

        // Tree and List Control
        // Removed InsertChild( CDDLBase * pHdw, CDDIdeDoc* pDoc ), POB - 5/22/2014
        public bool AddChild(CDDLBase pChild)// true@success(doesn't exist)
        {
            if (m_ChildList.Contains(pChild))
            {
                return false;
            }
            else
            {
                m_ChildList.Add(pChild);
                return true;
            }
        }

        void RemoveNdeleteChild(CDDLBase pChild)
        {
            m_ChildList.Remove(pChild);
        }

        void DeleteList()
        {
            m_ChildList.Clear();
        }

        int deleteLeaf(ulong targetID)// returns number deleted or negative if error
        {
            int removalcount = 0;

            foreach (CDDLBase cdb in m_ChildList)
            {
                if (targetID == cdb.GetID())
                {
                    m_ChildList.Remove(cdb);
                }
            }

            return removalcount;
        }

        // cast operators
        //virtual operator string(); // used to get a value from an object (VARABLES only)

        // Special menu handling
        public void Execute()
        {
            ;
        }

        public uint getID()
        {
            return m_lItemId;
        }

        public void Execute(string s, string t)
        {
            ;
        }

        ///public virtual void LoadContextMenu(CMenu* pMenu, int& nPos);
        ///public virtual void OnUpdateMenuExecute(CCmdUI* pCmdUI);

        public virtual void DisplayValue()
        {
            ;
        }

        public void DisplayLabel()
        {
            ;
        }

        public void DisplayHelp()
        {
            ;
        }

        // Access Functions
        public virtual bool IsMethod()
        {
            return false;
        }

        public virtual bool IsCollection()
        {
            return false;
        }

        public virtual bool IsVariable()
        {
            return false;
        }

        public virtual bool IsImage()
        {
            return false;
        }

        public virtual bool IsChart()
        {
            return false;
        }

        public virtual bool IsCmd()
        {
            return false;
        }

        public virtual bool IsMenu()
        {
            return false;
        }

        public virtual bool IsUnit()
        {
            return false;
        }

        public virtual bool IsRefresh()
        {
            return false;
        }

        public virtual bool IsWao()
        {
            return false;
        }

        public virtual bool IsArray()
        {
            return false;
        }

        public virtual bool IsFile()
        {
            return false;
        }

        public virtual bool IsGraph()
        {
            return false;
        }

        public virtual bool IsAxis()
        {
            return false;
        }

        public virtual bool IsGrid()
        {
            return false;
        }

        public virtual bool IsWaveform()
        {
            return false;
        }

        public virtual bool IsSource()
        {
            return false;
        }

        public virtual bool IsList()
        {
            return false;
        }

        public virtual bool IsDisplayValue()  // Value is set for DISPLAY_VALUE
        {
            return false;
        }

        public virtual bool IsReadOnly()
        {
            return true;
        }

        public virtual bool IsItemArray()
        {
            return false;
        }

        public virtual bool IsReview()        // only difference about REVIEW menu is all items
                                              // VARIABLEs and all are DISPLAY_VALUE  
        {
            return false;
        }

        public virtual bool IsEditDisplay()  /*Vibhor 170304: added*/
        {
            return false;
        }

        public virtual bool IsValid()
        {
            return false;
        }

        public virtual bool IsExpanded()     // for the expansion classes
        {
            return false;
        }

        public virtual bool IsTop()
        {
            return false;
        }// override in DDLMain

        public virtual void SetValidity(bool valid)
        {
            m_bIsValid = valid;
        }

        public virtual string GetRelation()
        {
            return "";
        }

        public virtual string GetHelp()
        {
            return m_strHelp;
        }

        public virtual void SetHelp(string strHelp)
        {
            m_strHelp = strHelp;
        }

        public virtual string GetCurrentName(CDDLBase pItem)// stevev added 02dec05
        {
            return pItem.GetName();
        }
        /*
        public virtual bool GetNameFull() 
        {
            return (m_strNmValid); 
        }
        */
        /*
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
        */
        public virtual void SetLabel(string strLable)
        {
            if (strLable == null)
            {
                strLable = "";
            }
            m_strLabel = Common.GetLangStr(strLable);
        }

        public virtual string GetName()///
        {
            return m_strLabel;
        }

        public virtual void SetSourceName(string strName)
        {
            m_strSourceName = Common.GetLangStr(strName);
        }

        public virtual string GetSourceName()///
        {
            return m_strSourceName;
        }

        public virtual string GetUnitStr()
        {
            return "";
        }

        public virtual List<CDDLBase> GetChildList()
        {
            return m_ChildList;
        }
        public virtual void UpdateChildList()
        {
            ;
        }

        public virtual CDDLBase GetParent()
        {
            return m_pParent;
        }
        public virtual void SetParent(CDDLBase pParent)
        {
            m_pParent = pParent;
        }
        //virtual HTREEITEM GetTreeItem();
        //virtual void SetTreeItem(HTREEITEM hTreeItem);
        public virtual void SetID(uint lItemId)
        {
            m_lItemId = lItemId;
        }

        public virtual uint GetID()
        {
            return m_lItemId;
        }

        /*public virtual CDDLMenu getMenuPtr()
        { 
            return null; 
        }// menus and menuitems will override*/

        public virtual CDDLBase FindValidBranch() // Added to aid in menu selection, POB - 5/22/2014
        {
            CDDLBase pTemp = this;
            CDDLBase pValidFalse = null;
            CDDLBase pValidParent = null;

            do
            {
                if (!pTemp.IsValid())
                {
                    // This is no longer valid                         
                    pValidFalse = pTemp;
                }

                // Go to the next parent
                pTemp = pTemp.GetParent();

            } while (pTemp != null); // Loop unitl a null is dicovered

            if (pValidFalse != null)
            {
                /*
                 * We have at least one parent menu in our
                 * selection branch which is not valid
                 * Get the object above this invalid one
                 * It should be a valid parent (if not
                 * null).
                 */
                pTemp = pValidFalse.GetParent();

                if (pTemp != null)
                {
                    /*
                     * We have found a valid parent in the
                     * branch
                     */
                    pValidParent = pTemp;
                }
            }
            else
            {
                /* There are no invalid parents
                 * get its immediate parent
                 */
                pValidParent = this;
            }

            return pValidParent;
        }

        // Attributes
        // These data fields are not serialized and must be reconstructed
        public virtual int GetImage()  // Added to access image id, POB - 5/16/2014
                                       //int m_nImage;           // image id number
                                       //stevev 21mar05 - add a real link to the device object
        {
            return 0;
        }

    }
 
    public struct Index_t
    {
        public uint byElemType;
        public uint uiIndex;
    }

    public class menu_item//menu items, maybe every type inculde menu
    {
        public ushort uType;
        public uint uiID;
        public uint uiMember;
        public string desc;
        public string name;
        public string help;
        public List<Index_t> index_s;

        public menu_item()
        {
            index_s = new List<Index_t>();
        }
    }

    public class menulist: List<menu_item>
    {
        public uint qual;
    }

    public class CDDLMenu : CDDLBase
    {
        public List<menulist>  menuItems;

        public CDDLMenu()
        {
            menuItems = new List<menulist>();
            eType = nitype.nMenu;
        }

        public override bool IsMenu()
        {
            return true;
        }

    }

    public enum maskClass_t
    {
        maskNoClass = 0x00000000,
        maskDiagnostic = 0x00000001,
        maskDynamic = 0x00000002,
        maskService = 0x00000004,
        maskCorrection = 0x00000008,
        maskComputation = 0x00000010,
        maskInputBlock = 0x00000020,
        maskAnalogOut = 0x00000040,
        maskHART = 0x00000080,
        maskLocalDisplay = 0x00000100,
        maskFrequency = 0x00000200,
        maskDiscrete = 0x00000400,
        maskDevice = 0x00000800,
        maskLocal = 0x00001000,
        maskInput = 0x00002000,
        maskFactory = 0x00100000   // originator proprietary I imagine
    }

    public enum variableType_t
    {
        vT_unused = 0,
        vT_undefined,       // 1
        vT_Integer,         // 2
        vT_Unsigned,        // 3
        vT_FloatgPt,        // 4
        vT_Double,          // 5
        vT_Enumerated,      // 6
        vT_BitEnumerated,   // 7
        vT_Index,           // 8
        vT_Ascii,           // 9
        vT_PackedAscii,     //10 - HART only
        vT_Password,        //11
        vT_BitString,       //12
        vT_HartDate,        //13 - HART only
        vT_Time,            //14 - not HART
        vT_DateAndTime,     //15 - not HART
        vT_Duration,        //16 - not HART
        vT_EUC,             //17 - not HART
        vT_OctetString,     //18 - not HART
        vT_VisibleString,   //19 - not HART
        vT_TimeValue,       //20
        vT_Boolean,         //21 - not HART
        vT_MaxType          //  22
    }

    public class colletion_member//collection members, collection, array, var
    {
        public uint uiElv;
        public uint uiName;
        public string desc;
        public string help;
        public List<menu_item> items;

        public colletion_member()
        {
            items = new List<menu_item>();
        }
    }

    public class CDDLCollection : CDDLBase
    {
        public List<colletion_member> collectionmembers;

        public CDDLCollection()
        {
            collectionmembers = new List<colletion_member>();
            eType = nitype.nCollection;
        }

        public override bool IsCollection()
        {
            return true;
        }
    }

    public class item_array//collection members, collection, array, var
    {
        public uint uiElv;
        public uint uiIndex;
        public string desc;
        public string help;
        public List<menu_item> items;

        public item_array()
        {
            items = new List<menu_item>();
        }
    }

    public class CDDLItemArray : CDDLBase
    {
        public List<item_array> arrayitems;

        public CDDLItemArray()
        {
            arrayitems = new List<item_array>();
            eType = nitype.nItemArray;
        }

        public override bool IsItemArray()
        {
            return true;
        }

    }

    public class CDDLUnit : CDDLBase
    {
        public CDDLUnit()
        {
            eType = nitype.nUnit;
            //enumlist = new Dictionary<uint, string>();
            VarIdList = new List<uint>();
        }

        public uint unitVarId;

        public Dictionary<uint, string> enumlist;
        public Dictionary<uint, uint> enumIDlist;

        public List<uint> VarIdList;

        public override bool IsUnit()
        {
            return true;
        }

        public int updateVarUnit()
        {
            int rc = Common.SUCCESS;
            //cmdOperationType_t o = cmdOperationType_t.cmdOpNone,
            //O = cmdOperationType_t.cmdOpNone;
            //listOptrs2dataItemLists_t::iterator iP2DIL;
            CDDLBase pThisVarItem = new CDDLBase();

            foreach(uint varid in VarIdList)
            {
                if(pDev.getItembyID(varid, ref pThisVarItem))
                {
                    CDDLVar var = (CDDLVar)pThisVarItem;
                    //var.unitrelationID = getID();
                    //var.unitlist = enumlist;
                    //var.unitidlist = enumIDlist;
                    var.unitrelationID = unitVarId;
                    //var.setUnitFromList();
                }
            }

            return rc;
        }
    }

    public class CDDLMethod : CDDLBase
    {
        public CDDLMethod()
        {
            eType = nitype.nMethod;
            m_MethodCall = new hCmethodCall();
            defData = null;
        }
        
        public hCmethodCall m_MethodCall;

        public string defData;

        new public void Execute(string szMethodName, string szHelp) //Changed Definition
        {
            // stevev - 31jan06 - refill the cleared methodcall...this may need to re-resolve??
            //m_MethodCall.m_pMeth = (hCmethod*)m_pItemBase;
            m_MethodCall.source = methodCallSource_t.msrc_EXTERN;
            m_MethodCall.methodID = GetID();
            m_MethodCall.paramList = new List<CValueVarient>();
            m_MethodCall.m_pMeth = this;

            CValueVarient retVarient = new CValueVarient();
            pDev.doExecute(m_MethodCall, ref retVarient, false);

        }

        public CDDLVar getVarPtrBySymNumber(uint symNum)
        {
            CDDLBase pItem = new CDDLBase();
            CDDLVar pVar = null;

            if (symNum != 0)
            {
                if (pDev.getItembyID(symNum, ref pItem))
                {
                    if (pItem.IsVariable())
                    {
                        pVar = (CDDLVar)pItem;
                    }
                    else
                    {
                        //DEBUGLOG(CLOG_LOG | CERR_LOG, "%s (0x%04x) Expected to be a variable.(dbg-only msg)\n", pItem.getName().c_str(), pItem.getID());
                    }
                }
            }
            return pVar;// null on error
        }

        public string getDef()
        {
            return defData;
        }

        public override bool IsMethod()
        {
            return true;
        }

    }

    public class CDDLEditDisp : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLEditDisp()
        {
            eType = nitype.nEditDisp;
            uiCmdNum = 0;
        }

        public override bool IsEditDisplay()
        {
            return true;
        }
    }

    public class CDDLRefresh : CDDLBase
    {
        //public uint uiCmdNum;
        public List<uint> watchVarList;
        public List<uint> updateVarList;

        public CDDLRefresh()
        {
            eType = nitype.nRefresh;
            watchVarList = new List<uint>();
            updateVarList = new List<uint>();
        }

        public override bool IsRefresh()
        {
            return true;
        }

        public void updateVarRelation()
        {
            foreach(uint wid in watchVarList)
            {
                CDDLVar wVar = null;
                if (pDev.getVarbyID(wid, ref wVar))
                {
                    foreach(uint uid in updateVarList)
                    {
                        wVar.relationVarList.Add(uid);
                    }
                }
            }
        }
    }

    public class CDDLWao : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLWao()
        {
            eType = nitype.nWao;
            uiCmdNum = 0;
        }

        public override bool IsWao()
        {
            return true;
        }
    }

    public class CDDLArray : CDDLBase
    {
        public uint uiEleNum;
        public uint uiRefID;

        public nitype rType;

        public CDDLBase[] items;

        public CDDLArray()
        {
            eType = nitype.nArray;
            uiEleNum = 0;
            uiRefID = 0;
        }

        public override bool IsArray()
        {
            return true;
        }
    }

    public class CDDLFile : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLFile()
        {
            eType = nitype.nFile;
            uiCmdNum = 0;
        }

        public override bool IsFile()
        {
            return true;
        }
    }

    public class CDDLGraph : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLGraph()
        {
            eType = nitype.nGraph;
            uiCmdNum = 0;
        }

        public override bool IsGraph()
        {
            return true;
        }
    }

    public class CDDLAxis : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLAxis()
        {
            eType = nitype.nAxis;
            uiCmdNum = 0;
        }

        public override bool IsAxis()
        {
            return true;
        }
    }

    public class CDDLWaveform : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLWaveform()
        {
            eType = nitype.nArray;
            uiCmdNum = 0;
        }

        public override bool IsWaveform()
        {
            return true;
        }
    }

    public class CDDLSource : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLSource()
        {
            eType = nitype.nSource;
            uiCmdNum = 0;
        }

        public override bool IsSource()
        {
            return true;
        }
    }

    public class CDDLList : CDDLBase
    {
        public uint uiCount;
        public uint uiRefID;

        public nitype rType;
        public List<CDDLBase> items;

        public int Insert(CDDLBase ddb, int index)
        {
            CDDLBase pIB = new CDDLBase();
            if (index > 0 && index < items.Count())
            {
                //if (pDev.getItembyID(uiRefID, ref pIB))
                if(ddb.eType == nitype.nList)
                {
                    items.Insert(index, ddb);
                    return Common.SUCCESS;
                }
            }
            return Common.FAILURE;
        }

        public int Remove(int index)
        {
            if (index > 0 && index < items.Count())
            {
                items.RemoveAt(index);
                return Common.SUCCESS;
            }
            return Common.FAILURE;
        }

        public CDDLList()
        {
            eType = nitype.nList;
            items = new List<CDDLBase>();
            uiRefID = 0;
            uiCount = 0;
        }

        public override bool IsList()
        {
            return true;
        }
    }

    public class CDDLGrid : CDDLBase
    {
        public uint uiCmdNum;

        public CDDLGrid()
        {
            eType = nitype.nGrid;
            uiCmdNum = 0;
        }

        public override bool IsGrid()
        {
            return true;
        }
    }

    public class CDDLChart : CDDLBase
    {
        public CDDLChart()
        {
            eType = nitype.nChart;
        }

        public override bool IsChart()
        {
            return true;
        }
    }

    public class CDDLImage : CDDLBase
    {
        public CDDLImage()
        {
            eType = nitype.nImage;
        }

        public override bool IsImage()
        {
            return true;
        }
    }

    public class CMenuList : List<CDDLMenu>
    {

    }

    public class CCollectionList : List<CDDLCollection>
    {

    }

    public class CItemArrayList : List<CDDLItemArray>
    {

    }

    public class CImageList : List<CDDLImage>
    {

    }

    public class CChartList : List<CDDLChart>
    {

    }

    public class CUnitList : List<CDDLUnit>
    {

    }

    public class CMethodList : List<CDDLMethod>
    {

    }

    public class CEditDisplayList : List<CDDLEditDisp>
    {

    }

    public class CWaoList : List<CDDLWao>
    {

    }

    public class CRefresheList : List<CDDLRefresh>
    {

    }

    public class CArrayList : List<CDDLArray>
    {

    }

    public class CFileList : List<CDDLFile>
    {

    }

    public class CGraphList : List<CDDLGraph>
    {

    }

    public class CAxiseList : List<CDDLAxis>
    {

    }

    public class CWaveformList : List<CDDLWaveform>
    {

    }

    public class CSourceList : List<CDDLSource>
    {

    }

    public class CListList : List<CDDLList>
    {

    }

    public class CGridList : List<CDDLGrid>
    {

    }



}
