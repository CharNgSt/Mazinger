using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args).Inject();

//����logging
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
        //theme.Themes.Dark.Primary = "XXX"; // ֧�����ð������Ԥ����ɫ

        //ǳ�� #EDEDF4   �����Ȱ� #8EA4CA
        //���«� #D6D4CD ʯ��ȹ #B3392F
        //���� #FEC13F  礵� #2D2E3D
        //���� #AFD0F2  ��� #003C75
        //���� #FBF7D8  ��ܽ���� #81D8D1
        //Ů��� #FBEEAF  �׶��� #D44636
        //����� #F9C701  ��ȸͷ�� #194955
        //����ɫ #FCEAD5  �й��� #FE0000
        //��ɫ #FAF8FA  ��ɽ #8BAF56

        theme.Themes.Light.Primary = "System:Color:Primary".GetConfigVal();
        theme.Themes.Light.Success = "System:Color:Success".GetConfigVal();
        theme.Themes.Light.Warning = "System:Color:Warning".GetConfigVal();
        theme.Themes.Light.Info = "System:Color:Info".GetConfigVal();
        theme.Themes.Light.Error = "System:Color:Error".GetConfigVal();

    });
});
builder.Services.AddGlobalForServer();

//**********
//local storage ֧��
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);
//**********

var url = "urls".GetConfigVal();
var template = TP.Wrapper(
  "System:Name".GetConfigVal(), 
  "ϵͳ��Ϣ",
  $"##����ʱ��## {DateTime.Now}",
  $"##���ʵ�ַ## {url}",
  $"##Copyright## �����з�ʱ��Ϣ�Ƽ����޹�˾");
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


