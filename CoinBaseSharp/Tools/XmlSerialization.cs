
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


		public static T DeserializeXmlFromFile<T>(string fileName)
		{
			T tReturnValue = default(T);

            using (System.IO.FileStream fstrm = new System.IO.FileStream(fileName, System.IO.FileMode.Open
                , System.IO.FileAccess.Read, System.IO.FileShare.Read)) 
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


        public static T DeserializeXmlFromUrl<T>(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {

                using (System.IO.Stream strm = client.OpenRead(url))
                {
                    return DeserializeXmlFromStream<T>(strm);
                }
            }
        }


        public static T DeserializeXmlFromUrl<T>(System.Uri uri)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream strm = client.OpenRead(uri))
                {
                    return DeserializeXmlFromStream<T>(strm);
                }
            }
        }


        public static void DeserializeXmlFromUrlAsync<T>(System.Uri uri, OnOpenReadCompleted_t onOpenReadCompleted)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                // http://stackoverflow.com/questions/25051674/how-to-wait-for-webclient-openreadasync-to-complete
                client.OpenReadCompleted += new System.Net.OpenReadCompletedEventHandler(onOpenReadCompleted);
                client.OpenReadAsync(uri, "userToken");


                // Lambda: 
                //client.OpenReadCompleted += (s, e) =>
                //{

                //    using (System.IO.Stream strm = e.Result)
                //    {

                //    }

                //};

                
                // Closure: 
                //client.OpenReadCompleted += delegate (object sender, System.Net.OpenReadCompletedEventArgs e)
                //{
                //    if (e.Cancelled == true)
                //    {
                //        // MessageBox.Show("Download has been canceled.");
                //        System.Console.WriteLine("Download has been canceled.");
                //        return;
                //    }
                //    else if (e.Error != null)
                //    {
                //        throw e.Error;
                //    }

                //    using (System.IO.Stream strm = e.Result)
                //    {

                //        strm.Close();
                //    }


                //    string userState = (string)e.UserState;
                //    System.Console.WriteLine("UserState: \"{0}\".", userState);
                //};
            }
        }


        public delegate void OnOpenReadCompleted_t(object sender, System.Net.OpenReadCompletedEventArgs e);

        /*
        public static void OnOpenReadCompleted(object sender, System.Net.OpenReadCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // MessageBox.Show("Download has been canceled.");
                System.Console.WriteLine("Download has been canceled.");
                return;
            }
            else if (e.Error != null)
            {
                throw e.Error;
            }

            using (System.IO.Stream strm = e.Result)
            {

                strm.Close();
            }


            string userState = (string)e.UserState;
            System.Console.WriteLine("UserState: \"{0}\".", userState);
        }
        */


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
