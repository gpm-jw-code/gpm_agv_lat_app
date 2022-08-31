using GPM_AGV_LAT_CORE;
using Microsoft.AspNetCore.Http.Json;
using GPM_AGV_LAT_APP.SingalRHubs;
using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers;
using GPM_AGV_LAT_APP.Models;

Startup.StartService();

OrderManerger.OnNewOrderCreate += WebsocketClientManager.BrocastOrder;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseWebSockets();
app.UseCors(options => options.WithOrigins("http://localhost:8080").AllowCredentials().AllowAnyHeader().AllowAnyMethod());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<AGVSOrderHub>("/OrdersHub");
app.Run();