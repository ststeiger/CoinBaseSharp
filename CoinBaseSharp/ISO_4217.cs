
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
            ISO_4217 ISO =
                Tools.XML.Serialization.DeserializeXmlFromUrl<ISO_4217>("http://www.currency-iso.org/dam/downloads/lists/list_one.xml");
            // Tools.XML.Serialization.DeserializeXmlFromFile<ISO_4217>(@"d:\username\documents\visual studio 2013\Projects\CoinBaseSharp\CoinBaseSharp\ISO4217.xml");
            System.Console.WriteLine(ISO);
        }

    }


    [XmlRoot(ElementName = "CcyNtry")]
    public class CcyNtry
    {
        [XmlElement(ElementName = "CtryNm")]
        public string CtryNm { get; set; }

        [XmlElement(ElementName = "CcyNm")]
        public string CcyNm { get; set; }

        [XmlElement(ElementName = "Ccy")]
        public string Ccy { get; set; }

        [XmlElement(ElementName = "CcyNbr")]
        public string CcyNbr { get; set; }

        [XmlElement(ElementName = "CcyMnrUnts")]
        public string CcyMnrUnts { get; set; }
    }


    [XmlRoot(ElementName = "CcyTbl")]
    public class CcyTbl
    {
        [XmlElement(ElementName = "CcyNtry")]
        public List<CcyNtry> CcyNtry { get; set; }
    }


    [XmlRoot(ElementName = "ISO_4217")]
    public class ISO_4217
    {
        [XmlElement(ElementName = "CcyTbl")]
        public CcyTbl CcyTbl { get; set; }

        [XmlAttribute(AttributeName = "Pblshd")]
        public string Pblshd { get; set; }
    }


}
