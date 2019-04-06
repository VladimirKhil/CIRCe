namespace SIIRC
{
    partial class StartUpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartUpForm));
            this.bPackagePathConfigure = new System.Windows.Forms.Button();
            this.lChannel = new System.Windows.Forms.Label();
            this.tBServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cBGameTypes = new System.Windows.Forms.ComboBox();
            this.bStart = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.openFilePackageDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tBChannelName = new System.Windows.Forms.TextBox();
            this.tBNick = new System.Windows.Forms.TextBox();
            this.bPackageStore = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lNick = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nUDPort = new System.Windows.Forms.NumericUpDown();
            this.cbPackagePath = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nUDPort)).BeginInit();
            this.SuspendLayout();
            // 
            // bPackagePathConfigure
            // 
            resources.ApplyResources(this.bPackagePathConfigure, "bPackagePathConfigure");
            this.bPackagePathConfigure.Name = "bPackagePathConfigure";
            this.toolTip1.SetToolTip(this.bPackagePathConfigure, resources.GetString("bPackagePathConfigure.ToolTip"));
            this.bPackagePathConfigure.UseVisualStyleBackColor = true;
            this.bPackagePathConfigure.Click += new System.EventHandler(this.bPackagePathConfigure_Click);
            // 
            // lChannel
            // 
            resources.ApplyResources(this.lChannel, "lChannel");
            this.lChannel.Name = "lChannel";
            // 
            // tBServerName
            // 
            resources.ApplyResources(this.tBServerName, "tBServerName");
            this.tBServerName.Name = "tBServerName";
            this.toolTip1.SetToolTip(this.tBServerName, resources.GetString("tBServerName.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cBGameTypes
            // 
            this.cBGameTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBGameTypes.FormattingEnabled = true;
            this.cBGameTypes.Items.AddRange(new object[] {
            resources.GetString("cBGameTypes.Items"),
            resources.GetString("cBGameTypes.Items1"),
            resources.GetString("cBGameTypes.Items2")});
            resources.ApplyResources(this.cBGameTypes, "cBGameTypes");
            this.cBGameTypes.Name = "cBGameTypes";
            this.toolTip1.SetToolTip(this.cBGameTypes, resources.GetString("cBGameTypes.ToolTip"));
            // 
            // bStart
            // 
            this.bStart.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.bStart, "bStart");
            this.bStart.Name = "bStart";
            this.bStart.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // openFilePackageDialog
            // 
            resources.ApplyResources(this.openFilePackageDialog, "openFilePackageDialog");
            // 
            // tBChannelName
            // 
            resources.ApplyResources(this.tBChannelName, "tBChannelName");
            this.tBChannelName.Name = "tBChannelName";
            this.toolTip1.SetToolTip(this.tBChannelName, resources.GetString("tBChannelName.ToolTip"));
            // 
            // tBNick
            // 
            resources.ApplyResources(this.tBNick, "tBNick");
            this.tBNick.Name = "tBNick";
            this.toolTip1.SetToolTip(this.tBNick, resources.GetString("tBNick.ToolTip"));
            // 
            // bPackageStore
            // 
            resources.ApplyResources(this.bPackageStore, "bPackageStore");
            this.bPackageStore.Name = "bPackageStore";
            this.toolTip1.SetToolTip(this.bPackageStore, resources.GetString("bPackageStore.ToolTip"));
            this.bPackageStore.UseVisualStyleBackColor = true;
            this.bPackageStore.Click += new System.EventHandler(this.bPackageStore_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lNick
            // 
            resources.ApplyResources(this.lNick, "lNick");
            this.lNick.Name = "lNick";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // nUDPort
            // 
            resources.ApplyResources(this.nUDPort, "nUDPort");
            this.nUDPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nUDPort.Name = "nUDPort";
            // 
            // cbPackagePath
            // 
            this.cbPackagePath.FormattingEnabled = true;
            resources.ApplyResources(this.cbPackagePath, "cbPackagePath");
            this.cbPackagePath.Name = "cbPackagePath";
            // 
            // StartUpForm
            // 
            this.AcceptButton = this.bStart;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bPackageStore);
            this.Controls.Add(this.cbPackagePath);
            this.Controls.Add(this.tBNick);
            this.Controls.Add(this.lNick);
            this.Controls.Add(this.nUDPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tBChannelName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.cBGameTypes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBServerName);
            this.Controls.Add(this.lChannel);
            this.Controls.Add(this.bPackagePathConfigure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartUpForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartUpForm_FormClosing);
            this.Load += new System.EventHandler(this.StartUpForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nUDPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bPackagePathConfigure;
        private System.Windows.Forms.Label lChannel;
        private System.Windows.Forms.TextBox tBServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBGameTypes;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.OpenFileDialog openFilePackageDialog;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox tBChannelName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nUDPort;
        private System.Windows.Forms.Label lNick;
        private System.Windows.Forms.TextBox tBNick;
        private System.Windows.Forms.ComboBox cbPackagePath;
        private System.Windows.Forms.Button bPackageStore;
    }
}