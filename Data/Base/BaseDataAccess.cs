
using System.Linq.Expressions;
/// <summary>
/// 数据接口基础类，构造函数初始化时获取cookie中登录信息
/// </summary>
public class BaseDataAccess
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly ISqlSugarClient _db;
    /// <summary>
    /// 租户代码
    /// </summary>
    protected readonly string tenantCode;
    /// <summary>
    /// 用户账号
    /// </summary>
    protected readonly string userCode;
    /// <summary>
    /// 用户姓名
    /// </summary>
    protected readonly string userName;
    /// <summary>
    /// 用户身份token
    /// </summary>
    protected readonly string authToken;

    public BaseDataAccess(IHttpContextAccessor httpContextAccessor, ISqlSugarClient db)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = db;

        var request = _httpContextAccessor.HttpContext.Request;
        tenantCode = request.Cookies["GlobalConfig_TenantCode"];
        userCode = request.Cookies["GlobalConfig_LoginUserCode"];
        userName = request.Cookies["GlobalConfig_LoginUserName"];
        authToken = request.Cookies["GlobalConfig_LoginAuthToken"];
    }

    /// <summary>
    /// 根据住户获取数据库对象(基础类内置函数)
    /// </summary>
    /// <returns></returns>
    public SqlSugarProvider BDA_GetSql() => _db.GetDb(tenantCode);

    /// <summary>
    /// 获取实体类的列的数据库列名(基础类内置函数)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public string BDA_GetColumnName<T>(Expression<Func<T, object>> expression)
    {
        MemberExpression memberExpression = null;

        if (expression.Body is UnaryExpression unaryExpression)
        {
            memberExpression = unaryExpression.Operand as MemberExpression;
        }
        else if (expression.Body is MemberExpression memExpression)
        {
            memberExpression = memExpression;
        }

        if (memberExpression != null)
        {
            var propertyInfo = memberExpression.Member as PropertyInfo;

            var columnName = propertyInfo.GetCustomAttribute<SugarColumn>(true)?.ColumnName ?? propertyInfo.Name;

            return columnName;
        }

        return null;
    }
}