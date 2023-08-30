using Mazinger.DataAccess.QueryFunction;
namespace Mazinger.DataAccess.ExecResult;

/// <summary>
/// 执行结果
/// </summary>
public static class ExecResultFunc
{
    /// <summary>
    /// 查询任务列表
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <returns></returns>
    public static ExecQueryResultDto GetTaskList(this SqlSugarProvider _sql, ExecQueryDto _dto,string _userCode)
    {
        int _totalCount = 0;
        var _rows = _sql.Queryable<ExecResultEntity>()
                         .LeftJoin<QueryFunctionEntity>((e, q) => e.execQueryGuid == q.queryGuid)
                         .LeftJoin<DbConnEntity>((e, q, c) => e.execDbCode == c.dbCode)
                         .LeftJoin<UserEntity>((e, q, c, u) => e.execCreateUser == u.userCode)
                         .Where((e, q, c, u) => e.execCreateUser == _userCode && q.queryType == _dto.queyrExecType)
                         .OrderByDescending(e => e.execCreateDay)
                         .Select((e, q, c, u) => new ExecDto
                         {
                             guid = e.execGuid,
                             queryDb = c.dbName,
                             queryType = q.queryType == "query" ? "查询" : "导出文件",
                             createdAt = e.execCreateDay,
                             updatedAt = e.execFinishDay,
                             queryName = q.queryName,
                             queryCondition = e.execCondition,
                             queryResult =  e.execResult? "完成":(e.execFinishDay == null || e.execFinishDay == DateTime.Parse("1900-01-01") ? (e.execCreateDay< DateTime.Now.AddHours(-1) ? "已超时":"查询中"):"失败"),
                             queryUser = u.userName,
                             queryResultMemo = e.execMsg
                         })
                         .ToPageList(_dto.page, _dto.pageSize, ref _totalCount);

        return new ExecQueryResultDto
        {
            rows = _rows,
            totalCount = _totalCount
        };
    }

    /// <summary>
    /// 查询任务
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <returns></returns>
    public static ExecDto GetTask(this SqlSugarProvider _sql, string _execGuid)
    {
       return  _sql.Queryable<ExecResultEntity>()
                         .LeftJoin<QueryFunctionEntity>((e, q) => e.execQueryGuid == q.queryGuid)
                         .LeftJoin<DbConnEntity>((e, q, c) => e.execDbCode == c.dbCode)
                         .LeftJoin<UserEntity>((e, q, c, u) => e.execCreateUser == u.userCode)
                         .Where((e, q, c, u) => e.execGuid == _execGuid)
                         .OrderByDescending(e => e.execCreateDay)
                         .Select((e, q, c, u) => new ExecDto
                         {
                             guid = e.execGuid,
                             queryDb = c.dbName,
                             queryType = q.queryType == "query" ? "查询" : "导出文件",
                             createdAt = e.execCreateDay,
                             updatedAt = e.execFinishDay,
                             queryName = q.queryName,
                             queryCondition = e.execCondition,
                             queryResult = e.execResult ? "完成" : (e.execFinishDay == null || e.execFinishDay == DateTime.Parse("1900-01-01") ? (e.execCreateDay < DateTime.Now.AddHours(-1) ? "已超时" : "查询中") : "失败"),
                             queryUser = u.userName,
                             queryResultMemo = e.execMsg
                         }).First();
    }

    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <returns></returns>
    public static async Task Create(this SqlSugarProvider _sql, ExecResultEntity _dto) => await _sql.Insertable(_dto).ExecuteCommandAsync();

    /// <summary>
    /// 执行任务并返回结果
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_execDbCode"></param>
    /// <param name="queryType"></param>
    /// <param name="sqlTxt"></param>
    /// <param name="_pars"></param>
    /// <param name="_taskGuid"></param>
    /// <returns></returns>
    public static async Task BuildFile(this SqlSugarProvider _sql,string _execDbCode, string queryType,string sqlTxt, List<ParameterDto>? _pars, string _taskGuid)
    {
        var _msg = "执行完毕";
        var _result = false;
        try
        {
            if (queryType == "query") await _sql.ExecBuildJsonFile( sqlTxt, _pars, _taskGuid);
            else await _sql.ExecBuildCsvFile( sqlTxt, _pars, _taskGuid);

            _result = true;
        }
        catch(Exception ex) { _msg ="失败！"+ ex.Message + ex.StackTrace; }

        await _sql.Updateable<ExecResultEntity>().SetColumns(_a => _a.execFinishDay, DateTime.Now).SetColumns(_a => _a.execResult, _result).SetColumns(_a => _a.execMsg, _msg).Where(_a => _a.execGuid == _taskGuid).ExecuteCommandAsync();
    }

}