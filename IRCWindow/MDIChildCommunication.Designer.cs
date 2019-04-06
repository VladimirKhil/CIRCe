namespace IRCWindow
{
    partial class MDIChildCommunication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIChildCommunication));
            this.printMessagePanel.SuspendLayout();
            this.mainAreaSplitContainer.Panel1.SuspendLayout();
            this.mainAreaSplitContainer.SuspendLayout();
            this.chatPanel.SuspendLayout();
            this.mainAreaPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // printMessagePanel
            // 
            this.printMessagePanel.AccessibleDescription = null;
            this.printMessagePanel.AccessibleName = null;
            resources.ApplyResources(this.printMessagePanel, "printMessagePanel");
            this.printMessagePanel.BackgroundImage = null;
            this.printMessagePanel.Font = null;
            this.userInfoToolTip1.SetToolTip(this.printMessagePanel, resources.GetString("printMessagePanel.ToolTip"));
            // 
            // mainAreaSplitContainer
            // 
            this.mainAreaSplitContainer.AccessibleDescription = null;
            this.mainAreaSplitContainer.AccessibleName = null;
            resources.ApplyResources(this.mainAreaSplitContainer, "mainAreaSplitContainer");
            this.mainAreaSplitContainer.BackgroundImage = null;
            this.mainAreaSplitContainer.Font = null;
            // 
            // mainAreaSplitContainer.Panel1
            // 
            this.mainAreaSplitContainer.Panel1.AccessibleDescription = null;
            this.mainAreaSplitContainer.Panel1.AccessibleName = null;
            resources.ApplyResources(this.mainAreaSplitContainer.Panel1, "mainAreaSplitContainer.Panel1");
            this.mainAreaSplitContainer.Panel1.BackgroundImage = null;
            this.mainAreaSplitContainer.Panel1.Font = null;
            this.userInfoToolTip1.SetToolTip(this.mainAreaSplitContainer.Panel1, resources.GetString("mainAreaSplitContainer.Panel1.ToolTip"));
            // 
            // mainAreaSplitContainer.Panel2
            // 
            this.mainAreaSplitContainer.Panel2.AccessibleDescription = null;
            this.mainAreaSplitContainer.Panel2.AccessibleName = null;
            resources.ApplyResources(this.mainAreaSplitContainer.Panel2, "mainAreaSplitContainer.Panel2");
            this.mainAreaSplitContainer.Panel2.BackgroundImage = null;
            this.mainAreaSplitContainer.Panel2.Font = null;
            this.userInfoToolTip1.SetToolTip(this.mainAreaSplitContainer.Panel2, resources.GetString("mainAreaSplitContainer.Panel2.ToolTip"));
            this.userInfoToolTip1.SetToolTip(this.mainAreaSplitContainer, resources.GetString("mainAreaSplitContainer.ToolTip"));
            // 
            // chatPanel
            // 
            this.chatPanel.AccessibleDescription = null;
            this.chatPanel.AccessibleName = null;
            resources.ApplyResources(this.chatPanel, "chatPanel");
            this.chatPanel.BackgroundImage = null;
            this.chatPanel.Font = null;
            this.userInfoToolTip1.SetToolTip(this.chatPanel, resources.GetString("chatPanel.ToolTip"));
            // 
            // topicPanel
            // 
            this.topicPanel.AccessibleDescription = null;
            this.topicPanel.AccessibleName = null;
            resources.ApplyResources(this.topicPanel, "topicPanel");
            this.topicPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.topicPanel.BackgroundImage = null;
            this.topicPanel.Font = null;
            this.userInfoToolTip1.SetToolTip(this.topicPanel, resources.GetString("topicPanel.ToolTip"));
            // 
            // mainAreaPanel
            // 
            this.mainAreaPanel.AccessibleDescription = null;
            this.mainAreaPanel.AccessibleName = null;
            resources.ApplyResources(this.mainAreaPanel, "mainAreaPanel");
            this.mainAreaPanel.BackgroundImage = null;
            this.mainAreaPanel.Font = null;
            this.userInfoToolTip1.SetToolTip(this.mainAreaPanel, resources.GetString("mainAreaPanel.ToolTip"));
            // 
            // irtbPrintMessage
            // 
            this.irtbPrintMessage.AccessibleDescription = null;
            this.irtbPrintMessage.AccessibleName = null;
            resources.ApplyResources(this.irtbPrintMessage, "irtbPrintMessage");
            this.irtbPrintMessage.BackgroundImage = null;
            this.userInfoToolTip1.SetToolTip(this.irtbPrintMessage, resources.GetString("irtbPrintMessage.ToolTip"));
            this.irtbPrintMessage.AcceptEdit += new System.EventHandler<IRCWindow.EnterPushedEventArgs>(this.printMessageIrcRichTextBox_EditFinished);
            // 
            // chatRTB
            // 
            this.chatRTB.AccessibleDescription = null;
            this.chatRTB.AccessibleName = null;
            resources.ApplyResources(this.chatRTB, "chatRTB");
            this.chatRTB.BackgroundImage = null;
            this.chatRTB.Text = global::IRCWindow.Properties.Resources.TooFastTargetChangeMessage;
            this.userInfoToolTip1.SetToolTip(this.chatRTB, resources.GetString("chatRTB.ToolTip"));
            this.chatRTB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chatRTB_MouseClick);
            this.chatRTB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chatRTB_MouseMove);
            // 
            // topPanel
            // 
            this.topPanel.AccessibleDescription = null;
            this.topPanel.AccessibleName = null;
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.BackgroundImage = null;
            this.topPanel.Font = null;
            this.userInfoToolTip1.SetToolTip(this.topPanel, resources.GetString("topPanel.ToolTip"));
            // 
            // buttonPanel
            // 
            this.buttonPanel.AccessibleDescription = null;
            this.buttonPanel.AccessibleName = null;
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            this.buttonPanel.BackgroundImage = null;
            this.buttonPanel.Font = null;
            this.userInfoToolTip1.SetToolTip(this.buttonPanel, resources.GetString("buttonPanel.ToolTip"));
            // 
            // MDIChildCommunication
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Icon = null;
            this.Name = "MDIChildCommunication";
            this.userInfoToolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.MDIChildCommunication_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MDIChildCommunication_FormClosing);
            this.Activated += new System.EventHandler(this.MDIChildCommunication_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MDIChildCommunication_KeyDown);
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
            this.ResumeLayout(false);

        }

        #endregion

    }
}