
// using System.Net.Http;

/*
namespace CoinBaseSharp.Helpers
{


    // https://stackoverflow.com/questions/4015324/http-request-with-post
    // https://www.nuget.org/packages/Microsoft.Net.Http
 * // https://stackoverflow.com/questions/15176538/net-httpclient-how-to-post-string-value
    public partial class HttpHelper
    {

        public static void HttpGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var responseString = client.GetStringAsync("http://www.example.com/recepticle.aspx");
            }
        }

        public static void HttpPost()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var values = new System.Collections.Generic.Dictionary<string, string>
                {
                   { "thing1", "hello" },
                   { "thing2", "world" }
                };

                var content = new System.Net.Http.FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

                string responseString = await response.Content.ReadAsStringAsync();
            }
        }

    }
}
*/