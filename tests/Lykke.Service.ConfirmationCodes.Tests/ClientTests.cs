using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Lykke.Service.ConfirmationCodes.Tests
{
    public class HomeTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public HomeTest()
        {
            Environment.SetEnvironmentVariable("SettingsUrl", "unit-test-settings.json");
            _server = new TestServer(
                WebHost.CreateDefaultBuilder()
                    .UseEnvironment("Development")
                    .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Fact]
        public async Task Index_Get_ReturnsIndexHtmlPage()
        {
            var request = new[]
            {
                new KeyValuePair<string, string>("Email", "test@test.com"),
                new KeyValuePair<string, string>("IsPriority", "false"),
            };

            // Act

            //var response = await _client.PostAsync("/api/EmailConfirmation", new SendSmsConfirmationRequest());

            // Assert
            //response.EnsureSuccessStatusCode();
        }
    }
}

