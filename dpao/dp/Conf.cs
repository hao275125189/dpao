using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Net;

namespace dpao.dp
{
    class Conf
    {
        public static bool d25 = true;
        public static bool bDqiu = false;
        public static bool bXqiu = false;
        public static double dSw1 = 0.7;
        public static double dSw2 = 0.7;
        public static  int   htis =1;
        public static int money = 50;
        public static int view1 = 0;
        public static int view2 = 0;
        public static Hashtable lsss = new Hashtable();//次数
        public static Hashtable teamKey = new Hashtable();//球队
        public static Hashtable LianSai = new Hashtable();
        public static Hashtable Dansi = new Hashtable();
        public static Hashtable Gj = new Hashtable();
        public static CookieContainer myCookieContainer = new CookieContainer();
        public static string Uid =null;
        public static string cUrl = null;



        public static int GetLianSaiID(string cName)
        {
            if (Conf.LianSai.ContainsKey(cName))
            {
                return Convert.ToInt32(Conf.LianSai[cName]);
            }
            return -1;
        }
        public static int GetteamKeyID(string cName)
        {
            if (Conf.teamKey.ContainsKey(cName))
            {
                return Convert.ToInt32(Conf.teamKey[cName]);
            }
            return -1;
        }
        public static void ConfInit()
        {
           

            DataTable dt = database.Querydt("select ls,val from lst order by id");
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["ls"].ToString().Trim();
                string val = dr["val"].ToString().Trim();
                if (!Conf.LianSai.ContainsKey(key)) Conf.LianSai.Add(key, val);
               
            }
             dt = database.Querydt("select qd,val from qdt order by id");
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["qd"].ToString().Trim();
                string val = dr["val"].ToString().Trim();
                if(!Conf.teamKey.ContainsKey(key))Conf.teamKey.Add(key, val);
            }
            //return ht;
        }
        
    }
}
