using HttpToolsLib;
using Newtonsoft.Json;
using PublicLib;
using RedisHelperLib;
using RedisHelperLib.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Redis测试
{
    class Program
    {
        const String Host = "http://localhost:46629/";
        const string tempkey = "000C_192.168.0.1:80";
        static void Main(string[] args)
        {
            //移除所有key();
            //RedisTest();
            IP推送测试();
            //获得所有key();
            //IP读取测试();

            Console.ReadKey();
        }

        private static void 移除所有key(String p = "")
        {
            RedisHelper helper = new RedisHelper();

            var server = RedisHelper._connMultiplexer.GetServer("127.0.0.1:6379");
            // get the target server

            // show all keys in database 0 that include "foo" in their name
            foreach (var key in server.Keys(pattern: p+"*"))
            {
                Console.WriteLine(key);
                helper.KeyDelete(key,false);
            }
        }

        private static void 获得所有key()
        {
            RedisHelper helper = new RedisHelper();

             var server= RedisHelper._connMultiplexer.GetServer("127.0.0.1:6379");
            // get the target server

            // show all keys in database 0 that include "foo" in their name
            foreach (var key in server.Keys(pattern: "*"))
            {
                Console.WriteLine(key);
                //helper.KeyDelete(key,-2);
            }

            // completely wipe ALL keys from database 0
            //server.FlushDatabase();
        }

        private static void IP读取测试()
        {
            ////IDatabase db = StackExchangeRedisHelper.GetDatabase();
            ////RedisValue[] values = db.SortedSetRangeByRank("192.168.0.1:80");
            ////var a = Deserialize<ProxyIp>(values[0]);
            //IDatabase db = StackExchangeRedisHelper.GetDatabase();
            ////RedisValue[] values = db.set("000C192.168.0.1:80");
            //var a = db.SetContains("000B192.168.0.1:80", StackExchangeRedisHelper.Serialize(1));
            //Console.WriteLine(a);
            ////var a = Deserialize<int>(values[0]);
            RedisHelper helper = new RedisHelper();
       var a=   helper.ListRange<ProxyIpBase>("000A_Sleep",false);
            //var a = helper.li("192.168.0.1:80",1);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        private static void IP推送测试()
        {
            String InsertUrl = Host+"ProxyIp/Insert?code=0";
            ProxyIPTransIn<ProxyIpChild> testinsert = new ProxyIPTransIn<ProxyIpChild>();
            testinsert.Guid = Guid.NewGuid().ToString("N");
       
            for (int i = 0; i < 10; i++)
            {
                ProxyIpChild ip = new ProxyIpChild();
                ip.VPSIp = "127.0.0.1:80";
                ip.VPSNum = "001";
                ip.ProxyStatue = 0;
                ip.Anonymity = 0;
                ip.CheckTime = 1;
                ip.City = "宁波";
                ip.Delay = 2000;
                ip.IpAddress = "192.168.0." + (i + 1);
                ip.LastCheckTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ip.Operator = "移动";
                ip.Province = "浙江省";
                ip.Country = "中国";
                //ip.UserCodeList = new List<int>{1,0,1,0};
                //ip.Ip_Str = ip.IpAddress + ":" + ip.Port;
                testinsert.DataList.Add(ip);
            }
            String str = JsonConvert.SerializeObject(testinsert);
            //RedisHelper helper = new RedisHelper();
            //helper.StringSet("testobject", testinsert);
            ////var a =
            //Console.WriteLine(JsonConvert.SerializeObject(helper.StringGet<ProxyIPInsert>("testobject")));
            Console.WriteLine(HttpMethod.FastPostMethod(InsertUrl, "data=" + str));
        }
        public static void RedisTest()
        {
            //Console.WriteLine(default(TimeSpan));
            //Console.WriteLine("Redis写入缓存：Name:张三丰");
            //StackExchangeRedisHelper.Set("Name", "张三丰");
            //Console.WriteLine("Redis获取缓存：Name：" + StackExchangeRedisHelper.Get("Name").ToString());
            //Thread.Sleep(1000);
            //Console.WriteLine("一秒后Redis获取缓存：Name：" + StackExchangeRedisHelper.Get("Name") ?? "");
            //Console.ReadKey();
        }
    }
}
