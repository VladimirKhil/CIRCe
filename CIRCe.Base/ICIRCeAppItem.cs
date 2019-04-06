using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIRCe.Base
{
    /// <summary>
    /// Произвольное окно сеанса в Цирцее (сервер, канал, приват)
    /// </summary>
    public interface ICIRCeAppItem: ICIRCeItem
    {
        IPanel ChatPanel { get; }

        void Echo(string text);

        /// <summary>
        /// Пользователь нажал клавишу
        /// </summary>
        event EventHandler<SerializableKeyEventArgs> InputKeyDown;

        /// <summary>
        /// Пользователь нажал символьную клавишу
        /// </summary>
        event EventHandler<SerializableKeyPressedEventArgs> InputKeyPress;

        /// <summary>
        /// Пользователь нажал мышкой в чате
        /// </summary>
        event EventHandler<ChatClickEventArgs> ChatClicked;

        /// <summary>
        /// Пользователь выбрал текст в чате
        /// </summary>
        event Action<string> ChatSelected;
    }
}
