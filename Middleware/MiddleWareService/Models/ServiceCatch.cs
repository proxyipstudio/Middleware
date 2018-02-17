using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PublicLib;

namespace MiddleWareService.Models
{
    public class ServiceCatch
    {
        #region 存活校验相关
        /// <summary>
        /// 获取开始索引
        /// </summary>
        static int _AliveStartIndex = 0;

        public static  int AliveStartIndex
        {
            get { return _AliveStartIndex; }
            set { _AliveStartIndex = value; }
        }
        #endregion



        static ConcurrentBag<ProxyIpChild> _CatchProxy = new ConcurrentBag<ProxyIpChild>();

        public static ConcurrentBag<ProxyIpChild> CatchProxy
        {
            get { return ServiceCatch._CatchProxy; }
            set { ServiceCatch._CatchProxy = value; }
        }

        static ConcurrentDictionary<int, int> _UserDIc =new  ConcurrentDictionary<int, int>();

        public static ConcurrentDictionary<int, int> UserDIc
        {
            get { return ServiceCatch._UserDIc; }
            set { ServiceCatch._UserDIc = value; }
        }
    }
}