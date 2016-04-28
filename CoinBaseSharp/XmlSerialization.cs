
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


namespace Tools.XML
{


	// http://www.switchonthecode.com/tutorials/csharp-tutorial-xml-serialization
	// http://www.codeproject.com/KB/XML/xml_serializationasp.aspx
	public class Serialization
	{


		public static void SerializeToXml<T>(T ThisTypeInstance, string strFileNameAndPath)
		{
			SerializeToXml<T>(ThisTypeInstance, new System.IO.StreamWriter(strFileNameAndPath));
		} // End Sub SerializeToXml


		public static string SerializeToXml<T>(T ThisTypeInstance)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			string strReturnValue = null;

			SerializeToXml<T>(ThisTypeInstance, new System.IO.StringWriter(sb));

			strReturnValue = sb.ToString();
			sb = null;

			return strReturnValue;
		} // End Function SerializeToXml


		public static void SerializeToXml<T>(T ThisTypeInstance, System.IO.TextWriter tw)
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

			using (System.IO.TextWriter twTextWriter = tw) 
            {
				serializer.Serialize(twTextWriter, ThisTypeInstance);
				twTextWriter.Close();
            } // End Using twTextWriter

			serializer = null;
		} // End Sub SerializeToXml


		public static T DeserializeXmlFromFile<T>(string strFileNameAndPath)
		{
			T tReturnValue = default(T);

			using (System.IO.FileStream fstrm = new System.IO.FileStream(strFileNameAndPath, System.IO.FileMode.Open, System.IO.FileAccess.Read)) 
            {
				tReturnValue = DeserializeXmlFromStream<T>(fstrm);
				fstrm.Close();
            } // End Using fstrm

			return tReturnValue;
		} // End Function DeserializeXmlFromFile


		public static T DeserializeXmlFromEmbeddedRessource<T>(string strRessourceName)
		{
            T tReturnValue = default(T);

			System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();


			using (System.IO.Stream fstrm = ass.GetManifestResourceStream(strRessourceName)) 
            {
				tReturnValue = DeserializeXmlFromStream<T>(fstrm);
				fstrm.Close();
            } // End Using fstrm

			return tReturnValue;
		} // End Function DeserializeXmlFromEmbeddedRessource


        public static T DeserializeXmlFromString<T>(string s)
        {
            T tReturnValue = default(T);

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream))
                {
                    writer.Write(s);
                    writer.Flush();
                    stream.Position = 0;

                    tReturnValue = DeserializeXmlFromStream<T>(stream);
                } // End Using writer

            } // End Using stream

            return tReturnValue;
        } // End Function DeserializeXmlFromString


		public static T DeserializeXmlFromStream<T>(System.IO.Stream strm)
		{
			System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
			T ThisType = default(T);

			using (System.IO.StreamReader srEncodingReader = new System.IO.StreamReader(strm, System.Text.Encoding.UTF8)) 
            {
				ThisType = (T)deserializer.Deserialize(srEncodingReader);
				srEncodingReader.Close();
            } // End Using srEncodingReader

			deserializer = null;

			return ThisType;
		} // End Function DeserializeXmlFromStream


		#if notneeded

		public static void SerializeToXML<T>(System.Collections.Generic.List<T> ThisTypeInstance, string strConfigFileNameAndPath)
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(System.Collections.Generic.List<T>));

			using (System.IO.TextWriter textWriter = new System.IO.StreamWriter(strConfigFileNameAndPath)) {
				serializer.Serialize(textWriter, ThisTypeInstance);
				textWriter.Close();
			}

			serializer = null;
		}
		// SerializeToXML


		public static System.Collections.Generic.List<T> DeserializeXmlFromFileAsList<T>(string strFileNameAndPath)
		{
			System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(System.Collections.Generic.List<T>));
			System.Collections.Generic.List<T> ThisTypeList = null;

			using (System.IO.StreamReader srEncodingReader = new System.IO.StreamReader(strFileNameAndPath, System.Text.Encoding.UTF8)) {
				ThisTypeList = (System.Collections.Generic.List<T>)deserializer.Deserialize(srEncodingReader);
				srEncodingReader.Close();
			}

			deserializer = null;

			return ThisTypeList;
		}
		// DeserializeXmlFromFileAsList

		#endif

	} // End Class Serialization


} // End Namespace COR.Tools.XML
