using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IRCProviders
{
    /// <summary>
    /// Сообщение IRC
    /// </summary>
    [Serializable]
    public class IRCMessage
    {
        /// <summary>
        /// Префикс отправителя
        /// </summary>
        public string Prefix { get { return this.Name + (this.User.Length > 0 ? "!" + this.User : string.Empty) + (this.Host.Length > 0 ? "@" + this.Host : string.Empty); } }
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
        public List<string> Param { get; private set; }
        /// <summary>
        /// Хвост сообщения
        /// </summary>
        public string Tail { get; set; }

        private static Regex IRCRegex = new Regex(@"^(\:(?<name>[^! ]+)(\!(?<user>[^@ ]+))?(\@(?<host>[^ ]+))? )?(?<command>\w+)( (?<param>[^:][^ ]*))*(?<param> )?( \:(?<tail>.*))?$", RegexOptions.Compiled);

        public IRCMessage() { }

        /// <summary>
        /// Создание IRC-сообщения путём парсинга строки с сообщением
        /// </summary>
        /// <param name="message">Строка с сообщением</param>
        public IRCMessage(string message)
        {
            var match = IRCRegex.Match(message);
            this.Name = match.Groups["name"].Value;
            this.User = match.Groups["user"].Value;
            this.Host = match.Groups["host"].Value;
            this.Command = match.Groups["command"].Value;
            this.Tail = match.Groups["tail"].Value;

            this.Param = new List<string>();
            foreach (Capture item in match.Groups["param"].Captures)
            {
                this.Param.Add(item.Value);
            }
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
            foreach (string item in this.Param)
            {
                message.Append(' ');
                message.Append(item);
            }
            if (this.Tail.Length > 0)
            {
                message.Append(" :");
                message.Append(this.Tail);
            }

            message.Append(Special.StringSeparator);

            return message.ToString();
        }
    }
}
