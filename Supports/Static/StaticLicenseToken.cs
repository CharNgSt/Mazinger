/// <summary>
/// 验证码生成
/// </summary>
public static class LicenseTokenHelper
{
    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <param name="_val">token密钥</param>
    /// <param name="_secretKey">系统密钥，传空值 App.Configuration["License:SecretKey"] 获取 </param>
    /// <param name="_offset">偏移量，默认 0 分钟</param>
    /// <returns></returns>
    public static string toLicenseToken(this string _val, string _secretKey = "", int _offset = 0)
    {
        if (string.IsNullOrEmpty(_secretKey)) _secretKey = "License:SecretKey".GetConfigVal();
        var _DayKey = DateTime.Now.AddMinutes(_offset).ToString("dd-MM-yyyy");
        var _TimeKey = DateTime.Now.AddMinutes(_offset).ToString("hh-mm");
        return $"{_DayKey}{_secretKey}{_TimeKey}{_val}".MD5Encode(true,true);
    }

    /// <summary>
    /// 检查验证码
    /// </summary>
    /// <param name="_val">token原文</param>
    /// <param name="_guid">token密钥</param>
    /// <param name="_secretKey">系统密钥</param>
    /// <param name="_offsetValue">允许的偏移量（单位：分钟）</param>
    /// <returns></returns>
    public static bool checkLicenseToken(this string _val,string _guid, string _secretKey ="" ,  int _offsetValue = 30)
    {
        if (string.IsNullOrEmpty(_secretKey)) _secretKey = "License:SecretKey".GetConfigVal();

        for (int i = -1 * _offsetValue; i <= _offsetValue; i++) if (_guid.toLicenseToken(_secretKey, i) == _val) return true;

        return false;
    }

}
