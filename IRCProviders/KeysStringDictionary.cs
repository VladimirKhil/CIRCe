using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace IRCProviders
{
    /// <summary>
    /// Словарь кнопок и строк
    /// </summary>
    [TypeConverter(typeof(KeysStringDictionaryTypeConverter))]
    public sealed class KeysStringDictionary: Dictionary<Keys, string>
    {
        public KeysStringDictionary()
        {

        }
    }
}
