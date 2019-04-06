using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IRCWindow.View;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IRCWindow.ViewModel.Common
{
    public sealed class EditableCollectionViewModel<T> : System.Windows.Interop.IWin32Window, IDisposable, INotifyPropertyChanged
        where T : ICloneable, IDataErrorInfo, INotifyPropertyChanged, new()
    {
        private ObservableCollection<T> list;
        private IntPtr handle;
        private string newTitle;
        private string editTitle;

        private SimpleCommand add;

        public ICommand Add { get { return this.add; } }
        public ICommand Delete { get; private set; }

        public ObservableCollection<T> List
        {
            get { return this.list; }
        }

        private bool error;

        public bool Error
        {
            get { return error; }
            set { if (error != value) { error = value; OnPropertyChanged("Error"); } }
        }

        public bool CanAdd
        {
            get { return this.add.CanBeExecuted; }
            set { this.add.CanBeExecuted = value; }
        }

        private Action<T> initializer;

        public EditableCollectionViewModel(IList<T> list, IntPtr handle, string newTitle, string editTitle, Action<T> initializer = null)
        {
            this.list = new ObservableCollection<T>(list.Select(item => (T)item.Clone()));
            this.handle = handle;
            this.newTitle = newTitle;
            this.editTitle = editTitle;

            this.initializer = initializer;

            this.add = new SimpleCommand(Add_Executed);
            this.Delete = new SimpleCommand(Delete_Executed);

            foreach (var item in this.list)
            {
                item.PropertyChanged += item_PropertyChanged;
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Error")
            {
                if (((T)sender).Error != null)
                    this.Error = true;
                else
                    this.Error = this.list.Any(item => item.Error != null);
            }
        }

        private void Add_Executed(object arg)
        {
            var item = new T();
            if (this.initializer != null)
                this.initializer(item);

            var diag = new ItemEditorView { DataContext = item };

            var helper = new System.Windows.Interop.WindowInteropHelper(diag);
            helper.Owner = this.handle;

            diag.Title = this.newTitle;

            if (diag.ShowDialog() == true)
            {
                this.list.Add(item);
                item.PropertyChanged += item_PropertyChanged;
            }
        }

        private void Delete_Executed(object arg)
        {
            var item = (T)arg;
            this.list.Remove(item);
            item.PropertyChanged -= item_PropertyChanged;

            if (this.error)
                this.Error = this.list.Any(x => x.Error != null);
        }

        public IntPtr Handle
        {
            get { return this.handle; }
        }

        public void Dispose()
        {
            foreach (var item in this.list)
            {
                item.PropertyChanged -= item_PropertyChanged;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
