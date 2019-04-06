using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIRCe.Base
{
    public sealed class PasswordInfos
    {
        public Dictionary<string, string> Data { get; set; }

        public PasswordInfos()
        {
            this.Data = new Dictionary<string, string>();
        }
    }
}
