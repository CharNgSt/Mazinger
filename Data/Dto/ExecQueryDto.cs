/// <summary>
/// 查询对象
/// </summary>
public class ExecQueryDto
{
    /// <summary>
    /// 查询名称
    /// </summary>
    public string queyrName { get; set; }
    /// <summary>
    /// 执行结果
    /// </summary>
    public bool queyrExec { get; set; }
    /// <summary>
    /// 查询类型
    /// </summary>
    public string queyrExecType { get; set; }
    /// <summary>
    /// 查询日期
    /// </summary>
    public DateOnly? queyrDay { get; set; }


    /// <summary>
    /// 页数
    /// </summary>
    public int page { get; set; } = 1;

    /// <summary>
    /// 每页大小
    /// </summary>
    public int pageSize { get; set; } = 25;
}
