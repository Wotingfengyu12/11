using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    class DDLItem
    {
    }

    public enum DDL_ATTR_DATA_TYPE
    {
        DDL_ATTR_DATA_TYPE_UNDEFINED = 0
        , DDL_ATTR_DATA_TYPE_INT
        , DDL_ATTR_DATA_TYPE_UNSIGNED_LONG
        , DDL_ATTR_DATA_TYPE_FLOAT
        , DDL_ATTR_DATA_TYPE_DOUBLE
        , DDL_ATTR_DATA_TYPE_BITSTRING /* for this data type we'll parse a ulong value only*/
        , DDL_ATTR_DATA_TYPE_STRING
        , DDL_ATTR_DATA_TYPE_ITEM_ID
        , DDL_ATTR_DATA_TYPE_ENUM_LIST
        , DDL_ATTR_DATA_TYPE_REFERENCE /*Just a single reference*/
        , DDL_ATTR_DATA_TYPE_REFERENCE_LIST /*List of references*/
        , DDL_ATTR_DATA_TYPE_TYPE_SIZE
        , DDL_ATTR_DATA_TYPE_TRANSACTION_LIST
        , DDL_ATTR_DATA_TYPE_RESPONSE_CODE_LIST
        , DDL_ATTR_DATA_TYPE_MENU_ITEM_LIST
        , DDL_ATTR_DATA_TYPE_DEFINITION
        , DDL_ATTR_DATA_TYPE_REFRESH_RELATION
        , DDL_ATTR_DATA_TYPE_UNIT_RELATION
        , DDL_ATTR_DATA_TYPE_ITEM_ARRAY_ELEMENT_LIST
        , DDL_ATTR_DATA_TYPE_MEMBER_LIST
        , DDL_ATTR_DATA_TYPE_EXPRESSION
        , DDL_ATTR_DATA_TYPE_MIN_MAX
        , DDL_ATTR_DATA_TYPE_LINE_TYPE  /*DDl6 :: Waveform,Source*/
        , DDL_ATTR_DATA_TYPE_WAVEFORM_TYPE //All below are just indicators for spl parse fns
        , DDL_ATTR_DATA_TYPE_CHART_TYPE //But all these will be stored as ulVal in VALUES 
        , DDL_ATTR_DATA_TYPE_MENU_STYLE
        , DDL_ATTR_DATA_TYPE_SCOPE_SIZE
        , DDL_ATTR_DATA_TYPE_GRID_SET   /* stevev 25mar05 */
        , DDL_ATTR_DATA_TYPE_DEBUG_DATA /* stevev 10may05 */
        , DDL_ATTR_DATA_TYPE_PARAM      /* stevev 10may05 */
        , DDL_ATTR_DATA_TYPE_PARAM_LIST

        /* I think we need a data type to hold the references to DD_ITEM also*/

    }

    public enum DDL_COND_TYPE
    {

        DDL_COND_TYPE_UNDEFINED = 0
        , DDL_COND_TYPE_IF
        , DDL_COND_TYPE_SELECT
        , DDL_COND_TYPE_DIRECT
    }

    public enum DDL_COND_SECTION_TYPE
    {
        DDL_SECT_TYPE_DIRECT = 0  /*Direct*/
        , DDL_SECT_TYPE_CONDNL   /*Condiional*/
        , DDL_SECT_TYPE_CHUNKS    /*Possible Combination of Direct & Conditional*/
    }

    public struct OUTPUT_STATUS
    {
        public ushort kind;
        public ushort which;
        public ushort oclass;
    }

    public class BIT_ENUM_STATUS
    {
        public uint status_class;
        public List<OUTPUT_STATUS> oclasses;// = new List<OUTPUT_STATUS>();

        public BIT_ENUM_STATUS()
        {
            status_class = 0;
            oclasses = new List<OUTPUT_STATUS>();
        }
        /*BIT_ENUM_STATUS(const BIT_ENUM_STATUS& bes):oclasses(bes.oclasses)
        { status_class = bes.status_class; };*/
        /*BIT_ENUM_STATUS& operator=(const BIT_ENUM_STATUS& s)
	{	status_class=s.status_class;	oclasses=s.oclasses;  return *this; };*/
    }


    public class ENUM_VALUE
    {
        /*
         *	Enumeration tags
         */

        public const int ENUM_VALUE_TAG = 0;
        public const int ENUM_STATUS_TAG = 1;
        public const int ENUM_ACTIONS_TAG = 2;
        public const int ENUM_DESC_TAG = 3;
        public const int ENUM_HELP_TAG = 4;
        public const int ENUM_CLASS_TAG = 5;

        public const int ENUM_VALUE_TAG_A = 0;
        public const int ENUM_DESC_TAG_A = 1;
        public const int ENUM_HELP_TAG_A = 2;
        public const int ENUM_STATUS_TAG_A = 3;
        public const int ENUM_ACTIONS_TAG_A = 4;
        public const int ENUM_CLASS_TAG_A = 5;

        /* The masks for ENUM_VALUE */

        public const int ENUM_ACTIONS_EVALED = 0X01;
        public const int ENUM_CLASS_EVALED = 0X02;
        public const int ENUM_DESC_EVALED = 0X04;
        public const int ENUM_HELP_EVALED = 0X08;
        public const int ENUM_STATUS_EVALED = 0X10;
        public const int ENUM_VAL_EVALED = 0X20;

        public ushort evaled;
        public uint val;
        public ddpSTRING desc;
        public ddpSTRING help;
        public uint func_class;   /* functional class */
        public BIT_ENUM_STATUS status;
        public uint actions;

        public ENUM_VALUE()
        {
            func_class = 0;
            actions = 0;
            evaled = 0;
            val = 0;
            func_class = 0;
            desc = new ddpSTRING();
            help = new ddpSTRING();
            status = new BIT_ENUM_STATUS();
        }
        /*
        ENUM_VALUE(const ENUM_VALUE& ev):desc(ev.desc),help(ev.help),status(ev.status)
        {
            evaled = ev.evaled; val = ev.val;
            func_class = ev.func_class; actions = ev.actions;
        };
        ENUM_VALUE& operator=(const ENUM_VALUE& s )
	    {	desc = s.desc;		help       = s.help;		status = s.status;	  evaled  = s.evaled;	
		    val  = s.val;		func_class = s.func_class;	actions = s.actions;  return *this;
	    }*/
        public void Cleanup()
        {
            desc.Cleanup();
            help.Cleanup();
        }
    }

    public struct TYPE_SIZE
    {
        public ushort type;
        public ushort size;
    }

    public class DATA_ITEM
    {
        /*
         * Data item types.
         */

        public const int DATA_CONSTANT = 0;
        public const int DATA_REFERENCE = 1;
        public const int DATA_REF_FLAGS = 2;       /* HART */
        public const int DATA_REF_WIDTH = 3;       /* HART */
        public const int DATA_REF_FLAGS_WIDTH = 4;     /* HART */
        public const int DATA_FLOATING = 5;
        public struct datad //union
        {
            public ushort iconst;
            public ddpREFERENCE reff;
            public float fconst;
        }
        public datad data;
        public ushort type;
        public ushort flags;
        // stevev 18jun09 ...this is NO LONGER WIDTH... unsigned short  width;
        public UInt64 mask;

        //DATA_ITEM(){ data.ref = NULL;data.fconst=0.0;type=0;flags=0;width=0;};
        public DATA_ITEM()
        {
            data.reff = null;
            data.fconst = 0;
            type = 0;
            flags = 0;
            mask = 0;
        }
        //DATA_ITEM(const DATA_ITEM& di):type(0),flags(0),width(0)
        /*
        DATA_ITEM(const DATA_ITEM& di) :type(0),flags(0),mask(0)
        { data.ref= NULL; operator= (di); };
        DATA_ITEM& operator=(const DATA_ITEM& di);*/
        public void Cleanup()
        {
            if ((type == DATA_REFERENCE) || (type == DATA_REF_FLAGS) || (type == DATA_REF_WIDTH) || (type == DATA_REF_FLAGS_WIDTH))
            {
                if (data.reff != null)
                {
                    data.reff.Cleanup();
                    data.reff = null;
                }
            }
        }
    }

    public class RESPONSE_CODE
    {

        /* The masks for RESPONSE_CODE */

        public const int RS_DESC_EVALED = 0X01;
        public const int RS_HELP_EVALED = 0X02;
        public const int RS_TYPE_EVALED = 0X04;
        public const int RS_VAL_EVALED = 0X08;

        public ushort evaled;
        public ushort val;
        public ushort type;
        public ddpSTRING desc;
        public ddpSTRING help;

        public RESPONSE_CODE()
        {
            evaled = 0;
            val = 0;
            type = 0;
            desc = new ddpSTRING();
            help = new ddpSTRING();
        }
        /*
        RESPONSE_CODE(const RESPONSE_CODE& rc):desc(rc.desc),help(rc.help)
        { evaled = rc.evaled; val = rc.val; type = rc.type; };
        ~RESPONSE_CODE() { Cleanup(); };
        void Cleanup();
        RESPONSE_CODE& operator=(const RESPONSE_CODE& s)
	    {	desc=s.desc;help=s.help;evaled=s.evaled;val=s.val;type=s.type;return *this;};*/
        public void Cleanup()
        {
            desc.Cleanup();
            help.Cleanup();
            evaled = val = type = 0;
        }
    }

    public class TRANSACTION
    {
        public ulong number;
        public DATA_ITEM_LIST request = new DATA_ITEM_LIST();//??????
        public DATA_ITEM_LIST reply = new DATA_ITEM_LIST();
        public List<RESPONSE_CODE> rcodes = new List<RESPONSE_CODE>();
        public List<ddpREFERENCE> post_rqst_rcv_act = new List<ddpREFERENCE>();
        /*

        TRANSACTION() :number(0)
        { request.clear(); reply.clear(); rcodes.clear(); post_rqst_rcv_act.clear(); };
        TRANSACTION(const TRANSACTION& src):number(0xffffffff) { operator= (src); };
        TRANSACTION& operator=(const TRANSACTION& s)
	    {	number = s.number;		request = s.request;	
		reply  = s.reply;	     rcodes = s.rcodes;	
		post_rqst_rcv_act   =   s.post_rqst_rcv_act;
		return *this;};*/

        void Cleanup()
        {
            request.Clear();
            reply.Clear();
            rcodes.Clear();
        }
    }

    public class MENU_ITEM
    {
        public ddpREFERENCE item = new ddpREFERENCE();
        public ushort qual; /*This is a bit string*/

        /*MENU_ITEM()
        {
            qual = 0;
        }*/
        public void Cleanup()
        {
            item.Cleanup();
            item.Clear();
            qual = 0;
        }
        /*
        MENU_ITEM(const MENU_ITEM& mi):item(mi.item),qual(mi.qual) { };
        ~MENU_ITEM() { Cleanup(); };
        MENU_ITEM& operator=(const MENU_ITEM& s){item = s.item;qual = s.qual; return *this;};

        # ifdef _DBGMIL
            void dumpItem(void);
        #endif*/
    }

    public struct DEFINITION
    {
        public uint size;
        public string data;
    }

    public class REFRESH_RELATION
    {
        public List<ddpREFERENCE> watch_list = new List<ddpREFERENCE>();
        public List<ddpREFERENCE> update_list = new List<ddpREFERENCE>();
        /*
        REFRESH_RELATION() { };
        REFRESH_RELATION(const REFRESH_RELATION& rr){operator=(rr);};
        ~REFRESH_RELATION() { Cleanup(); };

        REFRESH_RELATION& operator=(const REFRESH_RELATION& rr);*/

        void Cleanup()
        {
            watch_list.Clear();
            update_list.Clear();
        }
    }

    public class UNIT_RELATION
    {
        public ddpREFERENCE unit_var = new ddpREFERENCE();
        public List<ddpREFERENCE> var_units = new List<ddpREFERENCE>();

        /*
        UNIT_RELATION() { };
        UNIT_RELATION(const UNIT_RELATION& ur){operator=(ur);};
        ~UNIT_RELATION() { Cleanup(); };

        UNIT_RELATION& operator=(const UNIT_RELATION& ur);*/

        void Cleanup()
        {
            unit_var.Cleanup();
            unit_var.Clear();
            var_units.Clear();
        }
    }

    public class ITEM_ARRAY_ELEMENT
    {
        public ushort evaled;
        public uint index;
        public ddpREFERENCE item;
        public ddpSTRING desc;
        public ddpSTRING help;
        public string mem_name;/* collections only stevev 15sep05 (for now)*/

        public void Cleanup()
        {
            item.Cleanup();
            item.Clear();
            desc.Cleanup();
            help.Cleanup();
        }

        public ITEM_ARRAY_ELEMENT()
        {
            evaled = 0;
            index = 0;
            item = new ddpREFERENCE();
            desc = new ddpSTRING();
            help = new ddpSTRING();
        }

        /*
        ITEM_ARRAY_ELEMENT(const ITEM_ARRAY_ELEMENT& iae) : 
						item(iae.item),desc(iae.desc),help(iae.help)
        { evaled = iae.evaled; index = iae.index; mem_name = iae.mem_name; };
        ~ITEM_ARRAY_ELEMENT() { Cleanup(); };
            ITEM_ARRAY_ELEMENT& operator=(const ITEM_ARRAY_ELEMENT& s)
	    {evaled = s.evaled;index= s.index;mem_name= s.mem_name; 
	     item   = s.item;  desc = s.desc; help    = s.help;   return *this;};
         */
    }

    public class MEMBER
    {
        public ushort evaled;
        public uint name;
        public ddpREFERENCE item;
        public ddpSTRING desc;
        public ddpSTRING help;
        public string member_name;

        public MEMBER()
        {
            item = new ddpREFERENCE();
            desc = new ddpSTRING();
            help = new ddpSTRING();
        }
        /*
        void Cleanup();
        MEMBER() { evaled = 0; name = 0; };
        MEMBER(const MEMBER& iae) : item(iae.item),desc(iae.desc),help(iae.help)
        { evaled = iae.evaled; name = iae.name; member_name = iae.member_name; };
        ~MEMBER() { Cleanup(); };
        MEMBER& operator=(const MEMBER& s)
	{Cleanup(); evaled=s.evaled;name=s.name;item=s.item;desc=s.desc;
	 help=s.help;member_name=s.member_name; return *this;};*/

    }

    public class DDlSectionChunks
    {
        public DDL_ATTR_DATA_TYPE attrDataType; /*Ulong, String, ENUM , etc. */

        public List<DDL_COND_SECTION_TYPE> isChunkConditionalList = new List<DDL_COND_SECTION_TYPE>();

        public List<VALUES> directVals = new List<VALUES>();

        public List<DDlConditional> conditionalVals = new List<DDlConditional>();

        public byte byNumOfChunks;

        public DDlSectionChunks(DDL_ATTR_DATA_TYPE atttributeDataType = DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNDEFINED)
        {
            attrDataType = atttributeDataType;
            byNumOfChunks = 0;
        }
    }

    public class DDlConditional
    {

        public DDL_COND_TYPE condType = new DDL_COND_TYPE();/* IF, SWITCH, DIRECT*/

        public DDL_ATTR_DATA_TYPE attrDataType = new DDL_ATTR_DATA_TYPE(); /*Ulong, String, ENUM , etc. */

        public ddpExpression expr = new ddpExpression();/*The Expression as a Postfix list of elements */

        public byte byNumberOfSections;/* The number of Branches the Conditional is having*/

        public List<ddpExpression> caseVals = new List<ddpExpression>(); /*This is a list of expression*/  /* # define DEFAULT Tag to some value*/

        public List<DDL_COND_SECTION_TYPE> isSectionConditionalList = new List<DDL_COND_SECTION_TYPE>(); /* Vibhor 190105:Direct, Conditional(Nested) , Chunky (Combination)*/

        public List<VALUES> Vals = new List<VALUES>(); /* List of Value Structure for each section*/

        public List<DDlConditional> listOfChilds = new List<DDlConditional>(); /* Pointer to child if any ......*/

        public List<DDlSectionChunks> listOfChunks = new List<DDlSectionChunks>();/*Vibhor 190105 Added: If a section has chunks, it goes here*/

        public DDlConditional(DDL_COND_TYPE conditionalType = DDL_COND_TYPE.DDL_COND_TYPE_UNDEFINED,
                       DDL_ATTR_DATA_TYPE attributeDataType = DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNDEFINED,
                       byte byNumSections = 1 /*Since we have a conditional it means atleast one section*/)

        {
            condType = conditionalType;
            byNumberOfSections = byNumSections; /*Assuming the attribute is direct by default*/
            attrDataType = attributeDataType; /* this has to be set by the parent!!!!*/

        }

    }

    public struct MIN_MAX_VALUE
    {
        public uint which; //which min-max
                           //Expression	  value; //expression containing the min / max value 
                           //	bool isMinMaxConditional;
        public DDlConditional pCond;  //DDL_ATTR_DATA_TYPE_EXPRESSION
                                      //	VALUES			*pVals;
                                      //	ValueList		 directVals;
                                      //	SectionCondList	 isChunkConditionalList;

    }

    public struct LINE_TYPE
    {
        public ushort type;
        public int qual; /*1. valid only if type == DATA
						   2. if qual = Null ==> DATA, 
							  if qual > 0 ==> DATA#*/
    }

    public class GRID_SET
    {
        public ddpSTRING desc;
        public List<ddpREFERENCE> values;

        public GRID_SET()
        {
            desc = new ddpSTRING();
            values = new List<ddpREFERENCE>();
        }

        public void Cleanup()
        {
            desc.Cleanup();
            values.Clear();
        }
    }

    public struct MEMBER_DEBUG_T
    {
        public string symbol_name;
        public uint flags;
        public uint member_value;
    }

    public struct ATTR_DEBUG_INFO_T
    {
        public uint attr_tag;
        public uint attr_lineNo;
        public ddpSTRING attr_filename;
        public List<MEMBER_DEBUG_T> attr_member_list;/* empty if not a member type */

        //??????void Cleanup();
    }

    public class ITEM_DEBUG_INFO
    {
        public string symbol_name;
        public ddpSTRING file_name;
        public uint lineNo;
        public uint flags;
        public List<ATTR_DEBUG_INFO_T> attr_list;

        public ITEM_DEBUG_INFO()
        {
            attr_list = new List<ATTR_DEBUG_INFO_T>();
            file_name = new ddpSTRING();
        }

        void Cleanup()   /* delete the memory in attr list of ptrs */
        {
            ;//??????
        }
    }

    public struct METHOD_PARAM
    {
        public int param_type;
        public uint param_modifiers;
        public string param_name;//struct owns memory-delete[]

        public void Clear()
        {
            param_type = 0;
            param_modifiers = 0;
            param_name = null;
        }

        /*METHOD_PARAM() { Clear(); };
        METHOD_PARAM(const METHOD_PARAM& s):param_type(0),param_modifiers(0),param_name(NULL)
        {operator= (s); };
        ~METHOD_PARAM() { if (param_name) delete[] param_name; Clear(); };
        METHOD_PARAM& operator=(const METHOD_PARAM& s){param_type=s.param_type;
		param_modifiers=s.param_modifiers;
		if (s.param_name) {param_name=new char[strlen(s.param_name) + 1];
		strncpy(param_name, s.param_name, strlen(s.param_name)+1);
    }
		else param_name = NULL;    return * this;};*/
    }

    public class VALUES//union???
    {

        //int					iVal;	 
        public float fVal;
        public double dVal;
        //unsigned long		ulVal;
        public Int64 llVal;
        public UInt64 ullVal;
        public uint id;
        public ddpREFERENCE reff;//*** /*Just a reference!*/
        public List<ddpREFERENCE> refList; /* Pointer to a vector of REFERENCEs!!!*/
        public ddpSTRING strVal;//*****
        public List<ENUM_VALUE> enmList;
        public TYPE_SIZE typeSize;
        public List<TRANSACTION> transList; /*This needs to be optimized after */
        public List<RESPONSE_CODE> respCdList;
        public List<MENU_ITEM> menuItemsList;
        public DEFINITION defData; /*Method Definition Data*/
        public REFRESH_RELATION refrshReln;/* Refresh Relation*/
        public UNIT_RELATION unitReln;
        public List<ITEM_ARRAY_ELEMENT> itemArrElmnts;
        public List<MEMBER> memberList;
        public ddpExpression pExpr;
        public List<MIN_MAX_VALUE> minMaxList; /*Pointer to a vector of MIN_MAX_VALUES*/
        public LINE_TYPE lineType; //Vibhor 270804: added
        public List<GRID_SET> gridMemList;
        public ITEM_DEBUG_INFO debugInfo; // stevev 06may05 
        public METHOD_PARAM methodType;// stevev 10may05
        public List<METHOD_PARAM> paramList; // stevev 10may05

        public VALUES()
        {
            reff = new ddpREFERENCE();
            refList = new List<ddpREFERENCE>();
            strVal = new ddpSTRING();
            enmList = new List<ENUM_VALUE>();
            typeSize = new TYPE_SIZE();
            transList = new List<TRANSACTION>();
            respCdList = new List<RESPONSE_CODE>();
            menuItemsList = new List<MENU_ITEM>();
            defData = new DEFINITION();
            refrshReln = new REFRESH_RELATION();
            unitReln = new UNIT_RELATION();
            itemArrElmnts = new List<ITEM_ARRAY_ELEMENT>();
            memberList = new List<MEMBER>();
            pExpr = new ddpExpression();
            minMaxList = new List<MIN_MAX_VALUE>();
            lineType = new LINE_TYPE();
            gridMemList = new List<GRID_SET>();
            debugInfo = new ITEM_DEBUG_INFO();
            methodType = new METHOD_PARAM();
            paramList = new List<METHOD_PARAM>();
        }

        public void Cleanup(DDL_ATTR_DATA_TYPE dataType)//??????
        {
            ;
        }
    }

    public class DDlAttribute
    {
        public const int DEFAULT_ATTR_ID = 255;
        public string attrName; /*Just for Debugging purposes; Name of the attribute or SubAttribute*/
        public byte byAttrID; /*Unique Id of the Attribute, # defined in DDlDefs.h as per Bin file spec*/
        public DDL_ATTR_DATA_TYPE attrDataType; /*Ulong, String, ENUM , etc. */

        public VALUES pVals;//?????? /* Pointer to union containig all possible values*/
        public bool bIsAttributeConditional; /* true / false */
        public DDlConditional pCond; /*== NULL for a DIRECT object, else points to the next 
								conditional branch*/

        /*Vibhor 221003: Adding following to support Conditional Lists of Lists ,
                         basically  list attributes in different chunks. The chunks
                         may be any combination of Direct / IF / SELECT chunks*/

        public bool bIsAttributeConditionalList; /*true if we have a multi-chunk conditional List*/

        public List<DDL_COND_SECTION_TYPE> isChunkConditionalList = new List<DDL_COND_SECTION_TYPE>();

        public List<VALUES> directVals = new List<VALUES>();

        public List<DDlConditional> conditionalVals = new List<DDlConditional>();

        public byte byNumOfChunks;

        public DDlAttribute(string strName = "",
                        byte byAttributeID = DEFAULT_ATTR_ID,
                        DDL_ATTR_DATA_TYPE atttributeDataType = DDL_ATTR_DATA_TYPE.DDL_ATTR_DATA_TYPE_UNDEFINED,
                        bool bIsConditional = false
        )
        {
            attrName = strName;
            byAttrID = byAttributeID;
            attrDataType = atttributeDataType;
            //pVals;
            bIsAttributeConditional = bIsConditional;
            pCond = new DDlConditional();//??????
            bIsAttributeConditionalList = false;
            byNumOfChunks = 0;
            pVals = new VALUES();
        }

    }

    //class DDlVariable : DDlBaseItem     /*Item Type == 1*/
    //{


    //    public DDlVariable()
    //    {
    //        //ADDED by Deepak
    //        //		variableAttrList.clear();
    //    }

    //    //void AllocVarAttributes( unsigned long ulVarMask);
    //    public override void AllocAttributes(uint attrMask)
    //    {
    //        DDlAttribute pDDlAttr = null;


    //        /*	if(ulVarMask & VAR_TYPE_SIZE)
    //            { */

    //        /*Moved TYPE_SIZE attribute in to eval_variable*/
    //        /*		pDDlAttr = (DDlAttribute*)new DDlAttribute("attrVarTypeSize",
    //                                                VAR_TYPE_SIZE_ID,
    //                                                DDL_ATTR_DATA_TYPE_TYPE_SIZE,
    //                                                false);

    //                attrList.push_back(pDDlAttr); */

    //        /*	} */

    //        /*	if(ulVarMask & VAR_CLASS)
    //            { */
    //        /*Type is a mandatory attribute and should be allocated anyway*/
    //        pDDlAttr = new DDlAttribute("VarClass",
    //                                        VAR_CLASS_ID,
    //                                        DDL_ATTR_DATA_TYPE_BITSTRING,
    //                                        false);

    //        attrList.push_back(pDDlAttr);

    //        /*	} */

    //        /*Vibhor 141103 : Moving Handling to the beginning of the attribute list,
    //         So we will parse Handling in Eval as the first attribute*/
    //        /*	if(ulVarMask & VAR_HANDLING)
    //            {

    //                pDDlAttr = (DDlAttribute*)new DDlAttribute("VarHandling",
    //                                                VAR_HANDLING_ID,
    //                                                DDL_ATTR_DATA_TYPE_BITSTRING,
    //                                                false);

    //                attrList.push_back(pDDlAttr);

    //            }
    //        */

    //        if (ulVarMask & VAR_LABEL)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarLabel",
    //                                            VAR_LABEL_ID,
    //                                            DDL_ATTR_DATA_TYPE_STRING,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }




    //        if (ulVarMask & VAR_HELP)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarHelp",
    //                                            VAR_HELP_ID,
    //                                            DDL_ATTR_DATA_TYPE_STRING,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_DISPLAY)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarDisplayFormat",
    //                                            VAR_DISPLAY_ID,
    //                                            DDL_ATTR_DATA_TYPE_STRING,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_EDIT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarEditFormat",
    //                                            VAR_EDIT_ID,
    //                                            DDL_ATTR_DATA_TYPE_STRING,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_ENUMS)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarEnums",
    //                                            VAR_ENUMS_ID,
    //                                            DDL_ATTR_DATA_TYPE_ENUM_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }



    //        if (ulVarMask & VAR_UNIT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarConstantUnit",
    //                                            VAR_UNIT_ID,
    //                                            DDL_ATTR_DATA_TYPE_STRING,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }

    //        /* removed 15oct12
    //            if(ulVarMask & VAR_WIDTHSIZE)// used to be read timeout
    //            {

    //                pDDlAttr = (DDlAttribute*)new DDlAttribute("VarWidth",
    //                                                VAR_WIDTHSIZE_ID,
    //                                                DDL_ATTR_DATA_TYPE_SCOPE_SIZE,
    //                                                false);

    //                attrList.push_back(pDDlAttr);

    //            }

    //            if(ulVarMask & VAR_HEIGHTSIZE)
    //            {

    //                pDDlAttr = (DDlAttribute*)new DDlAttribute("VarHeight",
    //                                                VAR_HEIGHTSIZE_ID,
    //                                                DDL_ATTR_DATA_TYPE_SCOPE_SIZE,
    //                                                false);

    //                attrList.push_back(pDDlAttr);

    //            }
    //        ****/

    //        /*	if(ulVarMask & VAR_RESP_CODES)
    //            {

    //                pDDlAttr = (DDlAttribute*)new DDlAttribute("VarResponseCodes",
    //                                                VAR_RESP_CODES_ID,
    //                                                DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                                false);

    //                attrList.push_back(pDDlAttr);

    //            }  */

    //        if (ulVarMask & VAR_MIN_VAL)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarMinVal",
    //                                            VAR_MIN_VAL_ID,
    //                                            DDL_ATTR_DATA_TYPE_MIN_MAX,/*This needs to be taken care of*/
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_MAX_VAL)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarMaxVal",
    //                                            VAR_MAX_VAL_ID,
    //                                            DDL_ATTR_DATA_TYPE_MIN_MAX,/*This needs to be taken care of*/
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_SCALE)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarScalingFactor",
    //                                            VAR_SCALE_ID,
    //                                            DDL_ATTR_DATA_TYPE_EXPRESSION,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }



    //        if (ulVarMask & VAR_INDEX_ITEM_ARRAY)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarIndexItemArrayName",
    //                                            VAR_INDEX_ITEM_ARRAY_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_PRE_READ_ACT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarPreReadActions",
    //                                            VAR_PRE_READ_ACT_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }

    //        if (ulVarMask & VAR_POST_READ_ACT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarPostReadActions",
    //                                            VAR_POST_READ_ACT_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }

    //        if (ulVarMask & VAR_PRE_WRITE_ACT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarPreWriteActions",
    //                                            VAR_PRE_WRITE_ACT_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_POST_WRITE_ACT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarPostWriteActions",
    //                                            VAR_POST_WRITE_ACT_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }

    //        if (ulVarMask & VAR_PRE_EDIT_ACT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarPreEditActions",
    //                                            VAR_PRE_EDIT_ACT_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        if (ulVarMask & VAR_POST_EDIT_ACT)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarPostEditActions",
    //                                            VAR_POST_EDIT_ACT_ID,
    //                                            DDL_ATTR_DATA_TYPE_REFERENCE_LIST,
    //                                            false);

    //            attrList.push_back(pDDlAttr);

    //        }


    //        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
    //        /*After Defining all move this guy at the end*/
    //        /*We will allocate Validity if mask contains it;
    //        If not, then we will default it after parsing all other attributes in
    //        eval_variable*/

    //        if (ulVarMask & VAR_VALID)
    //        {

    //            pDDlAttr = (DDlAttribute*)new DDlAttribute("VarValidity",
    //                                            VAR_VALID_ID,
    //                                            DDL_ATTR_DATA_TYPE_UNSIGNED_LONG,
    //                                            false);

    //            /*	pDDlAttr->pVals->ulVal = 0x01L; /*Default Attribute*/

    //            attrList.push_back(pDDlAttr);

    //        }


    //        return;
    //    }


    //};

}
