#region 文件信息

/* ======================================================================== 

* 【本类功能概述】 

*  配置文件操作帮助

* 作者：献丑　　　　　　　　　时间：2018/1/27 15:55:17 

* 文件名：ConfigHelper 

* 版本：V1.0.0 

========================================================================  */

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace PublicLib
{
    /// <summary>
    /// 操作配置文件的方法
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 根据Key获取AppSetting_Value值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string As_GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch { }
            return string.Empty;
        }
        /// <summary>
        /// 根据Key获取AppSetting_Value值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string Cs_GetValue(string key)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch { }
            return string.Empty;
        }


        public static bool As_SetValue<T>(String key, T Value,bool IsWebAppSetting = false)
        {
            bool flag = false;
            try
            {

                Configuration config =IsWebAppSetting? WebConfigurationManager.OpenWebConfiguration("~"):ConfigurationManager.OpenExeConfiguration("~");
                //修改节点值
                config.AppSettings.Settings[key].Value = Convert.ToString(Value);
                config.Save(); //保存配置文件
                ConfigurationManager.RefreshSection("appSettings");
                flag = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }
    }
}
