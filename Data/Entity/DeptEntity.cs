namespace Mazinger.Models.CodeFist;

/// <summary>
/// 部门信息表
/// </summary>
[SugarTable("SYS_DIC_DEPT", TableDescription = "部门信息表")]
[SugarIndex("SYS_DIC_DEPT_INDEX_COMPANY", nameof(deptCompany), OrderByType.Asc)]
public class DeptEntity
{
    /// <summary>
    /// 部门代码
    /// </summary>
    [SugarColumn(ColumnName = "DEPT_CODE", ColumnDescription = "部门代码", IsPrimaryKey = true)]
    public string deptCode { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [SugarColumn(ColumnName = "DEPT_NAME", ColumnDescription = "名称")]
    public string deptName { get; set; }

    /// <summary>
    /// 部门所属单位代码
    /// </summary>
    [SugarColumn(ColumnName = "DEPT_COMPANY", ColumnDescription = "所属单位", IsNullable = true)]
    public string deptCompany { get; set; }

    /// <summary>
    /// 部门地址
    /// </summary>
    [SugarColumn(ColumnName = "DEPT_ADDR", ColumnDescription = "部门地址", IsNullable = true)]
    public string deptAddr { get; set; }

    /// <summary>
    /// 部门备注
    /// </summary>
    [SugarColumn(ColumnName = "DEPT_MEMO", ColumnDescription = "备注", IsNullable = true)]
    public string deptMemo { get; set; }
}
