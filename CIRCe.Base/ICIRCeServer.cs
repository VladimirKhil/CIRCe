using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// IRC-сервер в Цирцее
    /// </summary>
    public interface ICIRCeServer: IServer, ICIRCeAppItem
    {
        /// <summary>
        /// Список активных серверов
        /// </summary>
        new IChangeable<ICIRCeChannel> Channels { get; }

        /// <summary>
        /// Зайти на канал
        /// </summary>
        /// <param name="channel">Имя канала</param>
        new ICIRCeChannel JoinChannel(string channel);

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="receiver">Имя получателя или канала</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="silent">Отображать ли отправленное сообщение в окне канала/привата</param>
        void SendMessage(string receiver, string message, bool silent);

        /// <summary>
        /// Загружен файл по команде url
        /// </summary>
        event Action<string> DownloadFinished;
    }
}
