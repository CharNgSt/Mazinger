/// <summary>
/// 缓存调用
/// </summary>
public class MazingerMemoryCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    /// <summary>
    /// 租户代码
    /// </summary>
    private string tenantCode;
    public MazingerMemoryCache(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _httpContextAccessor = httpContextAccessor;
        var request = _httpContextAccessor.HttpContext.Request;
        tenantCode = request.Cookies["GlobalConfig_TenantCode"];
    }

    #region private 调用函数

    /// <summary>
    /// 获取值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyName"></param>
    /// <param name="emptyThrowMsg"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<T> GetVal<T>(string keyName, string emptyThrowMsg = "")
    {
        _memoryCache.TryGetValue(keyName, out var _val);
        if (_val == null && !string.IsNullOrEmpty(emptyThrowMsg)) throw new Exception(emptyThrowMsg);
        return _val.Adapt<T>();
    }

    /// <summary>
    /// 清楚键值
    /// </summary>
    /// <param name="keyName"></param>
    /// <returns></returns>
    private async Task RemoveKey(string keyName) => _memoryCache.Remove(keyName);

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="keyVal"></param>
    /// <param name="Offset"></param>
    /// <returns></returns>
    private async Task SetVal(string keyName, object? keyVal, TimeSpan? Offset = null)
    {
        if (Offset == null) Offset = TimeSpan.FromMinutes("System:Setting:Login_OverMinutes".GetConfigInt());
        _memoryCache.Set(keyName, keyVal, new MemoryCacheEntryOptions().SetSlidingExpiration((TimeSpan)Offset));
    }

    /// <summary>
    /// 获取值，如果不存在则创建并返回
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="keyValFunc"></param>
    /// <param name="Offset"></param>
    /// <returns></returns>
    private object? GetOrCreateVal(string keyName, Func<object> keyValFunc, TimeSpan? Offset = null)
    {
        if (Offset == null) Offset = TimeSpan.FromMinutes("System:Setting:Login_OverMinutes".GetConfigInt());

        return _memoryCache.GetOrCreate(keyName, entry =>
        {
            entry.SlidingExpiration = Offset;
            var result = keyValFunc.Invoke();
            return result;
        });
    }

    #endregion

    #region 登录使用

    /// <summary>
    /// 获取登录token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetOrResetVerifyCode(string token)
    {
        SetVal(token, StaticTxtHelper.GetRandomStr(), TimeSpan.FromSeconds(1800));
        return token;
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<string> GetVerifyCode(string token)
    {
        return await GetVal<string>(token, "登录许可已过期，请刷新页面重新获取！");
    }

    /// <summary>
    /// 获取验证码图片
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<string> GetVerifyCodeImg(string token)
    {
        return (await GetVerifyCode(token)).toVerifyCodeImgMs().toBase64();
    }

    #endregion

    #region 一般缓存

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="tenantCode"></param>
    /// <param name="keyName"></param>
    /// <param name="keyVal"></param>
    /// <returns></returns>
    public async Task SetVal(string userCode, string keyName, object? keyVal, TimeSpan? Offset = null)
    {
        if (Offset == null) Offset = TimeSpan.FromMinutes("System:Setting:Login_OverMinutes".GetConfigInt());
        await SetVal($"{tenantCode}_{userCode}_{keyName}", keyVal, Offset);
    }

    /// <summary>
    /// 获取键值，获取不到返回null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tenantCode"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public async Task<T> GetKeyVal<T>(string userCode, string keyName) => await GetVal<T>($"{tenantCode}_{userCode}_{keyName}");

    /// <summary>
    /// 删除键值
    /// </summary>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public async Task RemoveKey(string userCode, string keyName) => await RemoveKey($"{tenantCode}_{userCode}_{keyName}");

    /// <summary>
    /// 获取键值，不存在则创建并返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tenantCode"></param>
    /// <param name="keyName"></param>
    /// <param name="keyValFunc"></param>
    /// <param name="refresh"></param>
    /// <returns></returns>
    public async Task<T> GetOrCreateKeyVal<T>(string userCode, string keyName, Func<object> keyValFunc, bool refresh = false)
    {
        var _relKeyName = $"{tenantCode}_{userCode}_{keyName}";
        if (refresh) await RemoveKey(_relKeyName);
        return GetOrCreateVal(_relKeyName, keyValFunc).Adapt<T>();
    }

    #endregion

    #region 系统缓存

    /// <summary>
    /// 记录系统缓存键名
    /// </summary>
    /// <param name="newKeyName"></param>
    /// <param name="tenantCode"></param>
    /// <returns></returns>
    public async Task Sys_RecordKeyName(string newKeyName)
    {
        List<string> _list = await GetVal<List<string>>($"{tenantCode}_SysParList");
        if (_list == null) _list = new List<string>();
        if (!_list.Contains(newKeyName)) _list.Add(newKeyName);
        SetVal($"{tenantCode}_SysParList", _list, TimeSpan.FromHours(1));
    }

    /// <summary>
    /// 删除所有已记录的系统缓存键值
    /// </summary>
    /// <param name="tenantCode"></param>
    /// <returns></returns>
    public async Task Sys_RemoveKeyVal(string tenantCode)
    {
        var _relKeyName = $"{tenantCode}_SysParList";
        var _list = await GetVal<List<string>>(_relKeyName);
        if (_list != null && _list.Any()) _list.ForEach(_memoryCache.Remove);
    }

    /// <summary>
    /// 获取系统缓存值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tenantCode"></param>
    /// <param name="KeyName"></param>
    /// <param name="keyValFunc"></param>
    /// <param name="refresh"></param>
    /// <returns></returns>
    public async Task<T> Sys_GetKeyVal<T>(string KeyName, Func<object> keyValFunc, bool refresh = false)
    {
        if (refresh) await Sys_RemoveKeyVal(tenantCode);
        var _relKeyName = $"{tenantCode}_{KeyName}_sysCache";
        await Sys_RecordKeyName(_relKeyName);
        return GetOrCreateVal(_relKeyName, keyValFunc).Adapt<T>();
    }

    #endregion
}

