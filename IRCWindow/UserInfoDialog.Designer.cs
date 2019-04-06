namespace IRCWindow
{
    partial class UserInfoDialog
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
            this.userInfoPanel1 = new IRCWindow.UserInfoPanel();
            this.SuspendLayout();
            // 
            // userInfoPanel1
            // 
            this.userInfoPanel1.BackColor = System.Drawing.Color.Lavender;
            this.userInfoPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userInfoPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userInfoPanel1.Location = new System.Drawing.Point(0, 0);
            this.userInfoPanel1.Name = "userInfoPanel1";
            this.userInfoPanel1.Size = new System.Drawing.Size(354, 298);
            this.userInfoPanel1.TabIndex = 0;
            // 
            // UserInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 298);
            this.Controls.Add(this.userInfoPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInfoDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserInfoDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private UserInfoPanel userInfoPanel1;
    }
}