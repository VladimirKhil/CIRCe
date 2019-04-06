using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRCProviders
{
    /// <summary>
    /// Режим записи логов
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// Отсутствует
        /// </summary>
        [Description("LogModeNone")]
        None,
        /// <summary>
        /// Текст
        /// </summary>
        [Description("LogModeTxt")]
        Txt,
        /// <summary>
        /// RTF
        /// </summary>
        [Description("LogModeRtf")]
        Rtf,
        /// <summary>
        /// HTML
        /// </summary>
        [Description("LogModeHtml")]
        Html,
        /// <summary>
        /// HTML для форумов
        /// </summary>
        [Description("LogModeHtmlForum")]
        HtmlForum,
        /// <summary>
        /// Стиль для форума
        /// </summary>
        [Description("LogModeForum")]
        Forum,
        /// <summary>
        /// Стиль для форума 2
        /// </summary>
        [Description("LogModeForum2")]
        Forum2
    };

    /// <summary>
    /// Режим мигания
    /// </summary>
    public enum FlashMode
    {
        /// <summary>
        /// Мигает на любые сообщения
        /// </summary>
        [Description("FullFlashMode")]
        Full,
        /// <summary>
        /// Мигает на ник и приваты
        /// </summary>
        [Description("MediumFlashMode")]
        Meduim,
        /// <summary>
        /// Не мигает вообще
        /// </summary>
        [Description("WeakFlashMode")]
        Weak
    }

    /// <summary>
    /// Режим произвольного действия
    /// </summary>
    public enum PlayMode
    {
        /// <summary>
        /// Работает всегда
        /// </summary>
        [Description("PlayModeAll")]
        All,
        /// <summary>
        /// Работает только по команда оператора канала
        /// </summary>
        [Description("PlayModeOpOnly")]
        OpOnly,
        /// <summary>
        /// Не работает никогда
        /// </summary>
        [Description("PlayModeNone")]
        None
    }

    /// <summary>
    /// Набор специальных символов и строк для IRC
    /// </summary>
    public struct Special
    {
        /// <summary>
        /// Код цвета
        /// </summary>
        public const char Color = '';
        /// <summary>
        /// Код жирности
        /// </summary>
        public const char Bold = '';
        /// <summary>
        /// Код подчёркивания
        /// </summary>
        public const char Underlined = '';
        /// <summary>
        /// Код реверсивного текста
        /// </summary>
        public const char Reverse = '';
        /// <summary>
        /// Код простого текста
        /// </summary>
        public const char Plain = '';

        /// <summary>
        /// Обрамляющий символ для CTCP-сообщений
        /// </summary>
        public const char Ctcp = '';

        /// <summary>
        /// Символ, с которого начинаются сообщения-команды
        /// </summary>
        public const char CmdStarter = '/';

        /// <summary>
        /// Разделитель строк в IRC-сообщениях
        /// </summary>
        public const string StringSeparator = "\r\n";
    }
}
