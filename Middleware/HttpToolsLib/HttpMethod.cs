#region 说明
//---------------------------------------名称:封装的Http请求类 静态化
//---------------------------------------版本:1.1.2.0
//---------------------------------------更新时间:2017/11/03
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace HttpToolsLib
{
    /// <summary>
    /// 封装的Http请求类 
    /// </summary>
    public class HttpMethod
    {
        #region URL编解码
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">编码次数</param>
        /// <returns></returns>
        public static String URLEncode(String Str, int time = 1)
        {
            if (String.IsNullOrEmpty(Str))
            {
                return String.Empty;
            }
            else
            {
                Str = Str.Trim();
            }
            for (int i = 0; i < time; i++)
            {
                Str = HttpUtility.UrlEncode(Str, System.Text.Encoding.UTF8);
            }
            return Str;
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">解码次数</param>
        /// <returns></returns>
        public static String URLDecode(String Str, int time = 1)
        {
            if (String.IsNullOrEmpty(Str))
            {
                return String.Empty;
            }
            for (int i = 0; i < time; i++)
            {
                Str = HttpUtility.UrlDecode(Str, System.Text.Encoding.UTF8);
            }
            return Str;

        }
        /// URL编码 泛型重载
        /// </summary>
        /// <typeparam name="T">可以转换为String类型的对象</typeparam>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">编码次数</param>
        /// <returns></returns>
        public static String URLEncode<T>(T Str, int time = 1)
        {
            String str = Convert.ToString(Str);
            return str.Contains("%e") ? str : URLEncode(str, time);
        }
        /// <summary>
        /// URL解码 泛型重载
        /// </summary>
        /// <typeparam name="T">可以转换为String类型的对象</typeparam>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">解码次数</param>
        /// <returns></returns>
        public static String URLDecode<T>(T Str, int time = 1)
        {
            String str = Convert.ToString(Str);
            return URLDecode(str, time);
        }

        /// <summary>
        /// html编码 泛型
        /// </summary>
        /// <typeparam name="T">可以转换为String类型的对象</typeparam>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">编码次数</param>
        /// <returns></returns>
        public static String HtmlDecode<T>(T Str, int time = 1)
        {
            String str = Convert.ToString(Str);
            for (int i = 0; i < time; i++)
            {
                str = HttpUtility.HtmlDecode(str);
            }
            return str;
        }
        /// <summary>
        /// html解码 泛型
        /// </summary>
        /// <typeparam name="T">可以转换为String类型的对象</typeparam>
        /// <param name="Str">待解码字符串</param>
        /// <param name="time">解码次数</param>
        /// <returns></returns>
        public static String HtmlEecode<T>(T Str, int time = 1)
        {
            String str = Convert.ToString(Str);
            for (int i = 0; i < time; i++)
            {
                str = HttpUtility.HtmlEncode(str);
            }
            return str;
        }
        #endregion

        #region 文件/图片下载
        /// <summary>
        /// HttpInfo
        /// </summary>
        /// <param name="DownLoadUrl">下载地址</param>
        /// <param name="SavePath">文件目录</param>
        /// <param name="FileName">保存文件名</param>
        /// <returns></returns>
        public static bool DownLoadFile(HttpInfo info, string FloderPath, String FileName = "")
        {
            Regex filename_reg = new Regex("(&ldquo;)|(\\r)|(\\n)|(&nbsp;)|(\\t)|\\.|\"|/");

            #region 配置文件路径
            //去除非法字符和空格
            FileName = filename_reg.Replace(FileName, String.Empty);
            //文件后缀名 从文件地址后缀中获取
            String filetype = Path.GetExtension(info.RequestUrl);
            //如果文件名中没有后缀名
            if (!String.IsNullOrEmpty(Path.GetExtension(FileName)))
            {
                //拼接后缀名
                FileName = FileName + filetype;
            }
            //获得下载地址中匹配到文件名
            String filename = Path.GetFileName(info.RequestUrl);
            //形成最终保存路径 如果参数中的文件名是空的 则使用自动匹配到的文件路径  反之使用输入的文件名
            String FinalPath = Path.Combine(FloderPath, filename);
            #endregion

            try
            {
                var Myrq = info.CreatRequest();
                if (Myrq != null)
                {
                    using (System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse())
                    {
                        #region 下载
                        long totalBytes = myrp.ContentLength;
                        object[] args = { (int)totalBytes, 0 };

                        System.IO.Stream st = myrp.GetResponseStream();
                        System.IO.Stream so = new System.IO.FileStream(FinalPath, System.IO.FileMode.Create);
                        long totalDownloadedByte = 0;
                        byte[] by = new byte[1024];
                        int osize = st.Read(by, 0, (int)by.Length);
                        while (osize > 0)
                        {
                            int taskosiz = osize;
                            totalDownloadedByte = osize + totalDownloadedByte;
                            so.Write(by, 0, osize);
                            osize = st.Read(by, 0, (int)by.Length);
                        }
                        so.Close();
                        st.Close();
                        #endregion
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 使用HttpInfo下载文件
        /// </summary>
        /// <param name="DownLoadUrl">下载类型</param>
        /// <param name="SavePath">绝对路径</param>
        /// <returns></returns>
        public static bool DownLoadFile_ABPath(HttpInfo info, string SavePath)
        {
            #region 配置文件路径
            String FinalPath = SavePath;
            #endregion

            try
            {
                var Myrq = info.CreatRequest();
                if (Myrq != null)
                {
                    using (System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse())
                    {
                        #region 下载
                        long totalBytes = myrp.ContentLength;
                        object[] args = { (int)totalBytes, 0 };

                        System.IO.Stream st = myrp.GetResponseStream();
                        System.IO.Stream so = new System.IO.FileStream(FinalPath, System.IO.FileMode.Create);
                        long totalDownloadedByte = 0;
                        byte[] by = new byte[1024];
                        int osize = st.Read(by, 0, (int)by.Length);
                        while (osize > 0)
                        {
                            int taskosiz = osize;
                            totalDownloadedByte = osize + totalDownloadedByte;
                            so.Write(by, 0, osize);
                            osize = st.Read(by, 0, (int)by.Length);
                        }
                        so.Close();
                        st.Close();
                        #endregion
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 下载文件到指定路径 原生HttpRequest
        /// </summary>
        /// <param name="DownLoadUrl">文件地址</param>
        /// <param name="SavePath">保存目录</param>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public static bool DownLoadFile(string DownLoadUrl, string SavePath, String FileName = "")
        {
            Regex filename_reg = new Regex("(&ldquo;)|(\\r)|(\\n)|(&nbsp;)|(\\t)|\\.|\"|/");

            #region 配置文件路径
            //去除非法字符和空格
            FileName = filename_reg.Replace(FileName, String.Empty);
            //文件后缀名 从文件地址后缀中获取
            String filetype = Path.GetExtension(DownLoadUrl);
            //如果文件名中没有后缀名
            if (!String.IsNullOrEmpty(Path.GetExtension(FileName)))
            {
                //拼接后缀名
                FileName = FileName + filetype;
            }
            //获得下载地址中匹配到文件名
            String filename = Path.GetFileName(DownLoadUrl);
            //形成最终保存路径 如果参数中的文件名是空的 则使用自动匹配到的文件路径  反之使用输入的文件名
            String FinalPath = String.IsNullOrEmpty(FileName) ? Path.Combine(SavePath, filename) : Path.Combine(SavePath, FileName);
            #endregion

            try
            {
                #region 构造请求头
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(DownLoadUrl);
                Myrq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.04";
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                #endregion

                #region 下载
                long totalBytes = myrp.ContentLength;
                object[] args = { (int)totalBytes, 0 };

                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(FinalPath, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    int taskosiz = osize;
                    totalDownloadedByte = osize + totalDownloadedByte;
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                st.Close();
                #endregion
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 下载图片 返回Image格式 更改携带的cookie 原生request  一般用于登录时的验证码图片
        /// </summary>
        /// <param name="Url">图片地址</param>
        /// <param name="cookies">CookieContainer </param>
        /// <returns>返回的Image格式图片</returns>
        public static Image DownPic(String Url, ref CookieString Cookie)
        {
            Image img = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.04";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;//重定向
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream myResponseStream = response.GetResponseStream();
                    img = Image.FromStream(myResponseStream);
                    //更改cookie
                    Cookie = new CookieString(response.Headers[HttpResponseHeader.SetCookie]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return img;
        }
        /// <summary>
        /// 下载图片 原生request 
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static Image DownPic(String Url)
        {
            Image img = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.75 Safari/537.36";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;//重定向
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream myResponseStream = response.GetResponseStream();
                    var bytes = GetMemoryStream(myResponseStream).ToArray();
                    img = ByteToImage(bytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return img;
        }
        /// <summary>
        /// 下载图片 使用HttpInfo
        /// </summary>
        /// <param name="info">配置完毕的HttpInfo对象</param>
        /// <returns>Image对象</returns>
        public static Image DownPic(HttpInfo info)
        {
            Image img = null;
            try
            {
                var request = info.CreatRequest();
                if (request != null)
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream myResponseStream = response.GetResponseStream();
                        var bytes = GetMemoryStream(myResponseStream).ToArray();
                        img = ByteToImage(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return img;
        }
        #endregion

        #region Http请求
        /// <summary>
        /// 封装的Http参数化请求
        /// </summary>
        /// <param name="Httpinfo">携带请求参数的HttpInfo参数</param>
        /// <returns>返回的源代码</returns>
        public static string HttpWork(HttpInfo Httpinfo)
        {
            //默认的编码为UTF-8
            Encoding encoding = Encoding.UTF8;
            //待返回的网页源代码
            String retString = String.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            MemoryStream _stream = new MemoryStream();
            try
            {
                request = Httpinfo.CreatRequest();
                if (request != null)
                {
                    if (Httpinfo.UseTaskTimeOut && !String.IsNullOrEmpty(Httpinfo.Ip))
                    {

                        var a = request.BeginGetResponse(null, null);
                        var waitResult = a.AsyncWaitHandle.WaitOne(Httpinfo.Timeout + Httpinfo.WriteTimeout);
                        if (waitResult && a.IsCompleted)
                        {
                            response = request.EndGetResponse(a) as HttpWebResponse;
                        }
                        else
                        {
                            response = null;
                        }
                    }
                    else
                    {
                        response = request.GetResponse() as HttpWebResponse;
                    }

                    if (response == null)
                    {
                        retString = String.Empty;
                    }
                    else
                    {
                        //GZIIP处理
                        if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("gzip"))
                        {

                            //开始读取流并设置编码方式
                            //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                            //.net4.0以下写法
                            _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                        }
                        else
                        {
                            //开始读取流并设置编码方式
                            //response.GetResponseStream().CopyTo(_stream, 10240);
                            //.net4.0以下写法
                            _stream = GetMemoryStream(response.GetResponseStream());
                        }
                        //获取Byte
                        byte[] RawResponse = _stream.ToArray();
                        _stream.Close();
                        if (Httpinfo.Encoding != null)
                        {
                            encoding = Httpinfo.Encoding;
                        }
                        else
                        {
                            //从这里开始我们要无视编码了
                            Match meta = Regex.Match(Encoding.Default.GetString(RawResponse), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                            string charter = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower() : string.Empty;
                            charter = charter.Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk");
                            if (charter.Length > 2)
                                encoding = Encoding.GetEncoding(charter.Trim());
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet))
                                {
                                    try
                                    {
                                        encoding = Encoding.GetEncoding(charter.Trim());
                                    }
                                    catch
                                    {
                                        encoding = Encoding.UTF8;
                                    }
                                }
                                else if (response.CharacterSet.ToLower().Contains("iso-8859-1"))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                            }
                        }
                        //得到返回的HTML
                        retString = encoding.GetString(RawResponse);
                    }
                }

            }
            catch (System.Net.WebException e)
            {
                //Console.WriteLine(e.Message);
                return e.Message;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }
            }
            return retString;
        }


        /// <summary>
        /// 封装的Http参数化请求  改变Cookie
        /// </summary>
        /// <param name="Httpinfo"></param>
        /// <returns>返回的源代码</returns>
        public static string HttpWork(ref HttpInfo Httpinfo)
        {
            Encoding encoding = Encoding.UTF8;
            String retString = null;
            HttpWebResponse response = null;
            MemoryStream _stream = new MemoryStream();
            var request = Httpinfo.CreatRequest();
            if (request == null && Httpinfo.CheckUrl)
            {
                retString = "Url校验未通过";
            }
            try
            {
                if (request != null)
                {
                    if (Httpinfo.UseTaskTimeOut && !String.IsNullOrEmpty(Httpinfo.Ip))
                    {

                        var a = request.BeginGetResponse(null, null);
                        var waitResult = a.AsyncWaitHandle.WaitOne(Httpinfo.Timeout + Httpinfo.WriteTimeout);
                        if (waitResult && a.IsCompleted)
                        {
                            response = request.EndGetResponse(a) as HttpWebResponse;
                        }
                        else
                        {
                            response = null;
                        }
                    }
                    else
                    {
                        response = request.GetResponse() as HttpWebResponse;
                    }
                    if (response == null)
                    {
                        retString = String.Empty;
                    }
                    else
                    {
                        //GZIIP处理
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //开始读取流并设置编码方式
                            //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                            //.net4.0以下写法
                            _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                        }
                        else
                        {
                            //开始读取流并设置编码方式
                            //response.GetResponseStream().CopyTo(_stream, 10240);
                            //.net4.0以下写法
                            _stream = GetMemoryStream(response.GetResponseStream());
                        }
                        //获取Byte
                        byte[] RawResponse = _stream.ToArray();
                        _stream.Close();
                        if (Httpinfo.Encoding != null)
                        {
                            encoding = Httpinfo.Encoding;
                        }
                        else
                        {
                            //从这里开始我们要无视编码了
                            Match meta = Regex.Match(Encoding.Default.GetString(RawResponse), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                            string charter = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower() : string.Empty;
                            charter = charter.Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk");
                            if (string.IsNullOrEmpty(response.CharacterSet))
                            {
                                try
                                {
                                    encoding = Encoding.GetEncoding(charter.Trim());
                                }
                                catch
                                {
                                    encoding = Encoding.UTF8;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet))
                                {
                                    try
                                    {
                                        encoding = Encoding.GetEncoding(charter.Trim());
                                    }
                                    catch
                                    {
                                        encoding = Encoding.UTF8;
                                    }
                                }
                                else if (response.CharacterSet.ToLower().Contains("iso-8859-1"))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                            }
                        }
                        //得到返回的HTML
                        retString = encoding.GetString(RawResponse);


                        if (String.IsNullOrEmpty(Httpinfo.Cookie.BaseCookieStr))
                        {
                            Httpinfo.Cookie.AddCookie(response.Headers[HttpResponseHeader.SetCookie]);
                        }
                        else
                        {
                            Httpinfo.Cookie.BaseCookieStr = response.Headers[HttpResponseHeader.SetCookie];
                        }
                    }
                }
            }
            catch (System.Net.WebException e)
            {
                //Console.WriteLine(e.Message);
                return e.Message;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }
            }
            return retString;
        }
        #endregion

        #region 类型转化有关
        /// <summary>
        //图片 转为    base64编码的文本
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static String ImgToBase64String(Image bmp)
        {
            String strbaser64 = String.Empty;
            var btarr = convertByte(bmp);
            strbaser64 = Convert.ToBase64String(btarr);

            return strbaser64;
        }
        /// <summary>
        /// Image转byte[]
        /// </summary>
        /// <param name="img">Img格式数据</param>
        /// <returns>byte[]格式数据</returns>
        public static byte[] convertByte(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            //byte[] bytes = new byte[ms.Length];  
            //ms.Read(bytes, 0, Convert.ToInt32(ms.Length));  
            //以上两句改成下面两句  
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
        /// <summary>
        /// Stream转byte数组
        /// </summary>
        /// <param name="stream">Stream流对象</param>
        /// <returns></returns>
        public static byte[] convertByte(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// byte数组装Image
        /// </summary>
        /// <param name="bytes">待转换byte数组</param>
        /// <returns>Image对象</returns>
        public static Image ByteToImage(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            Image img = Image.FromStream(ms);
            ms.Close();
            return img;
        }
        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            try
            {
                int bytesRead = streamResponse.Read(buffer, 0, Length);
                // write the required bytes  
                while (bytesRead > 0)
                {
                    _stream.Write(buffer, 0, bytesRead);
                    bytesRead = streamResponse.Read(buffer, 0, Length);
                }
            }
            catch
            {
                Console.WriteLine("读取流失败");
            }

            return _stream;
        }
        #endregion

        #region IP相关
        /// <summary>
        /// 获得IP
        /// </summary>
        /// <param name="textbox">textbox控件</param>
        /// <param name="sender">响应事件</param>
        /// <returns>分割后的IP栈堆</returns>
        public static ConcurrentStack<String> InputApi(String ApiUrl)
        {
            //局部变量 分别表示IP总字符串和分割后的IP栈堆
            String IP = null;
            ConcurrentStack<String> API = new ConcurrentStack<string>();
            do
            {
                //获取IP
                IP = FastGetMethod(ApiUrl, null, 15000);
            }
            while (IP == null);
            //用正则表达式分割
            MatchCollection m = Regex.Matches(IP, @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}:\d{1,5}");
            foreach (Match c in m)
            {
                //进队
                API.Push(c.Value);
            }

            IP = null;
            m = null;

            return API;
        }
        /// <summary>
        /// 校验IP
        /// </summary>
        /// <param name="Ip">待校验的Ip地址</param>
        /// <param name="CheckUrl">校检地址</param>
        /// <returns></returns>
        public static bool CheckIp(String Ip, String CheckUrl = "http://2017.ip138.com/ic.asp")
        {
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            HttpResult result = new HttpResult();
            item.URL = CheckUrl;
            item.ProxyIp = Ip;
            item.Timeout = 5000;
            item.ReadWriteTimeout = 8000;
            //item.UserAgent = UAPool.GetRandomUA();
            item.UserAgent = "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.04";


            String html = helper.GetHtml(item).Html;

            if (string.IsNullOrEmpty(Ip))
            {
                return !String.IsNullOrEmpty(html);
            }
            string Ips = Ip.Substring(0, Ip.IndexOf(":"));
            return html.Contains(Ips);
        }

        #endregion

        #region 快速请求
        /// <summary>
        /// 快速获取网页源代码 Get请求
        /// </summary>
        /// <param name="Url">目标地址</param>
        /// <param name="ip">代理Ip</param>
        /// <param name="TimeOut">超时时间</param>
        /// <param name="UA">UA</param>
        /// <returns></returns>
        public static String FastGetMethod(String Url, String ip = null, int TimeOut = 15000, String UA = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36")
        {
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            HttpResult result = new HttpResult();
            item.Allowautoredirect = true;
            item.Timeout = TimeOut;
            item.UserAgent = UA;
            item.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            item.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            item.URL = Url;
            if (!String.IsNullOrEmpty(ip))
            {
                item.ProxyIp = ip;
                // item.Timeout = 5000;
                item.ReadWriteTimeout = 10000;
            }
            else
            {

                item.Timeout = 50000;

            }
            // info.Referer = String.Empty;
            result = helper.GetHtml(item);

            helper = null;
            item = null;

            return result.Html;
        }
        /// <summary>
        /// 返回重定向地址
        /// </summary>
        /// <param name="ReditUrl">重定向地址</param>
        /// <param name="Url">目标地址</param>
        /// <param name="ip">代理Ip</param>
        /// <param name="TimeOut">超时时间</param>
        /// <param name="UA">UA</param>
        /// <returns>网页源代码</returns>
        public static String FastGetMethod(out String ReditUrl, String Url, String ip = null, int TimeOut = 5000, String UA = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36")
        {
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            HttpResult result = new HttpResult();
            item.Timeout = TimeOut;
            if (String.IsNullOrEmpty(UA))
            {
                item.UserAgent = new UAPool().GetRandomUA();
            }
            else
            {
                item.UserAgent = UA;
            }
            item.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            item.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            item.URL = Url;
            if (!String.IsNullOrEmpty(ip))
            {
                item.ProxyIp = ip;
                item.Timeout = 8000;
                item.ReadWriteTimeout = 10000;
            }
            else
            {

                //  item.Ip = ip;
            }
            // info.Referer = String.Empty;
            result = helper.GetHtml(item);
            try
            {
                ReditUrl = result.Header["Location"].ToString();
            }
            catch
            {
                ReditUrl = String.Empty;
            }
            return result.Html;
        }
        /// <summary>
        /// 快速Post请求 HttpHelper
        /// </summary>
        /// <param name="Url">目标地址</param>
        /// <param name="Postdata">Post提交数据</param>
        /// <param name="Refrer">来源页</param>
        /// <param name="Cookie">字符串类型Cookie</param>
        /// <param name="ip">代理ip</param>
        /// <returns>网页源代码</returns>
        public static String FastPostMethod(String Url, String Postdata, String Refrer = null, String Cookie = null, String ip = null)
        {
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            HttpResult result = new HttpResult();
            item.Method = "Post";
            item.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
            //item.UserAgent = new UAPool().GetRandomUA();
            item.URL = Url;
            // item.Encoding = Encoding.GetEncoding("utf-8");
            item.Referer = Refrer;
            //item.Accept = "application/json, text/javascript, */*; q=0.01";
            item.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            item.Cookie = Cookie;
            item.Postdata = Postdata;
            if (!String.IsNullOrEmpty(ip))
            {
                item.ProxyIp = ip;
                item.Timeout = 5000;
                item.ReadWriteTimeout = 8000;
            }
            else
            {
                item.Timeout = 10000;
                item.ReadWriteTimeout = 8000;
                //  item.Ip = ip;
            }
            // info.Referer = String.Empty;
            result = helper.GetHtml(item);
            return result.Html;
        }
        /// <summary>
        /// 返回状态码
        /// </summary>
        /// <param name="Url">目标地址</param>
        /// <param name="code">Http状态码</param>
        /// <param name="ip">代理Ip</param>
        /// <param name="allow">是否允许重定向 默认不允许</param>
        /// <returns>网页源代码</returns>
        public static String FastGetMethod(String Url, ref HttpStatusCode code, String ip = null, bool allow = false)
        {
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            HttpResult result = new HttpResult();

            item.URL = Url;
            item.Allowautoredirect = allow;
            item.UserAgent = new UAPool().GetRandomUA();

            if (!String.IsNullOrEmpty(ip))
            {
                item.ProxyIp = ip;
                item.Timeout = 5000;
                item.ReadWriteTimeout = 1000;
            }

            //item.Referer = String.Empty;
            String html = String.Empty;

            result = helper.GetHtml(item);
            html = result.Html;
            code = result.StatusCode;


            return html;
        }
        /// <summary>
        /// 快速请求 基于HttpInfo
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <returns></returns>
        public static String FastGetMethod_HttpInfo(String Url)
        {
            HttpInfo info = new HttpInfo(Url);
            return HttpMethod.HttpWork(info);
        }
        /// <summary>
        /// 获得重定向的Url地址 基于Httpinfo
        /// </summary>
        /// <param name="tempinfo">配置完毕的HttpInfo对象</param>
        /// <returns>网页源代码</returns>
        public static string GetRedirectUrl(HttpInfo tempinfo)
        {
            tempinfo.AllowAutoRedirect = false;
            var req = tempinfo.CreatRequest();
            String reurl = tempinfo.RequestUrl;

            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                var stream = resp.GetResponseStream();
                reurl = resp.Headers["Location"].ToString();
                stream.Close();
            }

            if (String.IsNullOrEmpty(reurl))
            {
                return tempinfo.RequestUrl;
            }
            else
            {
                return reurl;
            }
        }
        #endregion
    }
}
