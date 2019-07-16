using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace InvoiceOcr
{
    class Program
    {

        private const String host = "https://nvoiceocr.market.alicloudapi.com";
        private const String path = "/taxinvoice";
        private const String method = "POST";
        private const String appcode = "1a051ed6e704403b8ce9a6bd0487c68a";

        static void Main(string[] args)
        {
            String querys = "";
            //String bodys = "image=" + decodeImageToBase64(@"C:\InvoiceTestPictures\百旺.JPG");
            String bodys = "image=" + decodeImageToBase64(@"C:\InvoiceTestPictures\航天.JPG");
            //" http://img3.fegine.com/image/taxinvoice.jpg";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            //根据API的要求，定义相对应的Content-Type
            httpRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(httpResponse.Method);
            Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            Console.WriteLine(reader.ReadToEnd());
            Console.WriteLine("\n");

            Console.ReadLine();

        }


        #region 图片转base64
        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="path">图片路径</param><br>        /// <returns>返回一个base64字符串</returns>
        public static string decodeImageToBase64(string path)
        {
            string base64str = "";

            try
            {                
                //读图片转为Base64String
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                bmp.Dispose();
                base64str = Convert.ToBase64String(arr);
            }
            catch (Exception e)
            {
                string mss = e.Message;
            }
            return "data:image/jpg;base64," + base64str;
        }
        #endregion

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}


