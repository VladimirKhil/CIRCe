using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Configuration;
using System.Security.Authentication;
using IRC.Client.Base;

namespace IRC.Client
{
    /// <summary>
    /// Подключение к IRC-серверу
    /// </summary>
    public sealed class Connection : IDisposable
    {
        private ConnectionInfo info = null;
        private TcpClient client = null;
        private byte[] buffer = new byte[5000];
        private string usingServer = string.Empty;
        private System.Threading.Timer timer = null;
        private object sync = new object();

        /// <summary>
        /// Базовая кодировка, используемая в IRC
        /// </summary>
        private static Encoding BasicEncoding = Encoding.GetEncoding(1251);

        /// <summary>
        /// Получено сообщение
        /// </summary>
        public event Action<string> MessageReceived;

        /// <summary>
        /// Подключение ликвидировано
        /// </summary>
        public event Action Disposed;

        /// <summary>
        /// Имя сервера, с которым мы общаемся
        /// </summary>
        public string UsingServer
        {
            get { return usingServer.Length > 0 ? usingServer : this.info.Server.Name; }
            set { this.usingServer = value; }
        }

        /// <summary>
        /// Имеется ли сейчас подключение
        /// </summary>
        public bool Connected
        {
            get { return this.client != null && this.client.Connected; }
        }

        internal bool UseProxy { get; private set; }

        /// <summary>
        /// Создание подключения
        /// </summary>
        /// <param name="info">Информация о подключении</param>
        /// <param name="useProxy">Использовать ли прокси-сервер</param>
        public Connection(ConnectionInfo info, bool useProxy = false)
        {
            this.info = info;
            this.UseProxy = useProxy;
        }

        /// <summary>
        /// Подкючение к серверу
        /// </summary>
        /// <param name="server">Целевой сервер</param>
        public void Connect()
        {
            this.client = new TcpClient { SendTimeout = 100 };

            var destination = new Uri(string.Format("irc://{0}:{1}", info.Server.Name, info.Server.Port));
            var endPoint = this.UseProxy ? WebRequest.DefaultWebProxy.GetProxy(destination) : destination;
            
            IPAddress ipAddress = Dns.GetHostEntry(endPoint.Host).AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, endPoint.Port);
            client.Connect(ipEndPoint);

            if (this.UseProxy) // Авторизация через прокси
            {
                var credentials = WebRequest.DefaultWebProxy.Credentials as NetworkCredential;
                var protocol = "HTTP/1.1";
                var userAgent = "CIRCe";

                string request = null;
                if (credentials == null)
                    request = string.Format("CONNECT {0}:{1} {2}\r\nUser-agent: {3}\r\n\r\n", info.Server.Name, info.Server.Port, protocol, userAgent);
                else if (credentials.Domain.Length == 0)
                    request = string.Format("CONNECT {0}:{1} {2}\r\nUser-agent: {3}\r\nProxy-authorization: {4} {5}\r\n\r\n", info.Server.Name, info.Server.Port, protocol, userAgent, credentials.UserName, credentials.Password);
                else
                    request = string.Format("CONNECT {0}:{1} {2}\r\nUser-agent: {3}\r\nProxy-authorization: {4}\\{5} {6}\r\n\r\n", info.Server.Name, info.Server.Port, protocol, userAgent, credentials.Domain, credentials.UserName, credentials.Password);
                var data = BasicEncoding.GetBytes(request);
                var stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();

                data = new Byte[1024];
                int i = stream.Read(data, 0, data.Length);

                var response = BasicEncoding.GetString(data);
                int index = response.IndexOf(' ');
                var code = response.Substring(index + 1, 3);

                if (code != "200")
                {
                    client.Close();
                    if (Disposed != null)
                        Disposed();
                    if (code == "407")
                        throw new AuthenticationException();
                    if (MessageReceived != null)
                        MessageReceived(response);
                    return;
                }
            }

            MessageReceived += Connection_MessageReceived;
            client.GetStream().BeginRead(buffer, 0, buffer.Length, ReceiverCallback, null);

            // Не все серверы отвечают "Found your"
            timer = new System.Threading.Timer(TimerCallback, null, 3000, System.Threading.Timeout.Infinite);
        }

        void TimerCallback(object state)
        {
            if (client.Connected)
            {
                Idenify();
            }
        }

        private void Idenify()
        {
            RunCmd("NICK", info.Nick);
            RunCmd("USER", info.User.UserName, "0", "*", ":" + info.User.RealName);
            timer.Dispose();
        }

        void Connection_MessageReceived(string m)
        {
            if (m.Contains("Found your"))
            {
                MessageReceived -= Connection_MessageReceived;
                Idenify();
            }
        }

        /// <summary>
        /// Сказать сообщение
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Say(string message)
        {
            lock (this.sync)
            {
                if (client.Connected)
                {
                    var data = BasicEncoding.GetBytes(message);
                    var stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="list">Параметры команды</param>
        public void RunCmd(params string[] list)
        {
            if (list.Length == 0)
                return;

            Say(string.Format(":{0} {1}\r\n", UsingServer, string.Join(" ", list)));
        }

        /// <summary>
        /// Получение входящего сообщения
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiverCallback(IAsyncResult ar)
        {
            try
            {
                if (client.Connected)
                {
                    int bytesRead = client.GetStream().EndRead(ar);
                    if (bytesRead < 1)
                    {
                        client.Close();
                        if (Disposed != null)
                            Disposed();
                        return;
                    }

                    string data = BasicEncoding.GetString(buffer, 0, bytesRead);
                    client.GetStream().BeginRead(buffer, 0, buffer.Length, ReceiverCallback, null);
                    if (MessageReceived != null)
                        MessageReceived(data);
                }
            }
            catch (Exception)
            {
                
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Разрыв соединения
        /// </summary>
        public void Dispose()
        {
            if (client != null && client.Connected)
            {
                client.Close();
            }

            if (Disposed != null)
                Disposed();
        }

        #endregion
    }
}
