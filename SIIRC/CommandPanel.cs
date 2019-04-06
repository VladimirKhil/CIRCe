using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SIIRC
{
    [Serializable]
    public sealed partial class CommandPanel : UserControl
    {
        public event EventHandler Next;
        public event EventHandler Yes;
        public event EventHandler No;
        public event EventHandler Ready;
        public event EventHandler ShowMain;
        public event EventHandler ShowScore;
        public event EventHandler ShowStats;
        public event EventHandler NewGame;
        public event EventHandler Closed;
        public new event EventHandler Select;
        public event EventHandler End;
        public event EventHandler ConfigureTimer;
        public event EventHandler Cancel;

        public bool ShowVisible { get { return this.tSBShow.Visible; } set { this.tSBShow.Visible = value; } }
        public bool InputSumVisible { get { return this.tSTBData.Visible; } set { this.tSTBData.Visible = value; } }
        public bool NextVisible { get { return this.tSBNext.Visible; } set { this.tSBNext.Visible = value; } }
        public bool YesVisible { get { return this.tSBYes.Visible; } set { this.tSBYes.Visible = value; } }
        public bool NoVisible { get { return this.tSBNo.Visible; } set { this.tSBNo.Visible = value; } }
        public bool CancelVisible { get { return this.tsbCancel.Visible; } set { this.tsbCancel.Visible = value; } }
        public string NextText { get { return this.tSBNext.Text; } set { this.tSBNext.Text = value; } }
        public bool ClosedChecked { get { return this.tSMIClosed.Checked; } set { this.tSMIClosed.Checked = value; } }
        public ToolStripItemCollection TimerDropDownItems { get { return this.tSMITimer.DropDownItems; } }
        public string InputSum { get { return this.tSTBData.Text; } set { this.tSTBData.Text = value; } }
        public int TimerMaximum { get { return this.tSPBTimer.Maximum; } set { this.tSPBTimer.Maximum = value; } }
        public int TimerValue { get { return this.tSPBTimer.Value; } set { this.tSPBTimer.Value = value; } }

        public CommandPanel()
        {
            InitializeComponent();
        }

        private void tSBEnd_Click(object sender, EventArgs e)
        {
            if (End != null) End(sender, e);
        }

        private void tSBShow_Click(object sender, EventArgs e)
        {
            if (ShowMain != null) ShowMain(sender, e);
        }

        private void tSBNext_Click(object sender, EventArgs e)
        {
            if (Next != null) Next(sender, e);
        }

        private void tSBYes_Click(object sender, EventArgs e)
        {
            if (Yes != null) Yes(sender, e);
        }

        private void tSBNo_Click(object sender, EventArgs e)
        {
            if (No != null) No(sender, e);
        }

        private void tSBReady_Click(object sender, EventArgs e)
        {
            if (Ready != null) Ready(sender, e);
        }

        private void tSMUScore_Click(object sender, EventArgs e)
        {
            if (ShowScore != null) ShowScore(sender, e);
        }

        private void tSMUStat_Click(object sender, EventArgs e)
        {
            if (ShowStats != null) ShowStats(sender, e);
        }

        private void tSMINewGame_Click(object sender, EventArgs e)
        {
            if (NewGame != null) NewGame(sender, e);
        }

        private void tSMIClosed_Click(object sender, EventArgs e)
        {
            if (Closed != null) Closed(sender, e);
        }

        private void tSMIScroll_Click(object sender, EventArgs e)
        {
            if (Select != null) Select(sender, e);
        }

        private void tSMIConfigureTimer_Click(object sender, EventArgs e)
        {
            if (ConfigureTimer != null) ConfigureTimer(sender, e);
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            if (Cancel != null) Cancel(sender, e);
        }
    }
}
