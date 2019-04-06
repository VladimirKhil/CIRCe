using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIRCe.Base
{
    public interface ICommandsList: ICommand
    {
        ICommand AddSeparator();
        ICommand AddCommand(string title, Action action);
        ICommandsList AddCommandList(string title);

        void Remove(ICommand command);
    }
}
