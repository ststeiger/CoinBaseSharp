
namespace CoinBaseSharp
{


    public class SQL
    {

        public class ColumnInfo
        {
            public string ColumnName;
            public System.Type FieldType;
        } // End Class ColumnInfo


        public class Table
        {
            public string Name;
            public System.Collections.Generic.List<ColumnInfo> Columns;
            public System.Collections.Generic.List<object[]> Rows;


            public Table()
            {
                this.Columns = new System.Collections.Generic.List<ColumnInfo>();
                this.Rows = new System.Collections.Generic.List<object[]>();
            } // End Constructor 

        } // End Class Table


        public class DataSetSerialization
        {
            public System.Collections.Generic.List<Table> Tables;
                

            public DataSetSerialization()
            {
                this.Tables = new System.Collections.Generic.List<Table>();
            } // End Constructor 

        } // End Class DataSetSerialization

        public static string GetMultipleDataSetsSQL()
        {
            string strSQL = @"
SELECT TOP 10 * FROM price_history; 
SELECT TOP 10 * FROM t_currency; 

-- SELECT * FROM price_history LIMIT 10; 
-- SELECT * FROM t_currency LIMIT 10; 

-- SELECT * FROM price_history OFFSET 0 FETCH NEXT 10 ROWS ONLY;
-- SELECT * FROM t_currency OFFSET 0 FETCH NEXT 10 ROWS ONLY; 
";

            return strSQL;
        }


        public static void MultipleDataSets()
        {
            MultipleDataSets(GetMultipleDataSetsSQL());
        }

        public static void MultipleDataSets(string strSQL)
        {
            DataSetSerialization ser = new DataSetSerialization();

            using (System.Data.Common.DbDataReader dr = SQL.ExecuteReader(strSQL
                , System.Data.CommandBehavior.CloseConnection 
                | System.Data.CommandBehavior.SequentialAccess
                )
            )
            {
                Table tbl = null;

                do
                {
                    tbl = new Table();

                    for (int i = 0; i < dr.FieldCount; ++i)
                    {

                        tbl.Columns.Add(
                            new ColumnInfo()
                            {
                                ColumnName = dr.GetName(i),
                                FieldType = dr.GetFieldType(i)
                            }
                        );

                    } // Next i 
                    
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            object[] thisRow = new object[dr.FieldCount];

                            for (int i = 0; i < dr.FieldCount; ++i)
                            {
                                thisRow[i] = dr.GetValue(i);
                            } // Next i
                            
                            tbl.Rows.Add(thisRow);
                        } // Whend 

                    } // End if (dr.HasRows) 

                    ser.Tables.Add(tbl);
                } while (dr.NextResult()); 

            } // End Using dr 

            string str = EasyJSON.JsonHelper.SerializePretty(ser);
            System.Console.WriteLine(str);

