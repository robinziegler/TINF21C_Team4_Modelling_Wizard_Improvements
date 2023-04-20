// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinRT;
using System.Windows.Controls;
using Page = Microsoft.UI.Xaml.Controls.Page;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModellingWizard.UIs.SubPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenericData_Detail : Page
    {
        public GenericData_Detail()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string parameter = e.Parameter as string;
            if (parameter == null)
            {
                //Navigate to emty Atrebutes page
            }else {
                Objects.Libaries.Libary lib = Processes.GeneralFunctions.SeartchforGuid.SeartchLibrary(parameter, Objects.Instances.Loaded_RoleClass_Data);
                if (lib.Attributes.Count > 0)
                {
                    /* Main Expandern */
                    if (lib.Attributes.FindAll(x => x.SubAttrebutes.Count == 0).Count > 0)
                    {
                        CommunityToolkit.WinUI.UI.Controls.Expander mainExpander = new()
                        {
                            IsExpanded = true,
                            ExpandDirection = CommunityToolkit.WinUI.UI.Controls.ExpandDirection.Down,
                            VerticalAlignment = VerticalAlignment.Top,
                            Header = "Main Attributes"
                        };

                        CommunityToolkit.WinUI.UI.Controls.DataGrid mainDataGrid = new()
                        {

                        };
                        mainDataGrid.ItemsSource = lib.Attributes.FindAll(x => x.SubAttrebutes.Count == 0);
                        mainExpander.Content = mainDataGrid;
                        DetailContent.Items.Add(mainExpander);
                    }
                    lib.Attributes.ForEach(x =>
                    {
                        SubObjects(x);
                    });
                }
            }
        }

        public void SubObjects(Objects.Libaries.LibaryObject lib)
        {
            /* Main Expandern */
            if (lib.SubAttrebutes.FindAll(x => x.SubAttrebutes.Count == 0).Count > 0)
            {
                CommunityToolkit.WinUI.UI.Controls.Expander mainExpander = new()
                {
                    IsExpanded = false,
                    ExpandDirection = CommunityToolkit.WinUI.UI.Controls.ExpandDirection.Down,
                    VerticalAlignment = VerticalAlignment.Top,
                    Header = lib.Name
                };

                CommunityToolkit.WinUI.UI.Controls.DataGrid mainDataGrid = new()
                {

                };
                mainDataGrid.ItemsSource = lib.SubAttrebutes.FindAll(x => x.SubAttrebutes.Count == 0);
                mainExpander.Content = mainDataGrid;
                DetailContent.Items.Add(mainExpander);
            }
            /* Sub Expanders for each Header */
            lib.SubAttrebutes.FindAll(x => x.SubAttrebutes.Count > 0).ForEach(attr =>
            {
                SubObjects(attr);
            });
        }
    }
}
