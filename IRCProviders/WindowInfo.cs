using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IRCProviders
{
    /// <summary>
    /// Информация о расположении и размере окна
    /// </summary>
    public sealed class WindowInfo
    {
        /// <summary>
        /// Идентификатор окна
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Позиция окна
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Размер окна
        /// </summary>
        public Size Size { get; set; }
    }
}
