using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Диалог для ввода числа
    /// </summary>
    public partial class InputNumDialog : Form
    {
        /// <summary>
        /// Выбранное значение
        /// </summary>
        public int Value
        {
            get { return (int)numericUpDown1.Value; }
        }
        
        /// <summary>
        /// Создание диалога
        /// </summary>
        /// <param name="title">Текст заголовка</param>
        public InputNumDialog(string title)
        {
            InitializeComponent();

            this.Text = title;
        }
    }
}
