
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
            //byte[] bkb = System.IO.File.ReadAllBytes(@"D:\username\Documents\Visual Studio 2013\TFS\COR-Basic\COR-Basic\Basic\Basic\images\Logo\logo_bkb.png");
            byte[] bkb = System.IO.File.ReadAllBytes(@"D:\stefan.steiger\Downloads\160330_bkb_logo_rz_pos.png");


            CoinBaseSharp.SQL.ExecuteNonQuery(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[t_binary]') AND type in (N'U')) 
            EXECUTE('
            CREATE TABLE t_binary(uid uniqueidentifier NOT NULL, binary varbinary(MAX));
            ');
            ");

            CoinBaseSharp.SQL.ExecuteNonQuery("DELETE FROM t_binary;");

            /*
            using (System.Data.IDbCommand cmd = CoinBaseSharp.SQL.CreateCommand("INSERT INTO t_binary(uid,binary) VALUES(NEWID(),@binary)"))
            {
                CoinBaseSharp.SQL.AddParameter(cmd, "binary", coop);

                CoinBaseSharp.SQL.ExecuteNonQuery(cmd);
            } // End Using cmd 
            */

            

            string varbin = "0x" + System.BitConverter.ToString(bkb).Replace("-", "");
            System.Console.WriteLine(varbin);


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

            /*
            // https://www.nuget.org/packages/System.Security.Cryptography.Algorithms/
            // https://dotnet.myget.org/feed/cli-deps/package/nuget/System.Security.Cryptography.Algorithms
            // PM> Install-Package System.Security.Cryptography.Algorithms -Version 4.2.0
            using (var algorithm = System.Security.Cryptography.Algorithms.SHA256.Create())
            {
                // Create the at_hash using the access token returned by CreateAccessTokenAsync.
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(response.AccessToken));

                // Note: only the left-most half of the hash of the octets is used.
                // See http://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken
                identity.AddClaim(JwtRegisteredClaimNames.AtHash, Base64UrlEncoder.Encode(hash, 0, hash.Length / 2));
            }
            */


            using (System.Security.Cryptography.SHA256CryptoServiceProvider sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {

                using (System.Security.Cryptography.RIPEMD160 md160 = System.Security.Cryptography.RIPEMD160.Create())
                {
                    ba = sha256.ComputeHash(ba);
                    ba = md160.ComputeHash(ba);
                }

                /*
                using (System.Security.Cryptography.RIPEMD160 md160 = new System.Security.Cryptography.RIPEMD160Managed())
                {
                    ba = sha256.ComputeHash(ba);
                    ba = md160.ComputeHash(ba);
                } // End Using md160 
                */
            } // End Using sha256 

            return System.BitConverter.ToString(ba).Replace("-", "").ToLowerInvariant();
        } // End Function BitcoinAddressHash


        // http://stackoverflow.com/questions/33245247/hashalgorithms-in-coreclr
        public static void HashTest()
        {
            string bla = Sha256_2("hello");
            System.Console.WriteLine(bla);

            bla = BitcoinAddressHash("hello");
            System.Console.WriteLine(bla);
        } // End Sub HashTest


        public static class LambdaExtensions
        {
            // public static void SetPropertyValue<T>(this T target,
            public static void SetPropertyValue<T>(T target,
                System.Linq.Expressions.Expression<System.Func<T, object>> memberLamda, object value)
            {
                var memberSelectorExpression = memberLamda.Body as System.Linq.Expressions.MemberExpression;
                if (memberSelectorExpression != null)
                {
                    var property = memberSelectorExpression.Member as System.Reflection.PropertyInfo;
                    if (property != null)
                    {
                        property.SetValue(target, value, null);
                    }
                }
            }
        }
        // https://stackoverflow.com/questions/8107134/how-set-value-a-property-selector-expressionfunct-tresult
        /// <summary>
        /// Convert a lambda expression for a getter into a setter
        /// </summary>
        public static System.Action<T, TProperty> GetSetter<T, TProperty>(
            System.Linq.Expressions.Expression<System.Func<T, TProperty>> expression)
        {
            var memberExpression = (System.Linq.Expressions.MemberExpression)expression.Body;
            var property = (System.Reflection.PropertyInfo)memberExpression.Member;
            var setMethod = property.GetSetMethod();

            var parameterT = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var parameterTProperty = System.Linq.Expressions.Expression.Parameter(typeof(TProperty), "y");

            var newExpression =
                System.Linq.Expressions.Expression.Lambda<System.Action<T, TProperty>>(
                    System.Linq.Expressions.Expression.Call(parameterT, setMethod, parameterTProperty),
                    parameterT,
                    parameterTProperty
                );

            return newExpression.Compile();
        }


        public static object GetProperty<T>(T ls, string fieldName)
        {
            System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(T));

            var prop = System.Linq.Expressions.Expression.PropertyOrField(p, fieldName);
            var con = System.Linq.Expressions.Expression.Convert(prop, typeof(object));
            var exp = System.Linq.Expressions.Expression.Lambda(con, p);

            var getValue = (System.Func<T, object>)exp.Compile();
            return getValue(ls);
        }


        // https://stackoverflow.com/questions/321650/how-do-i-set-a-field-value-in-an-c-sharp-expression-tree
        public static void SetProperty<TTarget, TValue>(TTarget target, string fieldName, TValue newValue)
        {
            System.Type tt = newValue.GetType();
            System.Type ttt = typeof(TValue);

            System.Linq.Expressions.ParameterExpression targetExp = System.Linq.Expressions.Expression.Parameter(typeof(TTarget), "target");
            System.Linq.Expressions.ParameterExpression valueExp = System.Linq.Expressions.Expression.Parameter(typeof(TValue), "value");
            

            System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(TTarget));
            // System.Linq.Expressions.MemberExpression fieldExp = System.Linq.Expressions.Expression.PropertyOrField(p, fieldName);

            // Expression.Property can be used here as well
            System.Linq.Expressions.MemberExpression fieldExp =
                // System.Linq.Expressions.Expression.Field(targetExp, fieldName);
                // System.Linq.Expressions.Expression.Property(targetExp, fieldName);
                System.Linq.Expressions.Expression.PropertyOrField(targetExp, fieldName);

            System.Linq.Expressions.BinaryExpression assignExp = 
                System.Linq.Expressions.Expression.Assign(fieldExp, valueExp);

            System.Action<TTarget, TValue> setter = System.Linq.Expressions.Expression
                .Lambda<System.Action<TTarget, TValue>>(assignExp, targetExp, valueExp).Compile();
                

            setter(target, newValue);
        }


        /*
        public static TP CleanProperty<T, TP>(T obj, System.Linq.Expressions.Expression<System.Func<T, TP>> propertySelector) where TP : class
        {
            var valueParam = System.Linq.Expressions.Expression.Parameter(typeof(TP), "value");
            var getValue = propertySelector.Compile();

            // Use the propertySelector body as the left sign of an assign and all the complexity of getting to the property is handled.
            var setValue = System.Linq.Expressions.Expression.Lambda<System.Action<T, TP>>(
                                System.Linq.Expressions.Expression.Assign(propertySelector.Body, valueParam),
                                propertySelector.Parameters[0], valueParam).Compile();

            var value = getValue(obj);
            // if (value != null && IsJunk(value))
            //     setValue(obj, null);
            return value;
        }
        */



        public class Person
        {
            public string Name { get; set; }
            public Person Brother { get; set; }
            public string Email { get; set; }

            public string SnailMail;
            public int Anumber;
        }


        public class T_Benutzer
        {
            public int BE_ID { get; set; }
            public string BE_Name { get; set; }

            public string SnailMail;
            public int Anumber;
        }


        public static void oldGet(System.Collections.Generic.List<string> ls)
        {
            System.Type targetType = ls.GetType();

            //System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(string));
            System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(targetType);
            //var prop = System.Linq.Expressions.Expression.Property(p, "Length");

            //System.Linq.Expressions.Expression.Field(p, "fieldName");
            var prop = System.Linq.Expressions.Expression.PropertyOrField(p, "Count");

            // var prop = System.Linq.Expressions.Expression.Property(p, "Count");
            var con = System.Linq.Expressions.Expression.Convert(prop, typeof(object));
            var exp = System.Linq.Expressions.Expression.Lambda(con, p);
            //var func = (System.Func<string, object>)exp.Compile();

            //var func = (System.Func<System.Collections.Generic.List<string>, object>)exp.Compile();

            var func = (System.Func<System.Collections.Generic.List<string>, object>)exp.Compile();



            var obj = ls;
            int len = (int)func(obj);

        }


        public static void LinqTest()
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
            ls.Add("foo");
            ls.Add("bar");
            ls.Add("foobar");
            int oobj = 123;
            Person someOne = new Person() { Name = "foo", Email = "foo@bar.com", SnailMail="Snail" };
            // SetProperty(someOne, "Anumber", oobj);
            // SetProperty(someOne, "SnailMail", "Turtle Mail");
            // SetProperty(someOne, "Email", "SpamMail");
            T_Benutzer ben = new T_Benutzer();
            GetProperty(ls, "Count");

            string SQL = @"SELECT TOP 1 BE_ID, BE_Name FROM T_Benutzer";
            using (System.Data.Common.DbDataReader rdr = CoinBaseSharp.SQL.ExecuteReader(SQL))
            {
                if (rdr.HasRows)
                {
                    int fieldCount = rdr.FieldCount;
                    System.Type[] ts = new System.Type[fieldCount];
                    string[] fieldNames = new string[fieldCount];
                    for (int i = 0; i < fieldCount; ++i)
                    {
                        ts[i] = rdr.GetFieldType(i);
                        fieldNames[i] = rdr.GetName(i);


                    }

                    while (rdr.Read())
                    {
                        for (int i = 0; i < fieldCount; ++i)
                        {
                            object objValue = rdr.GetValue(i);

                            System.Console.WriteLine(ts[i]);
                            int abc = 123;
                            SetProperty(ben, fieldNames[i], abc);
                            System.Console.WriteLine(objValue);
                        }
                    }

                }
            }
        }


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
            // DataTable dt = CoinBaseSharp.HttpClientHelper.Deserialize<DataTable>().Result;


            // System.Data2.DataTable daaa = System.Data2.Tests.Sql2DataTableTest("SELECT * FROM T_Benutzer");
            // System.Console.WriteLine(daaa);
            // System.Data2.Tests.DataTableTest();
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
