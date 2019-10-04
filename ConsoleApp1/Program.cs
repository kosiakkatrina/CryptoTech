using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
//using System.Web.Script.Serialization;
namespace ConsoleApp1
{
    class Program
    {
        
        static HttpListener _httpListener = new HttpListener();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add("http://localhost:5000/"); // add prefix "http://localhost:5000/"
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            Thread responseThread = new Thread(ResponseThread);
            responseThread.Start(); // start the response thread
        }
        
        static void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                /*Shows the token
                string text;
                using (var reader = new StreamReader(request.InputStream,
                    request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                Console.Write(text);*/

                var client = new WebClient();
                var body = client.DownloadString(
                    "https://api.airtable.com/v0/appxiufWMNuWFhrKN/Marketplace?fields%5B%5D=Name&api_key=API_KEY");

               // var better_string = Formatted(body);
                var formatted_body = "{\"blocks\": [{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"Stuff Written down\"} },{\"type\": \"image\",\"title\": {\"type\": \"plain_text\",\"text\": \"Example Image\",\"emoji\": true}, \"image_url\": \"https://api.slack.com/img/blocks/bkb_template_images/goldengate.png\",\"alt_text\": \"Example Image\"}, {\"type\": \"image\",\"title\": {\"type\": \"plain_text\",\"text\": \"image1\",\"emoji\": true}, \"image_url\": \"https://api.slack.com/img/blocks/bkb_template_images/beagle.png\",\"alt_text\": \"image1\"}] }";
                //formatted_body = formatted_body.Replace("blocks", "blo");

                var last_string = Format(body);
            //Console.Write(body);
                byte[] responseArray = Encoding.UTF8.GetBytes(formatted_body); // get the bytes to response
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                response.KeepAlive = false; // set the KeepAlive bool to false
                response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");
            }

           // static JObject Formatted(String str)
          //  {
                /*JObject json = JObject.Parse(str);
                json.Property("createdTime").RemoveAll();
                string result = new JavaScriptSerializer().Serialize(json.Data);
                json*/
                //return json;
            //}
            
            static String Format(String str)
            {
                return str;
            }


        }
    }
}