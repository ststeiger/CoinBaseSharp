
namespace CoinBaseSharp
{


    public class Trash
    {


        public static string ReplaceLeadingZeros(System.Text.RegularExpressions.Match m)
        {
            string retVal = m.Value;
            System.Text.RegularExpressions.Capture capt = m.Groups["ID"];


            if (capt == null)
                return retVal;

            int ind = capt.Index - m.Index;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(retVal);
            sb.Remove(ind, capt.Length);
            sb.Insert(ind, capt.Value.TrimStart('0'));
            retVal = sb.ToString();
            sb.Length = 0;
            sb = null;

            return retVal;
        }


        // -----------------------------------------------------------------------------
        // Optimization: Pass GroupName as additional arguments to MatchEvaluator
        // -----------------------------------------------------------------------------

        public delegate string ReplaceCallback_t(string str);


        public static string ReplaceGroup(System.Text.RegularExpressions.Regex regex, string input, string groupName, ReplaceCallback_t pfnReplace)
        {
            return regex.Replace(input, new System.Text.RegularExpressions.MatchEvaluator(
                delegate(System.Text.RegularExpressions.Match m) 
                {
                    string retVal = m.Value;
                    System.Text.RegularExpressions.Capture capt = m.Groups[groupName];

                    if (capt == null)
                        return retVal;

                    int ind = capt.Index - m.Index;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(retVal);
                    sb.Remove(ind, capt.Length);
                    sb.Insert(ind, pfnReplace(capt.Value));
                    retVal = sb.ToString();
                    sb.Length = 0;
                    sb = null;
                    return retVal;
                })
            );

        }

        // CoinBaseSharp.Trash.TestInputSanitation();
        public static void TestInputSanitation()
        {
            string input = "0010hello";
            input += System.Environment.NewLine + "-00020bla";
            input += System.Environment.NewLine + "-000100   ";
            input += System.Environment.NewLine + "-000101   ";
            input += System.Environment.NewLine + "-000110   ";
            input += System.Environment.NewLine + "-000200  ";
            input += System.Environment.NewLine + "bye 001000010";
            string pattern = @"^[-]?\s*(?<ID>[0-9]+)\s*$";

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);


            string sanitizedInput = regex.Replace(input, new System.Text.RegularExpressions.MatchEvaluator(ReplaceLeadingZeros));
            System.Console.WriteLine(sanitizedInput);

            string simplifiedSanitation = ReplaceGroup(regex, input, "ID", delegate(string str)
                {
                    return str.TrimStart('0');
                }
            );
            System.Console.WriteLine(simplifiedSanitation);
        }


    }


}
