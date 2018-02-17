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
        public RedisHelper redishelper = new RedisHelper();
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

        /// <summary>
        /// 有效IP插入 如热表和存档表
        /// </summary>
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
                        case Enums.存活状态.存活:
                            var insertobj_alive = JsonConvert.DeserializeObject<ProxyIPTransIn<ProxyIpChild>>(data);
                            try
                            {
                                Guid = insertobj_alive.Guid;
                                if (insertobj_alive != null && insertobj_alive.DataList.Count > 0)
                                {
                                    long length = redishelper.ListLength(ConstString.TableName_Alive);

                                    redishelper.ListRightPushRange(ConstString.TableName_Save, insertobj_alive.DataList, true);
                                    if (redishelper.ListRightPushRange(ConstString.TableName_Alive, insertobj_alive.DataList, true))
                                    {
                                        json.Code = 0;
                                        json.Guid = Guid;
                                        json.Message = String.Format("{0}条存活记录插入成功", insertobj_alive.Count);
                                    }
                                    else
                                    {
                                        json.Code = 0;
                                        json.Guid = Guid;
                                        json.Message = String.Format("{0}条存活记录插入成功,提交的数据有重复", redishelper.ListLength(ConstString.TableName_Alive) - length);
                                    }
                                }
                                else
                                {
                                    json.Code = 1;
                                    json.Guid = Guid;
                                    json.Message = String.Format("提交数据条数为0");
                                }
                            }
                            catch (Exception ex)
                            {
                                json.Code = 1;
                                json.Guid = Guid;
                                json.Message = String.Format("插入出错 错误信息为:{0}", ex.Message + "\r\n" + ex.StackTrace);
                            }
                            finally
                            {
                                insertobj_alive = null;
                            }
                            break;
                        case Enums.存活状态.失效:
                            var insertobj_dead = JsonConvert.DeserializeObject<ProxyIPTransIn<ProxyIpBase>>(data);
                            try
                            {
                                if (insertobj_dead != null && insertobj_dead.DataList.Count > 0)
                                {
                                    var length = redishelper.ListLength(ConstString.TableName_Sleep);
                                    if (redishelper.ListRightPushRange(ConstString.TableName_Sleep, insertobj_dead.DataList, true))
                                    {
                                        json.Code = 0;
                                        json.Guid = insertobj_dead.Guid;
                                        json.Message = String.Format("{0}条失效记录插入成功", insertobj_dead.DataList.Count);
                                    }
                                    else
                                    {
                                        json.Code = 0;
                                        json.Guid = insertobj_dead.Guid;
                                        json.Message = String.Format("{0}条失效记录插入成功,提交的数据有重复", redishelper.ListLength(ConstString.TableName_Sleep) - length);
                                    }

                                }
                                else
                                {
                                    json.Code = 1;
                                    json.Guid = Guid;
                                    json.Message = String.Format("提交数据条数为0");
                                }
                            }
                            catch (Exception ex)
                            {
                                json.Code = 1;
                                json.Guid = Guid;
                                json.Message = String.Format("插入出错 错误信息为:{0}", ex.Message + "\r\n" + ex.StackTrace);
                            }
                            finally
                            {
                                insertobj_dead = null;
                            }
                            break;
                    }
                }

                return Json(json);

            });


        }

        /// <summary>
        /// 获得IP
        /// </summary>
        /// <param name="count"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<JsonResult> GetProxyIP(int count, int user = 0)
        {
            return Task<JsonResult>.Factory.StartNew(() =>
            {
                JsonResult json = null;
                ProxyIPTransOut<ProxyIpBase> proxyget = new ProxyIPTransOut<ProxyIpBase>();
                switch (user)
                {
                    //失效扫描节点
                    case -1:
                        var length = redishelper.ListLength(ConstString.TableName_Sleep);
                        proxyget.DataList = new List<ProxyIpBase>();
                        if (length > count)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                proxyget.DataList.Add(redishelper.ListLeftPop<ProxyIpBase>(ConstString.TableName_Sleep));
                            }
                        }
                        else
                        {
                            long expectcount = 0;
                            var savelength = redishelper.ListLength(ConstString.TableName_Save);
                            proxyget.DataList.AddRange(redishelper.ListRange<ProxyIpBase>(ConstString.TableName_Sleep, true));
                            redishelper.KeyDelete(ConstString.TableName_Sleep);
                            var expectlist = redishelper.ListRange<ProxyIpBase>(ConstString.TableName_Save, true).Except(proxyget.DataList);
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
                        json = Json(proxyget, JsonRequestBehavior.AllowGet);
                        break;

                    //存活校验节点
                    case 0:
                        var alivelength = redishelper.ListLength(ConstString.TableName_Alive);
                        long selectcount = alivelength > (long)count ? (long)count : alivelength;
                        var alivelist = redishelper.ListRange<ProxyIpBase>(ConstString.TableName_Alive, true);
                        proxyget.DataList.AddRange((from a in alivelist where !redishelper.KeyExists(ConstString.TableName_AliveCatch + a.Ip_Str) select a).Take((int)selectcount));
                        //缓存两小时
                        proxyget.DataList.ForEach(d => { redishelper.StringSet(ConstString.TableName_AliveCatch + d.Ip_Str, "alive_catch", new TimeSpan(0, 1, 0, 0)); });

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
                    default:
                        ProxyIPTransOut<ProxyIpChild> user_proxyget = new ProxyIPTransOut<ProxyIpChild>();
                        var alivelist_user = redishelper.ListRange<ProxyIpChild>(ConstString.TableName_Alive, true);
                        //存活ip不为空
                        if (alivelist_user.Count() > 0)
                        {
                            if (alivelist_user.Count() > count)
                            {
                                //校验缓存
                                user_proxyget.DataList.AddRange((from a in alivelist_user where !redishelper.KeyExists(ConstString.TableName_UserCatch + a.Ip_Str + "_" + user) select a).Take(count));
                                //已经取过的插入缓存  在没有取完的情况下 保证一小时内不重复
                                if (user_proxyget.DataList.Count > 0)
                                {
                                    user_proxyget.DataList.ForEach(d => { redishelper.StringSet(ConstString.TableName_UserCatch + d.Ip_Str + "_" + user, "user_catch", new TimeSpan(0, 1, 0, 0)); });
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
                        json = Json(user_proxyget, JsonRequestBehavior.AllowGet);
                        user_proxyget = null;
                        break;
                }

                proxyget = null;
                return json;
            });
        }




        /// <summary>
        /// 从Redis缓存中获得指定条件的代理IP
        /// </summary>
        /// <param name="appuser">用户代号</param>
        /// <returns></returns>
        //public Task<JsonResult> GetProxyIP(int appuser, int count = 1000)
        //{
        //    return Task<JsonResult>.Factory.StartNew(() =>
        //    {
        //        Enums.ProxyUsers UserEnum = Enums.ProxyUsers.存活校验节点;
        //        String filter = Request["filter"];
        //        ProxyIpFilter proxyfilter;
        //        List<ProxyIp> ProxyList = new List<ProxyIp>();
        //        int delay = 10000;
        //        if (!String.IsNullOrEmpty(filter))
        //        {
        //            proxyfilter = JsonConvert.DeserializeObject(filter) as ProxyIpFilter;
        //        }

        //        if (proxyfilter != null)
        //        {
        //            delay = proxyfilter.Delay;
        //        }

        //        //用户校验
        //        try
        //        {
        //            UserEnum = (Enums.ProxyUsers)appuser;
        //        }
        //        catch
        //        {
        //            return Json(new JsonBase((int)Enums.StatueCode.失败, String.Format("用户身份校验失败,没有代号为{0}的用户", appuser)), JsonRequestBehavior.AllowGet);
        //        }

        //        if (ServiceCatch.CatchProxy.Count == 0)
        //        {

        //        }

        //        switch (UserEnum)
        //        {
        //            case Enums.ProxyUsers.存活校验节点:

        //                break;
        //            case Enums.ProxyUsers.扫描节点:
        //                break;
        //            case Enums.ProxyUsers.失效校验节点:
        //                break;
        //            case Enums.ProxyUsers.消费者用户:
        //                break;
        //        }

        //    });
        //}


    }
}
