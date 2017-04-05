using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Barcode.Retailing.Models;

namespace Pharos.Barcode.Retailing.Extensions
{
    public static class TreeViewForMenuExtensions
    {
        public static IEnumerable<TreeViewItemForMenuModel> GetEfficientItem(this IEnumerable<TreeViewItemForMenuModel> collections)
        {
            List<TreeViewItemForMenuModel> ChildrenCheckedItems = new List<TreeViewItemForMenuModel>();
            foreach (var item in collections)
            {
                if (item.IsChecked ?? false)
                    ChildrenCheckedItems.Add(item);
                ChildrenCheckedItems.AddRange(item.GetCheckedChildrenItems());
            }
            if (ChildrenCheckedItems.Count > 0)
            {
                return ChildrenCheckedItems;
            }

            var selectedItems = collections.Where(o => o.IsSelected == true).ToList();
            List<TreeViewItemForMenuModel> NextChildrenSelectedItems = new List<TreeViewItemForMenuModel>();
            foreach (var item in collections)
            {
                NextChildrenSelectedItems.AddRange(item.GetSelectedChildrenItems());
            }
            selectedItems.AddRange(NextChildrenSelectedItems);
            if (selectedItems.Count == 0)
            {
                return collections.Where(o => o.CategorySN == -1);
            }
            return selectedItems;
        }
    }
}
