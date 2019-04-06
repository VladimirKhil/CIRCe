using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIPackages;

namespace SIIRC
{
    public partial class PackageTreeViewDialog : Form
    {
        int round = -1, theme = -1, question = -1;

        internal int Round { get { return round; } }
        internal int Theme { get { return theme; } }
        internal int Question { get { return question; } }

        public PackageTreeViewDialog(SIDocument doc, GameConfiguration.GameTypes gameType)
        {
            InitializeComponent();

            treeView1.Nodes.Add(doc.Package.Name);

            foreach (Round round in doc.Package.Rounds)
            {
                TreeNode roundNode = new TreeNode(round.Name);
                treeView1.Nodes[0].Nodes.Add(roundNode);

                if (gameType != GameConfiguration.GameTypes.TeleSI)
                {
                    foreach (Theme theme in round.Themes)
                    {
                        TreeNode themeNode = new TreeNode(theme.Name);
                        roundNode.Nodes.Add(themeNode);

                        foreach (Question quest in theme.Questions)
                        {
                            themeNode.Nodes.Add(quest.ToString());
                        }
                    }
                }
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (treeView1.SelectedNode.Level)
            {
                case 0:
                    round = theme = question = 0;
                    break;

                case 1:
                    round = treeView1.SelectedNode.Index;
                    theme = question = 0;
                    break;

                case 2:
                    round = treeView1.SelectedNode.Parent.Index;
                    theme = treeView1.SelectedNode.Index;
                    question = 0;
                    break;

                case 3:
                    round = treeView1.SelectedNode.Parent.Parent.Index;
                    theme = treeView1.SelectedNode.Parent.Index;
                    question = treeView1.SelectedNode.Index;
                    break;

                default:
                    break;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
