namespace IRCWindow
{
    partial class MultimediaForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultimediaForm));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.bTest = new System.Windows.Forms.Button();
            this.bStop = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLoop = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPos = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudPos)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.HideSelection = false;
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.Name = "treeView1";
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.toolTip1.SetToolTip(this.listView1, resources.GetString("listView1.ToolTip"));
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // bTest
            // 
            resources.ApplyResources(this.bTest, "bTest");
            this.bTest.Name = "bTest";
            this.bTest.UseVisualStyleBackColor = true;
            this.bTest.Click += new System.EventHandler(this.bTest_Click);
            // 
            // bStop
            // 
            resources.ApplyResources(this.bStop, "bStop");
            this.bStop.Name = "bStop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "FullChannelName";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.ValueMember = "FullChannelName";
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbLoop
            // 
            resources.ApplyResources(this.tbLoop, "tbLoop");
            this.tbLoop.Name = "tbLoop";
            this.tbLoop.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // nudPos
            // 
            resources.ApplyResources(this.nudPos, "nudPos");
            this.nudPos.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudPos.Name = "nudPos";
            // 
            // MultimediaForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudPos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLoop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bStop);
            this.Controls.Add(this.bTest);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultimediaForm";
            this.ShowIcon = false;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MultimediaForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudPos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button bTest;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox tbLoop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPos;
    }
}