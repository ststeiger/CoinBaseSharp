
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;


namespace libCoinBaseSharp
{


    public class WebClientHelper
    {


        // GetStream<T>("MY_API_URL");
        public static async Task<T> GetStream<T>(string requestUri)
        {   
            return await GetStream<T>(requestUri, null, null);
        }


        // GetStream<T>("MY_API_URL", headers);
        public static async Task<T> GetStream<T>(string requestUri, Dictionary<string, string> headers)
        {    
            return await GetStream<T>(requestUri, headers, null);
        }


        // GetStream<T>("MY_API_URL", headers, null);
        public static async Task<T> GetStream<T>(string requestUri, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            T retValue = default(T);
            using (HttpClient client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Add("Authorization", "TOKEN");

                if (headers != null)
                {
                    // client.DefaultRequestHeaders.Clear();
                    foreach (KeyValuePair<string, string> kvp in headers)
                    {
                        client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    } // Next kvp 

                } // End if (headers != null)

                if (parameters != null)
                {
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in headers)
                    {
                        requestUri += ((i == 0 && !requestUri.Contains("?")) ? "?" : "&")
                            + System.Net.WebUtility.UrlEncode(kvp.Key)
                            + "=" + System.Net.WebUtility.UrlEncode(kvp.Value);
                        ++i;
                    } // Next kvp 

                } // End if (parameters != null)


                using (HttpResponseMessage response = await client.GetAsync(requestUri))
                {
                    response.EnsureSuccessStatusCode();

                    // if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    if (!response.IsSuccessStatusCode)
                    {
                        return retValue;
                    }

                    if (object.ReferenceEquals(typeof(T), typeof(System.IO.Stream)))
                        retValue = (T)(object)await response.Content.ReadAsStreamAsync();
                    else if (object.ReferenceEquals(typeof(T), typeof(string)))
                    {
                        retValue = (T)(object)await response.Content.ReadAsStringAsync();
                    }
                    else if (object.ReferenceEquals(typeof(T), typeof(byte[])))
                    {
                        retValue = (T)(object)await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        using (System.IO.Stream jsonResponse = await response.Content.ReadAsStreamAsync())
                        {
                            if (jsonResponse != null)
                                retValue = EasyJSON.JsonHelper.Deserialize<T>(jsonResponse);
                        } // End Using strm 
                    }

                } // End Using response 

            } // End Using client 

            return retValue;
        } // End Function TestGetStream 


        // PostStream<T>("MY_API_URL");
        public static async Task<T> PostStream<T>(string requestUri)
        {
            return await PostStream<T>(requestUri, null, null);
        } // End Function PostStream


        // PostStream<T>("MY_API_URL", parameters);
        public static async Task<T> PostStream<T>(string requestUri, Dictionary<string, string> parameters)
        {
            return await PostStream<T>(requestUri, parameters, null);
        } // End Function PostStream


        // PostStream<T>("MY_API_URL", parameters, headers);
        public static async Task<T> PostStream<T>(string requestUri, Dictionary<string, string> parameters, Dictionary<string, string> headers)
        {
            T retValue = default(T);

            using (HttpClient client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Add("Authorization", "TOKEN");

                if (headers != null)
                {
                    // client.DefaultRequestHeaders.Clear();
                    foreach (KeyValuePair<string, string> kvp in headers)
                    {
                        client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                }

                using (HttpContent encodedContent = new FormUrlEncodedContent(parameters))
                {

                    using (HttpResponseMessage response = await client.PostAsync(requestUri, null))
                    {
                        response.EnsureSuccessStatusCode();

                        // if(response.StatusCode == System.Net.HttpStatusCode.OK)
                        if (!response.IsSuccessStatusCode)
                        {
                            return retValue;
                        }

                        if (object.ReferenceEquals(typeof(T), typeof(System.IO.Stream)))
                            retValue = (T)(object)await response.Content.ReadAsStreamAsync();
                        else if (object.ReferenceEquals(typeof(T), typeof(string)))
                        {
                            retValue = (T)(object) await response.Content.ReadAsStringAsync();
                        }
                        else if (object.ReferenceEquals(typeof(T), typeof(byte[])))
                        {
                            retValue = (T)(object)await response.Content.ReadAsByteArrayAsync();
                        }
                        else
                        {
                            using (System.IO.Stream jsonResponse = await response.Content.ReadAsStreamAsync())
                            {
                                if (jsonResponse != null)
                                    retValue = EasyJSON.JsonHelper.Deserialize<T>(jsonResponse);
                            } // End Using strm 
                        }

                    } // End Using response 

                } // End Using encodedContent 

            } // End Using client 

            return retValue;
        } // End Function PostStream  


    }
}
