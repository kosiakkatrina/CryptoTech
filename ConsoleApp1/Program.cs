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

                string text;
                using (var reader = new StreamReader(request.InputStream,
                    request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                Console.Write(text);
                
                var client = new WebClient();
                
                var body = client.DownloadString("https://api.airtable.com/v0/appyDi8LHMvNg2Yxg/Marketplace?api_key=" + Credentials.ApiKey);
                //var formatted_body = "{\"blocks\": [{ \"type\": \"section\",\"text\": { \"type\": \"mrkdwn\",\"text\": \"Random\"}}]}";
                
                var test_run = "{\"blocks\": [{\"type\": \"section\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Please select a workshop event for each week:\"}},{\"type\": \"divider\"},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*CRYPTOTECH*\nFriday, October 21 1:30-5:00pm\nLevel 2 - 4 O' Meara St.\n*4 guests*\"},\"accessory\": {\"type\": \"image\",\"image_url\": \"https://api.slack.com/img/blocks/bkb_template_images/notifications.png\",\"alt_text\": \"calendar thumbnail\"}},{\"type\": \"context\",\"elements\": [{\"type\": \"image\",\"image_url\": \"https://api.slack.com/img/blocks/bkb_template_images/notificationsWarningIcon.png\",\"alt_text\": \"notifications warning icon\"},{\"type\": \"mrkdwn\",\"text\": \"*Conflicts with Team Huddle Presentation 2:15-4:30pm*\"}]},{\"type\": \"divider\"},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Proposed Workshops:*\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Friday, October 21 - 2:00-3:00pm*\n*How do you 'measure' Agile-Lean maturity in a team and why would you?*\n@zelda\"},\"accessory\": {\"type\": \"button\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Choose\"},\"value\": \"click_me_123\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Friday, October 21 - 2:00-3:30pm*\n*Terraform: adding resources not supported by a provider (part 3) - still dangerous, you have been warned...*\n @iris\"},\"accessory\": {\"type\": \"button\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Choose\"},\"value\": \"click_me_123\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*Friday, October 21 - 3:00-4:00pm*\n*AWS Cloud Practitioner Certification - Practise Exam questions* \n@irene, ~@johno~\"},\"accessory\": {\"type\": \"button\",\"text\": {\"type\": \"plain_text\",\"emoji\": true,\"text\": \"Choose\"},\"value\": \"click_me_123\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"*<fakelink.ToMoreTimes.com|Show more times>*\"}},{\"type\": \"section\",\"text\": {\"type\": \"mrkdwn\",\"text\": \"Pick an item from the dropdown list\"},\"accessory\": {\"type\": \"static_select\",\"placeholder\": {\"type\": \"plain_text\",\"text\": \"Select an item\",\"emoji\": true},\"options\": [{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 1\",\"emoji\": true},\"value\": \"value-0\"},{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 2\",\"emoji\": true},\"value\": \"value-1\"},{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 3\",\"emoji\": true},\"value\": \"value-2\"},{\"text\": {\"type\": \"plain_text\",\"text\": \"Week 4\",\"emoji\": true},\"value\": \"value-1\"}]}},{\"type\": \"divider\"},{\"type\": \"divider\"}]}";

                //Console.Write(body);

                byte[] responseArray = Encoding.UTF8.GetBytes(test_run); // get the bytes to response
                response.AddHeader("Content-type", "application/json");
                response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                response.KeepAlive = false; // set the KeepAlive bool to false
                response.Close(); // close the connection
                Console.WriteLine("Response given to a request.");
            }
        }
    }
}