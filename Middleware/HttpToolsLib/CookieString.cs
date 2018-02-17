#region 说明
//---------------------------------------名称:以字符串形式处理Cookie
//---------------------------------------版本:1.1.0.0
//---------------------------------------更新时间:2017/10/18
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools
#endregion

#region 名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#endregion


namespace HttpToolsLib
{
    /// <summary>
    /// 以字符串形式处理Cookie
    /// </summary>
    public class CookieString
    {
        #region 属性和成员变量
        /// <summary>
        /// 原始字符串型Cookie
        /// </summary>
        public string BaseCookieStr { get; set; }
        /// <summary>
        /// 存放键值对型Cookie
        /// </summary>
        private List<SingleCookie> CookieList = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化CookieString
        /// </summary>
        /// <param name="Cookie">字符串型Cookie</param>
        /// <param name="StrEnable">是否直接使用字符串Cookie</param>
        public CookieString(String Cookie, bool StrEnable = false)
        {
            //添加cookie
            if (StrEnable)
            {
                BaseCookieStr = Cookie;
            }
            else
            {
                CookieList = CreatCookieList(Cookie);
            }
        }
        /// <summary>
        /// 无参构造
        /// </summary>
        public CookieString()
        {
            CookieList = new List<SingleCookie>();
        }
        #endregion

        #region Cookie操作
        /// <summary>
        /// 将cookie加入singlecookie的List中 并剔除path expires domain
        /// </summary>
        /// <param name="Cookie">传入的String 类型Cookie</param>
        /// <returns></returns>
        private List<SingleCookie> CreatCookieList(string Cookie)
        {
            List<SingleCookie> Cookielist = new List<SingleCookie>();
            try
            {
                //去除 cookie字符串中的httponly path expires
                Regex httponlyreg = new Regex("httponly(,|/)*", RegexOptions.IgnoreCase);
                Regex pathreg = new Regex("path=/(,|/)*", RegexOptions.IgnoreCase);
                Regex expiresreg = new Regex("expires(,|/)*", RegexOptions.IgnoreCase);

                Cookie = httponlyreg.Replace(Cookie, String.Empty);
                Cookie = pathreg.Replace(Cookie, String.Empty);
                Cookie = expiresreg.Replace(Cookie, String.Empty);
                Cookie = Cookie.Replace(" ", String.Empty);
                //以;为界分割
                String[] StringArray = Cookie.Split(';');
                String[] SingleArray;
                StringArray.ToList().ForEach(singlecookiestr=> {
                    //以=为界分割
                    SingleArray = singlecookiestr.Split('=');
                    //将cookie名称转化为小写并trim()
                    String cookiename = SingleArray[0].ToLower().Replace(" ", String.Empty).Trim();
                    //单个数组长度
                    int length = SingleArray.Length;
                    if (cookiename != String.Empty && length == 2)
                    {
                        //构造cookie
                        SingleCookie sc = new SingleCookie(SingleArray);
                        //Add
                        Cookielist.Add(sc);
                    }
                });
                //Parallel.ForEach(StringArray, singlecookiestr =>
                //{
      
                //});
            }
            catch
            {

            }
            return Cookielist;
        }
        /// <summary>
        /// 后续添加cookie
        /// </summary>
        public void AddCookie(String cookie)
        {
            List<SingleCookie> AddList = CreatCookieList(cookie);
            bool flag = true;
            int index = -1;
            //循环去重
            foreach (SingleCookie asc in AddList)
            {
                if (asc == null)
                {
                    continue;
                }
                index = -1;
                flag = true;
                foreach (SingleCookie esc in CookieList)
                {
                    index++;
                    try
                    {
                        if (esc.Name.Trim() == asc.Name.Trim())
                        {
                            if (esc.Value != asc.Value)
                            {
                                CookieList[index].Value = asc.Value;
                            }
                            flag = false;
                            break;
                        }
                    }
                    catch
                    {
                        index--;
                    }
                }
                if (flag)
                {
                    CookieList.Add(asc);
                }
            }
        }
        /// <summary>
        /// 转化CookieString对象为字符串
        /// </summary>
        /// <returns></returns>
        public String ConventToString()
        {
            String Cookie = String.Empty;
            try
            {
                foreach (SingleCookie sc in CookieList)
                {
                    if (sc == null)
                    {
                        continue;
                    }
                    Cookie += sc.Name + "=" + sc.Value + ";";
                }
            }
            catch
            {

            }
            return Cookie;
        }
        /// <summary>
        /// 判断是否为空 空返回true 否则返回false
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if ((CookieList == null || CookieList.Count == 0) && String.IsNullOrEmpty(BaseCookieStr))
            {
                return true;
            }
            return false;
        }
        #endregion

    }
    /// <summary>
    /// Cookie 键值对
    /// </summary>
    class SingleCookie
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Cookie">Cookie数组 Key=Value格式</param>
        public SingleCookie(String[] Cookie)
        {
            //防止数组越界
            if (Cookie.Length == 2)
            {
                Name = Cookie[0];
                Value = Cookie[1];
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// cookie key值
        /// </summary>
        private String _Name = String.Empty;

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// cookie value
        /// </summary>
        private String _Value = String.Empty;

        public String Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        #endregion
    }
}
