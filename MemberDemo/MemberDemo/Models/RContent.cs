using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace MemberDemo.Models
{
    /// <summary>
    /// 回傳內容 T
    /// </summary>
    public class RContent<T>
    {
        public int err { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }

    /// <summary>
    /// 回傳內容
    /// </summary>
    public class RContent {
        public int err { get; set; }
        public string msg { get; set; }
        public string data { get; set; }
    }
}