using Mazinger.DataAccess.Permission;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Mazinger.DataAccess.Base;

/// <summary>
/// 基础表管理
/// </summary>
public static class BaseDataFunc
{
    #region 单位管理

    /// <summary>
    /// 获取单位列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <returns></returns>
    public static List<CompanyEntity> GetComList(this SqlSugarProvider _sql,List<InputItem> queryItems)
    {
        return _sql.Queryable<CompanyEntity>()
                    .WhereIF(queryItems.GetKeyHasVal(nameof(CompanyEntity.comName)), c => c.comName.Contains(queryItems.GetKeyVal(nameof(CompanyEntity.comName))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(CompanyEntity.comAddr)), c => c.comAddr.Contains(queryItems.GetKeyVal(nameof(CompanyEntity.comAddr))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(CompanyEntity.comPhone)), c => c.comPhone.Contains(queryItems.GetKeyVal(nameof(CompanyEntity.comPhone))))
                    .OrderBy(_a => _a.comName)
                    .ToList();

    }

    /// <summary>
    /// 获取单位信息
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_comCode"></param>
    /// <returns></returns>
    public static CompanyEntity GetCom(this SqlSugarProvider _sql, string _comCode) => _sql.Queryable<CompanyEntity>().Where(_a => _a.comCode == _comCode).First();

