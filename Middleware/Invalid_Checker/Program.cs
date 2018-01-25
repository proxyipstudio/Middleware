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
using MiddleWebApiLib;

namespace Invalid_Checker
{
    class Program
    {
        #region 预设常量

        /// <summary>
        /// 校验线程数量
        /// </summary>
        const int ThreadNum = 1;
        /// <summary>
        /// 获取代理IP间隔
        /// </summary>
        const int GetProxy_Delay = 5000;
        /// <summary>
        /// 获取命令间隔
        /// </summary>
        const int GetCmd_Delay = 5000;
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

        /// <summary>
        /// 获取命令开关
        /// </summary>
        static bool GetCmd = true;
        #endregion

        static void Main(string[] args)
        {
            #region 局部变量声明
            //校验线程数组
            Thread[] threadarr = new Thread[ThreadNum];
            #endregion

            #region 主任务逻辑
            //复活代理IP推送线程
            Task.Factory.StartNew(() => { APIHelper.PushRelive(); });
            //睡眠代理IP推送线程
            Task.Factory.StartNew(() => { APIHelper.PushSleep(); });
            //验IP线程
            for (int i = 0; i < ThreadNum; i++)
            {
                threadarr[i] = new Thread(Proxy_CheckFunc);
                threadarr[i].IsBackground = true;
                threadarr[i].Start();
            }

            //命令接受线程
            Task.Factory.StartNew(() =>
            {
                while (GetCmd)
                {
                    //获取命令
                    Command cmd = APIHelper.AcceptCmd();
                    //执行命令
                    RunCmd(cmd);
                    Thread.Sleep(GetCmd_Delay);
                }

            });
            //获取IP线程 死循环
            while (GetProxy)
            {
                APIHelper.PushSleep();
                Thread.Sleep(GetProxy_Delay);
            }
            #endregion
        }

        private static void RunCmd(Command cmd)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 线程函数
        /// </summary>
        private static void Proxy_CheckFunc()
        {
            throw new NotImplementedException();
        }
    }
}
