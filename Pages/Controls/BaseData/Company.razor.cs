using Mazinger.DataAccess.Base;

[Injection(Named = nameof(CompanyDataAccess))]
public class CompanyDataAccess : BaseDataAccess, IDataAccessInterface, IScoped
{
    public CompanyDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(httpContextAccessor, db) { }

    public Type InputEntity() => typeof(CompanyEntity);

    public List<InputItem> DefaultInputItems()
    {
        var _res = InputEntity().EntityToInputItemList();

        _res.ForEach(_item =>
        {
            if (_item.InputName == BDA_GetColumnName<CompanyEntity>(d => d.comDbCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<DbConnEntity>().OrderBy(_a => _a.dbName).Select(_a => new ComboxItem { Key = _a.dbName, Value = _a.dbCode }).ToList();
            }
        });


        return _res;
    }

    public List<DataTableHeader<object>> TableHeader()
        =>
        new()
        {
            new() { Text = "单位代码", Value = nameof(CompanyEntity.comCode),Width=180 },
            new() { Text = "名称", Value = nameof(CompanyEntity.comName) },
            new() { Text = "联系电话", Value = nameof(CompanyEntity.comPhone) },
            new() { Text = "地址", Value = nameof(CompanyEntity.comAddr) },
            new() { Text = "绑定租户", Value = nameof(CompanyEntity.comDbCode) },
            new() { Text = "操作", Value = "Action", Sortable = false ,Width=250 }
        };

    public List<InputItem> TableQueryItem()
        =>
        new()
        {
            new InputItem{ Flag = true, LabelTxt ="名称", InputName= nameof(CompanyEntity.comName), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="地址", InputName= nameof(CompanyEntity.comAddr), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="联系电话", InputName= nameof(CompanyEntity.comPhone),InputType = InputItemType.String }
        };

    public List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        var rows = BDA_GetSql().GetComList(queryItems);
        totalCount = rows.Count;
        return rows.Cast<object>().ToList();
    }

    public async Task SaveFunc(List<InputItem> inputItems, bool _isNew) => await BDA_GetSql().SaveCom(inputItems.ToObj<CompanyEntity>(), _isNew);
}