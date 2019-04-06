using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRC.Client.Base
{   
    /// <summary>
    /// Набор специальных символов и строк для IRC
    /// </summary>
    public struct Symbols
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
