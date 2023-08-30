namespace Mazinger.Models.CodeFist;
/// <summary>
/// 职称表
/// </summary>
[SugarTable("SYS_DIC_TECH")]
public class TechEntity
{
    /// <summary>
    /// 职称代码
    /// </summary>
    [SugarColumn(ColumnName = "TECH_NO", ColumnDescription = "职称代码", IsPrimaryKey = true)]
    public string techCode { get; set; }

    /// <summary>
    /// 职称名称
    /// </summary>
    [SugarColumn(ColumnName = "TECH_NAME", ColumnDescription = "名称")]
    public string techName { get; set; }

    /// <summary>
    /// 职称所属部门
    /// </summary>
    [SugarColumn(ColumnName = "TECH_DEPT", ColumnDescription = "所属部门", IsNullable = true)]
    public string techDeptCode { get; set; }
}
