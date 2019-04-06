namespace SpecialTreeView
{
    partial class SpecailTreeView
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
            this.SuspendLayout();
            // 
            // SpecailTreeView
            // 
            this.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.LineColor = System.Drawing.Color.Black;
            this.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.SpecailTreeView_DrawNode);
            this.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.SpecailTreeView_NodeMouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpecailTreeView_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
