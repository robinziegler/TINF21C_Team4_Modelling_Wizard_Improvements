﻿#pragma checksum "D:\DHBW\SWE\TINF21C_Team4_Modelling_Wizard_Improvements\ModellingWizard\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "776A77718ED0F8E13B152D54E051CB02F8E77F0271D7189C153A816956E79415"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModellingWizard
{
    partial class MainWindow : 
        global::Microsoft.UI.Xaml.Window, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainWindow.xaml line 12
                {
                    this.Grid_Main = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // MainWindow.xaml line 20
                {
                    this.AppTitleBar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 4: // MainWindow.xaml line 25
                {
                    this.Grid_Header = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 5: // MainWindow.xaml line 62
                {
                    this.MenuBar_Main = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuBar>(target);
                }
                break;
            case 6: // MainWindow.xaml line 90
                {
                    this.NavigationView = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationView>(target);
                    ((global::Microsoft.UI.Xaml.Controls.NavigationView)this.NavigationView).SelectionChanged += this.NavigationView_SelectionChanged;
                }
                break;
            case 7: // MainWindow.xaml line 92
                {
                    this.MainPage_Navigation_GenericData = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 8: // MainWindow.xaml line 96
                {
                    this.ContentFrame = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Frame>(target);
                }
                break;
            case 9: // MainWindow.xaml line 84
                {
                    this.Help_About = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.Help_About).Click += this.Help_About_Click;
                }
                break;
            case 10: // MainWindow.xaml line 85
                {
                    this.Help_Manual = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.Help_Manual).Click += this.Help_Manual_Click;
                }
                break;
            case 11: // MainWindow.xaml line 80
                {
                    this.AppMode = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.AppMode).Click += this.AppMode_Click;
                }
                break;
            case 12: // MainWindow.xaml line 70
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Click;
                }
                break;
            case 13: // MainWindow.xaml line 71
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA_Click;
                }
                break;
            case 14: // MainWindow.xaml line 72
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_Click;
                }
                break;
            case 15: // MainWindow.xaml line 73
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA_Click;
                }
                break;
            case 16: // MainWindow.xaml line 74
                {
                    this.ChangeLibary_ElectricConnectorLibrary_v1_0_0 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_ElectricConnectorLibrary_v1_0_0).Click += this.ChangeLibary_ElectricConnectorLibrary_v1_0_0_Click;
                }
                break;
            case 17: // MainWindow.xaml line 75
                {
                    this.ChangeLibary_IndustrialSensorLibrary_v1_0_0 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_IndustrialSensorLibrary_v1_0_0).Click += this.ChangeLibary_IndustrialSensorLibrary_v1_0_0_Click;
                }
                break;
            case 18: // MainWindow.xaml line 76
                {
                    this.ChangeLibary_AddLibary = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AddLibary).Click += this.ChangeLibary_AddLibary_Click;
                }
                break;
            case 19: // MainWindow.xaml line 64
                {
                    this.File_New = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.File_New).Click += this.File_New_Click;
                }
                break;
            case 20: // MainWindow.xaml line 65
                {
                    this.File_Open = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.File_Open).Click += this.File_Open_Click;
                }
                break;
            case 21: // MainWindow.xaml line 66
                {
                    this.File_Save = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.File_Save).Click += this.File_Save_Click;
                }
                break;
            case 22: // MainWindow.xaml line 48
                {
                    this.ThemeButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.ThemeButton).Click += this.ThemeButton_Click;
                }
                break;
            case 23: // MainWindow.xaml line 37
                {
                    this.TextBlock_AppTitle = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 24: // MainWindow.xaml line 39
                {
                    this.TextBlock_AppVersion = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 25: // MainWindow.xaml line 40
                {
                    this.TextBlock_InstallationDate = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

