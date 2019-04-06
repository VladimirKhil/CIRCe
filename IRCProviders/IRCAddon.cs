using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Класс, от которого нужно наследовать, чтобы подключить аддон к IRC-клиенту
    /// При запуске аддона через функцию Run передаётся ссылка на главное окно приложения
    /// Изменяя свойства, вызывая методы и подписываясь на события главного окна, можно взаимодействовать с приложением
    /// </summary>
    [Obsolete]
    public abstract class IRCAddon: MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// Событие остановки аддона
        /// </summary>
        public event EventHandler Stop;

        protected void RaiseStop()
        {
            if (Stop != null)
                Stop(this, EventArgs.Empty);
        }

        /// <summary>
        /// Запуск аддона
        /// </summary>
        /// <param name="mainWindow">Интерфейс вызывающего приложения (Цирцеи)</param>
        public abstract void Run(IMainWindow mainWindow);

        /// <summary>
        /// Нужно ли аддону обновление
        /// </summary>
        /// <returns>Да, если нужно, и нет в противном случае</returns>
        public abstract bool IsUpdateNeeded();

        /// <summary>
        /// Получить URI файла, который нужно скачать для обновления аддона
        /// </summary>
        /// <returns>URI файла с обновлением</returns>
        public abstract string GetUpdateUri();

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
