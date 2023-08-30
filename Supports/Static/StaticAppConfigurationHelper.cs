
/// <summary>
/// 读取配置文件
/// </summary>
public static class StaticAppConfigurationHelper
{
    /// <summary>
    /// get str
    /// </summary>
    /// <param name="_local"></param>
    /// <returns></returns>
    public static string GetConfigVal(this string _local)
    {
        var _res = App.Configuration[_local];
        return string.IsNullOrEmpty(_res) ? "" : _res;
    }

    /// <summary>
    /// get int
    /// </summary>
    /// <param name="_local"></param>
    /// <returns></returns>
    public static int GetConfigInt(this string _local)
    {
        int.TryParse(_local.GetConfigVal(),out var _val);
        return _val;
    }

    /// <summary>
    /// get decimal
    /// </summary>
    /// <param name="_local"></param>
    /// <returns></returns>
    public static decimal GetConfigDecimal(this string _local)
    {
        decimal.TryParse(_local.GetConfigVal(), out var _val);
        return _val;
    }

    /// <summary>
    /// get bool
    /// </summary>
    /// <param name="_local"></param>
    /// <returns></returns>
    public static bool GetConfigBool(this string _local)
    {
        bool.TryParse(_local.GetConfigVal(), out var _val);
        return _val;
    }

}