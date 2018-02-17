#region 说明
//---------------------------------------名称:代理IP帮助类
//---------------------------------------版本:1.1.0.0
//---------------------------------------更新时间:2017/10/19
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; 
#endregion

namespace HttpToolsLib
{
    /// <summary>
    /// 代理Ip帮助类 提高代码复用率
    /// </summary>
    public class ProxyHelper
    {
        #region 私有变量
        /// <summary>
        /// 有效Ip栈
        /// </summary>
        private static ConcurrentStack<String> IPCheckStack = new ConcurrentStack<string>();
        /// <summary>
        /// IP栈
        /// </summary>
        private static ConcurrentStack<String> IPS = new ConcurrentStack<string>();
        #endregion

        #region 属性
        /// <summary>
        /// 代理Ip地址
        /// </summary>
        static List<String> _API_Url_List = new List<string>();

        public static List<String> API_Url_List
        {
            get { return _API_Url_List; }
            set { _API_Url_List = value; }
        }
        #endregion

        #region 方法

        #region IP获取相关
        /// <summary>
        /// 验Ip线程函数
        /// </summary>
        /// <param name="index">代理ip地址在APIList中的索引</param>
        /// <param name="checknum">验ip线程数</param>
        public static void IPCheckFunc(int index = 1, int checknum = 100)
        {
            String API_Url = API_Url_List[index];
            var ips = HttpMethod.InputApi(API_Url);
            Task[] tarr = new Task[checknum];
            Task.Factory.StartNew(() =>
            {
                while (IPCheckStack.Count < 1000)
                {
                    if (ips.Count == 0)
                    {
                        ips = HttpMethod.InputApi(API_Url);
                    }
                    Thread.Sleep(5000);
                }
            });
            for (int i = 0; i < checknum; i++)
            {
                tarr[i] = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (IPCheckStack.Count < 200)
                        {
                            String ip = String.Empty;
                            if (ips.TryPop(out ip))
                            {
                                if (HttpMethod.CheckIp(ip))
                                {
                                    IPCheckStack.Push(ip);
                                }
                            }
                            else
                            {
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            Thread.Sleep(5000);
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 获取Ip线程函数
        /// </summary>
        /// <param name="index">代理ip地址在APIList中的索引<</param>
        /// <param name="DelayTime">获取间隔</param>
        public static void IPFunc(int index = 1, int DelayTime = 5000)
        {
            String API_Url = API_Url_List[index];
            while (true)
            {
                if (String.IsNullOrEmpty(API_Url))
                {
                    return;
                }
                String ip = String.Empty;
                if (IPS.Count == 0)
                {
                    var ips = HttpMethod.InputApi(API_Url);
                    if (ips == null || ips.Count <= 0)
                    {
                        continue;
                    }
                    foreach (var item in ips)
                    {
                        IPS.Push(item);
                    }
                }
                Thread.Sleep(DelayTime);
            }
        }
        #endregion

        #region IP栈堆操作
        /// <summary>
        /// IP出栈
        /// </summary>
        /// <returns></returns>
        public static string PopIp()
        {
            String ip = String.Empty;
            if (IPCheckStack.TryPop(out ip))
            {
                // Console.WriteLine(ip);
                return ip;
            }
            while (IPS.Count == 0)
            {
                Thread.Sleep(1000);
            }
            if (IPS.TryPop(out ip))
            {

            }
            //     Console.WriteLine(ip);
            return ip;
            //return null;
        }
        /// <summary>
        ///  有效Ip入栈
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool PushIp(String ip)
        {
            if (!String.IsNullOrEmpty(ip))
            {
                IPCheckStack.Push(ip);
                return true;
            }
            return false;
        }
        #endregion

        #endregion
    }
}
