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
        List<PrintSample2> printSample2s;
        PageController pageController;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            printController = new PrintController(this);
            printController.RegisterForPrinting();
        }


        private async void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            // Write dummy data to pages
            FillPrintSample2WithDummyData();
            // Add pages to print list
            List<Page> printPages = new List<Page>();
            for(int i = 0; i < printSample2s.Count(); i++)
            {
                printPages.Add(printSample2s[i]);
            }
            printController.SetPrintPages(printPages);
            // Show print preview
            await printController.ShowPrintUIAsync();
        }

        private void FillPrintSample2WithDummyData()
        {
            printSample2s = new List<PrintSample2>();
            DummyData dummyData = new DummyData();
            for (int i = 0; i < dummyData.crewDatas.Count(); i++)
            {
                int position = i % 4;
                if (position == 0)
                {
                    pageController = new PageController();
                    printSample2 = new PrintSample2();
                    pageController.SetPage(printSample2);
                    printSample2s.Add(printSample2);
                }
                TextBlock textBlock = pageController.GetTextBlockByName("TextCrewID" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "乗務員ID:" + dummyData.crewDatas[i].crewID);
                textBlock = pageController.GetTextBlockByName("TextCrewName" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "乗務員名:" + dummyData.crewDatas[i].crewName);
                textBlock = pageController.GetTextBlockByName("TextDepartment" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "所属:" + dummyData.crewDatas[i].department);
                textBlock = pageController.GetTextBlockByName("TextCarNo" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "車番:" + dummyData.crewDatas[i].carNo);
                textBlock = pageController.GetTextBlockByName("TextJudgment" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "判定:" + dummyData.crewDatas[i].judgment);
                textBlock = pageController.GetTextBlockByName("TextMeasureValue" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "測定値:" + dummyData.crewDatas[i].measureValue);
                textBlock = pageController.GetTextBlockByName("TextPhotoTime" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "撮影時間:" + dummyData.crewDatas[i].photoTime);
                textBlock = pageController.GetTextBlockByName("TextLongLatitude" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "緯度、軽度:" + dummyData.crewDatas[i].longLatitude);
                textBlock = pageController.GetTextBlockByName("TextMemo" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "メモ:" + dummyData.crewDatas[i].memo);
            }

        }
    }
}
