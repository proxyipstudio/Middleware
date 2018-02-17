using MiddleWareService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PublicLib;
using RedisHelperLib;
using Newtonsoft.Json.Linq;
using RedisHelperLib.Redis;
namespace MiddleWareService.Controllers
{
    public class ProxyIPController : Controller
    {
        #region 测试和视图
        //
        // GET: /ProxyIP/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获得测试用代理IP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<String> GetTestProxyIp()
        {
            return Task<String>.Factory.StartNew(() =>
            {
                return "127.0.0.1";
            });
        }
        #endregion

        #region 节点端接口
        /// <summary>
        /// IP插入  有效表 存档表 失效表
        /// </summary>
        /// <param name="code">插入标志失效0存活1</param>
        /// <returns></returns>
        [HttpPost]
        public Task<JsonResult> Insert(int code = 1)
        {
            //获得Postdata
            String data = Request["data"];
            var json = new GuidJson();
            String Guid = String.Empty;
            return Task<JsonResult>.Factory.StartNew(() =>
            {
                //没有数据
                if (String.IsNullOrEmpty(data))
                {
                    json = new GuidJson((int)Enums.StatueCode.失败, ConstString.ParmIsEmpty);
                }
                else
                {
                    switch ((Enums.存活状态)code)
                    {
                        //插入存活代理IP
                        case Enums.存活状态.存活:
                            //反序列化data
                            var insertobj_alive = JsonConvert.DeserializeObject<ProxyIPTransIn<ProxyIpChild>>(data);
                            try
                            {
                                //获得节点带来的Guid
                                Guid = insertobj_alive.Guid;
                                //判断IP条数
                                if (insertobj_alive != null && insertobj_alive.DataList.Count > 0)
                                {
                                    #region Redis操作
                                    //获得存活表数据长度
                                    long length = RedisHelper.GetInstance().ListLength(ConstString.TableName_Alive);
                                    //存活IP入队 去重
                                    RedisHelper.GetInstance().ListRightPushRange(ConstString.TableName_Save, insertobj_alive.DataList, true);
                                    //优先端口号插入集合
                                    RedisHelper.GetInstance().SetAddRange(ConstString.TableName_PortSet,from a in insertobj_alive.DataList select a.Port);
                                    //存档IP入队 去重
                                    if (RedisHelper.GetInstance().ListRightPushRange(ConstString.TableName_Alive, insertobj_alive.DataList, true))
                                    {
                                        json.Code = 0;
                                        json.Guid = Guid;
                                        json.Message = String.Format("{0}条存活记录插入成功", insertobj_alive.Count);
                                    }
                                    else
                                    {
                                        json.Code = 0;
                                        json.Guid = Guid;
                                        //返回成功插入的条数
                                        json.Message = String.Format("{0}条存活记录插入成功,提交的数据有重复", RedisHelper.GetInstance().ListLength(ConstString.TableName_Alive) - length);
                                    }
                                    #endregion

                                }
                                else
                                {
                                    json.Code = 1;
                                    json.Guid = Guid;
                                    json.Message = String.Format("提交数据条数为0");
                                }
                            }
                            //异常捕获
                            catch (Exception ex)
                            {
                                json.Code = 1;
                                json.Guid = Guid;
                                json.Message = String.Format("插入出错 错误信息为:{0}", ex.Message + "\r\n" + ex.StackTrace);
                            }
                            finally
                            {
                                //对象销毁
                                insertobj_alive = null;
                            }
                            break;
                        case Enums.存活状态.失效:
                            //反序列化data
                            var insertobj_dead = JsonConvert.DeserializeObject<ProxyIPTransIn<ProxyIpBase>>(data);
                            try
                            {
                                //条数判断
                                if (insertobj_dead != null && insertobj_dead.DataList.Count > 0)
                                {

                                    #region Redis相关
                                    //获得失效表数据长度
                                    var length = RedisHelper.GetInstance().ListLength(ConstString.TableName_Sleep);
                                    //插入失效数据
                                    if (RedisHelper.GetInstance().ListRightPushRange(ConstString.TableName_Sleep, insertobj_dead.DataList, true))
                                    {
                                        json.Code = 0;
                                        json.Guid = insertobj_dead.Guid;
                                        json.Message = String.Format("{0}条失效记录插入成功", insertobj_dead.DataList.Count);
                                    }
                                    else
                                    {
                                        json.Code = 0;
                                        json.Guid = insertobj_dead.Guid;
                                        //获得成功插入的条数
                                        json.Message = String.Format("{0}条失效记录插入成功,提交的数据有重复", RedisHelper.GetInstance().ListLength(ConstString.TableName_Sleep) - length);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    json.Code = 1;
                                    json.Guid = Guid;
                                    json.Message = String.Format("提交数据条数为0");
                                }
                            }
                            //异常捕获
                            catch (Exception ex)
                            {
                                json.Code = 1;
                                json.Guid = Guid;
                                json.Message = String.Format("插入出错 错误信息为:{0}", ex.Message + "\r\n" + ex.StackTrace);
                            }
                            finally
                            {
                                //对象销毁
                                insertobj_dead = null;
                            }
                            break;
                    }
                }

                //返回json对象
                return Json(json);
            });


        }

        /// <summary>
        /// 获得IP(节点用户)
        /// </summary>
        /// <param name="count">条数</param>
        /// <param name="code">用户 0为失效扫描节点 1为存活校验节点</param>
        /// <returns></returns>
        [HttpGet]
        public Task<JsonResult> GetProxyIP(int count, int code = 0)
        {
            return Task<JsonResult>.Factory.StartNew(() =>
            {
                //返回json对象声明
                JsonResult json = null;
                //校验扫描节点只需要知道最基本的IP信息
                ProxyIPTransOut<ProxyIpBase> proxyget = new ProxyIPTransOut<ProxyIpBase>();
                switch ((Enums.存活状态)code)
                {
                    //失效扫描节点
                    case   Enums.存活状态.失效:
                        //失效表数据长度
                        var length = RedisHelper.GetInstance().ListLength(ConstString.TableName_Sleep);
                        proxyget.DataList = new List<ProxyIpBase>();
                        //如果获取的长度小于失效表总长度  出队直到条数满足
                        if (length > count)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                proxyget.DataList.Add(RedisHelper.GetInstance().ListLeftPop<ProxyIpBase>(ConstString.TableName_Sleep));
                            }
                        }
                        //如果获取的长度大于失效表总长度 
                        else
                        {
                            long expectcount = 0;
                            //获得存档表长度
                            var savelength = RedisHelper.GetInstance().ListLength(ConstString.TableName_Save);
                            //将失效表中的数据全部插入
                            proxyget.DataList.AddRange(RedisHelper.GetInstance().ListRange<ProxyIpBase>(ConstString.TableName_Sleep, true));
                            //删除失效表(有新插入时会自动重建)
                            RedisHelper.GetInstance().KeyDelete(ConstString.TableName_Sleep);
                            //存档表和失效表数据作差集 补全条数
                            var expectlist = RedisHelper.GetInstance().ListRange<ProxyIpBase>(ConstString.TableName_Save, true).Except(proxyget.DataList);
                            if (expectlist.Count() > 0)
                            {
                                expectcount = count - (int)length < savelength ? count - (int)length : savelength;
                            }
                            proxyget.DataList.AddRange(expectlist.Take((int)expectcount));
                        }
                        if (proxyget.DataList.Count > 0)
                        {
                            proxyget.Code = (int)Enums.StatueCode.成功;
                            proxyget.Guid = Guid.NewGuid().ToString("N");
                            proxyget.Message = String.Format("成功获取{0}条失效代理IP", proxyget.Count);
                        }
                        else
                        {
                            proxyget.Code = (int)Enums.StatueCode.失败;
                            proxyget.Guid = Guid.NewGuid().ToString("N");
                            proxyget.Message = String.Format("获取失效代理IP失败");
                        }
                        //Get请求允许json
                        json = Json(proxyget, JsonRequestBehavior.AllowGet);
                        break;

                    //存活校验节点
                    case  Enums.存活状态.存活:
                        //获得存活表的长度
                        var alivelength = RedisHelper.GetInstance().ListLength(ConstString.TableName_Alive);
                        //存活表长度判断  如果长度小于提取 则提取存活表的全部数据
                        long selectcount = alivelength > (long)count ? (long)count : alivelength;
                        var alivelist = RedisHelper.GetInstance().ListRange<ProxyIpBase>(ConstString.TableName_Alive, true);
                        //剔除已经缓存(正在或已经被节点校验)的IP
                        proxyget.DataList.AddRange((from a in alivelist where !RedisHelper.GetInstance().KeyExists(ConstString.TableName_AliveCatch + a.Ip_Str) select a).Take((int)selectcount));
                        //缓存两小时 两小时内不重复校验
                        proxyget.DataList.ForEach(d => { RedisHelper.GetInstance().StringSet(ConstString.TableName_AliveCatch + d.Ip_Str, "alive_catch", new TimeSpan(0, 1, 0, 0)); });

                        if (proxyget.DataList.Count > 0)
                        {
                            proxyget.Code = (int)Enums.StatueCode.成功;
                            proxyget.Guid = Guid.NewGuid().ToString("N");
                            proxyget.Message = String.Format("成功获取{0}条存活代理IP", proxyget.Count);
                        }
                        else
                        {
                            proxyget.Code = (int)Enums.StatueCode.失败;
                            proxyget.Guid = Guid.NewGuid().ToString("N");
                            proxyget.Message = String.Format("没有需要校验的存活代理IP");
                        }
                        json = Json(proxyget, JsonRequestBehavior.AllowGet);
                        break;
                }

                proxyget = null;
                return json;
            });
        }
        /// <summary>
        /// 获得扫描用端口号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<JsonResult> GetPorts()
        {
            return Task<JsonResult>.Factory.StartNew(() => {
                var portset = RedisHelper.GetInstance().SetMembers<int>(ConstString.TableName_PortSet);
                return Json(portset,JsonRequestBehavior.AllowGet);
            });
        }
        #endregion

