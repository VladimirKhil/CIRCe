using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SIIRC.Properties;

namespace SIIRC
{
    internal class LocalizedDescriptionAttribute: DescriptionAttribute
    {
        public LocalizedDescriptionAttribute()
        {

        }

        public LocalizedDescriptionAttribute(string description)
            : base(description)
        {

        }

        public override string Description
        {
            get
            {
                return Resources.ResourceManager.GetString(base.Description) ?? base.Description;
            }
        }
    }
}
