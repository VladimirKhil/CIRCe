using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    [Serializable]
    public class SerializableKeyPressedEventArgs: EventArgs
    {
        public char KeyChar { get; set; }
        public bool Handled { get; set; }
    }
}
