using System;
using System.Collections.Generic;
using System.Text;
using IRCProviders;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IRCWindow
{
    /// <summary>
    /// Дерево окон
    /// </summary>
    public class WindowTreeView: System.Windows.Forms.TreeView, IWindowTree
    {
        private Color nodeBackColorLeft = SystemColors.ControlLightLight;
        private Color nodeBackColorRight = SystemColors.ControlLightLight;
        private Color selectedNodeBackColorLeft = Color.DarkBlue;
        private Color selectedNodeBackColorRight = Color.DarkBlue;

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.NodeBackColorLeft = value;
                this.NodeBackColorRight = value;
                this.SelectedNodeColorRight = value;
            }
        }

        [Browsable(true)]
        public event TreeNodeMouseClickEventHandler ClickedByMouseLeftButton;

        /// <summary>
        /// Создание дерева
        /// </summary>
        public WindowTreeView()
            : base()
        {
            InitializeComponent();
        }

        void WindowTreeView_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        void WindowTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Bounds.Width == 0 || e.Bounds.Height == 0)
                return;
            Color backColorLeft = e.Node.IsSelected ? this.selectedNodeBackColorLeft : this.nodeBackColorLeft;
            Color backColorRight = e.Node.IsSelected ? this.selectedNodeBackColorRight : this.nodeBackColorRight;
            
            Rectangle bounds = GetNodeBounds(e.Node);
            using (SolidBrush backBrush = new SolidBrush(backColorLeft))
            {
                e.Graphics.FillRectangle(backBrush, bounds);
            }

            Font nodeFont = e.Node.NodeFont != null ? e.Node.NodeFont : this.Font;
            Color foreColor = e.Node.ForeColor == Color.Empty ? this.ForeColor : e.Node.ForeColor;
            bounds.Offset(5, 0);

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            TextRenderer.DrawText(e.Graphics, e.Node.Text, nodeFont, bounds, foreColor, backColorLeft, TextFormatFlags.Left);
        }

        private Rectangle GetNodeBounds(TreeNode node)
        {
            return new Rectangle(node.Bounds.X, node.Bounds.Y, this.Width - node.Bounds.X, node.Bounds.Height);
        }

        private TreeNode GetNodeAtPointExtended(Point p)
        {
            for (TreeNode node = this.TopNode; node != null && node.IsVisible; node = node.NextVisibleNode)
            {
                if (GetNodeBounds(node).Contains(p))
                    return node;
            }
            return null;
        }

        #region IWindowTree Members

        public Color NodeBackColorLeft
        {
            get
            {
                return nodeBackColorLeft;
            }
            set
            {
                nodeBackColorLeft = value;
            }
        }

        public Color NodeBackColorRight
        {
            get
            {
                return nodeBackColorRight;
            }
            set
            {
                nodeBackColorRight = value;
            }
        }

        public Color SelectedNodeBackColorLeft
        {
            get
            {
                return selectedNodeBackColorLeft;
            }
            set
            {
                selectedNodeBackColorLeft = value;
            }
        }

        public Color SelectedNodeColorRight
        {
            get
            {
                return selectedNodeBackColorRight;
            }
            set
            {
                selectedNodeBackColorRight = value;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WindowTreeView
            // 
            this.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LineColor = System.Drawing.Color.Black;
            this.SizeChanged += new System.EventHandler(this.WindowTreeView_SizeChanged);
            this.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.WindowTree_DrawNode);
            this.ResumeLayout(false);

        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (!this.DesignMode)
                    if (m.Msg == 0x201)
                    {
                        var coord = (int)m.LParam;
                        var y = coord >> 16;
                        var x = coord - (y << 16);
                        TreeNode clickedNode = this.GetNodeAt(x, y);
                        this.SelectedNode = clickedNode;

                        if (ClickedByMouseLeftButton != null)
                            ClickedByMouseLeftButton(this, new TreeNodeMouseClickEventArgs(clickedNode, MouseButtons.Left, 1, x, y));
                        return;
                    }

                base.WndProc(ref m);
            }
            catch
            {

            }
        }
    }
}
