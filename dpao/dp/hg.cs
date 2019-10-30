using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading;

namespace dpao.dp
{
    class hg
    {
        /// <summary>
        /// 第一
        /// </summary>
        /// <returns></returns>
        public static UserInfo Login(UserInfo ui)
        {

            string blackbox = "";
            blackbox = "0400K6s@Dab1SY/jK9GFecOQi5pIV3/9NQ3MngzRxyXNRc/qMMDVfFJojtugpTIfCa6Rry180F8QxyBO0XXUIGemaubHa83CDZB6Z7YrOcfhveDp5LvIUnEYSYOC4sW7mwCcjaAMe/J7SVX2rG15s3ZaQqLfcLYG70qupx285qKQ3aopQs/EGzmZLy01f4NxTGqrWew2McS4m8yvFORLQbDH@mtGMuSZx2KTmkhXf/01DcyeDNHHJc1Fz@owwNV8UmiO26ClMh8JrpGvLXzQXxDHIE7RddQgZ6ZqwKr0Mf@ogWapr@Gp9VQdOe8uA1RmiRrD/GetxUfdnYwgrBH5xvykmQNrlmzbqoDTgtvWkbZ5xzCDafNUk6I0XKJggdZQwtJuaCijB4iJ8z449psRTVdVVq1Kr7ZouVfJggiqWk4m0Gq8d3WkXu75MIS04pZaInjTWeLdRqI82IurxHxP9@UfqE9SNnOoMRDwP7tAPvFhrJMruCt5fqI4KlpBvDCsjxlZvc5IxeMI9PHDtNiVIx2VimZ7@EnMfC4L6d/HnNkh/KA3koT6Hdqn9fi8fptuT4V@57LnmsP38HYt/DAN1odVqCYFUpNKXClEwgybEudID5aqDEHBx0Magn2PYxN2gV06PljubQOyZ@ldxGNGyhUUUYuoKmLN7sWccnDB2QnB46W7sXqfQSQYVwo5VNMvaDXfrI3MaSYrgzuOZTYvnKL0JQQ08ROukpTpu5iGRmShCocMOXSZDGcdkD/oPRKys1lS3x2E4teMfHAkUN31dtGTUv71FJqHcby5BQhNV0Xzfh2dtRE@K4Wh2PWAuGcmjw8E6LEtcYEgIDUmlBgAVSsOVyyofExHIwDdyG4AHEbU3IynXrhcnO4UgF6/3Hu5KfmDMV2HeXfsIpdhmfU9oknoVD8HnO@s8lyWpM5YbymrVY6MZz/oTLgW6cSmRZOWcyLm/md0MLboxU6/TDp0UzLYS8uJqh4v8tvLbly4EM9uG3xE42ys2aVDQXQqh5cC/sD07vXYvBlS65TXm0XBtvk2RW7s2Bf8Fy5FC24eZzo97tw16kQoiglK7Dd3ogqZI5/ZxBDKWPXT/mrIB9QxnksoWmbZIYRwZnYim9ceHDKRCiInEQmpeCXBJVkQGUXcLAkSsYCocoX3ay@ZYWs5ngHUun3EPopga4/ZK1h8TFCICVWxl7quNw2QSnxG1@L3njf1G8uDDo36NBOziBBkVbZqNMZvHCSu9IHSQn04mJBhiuGlD90Jr6o7rsl/jpMLFS4/y@J@2yTfSSYQcxUZlOBUd3hQ6gBTnvlYOkt6K6q1O5U0QfjCHuZEkWM2j2r/vd00vLPiJcIfBCw2qQNylBLVekVB30NFH3F0q1@giUtxHNZLBVfzGCP9Lk8uRdqE9AoiXHRDE85543nNZhMeU@JzHm02AdSpZGHWtQ1XZDb2xeQM9YLJcLTb@vPfBp8j9ZgcmucwtTH10ANtn8ppbyNYRDRf0TU0DlCgSrrpL1hcGwvPYEZ219XoxEoEHel4uVlvGbhi0ctoYT3d6Wf1rrmlEIj1qWhXypzMfZGcd1dnmrm@/ueaR08fgHdSQIOwcLTY3jL8n6MaMawJ9uoj5/7czbfBEJhtxz39cKwlQSqa0hyXDt4m1@EVGm@SBbA64HOcLwHfWv6XQxNP8bVLTtnnzKGjXxvmq9SsYNw3AXN@CYnvt@vqfNxVm0wBVllFa1OzGW57dOn8WTzqMEwNFKk43eRf18Tot1ram3C/y6wRxDzHG9cKKz0Cdx0mnNFuDA9/rHipV@5yVV7fPvutA5HFA0WFDMCaQ2IZeVW@kIa1hyhrJpGiVolCmS2AYXsS8cl0BH4iwOiJIQMgHhYrr2yFMmoGzyiHbMfXJ7g6/bQGrYU7VJfdOIZokTTxvgGhFHpBdm3/pb6mRaGYNGnkouXrjXPjmalo7ukUhV6/qRNLD96XhVgztUhvebe6lI6LPdag8@B7N2yKxgnvGOuet97Esr4h7eYMjvAZekSb8XqXuE4Fiw0KuaI/wFtugA2sd0gkmVOJAsWSjBhE800dRsNmPyuBawgS2T1Y7WMUA8f@xr5K3Z0Z6b4F44LBbjaGlk5w@Eu3/SuKk1FeY/83C@JqrhjglzmdlCIZcvEL2LiFUqRrlODJx3kwjDOIQ7xpEiPVin0UcqvbJSzFBkQLs6@pY3HeKM08xz1hMYs16llkMKID@tyJyt27AjeNP6S7emgqQ8ZT/@1t1naI7/cVeg0SRjFG0eB5D94OZ3vs7duroTD6MrHi6MHnHG6PYGwugnU0DTFQ53E3Gkky@zdjAZQtU78nFlsFH7Go05K6Bo5FLSpIkOtWb9V6l4nQda8xdI@MJqwFwTF9pHYCQOdVrIrmcqIVgHs@M/tmQlPreCwHrfI6c05ENexpnbHA1WwHUXKna2eugPXIhMEDR7@4FqLrtOVmHJ88Qy0S4wUcTGADzhodU7OMFjgd@VJ755HoVhzURRkK7Ym@9w7Iy35bH8SuflIjMGKQZ0GlS3W5imezeYpgg5K7BjbzKRJKOaCVb/Ge7jspqWW1E70IB9yKuN5IZJvHLUy3w/BHnVCReEDYgDrT/RRkoSkN0@miLQbdmPJoSnJJRT6bFj80rMSd/L62Yhqbe1RLEcSAqlMmvEJharumB/cnxkrFkVWdqbuBZGR2y@lvNN6OPMfe@Wff@xhvRA@H8irvaVc6Lppx7e2@kBL3UoR4dMMriUiX3S74cMDjhL@XxqT0y0Fhp5GPkZv/J/0d9@Gf7QEqCNmklW3KFwBukzBhvHO7@sy7Tm0GJdXR1IS@kWRcNzzlvOG751J95duL0qipHAA@ZWVcN0lcCI9tkE3vCniPcf8Uw3g1paUsaC@/Ar8TV189YzBlZvkFg29enXjYVna@P6RfOYSDkrcgt5Wcuf8zCQMmOuMUw9sHXhK6QKnRfUpc6wmI1cj8T5Ra3RzGpaQW@oHXbRUcgVaDx7erEryQwA7/@K6hwKZFIiA1DkEU/plY2xk5TfzPodlUG9BdG2u15/WQYkMkmDBSsw1sjy/2qNX7JqFG68l44@apDzAD6tL/SocwhpK0M/HrFrXhE9hFQVd5F/x@0rjOxsYwDbSDYwQNSiNMnhKFGIp/as7a2Xwu21MoDoNYWG/OMRPzqXvQQcpEnanSOmfgLzPBzlkm/yQ9mNIxuOAe2f4jByRSwX1N1YKpxQ8i9FS3mg9A9AD8gcGMCGsJd8SCw6oqS7SwDKUxJXwsB62FGRm1vHHfZ1nztbHPZppnak6CmSJZUhMB7tZ3HlaGtSQ4OGXId6/D8QBiH2aupe3WjsQwiJHnE4juB1aiyc7qGLJgwzfOEvi4D8jdcuKTw3BvvHJ8ZVlNiM9nEyuw@IhCKePJaRB7K10g4Sz5U7kRTsr2XMNPCihksJNP5sI/k6WaZSl5zyTu@EA2zjUu3DVzKZuYzlX@BcQmC688xvvl9T3wJhnJLxyf0cjWzXzN40ji6wHvmkDiUbR2VNLQRXOVqOoVTDE8kfoW73Yatan6q3/h7vWG7kWOyeXjPEEPFRmbXtaPzYJsgGrIe0RIPiFSXgOucv0tPzWd7M/M3wn6O3cJFS5p7Do8iI/s7qwFDMsHYYNdyOmCGX2PYxN2gV06iQVaBlYkVltMk7q1YjpkQ5wiGtVgxrFENGHBoT6vahth5/d3PHmnnX5KMVfAIEGZWPvDX5FFsvegSNJvhHHOvXWrfa/FIOvwYdIhpdEhdhgxp1CS2Pm@1sMkAtWNNVgNVXOQxYGdArj1ww4w9XZVMrnmUx3nLTi@EvbX3NIqf9QjP4L3OqUuYBMkWbRrX5ZJW2TpLTYzMBvFHUdzF466k7CjhpzrYnUh@Q4N3dtAiiMiw/u6SD0awNMjGyFI6myNfZeWtCMkKq3nqnMElstxSw==;0400dDcnTXD@yFqVebKatfMjIK1ERcM6ZNoONTTq0uyedkSVVakfgxwJGKO3zcmEmtu4EfI6ZGY03ZnZWJshrWw4a2TmOqLA3SnRCZUmq3zPGJExj8LxReaEyCB2iNik7iIr8jUlgc/esLdasSNeRCt8mLF@dSx4JcI71eHfkZ0HoJzSt0oGtrebhiPd@jrpPy8mm/9pJ@xHDpwoyGA2cdeICpVPfVKB9sZJaUE@/ER7RrJV08eta5oFVyHyWBxz1/zBKxOBAH1M/7npjUD4D5TC9wAH/1F3u3pMTtU6fOHu/7cJlSarfM8YkTGPwvFF5oTIIHaI2KTuIivyNSWBz96wt1qxI15EK3yYsX51LHglwjvvUSbJ@Ltz3R3@1lLYqHZEfQCvUdWhuUpMMcbzVQ5Kf1Rhqn1mAacBb@O5jfppV29TpBMXmWQDyAPU04zDOrU4PNeFcle3tcyQcvX72uJd91sH6hTfDiCwKNnXQKSQGIEgdQ96CsfXnvCEwsgaKn@ptC5PX323DKxhQIcdQInU0UZJHzcQn1wyS9qsJGo6D9bLlVf/4mF0SN3nwUF1kmdB@CTCf159hg3wYrrd0IzkpozknZJswSmTayZs9Aj1mjNgcn4QStjKmL786XE8OdE7om0Pb9/VslsNtsgeF66a7HSd@3cQQ5uQNgkfBf1FVTeLoiSptVZTjkN13n2vmAeyue@B4M2wkqI603mnG1d3lJKWH8Rzboz1meO52CQtIDMp@S3pTn9DoIzV7IDBIIRtgx5t4CWwXeyrVjpSL/tvtZhHDkjP/VESgriaMoAypQuyPr0t8ztVFq5TfgMH9KPXrrGto44dFZ9853x4wVtDOhzmWIC3GmtyYhGrWtLxXZsld8E1pw3t/nKidDtfDg7G51W@7h4F0BmqCMhOtBVzKv7F9SjGl3IQDIKINVrQrNJJ7TMAwER7lVTDSLoO5541pqenrHyiDh1WC4Z0KPmWF2NqlEr5uVAcOe0RxWZbWQwi@FoE/xhlgME25KpZgUeTfmJPV146JuOeK6E2gM4YS@shaHYP8Oq60HLHyg@On/HuhzmHVhuxKCWL4CMVF3tROHNEqRjKSr2P1XHbFbWdu9IgF8s35HQzCXZjVj@Jpy8kiBJpMqMw/SVBeKzAru@JYXyFHBbsmyyzb0vSwWRA@eSMeWgCk0M8gU5@pf0qGt1hmfU9oknoVD8HnO@s8lyWpM5YbymrVY5sgkvwelg@7eHGpR1rGZzjosGVnNFXCIRV8Lsfk4HM8i12UxXXpPOYi1x/PkiSrga@efjNEmDTN8/Atc66JFixlzv0IoWxE4jMo6UW8YjioM31H1JTQYwXhsCX3/B/a2rGATbVCnLypfj6q9L4HUSktuxY9kVdcXzSNhj/NHVIcptjSEWMMrddTfJhbTrnC8@lkCloMEqxA2Lb4CviK4lXTR0L2pIt46KgkcGTZ6ZDBUBP@tL/exC9lnzn0cIt4LcXwBmRkMWjWbMMV1wESSa8zMm9ldXBIXlFXx@VLtT2uizMMLIhZ29v1uffSuoARCCbabuLBDJ3/IABbIS7Ah1D/hSbhrUOybusKrZIfmD2q664Kxl68LkM/V4NR7tlsdDPZod/Q8LYFv@/zbP/@zffQ1iq1uTKn168H364SMBcF@oySEaWDpcbp8j@YQ7XE5DcdS0BeYFRWASA2ZIHUZl0QX0tKuQVwRM4eC2WKlVzWGwwnBeevqpgRenIywTtXqoW@j/@e7IFFXL7Jsr7Z3P5gCi/IUYfrFmn/VmTX1Ud1SAwJhkYMcreAOFypIxnMTp4iqZBwCuhnA0mTpg6a6sxK5ecnqitia//pRsG10hdsHz5nDkr9Agds@qdQ/WUqlYYP6GT95DCtR6MVtpYZvc0hFBKhWCj6XtK3@eX7t4TSz8tgClx1d1nx0WsatpOz4EA0d8HAl7Vu8fi@dP1dbx1lOfEWzY45jFgkfknF9XxKxunxlbxVX8ZzHX56MJkJOcEMkT@9FULOZA1AfHzEJo@HVqwPe0hPA2sTlzPJi0VLr4DxLG4IXMeo22d5gaxdgU4U@NSVzAEj5fpBsB5oIzE3nd5tVBhRoKGv5@umG6T0wP0jmVFKB5XjqeY9jp6GK7C88YE@OBMLLMAMZAnL4fedOIYgHC/3Qa@FgZF8ZJ0PORBIyvNed0Obpguo@ImB8YwPczMl6YPBc/VXt0Q0Axc41FsapL@xegxmk7GhTteDPdC6LJAJVPzRWHgsHPLq5eRbPFuF/l8fwE2lfLnVpkS981aWshsaoWsP4u2jo2vZE/NJpTFpzXd7Y58m56pIPagl6G09W/6g5jnqNjMlfn6pjpzDAiX6eB@Z6ri@rK75gW3M5QM5he@o90ZLYMPRPXz7T1qc@idKKCD3M9nOMB915362ubh5w8cjVspt9PJQZoKdLsnNr6F";


            string DefaultEn = "utf-8";
            CookieContainer cc = new CookieContainer();
            string cDoc = null;
            string cUrl = null;
            string cReferer = null;
            string cLoginData = null;
            cUrl = "http://" + ui.cUrl + "/app/member/";
            cReferer = "http://" + ui.cUrl + "/";
            cDoc = Connect.getDocument(cUrl, cc, cReferer, DefaultEn, false, true);

            cUrl = "http://" + ui.cUrl + "/iovation/vindex.php";
            cReferer = "http://" + ui.cUrl + "/";
            cLoginData = "blackbox=" + blackbox;
            cDoc = Connect.postDocument(cUrl, cLoginData, cc, DefaultEn);

            cLoginData = "username=" + ui.User + "&passwd=" + ui.Pwd + "&langx=zh-cn&auto=CHCAEF&blackbox=" + blackbox + "&nowsite=new";
            cUrl = "http://" + ui.cUrl + "/app/member/new_login.php";
            cReferer = "http://" + ui.cUrl + "/app/member/";
            cDoc = Connect.postDocument_hg(cUrl, cLoginData, cc, cReferer, DefaultEn, false, false);
            if (cDoc == null) return ui; ;
            if (cDoc.IndexOf("Plaese wait 3 minutes and try again") > 0)
            {
                return ui;
            }
            string[] uid = cDoc.Split('|');

            if (uid.Length > 5)
            {
                ui.Status = 1;
                ui.Uid = uid[3];
                ui.cc = cc;
            }
            else
            {
                ui.Status = 0;

            }
            return ui;
        }

