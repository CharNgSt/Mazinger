/// <summary>
/// 按钮类型
/// </summary>
public class TableButtonClass
{
    public string Name { get; set; }

    public string Color { get; set; }

    public string Icon { get; set; }

    public EventCallback<object?> EventCallback { get; set; }

    public string ShowColVal { get; set; }

    public PredicateDelegate? Show { get; set; } = value => true;
}


public delegate bool PredicateDelegate(object? value);