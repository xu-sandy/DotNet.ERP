﻿using Pharos.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Pharos.Wpf.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pharos.Wpf.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pharos.Wpf.Controls;assembly=Pharos.Wpf.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class Marquee : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(Marquee), new PropertyMetadata(""));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<INotice>), typeof(Marquee), new PropertyMetadata(new List<INotice>()));
        public string spaceString = "                                                              ";

        static Marquee()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Marquee), new FrameworkPropertyMetadata(typeof(Marquee)));
        }
        public Marquee() 
        {
            this.MouseDown += Marquee_MouseDown;
        }

        void Marquee_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //command
        }
        public string Content
        {
            get { return (string)GetValue(ContentProperty); }

            set
            {
                SetValue(ContentProperty, value);
                Animation();
            }
        }
        
        public IEnumerable<INotice> Items
        {
            get { return (IEnumerable<INotice>)GetValue(ItemsProperty); }

            set
            {
                SetValue(ItemsProperty, value);
                Animation();
            }
        }

        public INotice CurrentNotice { get; set; }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Animation();
        }
        private void Animation()
        {
            var txt = GetTemplateChild("txt") as TextBlock;
            if (txt != null)
            {
                var formattedText = new FormattedText(spaceString + txt.Text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(txt.FontFamily, txt.FontStyle, txt.FontWeight, txt.FontStretch), txt.FontSize, Brushes.Black);
                ThicknessAnimation ta = new ThicknessAnimation();
                ta.To = new Thickness(-formattedText.Width, 0, 0, 0);             //起始值
                ta.From = new Thickness(0, 0, 0, 0);        //结束值
                ta.Duration = TimeSpan.FromSeconds((ta.From.Value.Left - ta.To.Value.Left) / 50);         //动画持续时间
                ta.Completed += ta_Completed;
                txt.BeginAnimation(TextBlock.MarginProperty, ta);//开始动画
            }
        }

        void ta_Completed(object sender, EventArgs e)
        {
            if (CurrentNotice != null)
            {
                var index = CurrentNotice.Index++;
                if (index >= Items.Count())
                {
                    index = 0;
                }
                CurrentNotice = Items.ElementAt(index);
                //CurrentNotice
                Content = CurrentNotice.Content;
                Animation();
            }
        }

    }
}