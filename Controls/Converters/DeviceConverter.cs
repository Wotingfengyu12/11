using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQC.Controls.Converters
{
    public class DeviceConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }
    }
}
