/// <summary>
/// 参数类
/// </summary>
public class ParameterDto
{
    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public DateOnly? DateValue { get; set; }
    /// <summary>
    /// 前端使用flag
    /// </summary>
    public bool menuFlag { get; set; }

}