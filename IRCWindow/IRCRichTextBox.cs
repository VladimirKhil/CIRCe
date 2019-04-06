using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using System.Threading;
using IRCWindow.Properties;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using IRC.Client.Base;
using IRCWindow.ViewModel;

namespace IRCWindow
{
    /// <summary>
    /// Компонент, хранящий текст в IRC представлении и отображающий его в читабельном виде
    /// </summary>
    public partial class IRCRichTextBox : UserControl
    {
        private Color defaultColor = Color.Black;
        private bool editOnDoubleClick = false;

        public bool IsEditing
        {
            get { return !this.richTextBox1.ReadOnly; }
        }

        /// <summary>
        /// История введённых ранее сообщений
        /// </summary>
        List<string> msgHistory = new List<string>();
        /// <summary>
        /// Индекс в истории сообщений
        /// </summary>
        int historyInd = -1;
        string prevValue = string.Empty;

        public enum Mode
        {
            Normal,
            Editor
        }

        [Browsable(true)]
        public Mode WorkMode { get; set; }

		private static Win32.SCROLLINFO VScrollInfo(TextBoxBase richTextBox, int mask)
        {
            Win32.SCROLLINFO info = new Win32.SCROLLINFO();
            info.fMask = mask;
            info.cbSize = Marshal.SizeOf(info.GetType());
            Win32.GetScrollInfo(richTextBox.Handle, Win32.SB_VERT, ref info);
            return info;
        }

        /// <summary>
        /// Позиция вертикального скролла
        /// </summary>
        internal static int GetVScrollPos(TextBoxBase richTextBox)
        {
            Win32.SCROLLINFO info = VScrollInfo(richTextBox, Win32.SIF_ALL);
            if (info.nTrackPos == 0)
            {
                if (richTextBox.GetCharIndexFromPosition(new Point(2, 2)) == 0)
                    return 0;
                return info.nPos;
            }
            return info.nTrackPos;
        }

        internal int GetVScrollPos()
        {
            return GetVScrollPos(this.richTextBox1);
        }

        /// <summary>
        /// Позиция вертикального скролла
        /// </summary>
        internal static void SetVScrollPos(TextBoxBase richTextBox, int nPos)
        {
            Win32.SCROLLINFO info = new Win32.SCROLLINFO();
            info.cbSize = Marshal.SizeOf(info);
            info.fMask = Win32.SIF_POS;
            info.nPos = nPos;
            int oldPos = GetVScrollPos(richTextBox);
            Win32.SetScrollInfo(richTextBox.Handle, Win32.SB_VERT, ref info, true);
            int newPos = GetVScrollPos(richTextBox);

            IntPtr ptrWparam = new IntPtr(Win32.SB_THUMBTRACK + 0x10000 * newPos);
            IntPtr ptrLparam = new IntPtr(0);
            Win32.SendMessage(richTextBox.Handle, WM_VSCROLL, ptrWparam, ptrLparam);
        }

        internal void SetVScrollPos(int nPos)
        {
            SetVScrollPos(this.richTextBox1, nPos);
        }

        /// <summary>
        /// Максимальная позиция вертикального скролла
        /// </summary>
        private static int VScrollPosMax(RichTextBox richTextBox)
        {
            Win32.SCROLLINFO info = VScrollInfo(richTextBox, Win32.SIF_RANGE);
            return info.nMax;
        }

        /// <summary>
        /// Минимальная позиция вертикального скролла
        /// </summary>
        private static int VScrollPosMin(RichTextBox richTextBox)
        {
            Win32.SCROLLINFO info = VScrollInfo(richTextBox, Win32.SIF_RANGE);
            return info.nMin;
        }

        private const int WM_VSCROLL = 277;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_TOP = 6;
        private const int SB_BOTTOM = 7;

        public static void ScrollToBottom(RichTextBox richTextBox)
        {
            Win32.SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
        }

        public static void ScrollToTop(RichTextBox richTextBox)
        {
            Win32.SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_TOP, IntPtr.Zero);
        }

