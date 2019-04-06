using System;
using System.Collections.Generic;
using System.Text;

namespace IRCWindow
{
    public sealed class ColorSelectedEventArgs: EventArgs
    {
        int choosenColor = -1;

        public int ChoosenColor
        {
            get { return choosenColor; }
        }

        public ColorSelectedEventArgs(int choosenColor)
        {
            this.choosenColor = choosenColor;
        }
    }
}
