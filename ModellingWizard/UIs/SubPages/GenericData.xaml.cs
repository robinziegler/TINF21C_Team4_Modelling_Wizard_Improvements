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
        public GenericData()
        {
            this.InitializeComponent();

            //load test data (should be removed later)
            loadTestObject();

            // set height of the Grid (should be set to the maximum place that can be used)
            //GenericData_Grid_Row_1.Height = new Microsoft.UI.Xaml.GridLength(600, GridUnitType.Pixel); 
        }
        
        private async void AddRoleClassButton_Click(object sender, RoutedEventArgs e)
        {
            var Win = new ModalViews.GenericData.AddRoleClass();
            ContentDialog dialog = new ContentDialog();

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
                Win.RoleClassTreeView.SelectedItems.Count();
            }
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


        private void loadTestObject()
        {
            List<string> AMLNames = new List<string> {"First Object", "Second Object", "Third Object" };
            List<string> AttributsNames = new List<string> {"Manufactor", "ID", "Name", "Date", "Costs" };
            foreach (var name in AMLNames)
            {
                AMLObjectTest newObject = new AMLObjectTest();
                newObject.Name = name;
                newObject.Attributs = new List<AttributsTest> ();
                foreach (var AttributName in AttributsNames)
                {
                    AttributsTest attribut = new AttributsTest();
                    attribut.Name = AttributName;
                    attribut.Value = "Test";
                    attribut.Default = "Test";
                    attribut.Unit = "Test";
                    attribut.Semantic = "Test";
                    attribut.DataType = "xs:string";
                    newObject.Attributs.Add(attribut);
                }
                TestObjekt.Add(newObject);
            }
        }

        /* Navigation stuff */
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
            ContentFrame.Navigate(Type.GetType(item.Tag.ToString()), item.Content);
            //NavigationView.Header = item.Content;
            NavigationView.Header = null;
            NavigationView.SelectedItem = item;
        }

    }
}
