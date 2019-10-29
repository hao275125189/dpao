using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dpao.dp
{
    class sbobet
    {
        /// <summary>
        /// 第一
        /// </summary>
        /// <returns></returns>
        public static UserInfo Login(UserInfo ui)
        {
           // UserInfo ui = new UserInfo();
            CookieContainer cc = new CookieContainer();
            string Url = "https://www.sbobet.com/zh-cn/euro";
            string cReferer = "https://www.sbobet.com/"; ;
            string s = Connect.getDocument(Url, cc, cReferer);
           
            string data = "id=" + ui .User+ "&password=" + ui.Pwd+ "&lang=zh-cn&tk=34%2C34%2C2%2C0%2C0%2C0%2C0%2C20150-%2C1%2C3%2C0%2C4&5=1&type=form";
            cReferer = "https://www.sbobet.com/zh-cn/euro";
            Url = "https://www.sbobet.com/web/public/process-sign-in.aspx";
            s = Connect.postDocument(Url, data, cc, cReferer);
            if (s == null) return ui;
            Url = Connect.loctionUrl;
           
            s = Connect.getDocument(Url, cc, cReferer);
            Uri uri = new Uri(Url);
            string host=uri.Host.ToString();
            string loginname = uri.Query.Replace("&redirect=true", "");
            string zhurl = "http://" + host + "/web-root/restricted/default.aspx" + loginname + "&lang=zh-cn&sportId=1";
            s = Connect.getDocument(zhurl, cc, zhurl);
            
            ui.cc = cc;

            if (s != null && s.IndexOf(ui.User) > 0)
            {
                
                ui.Status = 1;
                ui.cUrl = Url;
            }
            else {
                ui.Status = 0;
            }
            return ui;
        }
        public static UserInfo Odds(UserInfo ui)
        {

          // string cReferer = "https://www.sbobet.com/zh-cn/euro";
            CookieContainer cc = ui.cc;
            string Url = ui.cUrl;
            Uri siteUri = new Uri(Url);
            // string cUrl="http://bbo3x32e5jh3.asia.sbobet.com/web-root/restricted/odds-display/today-data.aspx?od-param=1,1,1,1,1,2,2,2,1&fi=1&v=0&dl=1"

            string cUrl = "http://" + siteUri.Host.Trim() + "/web-root/restricted/odds-display/today-data.aspx?od-param=1,1,1,1,1,2,2,2,1&fi=1&v=0&dl=1";
            string s = Connect.getDocument(cUrl, cc);
            s = Pfun.FromUnicodeString(s);
            //Console.WriteLine(s);
            if (s.IndexOf("logout.aspx")>0)
            {
                Console.WriteLine("logout");
                ui.Status = 0;
                return ui;
            }
            System.Diagnostics.Stopwatch sh = new System.Diagnostics.Stopwatch();
            string jsFilePath = Application.StartupPath + "\\js\\seb.js";
            string funcName = "ODDS";
            try
            {
                string cJSONData = s.Substring(s.IndexOf("["), s.LastIndexOf(")") - s.IndexOf("["));
                // cJSONData = cJSONData.Substring(0, cJSONData.LastIndexOf("]") + 1).Trim();
                //cJSONData = "var arr=" + cJSONData+";";
                object[] paramers = new object[1] { cJSONData.Trim().Replace("\n", "").Replace("\r", "").Replace(",'", ",\"").Replace("',", "\",").Replace("\"'", "\"\"").Replace("']", "\"]").Replace("'", "") };
                string sScript = TextFile.ReadFile(jsFilePath);

                ScriptEngine se = new ScriptEngine(ScriptLanguage.JavaScript);
                object obj = se.Run(funcName, paramers, sScript);
                if (obj.Equals("缺少 ']'"))
                {
                    ui.ODDS = cJSONData;
                    return ui;
                }
                string[] p=obj.ToString().Split('\n');
                for (int i = 0; i < p.Length; i++)
                {
                    string[] arr=p[i].Split(new char[] { ',', '|', ':' });
                    Hashtable ht = ParseLineds(arr);
                }

                ui.ODDS = obj.ToString();
                ui.Status = 1;
                return ui;
            }catch(Exception ex){
                return ui;
            }
        }

        private static Hashtable ParseLineds(string[] arr) {
            return null;
              
        }
    }
}
