using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Pharos.Frame.Wpf.ViewModels;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pharos.Barcode.Retailing.Models
{
    public partial class TreeViewItemForMenuModel : BaseViewModel
    {
        public TreeViewItemForMenuModel()
        {

        }
        private Visibility enableShowCheckBox = Visibility.Collapsed;
        public Visibility EnableShowCheckBox
        {
            get
            {
                return enableShowCheckBox;
            }
            set
            {
                enableShowCheckBox = value;
                this.OnPropertyChanged(o => o.EnableShowCheckBox);
            }
        }

        private Visibility enableShowImage = Visibility.Collapsed;
        public Visibility EnableShowImage
        {
            get
            {
                return enableShowImage;
            }
            set
            {
                enableShowImage = value;
                this.OnPropertyChanged(o => o.EnableShowImage);
            }
        }

        private ImageSource imageUrl = new BitmapImage(new Uri("pack://application:,,,/Images/Warning.png"));
        public ImageSource ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                imageUrl = value;
                this.OnPropertyChanged(o => o.ImageUrl);
            }
        }

        private bool? isChecked = false;
        public bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                IsSelected = value;
                Children.ForEach(o => o.IsChecked = value);
                this.OnPropertyChanged(o => o.IsChecked);
            }
        }

        private bool? isSelected = false;
        public bool? IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                this.OnPropertyChanged(o => o.isSelected);
            }
        }

        private Thickness padding;
        public Thickness Padding
        {
            get
            {
                return padding;
            }
            set
            {
                padding = value;
                this.OnPropertyChanged(o => o.Padding);
            }
        }

        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                this.OnPropertyChanged(o => o.Content);
            }
        }
        private List<TreeViewItemForMenuModel> children = new List<TreeViewItemForMenuModel>();
        public List<TreeViewItemForMenuModel> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
                SetChildrenChanged();
                this.OnPropertyChanged(o => o.Children);
            }
        }
        private double height = 40;
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                this.OnPropertyChanged(o => o.Height);
            }
        }

        public bool HasChild { get; private set; }
        protected internal void SetChildrenChanged()
        {
            HasChild = Children.Count > 0;
            this.OnPropertyChanged(o => o.HasChild);
        }

        public IEnumerable<TreeViewItemForMenuModel> GetCheckedChildrenItems()
        {
            List<TreeViewItemForMenuModel> ChildrenSelectItem = new List<TreeViewItemForMenuModel>();
            foreach (var item in Children)
            {
                if (item.IsChecked ?? false)
                    ChildrenSelectItem.Add(item);
                ChildrenSelectItem.AddRange(item.GetCheckedChildrenItems());
            }
            return ChildrenSelectItem;
        }

        public IEnumerable<TreeViewItemForMenuModel> GetSelectedChildrenItems()
        {
            var items = Children.Where(o => o.IsSelected == true).ToList();
            List<TreeViewItemForMenuModel> NextChildrenSelectItem = new List<TreeViewItemForMenuModel>();
            foreach (var item in Children)
            {
                NextChildrenSelectItem.AddRange(item.GetSelectedChildrenItems());
            }
            items.AddRange(NextChildrenSelectItem);
            return items;
        }
    }
}
