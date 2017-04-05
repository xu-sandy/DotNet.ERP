using Pharos.Barcode.Retailing.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pharos.Frame.Wpf.Extensions;
using Pharos.Barcode.Retailing.Dtos;
using Pharos.Barcode.Retailing.Extensions;
using Pharos.Frame.Wpf.Components;
using System;

namespace Pharos.Barcode.Retailing.ChildTabContentControls
{
    /// <summary>
    /// BarcodePrintContentControl.xaml 的交互逻辑
    /// </summary>
    public partial class BarcodePrintContentControl : UserControl
    {
        BarcodePrintModel model = null;
        public BarcodePrintContentControl()
        {
            InitializeComponent();
            this.Loaded += BarcodePrintContentControl_Loaded;
        }

        void BarcodePrintContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            model = new BarcodePrintModel();
            win.ApplyBindings(this, model);
        }

        private void PagingDataGrid_PagingChanged(object sender, PagingChangedEventArgs args)
        {
            if (args != null)
            {
                model.SearchCommand.Execute(new ProductRequestDto()
                    {
                        Categories = model.CategoryTreeModel.GetEfficientItem().Select(p => p.CategorySN).ToList(),
                        KeyWord = model.KeyWord,
                        ProductBrand = model.ProductBrand,
                        Store = model.Store,
                        PageIndex = args.PageIndex,
                        PageSize = args.PageSize
                    });
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Products != null && model.Products.Count() > 0)
            {
                foreach (var item in model.Products)
                {
                    var ctr = sender as CheckBox;
                    item.IsChecked = ctr.IsChecked ?? false;
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PageDataGrid.RefreshPager();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            var ctr = (sender as CheckBox);
            if (ctr != null && ctr.DataContext is ProductDto)
            {
                var product = ctr.DataContext as ProductDto;
                product.IsChecked = ctr.IsChecked ?? false;
            }

        }
    }
}
