
// using System.Threading;
// using System.Threading.Tasks;

using System.Net.Http;
// using System.Net.Http.Headers;



// http://www.c-sharpcorner.com/UploadFile/dacca2/http-request-methods-get-post-put-and-delete/
// https://github.com/mono/mono/blob/master/mcs/class/System.Net.Http/System.Net.Http/HttpClient.cs
namespace CoinBaseSharp
{

    // http://codereview.stackexchange.com/questions/18519/real-world-async-and-await-code-example
    public class HttpClientHelper
    {

        // https://canbilgin.wordpress.com/2012/06/17/download-send-request-asyncwithprogress-with-httpclient/

        /*
        public static async System.Threading.Tasks.Task<int> GetStringAsyncWithProgress(this HttpClient client, HttpRequestMessage request, CancellationToken cancelToken)
        {
            int read = 0;
            int offset = 0;
            string result = string.Empty;

            byte[] responseBuffer = new byte[500];

            var operation = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancelToken);


            return await 
                System.Threading.Tasks.Task.Run<string>(async(token, progress) =>
                {
                    if (cancelToken != CancellationToken.None) token = cancelToken;

                    using (var responseStream = await operation.Result.Content.ReadAsStreamAsync())
                    {
                        do
                        {
                            if (token.IsCancellationRequested)
                            {
                                token.ThrowIfCancellationRequested();
                            }

                            // read = await responseStream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                            result += System.Text.Encoding.UTF8.GetString(responseBuffer, 0, read);
                            offset += read;

                            System.Diagnostics.Debug.WriteLine(string.Format("\t{0:o}\tExtension Read Bytes: {1}", System.DateTime.Now, offset));

                            progress.Report(offset);
                        } while (read != 0);
                    }

                    return result;
                }
            );

        }
        */


        public T Deserialize<T>()
        {
            return AsyncDeserialize<T>().Result;
        }


        // http://stackoverflow.com/questions/30163316/httpclient-getstreamasync-and-http-status-codes
        public async static System.Threading.Tasks.Task<T> AsyncDeserialize<T>()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://httpbin.org/get", HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode)
                {
                    // http://stackoverflow.com/questions/21097730/usage-of-ensuresuccessstatuscode-and-handling-of-httprequestexception-it-throws
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.Content != null)
                        response.Content.Dispose();

                    throw new SimpleHttpResponseException(response.StatusCode, content);
                }

                response.EnsureSuccessStatusCode(); // Throws Exception if no success

                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

