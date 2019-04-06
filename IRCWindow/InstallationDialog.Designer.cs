namespace IRCWindow
{
    partial class InstallationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallationDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lTitle = new System.Windows.Forms.Label();
            this.lDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lAuthor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lSource = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lDate = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lSize = new System.Windows.Forms.Label();
            this.bInstall = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lTitle);
            this.flowLayoutPanel1.Controls.Add(this.lDescription);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.lAuthor);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.lSource);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.lDate);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.lSize);
            this.flowLayoutPanel1.Controls.Add(this.bInstall);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lTitle
            // 
            resources.ApplyResources(this.lTitle, "lTitle");
            this.flowLayoutPanel1.SetFlowBreak(this.lTitle, true);
            this.lTitle.Name = "lTitle";
            // 
            // lDescription
            // 
            resources.ApplyResources(this.lDescription, "lDescription");
            this.flowLayoutPanel1.SetFlowBreak(this.lDescription, true);
            this.lDescription.Name = "lDescription";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lAuthor
            // 
            resources.ApplyResources(this.lAuthor, "lAuthor");
            this.flowLayoutPanel1.SetFlowBreak(this.lAuthor, true);
            this.lAuthor.Name = "lAuthor";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lSource
            // 
            resources.ApplyResources(this.lSource, "lSource");
            this.flowLayoutPanel1.SetFlowBreak(this.lSource, true);
            this.lSource.Name = "lSource";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lDate
            // 
            resources.ApplyResources(this.lDate, "lDate");
            this.flowLayoutPanel1.SetFlowBreak(this.lDate, true);
            this.lDate.Name = "lDate";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lSize
            // 
            resources.ApplyResources(this.lSize, "lSize");
            this.flowLayoutPanel1.SetFlowBreak(this.lSize, true);
            this.lSize.Name = "lSize";
            // 
            // bInstall
            // 
            resources.ApplyResources(this.bInstall, "bInstall");
            this.bInstall.Name = "bInstall";
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // InstallationDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallationDialog";
            this.ShowIcon = false;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddonsDialog_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Label lDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lAuthor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lSize;
    }
}