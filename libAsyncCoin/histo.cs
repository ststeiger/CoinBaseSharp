using System;
using System.Collections.Generic;
using System.Text;

namespace libAsyncCoin
{


    public class HistoTime
    {
        public string updated { get; set; }
        public string updatedISO { get; set; }
    }

    public class HisoRootObject
    {
        
        public System.Collections.Generic.Dictionary<System.DateTime, decimal> bpi;

        public string disclaimer { get; set; }
        public HistoTime time { get; set; }
    }

}
