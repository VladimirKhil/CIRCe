using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// Информация об объекте Цирцеи
    /// </summary>
    public abstract class AppItemInfo: ViewModelBase
    {
        private bool sticked = false;

        /// <summary>
        /// Закреплена ли ссылка в дерева
        /// </summary>
        [DefaultValue(false)]
        public bool Sticked
        {
            get { return this.sticked; }
            set
            {
                if (this.sticked != value)
                {
                    this.sticked = value;
                    OnPropertyChanged("Sticked");
                }
            }
        }

        /// <summary>
        /// Подключаться автоматически
        /// </summary>
        [DefaultValue(false)]
        public bool AutoOpen { get; set; }

        /// <summary>
        /// Отображаемое имя объекта
        /// </summary>
        public abstract string Header { get; }

        public override string ToString()
        {
            return this.Header;
        }
    }
}
