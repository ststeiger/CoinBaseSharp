using System;
using System.Collections.Generic;
using System.Text;

namespace CoinBaseSharp.API.V2.ServerTime
{
    // https://api.coinbase.com/v2/time
    class Time
    {
    }


    public class Data
    {
        public string iso { get; set; }
        public int epoch { get; set; }
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
