using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace IRCProviders
{
    /// <summary>
    /// Базовый интерфейс окна клиента
    /// </summary>
    public interface IBaseWindow: IDisposable
    {
        #region Properties

        /// <summary>
        /// Форма окна
        /// </summary>
        IRCForm Self { get; }
        
        /// <summary>
        /// Панель чата
        /// </summary>
        IPanel ChatPanel { get; }

        /// <summary>
        /// Панель ввода сообщения
        /// </summary>
        IPanel InputPanel { get; }

        /// <summary>
        /// Сервер, к которому относится данное окно
        /// </summary>
        IServerWindow ServerWindow { get; }

        #endregion

        #region Events

        /// <summary>
        /// Пользователь нажал клавишу
        /// </summary>
        event EventHandler<SerializableKeyEventArgs> InputKeyDown;

        /// <summary>
        /// Пользователь нажал символьную клавишу
        /// </summary>
        event EventHandler<SerializableKeyPressedEventArgs> InputKeyPress;

        /// <summary>
        /// Окно закрылось
        /// </summary>
        event EventHandler Disposed;

        #endregion

        #region Methods

        /// <summary>
        /// Закрыть окно
        /// </summary>
        void Close();

        /// <summary>
        /// Просто вывести текст в окно
        /// </summary>
        /// <param name="text">Выводимый текст</param>
        void Echo(string text);

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        void PutMessage(string msgText);

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColor">Цвет по умолчанию</param>
        void PutMessage(string msgText, Color defColor);

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColorIndex">Код цвета по умолчанию</param>
        void PutMessage(string msgText, int defColorIndex);

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColorIndex">Код цвета по умолчанию</param>
        /// <param name="putTime">Указать ли время добалвения сообщения</param>
        void PutMessage(string msgText, int defColorIndex, bool putTime);

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColor">Цвет по умолчанию</param>
        /// <param name="putTime">Указать ли время добалвения сообщения</param>
        void PutMessage(string msgText, Color defColor, bool putTime);

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColor">Цвет по умолчанию</param>
        /// <param name="putTime">Указать ли время добалвения сообщения</param>
        /// <param name="messageType">Тип публикуемого сообщения</param>
        void PutMessage(string msgText, Color defColor, bool putTime, MessageType messageType);

        /// <summary>
        /// Активировать окно
        /// </summary>
        void ActivateWindow();

        /// <summary>
        /// Передать фокус окну
        /// </summary>
        /// <returns>Успешность операции</returns>
        bool Focus();

        #endregion
    }
}
