using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace IRCProviders
{
    /// <summary>
    /// Контекстное меню
    /// </summary>
    public interface IContextMenuStrip
    {
        /// <summary>
        /// Добавить пункт меню
        /// </summary>
        /// <param name="item">Добавляемый пункт меню</param>
        void Add(ToolStripItem item);
        /// <summary>
        /// Получить номер позиции пункта меню в данном меню
        /// </summary>
        /// <param name="item">Пункт меню</param>
        /// <returns>Позиция переданного пункта</returns>
        int IndexOf(ToolStripItem item);
        /// <summary>
        /// Удалить пункт меню
        /// </summary>
        /// <param name="item">Удаляемый пункт</param>
        void Remove(ToolStripItem item);
        /// <summary>
        /// Удалить пункт меню по индексу
        /// </summary>
        /// <param name="index">Индекс пункта меню</param>
        void RemoveAt(int index);
        /// <summary>
        /// Возникает перед открытием меню
        /// </summary>
        event CancelEventHandler Opening;
    }
}
