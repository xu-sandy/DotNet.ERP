using Pharos.Barcode.Retailing.Dtos;
using Pharos.Barcode.Retailing.Helper;
using Pharos.Frame.Wpf.ViewModels;
using Pharos.Barcode.Retailing.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Pharos.Barcode.Retailing.Models.BarCodePriters.ZMIN;
using Pharos.Barcode.Retailing.ChildPages;
using Pharos.Utility;

namespace Pharos.Barcode.Retailing.Models
{
    public class BarcodePrintModel : BaseViewModel
    {
        private string getProductCategoryUrl = "Api/BarcodePrint/GetProductCategory";
        private string getSupplierUrl = "Api/BarcodePrint/GetStores";
        private string getProductBrand = "Api/BarcodePrint/GetProductBrand";
        private string getProducts = "Api/BarcodePrint/GetProducts";
        List<TreeViewItemForMenuModel> categoryTreeModel;
        public BarcodePrintModel()
        {
            PageIndex = 1;
            PageSize = 50;
            if (string.IsNullOrEmpty(SystemConfiguration.Current.ServerUrl))
            {
                MessageBox.Show("未能获取服务器位置。");
                return;
            }
            categoryTreeModel = new List<TreeViewItemForMenuModel>()
            {
               new TreeViewItemForMenuModel()
                {
                    CategoryPSN = -1,
                    CategorySN = -1,
                    Content = "全部",
                    EnableShowCheckBox = System.Windows.Visibility.Visible
                }
            };
            new HttpRequestHelper().GetRequst<List<ProductCategoryDto>>(SystemConfiguration.Current.ServerUrl + getProductCategoryUrl, null, null, new Action<IEnumerable<ProductCategoryDto>>((o) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var tree = new TreeViewItemForMenuModel()
                         {
                             CategoryPSN = -1,
                             CategorySN = -1,
                             Content = "全部",
                             Grade = -1,
                             EnableShowCheckBox = System.Windows.Visibility.Visible
                         };
                    tree.Children.Add(new TreeViewItemForMenuModel()
                         {
                             CategoryPSN = -10,
                             CategorySN = -10,
                             Content = "捆绑商品",
                             Grade = -10,
                             EnableShowCheckBox = System.Windows.Visibility.Visible
                         });
                    tree.InitChild(o);
                    CategoryTreeModel = new List<TreeViewItemForMenuModel>() { tree };
                }));
            }));
            new HttpRequestHelper().GetRequst<List<StoreDto>>(SystemConfiguration.Current.ServerUrl + getSupplierUrl, null, null, new Action<IEnumerable<StoreDto>>((o) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    List<StoreDto> result = new List<StoreDto>()
                    {
                        new StoreDto()
                        {
                         StoreId ="-1",
                         Title = "全部"
                        }
                    };
                    Store = "-1";
                    if (o != null && o.Count() > 0)
                        result.AddRange(o);
                    StoresSelectItem = result;
                    this.OnPropertyChanged(p => p.StoresSelectItem);
                    this.OnPropertyChanged(p => p.Store);
                }));
            }));

            new HttpRequestHelper().GetRequst<List<ProductBrandDto>>(SystemConfiguration.Current.ServerUrl + getProductBrand, null, null, new Action<IEnumerable<ProductBrandDto>>((o) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    List<ProductBrandDto> result = new List<ProductBrandDto>()
                    {
                        new ProductBrandDto()
                        {
                          BrandSN=-1,
                          Title = "全部"
                        }
                    };
                    ProductBrand = -1;
                    if (o != null && o.Count() > 0)
                        result.AddRange(o);
                    ProductBrandSelectItem = result;
                    this.OnPropertyChanged(p => p.ProductBrandSelectItem);
                    this.OnPropertyChanged(p => p.ProductBrand);
                }));
            }));
        }
        public int CheckedCount
        {
            get { return Products.Where(o => o.IsChecked ?? false).Count(); }
        }

        public uint RepeatNum { get; set; }
        public List<TreeViewItemForMenuModel> CategoryTreeModel
        {
            get
            {
                return categoryTreeModel;
            }
            set
            {
                categoryTreeModel = value;
                this.OnPropertyChanged(o => o.CategoryTreeModel);
            }
        }
        public string KeyWord { get; set; }

        public IEnumerable<StoreDto> StoresSelectItem { get; set; }
        public IEnumerable<ProductBrandDto> ProductBrandSelectItem { get; set; }
        public IEnumerable<ProductDto> Products { get; private set; }
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string Store { get; set; }
        public int ProductBrand { get; set; }

        public GeneralCommand<object> PrintCommand
        {
            get
            {
                return new GeneralCommand<object>((p, cmd) =>
                {
                    SetRepeatNum page = new SetRepeatNum(this);
                    page.ShowDialog();
                });
            }
        }

        public GeneralCommand<object> ExportCommand
        {
            get
            {
                return new GeneralCommand<object>((p, cmd) =>
                {

                    if (Products == null && Products.Count() == 0)
                    {
                        MessageBox.Show("没有数据不能导出！");
                        return;
                    }
                    var ranges = Products.Where(o => o.IsChecked ?? false);
                    if (ranges.Count() == 0)
                    {
                        MessageBox.Show("没有数据不能导出！");
                        return;
                    }
                    try
                    {
                        System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                        var dialogReturn = dialog.ShowDialog();
                        var savePath = dialog.SelectedPath.Trim();
                        if (dialogReturn == System.Windows.Forms.DialogResult.OK)
                        {
                            ExportExcelForCS.ToExcel(new ProductDto(), "条码数据导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", savePath, ranges);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("导出失败！");
                    }
                });
            }
        }
        public GeneralCommand<object> OKPrintCommand
        {
            get
            {
                return new GeneralCommand<object>((p, cmd) =>
                {
                    if (Products == null && Products.Count() == 0)
                    {
                        MessageBox.Show("没有数据不能打印！");
                        return;
                    }
                    var ranges = Products.Where(o => o.IsChecked ?? false && !o.IsWeigh);
                    if (ranges.Count() == 0)
                    {
                        MessageBox.Show("没有数据不能打印！");
                        return;
                    }
                    this.CurrentWindow.Close();
                    foreach (var item in ranges)
                    {
                        BarCodePrinter.Current.PrintBarCode(item, 1, RepeatNum);
                    }
                });
            }
        }


        public GeneralCommand<ProductRequestDto> SearchCommand
        {
            get
            {
                return new GeneralCommand<ProductRequestDto>((o, cmd) =>
                {
                    ProductRequestDto theParams = null;
                    if (o == null)
                    {
                        theParams = new ProductRequestDto()
                        {
                            Categories = CategoryTreeModel.GetEfficientItem().Select(p => p.CategorySN).ToList(),
                            KeyWord = KeyWord,
                            ProductBrand = ProductBrand,
                            Store = Store,
                            PageIndex = 1,
                            PageSize = 50
                        };
                    }
                    else
                    {
                        theParams = o;
                    }
                    new HttpRequestHelper().PostRequst<ProductRequestDto, DataGridPagingResult<List<ProductDto>>>(SystemConfiguration.Current.ServerUrl + getProducts, theParams, null, new Action<DataGridPagingResult<List<ProductDto>>>((p) =>
                     {
                         Application.Current.Dispatcher.Invoke(new Action(() =>
                         {
                             Products = p.Result;
                             Total = p.Total;
                             PageIndex = theParams.PageIndex;
                             PageSize = theParams.PageSize;
                             this.OnPropertyChanged(j => j.Total);
                             this.OnPropertyChanged(j => j.PageIndex);
                             this.OnPropertyChanged(j => j.PageSize);
                             this.OnPropertyChanged(j => j.Products);
                         }));
                     }));
                });
            }
        }

    }

    public partial class TreeViewItemForMenuModel
    {
        public int CategorySN { get; set; }
        public int CategoryPSN { get; set; }
        public short Grade { get; set; }

        public void InitChild(IEnumerable<ProductCategoryDto> childInfos, int psn = 0, int grade = 1)
        {
            var result = childInfos.Where(o => o.CategoryPSN == psn && o.Grade == grade);
            foreach (var item in result)
            {
                var child = new TreeViewItemForMenuModel()
                {
                    CategoryPSN = item.CategoryPSN,
                    CategorySN = item.CategorySN,
                    Content = "[" + item.CategorySN.ToString("00") + "] " + item.Title,
                    Grade = item.Grade,
                    EnableShowCheckBox = System.Windows.Visibility.Visible
                };
                child.InitChild(childInfos, item.CategorySN, grade + 1);
                Children.Add(child);
            }
            this.SetChildrenChanged();
        }

    }


}
