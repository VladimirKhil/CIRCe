using System.Collections.Generic;
using IRCWindow.Properties;
namespace IRCWindow
{
    partial class MDIChild
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIChild));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainAreaSplitContainer = new System.Windows.Forms.SplitContainer();
            this.chatPanel = new IRCWindow.IRCPanel();
            this.chatRTB = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripChat = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tSMIChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSMINick = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dgvUsers = new IRCWindow.ChannelUsersList();
            this.bsUsers = new System.Windows.Forms.BindingSource(this.components);
            this.mainAreaPanel = new IRCWindow.IRCPanel();
            this.topPanel = new IRCWindow.IRCPanel();
            this.topicPanel = new IRCWindow.IRCPanel();
            this.buttonPanel = new IRCWindow.IRCPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiStick = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAutoOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.printMessagePanel = new IRCWindow.IRCPanel();
            this.irtbPrintMessage = new IRCWindow.IRCRichTextBox();
            this.userInfoToolTip1 = new IRCWindow.UserInfoToolTip();
            ((System.ComponentModel.ISupportInitialize)(this.mainAreaSplitContainer)).BeginInit();
            this.mainAreaSplitContainer.Panel1.SuspendLayout();
            this.mainAreaSplitContainer.Panel2.SuspendLayout();
            this.mainAreaSplitContainer.SuspendLayout();
            this.chatPanel.SuspendLayout();
            this.contextMenuStripChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).BeginInit();
            this.mainAreaPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.printMessagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainAreaSplitContainer
            // 
            resources.ApplyResources(this.mainAreaSplitContainer, "mainAreaSplitContainer");
            this.mainAreaSplitContainer.Name = "mainAreaSplitContainer";
            // 
            // mainAreaSplitContainer.Panel1
            // 
            this.mainAreaSplitContainer.Panel1.Controls.Add(this.chatPanel);
            // 
            // mainAreaSplitContainer.Panel2
            // 
            this.mainAreaSplitContainer.Panel2.Controls.Add(this.dgvUsers);
            // 
            // chatPanel
            // 
            this.chatPanel.Controls.Add(this.chatRTB);
            resources.ApplyResources(this.chatPanel, "chatPanel");
            this.chatPanel.Name = "chatPanel";
            // 
            // chatRTB
            // 
            this.chatRTB.ContextMenuStrip = this.contextMenuStripChat;
            resources.ApplyResources(this.chatRTB, "chatRTB");
            this.chatRTB.Name = "chatRTB";
            this.chatRTB.ReadOnly = true;
            this.chatRTB.Text = global::IRCWindow.Properties.Resources.TooFastTargetChangeMessage;
            this.chatRTB.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.chatRTB_LinkClicked);
            this.chatRTB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chatRTB_MouseClick);
            this.chatRTB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chatRTB_MouseMove);
            this.chatRTB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chatRTB_MouseUp);
            // 
            // contextMenuStripChat
            // 
            this.contextMenuStripChat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSMIChannels,
            this.tSMINick});
            this.contextMenuStripChat.Name = "contextMenuStrip2";
            resources.ApplyResources(this.contextMenuStripChat, "contextMenuStripChat");
            // 
            // tSMIChannels
            // 
            this.tSMIChannels.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1});
            this.tSMIChannels.Image = global::IRCWindow.Properties.Resources.channel;
            this.tSMIChannels.Name = "tSMIChannels";
            resources.ApplyResources(this.tSMIChannels, "tSMIChannels");
            this.tSMIChannels.DropDownOpening += new System.EventHandler(this.tSMIChannels_DropDownOpening);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tSMINick
            // 
            this.tSMINick.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2});
            this.tSMINick.Image = global::IRCWindow.Properties.Resources.selectUser;
            this.tSMINick.Name = "tSMINick";
            resources.ApplyResources(this.tSMINick, "tSMINick");
            this.tSMINick.DropDownOpening += new System.EventHandler(this.tSMINick_DropDownOpening);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeColumns = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.AutoGenerateColumns = false;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvUsers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvUsers.CellsBackgroundColor = System.Drawing.Color.GhostWhite;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.ColumnHeadersVisible = false;
            this.dgvUsers.DataSource = this.bsUsers;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.GhostWhite;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvUsers, "dgvUsers");
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.ShowCellToolTips = false;
            this.dgvUsers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvUsers_DataError);
            // 
            // mainAreaPanel
            // 
            this.mainAreaPanel.Controls.Add(this.mainAreaSplitContainer);
            resources.ApplyResources(this.mainAreaPanel, "mainAreaPanel");
            this.mainAreaPanel.Name = "mainAreaPanel";
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.topicPanel);
            this.topPanel.Controls.Add(this.buttonPanel);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // topicPanel
            // 
            resources.ApplyResources(this.topicPanel, "topicPanel");
            this.topicPanel.Name = "topicPanel";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            this.buttonPanel.Name = "buttonPanel";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton2});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStick,
            this.tsmiAutoOpen});
            this.toolStripDropDownButton2.Image = global::IRCWindow.Properties.Resources.star;
            resources.ApplyResources(this.toolStripDropDownButton2, "toolStripDropDownButton2");
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            // 
            // tsmiStick
            // 
            this.tsmiStick.CheckOnClick = true;
            this.tsmiStick.Name = "tsmiStick";
            resources.ApplyResources(this.tsmiStick, "tsmiStick");
            // 
            // tsmiAutoOpen
            // 
            this.tsmiAutoOpen.CheckOnClick = true;
            this.tsmiAutoOpen.Name = "tsmiAutoOpen";
            resources.ApplyResources(this.tsmiAutoOpen, "tsmiAutoOpen");
            // 
            // printMessagePanel
            // 
            this.printMessagePanel.Controls.Add(this.irtbPrintMessage);
            resources.ApplyResources(this.printMessagePanel, "printMessagePanel");
            this.printMessagePanel.Name = "printMessagePanel";
            // 
            // irtbPrintMessage
            // 
            this.irtbPrintMessage.DefaultColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.irtbPrintMessage, "irtbPrintMessage");
            this.irtbPrintMessage.EditOnDoubleClick = false;
            this.irtbPrintMessage.InnerFont = new System.Drawing.Font("Arial", 12.75F);
            this.irtbPrintMessage.Name = "irtbPrintMessage";
            this.irtbPrintMessage.ReadOnly = false;
            this.irtbPrintMessage.SelectionBackColor = System.Drawing.Color.GhostWhite;
            this.irtbPrintMessage.SelectionColor = System.Drawing.Color.Black;
            this.irtbPrintMessage.SelectionLength = 0;
            this.irtbPrintMessage.SelectionStart = 0;
            this.irtbPrintMessage.ToolTip = global::IRCWindow.Properties.Resources.TooFastTargetChangeMessage;
            this.irtbPrintMessage.WorkMode = IRCWindow.IRCRichTextBox.Mode.Normal;
            this.irtbPrintMessage.InputKeyDown += new System.Windows.Forms.KeyEventHandler(this.printMessageIrcRichTextBox_InputKeyDown);
            this.irtbPrintMessage.InputKeyPress += new System.Windows.Forms.KeyPressEventHandler(this.printMessageIrcRichTextBox_InputKeyPress);
            this.irtbPrintMessage.InnerMouseWheel += new System.Windows.Forms.MouseEventHandler(this.irtbPrintMessage_InnerMouseWheel);
            // 
            // userInfoToolTip1
            // 
            this.userInfoToolTip1.AutoPopDelay = 4000;
            this.userInfoToolTip1.InitialDelay = 200;
            this.userInfoToolTip1.OwnerDraw = true;
            this.userInfoToolTip1.ReshowDelay = 1000;
            this.userInfoToolTip1.ShowAlways = true;
            this.userInfoToolTip1.UseAnimation = false;
            // 
            // MDIChild
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStripChat;
            this.Controls.Add(this.mainAreaPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.printMessagePanel);
            this.KeyPreview = true;
            this.Name = "MDIChild";
            this.ShowIcon = false;
            this.Activated += new System.EventHandler(this.MDIChild_Activated);
            this.Deactivate += new System.EventHandler(this.MDIChild_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MDIChild_FormClosed);
            this.Load += new System.EventHandler(this.MDIChild_Load);
            this.mainAreaSplitContainer.Panel1.ResumeLayout(false);
            this.mainAreaSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainAreaSplitContainer)).EndInit();
            this.mainAreaSplitContainer.ResumeLayout(false);
            this.chatPanel.ResumeLayout(false);
            this.contextMenuStripChat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).EndInit();
            this.mainAreaPanel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.printMessagePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected IRCWindow.IRCPanel printMessagePanel;
        protected System.Windows.Forms.SplitContainer mainAreaSplitContainer;
        protected IRCWindow.IRCPanel chatPanel;
        protected System.Windows.Forms.ContextMenuStrip contextMenuStripChat;
        protected IRCWindow.IRCPanel topicPanel;
        protected IRCWindow.IRCPanel mainAreaPanel;
        protected IRCWindow.IRCRichTextBox irtbPrintMessage;
        protected System.Windows.Forms.ToolStripMenuItem tSMIChannels;
        protected System.Windows.Forms.ToolStripMenuItem tSMINick;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        protected System.Windows.Forms.RichTextBox chatRTB;
        protected IRCWindow.ChannelUsersList dgvUsers;
        protected System.Windows.Forms.BindingSource bsUsers;
        protected IRCWindow.UserInfoToolTip userInfoToolTip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        protected IRCWindow.IRCPanel topPanel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        protected System.Windows.Forms.ToolStripMenuItem tsmiStick;
        protected System.Windows.Forms.ToolStripMenuItem tsmiAutoOpen;
        protected IRCWindow.IRCPanel buttonPanel;
    }
}

