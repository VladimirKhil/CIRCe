using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IRCConnection
{
    [Serializable]
    public class IRCObject: IIRCObject
    {
        [OptionalField]
        private bool sticked = false;
        [OptionalField]
        private bool autoOpen = false;

        /// <summary>
        /// Закрепить в дереве
        /// </summary>
        public bool Sticked
        {
            get { return this.sticked; }
            set
            {
                this.sticked = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Sticked"));
            }
        }

        /// <summary>
        /// Автоматическое открытие окна при входе
        /// </summary>
        public bool AutoOpen
        {
            get { return this.autoOpen; }
            set
            {
                this.autoOpen = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AutoOpen"));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
