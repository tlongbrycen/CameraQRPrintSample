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

        PrintController printController;
        PrintSample2 printSample2;
        PageController pageController;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            printController = new PrintController(this);
            printController.RegisterForPrinting();
            pageController = new PageController();
        }


        private async void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            // Add page to print list
            printSample2 = new PrintSample2();
            List<Page> printPages = new List<Page>();
            printPages.Add(printSample2);
            printController.SetPrintPages(printPages);
            // Show print preview
            await printController.ShowPrintUIAsync();
        }
    }
}
