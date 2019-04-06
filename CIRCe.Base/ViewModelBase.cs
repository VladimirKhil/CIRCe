using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CIRCe.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo, ICloneable
    {
        protected Dictionary<string, string> errors = new Dictionary<string, string>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void OnPropertiesChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (var item in names)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(item));
                }
            }
        }

        public string Error
        {
            get 
            {
                foreach (var item in this.errors)
                {
                    if (item.Value != null)
                        return item.Value;
                }

                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error;
                if (this.errors.TryGetValue(columnName, out error))
                    return error;

                return null;
            }
        }

        public abstract object Clone();
    }
}
