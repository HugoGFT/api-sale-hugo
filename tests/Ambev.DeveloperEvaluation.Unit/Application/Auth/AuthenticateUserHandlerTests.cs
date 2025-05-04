using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth
{
    public class AuthenticateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly AuthenticateUserHandler _handler;
        private readonly Faker _faker;

        public AuthenticateUserHandlerTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _userRepositoryMock = fixture.Freeze<Mock<IUserRepository>>();
            _passwordHasherMock = fixture.Freeze<Mock<IPasswordHasher>>();
            _jwtTokenGeneratorMock = fixture.Freeze<Mock<IJwtTokenGenerator>>();
            _handler = new AuthenticateUserHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object
            );
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new Faker<User>()
                .RuleFor(u => u.Email, _faker.Internet.Email())
                .RuleFor(u => u.Password, _passwordHasherMock.Object.HashPassword(_faker.Internet.Password()))
                .RuleFor(u => u.Username, _faker.Person.FullName)
                .RuleFor(u => u.Role, UserRole.Customer)
                .RuleFor(u => u.Status, UserStatus.Active)
                .Generate();

            var command = new AuthenticateUserCommand
            {
                Email = user.Email,
                Password = _faker.Internet.Password()
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(hasher => hasher.VerifyPassword(command.Password, user.Password))
                .Returns(true);

            _jwtTokenGeneratorMock
                .Setup(generator => generator.GenerateToken(user))
                .Returns("mocked-jwt-token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("mocked-jwt-token", result.Token);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Username, result.Name);
            Assert.Equal(user.Role.ToString(), result.Role);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = new Faker<User>()
                .RuleFor(u => u.Email, _faker.Internet.Email())
                .RuleFor(u => u.Password, _faker.Internet.Password())
                .Generate();

            var command = new AuthenticateUserCommand
            {
                Email = user.Email,
                Password = "wrong-password"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(hasher => hasher.VerifyPassword(command.Password, user.Password))
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIsNotActive()
        {
            // Arrange
            var user = new Faker<User>()
                .RuleFor(u => u.Email, _faker.Internet.Email())
                .RuleFor(u => u.Password, _faker.Internet.Password())
                .RuleFor(u => u.Status, UserStatus.Inactive)
                .Generate();

            var command = new AuthenticateUserCommand
            {
                Email = user.Email,
                Password = user.Password
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(hasher => hasher.VerifyPassword(command.Password, user.Password))
                .Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
