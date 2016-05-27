
using System.Collections.Generic;


namespace CoinBaseSharp.Exchanges.Bitfinex
{


    class Bitfinex
    {

        // https://api.bitfinex.com/v1/ticker/btcusd
        // http://docs.bitfinex.com/#ticker
        public class TickerRootObject
        {
            public decimal mid { get; set; } // (bid + ask) / 2
            public decimal bid { get; set; } // Innermost bid.
            public decimal ask { get; set; } // Innermost ask.
            public decimal last_price { get; set; } // The price at which the last order executed.
            public decimal timestamp { get; set; } // The timestamp at which this information was valid.
        }


        // https://api.bitfinex.com/v1/symbols
        // A list of symbol names. Currently “btcusd”, “ltcusd”, “ltcbtc”, “ethusd”, “ethbtc”
        // {"foo": ["btcusd","ltcusd","ltcbtc","ethusd","ethbtc"] }
        public class SymbolsRootObject
        {
            public List<string> foo { get; set; }
        }

        public class PubTickerRootObject
        {
            public string mid { get; set; }
            public string bid { get; set; }
            public string ask { get; set; }
            public string last_price { get; set; }
            public string low { get; set; }
            public string high { get; set; }
            public string volume { get; set; }
            public string timestamp { get; set; }
        }


        // http://stackoverflow.com/questions/7983441/unix-time-conversions-in-c-sharp
        // "Unix timestamp" means seconds since the epoch in most situations rather than milliseconds... 
        // be careful! However, things like Java use "milliseconds since the epoch" 
        // which may be what you actually care about - despite the tool you showed. 
        // It really depends on what you need.
        // Additionally, you shouldn't be doing anything with local time. Stick to universal time throughout.
        public class DateHelper
        {
            private static readonly System.DateTime UnixEpoch =
                new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

            public static long GetCurrentUnixTimestampMillis()
            {
                return (long) (System.DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
            }

            public static System.DateTime DateTimeFromUnixTimestampMillis(long millis)
            {
                return UnixEpoch.AddMilliseconds(millis);
            }

            public static long GetCurrentUnixTimestampSeconds()
            {
                return (long) (System.DateTime.UtcNow - UnixEpoch).TotalSeconds;
            }

            public static System.DateTime DateTimeFromUnixTimestampSeconds(long seconds)
            {
                return UnixEpoch.AddSeconds(seconds);
            }

        }


        public class Test
        {

            // https://api.bitfinex.com/v1/order/new

            // http://bitcoin.stackexchange.com/questions/25835/bitfinex-api-call-returns-400-bad-request
            public static void GetBalance()
            {
                string APISECRET = "";
                string APIKEY = "";

                // long nonce = System.DateTime.Now.ToUnixTimestampMS(); //returns a strictly increasing timestamp based number e.g. 1402207693893
                long nonce = DateHelper.GetCurrentUnixTimestampMillis();

                string path = "https://api.bitfinex.com/v1/balances";
                string paramDict = "{\"request\": \"/v1/balances\",\"nonce\": \"" + nonce + "\"}"; //ie. {"request": "/v1/balances","nonce": "1402207693893"}
                string payload = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(paramDict));

                //API Sign
                string hexHash = null;
                using (System.Security.Cryptography.HMACSHA384 hmac = 
                           new System.Security.Cryptography.HMACSHA384(System.Text.Encoding.UTF8.GetBytes(APISECRET)))
                {
                    byte[] hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
                    hexHash = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }


                System.Collections.Specialized.NameValueCollection headers = new System.Collections.Specialized.NameValueCollection();
                headers.Add("X-BFX-APIKEY", APIKEY); //My API KEY
                headers.Add("X-BFX-PAYLOAD", payload);
                headers.Add("X-BFX-SIGNATURE", hexHash);

                //POST data
                try
                {
                    //create post request
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(path);
                    request.KeepAlive = true;
                    request.Method = System.Net.Http.HttpMethod.Post.Method;
                   

                    //add headers
                    request.Headers.Add(headers);

                    //write out payload
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(paramDict);
                    request.ContentLength = byteArray.Length;
                    using (System.IO.Stream writer = request.GetRequestStream())
                    {
                        writer.Write(byteArray, 0, byteArray.Length);
                    }

                    //read reply
                    using (System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse)
                    {
                        using (System.IO.TextReader reader = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            //get reply (JSON)
                            string responseContent = reader.ReadToEnd();
                            System.Console.WriteLine(responseContent);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    //always throws an exception here
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

        }



        public class StatsRootObject
        {
            public int period { get; set; }
            public decimal volume { get; set; }
        }


        // https://api.bitfinex.com/v1/stats/btcusd
        // https://api.bitfinex.com/v1/pubticker/btcusd


        // https://api.bitfinex.com/v1/balances
        // https://api.bitfinex.com/v1/balances
        // https://api.bitfinex.com/v1/order/status
        // https://api.bitfinex.com/v1/positions


        // bids/ask:
        // https://api.bitfinex.com/v1/book/btcusd
        // https://api.bitfinex.com/v1/book/btcusd

    }


}
