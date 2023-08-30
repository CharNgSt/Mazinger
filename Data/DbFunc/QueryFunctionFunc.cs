namespace Mazinger.DataAccess.QueryFunction;

public static class QueryFunctionFunc
{
    /// <summary>
    /// 查询函数
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_queryName"></param>
    /// <param name="_queryDb"></param>
    /// <param name="_queryUser"></param>
    /// <returns></returns>
    public static List<QueryFunctionEntity> QueryFunc_GetList(this SqlSugarProvider _sql, string? _queryName = null, List<string>? _queryDb = null, string? _queryUser = null,string _execType = null)
    {
        var _query = _sql.Queryable<QueryFunctionEntity>()
           .WhereIF(!string.IsNullOrEmpty(_queryName), _a => _a.queryName.Contains(_queryName))
           .WhereIF(!string.IsNullOrEmpty(_execType), _a => _a.queryType == _execType)
           .WhereIF(!string.IsNullOrEmpty(_queryUser), _a => _a.queryAllAllow || SqlFunc.Subqueryable<QueryFunctionPrivEntity>().Where(p => p.privQueryGuid == _a.queryGuid && p.privUserCode == _queryUser).Any());

        if (_queryDb != null && _queryDb.Any())
        {
            _queryDb.ForEach(_q => {
                _query.Where(_a => _a.queryDb.Contains(_q));
            });
        }
        return _query.ToList();
    }

    /// <summary>
    /// 获取参数
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_guid"></param>
    /// <returns></returns>
    public static List<QueryFunctionParameterEntity> GetParameters(this SqlSugarProvider _sql, string _guid) => _sql.Queryable<QueryFunctionParameterEntity>().Where(_a => _a.parameterQueryGuid == _guid).ToList();    

    /// <summary>
    /// 保存查询
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_dto"></param>
    /// <param name="_par"></param>
    /// <param name="_new"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task Save(this SqlSugarProvider _sql, QueryFunctionEntity _dto,List<QueryFunctionParameterEntity>? _par, bool _new = true)
    {
        var _exists = _sql.Queryable<QueryFunctionEntity>().Where(_a => _a.queryName == _dto.queryName).Any();
        if (_new && _exists) throw new Exception($"查询:{_dto.queryName} 已存在，无法新增");
        else if (!_new && !_exists) throw new Exception($"查询:{_dto.queryName} 不存在，无法修改");

        await _sql.GoTransaction(() => {
            var _exec = false;

            _sql.Deleteable<QueryFunctionParameterEntity>().Where(_a => _a.parameterQueryGuid == _dto.queryGuid).ExecuteCommand();

            if(!_exists) _exec = _sql.Insertable(_dto).ExecuteCommand()>0;
            else _exec = _sql.Updateable(_dto).IgnoreColumns(_a=>_a.qeuryCreateDay).ExecuteCommand()>0;

            if (!_exec) throw new Exception("保存查询函数失败");

            if (_par != null && _par.Any())
            {
                _exec = _sql.Insertable(_par).ExecuteCommand() > 0;
                if (!_exec) throw new Exception("保存查询函数参数失败");
            }

            if(_dto.queryAllAllow) _sql.Deleteable<QueryFunctionPrivEntity>().Where(_a => _a.privQueryGuid == _dto.queryGuid).ExecuteCommand();

        });
    }

    /// <summary>
    /// 删除查询
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_guid"></param>
    /// <returns></returns>
    public static async Task Delete(this SqlSugarProvider _sql, string _guid)
    {
        _sql.Deleteable<QueryFunctionParameterEntity>().Where(_a => _a.parameterGuid == _guid).ExecuteCommand();
        _sql.Deleteable<QueryFunctionPrivEntity>().Where(_a => _a.privQueryGuid == _guid).ExecuteCommand();
        _sql.Deleteable<QueryFunctionEntity>().Where(_a => _a.queryGuid == _guid).ExecuteCommand();
    }

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_guid"></param>
    /// <returns></returns>
    public static List<string> GetQueryPriv(this SqlSugarProvider _sql, string _guid) => _sql.Queryable<QueryFunctionPrivEntity>().Where(_a => _a.privQueryGuid == _guid).ToList(_a => _a.privUserCode);

