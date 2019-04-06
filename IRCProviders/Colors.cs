using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace IRCProviders
{
    /// <summary>
    /// Цвета IRC
    /// </summary>
    [TypeConverter(typeof(ColorsTypeConverter))]
    public class Colors: IEnumerable<Color>
    {
        #region Default Colors
        Color[] defColors = new Color[]{
                // 0
                SystemColors.ControlLightLight,
                Color.Black,
                Color.DarkBlue,
                Color.DarkGreen,
                Color.Red,
                Color.Brown,
                Color.Purple,
                Color.Orange,
                Color.Yellow,
                Color.FromArgb(0, 255, 128),
                // 10
                Color.LightSeaGreen,
                Color.LightSkyBlue,
                Color.Blue,
                Color.Fuchsia,
                Color.FromArgb(85, 85, 85),
                Color.FromArgb(220, 220, 220),
                // End of standart IRC Colors
                Color.BurlyWood,
                Color.BlueViolet,
                Color.BlanchedAlmond,
                Color.Beige,
                // 20
                 Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //30
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //40
                 Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //50
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //60
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //70
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //80
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                Color.White,
                //90
                Color.DarkKhaki,
                Color.DarkRed,
                Color.DarkCyan,
                Color.OliveDrab,
                Color.DarkTurquoise,
                Color.DarkViolet,
                Color.DarkSlateGray,
                Color.Olive,
                Color.YellowGreen,
        };
        #endregion

        private Color[] colors;

        /// <summary>
        /// Создать таблицу цветов в формате RTF на основе текущих цветовых настроек
        /// </summary>
        /// <returns>Созданная цветовая таблица</returns>
        public string CreateRtfColorTable()
        {
            StringBuilder result = new StringBuilder(@"{\colortbl ");
            foreach (Color color in this.colors)
            {
                result.Append(@"\red");
                result.Append(color.R);
                result.Append(@"\green");
                result.Append(color.G);
                result.Append(@"\blue");
                result.Append(color.B);
                result.Append(';');
            }
            result.Append('}');
            return result.ToString();
        }
        
        /// <summary>
        /// Список цветов в данной настройке приложения
        /// </summary>
        public Color[] ColorList
        {
            get { return colors; }
            set { colors = value; }
        }

        /// <summary>
        /// Количество цветов
        /// </summary>
        public int Length
        {
            get { return colors.Length; }
        }

        /// <summary>
        /// Цвет по индексу
        /// </summary>
        /// <param name="i">Индекс цвета</param>
        /// <returns></returns>
        public Color this[int i]
        {
            get
            {
                if (i >= 0 && i < colors.Length)
                    return colors[i];
                return DefForeColor;
            }
            set
            {
                if (i >= 0 && i < colors.Length)
                    colors[i] = value;
            }
        }

        /// <summary>
        /// Цвет по умолчанию
        /// </summary>
        public Color DefForeColor
        {
            get { return colors[1]; }
        }

        /// <summary>
        /// Цвет фона по умолчанию
        /// </summary>
        public Color DefBackColor
        {
            get { return colors[0]; }
        }

        /// <summary>
        /// Создание настроек цветов
        /// </summary>
        public Colors()
        {
            colors = new Color[defColors.Length];
            MakeDefault();
        }

        /// <summary>
        /// Получить индекс цвета в таблице цветов
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <returns>Индекс цвета в таблице цветов или -1, если цвет не был найден</returns>
        public int IndexOf(Color color)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].Equals(color))
                {
                    return i;
                }
            }
            return -1;
        }

        #region IEnumerable<Color> Members

        public IEnumerator<Color> GetEnumerator()
        {
            for (int i = 0; i < colors.Length; i++)
                yield return colors[i];
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < colors.Length; i++)
                yield return colors[i];
        }

        #endregion

        /// <summary>
        /// Установить в таблице значения цветов по умолчанию
        /// </summary>
        public void MakeDefault()
        {
            colors = (Color[])defColors.Clone();

            for (int i = 0; i < 5; i++)
            {
                int first = i == 0 ? 128 : 64 * (5 - i) / 5;
                int second = i == 0 ? 224 : 192 * (5 - i) / 5;
                int third = i == 0 ? 255 : 255 * (5 - i) / 5;

                int big = 245 - Math.Max(0, i - 2) * 70;
                int small = 170 - Math.Min(i, 3) * 55;

                colors[20 + i] = Color.FromArgb(small, big, big);
                colors[25 + i] = Color.FromArgb(first, third, second);                
                colors[30 + i] = Color.FromArgb(big, small, big);
                colors[35 + i] = Color.FromArgb(second, first, third);                
                colors[40 + i] = Color.FromArgb(big, big, small);
                colors[45 + i] = Color.FromArgb(third, second, first);                
                colors[50 + i] = Color.FromArgb(small, small, big);
                colors[55 + i] = Color.FromArgb(first, second, third);
                colors[60 + i] = Color.FromArgb(small, big, small);
                colors[65 + i] = Color.FromArgb(second, third, first);                
                colors[70 + i] = Color.FromArgb(big, small, small);
                colors[75 + i] = Color.FromArgb(third, first, second);
                colors[80 + i] = Color.FromArgb(240 - i * 50, 240 - i * 50, 240 - i * 50);
                colors[85 + i] = Color.FromArgb(second, third, second);
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Клонировать цветовую таблицу
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Colors colors = new Colors();
            this.colors.CopyTo(colors.colors, 0);
            return colors;
        }

        #endregion

        /// <summary>
        /// Сохранить палитру цветов в файл
        /// </summary>
        /// <param name="fileName">Имя файла для сохранения</param>
        public void Save(string fileName)
        {
            using (var writer = XmlWriter.Create(fileName))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Сохранить палитру цветов в строку
        /// </summary>
        /// <param name="str">Строка для сохранения</param>
        public void Save(StringBuilder str)
        {
            using (var writer = XmlWriter.Create(str))
            {
                Save(writer);
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement("Colors");

            foreach (Color color in this)
            {
                writer.WriteStartElement("item");
                writer.WriteValue(color.ToArgb());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Загрузить палитры цветов из файла
        /// </summary>
        /// <param name="fileName">Имя файла для загрузки</param>
        public void Load(string fileName)
        {
            using (var reader = XmlReader.Create(fileName))
            {
                Load(reader);
            }
        }

        /// <summary>
        /// Загрузить палитры цветов из строки
        /// </summary>
        /// <param name="fileName">Строка для загрузки</param>
        public void Load(StringBuilder str)
        {
            using (var sr = new StringReader(str.ToString()))
            {
                using (var reader = XmlReader.Create(sr))
                {
                    Load(reader);
                }
            }
        }

        /// <summary>
        /// Загрузить цветовую таблицу из xml
        /// </summary>
        /// <param name="reader">Источник xml</param>
        public void Load(XmlReader reader)
        {
            int i = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "item")
                {
                    this.colors[i++] = Color.FromArgb(int.Parse(reader.ReadString()));
                    if (i >= this.colors.Length)
                        break;
                }
            }
            reader.Close();
        }
    }
}
