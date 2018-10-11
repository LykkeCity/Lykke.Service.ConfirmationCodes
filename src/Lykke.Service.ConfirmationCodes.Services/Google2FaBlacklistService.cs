using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.ConfirmationCodes.Core.Services;
using StackExchange.Redis;

namespace Lykke.Service.ConfirmationCodes.Services
{
    [UsedImplicitly]
    public class Google2FaBlacklistService : IGoogle2FaBlacklistService
    {
        private readonly IDatabase _redisDb;
        private readonly int _maxTries;

        public Google2FaBlacklistService(
            IDatabase redisDb,
            int maxTries)
        {
            _redisDb = redisDb;
            _maxTries = maxTries;
        }
        
        public Task ClientFailedAsync(string clientId)
        {
            return _redisDb.StringIncrementAsync(GetCounterKeyForClient(clientId));
        }

        public async Task<bool> IsClientBlockedAsync(string clientId)
        {
            return (int) await _redisDb.StringGetAsync(GetCounterKeyForClient(clientId)) >= _maxTries;
        }

        public async Task ClientSucceededAsync(string clientId)
        {
            var clientKey = GetCounterKeyForClient(clientId);

            const string Script = @"
                local num=redis.call('get', KEYS[1])
                if(num < ARGV[1])
                then
                    redis.call('del', KEYS[1])
                end";
            
            await _redisDb.ScriptEvaluateAsync(Script, new[] {(RedisKey)clientKey}, new[] {(RedisValue)_maxTries});
        }

        private string GetCounterKeyForClient(string clientId)
        {
            return $"Google2FaFailsCounter:{clientId}";
        }
    }
}
