using Newtonsoft.Json;
using Pharos.Wpf.ViewModelHelpers;
using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class TreeModel : BaseViewModel
    {
        #region Constructors
        public TreeModel(string name, object id)
            : this()
        {
            this.Name = name;
            this.Id = id;
        }
        public TreeModel()
        {
            this.Children = new List<TreeModel>();
        }
        #endregion

        [JsonProperty("Childrens")]
        public List<TreeModel> Children { get; set; }

        public bool HasChild { get { return Children.Count > 0; } }

        [JsonProperty("Title")]
        public string Name { get; set; }

        [JsonProperty("CategorySN")]
        public object Id { get; set; }

        public int CategoryPSN { get; set; }

        bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                this.OnPropertyChanged(o => o.IsSelected);
            }
        }

        public TreeModel FindSelectItem(TreeModel root)
        {
            if (root._isSelected)
            {
                return root;
            }
            foreach (var item in root.Children)
            {
                var result = FindSelectItem(item);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

    }
}
