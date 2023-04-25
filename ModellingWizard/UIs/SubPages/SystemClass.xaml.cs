﻿using System;
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

        private async void AddRoleClassButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void SetMode(bool Expert)
        {
            Visibility visibility = Expert ? Visibility.Visible : Visibility.Collapsed;
            /* Settings */
            //AddRoleClassButton.Visibility = visibility;
        }


        private void ListView_Loading(FrameworkElement sender, object args)
        {
            //ListView_AMLObjects.ItemsSource = TestObjekt;
        }

        private void ListView_AMLObjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AMLObjectTest selectedAMLObject = e.AddedItems[0] as AMLObjectTest;
                if (selectedAMLObject != null)
                {
                    List<AttributsTest> attributs = selectedAMLObject.Attributs;
                    //loadAttributs(attributs);
                }
            }
        }

        public List<AMLObjectTest> TestObjekt = new List<AMLObjectTest>();

        /// <summary>method <c>loadTestObject</c> lads the currently loaded Roll clases fron instances into a Navigation view</summary>
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
            else
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
            }
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
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
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
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
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
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
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
                        Tag = "ModellingWizard.UIs.SubPages.SystemClass_Details",
                        Name = sublib.myGuid,
                    });
                }
            }

        }
    }
}
