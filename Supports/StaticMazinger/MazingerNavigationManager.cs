/// <summary>
/// 导航跳转接口
/// </summary>
public class MazingerNavigationManager
{
    public delegate void GlobalConfigChanged();
    //选择菜单
    public event GlobalConfigChanged? OnCurrentNavChanged;
    private NavDto? _currentNav;
    public NavDto? CurrentNav
    {
        get => _currentNav;
        set
        {
            _currentNav = value;
            OnCurrentNavChanged?.Invoke();
        }
    }

    private NavigationManager _navigationManager;

    public MazingerNavigationManager(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    /// <summary>
    /// 页面跳转
    /// </summary>
    /// <param name="href"></param>
    public async Task NavigateTo(string href)
    {
        _navigationManager.NavigateTo(href ?? "/");
    }

    /// <summary>
    /// 页面跳转
    /// </summary>
    /// <param name="href"></param>
    public async Task NavigateToByEvent(NavDto _target)
    {
        CurrentNav = _target;
        _navigationManager.NavigateTo(_target.url ?? "/");
    }

    /// <summary>
    /// 获取当前页面路径
    /// </summary>
    /// <returns></returns>
    public string GetUrl()
    {
        var baseUri = _navigationManager.BaseUri;
        var uri = _navigationManager.Uri;
        return uri.Replace(baseUri, "");

    }


}

