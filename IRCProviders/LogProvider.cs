using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Класс, отвечающий за запись логов канала/привата
    /// </summary>
    public class LogProvider
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public string OpenB { get; set; }
        public string CloseB { get; set; }
        public string OpenU { get; set; }
        public string CloseU { get; set; }
        public string OpenK { get; set; }
        public string CloseK { get; set; }
        public string OpenL { get; set; }
        public string CloseL { get; set; }
        public string Bottom { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
