using Newtonsoft.Json;
using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.CRM.Retailing.Models
{
    public class DataDictionaryModel
    {
        #region Constructor

        public DataDictionaryModel()
            : this(new SysDataDictionary() { Status = true })
        { }

        public DataDictionaryModel(SysDataDictionary dict)
        {
            CurrentDict = dict;
        }
        #endregion Constructor

        #region Property

        internal SysDataDictionary CurrentDict { get; set; }

        public int Id
        {
            get { return CurrentDict.Id; }
            set { CurrentDict.Id = value; }
        }

        public int DicSN
        {
            get { return CurrentDict.DicSN; }
            set { CurrentDict.DicSN = value; }
        }

        public int DicPSN
        {
            get { return CurrentDict.DicPSN; }
            set { CurrentDict.DicPSN = value; }
        }

        public string DicParentTitle
        {
            get
            {
                var parent = SysDataDictService.Find(o => o.DicSN == DicPSN);
                if (parent != null)
                {
                    return parent.Title;
                }
                return string.Empty;
            }
        }

        public int SortOrder
        {
            get { return CurrentDict.SortOrder; }
            set { CurrentDict.SortOrder = value; }
        }

        public string Title
        {
            get { return CurrentDict.Title; }
            set { CurrentDict.Title = value; }
        }

        public short Depth
        {
            get { return CurrentDict.Depth; }
            set { CurrentDict.Depth = value; }
        }

        public bool Status
        {
            get { return CurrentDict.Status; }
            set { CurrentDict.Status = value; }
        }


        [JsonProperty("children")]
        public IEnumerable<DataDictionaryModel> Children
        {
            get
            {
                return SysDataDictService.GetChildList(DicSN).Select(o => new DataDictionaryModel(o));
            }
        }

        public IEnumerable<DataDictionaryModel> Items
        {
            get
            {
                return SysDataDictService.GetDictionaryItems(DicSN).Select(o => new DataDictionaryModel(o));
            }
        }

        #endregion Property

        #region Method

        public OpResult SaveChange()
        {
            OpResult re = new OpResult() { Successed = true };
            if (Id == 0)
            {
                re = SysDataDictService.AddDict(CurrentDict);
            }
            else
            {
                re = SysDataDictService.Update(CurrentDict);
            }
            return re;
        }

        #endregion Method

    }

    public class ChildDictionaryModel : DataDictionaryModel
    {
        public ChildDictionaryModel(SysDataDictionary entity)
            : base(entity)
        { }

        [JsonIgnore]
        public new IEnumerable<DataDictionaryModel> Items
        {
            get
            {
                return SysDataDictService.GetDictionaryItems(DicSN).Select(o => new DataDictionaryModel(o));
            }
        }

        [JsonProperty("children")]
        public new IEnumerable<ChildDictionaryModel> Children
        {
            get
            {
                var dict = (DicType)Enum.Parse(typeof(DicType), DicSN.ToString());
                return SysDataDictService.GetDictionaryList(dict).Select(o => new ChildDictionaryModel(o));
            }
        }
    }
}