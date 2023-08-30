namespace Mazinger.Models.CodeFist;

/// <summary>
/// 查询函数
/// </summary>
[SugarTable("SYS_DIC_QUERYFUNCTION", TableDescription = "查询函数表")]
public class QueryFunctionEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_GUID", IsPrimaryKey = true)]
    public string queryGuid { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_NAME")]
    public string queryName { get; set; }

    /// <summary>
    /// 类型（query 直接查询、file 生成csv文件）
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_TYPE")]
    public string queryType { get; set; }

    /// <summary>
    /// 查询数据库（对应多个数据库用 ',' 分割）
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_DB")]
    public string queryDb { get; set; }

    /// <summary>
    /// 查询语句
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_SQL")]
    public string querySql { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_CREATEDAY")]
    public DateTime qeuryCreateDay { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建用户
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_CREATEUSER")]
    public string queryCreateUser { get; set; }

    /// <summary>
    /// 首次执行耗时（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_FIRST_EXEC_MIN", DefaultValue = "0")]
    public int queryExecMinutes { get; set; } = 0;

    /// <summary>
    /// 所有人都可以使用
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_ALLALLOW")]
    public bool queryAllAllow { get; set; } = false;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "QUERY_MEMO")]
    public string queryMemo { get; set; }
}

