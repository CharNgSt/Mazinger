/// <summary>
/// Josn 扩展函数类
/// </summary>
public static class StaticJsonHelper
{
    /// <summary>
    /// 获取JObject的值
    /// </summary>
    /// <param name="_json"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static string getJsonValue(this JObject _json, string _name)
    {
        var _res = "";
        try
        {
            _res = _json[_name]?.ToString();
        }
        catch { }
        return _res; 
    }

    /// <summary>
    /// 对象转JsonString
    /// </summary>
    /// <param name="_obj">传入</param>
    /// <returns></returns>
    public static string toJsonStr(this object _obj)
    {
        return JsonConvert.SerializeObject(_obj);
    }

    /// <summary>
    /// string转JsonObject
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static JObject toJsonObj(this string _val)
    {
        return _val.toJsonObj(out string _no_use);
    }

    /// <summary>
    /// string转JsonObject
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static JObject toJsonObj(this string _val, out string _in_val)
    {
        _in_val = _val;
        return JsonConvert.DeserializeObject<JObject>(_val);
    }

    /// <summary>
    /// string转JsonObject
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static JArray toJsonArray(this string _val)
    {
        return _val.toJsonArray(out string _no_use);
    }

    /// <summary>
    /// string转JsonArray
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static JArray toJsonArray(this string _val, out string _in_val)
    {
        _in_val = _val;
        return JsonConvert.DeserializeObject<JArray>(_val);
    }

}