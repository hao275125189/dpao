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
            string Sql = "SELECT Lid FROM `db_odds`";
            DataTable Dt= DbHelperMySQL.DQuery(Sql);
            Console.WriteLine(Dt.Rows.Count);
            return Dt;
        }

        public static DataTable Select()
        {
            string Sql = "SELECT Lid FROM `db_odds` where fstatus=0";
            DataTable Dt = DbHelperMySQL.DQuery(Sql);
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                Console.WriteLine(Dt.Rows[i]["Lid"]);
            }
            
            return Dt;
        }

    }
}
