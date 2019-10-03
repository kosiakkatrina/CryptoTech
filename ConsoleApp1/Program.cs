using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;


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
                    "https://api.airtable.com/v0/appxiufWMNuWFhrKN/Marketplace?api_key=keyviCCmCsS89byTt");
                var formatted_body = "{\"blocks\": [{ \"type\": \"section\",\"text\": { \"type\": \"mrkdwn\",\"text\": \"Stuff Written down\"}}]}";
                
            //Console.Write(body);
                byte[] responseArray = Encoding.UTF8.GetBytes(formatted_body); // get the bytes to response
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                response.KeepAlive = false; // set the KeepAlive bool to false
                response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");
            }
            
            
        }
    }
}