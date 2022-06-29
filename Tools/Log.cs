using System.Text;

namespace Tools
{
    public enum PrintMode
    {
       Info,Error,Warning
    }

    public enum OpenMode
    {
       Read, Write
    }

    public static class Log
    {
        /// <summary>
        /// 打开日志文档流
        /// </summary>
        /// <returns></returns>
        private static FileStream OpenLog(OpenMode mode)
        {
            string Date = DateTime.Now.ToString("yyyy_M_dd");
            if (!Directory.Exists($"./Log"))
            {
                Directory.CreateDirectory($"./Log");
                new FileStream($"Log/{Date}.log", FileMode.Create);
            }
            if (mode == OpenMode.Read)
            {
                return new FileStream($"Log/{Date}.log", FileMode.Open, FileAccess.Read);
            }else
            {
                return new FileStream($"Log/{Date}.log", FileMode.Append, FileAccess.Write);
            }
        }

        /// <summary>
        /// 获取指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static FileStream OpenLog(string fileName)
        {
            if (!Directory.Exists($"./Log"))
            {
                Directory.CreateDirectory($"./Log");
                new FileStream($"Log/{fileName}", FileMode.Create);
            }
            return new FileStream($"Log/{fileName}", FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// 写入消息
        /// </summary>
        /// <param name="msg"></param>
        public static void Print(string msg,PrintMode mode)
        {
            FileStream file = OpenLog(OpenMode.Write);
            StreamWriter writer = new StreamWriter(file);
            //普通消息
            if (mode == PrintMode.Info)
            {
                writer.WriteLine($"INFO[{DateTime.Now.ToString("f")}]:{msg}");
            }
            //错误消息
            else if (mode == PrintMode.Error)
            {
                writer.WriteLine($"ERROR[{DateTime.Now.ToString("f")}]:{msg}");
            }
            //警告消息
            else if (mode == PrintMode.Warning)
            {
                writer.WriteLine($"WARNING[{DateTime.Now.ToString("f")}]:{msg}");
            }
            writer.Close();
            file.Close();
            writer.Dispose();
            file.Dispose();
        }

        /// <summary>
        /// 读取日志文档
        /// </summary>
        /// <returns></returns>
        public static List<string> Read(string fileName)
        {
            FileStream file = OpenLog(fileName);
            StreamReader reader = new StreamReader(file);
            List<string> builder = new List<string>(); 
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                builder.Add(line);
            }
            reader.Close();
            file.Close();
            reader.Dispose();
            file.Dispose();
            return builder;
        }

        public static List<string> GetFileList()
        {
            if (!Directory.Exists("./Log"))
            {
                Directory.CreateDirectory("./Log");
            }
            var result =new DirectoryInfo("./Log").GetFiles().Select(i=>i.Name).ToList();
            result.Reverse();
            return  result;
        }
    }
}
