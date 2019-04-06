using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCConnection;
using IRCWindow.Properties;
using CIRCe.Base;
using IRCWindow.ViewModel;

namespace IRCWindow
{
    /// <summary>
    /// Окно привата
    /// </summary>
    internal partial class MDIChildPrivate : MDIChildCommunication
    {
        public const string ReadSplitterString = @"38****************************"; // Первый символ невидим

        public MDIChildPrivate(MDIParent main, MDIChildServer mainWindow, string name, Dictionary<string, UserInfo> whois)
            : base(main, mainWindow, name, whois)
        {
            InitializeComponent();

            this.Size = UISettings.Default.IRCWindowSize;
            this.Text = name;
            AddPerson(name, false);
            mainAreaSplitContainer.Panel2.Visible = false;
            mainAreaSplitContainer.SplitterDistance = mainAreaSplitContainer.Width;
            topicPanel.Visible = false;
            this.topPanel.Visible = false;
        }

        public override void PutMessage(string msgText, Color defColor, bool putTime, MessageType messageType)
        {
            if (Form.ActiveForm == null && messageType == MessageType.Replic) // Приложение неактивно
            {
                if (reading)
                {
                    reading = false;
                    if (this.chatRTB.TextLength > 0 && UISettings.Default.ShowReadSplitters)
                    {
                        LogMode oldMode = this.logMode;
                        this.logMode = LogMode.None;
                        Echo(ReadSplitterString);
                        this.logMode = oldMode;
                    }
                }
                if (UISettings.Default.FlashingParams.FlashingMode == FlashMode.Full ||
                    UISettings.Default.FlashingParams.FlashingMode == FlashMode.Meduim && UISettings.Default.FlashingParams.FlashOnPrivate)
                    Main.Flash();
            }
            base.PutMessage(msgText, defColor, putTime, messageType);
        }

        public override void RenamePerson(string oldName, string newName)
        {
            base.RenamePerson(oldName, newName);
            if (this.WindowName == oldName)
                this.Text = newName;
        }
    }
}
