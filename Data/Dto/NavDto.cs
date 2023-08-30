using Newtonsoft.Json;
/// <summary>
/// 菜单列表
/// </summary>
public class NavDto
{
    /// <summary>
    /// 菜单名
    /// </summary>
    [AdaptMember("Wml_id")]
    public decimal id { get; set; }

    /// <summary>
    /// 父编号
    /// </summary>
    [AdaptMember("Wml_pid")]
    public decimal? pid { get; set; } = 0M;

    /// <summary>
    /// 菜单名
    /// </summary>
    [AdaptMember("Wml_name")]
    public string title { get; set; }

    /// <summary>
    /// 父菜单名
    /// </summary>
    public string parentTitle { get; set; }

    /// <summary>
    /// 图标名
    /// </summary>
    [AdaptMember("Wml_icon")]
    public string icon { get { return string.IsNullOrEmpty(_icon) ? "mdi-microsoft-xbox-controller-menu" : _icon; } set { _icon = value; } }
    private string _icon { get; set; }

    /// <summary>
    /// 0是刷新页面 1是打开新页面
    /// </summary>
    [AdaptMember("Wml_target")]
    public int target { get; set; } = 0;

    /// <summary>
    /// Url地址
    /// </summary>
    [AdaptMember("Wml_url")]
    public string url { get; set; }

    /// <summary>
    /// 选中
    /// </summary>
    public bool active { get; set; } = false;

    /// <summary>
    /// 下级列表
    /// </summary>
    public List<NavDto> child { get; set; } = new List<NavDto> { };
}
