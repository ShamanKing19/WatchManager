using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WatchManager.Components
{
    public partial class InputFieldWithTitle : UserControl
    {

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register
            (
                "Title",
                typeof(string),
                typeof(InputFieldWithTitle),
                new PropertyMetadata(string.Empty)
            );
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register
            (
                "Text",
                typeof(string),
                typeof(InputFieldWithTitle),
                new PropertyMetadata(string.Empty)
            );

        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register
            (
                "TitleBackground",
                typeof(object),
                typeof(InputFieldWithTitle),
                new PropertyMetadata()
            );

        public static readonly DependencyProperty InputBackgroundProperty =
            DependencyProperty.Register
            (
                "InputBackground",
                typeof(object),
                typeof(InputFieldWithTitle),
                new PropertyMetadata()
            );
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register
            (
                "CornerRadius",
                typeof(int),
                typeof(InputFieldWithTitle),
                new PropertyMetadata()
            );




        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public object TitleBackground
        {
            get { return (object)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }

        public object InputBackground
        {
            get { return (object)GetValue(InputBackgroundProperty); }
            set { SetValue(InputBackgroundProperty, value); }
        }

        public object CornerRadius
        {
            get { return (object)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public InputFieldWithTitle()
        {
            InitializeComponent();
        }
    }
}
