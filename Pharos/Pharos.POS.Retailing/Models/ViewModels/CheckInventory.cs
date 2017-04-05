﻿using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.Wpf.ViewModelHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class CheckInventory : BaseViewModel
    {
        public CheckInventory()
        {
            categoryTree = Global.CategoryTree;
            this.OnPropertyChanged(o => o.CategoryTree);

            Global.CategoryTreeRefreshEvent += Global_CategoryTreeRefreshEvent;
        }

        void Global_CategoryTreeRefreshEvent(object obj)
        {
            categoryTree = Global.CategoryTree;
            this.OnPropertyChanged(o => o.CategoryTree);
        }
        string keyword;
        public string Keyword
        {
            get
            {
                return keyword;
            }
            set
            {
                keyword = value;
                this.OnPropertyChanged(o => o.Keyword);
            }
        }
        decimal price;
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {

                price = value;
                this.OnPropertyChanged(o => o.Price);
            }
        }


        IEnumerable<InventoryItem> inventoryItems;
        public IEnumerable<InventoryItem> InventoryItems
        {
            get
            {
                return inventoryItems;
            }
            set
            {
                inventoryItems = value;
                this.OnPropertyChanged(o => o.InventoryItems);
            }
        }

        IEnumerable<TreeModel> categoryTree;
        public IEnumerable<TreeModel> CategoryTree
        {
            get
            {
                return categoryTree;
            }
        }


        Thread PageHandler { get; set; }
        public GeneralCommand<object> SearchCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    QueryModel.Current.IsQuery = true;
                    InventoryItems = new List<InventoryItem>();

                    if (PageHandler != null)
                    {
                        PageHandler.Abort();
                        PageHandler = null;
                    }
                    Task.Factory.StartNew(() =>
                    {
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        List<int> _sns = new List<int>();
                        _sns.Add(TreeModelExtensions.GetSelectItemSN(CategoryTree));
                        //查询本店库存
                        GetProductStockParams _params = new GetProductStockParams() { StoreId = _machinesInfo.StoreId, MachineSn = _machinesInfo.MachineSn, CategorySns = _sns, Keyword = keyword, Price = price, CID = _machinesInfo.CompanyId };
                        PageHandler = Thread.CurrentThread;
                        ApiManager.FullPager<GetProductStockParams, InventoryItem>(@"api/GetProductStock", _params, (o) =>
                        {
                            if (o.Code == "200")
                            {
                                if (o.Result.Pager.Index == 1)
                                {
                                    InventoryItems = o.Result.Datas;
                                }
                                else
                                {
                                    InventoryItems = InventoryItems.Concat(o.Result.Datas);
                                }
                            }
                            else
                            {
                                Toast.ShowMessage(o.Message, CurrentWindow);
                            }
                            QueryModel.Current.IsQuery = false;
                        });
                        PageHandler = null;
                    });
                });
            }
        }
    }
}
