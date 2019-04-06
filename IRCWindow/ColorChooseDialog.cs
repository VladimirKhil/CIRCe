using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCWindow.Properties;
using System.Resources;

namespace IRCWindow
{
    /// <summary>
    /// Диалог для выбора цвета
    /// </summary>
    public partial class ColorChooseDialog : Form
    {
        /// <summary>
        /// Режим работы
        /// </summary>
        public enum Mode 
        { 
            /// <summary>
            /// Редактирование палитры
            /// </summary>
            Edit,
            /// <summary>
            /// Выбор цвета
            /// </summary>
            Work 
        };

        private int colorCode = -1;
        private Mode mode = Mode.Work;

        public char LastChar { get { return this.colorPanel1.LastChar; } }

        /// <summary>
        /// Режим работы диалога
        /// </summary>
        public Mode WorkMode
        {
            get { return mode; }
            set { mode = value; SetMode(); }
        }

        /// <summary>
        /// Код выбранного цвета
        /// </summary>
        public int ColorCode
        {
            get { return colorCode; }
        }

        /// <summary>
        /// Заголовок палитры цветов
        /// </summary>
        public string Title
        {
            get { return colorPanel1.Text; }
        }

        public event EventHandler<ColorSelectedEventArgs> Selected;
        
        public ColorChooseDialog(string title)
        {
            InitializeComponent();

            this.colorPanel1.Text = title;
            this.colorPanel1.Focus();
            SetMode();
        }

        private void SetMode()
        {
            if (mode == Mode.Edit)
            {
                this.toolStrip1.Visible = true;
                this.Height += this.toolStrip1.Height;
            }
        }

        void bLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = Resources.ColorPaletteFiles + "|*.col";
            dialog.DefaultExt = "*.col";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.Colors.Load(dialog.FileName);
                this.colorPanel1.Refresh();
            }
        }

        void bSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Resources.ColorPaletteFiles + "|*.col";
            dialog.DefaultExt = "*.col";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.Colors.Save(dialog.FileName);
            }
        }
        
        /// <summary>
        /// Обновить палитру
        /// </summary>
        public void RefreshColors()
        {
            this.colorPanel1.Refresh();
        }

        void defButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Colors.MakeDefault();
            RefreshColors();
        }

        private void colorPanel1_Select(object sender, ColorSelectedEventArgs e)
        {
            switch (mode)
            {
                case Mode.Work:
                    if (Selected != null)
                        Selected(this, e);
                    Close();
                    break;

                case Mode.Edit:
                    if (e.ChoosenColor == -1)
                        return;
                    ColorDialog colorDialog = new ColorDialog();
                    colorDialog.Color = Settings.Default.Colors[e.ChoosenColor];
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        Settings.Default.Colors[e.ChoosenColor] = colorDialog.Color;
                        RefreshColors();
                    }
                    break;
            }
        }

        private void ColorChooseDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
