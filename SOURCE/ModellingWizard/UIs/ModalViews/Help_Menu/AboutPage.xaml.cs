// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ModellingWizard.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModellingWizard.UIs.ModalViews.Help_Menu
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
            this.Height = 500;
            this.Width = 500;
            
        }

        private void AboutText_Loaded(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new();
            sb.AppendLine("This Modelling Wizard for Devices standalone application can be used to create or modify Devices and Interfaces. It can als be used to import IODD and GSDML Files whih will be converted to an AMLX Package. Also added functionality to convert EDZ files." + Environment.NewLine);
            sb.AppendLine("Version: " + Instances.AppVersion + Environment.NewLine );
            sb.AppendLine("Build: " + Instances.Build + Environment.NewLine );
            sb.AppendLine("Design and Software development of earlier version by:" + Environment.NewLine);
            sb.AppendLine("TINF17C (DHBW Stuttgart)" + Environment.NewLine) ;
            sb.AppendLine("Raj Kumar Pulaparthi (Otto-von-Guericke University Magdeburg)" + Environment.NewLine) ;
            sb.AppendLine("TINF20C (DHBW Stuttgart)" + Environment.NewLine+ Environment.NewLine) ;
            sb.AppendLine("Design and Software development of current version by" + Environment.NewLine) ;
            sb.AppendLine("TINF21C (DHBW Stuttgart)" + Environment.NewLine + Environment.NewLine) ;
            sb.AppendLine("This Pulgin was created as a group project in the class \"Software Engineering\"" + Environment.NewLine) ;
            sb.AppendLine("Later this plugin was developed as a sample tool that create vendor independant automaton component and inclued as part of Master Thesis" + Environment.NewLine) ;
            sb.AppendLine("Terms and conditions for copying, distribution and modification:" + Environment.NewLine) ;
            sb.AppendLine("This project is licensed under the GPL 3.0 license." + Environment.NewLine) ;
            sb.AppendLine("Please visit out GitHub-Repository to learn more about this." + Environment.NewLine) ;
            sb.AppendLine("This Plugin uses third-party software for the convertions of IODD and GSDML." + Environment.NewLine) ;
            sb.AppendLine("All Rights reserved by the corresponding copyright owner." + Environment.NewLine) ;

            AboutText.Text = sb.ToString();
        }
    }
}
