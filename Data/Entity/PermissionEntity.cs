namespace Mazinger.Models.CodeFist;

/// <summary>
/// 用户权限表
/// </summary>
[SugarTable("SYS_DIC_PRIV", TableDescription = "用户权限表")]
public class UserPermissionEntity
{
    /// <summary>
    /// 用户账号
    /// </summary>
    [SugarColumn(ColumnName = "PRIV_USER", IsPrimaryKey = true, IsNullable = false)]
    public string permissionUser { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    [SugarColumn(ColumnName = "PRIV_MENU", IsPrimaryKey = true, IsNullable = false)]
    public string permissionName { get; set; }

    /// <summary>
    /// 权限类型，0 是来自角色，1 是自选添加
    /// </summary>
    [SugarColumn(ColumnName = "PRIV_LX",DefaultValue ="0",ColumnDescription = "权限类型，0 是来自角色、1 是自选添加")]
    public string permissionType { get; set; }

}
