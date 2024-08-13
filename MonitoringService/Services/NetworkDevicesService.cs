using Microsoft.EntityFrameworkCore;
using ModelLibrary.Model;
using MonitoringCommonLibrary.Interfaces.Services;
using MonitoringService.DataSources;
using MonitoringService.Model.Entities;

namespace MonitoringService.Services
{
    public class NetworkDevicesService : INetworkDevicesService
    {
        #region Инициализация

        private ILogger Logger { get; set; }

        private MonitoringContext Context { get; set; }


        public NetworkDevicesService(ILoggerFactory loggerFactory, MonitoringContext context)
        {
            Logger = loggerFactory.CreateLogger<NetworksService>();
            Context = context;
        }

        #endregion

        #region Вспомогательное

        public async Task<int?> FindNetworkDeviceIDAsync(int networkID, int deviceID)
        {
            var device = await Context.NetworkDevices.FirstOrDefaultAsync(x => x.NetworkID == networkID && x.DeviceID == deviceID);

            return device?.NetworkDeviceID;
        }

        public async Task<NetworkDeviceDto> GetNetworkDeviceAsync(int networkDeviceID)
        {
            var device = await Context.NetworkDevices.FindAsync(networkDeviceID)
                ?? throw new KeyNotFoundException($"NetworkDevice with networkDeviceID {networkDeviceID}");

            return Convert(device);
        }

        public async Task<IEnumerable<NetworkDeviceDto>> GetNetworkDevicesAsync()
        {
            var devices = await Context.NetworkDevices.ToListAsync();
            return devices.Select(Convert);
        }

        public async Task<IEnumerable<NetworkDeviceDto>> GetNetworkDevicesAsync(int networkID)
        {
            var devices = await Context.NetworkDevices.Where(x => x.NetworkID == networkID).ToListAsync();
            return devices.Select(Convert);
        }

        public async Task CreateNetworkDeviceAsync(NetworkDeviceDto networkDevice)
        {
            var device = new NetworkDevice()
            {
                NetworkDeviceID = networkDevice.NetworkDeviceID,
                DeviceID = networkDevice.DeviceID,
                NetworkID = networkDevice.NetworkID
            };

            Context.NetworkDevices.Add(device);

            await Context.SaveChangesAsync();
        }

        public async Task UpdateNetworkDeviceAsync(int networkDeviceID, NetworkDeviceDto networkDevice)
        {
            var device = await Context.NetworkDevices.FindAsync(networkDeviceID)
                ?? throw new KeyNotFoundException($"NetworkDevice with networkDeviceID {networkDeviceID}");

            device.DeviceID = networkDevice.DeviceID;
            device.NetworkID = networkDevice.NetworkID;

            await Context.SaveChangesAsync();
        }

        public async Task DeleteNetworkDeviceAsync(int networkDeviceID)
        {
            var device = await Context.NetworkDevices.FindAsync(networkDeviceID)
                ?? throw new KeyNotFoundException($"NetworkDevice with networkDeviceID {networkDeviceID}");

            Context.NetworkDevices.Remove(device);

            await Context.SaveChangesAsync();
        }

        #endregion

        #region Вспомогательное

        private static NetworkDeviceDto Convert(NetworkDevice networkDevice)
        {
            return new NetworkDeviceDto()
            {
                NetworkDeviceID = networkDevice.NetworkDeviceID,
                NetworkID = networkDevice.NetworkID,
                DeviceID = networkDevice.DeviceID
            };
        }

        #endregion
    }
}