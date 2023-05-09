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
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Store;
using CommunityToolkit.WinUI.Helpers;
using ModellingWizard.Objects;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModellingWizard.UIs.SubPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Attachments : Page
    {
        public Attachments()
        {
            this.InitializeComponent();
            LoadNavigation();
        }

        private void LoadNavigation()
        {
            NavigationView.MenuItems.Clear();
            Instances.Attachments.ForEach(attachment =>
            {
                var x = new NavigationViewItem
                {
                    Content = attachment.Title,
                    Tag = "ModellingWizard.UIs.SubPages.Attachments_Detail",
                    Name = attachment.UUID,
                    IsRightTapEnabled = true,
                    Icon = new SymbolIcon { Symbol = Symbol.Document }
                };
                x.RightTapped += RightClickForDelete;
                x.KeyDown += NavigationView_KeyDown;
                NavigationView.MenuItems.Add(x);
            });
        }

        private void LoadedAttachments_RowEditEnded(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            var x = (MainWindow)App.m_window;
            x.SomethingChanged(true);
        }

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
                var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);

                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add("*");

                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    var contentOfFile = await file.ReadBytesAsync();
                    Objects.Instances.Attachments.Add(new()
                    {
                        Title = file.Name,
                        Base64Content = System.Convert.ToBase64String(contentOfFile)
                    });
                    LoadNavigation();

                }
            }
        }

        private void ContentFrame_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Type.GetType("ModellingWizard.UIs.SubPages.Attachments_Detail"), "");
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
                    Title = "Remove attachment",
                    PrimaryButtonText = "Remove",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "Are you sure you want to remove " + name + "?",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Instances.Loaded_Interfaces_Data.RemoveLib(id);
                    var win = (MainWindow)App.m_window;
                    win.ReloadInformations();
                    win.SetWarning();
                }
            }
        }
    }
}
