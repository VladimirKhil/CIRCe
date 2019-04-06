using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace IRCWindow.ViewModel.Common
{
    /// <summary>
    /// Команда приложения
    /// </summary>
    public sealed class SimpleCommand: DispatcherObject, ICommand
    {
        #region Fields

        private readonly Action<object> execute;
        private bool canBeExecuted = true;
        
        /// <summary>
        /// Может ли эта команда в настоящий момент быть выполнена
        /// </summary>
        public bool CanBeExecuted
        {
            get { return this.canBeExecuted; }
            set
            {
                bool changed = this.canBeExecuted != value;
                this.canBeExecuted = value;
                if (changed)
                    this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (CanExecuteChanged != null)
                                CanExecuteChanged(this, EventArgs.Empty);
                        }));
            }
        }

        #endregion // Fields

        #region Constructors

        public SimpleCommand(Action<object> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
        }

        #endregion // Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">Этот параметр игнорируется</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.canBeExecuted;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
