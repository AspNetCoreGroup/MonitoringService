using Microsoft.EntityFrameworkCore;
using ModelLibrary.Model;
using MonitoringCommonLibrary.Interfaces.Services;
using MonitoringService.DataSources;
using MonitoringService.Model.Entities;

namespace MonitoringService.Services
{
    public class NetworksService : INetworksService
    {
        #region Инициализация

        private ILogger Logger { get; set; }

        private MonitoringContext Context { get; set; }


        public NetworksService(ILoggerFactory loggerFactory, MonitoringContext context)
        {
            Logger = loggerFactory.CreateLogger<NetworksService>();
            Context = context;
        }

        #endregion

        #region Функционал

        public async Task<NetworkDto> GetNetworkAsync(int networkID)
        {
            var network = await Context.Networks.FindAsync(networkID)
                ?? throw new KeyNotFoundException($"Network with networkID {networkID}");


            return Convert(network);
        }

        public async Task<IEnumerable<NetworkDto>> GetNetworksAsync()
        {
            var networks = await Context.Networks.ToListAsync();
            return networks.Select(Convert);
        }

        public async Task CreateNetworkAsync(NetworkDto network)
        {
            var newNetwork = new Network()
            {
                NetworkID = network.NetworkID,
            };

            Context.Networks.Add(newNetwork);

            await Context.SaveChangesAsync();
        }

        public async Task UpdateNetworkAsync(int networkID, NetworkDto network)
        {
            await Task.CompletedTask;

            /*
            var updNetwork = await Context.Networks.FindAsync(networkID)
                ?? throw new KeyNotFoundException($"Network with networkID {networkID}");

            await Context.SaveChangesAsync();
            */
        }

        public async Task DeleteNetworkAsync(int networkID)
        {
            var network = await Context.Networks.FindAsync(networkID)
                ?? throw new KeyNotFoundException($"Network with networkID {networkID}");

            Context.Networks.Remove(network);

            await Context.SaveChangesAsync();
        }

        #endregion

        #region Вспомогательное

        private static NetworkDto Convert(Network network)
        {
            return new NetworkDto()
            {
                NetworkID = network.NetworkID,
                NetworkTitle = "NULL"
            };
        }

        #endregion
    }
}