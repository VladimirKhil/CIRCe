using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net.Configuration;
using System.Security.Authentication;

namespace IRCConnection
{
    /// <summary>
    /// Подключение к IRC-серверу
    /// </summary>
    public class Connection: IDisposable
    {
        private ConnectionInfo info = null;
        private TcpClient client = null;
        private byte[] buffer = new byte[5000];
        private string quitMsg = string.Empty;
        private string usingServer = string.Empty;
        private System.Threading.Timer timer = null;
        private object sync = new object();

        /// <summary>
        /// Обработчик полученного сообщения с сервера
        /// </summary>
        /// <param name="m">Присланное сообщение</param>
        public delegate void ReceiveDel(string m);

        /// <summary>
        /// Получено сообщение
        /// </summary>
        public event ReceiveDel MessageReceived;

        public delegate void DisposedDel();

        /// <summary>
        /// Бот ликвидирован
        /// </summary>
        public event DisposedDel Disposed;

        /// <summary>
        /// Юзер
        /// </summary>
        public NickName Nick
        {
            get { return info.Nick; }
        }

        /// <summary>
        /// Сервер
        /// </summary>
        public Server Server
        {
            get { return info.Server; }
        }

        /// <summary>
        /// Имя сервера, с которым мы общаемся
        /// </summary>
        public string UsingServer
        {
            get { return usingServer.Length > 0 ? usingServer : this.Server.Name; }
            set { this.usingServer = value; }
        }

        /// <summary>
        /// Имеется ли сейчас подключение
        /// </summary>
        public bool Connected
        {
            get { return client != null && client.Connected; }
        }

        internal bool UseProxy { get; set; }

        /// <summary>
        /// Создание бота
        /// </summary>
        /// <param name="name">Имя бота</param>
        public Connection(ConnectionInfo info, string quitMsg, bool useProxy)
        {
            this.info = info;
            this.quitMsg = quitMsg;
            this.UseProxy = useProxy;
        }

        /// <summary>
        /// Подкючение к серверу
        /// </summary>
        /// <param name="server">Целевой сервер</param>
        public void Connect()
        {
            client = new TcpClient();
            client.SendTimeout = 100;

            var destination = new Uri(string.Format("irc://{0}:{1}", info.Server.Name, info.Server.Port));
            var endPoint = this.UseProxy ? WebRequest.DefaultWebProxy.GetProxy(destination) : destination;
            
            //IPAddress ipAddress = Dns.GetHostEntry(info.Server.Name).AddressList[0];
            //IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, info.Server.Port);
            var ipAddress = Dns.GetHostEntry(endPoint.Host).AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddress, endPoint.Port);
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
                var data = Encoding.GetEncoding(1251).GetBytes(request);
                var stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();

                data = new Byte[1024];
                int i = stream.Read(data, 0, data.Length);

                var response = Encoding.GetEncoding(1251).GetString(data);
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

            timer = new System.Threading.Timer(TimerCallback, null, 3000, System.Threading.Timeout.Infinite);
        }

        void TimerCallback(object state)
        {
            if (client.Connected)
            {
                RunCmd("NICK", info.Nick.Nick);
                RunCmd("USER", info.Nick.User.UserName, info.Nick.User.Host, info.Server.Name, String.Format(":{0}", info.Nick.User.Name));
                timer.Dispose();
            }
        }

        void Connection_MessageReceived(string m)
        {
            if (m.Contains("Found your"))
            {
                MessageReceived -= Connection_MessageReceived;
                RunCmd("NICK", info.Nick.Nick);
                RunCmd("USER", info.Nick.User.UserName, info.Nick.User.Host, info.Server.Name, ":" + info.Nick.User.Name);
                timer.Dispose();
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
                    var data = System.Text.Encoding.GetEncoding(1251).GetBytes(message);
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
                        Disposed();
                        return;
                    }
                    string data = System.Text.Encoding.GetEncoding(1251).GetString(buffer, 0, bytesRead);
                    client.GetStream().BeginRead(buffer, 0, buffer.Length, ReceiverCallback, null);
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
            lock (this.sync)
            {
                if (client != null && client.Connected)
                {
                    try
                    {
                        RunCmd("QUIT", ":" + quitMsg);
                    }
                    catch (InvalidOperationException)
                    {

                    }
                    catch (IOException)
                    {
                    }
                    client.Close();
                }
            }

            if (Disposed != null)
                Disposed();
        }

        #endregion
    }
}
