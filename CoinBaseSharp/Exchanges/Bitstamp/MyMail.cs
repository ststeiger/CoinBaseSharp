
namespace CoinBaseSharp
{


    public class MyMail
    {
        

        public class Parameters
        {
            public string smtpServerAddress;
            public string senderUsername;
            public string senderPassword;

            public string senderAddress;
            public string receiverAddress;
        }


        public class cRes
        {
            public string id
            {
                get
                { 
                    return null;
                }
            }

            public System.DateTime entryTime;
            public System.DateTime exitTime;
            public string exchNameLong;
            public string exchNameShort;
            public decimal exposure;
            public decimal usdTotBalanceAfter;
            public decimal usdTotBalanceBefore;


            public decimal actualPerf()
            {
                return 0.0m;
            }
        }


        public static string printDateTime(System.DateTime obj)
        {
            return obj.ToString("dddd, dd.MM.yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
        }


        public static void lol()
        {
            Parameters pars = new Parameters();
            cRes res = new cRes();

            //void sendEmail(Result& res, Parameters& params) {
            string tdStyle = "font-family:Georgia;font-size:11px;border-color:#A1A1A1;border-width:1px;border-style:solid;padding:2px;";
            string captionStyle = "font-family:Georgia;font-size:13px;font-weight:normal;color:#0021BF;padding-bottom:6px;text-align:left;";
            string tableTitleStyle = "font-family:Georgia;font-variant:small-caps;font-size:13px;text-align:center;border-color:#A1A1A1;border-width:1px;border-style:solid;background-color:#EAEAEA;";
                

            System.Text.StringBuilder oss = new System.Text.StringBuilder();

            oss.Append("sendemail -f ");


            oss.Append(pars.senderAddress);
            oss.Append(" -t ");
            oss.Append(pars.receiverAddress);
            oss.Append(" -u \"Blackbird Bitcoin Arbitrage - Trade ");
            oss.Append(res.id);
            oss.Append(" (");
            oss.Append("%)\" -m \"");
          


            if (res.actualPerf() >= 0)
            {
                oss.Append("+");
                oss.Append("res.actualPerf() * 100.0");
            }
            else
            {
                oss.Append("res.actualPerf() * 100.0");
            }
                

            oss.Append(@"
<html>
  <div>
    <br/><br/>
    <table style=""border-width:0px;border-collapse:collapse;text-align:center;"">
        <caption style=""");
            oss.Append(captionStyle);
            oss.Append(@""">Blackbird Bitcoin Arbitrage - Trade ");
            oss.Append(res.id);
            oss.Append(@"</caption>
        <tr style=""");
            oss.Append(tableTitleStyle);
            oss.Append( @""">");

            oss.Append(@"
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:120px;"">Entry Date</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:120px;"">Exit Date</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:70px;"">Long</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:70px;"">Short</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:70px;"">Exposure</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:70px;"">Profit</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@"width:70px;"">Return</td>
        </tr>
        <tr>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@""">");
            oss.Append(printDateTime(res.entryTime));
            oss.Append(@"</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append( @""">");
            oss.Append(printDateTime(res.exitTime));
            oss.Append(@"</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@""">");
            oss.Append(res.exchNameLong);
            oss.Append( @"</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@""">");
            oss.Append(res.exchNameShort);
            oss.Append(@"</td>
            <td style=""");
            oss.Append(tdStyle);
            oss.Append(@""">\\$");
            oss.Append(res.exposure * 2.0m);
            oss.Append(@"</td>
            <td style="""); 
            oss.Append(tdStyle);
            oss.Append(@""">\\$");
            oss.Append(res.usdTotBalanceAfter - res.usdTotBalanceBefore);
            oss.Append(@"</td>");



            if (res.actualPerf() >= 0)
            {
                oss.Append("<td style=\"");
                oss.Append(tdStyle);
                oss.Append("color:#000092;\">+");
            }
            else
            {
                oss.Append("<td style=\"");
                oss.Append(tdStyle);
                oss.Append("color:#920000;\">");

            }


            oss.Append(res.actualPerf() * 100.0m);
            oss.Append("%</td></tr>");

            oss.Append("    </table>");
            oss.Append("  </div>");
            oss.Append("</html>");
            oss.Append("\"  -s ");
            oss.Append(pars.smtpServerAddress);
            oss.Append(" -xu ");
            oss.Append(pars.senderUsername);
            oss.Append(" -xp ");
            oss.Append(pars.senderPassword);
            oss.Append(" -o tls=yes -o message-content-type=html >/dev/null");
            oss.Append("\r\n");

        }



    }



}

