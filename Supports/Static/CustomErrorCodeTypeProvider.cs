using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;

public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        // 获取 HttpContext 和 HttpRequest 对象
        var httpContext = App.HttpContext;
        var httpRequest = httpContext.Request;
        // 获取客户端 Ipv4 地址
        var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
        // 获取请求的 Url 地址
        var requestUrl = httpRequest.GetRequestUrlAddress();
        // 获取来源 Url 地址
        var refererUrl = httpRequest.GetRefererUrlAddress();
        // 获取请求参数（写入日志，需序列化成字符串后存储）
        string method = "";
        try
        {
            using var mem = new MemoryStream();
            using var reader = new StreamReader(mem);
            httpRequest.Body.Seek(0, SeekOrigin.Begin);
            httpRequest.Body.CopyTo(mem);
            mem.Seek(0, SeekOrigin.Begin);
            method = reader.ReadToEnd();
        }
        catch (Exception ex) { }

        // 这里写入日志~~~~~~~~~~~~~~~~~~~~
        StringBuilder _log = new StringBuilder();
        _log.AppendLine($"客户端Ip：{remoteIPv4}");
        _log.AppendLine($"来源Url：{refererUrl}");
        _log.AppendLine($"请求Url：{requestUrl}");
        _log.AppendLine($"请求参数：{method}");
        _log.AppendLine($"错误信息：{context.Exception.ToString()}");

        _log.WriteLog("Error");



        return Task.CompletedTask;
    }
}