    /// <summary>
    /// 检查参数是否合法
    /// </summary>
    /// <param name="_pars"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task CheckPar(List<ParameterDto>? _pars)
    {
        if (_pars != null && _pars.Any())
        {
            _pars.ForEach(_a => {
                if (_a.Value == null && _a.DateValue == null) throw new Exception($"参数【{_a.Name}】的值为空！");
                if (_a.Type == "int" && !int.TryParse(_a.Value, out int _n)) throw new Exception($"参数【{_a.Name}】的值类型错误！");
            });
        }
    }


    /// <summary>
    /// 分配权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_guid"></param>
    /// <param name="_priv"></param>
    /// <returns></returns>
    public static async Task SaveQueryPriv(this SqlSugarProvider _sql, string _guid, List<string>? _priv)
    {
        _sql.Updateable<QueryFunctionEntity>().SetColumns(_a => _a.queryAllAllow, false).Where(_a => _a.queryGuid == _guid).ExecuteCommand();
        _sql.Deleteable<QueryFunctionPrivEntity>().Where(_a => _a.privQueryGuid == _guid).ExecuteCommand();
        if (_priv != null && _priv.Any())
        {
            _sql.Insertable(_priv.Select(_a => new QueryFunctionPrivEntity { privGuid = Guid.NewGuid().ToString(), privQueryGuid = _guid, privUserCode = _a }).ToList()).ExecuteCommand();
        }

    }

    /// <summary>
    /// 分配权限
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_tenantCode"></param>
    /// <param name="_guid"></param>
    /// <param name="_priv"></param>
    /// <returns></returns>
    public static async Task SaveQueryPriv(this SqlSugarProvider _sql, string _guid, string _userCode,bool _allow)
    {
        if(_allow)
            _sql.Insertable(new QueryFunctionPrivEntity { privGuid = Guid.NewGuid().ToString(), privQueryGuid = _guid, privUserCode = _userCode }).ExecuteCommand();
        else
            _sql.Deleteable<QueryFunctionPrivEntity>().Where(_a => _a.privQueryGuid == _guid && _a.privUserCode == _userCode).ExecuteCommand();
    }

    public static async Task ExecSql(this SqlSugarProvider _sql, string _guid, List<ParameterDto>? _pars)
    {
        await CheckPar(_pars);

        var _target = _sql.Queryable<QueryFunctionEntity>().Where(_a => _a.queryGuid == _guid).First();

        var _splitChar = "@";
        if (_target.querySql.IndexOf(":") > 0) _splitChar = ":";
        var _parList = new List<SugarParameter>() { };
        if (_pars != null && _pars.Any()) 
        { 
            _pars.ForEach(_a => {
                _parList.Add(new SugarParameter($"{_splitChar}{_a.Name}", string.IsNullOrEmpty(_a.Value)?_a.DateValue: _a.Value));
            }); 
        }

        foreach (var _db_code in _target.queryDb.Split(','))
        {
            var _dbRes = _sql.GetDb(_db_code).Ado.GetDataTable(_target.querySql, _parList);
            if (_dbRes != null)
            {
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new Exception("Get the assembly root directory exception!");

                if (_target.queryType == "query")
                {
                    JArray _rows = new JArray();
                    var _header = new JArray { };
                    foreach (DataColumn _col in _dbRes.Columns) _header.Add(_col.ColumnName);
                    _rows.Add(_header);
                    var _colCount = _dbRes.Columns.Count;
                    foreach (DataRow _dr in _dbRes.Rows)
                    {
                        var _row = new JArray { };
                        for (var _i = 0; _i < _colCount; _i++) _row.Add(GetRowValue(_dr,_i));
                        _rows.Add(_row);
                    }
                    var _tmpPath = $"{basePath}/Tmp".GetRuntimeDirectory();
                    if (!Directory.Exists(_tmpPath)) Directory.CreateDirectory(_tmpPath);

                    var _filename = $"{_tmpPath}/{Guid.NewGuid().ToString()}.txt";
                    using StreamWriter sw = new StreamWriter(_filename, true);
                    sw.WriteLine(_rows.toJsonStr());
                }
            }
        }
    }

    /// <summary>
    /// 获取CSV文件地址
    /// </summary>
    /// <param name="_guid"></param>
    /// <param name="_fileType">Csv 或 Json </param>
    /// <returns></returns>
    public static string GetFilePath(string _guid,string _fileType)
    {
        //JsonFilePath、CsvFilePath
        var fileName = _fileType.ToLower() == "csv" ? "Csv" : "Json";
        var basePath = $"{fileName}FilePath".GetConfigVal();
        if (string.IsNullOrEmpty(basePath)) {
            basePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/{fileName}Files";
        }

        if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);

