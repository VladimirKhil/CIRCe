namespace IRCWindow
{
    partial class MDIChildServer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIChildServer));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmsServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAway = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCmd = new System.Windows.Forms.ToolStripMenuItem();
            this.администраторыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTime = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.списокКаналовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPing = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGMode = new System.Windows.Forms.ToolStripMenuItem();
            this.задатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.снятьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStats = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLiveTime = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCommands = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.printMessagePanel.SuspendLayout();
            this.mainAreaSplitContainer.Panel1.SuspendLayout();
            this.mainAreaSplitContainer.SuspendLayout();
            this.chatPanel.SuspendLayout();
            this.mainAreaPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).BeginInit();
            this.topPanel.SuspendLayout();
            this.cmsServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // printMessagePanel
            // 
            resources.ApplyResources(this.printMessagePanel, "printMessagePanel");
            // 
            // mainAreaSplitContainer
            // 
            resources.ApplyResources(this.mainAreaSplitContainer, "mainAreaSplitContainer");
            // 
            // chatPanel
            // 
            resources.ApplyResources(this.chatPanel, "chatPanel");
            // 
            // topicPanel
            // 
            resources.ApplyResources(this.topicPanel, "topicPanel");
            // 
            // mainAreaPanel
            // 
            resources.ApplyResources(this.mainAreaPanel, "mainAreaPanel");
            // 
            // irtbPrintMessage
            // 
            resources.ApplyResources(this.irtbPrintMessage, "irtbPrintMessage");
            this.irtbPrintMessage.AcceptEdit += new System.EventHandler<IRCWindow.EnterPushedEventArgs>(this.printMessageIrcRichTextBox_EditFinished);
            // 
            // chatRTB
            // 
            resources.ApplyResources(this.chatRTB, "chatRTB");
            // 
            // topPanel
            // 
            resources.ApplyResources(this.topPanel, "topPanel");
            // 
            // buttonPanel
            // 
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmsServer
            // 
            this.cmsServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tsmiAway,
            this.toolStripSeparator2,
            this.tsmiCmd,
            this.tsmiStats,
            this.toolStripSeparator3,
            this.tsmiReconnect});
            this.cmsServer.Name = "cmsServer";
            resources.ApplyResources(this.cmsServer, "cmsServer");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsmiAway
            // 
            this.tsmiAway.Name = "tsmiAway";
            resources.ApplyResources(this.tsmiAway, "tsmiAway");
            this.tsmiAway.Click += new System.EventHandler(this.tsmiAway_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsmiCmd
            // 
            this.tsmiCmd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.администраторыToolStripMenuItem,
            this.tsmiTime,
            this.tsmiInfo,
            this.списокКаналовToolStripMenuItem,
            this.tsmiPing,
            this.tsmiGMode});
            this.tsmiCmd.Name = "tsmiCmd";
            resources.ApplyResources(this.tsmiCmd, "tsmiCmd");
            // 
            // администраторыToolStripMenuItem
            // 
            this.администраторыToolStripMenuItem.Name = "администраторыToolStripMenuItem";
            resources.ApplyResources(this.администраторыToolStripMenuItem, "администраторыToolStripMenuItem");
            this.администраторыToolStripMenuItem.Click += new System.EventHandler(this.tsmiAdmin_Click);
            // 
            // tsmiTime
            // 
            this.tsmiTime.Name = "tsmiTime";
            resources.ApplyResources(this.tsmiTime, "tsmiTime");
            this.tsmiTime.Click += new System.EventHandler(this.tsmiTime_Click);
            // 
            // tsmiInfo
            // 
            this.tsmiInfo.Name = "tsmiInfo";
            resources.ApplyResources(this.tsmiInfo, "tsmiInfo");
            this.tsmiInfo.Click += new System.EventHandler(this.tsmiInfo_Click);
            // 
            // списокКаналовToolStripMenuItem
            // 
            this.списокКаналовToolStripMenuItem.Name = "списокКаналовToolStripMenuItem";
            resources.ApplyResources(this.списокКаналовToolStripMenuItem, "списокКаналовToolStripMenuItem");
            this.списокКаналовToolStripMenuItem.Click += new System.EventHandler(this.tsmiList_Click);
            // 
            // tsmiPing
            // 
            this.tsmiPing.Name = "tsmiPing";
            resources.ApplyResources(this.tsmiPing, "tsmiPing");
            this.tsmiPing.Click += new System.EventHandler(this.tsmiPing_Click);
            // 
            // tsmiGMode
            // 
            this.tsmiGMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.задатьToolStripMenuItem,
            this.снятьToolStripMenuItem});
            this.tsmiGMode.Name = "tsmiGMode";
            resources.ApplyResources(this.tsmiGMode, "tsmiGMode");
            // 
            // задатьToolStripMenuItem
            // 
            this.задатьToolStripMenuItem.Name = "задатьToolStripMenuItem";
            resources.ApplyResources(this.задатьToolStripMenuItem, "задатьToolStripMenuItem");
            // 
            // снятьToolStripMenuItem
            // 
            this.снятьToolStripMenuItem.Name = "снятьToolStripMenuItem";
            resources.ApplyResources(this.снятьToolStripMenuItem, "снятьToolStripMenuItem");
            // 
            // tsmiStats
            // 
            this.tsmiStats.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLiveTime,
            this.tsmiCommands});
            this.tsmiStats.Name = "tsmiStats";
            resources.ApplyResources(this.tsmiStats, "tsmiStats");
            // 
            // tsmiLiveTime
            // 
            this.tsmiLiveTime.Name = "tsmiLiveTime";
            resources.ApplyResources(this.tsmiLiveTime, "tsmiLiveTime");
            this.tsmiLiveTime.Click += new System.EventHandler(this.tsmiLiveTime_Click);
            // 
            // tsmiCommands
            // 
            this.tsmiCommands.Name = "tsmiCommands";
            resources.ApplyResources(this.tsmiCommands, "tsmiCommands");
            this.tsmiCommands.Click += new System.EventHandler(this.tsmiCommands_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsmiReconnect
            // 
            this.tsmiReconnect.Name = "tsmiReconnect";
            resources.ApplyResources(this.tsmiReconnect, "tsmiReconnect");
            this.tsmiReconnect.Click += new System.EventHandler(this.tsmiReconnect_Click);
            // 
            // MDIChildServer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MDIChildServer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MDIChildServer_FormClosed);
            this.Activated += new System.EventHandler(this.MDIChildServer_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MDIChildServer_KeyDown);
            this.Controls.SetChildIndex(this.printMessagePanel, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.mainAreaPanel, 0);
            this.printMessagePanel.ResumeLayout(false);
            this.mainAreaSplitContainer.Panel1.ResumeLayout(false);
            this.mainAreaSplitContainer.ResumeLayout(false);
            this.chatPanel.ResumeLayout(false);
            this.mainAreaPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.cmsServer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip cmsServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAway;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiReconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiCmd;
        private System.Windows.Forms.ToolStripMenuItem администраторыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокКаналовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiTime;
        private System.Windows.Forms.ToolStripMenuItem tsmiInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiGMode;
        private System.Windows.Forms.ToolStripMenuItem задатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem снятьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiPing;
        private System.Windows.Forms.ToolStripMenuItem tsmiStats;
        private System.Windows.Forms.ToolStripMenuItem tsmiLiveTime;
        private System.Windows.Forms.ToolStripMenuItem tsmiCommands;
    }
}