namespace IRCWindow
{
    partial class ColorChooseDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorChooseDialog));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.colorPanel1 = new IRCWindow.ColorPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = null;
            this.toolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackgroundImage = null;
            this.toolStrip1.Font = null;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.AccessibleDescription = null;
            this.newToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.newToolStripButton, "newToolStripButton");
            this.newToolStripButton.BackgroundImage = null;
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Click += new System.EventHandler(this.defButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.AccessibleDescription = null;
            this.openToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.openToolStripButton, "openToolStripButton");
            this.openToolStripButton.BackgroundImage = null;
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.AccessibleDescription = null;
            this.saveToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.saveToolStripButton, "saveToolStripButton");
            this.saveToolStripButton.BackgroundImage = null;
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Click += new System.EventHandler(this.bSave_Click);
            // 
            // colorPanel1
            // 
            this.colorPanel1.AccessibleDescription = null;
            this.colorPanel1.AccessibleName = null;
            resources.ApplyResources(this.colorPanel1, "colorPanel1");
            this.colorPanel1.BackgroundImage = null;
            this.colorPanel1.Font = null;
            this.colorPanel1.Name = "colorPanel1";
            this.colorPanel1.TextVisible = true;
            this.colorPanel1.Select += new System.EventHandler<IRCWindow.ColorSelectedEventArgs>(this.colorPanel1_Select);
            // 
            // ColorChooseDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.colorPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorChooseDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ColorChooseDialog_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorPanel colorPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
    }
}