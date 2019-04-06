using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SpecialTreeView
{
    public partial class SpecailTreeView : TreeView
    {
        TreeNode active = null;
        
        public SpecailTreeView()
        {
            InitializeComponent();
        }

        private void SpecailTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Bounds.Width <= 0 || e.Bounds.Height <= 0)
                return;
            var b = new Bitmap(e.Bounds.Width - 1, e.Bounds.Height - 1);
            var memG = Graphics.FromImage(b);
            if (GetNodeBounds(e.Node).Contains(this.PointToClient(MousePosition)))
            {
                memG.DrawPolygon(Pens.Gray, new Point[]{
                    new Point(0,2),
                    new Point(2,0),
                    new Point(b.Width - 3,0),
                    new Point(b.Width - 1,2),
                    new Point(b.Width - 1,b.Height - 3),
                    new Point(b.Width - 3,b.Height - 1),
                    new Point(2,b.Height - 1),
                    new Point(0,b.Height - 3),
                });
                    // .DrawRectangle(Pens.Gray, 0, 0, b.Width - 1, b.Height - 1);                
            }
            //var size = TextRenderer.MeasureText(memG, e.Node.Text, this.Font);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            TextRenderer.DrawText(e.Graphics, e.Node.Text, this.Font, e.Node.Bounds, this.ForeColor);
            e.Graphics.DrawImage(b, e.Bounds.X + 1, e.Bounds.Y, b.Width, b.Height);
            //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //TextRenderer.DrawText(e.Graphics, e.Node.Text, this.Font, e.Node.Bounds, this.ForeColor);
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

        private void SpecailTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            var item = this.GetNodeAtPointExtended(e.Location);
            if (item != null && item != active)
            {
                if (active != null)
                    Invalidate(active.Bounds);
                active = item;
                Invalidate(active.Bounds);
            }
        }

        private void SpecailTreeView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            /*var item = e.Node;
            if (item != null && item != active)
            {
                if (active != null)
                    Invalidate(active.Bounds);
                active = item;
                Invalidate(active.Bounds);
            }*/
        }
    }
}
