using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media.Imaging;

namespace CameraQRPrintSample
{
    class PageController
    {
        private Page page;

        public void SetPage(Page page)
        {
            this.page = page;
        }

        public TextBlock GetTextBlockByName(string name)
        {
            TextBlock textBlock = (TextBlock)page.FindName(name);
            return textBlock;
        }

        public void FillTextBlockWithText(TextBlock textBlock, string textToWrite, int fontSize = 11, Windows.UI.Text.FontStyle fontStyle = Windows.UI.Text.FontStyle.Normal)
        {
            Run textRun = new Run();
            textRun.Text = textToWrite;
            textRun.FontSize = fontSize;
            textRun.FontStyle = fontStyle;
            textBlock.Inlines.Clear();
            textBlock.Inlines.Add(textRun);
        }

        public Image GetImageByName(string name)
        {
            Image img = (Image)page.FindName(name);
            return img;
        }

        public async void SetImageSource(Image img, string filePath = "ms-appx:///Assets/StoreLogo.png")
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(filePath));
            var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(fileStream);
            img.Source = bitmapImage;
        }
    }
}
