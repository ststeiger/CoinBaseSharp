

namespace CoinBaseSharp.Helpers
{
    public partial class HttpHelper
    {

        // https://stackoverflow.com/questions/4015324/http-request-with-post
        // https://stackoverflow.com/questions/4088625/net-simplest-way-to-send-post-with-data-and-read-response
        public static class Http
        {
            public static byte[] Post(string uri, System.Collections.Specialized.NameValueCollection pairs)
            {
                byte[] response = null;
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    response = client.UploadValues(uri, pairs);
                }
                return response;
            }
        }



        // Sample call : DownLoadFileInBackground2 ("http://www.contoso.com/logs/January.txt");
        public static void DownLoadFileInBackground(string address)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            System.Uri uri = new System.Uri(address);

            // Specify that the DownloadFileCallback method gets called
            // when the download completes.
            client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(OnDownloadFileCompleted);

            client.DownloadDataCompleted += new System.Net.DownloadDataCompletedEventHandler(OnDownloadDataCompleted);
            client.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(OnDownloadStringCompleted);


            // Specify a progress notification handler.
            client.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(OnDownloadProgressChanged);
            client.UploadProgressChanged += new System.Net.UploadProgressChangedEventHandler(OnUploadProgressChanged);

            

            

            // https://stackoverflow.com/questions/22865612/passing-filename-to-downloadfilecompleted-in-async-file-download-using-webclient
            client.DownloadFileAsync(uri, "serverdata.txt", "data for callback, e.g. filename to save it as");
            
            

            // System.Threading.AutoResetEvent waiter = new System.Threading.AutoResetEvent(false);
            // client.DownloadDataAsync(uri, waiter);


            // client.DownloadStringAsync(uri, "userToken");

            // Block the main application thread. Real applications
            // can perform other tasks while waiting for the download to complete.
            // waiter.WaitOne();
        }


        // https://stackoverflow.com/questions/13917009/web-client-downloadfilecompleted-get-file-name
        // https://stackoverflow.com/questions/22865612/passing-filename-to-downloadfilecompleted-in-async-file-download-using-webclient
        public static void OnDownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // MessageBox.Show("Download has been canceled.");
                System.Console.WriteLine("Download has been canceled.");
            }
            else if (e.Error != null)
            {
                throw e.Error;
            }

            
            // var userToken = (DownloadState)e.UserState;
            string fileName = (string)e.UserState;
            System.Console.WriteLine("File has been downloaded to \"{0}\".", fileName);
        }

        public static void OnDownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && !string.IsNullOrEmpty(e.Result))
            {
                // do something with e.Result string.
                // _results.Add(e.Result);
            }
        }

        public static void OnDownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            System.Threading.AutoResetEvent waiter = (System.Threading.AutoResetEvent)e.UserState;

            try
            {
                // If the request was not canceled and did not throw
                // an exception, display the resource.
                if (!e.Cancelled && e.Error == null)
                {
                    byte[] data = (byte[])e.Result;
                    string textData = System.Text.Encoding.UTF8.GetString(data);

                    System.Console.WriteLine(textData);
                }
            }
            finally
            {
                // Let the main application thread resume.
                waiter.Set();
            }
        }


        private static void OnUploadProgressChanged(object sender, System.Net.UploadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            System.Console.WriteLine("{0}    uploaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesSent,
                e.TotalBytesToSend,
                e.ProgressPercentage);
        }


        public static void OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            System.Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }


        public static void foo()
        {
            string URI = "http://www.myurl.com/post.php";
            string myParameters = "param1=value1&param2=value2&param3=value3";

            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);
            }

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("param1", "<any> kinds & of = ? strings");
                reqparm.Add("param2", "escaping is already handled");
                byte[] responsebytes = client.UploadValues("http://localhost", "POST", reqparm);
                string responsebody = System.Text.Encoding.UTF8.GetString(responsebytes);
            }
        }


        public static void Example()
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {

                byte[] response =
                client.UploadValues("http://dork.com/service", new System.Collections.Specialized.NameValueCollection()
               {
                   { "home", "Cosby" },
                   { "favorite+flavor", "flies" }
               });

                string result = System.Text.Encoding.UTF8.GetString(response);
            }
        }

    }
}
