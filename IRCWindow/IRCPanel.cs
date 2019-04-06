using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IRCProviders;

namespace IRCWindow
{
    /// <summary>
    /// Панель для окна IRC
    /// </summary>
    public partial class IRCPanel : Panel, IPanel
    {
        public IRCPanel()
        {
            
        }

        #region IPanel Members
        
        private delegate void ControlDel(Control c);

        /// <summary>
        /// Добавить контрол
        /// </summary>
        /// <param name="c">Добавляемый контрол</param>
        public void AddControl(Control c)
        {
            if (this.InvokeRequired)
            {
                this.EndInvoke(this.BeginInvoke(new ControlDel(AddControl), c));
            }
            else
            {
                this.Controls.Add(c);
            }
        }

        /// <summary>
        /// Удалить контрол
        /// </summary>
        /// <param name="c">Удаляемый контрол</param>
        public void RemoveControl(Control c)
        {
            if (this.InvokeRequired)
            {
                this.EndInvoke(this.BeginInvoke(new ControlDel(RemoveControl), c));
            }
            else
            {
                if (this.Controls.Contains(c))
                    this.Controls.Remove(c);
            }
        }

        #endregion
    }
}
