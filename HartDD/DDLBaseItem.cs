using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    //   class aCitemBase
    //   {
    ///* for unitialized read from purify */

    //       // ddKey is redundant - has been removed
    //       public uint itemId;
    //       public uint itemType;

    //       ushort itemSize;
    //       public uint itemSubType;
    //       public UInt32 attrMask;   // one bit for each attribute in attrLst

    //       //UINT32			ddRef;		/* DD_REFERENCE - for dds compatability - removed*/
    //       public string itemName;

    //       /* VMKP added on 291203 */
    //       bool isConditional;
    //       /* VMKP added on 291203 */

    //       // main data::				// vector of aCattrBase*
    //       public AattributeList_t attrLst;   // if memory ownership removed, set ptr to NULL

    //   }

    public class DDlBaseItem
    {
        public const int RESERVED_ITYPE1 = 0;
        public const int VARIABLE_ITYPE = 1;
        public const int COMMAND_ITYPE = 2;
        public const int MENU_ITYPE = 3;
        public const int EDIT_DISP_ITYPE = 4;
        public const int METHOD_ITYPE = 5;
        public const int REFRESH_ITYPE = 6;
        public const int UNIT_ITYPE = 7;
        public const int WAO_ITYPE = 8;
        public const int ITEM_ARRAY_ITYPE = 9;
        public const int COLLECTION_ITYPE = 10;
        public const int RESERVED_ITYPE2 = 11;
        public const int BLOCK_ITYPE = 12;
        public const int PROGRAM_ITYPE = 13;   // not in HART
        public const int RECORD_ITYPE = 14;
        public const int ARRAY_ITYPE = 15;
        public const int VAR_LIST_ITYPE = 16;
        public const int RESP_CODES_ITYPE = 17;
        public const int DOMAIN_ITYPE = 18;    // not in HART
        public const int MEMBER_ITYPE = 19;
        public const int FILE_ITYPE = 20;
        public const int CHART_ITYPE = 21;
        public const int GRAPH_ITYPE = 22;
        public const int AXIS_ITYPE = 23;
        public const int WAVEFORM_ITYPE = 24;
        public const int SOURCE_ITYPE = 25;
        public const int LIST_ITYPE = 26;
        public const int GRID_ITYPE = 27;
        public const int IMAGE_ITYPE = 28;
        public const int BLOB_ITYPE = 29;  /* added oct-2012 */
        public const int PLUGIN_ITYPE = 30;
        public const int TEMPLATE_ITYPE = 31;
        public const int RESERVED_ITYPE3 = 32;
        public const int COMPONENT_ITYPE = 33; // not in HART
        public const int COMP_FOLDER_ITYPE = 34;   // not in HART
        public const int COMP_DESCRIPTOR_ITYPE = 35; // not in HART "component_reference" in spec
        public const int COMP_RELATION_ITYPE = 36; // not in HART
        public const int RESERVED_ITYPE4 = 37;
        public const int MAX_ITYPE = 38;	/* must be last in list */

        public UInt32 id;     /* Unique Id of the item*/
        public byte byItemType;        /* Type of the item*/
        public byte byItemSubType; /* Sub Type (if applicable of the item */

        public string strItemName; /*This guy might go....*/

        public UInt32 ulItemMasks; /*The mask of the attributes item is having */

        public const byte maskSizes = 4; //Vibhor 170904 : Added//??????size 4;

        public List<DDlAttribute> attrList = new List<DDlAttribute>();

        UInt32 getItemId()
        {
            return id;
        }
        byte getItemType()
        { 
            return byItemType; 
        }
        byte getItemSubType()
        {
            return byItemSubType; 
        }
        /*This one is applicable for ItemArrays and collections only*/

        // pure virtual function -over-ride in classes
        public virtual void AllocAttributes(UInt32 attrMask)
        {
            ;// PVFC( "DDlBaseItem_AA" );
        }

        public virtual void AllocAttributes()
        {
            ;// PVFC( "DDlBaseItem_AA" );
        }

        public DDlBaseItem()
        {
            id = 0;
            byItemType = byItemSubType = RESERVED_ITYPE1;
            ulItemMasks = 0xFFFFFFFF;
            attrList.Clear();
            strItemName = "";
        }

    };

}
