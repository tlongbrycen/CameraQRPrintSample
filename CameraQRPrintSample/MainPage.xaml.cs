using QRScannerUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CameraQRPrintSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title="Camera Selection", ClassType=typeof(CameraSelect)},
            new Scenario() { Title="QR Scan", ClassType=typeof(QRScan)},
            new Scenario() { Title="Export PDF", ClassType=typeof(ExportPDF)},
        };

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var itemCollection = new List<Scenario>();
            int i = 1;
            foreach (Scenario s in scenarios)
            {
                itemCollection.Add(new Scenario { Title = $"{i++}) {s.Title}", ClassType = s.ClassType });
            }
            ListMenu.ItemsSource = itemCollection;
        }

        private void ListMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox scenarioListBox = sender as ListBox;
            Scenario s = scenarioListBox.SelectedItem as Scenario;
            if (s != null)
            {
                ScenarioFrame.Navigate(s.ClassType);
            }
        }
    }
}
