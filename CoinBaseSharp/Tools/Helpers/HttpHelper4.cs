
using System.Net.Http;
using System.Threading.Tasks;

// http://www.codeproject.com/Articles/996857/Asynchronous-programming-and-Threading-in-Csharp-N
namespace CoinBaseSharp.Helpers
{


    class HttpHelper4
    {


        // http://www.codeproject.com/Articles/996857/Asynchronous-programming-and-Threading-in-Csharp-N
        // https://msdn.microsoft.com/en-us/library/dd460693(v=vs.110).aspx
        public static void TestParallel()
        {
            Parallel.For(0, 100, (int i) =>
                {
                    System.Console.WriteLine(i);
                }
            );

            Parallel.For(0, 100, async (int i) =>
                {
                    System.Console.WriteLine(i);
                    await Task.Delay(10);
                }
            );
        }



        public static System.Threading.Thread StartThread(int a, int b)
        {
            int aa = a*a;
            int bb = b*b;

            System.Threading.Thread thisThread = new System.Threading.Thread(
                delegate()
                {
                    int cc = aa + bb;
                    double c = System.Math.Sqrt(cc);
                }
            );

            thisThread.Name = System.Guid.NewGuid().ToString();
            thisThread.IsBackground = true;
            thisThread.Priority = System.Threading.ThreadPriority.Lowest;

            thisThread.Start();

            return thisThread;
        }




        async Task<string> HttpGetAsync(string URI)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    Task<System.IO.Stream> result = hc.GetStreamAsync(URI);

                    using (System.IO.Stream vs = await result)
                    {
                        using (System.IO.StreamReader am = new System.IO.StreamReader(vs))
                        {
                            return await am.ReadToEndAsync();    
                        }
                    }
                    
                }
                
            }
            catch (System.Net.WebException ex)
            {

                if(ex.Status == System.Net.WebExceptionStatus.NameResolutionFailure)
                    System.Console.WriteLine("Error: domain not found");

                //switch (ex.Status)
                //{
                //    case System.Net.WebExceptionStatus.NameResolutionFailure:
                //        System.Console.WriteLine("Error: domain not found");
                //        break;
                //    //Catch other exceptions here
                //}
                throw;
            }
        }



    } // End Class 


} // End Namespace 
