namespace IRCWindow
{
    partial class SearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.bGoUp = new System.Windows.Forms.Button();
            this.cbCaseSensitive = new System.Windows.Forms.CheckBox();
            this.cbWholeWords = new System.Windows.Forms.CheckBox();
            this.bGoDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            this.tbSearch.AcceptsReturn = true;
            this.tbSearch.AccessibleDescription = null;
            this.tbSearch.AccessibleName = null;
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.BackgroundImage = null;
            this.tbSearch.Font = null;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
            // 
            // bGoUp
            // 
            this.bGoUp.AccessibleDescription = null;
            this.bGoUp.AccessibleName = null;
            resources.ApplyResources(this.bGoUp, "bGoUp");
            this.bGoUp.BackgroundImage = null;
            this.bGoUp.Font = null;
            this.bGoUp.Name = "bGoUp";
            this.bGoUp.UseVisualStyleBackColor = true;
            this.bGoUp.Click += new System.EventHandler(this.bGo_Click);
            // 
            // cbCaseSensitive
            // 
            this.cbCaseSensitive.AccessibleDescription = null;
            this.cbCaseSensitive.AccessibleName = null;
            resources.ApplyResources(this.cbCaseSensitive, "cbCaseSensitive");
            this.cbCaseSensitive.BackgroundImage = null;
            this.cbCaseSensitive.Font = null;
            this.cbCaseSensitive.Name = "cbCaseSensitive";
            this.cbCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // cbWholeWords
            // 
            this.cbWholeWords.AccessibleDescription = null;
            this.cbWholeWords.AccessibleName = null;
            resources.ApplyResources(this.cbWholeWords, "cbWholeWords");
            this.cbWholeWords.BackgroundImage = null;
            this.cbWholeWords.Font = null;
            this.cbWholeWords.Name = "cbWholeWords";
            this.cbWholeWords.UseVisualStyleBackColor = true;
            // 
            // bGoDown
            // 
            this.bGoDown.AccessibleDescription = null;
            this.bGoDown.AccessibleName = null;
            resources.ApplyResources(this.bGoDown, "bGoDown");
            this.bGoDown.BackgroundImage = null;
            this.bGoDown.Font = null;
            this.bGoDown.Name = "bGoDown";
            this.bGoDown.UseVisualStyleBackColor = true;
            this.bGoDown.Click += new System.EventHandler(this.bGoDown_Click);
            // 
            // SearchForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.bGoDown);
            this.Controls.Add(this.cbWholeWords);
            this.Controls.Add(this.cbCaseSensitive);
            this.Controls.Add(this.bGoUp);
            this.Controls.Add(this.tbSearch);
            this.Font = null;
            this.Icon = null;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchForm";
            this.ShowIcon = false;
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button bGoUp;
        private System.Windows.Forms.CheckBox cbCaseSensitive;
        private System.Windows.Forms.CheckBox cbWholeWords;
        private System.Windows.Forms.Button bGoDown;
    }
}