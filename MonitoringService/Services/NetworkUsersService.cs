using Microsoft.EntityFrameworkCore;
using ModelLibrary.Model;
using MonitoringCommonLibrary.Interfaces.Services;
using MonitoringService.DataSources;
using MonitoringService.Model.Entities;

namespace MonitoringService.Services
{
    public class NetworkUsersService : INetworkUsersService
    {
        #region Инициализация

        private ILogger Logger { get; set; }

        private MonitoringContext Context { get; set; }


        public NetworkUsersService(ILoggerFactory loggerFactory, MonitoringContext context)
        {
            Logger = loggerFactory.CreateLogger<NetworksService>();
            Context = context;
        }

        #endregion

        #region Вспомогательное

        public async Task<int?> FindNetworkUserIDAsync(int networkID, int userID)
        {
            var user = await Context.NetworkUsers.FirstOrDefaultAsync(x => x.NetworkID == networkID && x.UserID == userID);

            return user?.NetworkUserID;
        }

        public async Task<NetworkUserDto> GetNetworkUserAsync(int networkUserID)
        {
            var user = await Context.NetworkUsers.FindAsync(networkUserID)
                ?? throw new KeyNotFoundException($"NetworkUser with networkUserID {networkUserID}");

            return Convert(user);
        }

        public async Task<IEnumerable<NetworkUserDto>> GetNetworkUsersAsync()
        {
            var users = await Context.NetworkUsers.ToListAsync();
            return users.Select(Convert);
        }

        public async Task<IEnumerable<NetworkUserDto>> GetNetworkUsersAsync(int networkID)
        {
            var users = await Context.NetworkUsers.Where(x => x.NetworkID == networkID).ToListAsync();
            return users.Select(Convert);
        }

        public async Task CreateNetworkUserAsync(NetworkUserDto networkUser)
        {
            var user = new NetworkUser()
            {
                NetworkUserID = networkUser.NetworkUserID,
                UserID = networkUser.UserID,
                NetworkID = networkUser.NetworkID
            };

            Context.NetworkUsers.Add(user);

            await Context.SaveChangesAsync();
        }

        public async Task UpdateNetworkUserAsync(int networkUserID, NetworkUserDto networkUser)
        {
            var user = await Context.NetworkUsers.FindAsync(networkUserID)
                ?? throw new KeyNotFoundException($"NetworkUser with networkUserID {networkUserID}");

            user.UserID = networkUser.UserID;
            user.NetworkID = networkUser.NetworkID;

            Context.NetworkUsers.Add(user);

            await Context.SaveChangesAsync();
        }

        public async Task DeleteNetworkUserAsync(int networkUserID)
        {
            var user = await Context.NetworkUsers.FindAsync(networkUserID)
                ?? throw new KeyNotFoundException($"NetworkUser with networkUserID {networkUserID}");

            Context.NetworkUsers.Remove(user);

            await Context.SaveChangesAsync();
        }

        #endregion

        #region Вспомогательное

        private static NetworkUserDto Convert(NetworkUser networkUser)
        {
            return new NetworkUserDto()
            {
                NetworkUserID = networkUser.NetworkUserID,
                NetworkID = networkUser.NetworkID,
                UserID = networkUser.UserID
            };
        }

        #endregion
    }
}