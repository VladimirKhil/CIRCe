namespace IRCWindow
{
    partial class FlashingDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlashingDialog));
            this.rbAlways = new System.Windows.Forms.RadioButton();
            this.cbUseBlackList = new System.Windows.Forms.CheckBox();
            this.tbBlackList = new System.Windows.Forms.TextBox();
            this.gbAlways = new System.Windows.Forms.GroupBox();
            this.rbRule = new System.Windows.Forms.RadioButton();
            this.gbRule = new System.Windows.Forms.GroupBox();
            this.cbUseWhiteList = new System.Windows.Forms.CheckBox();
            this.cbPrivate = new System.Windows.Forms.CheckBox();
            this.cbNick = new System.Windows.Forms.CheckBox();
            this.tbWhiteList = new System.Windows.Forms.TextBox();
            this.rbNever = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.gbAlways.SuspendLayout();
            this.gbRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbAlways
            // 
            this.rbAlways.AccessibleDescription = null;
            this.rbAlways.AccessibleName = null;
            resources.ApplyResources(this.rbAlways, "rbAlways");
            this.rbAlways.BackgroundImage = null;
            this.rbAlways.Font = null;
            this.rbAlways.Name = "rbAlways";
            this.rbAlways.TabStop = true;
            this.rbAlways.UseVisualStyleBackColor = true;
            this.rbAlways.CheckedChanged += new System.EventHandler(this.rbAlways_CheckedChanged);
            // 
            // cbUseBlackList
            // 
            this.cbUseBlackList.AccessibleDescription = null;
            this.cbUseBlackList.AccessibleName = null;
            resources.ApplyResources(this.cbUseBlackList, "cbUseBlackList");
            this.cbUseBlackList.BackgroundImage = null;
            this.cbUseBlackList.Font = null;
            this.cbUseBlackList.Name = "cbUseBlackList";
            this.cbUseBlackList.UseVisualStyleBackColor = true;
            // 
            // tbBlackList
            // 
            this.tbBlackList.AccessibleDescription = null;
            this.tbBlackList.AccessibleName = null;
            resources.ApplyResources(this.tbBlackList, "tbBlackList");
            this.tbBlackList.BackgroundImage = null;
            this.tbBlackList.Font = null;
            this.tbBlackList.Name = "tbBlackList";
            // 
            // gbAlways
            // 
            this.gbAlways.AccessibleDescription = null;
            this.gbAlways.AccessibleName = null;
            resources.ApplyResources(this.gbAlways, "gbAlways");
            this.gbAlways.BackgroundImage = null;
            this.gbAlways.Controls.Add(this.cbUseBlackList);
            this.gbAlways.Controls.Add(this.tbBlackList);
            this.gbAlways.Font = null;
            this.gbAlways.Name = "gbAlways";
            this.gbAlways.TabStop = false;
            // 
            // rbRule
            // 
            this.rbRule.AccessibleDescription = null;
            this.rbRule.AccessibleName = null;
            resources.ApplyResources(this.rbRule, "rbRule");
            this.rbRule.BackgroundImage = null;
            this.rbRule.Font = null;
            this.rbRule.Name = "rbRule";
            this.rbRule.TabStop = true;
            this.rbRule.UseVisualStyleBackColor = true;
            this.rbRule.CheckedChanged += new System.EventHandler(this.rbRule_CheckedChanged);
            // 
            // gbRule
            // 
            this.gbRule.AccessibleDescription = null;
            this.gbRule.AccessibleName = null;
            resources.ApplyResources(this.gbRule, "gbRule");
            this.gbRule.BackgroundImage = null;
            this.gbRule.Controls.Add(this.cbUseWhiteList);
            this.gbRule.Controls.Add(this.cbPrivate);
            this.gbRule.Controls.Add(this.cbNick);
            this.gbRule.Controls.Add(this.tbWhiteList);
            this.gbRule.Font = null;
            this.gbRule.Name = "gbRule";
            this.gbRule.TabStop = false;
            // 
            // cbUseWhiteList
            // 
            this.cbUseWhiteList.AccessibleDescription = null;
            this.cbUseWhiteList.AccessibleName = null;
            resources.ApplyResources(this.cbUseWhiteList, "cbUseWhiteList");
            this.cbUseWhiteList.BackgroundImage = null;
            this.cbUseWhiteList.Font = null;
            this.cbUseWhiteList.Name = "cbUseWhiteList";
            this.cbUseWhiteList.UseVisualStyleBackColor = true;
            // 
            // cbPrivate
            // 
            this.cbPrivate.AccessibleDescription = null;
            this.cbPrivate.AccessibleName = null;
            resources.ApplyResources(this.cbPrivate, "cbPrivate");
            this.cbPrivate.BackgroundImage = null;
            this.cbPrivate.Checked = true;
            this.cbPrivate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPrivate.Font = null;
            this.cbPrivate.Name = "cbPrivate";
            this.cbPrivate.UseVisualStyleBackColor = true;
            // 
            // cbNick
            // 
            this.cbNick.AccessibleDescription = null;
            this.cbNick.AccessibleName = null;
            resources.ApplyResources(this.cbNick, "cbNick");
            this.cbNick.BackgroundImage = null;
            this.cbNick.Checked = true;
            this.cbNick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNick.Font = null;
            this.cbNick.Name = "cbNick";
            this.cbNick.UseVisualStyleBackColor = true;
            // 
            // tbWhiteList
            // 
            this.tbWhiteList.AccessibleDescription = null;
            this.tbWhiteList.AccessibleName = null;
            resources.ApplyResources(this.tbWhiteList, "tbWhiteList");
            this.tbWhiteList.BackgroundImage = null;
            this.tbWhiteList.Font = null;
            this.tbWhiteList.Name = "tbWhiteList";
            // 
            // rbNever
            // 
            this.rbNever.AccessibleDescription = null;
            this.rbNever.AccessibleName = null;
            resources.ApplyResources(this.rbNever, "rbNever");
            this.rbNever.BackgroundImage = null;
            this.rbNever.Font = null;
            this.rbNever.Name = "rbNever";
            this.rbNever.TabStop = true;
            this.rbNever.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.AccessibleDescription = null;
            this.button1.AccessibleName = null;
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackgroundImage = null;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Font = null;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.AccessibleDescription = null;
            this.button2.AccessibleName = null;
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackgroundImage = null;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = null;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // FlashingDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.button2;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rbNever);
            this.Controls.Add(this.gbRule);
            this.Controls.Add(this.rbRule);
            this.Controls.Add(this.rbAlways);
            this.Controls.Add(this.gbAlways);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlashingDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.gbAlways.ResumeLayout(false);
            this.gbAlways.PerformLayout();
            this.gbRule.ResumeLayout(false);
            this.gbRule.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbAlways;
        private System.Windows.Forms.CheckBox cbUseBlackList;
        private System.Windows.Forms.TextBox tbBlackList;
        private System.Windows.Forms.GroupBox gbAlways;
        private System.Windows.Forms.RadioButton rbRule;
        private System.Windows.Forms.GroupBox gbRule;
        private System.Windows.Forms.CheckBox cbPrivate;
        private System.Windows.Forms.CheckBox cbNick;
        private System.Windows.Forms.TextBox tbWhiteList;
        private System.Windows.Forms.CheckBox cbUseWhiteList;
        private System.Windows.Forms.RadioButton rbNever;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}