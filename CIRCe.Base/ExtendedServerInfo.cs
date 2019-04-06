using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// Расширенная информация о сервере (содержит данные, имеющие отношение только к Цирцее)
    /// </summary>
    public sealed class ExtendedServerInfo: AppItemInfo, IEquatable<ExtendedServerInfo>
    {
        private string description;

        /// <summary>
        /// Базовая информация о сервере
        /// </summary>
        public ServerInfo Data { get; set; }

        /// <summary>
        /// Описание сервера
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set
            {
                if (this.description != value)
                {
                    this.description = value;

                    if (string.IsNullOrEmpty(value))
                        this.errors["Description"] = "Описание сервера не может быть пустым";
                    else
                        this.errors["Description"] = null;

                    OnPropertiesChanged("Description", "Header", "Error");
                }
            }
        }

        /// <summary>
        /// Каналы сервера
        /// </summary>
        public List<ExtendedChannelInfo> Channels { get; set; }

        /// <summary>
        /// Сохранённые пароли для ников
        /// </summary>
        public PasswordInfos Passwords { get; set; }

        /// <summary>
        /// Имя сервера
        /// </summary>
        public string Name
        { 
            get
            {
                return this.Data.Name;
            }
            set
            {
                if (this.Data.Name != value)
                {
                    this.Data.Name = value;

                    if (string.IsNullOrEmpty(value))
                        this.errors["Name"] = "Имя сервера не может быть пустым";
                    else
                        this.errors["Name"] = null;

                    OnPropertiesChanged("Name", "Header", "Error");
                }
            }
        }

        /// <summary>
        /// Порт сервера
        /// </summary>
        public int Port 
        { 
            get
            {
                return this.Data.Port;
            }
            set
            {
                if (this.Data.Port != value)
                {
                    this.Data.Port = value;

                    if (this.Port < 1 || this.Data.Port > 100000)
                        this.errors["Port"] = "Порт сервера должен быть в предеелах от 1 до 100000";
                    else
                        this.errors["Port"] = null;

                    OnPropertiesChanged("Port", "Header", "Error");
                }
            }
        }

        public override string Header
        {
            get
            {
                if (this.Description != null)
                    return this.Description;

                return string.Format("{0}:{1}", this.Data.Name, this.Data.Port);
            }
        }

        public ExtendedServerInfo()
        {
            this.Data = new ServerInfo();
            this.Channels = new List<ExtendedChannelInfo>();
            this.Passwords = new PasswordInfos();

            this.errors["Name"] = "Имя сервера не может быть пустым";
        }

        /// <summary>
        /// Создание сервера
        /// </summary>
        /// <param name="description">Описание</param>
        /// <param name="name">Имя</param>
        /// <param name="port">Номер порта</param>
        public ExtendedServerInfo(string description, string name, int port = 6667) : this()
        {
            this.Description = description;
            this.Name = name;
            this.Port = port;
        }

        public override object Clone()
        {
            return new ExtendedServerInfo(this.Description, this.Data.Name, this.Data.Port);
        }

        public bool Equals(ExtendedServerInfo other)
        {
            return this.Data.Equals(other.Data);
        }
    }
}
