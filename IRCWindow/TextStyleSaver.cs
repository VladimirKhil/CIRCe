using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Объект, позволяющий сохранить часть отформатированного текста из заданного RichTextBox'а, а азтем восстановить жто оформление. Используется для временной подсветки участков текста
    /// </summary>
    public class TextStyleSaver
    {
        private RichTextBox richTB = null;
        private string rtf = string.Empty;

        /// <summary>
        /// Индекс левой позиции выбранного фрагмента текста
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// Индекс правой позиции выбранного фрагмента текста
        /// </summary>
        public int Right { get; set; }

        public TextStyleSaver()
        {
            this.Left = -1;
            this.Right = -1;
        }

        /// <summary>
        /// Скопировать оформление
        /// </summary>
        /// <param name="richTB">редактор, содержащий текст</param>
        /// <param name="left">Индекс левой позиции выбранного фрагмента текста</param>
        /// <param name="right">Индекс правой позиции выбранного фрагмента текста</param>
        public void CopyFrom(RichTextBox richTB, int left, int right)
        {
            this.Left = left;
            this.Right = right;
            this.richTB = richTB;

            this.richTB.SelectionStart = left;
            this.richTB.SelectionLength = right - left;
            rtf = this.richTB.SelectedRtf;
        }

        /// <summary>
        /// Вставить оформление
        /// </summary>
        /// <param name="richTB"></param>
        public void Paste()
        {
            this.richTB.SelectionStart = this.Left;
            this.richTB.SelectionLength = this.Right - this.Left;
            this.richTB.SelectedRtf = this.rtf;
        }
    }
}
