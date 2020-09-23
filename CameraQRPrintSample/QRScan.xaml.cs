using QRScannerUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CameraQRPrintSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QRScan : Page
    {
        private QRCodeModel qRCodeModel;
        private Thread threadScanQR;
        private bool isScanningQR;

        public QRScan()
        {
            this.InitializeComponent();
            qRCodeModel = QRCodeModel.GetInstance();
            threadScanQR = null;
        }

        /*protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }*/

        private void StartScanQR()
        {
            if (threadScanQR == null)
            {
                isScanningQR = true;
                threadScanQR = new Thread(ScanQR);
                threadScanQR.Start();
            }
            else
            {
                
            }
        }

        void EndScanQR()
        {
            isScanningQR = false;
            threadScanQR = null;
        }

        private async void ScanQR()
        {
            QR_RESULT rst;
            while (isScanningQR)
            {
                rst = await qRCodeModel.LoadFrameFromCameraAsync();
                rst = qRCodeModel.DetectQR();
                if (rst == QR_RESULT.QR_SUCCESS)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        this.QRText.Text = qRCodeModel.getQRCodeText();
                        EndScanQR();
                        await qRCodeModel.CleanupCameraAsync();
                    }
                    ).AsTask();
                }
            }
        }

        private async void ButtonScan_Click(object sender, RoutedEventArgs e)
        {
            await qRCodeModel.CleanupCameraAsync();
            await qRCodeModel.CameraUpdateAsync(this.cameraPreview, null);
            StartScanQR();
        }
    }
}
