using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// Дополнение к IRC-клиенту, расширяющее его функциональность
    /// </summary>
    public abstract class Addon : InfiniteMarshalByRefObject, IDisposable
    {
        protected Dictionary<Delegate, Delegate> wrappersCache = new Dictionary<Delegate, Delegate>();

        /// <summary>
        /// Запустить аддон
        /// </summary>
        /// <param name="application">Вызывающее приложение</param>
        public abstract void Run(ICIRCeApplication application);

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

        /// <summary>
        /// Освободить ресурсы, связанные с дополнением
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Дать команду дополнению завершить свою работу
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Создать обёртку делегата, пригодную к подписке на события Цирцеи
        /// </summary>
        /// <param name="action">Обёртываемый делегат</param>
        /// <returns>Созданная обёртка</returns>
        public Action Wrap(Action action)
        {
            Delegate result = null;
            if (wrappersCache.TryGetValue(action, out result))
                return (Action)result;

            var result2 = ActionEventWrapper.Wrap(action);
            wrappersCache[action] = result2;
            return result2;
        }

        /// <summary>
        /// Создать обёртку делегата, пригодную к подписке на события Цирцеи
        /// </summary>
        /// <typeparam name="T">Тип параметра делегата</typeparam>
        /// <param name="action">Обёртываемый делегат</param>
        /// <returns>Созданная обёртка</returns>
        public Action<T> Wrap<T>(Action<T> action)
        {
            Delegate result = null;
            if (wrappersCache.TryGetValue(action, out result))
                return (Action<T>)result;

            var result2 = ActionEventWrapper<T>.Wrap(action);
            wrappersCache[action] = result2;
            return result2;
        }

        public Action<T1, T2> Wrap<T1, T2>(Action<T1, T2> action)
        {
            Delegate result = null;
            if (wrappersCache.TryGetValue(action, out result))
                return (Action<T1, T2>)result;

            var result2 = ActionEventWrapper<T1, T2>.Wrap(action);
            wrappersCache[action] = result2;
            return result2;
        }

        public Func<T, TResult> Wrap<T, TResult>(Func<T, TResult> func)
        {
            Delegate result = null;
            if (wrappersCache.TryGetValue(func, out result))
                return (Func<T, TResult>)result;

            var result2 = FuncEventWrapper<T, TResult>.Wrap(func);
            wrappersCache[func] = result2;
            return result2;
        }

        /// <summary>
        /// Создать обёртку делегата, пригодную к подписке на события Цирцеи
        /// </summary>
        /// <typeparam name="T">Тип параметра делегата</typeparam>
        /// <param name="del">Обёртываемый делегат</param>
        /// <returns>Созданная обёртка</returns>
        public EventHandler<T> Wrap<T>(EventHandler<T> del)
            where T: EventArgs
        {
            Delegate result = null;
            if (wrappersCache.TryGetValue(del, out result))
                return (EventHandler<T>)result;

            var result2 = EventHandlerWrapper<T>.Wrap(del);
            wrappersCache[del] = result2;
            return result2;
        }

        /// <summary>
        /// Создать обёртку делегата, пригодную к подписке на события Цирцеи
        /// </summary>
        /// <param name="del">Обёртываемый делегат</param>
        /// <returns>Созданная обёртка</returns>
        public EventHandler Wrap(EventHandler del)
        {
            Delegate result = null;
            if (wrappersCache.TryGetValue(del, out result))
                return (EventHandler)result;

            var result2 = EventHandlerWrapper.Wrap(del);
            wrappersCache[del] = result2;
            return result2;
        }
    }
}
