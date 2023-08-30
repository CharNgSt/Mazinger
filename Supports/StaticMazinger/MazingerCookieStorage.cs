
/// <summary>
/// cookie操作接口
/// </summary>
public class MazingerCookieStorage
{
    #region 登录用户

    public static string CookieKeyLoginUserCode { get; set; } = "GlobalConfig_LoginUserCode";
    private string _LoginUserCode;
    /// <summary>
    /// 登录账号
    /// </summary>
    public string LoginUserCode
    {
        get => _LoginUserCode;
        set
        {
            _LoginUserCode = value;
            _cookieStorage?.SetItemAsync(CookieKeyLoginUserCode, value);
        }
    }

    public static string CookieKeyLoginUserName { get; set; } = "GlobalConfig_LoginUserName";
    private string _LoginUserName;
    /// <summary>
    /// 登录姓名
    /// </summary>
    public string LoginUserName
    {
        get => _LoginUserName;
        set
        {
            _LoginUserName = value;
            _cookieStorage?.SetItemAsync(CookieKeyLoginUserName, value);
        }
    }

    public static string CookieKeyLoginAuthToken { get; set; } = "GlobalConfig_LoginAuthToken";
    private string _LoginAuthToken;
    /// <summary>
    /// 鉴权token
    /// </summary>
    public string LoginAuthToken
    {
        get => _LoginAuthToken;
        set
        {
            _LoginAuthToken = value;
            _cookieStorage?.SetItemAsync(CookieKeyLoginAuthToken, value);
        }
    }

    /// <summary>
    /// 登录租户
    /// </summary>
    public static string CookieKeyTenantCode { get; set; } = "GlobalConfig_TenantCode";
    private string _TenantCode;
    /// <summary>
    /// 登录租户
    /// </summary>
    public string TenantCode
    {
        get => _TenantCode;
        set
        {
            _TenantCode = value;
            _cookieStorage?.SetItemAsync(CookieKeyTenantCode, value);
        }
    }


    /// <summary>
    /// 单位绑定租户
    /// </summary>
    public static string CookieKeyCompanyTenantbCode { get; set; } = "GlobalConfig_CompanyTenantCode";
    private string _CompanyTenantCode;
    /// <summary>
    /// 单位绑定租户
    /// </summary>
    public string CompanyTenantCode
    {
        get => _CompanyTenantCode;
        set
        {
            _CompanyTenantCode = value;
            _cookieStorage?.SetItemAsync(CookieKeyCompanyTenantbCode, value);
        }
    }

    #endregion

    private CookieStorage? _cookieStorage;
    public MazingerCookieStorage(CookieStorage cookieStorage, IHttpContextAccessor httpContextAccessor)
    {
        _cookieStorage = cookieStorage;
        if (httpContextAccessor.HttpContext is not null)
        {
            var cookies = httpContextAccessor.HttpContext.Request.Cookies;
            _LoginUserCode = Decode(cookies[CookieKeyLoginUserCode]);
            _LoginUserName = Decode(cookies[CookieKeyLoginUserName]);
            _LoginAuthToken = Decode(cookies[CookieKeyLoginAuthToken]);
            _TenantCode = Decode(cookies[CookieKeyTenantCode]);
            _CompanyTenantCode = Decode(cookies[CookieKeyCompanyTenantbCode]);
        }
    }

    private string Decode(string _val) => HttpUtility.UrlDecode(_val);

    public bool CookieIsExists()
    {
        return !string.IsNullOrEmpty(_LoginUserCode) && !string.IsNullOrEmpty(_LoginUserName) && !string.IsNullOrEmpty(_LoginAuthToken);


    }

}

