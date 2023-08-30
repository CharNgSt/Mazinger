/// <summary>
/// 登录结果
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public bool login { get; set; } = false;

    /// <summary>
    /// 文本消息
    /// </summary>
    public string msg { get; set; }

    /// <summary>
    /// 修改密码用token
    /// </summary>
    public string licenseToken { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string userCode { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string userName { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public string userSex { get; set; }

    /// <summary>
    /// 租户代码
    /// </summary>
    public string tenantCode { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string tenantName { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string companyName { get; set; }

    /// <summary>
    /// 绑定数据库租户
    /// </summary>
    public string comDbCode { get; set; }
    
    /// <summary>
    /// 部门名称
    /// </summary>
    public string deptName { get; set; }

    /// <summary>
    /// 职务名称
    /// </summary>
    public string dutyName { get; set; }

    /// <summary>
    /// 职称名称
    /// </summary>
    public string techName { get; set; }

    /// <summary>
    /// 用户主页
    /// </summary>
    public string mainPage { get; set; }

    /// <summary>
    /// jwt token
    /// </summary>
    public string authorizationToken { get; set; }

}