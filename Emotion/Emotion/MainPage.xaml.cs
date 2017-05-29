using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Windows.Graphics.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Emotion
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly string _subscriptionKey;

        public MainPage()
        {
            //set your key here
            _subscriptionKey = "63682cf3bc7f424aba4c2b3634682204";
            this.InitializeComponent();
        }

        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".bmp");
            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                //await
                ShowPreviewAndAnalyzeImage(file);
            }
        }

        private async Task ShowPreviewAndAnalyzeImage(StorageFile file)
        {
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            var image = new BitmapImage();

            image.SetSource(stream);

            //imageView.Source = image;
            //preview image
            //var bitmap = await LoadImage(file);
            //var bitmap = new RenderTargetBitmap();
            //SharedStorageAccessManager.AddFile(file);
            ImageToAnalyze.Source = image;

            //analyze image
            var results = await AnalyzeImage(file);

            //"fr", "ru", "it", "hu", "ja", etc...
            //var ocrResults = await AnalyzeImageForText(file, "en");

            //parse result
            //ResultsTextBlock.Text = ParseResult(results) + "\n\n " + ParseOCRResults(ocrResults);
            ResultsTextBlock.Text = "efewfewfew" + results + "\n\n ";// + ocrResults;
        }

        private async Task<AnalysisResult> AnalyzeImage(StorageFile file)
        {

            VisionServiceClient VisionServiceClient = new VisionServiceClient(_subscriptionKey);

            using (Stream imageFileStream = await file.OpenStreamForReadAsync())
            {
                // Analyze the image for all visual features
                VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories
            , VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType
            , VisualFeature.Tags };
                AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageFileStream, visualFeatures);
                return analysisResult;
            }
        }

        private async Task<OcrResults> AnalyzeImageForText(StorageFile file, string language)
        {
            //language = "fr", "ru", "it", "hu", "ja", etc...
            VisionServiceClient VisionServiceClient = new VisionServiceClient(_subscriptionKey);
            using (Stream imageFileStream = await file.OpenStreamForReadAsync())
            {
                OcrResults ocrResult = await VisionServiceClient.RecognizeTextAsync(imageFileStream, language);
                return ocrResult;
            }
        }


        /*public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Hello, World!");
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }*/
    }
}
