﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Xml.Serialization;

namespace Pharos.Wpf.ViewModelHelpers
{
    public interface IBaseViewModel : INotifyPropertyChanged
    {
        void OnPropertyChanged(string propertyName);
    }

    public delegate void BindingViewModelHandler(BaseViewModel model);
    public abstract class BaseViewModel : IBaseViewModel
    {
        /// <summary>
        /// 设置ViewModel的当前窗体,并绑定数据
        /// </summary>
        /// <param name="win"></param>
        public virtual void ViewModelSettings(Window win, FrameworkElement bindingElement)
        {
            CurrentWindow = win;
            CurrentBindingElement = bindingElement;
            CurrentBindingElement.DataContext = this;
            if (ViewModelBinded != null)
                ViewModelBinded(this);
        }
        public event BindingViewModelHandler ViewModelBinded;

        [XmlIgnore]
        public FrameworkElement CurrentBindingElement { get; set; }

        [XmlIgnore]
        public Window CurrentWindow { get; set; }

        #region 数据更新监视
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 通知属性更新
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion 数据更新监视

    }

    public static class BaseViewModelExtensions
    {
        #region 数据更新监视 For Linq
        public static void OnPropertyChanged<T, TProperty>(this T propertyChangedBase, Expression<Func<T, TProperty>> expression) where T : IBaseViewModel
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                propertyChangedBase.OnPropertyChanged(propertyName);
            }
            else
                throw new NotImplementedException();
        }

        public static string GetPropertyName<T, TProperty>(this T propertyChangedBase, Expression<Func<T, TProperty>> expression) where T : IBaseViewModel
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                return propertyName;
            }
            else
                throw new NotImplementedException();
        }
        #endregion 数据更新监视 For Linq
    }
}
