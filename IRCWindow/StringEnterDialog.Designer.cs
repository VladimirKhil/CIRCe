namespace IRCWindow
{
    partial class StringEnterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StringEnterDialog));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AccessibleDescription = null;
            this.textBox1.AccessibleName = null;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.BackgroundImage = null;
            this.textBox1.Font = null;
            this.textBox1.Name = "textBox1";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // bOK
            // 
            this.bOK.AccessibleDescription = null;
            this.bOK.AccessibleName = null;
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.BackgroundImage = null;
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Font = null;
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.AccessibleDescription = null;
            this.bCancel.AccessibleName = null;
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.BackgroundImage = null;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Font = null;
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // StringEnterDialog
            // 
            this.AcceptButton = this.bOK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.bCancel;
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StringEnterDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
    }
}