        public static void ScrollLineDown(RichTextBox richTextBox)
        {
            Win32.SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_LINEDOWN, IntPtr.Zero);
        }

        public static void ScrollLineUp(RichTextBox richTextBox)
        {
            Win32.SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_LINEUP, IntPtr.Zero);
        }

        [Browsable(true)]
        public bool ReadOnly
        {
            get { return richTextBox1.ReadOnly; }
            set { richTextBox1.ReadOnly = value; }
        }

        [Browsable(true)]
        public bool EditOnDoubleClick
        {
            get { return editOnDoubleClick; }
            set { editOnDoubleClick = value; }
        }

        [Browsable(true)]
        public Color DefaultColor
        {
            get { return defaultColor; }
            set { defaultColor = value; }
        }

        [Browsable(true)]
        public new string Text
        {
            get { return GenerateCodes(richTextBox1)[0]; }
            set { Clear(); if (!value.Equals(string.Empty)) SetText(value); }
        }

        [Browsable(true)]
        public new Color ForeColor
        {
            get { return richTextBox1.ForeColor; }
            set { richTextBox1.ForeColor = value; }
        }

        [Browsable(true)]
        public new Color BackColor
        {
            get { return richTextBox1.BackColor; }
            set 
            {
                richTextBox1.BackColor = value;             
            }
        }

        [Browsable(true)]
        public int SelectionStart
        {
            get { return richTextBox1.SelectionStart; }
            set { richTextBox1.SelectionStart = value; }
        }

        [Browsable(true)]
        public int SelectionLength
        {
            get { return richTextBox1.SelectionLength; }
            set { richTextBox1.SelectionLength = value; }
        }

        [Browsable(true)]
        public Color SelectionColor
        {
            get { return richTextBox1.SelectionColor; }
            set { richTextBox1.SelectionColor = value; }
        }

        [Browsable(true)]
        public Color SelectionBackColor
        {
            get { return richTextBox1.SelectionBackColor; }
            set { richTextBox1.SelectionBackColor = value; }
        }

        public void Select(int start, int length)
        {
            richTextBox1.Select(start, length);
        }

        /// <summary>
        /// Всплывающая подсказка
        /// </summary>
        [Browsable(true)]
        public string ToolTip
        {
            get { return this.toolTip1.GetToolTip(this.richTextBox1); }
            set { this.toolTip1.SetToolTip(this.richTextBox1, value); }
        }

        /// <summary>
        /// Используемый шрифт
        /// </summary>
        [DefaultValue("Calibri; 9,75pt")]
        [Browsable(false)]
        public Font InnerFont
        {
            get
            {
                return this.richTextBox1.Font;
            }
            set
            {
                if (value != null)
                {
                    string oldFormat = GenerateCodes(richTextBox1)[0];
                    richTextBox1.Clear();
                    richTextBox1.Font = value;
                    this.Font = value;
                    SetText(oldFormat);
                }
            }
        }

        internal RichTextBox RichTextBox
        {
            get { return richTextBox1; }
        }

        /// <summary>
        /// Событие, возникающее при завершении пользователем редактирования текста
        /// </summary>
        [Browsable(true)]
        public event EventHandler<EnterPushedEventArgs> AcceptEdit;
        [Browsable(true)]
        public event EventHandler<EnterPushedEventArgs> DeclineEdit;

        [Browsable(true)]
        public event KeyEventHandler InputKeyDown;
        [Browsable(true)]
        public event KeyPressEventHandler InputKeyPress;
        [Browsable(true)]
        public event MouseEventHandler InnerMouseWheel;
        
        public IRCRichTextBox()
        {
            InitializeComponent();

            this.WorkMode = Mode.Normal;

            this.tsmiCut.Click += (sender, e) => this.richTextBox1.Cut();
            this.tsmiCopy.Click += (sender, e) => this.richTextBox1.Copy();
            this.tsmiPaste.Click += (sender, e) => this.richTextBox1.Paste();
            this.tsmiDelete.Click += (sender, e) => this.richTextBox1.SelectedText = string.Empty;

            this.richTextBox1.MouseWheel += (sender, e) => { if (this.InnerMouseWheel != null) InnerMouseWheel(sender, e); };
        }

        private void richTextBox1_ReadOnlyChanged(object sender, EventArgs e)
        {
            panel2.Visible = !richTextBox1.ReadOnly;
        }

        public void Clear()
        {
            this.richTextBox1.Multiline = true;
            this.richTextBox1.Clear();
            this.richTextBox1.Multiline = false;
            this.richTextBox1.SelectionStart = 0;
            this.richTextBox1.SelectionBackColor = this.BackColor;
            this.richTextBox1.SelectionFont = new Font(this.richTextBox1.SelectionFont ?? this.richTextBox1.Font, FontStyle.Regular);
        }

        internal void SetText(string original)
        {
            if (original.Length == 0)
                return;
            SetText(this.richTextBox1, original, defaultColor, this.Font);
        }
        
        /// <summary>
        /// Помещает в текстовый редактор размеченный текст
        /// </summary>
        /// <param name="richTextBox">Текстовый редактор</param>
        /// <param name="original">Строка с разметкой в виде кодов IRC</param>
        /// <param name="defaultColor">Цвет для текста по умолчанию</param>
        internal static void SetText(RichTextBox richTextBox, string original, Color defaultColor, Font font)
        {
            int start = richTextBox.Text.Length;
            int selStart = richTextBox.SelectionStart;
            int selLength = richTextBox.SelectionLength;

            int index = richTextBox.GetCharIndexFromPosition(new Point(2, 2)); 

            #region Text

            var text = new FormattedText(original,
                RtfManager.ColorIndex(defaultColor),
                99);

            int vpos = GetVScrollPos(richTextBox);
            int vmax = VScrollPosMax(richTextBox);
            bool restoreSelection = richTextBox.HideSelection == false;
            if (restoreSelection)
                richTextBox.HideSelection = true; // VERY IMPORTANT!!!
            richTextBox.AppendText(text.Text);
            richTextBox.Select(start, text.Text.Length);
            richTextBox.SelectionColor = defaultColor;
            richTextBox.SelectionFont = font;

            #endregion

            #region Style

            foreach (var pair in text.Colors)
            {
                richTextBox.Select(start + pair.Key, text.Text.Length - pair.Key);
                richTextBox.SelectionColor = pair.Value.ForegroundColorCode == -1 || pair.Value.ForegroundColorCode >= Settings.Default.Colors.Length ? Settings.Default.Colors.DefForeColor : Settings.Default.Colors[pair.Value.ForegroundColorCode];
                if (pair.Value.BackgroundColorCode != 99 && pair.Value.BackgroundColorCode != -1)
                    richTextBox.SelectionBackColor = pair.Value.BackgroundColorCode == -1 || pair.Value.BackgroundColorCode >= Settings.Default.Colors.Length ? Settings.Default.Colors.DefBackColor : Settings.Default.Colors[pair.Value.BackgroundColorCode];
                else
                    richTextBox.SelectionBackColor = richTextBox.BackColor;
            }

            bool b = false, u = false;
            int bi = 0, ui = 0;
            int j = -1;
            
            for (int i = 0; i < text.Text.Length + 1; i++)
                if (bi < text.Bold.Count && text.Bold[bi] == i ||
                    ui < text.Under.Count && text.Under[ui] == i ||
                    i == text.Text.Length)
                {
                    if (j > -1)
                    {
                        richTextBox.Select(start + j, i - j);
                        FontStyle style = b ? FontStyle.Bold : FontStyle.Regular;
                        if (u)
                            style = style | FontStyle.Underline;
                        richTextBox.SelectionFont = new Font(richTextBox.SelectionFont ?? richTextBox.Font, style);
                    }

                    j = i;

                    bool changed = false;
                    if (bi < text.Bold.Count && text.Bold[bi] == i)
                    {
                        b = !b;
                        bi++;
                        changed = true;
                    }

                    if (ui < text.Under.Count && text.Under[ui] == i)
                    {
                        u = !u;
                        ui++;
                        changed = true;
                    }

                    if (i == text.Text.Length && changed)
                    {
                        richTextBox.Select(text.Text.Length, 0);
                        FontStyle style = b ? FontStyle.Bold : FontStyle.Regular;
                        if (u)
                            style = style | FontStyle.Underline;
                        richTextBox.SelectionFont = new Font(richTextBox.SelectionFont ?? richTextBox.Font, style);
                    }
                }
            
            //for (int i = 0; i < text.Rev.Count; i += 2)
            //{
            //    int l = text.Rev[i];
            //    int r = i + 1 < text.Rev.Count ? text.Rev[i + 1] : text.Text.Length;
            //    richTextBox.Select(start + l, r - l);
            //    richTextBox.SelectionColor = Settings.Default.Colors.DefBackColor;
            //    richTextBox.SelectionBackColor = Settings.Default.Colors.DefForeColor;
            //}
            
            #endregion            
            
            try
            {
                if (vpos > vmax - richTextBox.Height * 1.05 && selLength == 0)
                {
                    richTextBox.Select(richTextBox.TextLength, 0);
                    richTextBox.ScrollToCaret();
                }
                else
                {
                    richTextBox.SelectionLength = 0;
                    SetVScrollPos(richTextBox, vpos);
                    if (selLength > 0)
                    {
                        richTextBox.Select(index, 0);
                        richTextBox.Select(selStart, selLength);
                    }
                }
            }
            catch (Exception) { }
            if (restoreSelection)
                richTextBox.HideSelection = false;
        }
        

        internal static void SetColorTable(RichTextBox richTextBox)
        {
            string text = Settings.Default.Colors.CreateRtfColorTable();
            int i = richTextBox.Rtf.IndexOf(@"{\colortbl");
            if (i > 0)
                richTextBox.Rtf = Regex.Replace(richTextBox.Rtf, "{\\colortbl.*}", text);
            else
            {
                i = richTextBox.Rtf.IndexOf('\n');
                richTextBox.Rtf = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204{\\*\\fname Courier New;}Courier New CYR;}{\\f1\\fnil\\fcharset204 Calibri;}}\r\n{\\colortbl ;\\red175\\green5\\blue5;\\red275\\green5\\blue5;}\r\n\\viewkind4\\uc1\\pard\\cf2\\cf1 \\f0\\fs20 \\cf0\\f1\\fs23\\par\r\n}\r\n";
                //richTextBox.Rtf = richTextBox.Rtf.Insert(i + 1, text + "\r\n");
            }
        }
        /*
        /// <summary>
        /// Помещает в текстовый редактор размеченный текст
        /// </summary>
        /// <param name="richTextBox">Текстовый редактор</param>
        /// <param name="original">Строка с разметкой в виде кодов IRC</param>
        /// <param name="defaultColor">Цвет для текста по умолчанию</param>
        internal static void SetText(RichTextBox richTextBox, string original, Color defaultColor, Font font)
        {
            if (original.Length == 0)
                return;
            StringBuilder rtf = new StringBuilder("{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204{\\*\\fname Courier New;}Courier New CYR;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs20");
            bool bold = false;
            bool underlined = false;

            #region Text

            for (int i = 0; i < original.Length; i++)
                switch (original[i])
                {
                    case Special.Color:
                        int colorNum = -1;
                        int backColorNum = -1;
                        if (i + 1 < original.Length && !Char.IsDigit(original[i + 1]))
                        {
                            // Код цвета без цифр далее, съедаем
                        }
                        else if (i + 1 < original.Length)
                        {
                            colorNum = int.Parse(original[i + 1].ToString());
                            i++;

                            if (i + 1 < original.Length && Char.IsDigit(original[i + 1]))
                            {
                                int addNum = int.Parse(original[i + 1].ToString());
                                colorNum = colorNum * 10 + addNum;
                                i++;
                            }
                            if (colorNum == 99)
                                colorNum = ColorIndex(defaultColor);

                            if (i + 2 < original.Length && original[i + 1] == ',' && Char.IsDigit(original[i + 2]))
                            {
                                backColorNum = int.Parse(original[i + 2].ToString());
                                i += 2;
                                if (i + 1 < original.Length && Char.IsDigit(original[i + 1]))
                                {
                                    int addNum = int.Parse(original[i + 1].ToString());
                                    backColorNum = backColorNum * 10 + addNum;
                                    i++;
                                }

                                //lastBackColor = backColorNum;
                            }
                            //else if (colors.Count > 0)
                            //    backColorNum = lastBackColor;
                        }

                        //colors[innerText.Length] = new Point(colorNum, backColorNum);
                        break;

                    case Special.Bold:
                        bold = !bold;
                        rtf.Append(bold ? @"\b " : @"\b0 ");
                        break;

                    case Special.Underlined:
                        underlined = !underlined;
                        rtf.Append(underlined ? @"\ul " : @"\ul0 ");
                        break;

                    case Special.Reverse:
                        //rev.Add(innerText.Length);
                        break;

                    case Special.Plain:
                        //colors[innerText.Length] = new Point(1, 0);
                        //if (bold.Count % 2 == 1 && bold[bold.Count - 1] == innerText.Length)
                        //    bold.RemoveAt(bold.Count - 1);

                        //if (bold.Count % 2 == 1)
                        //    bold.Add(innerText.Length);

                        //if (under.Count % 2 == 1 && under[under.Count - 1] == innerText.Length)
                        //    under.RemoveAt(under.Count - 1);

                        //if (under.Count % 2 == 1)
                        //    under.Add(innerText.Length);
                        break;

                    default:
                        //rtf.Append("\\u" + char.ConvertToUtf32(original, i).ToString());
                        rtf.Append(original[i]);
                        break;
                }

            rtf.Append("\\par\r\n}\r\n");

            int vpos = GetVScrollPos(richTextBox);
            int vmax = VScrollPosMax(richTextBox);
            richTextBox.Select(richTextBox.TextLength, 0);
            richTextBox.SelectedRtf = rtf.ToString();
            //richTextBox.Rtf = richTextBox.Rtf.Insert(richTextBox.Rtf.Length - 3, rtf.ToString());
            #endregion

            if (vpos > vmax - richTextBox.Height - 20 && richTextBox.SelectionLength == 0)
            {
                richTextBox.Select(richTextBox.Text.Length, 0);
                richTextBox.ScrollToCaret();
            }
        }
        */

        protected internal static string[] GenerateCodes(RichTextBox richTextBox, int start = 0, int end = -1, bool split = true)
        {
            int dummy = -1;
            return GenerateCodes(richTextBox, ref dummy, start, end, split);
        }

        /// <summary>
        /// Создать сообщение с IRC-кодами на основе форматированного текста
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <returns></returns>
        protected internal static string[] GenerateCodes(RichTextBox richTextBox, ref int relativeIndex, int start = 0, int end = -1, bool split = true)
        {
            return RtfManager.RtfToIRC(richTextBox, ref relativeIndex, start, end, split);
        }

        private static void ClearList(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
                if (list[i] == list[i + 1])
                {
                    list.RemoveAt(i);
                    list.RemoveAt(i);
                    i--;
                }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (InputKeyDown != null)
                InputKeyDown(this, e);

            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.K:
                        InsertColor(false);
                        e.Handled = true;
                        break;

                    case Keys.L:
                        InsertColor(true);
                        e.Handled = true;
                        break;

                    case Keys.B:
                        Bold();
                        e.Handled = true;
                        break;

                    case Keys.U:
                        Underline();
                        e.Handled = true;
                        break;

                    case Keys.R:
                        Reverse();
                        e.Handled = true;
                        break;

                    case Keys.P:
                        Plain();
                        e.Handled = true;
                        break;

                    case Keys.V:
                        OnPaste();
                        break;

                    default:
                        return;
                }                
            }
            else if (e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        OnPaste();
                        break;

                    default:
                        return;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (this.WorkMode == Mode.Normal)
                        {
                            string text = this.Text;
                            if (text.Length == 0)
                                break;
                            if (AcceptEdit != null)
                                foreach (string s in GenerateCodes(richTextBox1))
                                {
                                    if (s.Length > 0)
                                    {
                                        AcceptEdit(this, new EnterPushedEventArgs(s));
                                    }
                                }
                            if (msgHistory.Count > 0)
                                msgHistory[msgHistory.Count - 1] = text;
                            else
                                msgHistory.Add(text);
                            msgHistory.Add("");
                            historyInd = msgHistory.Count - 1;
                        }
                        break;

                    case Keys.Up:
                        if (msgHistory.Count == 0)
                            break;
                        historyInd = Math.Max(0, historyInd - 1);
                        this.Text = msgHistory[historyInd];
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        break;

                    case Keys.Down:
                        if (msgHistory.Count == 0)
                            break;
                        historyInd = Math.Min(msgHistory.Count - 1, historyInd + 1);
                        this.Text = msgHistory[historyInd];
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        break;

                    case Keys.Escape:
                        Cancel();
                        break;
                }
            }
        }

        private void Cancel()
        {
            if (!ReadOnly && editOnDoubleClick)
                this.ReadOnly = true;
            if (DeclineEdit != null)
                DeclineEdit(this, new EnterPushedEventArgs(this.Text));
            this.Text = prevValue;
        }
 
        private void OnPaste()
        {
            var wholeText = Clipboard.GetText();
            var text = wholeText.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                var s = text[i].Trim();
                if (s.Length > 0)
                    result.Add(s);
            }
            text = result.ToArray();
            if (text.Length > 1 && AcceptEdit != null)
            {
                //this.Clear();
                var memoryTB = new RichTextBox()
                {
                    Multiline = true
                };

                if (this.richTextBox1.TextLength > 0)
                {
                    memoryTB.SelectedText = this.richTextBox1.Text;
                    memoryTB.SelectionStart = memoryTB.TextLength;
                    this.Clear();
                }

                memoryTB.Paste();

                new Thread(PasteWork).Start(GenerateCodes(memoryTB));
            }
        }

        private void PasteWork(object param)
        {
            var strings = (string[])param;
            int counter = 0;
            foreach (string s in strings)
            {
                AcceptEdit(this, new EnterPushedEventArgs(s));
                counter++;
                if (counter > 5)
                    Thread.Sleep(1250);
            }
        }

        private void InsertColor(bool full)
        {
            ColorChooseDialog colorChooseDialog = new ColorChooseDialog(full ? Resources.Background : Resources.Text);
            colorChooseDialog.Location = new Point(MousePosition.X, Math.Max(MousePosition.Y - colorChooseDialog.Height, 0));
            colorChooseDialog.Selected += new EventHandler<ColorSelectedEventArgs>(colorChooseDialog_Selected);
            colorChooseDialog.ShowDialog();
        }

        void colorChooseDialog_Selected(object sender, ColorSelectedEventArgs e)
        {
            if (e.ChoosenColor != -1)
            {
                if (((ColorChooseDialog)sender).Title == "Текст")
                    richTextBox1.SelectionColor = Settings.Default.Colors[e.ChoosenColor];
                else
                    richTextBox1.SelectionBackColor = Settings.Default.Colors[e.ChoosenColor];
            }
            var c = ((ColorChooseDialog)sender).LastChar;
            if (c != '\0')
                richTextBox1.SelectedText = c.ToString();
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (editOnDoubleClick)
            {
                this.richTextBox1.ReadOnly = false;
                prevValue = this.Text;
            }
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (!ReadOnly && editOnDoubleClick)
                Cancel();
        }

        private void richTextBox1_RightToLeftChanged(object sender, EventArgs e)
        {
            richTextBox1.RightToLeft = RightToLeft.No;
        }

        private void foreColorButton_Click(object sender, EventArgs e)
        {
            InsertColor(false);
        }

        private void foreBackColorButton_Click(object sender, EventArgs e)
        {
            InsertColor(true);
        }

        private void Bold()
        {
            var font = richTextBox1.SelectionFont ?? richTextBox1.Font;
            FontStyle newStyleBold = (font.Bold ? font.Style & ~FontStyle.Bold : font.Style | FontStyle.Bold);
            richTextBox1.SelectionFont = new Font(font, newStyleBold);
        }

        private void Underline()
        {
            var font = richTextBox1.SelectionFont ?? richTextBox1.Font;
            FontStyle newStyleUnderline = (font.Underline ? font.Style & ~FontStyle.Underline : font.Style | FontStyle.Underline);
            richTextBox1.SelectionFont = new Font(font, newStyleUnderline);
        }

        private void Reverse()
        {
            var selColor = richTextBox1.SelectionColor;
            richTextBox1.SelectionColor = richTextBox1.SelectionBackColor;
            richTextBox1.SelectionBackColor = selColor;
        }

        private void Plain()
        {
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;            
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont ?? richTextBox1.Font, FontStyle.Regular);
        }
        
        private void boldButton_Click(object sender, EventArgs e)
        {
            Bold();
        }

        private void underlineButton_Click(object sender, EventArgs e)
        {
            Underline();
        }

        private void reverseButton_Click(object sender, EventArgs e)
        {
            Reverse();
        }

        private void plainButton_Click(object sender, EventArgs e)
        {
            Plain();
        }

        /// <summary>
        /// Восстановить значение, которое было в данном элементе до начала редактирвоания
        /// </summary>
        internal void Rollback()
        {
            this.Text = prevValue;
        }

        internal void SetFocus()
        {
            richTextBox1.Focus();
        }

        internal void Paste()
        {
            richTextBox1.Paste();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (InputKeyPress != null)
                InputKeyPress(this, e);
        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            bool mode = this.boldButton.Visible;

            if (mode)
            {
                this.hideButton.Image = Resources.arrow_left;
                this.hideButton.ToolTipText = Resources.ShowEditorPanel;
                this.panel2.Width = 30;
            }
            else
            {
                this.hideButton.Image = Resources.arrow_right;
                this.hideButton.ToolTipText = Resources.HideEditorPanel;
                this.panel2.Width = 165;
            }

            this.boldButton.Visible = !mode;
            this.underlineButton.Visible = !mode;
            this.plainButton.Visible = !mode;
            this.reverseButton.Visible = !mode;
            this.foreColorButton.Visible = !mode;
            this.backColorButton.Visible = !mode;
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            MessageBox.Show("aaa");
        }

        internal void HideEditor()
        {
            if (this.boldButton.Visible)
                this.hideButton_Click(this, new EventArgs());
        }
    }
}
