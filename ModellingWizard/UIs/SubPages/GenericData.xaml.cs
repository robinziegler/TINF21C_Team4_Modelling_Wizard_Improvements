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
            if(item.Tag.ToString() != "SystemAdd")
            {
                ContentFrame.Navigate(Type.GetType(item.Tag.ToString()), item.Name);
                //NavigationView.Header = item.Content;
                NavigationView.Header = null;
                NavigationView.SelectedItem = item;
            }
            else
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
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
                        Tag = "ModellingWizard.UIs.SubPages.GenericData_Detail",
                        Name = sublib.myGuid,
                    });
                }
            }

        }
    }
}
