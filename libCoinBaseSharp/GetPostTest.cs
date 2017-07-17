
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;


namespace libCoinBaseSharp
{


    public class SampleClass
    { }


    // http://www.newtonsoft.com/json/help/html/Performance.htmss
    class GetPostTest
    {


        // TestGet<T>("MY_API_URL");
        public static async Task<T> TestGet<T>(string requestUri)
        {
            T retValue = default(T);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "TOKEN");
                using (HttpResponseMessage response = await client.GetAsync(requestUri))
                {

                    if (!response.IsSuccessStatusCode)
                    {
                        return retValue;
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (jsonResponse != null)
                        retValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
                } // End Using response 

            } // End Using client 

            return retValue;
        } // End Function TestGet 


        // TestPost<T>("MY_API_URL");
        public static async Task<T> TestPost<T>(string requestUri)
        {
            T retValue = default(T);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "TOKEN");

                Dictionary<string, string> parameters = new Dictionary<string, string> {
                      { "param1", "1" }
                    , { "param2", "2" }
                };

                using (HttpContent encodedContent = new FormUrlEncodedContent(parameters))
                {

                    using (HttpResponseMessage response = await client.PostAsync(requestUri, null))
                    {

                        // response.EnsureSuccessStatusCode(); // Throws exception

                        // if(response.StatusCode == System.Net.HttpStatusCode.OK)
                        if (!response.IsSuccessStatusCode)
                        {
                            return retValue;
                        }

                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        if (jsonResponse != null)
                            retValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
                    } // End Using response 

                } // End Using encodedContent 

            } // End Using client 

            return retValue;
        } // End Function TestPost 


        public static async void TestGetStream()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>() {
                      { "param1", "1" }
                    , { "param2", "2" }
            };

            await WebClientHelper.GetStream<string>("", headers);
        } // End Function TestGetStream 


        public static async Task<T> TestPostStream<T>(string requestUri)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                      { "param1", "1" }
                    , { "param2", "2" }
            };

            System.Collections.Generic.Dictionary<string, string> head = new Dictionary<string, string>();
            head.Add("Authorization", "TOKEN");

            return await WebClientHelper.PostStream<T>(requestUri, parameters, head);
        } // End Function TestPostStream


    } // End Class 


} // End Namespace 
