
namespace TestApplication
{


    static class Program
    {


        public class JilMoney
        {
            public string Currency = "CHF";
            public int Amount = 123;
            public System.DateTime DateFrom = System.DateTime.Today;
        }


        public class JilEmployee
        {
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Designation { get; set; }
        }


        public class ServiceStackCustomer
        {
            // ServiceStack does not react to fields
            // public string Name;
            // public int Age;

            public string Name { get; set; }
            public int Age { get; set; }
            public System.DateTime DateFrom { get; set; }
        }


        public static void InsertLogo()
        {
            byte[] coop = System.IO.File.ReadAllBytes(@"D:\username\Documents\Visual Studio 2013\TFS\COR-Basic\COR-Basic\Basic\Basic\images\Logo\bank_coop.png");
            byte[] bkb = System.IO.File.ReadAllBytes(@"D:\username\Documents\Visual Studio 2013\TFS\COR-Basic\COR-Basic\Basic\Basic\images\Logo\logo_bkb.png");

            using (System.Data.IDbCommand cmd = CoinBaseSharp.SQL.CreateCommand("INSERT INTO t_binary(uid,binary) VALUES(NEWID(),@binary)"))
            {
                CoinBaseSharp.SQL.AddParameter(cmd, "binary", coop);

                CoinBaseSharp.SQL.ExecuteNonQuery(cmd);
            } // End Using cmd 

            using (System.Data.IDbCommand cmd = CoinBaseSharp.SQL.CreateCommand("INSERT INTO t_binary(uid,binary) VALUES(NEWID(),@binary)"))
            {
                CoinBaseSharp.SQL.AddParameter(cmd, "binary", bkb);
                CoinBaseSharp.SQL.ExecuteNonQuery(cmd);
            } // End Using cmd 

        } // End Sub InsertLogo 



        // https://msdn.microsoft.com/en-us/library/aa691323(v=vs.71).aspx
        // https://msdn.microsoft.com/en-us/library/2bxt6kc4.aspx
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int InlineTest()
        {
            return 2 * 2;
        } // End Function InlineTest 


        // https://msdn.microsoft.com/en-us/library/hh873175.aspx
        // http://blogs.msdn.com/b/pfxteam/archive/2012/04/12/10293335.aspx
        // http://blog.stephencleary.com/2012/02/async-and-await.html 
        // https://stackoverflow.com/questions/14455293/how-and-when-to-use-async-and-await
        // https://msdn.microsoft.com/en-us/library/mt674902.aspx
        // https://msdn.microsoft.com/en-us/library/xh29swte(v=vs.90).aspx
        public async static System.Threading.Tasks.Task<int> GetAnswerToLife()
        {
            await System.Threading.Tasks.Task.Delay(5000);
            int answer = 21 * 2;
            return answer;
        }


        // https://stackoverflow.com/questions/13002507/how-can-i-call-async-go-method-in-for-example-main
        public static void TestAsyncMethod()
        {
            GetAnswerToLife().Wait();
            int x = GetAnswerToLife().Result;

            System.Console.WriteLine(x);
        }


        public static string Sha256_2(string bla)
        {
            byte[] ba = System.Text.Encoding.UTF8.GetBytes(bla);

            using (System.Security.Cryptography.SHA256CryptoServiceProvider sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {
                ba = sha256.ComputeHash(ba);
                ba = sha256.ComputeHash(ba);
            } // End Using sha256 

            return System.BitConverter.ToString(ba).Replace("-", "").ToLowerInvariant();
        } // End Function Sha256_2 


        public static string BitcoinAddressHash(string bla)
        {
            byte[] ba = System.Text.Encoding.UTF8.GetBytes(bla);

            using (System.Security.Cryptography.SHA256CryptoServiceProvider sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {

                using (System.Security.Cryptography.RIPEMD160 md160 = new System.Security.Cryptography.RIPEMD160Managed())
                {
                    ba = sha256.ComputeHash(ba);
                    ba = md160.ComputeHash(ba);
                } // End Using md160 

            } // End Using sha256 

            return System.BitConverter.ToString(ba).Replace("-", "").ToLowerInvariant();
        } // End Function BitcoinAddressHash 


        public static void HashTest()
        {
            string bla = Sha256_2("hello");
            System.Console.WriteLine(bla);

            bla = BitcoinAddressHash("hello");
            System.Console.WriteLine(bla);
        } // End Sub HashTest 


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            if (false)
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new Form1());
            }

            // CoinBaseSharp.HttpClientHelper.Main6();
            CoinBaseSharp.HttpClientHelper.PostSomeData();
            CoinBaseSharp.SQL.MultipleLargeDataSets();
            // CoinBaseSharp.ExchangeRates.Tests.OpenExchangeRates.Test();

            CoinBaseSharp.ISO.ISO4217.Test();


            // CoinBaseSharp.ExchangeRates.Tests.Fixer.Test();
            CoinBaseSharp.ExchangeRates.Tests.ECB.Test();


            CoinBaseSharp.Trash.TestInputSanitation();

