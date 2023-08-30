namespace Mazinger.Models.CodeFist;

/// <summary>
/// 单位信息表
/// </summary>
[SugarTable("SYS_DIC_COMPANY",TableDescription = "单位信息表")]
public class CompanyEntity
{
    /// <summary>
    /// 单位代码
    /// </summary>
    [SugarColumn(ColumnName = "COMPANY_CODE",ColumnDescription = "单位代码", IsPrimaryKey = true)]
    public string comCode { get; set; }

    /// <summary>
    /// 单位统一信用代码
    /// </summary>
    [SugarColumn(ColumnName = "COMPANY_ID", ColumnDescription = "统一信用代码", IsNullable = true)]
    public string comId { get; set; }

    /// <summary>
    /// 单位名称
    /// </summary>
    [SugarColumn(ColumnName = "COMPANY_NAME", ColumnDescription = "单位名称")]
    public string comName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "COMPANY_PHONE", ColumnDescription = "联系电话", IsNullable = true)]
    public string comPhone { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [SugarColumn(ColumnName = "COMPANY_ADDR", ColumnDescription = "地址", IsNullable = true)]
    public string comAddr { get; set; }

    /// <summary>
    /// 绑定数据库租户
    /// </summary>
    [SugarColumn(ColumnName = "COMPANY_DBCODE", ColumnDescription = "绑定数据库租户", IsNullable = true)]
    public string comDbCode { get; set; }

}
