/// <summary>
/// 用户查询结果
/// </summary>
public class UserQueryResultDto
{
    /// <summary>
    /// 列表
    /// </summary>
    public List<UserSaveDto> rows { get; set; }

    /// <summary>
    /// 总条数
    /// </summary>
    public int totalCount { get; set; }
}
