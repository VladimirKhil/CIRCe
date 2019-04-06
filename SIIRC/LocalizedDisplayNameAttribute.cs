using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SIIRC.Properties;

namespace SIIRC
{
    /// <summary>
    /// Локализованное отображаемое имя
    /// </summary>
    internal sealed class LocalizedDisplayNameAttribute: DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute()
        {

        }

        public LocalizedDisplayNameAttribute(string displayName)
            : base(displayName)
	    {

        }

        public override string DisplayName
        {
            get
            {
                return Resources.ResourceManager.GetString(base.DisplayName) ?? base.DisplayName;
            }
        }
    }
}
