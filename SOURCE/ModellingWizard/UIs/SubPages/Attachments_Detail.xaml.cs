// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using ModellingWizard.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ModellingWizard.UIs.SubPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Attachments_Detail : Page
    {
        public Attachments_Detail()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) 
        {
            base.OnNavigatedTo(e);
            bool showError = false;
            string parameter = e.Parameter as string;
            if (parameter == null || parameter == "")
            {
                /* Stuff for empty */

                TextBlock textBlock = new()
                {
                    Text = "No attachment selected."
                };
                AttachmentGrid.Children.Add(textBlock);
            }
            else
            {
                var attachment = Instances.Attachments.Find(a => a.UUID == parameter);
                if (attachment == null)
                {
                    showError = true;
                    return;
                }
                    
                try
                {
                    if(attachment.Title.EndsWith("png") || attachment.Title.EndsWith("jpeg") || attachment.Title.EndsWith("jpg"))
                    {
                        showError = true;
                        Image image = new()
                        {
                           // Source = LoadImageFromString(attachment.Base64Content)
                        };

                        AttachmentGrid.Children.Add(image);
                    }
                    else if (attachment.Title.EndsWith("pdf"))
                    {
                        showError = true;
                    }
                    else if (attachment.Title.EndsWith("txt"))
                    {
                        TextBlock tb = new()
                        {
                            Text = Encoding.UTF8.GetString(Convert.FromBase64String(attachment.Base64Content))
                        };
                        AttachmentGrid.Children.Add(tb);
                    }
                    else
                    {
                        showError = true;
                    }
                    /*
                    byte[] pdfBytes = Convert.FromBase64String(attachment.Base64Content);
                    string base64HtmlContent = Convert.ToBase64String(pdfBytes);
                    string htmlContent = $"<html><body><embed src=\"data:application/pdf;base64,{base64HtmlContent}\" type=\"application/pdf\" width=\"100%\" height=\"100%\"></body></html>";

                    WebView2 webView = new();
                    webView.NavigateToString(htmlContent);
                    AttachmentGrid.Children.Add(webView);
                    */

                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    showError = true;
                }
                
            }
            if(showError)
            {
                /* Stuff for empty */

                TextBlock textBlock = new()
                {
                    Text = "File format is not supported."
                };
                AttachmentGrid.Children.Add(textBlock);
            }
        }


    }
}
