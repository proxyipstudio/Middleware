using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiddleWareService.Models
{
    /// <summary>
    /// 枚举声明
    /// </summary>
    public class Enums
    {
        public enum StatueCode { 成功 = 0, 失败 = 1 }
        public enum ProxyIPStatueCode { 可用 = 0, 失效 = 1 }
        /// <summary>
        /// 代理IP提取用户
        /// </summary>
        public enum ProxyUsers { 扫描节点 = 0,存活校验节点,失效校验节点,消费者用户};


        public enum 业务类型 { 已包含前缀 = -1, 存档库 = 0, 热库_存活库, 失效库, 使用情况库, 用户索引库 };
        public enum 存活状态 { 失效 = 0, 存活 };
    }
}