        #region 用户调用
        /// <summary>
        /// 用户调用 存活IP提取
        /// </summary>
        /// <param name="count">提取条数</param>
        /// <param name="user">用户索引(中间件唯一标识)</param>
        /// <returns></returns>
        [HttpPost]
        public Task<JsonResult> GetProxyIP_User(int count,int user)
        {
            return Task<JsonResult>.Factory.StartNew(() => {
                //获得过滤条件
                String filtersstr = Request["filters"];
                //子类泛化对象
                ProxyIPTransOut<ProxyIpChild> user_proxyget = new ProxyIPTransOut<ProxyIpChild>();
                //获得存活表的所有数据
                var alivelist_user = RedisHelper.GetInstance().ListRange<ProxyIpChild>(ConstString.TableName_Alive, true);
                //存活ip不为空
                if (alivelist_user.Count() > 0)
                {
                    if (alivelist_user.Count() > count)
                    {
                        //存在过滤条件
                        if (String.IsNullOrEmpty(filtersstr))
                        {
                            try
                            {
                                //使用反射进行属性过滤
                                var templist = new List<ProxyIpChild>();
                                var Filter = JsonConvert.DeserializeObject<ProxyIpFilter>(filtersstr);
                                var proe = typeof(ProxyIpFilter).GetProperties().ToList();
                                alivelist_user.ToList().ForEach(item =>
                                {
                                    proe.ForEach(pro =>
                                    {
                                        String provalue = Convert.ToString(pro.GetValue(Filter));
                                        if (!String.IsNullOrEmpty(provalue)&&provalue.Equals(Convert.ToString(pro.GetValue(item))))
                                        {
                                            templist.Add(item);
                                        }
                                    });
                                });
                                user_proxyget.DataList = templist;
                            }
                            catch 
                            {
                                user_proxyget.Code = (int)Enums.StatueCode.失败;
                                user_proxyget.Guid = Guid.NewGuid().ToString("N");
                                user_proxyget.Message = "过滤器序列化失败";
                            }
                        }
                        //校验缓存
                        user_proxyget.DataList.AddRange((from a in alivelist_user where !RedisHelper.GetInstance().KeyExists(ConstString.TableName_UserCatch + a.Ip_Str + "_" + user) select a).Take(count));
                        //已经取过的插入缓存  在没有取完的情况下 保证一小时内不重复
                        if (user_proxyget.DataList.Count > 0)
                        {
                            user_proxyget.DataList.ForEach(d => { RedisHelper.GetInstance().StringSet(ConstString.TableName_UserCatch + d.Ip_Str + "_" + user, "user_catch", new TimeSpan(0, 1, 0, 0)); });
                        }
                        else
                        {
                            //取完了随机取
                            Random r = new Random();
                            if (alivelist_user.Count() > count)
                            {
                                user_proxyget.DataList.AddRange(alivelist_user.ToList().GetRange(r.Next(0, alivelist_user.Count() - count), count));
                            }
                        }
                    }
                    else
                    {
                        user_proxyget.DataList.AddRange(alivelist_user);
                    }

                    user_proxyget.Code = (int)Enums.StatueCode.成功;
                    user_proxyget.Guid = Guid.NewGuid().ToString("N");
                    user_proxyget.Message = String.Format("成功获取{0}条存活代理IP", user_proxyget.DataList.Count);
                }
                else
                {
                    user_proxyget.Code = (int)Enums.StatueCode.失败;
                    user_proxyget.Guid = Guid.NewGuid().ToString("N");
                    user_proxyget.Message = "没有可用代理IP";
                }
                return Json(user_proxyget, JsonRequestBehavior.AllowGet);
            });
        }
        #endregion

    }
}
