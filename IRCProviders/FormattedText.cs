using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IRCProviders
{
    /// <summary>
    /// Форматированный текст
    /// </summary>
    public class FormattedText
    {
        private string original = string.Empty;
        private string text = string.Empty;
        private Dictionary<int, Point> colors = new Dictionary<int, Point>();
        private List<int> bold = new List<int>();
        private List<int> under = new List<int>();
        private List<int> rev = new List<int>();

        /// <summary>
        /// Строка, из которой был сформирован данный текст
        /// </summary>
        public string Original { get { return this.original; } }

        /// <summary>
        /// Чистый текст
        /// </summary>
        public string Text
        {
            get { return text; }
        }

        /// <summary>
        /// Метки цветов в тексте
        /// </summary>
        public Dictionary<int, Point> Colors
        {
            get { return colors; }
        }        

        /// <summary>
        /// Метки жирности в тексте
        /// </summary>
        public List<int> Bold
        {
            get { return bold; }
        }        

        /// <summary>
        /// Метки подчёркивания в тексте
        /// </summary>
        public List<int> Under
        {
            get { return under; }
        }

        /// <summary>
        /// Метки реверсивности в тексте
        /// </summary>
        public List<int> Rev
        {
            get { return rev; }
        }

        /// <summary>
        /// Создание форматированнного текста
        /// </summary>
        public FormattedText() { }

        /// <summary>
        /// Создание форматированнного текста
        /// </summary>
        /// <param name="original">Строка, из которой следует сформировтаь текст</param>
        /// <param name="defForeColorIndex">Индекс цвета текста по умолчнию</param>
        /// <param name="defBackColorIndex">Индекс цвета фона по умолчанию</param>
        public FormattedText(string original, int defForeColorIndex, int defBackColorIndex)
        {
            this.original = original;
            int lastBackColor = -1;
            var innerText = new StringBuilder();
            colors[0] = new Point(defForeColorIndex, defBackColorIndex);

            for (int i = 0; i < original.Length; i++)
                switch (original[i])
                {
                    case Special.Color:

                        int colorNum = -1;
                        int backColorNum = -1;
                        if (i + 1 < original.Length && !Char.IsDigit(original[i + 1]))
                        {
                            // Код цвета без цифр далее, съедаем
                            // Новое веяение: сбрасываем цвета в умолчания
                            colors[innerText.Length] = new Point(defForeColorIndex, defBackColorIndex);
                            lastBackColor = defBackColorIndex;
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
                                colorNum = defForeColorIndex;

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
                                if (backColorNum == 99)
                                    backColorNum = defBackColorIndex;

                                lastBackColor = backColorNum;
                            }
                            else if (colors.Count > 0)
                                backColorNum = lastBackColor;
                        }

                        colors[innerText.Length] = new Point(colorNum, backColorNum);
                        break;

                    case Special.Bold:
                        bold.Add(innerText.Length);
                        break;

                    case Special.Underlined:
                        under.Add(innerText.Length);
                        break;

                    case Special.Reverse:
                        //rev.Add(innerText.Length);
                        colors[innerText.Length] = new Point(defBackColorIndex, defForeColorIndex);
                        break;

                    case Special.Plain:
                        colors[innerText.Length] = new Point(defForeColorIndex, defBackColorIndex);
                        if (bold.Count % 2 == 1 && bold[bold.Count - 1] == innerText.Length)
                            bold.RemoveAt(bold.Count - 1);

                        if (bold.Count % 2 == 1)
                            bold.Add(innerText.Length);

                        if (under.Count % 2 == 1 && under[under.Count - 1] == innerText.Length)
                            under.RemoveAt(under.Count - 1);

                        if (under.Count % 2 == 1)
                            under.Add(innerText.Length);
                        break;

                    default:
                        innerText.Append(original[i]);
                        break;
                }

            this.text = innerText.ToString();

            ClearList(bold);
            ClearList(under);
            ClearList(rev);
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
    }
}
