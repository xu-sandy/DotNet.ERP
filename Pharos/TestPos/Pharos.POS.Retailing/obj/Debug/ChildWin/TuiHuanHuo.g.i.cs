﻿#pragma checksum "..\..\..\ChildWin\TuiHuanHuo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6360D7A25AAF8123EABD63A85E1D5F8"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Pharos.Wpf.Controls;
using Pharos.Wpf.XamlConverters;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Pharos.POS.Retailing.ChildWin {
    
    
    /// <summary>
    /// TuiHuanHuo
    /// </summary>
    public partial class TuiHuanHuo : Pharos.Wpf.Controls.DialogWindow02, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\ChildWin\TuiHuanHuo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tabControl;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\ChildWin\TuiHuanHuo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Pharos.Wpf.Controls.IconTextBox txtChangeBarcode;
        
        #line default
        #line hidden
        
        
        #line 262 "..\..\..\ChildWin\TuiHuanHuo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Pharos.Wpf.Controls.IconTextBox txtRefundBarcode;
        
        #line default
        #line hidden
        
        
        #line 329 "..\..\..\ChildWin\TuiHuanHuo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Pharos.Wpf.Controls.IconTextBox txtRefundAllBarcode;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Pharos.POS.Retailing;component/childwin/tuihuanhuo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tabControl = ((System.Windows.Controls.TabControl)(target));
            return;
            case 2:
            
            #line 16 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            ((System.Windows.Controls.TabItem)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.change_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 132 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.txtMode_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtChangeBarcode = ((Pharos.Wpf.Controls.IconTextBox)(target));
            
            #line 133 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            this.txtChangeBarcode.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.IconTextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 148 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            ((System.Windows.Controls.TabItem)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.refund_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtRefundBarcode = ((Pharos.Wpf.Controls.IconTextBox)(target));
            
            #line 262 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            this.txtRefundBarcode.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.IconTextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 276 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            ((System.Windows.Controls.TabItem)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.refunAll_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtRefundAllBarcode = ((Pharos.Wpf.Controls.IconTextBox)(target));
            
            #line 329 "..\..\..\ChildWin\TuiHuanHuo.xaml"
            this.txtRefundAllBarcode.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.IconTextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
