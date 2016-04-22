
namespace TestApplication
{


    static class Program
    {


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
            CoinBaseSharp.SQL.BatchedInsert(hist.data.prices, delegate(System.Text.StringBuilder sb, CoinBaseSharp.API.V2.History.Price thisPrice) {
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
                sb.Append(thisPrice.price.Replace("'","''"));
                sb.Append("'");
            }
            
            sb.AppendLine(");");
        } // End Sub SqlInsertItem 


    } // End Class Program 


} // End Namespace TestApplication
