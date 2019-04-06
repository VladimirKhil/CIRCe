using System;
using System.Collections.Generic;
using System.Text;

namespace CIRCe.Base
{
    [Serializable]
    public sealed class SerializableKeyEventArgs: EventArgs
    {
        public bool Alt { get; set; }
        public bool Control { get; set; }
        public bool Shift { get; set; }
        public Keys KeyCode { get; set; }
        public Keys KeyData { get; set; }
        public int KeyValue { get; set; }
        public Keys Modifiers { get; set; }
        public bool SuppressKeyPress { get; set; }
        public bool Handled { get; set; }
    }
}
