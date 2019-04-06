using System;
using System.Collections.Generic;
using System.Text;
using IRCProviders;

namespace IRCWindow
{
    /// <summary>
    /// Расширенные возможности коммуникации
    /// </summary>
    interface IExtendedCommunicationWindow: ICommunicationWindow
    {
        /// <summary>
        /// Записать полученное сообщение от ника в окно как если бы оно на самом деле было получено
        /// </summary>
        /// <param name="nick">Ник</param>
        /// <param name="msg">Сообщение</param>
        void InvokeReceive(string nick, string msg);

        /// <summary>
        /// Добавить пользователя в список
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="raiseEvent">Нужно ли создать событие, информирующее о добавлении</param>
        void AddPerson(string name, bool raiseEvent);

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="name">Ник пользователя</param>
        /// <param name="raiseEvent">Сгенерировать событие об удалении пользователя</param>
        void RemovePerson(string name, bool raiseEvent);

        /// <summary>
        /// Поменять имя пользователя
        /// </summary>
        /// <param name="oldName">Старый ник</param>
        /// <param name="newName">Новый ник</param>
        void RenamePerson(string oldName, string newName);

        /// <summary>
        /// Очистить список пользователей
        /// </summary>
        void ClearPersons();

        void Quit();
    }
}
