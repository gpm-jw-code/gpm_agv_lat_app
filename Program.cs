using GPM_AGV_LAT_CORE;
using Microsoft.AspNetCore.Http.Json;
using GPM_AGV_LAT_APP.SingalRHubs;
using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers;
using GPM_AGV_LAT_CORE.Logger;
using GPM_AGV_LAT_APP.Models;

Startup.StartService();

OrderManerger.OnNewOrderCreate += WebsocketClientManager.BrocastOrder;
LoggerInstance.Onlogging += WebsocketClientManager.BrocastLog;
MessageHandShakeLogger.OnHandShakelogging += WebsocketClientManager.BrocastHandShakeLog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "LAT APP", Version = "v1.0", Description = "APIAPI" });
    //var file = Path.Combine(AppContext.BaseDirectory, "app_webapi.xml");
    //var path = Path.Combine(AppContext.BaseDirectory, file);
    //c.IncludeXmlComments(path, true);
    //c.OrderActionsBy(o => o.RelativePath);

});
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.WriteIndented = true;
});
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseWebSockets();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();