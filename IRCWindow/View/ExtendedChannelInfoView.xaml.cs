﻿using System;
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
    /// Логика взаимодействия для ExtendedChannelInfoView.xaml
    /// </summary>
    public partial class ExtendedChannelInfoView : UserControl
    {
        public ExtendedChannelInfoView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.name);
            this.name.CaretIndex = this.name.Text.Length;
        }
    }
}
