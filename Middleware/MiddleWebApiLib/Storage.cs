#region 文件信息

/* ======================================================================== 

* 【本类功能概述】 

*  IP资源静态全局存储

* 作者：献丑　　　　　　　　　时间：2018/1/25 21:35:36 

* 文件名：Storage 

* 版本：V1.0.0 

========================================================================  */

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddleWebApiLib
{
   public class Storage
    {
        /// <summary>
        /// 从中间件服务端获取的失效IP队列
        /// </summary>
       public static ConcurrentQueue<ProxyIp> Invalid_ProxyIp_Queue = new ConcurrentQueue<ProxyIp>();

        /// <summary>
        /// 复活代理IP栈
        /// </summary>
       public static ConcurrentStack<ProxyIp> Relive_ProxyIp_Stack = new ConcurrentStack<ProxyIp>();

        /// <summary>
        /// 多次校验无效后  进入睡眠状态(推送给穷举扫描节点)
        /// </summary>
       public static ConcurrentQueue<ProxyIp> Sleep_ProxyIp_Queue = new ConcurrentQueue<ProxyIp>();
    }
}
