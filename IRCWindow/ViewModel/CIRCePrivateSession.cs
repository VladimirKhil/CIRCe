using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCePrivateSession: CIRCeSession, ICIRCePrivateSession
    {
        private MDIChildPrivate privateSession = null;

        public CIRCePrivateSession(MDIChildPrivate privateSession)
            : base(privateSession)
        {
            this.privateSession = privateSession;
        }
    }
}
