namespace IRCWindow
{
    partial class UninstallDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UninstallDialog));
            this.cbLogs = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAddons = new System.Windows.Forms.CheckBox();
            this.cbMedia = new System.Windows.Forms.CheckBox();
            this.bOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbLogs
            // 
            this.cbLogs.AccessibleDescription = null;
            this.cbLogs.AccessibleName = null;
            resources.ApplyResources(this.cbLogs, "cbLogs");
            this.cbLogs.BackgroundImage = null;
            this.cbLogs.Font = null;
            this.cbLogs.Name = "cbLogs";
            this.cbLogs.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // cbAddons
            // 
            this.cbAddons.AccessibleDescription = null;
            this.cbAddons.AccessibleName = null;
            resources.ApplyResources(this.cbAddons, "cbAddons");
            this.cbAddons.BackgroundImage = null;
            this.cbAddons.Font = null;
            this.cbAddons.Name = "cbAddons";
            this.cbAddons.UseVisualStyleBackColor = true;
            // 
            // cbMedia
            // 
            this.cbMedia.AccessibleDescription = null;
            this.cbMedia.AccessibleName = null;
            resources.ApplyResources(this.cbMedia, "cbMedia");
            this.cbMedia.BackgroundImage = null;
            this.cbMedia.Font = null;
            this.cbMedia.Name = "cbMedia";
            this.cbMedia.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.AccessibleDescription = null;
            this.bOk.AccessibleName = null;
            resources.ApplyResources(this.bOk, "bOk");
            this.bOk.BackgroundImage = null;
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Font = null;
            this.bOk.Name = "bOk";
            this.bOk.UseVisualStyleBackColor = true;
            // 
            // UninstallDialog
            // 
            this.AcceptButton = this.bOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.cbMedia);
            this.Controls.Add(this.cbAddons);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLogs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UninstallDialog";
            this.ShowIcon = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.UninstallDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbLogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbAddons;
        private System.Windows.Forms.CheckBox cbMedia;
        private System.Windows.Forms.Button bOk;
    }
}