
namespace CoinBaseSharp.ExchangeRates
{


    // http://api.fixer.io/latest
    // http://api.fixer.io/2000-01-03
    
    public class Fixer
    {
        public Fixer()
        {
        }

        public static void Test()
        {
            
            FixerRates fsx = JilHelper.DeserializeUrl<FixerRates>("http://api.fixer.io/latest");
            System.Console.WriteLine(fsx);
        }

        public static void TestEcb()
        {
            Xml2CSharp.EcbRates.Envelope env = 
            Tools.XML.Serialization.DeserializeXmlFromUrl<Xml2CSharp.EcbRates.Envelope>("http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            // Tools.XML.Serialization.DeserializeXmlFromFile<Xml2CSharp.EcbRates.Envelope>("/root/sources/CoinBaseSharp/CoinBaseSharp/ecb_feed.xml");
            System.Console.WriteLine(env);
        }
    }


    public class Rates
    {
        public decimal AUD { get; set; }
        public decimal BGN { get; set; }
        public decimal BRL { get; set; }
        public decimal CAD { get; set; }
        public decimal CHF { get; set; }
        public decimal CNY { get; set; }
        public decimal CZK { get; set; }
        public decimal DKK { get; set; }
        public decimal GBP { get; set; }
        public decimal HKD { get; set; }
        public decimal HRK { get; set; }
        public decimal HUF { get; set; }
        public decimal IDR { get; set; }
        public decimal ILS { get; set; }
        public decimal INR { get; set; }
        public decimal JPY { get; set; }
        public decimal KRW { get; set; }
        public decimal MXN { get; set; }
        public decimal MYR { get; set; }
        public decimal NOK { get; set; }
        public decimal NZD { get; set; }
        public decimal PHP { get; set; }
        public decimal PLN { get; set; }
        public decimal RON { get; set; }
        public decimal RUB { get; set; }
        public decimal SEK { get; set; }
        public decimal SGD { get; set; }
        public decimal THB { get; set; }
        public decimal TRY { get; set; }
        public decimal USD { get; set; }
        public decimal ZAR { get; set; }
    }


    public class FixerRates
    {
        public string @base { get; set; }
        public string date { get; set; }
        // public Rates rates { get; set; }
        public System.Collections.Generic.Dictionary<string, decimal> rates { get; set; }
    }


}
