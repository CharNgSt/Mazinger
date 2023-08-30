namespace Mazinger.Models.CodeFist;

/// <summary>
/// 用户信息表
/// </summary>
[SugarTable("SYS_DIC_USER",TableDescription = "用户信息表")]
[SugarIndex("SYS_DIC_USER_INDEX_DEPT", nameof(userDeptCode), OrderByType.Asc)]
[SugarIndex("SYS_DIC_USER_INDEX_DUTY", nameof(userDutyCode), OrderByType.Asc)]
[SugarIndex("SYS_DIC_USER_INDEX_TECH", nameof(userTechCode), OrderByType.Asc)]
public class UserEntity
{
    /// <summary>
    /// 用户账号
    /// </summary>
    [SugarColumn(ColumnName = "USER_CODE", ColumnDescription = "账号", IsPrimaryKey = true)]
    public string userCode { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    [SugarColumn(ColumnName = "USER_NAME", ColumnDescription = "姓名")]
    public string? userName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [SugarColumn(ColumnName = "USER_PWD", ColumnDescription = "密码")]
    public string userPwd { get; set; }
    /// <summary>
    /// 英文名
    /// </summary>
    [SugarColumn(ColumnName = "USER_ENAME", ColumnDescription = "英文名", IsNullable = true)]
    public string? userEname { get; set; }

    /// <summary>
    /// 用户主页
    /// </summary>
    [SugarColumn(ColumnName = "USER_MAINPAGE", ColumnDescription = "首页", IsNullable = true)]
    public string? userMainPage { get; set; }

    /// <summary>
    /// 性别（男/女）
    /// </summary>
    [SugarColumn(ColumnName = "USER_SEX", ColumnDescription = "性别", IsNullable = true)]
    public string? userSex { get; set; } = "男";

    /// <summary>
    /// 证件号码
    /// </summary>
    [SugarColumn(ColumnName = "USER_IDCARD", ColumnDescription = "证件号码", IsNullable = true)]
    public string? userIdcard { get; set; }

    /// <summary>
    /// 联系电话(手机号码)
    /// </summary>
    [SugarColumn(ColumnName = "USER_PHONE", ColumnDescription = "联系电话", IsNullable = true)]
    public string? userMoblie { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    [SugarColumn(ColumnName = "USER_BIRTHDAY", ColumnDescription = "生日", IsNullable = true)]
    public DateTime? userBirthday { get; set; }

    /// <summary>
    /// 单位代码
    /// </summary>
    [SugarColumn(ColumnName = "USER_COMPANY", ColumnDescription = "单位", IsNullable = true)]
    public string? userComCode { get; set; }

    /// <summary>
    /// 部门代码
    /// </summary>
    [SugarColumn(ColumnName = "USER_DEPT", ColumnDescription = "部门", IsNullable = true)]
    public string? userDeptCode { get; set; }

    /// <summary>
    /// 职位代码
    /// </summary>
    [SugarColumn(ColumnName = "USER_DUTY", ColumnDescription = "职位", IsNullable = true)]
    public string? userDutyCode { get; set; }

    /// <summary>
    /// 职称代码
    /// </summary>
    [SugarColumn(ColumnName = "USER_TECHNICAL", ColumnDescription = "职称", IsNullable = true)]
    public string? userTechCode { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [SugarColumn(ColumnName = "USER_EMAIL", ColumnDescription = "E-Mail", IsNullable = true)]
    public string? userEmail { get; set; }

    /// <summary>
    /// SSO登录KEY
    /// </summary>
    [SugarColumn(ColumnName = "USER_SSOKEY", ColumnDescription = "SSO TOKEN", IsNullable = true)]
    public string? userSsokey { get; set; }

    /// <summary>
    /// 失败次数
    /// </summary>
    [SugarColumn(ColumnName= "USER_FAILEDTIME", IsNullable = true)]
    public decimal? userFailedtime { get; set; } = 0M;

    /// <summary>
    /// 冻结时间
    /// </summary>
    [SugarColumn(ColumnName = "USER_FREEZETIME",IsNullable = true)]
    public DateTime? userFreezetime { get; set; }

    /// <summary>
    /// 密码过期时间
    /// </summary>
    [SugarColumn(ColumnName = "USER_PWD_OVERTIME", IsNullable = true)]
    public DateTime? userPwdOvertime { get; set; }

    /// <summary>
    /// 最后一次登录时间
    /// </summary>
    [SugarColumn(ColumnName = "USER_LAST_LOGINTIME", IsNullable = true)]
    public DateTime? userLastLogintime { get; set; }

}
