using System;
using System.Threading.Tasks;
using ZXing;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Media;
using Windows.UI.Xaml.Controls;
using Windows.System.Display;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.Devices.Enumeration;
using System.Collections.Generic;

namespace QRScannerUWP
{
    public enum QR_RESULT: uint
    {
        QR_SUCCESS,
        // Load frame from image
        QR_NO_FILE_LOADED,
        // Load frame from camera
        QR_CAMERA_NOT_WORKING,
        // Start Camera
        QR_CAMERA_INIT_FAIL,
        QR_CAMERA_UNAUTHORIZED,
        QR_CAMERA_DISPLAY_FAIL,
        // Detect QR
        QR_BITMAP_NULL,
        QR_DETECT_NONE,
        // Stop Camera
        QR_CAMERA_ALREADY_OFF
    }
    
    public class QRCodeModel
    {
        private string QRCodeText { get; set; }

        private SoftwareBitmap softwareBitmap { get; set; }

        private MediaCapture mediaCapture;

        public List<string> listDeviceID;

        private CaptureElement element;

        private string videoDeviceId;

        public MediaCapture getMediaCapture()
        {
            return this.mediaCapture;
        }

        public string getQRCodeText()
        {
            return QRCodeText;
        }

        public SoftwareBitmap getSoftwareBitmap()
        {
            return this.softwareBitmap;
        }

        private QRCodeModel()
        {
            QRCodeText = "";
            mediaCapture = null;
            softwareBitmap = null;
            listDeviceID = null;
            element = null;
            videoDeviceId = null;
        }

        // Singleton pattern
        private static QRCodeModel instance = null;
        public static QRCodeModel GetInstance()
        {
            if (instance == null)
            {
                instance = new QRCodeModel();
            }
            return instance;
        }

        public async Task MessageBox(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        public async Task<QR_RESULT> LoadFrameFromImgAsync()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;

            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile is null)
            {
                // The user cancelled the picking operation
                return QR_RESULT.QR_NO_FILE_LOADED;
            }

            using (IRandomAccessStream stream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            }

            return QR_RESULT.QR_SUCCESS;
        }

        // input: this.softwareBitmap
        // output: this.QRCodeText
        public QR_RESULT DetectQR()
        {
            if (this.softwareBitmap is null) return QR_RESULT.QR_BITMAP_NULL;

            var barcodeReader = new BarcodeReader();

            Result barcodeResult = barcodeReader.Decode(softwareBitmap);

            if (barcodeResult is null)
            {
                this.QRCodeText = null;
                return QR_RESULT.QR_DETECT_NONE;
            }
            else
            {
                this.QRCodeText = barcodeResult.Text;
                return QR_RESULT.QR_SUCCESS;
            }
        }

