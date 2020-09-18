using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        List<string> listDeviceID;

        public CameraSelect()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetCameraListAsync();
            if (listDeviceID != null)
            {
                ComboBoxCamera.Items.Add(listDeviceID);
            }
        }

        private void ComboBoxCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // List all available video capture devices
        // Ref: https://docs.microsoft.com/en-us/uwp/api/windows.devices.enumeration.devicewatcher?view=winrt-19041
        public async Task<bool> GetCameraListAsync()
        {
            DeviceWatcher watcher = DeviceInformation.CreateWatcher(DeviceClass.VideoCapture);
            listDeviceID = new List<string>();
            var completionSource = new TaskCompletionSource<bool>();
            // When a device is discovered by the watcher
            watcher.Added += (DeviceWatcher sender, DeviceInformation device) =>
            {
                listDeviceID.Add(device.Id);
            };
            // When the enumeration of devices completes
            watcher.EnumerationCompleted += (DeviceWatcher sender, object args) =>
            {
                // This sets the result to QR_RESULT if no task was able to produce a device.
                completionSource.TrySetResult(true);
            };
            // Start watching
            watcher.Start();
            // Wait for enumeration to complete or for a device to be found.
            bool result = await completionSource.Task;
            // Stop watching
            watcher.Stop();
            return result;
        }
    }
}
