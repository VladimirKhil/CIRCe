using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Параметры мигания
    /// </summary>
    public class FlashParams: ICloneable
    {
        public FlashMode FlashingMode { get; set; }
        public bool FlashOnNick { get; set; }
        public bool FlashOnPrivate { get; set; }
        public bool UseBlackList { get; set; }
        public bool UseWhiteList { get; set; }
        public string BlackList { get; set; }
        public string WhiteList { get; set; }

        public FlashParams()
        {
            this.FlashingMode = FlashMode.Meduim;
            this.FlashOnNick = this.FlashOnPrivate = true;
            this.UseBlackList = this.UseWhiteList = false;
            this.WhiteList = this.BlackList = string.Empty;
        }

        #region ICloneable Members

        public object Clone()
        {
            var clone = new FlashParams();
            clone.BlackList = this.BlackList;
            clone.FlashingMode = this.FlashingMode;
            clone.FlashOnNick = this.FlashOnNick;
            clone.FlashOnPrivate = this.FlashOnPrivate;
            clone.UseBlackList = this.UseBlackList;
            clone.UseWhiteList = this.UseWhiteList;
            clone.WhiteList = this.WhiteList;

            return clone;
        }

        #endregion
    }
}
