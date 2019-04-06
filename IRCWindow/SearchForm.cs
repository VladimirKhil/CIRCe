using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;

namespace IRCWindow
{
    /// <summary>
    /// Поисковая форма
    /// </summary>
    public sealed partial class SearchForm : IRCForm
    {
        private RichTextBox richTextBox = null;
        private TextStyleSaver saver = new TextStyleSaver();

        /// <summary>
        /// Создание поисковой формы
        /// </summary>
        /// <param name="richTextBox">Текстовый редактор, в котором будет производиться поиск</param>
        public SearchForm(RichTextBox richTextBox)
        {
            InitializeComponent();

            this.richTextBox = richTextBox;
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds options = RichTextBoxFinds.Reverse;
            if (cbCaseSensitive.Checked)
                options |= RichTextBoxFinds.MatchCase;
            if (cbWholeWords.Checked)
                options |= RichTextBoxFinds.WholeWord;
            int end = Math.Min(this.richTextBox.Text.Length, Math.Max(0, this.richTextBox.SelectionStart - 1));
            int res = richTextBox.Find(this.tbSearch.Text, 0, end, options);
            if (res == -1)
                res = richTextBox.Find(this.tbSearch.Text, end, this.richTextBox.Text.Length, options);
            Highlight(res);
        }

        /// <summary>
        /// Подсветить результат поиска
        /// </summary>
        /// <param name="start">Начало подсветки (длина определяется длиной поискового запроса)</param>
        private void Highlight(int start)
        {
            if (start > -1)
            {
                if (saver.Left > -1)
                    saver.Paste();
                saver.CopyFrom(this.richTextBox, start, start + this.tbSearch.Text.Length);
                this.richTextBox.SelectionBackColor = Color.Lavender;
                richTextBox.ScrollToCaret();
            }
        }

        private void bGoDown_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds options = RichTextBoxFinds.None;
            if (cbCaseSensitive.Checked)
                options |= RichTextBoxFinds.MatchCase;
            if (cbWholeWords.Checked)
                options |= RichTextBoxFinds.WholeWord;
            int start = Math.Min(this.richTextBox.Text.Length, Math.Max(0, this.richTextBox.SelectionStart + 1));
            int res = richTextBox.Find(this.tbSearch.Text, start, this.richTextBox.Text.Length, options);
            if (res == -1)
                res = richTextBox.Find(this.tbSearch.Text, 0, start, options);                
            Highlight(res);
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    // perform necessary action
                    e.Handled = true;
                    bGo_Click(sender, e);
                    this.tbSearch.Focus();
                    break;
            }
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (richTextBox != null && !richTextBox.IsDisposed)
            {
                int pos = IRCRichTextBox.GetVScrollPos(richTextBox);
                if (saver.Left > -1)
                    saver.Paste();
                this.richTextBox.DeselectAll();
                IRCRichTextBox.SetVScrollPos(richTextBox, pos);
            }
        }

        private void SearchForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        #region IVisual Members

        public void Deactivated()
        {
            
        }

        #endregion
    }
}
