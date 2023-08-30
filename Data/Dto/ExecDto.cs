/// <summary>
/// 查询结果对象
/// </summary>
public class ExecDto
{
    public string guid { get; set; }

    public string queryName { get; set; }
    public string queryCondition{ get; set; }

    public string queryType { get; set; }

    public string queryDb { get; set; }

    public string queryUser { get; set; }

    public DateTime createdAt { get; set; }

    public DateTime? updatedAt { get; set; }

    public string queryResult { get; set; }

    public string queryResultMemo { get; set; }
}
