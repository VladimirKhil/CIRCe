using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Serializable]
    public sealed class UserInfo: ICloneable
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Реальное имя
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// EMail
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// Дополнительные сведения
        /// </summary>
        public string Info { get; set; }

        public UserInfo()
        {

        }

        public override string ToString()
        {
            return this.UserName;
        }

        #region Члены ICloneable

        public object Clone()
        {
            return new UserInfo { RealName = this.RealName, UserName = this.UserName, EMail = this.EMail, Info = this.Info };
        }

        #endregion
    }
}
