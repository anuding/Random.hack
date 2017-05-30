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
using Windows.UI.Popups;
using Windows.Graphics.Display;
using System.Net;
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

          //这是干嘛的我忘了(⊙——⊙)

            string imageFilePath = ("d:\\1.jpg");
            this.InitializeComponent();
        }
                //转流
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        //召唤图片分析
        static async Task<string> MakeAnalysisRequest(string wangzhi)
        {
            var client = new HttpClient();


            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "63682cf3bc7f424aba4c2b3634682204");

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "visualFeatures=Tags&language=en";
            //endpoint一定要和key配套
            string uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;
            HttpResponseMessage response;
            
            
            byte[] byteData = Encoding.UTF8.GetBytes("{\"url\":\"" + wangzhi + "\"}");
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
             
                return await response.Content.ReadAsStringAsync();
            }
        }


        //召唤文字分析
        static async Task<string> TextAnalysisRequest(string wenzi)
        {
            ////////////////////////
            //读取分析文字
            var client1 = new HttpClient();
            //var queryString = HttpUtility.ParseQueryString(string.Empty);
            //var queryString = WebUtility.UrlEncode("{content:天门中断楚江开，碧水东流至此回。两岸青山相对出，孤帆一片日边来, tag:[mountains,iver]}");//i have a pen, you have an apple
            //string poe = response.Content.ReadAsStringAsync();
            var queryString = WebUtility.UrlEncode(string.Empty);
            // Request headers
            client1.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "608d39205c154d02a1599e77b637f21b");

            var uri1 = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases?" + queryString;

            HttpResponseMessage response1;

            // Request body
            /* byte[] byteData1 = Encoding.UTF8.GetBytes("{\"documents\":[" +
         "{\"id\":\"1\",\"text\":\"" + "i have a pen, you have an apple" + "\"},]}"););// */
            byte[] byteData1 = Encoding.UTF8.GetBytes("{\"documents\":[" +
        "{\"id\":\"1\",\"text\":\"" + wenzi + "\"},]}");

            using (var content1 = new ByteArrayContent(byteData1))
            {
                content1.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response1 = await client1.PostAsync(uri1, content1);
                return await response1.Content.ReadAsStringAsync();
            }
            ////////////////////////////*/
        }



        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            //读取输入网址
            string wangzhi = Input.Text;
            //显示图片
            var image = new BitmapImage();
            var uri = new System.Uri(wangzhi);
            image.UriSource = uri;
            ImageToAnalyze.Source = image;

            //显示分析结果
            string pic_fea = await MakeAnalysisRequest(wangzhi);
                //await TextAnalysisRequest("i have a pen, you have an apple") + "\n\n ";
            string poem_key = TextAnalysisRequest("i have a pen, you have an apple") + "\n\n ";
            ResultsTextBlock.Text = pic_fea + poem_key;





            //在这里更改要加在图片上的诗句
            //if(pic_fea)
            /*for(int i = 0; i < pic_fea.Length; i++)
            {
                if(pic_fea[i]=="s")
            }*/
            //string a = "china";
            string snow = "snow";
            if (pic_fea.IndexOf(snow) > -1)
            {
                Poems.Text = "白雪却嫌春色晚，故穿庭树作飞花"; //包含指定的字符串，执行相应的代码
            }

            string moon = "moon";
            if (pic_fea.IndexOf(moon) > -1)
            {
                Poems.Text = "青女素娥俱耐冷，月中霜里斗婵娟"; //包含指定的字符串，执行相应的代码
            }//会当凌绝顶，一览众山小
            string mountain = "mountain";
            if (pic_fea.IndexOf(mountain) > -1)
            {
                Poems.Text = "会当凌绝顶，一览众山小"; //包含指定的字符串，执行相应的代码
            }
            //Poems.Text = "两岸青山相对出，孤帆一片日边来";
            //////////////////////////////////
            //以下是抄来的截图保存代码
            string desiredName = DateTime.Now.Ticks + ".jpg";
            StorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            StorageFolder folder = await applicationFolder.CreateFolderAsync("Pic", CreationCollisionOption.OpenIfExists);
            StorageFile saveFile = await folder.CreateFileAsync(desiredName, CreationCollisionOption.OpenIfExists);
            RenderTargetBitmap bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(PicGrid);
            var pixelBuffer = await bitmap.GetPixelsAsync();
            using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                     (uint)bitmap.PixelWidth,
                     (uint)bitmap.PixelHeight,
                     DisplayInformation.GetForCurrentView().LogicalDpi,
                     DisplayInformation.GetForCurrentView().LogicalDpi,
                     pixelBuffer.ToArray());
                await encoder.FlushAsync();
            }
            await new MessageDialog("已保存成功至C:\\Users\anuding\\AppData\\Local\\Packages\\2a0f1772-af4f-4ee4-a3e0-a9d62c687795_654wjja6gg34r\\LocalState\\Pic").ShowAsync();
            ////////////////////////////

        }

    }
}
