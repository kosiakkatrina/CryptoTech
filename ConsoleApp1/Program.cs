using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;



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
            Thread _responseThread = new Thread(ResponseThread);
            _responseThread.Start(); // start the response thread
        }
        
        static void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string text;
                using (var reader = new StreamReader(request.InputStream,
                    request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                Console.Write(text);

                
                var client = new WebClient();
                
                var body = client.DownloadString("https://api.airtable.com/v0/appxiufWMNuWFhrKN/Marketplace?api_key=keyviCCmCsS89byTt");
                
                Console.Write(body);


                byte[] _responseArray = Encoding.UTF8.GetBytes(body); // get the bytes to response
                response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                response.KeepAlive = false; // set the KeepAlive bool to false
                response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");
            }
        }
    }
}