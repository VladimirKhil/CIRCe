using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CIRCeAddonTemplate
{
    /// <summary>
    /// Форма для подключения к серверу и захода на нужный канал
    /// </summary>
    public sealed partial class ConnectionForm : Form
    {
        public Data Data { get; set; }

        /// <summary>
        /// Создание формы
        /// </summary>
        /// <param name="data">Данные для подключения</param>
        public ConnectionForm(Data data)
        {
            InitializeComponent();

            this.Data = data;

            this.tbServer.DataBindings.Add("Text", data.Info.Server, "Name", true);
            this.nudPort.DataBindings.Add("Value", data.Info.Server, "Port", true);
            this.tbNick.DataBindings.Add("Text", data.Info.Nick, "Nick", true);
            this.tbChannel.DataBindings.Add("Text", data, "Channel", true);
        }
    }
}
