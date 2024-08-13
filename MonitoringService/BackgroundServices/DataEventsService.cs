using System.Text.Json;
using MonitoringCommonLibrary.Interfaces.BackgroundServices;
using MonitoringCommonLibrary.Interfaces.Services;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces.Listeners;
using ModelLibrary.Messages;
using ModelLibrary.Model;
using ModelLibrary.Model.Enums;

namespace MonitoringService.BackgroundServices
{
    public class DataEventsService : IDataEventsService
    {
        private IConfiguration Configuration { get; }

        private ILogger Logger { get; }

        private IUsersService UsersService { get; }

        private IDevicesService DevicesService { get; }

        private INetworksService NetworksService { get; }

        private INetworkUsersService NetworkUsersService { get; }

        private INetworkDevicesService NetworkDevicesService { get; }

        private IMessageListenerFactory MessageListenerFactory { get; }


        public DataEventsService(IConfiguration configuration, IDevicesService devicesService, IUsersService usersService, INetworksService networksService, INetworkDevicesService networkDevicesService, INetworkUsersService networkUsersService, IMessageListenerFactory messageListenerFactory, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            UsersService = usersService;
            DevicesService = devicesService;
            NetworksService = networksService;
            NetworkUsersService = networkUsersService;
            NetworkDevicesService = networkDevicesService;
            MessageListenerFactory = messageListenerFactory;
            Logger = loggerFactory.CreateLogger<DataEventsBackgroundService>();
        }

        public async Task WorkAsync(CancellationToken stoppingToken)
        {
            try
            {
                Logger.LogInformation("Сервис запущен.");

                using var listener = MessageListenerFactory.CreateListener("DataEventsMonitoring");

                listener.AddHandler(ProcessMessage);

                listener.Open();

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(200, stoppingToken);
                }

                await Task.CompletedTask;

                Logger.LogInformation("Сервис завершил работу.");
            }
            catch (TaskCanceledException)
            {
                Logger.LogInformation("Сервис завершил работу.");
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Ошибка при запуске фонового сервиса.");
            }
        }

        private void ProcessMessage(string queueName, string message, Dictionary<string, string> param)
        {
            try
            {
                Logger.LogInformation("Получено сообщение \"{queueName}\"  - \"{message}\".", queueName, message);

                var contentType = param["ContentType"];

                if (!contentType.StartsWith("DataEventMessage"))
                {
                    throw new Exception("В очереди изменений разрешается использовать только сообщения с типом DataEventMessage");
                }

                if (contentType == "DataEventMessage<UserDto>")
                {
                    var dataEvent = DeserializeMessage<DataEventMessage<UserDto>>(message);
                    ProcessUserMessage(dataEvent).Wait();
                }

                if (contentType == "DataEventMessage<DeviceDto>")
                {
                    var dataEvent = DeserializeMessage<DataEventMessage<DeviceDto>>(message);
                    ProcessDeviceMessage(dataEvent).Wait();
                }

                if (contentType == "DataEventMessage<UserDto>")
                {
                    var dataEvent = DeserializeMessage<DataEventMessage<UserDto>>(message);
                    ProcessUserMessage(dataEvent).Wait();
                }

                if (contentType == "DataEventMessage<NetworkUserDto>")
                {
                    var dataEvent = DeserializeMessage<DataEventMessage<NetworkUserDto>>(message);
                    ProcessNetworkUserMessage(dataEvent).Wait();
                }

                if (contentType == "DataEventMessage<NetworkDeviceDto>")
                {
                    var dataEvent = DeserializeMessage<DataEventMessage<NetworkDeviceDto>>(message);
                    ProcessNetworkDeviceMessage(dataEvent).Wait();
                }

                Logger.LogInformation("Обработано сообщение \"{queueName}\"  - \"{message}\".", queueName, message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Ошибка при обработке сообщения \"{queueName}\"  - \"{message}\".", queueName, message);
            }
        }

        private T DeserializeMessage<T>(string message)
        {
            return JsonSerializer.Deserialize<T>(message) ?? throw new Exception($"Ошибка десериализации \"{message}\" в {nameof(T)}");
        }

        private async Task ProcessUserMessage(DataEventMessage<UserDto> userDataEvent)
        {
            if (userDataEvent.Operation == DataEventOperationType.Add)
            {
                await UsersService.CreateUserAsync(userDataEvent.Data!);
            }
            else if (userDataEvent.Operation == DataEventOperationType.Update)
            {
                await UsersService.UpdateUserAsync(userDataEvent.Data!.UserID, userDataEvent.Data!);
            }
            else if (userDataEvent.Operation == DataEventOperationType.Delete)
            {
                await UsersService.DeleteUserAsync(userDataEvent.Data!.UserID);
            }
        }

        private async Task ProcessDeviceMessage(DataEventMessage<DeviceDto> deviceDataEvent)
        {
            if (deviceDataEvent.Operation == DataEventOperationType.Add)
            {
                await DevicesService.CreateDeviceAsync(deviceDataEvent.Data!);
            }
            else if (deviceDataEvent.Operation == DataEventOperationType.Update)
            {
                await DevicesService.UpdateDeviceAsync(deviceDataEvent.Data!.DeviceID, deviceDataEvent.Data!);
            }
            else if (deviceDataEvent.Operation == DataEventOperationType.Delete)
            {
                await DevicesService.DeleteDeviceAsync(deviceDataEvent.Data!.DeviceID);
            }
        }

        private async Task ProcessNetworkMessage(DataEventMessage<NetworkDto> networkDataEvent)
        {
            if (networkDataEvent.Operation == DataEventOperationType.Add)
            {
                await NetworksService.CreateNetworkAsync(networkDataEvent.Data!);
            }
            else if (networkDataEvent.Operation == DataEventOperationType.Update)
            {
                await NetworksService.UpdateNetworkAsync(networkDataEvent.Data!.NetworkID, networkDataEvent.Data!);
            }
            else if (networkDataEvent.Operation == DataEventOperationType.Delete)
            {
                await NetworksService.DeleteNetworkAsync(networkDataEvent.Data!.NetworkID);
            }
        }

        private async Task ProcessNetworkUserMessage(DataEventMessage<NetworkUserDto> userDataEvent)
        {
            if (userDataEvent.Operation == DataEventOperationType.Add)
            {
                await NetworkUsersService.CreateNetworkUserAsync(userDataEvent.Data!);
            }
            else if (userDataEvent.Operation == DataEventOperationType.Update)
            {
                await NetworkUsersService.UpdateNetworkUserAsync(userDataEvent.Data!.NetworkUserID, userDataEvent.Data!);
            }
            else if (userDataEvent.Operation == DataEventOperationType.Delete)
            {
                await NetworkUsersService.DeleteNetworkUserAsync(userDataEvent.Data!.NetworkUserID);
            }
        }

        private async Task ProcessNetworkDeviceMessage(DataEventMessage<NetworkDeviceDto> userDataEvent)
        {
            if (userDataEvent.Operation == DataEventOperationType.Add)
            {
                await NetworkDevicesService.CreateNetworkDeviceAsync(userDataEvent.Data!);
            }
            else if (userDataEvent.Operation == DataEventOperationType.Update)
            {
                await NetworkDevicesService.UpdateNetworkDeviceAsync(userDataEvent.Data!.NetworkDeviceID, userDataEvent.Data!);
            }
            else if (userDataEvent.Operation == DataEventOperationType.Delete)
            {
                await NetworkDevicesService.DeleteNetworkDeviceAsync(userDataEvent.Data!.NetworkDeviceID);
            }
        }
    }
}