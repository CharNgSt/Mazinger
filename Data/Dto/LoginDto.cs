public class LoginDto
{
    /// <summary>
    /// 登录账号
    /// </summary>
    [Required(ErrorMessage = "请输入您的账号")]
    public string _userCode { get; set; }

    /// <summary>
    /// 登录密码
    /// </summary>
    [Required(ErrorMessage = "请输入登陆密码")]
    public string _userPwd { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required(ErrorMessage = "请输入验证码")]
    public string _userVerifyCode { get; set; }

    /// <summary>
    /// 登录密钥
    /// </summary>
    public string defaultToken { get; set; }

    /// <summary>
    /// 加载中
    /// </summary>
    public bool loading { get; set; } = false;

}