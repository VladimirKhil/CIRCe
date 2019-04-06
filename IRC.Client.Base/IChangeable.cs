using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Коллекция, поддерживающая оповедения об изменениях
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    public interface IChangeable<out T>: IEnumerable<T>
    {
        event Action<object, T[]> ItemsAdded;
        event Action<object, T[]> ItemsRemoved;
    }
}
