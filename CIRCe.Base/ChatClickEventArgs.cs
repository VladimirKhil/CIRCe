using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIRCe.Base
{
    [Serializable]
    public sealed class ChatClickEventArgs: EventArgs
    {
        public string Line { get; set; }
        public int Position { get; set; }
    }
}
