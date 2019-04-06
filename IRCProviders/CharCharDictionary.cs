using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Configuration;

namespace IRCProviders
{
    /// <summary>
    /// Словарь соответствий между символами
    /// </summary>
    [TypeConverter(typeof(CharCharDictionaryTypeConverter))]
    public class CharCharDictionary : Dictionary<char, char>
    {

    }
}
