/// <summary>
/// Stream 、 FileStream、File 扩展函数类
/// </summary>
public static class StaticStreamHelper
{

    /// <summary>
    /// 读取文件并转为BASE64
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string fromFilePathToBase64(this string filePath)
    {
        filePath = filePath.GetRuntimeDirectory();
        if (!File.Exists(filePath)) return "";
        using FileStream filestream = new FileStream(filePath, FileMode.Open);
        byte[] bt = new byte[filestream.Length];
        filestream.Read(bt, 0, bt.Length);
        return Convert.ToBase64String(bt);
    }

    /// <summary>
    /// 读取文件夹内容（读取JSON文件、TXT文件等）
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string fromFilePathLoadStr(this string filePath)
    {
        filePath = filePath.GetRuntimeDirectory();
        if (!File.Exists(filePath)) return "";
        using FileStream filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using StreamReader sr = new StreamReader(filestream);
        return sr.ReadToEnd().ToString();
    }

    /// <summary>
    ///  Stream to base64
    /// </summary>
    /// <param name="fs">文件流</param>
    /// <returns></returns>
    public static string? fromStreamToBase64(this Stream fs)
    {
        string? strRet = null;

        try
        {
            if (fs == null) return null;
            byte[] bt = new byte[fs.Length];
            fs.Read(bt, 0, bt.Length);
            strRet = Convert.ToBase64String(bt);
            fs.Close();
        }
        catch { }

        return strRet;
    }

    /// <summary>
    /// string 转 MemoryStream
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static MemoryStream fromStringToMs(this string str)
    {
        byte[] array = Encoding.UTF8.GetBytes(str);
        return new MemoryStream(array);
    }

    /// <summary>
    /// MemoryStream 转 Stream
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    public static Stream fromMsToStream(this MemoryStream ms)
    {
        ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }

    /// <summary>
    /// base64 转 ms
    /// </summary>
    /// <param name="_base64"></param>
    /// <returns></returns>
    public static MemoryStream fromBase64ToMs(this string _base64)
    {
        byte[] Byte = Convert.FromBase64String(_base64);
        return new MemoryStream(Byte);
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="_ms"></param>
    /// <param name="filePath"></param>
    public static void saveToFile(this MemoryStream _ms, string filePath,bool overWrite = false)
    {
        filePath = filePath.GetRuntimeDirectory();
        if (!File.Exists(filePath))
        {
            if (overWrite) File.Delete(filePath);
            else throw new Exception($"{filePath}文件已存在！无法写入。");
        }
        _ms.Position = 0;
        byte[] buffer = new byte[_ms.Length];
        _ms.Read(buffer, 0, (int)_ms.Length);
        File.WriteAllBytes(filePath.GetRuntimeDirectory(), buffer);
    }

    /// <summary>
    /// 读取文件到FileStream
    /// </summary>
    /// <param name="_filePath"></param>
    /// <returns></returns>
    public static FileStream getTxtFileMs(this string filePath)
    {
        filePath = filePath.GetRuntimeDirectory();
        if (!File.Exists(filePath)) throw new Exception($"找不到文件{filePath}！");
        return new FileStream(filePath, FileMode.Open);
    }

    /// <summary>
    /// 获取文件夹下所有文件的名称
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="lastName">默认只获取txt后缀名文件</param>
    /// <returns></returns>
    public static List<string> GetFileList(this string folderPath, string lastName = ".txt")
    {
        DirectoryInfo folder = new DirectoryInfo(folderPath.GetRuntimeDirectory());
        List<string> _result = new List<string>();
        if (folder.Exists)
        {
            FileInfo[] files = folder.GetFiles();
            foreach (FileInfo _file in files)
            {
                if (_file.Name.ToLower().EndsWith(lastName))
                    _result.Add(_file.Name.Split('.')[0]);
            }
        }
        return _result;
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="_base64"></param>
    /// <param name="_path"></param>
    /// <param name="_filename">文件名需带后缀</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task fromBase64ToSaveFile(this string _base64, string _path, string _filename)
    {
        if (!_base64.IsBase64()) throw new Exception("base64字符串错误");

        await Task.Run(() =>
        {         //检查路径
            string _dic_path = _path.GetRuntimeDirectory();
            if (!Directory.Exists(_dic_path)) Directory.CreateDirectory(_dic_path);
            //检查文件
            string _file_path = $"{_path}/{_filename}".GetRuntimeDirectory();
            if (File.Exists(_file_path)) File.Delete(_file_path);

            using MemoryStream stream = _base64.fromBase64ToMs();
            stream.saveToFile(_dic_path);
        });
    }

}


