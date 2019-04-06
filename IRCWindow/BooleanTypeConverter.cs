using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IRCWindow.Properties;

namespace IRCWindow
{
    public class BooleanTypeConverter: BooleanConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string s = value as string;
                if (s == Resources.Yes)
                    return true;
                else if (s == Resources.No)
                    return false;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                bool obj = (bool)value;
                return obj ? Resources.Yes : Resources.No;
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
