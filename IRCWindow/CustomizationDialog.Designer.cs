namespace IRCWindow
{
    partial class CustomizationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizationDialog));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.bDefault = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AccessibleDescription = null;
            this.propertyGrid1.AccessibleName = null;
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.BackgroundImage = null;
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bOk);
            this.panel1.Controls.Add(this.bDefault);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
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
            // bDefault
            // 
            this.bDefault.AccessibleDescription = null;
            this.bDefault.AccessibleName = null;
            resources.ApplyResources(this.bDefault, "bDefault");
            this.bDefault.BackgroundImage = null;
            this.bDefault.Font = null;
            this.bDefault.Name = "bDefault";
            this.bDefault.UseVisualStyleBackColor = true;
            this.bDefault.Click += new System.EventHandler(this.bDefault_Click);
            // 
            // CustomizationDialog
            // 
            this.AcceptButton = this.bOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.bCancel;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomizationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bDefault;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOk;
    }
}