﻿#pragma checksum "..\..\..\Views\wndMain.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6533FFAE09871FDF8D29BC7CE7427BF1"
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
using GalleryOfLuna.Views;
using MahApps.Metro.Controls;
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
using System.Windows.Interactivity;
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
    /// wndMain
    /// </summary>
    public partial class wndMain : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 75 "..\..\..\Views\wndMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.Flyout flDownloads;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Views\wndMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtrGrid;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\Views\wndMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.Flyout flSettings;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\Views\wndMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.NumericUpDown numPage;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\Views\wndMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdMain;
        
        #line default
        #line hidden
        
        
        #line 154 "..\..\..\Views\wndMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrWorkspace;
        
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
            System.Uri resourceLocater = new System.Uri("/GalleryOfLuna;component/views/wndmain.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\wndMain.xaml"
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
            
            #line 61 "..\..\..\Views\wndMain.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnHelp_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.flDownloads = ((MahApps.Metro.Controls.Flyout)(target));
            return;
            case 3:
            this.dtrGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            
            #line 87 "..\..\..\Views\wndMain.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.miOpenExplorer_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 88 "..\..\..\Views\wndMain.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.miRepeat_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 89 "..\..\..\Views\wndMain.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.miCancel_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.flSettings = ((MahApps.Metro.Controls.Flyout)(target));
            return;
            case 8:
            this.numPage = ((MahApps.Metro.Controls.NumericUpDown)(target));
            return;
            case 9:
            this.grdMain = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.scrWorkspace = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 11:
            
            #line 195 "..\..\..\Views\wndMain.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnDownload_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
