using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class Eval
    {
        public const int CNTRYCDSTRLEN = 6;	/* '|'DDDD'|' */

        public enum EVALDIR_FN
        {
            eval_dir_dict_ref_tbl, eval_dir_item_tbl, eval_dir_string_tbl, eval_dir_image_tbl, eval_dir_cmd_num_id_tbl, eval_dir_dict_ref_tbl8, eval_dir_domain_tbl, eval_dir_prog_tbl, eval_dir_local_var_tbl
        }

        public static unsafe int eval_dir_dict_ref_tbl(ref DDlDevDescription.DICT_REF_TBL dict_ref_tbl, ref DDlDevDescription.BININFO bin)
        {

            uint* element = null;             /* temp pointer for the list */
            uint* end_element = null;         /* end pointer for the list */
            UInt64 temp_int;              /* integer value */
            //int rc;                     /* return code */

            fixed (byte* chunk = &bin.chunk[bin.uoffset])
            {
                fixed (uint* pz = &bin.size)
                {
                    Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                    /* parse count */
                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        dict_ref_tbl.count = 0;
                        dict_ref_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    dict_ref_tbl.count = (ushort)temp_int;

                    /* malloc the list */
                    dict_ref_tbl.list = new uint[((uint)(temp_int))];

                    if (dict_ref_tbl.list == null)
                    {

                        dict_ref_tbl.count = 0;
                        return Common.DDL_MEMORY_ERROR;
                    }

                    /* load list with zeros */
                    //(void)memset((char*)dict_ref_tbl->list, 0, (size_t)temp_int * sizeof(uint));

                    /*
                     * load the list
                     */
                    /*
                                for (element = dict_ref_tbl.list, end_element = element + temp_int;
                                     element < end_element; element++)
                                {

                                    DDlDevDescription.DDL_PARSE_INTEGER(&bin.chunk[0], &bin.size, &temp_int);
                                    *element = (uint)temp_int;
                                }
                                */
                    for (uint i = 0; i < dict_ref_tbl.count; i++)
                    {
                        Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                        dict_ref_tbl.list[i] = (uint)temp_int;
                    }

                }
                /* parse count */
            }
            return Common.DDL_SUCCESS;
        }

        public unsafe static int eval_dir_item_tbl(ref DDlDevDescription.ITEM_TBL item_tbl, ref DDlDevDescription.BININFO bin)
        {

            //DDlDevDescription.ITEM_TBL_ELEM end_element = NULL; /* end pointer for the list */
            UInt64 temp_int;    /* integer value */
            //int rc;           /* return code */

            //ASSERT_DBG(bin && bin->chunk && bin->size);

            /* parse count */
            fixed (byte* chunk = &bin.chunk[bin.uoffset])
            {
                fixed (uint* pz = &bin.size)
                {
                    Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                    /* parse count */

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        item_tbl.count = 0;
                        item_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    item_tbl.count = (ushort)temp_int;

                    /* malloc the list */
                    item_tbl.list = new DDlDevDescription.ITEM_TBL_ELEM[temp_int];

                    /*
                     * load the list
                     */

                    //for (element = item_tbl->list, end_element = element + temp_int;                 element < end_element; element++)

                    for (uint i = 0; i < item_tbl.count; i++)
                    {

                        Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                        item_tbl.list[i].item_id = (uint)temp_int;

                        Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                        item_tbl.list[i].dd_ref = (ushort)temp_int;//??????

                        Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                        item_tbl.list[i].item_type = (ushort)temp_int;
                    }
                }
            }

            return Common.DDL_SUCCESS;
        }

        public unsafe static int eval_dir_string_tbl(ref DDlDevDescription.STRING_TBL string_tbl, ref DDlDevDescription.BININFO bin)
        {

            //ddpSTRING * string = NULL;      /* temp pointer for the list */
            //ddpSTRING* end_string = NULL;   /* end pointer for the list */
            UInt64 temp_int;      /* integer value */
            uint size = 0;         /* temp size */
            //byte* root_ptr = null;     /* temp pointer */
            //int rc = 0;             /* return code */

            //ASSERT_DBG(bin && bin->chunk && bin->size);

            /* parse count */
            fixed (byte* chunk = &bin.chunk[bin.uoffset])
            {
                fixed (uint* pz = &bin.size)
                {
                    Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                    /* parse count */

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        string_tbl.count = 0;
                        string_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    //assert(temp_int < MAXIMUM_INT);

                    string_tbl.count = (int)temp_int;

                    /* malloc the list */
                    string_tbl.list = new ddpSTRING[temp_int];
                    //if (string_tbl->list == NULL)
                    //{

                    //    string_tbl->count = 0;
                    //    return DDL_MEMORY_ERROR;
                    //}

                    /* malloc the root */
                    /*	string_tbl->root =
                            (unsigned char *)new char[((size_t) (bin->size ))];
                        if (string_tbl->root == NULL) {

                            return DDL_MEMORY_ERROR;
                        }

                        /* load list with zeros */
                    /*	(void)memset((char *) string_tbl->list, 0,
                                (size_t) (temp_int * sizeof(ddpSTRING))); */

                    /* copy the chunk to the root */
                    /*	(void)memcpy((char *) string_tbl->root, (char *) bin->chunk,
                                (size_t) (bin->size * sizeof(unsigned char))); */

                    /*
                     * load the list
                     */


                    size = bin.size;
                    byte* ch = chunk;
                    for (uint i = 0; i < string_tbl.count; i++)
                    {
                        Common.DDL_PARSE_INTEGER(&ch, &size, &temp_int);
                        byte[] rootchar = new byte[temp_int];
                        string_tbl.list[i] = new ddpSTRING();
                        for (uint j = 0; j < temp_int; j++)
                        {
                            rootchar[j] = ch[j];//bin.chunk[bin.uoffset + j + i * temp_int];//Encoding.ASCII.GetChars(bin.chunk)
                        }
                        string_tbl.list[(int)i].len = (ushort)temp_int;//??????
                        string_tbl.list[(int)i].str = Encoding.Default.GetString(rootchar);//Convert.ToString(rootchar);// new string(rootchar);// Convert.ToBase64String(bin.chunk);//??????
                        string_tbl.list[(int)i].str = string_tbl.list[(int)i].str.TrimEnd('\0');
                        string_tbl.list[(int)i].flags = Common.DONT_FREE_STRING;
                        size -= (ushort)temp_int;
                        ch += temp_int;
                    }

                }
            }
            return Common.DDL_SUCCESS;
        }

        public unsafe static int eval_dir_cmd_num_id_tbl(ref DDlDevDescription.CMD_NUM_ID_TBL cmd_num_id_tbl, ref DDlDevDescription.BININFO bin)
        {

            UInt64 temp_int;    /* integer value */

            //ASSERT_DBG(bin && bin->chunk && bin->size);

            fixed (byte* chunk = &bin.chunk[bin.uoffset])
            {
                fixed (uint* pz = &bin.size)
                {
                    Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                    /* parse count */
                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        cmd_num_id_tbl.count = 0;
                        cmd_num_id_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    cmd_num_id_tbl.count = (ushort)temp_int;

                    /* malloc the list */
                    //cmd_num_id_tbl->list = (CMD_NUM_ID_TBL_ELEM*)new CMD_NUM_ID_TBL_ELEM[((size_t)(temp_int))];
                    cmd_num_id_tbl.list = new DDlDevDescription.CMD_NUM_ID_TBL_ELEM[temp_int];

                    /*
                     * load the list
                     */

                    for (uint i = 0; i < cmd_num_id_tbl.count; i++)
                    {

                        /* command number */
                        Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                        cmd_num_id_tbl.list[i].number = (ushort)temp_int;
                        /* parse ITEM_ID */
                        Common.DDL_PARSE_INTEGER(&chunk, pz, &temp_int);
                        cmd_num_id_tbl.list[i].item_id = (uint)temp_int;


                    }
                }
            }

            return Common.DDL_SUCCESS;
        }


        public unsafe static int eval_dir_device_tables(ref DDlDevDescription.FLAT_DEVICE_DIR device_dir, ref DDlDevDescription.BIN_DEVICE_DIR device_bin, uint mask)
        {

            int rc = Common.DDS_SUCCESS;   /* return code */

            if ((mask & DDlDevDescription.BLK_TBL_MASK) != 0)
            {

                /*		rc = dir_mask_man((uint) BLK_TBL_MASK,
                            device_bin->bin_exists, device_bin->bin_hooked,
                            &device_dir->attr_avail, (int (*) ()) eval_dir_blk_tbl,
                            (void *) &device_dir->blk_tbl, &device_bin->blk_tbl);

                        if (rc != DDL_SUCCESS) {

                            return rc;
                        }*/
            }
            if ((mask & DDlDevDescription.DICT_REF_TBL_MASK) != 0)
            {
                rc = Eval.eval_dir_dict_ref_tbl(ref device_dir.dict_ref_tbl, ref device_bin.dict_ref_tbl);
            }
            /*	if (mask & DOMAIN_TBL_MASK) {

                    rc = dir_mask_man((uint) DOMAIN_TBL_MASK,
                        device_bin->bin_exists, device_bin->bin_hooked,
                        &device_dir->attr_avail, EVALDIR_FN. eval_dir_domain_tbl,
                        (void *) &device_dir->domain_tbl, &device_bin->domain_tbl);

                    if (rc != DDL_SUCCESS) {

                        return rc;
                    }
                }
            */
            if ((mask & DDlDevDescription.ITEM_TBL_MASK) != 0)
            {
                rc = Eval.eval_dir_item_tbl(ref device_dir.item_tbl, ref device_bin.dict_ref_tbl);

                if (rc != Common.DDL_SUCCESS)
                {

                    return rc;
                }
            }
            /*	if (mask & PROG_TBL_MASK) {

                    rc = dir_mask_man((uint) PROG_TBL_MASK,
                        device_bin->bin_exists, device_bin->bin_hooked,
                        &device_dir->attr_avail, EVALDIR_FN. eval_dir_prog_tbl,
                        (void *) &device_dir->prog_tbl, &device_bin->prog_tbl);

                    if (rc != DDL_SUCCESS) {

                        return rc;
                    }
                }
            */
            if ((mask & DDlDevDescription.STRING_TBL_MASK) != 0)
            {

                rc = Eval.eval_dir_string_tbl(ref device_dir.string_tbl, ref device_bin.dict_ref_tbl);


                if (rc != Common.DDL_SUCCESS)
                {

                    return rc;
                }

                if (DDlDevDescription.pLitStringTable != null) // stevev 24apr08 - for fm <8..stevev 22apr13 moved to v5 from v6
                                                               // copy the parsed string table into the global lit string table
                {
                    DDlDevDescription.pLitStringTable.makelit(device_dir.string_tbl, true);
                }
            }

            /*	if (mask & LOCAL_VAR_TBL_MASK) {

                    rc = dir_mask_man((uint) LOCAL_VAR_TBL_MASK,
                        device_bin->bin_exists, device_bin->bin_hooked,
                        &device_dir->attr_avail, EVALDIR_FN. eval_dir_local_var_tbl,
                        (void *) &device_dir->local_var_tbl, &device_bin->local_var_tbl);
                    if (rc != DDL_SUCCESS) {

                        return rc;
                    }
                }
            */
            if ((mask & DDlDevDescription.CMD_NUM_ID_TBL_MASK) != 0)
            {

                rc = Eval.eval_dir_cmd_num_id_tbl(ref device_dir.cmd_num_id_tbl, ref device_bin.dict_ref_tbl);
                if (rc != Common.DDL_SUCCESS)
                {
                    return rc;
                }
            }

            return rc;
        }

        public static unsafe int eval_dir_device_tables_6(ref DDlDevDescription.FLAT_DEVICE_DIR_6 device_dir, ref DDlDevDescription.BIN_DEVICE_DIR_6 device_bin, uint mask)
        {

            int rc = Common.DDL_SUCCESS;   /* return code */


            if ((mask & DDlDevDescription.BLK_TBL_MASK) != 0)
            {

                /*		rc = dir_mask_man((uint) BLK_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, &device_dir.attr_avail, (int (*) ()) eval_dir_blk_tbl, (void *) &device_dir.blk_tbl, &device_bin.blk_tbl);

                        if ((rc != DDL_SUCCESS) { != 0)

                            return rc;
                        }*/
            }
            if ((mask & DDlDevDescription.DICT_REF_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.DICT_REF_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_dict_ref_tbl, ref device_dir, ref device_bin.dict_ref_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }

                // copy the parsed dictionary into the global dictionary
                DDlDevDescription.pGlobalDict.makedict(ref device_dir.dict_ref_tbl);
            }

            /*	if ((mask & DOMAIN_TBL_MASK) { != 0)

                    rc = dir_mask_man((uint) DOMAIN_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, &device_dir.attr_avail, EVALDIR_FN. eval_dir_domain_tbl, (void *) &device_dir.domain_tbl, &device_bin.domain_tbl);

                    if ((rc != DDL_SUCCESS) { != 0)

                        return rc;
                    }
                }
            */
            if ((mask & DDlDevDescription.ITEM_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.ITEM_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_item_tbl, ref device_dir, ref device_bin.item_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }

            /*	if ((mask & PROG_TBL_MASK) { != 0)

                    rc = dir_mask_man((uint) PROG_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, &device_dir.attr_avail, EVALDIR_FN. eval_dir_prog_tbl, (void *) &device_dir.prog_tbl, &device_bin.prog_tbl);

                    if ((rc != DDL_SUCCESS) { != 0)

                        return rc;
                    }
                }
            */
            if ((mask & DDlDevDescription.STRING_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.STRING_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_string_tbl, ref device_dir, ref device_bin.string_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }

                if (DDlDevDescription.pLitStringTable != null) // stevev 24apr08 - for fm <8 != 0)
                                                               // copy the parsed string table into the global lit string table
                    DDlDevDescription.pLitStringTable.makelit(device_dir.string_tbl, true);
            }

            /*	if ((mask & LOCAL_VAR_TBL_MASK) { != 0)

                    rc = dir_mask_man((uint) LOCAL_VAR_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, &device_dir.attr_avail, EVALDIR_FN. eval_dir_local_var_tbl, (void *) &device_dir.local_var_tbl, &device_bin.local_var_tbl);
                    if ((rc != DDL_SUCCESS) { != 0)

                        return rc;
                    }
                }
            */
            if ((mask & DDlDevDescription.CMD_NUM_ID_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.CMD_NUM_ID_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_cmd_num_id_tbl, ref device_dir, ref device_bin.cmd_num_id_tbl);
                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }

            if ((mask & DDlDevDescription.IMAGE_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.IMAGE_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_image_tbl, ref device_dir, ref device_bin.image_tbl);
                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }


            return rc;
        }

        /*Vibhor 020904: End of Code*/


        // timj added 9oct07
        public static unsafe int eval_dir_device_tables_8(ref DDlDevDescription.FLAT_DEVICE_DIR_6 device_dir, ref DDlDevDescription.BIN_DEVICE_DIR_6 device_bin, uint mask)
        {

            int rc = Common.DDL_SUCCESS;   /* return code */


            if ((mask & DDlDevDescription.BLK_TBL_MASK) != 0)
            {

                /*		rc = dir_mask_man((uint) BLK_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, &device_dir.attr_avail, (int (*) ()) eval_dir_blk_tbl, (void *) &device_dir.blk_tbl, &device_bin.blk_tbl);

                        if ((rc != DDL_SUCCESS) { != 0)

                            return rc;
                        }*/
            }
            if ((mask & DDlDevDescription.DICT_REF_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.DICT_REF_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_dict_ref_tbl8, ref device_dir, ref device_bin.dict_ref_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }

                // copy the parsed dictionary into the global dictionary
                DDlDevDescription.pGlobalDict.makedict(ref device_dir.dict_ref_tbl);

            }
            if ((mask & DDlDevDescription.DOMAIN_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.DOMAIN_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_domain_tbl, ref device_dir, ref device_bin.domain_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }
            if ((mask & DDlDevDescription.ITEM_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.ITEM_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_item_tbl, ref device_dir, ref device_bin.item_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }
            if ((mask & DDlDevDescription.PROG_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.PROG_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_prog_tbl, ref device_dir, ref device_bin.prog_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }
            if ((mask & DDlDevDescription.STRING_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.STRING_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_string_tbl, ref device_dir, ref device_bin.string_tbl);

                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }

                if (DDlDevDescription.pLitStringTable != null) // stevev 24apr08 - for fm <8 != 0)
                                                               // copy the parsed string table into the global lit string table
                    DDlDevDescription.pLitStringTable.makelit((device_dir.string_tbl), false);
            }

            if ((mask & DDlDevDescription.LOCAL_VAR_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.LOCAL_VAR_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_local_var_tbl, ref device_dir, ref device_bin.local_var_tbl);
                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }

            if ((mask & DDlDevDescription.CMD_NUM_ID_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.CMD_NUM_ID_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_cmd_num_id_tbl, ref device_dir, ref device_bin.cmd_num_id_tbl);
                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }

            if ((mask & DDlDevDescription.IMAGE_TBL_MASK) != 0)
            {

                rc = dir_mask_man((uint)DDlDevDescription.IMAGE_TBL_MASK, device_bin.bin_exists, device_bin.bin_hooked, ref device_dir.attr_avail, EVALDIR_FN.eval_dir_image_tbl, ref device_dir, ref device_bin.image_tbl);
                if ((rc != Common.DDL_SUCCESS))
                {

                    return rc;
                }
            }


            return rc;
        }

        public static unsafe int eval_dir_image_tbl(ref DDlDevDescription.IMAGE_TBL image_table, ref DDlDevDescription.BININFO bin)
        {
            byte* root_ptr = null;     /* temp pointer */
            fixed (byte* chu = &bin.chunk[bin.uoffset])
            {
                fixed (uint* puiSize = &bin.size)
                {

                    DDlDevDescription.IMAGE_TBL_ELEM element;// = null;     /* temp pointer for the list */
                                                             //IMAGE_TBL_ELEM* end_element = null; /* end pointer for the list */
                                                             //IMG_ITEM* item = null;      /*temp ptr to img item list*/
                                                             //IMG_ITEM* end_item = null;  /*end ptr to img item list*/
                    UInt64 temp_int = 0;      /* integer value */
                    uint size = 0;         /* temp size */
                                               //int rc = 0;             /* return code */

                    //uint img_list_size = 0; /*temp size of actual img list*/
                    byte* base_ptr = null;  /*base ptr of img list chunk*/

                    //ASSERT_DBG(bin && bin.chunk && bin.size);

                    /* parse count */
                    Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                    //assert(temp_int < MAXIMUM_INT);

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        image_table.count = 0;
                        image_table.list = null;
                        return Common.DDL_SUCCESS;
                    }
                    //else store the count

                    image_table.count = (ushort)temp_int;

                    //Allocate the list

                    image_table.list = new DDlDevDescription.IMAGE_TBL_ELEM[temp_int];

                    if (image_table.list == null)
                    {

                        image_table.count = 0;
                        return Common.DDL_MEMORY_ERROR;
                    }

                    size = bin.size;
                    root_ptr = chu;

                    /*Loop through the chunks and form the table*/

                    //for (element = image_table.list, end_element = image_table.list + temp_int; element < end_element && size > 0; element++)
                    for (int i = 0; i < image_table.count; i++)
                    {
                        element = new DDlDevDescription.IMAGE_TBL_ELEM();
                        //for each element , parse the number of languages
                        Common.DDL_PARSE_INTEGER(&root_ptr, &size, &temp_int);
                        //assert(temp_int < MAXIMUM_INT);

                        //TODO : Handle a graceful exit here as this may result in leaks.
                        /*this number must be a non zero*/
                        if (temp_int == 0)
                        {
                            return Common.DDL_MEMORY_ERROR;
                        }
                        //For now assume: Its a non-zer0 number
                        element.num_langs = (ushort)temp_int;//??????

                        //Allocate the number or IMAGE_ITEMs on the list for each lang.

                        element.img_list = new DDlDevDescription.IMG_ITEM[(temp_int)];
                        //if (image_table.list == null)
                        //{

                        //image_table.count = 0;
                        //return Common.DDL_MEMORY_ERROR;
                        //}

                        // Parse the image list.
                        DDlDevDescription.IMG_ITEM pImgItm;
                        // for each language
                        for (ushort y = 0; y < element.num_langs && size > 0; y++)
                        {
                            pImgItm = new DDlDevDescription.IMG_ITEM();
                            //pImgItm = &(element.img_list[y]);
                            // get byte string (6)  (define is same as used in tokenizer)
                            //strncpy((char*)(pImgItm.lang_code), (char*)root_ptr, CNTRYCDSTRLEN);

                            //image_table.list[i].img_list[y].lang_code = root_ptr;//??????
                            //image_table.list[i].img_list[y].SetLang(bin.chunk);//??????

                            //pImgItm.lang_code = new byte[CNTRYCDSTRLEN];// bin.chunk;

                            /*
                            for(ushort u = 0; u < CNTRYCDSTRLEN; u++)
                            {
                                pImgItm.lang_code[u] = root_ptr[u];
                            }
                            */
                            byte[] rootchar = new byte[CNTRYCDSTRLEN];
                            for (ushort u = 0; u < CNTRYCDSTRLEN; u++)
                            {
                                rootchar[u] = root_ptr[u];
                            }

                            pImgItm.lang_code = Encoding.Default.GetString(rootchar);
                            pImgItm.lang_code = pImgItm.lang_code.TrimEnd('\0');

                            size -= CNTRYCDSTRLEN;        /* decrement size */
                            root_ptr += CNTRYCDSTRLEN;        /* increment the root pointer */

                            // get datapart segment
                            //		get 	offset	uint,

                            Common.DDL_PARSE_INTEGER(&root_ptr, &size, &temp_int);

                            pImgItm.img_file.offset = (uint)temp_int;

                            //		get		size	ushort32				
                            Common.DDL_PARSE_INTEGER(&root_ptr, &size, &temp_int);

                            pImgItm.img_file.uSize = (ushort)temp_int;
                            //element.img_list.Add(pImgItm);
                            element.img_list[y] = pImgItm;
                            // stevev 11dec08 - missing images are getting through
                            // stevev 27jul12 - trust the tokenizer to filter images that are too big
                            /***
                            if (pImgItm.img_file.uSize > LARGEST_IMAGE_SIZE)
                            {// we have a bogus entry
                                pImgItm.lang_code[0] = '\0';
                                pImgItm.img_file.offset = 0;
                                pImgItm.img_file.uSize  = 0;
                                pImgItm.p2Graphik       = null;
                            }
                            ***/
                        }// next language
                        image_table.list[i] = (element);
                    }/*Endfor -  next image */
                }
            }

            return Common.DDL_SUCCESS;

        }/*End eval_dir_image_tbl*/

        public unsafe static int eval_dir_dict_ref_tbl8(ref DDlDevDescription.DICT_REF_TBL dict_ref_tbl, ref DDlDevDescription.BININFO bin)
        {

            //uint* element = null;             /* temp pointer for the list */
            //uint* end_element = null;         /* end pointer for the list */
            uint size = 0;         /* temp size */
            byte* root_ptr = null;     /* temp pointer */
            UInt64 temp_int;              /* integer value */
            //int rc;                     /* return code */
            fixed (byte* chu = &bin.chunk[bin.uoffset])
            {
                fixed (uint* puiSize = &bin.size)
                {

                    //ASSERT_DBG(bin && bin.chunk && bin.size);

                    /* parse count */
                    Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        dict_ref_tbl.count = 0;
                        dict_ref_tbl.list = null;
                        dict_ref_tbl.name = null;
                        dict_ref_tbl.text = null;
                        return Common.DDL_SUCCESS;
                    }

                    dict_ref_tbl.count = (ushort)temp_int;

                    /* malloc the lists */
                    dict_ref_tbl.list = new uint[temp_int];// (uint*)new uint[((size_t)(temp_int))];

                    dict_ref_tbl.name = new ddpSTRING[temp_int];
                    dict_ref_tbl.text = new ddpSTRING[temp_int];

                    for (ushort i = 0; i < temp_int; i++)
                    {
                        dict_ref_tbl.name[i] = new ddpSTRING();
                        dict_ref_tbl.text[i] = new ddpSTRING();
                    }

                    if (dict_ref_tbl.list == null || dict_ref_tbl.name == null || dict_ref_tbl.text == null)
                    {

                        dict_ref_tbl.count = 0;
                        return Common.DDL_MEMORY_ERROR;
                    }


                    /* load list with zeros */
                    //(void)memset((char*)dict_ref_tbl.list, 0, (size_t)temp_int * sizeof(uint));

                    /*
                     * load the list
                     */

                    size = bin.size;
                    root_ptr = chu;
                    for (ushort i = 0; i < dict_ref_tbl.count; i++)
                    {
                        //char* s;

                        // value

                        Common.DDL_PARSE_INTEGER(&root_ptr, &size, &temp_int);
                        dict_ref_tbl.list[i] = (uint)temp_int;

                        // name
                        // stevev 06jan10 - allow string to be freed.  The makedict call immediatly after this
                        //		makes a duplicate of both the strings.

                        Common.DDL_PARSE_INTEGER(&root_ptr, &size, &temp_int);
                        //assert(temp_int < MAXIMUM_USHRT);
                        dict_ref_tbl.name[i].len = (ushort)temp_int;

                        //dict_ref_tbl.name[i].str = new char[(uint)temp_int + 1];
                        //strcpy(dict_ref_tbl.name[i].str, (char*)root_ptr);
                        byte[] cc = new byte[temp_int];
                        for (int j = 0; j < dict_ref_tbl.name[i].len; j++)
                        {
                            cc[j] = root_ptr[j];
                        }
                        dict_ref_tbl.name[i].str = Encoding.Default.GetString(cc);//??????
                        dict_ref_tbl.name[i].str = dict_ref_tbl.name[i].str.TrimEnd('\0');//??????
                        size -= (ushort)temp_int;
                        root_ptr += temp_int;
                        dict_ref_tbl.name[i].flags = Common.FREE_STRING;// DONT_FREE_STRING;
                        //s = dict_ref_tbl.name[i].str;

                        // text

                        Common.DDL_PARSE_INTEGER(&root_ptr, &size, &temp_int);
                        //assert(temp_int < MAXIMUM_USHRT);
                        dict_ref_tbl.text[i].len = (ushort)temp_int;
                        //dict_ref_tbl.text[i].str = new char[(ushort)temp_int + 1];
                        //strcpy(dict_ref_tbl.text[i].str, (char*)root_ptr);
                        cc = new byte[temp_int];
                        for (int j = 0; j < dict_ref_tbl.text[i].len; j++)
                        {
                            cc[j] = root_ptr[j];
                        }
                        dict_ref_tbl.text[i].str = Encoding.Default.GetString(cc);//??????
                        dict_ref_tbl.text[i].str = dict_ref_tbl.text[i].str.TrimEnd('\0');//??????
                        size -= (ushort)temp_int;
                        root_ptr += temp_int;
                        dict_ref_tbl.text[i].flags = Common.FREE_STRING;//DONT_FREE_STRING;
                        //s = dict_ref_tbl.text[i].str;
                    }
                }
            }

            return Common.DDL_SUCCESS;
        }

        public unsafe static int eval_dir_domain_tbl(ref DDlDevDescription.DOMAIN_TBL domain_tbl, ref DDlDevDescription.BININFO bin)
        {

            //DOMAIN_TBL_ELEM* element = null;     /* temp pointer for the list */
            //DOMAIN_TBL_ELEM* end_element = null;/* end pointer for the list */
            UInt64 temp_int;   /* integer value */
            //int rc;          /* return code */

            //ASSERT_DBG(bin && bin.chunk && bin.size);
            fixed (byte* chu = &bin.chunk[bin.uoffset])
            {
                fixed (uint* puiSize = &bin.size)
                {
                    //byte* chu1 = chu;
                    //uint* puiS1 = puiSize;
                    //byte* chu2 = chu;
                    //uint* puiS2 = puiSize;

                    /* parse count */
                    Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        domain_tbl.count = 0;
                        domain_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    domain_tbl.count = (int)temp_int;

                    /* malloc the list */
                    domain_tbl.list = new DDlDevDescription.DOMAIN_TBL_ELEM[temp_int];

                    if (domain_tbl.list == null)
                    {

                        domain_tbl.count = 0;
                        return Common.DDL_MEMORY_ERROR;
                    }

                    /* load list with zeros */
                    //(void)memset((char*)domain_tbl.list, 0, (size_t)temp_int * sizeof(DOMAIN_TBL_ELEM));

                    /*
                     * load the list
                     */

                    //for (element = domain_tbl.list, end_element = element + temp_int; element < end_element; element++)
                    for(int i = 0; i < domain_tbl.count; i++)
                    {

                        /* parse ITEM_ID */

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);//??????
                        domain_tbl.list[i].item_id = (uint)temp_int;

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        domain_tbl.list[i].dd_ref/*.object_index*/ = (int)temp_int;
                    }
                }
            }
            return Common.DDL_SUCCESS;
        }

        public unsafe static int eval_dir_prog_tbl(ref DDlDevDescription.PROG_TBL prog_tbl, ref DDlDevDescription.BININFO bin)
        {

            //PROG_TBL_ELEM* element = null;   /* temp pointer for the list */
            //PROG_TBL_ELEM* end_element = null;/* end pointer for the list */
            UInt64 temp_int;   /* integer value */
            //int rc;          /* return code */

            //ASSERT_DBG(bin && bin.chunk && bin.size);

            /* parse count */
            fixed (byte* chu = &bin.chunk[bin.uoffset])
            {
                fixed (uint* puiSize = &bin.size)
                {
                    //byte* chu1 = chu;
                    //uint* puiS1 = puiSize;
                    //byte* chu2 = chu;
                    //uint* puiS2 = puiSize;//??????
                    Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        prog_tbl.count = 0;
                        prog_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    prog_tbl.count = (int)temp_int;

                    /* malloc the list */
                    prog_tbl.list = new DDlDevDescription.PROG_TBL_ELEM[temp_int];

                    if (prog_tbl.list == null)
                    {

                        prog_tbl.count = 0;
                        return Common.DDL_MEMORY_ERROR;
                    }

                    /* load list with zeros */
                    //(void)memset((char*)prog_tbl.list, 0, (size_t)temp_int * sizeof(PROG_TBL_ELEM));

                    /*
                     * load the list
                     */

                    //for (element = prog_tbl.list, end_element = element + temp_int; element < end_element; element++)
                    for (int i = 0; i < prog_tbl.count; i++)
                    {

                        /* parse ITEM_ID */

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        prog_tbl.list[i].item_id = (uint)temp_int;

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        prog_tbl.list[i].dd_ref = (int)temp_int;//??????                        element.dd_ref.object_index = (OBJECT_INDEX)temp_int;

                    }
                } 
            }
            return Common.DDL_SUCCESS;
        }

        public unsafe static int eval_dir_local_var_tbl(ref DDlDevDescription.LOCAL_VAR_TBL local_var_tbl, ref DDlDevDescription.BININFO bin)
        {

            //LOCAL_VAR_TBL_ELEM* element = null;   /* temp pointer for the list */
            //LOCAL_VAR_TBL_ELEM* end_element = null; /* end pointer for the list */
            UInt64 temp_int;    /* integer value */
            //int rc;           /* return code */


            fixed (byte* chu = &bin.chunk[bin.uoffset])
            {
                fixed (uint* puiSize = &bin.size)
                {            /* parse count */
                    Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);

                    /*
                     * if count is zero
                     */

                    if (temp_int == 0)
                    {

                        local_var_tbl.count = 0;
                        local_var_tbl.list = null;
                        return Common.DDL_SUCCESS;
                    }

                    local_var_tbl.count = (int)temp_int;

                    /* malloc the list */
                    local_var_tbl.list = new DDlDevDescription.LOCAL_VAR_TBL_ELEM[temp_int];

                    if (local_var_tbl.list == null)
                    {

                        local_var_tbl.count = 0;
                        return Common.DDL_MEMORY_ERROR;
                    }

                    /* load list with zeros */
                    //(void)memset((char*)local_var_tbl.list, 0, (size_t)temp_int * sizeof(LOCAL_VAR_TBL_ELEM));

                    /*
                     * load the list
                     */

                    //for (element = local_var_tbl.list, end_element = element + temp_int; element < end_element; element++)
                    for (int i = 0; i < local_var_tbl.count; i++)
                    {

                        /* parse ITEM_ID */

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        local_var_tbl.list[i].item_id = (uint)temp_int;

                        /* parse type */

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        local_var_tbl.list[i].type = (ushort)temp_int;

                        /* parse size */

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        local_var_tbl.list[i].size = (ushort)temp_int;

                        /* parse DD reference */

                        Common.DDL_PARSE_INTEGER(&chu, puiSize, &temp_int);
                        //element.dd_ref.object_index = (OBJECT_INDEX)temp_int;
                        local_var_tbl.list[i].dd_ref = (int)temp_int;

                    }
                }
            }
            return Common.DDL_SUCCESS;
        }

        public static unsafe int dir_mask_man(uint attr_mask, uint bin_exists, uint bin_hooked, ref uint attr_avail, EVALDIR_FN eval, ref DDlDevDescription.FLAT_DEVICE_DIR_6 device_dir, ref DDlDevDescription.BININFO bin)
        {

            int rc = Common.DDL_SUCCESS; /* return code */

            /*
             * No binary exists
             */

            if ((attr_mask & bin_exists) == 0)
            {

                /*
                 * This is a DDOD error, by definition all directory tables
                 * must have binary available
                 */

                rc = Common.DDL_BINARY_REQUIRED;
            }

            /*
             * No binary hooked
             */

            else if ((attr_mask & bin_hooked) == 0)
            {

                /*
                 * If value is already available
                 */

                if ((attr_mask & attr_avail) != 0)
                {

                    rc = Common.DDL_SUCCESS;
                }

                /*
                 * error, binary should be hooked up
                 */

                else
                {

                    rc = Common.DDL_BINARY_REQUIRED;
                }
            }

            else
            {

                /*
                 * check masks for evaluating
                 */

                if ((attr_mask & attr_avail) == 0)
                {

                    //rc = (*eval)(attribute, bin);
                    switch (eval)
                    {
                        case EVALDIR_FN.eval_dir_dict_ref_tbl:
                            rc = eval_dir_dict_ref_tbl(ref device_dir.dict_ref_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_item_tbl:
                            rc = eval_dir_item_tbl(ref device_dir.item_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_string_tbl:
                            rc = eval_dir_string_tbl(ref device_dir.string_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_image_tbl:
                            rc = eval_dir_image_tbl(ref device_dir.image_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_cmd_num_id_tbl:
                            rc = eval_dir_cmd_num_id_tbl(ref device_dir.cmd_num_id_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_dict_ref_tbl8:
                            rc = eval_dir_dict_ref_tbl8(ref device_dir.dict_ref_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_domain_tbl:
                            rc = eval_dir_domain_tbl(ref device_dir.domain_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_prog_tbl:
                            rc = eval_dir_prog_tbl(ref device_dir.prog_tbl, ref bin);
                            break;

                        case EVALDIR_FN.eval_dir_local_var_tbl:
                            rc = eval_dir_local_var_tbl(ref device_dir.local_var_tbl, ref bin);
                            break;

                        default:
                            rc = Common.DDL_ENCODING_ERROR;
                            break;
                    }

                    if (rc == Common.DDL_SUCCESS)
                    {

                        attr_avail |= attr_mask;
                    }
                }

                /*
                 * evaluation is not necessary
                 */

                else
                {

                    rc = Common.DDL_SUCCESS;
                }
            }

            return rc;
        }


    }
}
