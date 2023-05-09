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
using System.Threading.Tasks;
using ModellingWizard.Objects;
using ModellingWizard.Processes.Libary;
using System.CodeDom.Compiler;
using Microsoft.UI.Xaml.Documents;
using System.Drawing;

namespace ModellingWizard.UIs.SubPages
{


    
    // Test Objekt
    // Only tests 
    public class AMLObjectTest
    {
        public string Name { get; set; }
        public List<AttributsTest> Attributs { get; set; }
        public List<AMLObjectTest> underObjects { get; set; }
    }

    public class AttributsTest
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public string Unit { get; set; }
        public string Semantic { get; set; }
        public string DataType { get; set; }
        public List<string> ReferencedClassName { get; set; }
    }
    /// <summary>
    /// This is the Generic Data Page
    /// </summary>
    public sealed partial class GenericData : Page
    {
        private int LoadDepth = 0;
        private NavigationViewItem currentItem;
        public GenericData()
        {
            this.InitializeComponent();

            //load test data (should be removed later)
            loadTestObject(Instances.Loaded_RoleClass_Data);
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
            else if(item.Tag.ToString() == "SystemAdd")
            {
                var Win = new ModalViews.Add.AddSubLib(Objects.Enums.LibType.RoleClass);
                ContentDialog dialog = new()
                {
                    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                    XamlRoot = this.XamlRoot,
                    Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Add role class",
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
                            var x = (MyTreNode) item;
                            Instances.Loaded_RoleClass_Data.SubObjects.Add(x.lib);
                            Instances.LibReload();
                            var win = (MainWindow)App.m_window;
                            win.ReloadInformations();
                        });
                    }
                }
            }
            else
            {
                ContentFrame.Navigate(Type.GetType(item.Tag.ToString()), item.Name);
                NavigationView.Header = null;
                NavigationView.SelectedItem = item;
            }
        }

        private void GenerateNavigationMenueItems(Objects.Libaries.Libary sublib)
        {
            if (LoadDepth == 0)
            {
                if (sublib.SubObjects.Count != 0)
                {
                    var x = new NavigationViewItem
                    {
                        Content = sublib.Name,
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
                        Name = sublib.myGuid,
                        IsRightTapEnabled = true,
                        Icon = new SymbolIcon { Symbol = Symbol.Library }
                    };
                    x.RightTapped += RightClickForDelete;
                    x.KeyDown += NavigationView_KeyDown;
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
                        Name = sublib.myGuid,
                        IsRightTapEnabled = true,
                        Icon = new SymbolIcon { Symbol = Symbol.Library }
                    };
                    x.RightTapped += RightClickForDelete;
                    x.KeyDown += NavigationView_KeyDown;
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
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
                    Title = "Remove role class",
                    PrimaryButtonText = "Remove",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "Are you sure you want to remove " + name + "?",
                    RequestedTheme = Instances.CurrentTheme == 1 ? ElementTheme.Dark : ElementTheme.Light
                };

                ContentDialogResult result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Instances.Loaded_RoleClass_Data.RemoveLib(id);
                    var win = (MainWindow)App.m_window;
                    win.ReloadInformations();
                    win.SetWarning();
                }
            }
        }
    }
}
