using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.CMS.Retailing
{
    /// <summary>
    /// 操作按钮信息
    /// </summary>
    public class OpBtnInfo
    {
        public string AddText { get; set; }
        public string EditText { get; set; }
        public string DelText { get; set; }

        public bool HideDel { get; set; }
        public bool HideAdd { get; set; }
        public bool HideEdit { get; set; }
        public bool HideSearch { get; set; }

        /// <summary>
        /// 操作基本按钮
        /// </summary>
        /// <param name="addText">新增按钮文本</param>
        /// <param name="editText"></param>
        /// <param name="delText"></param>
        /// <param name="hideDel"></param>
        /// <param name="hideAdd"></param>
        /// <param name="hideEdit"></param>
        public OpBtnInfo(string addText="新增",string editText="修改",string delText="删除",bool hideDel=false,bool hideAdd=false,bool hideEdit=false,bool hideSearch=false)
        {
            AddText = addText;
            EditText = editText;
            DelText = delText;
            HideDel = hideDel;
            HideAdd = hideAdd;
            HideEdit = hideEdit;
            HideSearch = hideSearch;
        }
    }
    public static class Extends
    {
        /// <summary>
        /// 设置/获取工具栏基本按钮信息
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static OpBtnInfo OpBtnInfo(this ViewDataDictionary viewData, OpBtnInfo info = null)
        {
            if (info == null) return viewData["opbtnInfo"] as OpBtnInfo;
            viewData["opbtnInfo"] = info;
            return null;
        }
    }
}