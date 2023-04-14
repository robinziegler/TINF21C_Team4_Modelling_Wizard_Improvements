using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CommunityToolkit.WinUI.UI.Controls;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using ModellingWizard.Objects;
using Windows.ApplicationModel;
using Windows.Storage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Microsoft.UI;


namespace ModellingWizard
{
    /// <summary>
    /// Main Window of the Modelling Wizard
    /// Here is the menubar defined and the navigation bar to change between the sub pages
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            // Hide default title bar.
            ExtendsContentIntoTitleBar = true;
            // Set new title bar from xaml
            // the used informations can be changed in appxmanifest
            string applicationName = AppInfo.Current.DisplayInfo.DisplayName;
            string applicationVersionMajor = AppInfo.Current.Package.Id.Version.Major.ToString();
            string applicationVersionMinor = AppInfo.Current.Package.Id.Version.Minor.ToString();
            string applicationVersionRevision = AppInfo.Current.Package.Id.Version.Revision.ToString();
            string applicationVersion = applicationVersionMajor + "." + applicationVersionMinor + "." + applicationVersionRevision;
            string applicationInstallationnDate = AppInfo.Current.Package.InstalledDate.ToString().Split(" ")[0];

            AppbarTitle.Text = applicationName + " - " + applicationVersion;
            SetTitleBar(AppTitleBar);
            _currentTheme = (int)App.Current.RequestedTheme;

            ThemeButton.Text = _currentTheme == (int)ApplicationTheme.Dark ? "Lightmode" : "Darkmode";
            Instances.ExpertMode = false;
        }

        private int _currentTheme { get; set; }

        /* Navigation stuff */
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(args.SelectedItemContainer as NavigationViewItem);
        }

        public void SetCurrentNavigationViewItem(NavigationViewItem item)
        {
            if (item == null)
            {
                return;
            }
            if (item.Tag == null)
            {
                return;
            }
            if(item.Content == null)
            {
                return;
            }
            ContentFrame.Navigate(
                Type.GetType(item.Tag.ToString()), item.Content);
            //NavigationView.Header = item.Content;
            NavigationView.Header = null;
            NavigationView.SelectedItem = item;
        }

        /* File options */
        private void File_New_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void File_Open_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".aml");
            openPicker.FileTypeFilter.Add(".amlx");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var result = Processes.Open.Open.OpenFiles(File.ReadAllBytes(file.Path), file.Name, file.Path);
            }
        }

        private async void File_Save_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("AML", new List<string>() { ".aml" });
            savePicker.SuggestedFileName = "New Document";

            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {

            }
        }

        /* Change libary options */
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_Click(object sender, RoutedEventArgs e) { Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0); }
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA_Click(object sender, RoutedEventArgs e) { Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_CAEX3_BETA); }
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_Click(object sender, RoutedEventArgs e) { Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_Full);  }
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA_Click(object sender, RoutedEventArgs e) { Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA); }
        private void ChangeLibary_ElectricConnectorLibrary_v1_0_0_Click(object sender, RoutedEventArgs e) { Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.ElectricConnectorLibrary_v1_0_0); }
        private void ChangeLibary_IndustrialSensorLibrary_v1_0_0_Click(object sender, RoutedEventArgs e) { Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.IndustrialSensorLibrary_v1_0_0); }

        private async void ChangeLibary_AddLibary_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".aml");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var result = Processes.Libary.Load.LoadLib(File.ReadAllBytes(file.Path), file.Name);
                Objects.Instances.RoleClassLib = result.Item1;
                Objects.Instances.InterfacesLib = result.Item2;
            }
        }

        /* Options */
        private void AppMode_Click(object sender, RoutedEventArgs e)
        {
            Instances.ExpertMode = !Instances.ExpertMode;
            AppMode.Text = Instances.ExpertMode ? "Easy Mode" : "Advanced Mode";
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {

            if (_currentTheme == (int)ApplicationTheme.Dark)
            {
                _currentTheme = 0;
                Grid_Main.RequestedTheme = ElementTheme.Light;
                ThemeButton.Text = "Darkmode";
            }
            else if (_currentTheme == (int)ApplicationTheme.Light)
            {
                _currentTheme = 1;
                Grid_Main.RequestedTheme = ElementTheme.Dark;
                ThemeButton.Text = "Lightmode";

            }
            ApplicationData.Current.LocalSettings.Values["themeSetting"] = _currentTheme;
        }

        /* Help Options */
        private async void Help_About_Click(object sender, RoutedEventArgs e)
        {
            var Win = new UIs.ModalViews.Help_Menu.AboutPage();
            ContentDialog dialog = new()
            {
                XamlRoot = this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "About the Modelling Wizard for Devices Application",
                CloseButtonText = "Close",
                Content = Win
            };
            ContentDialogResult result = await dialog.ShowAsync();
        }

        private async void Help_Manual_Click(object sender, RoutedEventArgs e)
        {
            var Win = new UIs.ModalViews.Help_Menu.ManualPage();
            ContentDialog dialog = new()
            {
                XamlRoot = this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Manual",
                CloseButtonText = "Close",
                Content = Win
            };
            ContentDialogResult result = await dialog.ShowAsync();
        }


        /// <summary>
        /// Change the theme and safe the choice in local stoarage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {   

            if (_currentTheme == (int)ApplicationTheme.Dark)
            {
                _currentTheme = 0;
                Grid_Main.RequestedTheme = ElementTheme.Light;
            }
            else if (_currentTheme == (int)ApplicationTheme.Light)
            {
                _currentTheme = 1;
                Grid_Main.RequestedTheme = ElementTheme.Dark;

            }
            ApplicationData.Current.LocalSettings.Values["themeSetting"] = _currentTheme;
        }


    }
}