                using (System.IO.TextReader streamReader = new System.IO.StreamReader(stream))
                {

                    using (Newtonsoft.Json.JsonTextReader jsonReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                    {
                        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

                        //do some deserializing http://www.newtonsoft.com/json/help/html/Performance.htm
                        return serializer.Deserialize<T>(jsonReader);
                    } // End Using jsonReader 

                } // streamReader

            } // End Using client

        } // End Function asyncDeserialize 


        // https://github.com/enkafan/Swagger.WebApiProxy/blob/master/demo/Swagger.WebApiProxy.Demo.Client/BaseProxy.cs
        public class SimpleHttpResponseException : System.Exception
        {
            public System.Net.HttpStatusCode StatusCode { get; private set; }

            public SimpleHttpResponseException(System.Net.HttpStatusCode statusCode, string content)
                : base(content)
            {
                StatusCode = statusCode;
            }
        }

        static async void DownloadPageAsync()
        {
            // ... Target page.
            string page = "http://en.wikipedia.org/";

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();

                // ... Display the result.
                if (result != null &&
                result.Length >= 50)
                {
                    System.Console.WriteLine(result.Substring(0, 50) + "...");
                }
            }
        }


        // http://www.tugberkugurlu.com/archive/streaming-with-newnet-httpclient-and-httpcompletionoption-responseheadersread
        public static void ReadHttpStream()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = System.TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                string requestUri = "http://localhost:6797";

                using (System.IO.Stream stream = httpClient.GetStreamAsync(requestUri).Result)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            //We are ready to read the stream
                            string currentLine = reader.ReadLine();
                        }
                    }
                }

                
            }
        }


        // http://www.tugberkugurlu.com/archive/streaming-with-newnet-httpclient-and-httpcompletionoption-responseheadersread
        public static void ReadHttpStreamWithParameters()
        {
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.Timeout = System.TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                string requestUri = "http://localhost:6797";

                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(
                    new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() 
                    { 
                        new System.Collections.Generic.KeyValuePair<string, string>("userId", "1000") 
                    }
                );

                formUrlEncodedContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Content = formUrlEncodedContent;

                // HttpResponseMessage aresponse = httpClient.PostAsync(requestUri, formUrlEncodedContent).Result;

                HttpResponseMessage response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

                using (System.IO.Stream stream = response.Content.ReadAsStreamAsync().Result)
                {
                    /*
                    // response.Content.Headers.ContentEncoding
                    var enc = response.Content.Headers.ContentEncoding;
                    foreach (var x in enc)
                    { 
                    
                    }
                    */
                    
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                    {

                        while (!reader.EndOfStream)
                        {
                            //We are ready to read the stream
                            string currentLine = reader.ReadLine();
                        } // Whend 

                    } // End Using reader 

                } // End Using stream 

            } // End Using httpClient 

        } // End Sub ReadHttpStreamWithParameters 


        static void Main1()
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:11129/");
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // List all Names.
                System.Net.Http.HttpResponseMessage response = client.GetAsync("api/Values").Result;  // Blocking call!  

                if (response.IsSuccessStatusCode)
                {
                    string products = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    System.Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

        }


        static void Main2()
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:11129/");
                // Add an Accept header for JSON format.  
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // List all Names.  
                System.Net.Http.HttpResponseMessage response = client.GetAsync("api/Values").Result;  // Blocking call!  
                if (response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine("Request Message Information:- \n\n" + response.RequestMessage + "\n");
                    System.Console.WriteLine("Response Message Header \n\n" + response.Content.Headers + "\n");
                }
                else
                {
                    System.Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
                System.Console.ReadLine();
            }
        }


        public class person
        {
            public string name { get; set; }
            public string surname { get; set; }
        }


        static void Main3()
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                person p = new person { name = "Sourav", surname = "Kayal" };
                client.BaseAddress = new System.Uri("http://localhost:1565/");

                // System.Net.Http.HttpResponseMessage response = System.Net.Http.HttpClientExtensions.PostAsJsonAsync(client, "api/person", p).Result;
                System.Net.Http.HttpResponseMessage response = client.PostAsJsonAsync("api/person", p).Result;
                if (response.IsSuccessStatusCode)
                {
                    System.Console.Write("Success");
                }
                else
                    System.Console.Write("Error");
            }
        }


        static void Main4()
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                person p = new person { name = "Sourav", surname = "Kayal" };
                client.BaseAddress = new System.Uri("http://localhost:1565/");

                // System.Net.Http.HttpResponseMessage response = System.Net.Http.HttpClientExtensions.PutAsJsonAsync(client, "api/person", p).Result;
                System.Net.Http.HttpResponseMessage response = client.PutAsJsonAsync("api/person", p).Result;
                if (response.IsSuccessStatusCode)
                {
                    System.Console.Write("Success");
                }
                else
                    System.Console.Write("Error");
            }
        }


        public static void Main5()
        {
            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                httpClient.Timeout = System.TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                string requestUri = "http://localhost:6797";
                System.IO.Stream stream = httpClient.GetStreamAsync(requestUri).Result;

                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        //We are ready to read the stream
                        string currentLine = reader.ReadLine();
                    }
                }
            }
        }


        // CoinBaseSharp.HttpClientHelper.Main6();
        public static void Main6()
        {
            string requestUri = "http://localhost:53417/ajax/dataReceiver.ashx";

            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                httpClient.Timeout = System.TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);

                System.Net.Http.FormUrlEncodedContent formUrlEncodedContent = new System.Net.Http.FormUrlEncodedContent(
                    new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>()
                    {
                        new System.Collections.Generic.KeyValuePair<string, string>("userId", "1000")
                    }
                );

                formUrlEncodedContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, requestUri);
                request.Content = formUrlEncodedContent;

                System.Net.Http.HttpResponseMessage response = httpClient.SendAsync(
                    request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead).Result;

                System.IO.Stream stream = response.Content.ReadAsStreamAsync().Result;

                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        //We are ready to read the stream
                        string currentLine = reader.ReadLine();
                    }
                }
            }
        }


        public static async void PostSomeData()
        {
            string requestUri = "http://localhost:53417/ajax/dataReceiver.ashx";

            System.Uri uri = new System.Uri(requestUri);
            string comment = "hello world";
            string questionId = 1.ToString();


            System.Net.Http.FormUrlEncodedContent formContent = new System.Net.Http.FormUrlEncodedContent(new[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>("comment", comment),
                new System.Collections.Generic.KeyValuePair<string, string>("questionId", questionId)
            });


            using (System.Net.Http.HttpClient myHttpClient = new System.Net.Http.HttpClient())
            {
                // var response = await myHttpClient.PostAsync(uri.ToString(), formContent);
                System.Net.Http.HttpResponseMessage response = await myHttpClient.PostAsync(uri, formContent);
            }

        }


        public async void SendRequest(string adaptiveUri, string xmlRequest)
        {
            System.Net.Http.HttpResponseMessage ResponseMessage = new System.Net.Http.HttpResponseMessage();

            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                System.Net.Http.StringContent httpConent = new System.Net.Http.StringContent(xmlRequest, System.Text.Encoding.UTF8);

                try
                {
                    ResponseMessage = await httpClient.PostAsync(adaptiveUri, httpConent);
                }
                catch (System.Exception ex)
                {
                    ResponseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    ResponseMessage.ReasonPhrase = string.Format("RestHttpClient.SendRequest failed: {0}", ex);
                }
            }
        }


        public class JsonNetFormatter : System.Net.Http.Formatting.MediaTypeFormatter
        {
            private Newtonsoft.Json.JsonSerializerSettings _jsonSerializerSettings;
            private System.Text.Encoding m_encoding;


            public JsonNetFormatter(Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings)
            {
                _jsonSerializerSettings = jsonSerializerSettings ?? new Newtonsoft.Json.JsonSerializerSettings();

                // Fill out the mediatype and encoding we support
                SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
                m_encoding = new System.Text.UTF8Encoding(false, true);
            }


            public override bool CanReadType(System.Type type)
            {
                // if (type == typeof(IKeyValueModel)) { return false; }

                if (type == null)
                    throw new System.ArgumentNullException("type");

                return true;
            }


            public override bool CanWriteType(System.Type type)
            {
                if (type == null)
                    throw new System.ArgumentNullException("type");

                return true;
            }


            public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(System.Type type,
                System.IO.Stream readStream, System.Net.Http.HttpContent content,
                System.Net.Http.Formatting.IFormatterLogger formatterLogger)
            {

                // Create a serializer
                Newtonsoft.Json.JsonSerializer serializer =
                Newtonsoft.Json.JsonSerializer.Create(_jsonSerializerSettings);

                // Create task reading the content
                return System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    using (System.IO.StreamReader streamReader = new System.IO.StreamReader(readStream, m_encoding))
                    {
                        using (Newtonsoft.Json.JsonTextReader jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                        {
                            return serializer.Deserialize(jsonTextReader, type);
                        }
                    }
                });
            }


            public override System.Threading.Tasks.Task WriteToStreamAsync(System.Type type, object value
                , System.IO.Stream writeStream
                , System.Net.Http.HttpContent content
                , System.Net.TransportContext transportContext)
            {
                // Create a serializer
                Newtonsoft.Json.JsonSerializer serializer =
                    Newtonsoft.Json.JsonSerializer.Create(_jsonSerializerSettings);

                // Create task writing the serialized content
                return System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(writeStream, m_encoding))
                    {
                        using (Newtonsoft.Json.JsonTextWriter jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                        {
                            serializer.Serialize(jsonTextWriter, value);
                        }
                    }
                });
            }


        }


        public async System.Threading.Tasks.Task<string> Put<T>(string uri, T data)
        {
            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                System.Net.Http.Headers.MediaTypeHeaderValue mediaType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings =
                    new Newtonsoft.Json.JsonSerializerSettings();

                JsonNetFormatter jsonFormatter = new JsonNetFormatter(jsonSerializerSettings);

                System.Net.Http.ObjectContent content = new System.Net.Http.ObjectContent<T>(data
                    , new System.Net.Http.Formatting.JsonMediaTypeFormatter()
                );

                System.Net.Http.HttpResponseMessage response = await httpClient.PutAsync(uri, content);
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;

                /*
                var requestMessage = new System.Net.Http.HttpRequestMessage
                    (data, mediaType, new System.Net.Http.Formatting.MediaTypeFormatter[] { jsonFormatter });
                
                // var result = httpClient.PutAsync("_endpoint", requestMessage.Content).Result;
                // return result.Content.ReadAsStringAsync().Result;
                */
            }
        }


        public static void UploadJsonObjectAsync()
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:1565/");
                System.Net.Http.HttpResponseMessage response =
                    client.DeleteAsync("api/person/10").Result;

                if (response.IsSuccessStatusCode)
                {
                    System.Console.Write("Success");
                }
                else
                    System.Console.Write("Error");
            }
        }


        // http://www.thomaslevesque.com/2013/11/30/uploading-data-with-httpclient-using-a-push-model/
        async System.Threading.Tasks.Task UploadJsonObject0Async<T>(System.Uri uri, T data)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                System.Net.Http.HttpResponseMessage response =
                    await client.PostAsync(uri, new System.Net.Http.StringContent(json));

                response.EnsureSuccessStatusCode();
            }
        }


        async System.Threading.Tasks.Task UploadJsonObject1Async<T>(System.Uri uri, T data)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                System.Net.Http.PushStreamContent content = new System.Net.Http.PushStreamContent((stream, httpContent, transportContext) =>
                    {

                        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                        using (System.IO.TextWriter writer = new System.IO.StreamWriter(stream))
                        {
                            serializer.Serialize(writer, data);
                        }

                    });

                System.Net.Http.HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
            }

        }


        async System.Threading.Tasks.Task UploadJsonObject2Async<T>(System.Uri uri, T data)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                System.Net.Http.ObjectContent content = new System.Net.Http.ObjectContent<T>(data
                    , new System.Net.Http.Formatting.JsonMediaTypeFormatter()
                );
                System.Net.Http.HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
            }
        }


        public async void SimpleAsyncPost()
        {
            System.Collections.Generic.Dictionary<string, string> values =
                new System.Collections.Generic.Dictionary<string, string>();

            values.Add("ThisIs", "Annoying");
            System.Net.Http.FormUrlEncodedContent content = new System.Net.Http.FormUrlEncodedContent(values);

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                try
                {
                    System.Net.Http.HttpResponseMessage httpResponseMessage =
                        await client.PostAsync("http://SomeUrl.somewhere", content);

                    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Do something...
                    }
                }
                catch (System.OperationCanceledException opc)
                {
                    System.Console.WriteLine(opc);
                }
            }
        }


        // https://www.jayway.com/2012/01/18/webclientwebrequest-threading-untangled/
        // https://www.jayway.com/2012/03/13/httpclient-makes-get-and-post-very-simple/
        public async System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JObject> GetAsync(string uri)
        {
            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                System.Net.Http.HttpResponseMessage response = await httpClient.GetAsync(uri);

                //will throw an exception if not successful
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                return await System.Threading.Tasks.Task.Run(() => Newtonsoft.Json.Linq.JObject.Parse(content));
            }

        }


        public async System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JObject> PostAsync(string uri, string data)
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response =
                await httpClient.PostAsync(uri, new System.Net.Http.StringContent(data));

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return await System.Threading.Tasks.Task.Run(() => Newtonsoft.Json.Linq.JObject.Parse(content));
        }


        // https://forums.asp.net/t/1773007.aspx?How+to+correctly+use+PostAsync+and+PutAsync+
        public string Put<T>(T data)
        {
            System.Net.Http.Headers.MediaTypeHeaderValue mediaType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings =
                new Newtonsoft.Json.JsonSerializerSettings();

            // JsonNetFormatter jsonFormatter = new JsonNetFormatter(jsonSerializerSettings);
            // var requestMessage = new
            // System.Net.Http.HttpRequestMessage<T>(data, mediaType
            // , new System.Net.Http.Formatting.MediaTypeFormatter[] { jsonFormatter });

            System.Net.Http.ObjectContent content = new System.Net.Http.ObjectContent<T>(data
                    , new System.Net.Http.Formatting.JsonMediaTypeFormatter()
            );

            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                string _endpoint = "";
                httpClient.PutAsync(_endpoint, content).ContinueWith(httpResponseMessage =>
                {
                    return httpResponseMessage.Result.Content.ReadAsStringAsync();
                    // return await Task.Run(() => httpResponseMessage.Result.Content.ReadAsStringAsync());
                });
            }

            return null;
        }


        public class CreateAppRequest
        {
            public string userAgent;
            public string endpointId;
            public string culture;
        }


        // https://alanfeekery.com/2015/04/13/posting-json-data-using-httpclient/
        public void PostJSON()
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {

                CreateAppRequest request = new CreateAppRequest()
                {
                    userAgent = "myAgent",
                    endpointId = "1234",
                    culture = "en-US"
                };


                System.Net.Http.HttpResponseMessage response = client.PostAsync("https://domain.com/CreateApp",
                    new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request).ToString(),
                        System.Text.Encoding.UTF8, "application/json")
                ).Result;

                if (response.IsSuccessStatusCode)
                {
                    dynamic content = Newtonsoft.Json.JsonConvert.DeserializeObject(
                        response.Content.ReadAsStringAsync().Result
                    );

                    // Access variables from the returned JSON object
                    // var appHref = content.links.applications.href;
                }
            }
        }


        // http://haroldrv.com/2015/01/how-to-use-httpclient-with-basic-authentication-to-post-data-asynchronously/
        public async System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostJsonAsync(string apiUrl, string messageBody)
        {
            string BaseUrl = "";

            System.Net.Http.HttpResponseMessage response;
            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new System.ApplicationException("apiUrl required");
            }

            using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
            {
                System.Net.Http.StringContent request = new System.Net.Http.StringContent(messageBody);
                request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                string url = string.Format("{0}{1}", BaseUrl, apiUrl);
                byte[] credentials = System.Text.Encoding.ASCII.GetBytes("myUsername:myPassword");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic"
                    , System.Convert.ToBase64String(credentials)
                );

                response = await httpClient.PostAsync(new System.Uri(url), request);
            }

            return response;
        }


        public async void TestPostJson()
        {
            const string apiUrl = "api/yourApiEndpoint";
            var newUser = new { username = "abc", password = "123" };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(newUser);
            System.Net.Http.HttpResponseMessage response = await this.PostJsonAsync(apiUrl, json);
        }


    } // End Class HttpClientHelper


} // End Namespace CoinBaseSharp 
