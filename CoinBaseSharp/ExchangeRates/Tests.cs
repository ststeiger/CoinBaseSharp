
namespace CoinBaseSharp.ExchangeRates
{


    public class Tests
    {


        public static string MapProjectPath(string str)
        {
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dir = System.IO.Path.Combine(dir, "../../..");
            dir = System.IO.Path.GetFullPath(dir);

            str = str.Replace('\\', System.IO.Path.DirectorySeparatorChar);

            if (str.StartsWith("~"))
            {
                str = str.Substring(1);
                return System.IO.Path.Combine(dir, str);
            } // End if (str.StartsWith("~")) 

            return str;
        } // End Function MapProjectPath 



        // http://api.fixer.io/latest
        // http://api.fixer.io/2000-01-03
        public class Fixer
        {


            public static void Test()
            {
                FixerData fxd = JilHelper.DeserializeUrl<FixerData>("http://api.fixer.io/latest");
                System.Console.WriteLine(fxd);
            } // End Sub Test 

            
        } // End Class Fixer 


        public class ECB
        {
            public static void Test()
            {
                string url = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
                string fileName = MapProjectPath(@"~CoinBaseSharp/ExchangeRates/ecb_feed.xml");
                System.Console.WriteLine(fileName);

                // global::Tools.XML.Serialization.DeserializeXmlFromUrl
                // vs. CoinBaseSharp.Tools
                CoinBaseSharp.ExchangeRates.ECB.Envelope env =
                    Tools.XML.Serialization.DeserializeXmlFromUrl<CoinBaseSharp.ExchangeRates.ECB.Envelope>(url);
                    // Tools.XML.Serialization.DeserializeXmlFromFile<CoinBaseSharp.ExchangeRates.ECB.Envelope>(fileName);
                System.Console.WriteLine(env);
            } // End Sub Test 

        } // End Class ECB 


        // https://docs.openexchangerates.org/docs/historical-json
        public class OpenExchangeRates
        {


            // CoinBaseSharp.ExchangeRates.Test();
            public static void Test()
            {
                // https://docs.google.com/document/d/13rwJuIzL22bUILemGlZ6zf64Ej0CayPnKBcz1-zKjO0
                string sql = @"
SELECT 
	-- api_uid 
	--,api_name 
	--,api_app_id 
	--,api_href, 
	REPLACE(api_href, '{@api_app_id}', api_app_id) AS api_url 
	--,api_comment 
FROM t_api_configurations 
";

                string url = SQL.ExecuteScalar<string>(sql);
                System.Console.WriteLine(url);

                string fileName = MapProjectPath(@"~CoinBaseSharp\ExchangeRates\OpenExchangeRates_Data.txt");
                //string fileName = MapProjectPath(@"~CoinBaseSharp\ExchangeRates\OpenExchangeRates_Error.txt");
                System.Console.WriteLine(fileName);


                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                OpenExchangeRatesData oed = EasyJSON.JsonHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                sw.Stop();
                System.Console.WriteLine(sw.Elapsed);
                System.Console.WriteLine(oed);  


#if BENCHMARK || false
                // OpenExchangeRatesData oed = JilHelper.DeserializeUrl<OpenExchangeRatesData>(url);
                System.Diagnostics.Stopwatch swJil1 = new System.Diagnostics.Stopwatch();
                swJil1.Start();
                OpenExchangeRatesData oedJil = JilHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                swJil1.Stop();
                System.Console.WriteLine(swJil1.Elapsed);
                System.Console.WriteLine(oedJil);


                System.Diagnostics.Stopwatch swJil = new System.Diagnostics.Stopwatch();
                swJil.Start();
                OpenExchangeRatesData oedJil2 = JilHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                swJil.Stop();
                System.Console.WriteLine(swJil.Elapsed);
                System.Console.WriteLine(oedJil2); 


                    System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
                    sw2.Start();
                    OpenExchangeRatesData oed2 = ServiceStackHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                    sw2.Stop();
                    System.Console.WriteLine(sw2.Elapsed);
                    System.Console.WriteLine(oed2);

                    System.Diagnostics.Stopwatch sw3 = new System.Diagnostics.Stopwatch();
                    sw3.Start();
                    OpenExchangeRatesData oed3 = EasyJSON.JsonHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                    sw3.Stop();
                    System.Console.WriteLine(sw3.Elapsed);
                    System.Console.WriteLine(oed3);



                    System.Diagnostics.Stopwatch sw4 = new System.Diagnostics.Stopwatch();
                    sw4.Start();
                    OpenExchangeRatesData oed4 = ServiceStackHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                    sw4.Stop();
                    System.Console.WriteLine(sw4.Elapsed);
                    System.Console.WriteLine(oed4);
#endif


                if (oed.error)
                {
                    System.Console.WriteLine("Error {0}: {1}", oed.status, oed.message);
                    System.Console.WriteLine(oed.description);
                    return;
                } // End if (oed.error)


                sql = @"
INSERT INTO t_map_currency_rate(cur_uid,cur_rate,cur_name,cur_time)
VALUES
(
	 @cur_uid -- uniqueidentifier
	,@cur_rate -- decimal(35,15)
	,@cur_name -- char(3)
	,@cur_time -- datetime
);
";

                System.DateTime localShapshotTime = oed.SnapshotTime.ToLocalTime();
                System.Console.WriteLine(localShapshotTime);


                foreach (System.Collections.Generic.KeyValuePair<string, decimal> kvp in oed.RatesList)
                {
                    System.Console.WriteLine(kvp.Key);
                    System.Console.WriteLine(kvp.Value);

                    using (System.Data.IDbCommand cmd = SQL.CreateCommand(sql))
                    {
                        SQL.AddParameter(cmd, "cur_uid", System.Guid.NewGuid());
                        SQL.AddParameter(cmd, "cur_time", oed.SnapshotTime);
                        SQL.AddParameter(cmd, "cur_name", kvp.Key);
                        SQL.AddParameter(cmd, "cur_rate", kvp.Value);

                        SQL.ExecuteNonQuery(cmd);
                    } // End Using cmd 

                } // Next kvp 

            } // End Sub Test 


        } // End Class OpenExchangeRates 


    } // End Class Tests 


} // End Namespace CoinBaseSharp.ExchangeRates 
