namespace IRCWindow
{
    partial class IRCRichTextBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IRCRichTextBox));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.foreColorButton = new System.Windows.Forms.ToolStripButton();
            this.backColorButton = new System.Windows.Forms.ToolStripButton();
            this.boldButton = new System.Windows.Forms.ToolStripButton();
            this.underlineButton = new System.Windows.Forms.ToolStripButton();
            this.reverseButton = new System.Windows.Forms.ToolStripButton();
            this.plainButton = new System.Windows.Forms.ToolStripButton();
            this.hideButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox1.DetectUrls = false;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Text = global::IRCWindow.Properties.Resources.TooFastTargetChangeMessage;
            this.richTextBox1.ReadOnlyChanged += new System.EventHandler(this.richTextBox1_ReadOnlyChanged);
            this.richTextBox1.RightToLeftChanged += new System.EventHandler(this.richTextBox1_RightToLeftChanged);
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            this.richTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            this.richTextBox1.Leave += new System.EventHandler(this.richTextBox1_Leave);
            this.richTextBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCut,
            this.tsmiCopy,
            this.tsmiPaste,
            this.tsmiDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // tsmiCut
            // 
            this.tsmiCut.Name = "tsmiCut";
            resources.ApplyResources(this.tsmiCut, "tsmiCut");
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Name = "tsmiCopy";
            resources.ApplyResources(this.tsmiCopy, "tsmiCopy");
            // 
            // tsmiPaste
            // 
            this.tsmiPaste.Name = "tsmiPaste";
            resources.ApplyResources(this.tsmiPaste, "tsmiPaste");
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            resources.ApplyResources(this.tsmiDelete, "tsmiDelete");
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.foreColorButton,
            this.backColorButton,
            this.boldButton,
            this.underlineButton,
            this.reverseButton,
            this.plainButton,
            this.hideButton});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // foreColorButton
            // 
            this.foreColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.foreColorButton.ForeColor = System.Drawing.Color.DarkBlue;
            resources.ApplyResources(this.foreColorButton, "foreColorButton");
            this.foreColorButton.Name = "foreColorButton";
            this.foreColorButton.Click += new System.EventHandler(this.foreColorButton_Click);
            // 
            // backColorButton
            // 
            this.backColorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.backColorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.backColorButton.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.backColorButton, "backColorButton");
            this.backColorButton.Name = "backColorButton";
            this.backColorButton.Click += new System.EventHandler(this.foreBackColorButton_Click);
            // 
            // boldButton
            // 
            this.boldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.boldButton, "boldButton");
            this.boldButton.Name = "boldButton";
            this.boldButton.Click += new System.EventHandler(this.boldButton_Click);
            // 
            // underlineButton
            // 
            this.underlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.underlineButton, "underlineButton");
            this.underlineButton.Name = "underlineButton";
            this.underlineButton.Click += new System.EventHandler(this.underlineButton_Click);
            // 
            // reverseButton
            // 
            this.reverseButton.BackColor = System.Drawing.Color.Black;
            this.reverseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.reverseButton.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.reverseButton, "reverseButton");
            this.reverseButton.Name = "reverseButton";
            this.reverseButton.Click += new System.EventHandler(this.reverseButton_Click);
            // 
            // plainButton
            // 
            this.plainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.plainButton, "plainButton");
            this.plainButton.Name = "plainButton";
            this.plainButton.Click += new System.EventHandler(this.plainButton_Click);
            // 
            // hideButton
            // 
            resources.ApplyResources(this.hideButton, "hideButton");
            this.hideButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.hideButton.Image = global::IRCWindow.Properties.Resources.arrow_right;
            this.hideButton.Name = "hideButton";
            this.hideButton.Click += new System.EventHandler(this.hideButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBox1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // IRCRichTextBox
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            resources.ApplyResources(this, "$this");
            this.Name = "IRCRichTextBox";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCut;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        protected System.Windows.Forms.RichTextBox richTextBox1;
        protected System.Windows.Forms.ToolStrip toolStrip1;
        protected System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.ToolStripButton foreColorButton;
        protected System.Windows.Forms.ToolStripButton backColorButton;
        protected System.Windows.Forms.ToolStripButton boldButton;
        protected System.Windows.Forms.ToolStripButton underlineButton;
        protected System.Windows.Forms.ToolStripButton reverseButton;
        protected System.Windows.Forms.ToolStripButton plainButton;
        protected System.Windows.Forms.ToolStripButton hideButton;
        protected System.Windows.Forms.ContextMenuStrip contextMenuStrip1;


    }
}
