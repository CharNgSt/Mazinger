namespace Mazinger.Models.CodeFist;

/// <summary>
/// 功能菜单表
/// </summary>
[SugarTable("SYS_DIC_WEBMODELLIST", TableDescription = "功能菜单表")]
[SugarIndex("SYS_DIC_WEBMODELLIST_INDEX_WML_PURVIEW", nameof(Wml_purview), OrderByType.Asc)]
public class ModuleUrlEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnName = "WML_GUID", IsPrimaryKey = true)]
    public string Wml_guid { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// 编号
    /// </summary>
    [SugarColumn(ColumnName = "WML_ID")]
    public decimal Wml_id { get; set; }
    /// <summary>
    /// 菜单名
    /// </summary>
    [SugarColumn(ColumnName =  "WML_NAME")]
    public string Wml_name { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    [SugarColumn(ColumnName = "WML_ICON")]
    public string Wml_icon { get; set; }
    
    /// <summary>
    /// 父编号
    /// </summary>
    [SugarColumn(ColumnName =  "WML_PID", DefaultValue = "0")]
    public decimal? Wml_pid { get; set; } = 0M;
    /// <summary>
    /// 对应权限
    /// </summary>
    [SugarColumn(ColumnName =  "WML_PURVIEW",IsNullable = true)]
    public string Wml_purview { get; set; }

    /// <summary>
    /// 0是刷新页面 1是打开新页面（Vue版全部为0）
    /// </summary>
    [SugarColumn(ColumnName =  "WML_TARGET",DefaultValue = "0")]
    public decimal? Wml_target { get; set; } = 0M;

    /// <summary>
    /// Url地址
    /// </summary>
    [SugarColumn(ColumnName =  "WML_URL", IsNullable = true)]
    public string Wml_url { get; set; }
}
