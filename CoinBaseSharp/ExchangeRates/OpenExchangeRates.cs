
namespace CoinBaseSharp.ExchangeRates
{
    
    
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
        } // End Sub Test 


    } // End Class OpenExchangeRates


} // End Namespace CoinBaseSharp.ExchangeRates 
