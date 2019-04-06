using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCWindow.Properties;

namespace IRCWindow
{
    public partial class FlashingDialog : Form, INotifyPropertyChanged
    {
        private FlashParams flashParams = null;

        public FlashMode FlashingMode
        {
            get
            {
                return this.rbAlways.Checked ? FlashMode.Full :
                    this.rbRule.Checked ? FlashMode.Meduim : FlashMode.Weak;
            }
            set
            {
                switch (value)
                {
                    case FlashMode.Full:
                        this.rbAlways.Checked = true;
                        break;
                    case FlashMode.Meduim:
                        this.rbRule.Checked = true;
                        break;
                    case FlashMode.Weak:
                        this.rbNever.Checked = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public FlashingDialog(FlashParams flashParams)
        {
            InitializeComponent();

            this.flashParams = flashParams;

            this.gbAlways.DataBindings.Add("Enabled", this.rbAlways, "Checked");
            this.gbRule.DataBindings.Add("Enabled", this.rbRule, "Checked");
            this.DataBindings.Add("FlashingMode", flashParams, "FlashingMode", true, DataSourceUpdateMode.OnPropertyChanged);
            this.cbUseWhiteList.DataBindings.Add("Checked", flashParams, "UseWhiteList", true, DataSourceUpdateMode.OnPropertyChanged);
            this.cbUseBlackList.DataBindings.Add("Checked", flashParams, "UseBlackList", true, DataSourceUpdateMode.OnPropertyChanged);
            this.cbNick.DataBindings.Add("Checked", flashParams, "FlashOnNick", true, DataSourceUpdateMode.OnPropertyChanged);
            this.cbPrivate.DataBindings.Add("Checked", flashParams, "FlashOnPrivate", true, DataSourceUpdateMode.OnPropertyChanged);
            this.tbBlackList.DataBindings.Add("Text", flashParams, "BlackList", true, DataSourceUpdateMode.OnPropertyChanged);
            this.tbWhiteList.DataBindings.Add("Text", flashParams, "WhiteList", true, DataSourceUpdateMode.OnPropertyChanged);
            this.tbWhiteList.DataBindings.Add("Enabled", this.cbUseWhiteList, "Checked");
            this.tbBlackList.DataBindings.Add("Enabled", this.cbUseBlackList, "Checked");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void rbAlways_CheckedChanged(object sender, EventArgs e)
        {
            InformChange();
        }

        private void rbRule_CheckedChanged(object sender, EventArgs e)
        {
            InformChange();
        }

        private void InformChange()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("FlashingMode"));
        }
    }
}
