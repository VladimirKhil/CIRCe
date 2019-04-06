using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCWindow.Properties;

namespace IRCWindow
{
    public partial class ColorPanel : UserControl
    {
        private int printedNum = -1;
        private char lastChar = '\0';

        public char LastChar { get { return lastChar; } }

        /// <summary>
        /// Заголовок
        /// </summary>
        [Browsable(true)]
        public override string Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        [Browsable(true)]
        public bool TextVisible
        {
            get { return this.label1.Visible; }
            set { this.label1.Visible = value; }
        }

        [Browsable(true)]
        public new event EventHandler<ColorSelectedEventArgs> Select;

        public ColorPanel()
        {
            InitializeComponent();

            Refresh();
        }

        public override void Refresh()
        {
            try
            {
                dataGridView1.Rows.Clear();
                int i = 0, j = 0;
                dataGridView1.Rows.Add();
                Colors colors = Settings.Default.Colors == null ? new Colors() : Settings.Default.Colors;
                foreach (Color c in colors)
                {
                    dataGridView1.Rows[i].Cells[j].Value = i * 10 + j;
                    dataGridView1.Rows[i].Cells[j].Style.Format = "D2";
                    dataGridView1.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Rows[i].Cells[j].ReadOnly = true;
                    dataGridView1.Rows[i].Cells[j].Style.ForeColor = c.R + c.G + c.B < 255 * 3 / 2 ? Color.White : Color.Black;
                    dataGridView1.Rows[i].Cells[j++].Style.BackColor = c;
                    if (j == 10)
                    {
                        i++;
                        j = 0;
                        dataGridView1.Rows.Add();
                    }
                }
                while (dataGridView1.SelectedCells.Count > 0)
                    dataGridView1.SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Select != null && dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedCells[0].Value != null)
                Select(this, new ColorSelectedEventArgs((int)dataGridView1.SelectedCells[0].Value));
            while (dataGridView1.SelectedCells.Count > 0)
                dataGridView1.SelectedCells[0].Selected = false;
        }

        private void ColorPanel_Load(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
                dataGridView1.SelectedCells[0].Selected = false;
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                int val = int.Parse(e.KeyChar.ToString());
                if (printedNum == -1)
                    printedNum = val;
                else
                {
                    printedNum = printedNum * 10 + val;
                    if (Select != null)
                        Select(this, new ColorSelectedEventArgs(printedNum));
                    printedNum = -1;
                }
            }
            else
            {
                lastChar = e.KeyChar;
                if (Select != null)
                    Select(this, new ColorSelectedEventArgs(printedNum));
                printedNum = -1;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.FindForm().Close();
        }
    }
}
