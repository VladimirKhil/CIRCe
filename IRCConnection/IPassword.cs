using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace IRCConnection
{
    public interface IPassword
    {
        string Nick { get; set; }
        string Pass { get; set; }
    }
}
