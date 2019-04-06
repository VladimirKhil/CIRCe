using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using IRCProviders;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCeChannelUserItem : InfiniteMarshalByRefObject, ICIRCeChannelUserItem
    {
        internal IDataRow DataRow { get; set; }

        public System.Drawing.Color BackColor
        {
            get
            {
                return this.DataRow.BackColor;
            }
            set
            {
                this.DataRow.BackColor = value;
            }
        }
    }
}
