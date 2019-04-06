using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IRCWindow.Properties;
using IRCProviders;
using System.Resources;

namespace IRCWindow
{
    public class EnumTypeConverter: EnumConverter
    {
        private Type type = null;
        
        public EnumTypeConverter(Type type): base(type)
        {
            this.type = type;
        }
        
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            foreach (var item in type.GetFields())
            {
                var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute));
                if (descriptionAttribute != null)
                {
                    var str = Resources.ResourceManager.GetString(descriptionAttribute.Description);
                    if (str == (string)value)
                    {
                        return Enum.Parse(type, item.Name);
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertFrom(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var item = type.GetField(Enum.GetName(type, value));
            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute));
            if (descriptionAttribute != null)
            {
                var str = Resources.ResourceManager.GetString(descriptionAttribute.Description);
                if (str != null)
                {
                    return str;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
