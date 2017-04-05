using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pharos.Wpf.Controls
{
    public class IconTextBox : TextBox
    {
        static IconTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTextBox), new FrameworkPropertyMetadata(typeof(IconTextBox)));
        }
        public IconTextBox()
            : base()
        {
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(IconTextBox));
        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register("IconHeight", typeof(double), typeof(IconTextBox));
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register("IconWidth", typeof(double), typeof(IconTextBox));
        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(IconTextBox));
        public static readonly DependencyProperty DefualtEnterProperty = DependencyProperty.Register("DefualtEnter", typeof(bool), typeof(IconTextBox), new PropertyMetadata(true));
        public bool DefualtEnter
        {
            get { return (bool)GetValue(DefualtEnterProperty); }

            set { SetValue(DefualtEnterProperty, value); }
        }
        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }

            set { SetValue(IconMarginProperty, value); }
        }

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }

            set { SetValue(IconHeightProperty, value); }
        }
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }

            set { SetValue(IconWidthProperty, value); }
        }

        public ImageSource Icon
        {

            get { return (ImageSource)GetValue(IconProperty); }

            set { SetValue(IconProperty, value); }

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PreviewKeyDown += IconTextBox_PreviewKeyDown;
        }

        public void IconTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (DefualtEnter)
            {
                var ctrl = sender as Control;
                if (e.Key == Key.Enter)
                {
                    ctrl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
            //if (e.Key == Key.Space)
            //{
            //    e.Handled = true;
            //}
        }
    }
}
