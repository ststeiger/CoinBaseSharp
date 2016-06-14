
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


        public class DataRow
        {
            public DataTable Table;

            protected System.Collections.Generic.Dictionary<string, object> m_ColumnData;


            internal DataRow() :this(null)
            { }

            internal DataRow(DataTable table)
            {
                this.Table = table;

                if (this.Table != null && this.Table.CaseSensitive)
                    this.m_ColumnData = new System.Collections.Generic.Dictionary<string, object>(System.StringComparer.Ordinal);
                else
                    this.m_ColumnData = new System.Collections.Generic.Dictionary<string, object>(System.StringComparer.OrdinalIgnoreCase);
            }


            public object this[string columnName]
            {
                get
                {
                    if(this.m_ColumnData.ContainsKey(columnName))
                        return m_ColumnData[columnName];

                    return System.DBNull.Value;
                }

                set {
                    this.m_ColumnData[columnName] = value;
                }

            }


            public object this[int ordinal]
            {
                get
                {
                    string columnName = this.Table.Columns[ordinal].ColumnName;
                    return m_ColumnData[columnName];
                }

                set
                {
                    string columnName = this.Table.Columns[ordinal].ColumnName;
                    this.m_ColumnData[columnName] = value;
                }

            }
        }


        public class DataColumn
        {
            protected DataTable Table;
            public string ColumnName;
            public System.Type DataType;

            public DataColumn(DataTable table, string columnName, System.Type type)
            {
                this.Table = table;
                this.ColumnName = columnName;
                this.DataType = type;
            }


            public string Caption
            {
                get
                {
                    return "";
                }
            }


            public int Ordinal
            {
                get
                {
                    return this.Table.Columns.GetOrdinal(this.ColumnName);
                }

                set
                {
                    SetOrdinal(value);
                }
            }


            public void SetOrdinal(int newOrdinal)
            {
                if (newOrdinal >= this.Table.Columns.Count)
                {
                    throw new System.Exception("newOrdinal must be < Columns.Count");
                }

                this.Table.Columns.SetOrdinal(this.ColumnName, newOrdinal);
            }


            public bool AllowDBNull
            {
                get { return true; }
                set
                {
                    throw new System.NotImplementedException();
                }

            }


        }




        public class DataRowCollection : System.Collections.Generic.IEnumerable<DataRow>
        {
            protected DataTable m_Table;
            public System.Collections.Generic.List<DataRow> Rows; // Count

            public DataRowCollection()
            { }

            public DataRowCollection(DataTable table)
            {
                this.m_Table = table;
                this.Rows = new System.Collections.Generic.List<DataRow>();
            }


            public System.Collections.Generic.IEnumerator<DataRow> GetEnumerator()
            {
                return this.Rows.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get {
                    return this.Rows.Count;
                }
            }


            public void Add(DataRow row)
            {
                this.Rows.Add(row);
            }


            public void Add(params object[] values)
            {
                DataRow dr = this.m_Table.NewRow();

                for (int j = 0; j < values.Length; ++j)
                {
                    dr[j] = values[j];
                }

                this.Rows.Add(dr);
            }


            public DataRow this[int rowNum]
            {
                get
                {
                    return this.Rows[rowNum];
                }
            }
        }


        public class DataColumnCollection : System.Collections.Generic.IEnumerable<DataColumn>
        {
            protected DataTable m_Table;
            protected System.Collections.Generic.List<DataColumn> m_Columns; // Count
            
            
            public DataColumnCollection() :this(null)
            { }

            public DataColumnCollection(DataTable table)
            {
                this.m_Table = table;
                this.m_Columns = new System.Collections.Generic.List<DataColumn>();
            }


            public int Count
            {
                get {
                    return this.m_Columns.Count;
                }
            }

            public System.Collections.Generic.IEnumerator<DataColumn> GetEnumerator()
            {
                return this.m_Columns.GetEnumerator();
            }


            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }


            // public System.Collections.Generic.Dictionary<string, System.Type> Columns1; // Count



            public void Add(string columnName)
            {
                this.Add(columnName, typeof(string));
            }

            public void Add(string columnName, System.Type type)
            {
                DataColumn dc = new DataColumn(this.m_Table, columnName, type);
                this.m_Columns.Add(dc);
            }


            public int GetOrdinal(string columnName)
            {
                int ord = this.m_Columns.FindIndex(delegate(DataColumn that)
                {
                    if (this.m_Table.CaseSensitive)
                        return string.Equals(that.ColumnName, columnName, System.StringComparison.Ordinal);

                    return string.Equals(that.ColumnName, columnName, System.StringComparison.OrdinalIgnoreCase);
                }
                );

                return ord;
            }


            public void SetOrdinal(string columnName, int newOrdinal)
            {

                if (newOrdinal >= this.m_Columns.Count)
                {
                    throw new System.Exception("newOrdinal must be < Columns.Count");
                }

                DataColumn item = this[columnName];
                this.m_Columns.Remove(item);
                this.m_Columns.Insert(newOrdinal, item);
            }


            public DataColumn this[int index]
            {
                get
                {
                    return this.m_Columns[index];
                }
            }


            public DataColumn this[string columnName]
            {
                get
                {
                    int ord = this.GetOrdinal(columnName);
                    return this[ord];
                }
            }


        }


        public class DataTable
        {
            
            public string NameSpace;
            public string TableName;
            public System.Globalization.CultureInfo Culture;
            public bool CaseSensitive;


            public DataTable()
                : this(null, null)
            { }


            public DataTable(string tableName)
                : this(tableName, null)
            {
            }


            public DataTable(string tableName, string tableNamespace)
            {
                this.TableName = tableName;
                this.NameSpace = tableNamespace;
                this.Culture = System.Globalization.CultureInfo.InvariantCulture;

                this.Columns = new DataColumnCollection(this);
                this.Rows = new DataRowCollection(this);
            }


            public DataColumnCollection Columns; // Count
            public DataRowCollection Rows; // Count


            public DataRow NewRow()
            {
                return new DataRow(this);
            }


            public void WriteXml(System.IO.Stream stream)
            {
                throw new System.NotImplementedException();
            }


            public string ToHtml()
            {
                return this.ToHtml(null);
            }

            public string ToHtml(string id)
            {
                return this.ToHtml(null, null);
            }


            // http://www.mediaevent.de/xhtml/tbody.html
            public string ToHtml(string id, string className)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table");

                if (!string.IsNullOrWhiteSpace(id))
                {
                    sb.Append(" id=\"");
                    sb.Append(id);
                    sb.Append("\"");
                }

                if (!string.IsNullOrWhiteSpace(className))
                {
                    sb.Append(" class=\"");
                    sb.Append(className);
                    sb.Append("\"");
                }

                sb.AppendLine(">");

                sb.AppendLine("<thead>");
                sb.AppendLine("    <tr>");
                foreach (DataColumn dc in this.Columns)
                {
                    sb.Append("        <th>");
                    sb.Append(dc.ColumnName);
                    sb.AppendLine("</th>");
                }
                sb.AppendLine("    </tr>");
                sb.AppendLine("</thead>");


                sb.AppendLine("<tbody>");

                for (int i = 0; i < this.Rows.Count; ++i)
                {
                    sb.AppendLine("    <tr>");

                    foreach (DataColumn dc in this.Columns)
                    {
                        sb.Append("        <td>");
                        object val = this.Rows[i][dc.ColumnName];
                        string stringVal = null;

                        if (val != null)
                        {
                            if (object.ReferenceEquals(val.GetType(), typeof(System.DateTime)))
                            {
                                System.DateTime dat = System.Convert.ToDateTime(val).ToLocalTime();

                                stringVal = dat.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", this.Culture);
                            }
                            else
                                stringVal = val.ToString();
                        }

                        sb.Append(stringVal);
                        sb.AppendLine("</td>");
                    }

                     sb.AppendLine("    </tr>");
                }

                   

                sb.AppendLine("</tbody>");

                // sb.AppendLine("<tfoot>");
                // sb.AppendLine("    <tr>");
                // sb.Append("        <th>");
                // sb.Append(dc.ColumnName);
                // sb.AppendLine("</th>");
                // sb.AppendLine("    </tr>");
                // sb.AppendLine("</tfoot>");
                sb.AppendLine("</table>");

                return sb.ToString();
            }

        }


        public static void DataTableTest()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < 10; ++i)
            {
                dt.Columns.Add("Col " + i.ToString(), typeof(string));
                DataColumn dc = dt.Columns[i];
                System.Console.WriteLine(dc);
                System.Console.WriteLine(dc.Ordinal);
            }

            System.Console.WriteLine(dt.Columns);
            // dt.Columns[5].SetOrdinal(9);
            System.Console.WriteLine(dt.Columns);

            for (int i = 0; i < 10; ++i)
            {
                DataRow dr = dt.NewRow();

                foreach (DataColumn dc in dt.Columns)
                {
                    System.Console.WriteLine(dc.ColumnName);
                    object obj = dr[dc.ColumnName];
                    System.Console.WriteLine(obj);
                }

                for (int j = 0; j < dt.Columns.Count; ++j)
                {
                    dr[dt.Columns[j].ColumnName] = string.Format("Row {0} Column {1}", i, j);
                    dr[dt.Columns[j].ColumnName] = System.DateTime.Now;
                }

                System.Console.WriteLine(dr);
                dt.Rows.Add(dr);
            }

            System.Console.WriteLine(dt.Rows);
            dt.Columns[5].SetOrdinal(9);
            System.Console.WriteLine(dt.Rows);

            object cellValue = dt.Rows[9][9];
            System.Console.WriteLine(cellValue);

            string str = dt.ToHtml("myId", "MyClass");
            System.Console.WriteLine(str);
        }


        public static void InsertLogo()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            


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


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            // DataTable dt = CoinBaseSharp.HttpClientHelper.Deserialize<DataTable>().Result;

            // https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
            // http://www.symbolsource.org/Public/Metadata/NuGet/Project/CSLA-Core/4.5.700/Release/.NETCore,Version%3Dv4.5/Csla/Csla/Csla.WinRT/Reflection/TypeExtensions.cs?ImageName=Csla
            // https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
            // http://stackoverflow.com/questions/35370384/how-to-get-declared-and-inherited-members-from-typeinfo
            System.Type t = typeof(DataTable);
            // return t.GetTypeInfo().GetDeclaredField(fieldName);
            // GetTypeInfo().IsSubclassOf.
            // order to get access to the additional type information you have to invoke an extension method called GetTypeInfo() 
            // that lives in System.Reflection. 
            // typeof(DataTable).Assembly.Location



            DataTableTest();
            

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
