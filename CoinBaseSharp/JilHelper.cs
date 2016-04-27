using System;


using Jil;
using System.Text;

using System.Net.Http;
using System.Threading.Tasks;

//using System.Net.Http.Formatting;
using System.IO;
using System.Reflection;
using System.Net; // TransportContext


namespace CoinBaseSharp
{


    public class JilHelper
    {
        public JilHelper()
        {
        }


        public class Employee
        {
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Designation { get; set; }
        }


        private static string SerializeEmployee(Employee employee)
        {
            using (var output = new StringWriter())
            {
                JSON.Serialize(
                    employee,
                    output
                );
                return output.ToString();
            }
        }

        private static Employee DeserializeEmployee(string employeeString)
        {
            return JSON.Deserialize<Employee>(employeeString);
        }


    }

    public interface IFormatterLogger // System.Net.Http.Formatting
    {}

    public class MediaTypeHeaderValue // System.Net.Http.Formatting
    {
        public MediaTypeHeaderValue(string str)
        {}
    }


    // https://msdn.microsoft.com/en-us/library/system.net.http.formatting.mediatypeformatter%28v=vs.118%29.aspx
    public class MediaTypeFormatter
    {
        public System.Type type;
        public System.Collections.Generic.List<object> SupportedMediaTypes;
        public System.Collections.Generic.List<object> SupportedEncodings;

        public virtual bool CanReadType(Type type)
        {
            return false;
        }


        public virtual bool CanWriteType(Type type)
        {
            return false;
        }

        public virtual Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return null;
        }

        public virtual Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            return null;
        }
    }


    public class JilFormatter : MediaTypeFormatter
    {
        private readonly Options _jilOptions;


        public JilFormatter()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(this.DeserializeFromStream(type, readStream));           
        }


        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    MethodInfo method = typeof(JSON).GetMethod("Deserialize", new Type[] { typeof(TextReader), typeof(Options) });
                    MethodInfo generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[]{ reader, _jilOptions });
                }
            }
            catch
            {
                return null;
            }

        }


        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value, streamWriter, _jilOptions);
            streamWriter.Flush();
            return Task.FromResult(writeStream);
        }

    }

}
