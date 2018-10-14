using Autofac;
using Lykke.Service.ConfirmationCodes.Settings;
using Lykke.SettingsReader;
using StackExchange.Redis;

namespace Lykke.Service.ConfirmationCodes.Modules
{
    public class RedisModule : Module
    {
        private readonly CacheSettings _settings;

        public RedisModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings.CurrentValue.RedisConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var redis = ConnectionMultiplexer.Connect(_settings.ConnectionString);

            builder.RegisterInstance(redis).SingleInstance();
            builder.Register(
                c =>
                    c.Resolve<ConnectionMultiplexer>()
                        .GetServer(redis.GetEndPoints()[0]));

            builder.Register(
                c =>
                    c.Resolve<ConnectionMultiplexer>()
                        .GetDatabase());
        }
    }
}
