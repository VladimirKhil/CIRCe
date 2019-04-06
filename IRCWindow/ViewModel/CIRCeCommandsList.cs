using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCeCommandsList : InfiniteMarshalByRefObject, ICommandsList
    {
        private List<ICommand> commandsList = new List<ICommand>();

        private IRCProviders.IContextMenuStrip menu = null;

        internal IRCProviders.IContextMenuStrip Menu
        {
            set { this.menu = value; }
        }

        private IRCProviders.IListView list = null;

        internal IRCProviders.IListView List
        {
            set { this.list = value; }
        }

        public ICommand AddSeparator()
        {
            var separator = new System.Windows.Forms.ToolStripSeparator();
            this.menu.Add(separator);
            var command = new CIRCeCommand { Item = separator };
            this.commandsList.Add(command);

            return command;
        }

        public ICommand AddCommand(string title, Action<IEnumerable<IRC.Client.Base.ChannelUserInfo>> action/*, Func<IEnumerable<IRC.Client.Base.ChannelUserInfo>> canExecute = null*/)
        {
            var item = new System.Windows.Forms.ToolStripMenuItem { Text = title };
            item.Click += (sender, e) =>
                {
                    var items = this.list.LVSelectedItems;
                    if (items.Length > 0)
                    {
                        action(items.Select(i => i.ToNew()));
                    }
                };
            this.menu.Add(item);
            var command = new CIRCeCommand { Item = item };
            this.commandsList.Add(command);

            return command;
        }

        public void Remove(ICommand command)
        {
            if (this.commandsList.Contains(command))
            {
                this.commandsList.Remove(command);
                this.menu.Remove((command as CIRCeCommand).Item);
            }
        }
    }
}
