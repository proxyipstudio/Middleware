#region 说明
//---------------------------------------名称:UA池
//---------------------------------------版本:1.1.0.0
//---------------------------------------DLL支持:ExtractLib.dll
//---------------------------------------更新时间:2017/10/18
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using System;
using System.Collections.Concurrent;
using System.Linq; 
#endregion

namespace HttpToolsLib
{
    /// <summary>
    /// UA池
    /// </summary>
    public class UAPool
    {
        #region 变量
        /// <summary>
        /// UA池字典
        /// </summary>
        static ConcurrentDictionary<String, String> _UADic = new ConcurrentDictionary<string, string>();

        public static ConcurrentDictionary<String, String> UADic
        {
            get { return UAPool._UADic; }
            set { UAPool._UADic = value; }
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数  将预设的UA数据添加到UADic中
        /// </summary>
        public UAPool()
        {

            UADic.TryAdd("Windows 火狐", "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.04");
            UADic.TryAdd("safari 5.1 – MAC", "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50");
            UADic.TryAdd("safari 5.1 – Windows", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50");
            UADic.TryAdd("IE 9.0", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0;");
            UADic.TryAdd("IE 8.0", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)");
            UADic.TryAdd("Firefox 4.0.1 – MAC", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.1");
            UADic.TryAdd("Firefox 4.0.1 – Windows", "Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1");
            UADic.TryAdd("Opera 11.11 – MAC", "Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; en) Presto/2.8.131 Version/11.11");
            UADic.TryAdd("Opera 11.11 – Windows", "Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11");
            UADic.TryAdd("Chrome 17.0 – MAC", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11");
            //UADic.TryAdd("Android N1", "Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1");
            //UADic.TryAdd("Android QQ浏览器 For android", "MQQBrowser/26 Mozilla/5.0 (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1");
            //UADic.TryAdd("Android Opera Mobile", "Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10");
            //UADic.TryAdd("Android Pad Moto Xoom", "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13");
            //UADic.TryAdd("Windows Phone Mango", "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0; HTC; Titan)");
            //UADic.TryAdd("Android 微信", "Mozilla/5.0 (Linux; Android 5.0; SM-N9100 Build/LRX21V) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/37.0.0.0 Mobile Safari/537.36 MicroMessenger/6.0.2.56_r958800.520 NetType/WIFI");
            //UADic.TryAdd("IOS 微信", "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257 MicroMessenger/6.0.1 NetType/WIFI");
        } 
        #endregion

        #region 方法
        /// <summary>
        /// 获得随机UA
        /// </summary>
        /// <returns></returns>
        public String GetRandomUA()
        {
            Random r = new Random();
            int NextLength = UADic.Count - 1;
            int index = r.Next(0, NextLength);
            return UADic.ElementAt(index).Value;
        }

        /// <summary>
        /// 获得指定Key的UA
        /// </summary>
        public String GetUA(String Key)
        {
            if (UADic.ContainsKey(Key) && !String.IsNullOrEmpty(Key))
            {
                return UADic[Key];
            }
            return UADic["Windows 火狐"];
        } 
        #endregion
    }
}
