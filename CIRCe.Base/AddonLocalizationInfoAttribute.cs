using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace CIRCe.Base
{
    /// <summary>
    /// Информация о дополнении, относящаяся к конкретной культуре
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public sealed class AddonLocalizationInfoAttribute : Attribute
    {
        public string Culture { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
