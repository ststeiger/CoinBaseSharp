
using System.Collections.Generic;


namespace CoinBaseSharp.API.V2.History
{

    // https://api.coinbase.com/v2/prices/historic
    class Historic
    {
    }

    public class Price
    {
        public string time { get; set; }
        public string price { get; set; }
    }

    public class Data
    {
        public string currency { get; set; }
        public List<Price> prices { get; set; }
    }

    public class Warning
    {
        public string id { get; set; }
        public string message { get; set; }
        public string url { get; set; }
    }

    public class RootObject
    {
        public Data data { get; set; }
        public List<Warning> warnings { get; set; }
    }

}
