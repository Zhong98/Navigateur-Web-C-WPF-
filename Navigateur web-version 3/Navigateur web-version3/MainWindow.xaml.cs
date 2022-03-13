using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Navigateur_web_version3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int n;
        private readonly ObservableCollection<HistoryViewModel> _histories;
        public MainWindow()
        {
            InitializeComponent();
            webView.NavigationStarting += EnsureHttps;

            _histories = new ObservableCollection<HistoryViewModel>();

            HistoryViewModel h0 = new HistoryViewModel();
            h0.currentURL = webView.Source.ToString();
            _histories.Add(h0);
            n = 1;
            addressBar.ItemsSource = _histories;
            addressBar.Text = webView.Source.ToString();
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
            HistoryViewModel h = new HistoryViewModel();
            if (webView != null && webView.CoreWebView2 != null)
            {
                if (!addressBar.Text.StartsWith("https://"))
                {
                    addressBar.Text = "https://" + addressBar.Text;
                }
                webView.CoreWebView2.Navigate(addressBar.Text);
                h.currentURL = addressBar.Text;
            }
            _histories.Add(h);
            n++;
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

            public string currentURL
            {
                get { return history.URL; }
                set { history.URL = value; }
            }


        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            int previousURL = n - 2;
            if (previousURL >= 0)
            {
                webView.CoreWebView2.Navigate(_histories[previousURL].currentURL);
                addressBar.Text = _histories[previousURL].currentURL;
            }
            n--;
        }

        private void ButtonFix_Click(object sender, RoutedEventArgs e)
        {
            if (_histories[0].currentURL!= addressBar.Text)
            {
                HistoryViewModel historyView = new HistoryViewModel();
                historyView.currentURL = addressBar.Text;
                _histories.Insert(0, historyView);
            } 
        }
    }
}
