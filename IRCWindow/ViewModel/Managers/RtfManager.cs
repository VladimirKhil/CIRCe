using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IRC.Client.Base;
using IRCWindow.Properties;
using System.Drawing;

namespace IRCWindow.ViewModel
{
    /// <summary>
    /// Класс, отвечающий за работу с RTF
    /// </summary>
    internal static class RtfManager
    {
        private static RichTextBox ShadowedRichTextBox = new RichTextBox();

        internal static int ColorIndex(Color color, bool full = true)
        {
            if (Settings.Default.Colors == null)
                return 0;
            int dist = -1;
            int bestI = 0;
            for (int i = 0; i < (full ? Settings.Default.Colors.Length : 16); i++)
            {
                Color col = Settings.Default.Colors[i];
                int d = Math.Abs(color.R - col.R) + Math.Abs(color.G - col.G) + Math.Abs(color.B - col.B);
                if (d < dist || dist == -1)
                {
                    dist = d;
                    bestI = i;
                }
            }
            return bestI;
        }

        internal static string[] RtfToIRC(RichTextBox richTextBox, ref int relativeIndex, int start = 0, int end = -1, bool split = true)
        {
            var results = new List<string>();
            var result = new StringBuilder(255);
            bool b = false, u = false, simple = false;
            int fc = 1, bc = 99;
            var relativeIndexSet = false;

            ShadowedRichTextBox.ForeColor = richTextBox.ForeColor;
            ShadowedRichTextBox.BackColor = richTextBox.BackColor;
            ShadowedRichTextBox.Rtf = richTextBox.Rtf; // работаем с фновым RichTextBox'ом, чтобы не отображать выделеения на основном           

            if (end == -1)
                end = ShadowedRichTextBox.Text.Length;

            for (int i = start; i < end; i++)
            {
                if (!relativeIndexSet && relativeIndex == i)
                {
                    relativeIndex = result.Length;
                    relativeIndexSet = true;
                }
                var c = ShadowedRichTextBox.Text[i];

                if (split && (c == '\n' || result.Length > 470))
                {
                    results.Add(result.ToString());
                    result = new StringBuilder(255);
                    fc = 1;
                    bc = 99;
                    simple = false;
                    continue;
                }

                if (result.Length == 0 && c == '/')
                    simple = true;

                if (!simple)
                {
                    ShadowedRichTextBox.SelectionStart = i;
                    ShadowedRichTextBox.SelectionLength = 1;
                    var font = ShadowedRichTextBox.SelectionFont ?? ShadowedRichTextBox.Font;
                    if (font.Bold != b)
                    {
                        result.Append(Symbols.Bold);
                        b = !b;
                    }
                    if (font.Underline != u)
                    {
                        result.Append(Symbols.Underlined);
                        u = !u;
                    }
                    int newFc = ColorIndex(ShadowedRichTextBox.SelectionColor);
                    int newBc = ShadowedRichTextBox.SelectionBackColor.ToArgb() == ShadowedRichTextBox.BackColor.ToArgb() ? 99 : ColorIndex(ShadowedRichTextBox.SelectionBackColor);
                    bool colorChanged = newFc != fc;
                    if (colorChanged
                        && newFc == Settings.Default.Colors.IndexOf(Settings.Default.Colors.DefForeColor)
                        && (newBc == Settings.Default.Colors.IndexOf(Settings.Default.Colors.DefBackColor) || newBc == 99))
                    {
                        newFc = newBc = 99;
                        if (!Char.IsDigit(c))
                        {
                            result.Append(Symbols.Color);
                            fc = newFc;
                            bc = newBc;
                            result.Append(c);
                            continue;
                        }
                    }

                    if (colorChanged)
                    {
                        result.Append(Symbols.Color);
                        if (Char.IsDigit(c) && newFc < 10 && newBc == bc)
                            result.Append('0');
                        result.Append(newFc);
                        fc = newFc;
                    }

                    if (newBc != bc)
                    {
                        if (!colorChanged)
                        {
                            result.Append(Symbols.Color);
                            result.Append(newFc);
                        }

                        result.Append(',');
                        if (Char.IsDigit(c) && newBc < 10)
                            result.Append('0');
                        result.Append(newBc);
                        bc = newBc;
                    }
                }
                result.Append(c);
            }
            results.Add(result.ToString());
            return results.ToArray();
        }
    }
}
