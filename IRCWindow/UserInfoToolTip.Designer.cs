namespace IRCWindow
{
    partial class UserInfoToolTip
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
            // 
            // UserInfoToolTip
            // 
            this.AutoPopDelay = 4000;
            this.InitialDelay = 200;
            this.OwnerDraw = true;
            this.ReshowDelay = 1000;
            this.ShowAlways = true;
            this.UseAnimation = false;
            this.Popup += new System.Windows.Forms.PopupEventHandler(this.UserInfoToolTip_Popup);
            this.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.UserInfoToolTip_Draw);

        }

        #endregion

    }
}
