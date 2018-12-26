using System;
using FluentAssertions;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Factories;
using Moq;
using Xunit;

namespace Lykke.Service.ConfirmationCodes.Tests
{
    public class VerificationCodesFactoryTests
    {
        private readonly Mock<IRandomValueGenerator> _generator;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly DateTime _currentDateTime = new DateTime(2018, 06, 21);
        private readonly VerificationCodesFactory _factory;

        public VerificationCodesFactoryTests()
        {
            _generator = new Mock<IRandomValueGenerator>();
            _generator.Setup(x => x.GetInt(It.IsAny<int>(), It.IsAny<int>())).Returns(1);

            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetDateTime()).Returns(_currentDateTime);

            _factory = new VerificationCodesFactory(_generator.Object, _dateTimeProvider.Object);
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenParametersPassed_ReturnsNotNull()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            code.Should().NotBeNull();
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenParametersPassed_CallsDateTimeProviderOnce()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            _dateTimeProvider.Verify(x => x.GetDateTime(), Times.Once);
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenParametersPassed_DateTimeProviderOutputIsUsed()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var testDateTime = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetDateTime()).Returns(testDateTime);

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            code.CreationDateTime.Should().Be(testDateTime);
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenGenerateRealCodeFalse_Generates0000()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            code.Code.Should().BeEquivalentTo("0000");
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenGenerateRealCodeFalse_GeneratorNotCalled()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            _generator.Verify(x => x.GetInt(1, 9999), Times.Never);
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenGenerateRealCodeTrue_GeneratorCalled()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = true;

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            _generator.Verify(x => x.GetInt(1, 9999), Times.Once);
        }

        [Fact]
        public void CreateEmailVerificationCode_WhenGenerateReturns1_Returns0001()
        {
            var email = "email";
            var partnerId = "partnerId";
            var generateRealCode = true;
            _generator.Setup(x => x.GetInt(1, 9999)).Returns(1);

            var code = _factory.CreateEmailVerificationCode(email, partnerId, generateRealCode);

            code.Code.Should().Be("0001");
        }

        [Fact]
        public void CreateEmailVerificationPriorityCode_WhenParametersPassed_ReturnsNotNull()
        {
            var email = "email";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateEmailVerificationPriorityCode(email, partnerId, expiration);

            code.Should().NotBeNull();
        }

        [Fact]
        public void CreateEmailVerificationPriorityCode_WhenParametersPassed_CallsDateTimeProviderOnce()
        {
            var email = "email";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateEmailVerificationPriorityCode(email, partnerId, expiration);

            _dateTimeProvider.Verify(x => x.GetDateTime(), Times.Once);
        }

        [Fact]
        public void CreateEmailVerificationPriorityCode_WhenParametersPassed_DateTimeProviderOutputIsUsed()
        {
            var email = "email";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var testDateTime = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetDateTime()).Returns(testDateTime);

            var code = _factory.CreateEmailVerificationPriorityCode(email, partnerId, expiration);

            code.CreationDateTime.Should().Be(testDateTime);
        }

        [Fact]
        public void CreateEmailVerificationPriorityCode_WhenExpirationSpecified_ExpirationFilled()
        {
            var email = "email";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateEmailVerificationPriorityCode(email, partnerId, expiration);

            code.ExpirationDate.Should().Be(expiration);
        }

        [Fact]
        public void CreateEmailVerificationPriorityCode_WhenGenerateReturns1_Returns0001()
        {
            var email = "email";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateEmailVerificationPriorityCode(email, partnerId, expiration);

            code.Code.Should().Be("0001");
        }

        [Fact]
        public void CreateEmailVerificationPriorityCode_WhenParametersPassed_GeneratorCalled()
        {
            var email = "email";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateEmailVerificationPriorityCode(email, partnerId, expiration);

            _generator.Verify(x => x.GetInt(1, 9999), Times.Once);
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenParametersPassed_ReturnsNotNull()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            code.Should().NotBeNull();
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenParametersPassed_CallsDateTimeProviderOnce()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            _dateTimeProvider.Verify(x => x.GetDateTime(), Times.Once);
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenParametersPassed_DateTimeProviderOutputIsUsed()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var testDateTime = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetDateTime()).Returns(testDateTime);

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            code.CreationDateTime.Should().Be(testDateTime);
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenGenerateRealCodeFalse_Generates0000()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            code.Code.Should().BeEquivalentTo("0000");
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenGenerateRealCodeFalse_GeneratorNotCalled()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = false;

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            _generator.Verify(x => x.GetInt(1, 9999), Times.Never);
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenGenerateRealCodeTrue_GeneratorCalled()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = true;

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            _generator.Verify(x => x.GetInt(1, 9999), Times.Once);
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenPhoneIsInvalid_ThrowsException()
        {
            var phone = "123123445";
            var partnerId = "partnerId";
            var generateRealCode = true;

            Assert.Throws<ArgumentException>(() =>
                _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode));
        }

        [Fact]
        public void CreateSmsVerificationCode_WhenGenerateReturns1_Returns0001()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var generateRealCode = true;

            var code = _factory.CreateSmsVerificationCode(phone, partnerId, generateRealCode);

            code.Code.Should().Be("0001");
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenParametersPassed_ReturnsNotNull()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration);

            code.Should().NotBeNull();
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenParametersPassed_CallsDateTimeProviderOnce()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration);

            _dateTimeProvider.Verify(x => x.GetDateTime(), Times.Once);
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenParametersPassed_DateTimeProviderOutputIsUsed()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var testDateTime = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetDateTime()).Returns(testDateTime);

            var code = _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration);

            code.CreationDateTime.Should().Be(testDateTime);
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenExpirationSpecified_ExpirationFilled()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration);

            code.ExpirationDate.Should().Be(expiration);
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenParametersPassed_GeneratorCalled()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration);

            _generator.Verify(x => x.GetInt(1, 9999), Times.Once);
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenPhoneIsInvalid_ThrowsException()
        {
            var phone = "123412341243";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            Assert.Throws<ArgumentException>(() =>
                _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration));
        }

        [Fact]
        public void CreateSmsVerificationPriorityCode_WhenGenerateReturns1_Returns0001()
        {
            var phone = "+778978978978";
            var partnerId = "partnerId";
            var expiration = DateTime.UtcNow;

            var code = _factory.CreateSmsVerificationPriorityCode(phone, partnerId, expiration);

            code.Code.Should().Be("0001");
        }
    }
}
