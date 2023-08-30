/// <summary>
/// 密码复杂度判断
/// </summary>
public static class StaticPwdRule
{
    /// <summary>
    /// 密码强度检查，强密码
    /// |***  至少包含一个小写字母 [a-z]          ***|
    /// |***  至少包含一个大写字母[A - Z]         ***|
    /// |***  至少包含一个数字 \d                 ***|
    /// |***  至少包含一个特殊字符[^\da - zA - Z] ***|
    /// |***  长度至少为 8 个字符.{8,}            ***|
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static bool CheckPwd_IsLevel3(this string _val)
    {
        return Regex.IsMatch(_val, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
    }
    public static string Level3PwdIllustrate = "";

    /// <summary>
    /// 密码强度检查，中等密码
    /// |***  至少包含一个小写字母 [a-z]          ***|
    /// |***  至少包含一个数字 \d                 ***|
    /// |***  至少包含一个特殊字符[^\da - zA - Z] ***|
    /// |***  长度至少为 6 个字符.{6,}            ***|
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static bool CheckPwd_IsLevel2(this string _val)
    {
        return Regex.IsMatch(_val, @"^(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$");
    }
    public static string Level2PwdIllustrate = "至少包含一个小写字母、一个数字、一个特殊字符，长度不小于6位";

    /// <summary>
    /// 密码强度检查，弱密码
    /// |***  至少包含一个数字 (?=.*\d)                       ***|
    /// |***  至少包含一个字母（大小写不限） (?=.*[a-zA-Z])   ***|
    /// |***  长度至少为 6 个字符 .{6,}                       ***|
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static bool CheckPwd_IsLevel1(this string _val)
    {
        return Regex.IsMatch(_val, @"^(?=.*\d)(?=.*[a-zA-Z]).{6,}$");
    }
    public static string Level1PwdIllustrate = "至少至少包含一个数字、一个字母（大小写不限），长度不小于6位";


}

