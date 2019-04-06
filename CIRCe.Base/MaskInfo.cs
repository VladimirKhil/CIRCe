using System;
using System.Collections.Generic;
using System.Text;
//using ObjectEditors;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// Информация о маске
    /// </summary>
    public sealed class MaskInfo: ViewModelBase, IEquatable<MaskInfo>
    {
        private string mask = string.Empty;
        private string whoSet = string.Empty;
        private DateTime when = DateTime.Now;

        /// <summary>
        /// Маска бана
        /// </summary>
        //[Editable("Mask", typeof(Resources), EditableAttribute.BaseTypes.SingleString)]
        public string Mask { get { return mask; } 
            set 
            {
                if (this.mask != value)
                {
                    mask = value;

                    if (string.IsNullOrEmpty(value))
                        this.errors["Mask"] = "Маска не может быть пустой";
                    else
                        this.errors["Mask"] = null;

                    OnPropertiesChanged("Mask", "Header", "Error");
                }
            }
        }

        public string Header { get { return this.mask; } }

        /// <summary>
        /// Кем назначен
        /// </summary>
        public string WhoSet { get { return whoSet; } set { whoSet = value; } }
        /// <summary>
        /// Когда назначен
        /// </summary>
        public DateTime When { get { return when; } set { when = value; } }

        public MaskInfo() { this.errors["Mask"] = "Маска не может быть пустой"; }

        /// <summary>
        /// Создание информации о бане
        /// </summary>
        /// <param name="mask">Маска</param>
        /// <param name="whoSet">Кем назначен</param>
        /// <param name="when">Когда назначен</param>
        public MaskInfo(string mask, string whoSet, DateTime when)
        {
            this.mask = mask;
            this.whoSet = whoSet;
            this.when = when;
        }

        public override string ToString()
        {
            return this.mask + string.Format(" (установлена {0} {1})", this.whoSet, this.when);
        }

        public override object Clone()
        {
            return new MaskInfo(this.mask, this.whoSet, this.when);
        }

        public override bool Equals(object obj)
        {
            var mask = obj as MaskInfo;
            if (mask == null)
                return base.Equals(obj);

            return this.Equals(mask);
        }

        public override int GetHashCode()
        {
            return this.mask != null ? this.mask.GetHashCode() : 0;
        }

        public bool Equals(MaskInfo other)
        {
            return this.mask == other.mask;
        }
    }
}
