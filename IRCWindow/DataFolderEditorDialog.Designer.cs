namespace IRCWindow
{
    partial class DataFolderEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataFolderEditorDialog));
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.cbMoveData = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bSelectDir = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // bOk
            // 
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.bOk, "bOk");
            this.bOk.Name = "bOk";
            this.bOk.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // cbMoveData
            // 
            resources.ApplyResources(this.cbMoveData, "cbMoveData");
            this.cbMoveData.Checked = true;
            this.cbMoveData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMoveData.Name = "cbMoveData";
            this.cbMoveData.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // bSelectDir
            // 
            resources.ApplyResources(this.bSelectDir, "bSelectDir");
            this.bSelectDir.Name = "bSelectDir";
            this.bSelectDir.UseVisualStyleBackColor = true;
            this.bSelectDir.Click += new System.EventHandler(this.bSelectDir_Click);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // DataFolderEditorDialog
            // 
            this.AcceptButton = this.bOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.Controls.Add(this.bSelectDir);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbMoveData);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataFolderEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.DataEditorDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.CheckBox cbMoveData;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bSelectDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}