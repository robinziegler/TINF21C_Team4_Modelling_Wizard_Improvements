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
using Aml.Engine.CAEX;
using System.Drawing;

namespace ModellingWizard.UIs.SubPages
{
    public sealed partial class SystemClass : Page
    {
        private int LoadDepth = 0;
        private NavigationViewItem currentItem;
        public SystemClass()
        {
            this.InitializeComponent();
            loadTestObject(Instances.Loaded_System_Unit_Libs);

            // set height of the Grid (should be set to the maximum place that can be used)
            //GenericData_Grid_Row_1.Height = new Microsoft.UI.Xaml.GridLength(600, GridUnitType.Pixel); 
        }

        public List<AMLObjectTest> TestObjekt = new List<AMLObjectTest>();

        /// <summary>method <c>loadTestObject</c> lads the currently loaded Roll clases fron instances into a Navigation view</summary>
        private void loadTestObject(Objects.Libaries.Libary lib)
        {
            if (lib != null)
            {
                int index = 0; 
                foreach (Objects.Libaries.Libary sublib in lib.SubObjects)
                {
                    if (sublib != null)
                    {
                        GenerateNavigationMenueItems(sublib, index);
                    }
                    index++;
                }
                LoadDepth--;
            }
        }

        /* Navigation stuff */
        private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItemContainer as NavigationViewItem;

            if (item == null)
            {
                return;
            }
            if (item.Tag == null)
            {
                return;
            }
            if (item.Content == null)
            {
                return;
            }
            if (item.Tag.ToString() != "SystemAdd")
            {
                ContentFrame.Navigate(Type.GetType(item.Tag.ToString()), item.Name);
                //NavigationView.Header = item.Content;
                NavigationView.Header = null;
                NavigationView.SelectedItem = item;
            }
            else
            {
                var Win = new ModalViews.Add.AddSubLib(Objects.Enums.LibType.SystemUnitClass);
                ContentDialog dialog = new()
                {
                    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                    XamlRoot = this.XamlRoot,
                    Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Add system unit class",
                    PrimaryButtonText = "Add",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = Win,
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (Win.LibTreeView.SelectedItems != null && Win.LibTreeView.SelectedItems.Count > 0)
                    {
                        List<Objects.Libaries.Libary> libstoadd = new();
                        Win.LibTreeView.SelectedItems.ToList().ForEach(item =>
                        {
                            var x = (MyTreNode)item;
                            Instances.Loaded_System_Unit_Libs.SubObjects.Add(x.lib);
                            Instances.LibReload();
                            var win = (MainWindow)App.m_window;
                            win.ReloadInformations();
                            win.SetWarning();
                        });
                    }
                }
            }
        }


        private void GenerateNavigationMenueItems(Objects.Libaries.Libary sublib, int Index)
        {
            if (LoadDepth == 0)
            {
                if (sublib.SubObjects.Count != 0)
                {
                    var x = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
                        Name = sublib.myGuid,
                        IsRightTapEnabled = true,
                        Icon = new SymbolIcon { Symbol = Symbol.Library }
                    };
                    x.RightTapped += RightClickForDelete;
                    x.KeyDown += NavigationView_KeyDown;
                    x.IsExpanded = Index == 0;
                    x.IsSelected = Index == 0;
                    currentItem = x;
                    NavigationView.MenuItems.Add(x);
                    LoadDepth++;
                    loadTestObject(sublib);
                }
                else
                {
                    var x = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
                        Name = sublib.myGuid,
                        IsRightTapEnabled = true,
                        Icon = new SymbolIcon { Symbol = Symbol.Library }
                    };
                    x.RightTapped += RightClickForDelete;
                    x.KeyDown += NavigationView_KeyDown;
                    x.IsExpanded = Index == 0;
                    x.IsSelected = Index == 0;
                    NavigationView.MenuItems.Add(x);
                }
            }
            else
            {
                // Loads every other layer
                if (sublib.SubObjects.Count != 0)
                {
                    var x = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
                        Name = sublib.myGuid,
                        IsRightTapEnabled = false,
                        Icon = new SymbolIcon { Symbol = Symbol.Library }
                    };
                    currentItem.MenuItems.Add(x);
                    currentItem = x;
                    LoadDepth++;
                    loadTestObject(sublib);
                }
                else
                {
                    var x = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
                        Name = sublib.myGuid,
                        IsRightTapEnabled = false,
                        Icon = new SymbolIcon { Symbol = Symbol.Library }
                    };
                    currentItem.MenuItems.Add(x);
                }
            }
        }
        private void RightClickForDelete(object sender, RightTappedRoutedEventArgs e)
        {
            var navigationViewItem = sender as NavigationViewItem;
            DeleteObj(navigationViewItem.Content.ToString(), navigationViewItem.Name);
        }

        private void NavigationView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Delete || e.Key == Windows.System.VirtualKey.Back)
            {
                var selectedItem = NavigationView.SelectedItem as NavigationViewItem;
                DeleteObj(selectedItem.Content.ToString(), selectedItem.Name);
            }
        }

        private async void DeleteObj(string name, string id)
        {
            
            if (name != "SystemAdd")
            {
                ContentDialog dialog = new()
                {
                    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                    XamlRoot = this.XamlRoot,
                    Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Remove system class",
                    PrimaryButtonText = "Remove",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "Are you sure you want to remove " + name + "?",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Instances.Loaded_System_Unit_Libs.RemoveLib(id);
                    var win = (MainWindow)App.m_window;
                    win.ReloadInformations();
                    win.SetWarning();
                }
            }
        }
    }
}