            ServiceStackCustomer customer = new ServiceStackCustomer { Name = "Joe Bloggs", Age = 31, DateFrom = System.DateTime.Now };

            string json = CoinBaseSharp.ServiceStackHelper.Serialize(customer);
            System.Console.WriteLine(json);
            ServiceStackCustomer fromJson = CoinBaseSharp.ServiceStackHelper.Deserialize<ServiceStackCustomer>(json);
            System.Console.WriteLine(fromJson);



            JilEmployee emp = new JilEmployee();

            emp.FirstName = "Joe";
            emp.LastName = "Doe";
            emp.EmployeeId = 555;
            emp.Designation = "Mr";

            string crlf = System.Environment.NewLine;

            int c1 = crlf[0]; // 13 Cr
            int c2 = crlf[1]; // 10 LF
            System.Console.WriteLine(c1);
            System.Console.WriteLine(c2);

            string foo = CoinBaseSharp.JilHelper.Serialize(new JilMoney());
            string bar = CoinBaseSharp.JilHelper.SerializeObject(new JilMoney());

            JilMoney my = CoinBaseSharp.JilHelper.Deserialize<JilMoney>(foo);

            System.Console.WriteLine(my);
            System.Console.WriteLine(foo);
            System.Console.WriteLine(bar);

            CoinBaseSharp.API.V2.Currency.RootObject cur = EasyJSON.JsonHelper.DeserializeEmbeddedFile<CoinBaseSharp.API.V2.Currency.RootObject>("currencies.txt");
            System.Console.WriteLine(cur.data.Count);
            // public string id { get; set; }
            // public string name { get; set; }
            // public string min_size { get; set; }

            // Please supply API version (YYYY-MM-DD) as CB-VERSION header
            CoinBaseSharp.API.V2.History.RootObject hist = EasyJSON.JsonHelper.DeserializeEmbeddedFile<CoinBaseSharp.API.V2.History.RootObject>("historic.txt");
            System.Console.WriteLine(hist.data.prices.Count);
            System.Console.WriteLine(hist.warnings.Count);


            string createTable = @"
IF 0 = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'price_history' )
EXECUTE('
CREATE TABLE price_history
(
     uid uniqueidentifier NULL 
    ,time datetime NULL 
    ,price decimal(20, 9) NULL 
);
')
";

            createTable = @"
CREATE TABLE IF NOT EXISTS price_history
(
     uid uuid NULL 
    ,time timestamp without time zone NULL 
    ,price decimal(20, 9) NULL 
);
";

            CoinBaseSharp.SQL.ExecuteNonQuery(createTable);

            // CoinBaseSharp.SQL.BatchedInsert(hist.data.prices, SqlInsertItem);
            CoinBaseSharp.SQL.BatchedInsert(hist.data.prices, delegate(System.Text.StringBuilder sb, CoinBaseSharp.API.V2.History.Price thisPrice)
                {
                    sb.Append("INSERT INTO price_history(uid, time, price) VALUES (");
                    sb.Append("'");
                    sb.Append(System.Guid.NewGuid().ToString());
                    sb.Append("'");
                    sb.Append(", ");

                    if (thisPrice.time == null)
                        sb.Append("NULL");
                    else
                    {
                        sb.Append("'");
                        sb.Append(thisPrice.time.Replace("'", "''"));
                        sb.Append("'");
                    }


                    sb.Append(", ");

                    if (thisPrice.price == null)
                        sb.Append("NULL");
                    else
                    {
                        sb.Append("'");
                        sb.Append(thisPrice.price.Replace("'", "''"));
                        sb.Append("'");
                    }

                    sb.AppendLine(");");
                });


            CoinBaseSharp.API.V2.ServerTime.RootObject time = EasyJSON.JsonHelper.DeserializeEmbeddedFile<CoinBaseSharp.API.V2.ServerTime.RootObject>("time.txt");
            System.Console.WriteLine(time.data.epoch);
            System.Console.WriteLine(time.data.iso);
            System.Console.WriteLine(time.warnings.Count);


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main


        // public delegate void sqlGenerator_t<T>(System.Text.StringBuilder sb, T thisItem);
        public static void SqlInsertItem(System.Text.StringBuilder sb, CoinBaseSharp.API.V2.History.Price thisPrice)
        {
            sb.Append("INSERT INTO price_history(uid, time, price) VALUES (");
            sb.Append("'");
            sb.Append(System.Guid.NewGuid().ToString());
            sb.Append("'");
            sb.Append(", ");

            if (thisPrice.time == null)
                sb.Append("NULL");
            else
            {
                sb.Append("'");
                sb.Append(thisPrice.time.Replace("'", "''"));
                sb.Append("'");
            }


            sb.Append(", ");

            if (thisPrice.price == null)
                sb.Append("NULL");
            else
            {
                sb.Append("'");
                sb.Append(thisPrice.price.Replace("'", "''"));
                sb.Append("'");
            }

            sb.AppendLine(");");
        } // End Sub SqlInsertItem


    } // End Class Program 


} // End Namespace TestApplication
