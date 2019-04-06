using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRC.Client.Base
{
    /// <summary>
    /// Приложение клиента
    /// </summary>
    public interface IApplication: IDisposable
    {
        /// <summary>
        /// Список активных серверов
        /// </summary>
        IChangeable<IServer> Servers { get; }

        /// <summary>
        /// Открыть подключение
        /// </summary>
        /// <param name="connectionInfo">Информация о подключении</param>
        /// <returns>Созданное подключение к серверу</returns>
        IServer CreateConnection(ConnectionInfo connectionInfo);
    }
}
