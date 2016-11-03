
// http://blog.bondigeek.com/2011/09/08/a-simple-c-thread-safe-logging-class/
namespace BondiGeek.Logging
{


    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// </summary>
    public class LogWriter
    {
        private static LogWriter instance;
        private static System.Collections.Generic.Queue<Log> logQueue;
        private static string logDir = @""; //<Path to your Log Dir or Config Setting>;
        private static string logFile = @""; //<Your Log File Name or Config Setting>;
        private static int maxLogAge = 60; // int.Parse(<Max Age in seconds or Config Setting>);
        private static int queueSize = 10000; // int.Parse(<Max Queue Size or Config Setting);
        private static System.DateTime LastFlushed = System.DateTime.Now;


        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter() { }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (instance == null)
                {
                    logQueue = new System.Collections.Generic.Queue<Log>();
                    instance = new LogWriter();
                }
                return instance;
            }
        }


        ~LogWriter()
        {
            FlushLog();
        }


        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToLog(string message)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (logQueue)
            {
                // Create the entry and push to the Queue
                Log logEntry = new Log(message);
                logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (logQueue.Count >= queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }            
        }

        private bool DoPeriodicFlush()
        {
            System.TimeSpan logAge = System.DateTime.Now - LastFlushed;
            if (logAge.TotalSeconds >= maxLogAge)
            {
                LastFlushed = System.DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private void FlushLog()
        {
            while (logQueue.Count > 0)
            {
                Log entry = logQueue.Dequeue();
                string logPath = logDir + entry.LogDate + "_" + logFile;

                // This could be optimised to prevent opening and closing the file for each write
                using (System.IO.FileStream fs = System.IO.File.Open(logPath, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    using (System.IO.StreamWriter log = new System.IO.StreamWriter(fs))
                    {
                        log.WriteLine(string.Format("{0}\t{1}",entry.LogTime,entry.Message));
                    }
                }
            }            
        }
    }


    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }

        public Log(string message)
        {
            Message = message;
            LogDate = System.DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = System.DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }


}