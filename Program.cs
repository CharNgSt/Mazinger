using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args).Inject();

//配置logging
builder.Logging.AddFilter((provider, category, logLevel) => { return !new[] { "Microsoft.Hosting", "Microsoft.AspNetCore" }.Any(u => category.StartsWith(u)) && logLevel >= LogLevel.None; });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Add services to the container.
builder.Services.AddMasaBlazor(options =>
{
    options.ConfigureTheme(theme =>
    {
        //theme.Dark = true;
        //theme.Themes.Dark.Primary = "XXX"; // 支持配置暗主题的预设颜色

        //浅云 #EDEDF4   东方既白 #8EA4CA
        //明月珰 #D6D4CD 石榴裙 #B3392F
        //栀子 #FEC13F  绀蝶 #2D2E3D
        //碧落 #AFD0F2  绀宇 #003C75
        //奶酪 #FBF7D8  蒂芙尼蓝 #81D8D1
        //女贞黄 #FBEEAF  鹤顶红 #D44636
        //琥珀黄 #F9C701  青雀头戴 #194955
        //淡黄色 #FCEAD5  中国红 #FE0000
        //玉色 #FAF8FA  碧山 #8BAF56

        theme.Themes.Light.Primary = "System:Color:Primary".GetConfigVal();
        theme.Themes.Light.Success = "System:Color:Success".GetConfigVal();
        theme.Themes.Light.Warning = "System:Color:Warning".GetConfigVal();
        theme.Themes.Light.Info = "System:Color:Info".GetConfigVal();
        theme.Themes.Light.Error = "System:Color:Error".GetConfigVal();

    });
});
builder.Services.AddGlobalForServer();

//**********
//local storage 支持
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);
//**********

var url = "urls".GetConfigVal();
var template = TP.Wrapper(
  "System:Name".GetConfigVal(), 
  "系统信息",
  $"##启动时间## {DateTime.Now}",
  $"##访问地址## {url}",
  $"##Copyright## 广州市飞时信息科技有限公司");
Console.WriteLine(template);


builder.Services.AddRemoteRequest();
builder.Services.AddControllers()
                 .AddFriendlyException()
                 .AddInject();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.UseInjectBase();

if (Directory.Exists($"{AppContext.BaseDirectory}\\PdfFile".GetRuntimeDirectory())) Directory.CreateDirectory($"{AppContext.BaseDirectory}\\PdfFile".GetRuntimeDirectory());
app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider($"{AppContext.BaseDirectory}\\PdfFile".GetRuntimeDirectory()), RequestPath = "/pdf" });

app.Run();


