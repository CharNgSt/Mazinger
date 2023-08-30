
using Mazinger.DataAccess.Base;
using Mazinger.DataAccess.User;

[Injection(Named = nameof(UserDataAccess))]
public class UserDataAccess : BaseDataAccess, IDataAccessInterface, IScoped
{
    public UserDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(httpContextAccessor, db) { }

    public Type InputEntity() => typeof(UserEntity);

    public List<InputItem> DefaultInputItems()
    {;
        var _res = InputEntity().EntityToInputItemList();
        _res.ForEach(_item =>
        {
            if (_item.InputName == BDA_GetColumnName<UserEntity>(u => u.userComCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<CompanyEntity>().OrderBy(_a => _a.comName).Select(_a => new ComboxItem { Key = _a.comName, Value = _a.comCode }).ToList();
            }
            else if (_item.InputName == BDA_GetColumnName<UserEntity>(u=>u.userDeptCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<DeptEntity>().OrderBy(_a => _a.deptName).Select(_a => new ComboxItem { Key = _a.deptName, Value = _a.deptCode }).ToList();
            }
            else if (_item.InputName == BDA_GetColumnName<UserEntity>(u => u.userDutyCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<DutyEntity>().OrderBy(_a => _a.dutyName).Select(_a => new ComboxItem { Key = _a.dutyName, Value = _a.dutyCode }).ToList();
            }
            else if (_item.InputName == BDA_GetColumnName<UserEntity>(u => u.userTechCode))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = BDA_GetSql().Queryable<TechEntity>().OrderBy(_a => _a.techName).Select(_a => new ComboxItem { Key = _a.techName, Value = _a.techCode }).ToList();
            }
            else if (_item.InputName == BDA_GetColumnName<UserEntity>(u => u.userSex))
            {
                _item.InputType = InputItemType.Select;
                _item.SelectCombox = new List<ComboxItem> { new ComboxItem { Key = "男", Value = "男" }, new ComboxItem { Key = "女", Value = "女" } };
            }
        });
        return _res;
    }

    public List<DataTableHeader<object>> TableHeader()
        =>
        new()
        {
            new() { Text = "账号", Value = nameof(UserEntity.userCode),Width=180 },
            new() { Text = "姓名", Value = nameof(UserEntity.userName) },
            new() { Text = "联系电话", Value = nameof(UserEntity.userMoblie) },
            new() { Text = "单位", Value = nameof(CompanyEntity.comName) },
            new() { Text = "部门", Value = nameof(DeptEntity.deptName) },
            new() { Text = "职务", Value = nameof(DutyEntity.dutyName) },
            new() { Text = "职称", Value = nameof(TechEntity.techName) },
            new() { Text = "状态", Value = "UserStatus" },
            new() { Text = "操作", Value = "Action", Sortable = false ,Width=250 }
        };

    public List<InputItem> TableQueryItem()
        =>
        new()
        {
            //new InputItem{ Flag = true, LabelTxt ="账号", InputName= nameof(UserEntity.userCode), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="姓名", InputName= nameof(UserEntity.userName), InputType = InputItemType.String },
            new InputItem{ Flag = true, LabelTxt ="所属单位", InputName= nameof(UserEntity.userComCode),InputType = InputItemType.Select, SelectCombox = BDA_GetSql().Queryable<CompanyEntity>().OrderBy(_a=>_a.comName).Select(_a=>new ComboxItem{ Key = _a.comName, Value =_a.comCode } ).ToList() },
            new InputItem{ Flag = true, LabelTxt ="所属部门", InputName= nameof(UserEntity.userDeptCode),InputType = InputItemType.Select, SelectCombox = BDA_GetSql().Queryable<DeptEntity>().OrderBy(_a => _a.deptName).Select(_a => new ComboxItem { Key = _a.deptName, Value = _a.deptCode } ).ToList() }
        };

    public List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        var rows = BDA_GetSql().User_Query(queryItems,out totalCount, pageIndex, pageSize);
        return rows.Cast<object>().ToList();
    }

    public async Task SaveFunc(List<InputItem> inputItems, bool _isNew)
    {
        await BDA_GetSql().User_Save(inputItems.ToObj<UserEntity>(), _isNew);
    }
}