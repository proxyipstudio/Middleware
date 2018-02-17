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

namespace PublicLib
{
    /// <summary>
    /// 代理IP过滤属性
    /// </summary>
    public class ProxyIpFilter
    {
        int _Delay = 15000;
        String _Province = String.Empty;
        String _City = String.Empty;
        String _Operator = String.Empty;
        int _Anonymity = 0;
        private string _Country = String.Empty;
        String _VPSIp = String.Empty;
        String _VPSNum = String.Empty;
        String _Port = String.Empty;

        /// <summary>
        /// 端口号
        /// </summary>
        public String Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        /// <summary>
        /// 宿主机编号/映射名称
        /// </summary>
        public String VPSNum
        {
            get { return _VPSNum; }
            set { _VPSNum = value; }
        }
        /// <summary>
        /// 宿主机IP地址
        /// </summary>
        public String VPSIp
        {
            get { return _VPSIp; }
            set { _VPSIp = value; }
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
        /// 国家
        /// </summary>
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

    }
    /// <summary>
    ///  匿名度枚举声明
    /// </summary>
    public enum Anonymity { 全部 = 0, 透明 = 1, 匿名 = 2 };
    /// <summary>
    /// 代理IP子类 具有匿名的延迟等详细信息
    /// </summary>
    [Serializable()]
    public class ProxyIpChild : ProxyIpBase
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            ProxyIpChild temp = null;
            temp = (ProxyIpChild)obj;

            return this.Ip_Str.Equals(temp.Ip_Str);

        }

        public override int GetHashCode()
        {
            return this.Ip_Str.GetHashCode();
        }

        #region 构造函数
        /// <summary>
        /// 使用Ip地址和端口号组成的字符串实例化
        /// </summary>
        /// <param name="IpAndPortStr">IP+端口号字符串</param>
        public ProxyIpChild(String IpAndPortStr)
        {
            //输入校验

        }


        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ProxyIpChild()
        { }

        /// <summary>
        /// 使用Ip地址和端口号实例化
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <param name="Port"></param>
        public ProxyIpChild(String IPAddress, int Port)
        {
            //输入校验
        }
        #endregion

        #region 成员
        int _Delay = 0;
        String _Province = String.Empty;
        String _City = String.Empty;
        String _Operator = String.Empty;
        int _Anonymity = 0;
        //List<int> _UserCodeList = new List<int>();
        int _CheckTime = 3;
        String _LastCheckTime = String.Empty;
        private string _Country = String.Empty;
        String _VPSIp = String.Empty;
        String _VPSNum = String.Empty;
        int _ProxyStatue = 0;


        #endregion

        #region 构造器属性
        /// <summary>
        /// 宿主机编号/映射名称
        /// </summary>
        public String VPSNum
        {
            get { return _VPSNum; }
            set { _VPSNum = value; }
        }
        /// <summary>
        /// 宿主机IP地址
        /// </summary>
        public String VPSIp
        {
            get { return _VPSIp; }
            set { _VPSIp = value; }
        }
        public int ProxyStatue
        {
            get { return _ProxyStatue; }
            set { _ProxyStatue = value; }
        }

        /// <summary>
        /// 上一次校验时间
        /// </summary>
        public String LastCheckTime
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
        //public List<int> UserCodeList
        //{
        //    get { return _UserCodeList; }
        //    set { _UserCodeList = value; }
        //}
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
        /// 国家
        /// </summary>
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        #endregion

        #region 常量
        /// <summary>
        /// IP地址正则表达式
        /// </summary>
        const String Ip_Reg = "";
        #endregion

    }

    /// <summary>
    /// 代理IP传输实体类 泛化  插入用
    /// </summary>
    [Serializable()]
    public class ProxyIPTransIn<T>
    {
        String _guid = String.Empty;

        /// <summary>
        /// 请求GUID
        /// </summary>
        public String Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        List<T> _ProxyIPList = new List<T>();
        /// <summary>
        /// 插入代理IP列表
        /// </summary>
        public List<T> DataList
        {
            get { return _ProxyIPList; }
            set { _ProxyIPList = value; }
        }

        public int Count
        {
            get { return _ProxyIPList.Count; }
        }
    }

    /// <summary>
    /// 代理IP传输实体类 泛化 继承json标识实体类 输出用
    /// </summary>
    [Serializable()]
    public class ProxyIPTransOut<T>:JsonBase
    {
        String _guid = String.Empty;
        /// <summary>
        /// 请求GUID
        /// </summary>
        public String Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        List<T> _ProxyIPList = new List<T>();
        /// <summary>
        /// 插入代理IP列表
        /// </summary>
        public List<T> DataList
        {
            get { return _ProxyIPList; }
            set { _ProxyIPList = value; }
        }

        public int Count
        {
            get { return _ProxyIPList.Count; }
        }
    }

    /// <summary>
    /// 代理IP底层基类
    /// </summary>
    [Serializable()]
    public class ProxyIpBase
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            ProxyIpBase temp = null;
            temp = (ProxyIpBase)obj;

            return this.Ip_Str.Equals(temp.Ip_Str);

        }

        public override int GetHashCode()
        {
            return this.Ip_Str.GetHashCode();
        }
        public ProxyIpBase()
        {

        }

        public ProxyIpBase(String ipstr)
        {
            var arr = ipstr.Split(':');
            if(arr.Length ==2)
            {
                this.IpAddress = arr[0];
                this.Port =Convert.ToInt32(arr[1]);
            }
        }

        String _Ip_Str = String.Empty;
        String _IpAddress = String.Empty;
        int _Port = 80;

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public String IpAddress
        {
            get { return _IpAddress; }
            set { _IpAddress = value; }
        }

        /// <summary>
        /// 完整ip+端口号字符串
        /// </summary>
        public String Ip_Str
        {
            get { return String.Format("{0}:{1}", IpAddress, Port); }
            set { _Ip_Str = value; }
        }


        public static ProxyIpBase Converter(string input)
        {
            var arr= input.Split('_');

            return arr.Length == 1 ?new ProxyIpBase(input)  : new ProxyIpBase(arr[1]);
        }
    }
}
