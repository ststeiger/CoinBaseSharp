
using System.Xml.Serialization;


// https://www.quora.com/Is-there-an-API-for-real-time-currency-conversion


// https://stackoverflow.com/questions/3139879/how-do-i-get-currency-exchange-rates-via-an-api-such-as-google-finance



// https://stackoverflow.com/questions/3139879/how-do-i-get-currency-exchange-rates-via-an-api-such-as-google-finance
// https://stackoverflow.com/questions/17773898/google-currency-converter-api-will-it-shut-down-with-igoogle
// https://rate-exchange.herokuapp.com/fetchRate?from=CAD&to=USD
// https://www.google.com/finance/converter?a=16.6700&from=GBP&to=USD


// https://currencylayer.com/
// http://jsonrates.com/
// https://openexchangerates.org/





//h ttps://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml
namespace Xml2CSharp.EcbRates
{


    [XmlRoot(ElementName = "Sender", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
    public class Sender
    {
        [XmlElement(ElementName = "name", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
        public string Name { get; set; }
    }


    [XmlRoot(ElementName = "Cube", Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
    public class Cube
    {
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlAttribute(AttributeName = "rate")]
        public string Rate { get; set; }
    }


    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
    public class Envelope
    {
        [XmlElement(ElementName = "subject", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
        public string Subject { get; set; }
        [XmlElement(ElementName = "Sender", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
        public Sender Sender { get; set; }
        [XmlElement(ElementName = "Cube", Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
        public Cube Cube { get; set; }
        [XmlAttribute(AttributeName = "gesmes", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Gesmes { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }


}
