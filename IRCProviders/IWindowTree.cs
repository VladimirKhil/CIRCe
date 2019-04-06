using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IRCProviders
{
    /// <summary>
    /// Дерево окон в программе
    /// </summary>
    public interface IWindowTree
    {
        /// <summary>
        /// Цвет фона
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Цвет фона узла (левый)
        /// </summary>
        Color NodeBackColorLeft { get; set; }

        /// <summary>
        /// Цвет фона узла (правый)
        /// </summary>
        Color NodeBackColorRight { get; set; }

        /// <summary>
        /// Цвет фона выделенного узла (левый)
        /// </summary>
        Color SelectedNodeBackColorLeft { get; set; }

        /// <summary>
        /// Цвет фона выделенного узла (правый)
        /// </summary>
        Color SelectedNodeColorRight { get; set; }
    }
}
