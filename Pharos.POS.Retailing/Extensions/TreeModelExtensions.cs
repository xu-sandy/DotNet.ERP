using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.POS.Retailing.Extensions
{
    public static class TreeModelExtensions
    {
        public static IEnumerable<TreeModel> InitCategory(this IEnumerable<TreeModel> _this, TreeModel treemodel)
        {
            if (treemodel != null)
            {
                var tree = new List<TreeModel>();
                tree.Add(treemodel);
                var node = tree.FirstOrDefault();
                if (node != null)
                {
                    node.IsSelected = true;
                }
                return tree;
            }
            else
            {
                _this = new List<TreeModel>() { new TreeModel("全部", 0) { IsSelected = true } };
            }
            return _this;
        }

        public static int GetSelectItemSN(this IEnumerable<TreeModel> root)
        {
            var rootNode = root.FirstOrDefault();
            if (root == null)
            {
                return 0;
            }
            var selectItem = rootNode.FindSelectItem(rootNode);
            if (selectItem == null)
            {
                return Convert.ToInt32(rootNode.Id);
            }
            return Convert.ToInt32(selectItem.Id);
        }
    }
}
