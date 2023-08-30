namespace Mazinger.Models.CodeFist;

/// <summary>
/// 查询参数
/// </summary>
[SugarTable("SYS_DIC_QUERYFUNCTION_PARAMETER", TableDescription = "查询函数参数表")]
public class QueryFunctionParameterEntity
{

    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnName = "PARAMETER_GUID", IsPrimaryKey = true)]
    public string parameterGuid { get; set; }

    /// <summary>
    /// 父ID
    /// </summary>
    [SugarColumn(ColumnName = "PARAMETER_PGUID")]
    public string parameterQueryGuid { get; set; }

    /// <summary>
    /// 参数名
    /// </summary>
    [SugarColumn(ColumnName = "PARAMETER_NAME")]
    public string parameterName { get; set; }

    /// <summary>
    /// 参数类型 （int、txt、date）
    /// </summary>
    [SugarColumn(ColumnName = "PARAMETER_TYPE")]
    public string parameterType { get; set; }

    /// <summary>
    /// date类型加一天标识，0不加、1加一天
    /// </summary>
    [SugarColumn(ColumnName = "PARAMETER_FLAG")]
    public int parameterFlag { get; set; } = 0;


}

