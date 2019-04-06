using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
using System.Diagnostics;
using IRCWindow.Properties;
using System.Threading;
using IRCProviders;
using IRCWindow.ViewModel;

namespace IRCWindow
{
    /// <summary>
    /// Диалог для установки компонентов
    /// </summary>
    public partial class InstallationDialog : IRCForm
    {
        private DataTable data = new DataTable();

        /// <summary>
        /// Папка для установки элементов
        /// </summary>
        public string InstallationFolder { get; set; }

        /// <summary>
        /// Сообщение об успешной установке
        /// </summary>
        public string SuccessMessage { get; set; }

        internal event Action Success;

        /// <summary>
        /// Необходима установка
        /// </summary>
        public event EventHandler<InstallEventArgs> Install;

        public InstallationDialog(XmlDocument doc)
        {
            InitializeComponent();

            data.Columns.Add("Title");
            data.Columns.Add("Description");
            data.Columns.Add("Author");
            data.Columns.Add("Date");
            data.Columns.Add("Url");
            data.Columns.Add("Size");
            data.Columns.Add("Source");
            data.Columns.Add("Guid");
            foreach (XmlNode parent in doc.ChildNodes)
            {
                if (parent is XmlElement)
                {
                    foreach (XmlNode node in parent.ChildNodes)
                    {
                        if (node.Name != "addon")
                            continue;

                        var title = ExtractValue(node, "title");
                        var description = ExtractValue(node, "description");
                        var author = ExtractValue(node, "author");
                        var date = ExtractValue(node, "date");
                        var url = ExtractValue(node, "url");
                        var size = ExtractValue(node, "size");
                        var source = ExtractValue(node, "source");
                        var guid = ExtractValue(node, "guid");

                        data.Rows.Add(title, description, author, date, url, size, source, guid);
                        this.listBox1.Items.Add(title);
                    }
                }
            }

            if (this.listBox1.Items.Count > 0)
            {
                this.listBox1.SelectedIndex = 0;
                listBox1_SelectedIndexChanged(this, EventArgs.Empty);
            }
            else
                this.bInstall.Enabled = false;
        }

        private string ExtractValue(XmlNode node, string name)
        {
            var childNode = node[name];
            return childNode == null ? string.Empty : childNode.InnerText;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = this.listBox1.SelectedIndex;
            this.lTitle.Text = data.Rows[i]["Title"].ToString();
            this.lDescription.Text = data.Rows[i]["Description"].ToString();
            this.lAuthor.Text = data.Rows[i]["Author"].ToString();
            this.lDate.Text = data.Rows[i]["Date"].ToString();
            this.lSource.Text = data.Rows[i]["Source"].ToString();
            this.lSize.Text = data.Rows[i]["Size"].ToString();
        }

        private void bInstall_Click(object sender, EventArgs e)
        {
            if (Install != null)
            {
                var eventArgs = new InstallEventArgs(data.Rows[this.listBox1.SelectedIndex]["Guid"].ToString());
                Install(this, eventArgs);
                if (!eventArgs.Install)
                    return;
            }

            var url = data.Rows[this.listBox1.SelectedIndex]["Url"].ToString();
            InstallFromUrl(url);
        }

        private void InstallFromUrl(string url)
        {
            var uri = new Uri(url);
            var fileName = Path.GetFileName(uri.AbsolutePath);
            var destination = Path.Combine(InstallationFolder, fileName);

            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.UserAgent, Program.UserAgentHeader);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            var progressDialog = new MyProgressDialog() { Text = Resources.FileDownloading };
            progressDialog.FormClosed += (sender2, e2) =>
            {
                if (webClient != null)
                {
                    webClient.CancelAsync();
                }
            };

            webClient.DownloadFileCompleted += (sender2, e2) => { if (progressDialog.Visible) progressDialog.Close(); };
            webClient.DownloadFileAsync(uri, destination, new object[] { destination, progressDialog });

            progressDialog.Show(this);
        }

        public void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var state = (object[])e.UserState;
            var destination = (string)state[0];

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, Application.ProductName);
                return;
            }

            if (e.Cancelled)
            {
                if (File.Exists(destination))
                    File.Delete(destination);
                //this.webClient = null;
                
                return;
            }

            Thread.Sleep(500);
            //this.webClient = null;

            if (ArchiveManager.ExtractArchive(destination))
            {
                MessageBox.Show(SuccessMessage);

                if (Success != null)
                    Success();
            }
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var state = (object[])e.UserState;
            var progressDialog = (MyProgressDialog)state[1];
            if (progressDialog.InvokeRequired)
            {
                var func = new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                this.BeginInvoke(func, sender, e);
            }
            else
                progressDialog.Value = e.ProgressPercentage;
        }

        private void AddonsDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
