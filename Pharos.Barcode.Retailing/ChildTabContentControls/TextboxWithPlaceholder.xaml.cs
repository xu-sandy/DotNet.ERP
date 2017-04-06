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

namespace Pharos.Barcode.Retailing.ChildTabContentControls
{
    /// <summary>
    /// TextboxWithPlaceholder.xaml 的交互逻辑
    /// </summary>
    public partial class TextboxWithPlaceholder : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(TextboxWithPlaceholder), new PropertyMetadata(""));
        public TextboxWithPlaceholder()
        {
            InitializeComponent();
            Placeholder = "提示";
        }
        public string Placeholder { get; set; }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Text = (sender as TextBox).Text;
        }
    }
}
