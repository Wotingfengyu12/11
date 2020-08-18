using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    public class PackData: List<byte>
    {
        public void AddInt(int input, int len = 4)
        {
            byte[] source_ptr = BitConverter.GetBytes(input);
            Add(source_ptr[2]);
            Add(source_ptr[1]);
            Add(source_ptr[0]);
            if (len == 4)
            {
                Add(source_ptr[3]);
            }

        }

        public void AddShort(short input)
        {
            byte[] source_ptr = BitConverter.GetBytes(input);
            Add(source_ptr[1]);
            Add(source_ptr[0]);
        }

        public void AddFloat(float input)
        {
            byte[] source_ptr = BitConverter.GetBytes(input);
            Add(source_ptr[3]);
            Add(source_ptr[2]);
            Add(source_ptr[1]);
            Add(source_ptr[0]);
        }
    }

}
