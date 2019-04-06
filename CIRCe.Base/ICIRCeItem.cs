using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;

namespace CIRCe.Base
{
    /// <summary>
    /// Произвольное окно Цирцеи
    /// </summary>
    public interface ICIRCeItem: IItem
    {
        event Action Closed;
        void Close();
        void Activate();
    }
}
