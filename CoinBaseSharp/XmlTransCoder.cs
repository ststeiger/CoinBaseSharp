
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TelSearch
{


    class XmlTransCoder
    {


        // http://stackoverflow.com/questions/1132494/string-escape-into-xml
        public static string XmlEscape(string unescaped)
        {
            string strRetVal = null;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            strRetVal = node.InnerXml;
            node = null;
            doc = null;

            return strRetVal;
        }


        public static string XmlUnescape(string escaped)
        {
            string strRetVal = null;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerXml = escaped;
            strRetVal = node.InnerText;
            node = null;
            doc = null;

            return strRetVal;
        }


        public static string XmlDecode(string value)
        {
            string strRetVal = null;

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml("<root>" + value + "</root>");
            strRetVal = xmlDoc.InnerText;
            xmlDoc = null;

            return strRetVal;
        }

        // http://wpl.codeplex.com/SourceControl/latest
        // http://stackoverflow.com/questions/157646/best-way-to-encode-text-data-for-xml



        // http://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        //string EncodedXml = SpecialXmlEscape("привет мир");
        //Console.WriteLine(EncodedXml);
        //string DecodedXml = XmlUnescape(EncodedXml);
        //Console.WriteLine(DecodedXml);
        public static string SpecialXmlEscape(string input)
        {
            //string content = System.Xml.XmlConvert.EncodeName("\t");
            //string content = System.Security.SecurityElement.Escape("\t");
            //string strDelimiter = System.Web.HttpUtility.HtmlEncode("\t"); 
            // XmlEscape("\t"); 
            // XmlDecode("&#09;");
            //strDelimiter = XmlUnescape("&#59;");
            //Console.WriteLine(strDelimiter);
            //Console.WriteLine(string.Format("&#{0};", (int)';'));
            //Console.WriteLine(System.Text.Encoding.ASCII.HeaderName);
            //Console.WriteLine(System.Text.Encoding.UTF8.HeaderName);


            string strXmlText = "";

            if (string.IsNullOrEmpty(input))
                return input;


            System.Text.StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; ++i)
            {
                sb.AppendFormat("&#{0};", (int)input[i]);
            } // Next i

            strXmlText = sb.ToString();
            sb.Clear();
            sb = null;

            return strXmlText;
        } // End Function SpecialXmlEscape


    }


}
