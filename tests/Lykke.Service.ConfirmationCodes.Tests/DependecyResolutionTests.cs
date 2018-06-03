using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Common.Log;
using FluentAssertions;
using Lykke.Service.ConfirmationCodes.Controllers;
using Lykke.Service.ConfirmationCodes.Modules;
using Lykke.Service.ConfirmationCodes.Settings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Lykke.Service.ConfirmationCodes.Tests
{
    public class DependecyResolutionTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DependecyResolutionTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact(Skip = "Not working on TC now")]
        public void ControllerParameters_CanBeResolved()
        {
            var builder = new ContainerBuilder();

            var settingsMock = new TestSettingsReloadingManager<AppSettings>("unit-test-settings.json");

            var log = RegisterMock<ILog>(builder);
            builder.RegisterModule(new ServiceModule(settingsMock, log.Object));
            var container = builder.Build();

            var unresolvedTypes = new List<string>();
            var controllerTypes = GetAllControllers();
            foreach (var type in controllerTypes)
            {
                var parameters = GetConstructorParameters(type);
                foreach (var parameter in parameters)
                {
                    try
                    {
                        container.Resolve(parameter.ParameterType);
                    }
                    catch (Exception ex)
                    {
                        _testOutputHelper.WriteLine($"Cannot resolve service {parameter.ParameterType.Name}. Exception: {ex.Message}");
                        unresolvedTypes.Add(parameter.ParameterType.Name);
                    }
                }
            }
            
            unresolvedTypes.Should().BeEmpty();
        }

        private static Mock<T> RegisterMock<T>(ContainerBuilder builder) where T : class
        {
            var mock = new Mock<T>();
            builder.RegisterInstance(mock.Object).As<T>().SingleInstance();
            return mock;
        }

        private IEnumerable<Type> GetAllControllers()
        {
            var asm = Assembly.GetAssembly(typeof(EmailConfirmationController));

            return asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type));
        }

        private IEnumerable<ParameterInfo> GetConstructorParameters(Type T)
        {
            var ctors = T.GetConstructors();
            var ctor = ctors[0];

            foreach (var param in ctor.GetParameters())
            {
                yield return param;
            }
        }
    }
}
