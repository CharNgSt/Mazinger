/// <summary>
/// 粘土对象函数
/// </summary>
public static class StaticClay
{
    public static string GetKeyVal(this Clay _item, string _keyName)
    {
        var _res = "";
        foreach (KeyValuePair<string, dynamic> item in _item)
        {
            if (item.Key == _keyName)
            {
                _res = item.Value;
                break;
            }
        }
        return _res;
    }
    public static string GetKeyVal(this object _item, string? _keyName)
    {
        if (string.IsNullOrEmpty(_keyName)) return "";
        var _res = "";
        Type objectType = _item.GetType();
        PropertyInfo[] properties = objectType.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.Name == _keyName)
            {
                _res = property.GetValue(_item)?.ToString();
                break;
            }
        }
        return _res;
    }

    public static string GetKeyVal(this List<InputItem> _list,string _keyName)
    {
        var _res = "";
        _list.ForEach(_a => {
            if (_a.InputName == _keyName)
            {
                _res = _a.InputVal;
                return;
            }
        });
        return _res;
    }

    public static bool GetKeyHasVal(this List<InputItem> _list, string _keyName)
    {
        return !string.IsNullOrEmpty(_list.GetKeyVal(_keyName));
    }
}

