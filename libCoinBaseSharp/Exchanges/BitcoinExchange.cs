
namespace CoinBaseSharp.Exchanges
{


    public abstract class BitcoinExchange
    {
        private uint id;
        private string exchName;
        private decimal fees;
        private bool hasShort;
        private bool isImplemented;
        private decimal bid;
        private decimal ask;


        public BitcoinExchange(uint id, string n, decimal f, bool h, bool m)
        { }


        public virtual bool IsImplemented
        {
            get { return false; }
        }
        

        public virtual uint Id
        {
            get { return 0; }
        }


        public virtual void updateData(double b, double a) { }
        public virtual decimal getAsk() { return 0; }
        public virtual decimal getBid() { return 0; }
        public virtual decimal getMidPrice() { return 0; }
        public virtual string getExchName() { return null; }
        public virtual decimal getFees() { return 0; }
        public virtual bool getHasShort() { return false; }
        

    }


}
