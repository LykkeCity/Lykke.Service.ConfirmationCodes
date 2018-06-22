using System;
using Lykke.Sdk;
using Lykke.Service.ConfirmationCodes.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.ConfirmationCodes
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {                                   
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.ApiTitle = "ConfirmationCodes API";
                options.Logs = ("ConfirmationCodesLog", x => x.ConfirmationCodeServiceSettings.Db.LogsConnString);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration();
        }
    }
}
