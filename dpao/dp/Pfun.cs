using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Management;

namespace dpao.dp
{
    class Pfun
    {
        public static string FromUnicodeString(string str)
        {
            //最直接的方法Regex.Unescape(str);
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException ex)
                {
                    return Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }
        public static Hashtable fromSetting(string html)
        {
            Hashtable ht = new Hashtable();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            string data = null;
            string cUrl = null;
            foreach (HtmlNode form in doc.DocumentNode.SelectNodes("//form"))
            {
                cUrl = form.Attributes["action"].Value;
                ht["url"] = cUrl;
                //Console.WriteLine("Found: " + form.Attributes["action"].Value);               
                //HtmlAttributeCollection attrs = form.Attributes;
                //// Console.Write(attrs.Count.ToString());
                //Console.WriteLine("Found: " + form.ChildAttributes("action"));


                foreach (HtmlNode row in form.SelectNodes("//input"))//这里只是遍历所有的INPUT元素
                {
                    HtmlAttributeCollection input = row.Attributes;
                    Console.WriteLine("row");
                    string cInputName = null;
                    string cInputValue = null;
                    foreach (var item in input)//这里只是遍历一个元素所有属性
                    {
                        if (item.Name.ToString().ToLower().Equals("name"))
                        {
                            cInputName = item.Value;
                        }
                        if (item.Name.ToString().ToLower().Equals("value"))
                        {
                            cInputValue = item.Value;
                        }

                    }
                    if (cInputName == null) continue;
                    if (cInputName.Equals("key")) ht["key"] = cInputValue; ;
                    data += cInputName + "=" + cInputValue;
                    data += "&";
                }

            }
            if (data != null) ht["data"] = data;
            return ht;
        }

        public static Hashtable HtmlTable(string s)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(s);
            Hashtable htc = new Hashtable();
            int index = 1;
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                // table.Attributes
               // Console.WriteLine("Found: " + table.Id);
                HtmlAttributeCollection attrs = table.Attributes;
                // Console.Write(attrs.Count.ToString());
                if (attrs.Count != 6) continue;

               
                
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                   // Console.WriteLine("row");
                   // Console.WriteLine(row.HasAttributes);
                    int i = 0;
                    Hashtable ht =new Hashtable();
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        string val = null;
                        //Console.WriteLine(i);
                        val = cell.InnerText;
                       // Console.WriteLine("cell: " + cell.InnerText);
                            //string sp = SelectSP(cell.InnerText.ToString());
                        if (i == 9)
                        {
                            string ss = Selectvalue(cell.InnerHtml,"a");
                            string u = "http://vip.win007.com/changeDetail/overunder.aspx?id="+ss+"&companyid=3";
                            val=u;
                           
                        }
                        ht.Add(i, val);
                        i++;
                           
                      
                    }
                  
                    htc.Add(index,ht);
                    index++;
                   
                }
            }
            return htc;
        }



        public static ArrayList HtmlTableId(string s,string Id)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(s);
            ArrayList al = new ArrayList();
           // HtmlAgilityPack.HtmlNodeCollection node = nodes.SelectNodes("//span[@class='category-pop']/a");
          HtmlNodeCollection hc=doc.DocumentNode.SelectNodes("//td[@class='td3']");
          foreach (HtmlNode nd in hc)
          {

            //Console.WriteLine(  nd.InnerText);
            string[] p = nd.InnerText.Replace(" ","").Split('\n');
            al.Add(p);
          }
            return al;
        }
        /// <summary>
        /// 走势
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Hashtable HtmlTableDX(string s)
        {
            if (s == "") return null;
            HtmlDocument doc = new HtmlDocument();
            s = s.Replace("<td><table","<td>");
            s = s.Replace("</tr></table>", "</tr>") + "</table>";
            doc.LoadHtml(s);

            Hashtable htc = new Hashtable();
            int index = 1;
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                // table.Attributes
                // Console.WriteLine("Found: " + table.Id);
                HtmlAttributeCollection attrs = table.Attributes;
                // Console.Write(attrs.Count.ToString());
                //if (attrs.Count != 6) continue;



                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    // Console.WriteLine("row");
                    // Console.WriteLine(row.OuterHtml);
                    string style = null;
                    if ((row.OuterHtml.ToString().IndexOf("blue") > 0)) style = "blue";
                    if ((row.OuterHtml.ToString().IndexOf("red") > 0)) style = "red";
                    if ((row.OuterHtml.ToString().IndexOf("green") > 0)) style = "green";
                    int i = 0;
                    Hashtable ht = new Hashtable();
                    string[] xval = row.OuterHtml.ToString().Split(new Char[]{'<','>','\''});
                    Console.WriteLine();
                    ht.Add("00", xval[2]);
                    ht.Add("style", style);
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        string val = null;
                        //Console.Write(i);

                        val = cell.InnerText;
                       // Console.WriteLine(val);
                       
                        //string sp = SelectSP(cell.InnerText.ToString());
                        if (i == 2)
                        {
                            val = val.Replace("border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\">&nbsp;", "");
                        }
                       // Console.WriteLine("cell: " + val);
                        ht.Add(i, val);
                        i++;


                    }

                    htc.Add(index, ht);
                    index++;

                }
            }
            return htc;
        }
        public static string Selectvalue(string doc, string node)
        {
            if (doc.Equals("数据")) return null;
            HtmlDocument docnode = new HtmlDocument();
            docnode.LoadHtml(doc);
            HtmlNode font = docnode.DocumentNode.SelectSingleNode(node);
            string str=font.OuterHtml.ToString();
            string[] arrays = str.Split(new char[] {'(',')',':'});
           
           // Console.WriteLine(font.InnerText);
           // return font.InnerText.ToString();
            return arrays[2];
        }

        public static string GetElementbyId(string doc,string Id) {

            HtmlDocument docnode = new HtmlDocument();
            docnode.LoadHtml(doc);
            HtmlNode ne = docnode.GetElementbyId(Id);
            string html=  ne.InnerHtml;
          return html;
        }
/// <summary>
        ///  //取CPU编号
/// </summary>
/// <returns></returns>
        public static String GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }//end method
         public static string GetMainHardDiskId() 
        { 
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia"); 
                String hardDiskID=null; 
                //foreach (ManagementObject mo in searcher.Get()) 
                //{ 
                //        hardDiskID = mo["SerialNumber"].ToString().Trim(); 
                //        break; 
                //} 
                return hardDiskID;    
        } 

    }
}
