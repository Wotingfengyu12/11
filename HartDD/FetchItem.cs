using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class FetchItem
    {

        public const int ATTR_RI_TAG_SIZE = 1;/* size of reference indicator/tag */
        public const int LOCAL_REF_SIZE = 2;   /* size of local data field reference */
        public const int EXTERNAL_REF_SIZE = 2;    /* size of external object reference */
                                                   /* stevev 9/24/04 const int MAX_OBJ_EXTN_LEN            180	- see note below */
        public const int MAX_OBJ_EXTN_LEN = 250;/* leave a 5 byte pad */
                                                //was:: 204       /* maximum size of object extension */										
        public const int MAX_LOCAL_REF_SIZE = 6;   /* maximum size of encode int + ri/tag*/

        public const int REF_INDICATOR_MASK     = 0x03;
        public const int ATTR_TAG_MASK          = 0x3F;
        public const int TERMINATING_BIT_MASK   = 0x80;
        public const int LENGTH_MASK            = 0x7F;

        public const int REF_INDICATOR_SHIFT    = 6;
        public const int LENGTH_SHIFT           = 7;
        public const uint MAX_LENGTH_MASK = 0xfe000000;
        public const int LENGTH_ENCODE_MASK = ~LENGTH_MASK;

        public const int FETCH_INVALID_DEVICE_HANDLE = (1600 + 0);
        public const int FETCH_DEVICE_NOT_FOUND = (1600 + 1);
        public const int FETCH_INVALID_DEV_TYPE_HANDLE = (1600 + 2);
        public const int FETCH_DEV_TYPE_NOT_FOUND = (1600 + 3);
        public const int FETCH_INVALID_DD_HANDLE_TYPE = (1600 + 4);
        public const int FETCH_TABLES_NOT_FOUND = (1600 + 5);
        public const int FETCH_ITEM_NOT_FOUND = (1600 + 6);
        public const int FETCH_DIRECTORY_NOT_FOUND = (1600 + 7);
        public const int FETCH_INSUFFICIENT_SCRATCHPAD = (1600 + 10);
        public const int FETCH_NULL_POINTER = (1600 + 11);
        public const int FETCH_ITEM_TYPE_MISMATCH = (1600 + 12);
        public const int FETCH_INVALID_ATTRIBUTE = (1600 + 13);
        public const int FETCH_INVALID_RI = (1600 + 14);
        public const int FETCH_INVALID_ITEM_TYPE = (1600 + 15);
        public const int FETCH_EMPTY_ITEM_MASK = (1600 + 16);
        public const int FETCH_ATTRIBUTE_NO_MASK_BIT = (1600 + 17);
        public const int FETCH_ATTRIBUTE_NOT_FOUND = (1600 + 18);
        public const int FETCH_ATTR_LENGTH_OVERFLOW = (1600 + 19);
        public const int FETCH_ATTR_ZERO_LENGTH = (1600 + 20);
        public const int FETCH_OBJECT_NOT_FOUND = (1600 + 21);
        public const int FETCH_DATA_NOT_FOUND = (1600 + 22);
        public const int FETCH_DATA_OUT_OF_RANGE = (1600 + 23);
        public const int FETCH_INVALID_DIR_TYPE = (1600 + 24);
        public const int FETCH_INVALID_TABLE = (1600 + 25);
        public const int FETCH_INVALID_EXTN_LEN = (1600 + 26);
        public const int FETCH_INVALID_PARAM = (1600 + 27);
        public const int FETCH_INVALID_ITEM_ID = (1600 + 28);
        public const int FETCH_INVALID_ATTR_LENGTH = (1600 + 29);
        public const int FETCH_BAD_DD_DEVICE_LOAD = (1600 + 30);
        public const int FETCH_DIR_TYPE_MISMATCH = (1600 + 31);
        public const int FETCH_NO_OBJ_EXTN = (1600 + 32);
        /*Vibhor 070803: Adding this return code: ;*/
        public const int FETCH_EXTERNAL_OBJECT = (1600 + 33);
        public const int FETCH_ERROR_END = (1600 + 99);
        public const int RI_IMMEDIATE = 0;
        public const int RI_LOCAL = 1;
        public const int RI_EXTERNAL_SINGLE = 2;
        public const int RI_EXTERNAL_ALL = 3;
        
        unsafe public static int parse_attribute_id(byte * ptr, ref uint attr_ptr_offset, ushort* attr_RI, ushort* attr_tag, uint* attr_len)
        {

            byte* local_attr_ptr;  /* points to current attribute in
									 * object extension */
            ushort temp;            /* used to unpack attribute ID field */
            uint calc_length;  /* used to unpack attribute length */

            /*
             * Check for valid parameters
             */

           // ASSERT_RET(attr_ptr && attr_RI && attr_tag && attr_len, FETCH_INVALID_PARAM);

            /*
             * Extract the RI and tag from the Attribute Identifier (octet #1).
             * Check the validity of the input parameters before using any of the
             * values or references.
             */

            local_attr_ptr = ptr + attr_ptr_offset;
            //ASSERT_RET(local_attr_ptr, FETCH_INVALID_PARAM);

            /*
             * The attribute RI is in the MS 2 bits of the Attribute Identifier
             * field while the attribute tag is in the LS 6 bits.
             */

            temp = (ushort) *local_attr_ptr++;
            *attr_tag = (ushort)(temp & (ushort)ATTR_TAG_MASK);
            *attr_RI = (ushort)((temp >> REF_INDICATOR_SHIFT) & REF_INDICATOR_MASK);

            /*
             * If bit 8 of the length byte is set, the length is encoded into the
             * LS 7 bits of this byte and subsequent bytes.  Bit 8 of the last byte
             * of the encoded sequence is 0.  The upper 7 bits of the calculated
             * length are monitored during unpacking and a length overflow error is
             * returned if these are nonzero.
             */

            calc_length = 0;
            do
            {
                if ((calc_length & MAX_LENGTH_MASK) != 0)
                {
                    return (1619);
                }
                calc_length = (calc_length << LENGTH_SHIFT) | (uint) (LENGTH_MASK & *local_attr_ptr);
            }
            while ((LENGTH_ENCODE_MASK & *local_attr_ptr++) != 0);

            *attr_len = calc_length;
            if (calc_length == 0)
            {
                return (FETCH_ATTR_ZERO_LENGTH);
            }

            attr_ptr_offset = (byte)(local_attr_ptr - ptr);
            return (0);
        }

    }
}
