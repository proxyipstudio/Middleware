#region 文件信息

/* ======================================================================== 

* 【本类功能概述】 

*  WebAPI接口地址

* 作者：献丑　　　　　　　　　时间：2018/1/26 21:13:32 

* 文件名：WebAPI_Url 

* 版本：V1.0.0 

========================================================================  */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddleWareLib
{
    public class WebAPI_Url
    {
        /// <summary>
        /// 域名
        /// </summary>
        public const String MiddleWareHost = "";

        static String _GetInvalidIp = "{0}/GetInvalidIp";
        /// <summary>
        /// 获得失效IP 数量由服务端决定
        /// </summary>
        public static String GetInvalidIp
        {
            get { return String.Format(WebAPI_Url._GetInvalidIp,MiddleWareHost); }
        }

        static String _GetUsefulIp = "{0}/GetUsefulIp ";
        /// <summary>
        /// 获得有效IP 数量由服务端决定
        /// </summary>
        public static String GetUsefulIp
        {
            get { return String.Format(WebAPI_Url._GetUsefulIp, MiddleWareHost); }
        }



        static String _WebApiCallBack = "{0}/ApiCallBack?RequestGuid={1}";
        /// <summary>
        /// 回告接口 需用Format传参使用  传入RequestGuid
        /// </summary>
        public static String WebApiCallBack
        {
            get { return String.Format(WebAPI_Url._WebApiCallBack, MiddleWareHost,"{0}"); }
        }
    }
}
