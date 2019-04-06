using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SIPackages;
using System.Reflection;
using System.ServiceModel;

namespace SIIRC
{
    public partial class StartUpForm : Form
    {
        SIDocument doc = null;

        /// <summary>
        /// Игровой документ пакета
        /// </summary>
        internal SIDocument PackageDoc
        {
            get { return doc; }
        }

        internal int Port
        {
            get { return (int)this.nUDPort.Value; }
        }

        internal SIIRC.GameConfiguration.GameTypes GameType
        {
            get { return (SIIRC.GameConfiguration.GameTypes)Enum.ToObject(typeof(SIIRC.GameConfiguration.GameTypes), cBGameTypes.SelectedIndex); }
        }

        public StartUpForm(GameConfiguration gameConfig)
        {
            InitializeComponent();

            this.cBGameTypes.SelectedIndex = (int)gameConfig.GameType;

            Binding binding = new Binding("Text", gameConfig, "DefPackagePath");
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

#if DEBUG
            //gameConfig.DefPackagePath = @"C:\Users\Владимир\Documents\Data\stud2004.siq";
#endif

            this.cbPackagePath.DataBindings.Add(binding);
            this.tBChannelName.DataBindings.Add("Text", gameConfig, "DefChannelName", true);
            this.tBServerName.DataBindings.Add("Text", gameConfig, "DefServerName", true);
            this.tBNick.DataBindings.Add("Text", gameConfig, "DefUserName", true);

            this.nUDPort.Value = gameConfig.DefServerPort;
        }

        private void StartUpForm_Load(object sender, EventArgs e)
        {
            if (cbPackagePath.Text.Length > 0)
            {
                CheckPackage();
            }
        }

        private void CheckPackage()
        {
            var filePath = cbPackagePath.Text;
            if (!File.Exists(filePath))
            {
                doc = null;
                cbPackagePath.Text = "";
                return;
            }

            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    doc = SIDocument.Load(stream);
                }
            }
            catch (Exception e)
            {
                cbPackagePath.Text = "";
                MessageBox.Show(this, e.Message);
            }
        }

        private void bPackagePathConfigure_Click(object sender, EventArgs e)
        {
            if (openFilePackageDialog.ShowDialog() == DialogResult.OK)
            {
                this.cbPackagePath.Text = openFilePackageDialog.FileName;
                CheckPackage();
            }
        }

        private void bPackageStore_Click(object sender, EventArgs e)
        {
            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress("http://vladimirkhil.com/services/si");

                var siServiceClient = new SIService.SIServiceClient(binding, endpoint);
                var result = siServiceClient.GetCategories();

                var data = new Dictionary<SIService.PackageCategory, SIService.Package[]>();
                foreach (var item in result)
                {
                    data[item] = siServiceClient.GetPackagesByCategory(item.ID);
                }

                var packageStoreWindow = new PackageStoreForm(data) { Owner = this };
                if (packageStoreWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {                    
                    var package = siServiceClient.GetPackageByID(packageStoreWindow.SelectedValue.ID);

                    if (this.doc != null)
                        this.doc.Dispose();

                    SIAddon.ClearTempFile();
                    SIAddon.TempFile = Path.GetTempFileName();

                    var webClient = new System.Net.WebClient();
                    webClient.DownloadFile(package, SIAddon.TempFile);

                    this.cbPackagePath.Text = SIAddon.TempFile;
                    CheckPackage();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format("Ошибка работы с хранилищем пакетов: {0}", exc.Message), "Дополнение СИ", MessageBoxButtons.OK);
            }
        }

        private void StartUpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK && string.IsNullOrWhiteSpace(this.cbPackagePath.Text))
            {
                e.Cancel = true;
                MessageBox.Show(this, "Вы должны выбрать пакет для игры!");
            }
        }
    }
}
