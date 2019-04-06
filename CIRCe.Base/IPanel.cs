using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIRCe.Base
{
    public interface IPanel
    {
        void AddBottom(IntPtr handle);
        void RemoveBottom(IntPtr handle);
    }
}
