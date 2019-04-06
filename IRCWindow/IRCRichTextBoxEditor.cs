using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Редактор IRC
    /// </summary>
    public partial class IRCRichTextBoxEditor : IRCRichTextBox
    {
        public IRCRichTextBoxEditor()
        {
            InitializeComponent();

            this.toolStrip1.Dock = DockStyle.Top;
            this.WorkMode = Mode.Editor; 
            this.richTextBox1.AutoWordSelection = false;
            this.richTextBox1.SelectAll();
            this.richTextBox1.SelectionBackColor = Color.Snow;
        }

        private void bCopyCode_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.UnicodeText, string.Join(Environment.NewLine, GenerateCodes(this.richTextBox1)));
        }
    }
}
