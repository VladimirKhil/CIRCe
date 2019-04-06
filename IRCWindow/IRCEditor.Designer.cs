namespace IRCWindow
{
    partial class IRCEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRCEditor));
            this.ircRichTextBoxEditor1 = new IRCWindow.IRCRichTextBoxEditor();
            this.SuspendLayout();
            // 
            // ircRichTextBoxEditor1
            // 
            this.ircRichTextBoxEditor1.DefaultColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.ircRichTextBoxEditor1, "ircRichTextBoxEditor1");
            this.ircRichTextBoxEditor1.EditOnDoubleClick = false;
            this.ircRichTextBoxEditor1.InnerFont = new System.Drawing.Font("Courier New", 11.25F);
            this.ircRichTextBoxEditor1.Name = "ircRichTextBoxEditor1";
            this.ircRichTextBoxEditor1.ReadOnly = false;
            this.ircRichTextBoxEditor1.SelectionBackColor = System.Drawing.Color.White;
            this.ircRichTextBoxEditor1.SelectionColor = System.Drawing.Color.Black;
            this.ircRichTextBoxEditor1.SelectionLength = 0;
            this.ircRichTextBoxEditor1.SelectionStart = 0;
            this.ircRichTextBoxEditor1.ToolTip = "";
            this.ircRichTextBoxEditor1.WorkMode = IRCWindow.IRCRichTextBox.Mode.Editor;
            // 
            // IRCEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ircRichTextBoxEditor1);
            this.Name = "IRCEditor";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        private IRCRichTextBoxEditor ircRichTextBoxEditor1;

    }
}