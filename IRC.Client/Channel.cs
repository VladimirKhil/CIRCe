using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;

namespace IRC.Client
{
    internal sealed class Channel: Session, IChannel
    {
        private Changeable<ChannelUserInfo> persons = new Changeable<ChannelUserInfo>();
        private ChannelModes modes = ChannelModes.None;

        private string password = null;
        private int limit = 0;

        public ChannelModes Modes
        {
            get { return this.modes; }
        }

        public IChangeable<ChannelUserInfo> Users
        {
            get { return this.persons; }
        }

        public Channel(Server server, string name)
            : base(server, name)
        {

        }

        internal void AddPerson(string name)
        {
            foreach (var item in this.persons)
            {
                if (item.NickName == name)
                    return;
            }

            this.persons.Add(new ChannelUserInfo { NickName = name });
            OnJoined(name);
        }

        internal void KickPerson(string name, string kicker, string words)
        {
            for (int i = 0; i < this.persons.Count; i++)
            {
                if (this.persons[i].NickName == name)
                {
                    this.persons.RemoveAt(i);
                    OnKicked(name, kicker, words);
                    break;
                }
            }
        }

        private void OnKicked(string name, string kicker, string words)
        {
            throw new NotImplementedException();
        }

        private void OnJoined(string name)
        {
            throw new NotImplementedException();
        }

        internal void SetMode(string name, string mode, params string[] param)
        {
            if (mode.Length < 2)
                return;
            bool add = mode[0] == '+';
            int paramInd = 0;
            for (int i = 1; i < mode.Length; i++)
            {
                if (mode[i] == '+')
                {
                    add = true;
                    continue;
                }
                else if (mode[i] == '-')
                {
                    add = false;
                    continue;
                }

                switch (mode[i])
                {
                    case 'b':
                        break;

                    case 'e':
                        break;

                    case 'I':
                        break;

                    case 'h':
                        goto case 'v';

                    case 'k':
                        if (paramInd < param.Length)
                            password = param[paramInd++];
                        goto default;

                    case 'l':
                        if (add && paramInd < param.Length)
                            limit = int.Parse(param[paramInd++]);
                        goto default;

                    case 'o':
                        goto case 'v';

                    case 'v':
                        {
                            var person = param[paramInd++];
                            foreach (var item in this.persons)
                            {
                                if (item.NickName == person)
                                {
                                    item.SetMode(name, Application.ChannelUserModeByChar(person[i]), add);
                                }
                            }
                        }
                        break;

                    default:
                        {
                            var person = param[paramInd++];
                            var m = Application.ChannelModeByChar(person[i]);
                            if (m == ChannelModes.None)
                            {
                                int maxValue = 1;
                                foreach (var item in Application.ChannelModesTable)
                                {
                                    maxValue = Math.Max(maxValue, (int)item.Key);
                                }
                                m = (ChannelModes)(maxValue * 2);
                                Application.ChannelModesTable[m] = person[i];
                            }
                            SetChannelMode(name, m, add);
                        }
                        break;
                }
            }
        }

        private void SetChannelMode(string name, ChannelModes mode, bool set)
        {
            if (set)
                this.modes = this.modes | mode;
            else
                this.modes = this.modes & ~mode;
            OnSetMode(name, mode, set);
        }

        private void OnSetMode(string name, ChannelModes mode, bool set)
        {
            throw new NotImplementedException();
        }
    }
}
