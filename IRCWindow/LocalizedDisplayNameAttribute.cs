using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IRCWindow.Properties;

namespace IRCWindow
{
    /// <summary>
    /// Локализованное отображаемое имя
    /// </summary>
    class LocalizedDisplayNameAttribute: DisplayNameAttribute
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
