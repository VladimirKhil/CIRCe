using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using System.Windows.Forms;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    internal class CIRCeCommand : InfiniteMarshalByRefObject, ICommand
    {
        internal ToolStripItem Item { get; set; }
    }
}
