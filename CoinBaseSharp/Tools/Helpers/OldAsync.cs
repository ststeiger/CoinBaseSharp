
namespace CoinBaseSharp.Helpers
{

    // http://www.c-sharpcorner.com/uploadfile/vendettamit/delegate-and-async-programming-C-Sharp-asynccallback-and-object-state/
    // https://msdn.microsoft.com/en-us/library/mt674882.aspx
    // http://blog.markshead.com/869/state-machines-computer-science/
    // https://msdn.microsoft.com/en-us/library/dd460693(v=vs.110).aspx
    class OldAsync
    {


        //Simple delegate declaration
        public delegate int BinaryOp(int x, int y);
        static void Test()
        {
            System.Console.WriteLine("Main() running on thread {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
 
            
            BinaryOp bp = new BinaryOp(OldAsync.Add);
            System.IAsyncResult iftAr = bp.BeginInvoke(5, 5, new System.AsyncCallback(OldAsync.AddComplete), "This message is from Main() thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            while (!iftAr.AsyncWaitHandle.WaitOne(100,true))
            {
                System.Console.WriteLine("Doing some work in Main()!");
            }
            System.Console.Read();
        }

        //An Add() method that do some simple arithamtic operation
        public static int Add(int a, int b)
        {
            System.Console.WriteLine("Add() running on thread {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
            System.Threading.Thread.Sleep(500);
            return (a + b);
        }

        //Target of AsyncCallback delegate should match the following pattern
        public static void AddComplete(System.IAsyncResult iftAr)
        {
            System.Console.WriteLine("AddComplete() running on thread {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
            System.Console.WriteLine("Operation completed.");

            //Getting result
            System.Runtime.Remoting.Messaging.AsyncResult ar = (System.Runtime.Remoting.Messaging.AsyncResult)iftAr;
            BinaryOp bp = (BinaryOp)ar.AsyncDelegate;
            int result = bp.EndInvoke(iftAr);

            //Recieving the message from Main() thread.
            string msg = (string)iftAr.AsyncState;
            System.Console.WriteLine("5 + 5 ={0}", result);
            System.Console.WriteLine("Message recieved on thread {0}: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, msg);
        } 

    }
}
