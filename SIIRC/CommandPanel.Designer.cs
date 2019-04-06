namespace SIIRC
{
    partial class CommandPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandPanel));
            this.tSBNext = new System.Windows.Forms.ToolStripButton();
            this.tSBYes = new System.Windows.Forms.ToolStripButton();
            this.tSBNo = new System.Windows.Forms.ToolStripButton();
            this.tSBReady = new System.Windows.Forms.ToolStripButton();
            this.tSDDBShow = new System.Windows.Forms.ToolStripDropDownButton();
            this.tSMUScore = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMUStat = new System.Windows.Forms.ToolStripMenuItem();
            this.tSDDBOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.tSMINewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMIClosed = new System.Windows.Forms.ToolStripMenuItem();
            this.tSMIScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tSBEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tSTBData = new System.Windows.Forms.ToolStripTextBox();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tSMITimer = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSMIConfigureTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.tSBShow = new System.Windows.Forms.ToolStripButton();
            this.tSPBTimer = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tSBNext
            // 
            this.tSBNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tSBNext, "tSBNext");
            this.tSBNext.ForeColor = System.Drawing.Color.Red;
            this.tSBNext.Name = "tSBNext";
            this.tSBNext.Click += new System.EventHandler(this.tSBNext_Click);
            // 
            // tSBYes
            // 
            this.tSBYes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tSBYes, "tSBYes");
            this.tSBYes.ForeColor = System.Drawing.Color.Red;
            this.tSBYes.Name = "tSBYes";
            this.tSBYes.Click += new System.EventHandler(this.tSBYes_Click);
            // 
            // tSBNo
            // 
            this.tSBNo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tSBNo, "tSBNo");
            this.tSBNo.ForeColor = System.Drawing.Color.Red;
            this.tSBNo.Name = "tSBNo";
            this.tSBNo.Click += new System.EventHandler(this.tSBNo_Click);
            // 
            // tSBReady
            // 
            this.tSBReady.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tSBReady, "tSBReady");
            this.tSBReady.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tSBReady.Name = "tSBReady";
            this.tSBReady.Click += new System.EventHandler(this.tSBReady_Click);
            // 
            // tSDDBShow
            // 
            this.tSDDBShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSDDBShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSMUScore,
            this.tSMUStat});
            resources.ApplyResources(this.tSDDBShow, "tSDDBShow");
            this.tSDDBShow.Name = "tSDDBShow";
            // 
            // tSMUScore
            // 
            this.tSMUScore.Name = "tSMUScore";
            resources.ApplyResources(this.tSMUScore, "tSMUScore");
            this.tSMUScore.Click += new System.EventHandler(this.tSMUScore_Click);
            // 
            // tSMUStat
            // 
            this.tSMUStat.Name = "tSMUStat";
            resources.ApplyResources(this.tSMUStat, "tSMUStat");
            this.tSMUStat.Click += new System.EventHandler(this.tSMUStat_Click);
            // 
            // tSDDBOptions
            // 
            this.tSDDBOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSDDBOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSMINewGame,
            this.tSMIClosed,
            this.tSMIScroll,
            this.tSBEnd});
            resources.ApplyResources(this.tSDDBOptions, "tSDDBOptions");
            this.tSDDBOptions.Name = "tSDDBOptions";
            // 
            // tSMINewGame
            // 
            this.tSMINewGame.Name = "tSMINewGame";
            resources.ApplyResources(this.tSMINewGame, "tSMINewGame");
            this.tSMINewGame.Click += new System.EventHandler(this.tSMINewGame_Click);
            // 
            // tSMIClosed
            // 
            this.tSMIClosed.Name = "tSMIClosed";
            resources.ApplyResources(this.tSMIClosed, "tSMIClosed");
            this.tSMIClosed.Click += new System.EventHandler(this.tSMIClosed_Click);
            // 
            // tSMIScroll
            // 
            this.tSMIScroll.Name = "tSMIScroll";
            resources.ApplyResources(this.tSMIScroll, "tSMIScroll");
            this.tSMIScroll.Click += new System.EventHandler(this.tSMIScroll_Click);
            // 
            // tSBEnd
            // 
            this.tSBEnd.Name = "tSBEnd";
            resources.ApplyResources(this.tSBEnd, "tSBEnd");
            this.tSBEnd.Click += new System.EventHandler(this.tSBEnd_Click);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSTBData,
            this.tSBNext,
            this.tSBYes,
            this.tSBNo,
            this.tsbCancel,
            this.tSBReady,
            this.tSDDBShow,
            this.tSDDBOptions,
            this.tSMITimer,
            this.tSBShow,
            this.tSPBTimer});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // tSTBData
            // 
            this.tSTBData.BackColor = System.Drawing.Color.Thistle;
            this.tSTBData.Name = "tSTBData";
            resources.ApplyResources(this.tSTBData, "tSTBData");
            // 
            // tsbCancel
            // 
            this.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCancel.ForeColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.tsbCancel, "tsbCancel");
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tSMITimer
            // 
            this.tSMITimer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tSMITimer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tSMIConfigureTimer});
            resources.ApplyResources(this.tSMITimer, "tSMITimer");
            this.tSMITimer.Name = "tSMITimer";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tSMIConfigureTimer
            // 
            this.tSMIConfigureTimer.Name = "tSMIConfigureTimer";
            resources.ApplyResources(this.tSMIConfigureTimer, "tSMIConfigureTimer");
            this.tSMIConfigureTimer.Click += new System.EventHandler(this.tSMIConfigureTimer_Click);
            // 
            // tSBShow
            // 
            this.tSBShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tSBShow, "tSBShow");
            this.tSBShow.Name = "tSBShow";
            this.tSBShow.Click += new System.EventHandler(this.tSBShow_Click);
            // 
            // tSPBTimer
            // 
            resources.ApplyResources(this.tSPBTimer, "tSPBTimer");
            this.tSPBTimer.Name = "tSPBTimer";
            this.tSPBTimer.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // CommandPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "CommandPanel";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tSBNext;
        private System.Windows.Forms.ToolStripButton tSBYes;
        private System.Windows.Forms.ToolStripButton tSBNo;
        private System.Windows.Forms.ToolStripButton tSBReady;
        private System.Windows.Forms.ToolStripDropDownButton tSDDBShow;
        private System.Windows.Forms.ToolStripMenuItem tSMUScore;
        private System.Windows.Forms.ToolStripMenuItem tSMUStat;
        private System.Windows.Forms.ToolStripTextBox tSTBData;
        private System.Windows.Forms.ToolStripDropDownButton tSDDBOptions;
        private System.Windows.Forms.ToolStripMenuItem tSMINewGame;
        private System.Windows.Forms.ToolStripMenuItem tSMIClosed;
        private System.Windows.Forms.ToolStripMenuItem tSMIScroll;
        private System.Windows.Forms.ToolStripMenuItem tSBEnd;
        private System.Windows.Forms.ToolStripDropDownButton tSMITimer;
        private System.Windows.Forms.ToolStripMenuItem tSMIConfigureTimer;
        private System.Windows.Forms.ToolStripButton tSBShow;
        private System.Windows.Forms.ToolStripProgressBar tSPBTimer;
        private System.Windows.Forms.ToolStripButton tsbCancel;

    }
}
