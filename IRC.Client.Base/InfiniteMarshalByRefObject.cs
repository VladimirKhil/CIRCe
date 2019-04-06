using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Объект, доступный по ссылке, с бесконечным временем жизни
    /// </summary>
    public abstract class InfiniteMarshalByRefObject: MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null; // Благодаря этому трюку объект не будет уничтожаться по таймауту
        }
    }
}
