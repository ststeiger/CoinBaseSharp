
namespace CoinBaseSharp.API.V2.Currency
{


    // https://api.coinbase.com/v2/currencies
    class Currencies
    {
    }


    public class Datum
    {
        public string id { get; set; }
        public string name { get; set; }
        public string min_size { get; set; }
    }


    public class RootObject
    {
        public System.Collections.Generic.List<Datum> data { get; set; }
    }


} // End Namespace CoinBaseSharp.API.V2.Currency 
