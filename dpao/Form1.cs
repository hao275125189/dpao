﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dpao.dp;

using System.Threading;
using System.Collections;
using dpao.v;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace dpao
{
    public partial class Form1 : Form
    {

        DataTable site = new DataTable();
        DataTable dt_user = new DataTable();//用户登录成功表
        DataTable user = new DataTable();//初始到用户表
        int iSpot = 0;//采集线程停止

        public Form1()
        {

            if ("Z1W0D3QF" != Pfun.GetMainHardDiskId())
            {
                //System.Environment.Exit(0);
            }
            InitializeComponent();
            dt_user.Columns.Add("site_ID", System.Type.GetType("System.Int32"));
            dt_user.Columns.Add("name", System.Type.GetType("System.String"));
            dt_user.Columns.Add("UserObj", System.Type.GetType("System.Object"));
            user = database.Querydt("select * from user");
            DataRow dr = user.Rows[0];
            textBox5.Text=dr["name"].ToString();
            Console.WriteLine(dr["name"]);
       
            //Gva.ReadOnly = true;
          
            //Gva.RowHeadersVisible = false;
            //Gvb.ReadOnly = true;
            //Gvb.RowHeadersVisible = false;
            //Gva.AllowUserToResizeColumns = false;

            //禁止用户改变DataGridView1の所有行的行高
            //Gva.AllowUserToResizeRows = false;
            //Gvb.AllowUserToResizeColumns = false;

            //禁止用户改变DataGridView1の所有行的行高
            //Gvb.AllowUserToResizeRows = false;

            //Gvb.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
           
            Conf.ConfInit();

            textBox1.Text = Conf.dSw1.ToString();
            textBox2.Text = Conf.dSw2.ToString();
            textBox3.Text = Conf.htis.ToString();
            textBox4.Text = Conf.money.ToString();
            checkBox1.Checked = Conf.bDqiu;
            checkBox2.Checked = Conf.bXqiu;
            checkBox3.Checked = Conf.d25;
            Gva.DataSource = Conf.WhereLianSai;
            for (int j = 0; j < Gvb.Columns.Count; j++)
            {
                //Gva.Columns[j].Width = 20;
            }
            Gva.Columns[0].Width = 20;
            Gva.Columns[1].Width = 80;
            //Gva.AutoGenerateColumns = false;



            //button9.Visible = false;
            //button7.Visible = false;
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            if ("Z1W0D3QF" != Pfun.GetMainHardDiskId())
            {               
                //System.Environment.Exit(0);
            }
            Loginb.Enabled = false;
            UserLogin();            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            iSpot = 0;
           
            startJson(1);
            //startJson(2);
            Bstart.Enabled = false;
            Bstop.Enabled = true;          
        }
        private void button2_Click(object sender, EventArgs e)
        {
            iSpot = 1;
            Bstop.Enabled = false;
            Bstart.Enabled = true;

        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            Conf.view1 = 1;
            Conf.view2 = 2;
            Refresh(1);
            Refresh(2);
        }
      #region  线程区块

        #region  登录区块

        public void UserLogin()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bw_Login;
            bw.ProgressChanged +=Pc_Login;
            bw.RunWorkerCompleted += bw_Login_Stop;
            bw.RunWorkerAsync();
        }

        private void bw_Login_Stop(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Pc_Login(object sender, ProgressChangedEventArgs e)
        {
            Loginb.Text = "成功";
        }

        private void bw_Login(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            foreach (DataRow rw in user.Rows)
            {

                UserInfo ui = new UserInfo();
                ui.Site = Convert.ToInt32(rw["site"]);
                ui.User = rw["name"].ToString();
                ui.Pwd = rw["pwd"].ToString();
                ui.cUrl = rw["url"].ToString();
                ui.cUrl = "199.26.96.55";
                ui = Route.Login(ui);
                object ob = (object)ui;
                if (ui.Status == 1)
                {

                    DataRow r = dt_user.NewRow();
                    r["site_ID"] = rw["site"];
                    r["name"] = rw["name"];
                    r["UserObj"] = ob;
                    dt_user.Rows.Add(r);
                    Console.WriteLine(rw["name"].ToString() + "okokokok");
                    bw.ReportProgress(1,1);
                }

                Thread.Sleep(100);
            }
            
        }

        #endregion

        #region  采集水位区块

        public void startJson(int i)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bw_json;
            bw.ProgressChanged += win_json;
            bw.RunWorkerCompleted += bw_stop_json;
            bw.RunWorkerAsync(i);
        }

        private void bw_json(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            string site = e.Argument.ToString();
            DataRow[] drw = dt_user.Select("site_ID=" + site);
            UserInfo ui = new UserInfo();
            ui.cc = Conf.myCookieContainer;
            ui.Uid = Conf.Uid;
            ui.Site = 1;
            ui.cUrl = Conf.cUrl;


            while (true)
           {
            
               if (iSpot == 1) break;

                //foreach (DataRow rw in drw)
                //{


                //if (ui.Al.Count > 0) ui.Al.Clear();

                ui = Route.Ball(ui);
                if (ui.Status==1) {
                       
                      //string dir=  DirFile.GetDateDir();
                      //string path = Application.StartupPath + "\\" + site + "\\" + dir + "\\" + DirFile.GetH() +  "\\" + DirFile.Getmm();
                      //if (!DirFile.IsExistDirectory(path)){
                      //    DirFile.CreateDirectory(path);
                      //}
                      //  path+="\\"+DirFile.Getss()+".txt";
                      //  if (!DirFile.IsExistFile(path)){
                      //      DirFile.WriteText(path, ui.ODDS, Encoding.UTF8);
                      //  }
                        bw.ReportProgress(1,ui);
                    }
                    else if (ui.Status == 0){
                        //rw["UserObj"] = (object)ui;
                    }
                    bw.ReportProgress(1, ui);
                    Thread.Sleep(10000);
                //}
                

           }
        }
        private void win_json(object sender, ProgressChangedEventArgs e)
        {
           
            UserInfo ui = (UserInfo)e.UserState;
           
            
            label1.Text = DateTime.Now.ToString()+"---"+ui.Msg;
           
           
            if (ui.Al.Count > 0)
            {
                ArrayList al = ui.Al;
               foreach(string[] a in al)
                {
                   
                    int index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value =a[3];
                    this.dataGridView1.Rows[index].Cells[1].Value = a[9];
                    this.dataGridView1.Rows[index].Cells[2].Value = a[0];
                    this.dataGridView1.Rows[index].Cells[3].Value = Conf.money.ToString();
                }
            }
            //throw new NotImplementedException();
        }
        private void bw_stop_json(object sender, RunWorkerCompletedEventArgs e)
        {
           // Console.WriteLine("asdf");
            //throw new NotImplementedException();
        }

        #endregion

        #region  采集过滤球队区块

        public void Refresh(int i)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += bw_Start;
            bw.ProgressChanged += bw_Win_Ref;
            bw.RunWorkerCompleted += bw_End;
            bw.RunWorkerAsync(i);
        }

        private void bw_End(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void bw_Win_Ref(object sender, ProgressChangedEventArgs e)
        {
            int site = (int)e.ProgressPercentage;
            DataTable ht = (DataTable)e.UserState;
           
            if (site == Conf.view1)
            {
               // Gva.DataSource = ht;
                DataView dv = ht.DefaultView;
                dv.Sort = "联赛  desc";
                DataTable dtt = dv.ToTable(true);
                Gva.DataSource = dtt;
                Gird_Style(Gva);
            }
            if (site == Conf.view2)
            {
                DataView dv = ht.DefaultView;
                dv.Sort = "联赛  desc";
                DataTable dtt = dv.ToTable();
               // Gvb.DataSource = dtt;
               // Gird_Style(Gvb);
               
            }

        }

      private void bw_Start(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            int site = (int)e.Argument;
            DataRow[] drw = dt_user.Select("site_ID=" + site.ToString());
            ArrayList al = new ArrayList();           
            DataTable dt = new DataTable();
            dt.Columns.Add("联赛", System.Type.GetType("System.String"));
            dt.Columns.Add("主队", System.Type.GetType("System.String"));
            dt.Columns.Add("客队", System.Type.GetType("System.String"));
            foreach (DataRow rw in drw)
            {
               
                UserInfo ui = (UserInfo)rw["UserObj"];
               
                if (ui.ODDS ==null) continue;
                string[] arr = ui.ODDS.Split('\n');
                foreach(string str in arr){
                    string[] arrays = str.Split(new char[] {',','|',':'});
                    if (arrays.Length < 5) continue;
                    DataRow dr = dt.NewRow();
                   if (ui.Status == 1 && site==1) {                   
                      dr["联赛"]=arrays[0];
                      dr["主队"]=arrays[1];
                      dr["客队"]=arrays[2];
                      dt.Rows.Add(dr);
                   }else if (ui.Status == 1 && site == 2){
                       dr["联赛"] = arrays[0];
                       dr["主队"] = arrays[1];
                       dr["客队"] = arrays[2];
                       dt.Rows.Add(dr);                      
                   }
                   
                }
                if(dt.Rows.Count>0) break;
                          
               
            }
            bw.ReportProgress(site, dt);
              
        }
       
        public void Gird_Style(DataGridView DGV)
        {
            Hashtable ht=new Hashtable();
           
                ht=Conf.teamKey;
           
            if (ht == null) return;
            for (int i = 0; i < DGV.Rows.Count-1; i++) {
                string team1_a = DGV.Rows[i].Cells[1].Value.ToString();
                string team2_a = DGV.Rows[i].Cells[2].Value.ToString();
                string site = DGV.Rows[i].Cells[0].Value.ToString();
                if (ht.ContainsKey(team1_a))
                   {
                             //DGV.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                       DGV.Rows[i].Cells[1].Style.ForeColor = Color.Blue;
                   }
                if (ht.ContainsKey(team2_a))
                {
                    //DGV.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    DGV.Rows[i].Cells[2].Style.ForeColor = Color.Blue;
                }
                if (Conf.LianSai.ContainsKey(site))
                {
                   // DGV.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    DGV.Rows[i].Cells[0].Style.ForeColor = Color.Blue;
                }
            }
        }

     


        #endregion

        private void Gva_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            Gva.Rows[Gva.CurrentCell.RowIndex].Selected = true;
            
            
        }
        #endregion

        private void Gvb_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
            Gvb.Rows[Gvb.CurrentCell.RowIndex].Selected = true;
                

        }


        #region 对比替换

        public int ReplaceLianSai(string LianSai_a, string LianSai_b,int i)
        {
            string sql = null;
            int iLianSaiID1 = Conf.GetLianSaiID(LianSai_a);
            int iLianSaiID2 = Conf.GetLianSaiID(LianSai_b);
            if (iLianSaiID1 == -1 && iLianSaiID2 == -1)
            {
                if (LianSai_a == LianSai_b)
                {
                    Conf.LianSai.Add(LianSai_a, i);
                    sql += "INSERT INTO lst (ls,val) VALUES ('" + LianSai_a + "'," + i.ToString() + ")";
                }
                else
                {
                    Conf.LianSai.Add(LianSai_a, i);
                    Conf.LianSai.Add(LianSai_b, i);
                    sql += "INSERT INTO lst (ls,val) VALUES ('" + LianSai_a + "'," + i.ToString() + ");";
                    sql += "INSERT INTO lst (ls,val) VALUES ('" + LianSai_b + "'," + i.ToString() + ")";
                }
            }
            else
            {
                if (iLianSaiID1 > -1 && iLianSaiID2 == -1) {
                    Conf.LianSai.Add(LianSai_b, iLianSaiID1);
                    sql += "INSERT INTO lst (ls,val) VALUES ('" + LianSai_b + "'," + iLianSaiID1.ToString() + ")";

                }
                else if (iLianSaiID1 ==-1 && iLianSaiID2 > -1) {
                    Conf.LianSai.Add(LianSai_a, iLianSaiID2);
                    sql += "INSERT INTO lst (ls,val) VALUES ('" + LianSai_a + "'," + iLianSaiID2.ToString() + ")";
                
                }

            }
            if(sql!=null)database.ExecuteSQLiteTran(sql);
            return 1;
        }

        public int ReplaceTeam(string LianSai_a, string LianSai_b, int i)
        {
            string sql = null;
            int iLianSaiID1 = Conf.GetteamKeyID(LianSai_a);
            int iLianSaiID2 = Conf.GetteamKeyID(LianSai_b);
            if (iLianSaiID1 == -1 && iLianSaiID2 == -1)
            {
                if (LianSai_a == LianSai_b)
                {
                    Conf.teamKey.Add(LianSai_a, i);
                    sql += "INSERT INTO qdt (qd,val) VALUES ('" + LianSai_a + "'," + i.ToString() + ")";
                }
                else
                {
                    Conf.teamKey.Add(LianSai_a, i);
                    Conf.teamKey.Add(LianSai_b, i);
                    sql += "INSERT INTO qdt (qd,val) VALUES ('" + LianSai_a + "'," + i.ToString() + ");";
                    sql += "INSERT INTO qdt (qd,val) VALUES ('" + LianSai_b + "'," + i.ToString() + ")";
                }
            }
            else
            {
                if (iLianSaiID1 > -1 && iLianSaiID2 == -1)
                {
                    Conf.teamKey.Add(LianSai_b, iLianSaiID1);
                    sql += "INSERT INTO qdt (qd,val) VALUES ('" + LianSai_b + "'," + iLianSaiID1.ToString() + ")";
                }
                else if (iLianSaiID1 == -1 && iLianSaiID2 > -1)
                {
                    Conf.teamKey.Add(LianSai_a, iLianSaiID2);
                    sql += "INSERT INTO qdt (qd,val) VALUES ('" + LianSai_a + "'," + iLianSaiID2.ToString() + ")";
                }

            }
            if (sql != null) database.ExecuteSQLiteTran(sql);
            return 1;
        }

        #endregion

        private void button2_Click_1(object sender, EventArgs e)
        {

          int i=Conf.LianSai.Count+1;
           

            string LianSai_a = Gva.Rows[Gva.CurrentCell.RowIndex].Cells[0].Value.ToString();
            string LianSai_b = Gvb.Rows[Gvb.CurrentCell.RowIndex].Cells[0].Value.ToString();
            this.ReplaceLianSai(LianSai_a,LianSai_b,i);

           i=Conf.teamKey.Count+1;
           
          string team1_a = Gva.Rows[Gva.CurrentCell.RowIndex].Cells[1].Value.ToString();
          string team1_b = Gvb.Rows[Gvb.CurrentCell.RowIndex].Cells[1].Value.ToString();
            this.ReplaceTeam(team1_a,team1_b,i);
           i=Conf.teamKey.Count+1;
          string team2_a = Gva.Rows[Gva.CurrentCell.RowIndex].Cells[2].Value.ToString();
          string team2_b = Gvb.Rows[Gvb.CurrentCell.RowIndex].Cells[2].Value.ToString();
          this.ReplaceTeam(team2_a, team2_b, i);
          

          Gird_Style(Gva); Gird_Style(Gvb);
         
        }

        private void Gva_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string LianSai_a = Gva.Rows[Gva.CurrentCell.RowIndex].Cells[0].Value.ToString();
                if (!Conf.LianSai.ContainsKey(LianSai_a))
                {
                    Conf.LianSai.Add(LianSai_a, 1);
                }
            }
            catch { }
            Guid();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                string team1_a = Gva.Rows[Gva.CurrentCell.RowIndex].Cells[1].Value.ToString();
                if (!Conf.teamKey.ContainsKey(team1_a))
                {
                    Conf.teamKey.Add(team1_a, 1);
                }
            }
            catch { }
            Guid();
           
        }
        private void Guid() {
            DataTable dt = new DataTable();
            dt.Columns.Add("过滤数据", System.Type.GetType("System.String"));
            if (Conf.teamKey.Count > 0){
                foreach (DictionaryEntry de in Conf.LianSai)
                {
                    DataRow dr = dt.NewRow();
                    dr["过滤数据"] = de.Key.ToString();
                    dt.Rows.Add(dr);
                
                }
            }
            if (Conf.teamKey.Count>0)
            {
                foreach (DictionaryEntry de in Conf.teamKey)
                {
                    DataRow dr = dt.NewRow();
                    dr["过滤数据"] = de.Key.ToString();
                    dt.Rows.Add(dr);
                }
            }
            Gvb.DataSource = dt;
            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Conf.dSw1 = Convert.ToDouble(textBox1.Text);
            Conf.dSw2 = Convert.ToDouble(textBox2.Text);
            Conf.htis = Convert.ToInt32(textBox3.Text);
            Conf.money = Convert.ToInt32(textBox4.Text);
            Console.WriteLine(checkBox2.Checked);
            Conf.bDqiu=checkBox1.Checked;
            Conf.bXqiu = checkBox2.Checked;
            Conf.d25 = checkBox3.Checked;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                UserInfo ui = null;
                DataRow[] drw = dt_user.Select("site_ID=1");
                foreach (DataRow rw in drw)
                {
                    ui = (UserInfo)rw["UserObj"];
                }
                webBrowser1.ScriptErrorsSuppressed = true;
                string cUrl = "http://" + ui.cUrl + "/app/member/today/today_wagers.php?uid=" + ui.Uid + "&langx=zh-cn";
                string s = Connect.getDocument(cUrl, ui.cc);
                //Console.WriteLine(s);
                s = s.Replace("http://img.hg7088.com/style/member/",Application.StartupPath+"\\css\\");
                webBrowser1.DocumentText = s;
            }
            catch { }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string name = textBox5.Text;
            string pwd = textBox6.Text;
            string sql = "update user set name='"+name+"',pwd='"+pwd+"' where id=2";
            database.ExecuteSQLite(sql);
            button7.Text = "修改成功";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                UserInfo ui = null;
                DataRow[] drw = dt_user.Select("site_ID=1");
                foreach (DataRow rw in drw)
                {
                    ui = (UserInfo)rw["UserObj"];
                }
                webBrowser1.ScriptErrorsSuppressed = true;
                string cUrl = "http://" + ui.cUrl + "/app/member/history/history_data.php?uid=" + ui.Uid + "&langx=zh-cn";
                string s = Connect.getDocument(cUrl, ui.cc);
                //Console.WriteLine(s);
                s = s.Replace("http://img.hg7088.com/style/member/", Application.StartupPath + "\\css\\");
                webBrowser1.DocumentText = s;
            }
            catch { }
            Gva.Columns[0].Width = 50;
            Gva.Columns[1].Width = 100;

            Gva.Columns[2].Width = 100;
            Gva.Columns[3].Width = 100;
            Gva.Columns[4].Width = 20;
            Gva.Columns[5].Width = 20;
            Gva.Columns[6].Width = 40;
            Gva.Columns[7].Width = 40;
            Gva.Columns[8].Width = 80;
            Gva.Columns[9].Width = 80;
            Gva.Columns[10].Width = 80;
            Gva.Columns[1].HeaderText = "联赛";
            Gva.Columns[2].HeaderText = "球队1";
            Gva.Columns[3].HeaderText = "球队2";
            Gva.Columns[4].HeaderText = "主";
            Gva.Columns[5].HeaderText = "变";
            Gva.Columns[6].HeaderText = "盘";
            Gva.Columns[7].HeaderText = "变盘";
            Gva.Columns[8].HeaderText = "球1水位";
            Gva.Columns[9].HeaderText = "变盘水位";
            Gva.Columns[10].HeaderText = "球2水位";
            Gva.Columns[11].HeaderText = "变盘水位";

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string yoi=DateTime.Now.ToString("yyyy");
            DataTable Dt= Check.Select1();
            //Gva.DataSource = Conf.WhereLianSai;
            long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000 - 1*24*3600;
            string date = "2019-10-30 16:02:33";
            string time = Convert.ToDateTime(date).ToString("yyyy-MM-dd hh:mm");//12小时 
            Console.WriteLine(yoi);


            DateTime t1 = DateTime.Parse("2007-01-01 12:10:00");
            DateTime t2 = DateTime.Parse("2007-01-01 12:00:00");

            System.TimeSpan t3 = t1 - t2;  //两个时间相减 。默认得到的是 两个时间之间的天数   得到：365.00:00:00 
            var rs = t3.TotalMinutes;
            Console.WriteLine(rs);
            string op = DateTime.Now.ToString("r");
       









        }

        private void button10_Click(object sender, EventArgs e)
        {
            Console.WriteLine(20 % 10);
            string cUrl = "http://baidu.lecai.com/lottery/draw/list/557?d=2015-04-10";
            string sp = Connect.getDocument(cUrl);
            if (sp == null)
            {
                Console.WriteLine("NONONO");
                return;
            }
            ArrayList al=Pfun.HtmlTableId(sp, "draw_list");

           
            int sum1 = 0;
            int sum2 = 0;
            int val1 = 0;
            int val2 = 0;
            int lp = 0;
            int _lp = 0;
            int psum = 0;
            int po = 0;
            int _po = 0;
            for (int i = al.Count - 1; i >= 0; i--)
            {
                string[] p = (string[])al[i];
                Console.WriteLine(p[0]);
                if ("开奖号码" == p[0]) continue;
                int in1 = Convert.ToInt32(p[0].Trim());
                int in2 = Convert.ToInt32(p[1].Trim());
                int in3 = Convert.ToInt32(p[2].Trim());
                int in4 = Convert.ToInt32(p[3].Trim());
                int in5 = Convert.ToInt32(p[4].Trim());
                int in6 = Convert.ToInt32(p[5].Trim());
                int in7 = Convert.ToInt32(p[6].Trim());
                int in8 = Convert.ToInt32(p[7].Trim());
                int in9 = Convert.ToInt32(p[8].Trim());
                int in10 = Convert.ToInt32(p[9].Trim());
                 sum1 = in1 + in2 + in3 + in4 + in5;
                 sum2 = in6 + in7 + in8 + in9 + in10;
                 int bosum = 0;
                 int cosum = 0;
                 if (val1 == in1 || val1 == in2 || val1 == in3 || val1 == in4 || val1 == in5)
                {
                   // Console.Write("在" + sum1.ToString());
                    lp++;
                    bosum = 1;
                }
                else
                {
                    bosum = -1;
                    _lp++;
                }
                 if (val2 == in6 || val2 == in7 || val2 == in8 || val2 == in9 || val2 == in10)
                {
                    cosum = 1;
                    
                }
                else
                {
                    cosum = -1;
                }
                psum++;
                val1 = sum1 % 10;
                if (val1 == 0) val1 = 10;
                val2 = sum2 % 10;
                if (val2 == 0) val2 = 10;
                int bs=bosum + cosum;

                if (bs == 0) Console.WriteLine("走水");
                if (bs == -2){ Console.WriteLine("输");
                po++;
                }
                if (bs == 2) {Console.WriteLine("赢");_po++;}

                Console.WriteLine(string.Join(",", p) + "**" + val1.ToString() + "***" + val2.ToString() + "**" + i.ToString());
            }
            Console.WriteLine(po.ToString());
            Console.WriteLine(_po.ToString());
            //Console.WriteLine(lp.ToString());
            //Console.WriteLine(_lp.ToString());
            // Console.WriteLine(psum.ToString());


        
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        

    }
}
