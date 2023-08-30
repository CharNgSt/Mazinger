using System.Diagnostics.CodeAnalysis;
/// <summary>
/// 保存用户对象
/// </summary>
public class UserSaveDto
{
    /// <summary>
    /// 用户账号
    /// </summary>
    [Required(ErrorMessage ="账号不能为空"),DisallowNull]
    public string userCode { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    [Required(ErrorMessage = "姓名不能为空"), DisallowNull]
    public string userName { get; set; }

    /// <summary>
    /// 用户英文名
    /// </summary>
    public string userEname { get; set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    public string userPwd { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public string userSex { get; set; }

    /// <summary>
    /// 联系电话(手机号码)
    /// </summary>
    public string userMoblie { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string userEmail { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? userBirthday { get; set; }

    /// <summary>
    /// 证件号码
    /// </summary>
    public string userIdcard { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string companyName { get; set; }

    /// <summary>
    /// 公司绑定租户
    /// </summary>
    public string companyDbCode { get; set; }

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
    /// 公司代码
    /// </summary>
    public string userComCode { get; set; }

    /// <summary>
    /// 部门代码
    /// </summary>
    public string userDeptCode { get; set; }

    /// <summary>
    /// 职务代码
    /// </summary>
    public string userDutyCode { get; set; }

    /// <summary>
    /// 职称代码
    /// </summary>
    public string userTechCode { get; set; }

    /// <summary>
    /// 用户主页
    /// </summary>
    public string mainPage { get; set; }

    /// <summary>
    /// SSO登录KEY
    /// </summary>
    public string? userSsokey { get; set; }

    /// <summary>
    /// 密码过期时间
    /// </summary>
    public DateTime? userPwdOvertime { get; set; }

    /// <summary>
    /// 失败次数
    /// </summary>
    public decimal? userFailedtime { get; set; } = 0M;

    /// <summary>
    /// 冻结时间
    /// </summary>
    public DateTime? userFreezetime { get; set; }

    /// <summary>
    /// 最后一次登录时间
    /// </summary>
    public DateTime? userLastLogintime { get; set; }
}

