using dpao.dp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dpao.v
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        private const int UrlmonOptionUseragent = 0x10000001;

        public static void ChangeUserAgent(string agent)
        {
            UrlMkSetSessionOption(UrlmonOptionUseragent, agent, agent.Length, 0);
        }
        void web1()
        {
            //string use = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Mobile Safari/537.36";
            string strss = "Mozilla/5.0 (Windows; U; Windows NT 5.2) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.27 Safari/525.13;";
            ChangeUserAgent(strss);
            webBrowser1.Navigate("http://199.26.96.55/");

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //在WebBrowser中登录cookie保存在WebBrowser.Document.Cookie中  会员aih32642   密aaa111    
            CookieContainer myCookieContainer = new CookieContainer();
            //String 的Cookie　要转成　Cookie型的　并放入CookieContainer中 

            string cookieString = webBrowser1.Document.Cookie;
            if (cookieString == null) return;

            string[] cookstr = cookieString.Split(';');
            //Console.WriteLine(webBrowser1.Url.Host.ToString());
            foreach (string str in cookstr)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                ck.Domain = webBrowser1.Url.Host.ToString();//必须写对  
                myCookieContainer.Add(ck);
                Conf.myCookieContainer = myCookieContainer;


            }
            Console.WriteLine(webBrowser1.Document.Window.Frames[0].Document.Url.ToString());
            string Url = webBrowser1.Document.Window.Frames[0].Document.Url.ToString();
            string Urls = webBrowser1.Url.ToString();
            //Url = "http://199.26.96.55/app/member/FT_index.php?uid=t50uihbgrm22379403l328907&langx=zh-cn&mtype=3&news_mem=Y";
            if (Url.IndexOf("uid") >-1)
            {
                string[] Up = Url.Split('=');
                string[] p = Up[1].Split('&');
                Conf.Uid = p[0];
                Conf.cUrl = Urls.Replace("http://", "").Replace("/",""); ;
                Console.WriteLine(Conf.Uid);
            }

            string conn = System.Windows.Forms.Application.StartupPath + "\\cok.txt";
            FileInfo myFile = new FileInfo(conn);
            StreamWriter sw5 = myFile.CreateText();
            //初始化完成后，可以用StreamWriter对象一次写入一行，一个字符，一个字符数组，甚至一个字符数组的一部分。
            // 写一个字符            
            sw5.Write(cookieString);
            sw5.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            web1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            web1();
        }
    }
}
