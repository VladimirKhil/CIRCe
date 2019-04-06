using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;

namespace IRCProviders
{
    internal class CharCharDictionaryTypeConverter: TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string s = value as string;
            if (s != null)
            {
                try
                {
                    CharCharDictionary dict = new CharCharDictionary();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(s);
                    foreach (XmlNode node in doc["table"].ChildNodes)
                    {
                        dict[char.Parse(node["key"].InnerText)] = char.Parse(node["value"].InnerText);
                    }
                    return dict;
                }
                catch (Exception) { return null; }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                CharCharDictionary dict = value as CharCharDictionary;
                StringBuilder str = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(str);
                writer.WriteStartElement("table");
                foreach (KeyValuePair<char, char> pair in dict)
                {
                    writer.WriteStartElement("item");

                    writer.WriteStartElement("key");
                    writer.WriteValue(pair.Key.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("value");
                    writer.WriteValue(pair.Value.ToString());
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
                return str.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
