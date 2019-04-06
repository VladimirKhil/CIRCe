using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRCWindow
{
    public class ProgramOptionsConverter: TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(context.Instance).Sort(new string[] { "Proxy", "PlayMusic", "ShowUrlOnCmd", "WaitServer", "PingServer" });
        }
    }
}
