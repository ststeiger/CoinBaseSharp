
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


            string strSQL = @"
INSERT INTO t_currency( ccy_uid, ccy_number, ccy_name,ccy_abbreviation, ccy_country_name, ccy_minor_units )
VALUES
(
	 @__uid -- ccy_uid uniqueidentifier
	,@__ccy_number -- ccy_number int
	,@__ccy_name -- ccy_name nvarchar(255)
	,@__ccy_abbreviation -- ccy_abbreviation nvarchar(20)
	,@__ccy_country_name -- ccy_country_name nvarchar(255)
	,@__ccy_minor_units -- ccy_minor_units nvarchar(20)
);
";

            foreach (cCurrencyEntry currencyEntry in ISO.CurrencyTable.CurrencyEntries)
            {
                System.Console.WriteLine(currencyEntry.CurrencyAbbreviation);

                using (System.Data.IDbCommand cmd = SQL.CreateCommand(strSQL))
                {
                    SQL.AddParameter(cmd, "__uid", System.Guid.NewGuid());
                    SQL.AddParameter(cmd, "__ccy_number", currencyEntry.CurrencyNumber);
                    SQL.AddParameter(cmd, "__ccy_name", currencyEntry.CurrencyName);
                    SQL.AddParameter(cmd, "__ccy_abbreviation", currencyEntry.CurrencyAbbreviation);
                    SQL.AddParameter(cmd, "__ccy_country_name", currencyEntry.CountryName);
                    SQL.AddParameter(cmd, "__ccy_minor_units", currencyEntry.CurrencyMinorUnits);

                    SQL.ExecuteNonQuery(cmd);
                } // End Using cmd 

            } // Next currencyEntry 

            System.Console.WriteLine("Finished");
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
