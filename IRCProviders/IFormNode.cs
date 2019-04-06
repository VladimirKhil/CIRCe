using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IRCProviders
{
    /// <summary>
    /// Узел в дереве, представляющий форму
    /// </summary>
    public interface IFormNode
    {
        /// <summary>
        /// Цвет текста узла
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Картинка узла
        /// </summary>
        Image Image { get; set; }
    }
}
