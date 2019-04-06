using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using IRCWindow.Properties;

namespace IRCWindow
{
    public class ProxyTypeConverter: TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (!Settings.Default.UseProxy)
                    return Resources.NotUsed;
                var uri = value as Uri;
                return uri != null ? uri.ToString() : Resources.NotUsed;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
