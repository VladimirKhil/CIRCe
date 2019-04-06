using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace IRCProviders
{
    /// <summary>
    /// Список из элементов
    /// </summary>
    public interface IListView
    {
        /// <summary>
        /// Массив элементов
        /// </summary>
        IChannelUser[] LVItems { get; }

        /// <summary>
        /// Массив выбранных элементов
        /// </summary>
        IChannelUser[] LVSelectedItems { get; }

        /// <summary>
        /// Контекстное меню
        /// </summary>
        IContextMenuStrip LVContextMenu { get; }

        /// <summary>
        /// Строка пользователя
        /// </summary>
        /// <param name="nickName">Имя пользователя</param>
        /// <returns>Строка пользователя</returns>
        IDataRow this[string nickName] { get; }
    }
}
