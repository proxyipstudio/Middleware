#region 文件信息

/* ======================================================================== 

* 【本类功能概述】 

*  

* 作者：献丑　　　　　　　　　时间：2018/1/26 21:29:30 

* 文件名：ServiceAPIHelper 

* 版本：V1.0.0 

========================================================================  */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddleWareLib
{
    public class ServiceAPI_Urlr
    {
      /// <summary>
      /// 服务端域名
      /// </summary>
      public const String ServiceHost = "";

      static String _GetCmd = "{0}/GetCmd ";
      /// <summary>
      /// 获得节点命令
      /// </summary>
      public static String GetCmd
      {
          get { return String.Format(ServiceAPI_Urlr._GetCmd, ServiceHost); }
      }

      static String _GetCheckRule= "{0}/GetCmd ";
      /// <summary>
      /// 获得节点校验规则
      /// </summary>
      public static String GetCheckRule
      {
          get { return String.Format(ServiceAPI_Urlr._GetCheckRule, ServiceHost); }
      }

      static String _GetUserIndex= "{0}/GetCmd ";
      /// <summary>
      /// 获得用户索引
      /// </summary>
      public static String GetUserIndex
      {
          get { return String.Format(ServiceAPI_Urlr._GetUserIndex, ServiceHost); }
      }
    }
}
