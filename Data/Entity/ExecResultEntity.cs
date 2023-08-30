namespace Mazinger.Models.CodeFist;

/// <summary>
/// 查询结果表
/// </summary>
[SugarTable("SYS_DIC_EXEC_RESULT", TableDescription = "查询结果表")]
public class ExecResultEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_GUID", IsPrimaryKey = true)]
    public string execGuid { get; set; }

    /// <summary>
    /// 查询函数主键
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_QUERYGUID")]
    public string execQueryGuid { get; set; }

    /// <summary>
    /// 查询函数主键
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_CONDITION")]
    public string execCondition { get; set; }

    /// <summary>
    /// 查询函数主键
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_DBCODE")]
    public string execDbCode { get; set; }

    /// <summary>
    /// 创建用户
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_CREATEUSER")]
    public string execCreateUser { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_CREATEDAY")]
    public DateTime execCreateDay { get; set; } = DateTime.Now;

    /// <summary>
    /// 完成日期
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_FINISHDAY",IsNullable = true)]
    public DateTime execFinishDay { get; set; }

    /// <summary>
    /// 执行消息
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_MSG", IsNullable = true)]
    public string execMsg { get; set; }

    /// <summary>
    /// 执行结果
    /// </summary>
    [SugarColumn(ColumnName = "EXEC_RESULT")]
    public bool execResult { get; set; }



}

