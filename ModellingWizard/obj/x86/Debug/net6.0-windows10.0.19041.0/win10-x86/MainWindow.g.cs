﻿#pragma checksum "D:\DHBW\SWE\TINF21C_Team4_Modelling_Wizard_Improvements\ModellingWizard\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5019DE00882DF09D8C62037B800FEF1893B6E7833227A5B332E5BCFE3D867A68"
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
            case 2: // MainWindow.xaml line 48
                {
                    this.NavigationView = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationView>(target);
                    ((global::Microsoft.UI.Xaml.Controls.NavigationView)this.NavigationView).SelectionChanged += this.NavigationView_SelectionChanged;
                }
                break;
            case 3: // MainWindow.xaml line 54
                {
                    this.ContentFrame = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Frame>(target);
                }
                break;
            case 4: // MainWindow.xaml line 42
                {
                    this.Help_About = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.Help_About).Click += this.Help_About_Click;
                }
                break;
            case 5: // MainWindow.xaml line 43
                {
                    this.Help_Manual = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.Help_Manual).Click += this.Help_Manual_Click;
                }
                break;
            case 6: // MainWindow.xaml line 38
                {
                    this.AppMode = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.AppMode).Click += this.AppMode_Click;
                }
                break;
            case 7: // MainWindow.xaml line 28
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Click;
                }
                break;
            case 8: // MainWindow.xaml line 29
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA_Click;
                }
                break;
            case 9: // MainWindow.xaml line 30
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_Click;
                }
                break;
            case 10: // MainWindow.xaml line 31
                {
                    this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA).Click += this.ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA_Click;
                }
                break;
            case 11: // MainWindow.xaml line 32
                {
                    this.ChangeLibary_ElectricConnectorLibrary_v1_0_0 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_ElectricConnectorLibrary_v1_0_0).Click += this.ChangeLibary_ElectricConnectorLibrary_v1_0_0_Click;
                }
                break;
            case 12: // MainWindow.xaml line 33
                {
                    this.ChangeLibary_IndustrialSensorLibrary_v1_0_0 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_IndustrialSensorLibrary_v1_0_0).Click += this.ChangeLibary_IndustrialSensorLibrary_v1_0_0_Click;
                }
                break;
            case 13: // MainWindow.xaml line 34
                {
                    this.ChangeLibary_AddLibary = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.ChangeLibary_AddLibary).Click += this.ChangeLibary_AddLibary_Click;
                }
                break;
            case 14: // MainWindow.xaml line 22
                {
                    this.File_New = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.File_New).Click += this.File_New_Click;
                }
                break;
            case 15: // MainWindow.xaml line 23
                {
                    this.File_Open = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.File_Open).Click += this.File_Open_Click;
                }
                break;
            case 16: // MainWindow.xaml line 24
                {
                    this.File_Save = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)this.File_Save).Click += this.File_Save_Click;
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

