using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LaboratoryApp.Support.Controls
{
    public class BindableWebView2 : WebView2
    {
        public static readonly DependencyProperty SourceUriProperty =
            DependencyProperty.Register(
                nameof(BindableSource),
                typeof(Uri),
                typeof(BindableWebView2),
                new PropertyMetadata(null, OnBindableSourceChanged));

        public Uri BindableSource
        {
            get => (Uri)GetValue(SourceUriProperty);
            set => SetValue(SourceUriProperty, value);
        }

        private static async void OnBindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableWebView2 webView2)
            {
                string newPath = e.NewValue as string;

                if (!string.IsNullOrEmpty(newPath))
                {
                    if(webView2.CoreWebView2 == null)
                    {
                        await webView2.EnsureCoreWebView2Async();
                    }

                    if (Uri.TryCreate(newPath, UriKind.Absolute, out Uri uri))
                    {
                        webView2.Source = uri;
                    }
                }
            }
        }
    }
}
