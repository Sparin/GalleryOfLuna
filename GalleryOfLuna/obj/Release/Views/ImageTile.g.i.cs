﻿#pragma checksum "..\..\..\Views\ImageTile.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FCB7A1BDF4E092D354D25EF460814BA7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using GalleryOfLuna.Controls;
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


namespace GalleryOfLuna.Views {
    
    
    /// <summary>
    /// ImageTile
    /// </summary>
    public partial class ImageTile : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\Views\ImageTile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock gifMark;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Views\ImageTile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GalleryOfLuna.Controls.AnimatedImage Image;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Views\ImageTile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel extMenu;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Views\ImageTile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
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
            System.Uri resourceLocater = new System.Uri("/GalleryOfLuna;component/views/imagetile.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ImageTile.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 12 "..\..\..\Views\ImageTile.xaml"
            ((GalleryOfLuna.Views.ImageTile)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.extMenu_MouseEnter);
            
            #line default
            #line hidden
            
            #line 13 "..\..\..\Views\ImageTile.xaml"
            ((GalleryOfLuna.Views.ImageTile)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.extMenu_MouseLeave);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Views\ImageTile.xaml"
            ((GalleryOfLuna.Views.ImageTile)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ImageTile_Loaded);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\Views\ImageTile.xaml"
            ((GalleryOfLuna.Views.ImageTile)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.ImageTile_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gifMark = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.Image = ((GalleryOfLuna.Controls.AnimatedImage)(target));
            return;
            case 4:
            this.extMenu = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
