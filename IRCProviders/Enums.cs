using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Тип сообщения для печати
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Реплика
        /// </summary>
        Replic,
        /// <summary>
        /// Событие пользователей
        /// </summary>
        UserEvent,
        /// <summary>
        /// Событие каналов
        /// </summary>
        ChannelEvent,
        /// <summary>
        /// Оповещения
        /// </summary>
        NotifyEvent
    }
}
