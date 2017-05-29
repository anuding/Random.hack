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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Windows.Storage.Streams;
//using System.Data.SQLite;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Emotion
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //readonly string _subscriptionKey;
        /*void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("D:\\Random.hack\\Random.hack\\Emotion\\Emotion\\Assets");
            m_dbConnection.Open();
        }*/

        public MainPage()
        {

            //set your key here
           // Console.Write("Enter image file path: ");
            string imageFilePath = ("d:\\1.jpg");//= Console.ReadLine();
            //

            //MakeAnalysisRequest(imageFilePath);
            //MakeAnalysisRequest(wangzhi);
            // Console.WriteLine("\n\n\nHit ENTER to exit...");
            //Console.ReadLine();
            // _subscriptionKey = "63682cf3bc7f424aba4c2b3634682204";
            this.InitializeComponent();
        }


        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


        static async Task<string> MakeAnalysisRequest(string wangzhi)
        {
            var client = new HttpClient();


            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "63682cf3bc7f424aba4c2b3634682204");

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "visualFeatures=Tags&language=en";
            string uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;
            HttpResponseMessage response;
            
            
            byte[] byteData = Encoding.UTF8.GetBytes("{\"url\":\"" + wangzhi + "\"}");
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                var r1 = await response.Content.ReadAsStringAsync();
                
                
                return await response.Content.ReadAsStringAsync();
            }
        }

        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            string wangzhi = Input.Text;

            //var file = await openPicker.PickSingleFileAsync();

            //if (file != null)
            {
                //await
              //  var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                var image = new BitmapImage();
               // imgBitmap = new BitmapImage();
                var uri = new System.Uri(wangzhi);
                image.UriSource = uri;
               // img.Source = imgBitmap;
                //image.SetSource(stream);
                ImageToAnalyze.Source = image;
            }
            
            ResultsTextBlock.Text = await MakeAnalysisRequest(wangzhi) + "\n\n ";
            
        }

    }
}
