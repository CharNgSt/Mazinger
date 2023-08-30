namespace Mazinger.DataAccess.Permission;

/// <summary>
/// 权限数据操作
/// </summary>
public static class PermissionFunc
{

    /// <summary>
    /// 获取系统全部权限列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <returns></returns>
    public static List<string> Permission_Get(this SqlSugarProvider _sql)
    {
        return  _sql.Queryable<ModuleUrlEntity>()
            .Where(_a => !string.IsNullOrEmpty(_a.Wml_purview))
            .GroupBy(_a => _a.Wml_purview)
            .Select(_a => new { indexNum = SqlFunc.AggregateMax(_a.Wml_id), permissionName = _a.Wml_purview })
            .ToList()
            .OrderBy(_a => _a.indexNum)
            .ToList()
            .ConvertAll(_a => _a.permissionName);

    }

    /// <summary>
    /// 获取用户所有权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <returns></returns>
    public static List<UserPermissionEntity> Permission_GetUserPermission(this SqlSugarProvider _sql,string _userCode)
    {
        return _sql.Queryable<UserPermissionEntity>()
            .Where(_a => _a.permissionUser == _userCode)
            .OrderBy(_a => _a.permissionName)
            .ToList();
    }

    /// <summary>
    /// 保存用户权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <param name="_userPriv"></param>
    /// <param name="type">权限类型，0 是来自角色，1 是自选添加</param>
    /// <returns></returns>
    public static async Task Permission_SaveUserPermission(this SqlSugarProvider _sql, string _userCode,List<string>? _userPriv,string type = "1")
    {
        await _sql.GoTransaction(() => {
            _sql.Deleteable<UserPermissionEntity>().Where(_a => _a.permissionUser == _userCode && _a.permissionType == type).ExecuteCommand();
            if (_userPriv != null && _userPriv.Any())
            {
                //删除自定义权限与职务权限重复的项
                if (type == "0") _sql.Deleteable<UserPermissionEntity>().Where(_a => _a.permissionUser == _userCode && _userPriv.Contains(_a.permissionName) && _a.permissionType == "1").ExecuteCommand();

                _sql.Insertable(_userPriv.ConvertAll(_a => new UserPermissionEntity { permissionName = _a, permissionUser = _userCode, permissionType = type })).ExecuteCommand();
            }
        });
    }


    /// <summary>
    /// 获取职务权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dutyCode"></param>
    /// <returns></returns>
    public static List<string> Permission_GetDutyPermission(this SqlSugarProvider _sql, string _dutyCode) => _sql.Queryable<DutyPermissionEntity>().Where(_a => _a.permissionDuty == _dutyCode).OrderBy(_a => _a.permissionName).Select(_a => _a.permissionName).ToList();

    /// <summary>
    /// 保存职务权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dutyCode"></param>
    /// <param name="_priv"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task Permission_SaveDutyPermission(this SqlSugarProvider _sql, string _dutyCode, List<string>? _priv)
    {
        await _sql.GoTransaction(() => {
            _sql.Deleteable<DutyPermissionEntity>().Where(_a => _a.permissionDuty == _dutyCode).ExecuteCommand();
            if (_priv != null && _priv.Any())
            {
                if (_sql.Insertable(_priv.ConvertAll(_a => new DutyPermissionEntity { permissionName = _a, permissionDuty = _dutyCode })).ExecuteCommand() <= 0)
                    throw new Exception("保存权限失败！");
            }

            _sql.Queryable<UserEntity>().Where(_a => _a.userDutyCode == _dutyCode).Select(_a => _a.userCode).ToList()
               .ForEach(u => {
                   _sql.Permission_SaveUserPermission(u, _priv, "0");
               });


        });

    }

}
