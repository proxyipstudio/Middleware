#region 文件信息

/* ======================================================================== 

* 【本类功能概述】 

*  代理IP实体类

* 作者：献丑　　　　　　　　　时间：2018/1/25 21:11:05 

* 文件名：ProxyIp 

* 版本：V1.0.0 

========================================================================  */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddleWareLib
{
    /// <summary>
    ///  匿名度枚举声明
    /// </summary>
    public enum Anonymity { 透明 = 0, 普通匿名 = 1, 高度匿名 = 2 };
    public class ProxyIp
    {

        #region 构造函数
        /// <summary>
        /// 使用Ip地址和端口号组成的字符串实例化
        /// </summary>
        /// <param name="IpAndPortStr">IP+端口号字符串</param>
        public ProxyIp(String IpAndPortStr)
        {
            //输入校验

        }


        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ProxyIp()
        { }

        /// <summary>
        /// 使用Ip地址和端口号实例化
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <param name="Port"></param>
        public ProxyIp(String IPAddress, int Port)
        {
            //输入校验
        }
        #endregion

        #region 成员
        String _Ip_Str = String.Empty;
        int _Delay = 0;
        String _Province = String.Empty;
        String _City = String.Empty;
        String _Operator = String.Empty;
        int _Anonymity = 0;
        List<int> _UserCodeList = new List<int>();
        int _CheckTime = 3;
        DateTime _LastCheckTime = DateTime.MinValue;
        #endregion

        #region 构造器属性
        /// <summary>
        /// 上一次校验时间
        /// </summary>
        public DateTime LastCheckTime
        {
            get { return _LastCheckTime; }
            set { _LastCheckTime = value; }
        }
        /// <summary>
        /// 校验次数
        /// </summary>
        public int CheckTime
        {
            get { return _CheckTime; }
            set { _CheckTime = value; }
        }
        /// <summary>
        /// 用户调用情况码表 下标表示用户 值表示调用次数
        /// </summary>
        public List<int> UserCodeList
        {
            get { return _UserCodeList; }
            set { _UserCodeList = value; }
        }
        /// <summary>
        /// 匿名度
        /// </summary>
        public int Anonymity
        {
            get { return _Anonymity; }
            set { _Anonymity = value; }
        }
        /// <summary>
        /// 运营商
        /// </summary>
        public String Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }

        /// <summary>
        /// 城市
        /// </summary>
        public String City
        {
            get { return _City; }
            set { _City = value; }
        }
        /// <summary>
        /// 省份
        /// </summary>
        public String Province
        {
            get { return _Province; }
            set { _Province = value; }
        }
        /// <summary>
        /// 延迟(以毫秒为单位地)
        /// </summary>
        public int Delay
        {
            get { return _Delay; }
            set { _Delay = value; }
        }
        /// <summary>
        /// 完整ip+端口号字符串
        /// </summary>
        public String Ip_Str
        {
            get { return _Ip_Str; }
            set { _Ip_Str = value; }
        }
        #endregion

        #region 常量
        /// <summary>
        /// IP地址正则表达式
        /// </summary>
        const String Ip_Reg = "";
        #endregion

    }
}
