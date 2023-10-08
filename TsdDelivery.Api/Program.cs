using Hangfire;
using HangfireBasicAuthenticationFilter;
using TsdDelivery.Api;
using TsdDelivery.Api.Middlewares;
using TsdDelivery.Application.Commons;
using TsdDelivery.Application.Interface;
using TsdDelivery.Infrastructures;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppConfiguration>();

builder.Services.AddInfrastructuresService(configuration.DatabaseConnection,configuration.RedisConnection);
builder.Services.AddWebAPIService(configuration.JwtSettings);
builder.Services.AddSingleton(configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000",
                "http://localhost:3001",
                "https://exe202.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "TsdDelivery API V1");
    });
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "TsdDelivery API V1");
});

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "My Website",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter{
            User = "admin",
            Pass = "123456"
        }
    }
});

app.Run();
RecurringJob.AddOrUpdate<IBackgroundService>(x => x.AutoResetCacheUserLoginCount(),Cron.Daily(17, 0));
public partial class Program { }