    /// <summary>
    /// 保存单位信息
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    public static async Task SaveCom(this SqlSugarProvider _sql, CompanyEntity _dto, bool _new = true)
    {
        var _exists = _sql.Queryable<CompanyEntity>().Where(_a => _a.comCode == _dto.comCode).Any();
        if (_new && _exists) throw new Exception($"单位代码:{_dto.comCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"单位代码:{_dto.comCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;
            if (!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand() > 0;
            else _exec = _sql.Updateable(_dto).ExecuteCommand() > 0;
            if (!_exec) throw new Exception("保存单位信息失败");
        });

    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_comCode"></param>
    /// <returns></returns>
    public static async Task DeleteCom(this SqlSugarProvider _sql, string _comCode)
    {
        var _errMsg = "";
        if (_sql.Queryable<UserEntity>().Where(_a => _a.userComCode == _comCode).Any()) _errMsg = "账号";
        if (_sql.Queryable<DeptEntity>().Where(_a=>_a.deptCompany == _comCode).Any()) _errMsg = "部门";
        if (_errMsg != "") throw new Exception($"有{_errMsg}归属本单位，无法删除");
        await  _sql.Deleteable<CompanyEntity>().Where(_a => _a.comCode == _comCode).ExecuteCommandAsync();
    }

    /// <summary>
    /// 强制删除单位信息
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_comCode"></param>
    /// <returns></returns>
    public static async Task DeleteComForce(this SqlSugarProvider _sql, string _comCode)
    {
        await _sql.GoTransaction(() => {

            if (_sql.Queryable<UserEntity>().Where(_a => _a.userComCode == _comCode).Any())
            {
                if (_sql.Updateable<UserEntity>().SetColumns(_a => _a.userComCode, null).Where(_a => _a.userComCode == _comCode).ExecuteCommand() <= 0)
                    throw new Exception("删除用户单位信息失败");
            }

            if (_sql.Queryable<DeptEntity>().Where(_a => _a.deptCompany == _comCode).Any())
            {
                if (_sql.Updateable<DeptEntity>().SetColumns(_a => _a.deptCompany, null).Where(_a => _a.deptCompany == _comCode).ExecuteCommand() <= 0)
                    throw new Exception("删除部门单位信息失败");
            }

            if (_sql.Deleteable<CompanyEntity>().Where(_a => _a.comCode == _comCode).ExecuteCommand() <= 0)
                throw new Exception("删除单位信息失败");
        });
    }

    #endregion

    #region 部门管理

    /// <summary>
    /// 部门列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_comCode"></param>
    /// <returns></returns>
    public static List<object> GetDeptList(this SqlSugarProvider _sql, List<InputItem> queryItems)
    {
        return _sql.Queryable<DeptEntity>()
                    .LeftJoin<CompanyEntity>((d, c) => d.deptCompany == c.comCode)
                    .WhereIF(queryItems.GetKeyHasVal(nameof(DeptEntity.deptName)), (d, c) => d.deptName.Contains(queryItems.GetKeyVal(nameof(DeptEntity.deptName))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(DeptEntity.deptAddr)), (d, c) => d.deptAddr.Contains(queryItems.GetKeyVal(nameof(DeptEntity.deptAddr))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(DeptEntity.deptCompany)), (d, c) => d.deptCompany == queryItems.GetKeyVal(nameof(DeptEntity.deptCompany)))
                    .OrderBy((d, c) => d.deptName)
                    .Select((d, c) => new
                    {
                        d.deptCode,
                        d.deptName,
                        d.deptAddr,
                        d.deptMemo,
                        d.deptCompany,
                        c.comName
                    })
                    .ToList().Cast<object>().ToList();
    }

    /// <summary>
    /// 保存部门
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task SaveDept(this SqlSugarProvider _sql, DeptEntity _dto, bool _new = true)
    {
        var _exists = _sql.Queryable<DeptEntity>().Where(_a => _a.deptCode == _dto.deptCode).Any();
        if (_new && _exists) throw new Exception($"部门代码:{_dto.deptCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"部门代码:{_dto.deptCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;
            if (!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand() > 0;
            else _exec = _sql.Updateable(_dto).ExecuteCommand() > 0;
            if (!_exec) throw new Exception("保存部门信息失败");
        });
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_deptCode"></param>
    /// <returns></returns>
    public static async Task DeleteDept(this SqlSugarProvider _sql, string _deptCode)
    {
        if (_sql.Queryable<UserEntity>().Where(_a => _a.userDeptCode == _deptCode).Any()) throw new Exception($"有账号归属本部门，无法删除"); 
        await _sql.Deleteable<DeptEntity>().Where(_a => _a.deptCode == _deptCode).ExecuteCommandAsync();
    }

    /// <summary>
    /// 强制删除部门信息
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_deptCode"></param>
    /// <returns></returns>
    public static async Task DeleteDeptForce(this SqlSugarProvider _sql, string _deptCode)
    {
        await _sql.GoTransaction(() => {

            if (_sql.Queryable<UserEntity>().Where(_a => _a.userDeptCode == _deptCode).Any())
            {
                if (_sql.Updateable<UserEntity>().SetColumns(_a => _a.userDeptCode, null).Where(_a => _a.userDeptCode == _deptCode).ExecuteCommand() <= 0)
                    throw new Exception("删除用户部门信息失败");
            }
            if (_sql.Deleteable<DeptEntity>().Where(_a => _a.deptCode == _deptCode).ExecuteCommand() <= 0)
                throw new Exception("删除单位信息失败");
        });
    }

    #endregion

    #region 职务

    /// <summary>
    /// 职务列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_comCode"></param>
    /// <returns></returns>
    public static List<object> GetDutyList(this SqlSugarProvider _sql, List<InputItem> queryItems)
    {
        return _sql.Queryable<DutyEntity>()
                    .LeftJoin<DeptEntity>((d, dept) => d.dutyDeptCode == dept.deptCode)
                    .WhereIF(queryItems.GetKeyHasVal(nameof(DutyEntity.dutyName)), (d, dept) => d.dutyName.Contains(queryItems.GetKeyVal(nameof(DutyEntity.dutyName))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(DutyEntity.dutyDeptCode)), (d, dept) => dept.deptCode == queryItems.GetKeyVal(nameof(DutyEntity.dutyDeptCode)))

            .OrderBy((d, dept) => d.dutyName)
             .Select((d, dept) => new
             {
                 d.dutyCode,
                 d.dutyName,
                 d.dutyMemo,
                 d.dutyDeptCode,
                 dept.deptName
             })
            .ToList().Cast<object>().ToList();

    }

    /// <summary>
    /// 保存职务
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task SaveDuty(this SqlSugarProvider _sql, DutyEntity _dto, bool _new = true)
    {
        var _exists = _sql.Queryable<DutyEntity>().Where(_a => _a.dutyCode == _dto.dutyCode).Any();
        if (_new && _exists) throw new Exception($"职务代码:{_dto.dutyCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"职务代码:{_dto.dutyCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;
            if (!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand() > 0;
            else _exec = _sql.Updateable(_dto).ExecuteCommand() > 0;
            if (!_exec) throw new Exception("保存职务信息失败");
        });
    }

    /// <summary>
    /// 删除职务
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_deptCode"></param>
    /// <returns></returns>
    public static async Task DeleteDuty(this SqlSugarProvider _sql, string _dutyCode, bool _force = false)
    {
        if (!_force)
        {
            if (_sql.Queryable<UserEntity>().Where(_a => _a.userDutyCode == _dutyCode).Any()) throw new Exception($"有账号归属本职务，无法删除");
        }


        await _sql.GoTransaction(async () =>
        {

            if (_sql.Queryable<UserEntity>().Where(_a => _a.userDutyCode == _dutyCode).Any())
            {
                //清除职务权限
                await _sql.Permission_SaveDutyPermission(_dutyCode, new List<string> { });

                if (_sql.Updateable<UserEntity>().SetColumns(_a => _a.userDutyCode, null).Where(_a => _a.userDutyCode == _dutyCode).ExecuteCommand() <= 0)
                    throw new Exception("删除用户职务信息失败");
            }

            if (_sql.Deleteable<DutyEntity>().Where(_a => _a.dutyCode == _dutyCode).ExecuteCommand() <= 0)
                throw new Exception("删除职务信息失败");
        });


        //====
        //if (_sql.Queryable<UserEntity>().Where(_a => _a.userDutyCode == _dutyCode).Any()) throw new Exception($"有账号归属本职务，无法删除");
        //if (_sql.Deleteable<DutyEntity>().Where(_a => _a.dutyCode == _dutyCode).ExecuteCommand() > 0)
        //{ 
        
        
        //}
        //else throw new Exception($"职务删除失败");
    }

    ///// <summary>
    ///// 强制删除职务
    ///// </summary>
    ///// <param name="_db"></param>
    ///// <param name="_tenantCode"></param>
    ///// <param name="_dutyCode"></param>
    ///// <returns></returns>
    //public static async Task DeleteDutyForce(this SqlSugarProvider _sql, string _dutyCode)
    //{
    //    await _db.GoTransaction(() => {

    //        if (_sql.Queryable<UserEntity>().Where(_a => _a.userDutyCode == _dutyCode).Any())
    //        {
    //            if (_sql.Updateable<UserEntity>().SetColumns(_a => _a.userDutyCode, null).Where(_a => _a.userDutyCode == _dutyCode).ExecuteCommand() <= 0)
    //                throw new Exception("删除用户职务信息失败");
    //        }
    //        if (_sql.Deleteable<DutyEntity>().Where(_a => _a.dutyCode == _dutyCode).ExecuteCommand() <= 0)
    //            throw new Exception("删除职务信息失败");
    //    });
    //}

    #endregion

    #region 职称

    /// <summary>
    /// 职称列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_comCode"></param>
    /// <returns></returns>
    public static List<object> GetTechList(this SqlSugarProvider _sql, List<InputItem> queryItems)
    {
        return _sql.Queryable<TechEntity>()
                    .LeftJoin<DeptEntity>((t, dept) => t.techDeptCode == dept.deptCode)
                    .WhereIF(queryItems.GetKeyHasVal(nameof(TechEntity.techName)), (d, dept) => d.techName.Contains(queryItems.GetKeyVal(nameof(TechEntity.techName))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(TechEntity.techDeptCode)), (d, dept) => dept.deptCode == queryItems.GetKeyVal(nameof(TechEntity.techDeptCode)))

            .OrderBy((t, dept) => t.techName)
             .Select((t, dept) => new
             {
                 t.techName,
                 t.techCode,
                 t.techDeptCode,
                 dept.deptName
             })
            .ToList().Cast<object>().ToList();
    }

    /// <summary>
    /// 保存职称
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    public static async Task SaveTech(this SqlSugarProvider _sql, TechEntity _dto, bool _new = true)
    {
        var _exists = _sql.Queryable<TechEntity>().Where(_a => _a.techCode == _dto.techCode).Any();
        if (_new && _exists) throw new Exception($"职称代码:{_dto.techCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"职称代码:{_dto.techCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;
            if (!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand() > 0;
            else _exec = _sql.Updateable(_dto).ExecuteCommand() > 0;
            if (!_exec) throw new Exception("保存职称信息失败");
        });
    }

    /// <summary>
    /// 删除职称
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_techCode"></param>
    /// <returns></returns>
    public static async Task DeleteTech(this SqlSugarProvider _sql, string _techCode)
    {
        if (_sql.Queryable<UserEntity>().Where(_a => _a.userTechCode == _techCode).Any()) throw new Exception($"有账号选择本职称，无法删除");
        await _sql.Deleteable<TechEntity>().Where(_a => _a.techCode == _techCode).ExecuteCommandAsync();
    }

    /// <summary>
    /// 强制删除职称
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_techCode"></param>
    /// <returns></returns>
    public static async Task DeleteTechForce(this SqlSugarProvider _sql, string _techCode)
    {
        await _sql.GoTransaction(() => {

            if (_sql.Queryable<UserEntity>().Where(_a => _a.userTechCode == _techCode).Any())
            {
                if (_sql.Updateable<UserEntity>().SetColumns(_a => _a.userTechCode, null).Where(_a => _a.userTechCode == _techCode).ExecuteCommand() <= 0)
                    throw new Exception("删除用户职称信息失败");
            }
            if (_sql.Deleteable<TechEntity>().Where(_a => _a.techCode == _techCode).ExecuteCommand() <= 0)
                throw new Exception("删除职称信息失败");
        });
    }

    #endregion
}

