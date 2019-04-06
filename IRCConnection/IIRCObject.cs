using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRCConnection
{
    public interface IIRCObject : INotifyPropertyChanged
    {
        bool Sticked { get; set; }
        bool AutoOpen { get; set; }
    }
}
