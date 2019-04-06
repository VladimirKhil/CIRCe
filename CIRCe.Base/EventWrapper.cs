using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// Обёртывает делегат для вызова из другого домена приложения
    /// </summary>
    /// <typeparam name="TDelegate">Тип обёртываемого делегата</typeparam>
    public abstract class EventWrapper<TDelegate> : InfiniteMarshalByRefObject
        where TDelegate: class
    {
        protected TDelegate del = null;

        protected EventWrapper(TDelegate del)
        {
            this.del = del;
        }
    }

    public sealed class ActionEventWrapper : EventWrapper<Action>
    {
        private void Call()
        {
            this.del();
        }

        private ActionEventWrapper(Action action)
            : base(action)
        {

        }

        public static Action Wrap(Action action)
        {
            return new ActionEventWrapper(action).Call;
        }
    }

    public sealed class ActionEventWrapper<T> : EventWrapper<Action<T>>
    {
        private void Call(T arg)
        {
            this.del(arg);
        }

        private ActionEventWrapper(Action<T> action)
            : base(action)
        {

        }

        public static Action<T> Wrap(Action<T> action)
        {
            return new ActionEventWrapper<T>(action).Call;
        }
    }

    public sealed class ActionEventWrapper<T1, T2> : EventWrapper<Action<T1, T2>>
    {
        private void Call(T1 arg1, T2 arg2)
        {
            this.del(arg1, arg2);
        }

        private ActionEventWrapper(Action<T1, T2> action)
            : base(action)
        {

        }

        public static Action<T1, T2> Wrap(Action<T1, T2> action)
        {
            return new ActionEventWrapper<T1, T2>(action).Call;
        }
    }

    public sealed class FuncEventWrapper<T, TResult> : EventWrapper<Func<T, TResult>>
    {
        private TResult Call(T arg)
        {
            return this.del(arg);
        }

        private FuncEventWrapper(Func<T, TResult> func)
            : base(func)
        {

        }

        public static Func<T, TResult> Wrap(Func<T, TResult> func)
        {
            return new FuncEventWrapper<T, TResult>(func).Call;
        }
    }

    public sealed class EventHandlerWrapper : EventWrapper<EventHandler>
    {
        private void Call(object sender, EventArgs e)
        {
            this.del(sender, e);
        }

        private EventHandlerWrapper(EventHandler handler)
            : base(handler)
        {

        }

        public static EventHandler Wrap(EventHandler handler)
        {
            return new EventHandlerWrapper(handler).Call;
        }
    }

    public sealed class EventHandlerWrapper<T> : EventWrapper<EventHandler<T>>
        where T: EventArgs
    {
        private void Call(object sender, T e)
        {
            this.del(sender, e);
        }

        private EventHandlerWrapper(EventHandler<T> handler)
            : base(handler)
        {

        }

        public static EventHandler<T> Wrap(EventHandler<T> handler)
        {
            return new EventHandlerWrapper<T>(handler).Call;
        }
    }
}
