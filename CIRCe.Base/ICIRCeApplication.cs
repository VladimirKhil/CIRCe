using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client.Base;
using System.IO;
using System.ComponentModel;
using System.Drawing;

namespace CIRCe.Base
{
    /// <summary>
    /// Интерфейс приложения, предоставляющий доступ к возможностям Цирцеи
    /// </summary>
    public interface ICIRCeApplication: IApplication
    {
        /// <summary>
        /// Список активных серверов
        /// </summary>
        new IChangeable<ICIRCeServer> Servers { get; }

        /// <summary>
        /// Открыть подключение
        /// </summary>
        /// <param name="connectionInfo">Информация о подключении</param>
        /// <returns>Созданное подключение к серверу</returns>
        ICIRCeServer CreateConnection(ExtendedConnectionInfo connectionInfo);

        /// <summary>
        /// Открыть подключение
        /// </summary>
        /// <param name="connectionInfo">Информация о подключении</param>
        /// <returns>Созданное подключение к серверу</returns>
        new ICIRCeServer CreateConnection(ConnectionInfo connectionInfo);

        /// <summary>
        /// Палитра цветов текущего пользователя
        /// </summary>
        Color[] ColorsTable { get; }

        /// <summary>
        /// Текущий активный элемент приложения (сервер, канал, приват или другой объект)
        /// </summary>
        ICIRCeItem ActiveItem { get; }

        /// <summary>
        /// Ник по умолчанию
        /// </summary>
        string DefaultNickName { get; }

        /// <summary>
        /// Команды приложения
        /// </summary>
        ICommandsList Commands { get; }

        /// <summary>
        /// Добавить дочернее окно для Цирцеи
        /// </summary>
        /// <param name="handle">Дескриптор дочернего окна</param>
        void AddOwnedWindow(IntPtr handle);

        /// <summary>
        /// Добавить элемент в дерево объектов Цирцеи
        /// </summary>
        /// <param name="parent">Родительский элемент или null</param>
        /// <param name="handle"></param>
        /// <param name="hIcon"></param>
        /// <returns></returns>
        ICIRCeItem AddItem(ICIRCeItem parent, IntPtr handle, string title, IntPtr hIcon);

        /// <summary>
        /// Задать сообщение в статусной строке приложения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void Status(string message);

        /// <summary>
        /// Приложение потеряло фокус
        /// </summary>
        event Action Deactivated;
    }
}
