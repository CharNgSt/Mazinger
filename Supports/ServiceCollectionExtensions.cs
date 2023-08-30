public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlobalForServer(this IServiceCollection services)
    {
        //var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new Exception("Get the assembly root directory exception!");

        //LocalStorage
        services.AddScoped<MazingerLocalStorage>();
        //cookies
        services.AddScoped<MazingerCookieStorage>();
        //页面跳转
        services.AddScoped<MazingerNavigationManager>();
        //缓存
        services.AddScoped<MazingerMemoryCache>();

        //文件下载
        services.AddBlazorDownloadFile();
        //数据库加载
        services.SaasSetup();

        return services;
    }
}
