
namespace CoinBaseSharp.ExchangeRates
{


    public class Tests
    {


        // http://api.fixer.io/latest
        // http://api.fixer.io/2000-01-03
        public class Fixer
        {


            public static void Test()
            {
                FixerData fxd = JilHelper.DeserializeUrl<FixerData>("http://api.fixer.io/latest");
                System.Console.WriteLine(fxd);
            }

            
        } // End Class Fixer 


        public class ECB
        {
            public static void Test()
            {
                string url = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
                // string fileName = "/root/sources/CoinBaseSharp/CoinBaseSharp/ecb_feed.xml";

                CoinBaseSharp.ExchangeRates.ECB.Envelope env =
                    Tools.XML.Serialization.DeserializeXmlFromUrl<CoinBaseSharp.ExchangeRates.ECB.Envelope>(url);
                    // Tools.XML.Serialization.DeserializeXmlFromFile<CoinBaseSharp.ExchangeRates.ECB.Envelope>(fileName);
                System.Console.WriteLine(env);
            }

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

                //string fileName = @"d:\username\documents\visual studio 2013\Projects\CoinBaseSharp\CoinBaseSharp\ExchangeRates\OpenExchangeRates_Data.txt";
                string fileName = @"d:\username\documents\visual studio 2013\Projects\CoinBaseSharp\CoinBaseSharp\ExchangeRates\OpenExchangeRates_Error.txt";


                // OpenExchangeRatesData oed = JilHelper.DeserializeUrl<OpenExchangeRatesData>(url);
                OpenExchangeRatesData oed = JilHelper.DeserializeFromFile<OpenExchangeRatesData>(fileName);
                System.Console.WriteLine(oed);

                if (oed.error)
                {
                    System.Console.WriteLine("OMG errorz {0}: {1}", oed.status, oed.message);
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


                foreach (System.Collections.Generic.KeyValuePair<string,decimal> kvp in oed.RatesList)
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
