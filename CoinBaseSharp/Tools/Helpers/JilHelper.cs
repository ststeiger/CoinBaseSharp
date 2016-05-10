
namespace CoinBaseSharp
{

    public class ServiceStackHelper
    {


        static ServiceStackHelper()
        {
            ServiceStack.Text.JsConfig.DateHandler = ServiceStack.Text.DateHandler.ISO8601;
        }



        public static string Serialize<T>(T obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }


        public static T Deserialize<T>(string json)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(json);
        }


        public static T Deserialize<T>(System.IO.Stream strm)
        {

            return ServiceStack.Text.JsonSerializer.DeserializeFromStream<T>(strm);
        }




        public static T DeserializeFromFile<T>(string fileName)
        {
            T tReturnValue = default(T);

            using (System.IO.FileStream fstrm = new System.IO.FileStream(fileName, System.IO.FileMode.Open
                , System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                tReturnValue = Deserialize<T>(fstrm);
                fstrm.Close();
            } // End Using fstrm

            return tReturnValue;
        } // End Function DeserializeFromFile

    }



    // https://github.com/ngs-doo/json-benchmark
    // http://theburningmonk.com/2014/08/json-serializers-benchmarks-updated-2/
    public class JilHelper
    {

        private static Jil.Options defaultOptions;

        static JilHelper()
        {
            // https://github.com/kevin-montrose/Jil#configuration
            defaultOptions = new Jil.Options(
                prettyPrint: true, excludeNulls: true, dateFormat: Jil.DateTimeFormat.ISO8601
            );
            
        }


        public static string SerializeObject(object obj)
        {
            using (System.IO.StringWriter output = new System.IO.StringWriter())
            {
                Jil.JSON.SerializeDynamic(obj, output, defaultOptions);
                return output.ToString();
            }
        }


        public static string Serialize<T>(T obj)
        {
            using (System.IO.StringWriter output = new System.IO.StringWriter())
            {
                Jil.JSON.Serialize(obj, output, defaultOptions);

                return output.ToString();
            }
        }


        public static T Deserialize<T>(string json)
        {
            return Jil.JSON.Deserialize<T>(json, defaultOptions);
        }


        public static T Deserialize<T>(System.Uri uri)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream strm = client.OpenRead(uri))
                {
                    return Deserialize<T>(strm);
                }
            }
        }


        public static T DeserializeUrl<T>(string uri)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream strm = client.OpenRead(uri))
                {
                    return Deserialize<T>(strm);
                }
            }
        }


        public static T DeserializeFromFile<T>(string fileName)
        {
            T tReturnValue = default(T);

            using (System.IO.FileStream fstrm = new System.IO.FileStream(fileName, System.IO.FileMode.Open
                , System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                tReturnValue = Deserialize<T>(fstrm);
                fstrm.Close();
            } // End Using fstrm

            return tReturnValue;
        } // End Function DeserializeFromFile

        public static T Deserialize<T>(System.IO.Stream strm)
        {
            
            using (System.IO.TextReader tr = new System.IO.StreamReader(strm))
            {
                return Jil.JSON.Deserialize<T>(tr);
            }
             
        }

    }

    /*
    public interface IFormatterLogger // System.Net.Http.Formatting
    {}

    
    public class MediaTypeHeaderValue // System.Net.Http.Formatting
    {
        public MediaTypeHeaderValue(string str)
        {}
    }

    
    // https://msdn.microsoft.com/en-us/library/system.net.http.formatting.mediatypeformatter%28v=vs.118%29.aspx
    public class MediaTypeFormatter2
    {
        public System.Type type;
        public System.Collections.Generic.List<object> SupportedMediaTypes;
        public System.Collections.Generic.List<object> SupportedEncodings;

        public virtual bool CanReadType(System.Type type)
        {
            return false;
        }


        public virtual bool CanWriteType(System.Type type)
        {
            return false;
        }

        public virtual System.Threading.Tasks.Task<object> ReadFromStreamAsync(System.Type type
            , System.IO.Stream readStream
            , System.Net.Http.HttpContent content
            , IFormatterLogger formatterLogger)
        {
            return null;
        }

        public virtual System.Threading.Tasks.Task WriteToStreamAsync(System.Type type, object value
            , System.IO.Stream writeStream
            , System.Net.Http.HttpContent content
            , System.Net.TransportContext transportContext)
        {
            return null;
        }
    }
    */


    public class JilFormatter : System.Net.Http.Formatting.MediaTypeFormatter
    {

        private readonly Jil.Options _jilOptions;


        public JilFormatter()
        {
            _jilOptions = new Jil.Options(dateFormat: Jil.DateTimeFormat.ISO8601);
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new System.Text.UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        public override bool CanReadType(System.Type type)
        {
            if (type == null)
            {
                throw new System.ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(System.Type type)
        {
            if (type == null)
            {
                throw new System.ArgumentNullException("type");
            }

            return true;
        }

        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(System.Type type
            , System.IO.Stream readStream
            , System.Net.Http.HttpContent content
            , System.Net.Http.Formatting.IFormatterLogger formatterLogger)
        {
            return System.Threading.Tasks.Task.FromResult(this.DeserializeFromStream(type, readStream));           
        }


        private object DeserializeFromStream(System.Type type, System.IO.Stream readStream)
        {
            try
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(readStream))
                {
                    System.Reflection.MethodInfo method = typeof(Jil.JSON).GetMethod("Deserialize"
                        , new System.Type[] { typeof(System.IO.TextReader), typeof(Jil.Options) }
                    );
                    System.Reflection.MethodInfo generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[]{ reader, _jilOptions });
                }
            }
            catch
            {
                return null;
            }

        }


        public override System.Threading.Tasks.Task WriteToStreamAsync(System.Type type, object value
            , System.IO.Stream writeStream
            , System.Net.Http.HttpContent content
            , System.Net.TransportContext transportContext)
        {
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(writeStream);
            Jil.JSON.Serialize(value, streamWriter, _jilOptions);
            streamWriter.Flush();
            return System.Threading.Tasks.Task.FromResult(writeStream);
        }


    }


}
