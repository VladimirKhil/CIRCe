using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;

namespace CIRCe.Base
{
    /// <summary>
    /// Набор управляющих команд
    /// </summary>
    public interface IUsersCommandsList
    {
        ICommand AddSeparator();
        ICommand AddCommand(string title, Action<IEnumerable<ChannelUserInfo>> action/*, Func<IEnumerable<ChannelUserInfo>> canExecute = null*/);

        void Remove(ICommand command);
    }
}
