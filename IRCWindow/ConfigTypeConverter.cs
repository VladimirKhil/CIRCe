using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IRCWindow.Properties;

namespace IRCWindow
{
    class ConfigTypeConverter: TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return Resources.Configure + "...";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
