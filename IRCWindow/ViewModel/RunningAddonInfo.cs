using System;
using System.Collections.Generic;
using System.Text;
using CIRCe.Base;
using System.Threading;

namespace IRCWindow.ViewModel
{
    /// <summary>
    /// Информация о запущенном дополнении
    /// </summary>
    public sealed class RunningAddonInfo
    {
        /// <summary>
        /// Информация о дополнении
        /// </summary>
        public AddonInformation Info;
        /// <summary>
        /// Запущенный экземпляр дополнения
        /// </summary>
        public Addon Instanse;
        /// <summary>
        /// Домен, в котором выполняется дополнение
        /// </summary>
        public AppDomain Domain;
        /// <summary>
        /// Поток, в котором выполняется дополнение
        /// </summary>
        public Thread MainThread;
    }
}
