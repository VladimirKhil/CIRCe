namespace IRCWindow
{
    partial class IRCRichTextBoxEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRCRichTextBoxEditor));
            this.panel3 = new System.Windows.Forms.Panel();
            this.bCopyCode = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.SetChildIndex(this.panel3, 0);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.bCopyCode);
            this.panel3.Name = "panel3";
            // 
            // bCopyCode
            // 
            resources.ApplyResources(this.bCopyCode, "bCopyCode");
            this.bCopyCode.Name = "bCopyCode";
            this.bCopyCode.UseVisualStyleBackColor = true;
            this.bCopyCode.Click += new System.EventHandler(this.bCopyCode_Click);
            // 
            // IRCRichTextBoxEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.InnerFont = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "IRCRichTextBoxEditor";
            this.SelectionBackColor = System.Drawing.Color.Snow;
            this.WorkMode = IRCWindow.IRCRichTextBox.Mode.Editor;
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button bCopyCode;

    }
}
