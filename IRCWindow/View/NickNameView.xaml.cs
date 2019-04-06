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
    /// Логика взаимодействия для NickNameView.xaml
    /// </summary>
    public partial class NickNameView : UserControl
    {
        public NickNameView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.name);
        }
    }
}
