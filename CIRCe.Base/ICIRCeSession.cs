using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;

namespace CIRCe.Base
{
    public interface ICIRCeSession: ISession, ICIRCeAppItem
    {
        /// <summary>
        /// Сервер, владеющий данной сесией
        /// </summary>
        new ICIRCeServer OwnerServer { get; }
    }
}
