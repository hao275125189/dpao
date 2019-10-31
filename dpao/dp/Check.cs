using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpao.dp
{

    /// <summary>
    /// 检查类，条件符合的在这个类做检查
    /// </summary>
    class Check
    {
        public static void Di(string d1,string d2)
        {

            DateTime t1 = DateTime.Parse("2007-01-01 12:10:00");
            DateTime t2 = DateTime.Parse("2007-01-01 12:00:00");

            System.TimeSpan t3 = t1 - t2;  //两个时间相减 。默认得到的是 两个时间之间的天数   得到：365.00:00:00 
            var rs = t3.TotalMinutes;
            Console.WriteLine(rs);
        }

        public static DataTable Select1()
        {
            string Sql = "SELECT Lid FROM db_liansai where fstatus=0";
            DataTable Dt= DbHelperMySQL.DQuery(Sql);
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
               string Lid=Dt.Rows[i]["Lid"].ToString();
               Select(Lid,"0","0.2");
            }
            return Dt;
        }

        public static DataTable Select(string Lid,string OddsID,string Odds)
        {

            double Q1 = 0;
            double Q2 = 0;

            string Sql = "SELECT * FROM `db_odds` where Lid="+ Lid;
            DataTable Dt = DbHelperMySQL.DQuery(Sql);
            
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                DataRow dr = null;
                try
                {
                    double Q1adds = Convert.ToDouble(Dt.Rows[i]["Q1adds"]);
                    double Q2adds = Convert.ToDouble(Dt.Rows[i]["Q2adds"]);
                    if (i == 0)
                    {
                        Q1 = Q1adds;
                        Q2 = Q2adds;
                        dr = Dt.Rows[i];
                    }
                    if (Q1 - Q1adds > 0.2)
                    {
                        DataRow drs = null;
                        Console.Write(Dt.Rows[i]["Q1"]);
                        Console.WriteLine(Dt.Rows[i]["Q1adds"] +"---"+ Q1+"--"+ Lid);
                        drs["Lid"] = 0;
                        drs["Ls"] = 0;
                        drs["Q1"] = 0;
                        drs["Q2"] = 0;
                        drs["H"] = 0;
                        drs["C"] = 0;
                        drs["PAN1"] = 0;
                        drs["PAN2"] = 0;
                        drs["Q1ODDS"] = 0;
                        drs["Q1ODDS_F"] = 0;
                        drs["Q2ODDS"] = 0;
                        drs["Q2ODDS_F"] = 0;
                        drs["stime"]  = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;





                    }
                    if (Q2 - Q2adds > 0.2)
                    {
                        Console.Write(Dt.Rows[i]["Q2"]);
                        Console.WriteLine(Dt.Rows[i]["Q2adds"] + "---" + Q2+"--"+ Lid);
                    }

 
                   Q1 = Q1adds;
                   Q2 = Q2adds;
                   dr = Dt.Rows[i];

                }
                catch {
                    Console.WriteLine(Dt.Rows[i]["Q1adds"]);
                }

            }            
            return Dt;
        }

    }
}
