using ModelLibrary.Model;

namespace MonitoringCommonLibrary.Interfaces.Services
{
    public interface INetworkDevicesService
    {
        public Task<int?> FindNetworkDeviceIDAsync(int networkID, int deviceID);

        public Task<NetworkDeviceDto> GetNetworkDeviceAsync(int networkDeviceID);

        public Task<IEnumerable<NetworkDeviceDto>> GetNetworkDevicesAsync();

        public Task<IEnumerable<NetworkDeviceDto>> GetNetworkDevicesAsync(int networkID);

        public Task CreateNetworkDeviceAsync(NetworkDeviceDto networkDevice);

        public Task UpdateNetworkDeviceAsync(int networkDeviceID, NetworkDeviceDto networkDevice);

        public Task DeleteNetworkDeviceAsync(int networkDeviceID);
    }
}