/// <summary>
/// 用户查询对象
/// </summary>
public class UserQueryDto
{
    /// <summary>
    /// 用户名称
    /// </summary>
    public string userName { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string userCode { get; set; }

    /// <summary>
    /// 部门代码
    /// </summary>
    public string deptCode { get; set; }

    /// <summary>
    /// 职位代码
    /// </summary>
    public string dutyCode { get; set; }

    /// <summary>
    /// 职称代码
    /// </summary>
    public string techCode { get; set; }

    /// <summary>
    /// 单位代码
    /// </summary>
    public string comCode { get; set; }

    /// <summary>
    /// 页数
    /// </summary>
    public int page { get; set; } = 1;

    /// <summary>
    /// 每页大小
    /// </summary>
    public int pageSize { get; set; } = 25;
}
