using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;

namespace Navigateur_web
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HistoryViewModel _history;
        public MainWindow()
        {
            InitializeComponent();
            webView.NavigationStarting += EnsureHttps;

            _history = new HistoryViewModel();
            addressBar.ItemsSource = _history.URLs;
        }
        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            string uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                args.Cancel = true;
            }
        }
        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                if (!addressBar.Text.StartsWith("https://"))
                {
                    addressBar.Text = "https://" + addressBar.Text;
                }
                webView.CoreWebView2.Navigate(addressBar.Text);
            }
            _history.URLs.Add(addressBar.Text);
        }

        public class History
        {
            public History()
            {
                URLs = new ObservableCollection<string>();
            }
            public DateTime Time { get; set; }

            private string _URL;
            public string URL
            {
                get { return _URL; }
                set { _URL = value; }
            }

            private ObservableCollection<string> _URLs;

            public ObservableCollection<string> URLs
            {
                get { return _URLs; }
                set { _URLs = value; }
            }
        }

        public class HistoryViewModel
        {
            private History _history;

            public History history
            {
                get { return _history; }
                set { _history = value; }
            }
            public HistoryViewModel()
            {
                _history = new History();
            }


            public ObservableCollection<string> URLs
            {
                get { return history.URLs; }
                set { history.URLs = value; }
            }
        }

    }
}
