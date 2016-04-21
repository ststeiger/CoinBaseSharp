
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


            CoinBaseSharp.SQL.BatchedInsert(hist.data.prices, ItemToSqlInsert);



            CoinBaseSharp.API.V2.ServerTime.RootObject time = EasyJSON.JsonHelper.DeserializeEmbeddedFile<CoinBaseSharp.API.V2.ServerTime.RootObject>("time.txt");
            System.Console.WriteLine(time.data.epoch);
            System.Console.WriteLine(time.data.iso);
            System.Console.WriteLine(time.warnings.Count);



            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();

        } // End Sub Main 

        // public static void ItemToSqlInsert<T>(System.Text.StringBuilder sb, T thisPrice)


        public static void ItemToSqlInsert(System.Text.StringBuilder sb, CoinBaseSharp.API.V2.History.Price thisPrice)
        {
            sb.Append("INSERT INTO T_FOO(uid, time, price) VALUES (");
            sb.Append(System.Guid.NewGuid().ToString());
            sb.Append(", ");

            if(thisPrice.time == null)
                sb.Append("NULL");
            else
                sb.Append(thisPrice.time);

            sb.Append(", ");

            if(thisPrice.price == null)
                sb.Append("NULL");
            else
                sb.Append(thisPrice.price);
            
            sb.AppendLine(");");
        }


    } // End Class Program 


} // End Namespace TestApplication
