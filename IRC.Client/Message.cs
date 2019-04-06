using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using IRC.Client.Base;

namespace IRC.Client
{
    /// <summary>
    /// Сообщение IRC
    /// </summary>
    /// <remarks>Класс является публичным лишь на время переходного периода. Его явное использование категорически не рекомендуется</remarks>
    [Serializable]
    public sealed class Message : IMessage
    {
        /// <summary>
        /// Шаблон IRC-сообщения
        /// </summary>
        private static Regex IRCRegex = new Regex(@"^(\:(?<name>[^! ]+)(\!(?<user>[^@ ]+))?(\@(?<host>[^ ]+))? )?(?<command>\w+)( (?<param>[^:][^ ]*))*(?<param> )?( \:(?<tail>.*))?$", RegexOptions.Compiled);

        /// <summary>
        /// Префикс отправителя
        /// </summary>
        public string Prefix { get { return string.Format("{0}{1}{2}", this.Name, this.User.Length > 0 ? "!" + this.User : string.Empty, this.Host.Length > 0 ? "@" + this.Host : string.Empty); } }
        /// <summary>
        /// Имя отправителя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Имя юзера отправителя
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Имя хоста отправителя
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Команда
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Параметры команды
        /// </summary>
        public string[] Params { get; set; }
        /// <summary>
        /// Хвост сообщения
        /// </summary>
        public string Tail { get; set; }

        public Message() { }

        /// <summary>
        /// Создание IRC-сообщения путём парсинга строки с сообщением
        /// </summary>
        /// <param name="message">Строка с сообщением</param>
        public static Message Parse(string message)
        {
            var result = new Message();
            var match = IRCRegex.Match(message);
            result.Name = match.Groups["name"].Value;
            result.User = match.Groups["user"].Value;
            result.Host = match.Groups["host"].Value;
            result.Command = match.Groups["command"].Value;
            result.Tail = match.Groups["tail"].Value;

            var parameters = new List<string>();
            foreach (Capture item in match.Groups["param"].Captures)
            {
                parameters.Add(item.Value);
            }

            result.Params = parameters.ToArray();
            return result;
        }

        public override string ToString()
        {
            var message = new StringBuilder();
            string prefix = Prefix;
            if (prefix.Length > 0)
            {
                message.Append(':');
                message.Append(prefix);
                message.Append(' ');
            }
            message.Append(this.Command);
            foreach (string item in this.Params)
            {
                message.Append(' ');
                message.Append(item);
            }
            if (this.Tail.Length > 0)
            {
                message.Append(" :");
                message.Append(this.Tail);
            }

            message.Append(Symbols.StringSeparator);

            return message.ToString();
        }
    }
}
