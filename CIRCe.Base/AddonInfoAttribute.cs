using System;
using System.Collections.Generic;
using System.Text;

namespace CIRCe.Base
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public sealed class AddonInfoAttribute: Attribute
    {
        public string AddonType { get; set; }
        public AddonStartMode StartMode { get; set; }
        public bool VisibleInMenu { get; set; }

        public AddonInfoAttribute()
        {
            this.StartMode = AddonStartMode.Manual;
            this.VisibleInMenu = true;
        }
    }
}
