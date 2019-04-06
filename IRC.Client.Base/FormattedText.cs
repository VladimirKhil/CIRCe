using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IRC.Client.Base
{
    /// <summary>
    /// Форматированный текст
    /// </summary>
    [Serializable]
    public sealed class FormattedText
    {
        /// <summary>
        /// Строка, из которой был сформирован данный текст
        /// </summary>
        public string Original { get; private set; }

        /// <summary>
        /// Чистый текст
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Метки цветов в тексте
        /// </summary>
        public Dictionary<int, ColorInfo> Colors { get; private set; }

        /// <summary>
        /// Метки жирности в тексте
        /// </summary>
        public List<int> Bold { get; private set; }

        /// <summary>
        /// Метки подчёркивания в тексте
        /// </summary>
        public List<int> Under { get; private set; }

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
            this.Original = original;
            int lastBackColor = -1;
            var innerText = new StringBuilder();

            this.Colors = new Dictionary<int, ColorInfo>();
            this.Bold = new List<int>();
            this.Under = new List<int>();

            this.Colors[0] = new ColorInfo(defForeColorIndex, defBackColorIndex);
            var lastColorInfo = this.Colors[0];

            for (int i = 0; i < original.Length; i++)
                switch (original[i])
                {
                    case Symbols.Color:
                        int colorNum = -1;
                        int backColorNum = -1;
                        if (i + 1 < original.Length && !Char.IsDigit(original[i + 1]))
                        {
                            // Код цвета без цифр далее, съедаем
                            // Новое веяние: сбрасываем цвета в умолчания
                            lastColorInfo = this.Colors[innerText.Length] = new ColorInfo(defForeColorIndex, defBackColorIndex);
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
                            else if (this.Colors.Count > 0)
                                backColorNum = lastBackColor;

                            lastColorInfo = this.Colors[innerText.Length] = new ColorInfo(colorNum, backColorNum == -1 ? lastColorInfo.BackgroundColorCode : backColorNum);
                        }
                        
                        break;

                    case Symbols.Bold:
                        this.Bold.Add(innerText.Length);
                        break;

                    case Symbols.Underlined:
                        this.Under.Add(innerText.Length);
                        break;

                    case Symbols.Reverse:
                        lastColorInfo = this.Colors[innerText.Length] = new ColorInfo(lastColorInfo.BackgroundColorCode, lastColorInfo.ForegroundColorCode);
                        break;

                    case Symbols.Plain:
                        lastColorInfo = this.Colors[innerText.Length] = new ColorInfo(defForeColorIndex, defBackColorIndex);
                        if (this.Bold.Count % 2 == 1 && this.Bold[this.Bold.Count - 1] == innerText.Length)
                            this.Bold.RemoveAt(this.Bold.Count - 1);

                        if (this.Bold.Count % 2 == 1)
                            this.Bold.Add(innerText.Length);

                        if (this.Under.Count % 2 == 1 && this.Under[this.Under.Count - 1] == innerText.Length)
                            this.Under.RemoveAt(this.Under.Count - 1);

                        if (this.Under.Count % 2 == 1)
                            this.Under.Add(innerText.Length);
                        break;

                    default:
                        innerText.Append(original[i]);
                        break;
                }

            this.Text = innerText.ToString();

            ClearList(this.Bold);
            ClearList(this.Under);
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
