using System.Collections.Generic;
using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.Contract;
using Lykke.Messaging.RabbitMq;
using Lykke.Messaging.Serialization;
using Lykke.Service.ConfirmationCodes.Contract;
using Lykke.Service.ConfirmationCodes.Contract.Commands;
using Lykke.Service.ConfirmationCodes.Contract.Events;
using Lykke.Service.ConfirmationCodes.Cqrs.CommandHandlers;
using Lykke.Service.ConfirmationCodes.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.ConfirmationCodes.Modules
{
    [UsedImplicitly]
    public class CqrsModule : Module
    {
        private readonly IReloadingManager<SagasRabbitMqSettings> _sagasRabbitMq;

        public CqrsModule(IReloadingManager<AppSettings> appSettings)
        {
            _sagasRabbitMq = appSettings.Nested(x => x.SagasRabbitMq);
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            string selfRoute = "self";
            string commandsRoute = "commands";
            string eventsRoute = "events";
            MessagePackSerializerFactory.Defaults.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolver.Instance;
            var rabbitMqSagasSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _sagasRabbitMq.CurrentValue.RabbitConnectionString };

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>();

            builder
                .RegisterType<ValidationCommandHandler>()
                .SingleInstance();

            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();

                    return new MessagingEngine(
                        logFactory,
                        new TransportResolver(new Dictionary<string, TransportInfo>
                        {
                            {
                                "RabbitMq",
                                new TransportInfo(rabbitMqSagasSettings.Endpoint.ToString(),
                                    rabbitMqSagasSettings.UserName, rabbitMqSagasSettings.Password, "None", "RabbitMq")
                            }
                        }),
                        new RabbitMqTransportFactory(logFactory));
                })
                .As<IMessagingEngine>()
                .SingleInstance();
            
            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();
                    
                    return new CqrsEngine(logFactory,
                        ctx.Resolve<IDependencyResolver>(),
                        ctx.Resolve<IMessagingEngine>(),
                        new DefaultEndpointProvider(),
                        true,
                        Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                            "RabbitMq",
                            SerializationFormat.MessagePack,
                            environment: "lykke",
                            exclusiveQueuePostfix: "k8s")),
                        Register.BoundedContext(ConfirmationCodesBoundedContext.Name)
                            .ListeningCommands(typeof(ValidateConfirmationCommand))
                            .On(commandsRoute)
                            .WithCommandsHandler<ValidationCommandHandler>()
                            .PublishingEvents(
                                typeof(ConfirmationCodeValidationSuccessEvent),
                                typeof(ConfirmationCodeValidationFailEvent))
                            .With(eventsRoute)
                    );
                })
                .As<ICqrsEngine>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
