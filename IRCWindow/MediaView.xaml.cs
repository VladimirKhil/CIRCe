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
using System.ComponentModel;

namespace IRCWindow
{
    /// <summary>
    /// Логика взаимодействия для MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl
    {
        private static DependencyPropertyDescriptor SourceDescriptor = DependencyPropertyDescriptor.FromProperty(Image.SourceProperty, typeof(Image));

        public MediaView()
        {
            InitializeComponent();

            SourceDescriptor.AddValueChanged(this.image, image_SourceUpdated);
        }

        private void image_SourceUpdated(object sender, EventArgs e)
        {
            if (this.image.Source.Width < this.ActualWidth && this.image.Source.Height < this.ActualHeight)
                image.Stretch = Stretch.None;
            else
                image.ClearValue(Image.StretchProperty);
        }
    }
}
