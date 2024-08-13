using CommonLibrary.Interfaces.Listeners;
using CommonLibrary.Interfaces.Senders;
using CommonLibrary.Interfaces.Services;
using DefaultRealisationLibrary.Factories;
using DefaultRealisationLibrary.Services;
using MonitoringCommonLibrary.Interfaces.BackgroundServices;
using MonitoringCommonLibrary.Interfaces.Services;
using MonitoringService.BackgroundServices;
using MonitoringService.DataSources;
using MonitoringService.Middlewares;
using MonitoringService.Services;
using RabbitLibrary.Listeners;
using RabbitLibrary.Senders;
// TODO: Заменить на встроенный вариант
using IHttpClientFactory = CommonLibrary.Interfaces.Factories.IHttpClientFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddDbContext<MonitoringContext>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IDevicesService, DevicesService>();
builder.Services.AddScoped<INetworksService, NetworksService>();
builder.Services.AddScoped<INetworkDevicesService, NetworkDevicesService>();
builder.Services.AddScoped<INetworkUsersService, NetworkUsersService>();

builder.Services.AddScoped<IDataStorageService, DataStorageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IMessageSender, RabbitSender>();
builder.Services.AddScoped<IMessageListenerFactory, RabbitListenerFactory>();
builder.Services.AddScoped<IRequestsService, HttpRequestService>();
builder.Services.AddScoped<IHttpClientFactory, HttpClientFactory>();

builder.Services.AddScoped<IDataEventsService, DataEventsService>();
builder.Services.AddHostedService<DataEventsBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionsMiddleware>();

app.MapControllers();

app.Run();

