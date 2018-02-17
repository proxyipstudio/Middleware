#region 说明
//---------------------------------------名称:封装的Http请求头配置类
//---------------------------------------更新时间:2017/10/31
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
#endregion


namespace HttpToolsLib
{
    /// <summary>
    /// 构造Http请求头
    /// </summary>
    public class HttpInfo
    {
        #region 构造函数
        /// <summary>
        /// 无参构造函数 默认使用火狐浏览器UA
        /// </summary>
        public HttpInfo()
        {
            //实例化UA池
            UAPool uapool = new UAPool();
            //获得火狐UA
            User_Agent = uapool.GetUA("Windows 火狐");
        }
        /// <summary>
        /// 实例化并传入请求地址
        /// </summary>
        /// <param name="Url">待请求的地址</param>
        public HttpInfo(String Url)
        {
            //实例化UA池
            UAPool uapool = new UAPool();
            //获得火狐UA
            User_Agent = uapool.GetUA("Windows 火狐");
            //设置请求地址
            RequestUrl = Url;
            //默认使用utf编码
            this.Encoding = Encoding.GetEncoding("utf-8");
        }
        #endregion

        #region 属性
        /// <summary>
        /// 用于校验请求Url的正则表达式
        /// </summary>
        String _UrlReg = @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";

        public String UrlReg
        {
            get { return _UrlReg; }
            set { _UrlReg = value; }
        }


        /// <summary>
        /// 编码 默认为空 自动获取网页编码
        /// </summary>
        Encoding _Encoding = null;

        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        /// <summary>
        /// Cookie
        /// </summary>
        CookieString _Cookie = new CookieString();

        public CookieString Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }
        /// <summary>
        /// 域名 自动设定 可手动配置
        /// </summary>
        String _Host = String.Empty;

        public String Host
        {
            get { return _Host; }
            set { _Host = value; }
        }


        /// <summary>
        /// UA 默认为空
        /// </summary>
        String _User_Agent = "";

        public String User_Agent
        {
            get { return _User_Agent; }
            set { _User_Agent = value; }
        }

        /// <summary>
        /// 来源页 默认为空
        /// </summary>
        String _Referer = String.Empty;

        public String Referer
        {
            get { return _Referer; }
            set { _Referer = value; }
        }

        /// <summary>
        /// Post数据
        /// </summary>
        String _PostData = string.Empty;

        public String PostData
        {
            get { return _PostData; }
            set { _PostData = value; ContentLength = Encoding.UTF8.GetByteCount(value); }
        }
        /// <summary>
        /// 请求URL
        /// </summary>
        String _RequestUrl = String.Empty;

        public String RequestUrl
        {
            get { return _RequestUrl; }
            set { _RequestUrl = value; }
        }
        /// <summary>
        /// 代理ip 默认为空
        /// </summary>
        String _Ip = String.Empty;

        public String Ip
        {
            get { return _Ip; }
            set { _Ip = value; }
        }
        /// <summary>
        /// 是否允许重定向 默认为false
        /// </summary>
        private bool _AllowAutoRedirect = false;

        public bool AllowAutoRedirect
        {
            get { return _AllowAutoRedirect; }
            set { _AllowAutoRedirect = value; }
        }
        /// <summary>
        /// 100Continue行为 默认为false
        /// </summary>
        private bool _Expect100Continue = false;

        public bool Expect100Continue
        {
            get { return _Expect100Continue; }
            set { _Expect100Continue = value; }
        }
        /// <summary>
        /// 默认传输类型  默认值 application/x-www-form-urlencoded; charset=UTF-8
        /// </summary>
        private String _ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

        public String ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        /// <summary>
        /// 接受类型  默认接受所有
        /// </summary>
        private String _Accept = "*/*";

