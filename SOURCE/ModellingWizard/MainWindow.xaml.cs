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
using System.ComponentModel.Design.Serialization;
using ModellingWizard.UIs.SubPages;
using Windows.Storage.Pickers;
using ModellingWizard.Processes.Save;
using System.Windows.Media;
using Microsoft.UI.Xaml.Automation;
using ModellingWizard.Processes.GeneralFunctions;
using System.Xml.Linq;
using CommunityToolkit.WinUI.Helpers;
using Windows.System;

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
            var version = Package.Current.Id.Version;
            Instances.AppVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            Instances.Build = string.Format("{0}", version.Revision);
            AppbarTitle.Text = applicationName + " | " + Instances.AppVersion;
            SetTitleBar(AppTitleBar);
            Instances.CurrentTheme = (int)App.Current.RequestedTheme;
            ThemeSwitch.Text = Instances.CurrentTheme == (int)ApplicationTheme.Dark ? "Lightmode" : "Darkmode";


            AppMode.Text = Instances.ExpertMode ? "Easy Mode" : "Expert Mode";
            CAEXVersionButton.Text = Instances.UseCAEX3 ? "Use CAEX 3.0 File" : "Use CAEX 2.15 File";

            /* Informations about opend file */
            OpenedFileName.Text = Instances.FileName;
            SavedInformation.Text = "[unloaded]";
        }

        /* Infos if opend file is saved or not */
        private bool unsavedInformations = false;

        public void SomethingChanged(bool changed)
        {
            unsavedInformations = changed;
            SavedInformation.Text = changed ? "[Unsaved]" : "[Saved]";
            SetWarning();
        }


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
            SetWarning();
        }

        /* File options */
        private async void File_New_Click(object sender, RoutedEventArgs e)
        {
            if(unsavedInformations)
            {
                ContentDialog dialog = new()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "You have unsaved data",
                    Content = "If you continue, data may be lost",
                    PrimaryButtonText = "Continue",
                    SecondaryButtonText = "Cancel",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };
                ContentDialogResult result = await dialog.ShowAsync();
                if(result == ContentDialogResult.Primary)
                {
                    unsavedInformations = false;
                    SomethingChanged(false);
                    Processes.New.CreateSysClass.Execute();
                    ReloadInformations();
                    SetWarning();
                    
                }
            }
            else
            {
                Processes.New.CreateSysClass.Execute();
                ReloadInformations();
                SetWarning();
            }
        }

        private async void File_Open_Click(object sender, RoutedEventArgs e)
        {
            if (unsavedInformations)
            {
                ContentDialog dialog = new()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "You have unsaved data",
                    Content = "If you continue, data may be lost",
                    PrimaryButtonText = "Continue",
                    SecondaryButtonText = "Cancel",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };
                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    unsavedInformations = false;
                    SomethingChanged(false);
                    OpenFile();
                }
            }
            else
            {
                OpenFile();
            }
            SetWarning();


        }

        private async void OpenFile()
        {
            try
            {
                var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".aml");
                openPicker.FileTypeFilter.Add(".edz");
                openPicker.FileTypeFilter.Add(".amlx");

                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {

                    if (file.Name.EndsWith(".edz"))
                    {
                        ConverterAML converterAML = new ConverterAML();

                        //add path to generate amlx file
                        converterAML._pathAMLDestinationDirectory = Path.GetDirectoryName(file.Path);
                        //function of class to export .edz file to .amlx
                        converterAML.exportStart(file.Path);
                        //get path to amlx file generated
                        string AMLXFile = converterAML._pathAMLDestinationDirectory + "\\" + converterAML._AMLXFileName;
                        //send path to function to open amlx file generated
                        Console.WriteLine(AMLXFile);
                        var results = Processes.Open.Open.OpenFiles(File.ReadAllBytes(AMLXFile), converterAML._AMLXFileName, AMLXFile);
                    }
                    Instances.Attachments.Clear();
                    var result = Processes.Open.Open.OpenFiles(File.ReadAllBytes(file.Path), file.Name, file.Path);
                    OpenedFileName.Text = file.DisplayName;
                    Instances.CurrentFile = new(file.Path);
                }
                ReloadInformations();
            }
            catch (Exception e)
            {
                ContentDialog dialog = new()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Error while opening file",
                    Content = "Please check your file! ", // + e,
                    PrimaryButtonText = "Ok",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };
                ContentDialogResult result = await dialog.ShowAsync();
            }
            
        }

        private async void File_Save_Click(object sender, RoutedEventArgs e)
        {
            switch (CheckFileAndGetWarning())
            {
                case Objects.Enums.WarningType.Non:
                    ShowSaveDialog();
                    break;
                case Objects.Enums.WarningType.SUCNotFound:
                    ContentDialog dialogNotFound = new()
                    {
                        XamlRoot = this.Content.XamlRoot,
                        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                        Title = "Error no system unit class",
                        Content = "Your file doesn't include a system unit class. Please add a valid system unit class.",
                        PrimaryButtonText = "Ok",
                        RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                    };
                    _ = await dialogNotFound.ShowAsync();
                    break;
                case Objects.Enums.WarningType.AttributesNull:
                    ContentDialog dialogAttribute = new()
                    {
                        XamlRoot = this.Content.XamlRoot,
                        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                        Title = "Error in system unit class",
                        Content = "Check if the system unit class includes valid data for manufacturer and productcode.",
                        PrimaryButtonText = "Ok",
                        RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                    };
                    _ = await dialogAttribute.ShowAsync();
                    break;
            }
        }

        private async void ShowSaveDialog()
        {
            try
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("AML", new List<string>() { ".aml" });
                savePicker.FileTypeChoices.Add("AMLX", new List<string>() { ".amlx" });
                savePicker.SuggestedFileName = ((MainWindow)App.m_window).OpenedFileName.Text;

                // Open the picker for the user to pick a file
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    Save.SaveFile(file.Path, file.Name, file.FileType);
                    SavedInformation.Text = "[Saved]";
                }
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Error while saving the file",
                    Content = "Please check your inserted data! " + ex.ToString(),
                    PrimaryButtonText = "Ok",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };
                ContentDialogResult result = await dialog.ShowAsync();
            }
        }

        /* Change libary options */
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_Click(object sender, RoutedEventArgs e) { 
            Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0); 
        }

        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_CAEX3_BETA_Click(object sender, RoutedEventArgs e) { 
            Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_CAEX3_BETA); 
        }
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_Click(object sender, RoutedEventArgs e) {
            Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_Full);  
        }
        private void ChangeLibary_AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA_Click(object sender, RoutedEventArgs e) {
            Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA); 
        }
        private void ChangeLibary_ElectricConnectorLibrary_v1_0_0_Click(object sender, RoutedEventArgs e) {
            Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.ElectricConnectorLibrary_v1_0_0); 
        }
        private void ChangeLibary_IndustrialSensorLibrary_v1_0_0_Click(object sender, RoutedEventArgs e) {
            Processes.Libary.LoadStandardLibaries.Load(Objects.Libaries.LibaryTypes.IndustrialSensorLibrary_v1_0_0); 
        }





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
                Objects.Instances.System_Unit_Libs = result.Item3;
            }
        }

        /* Options */
        private void AppMode_Click(object sender, RoutedEventArgs e)
        {
            Instances.ExpertMode = !Instances.ExpertMode;
            AppMode.Text = Instances.ExpertMode ? "Easy Mode" : "Expert Mode";
            ReloadInformations();
        }

        /* Help Options */
        private async void Help_About_Click(object sender, RoutedEventArgs e)
        {
            var Win = new UIs.ModalViews.Help_Menu.AboutPage();
            ContentDialog dialog = new()
            {
                XamlRoot = this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "About",
                CloseButtonText = "Close",
                Content = Win,
                RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
            };
            ContentDialogResult result = await dialog.ShowAsync();
        }
        private void Help_GitHub_Click(object sender, RoutedEventArgs e)
        {
            /*  var Win = new UIs.ModalViews.Help_Menu.ManualPage();
              ContentDialog dialog = new()
              {
                  XamlRoot = this.Content.XamlRoot,
                  Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                  Title = "Manual",
                  CloseButtonText = "Close",
                  Content = Win,
                  RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
              };
              ContentDialogResult result = await dialog.ShowAsync();
            */

        }

        private async void Help_Manual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri uri = new("https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/wiki/Manual");
                bool success = await Launcher.LaunchUriAsync(uri);
            }
            catch
            {
                
            }

          /*  var Win = new UIs.ModalViews.Help_Menu.ManualPage();
            ContentDialog dialog = new()
            {
                XamlRoot = this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Manual",
                CloseButtonText = "Close",
                Content = Win,
                RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
            };
            ContentDialogResult result = await dialog.ShowAsync();
          */

        }


        /// <summary>
        /// Change the theme and safe the choice in local stoarage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {   

            if (Instances.CurrentTheme == (int)ApplicationTheme.Dark)
            {
                Instances.CurrentTheme = 0;
                ThemeSwitch.Text = "Darkmode";
                
                //App.Current.RequestedTheme = ApplicationTheme.Light;
                Grid_Main.RequestedTheme = ElementTheme.Light;
            }
            else if (Instances.CurrentTheme == (int)ApplicationTheme.Light)
            {
                Instances.CurrentTheme = 1;
                ThemeSwitch.Text = "Lightmode";
                Grid_Main.RequestedTheme = ElementTheme.Dark;
                //App.Current.RequestedTheme = ApplicationTheme.Dark;

            }
            ApplicationData.Current.LocalSettings.Values["themeSetting"] = Instances.CurrentTheme;
        }

        private async void Grid_Main_Loaded(object sender, RoutedEventArgs e)
        {
            var Win = new UIs.ModalViews.Help_Menu.ManualPage();
            ContentDialog dialog = new()
            {
                XamlRoot = this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Modelling Wizard",
                Content = "Do you want to open or create a new file?",
                PrimaryButtonText = "Open File",
                SecondaryButtonText = "Create new File",
                RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                OpenFile();
                SavedInformation.Text = "[Saved]";
            }
            else if(result == ContentDialogResult.Secondary)
            {
                Processes.New.CreateSysClass.Execute();
                ReloadInformations();
            }
        }

        private void Grid_Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ReloadInformations();
        }

        public void ReloadInformations()
        {
            var currentSelected = NavigationView.SelectedItem;
            NavigationView.SelectedItem = MainPage_Navigation_SystemClass ;
            NavigationView.SelectedItem = MainPage_Navigation_GenericData;
            NavigationView.SelectedItem = currentSelected;
        }

        public void ChangedFileName()
        {
            
            Objects.Libaries.Libary ret = Instances.Loaded_System_Unit_Libs.Find("IdentificationData", false);
            if(ret != null)
            {
                ret.Attributes.ForEach(a =>
                {
                    if(a.Name == "IdentificationData")
                    {
                        string manufactur = "";
                        string productCode = "";
                        a.SubAttrebutes.ForEach(sa =>
                        {
                            if(sa.Name.ToLower() == "manufacturer")
                            {
                                manufactur = sa.Value;
                            }
                            if(sa.Name.ToLower() == "productcode")
                            {
                                productCode = sa.Value;
                            }
                        });
                        if(manufactur != "" && productCode != "")
                        {
                            OpenedFileName.Text = manufactur + "." + productCode;
                        }
                    }
                });
               
            }
        }

        private Objects.Enums.WarningType CheckFileAndGetWarning()
        {
            if (Instances.Loaded_System_Unit_Libs == null)
                return Objects.Enums.WarningType.Non;

            Objects.Enums.WarningType warning = Objects.Enums.WarningType.Non;
            Objects.Libaries.Libary ret = Instances.Loaded_System_Unit_Libs.Find("IdentificationData", false);
            if (ret != null)
            {
                ret.Attributes.ForEach(a =>
                {
                    if (a.Name == "IdentificationData")
                    {
                        string manufactur = "";
                        string productCode = "";
                        a.SubAttrebutes.ForEach(sa =>
                        {
                            if (sa.Name.ToLower() == "manufacturer")
                            {
                                manufactur = sa.Value;
                            }
                            if (sa.Name.ToLower() == "productcode")
                            {
                                productCode = sa.Value;
                            }
                        });
                        if (manufactur != "" && productCode != "")
                        {
                            warning = Objects.Enums.WarningType.Non;
                        }
                        else
                        {
                            warning = Objects.Enums.WarningType.AttributesNull;
                        }
                    }
                });
            }
            else
            {
                warning = Objects.Enums.WarningType.SUCNotFound;
            }
            return warning;
        }

        public void SetWarning()
        {
            Objects.Enums.WarningType warning = CheckFileAndGetWarning();
            switch (warning)
            {
                case Objects.Enums.WarningType.Non:
                    WarningIcon.Visibility = Visibility.Collapsed;
                    break;
                case Objects.Enums.WarningType.SUCNotFound:
                    WarningIcon.Visibility = Visibility.Visible;
                    WarningIcon.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
                    ToolTip tooltipSUCNotFound = new()
                    {
                        Content = "Error: Missing System Unit Class"
                    };
                    ToolTipService.SetToolTip(WarningIcon, tooltipSUCNotFound);
                    break;
                case Objects.Enums.WarningType.AttributesNull:
                    WarningIcon.Visibility = Visibility.Visible;
                    WarningIcon.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 128, 0));
                    ToolTip tooltipAttribute = new()
                    {
                        Content = "Error: Missing attributes for manufacturer or productcode in system unit class"
                    };
                    ToolTipService.SetToolTip(WarningIcon, tooltipAttribute);
                    break;
            }
        }

        private void TextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            NavigationView.SelectedItem = MainPage_Navigation_SystemClass;
        }

        private void CAEXVersionButton_Click(object sender, RoutedEventArgs e)
        {
            Instances.UseCAEX3 = !Instances.UseCAEX3;
            CAEXVersionButton.Text = Instances.UseCAEX3 ? "Use CAEX 3.0 File" : "Use CAEX 2.15 File";
        }
    }
}