        // input: this.mediaCapture
        // output: this.softwareBitmap
        public async Task<QR_RESULT> LoadFrameFromCameraAsync()
        {
            try
            {
                if(mediaCapture is null)
                {
                    return QR_RESULT.QR_CAMERA_NOT_WORKING;
                }
                // Get information about the preview
                var previewProperties = this.mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview)
                    as VideoEncodingProperties;
                // Create a video frame in the desired format for the preview frame
                VideoFrame videoFrame = new VideoFrame(BitmapPixelFormat.Bgra8, (int)previewProperties.Width, (int)previewProperties.Height);
                //
                VideoFrame previewFrame = await this.mediaCapture.GetPreviewFrameAsync(videoFrame);
                this.softwareBitmap = previewFrame.SoftwareBitmap;
                return QR_RESULT.QR_SUCCESS;
            }
            catch(Exception)
            {
                return QR_RESULT.QR_CAMERA_NOT_WORKING;
            }
        }

        // Init media capture by default setting
        // Update camera frame on preview panel
        // Ref: https://docs.microsoft.com/en-us/windows/uwp/audio-video-camera/simple-camera-preview-access
        public async Task<QR_RESULT> DefaultCameraUpdateAsync(CaptureElement element)
        {
            this.element = element;
            //Initialize the capture device
            try
            {

                mediaCapture = new MediaCapture();
                await mediaCapture.InitializeAsync();

                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                // The app was denied access to the camera
                return QR_RESULT.QR_CAMERA_UNAUTHORIZED;
            }

            try
            {
                // Connect the MediaCapture to the CaptureElement by setting the Source property. 
                this.element.Source = mediaCapture;
                // Start the preview
                await mediaCapture.StartPreviewAsync();
            }
            catch (Exception)
            {
                return QR_RESULT.QR_CAMERA_DISPLAY_FAIL;
            }

            return QR_RESULT.QR_SUCCESS;
        }

        // Initialize media capture by device ID
        // Update camera frame on preview panel
        public async Task<QR_RESULT> CameraUpdateAsync(CaptureElement element, string videoDeviceId = null)
        {
            this.element = element;
            if(videoDeviceId != null)
            {
                this.videoDeviceId = videoDeviceId;
            }
            mediaCapture = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings
            {
                VideoDeviceId = this.videoDeviceId,
                StreamingCaptureMode = StreamingCaptureMode.Video,
                SharingMode = MediaCaptureSharingMode.SharedReadOnly,
            };
            try
            {
                await mediaCapture.InitializeAsync(settings);
            }
            catch(Exception)
            {
                return QR_RESULT.QR_CAMERA_INIT_FAIL;
            }
            try
            {
                // Connect the MediaCapture to the CaptureElement by setting the Source property. 
                element.Source = mediaCapture;
                // Start the preview
                await mediaCapture.StartPreviewAsync();
            }
            catch (Exception)
            {
                return QR_RESULT.QR_CAMERA_DISPLAY_FAIL;
            }

            return QR_RESULT.QR_SUCCESS;
        }

        // Release media capture
        public async Task<QR_RESULT> CleanupCameraAsync()
        {
            if (this.mediaCapture != null && this.element != null)
            {
                try
                {
                    //If the camera is currently previewing, call StopPreviewAsync to stop the preview stream.
                    await mediaCapture.StopPreviewAsync();
                }
                catch
                {
                    // An exception will be thrown if you call StopPreviewAsync while the preview is not running.
                    return QR_RESULT.QR_CAMERA_ALREADY_OFF;
                }
                // Set the Source property of the CaptureElement to null.
                element.Source = null;
                // Call the MediaCapture object's Dispose method to release the object.
                mediaCapture.Dispose();
                mediaCapture = null;
                return QR_RESULT.QR_SUCCESS;
            }
            return QR_RESULT.QR_CAMERA_ALREADY_OFF;
        }

        // List all available video capture devices
        // Ref: https://docs.microsoft.com/en-us/uwp/api/windows.devices.enumeration.devicewatcher?view=winrt-19041
        public async Task<QR_RESULT> GetCameraListAsync()
        {
            DeviceWatcher watcher = DeviceInformation.CreateWatcher(DeviceClass.VideoCapture);
            listDeviceID = new List<string>();
            var completionSource = new TaskCompletionSource<QR_RESULT>();
            // When a device is discovered by the watcher
            watcher.Added += (DeviceWatcher sender, DeviceInformation device) =>
            {
                listDeviceID.Add(device.Id);
            };
            // When the enumeration of devices completes
            watcher.EnumerationCompleted += (DeviceWatcher sender, object args) =>
            {
                // This sets the result to QR_RESULT if no task was able to produce a device.
                completionSource.TrySetResult(QR_RESULT.QR_SUCCESS);
            };
            // Start watching
            watcher.Start();
            // Wait for enumeration to complete or for a device to be found.
            QR_RESULT result = await completionSource.Task;
            // Stop watching
            watcher.Stop();
            return result;
        }
    }
}
