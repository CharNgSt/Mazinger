using Furion.DatabaseAccessor;
using Mazinger.DataAccess.Permission;
namespace Mazinger.DataAccess.User;

/// <summary>
/// 用户数据操作
/// </summary>
public static class UserFunc
{
    /// <summary>
    /// 获取账号列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <returns></returns>
    public static List<object> User_Query(this SqlSugarProvider _sql, List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        totalCount = 0;
        return _sql.Queryable<UserEntity>()
                    .LeftJoin<DeptEntity>((u, d) => u.userDeptCode == d.deptCode)
                    .LeftJoin<DutyEntity>((u, d, du) => u.userDutyCode == du.dutyCode)
                    .LeftJoin<TechEntity>((u, d, du, t) => u.userTechCode == t.techCode)
                    .LeftJoin<CompanyEntity>((u, d, du, t, c) => u.userComCode == c.comCode)

                    .WhereIF(queryItems.GetKeyHasVal(nameof(UserEntity.userCode)), (u, d, du, t, c) => u.userCode == queryItems.GetKeyVal(nameof(UserEntity.userCode)))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(UserEntity.userName)), (u, d, du, t, c) => u.userName.Contains(queryItems.GetKeyVal(nameof(UserEntity.userName))))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(UserEntity.userComCode)), (u, d, du, t, c) => u.userComCode == queryItems.GetKeyVal(nameof(UserEntity.userComCode)))
                    .WhereIF(queryItems.GetKeyHasVal(nameof(UserEntity.userDeptCode)), (u, d, du, t, c) => u.userDeptCode == queryItems.GetKeyVal(nameof(UserEntity.userDeptCode)))

                    .Select((u, d, du, t, c) => new 
                    {
                        u.userCode,
                        u.userName,
                        u.userBirthday,
                        u.userEmail,
                        u.userEname,
                        u.userIdcard,
                        u.userMoblie,
                        u.userSex,
                        u.userMainPage,

                        c.comDbCode,
                        c.comName,
                        d.deptName,
                        du.dutyName,
                        t.techName,

                        d.deptCode,
                        du.dutyCode,
                        t.techCode,
                        c.comCode,

                        u.userFailedtime,
                        u.userFreezetime,
                        u.userPwdOvertime,
                        u.userSsokey,

                        UserStatus = u.userFreezetime == null ? "正常" : "冻结"

                    })
            .ToPageList(pageIndex, pageSize, ref totalCount)
            .Cast<object>().ToList();

    }

    /// <summary>
    /// 保存用户信息
    /// </summary>
    /// <param name="_sql"></param>
    /// <param name="_dto"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task User_Save(this SqlSugarProvider _sql, UserEntity _dto, bool _new = true)
    {
        var _exists = _sql.Queryable<UserEntity>().Where(_a => _a.userCode == _dto.userCode).Any();
        if (_new && _exists) throw new Exception($"账号:{_dto.userCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"账号:{_dto.userCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;

            

            if (!_exists)
            {
                _exec = _sql.Insertable(_dto.Adapt<UserEntity>()).ExecuteCommand() > 0;
            }
            else
            {
                _exec = _sql.Updateable(_dto.Adapt<UserEntity>()).IgnoreColumnsIF(string.IsNullOrEmpty(_dto.userPwd), a => a.userPwd).ExecuteCommand() > 0;
            }

            if (!_exec) throw new Exception("保存用户信息失败");

            List<string> _u_p = new List<string> { };
            if (!string.IsNullOrEmpty(_dto.userDutyCode)) _u_p = _sql.Permission_GetDutyPermission(_dto.userDutyCode);
            _sql.Permission_SaveUserPermission(_dto.userCode, _u_p, "0");

        });

    }

    /// <summary>
    /// 冻结或解冻账号
    /// </summary>
    /// <param name="_userCode"></param>
    /// <param name="_freeze"></param>
    /// <returns></returns>
    public static async Task User_Freeze(this SqlSugarProvider _sql, string _userCode, bool _freeze)
    {
        if (await _sql.Updateable<UserEntity>()
                        .SetColumnsIF(_freeze, _a => _a.userFreezetime, DateTime.Now.AddYears(10))
                        .SetColumnsIF(!_freeze, _a => _a.userFreezetime, null)
                        .SetColumnsIF(!_freeze, _a => _a.userFailedtime, 0)
                        .Where(_a => _a.userCode == _userCode).ExecuteCommandAsync() < 0)
        {
            throw new Exception($"{(_freeze ? "冻结" : "解冻")}用户失败");
        }
    }





    /// <summary>
    /// 更新最后登录时间
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <returns></returns>
    public static async Task User_UpdateLastLoginTime(this SqlSugarProvider _sql, string _userCode)
    {
        _sql.Updateable<UserEntity>().SetColumns(_a => _a.userLastLogintime, DateTime.Now).Where(_a => _a.userCode == _userCode).ExecuteCommand();
    }

    /// <summary>
    /// 更新登录失败次数、时间
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <param name="_trytime"></param>
    /// <param name="_date"></param>
    /// <returns></returns>
    public static async Task User_UpdateTryTime(this SqlSugarProvider _sql, string _userCode, decimal? _trytime, DateTime? _date = null)
    {
        _sql
            .Updateable<UserEntity>()
            .SetColumns(_a => _a.userFailedtime, _trytime)
            .SetColumns(_a=>_a.userFreezetime,_date)
            .Where(_a=>_a.userCode == _userCode)
            .ExecuteCommandAsync();    
    }

    /// <summary>
    /// 登录时强制修改密码
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <param name="_pwd"></param>
    /// <returns></returns>
    public static async Task User_UpdatePwd(this SqlSugarProvider _sql, string _userCode, string _pwd,string _y_pwd)
    {
        var _user = _sql.Queryable<UserEntity>().Where(_a => _a.userCode == _userCode).First();
        if (_user == null) throw new Exception($"账号【{_userCode}】不存在");
        if (_user.userPwd == _y_pwd) throw new Exception("原密码错误，修改失败");
        if (_user.userPwd == _pwd) throw new Exception("新密码与现在的密码相同，修改失败");
        var _days = "System:Setting:Login_PwdOverTime".GetConfigInt();
        _sql.Updateable<UserEntity>().SetColumns(_a => _a.userPwd, _pwd).SetColumnsIF(_days > 0, _a => _a.userPwdOvertime, DateTime.Now.AddDays(_days))
            .Where(_a => _a.userCode == _userCode).ExecuteCommand();
    }

    /// <summary>
    /// 保存用户
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <returns></returns>
    public static async Task User_Save(this SqlSugarProvider _sql, UserSaveDto _dto,bool _new = true)
    {
        var _exists = _sql.Queryable<UserEntity>().Where(_a => _a.userCode == _dto.userCode).Any();
        if (_new && _exists) throw new Exception($"账号:{_dto.userCode} 对象已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"账号:{_dto.userCode} 对象不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;

            if (!_exists)
            {
                _exec = _sql.Insertable(_dto.Adapt<UserEntity>()).ExecuteCommand() > 0;
            }
            else
            {
                _exec = _sql.Updateable(_dto.Adapt<UserEntity>()).IgnoreColumnsIF(string.IsNullOrEmpty(_dto.userPwd), a => a.userPwd).ExecuteCommand() > 0;
            }

            if (!_exec) throw new Exception("保存用户信息失败");

            List<string> _u_p = new List<string> { };
            if (!string.IsNullOrEmpty(_dto.userDutyCode)) _u_p = _sql.Permission_GetDutyPermission(_dto.userDutyCode);
            _sql.Permission_SaveUserPermission(_dto.userCode, _u_p, "0");

        });

    }


    /// <summary>
    /// 获取账号列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <returns></returns>
    public static UserQueryResultDto User_Query(this SqlSugarProvider _sql, UserQueryDto _dto)
    {
        int _totalCount = 0;

        return new UserQueryResultDto
        {
            rows = _sql.Queryable<UserEntity>()
                                    .LeftJoin<DeptEntity>((u, d) => u.userDeptCode == d.deptCode)
                                    .LeftJoin<DutyEntity>((u, d, du) => u.userDutyCode == du.dutyCode)
                                    .LeftJoin<TechEntity>((u, d, du, t) => u.userTechCode == t.techCode)
                                    .LeftJoin<CompanyEntity>((u, d, du, t, c) => u.userComCode == c.comCode)

                                    .WhereIF(!string.IsNullOrEmpty(_dto.userName), (u, d, du, t, c) => u.userName.Contains(_dto.userName))
                                    .WhereIF(!string.IsNullOrEmpty(_dto.userCode), (u, d, du, t, c) => u.userName.Contains(_dto.userCode))
                                    .WhereIF(!string.IsNullOrEmpty(_dto.deptCode), (u, d, du, t, c) => u.userDeptCode == _dto.deptCode)
                                    .WhereIF(!string.IsNullOrEmpty(_dto.dutyCode), (u, d, du, t, c) => u.userDutyCode == _dto.dutyCode)
                                    .WhereIF(!string.IsNullOrEmpty(_dto.techCode), (u, d, du, t, c) => u.userTechCode == _dto.techCode)
                                    .WhereIF(!string.IsNullOrEmpty(_dto.comCode), (u, d, du, t, c) => c.comCode == _dto.comCode)

                                    .Select((u, d, du, t, c) => new UserSaveDto
                                    {
                                        userCode = u.userCode,
                                        userName = u.userName,
                                        userBirthday = u.userBirthday,
                                        userEmail = u.userEmail,
                                        userEname = u.userEname,
                                        userIdcard = u.userIdcard,
                                        userMoblie = u.userMoblie,
                                        userSex = u.userSex,
                                        mainPage = u.userMainPage,

                                        companyName = c.comName,
                                        deptName = d.deptName,
                                        dutyName = du.dutyName,
                                        techName = t.techName,

                                        userDeptCode = d.deptCode,
                                        userDutyCode = du.dutyCode,
                                        userTechCode = t.techCode,
                                        userComCode = c.comCode,

                                        userFailedtime = u.userFailedtime,
                                        userFreezetime = u.userFreezetime,
                                        userPwdOvertime = u.userPwdOvertime,
                                        userSsokey = u.userSsokey,

                                    }).ToPageList(_dto.page, _dto.pageSize, ref _totalCount),
            totalCount = _totalCount
        };
    }

    /// <summary>
    /// 获取用户保存对象
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <returns></returns>
    public static UserSaveDto User_GetSaveDto(this SqlSugarProvider _sql, string _userCode)
    {
        return _sql.Queryable<UserEntity>()
                                    .LeftJoin<DeptEntity>((u, d) => u.userDeptCode == d.deptCode)
                                    .LeftJoin<DutyEntity>((u, d, du) => u.userDutyCode == du.dutyCode)
                                    .LeftJoin<TechEntity>((u, d, du, t) => u.userTechCode == t.techCode)
                                    .LeftJoin<CompanyEntity>((u, d, du, t, c) => u.userComCode == c.comCode)

                                    .Where((u, d, du, t, c) => u.userCode == _userCode)
                                    .Select((u, d, du, t, c) => new UserSaveDto
                                    {
                                        userPwd = u.userPwd,
                                        userCode = u.userCode,
                                        userName = u.userName,
                                        userBirthday = u.userBirthday,
                                        userEmail = u.userEmail,
                                        userEname = u.userEname,
                                        userIdcard = u.userIdcard,
                                        userMoblie = u.userMoblie,
                                        userSex = u.userSex,
                                        mainPage = u.userMainPage,

                                        companyDbCode = c.comDbCode,
                                        companyName = c.comName,
                                        deptName = d.deptName,
                                        dutyName = du.dutyName,
                                        techName = t.techName,

                                        userDeptCode = d.deptCode,
                                        userDutyCode = du.dutyCode,
                                        userTechCode = t.techCode,
                                        userComCode = c.comCode,

                                        userFailedtime = u.userFailedtime,
                                        userFreezetime = u.userFreezetime,
                                        userPwdOvertime = u.userPwdOvertime,
                                        userSsokey = u.userSsokey,

                                    }).First();
    }

    /// <summary>
    /// 用户数据对象
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_userCode"></param>
    /// <returns></returns>
    public static UserEntity User_GetEntity(this SqlSugarProvider _sql, string _userCode) => _sql.Queryable<UserEntity>().Single(_a => _a.userCode == _userCode);

}


