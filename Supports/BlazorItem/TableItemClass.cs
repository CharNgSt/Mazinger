/// <summary>
/// 表格类
/// </summary>
public class TableItemClass
{
    #region 对象查询

    /// <summary>
    /// 可查询对象
    /// </summary>
    public List<InputItem> QueryItems { get; set; } = new List<InputItem> { };

    /// <summary>
    /// 表头
    /// </summary>
    public List<DataTableHeader<object>> Headers { get; set; } = new List<DataTableHeader<object>> { };

    /// <summary>
    /// 操作按钮组
    /// </summary>
    public List<TableButtonClass> Buttons { get; set; } = new List<TableButtonClass> { };

    /// <summary>
    /// 数据行
    /// </summary>
    public List<object> DataRows { get; set; } = new List<object> { };

    /// <summary>
    /// 查询函数
    /// </summary>
    public EventCallback<int> QueryFunc { get; set; }

    /// <summary>
    /// 加载页
    /// </summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 每页展示
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount => (int)Math.Ceiling(CurrentCount / (double)PageSize);

    /// <summary>
    /// 总条数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 加载中
    /// </summary>
    public bool loading { get; set; } = true;

    /// <summary>
    /// 开始查询，用于更新查询状态变量
    /// </summary>
    /// <returns></returns>
    public async Task StartLoading()
    {

        loading = true;
        DataRows = new List<object> { };
    }

    /// <summary>
    /// 查询函数
    /// </summary>
    public QueryFunctionDelegate QueryFunction { get; set; }
    public async Task ExecQuery(int pageIndex)
    {
        //loading = true;
        //DataRows = new List<object> { };
        int _totalCount = 0;
        PageIndex = pageIndex;
        if (QueryFunction != null) DataRows = QueryFunction.Invoke(QueryItems, out _totalCount, PageIndex, PageSize);
        CurrentCount = _totalCount;
        loading = false;
    }

    #endregion

    #region 对象编辑

    /// <summary>
    /// 当前编辑为新增还是修改
    /// </summary>
    public bool InputIsNew { get; set;}
    /// <summary>
    /// 新增对象名称
    /// </summary>
    public string InputPageName { get; set; }
    /// <summary>
    /// Input类型，默认为null。对其赋值表示需要修改功能，会显示新增按钮
    /// </summary>
    public Type InputEntity { get; set; } = null;
    /// <summary>
    /// 新增对象
    /// </summary>
    public List<InputItem> InputItems { get; set; } = new List<InputItem> { };
    /// <summary>
    /// 控制新增页面显示与隐藏
    /// </summary>
    public bool InputPageShow { get; set; }
    /// <summary>
    /// 保存函数(bool 值为是否新增)
    /// </summary>
    public EventCallback SaveFunc { get; set; }

    #endregion

    #region 弹窗

    /// <summary>
    /// 弹窗显示
    /// </summary>
    public bool DialogShow { set; get; }
    /// <summary>
    /// 弹窗消息图标
    /// </summary>
    public string DialogIcon { set; get; }
    /// <summary>
    /// 弹窗主信息
    /// </summary>
    public string DialogMsg { set; get; }
    /// <summary>
    /// 弹窗宽度
    /// </summary>
    public StringNumber DialogWidth { set; get; } = "400";
    /// <summary>
    /// 弹窗副信息
    /// </summary>
    public string DialogSubMsg { set; get; }
    /// <summary>
    /// 是否显示取消按钮
    /// </summary>
    public bool DialogCancelBtnVisable { set; get; }
    /// <summary>
    /// 提交执行函数
    /// </summary>
    public EventCallback DialogConfirmFunc { get; set; }

    #endregion

}

/// <summary>
/// 功能函数
/// </summary>
public static class TableItemFunc
{
    /// <summary>
    /// 弹窗
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_msg"></param>
    /// <param name="_confirmFunc"></param>
    /// <param name="_cancelBtnVisable"></param>
    /// <param name="_sub_msg"></param>
    /// <param name="_icon"></param>
    public static void CallDialog(this TableItemClass _item,string _msg,EventCallback _confirmFunc,StringNumber _width = null, bool _cancelBtnVisable = false, string _sub_msg ="",string _icon = "")
    {
        _item.DialogMsg = _msg;
        _item.DialogSubMsg = _sub_msg;
        _item.DialogWidth = _width == null ? 400 : _width;
        _item.DialogIcon = !string.IsNullOrEmpty(_icon)? _icon : "mdi-help-rhombus";
        _item.DialogCancelBtnVisable = _cancelBtnVisable;
        _item.DialogConfirmFunc = _confirmFunc;
        _item.DialogShow = true;
    }

    public static void CleanInputItemVal(this TableItemClass _item)
    {
        _item.InputItems.ForEach(_a => _a.InputVal = null);
    }

    public static void SetInputItemsVal<T>(this TableItemClass _item, object _vals)
    {
        //需要在这里映射一次，将传入的对象中不属于T泛型的列名去掉。 如果不去掉会导致获取sugarColumnAttribute时报错。
        _vals = _vals.Adapt<T>();
        var properties = typeof(T).GetProperties();
        _item.InputItems.ForEach(_a =>
        {
            foreach (var property in properties)
            {
                var sugarColumnAttribute = (SugarColumn)Attribute.GetCustomAttribute(property, typeof(SugarColumn));
                if (sugarColumnAttribute.ColumnName == _a.InputName)
                {
                    _a.InputVal = property.GetValue(_vals) == null ? null : property.GetValue(_vals)?.ToString(); //property.GetValue(_vals)?.ToString();
                    break;
                }
            }
        });
    }

    

}

/// <summary>
/// 委托函数
/// </summary>
/// <param name="queryItems"></param>
/// <param name="totalCount"></param>
/// <param name="pageIndex"></param>
/// <param name="pageSize"></param>
/// <returns></returns>
public delegate List<object> QueryFunctionDelegate(List<InputItem> queryItems, out int totalCount, int pageIndex = 1, int pageSize = 10);