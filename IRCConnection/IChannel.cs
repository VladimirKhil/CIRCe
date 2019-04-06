using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    public interface IChannel:IIRCObject
    {
        /// <summary>
        /// Имя канала
        /// </summary>
        string Name { get; set; }
    }
}
