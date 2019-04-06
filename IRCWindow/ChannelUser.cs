using System;
using System.Collections.Generic;
using System.Text;
using IRCProviders;
using System.Windows.Forms;
using IRCWindow.Properties;

namespace IRCWindow
{
    [Serializable]
    public sealed class ChannelUser: IChannelUser, IComparable<ChannelUser>
    {
        private Dictionary<char, bool> modes = new Dictionary<char, bool>();

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

        public string VisibleNick
        {
            get { return this.ToString(); }
        }

        public ChannelUser()
        {

        }

        public ChannelUser(string nickName)
        {
            int i = 0;
            while (MDIChildServer.ModesTable2.ContainsValue(nickName[i]))
            {
                modes[nickName[i]] = true;
                i++;
            }
            this.NickName = nickName.Substring(i);
        }

        public void SetMode(char c, bool set)
        {
            modes[c] = set;
        }

        public override string ToString()
        {
            foreach (char mode in MDIChildServer.ModesTable2.Values)
                if (modes.ContainsKey(mode) && modes[mode])
                {
                    return mode + this.NickName;
                }
            return this.NickName;
        }

        #region IComparable<ChannelUser> Members

        public int CompareTo(ChannelUser other)
        {
            int comp = this.Priority.CompareTo(other.Priority);
            if (comp != 0)
                return comp;
            return this.NickName.CompareTo(other.NickName);
        }

        #endregion

        #region IChannelUser Members

        public string NickName { get; set; }

        public Dictionary<char, bool> Modes
        {
            get { return modes; }
        }

        #endregion
    }
}
