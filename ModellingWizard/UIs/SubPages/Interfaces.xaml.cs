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
using ModellingWizard.Objects;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModellingWizard.UIs.SubPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Interfaces : Page
    {
        public Interfaces()
        {
            this.InitializeComponent();
            loadTestObject(Objects.Instances.Loaded_Interfaces_Data);
        }
        private int LoadDepth = 0;
        private NavigationViewItem currentItem;

        private async void AddInterfaceButton_Click(object sender, RoutedEventArgs e)
        {
            var Win = new ModalViews.Interfaces.AddInterface();
            ContentDialog dialog = new()
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Add interface",
                PrimaryButtonText = "Add",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = Win
            };

            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Win.InterfaceTreeView.SelectedItems.Count();
            }
        }
        private void loadTestObject(Objects.Libaries.Libary lib)
        {
            if (lib != null)
            {

                foreach (Objects.Libaries.Libary sublib in lib.SubObjects)
                {
                    if (sublib != null)
                    {
                        GenerateNavigationMenueItems(sublib);
                    }
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
            /*else
            {
                var Win = new ModalViews.GenericData.AddRoleClass();
                ContentDialog dialog = new();

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = this.XamlRoot;
                dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Add role class";
                dialog.PrimaryButtonText = "Add";
                dialog.CloseButtonText = "Cancel";
                dialog.DefaultButton = ContentDialogButton.Primary;
                dialog.Content = Win;

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (Win.RoleClassTreeView.SelectedItems != null && Win.RoleClassTreeView.SelectedItems.Count > 0)
                    {
                        List<Objects.Libaries.Libary> libstoadd = new();
                        Win.RoleClassTreeView.SelectedItems.ToList().ForEach(item =>
                        {
                            var x = item as Objects.MyTreNode;
                            x.lib.SubObjects.Clear();
                            if (x.Depth == 0)
                            {
                                Instances.Loaded_RoleClass_Data ??= new();
                                libstoadd.Add(x.lib);
                            }
                            else
                            {
                            }


                        });
                    }
                }
            }*/
        }


        /// <summary>method <c>GenerateNavigationMenueItems</c> Generates the Navigation view items depending on the depth in the Libraty tree</summary>
        private void GenerateNavigationMenueItems(Objects.Libaries.Libary sublib)
        {
            if (LoadDepth == 0)
            {
                if (sublib.SubObjects.Count != 0)
                {
                    NavigationView.MenuItems.Add(currentItem = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.Interfaces_Detail",
                        Name = sublib.myGuid,
                    });
                    LoadDepth++;
                    loadTestObject(sublib);
                }
                else
                {
                    NavigationView.MenuItems.Add(new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.Interfaces_Detail",
                        Name = sublib.myGuid,
                    });
                }
            }
            else
            {
                // Loads every other layer
                if (sublib.SubObjects.Count != 0)
                {
                    currentItem.MenuItems.Add(currentItem = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.Interfaces_Detail",
                        Name = sublib.myGuid,
                    });
                    LoadDepth++;
                    loadTestObject(sublib);
                }
                else
                {
                    currentItem.MenuItems.Add(new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.Interfaces_Detail",
                        Name = sublib.myGuid,
                    });
                }
            }

        }
    }
}

