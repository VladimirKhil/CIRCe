using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using IRCProviders;
using IRCWindow.Properties;
using IRC.Client.Base;

namespace IRCWindow
{
    public class MultiColorCell: DataGridViewTextBoxCell
    {
        public override Type ValueType
        {
            get
            {
                return typeof(FormattedText);
            }
        }

        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            var text = value as FormattedText;
            if (text == null)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                return;
            }
            graphics.FillRectangle((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected ? new SolidBrush(System.Drawing.SystemColors.Highlight) : Brushes.White, cellBounds);
            graphics.DrawRectangle(Pens.Gray, new Rectangle(cellBounds.X - 1, cellBounds.Y - 1, cellBounds.Width, cellBounds.Height));
            int p = 0;
            float x = cellBounds.X;
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
                            style = style | FontStyle.Bold;
                        if (underline)
                            style = style | FontStyle.Underline;
                        Font f = new Font(cellStyle.Font, style);
                        string s = text.Text.Substring(p, (i == text.Text.Length - 1 ? text.Text.Length : i) - p);
                        float len = TextRenderer.MeasureText(graphics, s, f, new Size(0, 0), TextFormatFlags.NoPadding).Width;//(e.Graphics.MeasureString(s, f)).Width - 3;

                        float y = cellBounds.Y + (cellBounds.Height - TextRenderer.MeasureText(graphics, text.Text, f, new Size(0, 0), TextFormatFlags.NoPadding).Height) / 2;

                        if (backColor == 99)
                            TextRenderer.DrawText(graphics, s, f, new Point((int)x, (int)y), Settings.Default.Colors[foreColor], TextFormatFlags.NoPadding);
                        else
                            TextRenderer.DrawText(graphics, s, f, new Point((int)x, (int)y), Settings.Default.Colors[foreColor], Settings.Default.Colors[backColor], TextFormatFlags.NoPadding);
                        // measure text and advance x
                        x += len;
                    }
                    p = i;
                    change = false;

                    if (newForeColor != -1)
                        foreColor = newForeColor;
                    if (newBackColor != -1)
                        backColor = newBackColor;
                    bold = newBold;
                    underline = newUnderline;
                }
            }
        }
    }
}