        return $"{basePath}/{_guid}.{_fileType.ToLower()}".GetRuntimeDirectory();
    }

    /// <summary>
    /// 查询并生成Csv文件
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_db_code"></param>
    /// <param name="_querySql"></param>
    /// <param name="_pars"></param>
    /// <param name="_taskGuid"></param>
    /// <returns></returns>
    public static async Task ExecBuildCsvFile(this SqlSugarProvider _sql, string _querySql, List<ParameterDto>? _pars,string _taskGuid)
    {
        var _dbRes = _sql.ExecSqlReturnDataTable(_querySql, _pars);

        StringBuilder str = new StringBuilder();
        string separate = ",";

        if (_dbRes == null || _dbRes.Rows.Count == 0)
        {
            str.AppendLine("无符合条件的数据");
        }
        else
        {
            foreach (DataColumn _col in _dbRes.Columns) str.Append($"{_col.ColumnName}{separate}");
            str.Append(Environment.NewLine);

            var _colCount = _dbRes.Columns.Count;
            foreach (DataRow _dr in _dbRes.Rows)
            {
                for (var _i = 0; _i < _colCount; _i++) str.Append($"{GetRowValue(_dr, _i)}{separate}");
                str.Append(Environment.NewLine);
            }

            str.Remove(str.Length - 2, 1);
        }
        var _fileName = GetFilePath(_taskGuid, "Csv");
        using StreamWriter sw = new StreamWriter(_fileName, true);        
        sw.WriteLine(str.ToString());
    }

    /// <summary>
    /// 查询并生成Json文件
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_db_code"></param>
    /// <param name="_querySql"></param>
    /// <param name="_pars"></param>
    /// <param name="_taskGuid"></param>
    /// <returns></returns>
    public static async Task ExecBuildJsonFile(this SqlSugarProvider _sql, string _querySql, List<ParameterDto>? _pars, string _taskGuid)
    {
        var _json = _sql.ExecGetJarray(_querySql, _pars);
        var _fileName = GetFilePath(_taskGuid,"Json");
        using StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine(_json.ToString());
    }

    public static JArray ExecGetJarray(this SqlSugarProvider _sql, string _querySql, List<ParameterDto>? _pars)
    { 
        var _dbRes = _sql.ExecSqlReturnDataTable( _querySql, _pars);
        JArray _rows = new JArray();
        var _header = new JArray { };
        foreach (DataColumn _col in _dbRes.Columns) _header.Add(_col.ColumnName);
        _rows.Add(_header);
        var _colCount = _dbRes.Columns.Count;
        foreach (DataRow _dr in _dbRes.Rows)
        {
            var _row = new JArray { };
            for (var _i = 0; _i < _colCount; _i++) _row.Add(GetRowValue(_dr, _i));
            _rows.Add(_row);
        }
        return _rows;
    }

    private static DataTable ExecSqlReturnDataTable(this SqlSugarProvider _sql, string _querySql, List<ParameterDto>? _pars)
    {
        var _splitChar = "@";
        if (_querySql.IndexOf(":") > 0) _splitChar = ":";
        var _parList = new List<SugarParameter>() { };
        if (_pars != null && _pars.Any())
        {
            _pars.ForEach(_a => {
                if (string.IsNullOrEmpty(_a.Value) && _a.DateValue == null) throw new Exception($"参数【{_a.Name}】的值为空！");
                if (_a.Type == "int" && !int.TryParse(_a.Value, out int _n)) throw new Exception($"参数【{_a.Name}】的值类型错误！");
                _parList.Add(new SugarParameter($"{_splitChar}{_a.Name}", string.IsNullOrEmpty(_a.Value) ? _a.DateValue : _a.Value));
            });
        }
        return _sql.Ado.GetDataTable(_querySql, _parList);
    }

    /// <summary>
    /// 返回DataRow的列值
    /// </summary>
    /// <param name="_dr">DataRow</param>
    /// <param name="_name">列名</param>
    /// <returns></returns>
    private static string GetRowValue(DataRow _dr, int _index)
    {
        try
        {
            return _dr[_index].ToString();
        }
        catch
        {
            return "";
        }
    }
}

