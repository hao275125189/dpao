using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading;
using Ronlier;

namespace dpao.dp
{
    #region Connect 网络连接类
    public class Connect
    {
        public static string loctionUrl = null;
        public Connect() { }

        #region 定义全局变量
        public static string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.27 Safari/525.13;";
        private static int Timeout = 20000;
        #endregion


        public static string GetTextContent(HttpWebResponse rp, string en)
        {
            try
            {
                string cContent = null;
                string cAcceptEncoding = rp.ContentEncoding;
                if (en == null) en = rp.CharacterSet;
                if (en == null) en = Encoding.Default.ToString();

                string cTransferEncoding = rp.Headers["Transfer-Encoding"];

                Stream rs = rp.GetResponseStream();
                if (cAcceptEncoding.ToLower().IndexOf("gzip") >= 0)
                {

                    List<byte> _lData = new List<byte>();
                    while (rs.CanRead)
                    {
                        int v = rs.ReadByte();
                        if (v == -1) break;
                        _lData.Add((byte)v);
                    }
                    byte[] arrData = new byte[_lData.Count];
                    for (int i = 0; i < arrData.Length; i++)
                    {
                        arrData[i] = _lData[i];
                    }
                    MemoryStream ms = new MemoryStream(arrData);

                    #region 解密GZIP
                    GZipStream compressedStream = new GZipStream(ms, CompressionMode.Decompress, true);
                    if (cTransferEncoding != null)
                    {

                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[10240000];
                            List<byte> lData = new List<byte>();

                            while (compressedStream.CanRead)
                            {
                                int v = compressedStream.ReadByte();
                                if (v == -1)
                                    break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(compressedStream, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }
                    compressedStream.Close();
                    #endregion
                }
                else
                {
                    if (cTransferEncoding != null)
                    {
                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[2048000];
                            List<byte> lData = new List<byte>(bData.Length);
                            while (rs.CanRead)
                            {
                                int v = rs.ReadByte();
                                if (v == -1) break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("无法解密:" + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(rs, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }

                }
                return cContent;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #region Post请求
        public static string postDocument(string cUrl, string cData)
        {
            return postDocument(cUrl, cData, null, null, null, false);
        }
        public static string postDocument(string cUrl, string cData, CookieContainer cc)
        {
            return postDocument(cUrl, cData, cc, null, null, false);
        }
        public static string postDocument(string cUrl, string cData, CookieContainer cc, string cReferer)
        {
            return postDocument(cUrl, cData, cc, cReferer, null, false);
        }
        public static string postDocument(string cUrl, string cData, CookieContainer cc, string cReferer, string en)
        {
            return postDocument(cUrl, cData, cc, cReferer, en, false);
        }
        public static string postDocument(string cUrl, string cData, CookieContainer cc, string cReferer, string en, bool ReturnResponseUri)
        {
            return Connect.postDocument(cUrl, cData, cc, cReferer, en, ReturnResponseUri, true);
        }
        public static string postDocument(string cUrl, string cData, CookieContainer cc, string cReferer, string en, bool ReturnResponseUri, bool UnGzip)
        {
            System.Diagnostics.Stopwatch sh = new System.Diagnostics.Stopwatch();
            sh.Start();
            if (cUrl == null) return null;
            cUrl = cUrl.Trim();
            if (cUrl.ToLower().StartsWith("http:///")) return null;
            if (cUrl.ToLower().StartsWith("https://"))
            {
                //if (cUrl.ToLower().StartsWith("http:///")) return null;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            Uri u = null;
            try
            {
                u = new Uri(cUrl);
            }
            catch (Exception ex)
            {
                return null;
            }
            try
            {
                string connName = Encryption.MD5(u.Host);

                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(u);
                rq.ConnectionGroupName = connName;
                ServicePoint currentServicePoint = rq.ServicePoint;
                System.Console.WriteLine("正在连接:" + currentServicePoint.CurrentConnections + "个");

                rq.Method = "POST";
                rq.Timeout = Connect.Timeout;
                if (cc != null) rq.CookieContainer = cc;
                if (cReferer != null) rq.Referer = cReferer;
                rq.AllowAutoRedirect = true;
                rq.KeepAlive = true;

                rq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                rq.UserAgent = Connect.UserAgent;
                SetRequest(ref rq);
                rq.Headers["Accept-Encoding"] = "gzip";
                rq.Headers["Accept-Language"] = "zh-cn,zh;q=0.5";
                rq.Headers["Accept-Charset"] = "GB2312,utf-8;q=0.7,*;q=0.7";
                rq.Headers["Cache-control"] = "no-cache";

                rq.ContentType = "application/x-www-form-urlencoded";

                byte[] SomeBytes = null;
                if (cData != null)
                {
                    SomeBytes = Encoding.ASCII.GetBytes(cData);
                    rq.ContentLength = SomeBytes.Length;
                    Stream newStream = rq.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    rq.ContentLength = 0;
                }
                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                Stream rs = rp.GetResponseStream();
                string cTransferEncoding = rp.Headers["Transfer-Encoding"];
                string cContent = null;
                string cAcceptEncoding = rp.ContentEncoding;
                if (en == null) en = rp.CharacterSet;

                if (rp.ResponseUri.ToString() != null) loctionUrl = rp.ResponseUri.ToString();  
                if (cAcceptEncoding.ToLower().IndexOf("gzip") >= 0 && UnGzip)
                {

                    List<byte> _lData = new List<byte>();
                    while (rs.CanRead)
                    {
                        int v = rs.ReadByte();
                        if (v == -1) break;
                        _lData.Add((byte)v);
                    }
                    byte[] arrData = new byte[_lData.Count];
                    for (int i = 0; i < arrData.Length; i++)
                    {
                        arrData[i] = _lData[i];
                    }
                    MemoryStream ms = new MemoryStream(arrData);


                    #region 解密GZIP
                    GZipStream compressedStream = new GZipStream(ms, CompressionMode.Decompress, true);
                    if (cTransferEncoding != null)
                    {

                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[10240000];
                            //compressedStream.Read(bData, 0, bData.Length);

                            List<byte> lData = new List<byte>();

                            while (compressedStream.CanRead)
                            {
                                int v = compressedStream.ReadByte();
                                //System.Console.WriteLine(v);
                                if (v == -1)
                                    break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(compressedStream, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }
                    compressedStream.Close();
                    #endregion
                }
                else
                {
                    if (cTransferEncoding != null && UnGzip)
                    {
                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[2048000];
                            List<byte> lData = new List<byte>(bData.Length);
                            while (rs.CanRead)
                            {
                                int v = rs.ReadByte();
                                if (v == -1) break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(rs, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }

                }

                #region 处理ReturnResponseUri
                if (ReturnResponseUri)
                {
                    cContent = rp.ResponseUri + "\n" + cContent;
                }
                #endregion

                rs.Close();
                rp.Close();
                rs = null;
                rp = null;
                rq = null;
               
                return cContent;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("POST 数据错误 " + cUrl + ":\n" + ex.ToString());
                return null;
            }

        }

        public static string postDocument_hg(string cUrl, string cData, CookieContainer cc, string cReferer, string en, bool ReturnResponseUri, bool UnGzip)
        {
            if (cUrl == null) return null;
            cUrl = cUrl.Trim();
            if (cUrl.ToLower().StartsWith("http:///")) return null;
            if (cUrl.ToLower().StartsWith("https://"))
            {
                //if (cUrl.ToLower().StartsWith("http:///")) return null;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            Uri u = null;
            try
            {
                u = new Uri(cUrl);
            }
            catch (Exception ex)
            {
                return null;
            }
            try
            {
                string connName = Encryption.MD5(u.Host);

                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(u);
                rq.ConnectionGroupName = connName;
                ServicePoint currentServicePoint = rq.ServicePoint;
                System.Console.WriteLine("正在连接:" + currentServicePoint.CurrentConnections + "个");

                rq.Method = "POST";
                rq.Timeout = Connect.Timeout;
                if (cc != null) rq.CookieContainer = cc;
                if (cReferer != null) rq.Referer = cReferer;
                rq.AllowAutoRedirect = true;
                rq.KeepAlive = true;

                rq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                rq.UserAgent = Connect.UserAgent;
                SetRequest(ref rq);
                rq.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
                rq.Headers["Accept-Language"] = "zh-cn,zh;q=0.5";
                //rq.Headers["Accept-Charset"] = "GB2312,utf-8;q=0.7,*;q=0.7";
                rq.Headers["Cache-control"] = "no-cache";

                rq.ContentType = "application/x-www-form-urlencoded";

                byte[] SomeBytes = null;
                if (cData != null)
                {
                    SomeBytes = Encoding.ASCII.GetBytes(cData);
                    rq.ContentLength = SomeBytes.Length;
                    Stream newStream = rq.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    rq.ContentLength = 0;
                }
                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                Stream rs = rp.GetResponseStream();
                string cTransferEncoding = rp.Headers["Transfer-Encoding"];
                string cContent = null;
                string cAcceptEncoding = rp.ContentEncoding;
                if (en == null) en = rp.CharacterSet;
                if (rp.ResponseUri.ToString() != null) loctionUrl = rp.ResponseUri.ToString();  
                if (cAcceptEncoding.ToLower().IndexOf("gzip") >= 0)
                {

                    List<byte> _lData = new List<byte>();
                    while (rs.CanRead)
                    {
                        int v = rs.ReadByte();
                        if (v == -1) break;
                        _lData.Add((byte)v);
                    }
                    byte[] arrData = new byte[_lData.Count];
                    for (int i = 0; i < arrData.Length; i++)
                    {
                        arrData[i] = _lData[i];
                    }
                    MemoryStream ms = new MemoryStream(arrData);


                    #region 解密GZIP
                    GZipStream compressedStream = new GZipStream(ms, CompressionMode.Decompress, true);
                    if (cTransferEncoding != null)
                    {

                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[10240000];
                            //compressedStream.Read(bData, 0, bData.Length);

                            List<byte> lData = new List<byte>();

                            while (compressedStream.CanRead)
                            {
                                int v = compressedStream.ReadByte();
                                //System.Console.WriteLine(v);
                                if (v == -1)
                                    break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(compressedStream, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }
                    compressedStream.Close();
                    #endregion
                }
                else
                {
                    if (cTransferEncoding != null && UnGzip)
                    {
                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[2048000];
                            List<byte> lData = new List<byte>(bData.Length);
                            while (rs.CanRead)
                            {
                                int v = rs.ReadByte();
                                if (v == -1) break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(rs, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }

                }

                #region 处理ReturnResponseUri
                if (ReturnResponseUri)
                {
                    cContent = rp.ResponseUri + "\n" + cContent;
                }
                #endregion

                rs.Close();
                rp.Close();
                rs = null;
                rp = null;
                rq = null;
                return cContent;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("POST 数据错误 " + cUrl + ":\n" + ex.ToString());
                return null;
            }

        }


        #endregion

        #region Get请求
        public static string getDocument(string cUrl)
        {
            return getDocument(cUrl, null, null, null, false);
        }
        public static string getDocument(string cUrl, CookieContainer cc)
        {
            return getDocument(cUrl, cc, null, null, false);
        }
        public static string getDocument(string cUrl, CookieContainer cc, string cReferer)
        {
            return getDocument(cUrl, cc, cReferer, null, false);
        }
        public static string getDocument(string cUrl, CookieContainer cc, string cReferer, string en)
        {
            return getDocument(cUrl, cc, cReferer, en, false);
        }
        public static string getDocument(string cUrl, CookieContainer cc, string cReferer, string en, bool ReturnResponseUri)
        {
            return getDocument(cUrl, cc, cReferer, en, ReturnResponseUri, false);

        }
        public static string getDocument(string cUrl, CookieContainer cc, string cReferer, string en, bool ReturnResponseUri, bool bKeepAlive)
        {
            return getDocument(cUrl, cc, cReferer, en, ReturnResponseUri, bKeepAlive, Connect.Timeout);
        }
        public static string getDocument(string cUrl, CookieContainer cc, string cReferer, string en, bool ReturnResponseUri, bool bKeepAlive, int iTimeout)
        {
            //System.Diagnostics.Stopwatch sh = new System.Diagnostics.Stopwatch();
            //sh.Start();
            if (cUrl == null) return null;
            if (cUrl.ToLower().StartsWith("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            Uri u = null;
            try
            {
                u = new Uri(cUrl);
            }
            catch (Exception ex)
            {
                return null;
            }
            try
            {
                string connName = Encryption.MD5(u.Host);

                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(u);
                rq.ConnectionGroupName = connName;
                ServicePoint currentServicePoint = rq.ServicePoint;
                //System.Console.WriteLine("正在连接:" + currentServicePoint.CurrentConnections + "个");
                rq.Method = "GET";
                if (iTimeout > 0) rq.Timeout = iTimeout;
                rq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                rq.UserAgent = Connect.UserAgent;
                SetRequest(ref rq);
                rq.Headers["Accept-Encoding"] = "gzip, deflate";
                rq.Headers["Accept-Language"] = "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2";
                //rq.Headers["Accept-Charset"] = "GB2312,utf-8;q=0.7,*;q=0.7";
                rq.Headers["Cache-control"] = "no-cache";
                rq.AllowAutoRedirect = true;
                rq.KeepAlive = bKeepAlive;

                if (cc != null) rq.CookieContainer = cc;
                if (cReferer != null) rq.Referer = cReferer;

                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                Stream rs = rp.GetResponseStream();
                string cTransferEncoding = rp.Headers["Transfer-Encoding"];
                for (int i = 0; i < rp.Cookies.Count; i++)
                {
                    Cookie ck = rp.Cookies[i];
                    //System.Console.WriteLine(ck.Name + "=" + ck.Value);
                }
                string cContent = null;
                string cAcceptEncoding = rp.ContentEncoding;
                if (en == null) en = rp.CharacterSet;
                if (en.Length == 0) en = "utf-8";
                if (cAcceptEncoding.ToLower().IndexOf("gzip") >= 0)
                {
                    #region 解密GZIP
                    GZipStream compressedStream = new GZipStream(rs, CompressionMode.Decompress, true);
                    if (cTransferEncoding != null)
                    {
                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[2048000];
                            List<byte> lData = new List<byte>(bData.Length);
                            while (compressedStream.CanRead)
                            {
                                int v = compressedStream.ReadByte();
                                if (v == -1) break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(compressedStream, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }
                    compressedStream.Close();
                    #endregion
                }
                else
                {
                    if (cTransferEncoding != null)
                    {
                        #region 解密Chunded
                        if (cTransferEncoding.ToLower().Equals("chunked"))
                        {
                            byte[] bData = new byte[2048000];
                            List<byte> lData = new List<byte>(bData.Length);
                            while (rs.CanRead)
                            {
                                int v = rs.ReadByte();
                                if (v == -1) break;
                                lData.Add((byte)v);
                            }
                            byte[] b = new byte[lData.Count];
                            for (int i = 0; i < b.Length; i++)
                            {
                                b[i] = lData[i];
                            }
                            string str = System.Text.Encoding.GetEncoding(en).GetString(b);
                            cContent = str;
                        }
                        else
                        {
                            System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 不解密Chunded，直接读取
                        StreamReader sr = new StreamReader(rs, Encoding.GetEncoding(en));
                        cContent = sr.ReadToEnd();
                        sr.Close();
                        #endregion
                    }

                }

                #region 处理ReturnResponseUri
                if (ReturnResponseUri)
                {
                    cContent = rp.ResponseUri + "\n" + cContent;
                }
                #endregion

                rs.Close();
                rp.Close();

                rs = null;
                rp = null;
                rq = null;
                //sh.Stop();
                //TimeSpan ti = sh.Elapsed;
                //Console.WriteLine(ti.TotalSeconds + cUrl);
                return cContent;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("GET 数据错误 " + cUrl + ":\n" + ex.ToString());
                return null;
            }
            finally
            {

            }

        }

        #endregion

        #region GetHttpWebResponse
        public static HttpWebResponse getHttpWebResponse(string cUrl)
        {
            return Connect.getHttpWebResponse(cUrl, null, null, false);
        }
        public static HttpWebResponse getHttpWebResponse(string cUrl, CookieContainer cc, string cReferer, bool bAutoRedirect)
        {
            if (cUrl == null) return null;
            if (cUrl.ToLower().StartsWith("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            Uri u = null;
            try
            {
                u = new Uri(cUrl);
            }
            catch (Exception ex)
            {
                return null;
            }
            try
            {
                string connName = Encryption.MD5(u.Host);

                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(u);
                rq.ConnectionGroupName = connName;
                ServicePoint currentServicePoint = rq.ServicePoint;
                System.Console.WriteLine("正在连接:" + currentServicePoint.CurrentConnections + "个");
                rq.Method = "GET";
                rq.Timeout = Connect.Timeout;
                rq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                rq.UserAgent = Connect.UserAgent;
                rq.Headers["Accept-Encoding"] = "gzip";
                rq.Headers["Accept-Language"] = "zh-cn,zh;q=0.5";
                rq.Headers["Accept-Charset"] = "GB2312,utf-8;q=0.7,*;q=0.7";
                rq.Headers["Cache-control"] = "no-cache";
                rq.AllowAutoRedirect = bAutoRedirect;
                rq.KeepAlive = true;

                if (cc != null) rq.CookieContainer = cc;
                if (cReferer != null) rq.Referer = cReferer;
                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                return rp;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("GET 数据错误 " + cUrl + ":\n" + ex.ToString());

            }
            finally
            {

            }
            return null;
        }
        #endregion

        #region 获取二进制文件
        public static MemoryStream GetBinFile(string cUrl, CookieContainer cc, string cReferer)
        {
            return GetBinFile(cUrl, cc, cReferer, 0);
        }
        public static MemoryStream GetBinFile(string cUrl, CookieContainer cc, string cReferer, int iTimeout)
        {
            if (cUrl == null) return null;
            if (cUrl.ToLower().StartsWith("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(cUrl);
            ServicePoint currentServicePoint = rq.ServicePoint;
            currentServicePoint.ConnectionLimit = 1000;
            if (cc != null) rq.CookieContainer = cc;
            rq.UserAgent = UserAgent;
            if (cReferer != null) rq.Referer = cReferer;
            if (iTimeout > 0) rq.Timeout = iTimeout;

            rq.Method = "GET";
            rq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            rq.UserAgent = Connect.UserAgent;//"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.0.7) Gecko/2009021910 Firefox/3.0.7";
            rq.Headers["Accept-Encoding"] = "gzip";
            rq.Headers["Accept-Language"] = "zh-cn,zh;q=0.5";
            rq.Headers["Accept-Charset"] = "gb2312,utf-8;q=0.7,*;q=0.7";
            rq.Headers["Cache-control"] = "no-cache";
            HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
            Stream stream = rp.GetResponseStream();

            MemoryStream memoryStream = new MemoryStream();
            string cTransferEncoding = rp.Headers["Transfer-Encoding"];
            if (rp.ContentEncoding != null && rp.ContentEncoding.ToLower().IndexOf("gzip") >= 0)
            {
                #region 解密GZIP
                GZipStream compressedStream = new GZipStream(stream, CompressionMode.Decompress, true);
                if (cTransferEncoding != null)
                {
                    #region 解密Chunded
                    if (cTransferEncoding.ToLower().Equals("chunked"))
                    {
                        byte[] bData = new byte[2048000];
                        List<byte> lData = new List<byte>(bData.Length);
                        while (compressedStream.CanRead)
                        {
                            int v = compressedStream.ReadByte();
                            if (v == -1) break;
                            lData.Add((byte)v);
                        }
                        byte[] b = new byte[lData.Count];
                        for (int i = 0; i < b.Length; i++)
                        {
                            b[i] = lData[i];
                        }

                        memoryStream.Write(b, 0, b.Length);

                    }
                    else
                    {
                        System.Console.WriteLine("TransferE-ncoding: " + cTransferEncoding);
                    }
                    #endregion
                }
                else
                {
                    #region 不解密Chunded，直接读取
                    const int bufferLength = 1024;

                    int actual;
                    byte[] buffer = new byte[bufferLength];
                    while ((actual = compressedStream.Read(buffer, 0, bufferLength)) > 0)
                    {
                        memoryStream.Write(buffer, 0, actual);
                    }
                    #endregion
                }
                compressedStream.Close();
                #endregion
            }
            else
            {

                const int bufferLength = 1024;

                int actual;
                byte[] buffer = new byte[bufferLength];
                while ((actual = stream.Read(buffer, 0, bufferLength)) > 0)
                {
                    memoryStream.Write(buffer, 0, actual);
                }
            }
            rp.Close();
            rp = null;
            stream.Close();
            stream = null;
            rq = null;
            return memoryStream;

        }
        #endregion

        #region PostHttpWebResponse
        public static HttpWebResponse postHttpWebResponse(string cUrl, string cData, CookieContainer cc, string cReferer, bool bAutoRedirect)
        {
            return postHttpWebResponse(cUrl, cData, cc, cReferer, bAutoRedirect, false);
        }
        public static HttpWebResponse postHttpWebResponse(string cUrl, string cData, CookieContainer cc, string cReferer, bool bAutoRedirect, bool bKeepAlive)
        {
            if (cUrl == null) return null;
            if (cUrl.ToLower().StartsWith("http:///")) return null;
            if (cUrl.ToLower().StartsWith("https://"))
            {
                if (cUrl.ToLower().StartsWith("http:///")) return null;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            Uri u = null;
            try
            {
                u = new Uri(cUrl);
            }
            catch (Exception ex)
            {
                return null;
            }
            try
            {
                string connName = Encryption.MD5(u.Host);

                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(u);
                rq.ConnectionGroupName = connName;
                ServicePoint currentServicePoint = rq.ServicePoint;
                System.Console.WriteLine("正在连接:" + currentServicePoint.CurrentConnections + "个");
                rq.Method = "POST";
                if (cc != null) rq.CookieContainer = cc;
                if (cReferer != null) rq.Referer = cReferer;
                rq.AllowAutoRedirect = bAutoRedirect;
                rq.KeepAlive = bKeepAlive;
                rq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                rq.UserAgent = Connect.UserAgent;
                rq.Headers["Accept-Encoding"] = "gzip,deflate";
                rq.Headers["Accept-Language"] = "zh-cn,zh;q=0.5";
                rq.Headers["Accept-Charset"] = "GB2312,utf-8;q=0.7,*;q=0.7";
                rq.Headers["Cache-control"] = "no-cache";

                rq.ContentType = "application/x-www-form-urlencoded";

                byte[] SomeBytes = null;
                if (cData != null)
                {
                    SomeBytes = Encoding.ASCII.GetBytes(cData);
                    rq.ContentLength = SomeBytes.Length;
                    Stream newStream = rq.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    rq.ContentLength = 0;
                }
                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                return rp;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        #endregion

        #region 获取文件
        public static bool getRemoteFile(string cUrl, string cFilePath)
        {
            WebClient c = new WebClient();
            try
            {
                if (File.Exists(cFilePath)) File.Delete(cFilePath);
                c.DownloadFile(cUrl, cFilePath);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace + "\n" + ex.ToString());
            }
            return false;

        }
        #endregion

        #region 获取图片
        public static void getPic(string cUrl, CookieContainer cc, string referer, string cProxy, string cPath)
        {
            Uri u = null;
            try
            {
                u = new Uri(cUrl);
            }
            catch (Exception ex)
            {
                return;
            }
            try
            {
                string connName = Encryption.MD5(u.Host);

                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(u);
                rq.ConnectionGroupName = connName;
                ServicePoint currentServicePoint = rq.ServicePoint;
                System.Console.WriteLine("正在连接:" + currentServicePoint.CurrentConnections + "个");
                if (cProxy != null)
                {
                    WebProxy myProxy = new WebProxy();
                    System.Uri uri = new System.Uri(cProxy);
                    myProxy.Address = uri;
                    rq.Proxy = myProxy;
                }
                if (cc != null) rq.CookieContainer = cc;
                rq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
                if (referer != null) rq.Referer = referer;
                rq.Method = "GET";
                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                Stream rs = rp.GetResponseStream();
                StreamReader sr = new StreamReader(rs);
                byte[] mbyte = new byte[100000];

                int allmybyte = (int)mbyte.Length;
                int startmbyte = 0;

                while (allmybyte > 0)
                {
                    int m = rs.Read(mbyte, startmbyte, allmybyte);
                    if (m == 0)
                        break;

                    startmbyte += m;
                    allmybyte -= m;
                }
                FileStream fstr = new FileStream(cPath, FileMode.OpenOrCreate, FileAccess.Write);
                fstr.Write(mbyte, 0, startmbyte);
                rs.Close();
                fstr.Close();
            }
            catch (WebException ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                System.Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        private static void SetRequest(ref HttpWebRequest rq)
        {
            #region 设置小利记Headers

            if (rq.RequestUri.Host.EndsWith("stsbet.com"))
            {
                rq.Headers["x-requested-with"] = "XMLHttpRequest";
                rq.Headers["x-prototype-version"] = "1.6.0.3.A";
            }
            #endregion
        }

        public static string GetSocket(string cUrl, string cReferer, CookieContainer cc, bool bAutoRedirect, string en, int Timeout)
        {

            AsynchronousClient ac = new AsynchronousClient(cUrl, 3000);
            ac.Connect();
            string c = ac.GetBody();
            return c;
        }
    }
    #endregion

    public class AsynchronousClient
    {

        Socket client = null;
        // ManualResetEvent instances signal completion.
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
      //  private string ResponseEncoding = null;
        // The response from the remote device.
        private String response = String.Empty;

        // Size of receive buffer.
        private int BufferSize = 1024;
        // Receive buffer.
        private byte[] buffer = new byte[1024];
        // Received data string.
        private StringBuilder sb = new StringBuilder();
        string cUrl = "";
        int iTimeout = 20000;
        bool bClose = false;
        private byte[] byteHead = new byte[0];
        private byte[] byteBody = new byte[0];
        private byte[] byteAll = new byte[0];

        public AsynchronousClient(string cUrl, int iTimeout)
        {
            this.cUrl = cUrl;
            if (iTimeout > 0) this.iTimeout = iTimeout;
        }

        #region 连接
        public void Connect()
        {
            if (client != null)
            {
                System.Console.WriteLine("已经连接，不能重新连接");
                return;
            }

            // Connect to a remote device.
            try
            {
                Uri u = new Uri(cUrl);

                #region 设置HTTP头
                StringBuilder headStr = new StringBuilder();
                headStr.Append("GET " + u.PathAndQuery + " HTTP/1.1\r\n");
                headStr.Append("Host:" + u.Host + "\r\n");
                headStr.Append("Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\n");
                headStr.Append("User-Agent:Mozilla/4.0(compatible;MSIE6.0;Windows NT 5.0)\r\n");
                headStr.Append("Accept-Language:zh-cn,zh;q=0.5\r\n");
                headStr.Append("Accept-Encoding:gzip, deflate\r\n");
                headStr.Append("Accept-Charset:GB2312,utf-8;q=0.7,*;q=0.7\r\n");
                headStr.Append("\r\n");
                #endregion

                IPHostEntry gist = Dns.GetHostEntry(u.Host);
                ManualResetEvent TimeoutObject = new ManualResetEvent(false);
                IPAddress ip = gist.AddressList[0];     //得到IP
                IPEndPoint ipEnd = new IPEndPoint(ip, u.Port);//默认80端口号
                this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//使用tcp协议 stream类型 

                // 与目标终端连接. 
                client.BeginConnect(ipEnd, new AsyncCallback(ConnectCallback), client);
                //等待，直到连接程序完成。在ConnectCallback中适当位置有connecDone.Set()语句 
                connectDone.WaitOne(this.iTimeout, false);
                // 发送数据到远程终端.
                Send(this.client, headStr.ToString());
                sendDone.WaitOne(this.iTimeout, false);
                // 接收返回数据. 
                Receive(this.client);
                receiveDone.WaitOne(this.iTimeout, false);
                // Write the response to the console.
                //Console.WriteLine("Response received : {0}", response);

            }
            catch (Exception ex)
            {

            }
            try
            {
                client.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {

            }
            try
            {
                this.client.Close();
                this.bClose = true;
            }
            catch (Exception ex)
            {
                this.bClose = true;

            }
            client = null;
            string headStr1 = null;
            if (byteAll.Length == 0)
            {
                System.Console.WriteLine("连接超时");
                return;
            }
            if (byteAll.Length > 0)
            {

                for (int i = 0; i < byteAll.Length - 4; i++)
                {
                    if ((int)byteAll[i] == 13 && (int)byteAll[i + 1] == 10 && (int)byteAll[i + 2] == 13 && (int)byteAll[i + 3] == 10)
                    {
                        byteHead = new byte[i + 4];
                        byteBody = new byte[byteAll.Length - i - 4];
                        Array.Copy(byteAll, 0, byteHead, 0, i + 4);
                        Array.Copy(byteAll, i + 4, byteBody, 0, byteBody.Length);
                        headStr1 = System.Text.Encoding.UTF8.GetString(byteHead);
                        break;
                    }
                }
            }
            string str1 = System.Text.Encoding.UTF8.GetString(byteBody);
            //int iStart = 0;
            byte[] tagData = new byte[0];
            byte[] unReadData = byteBody;

            if (headStr1.ToLower().IndexOf("chunked") > 0)
            {
                int i = 0;
                while (unReadData.Length > 0)
                {

                    if ((int)unReadData[i] == 13 && (int)byteBody[i + 1] == 10)
                    {
                        int iLen = 0;
                        byte[] byteLen = new byte[i];
                        Array.Copy(unReadData, byteLen, i);
                        string cLen = System.Text.Encoding.UTF8.GetString(byteLen);
                        iLen = Convert.ToInt32(cLen, 16);

                        byte[] newTagData = new byte[iLen];
                        Array.Copy(unReadData, i + 2, newTagData, 0, iLen);

                        MemoryStream ms = new MemoryStream(newTagData);

                        #region 解密GZIP
                        GZipStream compressedStream = new GZipStream(ms, CompressionMode.Decompress, true);
                        StreamReader sr = new StreamReader(compressedStream, Encoding.UTF8);
                        string cContent = sr.ReadToEnd();
                        this.response = cContent;
                        sr.Close();
                        ms.Close();
                        #endregion
                        tagData = newTagData;
                        break;

                        //int iUnReadLen = unReadData.Length - (i + 2 + iLen);
                        //byte[] newUnReadData = new byte[iUnReadLen];
                        //Array.Copy(unReadData, i + 2 + iLen, newUnReadData, 0, iUnReadLen);

                        //byte[] byteNewTagData = new byte[tagData.Length + newTagData.Length];
                        //if (tagData.Length > 0) Array.Copy(tagData, byteNewTagData, tagData.Length);
                        //if (newTagData.Length > 0) Array.Copy(newTagData, 0, byteNewTagData, tagData.Length, byteNewTagData.Length);
                        //tagData = byteNewTagData;
                        //i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            else if (headStr1.ToLower().IndexOf("gzip") > 0)
            {
                #region 解密GZIP
                MemoryStream ms = new MemoryStream(byteBody);
                GZipStream compressedStream = new GZipStream(ms, CompressionMode.Decompress, true);
                StreamReader sr = new StreamReader(compressedStream, Encoding.UTF8);
                string cContent = sr.ReadToEnd();
                this.response = cContent;
                sr.Close();
                ms.Close();
                #endregion
            }
            else
            {
                string cContent = System.Text.Encoding.UTF8.GetString(byteBody);
                this.response = cContent;
            }
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString() + "：正在连接到:" + cUrl);
                //从state对象获取socket. 
                Socket client = (Socket)ar.AsyncState;
                if (!bClose && client != null)
                {
                    // 完成连接.  
                    client.EndConnect(ar);
                }
                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                // 连接已完成，主线程继续. 
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion

        #region 发送数据

        public void Send(Socket client, String data)
        {
            // 格式转换.  
            byte[] ms = System.Text.UTF8Encoding.UTF8.GetBytes(data);

            // 开始发送数据到远程设备. 
            client.BeginSend(ms, 0, ms.Length, 0, new AsyncCallback(SendCallback), this.client);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // 从state对象中获取socket  
                Socket client = (Socket)ar.AsyncState;
                if (!bClose && client != null)
                {
                    // 完成数据发送.  
                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                    // 指示数据已经发送完成，主线程继续.                     
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            sendDone.Set();
        }

        #endregion

        #region 接收数据

        public void Receive(Socket client)
        {
            try
            {
                //从远程目标接收数据. 
                System.Console.WriteLine(DateTime.Now.ToString() + "开始接收数据");
                client.BeginReceive(buffer, 0, this.BufferSize, 0, new AsyncCallback(ReceiveCallback), this.client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // 从输入参数异步读取socket对象
                Socket client = (Socket)ar.AsyncState;
                if (!bClose && client != null)
                {
                    //从远程设备读取数据
                    int bytesRead = client.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        //有数据，存储.
                        byte[] tagByte = new byte[byteAll.Length + bytesRead];
                        if (byteAll.Length > 0) Array.Copy(byteAll, 0, tagByte, 0, byteAll.Length);
                        if (buffer.Length > 0) Array.Copy(buffer, 0, tagByte, byteAll.Length, bytesRead);
                        byteAll = tagByte;
                        tagByte = null;
                        // 继续读取.
                        client.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), this.client);
                    }
                    else
                    {
                        //所有数据读取完毕的指示信号. 
                        receiveDone.Set();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion


        internal string GetBody()
        {
            if (this.response == null) return null;
            return this.response.ToString();
        }
    }
}
