using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRCProviders
{
    /// <summary>
    /// Информация об аддоне
    /// </summary>
    public class AddonInfo
    {
        private string path = string.Empty;
        private string type = null;
        private string name = string.Empty;
        private string author = string.Empty;
        private string description = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Режим запуска аддона
        /// </summary>
        public enum StartMode
        {
            /// <summary>
            /// Отключён
            /// </summary>
            None,
            /// <summary>
            /// Автоматический
            /// </summary>
            Automatic,
            /// <summary>
            /// Ручной
            /// </summary>
            Manual
        };

        private StartMode startMode = StartMode.Manual;

        private bool visible = true;

        /// <summary>
        /// Путь к аддону
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// Расположение манифеста
        /// </summary>
        public string ManifestPath { get; set; }
        
        /// <summary>
        /// Имя аддона
        /// </summary>
        public string AddonType
        {
            get { return type; }
            set { type = value; }
        }
        
        /// <summary>
        /// Режим запуска
        /// </summary>
        public StartMode Mode
        {
            get { return startMode; }
            set { startMode = value; }
        }
        
        /// <summary>
        /// Видимый ли в главном меню
        /// </summary>
        /// <remarks>Имеет смысл только при Mode = StartMode.Manual</remarks>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// Уникальный идентификатор аддона
        /// </summary>
        public string Guid { get; set; }

        public AddonInfo()
        {
            this.ManifestPath = string.Empty;
        }
    }
}
