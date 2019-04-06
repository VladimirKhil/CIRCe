using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IRCConnection;

namespace IRCProviders
{
    /// <summary>
    /// Основной интерфейс, через который аддон может взаимодействовать с приложением
    /// </summary>
    public interface IMainWindow
    {
        #region Properties

        /// <summary>
        /// Расположение папки данных Цирцеи (может меняться в процессе работы программы)
        /// </summary>
        /// <remarks>Для хранения настроек аддона лучше использовать его собственную папку</remarks>
        string DataFolderPath { get; }

        /// <summary>
        /// Активное окно среди окон серверов, каналов и приватов
        /// </summary>
        IBaseWindow ActiveIRCWindow { get; }

        /// <summary>
        /// Окна существующих подключений
        /// </summary>
        IServerWindow[] ServerWindows { get; }

        /// <summary>
        /// Ник по умолчанию
        /// </summary>
        //INick DefaultNick { get; }

        /// <summary>
        /// Личные данные пользователя
        /// </summary>
        IUser User { get; }

        #endregion

        #region Events



        #endregion

        #region Methods

        /// <summary>
        /// Создать объект в Цирцее.
        /// Все визуальные элементы в аддонах должны создаваться только через эту функцию
        /// </summary>
        /// <param name="domain">Домен аддона, в котором должен быть создан объект</param>
        /// <param name="objectType">Тип создаваемого объекта</param>
        /// <param name="args">Параметры конструктора</param>
        /// <returns></returns>
        object CreateObject(AppDomain domain, Type objectType, params object[] args);

        /// <summary>
        /// Уничтожить объект
        /// </summary>
        /// <param name="obj">Уничтожаемый объект</param>
        void DestroyObject(object obj);

        /// <summary>
        /// Позволяет выполнить функцию в потоке Цирцеи
        /// В этой функции разрешено создавать свои визуальные элементы
        /// </summary>
        /// <param name="del">Исполяемая функция</param>
        void RunCallback(Action func);

        /// <summary>
        /// Позволяет выполнить функцию в потоке Цирцеи
        /// В этой функции разрешено создавать свои визуальные элементы
        /// </summary>
        /// <param name="del">Исполяемая функция</param>
        /// <param name="param">Параметр функции</param>
        void RunCallback(Action<object> del, object param);

        /// <summary>
        /// Показать форму в качестве диалогового окна в клиенте
        /// </summary>
        /// <param name="dialogForm">Отображаемая форма</param>
        /// <returns>Результат показа</returns>
        DialogResult ShowDialog(Form dialogForm);

        /// <summary>
        /// Зарегистрировать форму в качестве MDI окна и показать её
        /// </summary>
        /// <param name="owner">Форма, которая будет являться главной по отношению к данной</param>
        /// <param name="child">Переданная форма</param>
        /// <returns>Узел в дереве, который отображает данную форму</returns>
        IFormNode RegisterAsMDIChild(IRCForm owner, IRCForm child, IWin32Window ownerForm);

        /// <summary>
        /// Получить окно, владеющее формой
        /// </summary>
        /// <param name="form">Форма</param>
        /// <returns>Форма-владелец или null. Имеет тип IBaseWindow для окон IRC (серверов, каналов и приват) и Form для прочих окон</returns>
        IRCForm GetWindowOwner(Form form);

        /// <summary>
        /// Показать MessageBox
        /// </summary>
        /// <param name="dialogForm">Отображаемая форма</param>
        /// <returns>Результат показа</returns>
        DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

        /// <summary>
        /// Установить статусное сообщение
        /// </summary>
        /// <param name="statusString">Статусное сообщение</param>
        void Status(string statusString);

        /// <summary>
        /// Заставить окно мигать (только в свёрнутом состоянии)
        /// </summary>
        void Flash();

        /// <summary>
        /// Подключиться к определённому серверу
        /// </summary>
        /// <param name="info">Информация о подключении</param>
        /// <returns>Созданное окно подключения (или существующее, если подключение уже состоялось)</returns>
        //IServerWindow OpenConnection(ConnectionInfo info);

        /// <summary>
        /// Сыграть файл мультимедиа
        /// </summary>
        /// <param name="multimediaFile">Адрес мультимедиа-файла</param>
        /// <param name="numOfPlayer">Номер проигрывателя: 0 - основной, 1 - вспомогательный</param>
        /// <returns>Строка, в которую передаётся сообщение об ошибке</returns>
        string Play(string multimediaFile, int numOfPlayer);

        #endregion
    }
}
