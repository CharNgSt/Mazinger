using Mazinger.DataAccess.Base;

[Injection(Named = nameof(DutyDataAccess))]
public class DutyDataAccess : BaseDataAccess, IDataAccessInterface, IScoped
{
    public DutyDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(httpContextAccessor, db) { }

    public Type InputEntity() => typeof(DutyEntity);

    public List<InputItem> DefaultInputItems()
    {
        var _res = InputEntity().EntityToInputItemList();
        _res.ForEach(_item =>
        {
            if (_item.InputName == BDA_GetColumnName<DutyEntity>(d => d.dutyDeptCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<DeptEntity>().OrderBy(_a => _a.deptName).Select(_a => new ComboxItem { Key = _a.deptName, Value = _a.deptCode }).ToList();
            }
            else if (_item.InputName == BDA_GetColumnName<DutyEntity>(d => d.dutyMemo))
            {
                _item.InputType = InputItemType.Textarea;
            }
        });
        return _res;
    }

    public List<DataTableHeader<object>> TableHeader()
        =>
        new()
        {
            new() { Text = "职务代码", Value = nameof(DutyEntity.dutyCode),Width=180 },
            new() { Text = "名称", Value = nameof(DutyEntity.dutyName) },
            new() { Text = "所属部门", Value = nameof(DeptEntity.deptName) },
            new() { Text = "备注", Value = nameof(DutyEntity.dutyMemo) },
            new() { Text = "操作", Value = "Action", Sortable = false ,Width=250 }
        };

    public List<InputItem> TableQueryItem()
        =>
        new()
        {
            new InputItem{ Flag = true, LabelTxt ="名称", InputName= nameof(DutyEntity.dutyName), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="所属部门", InputName= nameof(DutyEntity.dutyDeptCode),InputType = InputItemType.Select, SelectCombox = BDA_GetSql().Queryable<DeptEntity>().OrderBy(_a => _a.deptName).Select(_a => new ComboxItem { Key = _a.deptName, Value = _a.deptCode } ).ToList() }
        };

    public List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        totalCount = 0;
        var rows = BDA_GetSql().GetDutyList(queryItems);
        if (rows != null)
        {
            totalCount = rows.Count;
            return rows.Cast<object>().ToList();
        }
        else return new List<object> { };
    }

    public async Task SaveFunc(List<InputItem> inputItems, bool _isNew) => await BDA_GetSql().SaveDuty(inputItems.ToObj<DutyEntity>(), _isNew);
}