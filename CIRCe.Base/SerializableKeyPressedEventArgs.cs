using System;
using System.Collections.Generic;
using System.Text;

namespace CIRCe.Base
{
    [Serializable]
    public sealed class SerializableKeyPressedEventArgs: EventArgs
    {
        public char KeyChar { get; set; }
        public bool Handled { get; set; }
    }
}
