using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIIRC
{
    public partial class TimerDurationDialog : Form
    {
        private GameConfiguration config = null;

        public TimerDurationDialog(GameConfiguration config)
        {
            InitializeComponent();

            this.config = config;

            ((ISupportInitialize)this.dataGridView1).BeginInit();
            foreach (int item in config.TimerDurations)
            {
                this.dataGridView1.Rows.Add(new object[] { item });
            }           
            ((ISupportInitialize)this.dataGridView1).EndInit();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            config.TimerDurations.Clear();
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            {
                int value = 0;
                if (int.TryParse(this.dataGridView1.Rows[i].Cells[0].Value.ToString(), out value))
                    if  (!config.TimerDurations.Contains(value) && value > 0)
                        config.TimerDurations.Add(value);
            }
            config.TimerDurations.Sort();
        }
    }
}
