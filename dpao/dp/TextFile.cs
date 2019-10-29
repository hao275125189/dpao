using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace dpao.dp
{
    #region TextFile 文本文件读写类
    public class TextFile
    {
        #region 读文件
        public static string ReadFile(string cPath)
        {
            return ReadFile(cPath, "utf-8");
        }
        public static string ReadFile(string cPath, string en)
        {
            if (!File.Exists(cPath)) return null;
            try
            {
                FileStream aFile = new FileStream(cPath, FileMode.Open,FileAccess.Read,FileShare.Read);
                StreamReader sr = new StreamReader(aFile,System.Text.Encoding.GetEncoding(en));
                string s = sr.ReadToEnd();
                sr.Close();
                aFile.Close();
                return s;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public static void WriteFile(string cPath, string cDoc)
        {
            WriteFile(cPath, cDoc, "utf-8");
        }

        public static void WriteFile(string cPath, string cDoc, string en)
        {
            if (en == null) en = "utf-8";
            try
            {
                StreamWriter sw = new StreamWriter(cPath, false, System.Text.Encoding.GetEncoding(en));
                sw.Write(cDoc);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
    #endregion
}
