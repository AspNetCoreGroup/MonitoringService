using Microsoft.EntityFrameworkCore;
using ModelLibrary.Model;
using MonitoringCommonLibrary.Interfaces.Services;
using MonitoringService.DataSources;
using MonitoringService.Model.Entities;

namespace MonitoringService.Services
{
    public class NetworkRulesService : INetworkRulesService
    {
        #region Инициализация

        private ILogger Logger { get; set; }

        private MonitoringContext Context { get; set; }


        public NetworkRulesService(ILoggerFactory loggerFactory, MonitoringContext context)
        {
            Logger = loggerFactory.CreateLogger<NetworksService>();
            Context = context;
        }

        #endregion

        #region Вспомогательное

        public async Task<NetworkRuleDto> GetNetworkRuleAsync(int networkRuleID)
        {
            var Rule = await Context.NetworkRules.FindAsync(networkRuleID)
                ?? throw new KeyNotFoundException($"NetworkRule with networkRuleID {networkRuleID}");

            return Convert(Rule);
        }

        public async Task<IEnumerable<NetworkRuleDto>> GetNetworkRulesAsync()
        {
            var Rules = await Context.NetworkRules.ToListAsync();
            return Rules.Select(Convert);
        }

        public async Task<IEnumerable<NetworkRuleDto>> GetNetworkRulesAsync(int networkID)
        {
            var Rules = await Context.NetworkRules.Where(x => x.NetworkID == networkID).ToListAsync();
            return Rules.Select(Convert);
        }

        public async Task CreateNetworkRuleAsync(NetworkRuleDto networkRule)
        {
            var Rule = new NetworkRule()
            {
                NetworkRuleID = networkRule.NetworkRuleID,
                NetworkID = networkRule.NetworkID,
                UserID = networkRule.UserID,
                NotificationType = networkRule.NotificationType,
                RuleExpression = networkRule.RuleExpression,
            };

            Context.NetworkRules.Add(Rule);

            await Context.SaveChangesAsync();
        }

        public async Task UpdateNetworkRuleAsync(int networkRuleID, NetworkRuleDto networkRule)
        {
            var rule = await Context.NetworkRules.FindAsync(networkRuleID)
                ?? throw new KeyNotFoundException($"NetworkRule with networkRuleID {networkRuleID}");

            rule.NetworkID = networkRule.NetworkID;
            rule.UserID = networkRule.UserID;
            rule.NotificationType = networkRule.NotificationType;
            rule.RuleExpression = networkRule.RuleExpression;

            Context.NetworkRules.Add(rule);

            await Context.SaveChangesAsync();
        }

        public async Task DeleteNetworkRuleAsync(int networkRuleID)
        {
            var Rule = await Context.NetworkRules.FindAsync(networkRuleID)
                ?? throw new KeyNotFoundException($"NetworkRule with networkRuleID {networkRuleID}");

            Context.NetworkRules.Remove(Rule);

            await Context.SaveChangesAsync();
        }

        #endregion

        #region Вспомогательное

        private static NetworkRuleDto Convert(NetworkRule networkRule)
        {
            return new NetworkRuleDto()
            {
                NetworkRuleID = networkRule.NetworkRuleID,
                NetworkID = networkRule.NetworkID,
                UserID = networkRule.UserID,
                NotificationType = networkRule.NotificationType,
                RuleExpression = networkRule.RuleExpression,
            };
        }

        #endregion
    }
}