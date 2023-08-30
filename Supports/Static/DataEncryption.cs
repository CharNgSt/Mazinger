using Furion.ClayObject;
/// <summary>
/// 加解密类
/// </summary>
public static class StaticDataEncryptionHelper
{
    /// <summary>
    /// 获取Desc加密串
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_key"></param>
    /// <returns></returns>
    public static string DescEncode(this string _val, string _key) => DESCEncryption.Encrypt(_val, _key);

    /// <summary>
    /// 获取Desc原文
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_key"></param>
    /// <returns></returns>
    public static string DescDecode(this string _val, string _key) => DESCEncryption.Decrypt(_val, _key);

    /// <summary>
    /// 获取md5串
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_toUpper">全大写</param>
    /// <param name="_is16">获取16位</param>
    /// <returns></returns>
    public static string MD5Encode(this string _val, bool _toUpper = true, bool _is16 = false) => MD5Encryption.Encrypt(_val, _toUpper, _is16);

}
