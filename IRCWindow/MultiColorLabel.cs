using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCWindow.Properties;
using IRC.Client.Base;

namespace IRCWindow
{
    public partial class MultiColorLabel : UserControl
    {
        FormattedText text = null;
        ContentAlignment textAlign = ContentAlignment.MiddleLeft;

        [Browsable(true)]
        public new string Text
        {
            get { return this.text == null ? string.Empty : this.text.Original; }
            set { this.text = new FormattedText(value, 1, 99); }
        }

        public FormattedText FormattedText
        {
            set
            { 
                this.text = value;
                Invalidate();
            }
        }

        public ContentAlignment TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        public MultiColorLabel()
        {
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (SolidBrush backBrush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
            }            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.text == null)
                return;
            int p = 0;
            float x = 0;
            float y = 0;

            if (textAlign == ContentAlignment.MiddleCenter || textAlign == ContentAlignment.BottomCenter || textAlign == ContentAlignment.TopCenter)
            {
                int totalLength = TextRenderer.MeasureText(e.Graphics, this.text.Text, this.Font, new Size(0, 0), TextFormatFlags.NoPadding).Width;
                x = (this.ClientRectangle.Width - totalLength) / 2;
            }

            if (textAlign == ContentAlignment.MiddleCenter || textAlign == ContentAlignment.MiddleLeft || textAlign == ContentAlignment.MiddleRight)
            {
                y = (this.ClientRectangle.Height - TextRenderer.MeasureText(e.Graphics, text.Text, this.Font, new Size(0, 0), TextFormatFlags.NoPadding).Height) / 2;
            }

            int foreColor = 1, newForeColor = 1;
            int backColor = 0, newBackColor = 0;
            bool bold = false, newBold = false;
            bool underline = false, newUnderline = false;
            bool change = false;

            for (int i = 0; i < text.Text.Length; i++)
            {
                if (text.Colors.ContainsKey(i))
                {
                    newForeColor = text.Colors[i].ForegroundColorCode;
                    newBackColor = text.Colors[i].BackgroundColorCode;
                    change = true;
                }

                if (text.Bold.Contains(i))
                {
                    newBold = !bold;
                    change = true;
                }

                if (text.Under.Contains(i))
                {
                    newUnderline = !underline;
                    change = true;
                }

                if (change || i == text.Text.Length - 1)
                {
                    if (p < i)
                    {
                        // draw text in whatever color
                        FontStyle style = FontStyle.Regular;
                        if (bold)
                            style = style|FontStyle.Bold;
                        if (underline)
                            style = style|FontStyle.Underline;
                        Font f = new Font(this.Font, style);
                        string s = text.Text.Substring(p, (i == text.Text.Length - 1 ? text.Text.Length : i) - p);
                        float len = TextRenderer.MeasureText(e.Graphics, s, f, new Size(0, 0), TextFormatFlags.NoPadding).Width;//(e.Graphics.MeasureString(s, f)).Width - 3;
                        if (backColor == 99)
                            TextRenderer.DrawText(e.Graphics, s, f, new Point((int)x, (int)y), Settings.Default.Colors[foreColor], TextFormatFlags.NoPadding);
                        else
                            TextRenderer.DrawText(e.Graphics, s, f, new Point((int)x, (int)y), Settings.Default.Colors[foreColor], Settings.Default.Colors[backColor], TextFormatFlags.NoPadding);
                        // measure text and advance x
                        x += len;
                    }
                    p = i;
                    change = false;

                    foreColor = newForeColor;
                    backColor = newBackColor;
                    bold = newBold;
                    underline = newUnderline;
                }
            }
        }
    }
}
