using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OneOf.Types;
using SqlSugar.Extensions;
using System.Linq.Expressions;
/// <summary>
/// 输入对象
/// </summary>
public class InputItem
{
    /// <summary>
    /// 判断是否为主键、或是否为快捷查询，默认false
    /// </summary>
    public bool Flag { get; set; } = false;
    /// <summary>
    /// 数据列名(英文)
    /// </summary>
    public string InputName { get; set; }
    /// <summary>
    /// 查询值
    /// </summary>
    public string? InputVal { get; set; }
    /// <summary>
    /// 中文名
    /// </summary>
    public string? LabelTxt { get; set; }
    /// <summary>
    /// 提示文字
    /// </summary>
    public string? HolderTxt { get; set; }
    /// <summary>
    /// 能否为空
    /// </summary>
    public bool Nullable { get; set; } = true;
    /// <summary>
    /// 值类型
    /// </summary>
    public InputItemType InputType { get; set; }
    /// <summary>
    /// 前端控件使用的参数，例如datepicker需要一个变量用于控制展示
    /// </summary>
    public bool ItemFlag { get; set; }
    /// <summary>
    /// 选择框内容
    /// </summary>
    public List<ComboxItem>? SelectCombox { get; set; } = null;

}

public static class InputItemFunc
{
    /// <summary>
    /// 遍历对象，转为Json对象
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T ToObj<T>(this List<InputItem>? list)
    {
        var instance = Activator.CreateInstance<T>();
        list?.ForEach(item =>
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    var sugarColumnAttribute = (SugarColumn)Attribute.GetCustomAttribute(property, typeof(SugarColumn));
                    if (sugarColumnAttribute.ColumnName == item.InputName)
                    {
                        switch (item.InputType)
                        {
                            case InputItemType.Int:
                            case InputItemType.Double:
                                property.SetValue(instance, item.InputVal.ObjToDecimal());
                                break;
                            case InputItemType.Date:
                                DateTime? _inputDate = null;
                                if (DateTime.TryParse(item.InputVal, out DateTime _inputDate1)) _inputDate = _inputDate1;
                                property.SetValue(instance, _inputDate);
                                break;
                            default:
                                property.SetValue(instance, item.InputVal);
                                break;
                        }
                    }
                }
                catch (Exception ex) {
                    //Console.WriteLine($"{item.InputName},{item.InputVal}");                    
                    throw ex;                 
                }
            }
        });

        return instance;
    }

    /// <summary>
    /// 遍历对象，转为List<InputItem>保存对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static List<InputItem> ToInputItemList<T>(this T entity)
    {
        List<InputItem> result = new List<InputItem>();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var sugarColumnAttribute = (SugarColumn)Attribute.GetCustomAttribute(property, typeof(SugarColumn));
            result.Add(
                new InputItem
                {
                    InputName = sugarColumnAttribute.ColumnName,
                    LabelTxt = sugarColumnAttribute.ColumnDescription,
                    Flag = sugarColumnAttribute.IsPrimaryKey,
                    Nullable = sugarColumnAttribute.IsNullable,
                    InputType = IsDateType(property.PropertyType) ? InputItemType.Date : InputItemType.String,
                    InputVal = property.GetValue(entity) == null ? null : property.GetValue(entity)?.ToString()
                });
        }
        return result;
    }

    private static bool IsDateType(Type propertyType)
    {
        return propertyType == typeof(DateTime) || propertyType == typeof(DateTime?);
    }

    private static InputItemType GetInputType(Type propertyType)
    {
        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            return InputItemType.Date;
        else if(propertyType == typeof(decimal) || propertyType == typeof(decimal?) || propertyType == typeof(int) || propertyType == typeof(int?))
            return InputItemType.Int;
        else 
            return InputItemType.String;


    }


    public static List<InputItem> EntityToInputItemList(this Type entityType)
    {

        List<InputItem> result = new List<InputItem>();
        PropertyInfo[] properties = entityType.GetProperties();
        foreach (PropertyInfo property in properties)
        {

            var sugarColumnAttribute = (SugarColumn)Attribute.GetCustomAttribute(property, typeof(SugarColumn));
            result.Add(
                new InputItem
                {
                    InputName = sugarColumnAttribute.ColumnName,
                    LabelTxt = sugarColumnAttribute.ColumnDescription,
                    Flag = sugarColumnAttribute.IsPrimaryKey,
                    Nullable = sugarColumnAttribute.IsNullable,
                    InputType = GetInputType(property.PropertyType) //IsDateType(property.PropertyType) ? InputItemType.Date : InputItemType.String
                });
        }
        return result;
    }

}




/// <summary>
/// 选择框
/// </summary>
public class ComboxItem
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// Input类型
/// </summary>
public enum InputItemType
{
    String,
    Date,
    Int,
    Double,
    Select,
    Textarea,
    Password
}