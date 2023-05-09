// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using ModellingWizard.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModellingWizard.UIs.ModalViews.Add
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddSubLib : Page
    {
        public AddSubLib(Objects.Enums.LibType libType)
        {
            this.InitializeComponent();
            
            GetLib(libType).SubObjects.ForEach(x =>
            {
                MyTreNode t = new()
                {
                    Content = x.Name,
                    lib = x
                };
                x.SubObjects.ForEach(y =>
                {
                    t.Children.Add(CreateSubNodes(y));
                });

                LibTreeView.RootNodes.Add(t);
            });
        }

        private Objects.Libaries.Libary GetLib(Objects.Enums.LibType libType)
        {
            switch (libType)
            {
                case Objects.Enums.LibType.RoleClass:
                    return Objects.Instances.RoleClassLib;
                case Objects.Enums.LibType.SystemUnitClass:
                    return Objects.Instances.System_Unit_Libs;
                case Objects.Enums.LibType.Interfaces:
                    return Objects.Instances.InterfacesLib;
                default: 
                    return null;
            }
        }

        private MyTreNode CreateSubNodes(Objects.Libaries.Libary libFile)
        {
            MyTreNode node = new() { Content = libFile.Name };
            node.lib = libFile;
            libFile.SubObjects.ForEach(x =>
            {
                node.Children.Add(CreateSubNodes(x));
            });
            return node;
        }
    }
}
