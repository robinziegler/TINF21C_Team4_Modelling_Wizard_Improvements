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
    /// <summary>
    /// This is the Generic Data Page
    /// </summary>
    public sealed partial class GenericData : Page
    {
        public GenericData()
        {
            this.InitializeComponent();
            loadAttributs();
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
            if(result == ContentDialogResult.Primary)
            {
                Win.RoleClassTreeView.SelectedItems.Count();
            }
        }

        public void SetMode(bool Expert)
        {
            Visibility visibility = Expert ? Visibility.Visible : Visibility.Collapsed;
            /* Settings */
            //AddRoleClassButton.Visibility = visibility;

            /* Table */
            DefaultGrid.Visibility = visibility;
            UnitsGrid.Visibility = visibility;
            DataTypeGrid.Visibility = visibility;
            SemanticGrid.Visibility = visibility;

        }

        public class LibaryObjectTest
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Default { get; set; }
            public string Unit { get; set; }
            public string Semantic { get; set; }
            public string DataType { get; set; }
        }

        public void loadAttributs()
        {
            
            List<LibaryObjectTest> LibariesTest = new List<LibaryObjectTest>(10);


            for (int i = 0; i < 10; i++) { 
                LibaryObjectTest libary = new LibaryObjectTest();
                libary.Name = i.ToString();
                libary.Value = "10";
                libary.Default = "Default";
                libary.Unit = "1";
                libary.Semantic = "maybe";
                libary.DataType = "String";
                libary.Default = "TEst";

                LibariesTest.Add(libary);
            }
            DataGridAttributs.ItemsSource = LibariesTest;
        }
    }
}
