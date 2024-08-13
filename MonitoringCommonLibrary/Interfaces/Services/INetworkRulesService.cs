using ModelLibrary.Model;

namespace MonitoringCommonLibrary.Interfaces.Services
{
    public interface INetworkRulesService
    {
        public Task<NetworkRuleDto> GetNetworkRuleAsync(int networkRuleID);

        public Task<IEnumerable<NetworkRuleDto>> GetNetworkRulesAsync();

        public Task<IEnumerable<NetworkRuleDto>> GetNetworkRulesAsync(int networkID);

        public Task CreateNetworkRuleAsync(NetworkRuleDto networkRule);

        public Task UpdateNetworkRuleAsync(int networkRuleID, NetworkRuleDto networkRule);

        public Task DeleteNetworkRuleAsync(int networkRuleID);
    }
}