
using System.Linq.Expressions;

public interface IDataAccessInterface
{
    Type InputEntity();

    /// <summary>
    /// 根据租户获取数据库对象
    /// </summary>
    /// <returns></returns>
    SqlSugarProvider BDA_GetSql();

    /// <summary>
    /// 获取实体类的列的数据库列名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    string BDA_GetColumnName<T>(Expression<Func<T, object>> expression);

    /// <summary>
    /// 表头
    /// </summary>
    /// <returns></returns>
    List<DataTableHeader<object>> TableHeader();

    /// <summary>
    /// 查询项
    /// </summary>
    /// <returns></returns>
    List<InputItem> TableQueryItem();

    /// <summary>
    /// 查询函数
    /// </summary>
    /// <param name="queryItems"></param>
    /// <param name="totalCount"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    List<object> QueryFunc(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10);

    /// <summary>
    /// 保存函数
    /// </summary>
    /// <param name="inputItems"></param>
    /// <param name="_isNew"></param>
    /// <returns></returns>
    Task SaveFunc(List<InputItem> inputItems, bool _isNew);

    /// <summary>
    /// 默认编辑对象
    /// </summary>
    List<InputItem> DefaultInputItems();
}
