using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Режим канала
    /// </summary>
    internal class ChannelMode
    {
        private char letter = ' ';
        private bool set = false;
        /// <summary>
        /// Пункт меню, изменяющий данный режим
        /// </summary>
        private ToolStripMenuItem setter = null;
        /// <summary>
        /// Параметры режима (например, пароль для запароленного канала)
        /// </summary>
        private List<object> param = new List<object>();

        /// <summary>
        /// Буква, обозначающая режим (например, m для модерируемого)
        /// </summary>
        internal char Letter
        {
            get { return letter; }
        }

        /// <summary>
        /// Установлен ли режим в настоящий момент
        /// </summary>
        internal bool Set
        {
            get { return set; }
            set { set = value; if (setter != null) setter.Checked = value; }
        }

        internal ChannelMode(char letter, string description, EventHandler onClick, ToolStripMenuItem parent)
        {
            this.letter = letter;
            if (onClick != null)
            {
                this.setter = new ToolStripMenuItem(description, null, onClick);
                this.setter.Tag = this;
                this.setter.ToolTipText = letter.ToString();
                if (parent != null)
                    parent.DropDownItems.Add(this.setter);
            }
        }
    }
}
