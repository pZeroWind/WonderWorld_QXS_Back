using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.Result
{
    /// <summary>
    /// 返回类 - 单对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public int code { get; set; } = 200;
        public string msg { get; set; } = "操作成功";
        public T? data { get; set; }
    }

    /// <summary>
    /// 返回类 - 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultList<T>:Result<T>
    {
        public int total { get; set; }

        public int count { get; set; }

        public int size { get; set; }

        public int page { get; set; }
    }

}
