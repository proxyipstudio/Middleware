using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PublicLib
{
    /// <summary>
    /// json 基类
    /// </summary>
    [Serializable()]
    public class JsonBase
    {
        public JsonBase()
        {

        }
        public JsonBase(int code, String message)
        {
            this.Code = code;
            this.Message = message;
        }

        int _code = 0;
        /// <summary>
        /// 返回状态码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        String _message = String.Empty;
        /// <summary>
        /// 反馈消息
        /// </summary>
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

    }

    /// <summary>
    /// 包含请求标识的json实体类
    /// </summary>
    public class GuidJson : JsonBase
    {
        public GuidJson()
        {

        }
        public GuidJson(int code, String message)
        {
            this.Code = code;
            this.Message = message;
        }
        String _guid = String.Empty;
        /// <summary>
        /// 请求GUID
        /// </summary>
        public String Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
    }
}
