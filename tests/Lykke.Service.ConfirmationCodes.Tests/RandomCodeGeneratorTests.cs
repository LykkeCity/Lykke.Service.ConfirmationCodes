using System;
using FluentAssertions;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Factories;
using Xunit;
using Xunit.Abstractions;

namespace Lykke.Service.ConfirmationCodes.Tests
{
    public class RandomCodeGeneratorTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RandomCodeGeneratorTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetInt_Run10Times_OutputToConsole()
        {
            var generator = new RandomValueGenerator();

            for (var i = 0; i < 10; i++)
            {
                var result = generator.GetInt(1, 9999);

                _testOutputHelper.WriteLine($"RandomNumber: {result}");
            }
        }

        [Fact]
        public void GetInt_WhenLeftBorderSpeficied_ReturnsNumberBiggerThanLeftBorder()
        {
            var generator = new RandomValueGenerator();

            var result = generator.GetInt(1, 2);

            result.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public void GetInt_WhenRightBorderSpeficied_ReturnsNumberBiggerThanRightBorder()
        {
            var generator = new RandomValueGenerator();

            var result = generator.GetInt(1, 2);

            result.Should().BeLessOrEqualTo(2);
        }

        [Fact]
        public void GetInt_WhenBordersAreTheSame_ReturnsBorder()
        {
            var generator = new RandomValueGenerator();

            var result = generator.GetInt(6, 6);

            result.Should().Be(6);
        }

        [Fact]
        public void GetInt_WhenBordersInconsistent_ThrowsArgumentOutOfRangeException()
        {
            var generator = new RandomValueGenerator();

            Assert.Throws<ArgumentOutOfRangeException>(() => generator.GetInt(5, 1));
        }
    }
}
