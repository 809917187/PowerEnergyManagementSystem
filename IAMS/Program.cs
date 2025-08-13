using IAMS.MQTT;
using IAMS.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try {
    var builder = WebApplication.CreateBuilder(args);

    string connectionString_mysql = builder.Configuration.GetConnectionString("ems");
    string connectionString_clickhouse = builder.Configuration.GetConnectionString("ems_clickhouse");
    MQTTHelper.SetConnectionString(connectionString_mysql, connectionString_clickhouse);

    // 1. 清除默认日志提供程序
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    // 2. 使用 NLog 作为日志提供程序
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(option => {
            option.LoginPath = "/Access/Login";
            option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        });
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IPowerStationService, PowerStationService>();
    builder.Services.AddScoped<ITemplateService, TemplateService>();
    builder.Services.AddScoped<IStationSystemService, StationSystemService>();
    builder.Services.AddScoped<IDeviceMonitorService, DeviceMonitorService>();
    builder.Services.AddScoped<IPowerStationOverviewService, PowerStationOverviewService>();
    builder.Services.AddScoped<IMultiSatationOverviewService, MultiSatationOverviewService>();
    builder.Services.AddScoped<IElectricityReportService, ElectricityReportService>();
    builder.Services.AddScoped<IClickHouseService, ClickHouseService>();
    builder.Services.AddHostedService<MqttSubscribeService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment()) {
        app.UseExceptionHandler("/Home/Error");
        //app.UseHsts();
    } else {
        app.UseExceptionHandler("/Home/Error");
        //app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Access}/{action=Login}/{id?}");

    app.Run();
} catch (Exception ex) {
    logger.Error(ex, "应用程序启动失败");
    throw;
} finally {
    NLog.LogManager.Shutdown();
}

