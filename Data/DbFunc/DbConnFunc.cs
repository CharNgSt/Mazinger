namespace Mazinger.DataAccess.DbConn;

/// <summary>
/// 数据库租户
/// </summary>
public static class DbConnFunc
{
    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <returns></returns>
    public static List<DbConnEntity> GetConns(this SqlSugarProvider _sql) => _sql.Queryable<DbConnEntity>().OrderBy(_a => _a.dbCreateDay).ToList();

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task Save(this SqlSugarProvider _sql, DbConnEntity _dto,bool _new = true)
    {
        var _exists = _sql.Queryable<DbConnEntity>().Where(_a => _a.dbCode == _dto.dbCode).Any();
        if (_new && _exists) throw new Exception($"数据库:{_dto.dbCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"数据库:{_dto.dbCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;
            if (!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand() > 0;
            else _exec = _sql.Updateable(_dto).IgnoreColumns(a => a.dbCreateDay).ExecuteCommand() > 0;
            if (!_exec) throw new Exception($"{(_new ? "新增" : "修改")}数据库失败");
        });

    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dbCode"></param>
    /// <returns></returns>
    public static async Task Delete(this SqlSugarProvider _sql, string _dbCode)
    {
        _sql.Deleteable<DbConnEntity>().Where(_a=>_a.dbCode == _dbCode).ExecuteCommand();
    }

    /// <summary>
    /// 获取数据库链接
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <returns></returns>
    public static List<DbConnDto> GetConnSelect(this SqlSugarProvider _sql) 
        => 
        _sql.Queryable<DbConnEntity>().OrderBy(_a => _a.dbCreateDay).Select(_a => new DbConnDto
        {
            dbCode = _a.dbCode,
            dbName = _a.dbName,
            dbType = _a.dbType
        }).ToList();
}

