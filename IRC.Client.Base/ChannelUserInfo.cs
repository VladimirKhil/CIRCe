using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Информация о пользователе на канале
    /// </summary>
    [Serializable]
    public sealed class ChannelUserInfo
    {
        /// <summary>
        /// Ник пользователя
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// Режимы пользователя
        /// </summary>
        public ChannelUserModes Modes { get; set; }

        public void SetMode(string name, ChannelUserModes mode, bool set)
        {
            if (set)
                this.Modes = this.Modes | mode;
            else
                this.Modes = this.Modes & ~mode;

            OnSetMode(name, mode, set);
        }

        private void OnSetMode(string name, ChannelUserModes mode, bool set)
        {
            
        }
    }
}
