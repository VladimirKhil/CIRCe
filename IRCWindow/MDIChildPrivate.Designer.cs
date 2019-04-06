namespace IRCWindow
{
    partial class MDIChildPrivate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIChildPrivate));
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
            resources.ApplyResources(this.printMessagePanel, "printMessagePanel");
            // 
            // mainAreaSplitContainer
            // 
            // 
            // topicPanel
            // 
            resources.ApplyResources(this.topicPanel, "topicPanel");
            // 
            // buttonPanel
            // 
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            // 
            // MDIChildPrivate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MDIChildPrivate";
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