        public String Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }
        /// <summary>
        /// 传输长度 自动设定 
        /// </summary>
        private long _ContentLength = 0;

        public long ContentLength
        {
            get { return _ContentLength; }
            set { _ContentLength = value; }
        }
        /// <summary>
        /// 超时值（毫秒） 默认为30秒
        /// </summary>
        private int _Timeout = 30000;

        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        /// <summary>
        /// 写入超时时间 默认为10秒
        /// </summary>
        private int _WriteTimeout = 30000;

        public int WriteTimeout
        {
            get { return _WriteTimeout; }
            set { _WriteTimeout = value; }
        }

        /// <summary>
        /// 可接受的压缩方式 默认为gzip,deflate
        /// </summary>
        private String _AcceptEncoding = "gzip,deflate";

        public String AcceptEncoding
        {
            get { return _AcceptEncoding; }
            set { _AcceptEncoding = value; }
        }
        /// <summary>
        /// 数据缓存 默认为false
        /// </summary>
        private bool _AllowWriteStreamBuffering = false;

        public bool AllowWriteStreamBuffering
        {
            get { return _AllowWriteStreamBuffering; }
            set { _AllowWriteStreamBuffering = value; }
        }
        /// <summary>
        /// 最大连接数 默认为 int.MaxValue
        /// </summary>
        private int _ConnectionLimit = short.MaxValue;

        public int ConnectionLimit
        {
            get { return _ConnectionLimit; }
            set { _ConnectionLimit = value; }
        }
        /// <summary>
        /// 是否保持连接 默认为true
        /// </summary>
        private bool _KeepLive = false;

        public bool KeepLive
        {
            get { return _KeepLive; }
            set { _KeepLive = value; }
        }
        /// <summary>
        /// Http版本类型 默认为HttpVersion.Version10
        /// </summary>
        private Version _ProtocolVersion = HttpVersion.Version11;

        public Version ProtocolVersion
        {
            get { return _ProtocolVersion; }
            set { _ProtocolVersion = value; }
        }
        /// <summary>
        /// CookieContainer格式Cookie
        /// </summary>
        CookieContainer _CC = null;

        public CookieContainer CC
        {
            get { return _CC; }
            set { _CC = value; }
        }
        /// <summary>
        /// 自定义请求头
        /// </summary>
        Dictionary<String, String> _Header = new Dictionary<String, string>();

        public Dictionary<String, String> Header
        {
            get { return _Header; }
            set { _Header = value; }
        }
        /// <summary>
        /// 请求时是否使用正则表达式校验请求地址 默认为false
        /// </summary>
        bool _CheckUrl = false;

        public bool CheckUrl
        {
            get { return _CheckUrl; }
            set { _CheckUrl = value; }
        }
        /// <summary>
        /// 请求的最大重定向数 默认值为5
        /// </summary>
        int _MaximumAutomaticRedirections = 50;

        public int MaximumAutomaticRedirections
        {
            get { return _MaximumAutomaticRedirections; }
            set { _MaximumAutomaticRedirections = value; }
        }
        /// <summary>
        /// 代理IP用户名
        /// </summary>
        public String Proxy_UserName
        {
            get { return _Proxy_UserName; }
            set { _Proxy_UserName = value; }
        }
        String _Proxy_UserName = String.Empty;
        /// <summary>
        /// 代理IP密码
        /// </summary>
        public String Proxy_PassWord
        {
            get { return _Proxy_PassWord; }
            set { _Proxy_PassWord = value; }
        }

        String _Proxy_PassWord = String.Empty;
        /// <summary>
        /// 是否使用 Nagle
        /// </summary>
        bool _UseNagleAlgorithm = true;
        public bool UseNagleAlgorithm
        {
            get { return _UseNagleAlgorithm; }
            set { _UseNagleAlgorithm = value; }
        }
        /// <summary>
        /// 异步自定义超时
        /// </summary>
        public bool UseTaskTimeOut
        {
            get { return _UseTaskTimeOut; }
            set { _UseTaskTimeOut = value; }
        }

        bool _UseTaskTimeOut = false;
        #endregion

        #region 构造请求
        /// <summary>
        /// 使用现有参数构造request
        /// </summary>
        /// <returns>配置完毕的HttpWebRequest对象(或null)</returns>
        public HttpWebRequest CreatRequest()
        {
            Regex url_reg = new Regex(UrlReg);
            //Url校验 正则表达式
            if (url_reg.IsMatch(RequestUrl) || !CheckUrl)
            {
                #region Request创建
                WebRequest webRequest = null;
                try
                {
                    //创建一个WebRequest
                    webRequest = WebRequest.Create(RequestUrl);
                }
                catch
                {
                    Console.WriteLine("初始化Request出错:{0}", RequestUrl);
                    return null;
                }
                #endregion
                
                #region 请求头参数配置
                //强制转化为HttpWebRequest
                var Request = webRequest as HttpWebRequest;
                //设置最大重定向数
                Request.MaximumAutomaticRedirections = MaximumAutomaticRedirections;
                //设置是否开启重定向
                Request.AllowAutoRedirect = AllowAutoRedirect;
                //设置头信息
                Request.Accept = Accept;
                //设置传输类型
                Request.ContentType = ContentType;
                //设置100Continue行为
                Request.ServicePoint.Expect100Continue = Expect100Continue;
                //设置响应超时时间
                Request.Timeout = Timeout;
                //设置写入超时时间
                Request.ReadWriteTimeout = WriteTimeout;
                //设置UA
                Request.UserAgent = User_Agent;
                //设置最大连接数
                Request.ServicePoint.ConnectionLimit = ConnectionLimit;
                System.Net.ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
                //设置KeepAlive
                Request.KeepAlive = KeepLive;
                //设置HttpVersion
                Request.ProtocolVersion = ProtocolVersion;
                //设置UseNagleAlgorithm
                Request.ServicePoint.UseNagleAlgorithm = UseNagleAlgorithm;
                //优先使用CC
                if (CC != null)
                {
                    Request.CookieContainer = CC;
                }
                else if (!Cookie.IsEmpty())
                {
                    //优先使用处理过的Cookie
                    if (String.IsNullOrEmpty(Cookie.BaseCookieStr))
                    {
                        Request.Headers[HttpRequestHeader.Cookie] = Cookie.ConventToString();
                    }
                    //使用原生Cookie
                    else
                    {
                        Request.Headers[HttpRequestHeader.Cookie] = Cookie.BaseCookieStr;
                    }
                }
                //设置压缩方式
                Request.Headers.Add(HttpRequestHeader.AcceptEncoding, AcceptEncoding);
                //设置自定义请求头
                foreach (var head in Header.Keys)
                {
                    Request.Headers.Add(head, Header[head]);
                }
                //设置读写缓存
                Request.AllowWriteStreamBuffering = AllowWriteStreamBuffering;
                //设置域名
                if (!String.IsNullOrEmpty(Host.Trim()))
                {
                    Request.Host = Host;
                }
                //设置来源页
                if (!String.IsNullOrEmpty(Referer))
                {
                    Request.Referer = Referer;
                }
                //设置代理ip
                if (!String.IsNullOrEmpty(Ip))
                {
                    String ip = Ip;
                    try
                    {
                        Request.Proxy = new WebProxy(ip.Split(':')[0], Convert.ToInt32(ip.Split(':')[1]));
                        if (!String.IsNullOrEmpty(Proxy_UserName))
                        {
                            Request.Proxy.Credentials = new NetworkCredential(Proxy_UserName, Proxy_PassWord);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("设置代理ip失败:{0}", ex.Message);
                    }
                }
                else
                {
                    Request.Proxy = null;
                }
                #endregion

                #region 请求体配置 Post
                //存在PostData则自动识别为Post提交
                if (!String.IsNullOrEmpty(PostData.Trim()))
                {
                    Request.Method = "POST";
                    Request.ContentLength = ContentLength;
                    using (Stream myRequestStream = Request.GetRequestStream())
                    {
                        StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                        try
                        {
                            myStreamWriter.Write(PostData);
                            myStreamWriter.Close();
                            myStreamWriter.Dispose();
                        }
                        catch { }
                    }
                }
                #endregion

                return Request;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
