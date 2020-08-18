using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace FieldIot.HARTDD
{
    public class DDlDevDescription
    {
        /*
         *	Parameter Table tags
         */

        const int PT_BLK_ITEM_NAME_TBL_OFFSET_TAG = 0;
        const int PT_PARAM_MEM_TBL_OFFSET_TAG = 1;
        const int PT_PARAM_MEM_COUNT_TAG = 2;
        const int PT_PARAM_ELEM_TBL_OFFSET_TAG = 3;
        const int PT_PARAM_ELEM_COUNT_TAG = 4;
        const int PT_PARAM_ELEM_MAX_COUNT_TAG = 5;
        const int PT_ARRAY_ELEM__ITEM_TBL_OFFSET_TAG = 6;
        const int PT_ARRAY_ELEM_TYPE_OR_VAR_TYPE_TAG = 7;
        const int PT_ARRAY_ELEM_SIZE_OR_VAR_SIZE_TAG = 8;
        const int PT_ARRAY_ELEM_CLASS_VAR_CLASS_TAG = 9;

        /*
         *	Block Item Name Table tags
         */

        const int BINT_BLK_ITEM_NAME_TAG = 0;
        const int BINT_ITEM_TBL_OFFSET_TAG = 1;
        const int BINT_PARAM_TBL_OFFSET_TAG = 2;
        const int BINT_PARAM_LIST_TBL_OFFSET_TAG = 3;
        const int BINT_REL_TBL_OFFSET_TAG = 4;
        const int BINT_READ_CMD_TBL_OFFSET_TAG = 5;
        const int BINT_ITEM_CMD_TBL_OFFSET_TAG = 5;// for fmA
        const int BINT_READ_CMD_TBL_COUNT_TAG = 6;
        const int BINT_WRITE_CMD_TBL_OFFSET_TAG = 7;
        const int BINT_WRITE_CMD_TBL_COUNT_TAG = 8;


        const int TABLE_OFFSET_INVALID = -1;

        const int MAGIC_NUMBER_SIZE = 4;
        const int HEADER_SIZE_SIZE = 4;
        const int OBJECTS_SIZE_SIZE = 4;
        const int DATA_SIZE_SIZE = 4;
        const int MANUFACTURER_SIZE = 3;
        const int DEVICE_TYPE_SIZE = 2;
        const int DEVICE_REV_SIZE = 1;
        const int DD_REV_SIZE = 1;
        const int TOK__MAJOR_REV_SIZE = 1;
        const int TOK__MINOR_REV_SIZE = 1;
        const int RESERVED1_SIZE = 2;
        const int A_PROFILE_SIZE = 1;
        const int A_RESERVED1_SIZE = 1;
        const int SIGNATURE_SIZE = 4;
        const int RESERVED3_SIZE = 4;
        const int RESERVED4_SIZE = 4;

        const int HEADER_SIZE = (MAGIC_NUMBER_SIZE + HEADER_SIZE_SIZE +
                                     OBJECTS_SIZE_SIZE +
                                     DATA_SIZE_SIZE +
                                     MANUFACTURER_SIZE +
                                     DEVICE_TYPE_SIZE +
                                     DEVICE_REV_SIZE +
                                     DD_REV_SIZE +
                                    TOK__MAJOR_REV_SIZE +
                                    TOK__MINOR_REV_SIZE +
                                    RESERVED1_SIZE +
                                     SIGNATURE_SIZE +
                                     RESERVED3_SIZE +
                                     RESERVED4_SIZE);

        const int MAGIC_NUMBER_OFFSET = 0;

        const int HEADER_SIZE_OFFSET = (MAGIC_NUMBER_OFFSET + MAGIC_NUMBER_SIZE);

        const int OBJECTS_SIZE_OFFSET = (HEADER_SIZE_OFFSET + HEADER_SIZE_SIZE);

        const int DATA_SIZE_OFFSET = (OBJECTS_SIZE_OFFSET + OBJECTS_SIZE_SIZE);

        const int MANUFACTURER_OFFSET = (DATA_SIZE_OFFSET + DATA_SIZE_SIZE);

        const int DEVICE_TYPE_OFFSET = (MANUFACTURER_OFFSET + MANUFACTURER_SIZE);

        const int DEVICE_REV_OFFSET = (DEVICE_TYPE_OFFSET + DEVICE_TYPE_SIZE);

        const int DD_REV_OFFSET = (DEVICE_REV_OFFSET + DEVICE_REV_SIZE);

        const int TOKENIZER_MAJOR_REV_OFFSET = (DD_REV_OFFSET + DD_REV_SIZE);

        const int TOKENIZER_MINOR_REV_OFFSET = (TOKENIZER_MAJOR_REV_OFFSET + TOK__MAJOR_REV_SIZE);

        const int RESERVED1_OFFSET = (TOKENIZER_MINOR_REV_OFFSET + TOK__MINOR_REV_SIZE);
        const int A_RESERVED1_OFFSET = (RESERVED1_OFFSET + A_PROFILE_SIZE);
        const int SIGNATURE_OFFSET = (RESERVED1_OFFSET + RESERVED1_SIZE);

        const int RESERVED3_OFFSET = (SIGNATURE_OFFSET + SIGNATURE_SIZE);

        const int RESERVED4_OFFSET = (RESERVED3_OFFSET + RESERVED3_SIZE);

        const int MFG_ID_SIZE = 3;

        const int MAX_SOD = 0x4000;

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
        public const int COMPONENT_ITYPE = 33;// not in HART
        public const int COMP_FOLDER_ITYPE = 34;// not in HART
        public const int COMP_DESCRIPTOR_ITYPE = 35;// not in HART "component_reference" in spec
        public const int COMP_RELATION_ITYPE = 36; // not in HART
        public const int RESERVED_ITYPE4 = 37;
        public const int MAX_ITYPE = 38;/* must be last in list */

        /* BLOCK DIRECTORY tables */

        const int BLK_ITEM_TBL_ID = 0;
        const int BLK_ITEM_NAME_TBL_ID = 1;
        const int PARAM_TBL_ID = 2;
        const int PARAM_MEM_TBL_ID = 3;
        const int PARAM_MEM_NAME_TBL_ID = 4;
        const int PARAM_ELEM_TBL_ID = 5;
        const int PARAM_LIST_TBL_ID = 6;
        const int PARAM_LIST_MEM_TBL_ID = 7;
        const int PARAM_LIST_MEM_NAME_TBL_ID = 8;
        const int CHAR_MEM_TBL_ID = 9;
        const int CHAR_MEM_NAME_TBL_ID = 10;
        const int REL_TBL_ID = 11;
        const int UPDATE_TBL_ID = 12;

        const int COMMAND_TBL_ID = 13;
        const int CRIT_PARAM_TBL_ID = 14;
        const int MAX_BLOCK_TBL_ID_HCF = 15;/* must be last in HART list */

        const int MAX_BLOCK_TBL_ID = MAX_BLOCK_TBL_ID_HCF;

        /* - - - - Version 8 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

        const int BLK_ITEM_TBL_ID_8 = 0;
        const int BLK_ITEM_NAME_TBL_ID_8 = 1;
        const int PARAM_TBL_ID_8 = 2;
        const int REL_TBL_ID_8 = 3;
        const int UPDATE_TBL_ID_8 = 4;

        const int COMMAND_TBL_ID_8 = 5;
        const int CRIT_PARAM_TBL_ID_8 = 6;
        const int MAX_BLOCK_TBL_ID_HCF_8 = 7;   /* must be last in HART list */

        const int MAX_BLOCK_TBL_ID_8 = MAX_BLOCK_TBL_ID_HCF_8;


        const int DDOD_REV_SUPPORTED_TEN = 0x0A;
        const int DDOD_REV_SUPPORTED_EIGHT = 8;
        const int DDOD_REV_SUPPORTED_SEVEN = 0; /* there is no rev 7 */
        const int DDOD_REV_SUPPORTED_SIX = 6; /* via fm6 */
        const int DDOD_REV_SUPPORTED_FIVE = 5; /* via fms */

        const int DDOD_REV_MAJOR_FDI = 4;
        const int DDOD_REV_MINOR_FDI = 0;

        const int DDOD_REV_MAJOR_HCF = 8;   /* set to 8 11oct07 - timj */
        const int DDOD_REV_MINOR_HCF = 3;   /* set to 3 12aug13 - sjv - working up to fma files*/
                                            /* set to 2 18jan12 - sjv - fixed references */
                                            /* set to 1 03mar08 - sjv - table update addition*/
                                            /* set to 0 11oct07 - timj - utf8 enhancement */
                                            /* set to 2 09Sept05- added member name strings to       */
                                            /* set to 1 27apr05 - attribute masks sizes have changed */
                                            /* set to 0 10/20/04- reset for the release to a major member */
                                            /* set to 5 9/27/04 - encoded integers for extn offset&len */
                                            /* set to 4 9/20/04 - munging removed                   */
                                            /* set to 3 9/17/04 - image table added                 */
                                            /* set to 2 9/16/04 - datapart segment size - size diff */
                                            /* set to 1 8/16/04 - datapart segment size diff        */
        const int DDOD_REV_BUILD_HCF = 12;

        const int BLK_TBL_ID = 0;
        const int ITEM_TBL_ID = 1;
        const int PROG_TBL_ID = 2;
        const int DOMAIN_TBL_ID = 3;
        const int STRING_TBL_ID = 4;
        const int DICT_REF_TBL_ID = 5;
        const int LOCAL_VAR_TBL_ID = 6;

        const int CMD_NUM_ID_TBL_ID = 7;
        const int IMAGE_TBL_ID = 8;   // Vibhor 020904: Added; HART 6
        const int MAX_DEVICE_TBL_ID_HCF = 8;    /* must be last in HART list  for HART 5*/
        const int MAX_DEVICE_TBL_ID_HCF_6 = 9;  //has to be 9 //Vibhor 020904: Added; HART 6
                                                /* DEVICE DIRECTORY table masks */
        public const int BLK_TBL_MASK = (1 << BLK_TBL_ID);
        public const int ITEM_TBL_MASK = (1 << ITEM_TBL_ID);
        public const int PROG_TBL_MASK = (1 << PROG_TBL_ID);
        public const int DOMAIN_TBL_MASK = (1 << DOMAIN_TBL_ID);
        public const int STRING_TBL_MASK = (1 << STRING_TBL_ID);
        public const int DICT_REF_TBL_MASK = (1 << DICT_REF_TBL_ID);
        public const int LOCAL_VAR_TBL_MASK = (1 << LOCAL_VAR_TBL_ID);

        public const int CMD_NUM_ID_TBL_MASK = (1 << CMD_NUM_ID_TBL_ID);
        public const int IMAGE_TBL_MASK = (1 << IMAGE_TBL_ID); //Vibhor 020904: Added; HART 6
        const int RESV_DEV_MASK = IMAGE_TBL_MASK;    // sjv add image table 9/17/04
        const int RESV_DEV_MASK_6 = IMAGE_TBL_MASK;


        const int DEVICE_TBL_MASKS = BLK_TBL_MASK | ITEM_TBL_MASK |
                                        PROG_TBL_MASK |
                                        DOMAIN_TBL_MASK |
                                        STRING_TBL_MASK |
                                        DICT_REF_TBL_MASK |
                                        LOCAL_VAR_TBL_MASK |
                                        CMD_NUM_ID_TBL_MASK |
                                        IMAGE_TBL_MASK;     //Vibhor 020904: Added; HART 6

        public struct DEVICE_DIR_EXT
        {

            public byte byLength;
            public byte byDeviceDirObjectCode;
            public byte byFormatCode;
            public DATAPART_SEGMENT BlockNameTable;
            public DATAPART_SEGMENT ItemTable;
            public DATAPART_SEGMENT ProgramTable;
            public DATAPART_SEGMENT DomainTable;
            public DATAPART_SEGMENT StringTable;
            public DATAPART_SEGMENT DictReferenceTable;
            public DATAPART_SEGMENT LocalVariableTable;
            public DATAPART_SEGMENT CommandTable;
        }

        public struct BLOCK_DIR_EXT_6
        {
            public byte byLength;
            public byte byBlockDirObjectCode;
            public byte byFormatCode;
            public DATAPART_SEGMENT_6 BlockItemTable;
            public DATAPART_SEGMENT_6 BlockItemNameTable;
            public DATAPART_SEGMENT_6 ParameterTable;
            public DATAPART_SEGMENT_6 ParameterMemberTable;
            public DATAPART_SEGMENT_6 ParameterMemberNameTable;
            public DATAPART_SEGMENT_6 ParameterElementTable;
            public DATAPART_SEGMENT_6 ParameterListTable;
            public DATAPART_SEGMENT_6 ParameterListMemberTable;
            public DATAPART_SEGMENT_6 ParameterListMemberNameTable;
            public DATAPART_SEGMENT_6 CharectersiticsMemberTable;
            public DATAPART_SEGMENT_6 CharectersiticsMemberNameTable;
            public DATAPART_SEGMENT_6 RelationTable;
            public DATAPART_SEGMENT_6 UpdateTable;
            public DATAPART_SEGMENT_6 ParameterCommandTable;
            public DATAPART_SEGMENT_6 CriticalParameterTable;
        }


        const int DEV_DIR_LENGTH_OFFSET = 0;
        const int DEV_DIR_OBJ_CODE_OFFSET = 1;
        const int DEV_DIR_FORMAT_CODE_OFFSET = 2;
        const int BLK_NAME_TBL_OFFSET = 3;
        const int ITEM_TBL_OFFSET = 7;
        const int PROG_TBL_OFFSET = 11;
        const int DOM_TBL_OFFSET = 15;
        const int STRNG_TBL_OFFSET = 19;
        const int DICT_REF_TBL_OFFSET = 23;
        const int LOC_VAR_TBL_OFFSET = 27;
        const int CMD_TBL_OFFSET = 31;

        const int DEVICE_DIR_EXT_SIZE = 35;
        const int DEVICE_DIR_LENGTH = (DEVICE_DIR_EXT_SIZE - 1);

        const int BLK_DIR_LENGTH_6_OFFSET = 0;
        const int BLK_DIR_OBJ_CODE_6_OFFSET = 1;
        const int BLK_DIR_FORMAT_CODE_6_OFFSET = 2;
        const int BLK_ITEM_TBL_6_OFFSET = 3;
        const int BLK_ITEMNAME_TBL_6_OFFSET = 11;
        const int BLK_PARAM_TBL_6_OFFSET = 19;
        const int BLK_PARAMEMBER_TBL_6_OFFSET = 27;
        const int BLK_PARAMEMBERNAME_TBL_6_OFFSET = 35;
        const int BLK_ELEMENT_TBL_6_OFFSET = 43;
        const int BLK_PARAMLIST_TBL_6_OFFSET = 51;
        const int BLK_PARAMLISTMEMBER_TBL_6_OFFSET = 59;
        const int BLK_PARAMLISTMEMBERNAME_TBL_6_OFFSET = 67;
        const int BLK_CHARMEMBER_TBL_6_OFFSET = 75;
        const int BLK_CHARMEMBERNAME_TBL_6_OFFSET = 83;
        const int BLK_RELATION_TBL_6_OFFSET = 91;
        const int BLK_UPDATE_TBL_6_OFFSET = 99;
        const int BLK_PARAM2COMMAND_TBL_6_OFFSET = 107;
        const int BLK_CRITICALPARAM_TBL_6_OFFSET = 115;


        const int BLK_DIR_EXT_6_SIZE = 123;
        const int BLK_DIR_LENGTH_6 = (BLK_DIR_EXT_6_SIZE - 1);

        const int DEV_DIR_LENGTH_8_OFFSET = 0;
        const int DEV_DIR_OBJ_CODE_8_OFFSET = 1;
        const int DEV_DIR_FORMAT_CODE_8_OFFSET = 2;
        const int ITEM_TBL_8_OFFSET = 3;/* previous + DATAPART_SEGMENT_8_SIZE */
        const int STRNG_TBL_8_OFFSET = 11;
        const int DICT_REF_TBL_8_OFFSET = 19;
        const int CMD_TBL_8_OFFSET = 27;
        const int IMG_TBL_8_OFFSET = 35;

        const int DEVICE_DIR_EXT_8_SIZE = 43;
        const int DEVICE_DIR_LENGTH_8 = (DEVICE_DIR_EXT_8_SIZE - 1);

        const int BLK_DIR_LENGTH_8_OFFSET = 0;
        const int BLK_DIR_OBJ_CODE_8_OFFSET = 1;
        const int BLK_DIR_FORMAT_CODE_8_OFFSET = 2;
        const int BLK_ITEM_TBL_8_OFFSET = 3;
        const int BLK_ITEMNAME_TBL_8_OFFSET = 11;
        const int BLK_PARAM_TBL_8_OFFSET = 19;
        const int BLK_RELATION_TBL_8_OFFSET = 27;
        const int BLK_UPDATE_TBL_8_OFFSET = 35;
        const int BLK_PARAM2COMMAND_TBL_8_OFFSET = 43;
        const int BLK_CRITICALPARAM_TBL_8_OFFSET = 51;


        const int BLK_DIR_EXT_8_SIZE = 59;
        const int BLK_DIR_LENGTH_8 = (BLK_DIR_EXT_8_SIZE - 1);

        const int SEG_DATA_OFFSET_6 = 0;
        const int SEG_SIZE_OFFSET_6 = 4;
        const int DATAPART_SEGMENT_6_SIZE = 8;
        //const int DATAPART_SEGMENT_8 = DATAPART_SEGMENT_6;
        const int SEG_DATA_OFFSET_8 = SEG_DATA_OFFSET_6;
        const int SEG_SIZE_OFFSET_8 = SEG_SIZE_OFFSET_6;
        const int DATAPART_SEGMENT_8_SIZE = DATAPART_SEGMENT_6_SIZE;

        const int DEV_DIR_LENGTH_6_OFFSET = 0;
        const int DEV_DIR_OBJ_CODE_6_OFFSET = 1;
        const int DEV_DIR_FORMAT_CODE_6_OFFSET = 2;
        const int BLK_NAME_TBL_6_OFFSET = 3;
        const int ITEM_TBL_6_OFFSET = 11;/* previous + DATAPART_SEGMENT_6_SIZE */
        const int PROG_TBL_6_OFFSET = 19;
        const int DOM_TBL_6_OFFSET = 27;
        const int STRNG_TBL_6_OFFSET = 35;
        const int DICT_REF_TBL_6_OFFSET = 43;
        const int LOC_VAR_TBL_6_OFFSET = 51;
        const int CMD_TBL_6_OFFSET = 59;
        const int IMG_TBL_6_OFFSET = 67;

        const int DEVICE_DIR_EXT_6_SIZE = 75;
        const int DEVICE_DIR_LENGTH_6 = (DEVICE_DIR_EXT_6_SIZE - 1);

        public struct BLOCK_DIR_EXT
        {
            public byte byLength;
            public byte byBlockDirObjectCode;
            public byte byFormatCode;
            public DATAPART_SEGMENT BlockItemTable;
            public DATAPART_SEGMENT BlockItemNameTable;
            public DATAPART_SEGMENT ParameterTable;
            public DATAPART_SEGMENT ParameterMemberTable;
            public DATAPART_SEGMENT ParameterMemberNameTable;
            public DATAPART_SEGMENT ParameterElementTable;
            public DATAPART_SEGMENT ParameterListTable;
            public DATAPART_SEGMENT ParameterListMemberTable;
            public DATAPART_SEGMENT ParameterListMemberNameTable;
            public DATAPART_SEGMENT CharectersiticsMemberTable;
            public DATAPART_SEGMENT CharectersiticsMemberNameTable;
            public DATAPART_SEGMENT RelationTable;
            public DATAPART_SEGMENT UpdateTable;
            public DATAPART_SEGMENT ParameterCommandTable;
            public DATAPART_SEGMENT CriticalParameterTable;
        }

        const int BLK_DIR_LENGTH_OFFSET = 0;
        const int BLK_DIR_OBJ_CODE_OFFSET = 1;
        const int BLK_DIR_FORMAT_CODE_OFFSET = 2;
        const int BLK_ITEM_TBL_OFFSET = 3;
        const int BLK_ITEMNAME_TBL_OFFSET = 7;
        const int BLK_PARAM_TBL_OFFSET = 11;
        const int BLK_PARAMEMBER_TBL_OFFSET = 15;
        const int BLK_PARAMEMBERNAME_TBL_OFFSET = 19;
        const int BLK_ELEMENT_TBL_OFFSET = 23;
        const int BLK_PARAMLIST_TBL_OFFSET = 27;
        const int BLK_PARAMLISTMEMBER_TBL_OFFSET = 31;
        const int BLK_PARAMLISTMEMBERNAME_TBL_OFFSET = 35;
        const int BLK_CHARMEMBER_TBL_OFFSET = 39;
        const int BLK_CHARMEMBERNAME_TBL_OFFSET = 43;
        const int BLK_RELATION_TBL_OFFSET = 47;
        const int BLK_UPDATE_TBL_OFFSET = 51;
        const int BLK_PARAM2COMMAND_TBL_OFFSET = 55;
        const int BLK_CRITICALPARAM_TBL_OFFSET = 59;

        const int BLK_DIR_EXT_SIZE = 63;

        const int EXTEN_LENGTH_OFFSET = 0;

        public const int EXTEN_LENGTH_SIZE = 1;

        const int DIR_TYPE_SIZE = 1;
        const int FORMAT_CODE_SIZE = 1;
        /*  stevev - changed to new encoding 9/17/04 */
        const int DATAFIELD_OFFSET_SIZE = 2;    /* 11jan05 - sjv redefined for blktbl decoding */
        const int DATAFIELD_SIZE_SIZE = 2;
        const int TABLE_REF_LEN = (DATAFIELD_OFFSET_SIZE + DATAFIELD_SIZE_SIZE);

        /*   Offsets of the extension data fields (in octets).   */

        const int DIR_TYPE_OFFSET = (EXTEN_LENGTH_OFFSET + EXTEN_LENGTH_SIZE);
        const int FORMAT_CODE_OFFSET = (DIR_TYPE_OFFSET + DIR_TYPE_SIZE);
        const int DIRECTORY_DATA_OFFSET = (FORMAT_CODE_OFFSET + FORMAT_CODE_SIZE);

        const int DEVICE_DIR_LEN_HCF = ((MAX_DEVICE_TBL_ID_HCF * TABLE_REF_LEN) + DIRECTORY_DATA_OFFSET - EXTEN_LENGTH_SIZE);

        /* for blk dir parsing to get critical param list */
        const int BLK_DIR_LEN_HCF = ((MAX_BLOCK_TBL_ID_HCF * TABLE_REF_LEN) + DIRECTORY_DATA_OFFSET - EXTEN_LENGTH_SIZE);

        /* BLOCK DIRECTORY table masks */

        const int BLK_ITEM_TBL_MASK = (1 << BLK_ITEM_TBL_ID);
        const int BLK_ITEM_NAME_TBL_MASK = (1 << BLK_ITEM_NAME_TBL_ID);
        const int PARAM_TBL_MASK = (1 << PARAM_TBL_ID);
        const int PARAM_MEM_TBL_MASK = (1 << PARAM_MEM_TBL_ID);
        const int PARAM_MEM_NAME_TBL_MASK = (1 << PARAM_MEM_NAME_TBL_ID);
        const int PARAM_ELEM_TBL_MASK = (1 << PARAM_ELEM_TBL_ID);
        const int PARAM_LIST_TBL_MASK = (1 << PARAM_LIST_TBL_ID);
        const int PARAM_LIST_MEM_TBL_MASK = (1 << PARAM_LIST_MEM_TBL_ID);
        const int PARAM_LIST_MEM_NAME_TBL_MASK = (1 << PARAM_LIST_MEM_NAME_TBL_ID);
        const int CHAR_MEM_TBL_MASK = (1 << CHAR_MEM_TBL_ID);
        const int CHAR_MEM_NAME_TBL_MASK = (1 << CHAR_MEM_NAME_TBL_ID);
        const int REL_TBL_MASK = (1 << REL_TBL_ID);
        const int UPDATE_TBL_MASK = (1 << UPDATE_TBL_ID);

        const int COMMAND_TBL_MASK = (1 << COMMAND_TBL_ID);
        const int CRIT_PARAM_TBL_MASK = (1 << CRIT_PARAM_TBL_ID);
        const int RESV_BLK_MASK = CRIT_PARAM_TBL_MASK;

        const int BLOCK_TBL_MASKS_HCF = BLK_ITEM_TBL_MASK | BLK_ITEM_NAME_TBL_MASK | PARAM_TBL_MASK | PARAM_MEM_TBL_MASK | PARAM_MEM_NAME_TBL_MASK | PARAM_ELEM_TBL_MASK | PARAM_LIST_TBL_MASK | PARAM_LIST_MEM_TBL_MASK | PARAM_LIST_MEM_NAME_TBL_MASK | CHAR_MEM_TBL_MASK | CHAR_MEM_NAME_TBL_MASK | REL_TBL_MASK | UPDATE_TBL_MASK | COMMAND_TBL_MASK | RESV_BLK_MASK;

        public struct DATAPART_SEGMENT
        {

            public ushort offset;
            public ushort wSize;
        }

        const int SEG_DATA_OFFSET = 0;
        const int SEG_SIZE_OFFSET = 2;
        const int DATAPART_SEGMENT_SIZE = 4;


        /* Device Directory Binary */

        public struct BININFO
        {
            public UInt32 size;
            public byte[] chunk;
            public uint uoffset;
        }

        public struct BIN_DEVICE_DIR
        {
            public UInt32 bin_exists;
            public UInt32 bin_hooked;
            //public BININFO blk_tbl;
            public BININFO item_tbl;
            //public BININFO prog_tbl;
            //public BININFO domain_tbl;
            public BININFO string_tbl;
            public BININFO dict_ref_tbl;
            //public BININFO local_var_tbl;
            public BININFO cmd_num_id_tbl;
        }

        /*
         *	Block Directory Definitions
         */
        public struct BLK_ITEM_TBL_ELEM
        {
            public UInt32 blk_item_id;
            public int blk_item_name_tbl_offset;
        }

        public struct BLK_ITEM_TBL
        {
            public uint count;
            public BLK_ITEM_TBL_ELEM[] list;
        }

        /* Block Item Name Table */

        public struct BLK_ITEM_NAME_TBL_ELEM
        {
            public UInt32 blk_item_name;
            public int item_tbl_offset;
            public int param_tbl_offset;
            public int param_list_tbl_offset;
            public int rel_tbl_offset;
            public int read_cmd_tbl_offset;
            public int read_cmd_count;
            public int write_cmd_tbl_offset;
            public int write_cmd_count;

        }

        public struct DATAPART_SEGMENT_6
        {

            public uint offset;
            //	WORD				wSize ;// just till next Tokenizer iteration
            public uint uSize;
        }

        public struct BIN_DEVICE_DIR_6
        {
            public uint bin_exists;
            public uint bin_hooked;
            //public BININFO blk_tbl;
            public BININFO item_tbl;
            public BININFO prog_tbl;
            public BININFO domain_tbl;
            public BININFO string_tbl;
            public BININFO dict_ref_tbl;
            public BININFO local_var_tbl;
            public BININFO cmd_num_id_tbl;
            public BININFO image_tbl;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct IMG_ITEM
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public string lang_code; /*Which language uses this image*/
            public DATAPART_SEGMENT_6 img_file;  /*Copy of image file*/
            public byte[] p2Graphik; /* pointer to the binary, raw image*/
            /*
            public void SetLang(byte[] lang)
            {
                lang_code = lang;
            }
            */
        }

        public struct IMAGE_TBL_ELEM
        {
            public ushort num_langs; /*Number of Languages*/
            public IMG_ITEM[] img_list;//??????
            /*
            public void SetLang(ushort lang)
            {
                num_langs = lang;
            }
            */
        }

        public struct IMAGE_TBL
        {
            public ushort count;
            //public List<IMAGE_TBL_ELEM> list;//??????
            public IMAGE_TBL_ELEM[] list;//??????
        }

        /* Device Directory Flat */

        public struct FLAT_DEVICE_DIR_6
        {
            public uint attr_avail;
            //public BLK_TBL blk_tbl;
            public ITEM_TBL item_tbl;
            public PROG_TBL prog_tbl;
            public DOMAIN_TBL domain_tbl;
            public STRING_TBL string_tbl;
            public DICT_REF_TBL dict_ref_tbl;
            public LOCAL_VAR_TBL local_var_tbl;
            public CMD_NUM_ID_TBL cmd_num_id_tbl;
            public IMAGE_TBL image_tbl; // New table added in eDDL
        }

        /*Vibhor 010904: End of Code*/

        // stevev 22feb07 --- added

        /* Block Directory Binary */

        public struct BIN_BLOCK_DIR_6
        {
            public uint bin_exists;
            //public uint bin_hooked;

            //public BININFO blk_item_tbl;
            //public BININFO blk_item_name_tbl;
            //public BININFO param_tbl;
            //public BININFO param_mem_tbl;
            //public BININFO param_mem_name_tbl;
            //public BININFO param_elem_tbl;
            //public BININFO param_list_tbl;
            //public BININFO param_list_mem_tbl;
            //public BININFO param_list_mem_name_tbl;
            //public BININFO char_mem_tbl;
            //public BININFO char_mem_name_tbl;
            //public BININFO rel_tbl;
            //public BININFO update_tbl;

            //public BININFO command_tbl;
            //public BININFO crit_param_tbl;

        }

        /* Block Directory Flat */

        public struct FLAT_BLOCK_DIR_6
        {
            //public uint attr_avail;
            public BLK_ITEM_TBL blk_item_tbl;
            public BLK_ITEM_NAME_TBL blk_item_name_tbl;
            public PARAM_TBL param_tbl;
            //public PARAM_MEM_TBL param_mem_tbl;
            //public PARAM_MEM_NAME_TBL param_mem_name_tbl;
            //public PARAM_ELEM_TBL param_elem_tbl;
            //public PARAM_LIST_TBL param_list_tbl;
            //public PARAM_LIST_MEM_TBL param_list_mem_tbl;
            //public PARAM_LIST_MEM_NAME_TBL param_list_mem_name_tbl;
            //public CHAR_MEM_TBL char_mem_tbl;
            //public CHAR_MEM_NAME_TBL char_mem_name_tbl;
            public REL_TBL rel_tbl;
            public UPDATE_TBL update_tbl;

            public COMMAND_TBL command_tbl;
            public COMMAND_TBL_8 command_to_var_tbl;
            public CRIT_PARAM_TBL crit_param_tbl;
        }

        public struct BLK_ITEM_NAME_TBL
        {
            public uint count;
            public BLK_ITEM_NAME_TBL_ELEM[] list;
        }

        /* Parameter Table */

        public struct PARAM_TBL_ELEM
        {
            public int blk_item_name_tbl_offset;
            public int param_mem_tbl_offset;
            public int param_mem_count;
            public int param_elem_tbl_offset;
            public int param_elem_count;
            public int array_elem_item_tbl_offset;
            public int array_elem_count;
            public int array_elem_type_or_var_type;
            public int array_elem_size_or_var_size;
            public uint array_elem_class_or_var_class;
        }

        public struct PARAM_TBL
        {
            public int count;
            public PARAM_TBL_ELEM[] list;
        }


        /* Parameter Member Table */
        /*
        public struct PARAM_MEM_TBL_ELEM
        {
            public int item_tbl_offset;
            public int param_mem_type;
            public int param_mem_size;
            public uint param_mem_class;
            public int rel_tbl_offset;
        }

        public struct PARAM_MEM_TBL
        {
            public int count;
            public PARAM_MEM_TBL_ELEM[] list;
        }
        */
        /* Parameter Element Table */
        /*
        public struct PARAM_ELEM_TBL_ELEM
        {
            int param_elem_subindex;
            int rel_tbl_offset;
        }

        public struct PARAM_ELEM_TBL
        {
            public int count;
            public PARAM_ELEM_TBL_ELEM[] list;
        }

            */
        /* Parameter Member Name Table */
        /*
        public struct PARAM_MEM_NAME_TBL_ELEM
        {
            uint param_mem_name;
            int param_mem_offset;
        }

        public struct PARAM_MEM_NAME_TBL
        {
            public int count;
            public PARAM_MEM_NAME_TBL_ELEM[] list;
        }
        */

        /* Parameter List Table */
        /*
        public struct PARAM_LIST_TBL_ELEM
        {
            public int blk_item_name_tbl_offset;
            public int param_list_mem_tbl_offset;
            public int param_list_mem_count;
        }

        public struct PARAM_LIST_TBL
        {
            public int count;
            public PARAM_LIST_TBL_ELEM[] list;
        }


        /* Parameter List Member Table */
        /*
        public struct PARAM_LIST_MEM_TBL_ELEM
        {
            public int blk_item_name_tbl_offset;
        }

        public struct PARAM_LIST_MEM_TBL
        {
            public int count;
            public PARAM_LIST_MEM_TBL_ELEM[] list;
        }


        /* Parameter List Member Name Table */
        /*
        public struct PARAM_LIST_MEM_NAME_TBL_ELEM
        {
            public uint param_list_mem_name;
            public int param_list_mem_tbl_offset;
        }

        public struct PARAM_LIST_MEM_NAME_TBL
        {
            public int count;
            public PARAM_LIST_MEM_NAME_TBL_ELEM[] list;
        }


        /* Characteristic Member Table */
        /*
        public struct CHAR_MEM_TBL_ELEM
        {
            public int item_tbl_offset;
            public int char_mem_type;
            public int char_mem_size;
            public uint char_mem_class;
            public int rel_tbl_offset;
        }

        public struct CHAR_MEM_TBL
        {
            public int count;
            public CHAR_MEM_TBL_ELEM[] list;
        }


        /* Characteristic Member Name Table */
        /*
        public struct CHAR_MEM_NAME_TBL_ELEM
        {
            public uint char_mem_name;
            public int char_mem_offset;
        }

        public struct CHAR_MEM_NAME_TBL
        {
            public int count;
            public CHAR_MEM_NAME_TBL_ELEM[] list;
        }


        /* Relation Table */

        public struct REL_TBL_ELEM
        {
            public int wao_item_tbl_offset;
            public int unit_item_tbl_offset;
            public int update_tbl_offset;
            public int update_count;
            public int unit_count;// added 27feb08
        }

        public struct REL_TBL
        {
            public int count;
            public REL_TBL_ELEM[] list;
        }

        /* Update Table */

        public struct UPDATE_TBL_ELEM
        {
            public int op_it_offset;
            public int op_subindex;
            public int desc_it_offset;
        }

        public struct UPDATE_TBL
        {
            public int count;
            public UPDATE_TBL_ELEM[] list;
        }



        /* Command Table Structures */

        public struct COMMAND_INDEX
        {
            public uint id;
            public ulong value;
        }

        public struct DEVICE_DIR_EXT_6
        {

            public byte byLength;
            public byte byDeviceDirObjectCode;
            public byte byFormatCode;
            public DATAPART_SEGMENT_6 BlockNameTable;
            public DATAPART_SEGMENT_6 ItemTable;
            public DATAPART_SEGMENT_6 ProgramTable;
            public DATAPART_SEGMENT_6 DomainTable;
            public DATAPART_SEGMENT_6 StringTable;
            public DATAPART_SEGMENT_6 DictReferenceTable;
            public DATAPART_SEGMENT_6 LocalVariableTable;
            public DATAPART_SEGMENT_6 CommandTable;
            public DATAPART_SEGMENT_6 ImageTable;
        }

        public struct COMMAND_TBL_ELEM
        {
            public ulong number;
            public ulong transaction;
            public int subindex;
            public ushort weight;
            public int count;
            public COMMAND_INDEX[] index_list;
        }

        public struct COMMAND_TBL
        {
            public int count;
            public COMMAND_TBL_ELEM[] list;
        }

        // stevev 04mar08 - for edd 8.1
        public struct COMMAND_TBL_8_ELEM
        {
            public int subindex;
            public ulong number;
            public ulong transaction;
            public ushort weight;
            public int count;
            public COMMAND_INDEX[] index_list;
        }

        public struct PTOC_TBL_8_ELEM
        {
            public uint item_id;
            public int rd_count;
            public COMMAND_TBL_8_ELEM[] rd_list;
            public int wr_count;
            public COMMAND_TBL_8_ELEM[] wr_list;
        }

        public struct COMMAND_TBL_8
        {
            public int count;
            public PTOC_TBL_8_ELEM[] list;
        }
        // end stevev 04mar08 ---------

        public struct CRIT_PARAM_TBL
        {
            public int count;
            public uint[] list;
        }
        /* Block Directory Flat */
        /*
        public struct FLAT_BLOCK_DIR
        {
            UInt32 attr_avail;
            BLK_ITEM_TBL blk_item_tbl;
            BLK_ITEM_NAME_TBL blk_item_name_tbl;
            PARAM_TBL param_tbl;
            PARAM_MEM_TBL param_mem_tbl;
            PARAM_MEM_NAME_TBL param_mem_name_tbl;
            PARAM_ELEM_TBL param_elem_tbl;
            PARAM_LIST_TBL param_list_tbl;
            PARAM_LIST_MEM_TBL param_list_mem_tbl;
            PARAM_LIST_MEM_NAME_TBL param_list_mem_name_tbl;
            CHAR_MEM_TBL char_mem_tbl;
            CHAR_MEM_NAME_TBL char_mem_name_tbl;
            REL_TBL rel_tbl;
            UPDATE_TBL update_tbl;

            COMMAND_TBL command_tbl;
            CRIT_PARAM_TBL crit_param_tbl;

        }

        public struct BLK_TBL_ELEM
        {
            UInt32 blk_id;
            int item_tbl_offset;
            int char_rec_item_tbl_offset;
            int char_rec_bint_offset;
            int blk_dir_dd_ref;
            int usage;
            FLAT_BLOCK_DIR flat_block_dir;
        }

        public struct BLK_TBL
        {
            int count;
            BLK_TBL_ELEM[] list;
        }
        */

        /* Item Table */

        public struct ITEM_TBL_ELEM
        {
            public uint item_id;
            public int dd_ref;
            public ushort item_type;
        }

        public struct ITEM_TBL
        {
            public ushort count;
            public ITEM_TBL_ELEM[] list;
        }


        /* Program Table */

        public struct PROG_TBL_ELEM
        {
            public uint item_id;
            public int dd_ref;
        }

        public struct PROG_TBL
        {
            public int count;
            public PROG_TBL_ELEM[] list;
        }


        /* Domain Table */

        public struct DOMAIN_TBL_ELEM
        {
            public uint item_id;
            public int dd_ref;
        }

        public struct DOMAIN_TBL
        {
            public int count;
            public DOMAIN_TBL_ELEM[] list;
        }


        /* String Table */

        public struct STRING_TBL
        {
            //public byte[] root;    /* memory chunk pointer */
            public int count;
            //public List<ddpSTRING> list;
            public ddpSTRING[] list;
        }


        ///* Dictionary Reference Table */

        public struct DICT_REF_TBL
        {
            public ushort count;
            public uint[] list;   // list of dict keys

            // timj added 9oct07
            public ddpSTRING[] name;// list of names
            public ddpSTRING[] text;// list of dict strings
        }


        /* Local variable table */

        public struct LOCAL_VAR_TBL_ELEM
        {
            public uint item_id;
            public int dd_ref;
            public ushort type;
            public ushort size;
        }

        public struct LOCAL_VAR_TBL
        {
            public int count;
            public LOCAL_VAR_TBL_ELEM[] list;
        }


        /* Command number to item id conversion table */

        public struct CMD_NUM_ID_TBL_ELEM
        {
            public ulong number;
            public uint item_id;
        }


        public struct CMD_NUM_ID_TBL
        {
            public ushort count;
            public CMD_NUM_ID_TBL_ELEM[] list;
        }


        /* Device Directory Flat */

        public struct FLAT_DEVICE_DIR
        {
            //public UInt32 attr_avail;
            //public BLK_TBL blk_tbl;
            public ITEM_TBL item_tbl;
            //public PROG_TBL prog_tbl;
            //public DOMAIN_TBL domain_tbl;
            public STRING_TBL string_tbl;
            public DICT_REF_TBL dict_ref_tbl;
            //public LOCAL_VAR_TBL local_var_tbl;
            public CMD_NUM_ID_TBL cmd_num_id_tbl;
        }

        public struct DOMAIN_FIXED
        {
            public ushort index;
            public byte byObjectCode;
            public ushort wDomainDataSize;
            //public byte byReserved1;
            //public byte byReserved2;
            //public ushort wReserved3;
            public uint longAddress;
            public byte byDomainState;
            public byte byUploadState;
            public char chCounter;
        }

        public struct FORMAT_EXTENSION
        {
            public byte byLength;
            public byte byFormatObjectCode;
            public byte byCodingFormatMajor;
            public byte byCodingFormatMinor;
            public byte byDDRevision;
            public char pchProfileNumber1;
            public char pchProfileNumber2;
            public ushort wNumberOfImports;
            public ushort wNumberOfLikes;
        }


        const int DD_ODES_INDEX_OFFSET = 0;
        const int RAM_ROM_FLAG_OFFSET = 2;
        const int NAME_LENGTH_OFFSET = 3;
        const int ACCESS_PROT_FLAG_OFFSET = 4;
        const int VERSION_OFFSET = 5;
        const int LOC_ADDR_ODES_OFFSET = 7;
        const int STOD_LENGTH_OFFSET = 11;
        const int LOC_ADDR_STOD_OFFSET = 13;
        const int SOD_FIRST_INDX_OFFSET = 17;
        const int SOD_LENGTH_OFFSET = 19;
        const int LOC_ADDR_SOD_OFFSET = 21;
        const int DPOD_FIRST_INDX_OFFSET = 25;
        const int DPOD_LENGTH_OFFSET = 27;
        const int LOC_ADDR_DPOD_OFFSET = 29;
        const int DVOD_FIRST_INDX_OFFSET = 33;
        const int DVOD_LENGTH_OFFSET = 35;
        const int LOC_ADDR_DVOD_OFFSET = 37;

        const int DD_ODES_SIZE = 41;
        const int DOMAIN_FIXED_SIZE = 16;

        const int DOM_FIXED_INDX_OFFSET = 0;
        const int OBJ_CODE_OFFSET = 2;
        const int DOM_DATA_SIZE_OFFSET = 3;
        const int LONG_ADDR_OFFSET = 9;
        const int DOM_STATE_OFFSET = 13;
        const int UPLOAD_STATE_OFFSET = 14;
        const int COUNTER_OFFSET = 15;


        const int FORMAT_OBJECT_TYPE = 128;
        const int DEVICE_DIR_TYPE = 129;
        const int BLOCK_DIR_TYPE = 130;

        const int FORMAT_BIG_ENDIAN = 1;
        const int FORMAT_LITTLE_ENDIAN = 2;

        const int FMT_EXTN_LENGTH_OFFSET = 0;
        const int FMT_OBJ_CODE_OFFSET = 1;
        const int CODING_FMT_MAJ_OFFSET = 2;
        const int CODING_FMT_MIN_OFFSET = 3;
        const int DDREV_OFFSET = 4;
        const int PROFILE_NO_OFFSET = 5;
        const int NO_O_IMPORTS_OFFSET = 7;
        const int NO_O_LIKES_OFFSET = 9;

        public static FLAT_DEVICE_DIR device_dir;
        public static BIN_DEVICE_DIR bin_dev_dir;
        public static FLAT_DEVICE_DIR_6 device_dir_6;
        public static BIN_DEVICE_DIR_6 bin_dev_dir_6;
        public static FLAT_BLOCK_DIR_6 block_dir_6;
        public static BIN_BLOCK_DIR_6 bin_blk_dir_6;

        //const int SYM_EXTN_LEN = 4;
        const string DEFAULT_LANGUAGE_CODE = "|en|";

        bool isInTokizer = false;

        static bool bGlobalDictAllocated = false;

        public static HartDictionary pGlobalDict;
        public static litstringtable pLitStringTable;// = new litstringtable();

        public static DOMAIN_FIXED[] ObjectFixed = new DOMAIN_FIXED[MAX_SOD];

        public static byte[] byExtLengths = new byte[MAX_SOD]; /*Array to hold the object Extension Lengths*/ //Vibhor 300904: Restored
        //byte* pbyExtensions[MAX_SOD];/*Array of pointers pointing to the Extension parts of objects*/
        public static byte[][] pbyExtensions = new byte[MAX_SOD][];
        //byte* pbyObjectValue[MAX_SOD];/*Array of pointers pointing to the value (data) parts of the objects*/
        public static byte[][] pbyObjectValue = new byte[MAX_SOD][];

        public struct DEV_ID
        {
            public UInt32 ulMfgID;
            public ushort uDevType;
            public byte byDevRev;
            public byte byDDRev;

        }

        public struct DDOD_HEADER
        {
            public uint magic_number;
            public uint header_size;
            public uint objects_size;
            public uint data_size;
            public byte[] byManufacturer;
            public ushort device_type;
            public byte device_revision;
            public byte dd_revision;
            public byte tok_rev_major;/*Vibhor 300904: New binary spec,2 bytes from reserved1 */
            public byte tok_rev_minor;/*moved out & defined as these 2 rev nos.*/
            public ushort reserved1;
            public uint fileSignature;
            //public uint reserved3;
            //public uint reserved4;
        }

        public struct DD_ODES
        {
            public ushort index;
            public byte ram_rom_flag;
            public byte name_length;
            public byte access_protection_flag;
            public ushort version;

            public uint local_address_odes;

            public ushort STOD_length;

            public uint local_address_stod;

            public ushort sod_first_index;
            public ushort sod_length;
            public uint local_address_sod;

            public ushort dpod_first_index;
            public ushort dpod_length;
            public uint local_address_dpod;


            public ushort dvod_first_index;
            public ushort dvod_length;
            public uint local_address_dvod;
        }

        public struct IMAGEFRAME_S
        {
            public uint ifs_size;      // in bytes
            public byte[] ifs_pRawFrame;   // pointer to the raw image
            public uint ifs_offset;        // for reference
            public string ifs_language; // only the first 3 are used
        }

        string langCode;

        BinaryReader fbr;
        //BinaryWriter fbw;

        public static DDOD_HEADER header;
        DD_ODES descriptor;

        DEV_ID devID;

        byte CodingMajor; /*Major Revision Number of the Tokenizer*/
        byte CodingMinor; /*Minor Revision Number of the Tokenizer*/
        byte TokenizerType;
        byte DDRevision;  /*DD Revision*/
        ushort ImpCnt;      /*Number of Imports in the DD*/
        ushort LikCnt;      /*Number of Likes in the DD*/

        //string strInputFileName;
        string dictfilepath;
        string symFilePath;

        ushort uSodLen;
        public static ushort uSODLength;
        //bool bDevDirAllocated;
        //bool bDevDir6Allocated; //Vibhor 080904: Added
        //bool bDictAllocated;
        //bool bSymAllocated;

        //bool bTokRev6Flag = false;
        /*Vibhor 090804: This flag will help some functions down under, 
										   decide which DeviceDirectory to use, HART 5 or HART 6*/

        byte[] pGraphics;// raw image blob

        public List<DDlBaseItem> ItemsList = new List<DDlBaseItem>(); /*This is the list of all the Items defined in the 
							    DD file for the particular device*/
        public List<List<IMAGEFRAME_S>> ImagesList = new List<List<IMAGEFRAME_S>>();   /* a list of frame lists: frames are lang/size/ptr */
        public List<uint> CriticalParamList = new List<uint>();


        public DDlDevDescription()
        {
            uSodLen = 0;
            devID = new DEV_ID();
            pLitStringTable = new litstringtable();

            device_dir = new FLAT_DEVICE_DIR();
            bin_dev_dir = new BIN_DEVICE_DIR();
            device_dir_6 = new FLAT_DEVICE_DIR_6();
            bin_dev_dir_6 = new BIN_DEVICE_DIR_6();
            block_dir_6 = new FLAT_BLOCK_DIR_6();
            bin_blk_dir_6 = new BIN_BLOCK_DIR_6();
            //bDevDirAllocated = false;
            //bDictAllocated = false;
            //bSymAllocated = false;

            //// for unitialized read (purify)
            //bDevDir6Allocated = false;
        }

        public DDlDevDescription(string dictPath)
        {
            uSodLen = 0;
            devID = new DEV_ID();
            //bDevDirAllocated = false;
            //bDictAllocated = false;
            //bSymAllocated = false;
            //bDevDir6Allocated = false;// sjv 11jan06 - try this

            dictfilepath = dictPath;
            device_dir = new FLAT_DEVICE_DIR();
            bin_dev_dir = new BIN_DEVICE_DIR();
            device_dir_6 = new FLAT_DEVICE_DIR_6();
            bin_dev_dir_6 = new BIN_DEVICE_DIR_6();
            block_dir_6 = new FLAT_BLOCK_DIR_6();
            bin_blk_dir_6 = new BIN_BLOCK_DIR_6();

            ItemsList.Clear();
            ImagesList.Clear();
            CriticalParamList.Clear();
            ClearArrays();
            pLitStringTable = new litstringtable();
        }

        private void ClearArrays()
        {
            for (int i = 0; i < MAX_SOD; i++)
            {
                pbyExtensions[i] = null;// assumed random or already deleted

                pbyObjectValue[i] = null;
            }
        }

        public static returncode ReadHeader(BinaryReader read, ref DDOD_HEADER hdr)
        {
            int iRetVal;
            byte[] bytempHeader = new byte[HEADER_SIZE * 2];

            iRetVal = read.Read(bytempHeader, 0, HEADER_SIZE);

            if (iRetVal != HEADER_SIZE)
            {
                return returncode.eFileErr;
            }

            hdr.magic_number = BitConverter.ToUInt32(bytempHeader, MAGIC_NUMBER_OFFSET);// * ((uint*) &bytempHeader[MAGIC_NUMBER_OFFSET]);
            hdr.header_size = BitConverter.ToUInt32(bytempHeader, HEADER_SIZE_OFFSET);//*((uint*) &bytempHeader[HEADER_SIZE_OFFSET]);
            hdr.objects_size = BitConverter.ToUInt32(bytempHeader, OBJECTS_SIZE_OFFSET);//**((uint*) &bytempHeader[OBJECTS_SIZE_OFFSET]);
            hdr.data_size = BitConverter.ToUInt32(bytempHeader, DATA_SIZE_OFFSET);//**((uint*) &bytempHeader[DATA_SIZE_OFFSET]);
            hdr.byManufacturer[0] = bytempHeader[MANUFACTURER_OFFSET];
            hdr.byManufacturer[1] = bytempHeader[MANUFACTURER_OFFSET + 1];
            hdr.byManufacturer[2] = bytempHeader[MANUFACTURER_OFFSET + 2];
            hdr.device_type = BitConverter.ToUInt16(bytempHeader, DEVICE_TYPE_OFFSET);// * ((ushort*) &bytempHeader[DEVICE_TYPE_OFFSET]);
            hdr.device_revision = bytempHeader[DEVICE_REV_OFFSET];
            hdr.dd_revision = bytempHeader[DD_REV_OFFSET];
            hdr.tok_rev_major = bytempHeader[TOKENIZER_MAJOR_REV_OFFSET];
            hdr.tok_rev_minor = bytempHeader[TOKENIZER_MINOR_REV_OFFSET];
            hdr.reserved1 = BitConverter.ToUInt16(bytempHeader, RESERVED1_OFFSET);//*((ushort*) &bytempHeader[RESERVED1_OFFSET]);
            hdr.fileSignature = BitConverter.ToUInt32(bytempHeader, SIGNATURE_OFFSET);//*((uint*)  &bytempHeader[SIGNATURE_OFFSET]); ;
            //hdr.reserved3;
            //hdr.reserved4;

            return returncode.eOk;
        }

        public returncode LoadDictionary(string pchLanguageCode)
        {
            //returncode iRetVal;

            /*Vibhor 180105: Start of Code*/

            /*Modifications for loading E&H dictionary*/
            /* 7aug07 - mods for loading the Siemens dictionary */

            string dictfile, draegerfile, mmifile, EnHfile, Siemens;

            dictfile = dictfilepath + "standard.dct";
            draegerfile = dictfilepath + "standard.dct";
            mmifile = dictfilepath + "standard.dct";
            EnHfile = dictfilepath + "standard.dct";
            Siemens = dictfilepath + "standard.dct";

            /*Assumption: The standard dictionary should be in the same location as the execuatble*/

            string[] chDictionaryExtensions =
            {
                draegerfile,
                mmifile,
                EnHfile,
                Siemens,	// end 7aug7 
    			null
            };

            /*Vibhor 180105: End of Code*/

            pGlobalDict = new HartDictionary(pchLanguageCode);
            // this MUST be passed in...(it belongs to the device) pLitStringTable  = new LitStringTable();
            if (pGlobalDict != null)    // J.U. routine check
            {
                int iRet = pGlobalDict.makedict(dictfile, chDictionaryExtensions);

                if (iRet != 0)//??????
                {
                    /*Log an error in the error file in the calling routine */
                    pGlobalDict = null; // J.U. null pGlobalDict
                    return returncode.eDicErr;
                }

//                bDictAllocated = true; /*We have allocated the dictionary , so we own the responsiblty to delete it too!!*/
                bGlobalDictAllocated = true;

                return returncode.eOk;
            }
            else
            {
                return returncode.eDicErr;   // J.U. if new pGlobal dictionari is not created
            }

        }/*End LoadDictionary */

        public bool ValidateHeader(ref DDOD_HEADER pDDODheader)
        {

            uint dwMagicNumber = 0;
            Endian.read_dword(ref dwMagicNumber, pDDODheader.magic_number, FORMAT_BIG_ENDIAN);
            if (dwMagicNumber != 0x7F3F5F77L)
            {
                return false;
            }
            pDDODheader.magic_number = dwMagicNumber;

            uint dwHeaderSize = 0;
            Endian.read_dword(ref dwHeaderSize, pDDODheader.header_size, FORMAT_BIG_ENDIAN);
            if (dwHeaderSize < HEADER_SIZE)
            {
                return false;
            }
            pDDODheader.header_size = dwHeaderSize;

            /* size of the Objects section */
            uint dwDDObjectsSectionSize = 0;
            Endian.read_dword(ref dwDDObjectsSectionSize, pDDODheader.objects_size, FORMAT_BIG_ENDIAN);

            pDDODheader.objects_size = dwDDObjectsSectionSize;

            /* size of the Objects Data section */
            uint dwDDObjectsDataSectionSize = 0;
            Endian.read_dword(ref dwDDObjectsDataSectionSize, pDDODheader.data_size, FORMAT_BIG_ENDIAN);

            pDDODheader.data_size = dwDDObjectsDataSectionSize;

            /*Add code to read the Manufacturer */


            uint dwMfg = 0;
            Endian.read_dword_spl(ref dwMfg, pDDODheader.byManufacturer, MFG_ID_SIZE, FORMAT_BIG_ENDIAN);

            devID.ulMfgID = dwMfg;

            /* Device Type */
            ushort wDeviceType = 0;
            Endian.read_word(ref wDeviceType, pDDODheader.device_type, FORMAT_BIG_ENDIAN);

            devID.uDevType = wDeviceType;

            pDDODheader.device_type = wDeviceType;
            /*Add code to read the DevRev & DDRev too */

            devID.byDevRev = pDDODheader.device_revision;
            devID.byDDRev = pDDODheader.dd_revision;


            /* TODO!!! : It would be a good check to verify the Device ID against the
              Mfg , Devtype, DevRev & DDRev obtained from the DD Name */

            return true;
        }

        public bool ValidateObjectDescription(ref DD_ODES pDescriptor)
        {
            ushort wVersion = 0;
            Endian.read_word(ref wVersion, pDescriptor.version, FORMAT_BIG_ENDIAN);
            pDescriptor.version = wVersion;


            ushort wFirstIndex = 0;
            Endian.read_word(ref wFirstIndex, pDescriptor.sod_first_index, FORMAT_BIG_ENDIAN);
            if (wFirstIndex != 100)
            {
                return false;
            }
            pDescriptor.sod_first_index = wFirstIndex;

            ushort wSODLength = 0;// a ushort
            Endian.read_word(ref wSODLength, pDescriptor.sod_length, FORMAT_BIG_ENDIAN);
            pDescriptor.sod_length = wSODLength;

            if (wSODLength >= MAX_SOD)// ste at x4000 well short of an ushort
            {// tell everybody
                return false;//too many sod entry
            }

            uint wLocalAddressSOD = 0;
            Endian.read_dword(ref wLocalAddressSOD, pDescriptor.local_address_sod, FORMAT_BIG_ENDIAN);
            pDescriptor.local_address_sod = wLocalAddressSOD;

            return true;

        }


        public bool ReadObjectDescription()
        {
            int iRetVal;
            byte[] byTmpBuf = new byte[DD_ODES_SIZE];

            iRetVal = fbr.Read(byTmpBuf, 0, DD_ODES_SIZE);

            if (iRetVal != DD_ODES_SIZE)
            {
                return false;
            }

            descriptor.index = BitConverter.ToUInt16(byTmpBuf, DD_ODES_INDEX_OFFSET);
            descriptor.ram_rom_flag = byTmpBuf[RAM_ROM_FLAG_OFFSET];
            descriptor.name_length = byTmpBuf[NAME_LENGTH_OFFSET];
            descriptor.access_protection_flag = byTmpBuf[ACCESS_PROT_FLAG_OFFSET];
            descriptor.version = BitConverter.ToUInt16(byTmpBuf, VERSION_OFFSET);
            descriptor.local_address_odes = BitConverter.ToUInt32(byTmpBuf, LOC_ADDR_ODES_OFFSET);
            descriptor.STOD_length = BitConverter.ToUInt16(byTmpBuf, STOD_LENGTH_OFFSET);
            descriptor.local_address_stod = BitConverter.ToUInt32(byTmpBuf, LOC_ADDR_STOD_OFFSET);
            descriptor.sod_first_index = BitConverter.ToUInt16(byTmpBuf, SOD_FIRST_INDX_OFFSET);
            descriptor.sod_length = BitConverter.ToUInt16(byTmpBuf, SOD_LENGTH_OFFSET);
            descriptor.local_address_sod = BitConverter.ToUInt32(byTmpBuf, LOC_ADDR_SOD_OFFSET);
            descriptor.dpod_first_index = BitConverter.ToUInt16(byTmpBuf, DPOD_FIRST_INDX_OFFSET);
            descriptor.dpod_length = BitConverter.ToUInt16(byTmpBuf, DPOD_LENGTH_OFFSET);
            descriptor.local_address_dpod = BitConverter.ToUInt32(byTmpBuf, LOC_ADDR_DPOD_OFFSET);
            descriptor.dvod_first_index = BitConverter.ToUInt16(byTmpBuf, DVOD_FIRST_INDX_OFFSET);
            descriptor.dvod_length = BitConverter.ToUInt16(byTmpBuf, DVOD_LENGTH_OFFSET);
            descriptor.local_address_dvod = BitConverter.ToUInt32(byTmpBuf, LOC_ADDR_DVOD_OFFSET);
            return true;
        }

        public returncode LoadDeviceDescription(bool isInTok = false)
        {
            isInTokizer = isInTok;

            if (header.byManufacturer == null)
            {
                header.byManufacturer = new byte[3];
            }

            returncode rc = ReadHeader(fbr, ref header);
            if (rc != returncode.eOk)
            {
                return rc;
            }

            bool bRet = ValidateHeader(ref header);
            if (!bRet)
            {
                return returncode.eDDHeaderErr;
            }

            bRet = ReadObjectDescription();
            if (!bRet)
            {
                return returncode.eDDOdError;
            }

            bRet = ValidateObjectDescription(ref descriptor);
            if (!bRet)
            {
                return returncode.eDDOdError;
            }
            uSODLength = descriptor.sod_length;

            uSodLen = uSODLength;

            bRet = ReadSOD();
            if (!bRet)
            {
                return returncode.eSOdError;
            }

            bRet = ReadObjectValues();
            if (!bRet)
            {
                return returncode.eObjectError;
            }

            /*Vibhor 010904: Start of Multiple Code Changes*/

            /*Read the Format Object and determine the Tokenizer Revision numbers*/

            bRet = ReadFormatObject();

            if (!bRet)
            {
                return returncode.eSOdError;
            }
            /*NOTE: We may need to read the binary file in LoadDeviceDirectory_6(), 
                    so holding it till done, whereas in HART 5 LoadDeviceDirectory
                    we don't need it, so we close the before loading the directory*/

            if ((CodingMajor == DDOD_REV_SUPPORTED_EIGHT && CodingMinor <= DDOD_REV_MINOR_HCF) 
                || CodingMajor == DDOD_REV_SUPPORTED_SIX)
            {
                bRet = LoadDeviceDirectory_6();
                if (!bRet)
                {
                    return returncode.eDDOdError;
                }
                /*Close the fms file as its no more needed*/
                if (fbr != null)
                {
                    fbr.Close();
                }

                Common.bTokRev6Flag = true;

                bRet = GetItems6();
                if (!bRet)
                {
                    return returncode.eDDOdError;
                }

                bRet = GetImages6();
                if (!bRet)
                {
                    return returncode.eDDOdError;
                }

            }
            else if (CodingMajor == DDOD_REV_SUPPORTED_FIVE)
            {
                /*Close the fms file as its no more needed*/

                if (fbr != null)
                {
                    fbr.Close();
                }

                bRet = LoadDeviceDirectory();
                if (bRet != true) /*Quit if it fails!!*/
                {
                    return returncode.eDDOdError;
                }

                Common.bTokRev6Flag = false;// Just be safe.

                bRet = GetItems6();//items
                if (!bRet)
                {
                    return returncode.eDDOdError;
                }
            }
            else
            {
                return returncode.eDDOdError;
            }

            /*Vibhor 010904: End of Multiple Code Changes*/

            ResolveItemName();

            //CleanGlobals();
            return returncode.eOk;

        }
        /*
        public static int RD_DATAPARTSEG(d, s)
        {
            pBlkDirExt.d.offset = *((long_offset*)&pbyPointer[(s)]);
            pBlkDirExt.d.uSize = *((uint*)&pbyPointer[(s) + SEG_SIZE_OFFSET_6]);
        }
        */
        public bool LoadDeviceDirectory_6()
        {
            int iRetVal;
            ushort objectIndex;
            //bool bDevDirLoadedFlag = false;

            for (int i = 0; i < descriptor.sod_length; i++)
            {
                iRetVal = 0;
                objectIndex = ObjectFixed[i].index;

                switch (pbyExtensions[i][1])
                {
                    case DEVICE_DIR_TYPE:
                        {   // no classes in FLAT_DEVICE_DIR_6 nor BIN_DEVICE_DIR_6; memset ok
                            //(void)memset((char*)&device_dir_6, 0, sizeof(FLAT_DEVICE_DIR_6));
                            //(void)memset((char*)&bin_dev_dir_6, 0, sizeof(BIN_DEVICE_DIR_6));

                            /*
                            FLAT_DEVICE_DIR_6* flatDevDir = &device_dir_6;
                            BIN_DEVICE_DIR_6* bin_dev_dir_6 = &bin_dev_dir_6;
                            BININFO binTablePtr;
                            DATAPART_SEGMENT_6 dirExtnOffset;
                            */
                            uint dwOffset = 0;
                            uint dwTblLength = 0;
                            bool bRet;

                            DEVICE_DIR_EXT_6 pDevDirExt;
                            byte[] pbyPointer = pbyExtensions[i];

                            pDevDirExt = new DEVICE_DIR_EXT_6();
                            //memset(pDevDirExt, 0, sizeof(DEVICE_DIR_EXT_6));// ok


                            pDevDirExt.byLength = pbyPointer[DEV_DIR_LENGTH_6_OFFSET]; // Not sure abt this guy
                            pDevDirExt.byDeviceDirObjectCode = pbyPointer[DEV_DIR_OBJ_CODE_6_OFFSET];
                            pDevDirExt.byFormatCode = pbyPointer[DEV_DIR_FORMAT_CODE_6_OFFSET];
                            if (CodingMajor > 7)
                            {
                                pDevDirExt.ItemTable.offset = BitConverter.ToUInt32(pbyPointer, ITEM_TBL_8_OFFSET);
                                pDevDirExt.ItemTable.uSize = BitConverter.ToUInt32(pbyPointer, ITEM_TBL_8_OFFSET + SEG_SIZE_OFFSET_8);
                                pDevDirExt.StringTable.offset = BitConverter.ToUInt32(pbyPointer, STRNG_TBL_8_OFFSET);
                                pDevDirExt.StringTable.uSize = BitConverter.ToUInt32(pbyPointer, STRNG_TBL_8_OFFSET + SEG_SIZE_OFFSET_8);
                                pDevDirExt.DictReferenceTable.offset = BitConverter.ToUInt32(pbyPointer, DICT_REF_TBL_8_OFFSET);
                                pDevDirExt.DictReferenceTable.uSize = BitConverter.ToUInt32(pbyPointer, DICT_REF_TBL_8_OFFSET + SEG_SIZE_OFFSET_8);
                                pDevDirExt.CommandTable.offset = BitConverter.ToUInt32(pbyPointer, CMD_TBL_8_OFFSET);
                                pDevDirExt.CommandTable.uSize = BitConverter.ToUInt32(pbyPointer, CMD_TBL_8_OFFSET + SEG_SIZE_OFFSET_8);
                                pDevDirExt.ImageTable.offset = BitConverter.ToUInt32(pbyPointer, IMG_TBL_8_OFFSET);
                                pDevDirExt.ImageTable.uSize = BitConverter.ToUInt32(pbyPointer, IMG_TBL_8_OFFSET + SEG_SIZE_OFFSET_8);
                                /*Do Some Validations*/
                                if (pDevDirExt.byLength < DEVICE_DIR_LENGTH_8)
                                {
                                    //delete pDevDirExt;
                                    return false; /* INVALID_EXTN_LENGTH*/
                                }
                            }
                            else // CodingMajor is 6 (or 7)
                            {
                                pDevDirExt.BlockNameTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_NAME_TBL_6_OFFSET);
                                pDevDirExt.BlockNameTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_NAME_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.ItemTable.offset = BitConverter.ToUInt32(pbyPointer, ITEM_TBL_6_OFFSET);
                                pDevDirExt.ItemTable.uSize = BitConverter.ToUInt32(pbyPointer, ITEM_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.ProgramTable.offset = BitConverter.ToUInt32(pbyPointer, PROG_TBL_6_OFFSET);
                                pDevDirExt.ProgramTable.uSize = BitConverter.ToUInt32(pbyPointer, PROG_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.DomainTable.offset = BitConverter.ToUInt32(pbyPointer, DOM_TBL_6_OFFSET);
                                pDevDirExt.DomainTable.uSize = BitConverter.ToUInt32(pbyPointer, DOM_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.StringTable.offset = BitConverter.ToUInt32(pbyPointer, STRNG_TBL_6_OFFSET);
                                pDevDirExt.StringTable.uSize = BitConverter.ToUInt32(pbyPointer, STRNG_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.DictReferenceTable.offset = BitConverter.ToUInt32(pbyPointer, DICT_REF_TBL_6_OFFSET);
                                pDevDirExt.DictReferenceTable.uSize = BitConverter.ToUInt32(pbyPointer, DICT_REF_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.LocalVariableTable.offset = BitConverter.ToUInt32(pbyPointer, LOC_VAR_TBL_6_OFFSET);
                                pDevDirExt.LocalVariableTable.uSize = BitConverter.ToUInt32(pbyPointer, LOC_VAR_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.CommandTable.offset = BitConverter.ToUInt32(pbyPointer, CMD_TBL_6_OFFSET);
                                pDevDirExt.CommandTable.uSize = BitConverter.ToUInt32(pbyPointer, CMD_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pDevDirExt.ImageTable.offset = BitConverter.ToUInt32(pbyPointer, IMG_TBL_6_OFFSET);
                                pDevDirExt.ImageTable.uSize = BitConverter.ToUInt32(pbyPointer, IMG_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                /*Do Some Validations*/
                                if (pDevDirExt.byLength < DEVICE_DIR_LENGTH_6) //Vibhor 280904: Changed
                                {
                                    return false; /* INVALID_EXTN_LENGTH*/
                                }
                            }


                            if (pDevDirExt.byDeviceDirObjectCode != DEVICE_DIR_TYPE)
                            {
                                return false; /* DIR_TYPE_MISMATCH*/
                            }

                            if ((bin_dev_dir_6.bin_exists) == 0)
                            {
                                bin_dev_dir_6.bin_exists = 0;

                                if (pDevDirExt.BlockNameTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << BLK_TBL_ID);
                                if (pDevDirExt.ItemTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << ITEM_TBL_ID);
                                if (pDevDirExt.ProgramTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << PROG_TBL_ID);
                                if (pDevDirExt.DomainTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << DOMAIN_TBL_ID);
                                if (pDevDirExt.StringTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << STRING_TBL_ID);
                                if (pDevDirExt.DictReferenceTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << DICT_REF_TBL_ID);
                                if (pDevDirExt.LocalVariableTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << LOCAL_VAR_TBL_ID);
                                if (pDevDirExt.CommandTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << CMD_NUM_ID_TBL_ID);
                                if (pDevDirExt.ImageTable.uSize != 0)
                                    bin_dev_dir_6.bin_exists |= (1 << IMAGE_TBL_ID);
                            }

                            /* start stevev added  01nov05 */
                            /* handles the too long heap reference */

                            if (ObjectFixed[i].wDomainDataSize == 0xffff && pbyObjectValue[i] == null)
                            {
                                int sizeTotal = 0;
                                //dirExtnOffset = &(pDevDirExt.StringTable);
                                if (CodingMajor <= 7)
                                {
                                    bRet = Endian.read_dword(ref dwOffset, pDevDirExt.BlockNameTable.offset, FORMAT_BIG_ENDIAN);
                                    bRet = Endian.read_dword(ref dwTblLength, (uint)pDevDirExt.BlockNameTable.uSize, FORMAT_BIG_ENDIAN);
                                    if (bRet != false)
                                    {
                                        sizeTotal += (int)dwTblLength; 
                                    }

                                    bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.ProgramTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false)
                                    { 
                                        sizeTotal += (int)dwTblLength; 
                                    }
                                    bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.DomainTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false)
                                    { 
                                        sizeTotal += (int)dwTblLength;
                                    }

                                    bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.LocalVariableTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false)
                                    {
                                        sizeTotal += (int)dwTblLength; 
                                    }
                                }
                                else
                                {
                                    bRet = Endian.read_dword(ref dwOffset, (pDevDirExt.ItemTable.offset), FORMAT_BIG_ENDIAN);
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.ItemTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false)
                                { 
                                    sizeTotal += (int)dwTblLength;
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.StringTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false)
                                { 
                                    sizeTotal += (int)dwTblLength;
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.DictReferenceTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false)
                                { 
                                    sizeTotal += (int)dwTblLength;
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.CommandTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false)
                                { 
                                    sizeTotal += (int)dwTblLength;
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pDevDirExt.ImageTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false)
                                { 
                                    sizeTotal += (int)dwTblLength; 
                                }


                                // seek to heap
                                long offset = ObjectFixed[i].longAddress + header.header_size + header.objects_size;

                                //iRetVal = fseek(fp, (long)offset, 0);
                                iRetVal = (int)fbr.BaseStream.Seek((Int32)offset, SeekOrigin.Begin);

                                if (iRetVal < 0)
                                {
                                    pbyObjectValue[i] = null;
                                }
                                else
                                {   // alloc size bytes to pbyObjectValue[i]
                                    pbyObjectValue[i] = new byte[sizeTotal];
                                    // read in size

                                    //iRetVal = fread((byte*)pbyObjectValue[i], 1, sizeTotal, fp);
                                    iRetVal = fbr.Read(pbyObjectValue[i], 0, sizeTotal);

                                    if (iRetVal != sizeTotal)
                                    {/*
                                        if (fbr.BaseStream.Position > fbr.BaseStream.Length)
                                        {
                                            //LOGIT(CERR_LOG, L"End of File reached unexpectedly.\n");
                                        }
                                        else
                                        {
                                            if (ferror(fp))
                                            {
                                                //perror( "File read failure" );	 PAW 09/04/09 see below
                                                //fprintf(stderr, "File read failure");// PAW 09/04/09 see below
                                                //LOGIT(CERR_LOG, "File read failure.\n"); // stevev 12aug10
                                            }
                                            else
                                            {
                                                //delete[] pbyObjectValue;//via DD@F
                                                CleanArrays();
                                                //LOGIT(CERR_LOG, L" Count mismatch without EOF and without a file error.\n");
                                            }
                                        }*/
                                        return false;
                                    }
                                }
                            }
                            /* end added 01nov05 */
                            ushort uTag = 0;
                            uint ulTableMaskBit = 0;
                            //binTablePtr = (BININFO*)0L;
                            uint ulReqMask = DEVICE_TBL_MASKS;
                            long lOffset = 0;

                            while ((ulReqMask != 0) && (uTag < MAX_DEVICE_TBL_ID_HCF_6))
                            {
                                /*
                                * Check for request mask bit corresponding to the tag value.
                                * Skip to next tag value if not requested.
                                */
                                lOffset = 0;

                                if (((ulReqMask) & (1L << uTag)) == 0)
                                {
                                    uTag++;
                                    continue;
                                }
                                /*
                                * Point to appropriate values for the table type
                                */

                                switch (uTag++)
                                {
                                    case BLK_TBL_ID:    /* Block Table */
                                                        /*			ulTableMaskBit = BLK_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.BlockNameTable);
                                                                    binTablePtr =&(bin_dev_dir_6.blk_tbl); */
                                        break;

                                    case ITEM_TBL_ID:   /* Item Table */
                                        {
                                            ulTableMaskBit = ITEM_TBL_MASK;
                                            //dirExtnOffset = &(pDevDirExt.ItemTable);
                                            //binTablePtr = &(bin_dev_dir_6.item_tbl);

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                            if ((bin_dev_dir_6.item_tbl.chunk) == null)
                                            {
                                                bRet = Endian.read_dword(ref dwOffset, pDevDirExt.ItemTable.offset, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    //delete pDevDirExt;
                                                    return false;
                                                }

                                                bRet = Endian.read_dword(ref dwTblLength, pDevDirExt.ItemTable.uSize, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }
                                                /*
                                                 * Attach the table if non-zero length, else go
                                                 * to the next table
                                                 */

                                                if (dwTblLength != 0)
                                                {
                                                    if (null != pbyObjectValue[i])
                                                    {
                                                        bin_dev_dir_6.item_tbl.chunk = pbyObjectValue[i];
                                                        bin_dev_dir_6.item_tbl.size = dwTblLength;
                                                        bin_dev_dir_6.item_tbl.uoffset = dwOffset;
                                                    }
                                                    else
                                                    {
                                                        bin_dev_dir_6.item_tbl.size = dwTblLength;
                                                        bin_dev_dir_6.item_tbl.chunk = new byte[dwTblLength];

                                                        lOffset = ObjectFixed[i].longAddress + header.header_size + header.objects_size + dwTblLength;

                                                        //iRetVal = fseek(fp, (long)lOffset, SEEK_SET);
                                                        iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.Begin);

                                                        if (iRetVal < 0)
                                                        {
                                                            //binTablePtr.chunk = null;
                                                            return false;
                                                        }
                                                        //iRetVal = fread((byte*)binTablePtr.chunk, 1, dwTblLength, fp);
                                                        iRetVal = fbr.Read(pbyObjectValue[i], 0, (int)dwTblLength);

                                                    }
                                                    bin_dev_dir_6.bin_hooked |= ulTableMaskBit;
                                                }
                                                // no table available (0 length)
                                            }
                                        }
                                        break;

                                    case PROG_TBL_ID:   /* Program Table */
                                                        /*			ulTableMaskBit = PROG_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.ProgramTable);
                                                                    binTablePtr = &(bin_dev_dir_6.prog_tbl); */
                                        break;

                                    case DOMAIN_TBL_ID: /* Domain Table */
                                                        /*			ulTableMaskBit = DOMAIN_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.DomainTable);
                                                                    binTablePtr = &(bin_dev_dir_6.domain_tbl); */
                                        break;

                                    case STRING_TBL_ID: /* String Table */
                                        ulTableMaskBit = STRING_TBL_MASK;
                                        //dirExtnOffset = &(pDevDirExt.StringTable);
                                        //binTablePtr = &(bin_dev_dir_6.string_tbl);
                                        ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                        if ((bin_dev_dir_6.string_tbl.chunk) == null)
                                        {
                                            bRet = Endian.read_dword(ref dwOffset, pDevDirExt.StringTable.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref dwTblLength, pDevDirExt.StringTable.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            /*
                                             * Attach the table if non-zero length, else go
                                             * to the next table
                                             */

                                            if (dwTblLength > 0)
                                            {
                                                if (null != pbyObjectValue[i])
                                                {
                                                    bin_dev_dir_6.string_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir_6.string_tbl.size = dwTblLength;
                                                    bin_dev_dir_6.string_tbl.uoffset = dwOffset;
                                                }
                                                else
                                                {
                                                    bin_dev_dir_6.string_tbl.size = dwTblLength;
                                                    bin_dev_dir_6.string_tbl.chunk = new byte[dwTblLength];

                                                    lOffset = ObjectFixed[i].longAddress + header.header_size + header.objects_size + dwTblLength;

                                                    //iRetVal = fseek(fp, (long)lOffset, SEEK_SET);
                                                    iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.Begin);
                                                    if (iRetVal < 0)
                                                    {
                                                        return false;
                                                    }
                                                    //iRetVal = fread((byte*)binTablePtr.chunk, 1, dwTblLength, fp);
                                                    iRetVal = fbr.Read(pbyObjectValue[i], 0, (int)dwTblLength);
                                                }

                                                bin_dev_dir_6.bin_hooked |= ulTableMaskBit;
                                            }

                                        }
                                        break;

                                    case DICT_REF_TBL_ID:   /* Dictionary Reference Table */
                                        ulTableMaskBit = DICT_REF_TBL_MASK;
                                        //dirExtnOffset = &(pDevDirExt.DictReferenceTable);
                                        //binTablePtr = &(bin_dev_dir_6.dict_ref_tbl);
                                        ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                        if ((bin_dev_dir_6.dict_ref_tbl.chunk) == null)
                                        {

                                            bRet = Endian.read_dword(ref dwOffset, pDevDirExt.DictReferenceTable.offset, FORMAT_BIG_ENDIAN);

                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref dwTblLength, pDevDirExt.DictReferenceTable.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            /*
                                             * Attach the table if non-zero length, else go
                                             * to the next table
                                             */

                                            if (dwTblLength > 0)
                                            {
                                                if (null != pbyObjectValue[i])
                                                {
                                                    bin_dev_dir_6.dict_ref_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir_6.dict_ref_tbl.size = dwTblLength;
                                                    bin_dev_dir_6.dict_ref_tbl.uoffset = dwOffset;
                                                }
                                                else
                                                {
                                                    bin_dev_dir_6.dict_ref_tbl.size = dwTblLength;
                                                    bin_dev_dir_6.dict_ref_tbl.chunk = new byte[dwTblLength];

                                                    lOffset = ObjectFixed[i].longAddress + header.header_size + header.objects_size + dwTblLength;

                                                    //iRetVal = fseek(fp, (long)lOffset, SEEK_SET);
                                                    iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.Begin);
                                                    if (iRetVal < 0)
                                                    {
                                                        return false;
                                                    }
                                                    //iRetVal = fread((byte*)binTablePtr.chunk, 1 , dwTblLength, fp);
                                                    iRetVal = fbr.Read(pbyObjectValue[i], 0, (int)dwTblLength);
                                                }
                                                bin_dev_dir_6.bin_hooked |= ulTableMaskBit;
                                            }

                                        }

                                        break;

                                    case LOCAL_VAR_TBL_ID:  /* Dictionary Reference Table */
                                                            /*		ulTableMaskBit = LOCAL_VAR_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.LocalVariableTable);
                                                                    binTablePtr = &(bin_dev_dir_6.local_var_tbl); */
                                        break;

                                    case CMD_NUM_ID_TBL_ID: /* Command Number to Item ID Table */
                                        {
                                            ulTableMaskBit = CMD_NUM_ID_TBL_MASK;
                                            //dirExtnOffset = &(pDevDirExt.CommandTable);
                                            //binTablePtr = &(bin_dev_dir_6.cmd_num_id_tbl);

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                            if ((bin_dev_dir_6.cmd_num_id_tbl.chunk) == null)
                                            {
                                                bRet = Endian.read_dword(ref dwOffset, pDevDirExt.CommandTable.offset, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }

                                                bRet = Endian.read_dword(ref dwTblLength, pDevDirExt.CommandTable.uSize, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }
                                                /*
                                                 * Attach the table if non-zero length, else go
                                                 * to the next table
                                                 */

                                                if (dwTblLength > 0)
                                                {
                                                    if (null != pbyObjectValue[i])
                                                    {
                                                        bin_dev_dir_6.cmd_num_id_tbl.chunk = pbyObjectValue[i];
                                                        bin_dev_dir_6.cmd_num_id_tbl.size = dwTblLength;
                                                        bin_dev_dir_6.cmd_num_id_tbl.uoffset = dwOffset;
                                                    }
                                                    else
                                                    {
                                                        bin_dev_dir_6.cmd_num_id_tbl.chunk = new byte[dwTblLength];
                                                        bin_dev_dir_6.cmd_num_id_tbl.size = dwTblLength;

                                                        lOffset = ObjectFixed[i].longAddress + header.header_size + header.objects_size + dwTblLength;

                                                        //iRetVal = fseek(fp, (long)lOffset, SEEK_SET);
                                                        iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.Begin);
                                                        if (iRetVal < 0)
                                                        {
                                                            return false;
                                                        }
                                                        //iRetVal = fread((byte*)binTablePtr.chunk, 1, dwTblLength, fp);
                                                        iRetVal = fbr.Read(pbyObjectValue[i], 0, (int)dwTblLength);
                                                    }
                                                    bin_dev_dir_6.bin_hooked |= ulTableMaskBit;
                                                }
                                                // no table available (0 length)
                                            }
                                        }
                                        break;

                                    case IMAGE_TBL_ID:

                                        ulTableMaskBit = IMAGE_TBL_MASK;
                                        //dirExtnOffset = &(pDevDirExt.ImageTable);
                                        //binTablePtr = &(bin_dev_dir_6.image_tbl);
                                        ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                        if ((bin_dev_dir_6.image_tbl.chunk) == null)
                                        {
                                            bRet = Endian.read_dword(ref dwOffset, pDevDirExt.ImageTable.offset, FORMAT_BIG_ENDIAN);

                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref dwTblLength, pDevDirExt.ImageTable.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            /*
                                             * Attach the table if non-zero length, else go
                                             * to the next table
                                             */
                                            if (dwTblLength > 0)
                                            {
                                                if (null != pbyObjectValue[i])
                                                {
                                                    bin_dev_dir_6.image_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir_6.image_tbl.size = dwTblLength;
                                                    bin_dev_dir_6.image_tbl.uoffset = dwOffset;
                                                }
                                                else
                                                {
                                                    bin_dev_dir_6.image_tbl.size = dwTblLength;
                                                    bin_dev_dir_6.image_tbl.chunk = new byte[dwTblLength];

                                                    lOffset = ObjectFixed[i].longAddress + header.header_size + header.objects_size + dwTblLength;

                                                    //iRetVal = fseek(fp, (long)lOffset, SEEK_SET);
                                                    iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.Begin);
                                                    if (iRetVal < 0)
                                                    {
                                                        return false;
                                                    }
                                                    //iRetVal = fread((byte*)binTablePtr.chunk, 1, dwTblLength, fp);
                                                    iRetVal = fbr.Read(pbyObjectValue[i], 0, (int)dwTblLength);
                                                }

                                                bin_dev_dir_6.bin_hooked |= ulTableMaskBit;
                                            }

                                        }
                                        break;

                                    default:    /* goes here for reserved or undefined table IDs */
                                        break;
                                }
                            }/* end while */

                            /*We have Fetched the Device dir binary chunks, now Evaluate the device directories*/

                            // timj 9oct07  was eval_dir_device_tables_6()
                            const int DIR_TABLES_OF_INTEREST = ITEM_TBL_MASK | STRING_TBL_MASK | DICT_REF_TBL_MASK | CMD_NUM_ID_TBL_MASK | IMAGE_TBL_MASK;
                            if (CodingMajor <= DDOD_REV_SUPPORTED_SIX)
                                iRetVal = Eval.eval_dir_device_tables_6(ref device_dir_6, ref bin_dev_dir_6, DIR_TABLES_OF_INTEREST);
                            else
                                iRetVal = Eval.eval_dir_device_tables_8(ref device_dir_6, ref bin_dev_dir_6, DIR_TABLES_OF_INTEREST);

                            /*DEVICE_TBL_MASKS );*/
                            if (iRetVal != Common.SUCCESS)
                            {
                                //delete pDevDirExt;
                                return false;
                            }
                            else if (device_dir_6.image_tbl.count > 0)// we have graphics
                            {
                                long lPos, lSize, lLoc, lILen;
                                //int iRetVal;
                                lOffset = header.header_size + header.objects_size + header.data_size;

                                //lPos = ftell(fp);// save for others (may not be needed)
                                lPos = fbr.BaseStream.Position;
                                //fseek(fp, 0, SEEK_END);
                                iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.End);
                                //lSize = ftell(fp);// total file size
                                lSize = fbr.BaseStream.Position;

                                //iRetVal = fseek(fp, (long)lOffset, SEEK_SET);// first graphic byte
                                iRetVal = (int)fbr.BaseStream.Seek((Int32)lOffset, SeekOrigin.Begin);
                                //lLoc = ftell(fp);
                                lLoc = fbr.BaseStream.Position;
                                lILen = lSize - lLoc;// graphic size
                                                     // debug
                                if (lLoc != lOffset)
                                {// seek did not work
                                    //LOGIT(CERR_LOG, L"Seek error brkpt.\n");
                                }
                                if (lILen < 4)
                                {// some type of file error - or no graphics
                                    //LOGIT(CERR_LOG, L"No Image data.(%d bytes for %d images)\n", lILen, flatDevDir.image_tbl.count);
                                    // stevev 11dec08, let it continue...	return DDL_SUCCESS;
                                }
                                // end debug
                                pGraphics = new byte[lILen];// allocate memory
                                //iRetVal = fread(pGraphics, 1, lILen, fp);// lILen ALL the raw images
                                iRetVal = fbr.Read(pGraphics, 0, (int)dwTblLength);
                                if (iRetVal < 0)
                                {
                                    return false;
                                }
                                else
                                {
                                    //iRetVal = fseek(fp, (long)lPos, SEEK_SET);// back were we started (in case somebody cares)
                                                                              // insert raw pointers into the image table
                                    iRetVal = (int)fbr.BaseStream.Seek((Int32)lPos, SeekOrigin.Begin);
                                    //IMAGE_TBL_ELEM pImgTblElem;// = device_dir_6.image_tbl.list[i];
                                    //IMG_ITEM pImgItm;
                                    // for each image
                                    for (int x = 0; x < device_dir_6.image_tbl.count; x++)
                                    {
                                        //pImgTblElem = device_dir_6.image_tbl.list[x];
                                        //	for each language
                                        /*for (int y = 0; y < pImgTblElem.num_langs; y++)
                                        {// set the pointer to the raw graphic
                                            pImgItm = pImgTblElem.img_list[y];
                                            pImgItm.p2Graphik = pGraphics + min((int)(pImgItm.img_file.offset), lILen);
                                        }*/
                                        for (int y = 0; y < device_dir_6.image_tbl.list[x].num_langs; y++)
                                        {
                                            int imglen = (int)Math.Min(device_dir_6.image_tbl.list[x].img_list[y].img_file.offset, lILen);
                                            byte[] bytemp = new byte[lILen - imglen];
                                            for(int j = 0; j < bytemp.Length; j++)
                                            {
                                                bytemp[j] = pGraphics[j + imglen];
                                            }
                                            device_dir_6.image_tbl.list[x].img_list[y].p2Graphik = pGraphics;
                                        }
                                    }
                                }
                            }
                            // else - no-op
                            //TODO : See memory, if allocated to binTablePtr.chunk is freed properly.
                            //bDevDirLoadedFlag = true; /*Device Directory Loaded Successfully*/
                        }
                        break;
                    case BLOCK_DIR_TYPE:
                        {   // no classes in FLAT_BLOCK_DIR_6 nor BIN_BLOCK_DIR_6; memset ok

                            //FLAT_BLOCK_DIR_6* pflatBlkDir = &block_dir_6;
                            //BIN_BLOCK_DIR_6* bin_blk_dir_6 = &bin_blk_dir_6;
                            //	BININFO *binTablePtr;
                            //	DATAPART_SEGMENT_6 *pBlkExtnOffset;
                            //	uint dwOffset;
                            uint dwTblLength = 0;
                            bool bRet = false;

                            BLOCK_DIR_EXT_6 pBlkDirExt = new BLOCK_DIR_EXT_6();
                            byte[] pbyPointer = pbyExtensions[i];

                            //pBlkDirExt = new BLOCK_DIR_EXT_6;

                            pBlkDirExt.byLength = pbyPointer[BLK_DIR_LENGTH_6_OFFSET];
                            pBlkDirExt.byBlockDirObjectCode = pbyPointer[BLK_DIR_OBJ_CODE_6_OFFSET];
                            pBlkDirExt.byFormatCode = pbyPointer[BLK_DIR_FORMAT_CODE_6_OFFSET];
                            /*
                            RD_DATAPARTSEG(BlockItemTable, BLK_ITEM_TBL_6_OFFSET);
                            RD_DATAPARTSEG(BlockItemNameTable, BLK_ITEMNAME_TBL_6_OFFSET);
                            RD_DATAPARTSEG(ParameterTable, BLK_PARAM_TBL_6_OFFSET);
                            */
                            pBlkDirExt.BlockItemTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_ITEM_TBL_6_OFFSET);
                            pBlkDirExt.BlockItemTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_ITEM_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                            pBlkDirExt.BlockItemNameTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_ITEMNAME_TBL_6_OFFSET);
                            pBlkDirExt.BlockItemNameTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_ITEMNAME_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                            pBlkDirExt.ParameterTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAM_TBL_6_OFFSET);
                            pBlkDirExt.ParameterTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAM_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);

                            if (CodingMajor < DDOD_REV_SUPPORTED_EIGHT)
                            {
                                /*
                                RD_DATAPARTSEG(ParameterMemberTable, BLK_PARAMEMBER_TBL_6_OFFSET);
                                RD_DATAPARTSEG(ParameterMemberNameTable, BLK_PARAMEMBERNAME_TBL_6_OFFSET);
                                RD_DATAPARTSEG(ParameterElementTable, BLK_ELEMENT_TBL_6_OFFSET);
                                RD_DATAPARTSEG(ParameterListTable, BLK_PARAMLIST_TBL_6_OFFSET);
                                RD_DATAPARTSEG(ParameterListMemberTable, BLK_PARAMLISTMEMBER_TBL_6_OFFSET);
                                RD_DATAPARTSEG(ParameterListMemberNameTable, BLK_PARAMLISTMEMBERNAME_TBL_6_OFFSET);
                                RD_DATAPARTSEG(CharectersiticsMemberTable, BLK_CHARMEMBER_TBL_6_OFFSET);
                                RD_DATAPARTSEG(CharectersiticsMemberNameTable, BLK_CHARMEMBERNAME_TBL_6_OFFSET);

                                RD_DATAPARTSEG(RelationTable, BLK_RELATION_TBL_6_OFFSET);
                                RD_DATAPARTSEG(UpdateTable, BLK_UPDATE_TBL_6_OFFSET);
                                RD_DATAPARTSEG(ParameterCommandTable, BLK_PARAM2COMMAND_TBL_6_OFFSET);
                                RD_DATAPARTSEG(CriticalParameterTable, BLK_CRITICALPARAM_TBL_6_OFFSET);
                                /*Do Some Validations*/
                                pBlkDirExt.ParameterMemberTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAMEMBER_TBL_6_OFFSET);
                                pBlkDirExt.ParameterMemberTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAMEMBER_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterMemberNameTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAMEMBERNAME_TBL_6_OFFSET);
                                pBlkDirExt.ParameterMemberNameTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAMEMBERNAME_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterElementTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_ELEMENT_TBL_6_OFFSET);
                                pBlkDirExt.ParameterElementTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_ELEMENT_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterListTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAMLIST_TBL_6_OFFSET);
                                pBlkDirExt.ParameterListTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAMLIST_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterListMemberTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAMLISTMEMBER_TBL_6_OFFSET);
                                pBlkDirExt.ParameterListMemberTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAMLISTMEMBER_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterListMemberNameTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAMLISTMEMBERNAME_TBL_6_OFFSET);
                                pBlkDirExt.ParameterListMemberNameTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAMLISTMEMBERNAME_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.CharectersiticsMemberTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_CHARMEMBER_TBL_6_OFFSET);
                                pBlkDirExt.CharectersiticsMemberTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_CHARMEMBER_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.CharectersiticsMemberNameTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_CHARMEMBERNAME_TBL_6_OFFSET);
                                pBlkDirExt.CharectersiticsMemberNameTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_CHARMEMBERNAME_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);

                                pBlkDirExt.RelationTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_RELATION_TBL_6_OFFSET);
                                pBlkDirExt.RelationTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_RELATION_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.UpdateTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_UPDATE_TBL_6_OFFSET);
                                pBlkDirExt.UpdateTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_UPDATE_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterCommandTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAM2COMMAND_TBL_6_OFFSET);
                                pBlkDirExt.ParameterCommandTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAM2COMMAND_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.CriticalParameterTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_CRITICALPARAM_TBL_6_OFFSET);
                                pBlkDirExt.CriticalParameterTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_CRITICALPARAM_TBL_6_OFFSET + SEG_SIZE_OFFSET_6);
                                if (pBlkDirExt.byLength < BLK_DIR_LENGTH_6)//was BLK_DIR_LEN_HCF)
                                {
                                    return false; /* INVALID_EXTN_LENGTH*/
                                }
                            }
                            else// eight and above
                            {
                                /*
                                RD_DATAPARTSEG(RelationTable, BLK_RELATION_TBL_8_OFFSET);
                                RD_DATAPARTSEG(UpdateTable, BLK_UPDATE_TBL_8_OFFSET);
                                RD_DATAPARTSEG(ParameterCommandTable, BLK_PARAM2COMMAND_TBL_8_OFFSET);
                                RD_DATAPARTSEG(CriticalParameterTable, BLK_CRITICALPARAM_TBL_8_OFFSET);
                                /*Do Some Validations*/
                                pBlkDirExt.RelationTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_RELATION_TBL_8_OFFSET);
                                pBlkDirExt.RelationTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_RELATION_TBL_8_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.UpdateTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_UPDATE_TBL_8_OFFSET);
                                pBlkDirExt.UpdateTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_UPDATE_TBL_8_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.ParameterCommandTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_PARAM2COMMAND_TBL_8_OFFSET);
                                pBlkDirExt.ParameterCommandTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_PARAM2COMMAND_TBL_8_OFFSET + SEG_SIZE_OFFSET_6);
                                pBlkDirExt.CriticalParameterTable.offset = BitConverter.ToUInt32(pbyPointer, BLK_CRITICALPARAM_TBL_8_OFFSET);
                                pBlkDirExt.CriticalParameterTable.uSize = BitConverter.ToUInt32(pbyPointer, BLK_CRITICALPARAM_TBL_8_OFFSET + SEG_SIZE_OFFSET_6);
                                if (pBlkDirExt.byLength < BLK_DIR_LENGTH_8)
                                {
                                    return false; /* INVALID_EXTN_LENGTH*/
                                }

                            }


                            if (pBlkDirExt.byBlockDirObjectCode != BLOCK_DIR_TYPE)
                            {
                                return false; /* DIR_TYPE_MISMATCH*/
                            }

                            if (bin_blk_dir_6.bin_exists == 0)
                            {
                                if (pBlkDirExt.BlockItemTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << BLK_ITEM_TBL_ID);
                                if (pBlkDirExt.BlockItemNameTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << BLK_ITEM_NAME_TBL_ID);
                                if (pBlkDirExt.ParameterTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << PARAM_TBL_ID);
                                if (CodingMajor < DDOD_REV_SUPPORTED_EIGHT)
                                {
                                    if (pBlkDirExt.ParameterMemberTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << PARAM_MEM_TBL_ID);
                                    if (pBlkDirExt.ParameterMemberNameTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << PARAM_MEM_NAME_TBL_ID);
                                    if (pBlkDirExt.ParameterElementTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << PARAM_ELEM_TBL_ID);
                                    if (pBlkDirExt.ParameterListTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << PARAM_LIST_TBL_ID);
                                    if (pBlkDirExt.ParameterListMemberTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << PARAM_LIST_MEM_TBL_ID);
                                    if (pBlkDirExt.ParameterListMemberNameTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << PARAM_LIST_MEM_NAME_TBL_ID);
                                    if (pBlkDirExt.CharectersiticsMemberTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << CHAR_MEM_TBL_ID);
                                    if (pBlkDirExt.CharectersiticsMemberNameTable.uSize > 0)
                                        bin_blk_dir_6.bin_exists |= (1 << CHAR_MEM_NAME_TBL_ID);
                                }// else they ain't there
                                if (pBlkDirExt.RelationTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << REL_TBL_ID);
                                if (pBlkDirExt.UpdateTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << UPDATE_TBL_ID);
                                if (pBlkDirExt.ParameterCommandTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << COMMAND_TBL_ID);
                                if (pBlkDirExt.CriticalParameterTable.uSize > 0)
                                    bin_blk_dir_6.bin_exists |= (1 << CRIT_PARAM_TBL_ID);
                            }

                            /* handle the too long heap reference */
                            if (ObjectFixed[i].wDomainDataSize == 0xffff && pbyObjectValue[i] == null)
                            {
                                uint sizeTotal = 0;

                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.BlockItemTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) 
                                { 
                                    sizeTotal += dwTblLength; 
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.BlockItemNameTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) 
                                {
                                    sizeTotal += dwTblLength;
                                }
                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) { sizeTotal += dwTblLength; }
                                if (CodingMajor < DDOD_REV_SUPPORTED_EIGHT)
                                {
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterMemberTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterMemberNameTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterElementTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterListTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterListMemberTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterListMemberNameTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.CharectersiticsMemberTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                    bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.CharectersiticsMemberNameTable.uSize), FORMAT_BIG_ENDIAN);
                                    if (bRet != false) { sizeTotal += dwTblLength; }
                                }// else the tables are not there
                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.RelationTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) { sizeTotal += dwTblLength; }
                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.UpdateTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) { sizeTotal += dwTblLength; }
                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.ParameterCommandTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) { sizeTotal += dwTblLength; }
                                bRet = Endian.read_dword(ref dwTblLength, (pBlkDirExt.CriticalParameterTable.uSize), FORMAT_BIG_ENDIAN);
                                if (bRet != false) { sizeTotal += dwTblLength; }


                                // seek to heap
                                long offset = ObjectFixed[i].longAddress + header.header_size + header.objects_size;

                                //iRetVal = fseek(fp, (long)offset, 0);
                                iRetVal = (int)fbr.BaseStream.Seek((Int32)offset, SeekOrigin.Begin);
                                if (iRetVal < 0)
                                {
                                    pbyObjectValue[i] = null;
                                }
                                else
                                {// alloc size bytes to pbyObjectValue[i]
                                    pbyObjectValue[i] = new byte[sizeTotal];
                                    // read in size

                                    //iRetVal = fread((byte*)pbyObjectValue[i], 1 , sizeTotal, fp);
                                    fbr.Read(pbyObjectValue[i], 0, (int)sizeTotal);
                                    if (iRetVal != sizeTotal)
                                    {
                                        /*
                                        if (feof(fp))
                                        {
                                            //LOGIT(CERR_LOG, L"End of File reached unexpectedly.\n");
                                        }
                                        else
                                        {
                                            if (ferror(fp))
                                            {
                                                //perror( "File read failure" );	 PAW 09/04/09 see below
                                                //fprintf(stderr, "File read failure");// PAW 09/04/09 see below
                                                //LOGIT(CERR_LOG, "File read failure.\n"); // stevev 12aug10
                                            }
                                            else
                                            {
                                                //delete[] pbyObjectValue;//via DD@F
                                                //CleanArrays();
                                                //LOGIT(CERR_LOG, L" Count mismatch without EOF and without a file error.\n");
                                            }
                                        }
                                        */
                                        return false;
                                    }
                                }
                            }

                            ushort uTag = 0;
                            uint ulTableMaskBit = 0;
                            //binTablePtr = (BININFO*) 0L;
                            uint ulReqMask = BLOCK_TBL_MASKS_HCF;
                            DATAPART_SEGMENT_6 tblExtnOffset;

                            uint lOffset = 0, lSize = 0;
                            uint size, tag;
                            UInt64 LL;

                            if (pbyObjectValue[i] == null)
                            {
                                return false;
                            }

                            while ((ulReqMask > 0) && (uTag < MAX_BLOCK_TBL_ID_HCF))
                            {  /*Check for request mask bit corresponding to the tag value.
					            * Skip to next tag value if not requested.
					            */
                                lOffset = 0;
                                size = 0;

                                if (((ulReqMask) & (1L << uTag)) == 0)
                                {
                                    uTag++;
                                    continue;
                                }
                                /*
                                * Process Tables
                                */

                                switch (uTag++)
                                {
                                    case BLK_ITEM_TBL_ID:
                                        {
                                            ulTableMaskBit = BLK_ITEM_TBL_MASK;
                                            //binTablePtr = &(bin_dev_dir_6.item_tbl); 
                                            tblExtnOffset = (pBlkDirExt.BlockItemTable);

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */

                                            bRet = Endian.read_dword(ref lOffset, tblExtnOffset.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref lSize, tblExtnOffset.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            if (lSize > 0)
                                            {
                                                //int rc; // for parse integer func
                                                //BLK_ITEM_TBL_ELEM* pItmTblElem, *pEndItemTblElem;
                                                unsafe
                                                {
                                                    fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                    {
                                                        byte* chunk = chunk1 + lOffset;
                                                        size = lSize;
                                                        uint cnt = 0;
                                                        UInt64 temp_int;

                                                        //BLK_ITEM_TBL* pFlatItemTbl = &(block_dir_6.blk_item_tbl);

                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &LL);
                                                        cnt = (uint)LL;
                                                        block_dir_6.blk_item_tbl.count = cnt;
                                                        block_dir_6.blk_item_tbl.list = new BLK_ITEM_TBL_ELEM[cnt];

                                                        if (block_dir_6.blk_item_tbl.list == null)
                                                        {
                                                            block_dir_6.blk_item_tbl.count = 0;
                                                            return false;// out-of-memory error
                                                        }
                                                        // clear the table
                                                        //memset((char*)pFlatItemTbl.list, 0, cnt * sizeof(BLK_ITEM_TBL_ELEM));//ok

                                                        // load the list
                                                        //
                                                        //for (pItmTblElem = pFlatItemTbl.list, pEndItemTblElem = pItmTblElem + cnt; pItmTblElem < pEndItemTblElem; pItmTblElem++)
                                                        for ( int j = 0; j < block_dir_6.blk_item_tbl.count; j++)
                                                        {
                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.blk_item_tbl.list[j].blk_item_id = (uint)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.blk_item_tbl.list[j].blk_item_name_tbl_offset = (int)temp_int;
                                                        }
                                                    }
                                                }


                                            }// else no table
                                            else
                                            {
                                                block_dir_6.blk_item_tbl.count = 0;
                                                block_dir_6.blk_item_tbl.list = null;
                                            }
                                        }
                                        break;
                                    case BLK_ITEM_NAME_TBL_ID:
                                        {
                                            ulTableMaskBit = BLK_ITEM_NAME_TBL_MASK;
                                            //binTablePtr = &(bin_dev_dir_6.item_tbl); 
                                            tblExtnOffset = (pBlkDirExt.BlockItemNameTable);

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */

                                            bRet = Endian.read_dword(ref lOffset, tblExtnOffset.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref lSize, tblExtnOffset.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }
                                            if (lSize > 0)
                                            {
                                                unsafe
                                                {
                                                    fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                    { 
                                                        //int rc; // for parse integer func
                                                        //BLK_ITEM_NAME_TBL_ELEM* pItmTblElem;
                                                        byte* chunk = chunk1 + lOffset;
                                                        size = lSize;
                                                        int item = 0;
                                                        UInt64 cnt = 0, temp_int = 0;

                                                        //BLK_ITEM_NAME_TBL* pFlatItemTbl = &(pflatBlkDir.blk_item_name_tbl);

                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                        block_dir_6.blk_item_name_tbl.count = (uint)cnt;
                                                        block_dir_6.blk_item_name_tbl.list = new BLK_ITEM_NAME_TBL_ELEM[cnt];

                                                        if (block_dir_6.blk_item_name_tbl.list == null)
                                                        {
                                                            block_dir_6.blk_item_name_tbl.count = 0;
                                                            return false;// out-of-memory error
                                                        }
                                                        // clear the table
                                                        //memset((char*)pFlatItemTbl.list, 0, (cnt) * sizeof(BLK_ITEM_NAME_TBL_ELEM));//ok
                                                        // load the list
                                                        //
                                                        //pItmTblElem = (block_dir_6.blk_item_name_tbl.list) - 1;//less first increment
                                                        item = -1;                    // ditto
                                                        while (size > 0)
                                                        {// tagged ints...tag,implicit,value - most are optional
                                                            Common.DDL_PARSE_TAG(&chunk, &size, &tag, null);
                                                            switch (tag)
                                                            {
                                                                case BINT_BLK_ITEM_NAME_TAG: // req'd
                                                                    {
                                                                        //pItmTblElem += 1;
                                                                        //item++;

                                                                        // if (item == cnt) return DDL_ENCODING_ERROR;

                                                                        // clear the optional data elements
                                                                        item++;
                                                                        block_dir_6.blk_item_name_tbl.list[item].param_tbl_offset = TABLE_OFFSET_INVALID;
                                                                        block_dir_6.blk_item_name_tbl.list[item].param_list_tbl_offset = TABLE_OFFSET_INVALID;
                                                                        block_dir_6.blk_item_name_tbl.list[item].rel_tbl_offset = TABLE_OFFSET_INVALID;
                                                                        block_dir_6.blk_item_name_tbl.list[item].read_cmd_tbl_offset = TABLE_OFFSET_INVALID;
                                                                        block_dir_6.blk_item_name_tbl.list[item].read_cmd_count = 0;
                                                                        block_dir_6.blk_item_name_tbl.list[item].write_cmd_tbl_offset = TABLE_OFFSET_INVALID;
                                                                        block_dir_6.blk_item_name_tbl.list[item].write_cmd_count = 0;
                                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                        block_dir_6.blk_item_name_tbl.list[item].blk_item_name = (uint)temp_int;

                                                                    }
                                                                    break;
                                                                case BINT_ITEM_TBL_OFFSET_TAG:  // req'd
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].item_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case BINT_PARAM_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].param_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case BINT_PARAM_LIST_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].param_list_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case BINT_REL_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].rel_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case BINT_READ_CMD_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].read_cmd_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case BINT_READ_CMD_TBL_COUNT_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].read_cmd_count = (int)temp_int;
                                                                    break;
                                                                case BINT_WRITE_CMD_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].write_cmd_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case BINT_WRITE_CMD_TBL_COUNT_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.blk_item_name_tbl.list[item].write_cmd_count = (int)temp_int;
                                                                    break;
                                                                default:
                                                                    return false;// DDL_ENCODING_ERROR
                                                                                 //break;
                                                            }// end switch on tag for optional table elements								
                                                        }// wend more size to parse
                                                    }// else no table
                                                }
                                            }
                                            else
                                            {
                                                block_dir_6.blk_item_name_tbl.count = 0;
                                                block_dir_6.blk_item_name_tbl.list = null;
                                            }
                                        }
                                        break;
                                    case PARAM_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_TBL_MASK;
                                            tblExtnOffset = (pBlkDirExt.ParameterTable);
                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */

                                            bRet = Endian.read_dword(ref lOffset, tblExtnOffset.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref lSize, tblExtnOffset.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }
                                            if (lSize != 0)
                                            {
                                                //int rc; // for parse integer func
                                                //PARAM_TBL_ELEM pItmTblElem;
                                                unsafe
                                                {
                                                    fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                    {
                                                        byte* chunk = chunk1 + lOffset;
                                                        size = lSize;
                                                        uint item = 0;
                                                        UInt64 cnt = 0, temp_int = 0;

                                                        //PARAM_TBL* pFlatItemTbl = &(block_dir_6.param_tbl);

                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                        block_dir_6.param_tbl.count = (int)cnt;
                                                        block_dir_6.param_tbl.list = new PARAM_TBL_ELEM[cnt];

                                                        if (block_dir_6.param_tbl.list == null)
                                                        {
                                                            block_dir_6.param_tbl.count = 0;
                                                            return false;// out-of-memory error
                                                        }
                                                        // clear the table
                                                        //memset((char*)pFlatItemTbl.list, 0, (cnt) * sizeof(PARAM_TBL_ELEM));//ok
                                                        // load the list
                                                        //
                                                        //pItmTblElem = (block_dir_6.param_tbl.list) - 1;//less first increment
                                                        //item = -1;                    // ditto
                                                        while (size != 0)
                                                        {// tagged ints...tag,implicit,value - most are optional
                                                            Common.DDL_PARSE_TAG(&chunk, &size, &tag, null);
                                                            switch (tag)
                                                            {
                                                                case PT_BLK_ITEM_NAME_TBL_OFFSET_TAG: // req'd
                                                                    {
                                                                        //pItmTblElem += 1;
                                                                        //item++;
                                                                        // if (item > cnt) return DDL_ENCODING_ERROR;

                                                                        // clear the optional data elements
                                                                        block_dir_6.param_tbl.list[item].blk_item_name_tbl_offset =
                                                                        block_dir_6.param_tbl.list[item].param_mem_tbl_offset =
                                                                        block_dir_6.param_tbl.list[item].param_elem_tbl_offset =
                                                                        block_dir_6.param_tbl.list[item].array_elem_item_tbl_offset = TABLE_OFFSET_INVALID;

                                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                        block_dir_6.param_tbl.list[item].blk_item_name_tbl_offset = (int)temp_int;
                                                                    }
                                                                    break;
                                                                case PT_PARAM_MEM_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].param_mem_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case PT_PARAM_MEM_COUNT_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].param_mem_count = (int)temp_int;
                                                                    break;
                                                                case PT_PARAM_ELEM_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].param_elem_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case PT_PARAM_ELEM_COUNT_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].param_elem_count = (int)temp_int;
                                                                    break;
                                                                case PT_PARAM_ELEM_MAX_COUNT_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].array_elem_count = (int)temp_int;
                                                                    break;
                                                                case PT_ARRAY_ELEM__ITEM_TBL_OFFSET_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].array_elem_item_tbl_offset = (int)temp_int;
                                                                    break;
                                                                case PT_ARRAY_ELEM_TYPE_OR_VAR_TYPE_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].array_elem_type_or_var_type = (int)temp_int;
                                                                    break;
                                                                case PT_ARRAY_ELEM_SIZE_OR_VAR_SIZE_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].array_elem_size_or_var_size = (int)temp_int;
                                                                    break;
                                                                case PT_ARRAY_ELEM_CLASS_VAR_CLASS_TAG:
                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                    block_dir_6.param_tbl.list[item].array_elem_class_or_var_class = (uint)temp_int;
                                                                    break;
                                                                default:
                                                                    return false;// DDL_ENCODING_ERROR
                                                                    //break;
                                                            }// end switch on tag for optional table elements								
                                                        }// wend more size to parse
                                                    }// else no table
                                                }
                                            }
                                            else
                                            {
                                                block_dir_6.param_tbl.count = 0;
                                                block_dir_6.param_tbl.list = null;
                                            }

                                        }
                                        break;
                                    case PARAM_MEM_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_MEM_TBL_MASK;
                                            tblExtnOffset = (pBlkDirExt.ParameterMemberTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case PARAM_MEM_NAME_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_MEM_NAME_TBL_MASK;
                                            //tblExtnOffset = &(pBlkDirExt.ParameterMemberNameTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case PARAM_ELEM_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_ELEM_TBL_MASK;
                                            //tblExtnOffset = &(pBlkDirExt.ParameterElementTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case PARAM_LIST_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_LIST_TBL_MASK;
                                            //tblExtnOffset = &(pBlkDirExt.ParameterListTable);//??????
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case PARAM_LIST_MEM_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_LIST_MEM_TBL_MASK;
                                           //tblExtnOffset = &(pBlkDirExt.ParameterListMemberTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case PARAM_LIST_MEM_NAME_TBL_ID:
                                        {
                                            ulTableMaskBit = PARAM_LIST_MEM_NAME_TBL_MASK;
                                            //tblExtnOffset = &(pBlkDirExt.ParameterListMemberNameTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case CHAR_MEM_TBL_ID:
                                        {
                                            ulTableMaskBit = CHAR_MEM_TBL_MASK;
                                            //tblExtnOffset = &(pBlkDirExt.CharectersiticsMemberTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case CHAR_MEM_NAME_TBL_ID:
                                        {
                                            ulTableMaskBit = CHAR_MEM_NAME_TBL_MASK;
                                            //tblExtnOffset = &(pBlkDirExt.CharectersiticsMemberNameTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                        }
                                        break;
                                    case REL_TBL_ID:
                                        {
                                            ulTableMaskBit = REL_TBL_MASK;
                                            tblExtnOffset = (pBlkDirExt.RelationTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */

                                            bRet = Endian.read_dword(ref lOffset, tblExtnOffset.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            bRet = Endian.read_dword(ref lSize, tblExtnOffset.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            if (lSize != 0)
                                            {
                                                //int rc; // for parse integer func
                                                //REL_TBL_ELEM* pItmTblElem, *pEndItemTblElem;
                                                unsafe
                                                {
                                                    fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                    {
                                                        byte* chunk = chunk1 + lOffset;
                                                        size = lSize;
                                                        //uint item = 0;
                                                        UInt64 cnt = 0, temp_int = 0;

                                                        //REL_TBL* pFlatItemTbl = &(block_dir_6.rel_tbl);

                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                        if (cnt == 0)
                                                        {
                                                            // if cnt is zero, then 
                                                            //   don't allocate the block, since it will not be freed later
                                                            // initialize all varibles from this case, and break out early.
                                                            block_dir_6.rel_tbl.count = 0;
                                                            block_dir_6.rel_tbl.list = null;
                                                            break;// out of switch
                                                        }

                                                        block_dir_6.rel_tbl.count = (int)cnt;
                                                        block_dir_6.rel_tbl.list = new REL_TBL_ELEM[cnt];

                                                        if (block_dir_6.rel_tbl.list == null)
                                                        {
                                                            block_dir_6.rel_tbl.count = 0;
                                                            return false;// out-of-memory error
                                                        }
                                                        // clear the table
                                                        //memset((char*)pFlatItemTbl.list, 0, (cnt) * sizeof(REL_TBL_ELEM));//ok
                                                        // load the list
                                                        //
                                                        // all are required, load 'em in sequence

                                                        //for (pItmTblElem = pFlatItemTbl.list, pEndItemTblElem = pItmTblElem + cnt; pItmTblElem < pEndItemTblElem; pItmTblElem++)
                                                        for (int j = 0; j < block_dir_6.rel_tbl.count; j++)
                                                        {
                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            if (temp_int == (~(uint)0))
                                                                block_dir_6.rel_tbl.list[j].wao_item_tbl_offset = TABLE_OFFSET_INVALID;
                                                            else
                                                                block_dir_6.rel_tbl.list[j].wao_item_tbl_offset = (int)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            if (temp_int == (~(uint)0))
                                                                block_dir_6.rel_tbl.list[j].unit_item_tbl_offset = TABLE_OFFSET_INVALID;
                                                            else
                                                                block_dir_6.rel_tbl.list[j].unit_item_tbl_offset = (int)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            if (temp_int == (~(uint)0))
                                                                block_dir_6.rel_tbl.list[j].update_tbl_offset = TABLE_OFFSET_INVALID;
                                                            else
                                                                block_dir_6.rel_tbl.list[j].update_tbl_offset = (int)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.rel_tbl.list[j].update_count = (int)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.rel_tbl.list[j].unit_count = (int)temp_int;
                                                        }// next element
                                                    }
                                                }
                                            }// else no table
                                            else
                                            {
                                                block_dir_6.rel_tbl.count = 0;
                                                block_dir_6.rel_tbl.list = null;
                                            }
                                        }
                                        break;
                                    case UPDATE_TBL_ID:
                                        {
                                            ulTableMaskBit = UPDATE_TBL_MASK;
                                            tblExtnOffset = (pBlkDirExt.UpdateTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */

                                            bRet = Endian.read_dword(ref lOffset, tblExtnOffset.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }
                                            bRet = Endian.read_dword(ref lSize, tblExtnOffset.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            if (lSize > 0)
                                            {
                                                //int rc; // for parse integer func
                                                //UPDATE_TBL_ELEM* pItmTblElem, *pEndItemTblElem;
                                                unsafe
                                                {
                                                    fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                    {
                                                        byte *chunk = chunk1 + lOffset;
                                                        size = lSize;
                                                        //uint item = 0;
                                                        UInt64 cnt = 0, temp_int = 0;

                                                        //UPDATE_TBL pFlatItemTbl = &(block_dir_6.update_tbl);

                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                        if (cnt == 0)
                                                        {
                                                            // if cnt is zero, then 
                                                            //    don't allocate the block, since it will not be freed later
                                                            // initialize all varibles from this case, and break out early.
                                                            block_dir_6.update_tbl.count = 0;
                                                            block_dir_6.update_tbl.list = null;
                                                            break;// out of switch
                                                        }

                                                        block_dir_6.update_tbl.count = (int)cnt;
                                                        block_dir_6.update_tbl.list = new UPDATE_TBL_ELEM[cnt];

                                                        if (block_dir_6.update_tbl.list == null)
                                                        {
                                                            block_dir_6.update_tbl.count = 0;
                                                            return false;// out-of-memory error
                                                        }
                                                        // clear the table
                                                        // load the list
                                                        //
                                                        // all are required, load 'em in sequence

                                                        for (int j = 0; j < block_dir_6.update_tbl.count; j++)
                                                        {
                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.update_tbl.list[j].desc_it_offset = (int)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.update_tbl.list[j].op_it_offset = (int)temp_int;

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.update_tbl.list[j].op_subindex = (int)temp_int;
                                                        }// next element
                                                    }// else no table
                                                }
                                            }
                                            else
                                            {
                                                block_dir_6.update_tbl.count = 0;
                                                block_dir_6.update_tbl.list = null;
                                            }
                                        }
                                        break;
                                    case COMMAND_TBL_ID:
                                        {
                                            ulTableMaskBit = COMMAND_TBL_MASK;
                                            //tblExtnOffset  = &(pBlkDirExt.ParameterCommandTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */
                                                                         //command_tbl

                                            bRet = Endian.read_dword(ref lOffset, pBlkDirExt.ParameterCommandTable.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }
                                            bRet = Endian.read_dword(ref lSize, pBlkDirExt.ParameterCommandTable.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            if (lSize > 0)
                                            {
                                                if (CodingMajor == DDOD_REV_SUPPORTED_SIX)
                                                {

                                                    //int rc; // for parse integer func
                                                    //COMMAND_TBL_ELEM* pItmTblElem, *pEndItemTblElem;
                                                    //COMMAND_INDEX* pCmdIndex,   *pEndCmdIndex;
                                                    unsafe
                                                    {
                                                        fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                        {
                                                            byte* chunk = chunk1 + lOffset;
                                                            size = lSize;
                                                            //uint item = 0;
                                                            UInt64 cnt = 0;
                                                            UInt64 temp_int = 0;

                                                            //COMMAND_TBL* pFlatItemTbl = &(block_dir_6.command_tbl);

                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                            block_dir_6.command_tbl.count = (int)cnt;
                                                            block_dir_6.command_tbl.list = new COMMAND_TBL_ELEM[cnt];

                                                            if (block_dir_6.command_tbl.list == null)
                                                            {
                                                                block_dir_6.command_tbl.count = 0;
                                                                return false;// out-of-memory error
                                                            }
                                                            // clear the table
                                                            //memset((char*)pFlatItemTbl.list, 0, (cnt) * sizeof(COMMAND_TBL_ELEM));//ok
                                                            // load the list
                                                            //
                                                            // all are required, load 'em in sequence

                                                            for (int j = 0; j < block_dir_6.command_tbl.count; j++)
                                                            {
                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_tbl.list[j].subindex = (ushort)temp_int;

                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_tbl.list[j].number = (uint)temp_int;

                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_tbl.list[j].transaction = (uint)temp_int;

                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_tbl.list[j].weight = (ushort)temp_int;

                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_tbl.list[j].count = (int)temp_int;//??????
                                                                //
                                                                // if there are index elements, parse them
                                                                //
                                                                if (temp_int != 0)
                                                                {   // make the list 
                                                                    block_dir_6.command_tbl.list[j].index_list = new COMMAND_INDEX[(int)temp_int];

                                                                    if (block_dir_6.command_tbl.list[j].index_list == null)
                                                                    {
                                                                        block_dir_6.command_tbl.list[j].count = 0;
                                                                        return false;// out-of-memory error
                                                                    }
                                                                    //
                                                                    // load the list of indexes
                                                                    //
                                                                    for (int k = 0; k < block_dir_6.command_tbl.list[j].count; k++)
                                                                    {
                                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                        block_dir_6.command_tbl.list[j].index_list[k].id = (uint)temp_int;

                                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                        block_dir_6.command_tbl.list[j].index_list[k].value = (uint)temp_int;

                                                                    }// next index
                                                                }
                                                                else
                                                                {

                                                                    block_dir_6.command_tbl.list[j].index_list = null;
                                                                }

                                                            }//for next element
                                                        }//fixed
                                                    }//unsafe
                                                }
                                                else if (CodingMajor == DDOD_REV_SUPPORTED_EIGHT)
                                                {
                                                    //int rc; // for parse integer func
                                                    //COMMAND_TBL_8* pFlatItemTbl = &(block_dir_6.command_to_var_tbl);

                                                    //PTOC_TBL_8_ELEM* pItmTblElem, *pEndItemTblElem;
                                                    //COMMAND_TBL_8_ELEM* pTOCTblElem, *pEndTOCTblElem;
                                                    //COMMAND_INDEX* pCmdIndex,   *pEndCmdIndex;

                                                    unsafe
                                                    {
                                                        fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                        {
                                                            byte* chunk = chunk1 + lOffset;
                                                            size = lSize;
                                                            //uint item = 0;
                                                            UInt64 cnt = 0, temp_int = 0;


                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                            if (cnt == 0)
                                                            {
                                                                // if cnt is zero, then don't allocate the block, 
                                                                //   since it will not be freed later
                                                                // initialize all varibles from this case, and break out early.
                                                                block_dir_6.command_to_var_tbl.count = 0;
                                                                block_dir_6.command_to_var_tbl.list = null;
                                                                if (devID.ulMfgID != 0xf9)// hart is allowed weird DDs
                                                                                          //LOGIT(CERR_LOG, "Error reading PTOC table, entry count is zero.\n");
                                                                {
                                                                    ;
                                                                }
                                                                break;// out of switch
                                                            }
                                                            block_dir_6.command_to_var_tbl.count = (int)cnt;
                                                            block_dir_6.command_to_var_tbl.list = new PTOC_TBL_8_ELEM[cnt];

                                                            if (block_dir_6.command_to_var_tbl.list == null)
                                                            {
                                                                block_dir_6.command_to_var_tbl.count = 0;
                                                                return false;// out-of-memory error
                                                            }
                                                            // clear the table
                                                            //memset((char*)pFlatItemTbl.list, 0, (cnt) * sizeof(PTOC_TBL_8_ELEM));//ok
                                                            // load the list
                                                            //
                                                            // all are required, load 'em in sequence
                                                            //int z = 0;
                                                            for (int j = 0; j < block_dir_6.command_to_var_tbl.count; j++)
                                                            {
                                                                if (size <= 0)
                                                                {
                                                                    //LOGIT(CERR_LOG, "Error: PTOC decode ended with %d size having decoded %d of %d entries.\n", size, z, cnt);
                                                                    return true;// temporary true
                                                                }
                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_to_var_tbl.list[j].item_id = (uint)temp_int;


                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_to_var_tbl.list[j].rd_count = (int)temp_int;

                                                                if (temp_int != 0)
                                                                {// there are read commands							
                                                                    block_dir_6.command_to_var_tbl.list[j].rd_list = new COMMAND_TBL_8_ELEM[temp_int];

                                                                    if (block_dir_6.command_to_var_tbl.list[j].rd_list != null)
                                                                    {
                                                                        UInt64 tmp_int;
                                                                        for (int k = 0; k < block_dir_6.command_to_var_tbl.list[j].rd_count; k++)
                                                                        {
                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].rd_list[k].subindex = (ushort)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].rd_list[k].number = (uint)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].rd_list[k].transaction = (uint)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].rd_list[k].weight = (ushort)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].rd_list[k].count = (int)tmp_int;
                                                                            //
                                                                            // if there are index elements, parse them
                                                                            //
                                                                            if (tmp_int != 0)
                                                                            {   // make the list 
                                                                                UInt64 tmpint;
                                                                                block_dir_6.command_to_var_tbl.list[j].rd_list[k].index_list = new COMMAND_INDEX[tmp_int];

                                                                                if (block_dir_6.command_to_var_tbl.list[j].rd_list[k].index_list == null)
                                                                                {
                                                                                    block_dir_6.command_to_var_tbl.list[j].rd_list[k].count = 0;
                                                                                    return false;// out-of-memory error
                                                                                }
                                                                                //
                                                                                // load the list of indexes
                                                                                //
                                                                                for (int l = 0; l < block_dir_6.command_to_var_tbl.list[j].rd_list[k].count; l++)
                                                                                {
                                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &tmpint);
                                                                                    block_dir_6.command_to_var_tbl.list[j].rd_list[k].index_list[l].id = (uint)tmpint;

                                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &tmpint);
                                                                                    block_dir_6.command_to_var_tbl.list[j].rd_list[k].index_list[l].value = (uint)tmpint;


                                                                                }// next index
                                                                            }
                                                                            else
                                                                            {
                                                                                block_dir_6.command_to_var_tbl.list[j].rd_list[k].index_list = null;
                                                                            }

                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        block_dir_6.command_to_var_tbl.count = 0;
                                                                        return false;// out-of-memory error
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    block_dir_6.command_to_var_tbl.list[j].rd_list = null;
                                                                }

                                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                                block_dir_6.command_to_var_tbl.list[j].wr_count = (int)temp_int;
                                                                if (temp_int != 0)
                                                                {// there are write commands
                                                                    block_dir_6.command_to_var_tbl.list[j].wr_list = new COMMAND_TBL_8_ELEM[temp_int];
                                                                    if (block_dir_6.command_to_var_tbl.list[j].wr_list != null)
                                                                    {
                                                                        UInt64 tmp_int;
                                                                        for (int k = 0; k < block_dir_6.command_to_var_tbl.list[j].wr_count; k++)
                                                                        {
                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].wr_list[k].subindex = (ushort)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].wr_list[k].number = (uint)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].wr_list[k].transaction = (uint)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].wr_list[k].weight = (ushort)tmp_int;

                                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &tmp_int);
                                                                            block_dir_6.command_to_var_tbl.list[j].wr_list[k].count = (int)tmp_int;
                                                                            //
                                                                            // if there are index elements, parse them
                                                                            //
                                                                            if (tmp_int != 0)
                                                                            {   // make the list 
                                                                                UInt64 tmpint;
                                                                                block_dir_6.command_to_var_tbl.list[j].wr_list[k].index_list = new COMMAND_INDEX[tmp_int];

                                                                                if (block_dir_6.command_to_var_tbl.list[j].wr_list[k].index_list == null)
                                                                                {
                                                                                    block_dir_6.command_to_var_tbl.list[j].wr_list[k].count = 0;
                                                                                    return false;// out-of-memory error
                                                                                }
                                                                                //
                                                                                // load the list of indexes
                                                                                //
                                                                                for (int l = 0; l < block_dir_6.command_to_var_tbl.list[j].wr_list[k].count; l++)
                                                                                {
                                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &tmpint);
                                                                                    block_dir_6.command_to_var_tbl.list[j].wr_list[k].index_list[l].id = (uint)tmpint;

                                                                                    Common.DDL_PARSE_INTEGER(&chunk, &size, &tmpint);
                                                                                    block_dir_6.command_to_var_tbl.list[j].wr_list[k].index_list[l].value = (uint)tmpint;



                                                                                }// next index
                                                                            }
                                                                            else
                                                                            {
                                                                                block_dir_6.command_to_var_tbl.list[j].wr_list[k].index_list = null;
                                                                            }

                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        block_dir_6.command_to_var_tbl.count = 0;
                                                                        return false;// out-of-memory error
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    block_dir_6.command_to_var_tbl.list[j].wr_list = null;
                                                                }
                                                            }// next item element
                                                        }
                                                    }
                                                }

                                                else
                                                {// binary file format not supported								
                                                    return false;
                                                }


                                            }// else no table

                                            else
                                            {
                                                block_dir_6.command_tbl.count = 0;
                                                block_dir_6.command_tbl.list = null;
                                            }
                                        }
                                        break;
                                    case CRIT_PARAM_TBL_ID:
                                        {
                                            ulTableMaskBit = CRIT_PARAM_TBL_MASK;
                                            tblExtnOffset = (pBlkDirExt.CriticalParameterTable);
                                            ulReqMask &= ~ulTableMaskBit;/* clear request mask bit */

                                            bRet = Endian.read_dword(ref lOffset, tblExtnOffset.offset, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }
                                            bRet = Endian.read_dword(ref lSize, tblExtnOffset.uSize, FORMAT_BIG_ENDIAN);
                                            if (bRet == false)
                                            {
                                                return false;
                                            }

                                            if (lSize > 0)
                                            {
                                                unsafe
                                                {
                                                    fixed (byte* chunk1 = &pbyObjectValue[i][0])
                                                    {
                                                        byte* chunk = chunk1 + lOffset;
                                                        size = lSize;
                                                        //uint item = 0;
                                                        UInt64 cnt = 0, temp_int = 0;

                                                        //CRIT_PARAM_TBL* pFlatItemTbl = &(block_dir_6.crit_param_tbl);

                                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &cnt);
                                                        if (cnt == 0)
                                                        {
                                                            // if cnt is zero, then don't allocate the block, 
                                                            //   since it will not be freed later
                                                            // initialize all varibles from this case, and break out early.
                                                            block_dir_6.crit_param_tbl.count = 0;
                                                            block_dir_6.crit_param_tbl.list = null;
                                                            break;
                                                        }
                                                        block_dir_6.crit_param_tbl.count = (int)cnt;
                                                        block_dir_6.crit_param_tbl.list = new uint[cnt];
                                                        uint y;  // PAW 03/03/09
                                                        for ( /*int*/ y = 0; y < cnt && size > 0; y++)
                                                        {
                                                            Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);
                                                            block_dir_6.crit_param_tbl.list[y] = (uint)temp_int;
                                                            //CriticalParamList.push_back(temp_int);
                                                        }
                                                        if (size != 0 || y < cnt)
                                                        {
                                                            //LOGIT(CERR_LOG, L"\n eval_crit_table failed!!!! size=%d cnt=%d\n", size, y);
                                                        }
                                                    }
                                                }
                                            }// no size : no table
                                        }
                                        break;
                                    default:
                                        break;// do nothing with it
                                }// endswitch - block directory tables
                            }// wend - next block directory table

                            /* we are only interested in a couple of tables */
                            //		WORD wOffset, wTblLength;
                            //		DATAPART_SEGMENT_6 *tblExtnOffset;

                            //		uint   size;
                            //		unsigned char  *chunk;
                            //		DDL_UINT    	temp_int, numeric;

                            /* critical parameter table */

                            //		tblExtnOffset = &(pBlkDirExt.CriticalParameterTable);

                            //		bRet = read_word(&wOffset,tblExtnOffset.offset,FORMAT_BIG_ENDIAN);
                            //		if(bRet == false)
                            //		{
                            //			delete pBlkDirExt;
                            //			return false;
                            //		}

                            //		bRet = read_word(&wTblLength,tblExtnOffset.uSize,FORMAT_BIG_ENDIAN);
                            //		if(bRet == false)
                            //		{
                            //			delete pBlkDirExt;
                            //			return false;
                            //		}						

                            //		if (wTblLength) 
                            //		{
                            //			int rc; // for parse integer func
                            //			chunk = chunk1 + wOffset;
                            //			size = wTblLength;
                            /* tbl == (encoded_int) count: count instances of (encoded_int)itemID */
                            //			CriticalParamList.clear();
                            /* eval_cpt */

                            //			Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);/* parse count */

                            //			if ( temp_int != 0 )
                            //			{
                            //				for ( int y = 0; y < temp_int && size > 0; y++)
                            //				{
                            //					Common.DDL_PARSE_INTEGER(&chunk, &size, &numeric);/* parse count */
                            //					CriticalParamList.push_back(numeric);
                            //				}
                            //				if ( size != 0 || y < temp_int )
                            //				{								
                            //					cerr<<"\n eval_crit_table failed!!!! size="<< size<<" cnt="<<y<<endl;
                            //				}
                            //			}
                            //		}
                            /* end critical parameter table */
                            /* we'll do the command table some other time */
                            break;
                        }
                    default:
                        /*These cases are handled in GetItems()*/
                        break;

                }/*End switch*/

                /*Just quit this loop once we are done with the loading of Device Directory*/
                /* we're looking for the block directory too...
                        if(bDevDirLoadedFlag == true)
                            break;
                ***/

            }/*End for*/

            //	bDevDirAllocated = true;//Commented by anil
            //bDevDir6Allocated = true;//Bug Fix Anil january 4 2006 -- When Std was fms and device was fm6, simulator was failing
            return true;

        }/*End LoadDeviceDirectory*/

        public bool ResolveItemName()
        {
            //vector<DDlBaseItem*>:: iterator p;
            //DDlBaseItem* pBaseItem;
            string itemName = "";

            foreach (DDlBaseItem dDlBaseItem in ItemsList)
            {
                // was
                //		get_item_name(pBaseItem.id,itemName);
                //		pBaseItem.strItemName = itemName;
                if (dDlBaseItem.strItemName == "" || dDlBaseItem.strItemName == null)
                {
                    Common.get_item_name(dDlBaseItem.id, ref itemName, symFilePath);
                    dDlBaseItem.strItemName = itemName;
                }
            }

            return true;

        }


        public bool LoadDeviceDirectory()
        {
            int iRetVal;
            ushort objectIndex;
            //bool bDevDirLoadedFlag = false;

            for (int i = 0; i < descriptor.sod_length; i++)
            {
                iRetVal = 0;
                objectIndex = ObjectFixed[i].index;

                switch (pbyExtensions[i][1])
                {
                    case DEVICE_DIR_TYPE:
                        {//neither FLAT_DEVICE_DIR nor BIN_DEVICE_DIR have classes; memset ok
                            //(void)memset((char*)&device_dir, 0, sizeof(FLAT_DEVICE_DIR));
                            //(void)memset((char*)&bin_dev_dir, 0, sizeof(BIN_DEVICE_DIR));

                            //FLAT_DEVICE_DIR flatDevDir = device_dir;
                            //BIN_DEVICE_DIR bin_dev_dir_6 = bin_dev_dir;
                            //BININFO[] binTablePtr;
                            //DATAPART_SEGMENT[] dirExtnOffset;

                            /*
                            unsafe
                            {
                                BININFO* binTablePtr = null;
                                DATAPART_SEGMENT* dirExtnOffset = null;
                            }
                            */

                            ushort wOffset = 0;
                            ushort wTblLength = 0;
                            bool bRet;

                            DEVICE_DIR_EXT pDevDirExt;
                            byte[] pbyPointer = pbyExtensions[i];
                            //					pDevDirExt = (DEVICE_DIR_EXT *)(pbyPointer);

                            pDevDirExt = new DEVICE_DIR_EXT();

                            pDevDirExt.byLength = pbyPointer[DEV_DIR_LENGTH_OFFSET];
                            pDevDirExt.byDeviceDirObjectCode = pbyPointer[DEV_DIR_OBJ_CODE_OFFSET];
                            pDevDirExt.byFormatCode = pbyPointer[DEV_DIR_FORMAT_CODE_OFFSET];

                            pDevDirExt.BlockNameTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_NAME_TBL_OFFSET);
                            pDevDirExt.BlockNameTable.wSize = BitConverter.ToUInt16(pbyPointer, BLK_NAME_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.ItemTable.offset = BitConverter.ToUInt16(pbyPointer, ITEM_TBL_OFFSET);
                            pDevDirExt.ItemTable.wSize = BitConverter.ToUInt16(pbyPointer, ITEM_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.ProgramTable.offset = BitConverter.ToUInt16(pbyPointer, PROG_TBL_OFFSET);
                            pDevDirExt.ProgramTable.wSize = BitConverter.ToUInt16(pbyPointer, PROG_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.DomainTable.offset = BitConverter.ToUInt16(pbyPointer, DOM_TBL_OFFSET);
                            pDevDirExt.DomainTable.wSize = BitConverter.ToUInt16(pbyPointer, DOM_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.StringTable.offset = BitConverter.ToUInt16(pbyPointer, STRNG_TBL_OFFSET);
                            pDevDirExt.StringTable.wSize = BitConverter.ToUInt16(pbyPointer, STRNG_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.DictReferenceTable.offset = BitConverter.ToUInt16(pbyPointer, DICT_REF_TBL_OFFSET);
                            pDevDirExt.DictReferenceTable.wSize = BitConverter.ToUInt16(pbyPointer, DICT_REF_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.LocalVariableTable.offset = BitConverter.ToUInt16(pbyPointer, LOC_VAR_TBL_OFFSET);
                            pDevDirExt.LocalVariableTable.wSize = BitConverter.ToUInt16(pbyPointer, LOC_VAR_TBL_OFFSET + SEG_SIZE_OFFSET);
                            pDevDirExt.CommandTable.offset = BitConverter.ToUInt16(pbyPointer, CMD_TBL_OFFSET);
                            pDevDirExt.CommandTable.wSize = BitConverter.ToUInt16(pbyPointer, CMD_TBL_OFFSET + SEG_SIZE_OFFSET);

                            /*Do Some Validations*/
                            if (pDevDirExt.byLength < DEVICE_DIR_LENGTH) //Vibhor 280904: Changed
                            {
                                return false; /* INVALID_EXTN_LENGTH*/
                            }

                            if (pDevDirExt.byDeviceDirObjectCode != DEVICE_DIR_TYPE)
                            {
                                return false; /* DIR_TYPE_MISMATCH*/
                            }

                            if ((bin_dev_dir.bin_exists) == 0)
                            {
                                bin_dev_dir.bin_exists = 0;

                                //if(pDevDirExt.BlockNameTable.wSize)
                                //    	bin_dev_dir_6.bin_exists |= (1<<BLK_TBL_ID);
                                if (pDevDirExt.ItemTable.wSize != 0)
                                    bin_dev_dir.bin_exists |= (1 << ITEM_TBL_ID);
                                //if(pDevDirExt.ProgramTable.wSize)
                                //    	bin_dev_dir_6.bin_exists |= (1<<PROG_TBL_ID);
                                //if(pDevDirExt.DomainTable.wSize)
                                //    	bin_dev_dir_6.bin_exists |= (1<<DOMAIN_TBL_ID);
                                if (pDevDirExt.StringTable.wSize != 0)
                                    bin_dev_dir.bin_exists |= (1 << STRING_TBL_ID);
                                if (pDevDirExt.DictReferenceTable.wSize != 0)
                                    bin_dev_dir.bin_exists |= (1 << DICT_REF_TBL_ID);
                                //if(pDevDirExt.LocalVariableTable.wSize)
                                //    	bin_dev_dir_6.bin_exists |= (1<<LOCAL_VAR_TBL_ID);
                                if (pDevDirExt.CommandTable.wSize != 0)
                                    bin_dev_dir.bin_exists |= (1 << CMD_NUM_ID_TBL_ID);
                            }

                            ushort uTag = 0;
                            uint ulTableMaskBit = 0;
                            uint ulReqMask = DEVICE_TBL_MASKS;

                            while ((ulReqMask != 0) && (uTag < MAX_DEVICE_TBL_ID_HCF))
                            {

                                /*
                                * Check for request mask bit corresponding to the tag value.
                                * Skip to next tag value if not requested.
                                */

                                if (((ulReqMask) & (1L << uTag)) == 0)
                                {
                                    uTag++;
                                    continue;
                                }

                                /*
                                * Point to appropriate values for the table type
                                */

                                switch (uTag++)
                                {

                                    case BLK_TBL_ID:    /* Block Table */
                                                        /*			ulTableMaskBit = BLK_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.BlockNameTable);
                                                                    binTablePtr =&(bin_dev_dir_6.blk_tbl); */
                                        break;

                                    case ITEM_TBL_ID:   /* Item Table */
                                        {
                                            ulTableMaskBit = ITEM_TBL_MASK;
                                            //dirExtnOffset = &(pDevDirExt.ItemTable);
                                            //binTablePtr = bin_dev_dir.item_tbl;

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                            if ((bin_dev_dir.item_tbl.chunk) == null)
                                            {
                                                bRet = Endian.read_word(ref wOffset, pDevDirExt.ItemTable.offset, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }

                                                bRet = Endian.read_word(ref wTblLength, pDevDirExt.ItemTable.wSize, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }
                                                /*
                                                 * Attach the table if non-zero length, else go
                                                 * to the next table
                                                 */
                                                if (wTblLength != 0)
                                                {
                                                    bin_dev_dir.item_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir.item_tbl.uoffset = wOffset;
                                                    bin_dev_dir.item_tbl.size = wTblLength;
                                                    bin_dev_dir.bin_hooked |= ulTableMaskBit;
                                                }
                                            }
                                        }
                                        break;

                                    case PROG_TBL_ID:   /* Program Table */
                                                        /*			ulTableMaskBit = PROG_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.ProgramTable);
                                                                    binTablePtr = &(bin_dev_dir_6.prog_tbl); */
                                        break;

                                    case DOMAIN_TBL_ID: /* Domain Table */
                                                        /*			ulTableMaskBit = DOMAIN_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.DomainTable);
                                                                    binTablePtr = &(bin_dev_dir_6.domain_tbl); */
                                        break;

                                    case STRING_TBL_ID: /* String Table */
                                        {
                                            ulTableMaskBit = STRING_TBL_MASK;
                                            //dirExtnOffset = &(pDevDirExt.StringTable);
                                            //binTablePtr = &(bin_dev_dir_6.string_tbl);

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                            if ((bin_dev_dir.string_tbl.chunk) == null)
                                            {
                                                bRet = Endian.read_word(ref wOffset, pDevDirExt.ItemTable.offset, FORMAT_BIG_ENDIAN);

                                                if (bRet == false)
                                                {
                                                    return false;
                                                }

                                                bRet = Endian.read_word(ref wTblLength, pDevDirExt.ItemTable.wSize, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }

                                                /*
                                                 * Attach the table if non-zero length, else go
                                                 * to the next table
                                                 */

                                                if (wTblLength != 0)
                                                {
                                                    bin_dev_dir.string_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir.string_tbl.uoffset = wOffset;
                                                    bin_dev_dir.string_tbl.size = wTblLength;
                                                    bin_dev_dir.bin_hooked |= ulTableMaskBit;
                                                }
                                            }
                                        }
                                        break;

                                    case DICT_REF_TBL_ID:   /* Dictionary Reference Table */
                                        {
                                            ulTableMaskBit = DICT_REF_TBL_MASK;
                                            //dirExtnOffset = pDevDirExt.DictReferenceTable;
                                            //binTablePtr = &(bin_dev_dir_6.dict_ref_tbl);
                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                            if ((bin_dev_dir.dict_ref_tbl.chunk) == null)
                                            {
                                                bRet = Endian.read_word(ref wOffset, pDevDirExt.DictReferenceTable.offset, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }

                                                bRet = Endian.read_word(ref wTblLength, pDevDirExt.DictReferenceTable.wSize, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }
                                                /*
                                                 * Attach the table if non-zero length, else go
                                                 * to the next table
                                                 */
                                                if (wTblLength != 0)
                                                {
                                                    bin_dev_dir.dict_ref_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir.dict_ref_tbl.uoffset = wOffset;
                                                    bin_dev_dir.dict_ref_tbl.size = wTblLength;
                                                    bin_dev_dir.bin_hooked |= ulTableMaskBit;
                                                }
                                            }
                                        }
                                        break;

                                    case LOCAL_VAR_TBL_ID:  /* Dictionary Reference Table */
                                                            /*		ulTableMaskBit = LOCAL_VAR_TBL_MASK;
                                                                    dirExtnOffset = &(pDevDirExt.LocalVariableTable);
                                                                    binTablePtr = &(bin_dev_dir_6.local_var_tbl); */
                                        break;

                                    case CMD_NUM_ID_TBL_ID: /* Command Number to Item ID Table */
                                        {
                                            ulTableMaskBit = CMD_NUM_ID_TBL_MASK;
                                            //dirExtnOffset = &(pDevDirExt.CommandTable);
                                            //binTablePtr = &(bin_dev_dir_6.cmd_num_id_tbl);

                                            ulReqMask &= ~ulTableMaskBit;   /* clear request mask bit */
                                            if ((bin_dev_dir.cmd_num_id_tbl.chunk) == null)
                                            {
                                                bRet = Endian.read_word(ref wOffset, pDevDirExt.CommandTable.offset, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }

                                                bRet = Endian.read_word(ref wTblLength, pDevDirExt.CommandTable.wSize, FORMAT_BIG_ENDIAN);
                                                if (bRet == false)
                                                {
                                                    return false;
                                                }
                                                /*
                                                 * Attach the table if non-zero length, else go
                                                 * to the next table
                                                 */
                                                if (wTblLength != 0)
                                                {
                                                    bin_dev_dir.cmd_num_id_tbl.chunk = pbyObjectValue[i];
                                                    bin_dev_dir.cmd_num_id_tbl.uoffset = wOffset;
                                                    bin_dev_dir.cmd_num_id_tbl.size = wTblLength;
                                                    bin_dev_dir.bin_hooked |= ulTableMaskBit;
                                                }
                                            }
                                        }
                                        break;

                                    default:    /* goes here for reserved or undefined table IDs */
                                        break;
                                }

                                /*
                                 * Attach the binary for the table if it was requested and if
                                 * it has not already been attached and if it not zero length.
                                 */

                                //				ulReqMask &= ~ulTableMaskBit;	/* clear request mask bit */
                                //				if (!(binTablePtr.chunk)) {

                                //				bRet = read_word(&wOffset,dirExtnOffset.offset,FORMAT_BIG_ENDIAN);

                                //				if(bRet == false)
                                //					return false;

                                //				bRet = read_word(&wTblLength,dirExtnOffset.wSize,FORMAT_BIG_ENDIAN);
                                //				if(bRet == false)
                                //					return false;

                                /*
                                 * Attach the table if non-zero length, else go
                                 * to the next table
                                 */

                                //				if (wTblLength) {
                                //					binTablePtr.chunk = pbyObjectValue[i] + wOffset;
                                //					binTablePtr.size = wTblLength;
                                //					bin_dev_dir_6.bin_hooked |= ulTableMaskBit;
                                //				}


                                //		}
                            }           /* end while */

                            /*We have Fetched the Device dir binary chunks, now Evaluate the device directories*/

                            iRetVal = Eval.eval_dir_device_tables(ref device_dir, ref bin_dev_dir, STRING_TBL_MASK | DICT_REF_TBL_MASK);
                            /*DEVICE_TBL_MASKS );*/
                            if (iRetVal != 0)//
                            {
                                return false;
                            }

                            //bDevDirLoadedFlag = true; /*Device Directory Loaded Successfully*/
                            break;
                        }
                    case BLOCK_DIR_TYPE:
                        {
                            BLOCK_DIR_EXT pBlkDirExt;
                            byte[] pbyPointer = pbyExtensions[i];

                            pBlkDirExt = new BLOCK_DIR_EXT();

                            pBlkDirExt.byLength = pbyPointer[BLK_DIR_LENGTH_OFFSET];
                            pBlkDirExt.byBlockDirObjectCode = pbyPointer[BLK_DIR_OBJ_CODE_OFFSET];
                            pBlkDirExt.byFormatCode = pbyPointer[BLK_DIR_FORMAT_CODE_OFFSET];



                            pBlkDirExt.BlockItemTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_ITEM_TBL_OFFSET);
                            pBlkDirExt.BlockItemTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_ITEM_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.BlockItemNameTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_ITEMNAME_TBL_OFFSET);
                            pBlkDirExt.BlockItemNameTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_ITEMNAME_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAM_TBL_OFFSET);
                            pBlkDirExt.ParameterTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAM_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterMemberTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAMEMBER_TBL_OFFSET);
                            pBlkDirExt.ParameterMemberTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAMEMBER_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterMemberNameTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAMEMBERNAME_TBL_OFFSET);
                            pBlkDirExt.ParameterMemberNameTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAMEMBERNAME_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterElementTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_ELEMENT_TBL_OFFSET);
                            pBlkDirExt.ParameterElementTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_ELEMENT_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterListTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAMLIST_TBL_OFFSET);
                            pBlkDirExt.ParameterListTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAMLIST_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterListMemberTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAMLISTMEMBER_TBL_OFFSET);
                            pBlkDirExt.ParameterListMemberTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAMLISTMEMBER_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterListMemberNameTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAMLISTMEMBERNAME_TBL_OFFSET);
                            pBlkDirExt.ParameterListMemberNameTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAMLISTMEMBERNAME_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.CharectersiticsMemberTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_CHARMEMBER_TBL_OFFSET);
                            pBlkDirExt.CharectersiticsMemberTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_CHARMEMBER_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.CharectersiticsMemberNameTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_CHARMEMBERNAME_TBL_OFFSET);
                            pBlkDirExt.CharectersiticsMemberNameTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_CHARMEMBERNAME_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.RelationTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_RELATION_TBL_OFFSET);
                            pBlkDirExt.RelationTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_RELATION_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.UpdateTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_UPDATE_TBL_OFFSET);
                            pBlkDirExt.UpdateTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_UPDATE_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.ParameterCommandTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_PARAM2COMMAND_TBL_OFFSET);
                            pBlkDirExt.ParameterCommandTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_PARAM2COMMAND_TBL_OFFSET) + SEG_SIZE_OFFSET);
                            pBlkDirExt.CriticalParameterTable.offset = BitConverter.ToUInt16(pbyPointer, BLK_CRITICALPARAM_TBL_OFFSET);
                            pBlkDirExt.CriticalParameterTable.wSize = BitConverter.ToUInt16(pbyPointer, (BLK_CRITICALPARAM_TBL_OFFSET) + SEG_SIZE_OFFSET);

                            /*Do Some Validations*/
                            if (pBlkDirExt.byLength < BLK_DIR_LEN_HCF)
                            {
                                return false; /* INVALID_EXTN_LENGTH*/
                            }

                            if (pBlkDirExt.byBlockDirObjectCode != BLOCK_DIR_TYPE)
                            {
                                return false; /* DIR_TYPE_MISMATCH*/
                            }
                            /* we are only interested in a couple of tables */
                            ushort wOffset = 0, wTblLength = 0;
                            //DATAPART_SEGMENT tblExtnOffset;

                            uint size;
                            bool bRet;
                            UInt64 temp_int, numeric;

                            /* critical parameter table */

                            //tblExtnOffset = &(pBlkDirExt.CriticalParameterTable);

                            bRet = Endian.read_word(ref wOffset, pBlkDirExt.CriticalParameterTable.offset, FORMAT_BIG_ENDIAN);
                            if (bRet == false)
                            {
                                return false;
                            }

                            bRet = Endian.read_word(ref wTblLength, pBlkDirExt.CriticalParameterTable.wSize, FORMAT_BIG_ENDIAN);
                            if (bRet == false)
                            {
                                return false;
                            }

                            if (wTblLength != 0)
                            {
                                //int rc; // for parse integer func
                                size = wTblLength;
                                /* tbl == (encoded_int) count: count instances of (encoded_int)itemID */
                                CriticalParamList.Clear();
                                /* eval_cpt */
                                unsafe 
                                {
                                    fixed (byte* chunk = &pbyObjectValue[i][0])
                                    {
                                        //chunk = &(pbyObjectValue[i][0]) + wOffset;
                                        Common.DDL_PARSE_INTEGER(&chunk, &size, &temp_int);/* parse count */

                                        if (temp_int != 0)
                                        {
                                            int y;// PAW 03/03/09
                                            for ( /*int*/ y = 0; y < (int)temp_int && size > 0; y++)
                                            {
                                                Common.DDL_PARSE_INTEGER(&chunk, &size, &numeric);/* parse count */
                                                CriticalParamList.Add((uint)numeric);
                                            }

                                            if (size != 0 || y < (int)temp_int)
                                            {
                                                //LOGIT(CERR_LOG, L"\n eval_crit_table failed!!!! size=%d cnt=%d\n", size, y);
                                            }
                                        }
                                    }
                                }
                                /* end critical parameter table */
                                /* we'll do the command table some other time */

                            }
                            break;
                        }
                    default:
                        /*These cases are handled in GetItems()*/
                        break;

                }/*End switch*/

                /*Just quit this loop once we are done with the loading of Device Directory*/

                //stevev 10/13/04		
                //		if(bDevDirLoadedFlag == true)
                //			break;

            }/*End for*/

            //bDevDirAllocated = true;
            return true;
        }/*End LoadDeviceDirectory*/

        unsafe public bool GetItems6()
        {
            /*Here we will loop through the SOD Object Extensions*/

            //byte byItemType;
            //byte byItemSubType;
            //uint ulItemID;
            //uint ulItemMask;
            int iRetVal;
            ushort objectIndex;


            for (int i = 0; i < descriptor.sod_length; i++)
            {
                //byItemType = 0;
                //byItemSubType = 0;
                //ulItemID = 0;
                //ulItemMask = 0;
                iRetVal = 0;
                objectIndex = ObjectFixed[i].index;



                if (byExtLengths[i] == 0)
                {
                    //cout << i << " has no data \n";
                    continue;
                }

                if (pbyExtensions[i] == null)
                {
                    //cout << i << " has no data \n";
                    continue;
                }


                switch (pbyExtensions[i][1])
                {
                    case VARIABLE_ITYPE:
                        {
                            DDl6Variable pVar = new DDl6Variable();
                            if (pVar == null)
                                return false;

                            iRetVal = pVar.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                            /*TODO see if wee have to delete pXwhatever here to return memory*/
                            {
                                break; /*Don't quit if an external object was tried as a base object*/
                            }
                            else if (iRetVal != Common.SUCCESS)
                            {
                                return false;
                            }
                            // else all is well, continue

                            /* We will set the masks after evaluating the "type" attribute */

                            iRetVal = pVar.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                ////EVAL_FAILED(Variable, pVar);
                                return false;
                            }

                            /*Push the parsed Item on the list*/
                            foreach (DDlBaseItem iY in ItemsList)
                            {
                                if (iY.id == pVar.id)
                                {
                                    ;// LOGIT(CERR_LOG, L"ERROR: Duplicate item ids 0x%04x\n", pVar.id);
                                }

                            }

                            ItemsList.Add(pVar);
                            pVar.clear_flat();

                            break;
                        }


                    case COMMAND_ITYPE://Command
                        {
                            DDl6Command pCmd = new DDl6Command();
                            if (pCmd == null)
                                return false;

                            iRetVal = pCmd.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue

                            iRetVal = pCmd.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                ////EVAL_FAILED(Command, pCmd);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pCmd);

                            pCmd.clear_flat();

                            break;
                        }
                    case MENU_ITYPE:// Menu
                        {
                            DDl6Menu pMenu = new DDl6Menu();
                            if (pMenu == null)
                                return false;

                            iRetVal = pMenu.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue

                            iRetVal = pMenu.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                ////EVAL_FAILED(Menu, pMenu);
                                //if (isInTokizer)
                                    //printf("\n eval_attrs failed for Menu: 0x%04x\t i = %d\t RetCode = %d\n",
                                                                                    //pMenu.id, i, iRetVal);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pMenu);

                            pMenu.clear_flat();

                            break;
                        }
                    case EDIT_DISP_ITYPE:// Edit Display
                        {
                            DDl6EditDisplay pEditDisp = new DDl6EditDisplay();
                            if (pEditDisp == null)
                                return false;

                            iRetVal = pEditDisp.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pEditDisp.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(EditDisplay, pEditDisp);
                                return false;
                            }


                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pEditDisp);

                            pEditDisp.clear_flat();

                            break;
                        }
                    case METHOD_ITYPE://Method
                        {
                            DDl6Method pMethod = new DDl6Method();
                            if (pMethod == null)
                                return false;

                            iRetVal = pMethod.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pMethod.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Method, pMethod);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pMethod);

                            pMethod.clear_flat();

                            break;
                        }
                    case REFRESH_ITYPE:// Refresh Relation
                        {
                            DDl6Refresh pRefresh = new DDl6Refresh();
                            if (pRefresh == null)
                                return false;

                            iRetVal = pRefresh.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pRefresh.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Refresh, pRefresh);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pRefresh);

                            pRefresh.clear_flat();

                            break;
                        }
                    case UNIT_ITYPE:// Unit Relation
                        {
                            DDl6Unit pUnit = new DDl6Unit();
                            if (pUnit == null)
                                return false;

                            iRetVal = pUnit.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pUnit.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Unit, pUnit);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pUnit);

                            pUnit.clear_flat();

                            break;
                        }
                    case WAO_ITYPE:// WAO Relation
                        {
                            DDl6Wao pWao = new DDl6Wao();
                            if (pWao == null)
                                return false;

                            iRetVal = pWao.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pWao.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Wao, pWao);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pWao);

                            pWao.clear_flat();

                            break;
                        }
                    case ITEM_ARRAY_ITYPE:// Item Array
                        {
                            DDl6ItemArray pItemArray = new DDl6ItemArray();
                            if (pItemArray == null)
                                return false;
                            pItemArray.byItemSubType = pbyExtensions[i][2];// ((ITEM_EXTN)pbyExtensions[i]).bySubType;

                            iRetVal = pItemArray.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pItemArray.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(ItemArray, pItemArray);
                                return false;
                            }

                            /*Push the parsed Item on the list*/
                            int unitid = 0;
                            for (int l = 0; l < ItemsList.Count; l++)
                            {
                                if (ItemsList[l].byItemType == UNIT_ITYPE)
                                {
                                    unitid = l;
                                    break;
                                }
                            }
                            if (unitid != 0)
                            {
                                ItemsList.Insert(unitid, pItemArray);
                            }
                            else
                            {
                                ItemsList.Add(pItemArray);
                            }

                            pItemArray.clear_flat();

                            break;
                        }
                    case COLLECTION_ITYPE:// Collection
                        {
                            DDl6Collection pCollection = new DDl6Collection();
                            if (pCollection == null)
                                return false;
                             
                            pCollection.byItemSubType = pbyExtensions[i][2];//((ITEM_EXTN*)pbyExtensions[i]).bySubType;

                            iRetVal = pCollection.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pCollection.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Collection, pCollection);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            int unitid = 0;
                            for (int l = 0; l < ItemsList.Count; l++)
                            {
                                if (ItemsList[l].byItemType == UNIT_ITYPE)
                                {
                                    unitid = l;
                                    break;
                                }
                            }
                            if (unitid != 0)
                            {
                                ItemsList.Insert(unitid, pCollection);
                            }
                            else
                            {
                                ItemsList.Add(pCollection);
                            }

                            pCollection.clear_flat();

                            break;
                        }
                    case RECORD_ITYPE://Record
                        {
                            DDl6Record pRecord = new DDl6Record();
                            if (pRecord == null)
                                return false;
                            pRecord.byItemSubType = pbyExtensions[i][2];//((ITEM_EXTN*)pbyExtensions[i]).bySubType;

                            iRetVal = pRecord.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pRecord.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Record, pRecord);
                                return false;
                            }

                            /*Vibhor 311003: For implementing the Demunging Solution We will store this
                             as a Collection item*/

                            pRecord.byItemType = COLLECTION_ITYPE;

                            pRecord.strItemName = "Collection";


                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pRecord);

                            pRecord.clear_flat();

                            break;
                        }
                    case ARRAY_ITYPE://Array
                        {
                            DDl6Array pArray = new DDl6Array();
                            if (pArray == null)
                                return false;
                            pArray.byItemSubType = pbyExtensions[i][2];//((ITEM_EXTN*)pbyExtensions[i]).bySubType;

                            iRetVal = pArray.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pArray.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Array, pArray);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pArray);

                            pArray.clear_flat();
                        }
                        break;

                    case VAR_LIST_ITYPE:// Variable List
                                        //#ifdef _PARSER_DEBUG
                        //LOGIT(CERR_LOG, L"\n############################VARRIABLE LIST ITEM!!!#################################\n");
                        //#endif
                        break;
                    case RESERVED_ITYPE1:
                        //LOGIT(CERR_LOG, L"\n###################################RESERVED ITEM 0#################################\n");
                        break;
                    case RESERVED_ITYPE2:
                        //LOGIT(CERR_LOG, L"\n###################################RESERVED ITEM 11################################\n");
                        break;
                    case PROGRAM_ITYPE:
                        //LOGIT(CERR_LOG, L"\n####################################PROGRAM ITEM ##################################\n");
                        break;

                    case RESP_CODES_ITYPE:// Response Code
                                          //#ifdef _PARSER_DEBUG
                        //LOGIT(CERR_LOG, L"\n#################################RESP CODE ITEM!!!#################################\n");
                        //#endif
                        break;

                    case BLOCK_ITYPE://Block
                        {
                            DDl6Block pBlock = new DDl6Block();
                            if (pBlock == null)
                                return false;
                            pBlock.byItemSubType = pbyExtensions[i][2];// ((Common.ITEM_EXTN)pbyExtensions[i]).bySubType;

                            iRetVal = pBlock.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pBlock.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //LOGIT(CERR_LOG, L"\n eval_attrs failed for BLOCK!!!!");
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pBlock);

                            pBlock.clear_flat();
                        }
                        break;

                    case DOMAIN_ITYPE:
                        //LOGIT(CERR_LOG, L"\n#####################################DOMAIN ITEM 0#################################\n");
                        break;
                    case MEMBER_ITYPE:
                        //LOGIT(CERR_LOG, L"\n###################################MEMBER ITEM ####################################\n");
                        break;
                    case FILE_ITYPE:
                        {
                            DDl6File pFile = new DDl6File();
                            if (pFile == null)
                                return false;

                            iRetVal = pFile.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pFile.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(File, pFile);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pFile);

                            pFile.clear_flat();

                        }
                        break;
                    case CHART_ITYPE:
                        {
                            DDl6Chart pChart = new DDl6Chart();
                            if (pChart == null)
                                return false;

                            iRetVal = pChart.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pChart.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Chart, pChart);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pChart);

                            pChart.clear_flat();

                        }
                        break;

                    case GRAPH_ITYPE:
                        {
                            DDl6Graph pGraph = new DDl6Graph();
                            if (pGraph == null)
                                return false;

                            iRetVal = pGraph.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pGraph.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Graph, pGraph);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pGraph);

                            pGraph.clear_flat();

                        }
                        break;

                    case AXIS_ITYPE:
                        {
                            DDl6Axis pAxis = new DDl6Axis();
                            if (pAxis == null)
                                return false;

                            iRetVal = pAxis.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pAxis.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Axis, pAxis);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pAxis);

                            pAxis.clear_flat();

                        }
                        break;

                    case WAVEFORM_ITYPE:
                        {
                            DDl6Waveform pWvForm = new DDl6Waveform();
                            if (pWvForm == null)
                                return false;

                            iRetVal = pWvForm.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pWvForm.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Waveform, pWvForm);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pWvForm);

                            pWvForm.clear_flat();
                        }
                        break;
                    case SOURCE_ITYPE:
                        {
                            DDl6Source pSrc = new DDl6Source();
                            if (pSrc == null)
                                return false;

                            iRetVal = pSrc.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue

                            iRetVal = pSrc.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Source, pSrc);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pSrc);

                            pSrc.clear_flat();
                        }
                        break;

                    case LIST_ITYPE:
                        {
                            DDl6List pLst = new DDl6List();
                            if (pLst == null)
                                return false;

                            iRetVal = pLst.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pLst.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(List, pLst);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pLst);

                            pLst.clear_flat();
                        }
                        break;

                    case GRID_ITYPE:
                        {
                            DDl6Grid pGrd = new DDl6Grid();
                            if (pGrd == null)
                                return false;

                            iRetVal = pGrd.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pGrd.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Grid, pGrd);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pGrd);

                            pGrd.clear_flat();
                        }
                        break;

                    case IMAGE_ITYPE:
                        {
                            DDl6Image pImg = new DDl6Image();
                            if (pImg == null)
                                return false;

                            iRetVal = pImg.fetch_item(ref pbyExtensions[i], ObjectFixed[i].index);
                            if (iRetVal == Common.FETCH_EXTERNAL_OBJECT)
                                break; /*Don't quit if an external object was tried as a base object*/
                            else
                            if (iRetVal != Common.SUCCESS)
                                return false;
                            // else all is well, continue


                            iRetVal = pImg.eval_attrs();
                            if (iRetVal != Common.SUCCESS)
                            {
                                //EVAL_FAILED(Image, pImg);
                                return false;
                            }

                            /*Push the parsed Item on the list*/

                            ItemsList.Add(pImg);

                            pImg.clear_flat();
                        }
                        break;

                    default:

                        //					printf("Error : Invalid Object Type  : Type Code = % d\n",pbyExtensions[i][1]);
                        //#ifdef _PARSER_DEBUG
                        if ((pbyExtensions[i][1] == MEMBER_ITYPE) ||
                            (pbyExtensions[i][1] == VAR_LIST_ITYPE) ||
                            (pbyExtensions[i][1] == RESP_CODES_ITYPE))
                        {
                            ;//LOGIT(CERR_LOG, L"Error : Invalid Object Type  : Type Code = %d\n", pbyExtensions[i][1]);
                        }

                        break;

                }/*End switch (pbyExtensions[i][1])*/

            }/*End for*/

            return true;
        }

        public bool ReadFormatObject()
        {
            FORMAT_EXTENSION FmtExt;
            bool bFmtDone = false;
            for (int i = 0; i < descriptor.sod_length; i++)
            {
                switch (pbyExtensions[i][1])
                {
                    case FORMAT_OBJECT_TYPE:
                        {
                            byte[] pbyPointer = pbyExtensions[i];

                            FmtExt.byLength = pbyPointer[FMT_EXTN_LENGTH_OFFSET];
                            FmtExt.byFormatObjectCode = pbyPointer[FMT_OBJ_CODE_OFFSET];
                            FmtExt.byCodingFormatMajor = pbyPointer[CODING_FMT_MAJ_OFFSET];
                            FmtExt.byCodingFormatMinor = pbyPointer[CODING_FMT_MIN_OFFSET];
                            FmtExt.byDDRevision = pbyPointer[DDREV_OFFSET];
                            FmtExt.pchProfileNumber1 = (char)pbyPointer[PROFILE_NO_OFFSET];
                            FmtExt.pchProfileNumber2 = (char)pbyPointer[PROFILE_NO_OFFSET + 1];
                            FmtExt.wNumberOfImports = BitConverter.ToUInt16(pbyPointer, NO_O_IMPORTS_OFFSET);
                            FmtExt.wNumberOfLikes = BitConverter.ToUInt16(pbyPointer, NO_O_LIKES_OFFSET);

                            if (FmtExt.byFormatObjectCode == 128)
                            {
                                CodingMajor = FmtExt.byCodingFormatMajor;
                                CodingMinor = (byte)(FmtExt.byCodingFormatMinor & 0x7f);//DDOD_REV_MINOR_HCF | (HCF_TOK_TYPE << CODING_FMT_MINOR_SIZE * 7)
                                TokenizerType = (byte)(FmtExt.byCodingFormatMinor >> 7);
                                DDRevision = FmtExt.byDDRevision;
                                if (false == Endian.read_word(ref ImpCnt, FmtExt.wNumberOfImports, FORMAT_BIG_ENDIAN))
                                {
                                    return false;
                                }
                                if (false == Endian.read_word(ref LikCnt, FmtExt.wNumberOfLikes, FORMAT_BIG_ENDIAN))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                            bFmtDone = true;
                        }/*End Case*/
                        break;
                }/*End Switch*/
                if (bFmtDone)
                    break; //Break from the loop if we got the format object

            }/*Endfor*/
            return true;
        }/*End of ReadFormatObject()*/

        public uint GetSize(int i)
        {
            uint size = ObjectFixed[i].wDomainDataSize;

            if (size < 0xffff)
                return size;

            size = 0;

            byte[][] pLocExtn = pbyExtensions;
            int rSize = 0;
            UInt32 attrMask = 0;

            Common._preFetchItem(4 /* mask size always 4 */, pLocExtn, ref rSize, ref attrMask, i);


            byte[][] obj_attr_ptr = pLocExtn;         /* pointer to attributes in object extension */
            byte extn_attr_length = (byte)rSize; /*length of Extn data in external obj*/
            UInt32 local_req_mask = attrMask;            /* request mask for base or external* objects */


            //while ((obj_attr_ptr < (pLocExtn + extn_attr_length)) && local_req_mask)
            //{
            //    ushort curr_attr_RI;    /* RI for current attribute */
            //    ushort curr_attr_tag;   /* tag for current attribute */
            //    unsigned Int32 curr_attr_length; /* data length for current attribute */

            //    parse_attribute_id(&obj_attr_ptr, &curr_attr_RI, &curr_attr_tag, &curr_attr_length);

            //    unsigned Int32 attr_mask_bit = (unsigned Int32) (1L << curr_attr_tag);

            //    local_req_mask &= ~attr_mask_bit;   // clear the bit in local mask

            //    // increment ptr
            //    if (curr_attr_RI == RI_LOCAL)
            //    {
            //        size += curr_attr_length;
            //        // consume the trailing offset integer
            //        unsigned Int32 attr_offset = 0L;
            //        do
            //        {
            //            if (attr_offset & MAX_LENGTH_MASK)
            //            {
            //                return (FETCH_ATTR_LENGTH_OVERFLOW);
            //            }
            //            attr_offset = (attr_offset << LENGTH_SHIFT) |
            //                (unsigned Int32) (LENGTH_MASK & *obj_attr_ptr);
            //        } while (LENGTH_ENCODE_MASK & *obj_attr_ptr++);
            //    }
            //    else
            //    if (curr_attr_RI == RI_IMMEDIATE)
            //    {// increment past the trailing attribute
            //        obj_attr_ptr += curr_attr_length;
            //    }
            //    else
            //    {// unknown RI type - we are out of sync
            //        LOGIT(CERR_LOG, "ERROR: reading DD. GetSize() read an unknown RI type.\n");
            //        return size;
            //    }

            //}

            //	if (error)
            //		return 0;
            //	else
            return size;


        }
        public bool ReadObjectValues()
        {
            int iRetVal;
            for (int i = 0; i < descriptor.sod_length; i++)
            {
                if ((ObjectFixed[i].wDomainDataSize) == 0)
                {
                    pbyObjectValue[i] = null;
                    continue;
                }
                if (ObjectFixed[i].longAddress == 0xffffffff)
                {
                    pbyObjectValue[i] = null;
                    continue;
                }
                if (byExtLengths[i] == 0)
                {
                    pbyObjectValue[i] = null;
                    continue;
                }
                Int32 offset = (Int32)(ObjectFixed[i].longAddress + header.header_size + header.objects_size);

                iRetVal = (int)fbr.BaseStream.Seek((Int32)offset, SeekOrigin.Begin);
                if (iRetVal < 0)
                {
                    pbyObjectValue[i] = null;
                    continue;
                }
                /*Vibhor 010904: Start of Code*/


                if (ObjectFixed[i].wDomainDataSize >= 0xffff && i < descriptor.sod_first_index)
                {
                    /*It should not come here for any object other than Device & Block Directory*/
                    pbyObjectValue[i] = null;

                    // find the correct size

                    if (pbyExtensions[i][1] == DEVICE_DIR_TYPE)
                    {
                        ;// messagebox cout << "Device Directory has max-bytes(wDomainDataSize) > 0xffff" << endl;
                    }
                    else if (pbyExtensions[i][1] == BLOCK_DIR_TYPE)
                    {
                        ;//cout << "Block Directory has max-bytes(wDomainDataSize) > 0xffff" << endl;
                    }
                    else
                    {
                        ;//cerr << "Object" << i + 1 << " has max-bytes(wDomainDataSize) > 0xffff !!" << endl;
                    }

                }
                else
                {
                    //size = GetSize(ObjectFixed[i].wDomainDataSize);	
                    int size = (int)GetSize(i);

                    pbyObjectValue[i] = new byte[size];
                    /* stevev 15apr11 - use the calculated size for all
                                iRetVal = fread((byte *)pbyObjectValue[i], 1
                                                        , ObjectFixed[i].wDomainDataSize, fp);
                                if (iRetVal != ObjectFixed[i].wDomainDataSize)
                    **/
                    iRetVal = fbr.Read(pbyObjectValue[i], 0, size);
                    if (iRetVal != size)
                    {
                        /*
                        if (feof(fp))
                        {
                            LOGIT(CERR_LOG, L"End of File reached unexpectedly.\n");
                        }
                        else
                        {
                            if (ferror(fp))
                            {
                                //perror( "File read failure" );	 PAW 09/04/09 see below
                                //fprintf(stderr, "File read failure");// PAW 09/04/09 see below
                                LOGIT(CERR_LOG, "File read failure.\n"); // stevev 12aug10
                            }
                            else
                            {
                                LOGIT(CERR_LOG, L" Count mismatch without EOF and without a file error.\n");
                            }
                        }*/
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateFixed(ref DOMAIN_FIXED pDomainFixed)
        {

            ushort index = 0;
            Endian.read_word(ref index, pDomainFixed.index, FORMAT_BIG_ENDIAN);
            pDomainFixed.index = index;

            ushort wDomainDataSize = 0;
            Endian.read_word(ref wDomainDataSize, pDomainFixed.wDomainDataSize, FORMAT_BIG_ENDIAN);
            pDomainFixed.wDomainDataSize = wDomainDataSize;

            uint longAddress = 0;
            Endian.read_dword(ref longAddress, pDomainFixed.longAddress, FORMAT_BIG_ENDIAN);
            pDomainFixed.longAddress = longAddress;

            return true;
        }

        public bool ReadSOD()
        {
            int iRetVal;
            byte[] byTmpBuf = new byte[DOMAIN_FIXED_SIZE];
            for (int i = 0; i < descriptor.sod_length; i++)
            {
                //ushort uVal = 0;
                //byte byLen = 0;
                iRetVal = fbr.Read(byTmpBuf, 0, DOMAIN_FIXED_SIZE);
                if (iRetVal != DOMAIN_FIXED_SIZE)
                {
                    return false;
                }

                ObjectFixed[i].index = BitConverter.ToUInt16(byTmpBuf, DOM_FIXED_INDX_OFFSET);
                ObjectFixed[i].byObjectCode = byTmpBuf[OBJ_CODE_OFFSET];
                ObjectFixed[i].wDomainDataSize = BitConverter.ToUInt16(byTmpBuf, DOM_DATA_SIZE_OFFSET);
                ObjectFixed[i].longAddress = BitConverter.ToUInt32(byTmpBuf, LONG_ADDR_OFFSET);
                ObjectFixed[i].byDomainState = byTmpBuf[DOM_STATE_OFFSET];
                ObjectFixed[i].byUploadState = byTmpBuf[UPLOAD_STATE_OFFSET];
                ObjectFixed[i].chCounter = (char)byTmpBuf[COUNTER_OFFSET];

                ValidateFixed(ref ObjectFixed[i]);


                /*Vibhor 310804: Start of Code*/
                /*
                        iRetVal = fread((byte *)&byExtLengths[i], 1
                                                    , sizeof(byte), fp);
                    In new Binary File format, the Extension Length is a 
                    multibyte INTEGER instead of unsigned8, so commenting 
                    out this one. Unsigned8 would still be parsed as a spl
                    case of new format.
                */
                //Read the integer and its length !!
                byte[] byread = new byte[1];
                iRetVal = fbr.Read(byread, 0, 1);
                byExtLengths[i] = byread[0];
                //		iRetVal = ReadIntegerValueFromFile(fp,uVal,byLen); 

                /*	if(DDL_SUCCESS != iRetVal)
                    {
                        cerr <<"Reading a (multibyte) integer from file failed !!!" << endl;
                        return false;
                    }
                */
                //		byExtLengths[i] = uVal;
                Int32 lSeekLcn = -1;//-byLen;
                if (byExtLengths[i] > 0)
                {
                    pbyExtensions[i] = new byte[byExtLengths[i] + 1];
                    iRetVal = (int)fbr.BaseStream.Seek(lSeekLcn, SeekOrigin.Current);// seek(fp, lSeekLcn, SEEK_CUR);
                    iRetVal = fbr.Read(pbyExtensions[i], 0, byExtLengths[i] + 1);

                }//Vibhor 310804: End of Code (change)
                else
                {

                    //cout << "Object " << i << "has no data \n";
                }
            }
            return true;
        }

        public returncode Initialize(string pchFileName, string pchLanguageCode, HartDictionary pDict/*, LitStringTable* pLit*/)
        {
            returncode bRetVal;
            //	char *tmpDictFilePath;
            //byte filelen;

            if ("" == pchFileName)
                return returncode.eFileErr;

            if (symFilePath != "")  //// HOMZ - Fix memory leak;  Release resources before allocating new ones...
            {
                symFilePath = "";
            }

            symFilePath = pchFileName.Substring(0, pchFileName.Length - 4) + ".sym";

            if ("" == pchLanguageCode)
            {
                langCode = DEFAULT_LANGUAGE_CODE;
            }
            else
            {
                langCode = pchLanguageCode;
            }

            /*Even before opening the fms file . load the standard dictionary ,
            if that itself fails there's no point in going forward, just log an error & return false*/

            if ((null == pDict) && (false == bGlobalDictAllocated) && (null == pGlobalDict)) //Vibhor 120504: Modified the condition	
            {
                bRetVal = LoadDictionary(langCode);     // Two allocated flags are set

                if (bRetVal != returncode.eOk)
                {
                    //cerr<<"\n Standard dictionary loading failed!!!!!\n"<<endl;
                    //LOGIT(CERR_LOG | UI_LOG, "\n Standard dictionary failed to load.\n");   // two allocated flags are not set
                    return returncode.eDDInitErr;
                }
            }
            else    // pDict != null || bGlobalDictAllocated == true || pGlobDict != null
            {
                //if (null == pGlobalDict)    // this means that pDict != null (or bGlobalDictAllocated == true incorrectly)
                {
                    pGlobalDict = pDict;
                    //bDictAllocated = false;
                    bGlobalDictAllocated = true;    // J.U.  in order to null pGlobal Dict 
                }

                // if(null == pLitStringTable)
                //pLitStringTable = pLit;// this is not ours
            }

            fbr = new BinaryReader(new FileStream(pchFileName, FileMode.Open, FileAccess.Read));

            if (fbr == null)
            {
                //		printf("Error opening DD file\n");
                //LOGIT(CLOG_LOG, "- Could not open DD file '%s'\n", pchFileName);/*sjv 26jan05 - caller logs this*/
                return returncode.eFileErr;
            }
            return returncode.eOk;
        }

        public unsafe bool GetImages6()
        {//AimageList_t ImagesList;
            if (device_dir_6.image_tbl.count <= 0)
            {
                return true;
            }// else do the work

            if (ImagesList.Count > 0)
            {
                // clear the list
            }

            IMAGEFRAME_S imgFrm = new IMAGEFRAME_S();
            List<IMAGEFRAME_S> imageL = new List<IMAGEFRAME_S>();

            //IMAGE_TBL_ELEM pITE = device_dir_6.image_tbl.list[0];// [0]
            //IMG_ITEM pII;

            for (ushort x = 0; x < device_dir_6.image_tbl.count; x++)
            {
                //pII = device_dir_6.image_tbl.list[i];
                if (device_dir_6.image_tbl.list[x].num_langs <= 0)
                {
                    //LOGIT(CERR_LOG, L"ERROR: Image with no languages.\n");
                    return false;
                }
                imageL = new List<IMAGEFRAME_S>();
                for (ushort y = 0; y < device_dir_6.image_tbl.list[x].num_langs; y++)
                {
                    //memcpy((void*)&(imgFrm.ifs_language[0]), (void*)&(pII.lang_code), CNTRYCDSTRLEN);
                    imgFrm.ifs_language = Convert.ToString(device_dir_6.image_tbl.list[x].img_list[y].lang_code);
                    imgFrm.ifs_size = device_dir_6.image_tbl.list[x].img_list[y].img_file.uSize;
                    imgFrm.ifs_pRawFrame = device_dir_6.image_tbl.list[x].img_list[y].p2Graphik;
                    imgFrm.ifs_offset = device_dir_6.image_tbl.list[x].img_list[y].img_file.offset;

                    imageL.Add(imgFrm);
                }// next lang/frame
                if (imageL.Count > 0)
                {
                    ImagesList.Add(imageL);
                    //imageL.Clear();
                }
                // stevev 28sep11 - delete the table memory now that we got what we needed
                //pITE.img_list = NULL;
            }// next image
             // stevev 28sep11 - delete the table memory now that we got what we needed
            device_dir_6.image_tbl.count = 0;
            return true;
        }

        //public string GetNameByID(uint uID)
        //{
        //    string rt = "";

        //    foreach (DDlBaseItem ddb in ItemsList)
        //    {
        //        if (ddb.id == uID)
        //        {
        //            switch(ddb.byItemType)
        //            {
        //                case DDlBaseItem.VARIABLE_ITYPE:
        //                    break;

        //                case DDlBaseItem.COMMAND_ITYPE:
        //                    break;

        //                case DDlBaseItem.MENU_ITYPE:

        //                    foreach(DDlAttribute ddattr in ddb.attrList)
        //                    {
        //                        if (ddattr.byAttrID == DDl6Menu.MENU_LABEL_ID)
        //                        {
        //                            rt = ddattr.pVals.strVal.str;//
        //                        }

        //                    }

        //                    break;

        //                case DDlBaseItem.EDIT_DISP_ITYPE:
        //                    break;

        //                case DDlBaseItem.METHOD_ITYPE:
        //                    break;

        //                case DDlBaseItem.REFRESH_ITYPE:
        //                    break;

        //                case DDlBaseItem.UNIT_ITYPE:
        //                    break;

        //                case DDlBaseItem.WAO_ITYPE:
        //                    break;

        //                case DDlBaseItem.ITEM_ARRAY_ITYPE:
        //                    break;

        //                case DDlBaseItem.COLLECTION_ITYPE:
        //                    break;

        //                case DDlBaseItem.BLOCK_ITYPE:
        //                    break;

        //                case DDlBaseItem.PROGRAM_ITYPE:
        //                    break;

        //                case DDlBaseItem.RECORD_ITYPE:
        //                    break;

        //                case DDlBaseItem.ARRAY_ITYPE:
        //                    break;

        //                case DDlBaseItem.VAR_LIST_ITYPE:
        //                    break;

        //                case DDlBaseItem.RESP_CODES_ITYPE:
        //                    break;

        //                case DDlBaseItem.DOMAIN_ITYPE:
        //                    break;

        //                case DDlBaseItem.MEMBER_ITYPE:
        //                    break;

        //                case DDlBaseItem.FILE_ITYPE:
        //                    break;

        //                case DDlBaseItem.CHART_ITYPE:
        //                    break;

        //                case DDlBaseItem.GRAPH_ITYPE:
        //                    break;

        //                case DDlBaseItem.AXIS_ITYPE:
        //                    break;

        //                case DDlBaseItem.WAVEFORM_ITYPE:
        //                    break;

        //                case DDlBaseItem.SOURCE_ITYPE:
        //                    break;

        //                case DDlBaseItem.LIST_ITYPE:
        //                    break;

        //                case DDlBaseItem.GRID_ITYPE:
        //                    break;

        //                case DDlBaseItem.IMAGE_ITYPE:
        //                    break;

        //                case DDlBaseItem.BLOB_ITYPE:
        //                    break;

        //                default:
        //                    break;
        //            }
        //            break;
        //        }
        //    }

        //    return rt;
        //}

        /*
        private bool ValidateHeader(ref DDOD_HEADER header)
        {
            return true;
        }
        */
        //        private bool GetItems()
        //        {
        //            /*Here we will loop through the SOD Object Extensions*/

        //            //byte byItemType;
        //            //byte byItemSubType;
        //            //ushort ulItemID;
        //            //ushort ulItemMask;
        //            int iRetVal;
        //            ushort objectIndex;

        //            // stevev moved to a higher level
        //            //	/*Load the device directory*/
        //            //
        //            //	iRetVal = LoadDeviceDirectory();
        //            //	if(iRetVal != true) /*Quit if it fails!!*/
        //            //		return false;

        //            /*Since we need the HART Block for some ref resolutions we'll load it
        //             first, out of turn!!!!*/

        //            //iRetVal = LoadBlockItem();
        //            bool bR = LoadBlockItem();
        //            if (bR != true) /*Quit if it fails!!*/
        //                return false;



        //            for (int i = 0; i < descriptor.sod_length; i++)
        //            {
        //                iRetVal = 0;
        //                objectIndex = ObjectFixed[i].index;



        //                if (byExtLengths[i] == 0)
        //                {
        //                    //			printf("%d has no data \n", i);
        //                    //#ifdef _PARSER_DEBUG
        //                    cout << i << " has no data \n";
        //                    //#endif
        //                    continue;
        //                }

        //                if (pbyExtensions[i] == NULL)
        //                {
        //                    //			printf("%d has no data \n", i);
        //                    //#ifdef _PARSER_DEBUG

        //                    cout << i << " has no data \n";
        //                    //#endif
        //                    continue;
        //                }


        //                switch (pbyExtensions[i][1])
        //                {
        //                    case VARIABLE_ITYPE:
        //                        {
        //                            //					memset((char *)fv, 0, sizeof(FLAT_VAR));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Variable Item*******************************\n");

        //                            //					printf("%d Variable Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Variable Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, VARIABLE_ITYPE,
        //                                                                            &ulItemMask, (void*)fv);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlVariable* pVar = new DDlVariable();

        //                            pVar->id = fv->id;
        //                            pVar->byItemType = VARIABLE_ITYPE;

        //                            pVar->strItemName = "Variable";

        //                            /* We will set the masks after evaluating the "type" attribute */

        //                            iRetVal = eval_variable(fv, ulItemMask, pVar);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(variable, pVar);
        //                                //getchar();
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            /*Push the parsed variable on the ItemList*/

        //                            ItemsList.push_back(pVar);

        //                            /*		dump_var(fout,fv);

        //                            if(!(fv->class_attr))
        //                            {
        //                                iNumberOfNoClassVars++;
        //                                myprintf(ferr,"%ul\n",fv->id);
        //                            } */

        //                            if (fv->depbin)
        //                            {
        //                                if (fv->depbin->db_class)
        //                                    delete(fv->depbin->db_class);
        //                                if (fv->depbin->db_display)
        //                                    delete(fv->depbin->db_display);
        //                                if (fv->depbin->db_edit)
        //                                    delete(fv->depbin->db_edit);
        //                                if (fv->depbin->db_enums)
        //                                    delete(fv->depbin->db_enums);
        //                                if (fv->depbin->db_handling)
        //                                    delete(fv->depbin->db_handling);
        //                                if (fv->depbin->db_help)
        //                                    delete(fv->depbin->db_help);
        //                                if (fv->depbin->db_index_item_array)
        //                                    delete(fv->depbin->db_index_item_array);
        //                                if (fv->depbin->db_label)
        //                                    delete(fv->depbin->db_label);
        //                                if (fv->depbin->db_resp_codes)
        //                                    delete(fv->depbin->db_resp_codes);
        //                                if (fv->depbin->db_type_size)
        //                                    delete(fv->depbin->db_type_size);
        //                                delete(fv->depbin);

        //                            }/*Endif fv->depbin*/

        //                            if (fv->misc)
        //                            {
        //                                if (fv->misc->depbin)
        //                                {
        //                                    if (fv->misc->depbin->db_max_val)
        //                                        delete(fv->misc->depbin->db_max_val);
        //                                    if (fv->misc->depbin->db_min_val)
        //                                        delete(fv->misc->depbin->db_min_val);
        //                                    //if(fv->misc->depbin->db_read_time_out)
        //                                    //	delete (fv->misc->depbin->db_read_time_out);
        //                                    //if(fv->misc->depbin->db_write_time_out)
        //                                    //	delete (fv->misc->depbin->db_write_time_out);
        //                                    if (fv->misc->depbin->db_height)
        //                                        delete(fv->misc->depbin->db_height);
        //                                    if (fv->misc->depbin->db_width)
        //                                        delete(fv->misc->depbin->db_width);
        //                                    if (fv->misc->depbin->db_scale)
        //                                        delete(fv->misc->depbin->db_scale);
        //                                    if (fv->misc->depbin->db_unit)
        //                                        delete(fv->misc->depbin->db_unit);
        //                                    if (fv->misc->depbin->db_valid)
        //                                        delete(fv->misc->depbin->db_valid);
        //                                    delete(fv->misc->depbin);
        //                                }
        //                                delete(fv->misc);
        //                            }
        //                            if (fv->actions)
        //                            {
        //                                if (fv->actions->depbin)
        //                                {
        //                                    if (fv->actions->depbin->db_post_edit_act)
        //                                        delete(fv->actions->depbin->db_post_edit_act);
        //                                    if (fv->actions->depbin->db_post_read_act)
        //                                        delete(fv->actions->depbin->db_post_read_act);
        //                                    if (fv->actions->depbin->db_post_write_act)
        //                                        delete(fv->actions->depbin->db_post_write_act);
        //                                    if (fv->actions->depbin->db_pre_edit_act)
        //                                        delete(fv->actions->depbin->db_pre_edit_act);
        //                                    if (fv->actions->depbin->db_pre_read_act)
        //                                        delete(fv->actions->depbin->db_pre_read_act);
        //                                    if (fv->actions->depbin->db_pre_write_act)
        //                                        delete(fv->actions->depbin->db_pre_write_act);
        //                                    if (fv->actions->depbin->db_refresh_act)
        //                                        delete(fv->actions->depbin->db_refresh_act);
        //# ifdef XMTR
        //                                    if (fv->actions->depbin->db_post_rqst_act)
        //                                        delete(fv->actions->depbin->db_post_rqst_act);
        //                                    if (fv->actions->depbin->db_post_user_act)
        //                                        delete(fv->actions->depbin->db_post_user_act);
        //#endif
        //                                    delete(fv->actions->depbin);
        //                                }
        //                                delete(fv->actions);
        //                            }

        //                            memset((char*)fv, 0, sizeof(FLAT_VAR));

        //                            break;
        //                        }


        //                    case COMMAND_ITYPE://Command
        //                        {
        //                            //					memset((char *)fcmd, 0, sizeof(FLAT_COMMAND));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Command Item*******************************\n");
        //                            //					printf("%d Command Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Command Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, COMMAND_ITYPE,
        //                                                                        &ulItemMask, (void*)fcmd);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }
        //                            //					iRetVal = eval_item(fcmd, ulItemMask, &errors,COMMAND_ITYPE);

        //                            DDlCommand* pCmd = new DDlCommand();

        //                            pCmd->id = fcmd->id;

        //                            pCmd->byItemType = COMMAND_ITYPE;

        //                            pCmd->strItemName = "Command";

        //                            iRetVal = eval_command(fcmd, ulItemMask, pCmd);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(command, pCmd);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            /*Push the parsed Command on the ItemList*/

        //                            ItemsList.push_back(pCmd);

        //                            if (fcmd->depbin)
        //                            {
        //                                if (fcmd->depbin->db_number)
        //                                    delete(fcmd->depbin->db_number);
        //                                if (fcmd->depbin->db_oper)
        //                                    delete(fcmd->depbin->db_oper);
        //                                if (fcmd->depbin->db_resp_codes)
        //                                    delete(fcmd->depbin->db_resp_codes);
        //                                if (fcmd->depbin->db_trans)
        //                                    delete(fcmd->depbin->db_trans);

        //                                delete(fcmd->depbin);
        //                            }

        //                            memset((char*)fcmd, 0, sizeof(FLAT_COMMAND));

        //                            break;
        //                        }
        //                    case MENU_ITYPE:// Menu
        //                        {
        //                            //					memset((char *)fmenu, 0, sizeof(FLAT_MENU));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Menu Item*******************************\n");
        //                            //					printf("%d Menu Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Menu Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, MENU_ITYPE,
        //                                                                        &ulItemMask, (void*)fmenu);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }
        //                            //					iRetVal = eval_item(fmenu, ulItemMask, &errors,MENU_ITYPE);

        //                            DDlMenu* pMenu = new DDlMenu();

        //                            pMenu->id = fmenu->id;

        //                            pMenu->byItemType = MENU_ITYPE;

        //                            pMenu->strItemName = "Menu";

        //                            iRetVal = eval_menu(fmenu, ulItemMask, pMenu);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(menu, pMenu);
        //                                //#endif /*_PARSER_DEBUG*/

        //                                return false;
        //                            }


        //                            /*Push the parsed Menu on the ItemList*/

        //                            ItemsList.push_back(pMenu);


        //                            if (fmenu->depbin)
        //                            {
        //                                if (fmenu->depbin->db_items)
        //                                    delete(fmenu->depbin->db_items);
        //                                if (fmenu->depbin->db_label)
        //                                    delete(fmenu->depbin->db_label);

        //                                delete(fmenu->depbin);
        //                            }

        //                            memset((char*)fmenu, 0, sizeof(FLAT_MENU));

        //                            //					dump_menu(fout,fmenu);
        //                            break;
        //                        }
        //                    case EDIT_DISP_ITYPE:// Edit Display
        //                        {
        //                            //					memset((char *)fedisp, 0, sizeof(FLAT_EDIT_DISPLAY));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Edit Display Item*******************************\n");
        //                            //					printf("%d Edit Display Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Edit Display Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, EDIT_DISP_ITYPE,
        //                                                                            &ulItemMask, (void*)fedisp);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlEditDisplay* pEditDisp = new DDlEditDisplay();

        //                            pEditDisp->id = fedisp->id;

        //                            pEditDisp->byItemType = EDIT_DISP_ITYPE;

        //                            pEditDisp->strItemName = "Edit Display";

        //                            iRetVal = eval_edit_display(fedisp, ulItemMask, pEditDisp);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(edit_display, pEditDisp);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            /*Push the parsed Edit Display on the list*/

        //                            ItemsList.push_back(pEditDisp);

        //                            //					dump_edit_display(fout,fedisp);


        //                            if (fedisp->depbin)
        //                            {
        //                                if (fedisp->depbin->db_disp_items)
        //                                    delete(fedisp->depbin->db_disp_items);
        //                                if (fedisp->depbin->db_edit_items)
        //                                    delete(fedisp->depbin->db_edit_items);
        //                                if (fedisp->depbin->db_label)
        //                                    delete(fedisp->depbin->db_label);
        //                                if (fedisp->depbin->db_post_edit_act)
        //                                    delete(fedisp->depbin->db_post_edit_act);
        //                                if (fedisp->depbin->db_pre_edit_act)
        //                                    delete(fedisp->depbin->db_pre_edit_act);
        //                                delete(fedisp->depbin);
        //                            }

        //                            memset((char*)fedisp, 0, sizeof(FLAT_EDIT_DISPLAY));

        //                            break;
        //                        }
        //                    case METHOD_ITYPE://Method
        //                        {
        //                            //					memset((char *)fmeth, 0, sizeof(FLAT_METHOD));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Method Item*******************************\n");
        //                            //					printf("%d Method Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Method Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, METHOD_ITYPE,
        //                                                                            &ulItemMask, (void*)fmeth);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }
        //                            /*if(i == 141)
        //                                break;*/

        //                            DDlMethod* pMeth = new DDlMethod();

        //                            pMeth->id = fmeth->id;

        //                            pMeth->byItemType = METHOD_ITYPE;

        //                            pMeth->strItemName = "Method";

        //                            /*					if (i == 142)
        //                                                {
        //                                                    DDlAttribute **p;
        //                                                    p = (DDlAttribute **)new (DDlAttribute *);

        //                                                }*/

        //                            iRetVal = eval_method(fmeth, ulItemMask, pMeth);



        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(method, pMeth);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }
        //                            ItemsList.push_back(pMeth);


        //                            if (fmeth->depbin)
        //                            {
        //                                if (fmeth->depbin->db_class)
        //                                    delete(fmeth->depbin->db_class);
        //                                if (fmeth->depbin->db_def)
        //                                    delete(fmeth->depbin->db_def);
        //                                if (fmeth->depbin->db_help)
        //                                    delete(fmeth->depbin->db_help);
        //                                if (fmeth->depbin->db_label)
        //                                    delete(fmeth->depbin->db_label);
        //                                if (fmeth->depbin->db_scope)
        //                                    delete(fmeth->depbin->db_scope);
        //                                if (fmeth->depbin->db_valid)
        //                                    delete(fmeth->depbin->db_valid);
        //                                delete(fmeth->depbin);
        //                            }

        //                            memset((char*)fmeth, 0, sizeof(FLAT_METHOD));

        //                            //					dump_method(fout,fmeth);
        //                            break;
        //                        }
        //                    case REFRESH_ITYPE:// Refresh Relation
        //                        {
        //                            //					memset((char *)frfrsh, 0, sizeof(FLAT_REFRESH));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Refresh Item*******************************\n");
        //                            //					printf("%d Refresh Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Refresh Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, REFRESH_ITYPE,
        //                                                                            &ulItemMask, (void*)frfrsh);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }


        //                            DDlRefresh* pRefresh = new DDlRefresh();

        //                            pRefresh->id = frfrsh->id;

        //                            pRefresh->byItemType = REFRESH_ITYPE;

        //                            pRefresh->strItemName = "Refresh Relation";

        //                            iRetVal = eval_refresh(frfrsh, ulItemMask, pRefresh);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(refresh, pRefresh);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            ItemsList.push_back(pRefresh);


        //                            if (frfrsh->depbin)
        //                            {
        //                                if (frfrsh->depbin->db_items)
        //                                    delete(frfrsh->depbin->db_items);
        //                                delete(frfrsh->depbin);
        //                            }

        //                            memset((char*)frfrsh, 0, sizeof(FLAT_REFRESH));

        //                            //					dump_refresh(fout,frfrsh);
        //                            break;
        //                        }
        //                    case UNIT_ITYPE:// Unit Relation
        //                        {
        //                            //					memset((char *)funit, 0, sizeof(FLAT_UNIT));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Unit Item*******************************\n");
        //                            //					printf("%d Unit Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Unit Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, UNIT_ITYPE,
        //                                                                        &ulItemMask, (void*)funit);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlUnit* pUnit = new DDlUnit();

        //                            pUnit->id = funit->id;

        //                            pUnit->byItemType = UNIT_ITYPE;

        //                            pUnit->strItemName = "Unit Relation";

        //                            iRetVal = eval_unit(funit, ulItemMask, pUnit);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(unit, pUnit);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            ItemsList.push_back(pUnit);


        //                            if (funit->depbin)
        //                            {
        //                                if (funit->depbin->db_items)
        //                                    delete(funit->depbin->db_items);
        //                                delete(funit->depbin);
        //                            }

        //                            memset((char*)funit, 0, sizeof(FLAT_UNIT));

        //                            //					dump_unit(fout,funit);
        //                            break;
        //                        }
        //                    case WAO_ITYPE:// WAO Relation
        //                        {
        //                            //					memset((char *)fwao, 0, sizeof(FLAT_WAO));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Wao Item*******************************\n");
        //                            //					printf("%d Wao Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Wao Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, WAO_ITYPE,
        //                                                                        &ulItemMask, (void*)fwao);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlWao* pWao = new DDlWao();

        //                            pWao->id = fwao->id;

        //                            pWao->byItemType = WAO_ITYPE;

        //                            pWao->strItemName = "WAO Relation";

        //                            iRetVal = eval_wao(fwao, ulItemMask, pWao);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(wao, pWao);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            ItemsList.push_back(pWao);


        //                            if (fwao->depbin)
        //                            {
        //                                if (fwao->depbin->db_items)
        //                                    delete(fwao->depbin->db_items);
        //                                delete(fwao->depbin);
        //                            }

        //                            memset((char*)fwao, 0, sizeof(FLAT_WAO));

        //                            //					dump_wao(fout,fwao);
        //                            break;
        //                        }
        //                    case ITEM_ARRAY_ITYPE:// Item Array
        //                        {
        //                            //					memset((char *)fiarr, 0, sizeof(FLAT_ITEM_ARRAY));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Item Array Item*******************************\n");
        //                            //					printf("%d Item Array Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Item Array Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, ITEM_ARRAY_ITYPE,
        //                                                                            &ulItemMask, (void*)fiarr);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlItemArray* pItemArray = new DDlItemArray();

        //                            pItemArray->id = fiarr->id;

        //                            pItemArray->byItemType = ITEM_ARRAY_ITYPE;

        //                            pItemArray->byItemSubType = ((ITEM_EXTN*)pbyExtensions[i])->bySubType;

        //                            pItemArray->strItemName = "Item Array";


        //                            iRetVal = eval_item_array(fiarr, ulItemMask, pItemArray);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(item_array, pItemArray);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            ItemsList.push_back(pItemArray);


        //                            if (fiarr->depbin)
        //                            {
        //                                if (fiarr->depbin->db_elements)
        //                                    delete(fiarr->depbin->db_elements);
        //                                if (fiarr->depbin->db_help)
        //                                    delete(fiarr->depbin->db_help);
        //                                if (fiarr->depbin->db_label)
        //                                    delete(fiarr->depbin->db_label);
        //                                delete(fiarr->depbin);
        //                            }

        //                            memset((char*)fiarr, 0, sizeof(FLAT_ITEM_ARRAY));

        //                            //					dump_item_array(fout,fiarr);
        //                            break;
        //                        }
        //                    case COLLECTION_ITYPE:// Collection
        //                        {
        //                            //					memset((char *)fcoll, 0, sizeof(FLAT_COLLECTION));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Collection Item*******************************\n");
        //                            //					printf("%d Collection Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Collection Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, COLLECTION_ITYPE,
        //                                                                            &ulItemMask, (void*)fcoll);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlCollection* pCollection = new DDlCollection();

        //                            pCollection->id = fcoll->id;

        //                            pCollection->byItemType = COLLECTION_ITYPE;

        //                            pCollection->byItemSubType = ((ITEM_EXTN*)pbyExtensions[i])->bySubType;

        //                            pCollection->strItemName = "Collection";


        //                            iRetVal = eval_collection(fcoll, ulItemMask, pCollection);


        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(collection, pCollection);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            ItemsList.push_back(pCollection);

        //                            if (fcoll->depbin)
        //                            {
        //                                if (fcoll->depbin->db_help)
        //                                    delete(fcoll->depbin->db_help);
        //                                if (fcoll->depbin->db_label)
        //                                    delete(fcoll->depbin->db_label);
        //                                if (fcoll->depbin->db_valid)
        //                                    delete(fcoll->depbin->db_valid);// added 22jan07
        //                                if (fcoll->depbin->db_members)
        //                                    delete(fcoll->depbin->db_members);// saw was missing 22jan07
        //                                if (fcoll->depbin->db_debug_info)
        //                                    delete(fcoll->depbin->db_debug_info);
        //                                delete(fcoll->depbin);
        //                            }

        //                            memset((char*)fcoll, 0, sizeof(FLAT_COLLECTION));

        //                            //					dump_collection(fout,fcoll);
        //                            break;
        //                        }
        //                    case RECORD_ITYPE://Record
        //                        {
        //                            //					memset((char *)frec, 0, sizeof(FLAT_RECORD));
        //                            //#ifdef _PARSER_DEBUG
        //                            //					myprintf(fout,"\n*******************************Record Item*******************************\n");
        //                            //					printf("%d Record Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //					myprintf(fout,"%d Record Object: \t Index : %u \n", i+1,ObjectFixed[i].index);
        //                            //#endif
        //                            iRetVal = fetch_item(pbyExtensions[i], objectIndex, RECORD_ITYPE,
        //                                                                        &ulItemMask, (void*)frec);
        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                if (iRetVal == FETCH_EXTERNAL_OBJECT)
        //                                    break; /*Don't quit if an external object was tried as a base object*/
        //                                return false;
        //                            }

        //                            DDlRecord* pRecord = new DDlRecord();

        //                            pRecord->id = frec->id;

        //                            pRecord->byItemType = RECORD_ITYPE;

        //                            pRecord->byItemSubType = ((ITEM_EXTN*)pbyExtensions[i])->bySubType;

        //                            pRecord->strItemName = "Record";

        //                            iRetVal = eval_record(frec, ulItemMask, pRecord);

        //                            if (iRetVal != SUCCESS)
        //                            {
        //                                //#ifdef _PARSER_DEBUG
        //                                EVAL_FAILED(record, pRecord);
        //                                //#endif /*_PARSER_DEBUG*/
        //                                return false;
        //                            }

        //                            /*Vibhor 311003: For implementing the Demunging Solution We will store this
        //                             as a Collection item*/

        //                            pRecord->byItemType = COLLECTION_ITYPE;

        //                            pRecord->strItemName = "Collection";

        //                            ItemsList.push_back(pRecord);


        //                            if (frec->depbin)
        //                            {
        //                                if (frec->depbin->db_help)
        //                                    delete(frec->depbin->db_help);
        //                                if (frec->depbin->db_label)
        //                                    delete(frec->depbin->db_label);
        //                                if (frec->depbin->db_members)
        //                                    delete(frec->depbin->db_members);
        //                                if (frec->depbin->db_resp_codes)
        //                                    delete(frec->depbin->db_resp_codes);
        //                                delete(frec->depbin);
        //                            }

        //                            memset((char*)frec, 0, sizeof(FLAT_RECORD));


        //                            //					dump_record(fout,frec);
        //                            break;
        //                        }
        //                    case ARRAY_ITYPE://Array
        //                                     //#ifdef _PARSER_DEBUG
        //                        LOGIT(CERR_LOG, L"\n#################################ARRAY ITEM!!!#################################\n");
        //                        //#endif
        //                        break;
        //                    case VAR_LIST_ITYPE:// Variable List
        //                                        //#ifdef _PARSER_DEBUG
        //                        LOGIT(CERR_LOG, L"\n#################################VARRIABLE LIST ITEM!!!#################################\n");
        //                        //#endif
        //                        break;

        //                    case RESP_CODES_ITYPE:// Response Code
        //                                          //#ifdef _PARSER_DEBUG
        //                        LOGIT(CERR_LOG, L"\n#################################RESP CODE ITEM!!!#################################\n");
        //                        //#endif
        //                        break;

        //                    default:

        //                        //					printf("Error : Invalid Object Type  : Type Code = % d\n",pbyExtensions[i][1]);
        //                        //#ifdef _PARSER_DEBUG
        //                        if ((pbyExtensions[i][1] == ARRAY_ITYPE)
        //                            || (pbyExtensions[i][1] == VAR_LIST_ITYPE)
        //                            || (pbyExtensions[i][1] == RESP_CODES_ITYPE)
        //                          )
        //                        {
        //                            cout << "Error : Invalid Object Type  : Type Code = " << pbyExtensions[i][1] << endl;
        //                        }
        //                        //#endif
        //                        break;

        //                }/*End switch (pbyExtensions[i][1])*/

        //            }/*End for*/



        //            return true;
        //        }/*End GetItems()*/

    }
}
