namespace Mazinger.Models.CodeFist;

/// <summary>
/// 查询函数权限
/// </summary>
[SugarTable("SYS_DIC_QUERYFUNCTION_PRIV", TableDescription = "查询函数权限表")]
public class QueryFunctionPrivEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnName = "PRIV_GUID", IsPrimaryKey = true)]
    public string privGuid { get; set; }
    /// <summary>
    /// 查询guid
    /// </summary>
    [SugarColumn(ColumnName = "PRIV_QGUID")]
    public string privQueryGuid { get; set; }
    /// <summary>
    /// 用户账号
    /// </summary>
    [SugarColumn(ColumnName = "PRIV_USER")]
    public string privUserCode { get; set; }

}

