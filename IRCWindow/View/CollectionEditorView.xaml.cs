using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRCWindow.View
{
    /// <summary>
    /// Логика взаимодействия для CollectionEditorView.xaml
    /// </summary>
    public partial class CollectionEditorView : Window
    {
        public CollectionEditorView()
        {
            InitializeComponent();
            this.DataContextChanged += CollectionEditorView_DataContextChanged;
        }

        void CollectionEditorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var helper = new System.Windows.Interop.WindowInteropHelper(this);
            helper.Owner = ((System.Windows.Interop.IWin32Window)this.DataContext).Handle;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
