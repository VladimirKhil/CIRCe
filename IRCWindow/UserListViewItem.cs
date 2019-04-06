using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCConnection;
using IRCWindow.Properties;

namespace IRCWindow
{
    public class UserListViewItem: ListViewItem, IChannelUser, IComparable<UserListViewItem>
    {
        private string nickName;
        private Dictionary<char, bool> modes = new Dictionary<char, bool>();

        /// <summary>
        /// Ник
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; SetText(); }
        }

        private int Priority
        {
            get
            {
                int i = 0;
                foreach (char mode in MDIChildServer.ModesTable2.Values)
                {
                    if (modes.ContainsKey(mode) && modes[mode])
                        return i;
                    i++;
                }
                return i;
            }
        }

        public UserListViewItem(string nickName, int imageIndex)
            : base(nickName, imageIndex)
        {
            int i = 0;
            while (MDIChildServer.ModesTable2.ContainsValue(nickName[i]))
            {
                modes[nickName[i]] = true;
                i++;
            }
            this.nickName = nickName.Substring(i);
            SetText();
        }

        public void SetMode(char c, bool set)
        {
            modes[c] = set;
            SetText();
        }

        void SetText()
        {
            foreach (char mode in MDIChildServer.ModesTable2.Values)
                if (modes.ContainsKey(mode) && modes[mode])
                {
                    Text = mode + nickName;
                    return;
                }
            Text = nickName;
        }

        #region IComparable<UserListViewItem> Members

        public int CompareTo(UserListViewItem other)
        {
            int comp = this.Priority.CompareTo(other.Priority);
            if (comp != 0)
                return comp;
            return this.Text.CompareTo(other.Text);
        }

        #endregion

        #region IChannelUser Members

        public Dictionary<char, bool> Modes
        {
            get { return modes; }
        }

        #endregion
    }
}
