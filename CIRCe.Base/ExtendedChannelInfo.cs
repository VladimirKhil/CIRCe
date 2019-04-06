using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIRCe.Base
{
    /// <summary>
    /// Информация о IRC-канале
    /// </summary>
    public sealed class ExtendedChannelInfo: AppItemInfo, ICloneable
    {
        private string name;

        /// <summary>
        /// Имя канала
        /// </summary>
        public string Name 
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;

                    if (string.IsNullOrEmpty(value))
                        this.errors["Name"] = "Имя канала не может быть пустым";
                    else if (!value.StartsWith("#") && !value.StartsWith("&"))
                        this.errors["Name"] = "Имя канала должно начинаться с символов # или &";
                    else if (value.Length == 1)
                        this.errors["Name"] = "Имя канала не может быть пустым";
                    else
                        this.errors["Name"] = null;

                    OnPropertiesChanged("Name", "Header", "Error");
                }
            }
        }

        public ExtendedChannelInfo()
        {
            this.name = "#";
            this.errors["Name"] = "Имя канала не может быть пустым";
        }

        public ExtendedChannelInfo(string name)
        {
            this.name = name;
        }

        public override string Header
        {
            get { return this.name; }
        }

        public override object Clone()
        {
            return new ExtendedChannelInfo(this.name) { AutoOpen = this.AutoOpen, Sticked = this.Sticked };
        }
    }
}
