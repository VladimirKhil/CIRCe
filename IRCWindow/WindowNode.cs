using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using System.Drawing;
using IRCWindow.Properties;

namespace IRCWindow
{
    /// <summary>
    /// Узел в дереве окон
    /// </summary>
    public class WindowNode: TreeNode, IFormNode, IDisposable
    {
        private IRCForm window = null;
        private bool ownImage = false;

        public bool Sticked { get; set; }

        /// <summary>
        /// Форма, к которой относится данный узел
        /// </summary>
        public IRCForm Window
        {
            get { return window; }
            set { window = value; SetHandlers(); }
        }

        /// <summary>
        /// Создание узла
        /// </summary>
        /// <param name="form"></param>
        /// <param name="text"></param>
        public WindowNode(IRCForm window, string text)
            : base(text)
        {
            this.window = window;
            SetHandlers();
        }

        public WindowNode(IRCForm window, string text, int image)
            : base(text, image, image)
        {
            this.window = window;
            SetHandlers();
        }
        
        private void SetHandlers()
        {
            if (this.window != null)
            {
                this.Text = this.window.Text;
                
                this.ContextMenuStrip = this.window.ContextMenuStrip != null ? this.window.ContextMenuStrip : new ContextMenuStrip();
                ToolStripMenuItem item = new ToolStripMenuItem(Resources.Close, null, delegate(object o, EventArgs e) { this.window.Close(); });
                item.ToolTipText = Resources.CloseToolTip;
                this.ContextMenuStrip.Items.Add(item);

                this.window.TextChanged += (sender, e) =>
                {
                    this.Text = this.window.Text;
                };
                this.window.Activated += (sender, e) =>
                {
                    if (this.TreeView != null && this.TreeView.SelectedNode != this && !MDIParent.BlockActivation)
                        this.TreeView.SelectedNode = this; 
                };
                this.window.Disposed += (sender, e) =>
                {
                    if (!this.Sticked)
                    {
                        try
                        {
                            Remove();
                            Dispose();
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        this.window = null;
                        if (this.IsSelected)
                            this.TreeView.SelectedNode = null;
                    }
                };
            }
        }

        #region IFormNode Members

        public System.Drawing.Image Image
        {
            get
            {
                return this.TreeView.ImageList.Images[this.ImageIndex];
            }
            set
            {
                this.TreeView.ImageList.Images.Add(value);
                this.ImageIndex = this.TreeView.ImageList.Images.Count - 1;
                this.SelectedImageIndex = this.ImageIndex;
                this.ownImage = true;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.ownImage)
            {
                this.TreeView.ImageList.Images.RemoveAt(this.ImageIndex);
            }
        }

        #endregion
    }
}
