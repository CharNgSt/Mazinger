namespace Mazinger.Models.CodeFist;

/// <summary>
/// 角色权限表
/// </summary>
[SugarTable("SYS_DIC_PRIV_MODEL", TableDescription = "角色权限表")]
[SugarIndex("SYS_DIC_PRIV_MODEL_INDEX_DUTY", nameof(permissionDuty), OrderByType.Asc)]
public class DutyPermissionEntity
{
    /// <summary>
    /// 权限名称
    /// </summary>
    [SugarColumn(ColumnName = "MODEL_PRIV", IsPrimaryKey = true)]
    public string permissionName { get; set; }

    /// <summary>
    /// 所属职称
    /// </summary>
    [SugarColumn(ColumnName = "MODEL_DUTY", IsPrimaryKey = true)]
    public string permissionDuty { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "MODEL_MEMO",IsNullable = true)]
    public string permissionMemo { get; set; }

}
