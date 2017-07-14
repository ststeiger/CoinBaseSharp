using System;
using System.Collections.Generic;
using System.Text;



namespace EasyJSON
{

}


namespace System.ComponentModel
{
    public class AsyncCompletedEventArgs : EventArgs
    {
        public AsyncCompletedEventArgs() { }
        public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
        { }

        public bool Cancelled { get; }
        public Exception Error { get; }
        public object UserState { get; }

        //protected void RaiseExceptionIfNecessary();
    }
}


namespace System.Net
{

    public delegate void OpenReadCompletedEventHandler(object sender, OpenReadCompletedEventArgs e);
    public delegate void OpenWriteCompletedEventHandler(object sender, OpenWriteCompletedEventArgs e);


    public class OpenReadCompletedEventArgs
    {
        public System.IO.Stream Result { get; }
    }

    public class OpenWriteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public System.IO.Stream Result { get; }
    }



    public class WebClient : System.IDisposable
    {

        public event OpenReadCompletedEventHandler OpenReadCompleted;
        public event OpenWriteCompletedEventHandler OpenWriteCompleted;


        public System.Collections.Generic.Dictionary<string, string> Headers = new Collections.Generic.Dictionary<string, string>();

        public WebClient()
        { }

        public System.IO.Stream OpenRead(string url)
        {
            return null;
        }

        public System.IO.Stream OpenRead(System.Uri url)
        {
            return null;
        }


        public void OpenReadAsync(System.Uri url, string token)
        {
            return;
        }

        public System.IO.Stream OpenWrite(string address, string method)
        {
            return null;
        }

        public void OpenWriteAsync(System.Uri address)
        { }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WebClient() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}


namespace Tools.XML
{
    internal static class foobar
    {

        public static void Close(this System.IO.Stream strm)
        { }

        public static void Close(this System.IO.TextWriter strm)
        { }

        public static void Close(this System.IO.StreamReader strm)
        { }

        

    }
}
