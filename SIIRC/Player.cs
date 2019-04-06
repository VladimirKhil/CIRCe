using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace SIIRC
{
    /// <summary>
    /// Игрок в СИ
    /// </summary>
    [Serializable]
    public sealed class Player : INotifyPropertyChanged
    {
        #region Fields

        private string name = string.Empty;
        private int sum = 0;
        private int right = 0;
        private int wrong = 0;

        private bool canPress = true;

        #endregion

        #region Properties

        /// <summary>
        /// Имя игрока
        /// </summary>
        [Description("Имя игрока")]
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        
        /// <summary>
        /// Сумма игрока
        /// </summary>
        [Description("Сумма")]
        public int Sum
        {
            get { return sum; }
            set
            {
                sum = value; if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Sum"));
            }
        }

        /// <summary>
        /// Верных ответов
        /// </summary>
        [Description("Верных ответов")]
        public int Right
        {
            get { return right; }
            set
            {
                right = value; if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Right"));
            }
        }

        /// <summary>
        /// Неверных ответов
        /// </summary>
        [Description("Неверных ответов")]
        public int Wrong
        {
            get { return wrong; }
            set
            {
                wrong = value; if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Wrong"));
            }
        }

        /// <summary>
        /// Может ли игрок нажимать на кнопку
        /// </summary>
        [Description("Может ли сейчас жать на кнопку")]
        public bool CanPress
        {
            get { return canPress; }
            set { canPress = value; }
        }

        #endregion

        public Player() { }

        /// <summary>
        /// Создание игрока
        /// </summary>
        /// <param name="name">Имя игрока</param>
        internal Player(string name)
        {
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            Player another = obj as Player;
            if (another == null)
            {
                return false;
            }
            else
            {
                return this.name == another.name;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} ({2}/{3})", name, sum, right, wrong);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
