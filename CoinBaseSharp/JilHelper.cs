
namespace CoinBaseSharp
{


    public class JilHelper
    {

        public JilHelper()
        { }


        public class Employee
        {
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Designation { get; set; }
        }


        public static string SerializeObject(object obj)
        {
            using (System.IO.StringWriter output = new System.IO.StringWriter())
            {
                Jil.JSON.SerializeDynamic(obj, output);
                return output.ToString();
            }
        }


        public static string SerializeEmployee(Employee employee)
        { 
            return Serialize(employee);
        }


        public static string Serialize<T>(T obj)
        {
            using (System.IO.StringWriter output = new System.IO.StringWriter())
            {
                Jil.JSON.Serialize(obj, output);

                return output.ToString();
            }
        }


        public static T Deserialize<T>(string json)
        {
            return Jil.JSON.Deserialize<T>(json);
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
