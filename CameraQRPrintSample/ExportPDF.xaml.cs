using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CameraQRPrintSample
{
    public sealed partial class ExportPDF:Page
    {
        // For print
        PrintController printController;
        PrintSample2 printSample2;
        List<PrintSample2> printSample2s;
        PrintSample1 printSample1;
        List<PrintSample1> printSample1s;
        PageController pageController;
        public ExportPDF()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            printController = new PrintController(this);
            // This try-catch block is indispensable because
            // When switching among tabs in menu
            // the RegisterForPrinting() runs one time
            // Redo the RegisterForPrinting() raises an exception
            try
            {
                printController.RegisterForPrinting();
            }
            catch(Exception)
            {

            }
        }


        private async void ButtonPrint2_Click(object sender, RoutedEventArgs e)
        {
            // Write dummy data to pages
            FillPrintSample2WithDummyData();
            // Add pages to print list
            List<Page> printPages = new List<Page>();
            for (int i = 0; i < printSample2s.Count(); i++)
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
                int position = i % PrintSample2.LINE_NUM;
                if (position == 0)
                {
                    pageController = new PageController();
                    printSample2 = new PrintSample2();
                    pageController.SetPage(printSample2);
                    printSample2s.Add(printSample2);
                }
                TextBlock textBlock = pageController.GetTextBlockByName("TextDate");
                pageController.FillTextBlockWithText(textBlock, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                textBlock = pageController.GetTextBlockByName("TextPageNumber");
                pageController.FillTextBlockWithText(textBlock, printSample2s.Count().ToString());

                Image img = pageController.GetImageByName("Photo" + position.ToString());
                pageController.SetImageSource(img, "ms-appx:///Assets/StoreLogo.png");
                textBlock = pageController.GetTextBlockByName("TextCrewID" + position.ToString());
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
                pageController.FillTextBlockWithText(textBlock, "緯度、経度:" + dummyData.crewDatas[i].longLatitude);
                textBlock = pageController.GetTextBlockByName("TextMemo" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, "メモ:" + dummyData.crewDatas[i].memo);
            }

        }

        private async void ButtonPrint1_Click(object sender, RoutedEventArgs e)
        {
            // Write dummy data to pages
            FillPrintSample1WithDummyData();
            // Add pages to print list
            List<Page> printPages = new List<Page>();
            for (int i = 0; i < printSample1s.Count(); i++)
            {
                printPages.Add(printSample1s[i]);
            }
            printController.SetPrintPages(printPages);
            // Show print preview
            await printController.ShowPrintUIAsync();
        }

        private void FillPrintSample1WithDummyData()
        {
            printSample1s = new List<PrintSample1>();
            DummyData dummyData = new DummyData();
            for (int i = 0; i < dummyData.crewDatas.Count(); i++)
            {
                int position = i % PrintSample1.LINE_NUM;
                if (position == 0)
                {
                    pageController = new PageController();
                    printSample1 = new PrintSample1();
                    pageController.SetPage(printSample1);
                    printSample1s.Add(printSample1);
                }
                TextBlock textBlock = pageController.GetTextBlockByName("TextDate");
                pageController.FillTextBlockWithText(textBlock, DateTime.Now.ToString());
                textBlock = pageController.GetTextBlockByName("TextPageNumber");
                pageController.FillTextBlockWithText(textBlock, printSample1s.Count().ToString());

                textBlock = pageController.GetTextBlockByName("TextJudgment" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].judgment);

                textBlock = pageController.GetTextBlockByName("TextMeasureValue" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].measureValue);

                textBlock = pageController.GetTextBlockByName("TextDevice" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].device);

                textBlock = pageController.GetTextBlockByName("TextPhotoTime" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].photoTime);

                textBlock = pageController.GetTextBlockByName("TextOutReturn" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].outReturn);

                textBlock = pageController.GetTextBlockByName("TextCarNo" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].carNo);

                textBlock = pageController.GetTextBlockByName("TextDepartment" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].department);

                textBlock = pageController.GetTextBlockByName("TextCrewID" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].crewID);

                textBlock = pageController.GetTextBlockByName("TextCrewName" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].crewName);

                textBlock = pageController.GetTextBlockByName("TextTitleLongLatitude" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].longLatitude);

                textBlock = pageController.GetTextBlockByName("TextMemo" + position.ToString());
                pageController.FillTextBlockWithText(textBlock, dummyData.crewDatas[i].memo);

            }
        }
    }
}