        public static UserInfo Odds(UserInfo ui)
        {

            //sh.Start();
            string DefaultEn = "utf-8";
            string cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=3&page_no=0&league_id=&hot_game=";
            string cReferer = "http://" + ui.cUrl + "/app/member/FT_browse/index.php?rtype=re&uid=" + ui.Uid + "&langx=zh-cn&mtype=3&showtype=&league_id=&hot_game=";
            string cDoc = Connect.getDocument(cUrl, ui.cc, cReferer, DefaultEn, false, true);
            //TimeSpan tii = sh.Elapsed;
            //Console.WriteLine(tii.TotalSeconds + cUrl);
            //TimeSpan ti = sh.Elapsed;
            //Console.WriteLine(ti.TotalSeconds + "hgodds");

            if (cDoc == null) return ui;
            string[] jsarr = cDoc.Split(';');
            if (jsarr.Length == 1)
            {
                if (jsarr[0].IndexOf("logout") > 0)
                {
                    ui.Status = 0;
                    return ui;
                }
            }
            string s = null;
            ArrayList al = new ArrayList();
            Hashtable ht = new Hashtable();
            System.Diagnostics.Stopwatch sh = new System.Diagnostics.Stopwatch();
            sh.Start();
            foreach (string a in jsarr)
            {
                if (a.StartsWith("\ng(["))
                {
                    string line = a.Replace("'", "");
                    line = line.Replace(" ", "");
                    string[] arr = line.Split(',');
                    s += arr[2].Trim();// pi_old.LianSai.Trim();
                    s += "," + arr[5].Trim();//pi_old.QiuDui.Trim();
                    s += "," + arr[6].Trim() + ",,,,\n";// pi_old.QiuDui_Fu.Trim();

                    int lsId = Conf.GetLianSaiID(arr[2].Trim());
                    int qd1 = Conf.GetteamKeyID(arr[5].Trim());
                    int qd2 = Conf.GetteamKeyID(arr[6].Trim());

                    //string lsId = (arr[2].Trim());
                    //string qd1 = (arr[5].Trim());
                    //string qd2 = (arr[6].Trim());

                    if (lsId == -1 || qd1 == -1 || qd2 == -1) continue;//这个条件是过滤没有比对的联赛跟球队，待定
                    string key = lsId.ToString() + qd1.ToString() + qd2.ToString();
                    // string pkkey = key + ":" + arr[8].Trim().ToString();
                    string[] htarr = ParseLineds(arr);

                    if (ht.ContainsKey(key))
                    {//判断第一层是否有数据，如果有就给第二层加数据
                        Hashtable hts = (Hashtable)ht[key];//取出第一层对象
                        string id = (hts.Count + 1).ToString();
                        hts.Add(id, htarr);//给对象加数据
                        ht[key] = hts; //更新第一层数据


                    }
                    else
                    {
                        Hashtable hts = new Hashtable();//第二层数据
                        string id = (hts.Count + 1).ToString();
                        hts.Add(id, htarr);
                        ht.Add(key, hts);
                    }

                }
            }
            TimeSpan tii = sh.Elapsed;
            Console.WriteLine(tii.TotalSeconds + "ieieieiei");

            ui.ODDS = s;

            ui.Al = al;

            return ui;

        }
        private static string[] ParseLineds(string[] arr)
        {
            //int lsId = Conf.GetLianSaiID(arr[2].Trim());
            //int qd1 = Conf.GetteamKeyID(arr[5].Trim());
            //int qd2 = Conf.GetteamKeyID(arr[6].Trim());
            //if (lsId == -1 || qd1 == -1 || qd2 == -1) return null;//这个条件是过滤没有比对的联赛跟球队，待定
            //string key = lsId.ToString() + qd1.ToString() + qd2.ToString();
            Hashtable ht = new Hashtable();
            //return ht;
            string cFormat = "时间,比分,联赛,主队,客队,全场亚洲盘上盘,全场亚洲盘下盘,上盘赔率,下盘赔率,全场大小盘盘口,大赔率,小赔率,上半场上盘,上半场下盘,上半场上盘赔率,上半场下盘赔率,上半场大小盘盘口,上半场大赔率,上半场小赔率,扩展信息1,扩展信息2,扩展信息3,扩展信息4,扩展信息5,扩展信息6,扩展信息7,扩展信息8";

            string cLianSai = arr[2].Trim();// pi_old.LianSai.Trim();
            string cQiuDui = arr[5].Trim();//pi_old.QiuDui.Trim();
            string cQiuDui_Fu = arr[6].Trim();// pi_old.QiuDui_Fu.Trim();
            string cBiFen = arr[18].Trim() + "-" + arr[19].Trim();// pi_old.BiFen;



            cBiFen = "0-0";
            string cTime = arr[1].Trim();
            if (cTime.IndexOf(":") > 0) cTime = "0";
            if (cTime.IndexOf("中场") >= 0)
            {
                cTime = "45";
            }
            //int iTime = Convert.ToInt32(cTime);
            string EventNo = arr[0].Replace("\n", "");
            //string EventNo2 = Convert.ToString(Convert.ToInt32(EventNo.Trim()) + 1);
            string GNum1 = arr[3];
            string GNum2 = arr[4];
            string strong1 = arr[7].Trim();
            string strong2 = arr[23].Trim();




            string[] arrReturn = cFormat.Split(',');
            arrReturn[0] = cTime.ToString();
            arrReturn[1] = cBiFen;
            arrReturn[2] = cLianSai;
            arrReturn[3] = cQiuDui;
            arrReturn[4] = cQiuDui_Fu;


            #region 全场亚洲盘_上盘
            if (arr[7].Trim().Equals("H"))
            {
                arrReturn[5] = arr[8].Trim().Replace("-", "/");
                arrReturn[6] = "";
                if (arrReturn[5].Equals("0.0")) arrReturn[5] = "0";
            }
            else
            {
                arrReturn[5] = "";
                arrReturn[6] = arr[8].Trim().Replace("-", "/");
                if (arrReturn[6].Equals("0.0")) arrReturn[6] = "0";
            }

            arrReturn[7] = arr[9];
            arrReturn[8] = arr[10];

            arrReturn[19] = "FT_order/FT_order_r.php?gid=" + EventNo + "&type=H&gnum=" + GNum1 + "&strong=" + strong1 + "&odd_f_type=H";

            arrReturn[20] = "FT_order/FT_order_r.php?gid=" + EventNo + "&type=C&gnum=" + GNum2 + "&strong=" + strong1 + "&odd_f_type=H";
            //Console.WriteLine(arrReturn[19]);
            //Console.WriteLine(arrReturn[20]);

            #endregion
            #region 全场大小盘_大盘
            arrReturn[9] = arr[11].Replace("O", "");
            arrReturn[10] = arr[14].ToString().Trim();
            arrReturn[21] = "FT_order/FT_order_rou.php?gid=" + EventNo + "&type=C&gnum=" + GNum2 + "&odd_f_type=H";

            arrReturn[11] = arr[13].ToString().Trim();
            arrReturn[22] = "FT_order/FT_order_rou.php?gid=" + EventNo + "&type=H&gnum=" + GNum1 + "&odd_f_type=H";



            #endregion

            #region 上半场亚洲盘_上盘

            if (arr[23].Trim().Equals("H"))
            {
                arrReturn[12] = arr[24].ToString().Trim().Replace("-", "/");
                arrReturn[13] = "";
                if (arrReturn[12].Equals("0.0")) arrReturn[12] = "0";
            }
            else
            {
                arrReturn[12] = "";
                arrReturn[13] = arr[24].ToString().Trim().Replace("-", "/");
                if (arrReturn[13].Equals("0.0")) arrReturn[13] = "0";
            }

            arrReturn[14] = arr[25];
            arrReturn[15] = arr[26];
            // arrReturn[23] = "FT_order/FT_order_hr.php?gid=" + EventNo2 + "&type=H&gnum=" + GNum1 + "&strong=" + strong2 + "&odd_f_type=H";

            //arrReturn[24] = "FT_order/FT_order_hr.php?gid=" + EventNo2 + "&type=C&gnum=" + GNum2 + "&strong=" + strong2 + "&odd_f_type=H";
            //Console.WriteLine(arrReturn[23]);
            //Console.WriteLine(arrReturn[24]);
            #endregion
            #region 上半场大小盘_大盘
            arrReturn[16] = arr[27].Replace("O", "");
            arrReturn[17] = arr[30];
            arrReturn[18] = arr[29];
            //arrReturn[25] = "FT_order/FT_order_hou.php?gid=" + EventNo2 + "&type=C&gnum=" + GNum2 + "&odd_f_type=H";
            //arrReturn[26] = "FT_order/FT_order_hou.php?gid=" + EventNo2 + "&type=H&gnum=" + GNum1 + "&odd_f_type=H";
            //Console.WriteLine(arrReturn[25]);
            //Console.WriteLine(arrReturn[26]);



            #endregion

            string cLineReturn = null;
            for (int i = 0; i < arrReturn.Length; i++)
            {
                if (cLineReturn == null)
                {
                    cLineReturn = arrReturn[i];
                }
                else
                {
                    cLineReturn += "," + arrReturn[i];
                }
            }
            //ht.Add(key, arr);
            //Console.WriteLine(cLineReturn);
            return arrReturn;


        }
        /// <summary>
        ///  早餐接水
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static UserInfo Ball(UserInfo ui)
        {

            Ball_ZZZ(ui);
            //return ui;

            int t_page = 0;
            int page   = 0;

           Path:  //goto 标签

           
            string DefaultEn = "utf-8";
            //cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=3&page_no=0&league_id=undefined&hot_game=&isie11=N";//今日
            string cUrl = "http://" + ui.cUrl + "/app/member/FT_future/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=1&page_no="+ page.ToString() + "&league_id=&hot_game=&g_date=ALL&isie11=N";//早餐
            string cReferer = "http://" + ui.cUrl + "/app/member/FT_browse/index.php?uid=" + ui.Uid + "&langx=zh-cn&mtype=3&league_id=";
            string cDoc = Connect.getDocument(cUrl, ui.cc, cReferer, DefaultEn, false, true);
          
            if (cDoc == null)
            {
                ui.Status = 2;
                ui.Msg = "采不到数据";
                return ui;
            }
            else
            {
                ui.Msg = "正常";
                ui.Status = 1;
            }
            string[] jsarr = cDoc.Split(';');
            if (jsarr.Length == 1)
            {
                if (jsarr[0].IndexOf("logout") > 0)
                {
                    ui.Status = 0;
                    ui.Msg = "退出，请登录";
                    return ui;
                }
            }
            else
            {
                ui.Msg = "正常";
                ui.Status = 1;
            }
            string s = null;
            ArrayList al = new ArrayList();
            Hashtable ht = new Hashtable();
            System.Diagnostics.Stopwatch sh = new System.Diagnostics.Stopwatch();
            sh.Start();
            foreach (string a in jsarr)
            {

                //找到分页总数
                if (a.IndexOf("_.t_page") > -1)
                {
                    string[] Pa = a.Split('=');
                    t_page=Convert.ToInt32(Pa[1]);
                }

                if (a.StartsWith("\ng(["))
                {
                    string line = a.Replace(" ", "");
                    //line = "g(['3894824','0.5','0.890','1.010','O3','U3','0.870','1.010','1.89','3.85','3.85','单','双','1.94','1.93','3894825','H','0 / 0.5','1.030','0.850','O1 / 1.5','U1 / 1.5','0.800','1.080','2.46','3.95','2.27','74','8DBCB9BCBDBCBABCB7CCB6CCBDBCB38AC8CBCAC7C8CDCBA9B3','','unas','N','2785475','Y','N','0','0','','N']);";
                    line = line.Replace("g(", "").Replace(")", "").Replace("[", "{").Replace("]", "}");
                    //Console.WriteLine(line);
                    line = line.Replace("'", "");
                    string[] arr = line.Split(',');
                    s += arr[2] + "," + arr[5] + "," + arr[6] + ",,,\n";
                    if (arr[1].IndexOf("半场") > 0) continue;
                    if (Conf.LianSai.ContainsKey(arr[2])) continue;//过滤联赛
                    if (Conf.teamKey.ContainsKey(arr[5])) continue;//过滤球队

                    string[] sArr = Regex.Split(arr[1], "<br>", RegexOptions.IgnoreCase);
                    string stime = sArr[0] + " " + sArr[1].Replace("a", "").Replace("p", "");



                    long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                    ht["stime"] = stime;
                    ht["Lid"] = arr[0].Replace("\n{", "");
                    ht["Ls"] = arr[2];
                    ht["Q1"] = arr[5];
                    ht["Q2"] = arr[6];
                    ht["HC"] = arr[7];
                    ht["pan"] = arr[8];
                    ht["Q1adds"] = arr[9];
                    ht["Q2adds"] = arr[10];
                    ht["addtime"] = epoch;
                    ht["ctype"] = 2;
                   
                    ArrayList alk = new ArrayList();
                    ArrayList alv = new ArrayList();

                    foreach (DictionaryEntry myDE in ht)
                    {
                        alk.Add(myDE.Key);
                        alv.Add("'" + myDE.Value.ToString() + "'");
                    }
                    string str = string.Join(",", (string[])alk.ToArray(typeof(string)));
                    string strv = string.Join(",", (string[])alv.ToArray(typeof(string)));

                    string sql = "INSERT INTO  db_odds(" + str + ") VALUES (" + strv + ");";
                    DbHelperMySQL.ExecuteSql(sql);

                }

            }

            page++;
            Console.WriteLine(page);
            if (page <= t_page - 1)
            {
                Thread.Sleep(10000);
                goto Path;
            }

            ui.Ht = ht;
            ui.ODDS = s;
            return ui;
        }

