namespace Mazinger.Models.CodeFist;

/// <summary>
/// 数据库租户信息表
/// </summary>
[SugarTable("SYS_DIC_DBCONNS",TableDescription = "数据库租户信息表")]
[SugarIndex("SYS_DIC_DBCONNS_INDEX_DB_TYPE", nameof(dbType), OrderByType.Asc)]
public class DbConnEntity
{
    /// <summary>
    /// 租户代码
    /// </summary>
    [SugarColumn(ColumnName = "DB_CODE", IsPrimaryKey = true)]
    public string dbCode { get; set; }
    /// <summary>
    /// 租户名
    /// </summary>
    [SugarColumn(ColumnName = "DB_NAME")]
    public string? dbName { get; set; }
    /// <summary>
    /// 数据库类型
    /// </summary>
    [SugarColumn(ColumnName = "DB_TYPE")]
    public string dbType { get; set; }
    /// <summary>
    /// 链接字符串
    /// </summary>
    [SugarColumn(ColumnName = "DB_CONN")]
    public string dbConn { get; set; }
    /// <summary>
    /// 创建用户姓名
    /// </summary>
    [SugarColumn(ColumnName = "DB_CREATEUSER")]
    public string dbCreateUser { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnName = "DB_CREATEDAY")]
    public DateTime dbCreateDay { get; set; }

}
