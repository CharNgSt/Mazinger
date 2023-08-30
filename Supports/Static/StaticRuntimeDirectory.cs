/// <summary>
/// 文件目录扩展类
/// </summary>
public static class StaticRuntimeDirectoryHelper
{
    /// <summary>
    /// 根据部署环境解析地址
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetRuntimeDirectory(this string path)
    {
        //ForLinux
        if (IsLinuxRunTime())
            return GetLinuxDirectory(path);
        //ForWindows
        if (IsWindowRunTime())
            return GetWindowDirectory(path);
        return path;
    }

    //OSPlatform.Windows监测运行环境
    private static bool IsWindowRunTime()
    {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    //OSPlatform.Linux运行环境
    private static bool IsLinuxRunTime()
    {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    private static string GetLinuxDirectory(string path)
    {
        string pathTemp = Path.Combine(path);
        return pathTemp.Replace("\\", "/").Replace("\\", "/");
    }
    private static string GetWindowDirectory(string path)
    {
        string pathTemp = Path.Combine(path);
        return pathTemp.Replace("/", "\\");
    }
}

