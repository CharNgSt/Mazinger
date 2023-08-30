using Mazinger.DataAccess.Base;
/// <summary>
/// 默认
/// </summary>
[Injection(Named = nameof(DefaultDataAccess))]
public class DefaultDataAccess : BaseDataAccess, IDataAccessInterface, IScoped
{
    public DefaultDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(httpContextAccessor, db) { }
    public Type InputEntity() => null;
    public List<InputItem> DefaultInputItems() => null;
    public List<DataTableHeader<object>> TableHeader() => null;
    public List<InputItem> TableQueryItem() => null;
    public List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10)
    {
        totalCount = 0;
        return null;
    }
    public async Task SaveFunc(List<InputItem> inputItems, bool _isNew) { }
}