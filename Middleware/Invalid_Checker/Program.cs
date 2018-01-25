#region 文件信息

/* ======================================================================== 

* 【本类功能概述】 

*  失效代理IP校验节点  入口主文件

* 作者：献丑　　　　　　　　　时间：2018/1/25 21:02:53 

* 文件名：Program 

* 版本：V1.0.0 

========================================================================  */

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Invalid_Checker
{
    class Program
    {
        #region 全局存储
        /// <summary>
        /// 从中间件服务端获取的失效IP队列
        /// </summary>
        ConcurrentQueue<ProxyIp> Invalid_ProxyIp_Queue = new ConcurrentQueue<ProxyIp>();

        /// <summary>
        /// 复活代理IP栈
        /// </summary>
        ConcurrentStack<ProxyIp> Relive_ProxyIp_Stack = new ConcurrentStack<ProxyIp>();

        /// <summary>
        /// 多次校验无效后  进入睡眠状态(推送给穷举扫描节点)
        /// </summary>
        ConcurrentQueue<ProxyIp> Sleep_ProxyIp_Queue = new ConcurrentQueue<ProxyIp>();
        #endregion

        #region 预设常量

        /// <summary>
        /// 校验线程数量
        /// </summary>
        const int ThreadNum = 1;


        #endregion

        #region 系统开关
        /// <summary>
        /// 应用程序退出开关
        /// </summary>
        static bool Exit = false;

        /// <summary>
        /// 代理IP获取开关
        /// </summary>
        static bool GetProxy = true;
        #endregion

        static void Main(string[] args)
        {
            #region 局部变量声明
            Thread[] threadarr = new Thread[ThreadNum];
            #endregion

            #region 主任务逻辑
            //命令接受线程
            Task.Factory.StartNew(() =>
            {

            });
            //复活代理IP推送线程
            Task.Factory.StartNew(() =>
            {

            });

            //验IP线程
            for (int i = 0; i < ThreadNum; i++)
            {
                threadarr[i] = new Thread(new ThreadStart(delegate { 
                
                }));
                threadarr[i].IsBackground = true;
                threadarr[i].Start();
            }
            //睡眠代理IP推送线程
            Task.Factory.StartNew(() =>
            {

            });
            //获取IP线程 死循环
            while (true)
            {

            }
            #endregion
        }
    }
}
