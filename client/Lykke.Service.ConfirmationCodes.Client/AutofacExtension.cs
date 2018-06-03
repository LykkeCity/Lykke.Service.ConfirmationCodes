using System;
using Autofac;
using Common.Log;
namespace Lykke.Service.ConfirmationCodes.Client
{
    public static class AutofacExtension
    {
        public static void RegisterConfirmationCodesClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Register(x =>
            {
                var generator = HttpClientGenerator.HttpClientGenerator.BuildForUrl(serviceUrl).Create();
                var client = generator.Generate<IConfirmationCodesClient>();
                return client;
            }).SingleInstance();
        }

        public static void RegisterConfirmationCodesClient(this ContainerBuilder builder, ConfirmationCodesServiceClientSettings settings)
        {
            builder.RegisterConfirmationCodesClient(settings?.ServiceUrl);
        }
    }
}
