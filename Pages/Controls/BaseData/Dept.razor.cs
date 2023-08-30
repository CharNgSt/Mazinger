using Mazinger.DataAccess.Base;

[Injection(Named = nameof(DeptDataAccess))]
public class DeptDataAccess : BaseDataAccess, IDataAccessInterface, IScoped
{
    public DeptDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(httpContextAccessor, db) { }

    public Type InputEntity() => typeof(DeptEntity);

    public List<InputItem> DefaultInputItems()
    {
        var _res = InputEntity().EntityToInputItemList();
        _res.ForEach(_item => 
        {
            if (_item.InputName == BDA_GetColumnName<DeptEntity>(d => d.deptCompany))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<CompanyEntity>().OrderBy(_a => _a.comName).Select(_a => new ComboxItem { Key = _a.comName, Value = _a.comCode }).ToList();
            }
            else if (_item.InputName == BDA_GetColumnName<DeptEntity>(d => d.deptMemo))
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
            new() { Text = "部门代码", Value = nameof(DeptEntity.deptCode),Width=180 },
            new() { Text = "名称", Value = nameof(DeptEntity.deptName) },
            new() { Text = "地址", Value = nameof(DeptEntity.deptAddr) },
            new() { Text = "所属单位", Value = nameof(CompanyEntity.comName) },
            new() { Text = "备注", Value = nameof(DeptEntity.deptMemo) },
            new() { Text = "操作", Value = "Action", Sortable = false ,Width=250 }
        };

    public List<InputItem> TableQueryItem()
        =>
        new()
        {
            new InputItem{ Flag = true, LabelTxt ="名称", InputName= nameof(DeptEntity.deptName), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="地址", InputName= nameof(DeptEntity.deptAddr), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="所属单位", InputName= nameof(CompanyEntity.comName),InputType = InputItemType.Select, SelectCombox = BDA_GetSql().Queryable<CompanyEntity>().OrderBy(_a=>_a.comName).Select(_a=>new ComboxItem{ Key = _a.comName, Value =_a.comCode } ).ToList() }
        };

    public List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        totalCount = 0;
        var rows = BDA_GetSql().GetDeptList(queryItems);
        if (rows != null)
        {
            totalCount = rows.Count;
            return rows.Cast<object>().ToList();
        }
        else return new List<object> { };
    }

    public async Task SaveFunc(List<InputItem> inputItems, bool _isNew) => await BDA_GetSql().SaveDept(inputItems.ToObj<DeptEntity>(), _isNew);
}