using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pharos.Wpf.Controls
{
    /// <summary>
    /// Carousel.xaml 的交互逻辑
    /// </summary>
    public partial class Carousel : UserControl, INotifyPropertyChanged
    {
        public Carousel()
        {
            InitializeComponent();
            this.FindCommonVisualAncestor(this);
            this.DataContext = this;
        }
        IEnumerable<ImageSource> _Images = new ImageSource[0];
        public IEnumerable<ImageSource> Images
        {
            get
            {
                return _Images;
            }
            set
            {
                _Images = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Images"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ImagesCount"));
                }
            }
        }
        public int ImagesCount
        {
            get { return Images.Count(); }
        }

        int _CurrentImageIndex = 0;
        public int CurrentImageIndex
        {
            get { return _CurrentImageIndex; }
            set
            {
                _CurrentImageIndex = value;

                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentImageIndex"));
            }
        }
        public void Timer(int interval)
        {

            if (Images != null)
            {
                List<ImageSource> images;
                lock (Images)
                {
                    images = Images.ToList();
                }
            SHOWIMAGE:
                if (CurrentImageIndex < images.Count)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        var img = images[CurrentImageIndex];
                        DoubleAnimation daV = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
                        imgControl.BeginAnimation(UIElement.OpacityProperty, daV);
                        imgControl.Source = img;
                        daV = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
                        imgControl.BeginAnimation(UIElement.OpacityProperty, daV);
                        CurrentImageIndex++;
                    }));
                }
                else if (CurrentImageIndex >= images.Count && images.Count > 1)
                {
                    CurrentImageIndex = 0;
                    goto SHOWIMAGE;
                }
            }
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(interval);
                Timer(interval);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
