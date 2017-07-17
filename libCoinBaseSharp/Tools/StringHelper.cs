
namespace CoinBaseSharp
{


    internal class StringHelper
    {


        public static string TrimSpecial(System.Object obj)
        {
            if (obj == null)
                return null;

            if (obj == System.DBNull.Value)
                return null;

            string str = obj.ToString();
            return str.Trim(new char[] { '\r', '\n', ' ', '\t' });
        } // End Function TrimSpecial 


    } /// End Class StringHelper 
    

} // End Namespace CoinBaseSharp.Tools 
