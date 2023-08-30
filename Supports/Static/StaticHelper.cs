/// <summary>
/// 工具函数
/// </summary>
public static class StaticHelper
{
    /// <summary>
    /// 根据appsettings构建URL
    /// </summary>
    /// <param name="_url"></param>
    /// <returns></returns>
    public static string GetUslStr(this string _url)
    {
        return  $"{"System:ApiUrl".GetConfigVal()}{(_url.Substring(0, 1) == "/" ? "" : "/")}{_url}";
    }

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_headers"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    public static string UrlGet(this string _url, int timeOut = 30000)=> _url.UrlGet(null, timeOut);

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_headers"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    public static string UrlGet(this string _url, Dictionary<string, object> _headers = null,int timeOut=30000)
    {
        string result = "";

        _url = _url.GetUslStr();
        using HttpClient httpClient = new HttpClient();
        if (!string.IsNullOrWhiteSpace(_url)) httpClient.BaseAddress = new Uri(_url);
        if (_headers != null && _headers.Any()) 
            foreach (var _h in _headers)
            {
                if(_h.Key.ToLower()== "authorization")
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _h.Value as string);
                else 
                    httpClient.DefaultRequestHeaders.Add(_h.Key, _h.Value as string);
            }

        var _task = httpClient.GetAsync(_url);
        _task.Wait(timeOut);
        var _res = _task.Result;

        if (_res.IsSuccessStatusCode) {

            //result = _res.Content.ReadAsStringAsync().Result; 
            var _postback = _res.Content.ReadAsByteArrayAsync().Result;
            result = System.Text.Encoding.Default.GetString(_postback);
        }


        return result;

    }


    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_body"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    public static string UrlPost(this string _url, out string _authorization, object _body, int timeOut = 30000) => _url.UrlPost( out _authorization, null, _body, timeOut);


    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_headers"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    public static string UrlPost(this string _url,  out string _authorization, Dictionary<string, object> _headers, int timeOut = 30000) => _url.UrlPost(out _authorization, _headers, null, timeOut);

    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_headers"></param>
    /// <param name="_body"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    public static string UrlPost(this string _url,  out string _authorization, Dictionary<string, object> _headers = null, object? _body = null, int timeOut = 30000)
    {

        string _postbackTxt = "";
        _url = _url.GetUslStr();
        HttpContent httpContent = new StringContent(_body.toJsonStr());
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        using HttpClient httpClient = new HttpClient();

        if (!string.IsNullOrWhiteSpace(_url)) httpClient.BaseAddress = new Uri(_url);

        if (_headers != null && _headers.Any())
            foreach (var _h in _headers)
            {
                if (_h.Key.ToLower() == "authorization")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _h.Value.ToString());
                }
                else
                    httpClient.DefaultRequestHeaders.Add(_h.Key, _h.Value.ToString());
            }

        var _task = httpClient.PostAsync(_url, httpContent);
        _task.Wait(timeOut);
        var _res = _task.Result;


        _authorization = "";
        if (_task.Result.Headers.TryGetValues("x-Authorization", out var _val)) _authorization = String.Concat(_val);

        if (_res.IsSuccessStatusCode)
        {
            var _postback = _res.Content.ReadAsByteArrayAsync().Result;
            _postbackTxt = System.Text.Encoding.Default.GetString(_postback);
        }

        return _postbackTxt;
    }

    public static JObject ReqResultDeserializeObj(this string _reqResult,out string _errMsg)
    {
        _errMsg = "";
        var _obj = (JObject)JsonConvert.DeserializeObject(_reqResult);
        _errMsg = _obj.getJsonValue("msg");
        return _obj;
    }

    public static JArray ReqResulDeserializeArray(this string _reqResult)
    {
        var _obj = (JArray)JsonConvert.DeserializeObject(_reqResult);
        return _obj;
    }


}