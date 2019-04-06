using System;
using System.Collections.Generic;
using System.Text;

namespace IRCWindow
{
    /// <summary>
    /// Аргументы инсталляции нового элемента
    /// </summary>
    public class InstallEventArgs: EventArgs
    {
        private string guid = string.Empty;

        public string Guid { get { return this.guid; } }
        public bool Install { get; set; }

        public InstallEventArgs()
        {
            
        }

        public InstallEventArgs(string guid)
        {
            this.guid = guid;
        }
    }
}
