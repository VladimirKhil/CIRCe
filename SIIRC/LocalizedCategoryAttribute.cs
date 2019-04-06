using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SIIRC.Properties;

namespace SIIRC
{
    /// <summary>
    /// Локализованные категории
    /// </summary>
    internal enum LocalizedCategories
    {
        LCCommon,
        LCSpecial,
        LCTopic,
        LCChat,
        LCInput,
        LCUsersList,
        LCWindowsPanel
    }

    internal sealed class LocalizedCategoryAttribute: CategoryAttribute
    {
        public LocalizedCategoryAttribute()
            : base()
        { 
        }

        public LocalizedCategoryAttribute(LocalizedCategories category)
            : base(category.ToString())
        {

        }

        protected override string GetLocalizedString(string value)
        {
            return Resources.ResourceManager.GetString(value) ?? base.GetLocalizedString(value);
        }
    }
}
