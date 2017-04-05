﻿using Pharos.Wpf.ViewModelHelpers;
using System.Windows;

namespace Pharos.Wpf.Extensions
{
    /// <summary>
    /// 窗台静态扩展
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// 绑定窗体数据
        /// </summary>
        /// <param name="_this">当前窗体</param>
        /// <param name="source">ViewModel对象</param>
        public static void ApplyBindings(this Window _this, FrameworkElement bindingElement, BaseViewModel source)
        {
            source.ViewModelSettings(_this, bindingElement);
        }

      
    }
}
