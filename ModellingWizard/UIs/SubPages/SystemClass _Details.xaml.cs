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
using ModellingWizard.Objects;
using Aml.Engine.CAEX.Extensions;
using Aml.Engine.CAEX;
using CommunityToolkit.WinUI.UI.Controls;

namespace ModellingWizard.UIs.SubPages
{
    public sealed partial class SystemClass_Details : Page
    {

        public SystemClass_Details()
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
            }
            else
            {
                Objects.Libaries.Libary lib = Processes.GeneralFunctions.SeartchforGuid.SeartchLibrary(parameter, Objects.Instances.Loaded_System_Unit_Libs);
                if (lib.Attributes.Count > 0)
                {
                    /* Main Expandern */
                    if (lib.Attributes.FindAll(x => x.SubAttrebutes.Count == 0).Count > 0)
                    {
                        CommunityToolkit.WinUI.UI.Controls.DataGrid mainDataGrid = new()
                        {

                        };
                        mainDataGrid.ItemsSource = lib.Attributes.FindAll(x => x.SubAttrebutes.Count == 0);
                        DetailContent.Items.Add(mainDataGrid);
                    }
                    lib.Attributes.FindAll(x => x.SubAttrebutes.Count > 0).ForEach(subAttr =>
                    {
                        DetailContent.Items.Add(SubObjects(subAttr));
                    });
                }
            }
        }

        public CommunityToolkit.WinUI.UI.Controls.Expander SubObjects(Objects.Libaries.LibaryObject lib)
        {
            /* Main Expandern */
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
            mainDataGrid.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(MainGridChanged);

            /* Create ListView */
            ListView listView = new ListView();
            listView.Items.Add(mainDataGrid);
            mainExpander.Content = listView;
            /* Sub Expanders for each Header */
            lib.SubAttrebutes.FindAll(x => x.SubAttrebutes.Count > 0).ForEach(attr =>
            {
                listView.Items.Add(SubObjects(attr));
            });
            return mainExpander;
        }


        private void MainGridChanged(object sender, DataGridCellEditEndedEventArgs e)
        {
            var mainWin = (MainWindow)App.m_window;
            mainWin.ChangedFileName();
            mainWin.SomethingChanged(true);
        }
    }
}
