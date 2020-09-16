﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

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

        public void FillTextBlockWithText(TextBlock textBlock, string textToWrite)
        {
            Run textRun = new Run();
            textRun.Text = textToWrite;
            textBlock.Inlines.Clear();
            textBlock.Inlines.Add(textRun);
        }
    }
}
