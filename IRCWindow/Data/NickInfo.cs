using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;

namespace IRCWindow.Data
{
    /// <summary>
    /// Информация о нике
    /// </summary>
    public sealed class NickInfo: ViewModelBase
    {
        private string name;

        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;

                    if (string.IsNullOrEmpty(value))
                        this.errors["Name"] = "Имя пользователя не может быть пустым";
                    else
                        this.errors["Name"] = null;

                    OnPropertiesChanged("Name", "Header", "Error");
                }
            }
        }

        public string Header { get { return this.name; } }

        public NickInfo()
        {
            this.errors["Name"] = "Имя пользователя не может быть пустым";
        }

        public override string ToString()
        {
            return this.name;
        }

        public override object Clone()
        {
            return new NickInfo { Name = this.name };
        }
    }
}
