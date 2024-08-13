using ModelLibrary.Model;

namespace MonitoringCommonLibrary.Interfaces.Services
{
    public interface INetworkUsersService
    {
        public Task<int?> FindNetworkUserIDAsync(int networkID, int userID);

        public Task<NetworkUserDto> GetNetworkUserAsync(int networkUserID);

        public Task<IEnumerable<NetworkUserDto>> GetNetworkUsersAsync();

        public Task<IEnumerable<NetworkUserDto>> GetNetworkUsersAsync(int networkID);

        public Task CreateNetworkUserAsync(NetworkUserDto networkUser);

        public Task UpdateNetworkUserAsync(int networkUserID, NetworkUserDto networkUser);

        public Task DeleteNetworkUserAsync(int networkUserID);
    }
}