namespace Mazinger.Models.CodeFist;

/// <summary>
/// 职务信息表
/// </summary>
[SugarTable("SYS_DIC_DUTY")]
public class DutyEntity
{
    /// <summary>
    /// 职务代码
    /// </summary>
    [SugarColumn(ColumnName = "DUTY_NO", ColumnDescription = "代码", IsPrimaryKey = true)]
    public string dutyCode { get; set; }

    /// <summary>
    /// 职务名称
    /// </summary>
    [SugarColumn(ColumnName = "DUTY_NAME", ColumnDescription = "名称")]
    public string dutyName { get; set; }

    /// <summary>
    /// 职务所属部门
    /// </summary>
    [SugarColumn(ColumnName = "DUTY_DEPT", ColumnDescription = "所属部门", IsNullable = true)]
    public string dutyDeptCode { get; set; }

    /// <summary>
    /// 职务备注
    /// </summary>
    [SugarColumn(ColumnName = "DUTY_MEMO", ColumnDescription = "备注", IsNullable = true)]
    public string dutyMemo { get; set; }
}
