/// <summary>
/// 日志输出
/// </summary>
public static class LogWriter
{


    /// <summary>
    /// 构造函数直出
    /// </summary>
    /// <param name="msg">消息内容</param>
    /// <param name="_secondFoler">日志路径，为空输出在根目录下log文件夹，多租户案例建议传入 $"{Error}\\{租户ID}" </param>
    /// <param name="_fileName">二级目录，为空默认error</param>
    public static void WriteLog(this string msg, string _secondFoler, string _fileName = "", bool _time_line = true)
    {
        try
        {
            if (string.IsNullOrEmpty(_secondFoler)) _secondFoler = "Error";

            var _path = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\{_secondFoler}".GetRuntimeDirectory();
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            var _filename = $"{DateTime.Now.ToString("yyyy-MM-dd")}{(string.IsNullOrEmpty(_fileName) ? "" : $"_{_fileName}")}.txt";
            using StreamWriter sw = new StreamWriter($"{_path}\\{_filename}".GetRuntimeDirectory(), true);
            if(_time_line) sw.WriteLine($"{DateTime.Now.ToLocalTime()}");
            sw.WriteLine(msg);
        }
        catch { }
    }


    /// <summary>
    /// 构造函数直出
    /// </summary>
    /// <param name="msg">消息内容</param>
    /// <param name="_secondFoler">日志路径，为空输出在根目录下log文件夹，多租户案例建议传入 $"{Error}\\{租户ID}"</param>
    /// <param name="_fileName">二级目录，为空默认error</param>
    public static void WriteLog(this StringBuilder msg, string _secondFoler, string _fileName = "")
    {
        msg.ToString().WriteLog(_secondFoler,_fileName);
    }

    /// <summary>
    /// 构造函数直出
    /// </summary>
    /// <param name="msg">消息内容</param>
    /// <param name="_secondFoler">日志路径，为空输出在根目录下log文件夹，多租户案例建议传入 $"{Error}\\{租户ID}"</param>
    /// <param name="_fileName">二级目录，为空默认error</param>
    public static void WriteLog(this Exception msg, string _secondFoler, string _fileName = "")
    {
        msg.ToString().WriteLog(_secondFoler, _fileName);
    }

    /// <summary>
    /// 构造函数直出
    /// </summary>
    /// <param name="msg">消息内容</param>
    /// <param name="_secondFoler">日志路径，为空输出在根目录下log文件夹，多租户案例建议传入 $"{Error}\\{租户ID}"</param>
    /// <param name="_fileName">二级目录，为空默认error</param>
    public static void WriteLog(this object msg, string _secondFoler, string _fileName = "")
    {
        msg.toJsonStr().WriteLog(_secondFoler, _fileName);
    }

    /// <summary>
    /// 构造函数直出(不输出时间行)
    /// </summary>
    /// <param name="msg">消息内容</param>
    /// <param name="_secondFoler">日志路径，为空输出在根目录下log文件夹，多租户案例建议传入 $"{Error}\\{租户ID}" </param>
    /// <param name="_fileName">二级目录，为空默认error</param>
    public static void WriteLogWithoutTime(this string msg, string _secondFoler, string _fileName = "")
    {
        msg.ToString().WriteLog(_secondFoler, _fileName, false);
    }
}