            DataSetSerialization ser2 = EasyJSON.JsonHelper.Deserialize<DataSetSerialization>(str);
            System.Console.WriteLine(ser2);
        } // End Sub MultipleDataSets 



        public static void PostJSON(string url, System.Net.Http.HttpMethod method, object obj)
        {
            if(obj == null || obj == System.DBNull.Value)
                throw new System.ArgumentNullException("obj");
            // url = "http://localhost:53417/ajax/dataReceiver.ashx";

            Newtonsoft.Json.JsonSerializer sr = new Newtonsoft.Json.JsonSerializer();

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Headers.Add("Content-Type","application/json");

                using (System.IO.Stream postStream = client.OpenWrite(url, method.Method))
                {
                    sr.Formatting = Newtonsoft.Json.Formatting.Indented;
                    sr.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                    // sr.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified;

                    using (System.IO.TextWriter tw = new System.IO.StreamWriter(postStream, System.Text.Encoding.UTF8))
                    {
                        sr.Serialize(tw, obj);
                    } // End Using tw 

                } // End Using postStream 

            } // End Using client 

        } // End Sub PostJSON 
            

        // https://github.com/mono/mono/blob/master/mcs/class/System.Web.Mvc3/Mvc/HttpVerbs.cs
        // http://www.ietf.org/rfc/rfc2616.txt
        [System.Flags]
        public enum HttpVerbs : int
        {
            GET = 1 << 0, // Section 9.3
            POST = 1 << 1, // Section 9.5
            PUT = 1 << 2, // Section 9.6
            DELETE = 1 << 3, // Section 9.7
            HEAD = 1 << 4, // Section 9.4
            OPTIONS = 1 << 5, // Section 9.2
            PATCH = 1 << 6,
            TRACE = 1 << 7, // Section 9.8
            CONNECT = 1 << 8 // Section 9.9
        }


        public static void MultipleLargeDataSets()
        {
            System.Net.Http.HttpMethod meth = System.Net.Http.HttpMethod.Post;

            string fn = "lobster.json.txt";


            using (System.IO.Stream strm = new System.IO.FileStream(fn, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                MultipleLargeDataSets(strm, GetMultipleDataSetsSQL());
            } // End Using strm

            string endpointUrl = "http://localhost:53417/ajax/dataReceiver.ashx";

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Headers.Add("Content-Type","application/json");


                // client.OpenWriteCompleted += (sender, e) =>
                client.OpenWriteCompleted += delegate (object sender, System.Net.OpenWriteCompletedEventArgs e)
                {
                    // System.Net.WebClient that = (System.Net.WebClient) sender;

                    if (e.Error != null)
                        throw e.Error;

                    using (System.IO.Stream postStream = e.Result)
                    {
                        MultipleLargeDataSets(postStream, GetMultipleDataSetsSQL());
                        postStream.Flush();
                    }

                    
                };

                client.OpenWriteAsync(new System.Uri( endpointUrl));
                
                
                using (System.IO.Stream postStream = client.OpenWrite(endpointUrl, meth.Method))
                {
                    // postStream.Write(fileContent, 0, fileContent.Length);
                    MultipleLargeDataSets(postStream, GetMultipleDataSetsSQL());
                } // End Using postStream 

                using (System.IO.Stream postStream = client.OpenRead(endpointUrl))
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(postStream))
                    {
                        string output = sr.ReadToEnd();
                        System.Console.WriteLine(output);
                    } // End Using sr 

                } // End Using postStream 


                // client.ResponseHeaders

            } // End Using client 

            DataSetSerialization thisDataSet = EasyJSON.JsonHelper.DeserializeFromFile<DataSetSerialization>(fn);
            System.Console.WriteLine(thisDataSet);
        } // End Sub MultipleLargeDataSets 


        public static void MultipleLargeDataSets(System.IO.Stream strm, string strSQL)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

            using (System.IO.StreamWriter output = new System.IO.StreamWriter(strm))
            {
                
                using (Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(output))
                {
                    jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                    jsonWriter.WriteStartObject();

                    jsonWriter.WritePropertyName("Tables");
                    jsonWriter.WriteStartArray();


                    using (System.Data.Common.DbDataReader dr = SQL.ExecuteReader(strSQL
                , System.Data.CommandBehavior.CloseConnection
                                                                | System.Data.CommandBehavior.SequentialAccess
                                                            ))
                    {
                    
                        do
                        {
                            jsonWriter.WriteStartObject(); // tbl = new Table();

                            jsonWriter.WritePropertyName("Columns");
                            jsonWriter.WriteStartArray();


                            for (int i = 0; i < dr.FieldCount; ++i)
                            {
                                jsonWriter.WriteStartObject();

                                jsonWriter.WritePropertyName("ColumnName");
                                jsonWriter.WriteValue(dr.GetName(i));

                                jsonWriter.WritePropertyName("FieldType");
                                jsonWriter.WriteValue(dr.GetFieldType(i).AssemblyQualifiedName);


                                jsonWriter.WriteEndObject();
                            } // Next i 
                            jsonWriter.WriteEndArray();
                      
                            jsonWriter.WritePropertyName("Rows");
                            jsonWriter.WriteStartArray();

                            if (dr.HasRows)
                            {

                                while (dr.Read())
                                {
                                    object[] thisRow = new object[dr.FieldCount];

                                    jsonWriter.WriteStartArray(); // object[] thisRow = new object[dr.FieldCount];
                                    for (int i = 0; i < dr.FieldCount; ++i)
                                    {
                                        jsonWriter.WriteValue(dr.GetValue(i));
                                    } // Next i
                                    jsonWriter.WriteEndArray(); // tbl.Rows.Add(thisRow);
                                } // Whend 

                            } // End if (dr.HasRows) 

                            jsonWriter.WriteEndArray();

                            jsonWriter.WriteEndObject(); // ser.Tables.Add(tbl);
                        } while (dr.NextResult()); 

                    } // End Using dr 

                    jsonWriter.WriteEndArray();

                    jsonWriter.WriteEndObject();

                    jsonWriter.Flush();
                    output.Flush();
                    output.BaseStream.Flush();

                    // context.Response.Output.Flush();
                    // context.Reponse.OutputStream.Flush();
                    // context.Response.Flush();
                } // End Using jsonWriter 

            } // End using output

        } // End Sub MultipleLargeDataSets 


        public static System.Data.Common.DbProviderFactory GetFactory(System.Type type)
        {
            

            if (type != null && System.Reflection.IntrospectionExtensions.GetTypeInfo(type)
                .IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))
            {
                // Provider factories are singletons with Instance field having
                // the sole instance

                

                System.Reflection.FieldInfo field = System.Reflection.IntrospectionExtensions.GetTypeInfo(type)
                    .GetField("Instance"
                    , System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
                );

                if (field != null)
                {
                    return (System.Data.Common.DbProviderFactory)field.GetValue(null);
                    //return field.GetValue(null) as DbProviderFactory;
                } // End if (field != null)

            } // End if (type != null && type.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))

            throw new System.Exception("DataProvider is missing!");
            //throw new System.Configuration.ConfigurationException("DataProvider is missing!");
        } // End Function GetFactory


        public static System.Data.Common.DbProviderFactory InitializeFactory()
        {
            GetConnectionString();

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(m_DataProvider, typeof(System.Data.SqlClient.SqlClientFactory).Namespace))
                return GetFactory(typeof(System.Data.SqlClient.SqlClientFactory));

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(m_DataProvider, typeof(Npgsql.NpgsqlFactory).Namespace))
                return GetFactory(typeof(Npgsql.NpgsqlFactory));
            
            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(m_DataProvider, typeof(MySql.Data.MySqlClient.MySqlClientFactory).Namespace))
                return GetFactory(typeof(MySql.Data.MySqlClient.MySqlClientFactory));
            
            return GetFactory(typeof(System.Data.SqlClient.SqlClientFactory));
        }


        public static System.Data.Common.DbProviderFactory m_fact = InitializeFactory();

        public enum DbType_t : int
        {
            MS_SQL,
            PostgreSQL,
            MySQL,
            Firebird,
            Interbase,
            Oracle,
            DB2,
            Sybase, 
            SQLite,


        }

        private static DbType_t? m_dbType;

        public static DbType_t DbType
        {
            get{ 
                if (m_dbType.HasValue)
                    return m_dbType.Value;

                System.Type t = m_fact.GetType();

                if (object.ReferenceEquals(t, typeof(System.Data.SqlClient.SqlClientFactory)))
                    m_dbType = DbType_t.MS_SQL;
                else if(object.ReferenceEquals(t, typeof(Npgsql.NpgsqlFactory)))
                    m_dbType = DbType_t.PostgreSQL;
                else if(object.ReferenceEquals(t, typeof(MySql.Data.MySqlClient.MySqlClientFactory)))
                    m_dbType = DbType_t.MySQL;
                
                if(!m_dbType.HasValue)
                    m_dbType = DbType_t.MS_SQL;

                return m_dbType.Value;
            }
        }


        public static bool Log(System.Exception ex)
        {
            return Log(ex, null);
        } // End Function Log 


        public static bool Log(System.Exception ex, System.Data.IDbCommand cmd)
        {
            return Log(null, ex, cmd);
        } // End Function Log 


        public static bool Log(string location, System.Exception ex, System.Data.IDbCommand cmd)
        {
            if (location != null)
                Notify(location);

            Notify(ex.Message);

            if (cmd != null)
                Notify(cmd.CommandText);

            return true;
        } // End Function Log 


        public static void Notify(object obj)
        {
            string text = "NULL";
            if (obj != null)
                text = obj.ToString();
            
            string caption = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // System.Windows.Forms.MessageBox.Show(text, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            System.Console.WriteLine(text);
        } // End Function Notify 



        public class Control
        {
            public System.Collections.Generic.List<Control> Children;

        }



        protected static string m_staticConnectionString;
        protected static string m_DataProvider;

        /*
        public static string GetMyConnectionString()
        {
            MySql.Data.MySqlClient.MySqlConnectionStringBuilder csb = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
            csb.Server = "127.0.0.1";
            csb.Database = "stackexchange";

            // MySql.Data.MySqlClient.MySqlClientFactory


            csb.UserID = "root";
            csb.Password = "";
            csb.Port = 3306;
            csb.Pooling = false;
            csb.PersistSecurityInfo = false;
            csb.ConvertZeroDateTime = false;
            csb.AllowZeroDateTime = false;
            csb.DefaultCommandTimeout = 30;
            return csb.ToString();
        }
        */

        public delegate void sqlGenerator_t<T>(System.Text.StringBuilder sb, T thisItem);


        public static int BatchedInsert<T>(System.Collections.Generic.IEnumerable<T> ls, sqlGenerator_t<T> sqlGenerator)
        {
            int iAffected = 0;
            int batchSize = 100; // Each batch corresponds to a single round-trip to the DB.

            using (System.Data.IDbConnection idbConn = GetConnection())
            {

                lock (idbConn)
                {

                    using (System.Data.IDbCommand cmd = idbConn.CreateCommand())
                    {

                        lock (cmd)
                        {
                            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                cmd.Connection.Open();

                            using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                            {

                                try
                                {
                                    cmd.Transaction = idbtTrans;


                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    int i = 0;
                                    foreach(T item in ls)
                                    {
                                        sqlGenerator(sb, item);
                                        if (i % batchSize == 0 && i != 0)
                                        {
                                            cmd.CommandText = sb.ToString();
                                            iAffected += cmd.ExecuteNonQuery();
                                            sb.Length = 0;
                                        }
                                        ++i;
                                    }

                                    if (sb.Length != 0)
                                    {
                                        cmd.CommandText = sb.ToString();
                                        iAffected += cmd.ExecuteNonQuery();
                                    }

                                    idbtTrans.Commit();
                                } // End Try
                                catch (System.Data.Common.DbException ex)
                                {
                                    if (idbtTrans != null)
                                        idbtTrans.Rollback();

                                    iAffected = -1;

                                    //if (Log(ex))
                                    throw;
                                } // End catch
                                finally
                                {
                                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                        cmd.Connection.Close();
                                } // End Finally

                            } // End Using idbtTrans

                        } // End lock cmd

                    } // End Using cmd 

                } // End lock idbConn

            } // End Using idbConn

            return iAffected;
        } // End Function BatchedInsert 


        public static string GetConnectionString()
        {
            string strReturnValue = null;

            if (string.IsNullOrEmpty(m_staticConnectionString))
            {
                string strConnectionStringName = System.Environment.MachineName;

                if (string.IsNullOrEmpty(strConnectionStringName))
                {
                    strConnectionStringName = "LocalSqlServer";
                }

                System.Configuration.ConnectionStringSettingsCollection settings = System.Configuration.ConfigurationManager.ConnectionStrings;
                if ((settings != null))
                {
                    foreach (System.Configuration.ConnectionStringSettings cs in settings)
                    {
                        if (System.StringComparer.OrdinalIgnoreCase.Equals(cs.Name, strConnectionStringName))
                        {
                            strReturnValue = cs.ConnectionString;
                            m_staticConnectionString = strReturnValue;
                            m_DataProvider = cs.ProviderName;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }

                if (string.IsNullOrEmpty(strReturnValue))
                {
                    strConnectionStringName = "server";

                    System.Configuration.ConnectionStringSettings conString = System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionStringName];

                    if (conString != null)
                    {
                        strReturnValue = conString.ConnectionString;
                    }
                }

                if (string.IsNullOrEmpty(strReturnValue))
                {
                    throw new System.ArgumentNullException("ConnectionString \"" + strConnectionStringName + "\" in file web.config.");
                }

                settings = null;
                strConnectionStringName = null;
            }
            else // of if (string.IsNullOrEmpty(strStaticConnectionString))
            {
                return m_staticConnectionString;
            }

            return strReturnValue;
        } // End Function GetConnectionString


        public static System.Data.Common.DbConnection GetConnection()
        {
            System.Data.Common.DbConnection dbConnection = m_fact.CreateConnection();
            dbConnection.ConnectionString = GetConnectionString();

            return dbConnection;
        } // End Function GetConnection 


        public static System.Data.Common.DbCommand CreateCommand()
        {
            return CreateCommand(null);
        } // End Function CreateCommand


        public static System.Data.Common.DbCommand CreateCommand(string SQL)
        {
            return CreateCommand(SQL, 30);
        } // End Function CreateCommand


        private static byte[] ReadAllBytesShareRead(string fileName)
        {
            byte[] file;

            using (System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {

                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                } // End Using reader

            } // End Using stream 

            return file;
        }


        public static System.Data.Common.DbCommand CreateCommand(string SQL, int timeout)
        {

            System.Data.Common.DbCommand cmd = m_fact.CreateCommand();
            cmd.CommandText = SQL;
            cmd.CommandTimeout = timeout;

            return cmd;
        } // End Function CreateCommand


        public static void SaveFile(string fileName, string SQL)
        {
            SaveFile(fileName, SQL, null);
        } // End Sub SaveFile


        // https://stackoverflow.com/questions/2579373/saving-any-file-to-in-the-database-just-convert-it-to-a-byte-array
        public static void SaveFile(string fileName, string SQL, string paramName)
        {
            byte[] file = ReadAllBytesShareRead(fileName);
            SaveFile(file, SQL, paramName);
        } // End Sub SaveFile



        public static void SaveFile(byte[] file, string SQL)
        {
            SaveFile(file, SQL, null);
        } // End Sub SaveFile


        public static void SaveFile(byte[] file, string SQL, string paramName)
        {
            using (System.Data.Common.DbCommand cmd = CreateCommand(SQL))
            {
                SaveFile(file, cmd, paramName);
            } // End Using cmd 

        } // End Sub SaveFile



        public static void SaveFile(string fileName, System.Data.IDbCommand cmd)
        {
            SaveFile(fileName, cmd, null);
        }


        public static void SaveFile(string fileName, System.Data.IDbCommand cmd, string paramName)
        {
            byte[] file = ReadAllBytesShareRead(fileName);
            SaveFile(file, cmd, paramName);
        } // End Sub SaveFile


        public static void SaveFile(byte[] file, System.Data.IDbCommand cmd)
        {
            SaveFile(file, cmd, null);
        }


        public static void SaveFile(byte[] file, System.Data.IDbCommand cmd, string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                paramName = "__file";

            if (!paramName.StartsWith("@"))
                paramName = "@" + paramName;

            if (!cmd.Parameters.Contains(paramName))
                AddParameter(cmd, paramName, file);

            ExecuteNonQuery(cmd);
        } // End Sub SaveFile




        // http://stackoverflow.com/questions/2885335/clr-sql-assembly-get-the-bytestream
        // http://stackoverflow.com/questions/891617/how-to-read-a-image-by-idatareader
        // http://stackoverflow.com/questions/4103406/extracting-a-net-assembly-from-sql-server-2005
        public static void RetrieveFile(string sql, string path)
        {
            RetrieveFile(sql, path, "data");
        } // End Sub RetrieveFile 


        public static void RetrieveFile(string sql, string path, string columnName)
        {

            using (System.Data.IDbCommand cmd = CreateCommand(sql, 0))
            {
                RetrieveFile(cmd, columnName, path);
            } // End Using cmd 

        } // End Sub RetrieveFile 


        public static void RetrieveFile(System.Data.IDbCommand cmd, string path)
        {
            RetrieveFile(cmd, null, path);
        } // End Sub RetrieveFile 


        // http://stackoverflow.com/questions/2885335/clr-sql-assembly-get-the-bytestream
        // http://stackoverflow.com/questions/891617/how-to-read-a-image-by-idatareader
        // http://stackoverflow.com/questions/4103406/extracting-a-net-assembly-from-sql-server-2005
        public static void RetrieveFile(System.Data.IDbCommand cmd, string columnName, string path)
        {
            using (System.Data.IDataReader reader = ExecuteReader(cmd, System.Data.CommandBehavior.SequentialAccess | System.Data.CommandBehavior.CloseConnection))
            {
                bool hasRows = reader.Read();
                if (hasRows)
                {
                    const int BUFFER_SIZE = 1024 * 1024 * 10; // 10 MB
                    byte[] buffer = new byte[BUFFER_SIZE];

                    int col = string.IsNullOrEmpty(columnName) ? 0 : reader.GetOrdinal(columnName);
                    int bytesRead = 0;
                    int offset = 0;

                    // Write the byte stream out to disk
                    using (System.IO.FileStream bytestream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        while ((bytesRead = (int)reader.GetBytes(col, offset, buffer, 0, BUFFER_SIZE)) > 0)
                        {
                            bytestream.Write(buffer, 0, bytesRead);
                            offset += bytesRead;
                        } // Whend

                        bytestream.Close();
                    } // End Using bytestream 

                } // End if (!hasRows)

                reader.Close();
            } // End Using reader

        } // End Function RetrieveFile


        protected static T InlineTypeAssignHelper<T>(object UTO)
        {
            if (UTO == null)
            {
                T NullSubstitute = default(T);
                return NullSubstitute;
            }
            return (T)UTO;
        } // End Template InlineTypeAssignHelper


        public static T ExecuteScalar<T>(System.Data.IDbCommand cmd)
        {
            string strReturnValue = null;
            System.Type tReturnType = null;
            object objReturnValue = null;

            lock (cmd)
            {

                using (System.Data.IDbConnection idbc = GetConnection())
                {
                    cmd.Connection = idbc;

                    lock (cmd.Connection)
                    {

                        try
                        {
                            tReturnType = typeof(T);

                            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                cmd.Connection.Open();

                            objReturnValue = cmd.ExecuteScalar();

                            if (objReturnValue != null)
                            {

                                if (!object.ReferenceEquals(tReturnType, typeof(System.Byte[])))
                                {
                                    strReturnValue = objReturnValue.ToString();
                                    objReturnValue = null;
                                } // End if (!object.ReferenceEquals(tReturnType, typeof(System.Byte[])))

                            } // End if (objReturnValue != null)

                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            if (Log("ExecuteScalar<T>(System.Data.IDbCommand cmd) - 1", ex, cmd))
                                throw;
                        
                        } // End Catch
                        finally
                        {
                            if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                cmd.Connection.Close();
                        } // End Finally

                    } // End lock (cmd.Connection)

                } // End using idbc

            } // End lock (cmd)


            try
            {
                if (object.ReferenceEquals(tReturnType, typeof(object)))
                {
                    return InlineTypeAssignHelper<T>(objReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(string)))
                {
                    return InlineTypeAssignHelper<T>(strReturnValue);
                } // End if string
                else if (object.ReferenceEquals(tReturnType, typeof(bool)))
                {
                    bool bReturnValue = false;
                    bool bSuccess = bool.TryParse(strReturnValue, out bReturnValue);

                    if (bSuccess)
                        return InlineTypeAssignHelper<T>(bReturnValue);

                    if (strReturnValue == "0")
                        return InlineTypeAssignHelper<T>(false);

                    return InlineTypeAssignHelper<T>(true);
                } // End if bool
                else if (object.ReferenceEquals(tReturnType, typeof(int)))
                {
                    int iReturnValue = int.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(iReturnValue);
                } // End if int
                else if (object.ReferenceEquals(tReturnType, typeof(uint)))
                {
                    uint uiReturnValue = uint.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(uiReturnValue);
                } // End if uint
                else if (object.ReferenceEquals(tReturnType, typeof(long)))
                {
                    long lngReturnValue = long.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(lngReturnValue);
                } // End if long
                else if (object.ReferenceEquals(tReturnType, typeof(ulong)))
                {
                    ulong ulngReturnValue = ulong.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(ulngReturnValue);
                } // End if ulong
                else if (object.ReferenceEquals(tReturnType, typeof(float)))
                {
                    float fltReturnValue = float.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(fltReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(double)))
                {
                    double dblReturnValue = double.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(dblReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(System.Net.IPAddress)))
                {
                    System.Net.IPAddress ipaAddress = null;

                    if (string.IsNullOrEmpty(strReturnValue))
                        return InlineTypeAssignHelper<T>(ipaAddress);

                    ipaAddress = System.Net.IPAddress.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(ipaAddress);
                } // End if IPAddress
                else if (object.ReferenceEquals(tReturnType, typeof(System.Byte[])))
                {
                    if (objReturnValue == System.DBNull.Value)
                        return InlineTypeAssignHelper<T>(null);

                    return InlineTypeAssignHelper<T>(objReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(System.Guid)))
                {
                    if (string.IsNullOrEmpty(strReturnValue))
                        return InlineTypeAssignHelper<T>(null);

                    return InlineTypeAssignHelper<T>(new System.Guid(strReturnValue));
                } // End if System.Guid
                else // No datatype matches
                {
                    throw new System.NotImplementedException("ExecuteScalar<T>: This type is not yet defined.");
                } // End else of if tReturnType = datatype

            } // End Try
            catch (System.Exception ex)
            {
                if (Log("ExecuteScalar<T>(System.Data.IDbCommand cmd) - 2", ex, cmd))
                    throw;
            } // End Catch

            return InlineTypeAssignHelper<T>(null);
        } // End Function ExecuteScalar(cmd)


        public static T ExecuteScalar<T>(string strSQL)
        {
            T tReturnValue = default(T);

            // pfff, mono C# compiler problem...
            //sqlCMD = new System.Data.SqlClient.SqlCommand(strSQL, m_SqlConnection);
            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                tReturnValue = ExecuteScalar<T>(cmd);
            } // End Using cmd

            return tReturnValue;
        } // End Function ExecuteScalar(strSQL)


        public static int ExecuteNonQuery(string SQL)
        {
            int iAffected = 0;
            using (System.Data.Common.DbCommand cmd = CreateCommand(SQL))
            {
                iAffected = ExecuteNonQuery(cmd);
            } // End Using cmd 

            return iAffected;
        } // End Function ExecuteNonQuery 


        public static int ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            int iAffected = -1;

            try
            {

                using (System.Data.IDbConnection idbConn = GetConnection())
                {

                    lock (idbConn)
                    {

                        lock (cmd)
                        {

                            cmd.Connection = idbConn;

                            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                cmd.Connection.Open();

                            using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                            {

                                try
                                {
                                    cmd.Transaction = idbtTrans;

                                    iAffected = cmd.ExecuteNonQuery();
                                    idbtTrans.Commit();
                                } // End Try
                                catch (System.Data.Common.DbException ex)
                                {
                                    if (idbtTrans != null)
                                        idbtTrans.Rollback();

                                    iAffected = -2;

                                    if (Log(ex, cmd))
                                        throw;
                                } // End catch
                                finally
                                {
                                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                        cmd.Connection.Close();
                                } // End Finally

                            } // End Using idbtTrans

                        } // End lock cmd

                    } // End lock idbConn

                } // End Using idbConn

            } // End Try
            catch (System.Exception ex)
            {
                iAffected = -3;
                if (Log(ex, cmd))
                    throw;
            }
            finally
            {

            }

            return iAffected;
        } // End Function ExecuteNonQuery 


        public static System.Data.Common.DbDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, System.Data.CommandBehavior.CloseConnection);
        }


        public static System.Data.Common.DbDataReader ExecuteReader(string sql, System.Data.CommandBehavior behaviour)
        {
            System.Data.Common.DbDataReader dr;

            using (System.Data.IDbCommand cmd = CreateCommand(sql))
            {
                dr = ExecuteReader(cmd, behaviour);
            } // End Using cmd

            return dr;
        }


        public static System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd)
        {
            return ExecuteReader(cmd, System.Data.CommandBehavior.CloseConnection);
        }


        public static System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd, System.Data.CommandBehavior behav)
        {
            System.Data.IDataReader idr = null;

            lock (cmd)
            {
                System.Data.IDbConnection idbc = GetConnection();
                cmd.Connection = idbc;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();

                try
                {
                    idr = cmd.ExecuteReader(behav);
                }
                catch (System.Exception ex)
                {
                    if (Log(ex, cmd))
                        throw;
                }
            } // End Lock cmd

            return (System.Data.Common.DbDataReader)  idr;
        } // End Function ExecuteReader


        public virtual System.Data.DataTable GetDataTable(string strSQL)
        {
            System.Data.DataTable dt = null;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                dt = GetDataTable(cmd);
            } // End Using cmd

            return dt;
        } // End Function GetDataTable


        public virtual System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.Common.DbConnection idbc = GetConnection())
            {

                lock (idbc)
                {

                    lock (cmd)
                    {

                        try
                        {
                            cmd.Connection = idbc;

                            using (System.Data.Common.DbDataAdapter daQueryTable = m_fact.CreateDataAdapter())
                            {
                                daQueryTable.SelectCommand = (System.Data.Common.DbCommand)cmd;
                                daQueryTable.Fill(dt);
                            } // End Using daQueryTable

                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            if (Log("SQL.GetDataTable(System.Data.IDbCommand cmd)", ex, cmd))
                                throw;
                        }// End Catch
                        finally
                        {
                            if (idbc.State != System.Data.ConnectionState.Closed)
                                idbc.Close();
                        } // End Finally

                    } // End lock cmd

                } // End lock idbc

            } // End Using idbc

            return dt;
        } // End Function GetDataTable


        // From Type to DBType
        protected static System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (DBtype == System.Data.DbType.UInt64)
                    DBtype = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // add error handling to suit your taste
            }

            return DBtype;
        } // End Function GetDbType


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            } // End if (objValue == null)

            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            } // End if (!strParameterName.StartsWith("@"))

            parameter.ParameterName = strParameterName;
            parameter.DbType = dbType;
            parameter.Direction = pad;

            // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
            // No association  DbType UInt64 to a known SqlDbType

            if (objValue == null)
                parameter.Value = System.DBNull.Value;
            else
                parameter.Value = objValue;

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter


    } // End Internal Class SQL

}
