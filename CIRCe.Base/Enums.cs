using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CIRCe.Base
{
    /// <summary>
    /// Режим запуска аддона
    /// </summary>
    public enum AddonStartMode
    {
        /// <summary>
        /// Отключён
        /// </summary>
        [Description("Отключено")]
        None,
        /// <summary>
        /// Автоматический
        /// </summary>
        [Description("Автоматически")]
        Automatic,
        /// <summary>
        /// Ручной
        /// </summary>
        [Description("Вручную")]
        Manual
    };

    public enum Keys
    {
    };
}
