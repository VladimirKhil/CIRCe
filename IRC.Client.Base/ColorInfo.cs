using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Информация о цвете
    /// </summary>
    [Serializable]
    public struct ColorInfo
    {
        /// <summary>
        /// Код цвета текста
        /// </summary>
        public int ForegroundColorCode;
        /// <summary>
        /// Код цвета фона
        /// </summary>
        public int BackgroundColorCode;

        public ColorInfo(int x, int y)
        {
            this.ForegroundColorCode = x;
            this.BackgroundColorCode = y;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.ForegroundColorCode, this.BackgroundColorCode);
        }
    }
}
