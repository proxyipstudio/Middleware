#region 说明
//---------------------------------------名称:封装的字符串处理操作类
//---------------------------------------依赖DLL:HtmlAgilityPack.dll
//---------------------------------------版本:1.1.0.0
//---------------------------------------更新时间:2017/10/18
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using System;
using System.Collections.Generic;
using HtmlAgilityPack; 
#endregion

namespace ExtractLib
{
    /// <summary>
    /// 使用xpath抽取文本
    /// </summary>
    public class XpathMethod
    {
        #region 构造函数
        /// <summary>
        /// 构造函数重载 传入html源代码 生成HtmlDocument对象
        /// </summary>
        /// <param name="html"></param>
        public XpathMethod(String html)
        {
            Document.LoadHtml(html);
        }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public XpathMethod()
        {
            // TODO: Complete member initialization
        } 
        #endregion

        #region 属性
        private HtmlAgilityPack.HtmlDocument _Document = new HtmlDocument();

        public HtmlAgilityPack.HtmlDocument Document
        {
            get { return _Document; }
            set { _Document = value; }
        }
        #endregion

        #region 对象方法 
        /// <summary>
        /// 抽取单个node的内容
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <returns></returns>
        public static String GetSingleResult(String Xpath, String Html)
        {
            String retStr = String.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(Xpath);
            if (node != null)
            {
                retStr = node.InnerText.Trim(); ;
            }
            return retStr;
        }
        /// <summary>
        /// 抽取单个node的内容  Flag标识抽取或匹配
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Flag">1为匹配 0为抽取</param>
        /// <returns></returns>
        public static String GetSingleResult(String Xpath, String Html, int Flag)
        {
            String retStr = String.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(Xpath);
            if (node != null)
            {
                if (Flag == 1)
                {
                    retStr = node.InnerText;
                }
                else if (Flag == 0)
                {
                    retStr = node.OuterHtml;
                }
            }
            return retStr;
        }
        /// <summary>
        /// 抽取单个node指定属性
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Attributes">待抽取的节点属性</param>
        /// <returns></returns>
        public static String GetSingleResult(String Xpath, String Html, String Attributes)
        {
            String retStr = String.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(Xpath);
            if (node != null)
            {
                try
                {
                    if (node.Attributes.Contains(Attributes))
                    {
                        retStr = node.Attributes[Attributes].Value;
                    }
                }
                catch
                {

                }
            }
            return retStr;
        }
        /// <summary>
        /// 抽取多个节点内容
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <returns></returns>
        public static List<String> GetMutResult(String Xpath, String Html)
        {
            List<String> retlist = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    retlist.Add(node.InnerText.Trim());
                }
            }
            return retlist;
        }
        /// <summary>
        /// 抽取多个节点内容
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Flag">0为匹配 1为抽取</param>
        /// <returns></returns>
        public static List<String> GetMutResult(String Xpath, String Html, int Flag)
        {
            List<String> retlist = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    if (Flag == 1)
                    {
                        retlist.Add(node.InnerText);
                    }
                    else if (Flag == 0)
                    {
                        retlist.Add(node.OuterHtml);
                    }
                }
            }
            return retlist;
        }
        /// <summary>
        /// 抽取多个node 指定属性
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Attributes">待抽取的节点属性</param>
        /// <returns></returns>
        public static List<String> GetMutResult(String Xpath, String Html, String Attributes)
        {
            List<String> retlist = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    try
                    {
                        if (node.Attributes.Contains(Attributes))
                        {
                            retlist.Add(node.Attributes[Attributes].Value);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return retlist;
        }
        /// <summary>
        /// 抽取多个node
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <returns></returns>
        public static HtmlNodeCollection GetMutNodes(String Xpath, String Html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);

            return nodes;
        }
        #endregion

        #region 动态方法 通过对象调用 多线程环境下建议用这种方式 资源消耗小
        /// <summary>
        /// 抽取单条 
        /// </summary>
        /// <param name="Xpath"></param>
        /// <param name="Attributes"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public String GetSingleResultA(String Xpath, String Attributes = null, int Flag = 1)
        {
            if (Document == null)
            {
                return String.Empty;
            }
            var node = Document.DocumentNode.SelectSingleNode(Xpath);
            if (node == null)
            {
                return null;
            }
            if (String.IsNullOrEmpty(Attributes))
            {
                if (Flag == 0)
                {
                    return node.InnerHtml;
                }
                else if (Flag == 1)
                {
                    return node.InnerText;
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                if (node.Attributes.Contains(Attributes))
                {
                    return node.Attributes[Attributes].Value;
                }
                return node.Attributes[Attributes].Value;
            }
        }
        /// <summary>
        /// 抽取多条
        /// </summary>
        /// <param name="Xpath"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public List<String> GetMutResultA(String Xpath, int Flag = 1)
        {
            List<String> list = new List<string>();
            if (Document == null)
            {
                return list;
            }
            var nodes = Document.DocumentNode.SelectNodes(Xpath);
            if (nodes == null || nodes.Count == 0)
            {
                return null;
            }
            foreach (var node in nodes)
            {
                if (Flag == 0)
                {
                    list.Add(node.InnerHtml);
                }
                else if (Flag == 1)
                {
                    list.Add(node.InnerText);
                }
            }
            return list;
        }
        /// <summary>
        /// 抽取多条 重载 抽取属性
        /// </summary>
        /// <param name="Xpath"></param>
        /// <param name="Attributes"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public List<String> GetMutResultA(String Xpath, String Attributes = null, int Flag = 1)
        {
            List<String> list = new List<string>();
            if (Document == null)
            {
                return list;
            }
            var nodes = Document.DocumentNode.SelectNodes(Xpath);
            if (nodes == null || nodes.Count == 0)
            {
                return null;
            }
            foreach (var node in nodes)
            {
                if (Flag == 0)
                {
                    if (String.IsNullOrEmpty(Attributes))
                    {
                        list.Add(node.InnerHtml);
                    }
                    else
                    {
                        list.Add(node.Attributes[Attributes].Value);
                    }
                }
                else if (Flag == 1)
                {
                    if (String.IsNullOrEmpty(Attributes))
                    {
                        list.Add(node.InnerText);
                    }
                    else
                    {
                        if (node.Attributes.Contains(Attributes))
                        {
                            list.Add(node.Attributes[Attributes].Value);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

    }
}