        /// <summary>
        ///  今日接水
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>

        public static UserInfo Ball_ZZZ(UserInfo ui)
        {
            int t_page = 0;
            int page = 0;

            Path:  //goto 标签  解决分页问题
            string DefaultEn = "utf-8";
            //string cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=re&langx=zh-cn&mtype=3&page_no=0&league_id=&hot_game=";
            //string cReferer = "http://" + ui.cUrl + "/app/member/FT_browse/index.php?rtype=re&uid=" + ui.Uid + "&langx=zh-cn&mtype=3&showtype=&league_id=&hot_game=";

            string cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=3&page_no=" + page.ToString() + "&league_id=undefined&hot_game=&isie11=N";//今日
                   //cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=3&page_no=" + page.ToString() + "&league_id =&hot_game=&isie11=N";
                   //cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=3&page_no=0&league_id=undefined&hot_game=&isie11=N";//今日
                   //cUrl = "http://" + ui.cUrl + "/app/member/FT_browse/body_var.php?uid=" + ui.Uid + "&rtype=r&langx=zh-cn&mtype=3&page_no=0&league_id=&hot_game=&isie11=N";
            string cReferer = "http://" + ui.cUrl + "/app/member/FT_browse/index.php?uid=" + ui.Uid + "&langx=zh-cn&mtype=3&league_id=";
            string cDoc = Connect.getDocument(cUrl, ui.cc, cReferer, DefaultEn, false, true);
        
            if (cDoc == null)
            {
                ui.Status = 2;
                ui.Msg = "采不到数据";
                return ui;
            }
            else
            {
                ui.Msg = "正常";
                ui.Status = 1;
            }
            string[] jsarr = cDoc.Split(';');
            if (jsarr.Length == 1)
            {
                if (jsarr[0].IndexOf("logout") > 0)
                {
                    ui.Status = 0;
                    ui.Msg = "退出，请登录";
                    return ui;
                }
            }
            else
            {
                ui.Msg = "正常";
                ui.Status = 1;
            }
            string s = null;
            ArrayList al = new ArrayList();
            Hashtable ht = new Hashtable();
            Hashtable hLianSai = new Hashtable();
            Hashtable hOdds = new Hashtable();
            System.Diagnostics.Stopwatch sh = new System.Diagnostics.Stopwatch();
            sh.Start();
            foreach (string a in jsarr)
            {
                //找到分页总数
                if (a.IndexOf("_.t_page") > -1)
                {
                    string[] Pa = a.Split('=');
                    t_page = Convert.ToInt32(Pa[1]);
                }
                if (a.StartsWith("\ng(["))
                {
                    string line = a.Replace(" ", "");
                    //line = "g(['3894824','0.5','0.890','1.010','O3','U3','0.870','1.010','1.89','3.85','3.85','单','双','1.94','1.93','3894825','H','0 / 0.5','1.030','0.850','O1 / 1.5','U1 / 1.5','0.800','1.080','2.46','3.95','2.27','74','8DBCB9BCBDBCBABCB7CCB6CCBDBCB38AC8CBCAC7C8CDCBA9B3','','unas','N','2785475','Y','N','0','0','','N']);";

                    line = line.Replace("g(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace("\n", "");
                    line = line.Replace("'", "");
                    string[] sArray = line.Split(',');
                    string key = sArray[0]; //联赛ID
                    string cArr = string.Join(",", sArray);
                    hOdds.Add(key, cArr);

                }
                //Console.WriteLine(a);
                if (a.IndexOf("gm[") > -1)
                {
                    string line = a.Replace(" ", "");
                    //line = "_.gm['3909468']=['10-29<br>04:30a<br><font color=red>Running Ball</font>','印尼甲组联赛','21664','21663','帕尔斯巴亚','斯莱曼','H'];";
                    line = line.Replace("_.gm['", "").Replace("'", "").Replace("\n", "");
                    string[] sArray = Regex.Split(line, "]=", RegexOptions.IgnoreCase);
                    string key = sArray[0]; //联赛ID
                    string v = sArray[1].Replace("'", "").Replace("[", "").Replace("]", "").Replace(";", "");
                    string[] sArr = Regex.Split(v, "<br>", RegexOptions.IgnoreCase);
                    string stime = sArr[0] + " " + sArr[1].Replace("a", "").Replace("p", "");
                    string[] str = v.Split(',');
                    str[0] = stime;
                    string cArr = string.Join(",", str);
                    hLianSai.Add(key, cArr);
                }

            }
            HeBing(hOdds, hLianSai);

            page++;
            Console.WriteLine(page);
            if (page <= t_page - 1)
            {
                Thread.Sleep(10000);
                goto Path;
            }
            ui.Ht = ht;
            ui.ODDS = s;
            return ui;
        }

        public static void HeBing(Hashtable hOdds, Hashtable hLianSai)
        {

            foreach (DictionaryEntry de in hOdds)
            {
                Hashtable ht = new Hashtable();
                //Console.WriteLine(string.Format("{0}-{1}", de.Key, de.Value));
                //Console.WriteLine(hLianSai[de.Key].ToString());
                string[] odds = de.Value.ToString().Split(',');
                string[] LianSai = hLianSai[de.Key].ToString().Split(',');
                long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                ht["stime"] = LianSai[0];
                ht["Lid"] = de.Key;
                ht["Ls"] = LianSai[1];
                ht["Q1"] = LianSai[4];
                ht["Q2"] = LianSai[5];
                ht["HC"] = LianSai[6];
                ht["pan"] = odds[1];
                ht["Q1adds"] = odds[2];
                ht["Q2adds"] = odds[3];
                ht["addtime"] = epoch;
                ht["ctype"] = 1;

                ArrayList alk = new ArrayList();
                ArrayList alv = new ArrayList();
                foreach (DictionaryEntry myDE in ht)
                {
                    alk.Add(myDE.Key);
                    alv.Add("'" + myDE.Value.ToString() + "'");
                }
                string str = string.Join(",", (string[])alk.ToArray(typeof(string)));
                string strv = string.Join(",", (string[])alv.ToArray(typeof(string)));

                string sql = "INSERT INTO  db_odds(" + str + ") VALUES (" + strv + ");";
                DbHelperMySQL.ExecuteSql(sql);

            }



        }
        public static int bet(UserInfo ui, string[] arr, int Type)
        {

            string cul = null;
            if (Type == 1)
            {
                cul = arr[21];
            }
            else
            {
                cul = arr[22];
            }

            string cUrl = "http://" + ui.cUrl + "/app/member/" + cul + "&uid=" + ui.Uid + "&langx=zh-cn";
            string cR = "http://" + ui.cUrl + "/app/member/select.php?uid=" + ui.Uid + "&langx=zh-cn";
            string s = Connect.getDocument(cUrl, ui.cc, cR);
            Hashtable data = Pfun.fromSetting(s);
            string Url = "http://" + ui.cUrl + "/" + data["url"].ToString();
            string data1 = data["data"].ToString().Replace("btnCancel=取消&Submit=确定交易&wgCancel=取消&wgSubmit=确定交易&", "");
            data1 = data1.Substring(0, data1.Length - 1);
            data1 = data1.Replace("gold=", "gold=" + Conf.money);
            s = Connect.postDocument(Url, data1, ui.cc, cUrl);
            if (s.IndexOf("成功提交注单") > 0)
            {
                Console.WriteLine("成功提交注单");
                return 1;
            }
            return 0;
        }

        public static string history(UserInfo ui)
        {

            string cUrl = "http://" + ui.cUrl + "/app/member/history/history_data.php?uid=" + ui.Uid + "&langx=zh-cn";
            string s = Connect.getDocument(cUrl, ui.cc);
            Console.WriteLine(s);
            return s;
        }
    }
}
