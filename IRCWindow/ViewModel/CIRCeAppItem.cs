using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;

namespace IRCWindow.ViewModel
{
    internal abstract class CIRCeAppItem: CIRCeItem, ICIRCeAppItem
    {
        private MDIChild childWindow = null;
        private CIRCePanel panel = null;

        public CIRCeAppItem(MDIChild childWindow)
            : base(childWindow)
        {
            this.childWindow = childWindow;
            
            this.childWindow.InputKeyDown += communicationWindow_InputKeyDown;
            this.childWindow.InputKeyPress += communicationWindow_InputKeyPress;
        }

        void communicationWindow_InputKeyPress(object sender, IRCProviders.SerializableKeyPressedEventArgs e)
        {
            if (InputKeyPress != null)
                InputKeyPress(this, e.ToNew());
        }

        void communicationWindow_InputKeyDown(object sender, IRCProviders.SerializableKeyEventArgs e)
        {
            if (InputKeyDown != null)
                InputKeyDown(this, e.ToNew());
        }

        public event EventHandler<SerializableKeyEventArgs> InputKeyDown;

        public event EventHandler<SerializableKeyPressedEventArgs> InputKeyPress;

        public IPanel ChatPanel
        {
            get
            {
                if (this.panel == null)
                {
                    this.panel = new CIRCePanel { Panel = this.childWindow.ChatPanel as IRCPanel };
                }
                return this.panel;
            }
        }

        public void Echo(string text)
        {
            this.childWindow.Echo(text);
        }

        public event EventHandler<ChatClickEventArgs> ChatClicked;

        public event Action<string> ChatSelected;

        protected internal void OnChatClicked(string line, int index)
        {
            if (ChatClicked != null)
                ChatClicked(this, new ChatClickEventArgs { Line = line, Position = index });
        }

        protected internal bool HasChatListeners()
        {
            return ChatClicked != null;
        }

        protected internal void OnChatSelected(string text)
        {
            if (ChatSelected != null)
                ChatSelected(text);
        }
    }
}
