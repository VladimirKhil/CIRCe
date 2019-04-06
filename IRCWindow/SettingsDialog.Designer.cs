namespace IRCWindow
{
    partial class SettingsDialog
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
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Сообщения");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Цвета");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Шрифты");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.pMessages = new System.Windows.Forms.Panel();
            this.pColors = new System.Windows.Forms.Panel();
            this.pFonts = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tBChannelQuitMsg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tBServerQuitMsg = new System.Windows.Forms.TextBox();
            this.tBChatFont = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bConfigureChatFont = new System.Windows.Forms.Button();
            this.pMessages.SuspendLayout();
            this.pFonts.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            treeNode4.Name = "nMessages";
            treeNode4.Text = "Сообщения";
            treeNode5.Name = "nColors";
            treeNode5.Text = "Цвета";
            treeNode6.Name = "nFonts";
            treeNode6.Text = "Шрифты";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            this.treeView1.Size = new System.Drawing.Size(193, 449);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // pMessages
            // 
            this.pMessages.Controls.Add(this.tBServerQuitMsg);
            this.pMessages.Controls.Add(this.label2);
            this.pMessages.Controls.Add(this.tBChannelQuitMsg);
            this.pMessages.Controls.Add(this.label1);
            this.pMessages.Location = new System.Drawing.Point(211, 12);
            this.pMessages.Name = "pMessages";
            this.pMessages.Size = new System.Drawing.Size(317, 449);
            this.pMessages.TabIndex = 1;
            // 
            // pColors
            // 
            this.pColors.Location = new System.Drawing.Point(211, 12);
            this.pColors.Name = "pColors";
            this.pColors.Size = new System.Drawing.Size(317, 449);
            this.pColors.TabIndex = 2;
            // 
            // pFonts
            // 
            this.pFonts.Controls.Add(this.bConfigureChatFont);
            this.pFonts.Controls.Add(this.tBChatFont);
            this.pFonts.Controls.Add(this.label3);
            this.pFonts.Location = new System.Drawing.Point(211, 12);
            this.pFonts.Name = "pFonts";
            this.pFonts.Size = new System.Drawing.Size(317, 449);
            this.pFonts.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "При выходе с канала";
            // 
            // tBChannelQuitMsg
            // 
            this.tBChannelQuitMsg.Location = new System.Drawing.Point(132, 8);
            this.tBChannelQuitMsg.Name = "tBChannelQuitMsg";
            this.tBChannelQuitMsg.Size = new System.Drawing.Size(182, 20);
            this.tBChannelQuitMsg.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "При выходе с сервера";
            // 
            // tBServerQuitMsg
            // 
            this.tBServerQuitMsg.Location = new System.Drawing.Point(132, 36);
            this.tBServerQuitMsg.Name = "tBServerQuitMsg";
            this.tBServerQuitMsg.Size = new System.Drawing.Size(182, 20);
            this.tBServerQuitMsg.TabIndex = 3;
            // 
            // tBChatFont
            // 
            this.tBChatFont.Location = new System.Drawing.Point(12, 32);
            this.tBChatFont.Name = "tBChatFont";
            this.tBChatFont.ReadOnly = true;
            this.tBChatFont.Size = new System.Drawing.Size(182, 20);
            this.tBChatFont.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Область сообщений";
            // 
            // bConfigureChatFont
            // 
            this.bConfigureChatFont.Location = new System.Drawing.Point(200, 30);
            this.bConfigureChatFont.Name = "bConfigureChatFont";
            this.bConfigureChatFont.Size = new System.Drawing.Size(114, 23);
            this.bConfigureChatFont.TabIndex = 4;
            this.bConfigureChatFont.Text = "Настроить...";
            this.bConfigureChatFont.UseVisualStyleBackColor = true;
            this.bConfigureChatFont.Click += new System.EventHandler(this.bConfigureChatFont_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 470);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.pFonts);
            this.Controls.Add(this.pColors);
            this.Controls.Add(this.pMessages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки Цирцеи";
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.pMessages.ResumeLayout(false);
            this.pMessages.PerformLayout();
            this.pFonts.ResumeLayout(false);
            this.pFonts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel pMessages;
        private System.Windows.Forms.Panel pColors;
        private System.Windows.Forms.Panel pFonts;
        private System.Windows.Forms.TextBox tBServerQuitMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBChannelQuitMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bConfigureChatFont;
        private System.Windows.Forms.TextBox tBChatFont;
        private System.Windows.Forms.Label label3;
    }
}