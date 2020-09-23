using QRScannerUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
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
    public sealed partial class CameraSelect : Page
    {
        QRCodeModel qRCodeModel;

        public CameraSelect()
        {
            this.InitializeComponent();
            qRCodeModel = QRCodeModel.GetInstance();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await qRCodeModel.GetCameraListAsync();
            if (qRCodeModel.listDeviceID != null)
            {
                for (int i = 0; i < qRCodeModel.listDeviceID.Count(); i++)
                {
                    ComboBoxCamera.Items.Add(qRCodeModel.listDeviceID[i]);
                }
            }
        }

        private async void ComboBoxCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await qRCodeModel.CleanupCameraAsync();
            string cameraID = (string)ComboBoxCamera.SelectedItem;
            await qRCodeModel.CameraUpdateAsync(cameraPreview, cameraID);
        }
    }
}
