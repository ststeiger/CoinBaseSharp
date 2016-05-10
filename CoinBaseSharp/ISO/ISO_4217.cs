
using System.Xml.Serialization;
using System.Collections.Generic;


namespace CoinBaseSharp.ISO
{


    // http://www.currency-iso.org/en/home/tables.html
    // http://www.currency-iso.org/en/home/tables/table-a3.html
    // http://www.currency-iso.org/dam/downloads/lists/list_three.xml
    public class ISO4217
    {

        public static void Test()
        {
            string fileName = CoinBaseSharp.ExchangeRates.Tests.MapProjectPath("~CoinBaseSharp/ISO/ISO4217.xml");

            ISO_4217 ISO =
                // Tools.XML.Serialization.DeserializeXmlFromUrl<ISO_4217>("http://www.currency-iso.org/dam/downloads/lists/list_one.xml");
                Tools.XML.Serialization.DeserializeXmlFromFile<ISO_4217>(fileName);
            System.Console.WriteLine(ISO);

            foreach (cCurrencyEntry currencyEntry in ISO.CurrencyTable.CurrencyEntries)
            {
                System.Console.WriteLine(currencyEntry.CurrencyAbbreviation);
            } // Next currencyEntry 

        } // End Sub Test 

    } // End Class ISO4217 


    [XmlRoot(ElementName = "CcyNtry")]
    public class cCurrencyEntry
    {
        [XmlElement(ElementName = "CtryNm")]
        public string CountryName { get; set; }

        [XmlElement(ElementName = "CcyNm")]
        public string CurrencyName { get; set; }

        [XmlElement(ElementName = "Ccy")]
        public string CurrencyAbbreviation { get; set; }

        [XmlElement(ElementName = "CcyNbr")]
        public string CurrencyNumber { get; set; }

        [XmlElement(ElementName = "CcyMnrUnts")]
        public string CurrencyMinorUnits { get; set; }
    } // End Class cCurrencyEntry 


    [XmlRoot(ElementName = "CcyTbl")]
    public class cCurrencyTable
    {
        [XmlElement(ElementName = "CcyNtry")]
        public List<cCurrencyEntry> CurrencyEntries { get; set; }
    } // End Class cCurrencyTable


    [XmlRoot(ElementName = "ISO_4217")]
    public class ISO_4217
    {
        [XmlElement(ElementName = "CcyTbl")]
        public cCurrencyTable CurrencyTable { get; set; }

        [XmlAttribute(AttributeName = "Pblshd")]
        public string Published { get; set; }
    } // End Class ISO_4217 


} // End Namespace CoinBaseSharp.ISO 
