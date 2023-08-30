
/// <summary>
/// LocalStorage读写接口
/// </summary>
public class MazingerLocalStorage
{
    private ILocalStorageService _localStorage;

    public MazingerLocalStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    /// <summary>
    /// 保存值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="_val"></param>
    /// <returns></returns>
    public async Task Set<T>(string key, object _val)
    {
        await _localStorage.SetItemAsync<T>(key, _val.Adapt<T>());
    }

    /// <summary>
    /// 获取值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<T> Get<T>(string key)
    {
        var _res = await _localStorage.GetItemAsync<T>(key);
        return _res;
    }

    /// <summary>
    /// 删除值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task Delete(string key)
    {
        await _localStorage.RemoveItemAsync(key);
    }

}

