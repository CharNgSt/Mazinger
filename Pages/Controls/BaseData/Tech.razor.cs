using Mazinger.DataAccess.Base;

[Injection(Named = nameof(TechDataAccess))]
public class TechDataAccess : BaseDataAccess, IDataAccessInterface, IScoped
{
    public TechDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(httpContextAccessor, db) { }

    public Type InputEntity() => typeof(TechEntity);

    public List<InputItem> DefaultInputItems()
    {
        var _res = InputEntity().EntityToInputItemList();
        _res.ForEach(_item =>
        {
            if (_item.InputName == BDA_GetColumnName<TechEntity>(d => d.techDeptCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<DeptEntity>().OrderBy(_a => _a.deptName).Select(_a => new ComboxItem { Key = _a.deptName, Value = _a.deptCode }).ToList();
                return;
            }
        });
        return _res;
    }

    public List<DataTableHeader<object>> TableHeader()
        =>
        new()
        {
            new() { Text = "职称代码", Value = nameof(TechEntity.techCode),Width=180 },
            new() { Text = "名称", Value = nameof(TechEntity.techName) },
            new() { Text = "所属部门", Value = nameof(DeptEntity.deptName) },
            new() { Text = "操作", Value = "Action", Sortable = false ,Width=250 }
        };

    public List<InputItem> TableQueryItem()
        =>
        new()
        {
            new InputItem{ Flag = true, LabelTxt ="名称", InputName= nameof(TechEntity.techName), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="所属部门", InputName= nameof(TechEntity.techDeptCode),InputType = InputItemType.Select, SelectCombox = BDA_GetSql().Queryable<DeptEntity>().OrderBy(_a => _a.deptName).Select(_a => new ComboxItem { Key = _a.deptName, Value = _a.deptCode } ).ToList() }
        };

    public List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        totalCount = 0;
        var rows = BDA_GetSql().GetTechList(queryItems);
        if (rows != null)
        {
            totalCount = rows.Count;
            return rows.Cast<object>().ToList();
        }
        else return new List<object> { };
    }

    public async Task SaveFunc(List<InputItem> inputItems, bool _isNew) => await BDA_GetSql().SaveTech(inputItems.ToObj<TechEntity>(), _isNew);
}