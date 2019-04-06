using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using CIRCe.Base;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCeMenuCommands : CIRCeCommand, ICommandsList
    {
        private List<ICommand> commandsList = new List<ICommand>();
        public System.Windows.Forms.ToolStripItemCollection Items { get; set; }
        public System.Windows.Forms.Control Dispatcher { get; set; }

        public CIRCeMenuCommands()
        {

        }

        public ICommand AddSeparator()
        {
            if (this.Dispatcher.InvokeRequired)
            {
                return (ICommand)this.Dispatcher.EndInvoke(this.Dispatcher.BeginInvoke(new Func<ICommand>(AddSeparator)));
            }

            var separator = new System.Windows.Forms.ToolStripSeparator();
            this.Items.Add(separator);
            var command = new CIRCeCommand { Item = separator };
            this.commandsList.Add(command);

            return command;
        }

        public ICommand AddCommand(string title, Action action)
        {
            if (this.Dispatcher.InvokeRequired)
            {
                return (ICommand)this.Dispatcher.EndInvoke(this.Dispatcher.BeginInvoke(new Func<string, Action, ICommand>(AddCommand), title, action));
            }

            var item = new System.Windows.Forms.ToolStripMenuItem { Text = title };
            item.Click += (sender, e) =>
            {
                action();
            };
            this.Items.Add(item);
            var command = new CIRCeCommand { Item = item };
            this.commandsList.Add(command);

            return command;
        }

        public void Remove(ICommand command)
        {
            if (this.Dispatcher.InvokeRequired)
            {
                if (this.Dispatcher.IsDisposed || !this.Dispatcher.Created)
                    return;

                try
                {
                    this.Dispatcher.Invoke(new Action<ICommand>(Remove), command);
                }
                catch (InvalidOperationException)
                {
                }

                return;
            }

            if (this.commandsList.Contains(command))
            {
                this.commandsList.Remove(command);
                this.Items.Remove(((CIRCeCommand)command).Item);
            }
        }
        
        public ICommandsList AddCommandList(string title)
        {
            if (this.Dispatcher.InvokeRequired)
            {
                return (ICommandsList)this.Dispatcher.EndInvoke(this.Dispatcher.BeginInvoke(new Func<string, ICommand>(AddCommandList), title));
            }

            var item = new System.Windows.Forms.ToolStripMenuItem { Text = title };
            this.Items.Add(item);
            var commands = new CIRCeMenuCommands { Item = item, Dispatcher = this.Dispatcher, Items = item.DropDownItems };
            this.commandsList.Add(commands);

            return commands;
        }
    }
}
