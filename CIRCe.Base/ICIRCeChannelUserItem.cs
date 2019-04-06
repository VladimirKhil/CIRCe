using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using System.Drawing;

namespace CIRCe.Base
{
    /// <summary>
    /// Интерфейс пользователя на канале
    /// </summary>
    public interface ICIRCeChannelUserItem
    {
        Color BackColor { get; set; }
    }
}
