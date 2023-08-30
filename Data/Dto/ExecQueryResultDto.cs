/// <summary>
/// 查询结果对象
/// </summary>
public class ExecQueryResultDto
{
    /// <summary>
    /// 列表
    /// </summary>
    public List<ExecDto> rows { get; set; }

    /// <summary>
    /// 总条数
    /// </summary>
    public int totalCount { get; set; }
}
