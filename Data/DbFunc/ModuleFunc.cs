using Mazinger.DataAccess.Permission;
using Mazinger.DataAccess.User;
using Mazinger.Shared;

namespace Mazinger.DataAccess.Module;

/// <summary>
/// 模块
/// </summary>
public static class ModuleFunc
{
    /// <summary>
    /// 获取用户菜单
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <returns></returns>
    public static List<NavDto> Module_GetUserPrivList(this SqlSugarProvider _sql, string _userCode)
    {
        var _navMenu = new List<NavDto> { };
        var _userObj = _sql.User_GetEntity(_userCode);
        _navMenu.Add(new NavDto { title = "主页", icon = "mdi-home-circle", url = _userObj != null && !string.IsNullOrEmpty(_userObj.userMainPage) ? _userObj.userMainPage : "/", target = 0 });
        _navMenu.AddRange(_sql.Module_GetNavList(_sql.Permission_GetUserPermission(_userCode).ToList().ConvertAll(_a => _a.permissionName)));
        return _navMenu;
    }

    /// <summary>
    /// 根据权限获取菜单列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userPriv"></param>
    /// <returns></returns>
    public static List<NavDto> Module_GetNavList(this SqlSugarProvider _sql, List<string>? _userPriv)
    {
        if (_userPriv == null) _userPriv = new List<string>();
        var _allList = _sql.Queryable<ModuleUrlEntity>()
                            .Where(_a => _a.Wml_purview == null || _userPriv.Contains(_a.Wml_purview)).ToList();
        var config = TypeAdapterConfig.GlobalSettings.Clone();
        config.ForType<ModuleUrlEntity, NavDto>().AfterMapping((src, dest) => { dest.child = _allList.Where(_a => _a.Wml_pid == src.Wml_id).OrderBy(_a=>_a.Wml_pid).ToList().ConvertAll(_a => _a.Adapt<NavDto>()); });
        return _allList.Where(_a => _a.Wml_pid == 0).OrderBy(_a => _a.Wml_id).ToList().ConvertAll(_a => _a.Adapt<NavDto>(config));
    }

    /// <summary>
    /// 获取链接列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <returns></returns>
    public static List<ModuleUrlEntity> Module_GetModules(this SqlSugarProvider _sql,string _q_name ="" ,string _q_priv ="") 
        => _sql
              .Queryable<ModuleUrlEntity>()
              .WhereIF(!string.IsNullOrEmpty(_q_name),_a=>_a.Wml_name.Contains(_q_name))
              .WhereIF(!string.IsNullOrEmpty(_q_priv), _a => _a.Wml_purview.Contains(_q_priv) || string.IsNullOrEmpty(_a.Wml_purview))
              .OrderBy(_a => _a.Wml_id).ToList();

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_wid"></param>
    /// <returns></returns>
    public static ModuleUrlEntity Module_GetModuleById(this SqlSugarProvider _sql, decimal? _wid) => _sql.Queryable<ModuleUrlEntity>().Where(_a=>_a.Wml_id == _wid).First();

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task Module_Save(this SqlSugarProvider _sql, ModuleUrlEntity _dto)
    {
        await _sql.GoTransaction(() => {
            var _exec = false;
            var _exists = true;

            if (_dto.Wml_id == 0)
            {
                _exists = false;
                var newid = _sql.Queryable<ModuleUrlEntity>()
                                .Where( _a => _a.Wml_pid == _dto.Wml_pid)
                                .Max(_a => _a.Wml_id);
                if (newid == 0) newid = (decimal)_dto.Wml_pid;                
                newid += _dto.Wml_pid > 0 ? 0.1M : 1;
                while (_sql.Queryable<ModuleUrlEntity>().Where(_a=>_a.Wml_id == newid).Any())
                {
                    newid += _dto.Wml_pid > 0 ? 0.1M : 1;
                }
                _dto.Wml_id = newid;
            }

            if (!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand() > 0;
            else _exec = _sql.Updateable(_dto).IgnoreColumns(_a=>_a.Wml_pid).ExecuteCommand() > 0;

            if (!_exec) throw new Exception($"{(_exists?"新增":"修改")}菜单信息失败");
        });

    
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static async Task Module_Delete(this SqlSugarProvider _sql, decimal _id)
    {
        if (_sql.Queryable<ModuleUrlEntity>().Where(_a => _a.Wml_pid == _id).Any()) throw new Exception("删除的菜单存在子项，请先删除子菜单");
        await _sql.Deleteable<ModuleUrlEntity>().Where(_a => _a.Wml_id == _id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_id"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task Module_Move(this SqlSugarProvider _sql, decimal _id,string _type ="up")
    {
        var source = _sql.Queryable<ModuleUrlEntity>().Where(_a => _a.Wml_id == _id).First();
        var targetList = _sql.Queryable<ModuleUrlEntity>().Where(_a => _a.Wml_pid == source.Wml_pid).OrderBy(_a=>_a.Wml_id).ToList();
        int sourceIndex = 0;
        for (int _index = 0; _index < targetList.Count(); _index++)
        {
            if (targetList[_index].Wml_id == source.Wml_id)
            {
                sourceIndex = _index;
                break;
            }
        }

        var targetIndex = 0;
        if (_type == "up" && sourceIndex - 1 >= 0)
        {
            targetIndex = sourceIndex - 1;
        }
        else if (_type == "down" && sourceIndex + 1 < targetList.Count()) {
            targetIndex = sourceIndex + 1;
        }
        else throw new Exception($"对象已经{(_type == "up"?"第":"最后")}一位，无法{(_type == "up" ? "上" : "下")}移");

        var target = targetList[targetIndex];

        var sourceId = source.Wml_id;
        var sourceTmpId = source.Wml_id + 10000;

        var targetId = target.Wml_id;
        var targetTmpId = target.Wml_id + 20000;

        await _sql.GoTransaction(() => {

            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_id, sourceTmpId).Where(_a => _a.Wml_id == sourceId).ExecuteCommand();
            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_pid, sourceTmpId).Where(_a => _a.Wml_pid == sourceId).ExecuteCommand();

            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_id, targetTmpId).Where(_a => _a.Wml_id == targetId).ExecuteCommand();
            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_pid, targetTmpId).Where(_a => _a.Wml_pid == targetId).ExecuteCommand();

            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_id, targetId).Where(_a => _a.Wml_id == sourceTmpId).ExecuteCommand();
            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_pid, targetId).Where(_a => _a.Wml_pid == sourceTmpId).ExecuteCommand();

            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_id, sourceId).Where(_a => _a.Wml_id == targetTmpId).ExecuteCommand();
            _sql.Updateable<ModuleUrlEntity>().SetColumns(_a => _a.Wml_pid, sourceId).Where(_a => _a.Wml_pid == targetTmpId).ExecuteCommand();


        });


    }

}

