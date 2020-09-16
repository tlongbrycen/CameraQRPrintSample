using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace CameraQRPrintSample
{
    class PrintController
    {
        private PrintDocument printDocument;
        private IPrintDocumentSource printDocumentSource;
        private List<UIElement> printPreviewPages;
        private List<Page> printPages;
        private Page scenarioPage;

        private event EventHandler PreviewPagesCreated;

        public PrintController(Page scenarioPage)
        {
            this.scenarioPage = scenarioPage;
            printPreviewPages = new List<UIElement>();
        }

        public void SetPrintPages(List<Page> printPages)
        {
            this.printPages = printPages;
        }

        public void RegisterForPrinting()
        {
            printDocument = new PrintDocument();
            printDocumentSource = printDocument.DocumentSource;
            printDocument.Paginate += CreatePrintPreviewPages;
            printDocument.GetPreviewPage += GetPrintPreviewPage;
            printDocument.AddPages += AddPrintPages;

            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;
        }

        public void UnregisterForPrinting()
        {
            if (printDocument == null)
            {
                return;
            }
            printDocument.Paginate -= CreatePrintPreviewPages;
            printDocument.GetPreviewPage -= GetPrintPreviewPage;
            printDocument.AddPages -= AddPrintPages;
            // Remove the handler for printing initialization.
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("Printing", sourceRequested =>
            {
                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    // Notify the user when the print operation fails.
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        await scenarioPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            //MainPage.Current.NotifyUser("Failed to print.", NotifyType.ErrorMessage);
                        });
                    }
                };

                sourceRequested.SetSource(printDocumentSource);
            });
        }

        private void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            // Loop over all of the preview pages and add each one to  add each page to be printied
            for (int i = 0; i < printPreviewPages.Count; i++)
            {
                // We should have all pages ready at this point...
                printDocument.AddPage(printPreviewPages[i]);
            }
            PrintDocument printDoc = (PrintDocument)sender;
            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        private void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, printPreviewPages[e.PageNumber - 1]);
        }

        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Clear the cache of preview pages
            printPreviewPages.Clear();

            if (PreviewPagesCreated != null)
            {
                PreviewPagesCreated.Invoke(printPreviewPages, null);
            }
            // Add pages to the page preview collection
            if (!(printPages is null))
            {
                for (int i = 0; i < printPages.Count(); i++)
                {
                    printPreviewPages.Add(printPages[i]);
                }
            }
            PrintDocument printDoc = (PrintDocument)sender;
            // Report the number of preview pages created
            printDoc.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Intermediate);
        }

        public async Task ShowPrintUIAsync()
        {
            // Catch and print out any errors reported
            try
            {
                await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception)
            {

            }
        }
    }
}
