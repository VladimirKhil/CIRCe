using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRCProviders
{
    internal class ColorsTypeConverter: TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                try
                {
                    var colors = new Colors();
                    colors.Load(new StringBuilder(s));
                    return colors;
                }
                catch (Exception) { return null; }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var colors = value as Colors;
                var str = new StringBuilder();
                colors.Save(str);
                return str.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
