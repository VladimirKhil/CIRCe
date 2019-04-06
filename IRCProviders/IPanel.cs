using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IRCProviders
{
    /// <summary>
    /// Интерфейс, обеспечивающий потокобезопасный доступ к функциям панели
    /// </summary>
    public interface IPanel
    {
        #region Methods

        /// <summary>
        /// Добавить контрол
        /// </summary>
        /// <param name="c"></param>
        void AddControl(Control c);
        /// <summary>
        /// Удалить контрол
        /// </summary>
        /// <param name="c"></param>
        void RemoveControl(Control c);

        #endregion
    }
}
