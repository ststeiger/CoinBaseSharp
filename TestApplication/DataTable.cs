
namespace System.Data2
{


    public class DataRow
    {
        public DataTable Table;

        protected System.Collections.Generic.Dictionary<string, object> m_ColumnData;


        internal DataRow()
            : this(null)
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
                if (this.m_ColumnData.ContainsKey(columnName))
                    return m_ColumnData[columnName];

                return System.DBNull.Value;
            }

            set
            {
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

    } // End Class DataRow 


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


    } // End Class DataColumn 




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
            get
            {
                return this.Rows.Count;
            }
        }


        public void Clear()
        {
            this.Rows.Clear();
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
    } // End Class DataRowCollection


    public class DataColumnCollection : System.Collections.Generic.IEnumerable<DataColumn>
    {
        protected DataTable m_Table;
        protected System.Collections.Generic.List<DataColumn> m_Columns; // Count


        public DataColumnCollection()
            : this(null)
        { }

        public DataColumnCollection(DataTable table)
        {
            this.m_Table = table;
            this.m_Columns = new System.Collections.Generic.List<DataColumn>();
        }


        public int Count
        {
            get
            {
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


        public void Clear()
        {
            this.m_Columns.Clear();
        }


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


    } // End Class DataColumnCollection



    public class DataTable
    {

        public string NameSpace;
        public string TableName;
        public System.Globalization.CultureInfo Culture;
        public bool CaseSensitive;

        public DataSet @DataSet; 

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


        public void Clear()
        {
            // this.Columns.Clear(): // ?? 
            this.Rows.Clear();
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

    } // End Class DataTable 




    public class DataTableCollection : System.Collections.Generic.IEnumerable<DataTable>
    {
        protected DataSet m_DataSet;
        protected System.Collections.Generic.List<DataTable> m_Tables;


        public DataTableCollection()
            : this(null)
        { }

        public DataTableCollection(DataSet dataSet)
        {
            this.m_DataSet = dataSet;
            this.m_Tables = new System.Collections.Generic.List<DataTable>();
        }

        public int Count
        {
            get
            {
                return this.m_Tables.Count;
            }
        }


        public void Clear()
        {
            this.m_Tables.Clear();
        }

        public DataTable Add()
        {
            DataTable dt = new DataTable();
            dt.DataSet = this.m_DataSet;
            this.m_Tables.Add(dt);

            return dt;
        }


        public void AddRange(DataTable[] dts)
        {
            for(int i = 0; i < dts.Length; ++i)
            {
                dts[i].DataSet = this.m_DataSet;
                this.m_Tables.Add(dts[i]);

            }

        }


        public System.Collections.Generic.IEnumerator<DataTable> GetEnumerator()
        {
            return this.m_Tables.GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public DataTable this[int index]
        {
            get
            {
                return this.m_Tables[index];
            }
        }


        public int GetOrdinal(string tableName)
        {
            int ord = this.m_Tables.FindIndex(delegate(DataTable that)
                {
                    if (this.m_DataSet.CaseSensitive)
                        return string.Equals(that.TableName, tableName, System.StringComparison.Ordinal);

                    return string.Equals(that.TableName, tableName, System.StringComparison.OrdinalIgnoreCase);
                }
            );

            return ord;
        }



        public DataTable this[string tableName]
        {
            get
            {
                int ord = this.GetOrdinal(tableName);
                return this[ord];
            }
        }


    } // End Class DataTableCollection


    public class DataSet
    {
        public bool CaseSensitive;
        public DataTableCollection Tables;
        public string Namespace;
        public string DataSetName;

        public System.Globalization.CultureInfo Locale;



        public DataSet()
            : this(null, null)
        { }


        public DataSet(string dataSetName)
            : this(dataSetName, null)
        {
        }


        public DataSet(string dataSetName, string dataSetNamespace)
        {
            this.DataSetName = dataSetName;
            this.Namespace = dataSetNamespace;
        }

        public void WriteXml(System.IO.Stream stream)
        {
            throw new System.NotImplementedException();
        }


        public void Clear()
        {
            this.Tables.Clear();
        }

    } // End Class DataSet 


    public class Tests
    {
        
        // https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
        // http://www.symbolsource.org/Public/Metadata/NuGet/Project/CSLA-Core/4.5.700/Release/.NETCore,Version%3Dv4.5/Csla/Csla/Csla.WinRT/Reflection/TypeExtensions.cs?ImageName=Csla
        // https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
        // http://stackoverflow.com/questions/35370384/how-to-get-declared-and-inherited-members-from-typeinfo
        // System.Type t = typeof(DataTable);
        // return t.GetTypeInfo().GetDeclaredField(fieldName);
        // GetTypeInfo().IsSubclassOf.
        // order to get access to the additional type information you have to invoke an extension method called GetTypeInfo() 
        // that lives in System.Reflection. 
        // typeof(DataTable).Assembly.Location


        public static void DataSetTest()
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            // ds.Tables.Count
            var dt1 = ds.Tables["foo"];
            var dt2 = ds.Tables[0];


            // DataTable[]

            //ds.Tables.AddRange();
            //ds.Tables.Add();

            // ds.HasErrors

            // ds.WriteXml
            // ds.WriteXmlSchema
            // ds.Load

            foreach (System.Data.DataTable dt in ds.Tables)
            {
                System.Console.WriteLine(dt);
                // dt.Locale
            }

        } // End Sub DataSetTest 


        public void Fill(DataTable dt)
        {
            System.Data.DataTable msDT = null;

            using (System.Data.Common.DbDataAdapter da = new Data.SqlClient.SqlDataAdapter("cmd", "conn"))
            {
                da.Fill(msDT);
            } // End Using da
            
            dt = Sql2DataTableTest("SQL");
        } // End Sub Fill 


        public static DataTable Sql2DataTableTest(string SQL)
        {
            DataTable dt = null;
            return Sql2DataTableTest(SQL, dt);
        } // End Function Sql2DataTableTest 


        public static DataTable Sql2DataTableTest(string SQL, DataTable dt)
        {

            using(System.Data.Common.DbCommand cmd = CoinBaseSharp.SQL.CreateCommand(SQL))
            {
                dt = Sql2DataTableTest(cmd, dt);
            } // End Using cmd 

            System.Console.WriteLine(dt.Rows.Count);
            return dt;
        } // End Function Sql2DataTableTest 


        public static DataTable Sql2DataTableTest(System.Data.Common.DbCommand cmd, DataTable dt)
        {
            if(dt == null)
                dt = new DataTable();
            //else dt.Clear();

            using (System.Data.Common.DbDataReader rdr = CoinBaseSharp.SQL.ExecuteReader(cmd, Data.CommandBehavior.SequentialAccess | Data.CommandBehavior.CloseConnection))
            {
                int fieldCount = rdr.FieldCount;

                string[] fieldNames = new string[fieldCount];
                System.Type[] fieldTypes = new System.Type[fieldCount];
                object[] objs = new object[fieldCount];

                for (int i = 0; i < fieldCount; ++i)
                {
                    fieldNames[i] = rdr.GetName(i);
                    fieldTypes[i] = rdr.GetFieldType(i);
                    dt.Columns.Add(fieldNames[i], fieldTypes[i]);
                } // Next i 


                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        DataRow dr = dt.NewRow();

                        for (int i = 0; i < fieldCount; ++i)
                        {
                            object obj = rdr.GetValue(i);

                            if (obj == null)
                                dr[fieldNames[i]] = System.DBNull.Value;
                            else
                                dr[fieldNames[i]] = obj;
                        } // Next i 

                        dt.Rows.Add(dr);
                    } // Whend 

                } // End if (rdr.HasRows) 

            } // End Using rdr 

            // string str = dt.ToHtml();
            // System.Console.WriteLine(str);
            // System.Console.WriteLine(dt.Rows.Count);
            return dt;
        } // End Function Sql2DataTableTest 


        public class DrawingInfo{
            // http://stackoverflow.com/questions/1528525/alternatives-to-system-drawing-for-use-with-asp-net
            // http://stackoverflow.com/questions/390532/system-drawing-in-windows-or-asp-net-services
            // https://github.com/StackExchange/dapper-dot-net/issues/241
            // https://github.com/mono/mono/blob/master/mcs/class/referencesource/System.Data/System/Data/Odbc/Odbc32.cs
            // https://github.com/mono/mono/tree/master/mcs/class/referencesource/System.Data/System/Data/Odbc
            // https://github.com/mono/mono/blob/master/mcs/class/System.Data/Test/OdbcTest.cs

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


    }


}
