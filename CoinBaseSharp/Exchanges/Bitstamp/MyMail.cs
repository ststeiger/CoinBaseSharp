using System;

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
        }


        public class cRes
        {
            public string id
            {
                get{ 
                    return null;
                }
            }


            public string actualPerf()
            {
                return null;
            }
        }

        public static void lol()
        {
            Parameters pars = new Parameters();
            cRes res = new cRes();

            //void sendEmail(Result& res, Parameters& params) {
            string tdStyle =         "font-family:Georgia;font-size:11px;border-color:#A1A1A1;border-width:1px;border-style:solid;padding:2px;";
                string captionStyle =    "font-family:Georgia;font-size:13px;font-weight:normal;color:#0021BF;padding-bottom:6px;text-align:left;";
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
          


                if (res.actualPerf() >= 0) {
                oss.Append("+");
                oss.Append("res.actualPerf() * 100.0");
                } else {
                oss.Append("res.actualPerf() * 100.0");
                }
                

            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append("");
            oss.Append(@"
<html>
  <div>
    <br/><br/>
    <table style=""border-width:0px;border-collapse:collapse;text-align:center;"">";
                oss << "      <caption style=\\\"" << captionStyle << "\\\">Blackbird Bitcoin Arbitrage - Trade " << res.id << "</caption>";
                oss << "      <tr style=\\\"" << tableTitleStyle << "\\\">";
                oss << "        <td style=\\\"" << tdStyle << "width:120px;\\\">Entry Date</td>";
                oss << "        <td style=\\\"" << tdStyle << "width:120px;\\\">Exit Date</td>";
                oss << "        <td style=\\\"" << tdStyle << "width:70px;\\\">Long</td>";
                oss << "        <td style=\\\"" << tdStyle << "width:70px;\\\">Short</td>";
                oss << "        <td style=\\\"" << tdStyle << "width:70px;\\\">Exposure</td>";
                oss << "        <td style=\\\"" << tdStyle << "width:70px;\\\">Profit</td>";
                oss << "        <td style=\\\"" << tdStyle << "width:70px;\\\">Return</td>";
                oss << "      </tr>";
                oss << "      <tr>";
                oss << "        <td style=\\\"" << tdStyle << "\\\">" << printDateTime(res.entryTime) << "</td>";
                oss << "        <td style=\\\"" << tdStyle << "\\\">" << printDateTime(res.exitTime) << "</td>";
                oss << "        <td style=\\\"" << tdStyle << "\\\">" << res.exchNameLong << "</td>";
                oss << "        <td style=\\\"" << tdStyle << "\\\">" << res.exchNameShort << "</td>";
                oss << "        <td style=\\\"" << tdStyle << "\\\">\\$" << res.exposure * 2.0 << "</td>";
                oss << "        <td style=\\\"" << tdStyle << "\\\">\\$" << res.usdTotBalanceAfter - res.usdTotBalanceBefore << "</td>";

                if (res.actualPerf() >= 0) 
                {
                    oss << "<td style=\\\"" << tdStyle << "color:#000092;\\\">+";
                } 
                else 
                {
                    oss << "<td style=\\\"" << tdStyle << "color:#920000;\\\">";
                }
                oss  << res.actualPerf() * 100.0 << "%</td></tr>";
                oss << "    </table>";
                oss << "  </div>";
                oss << "</html>\" -s " << pars.smtpServerAddress << " -xu " << pars.senderUsername << " -xp " << pars.senderPassword << " -o tls=yes -o message-content-type=html >/dev/null" << std::endl;

            }



        }



    }
}

