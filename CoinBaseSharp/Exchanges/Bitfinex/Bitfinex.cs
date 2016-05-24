
using System.Collections.Generic;


namespace CoinBaseSharp.Exchanges.Bitfinex
{


    class Bitfinex
    {

        // https://api.bitfinex.com/v1/ticker/btcusd
        // http://docs.bitfinex.com/#ticker
        public class TickerRootObject
        {
            public decimal mid { get; set; }
            public decimal bid { get; set; }
            public decimal ask { get; set; }
            public decimal last_price { get; set; }
            public decimal timestamp { get; set; }
        }


        // https://api.bitfinex.com/v1/symbols
        // A list of symbol names. Currently “btcusd”, “ltcusd”, “ltcbtc”, “ethusd”, “ethbtc”
        // {"foo": ["btcusd","ltcusd","ltcbtc","ethusd","ethbtc"] }
        public class SymbolsRootObject
        {
            public List<string> foo { get; set; }
        }



        // https://api.bitfinex.com/v1/balances
        // https://api.bitfinex.com/v1/balances
        // https://api.bitfinex.com/v1/order/status
        // https://api.bitfinex.com/v1/positions


        // bids/ask:
        // https://api.bitfinex.com/v1/book/btcusd
        // https://api.bitfinex.com/v1/book/btcusd

    }


}
