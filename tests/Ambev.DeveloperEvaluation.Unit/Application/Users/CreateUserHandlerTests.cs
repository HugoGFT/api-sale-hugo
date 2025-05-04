using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Dto.UserDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using Bogus.DataSets;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly CreateUserHandler _handler;
        private readonly Faker<CreateUserCommand> _faker;

        public CreateUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new CreateUserHandler(_userRepositoryMock.Object, _mapperMock.Object, _passwordHasherMock.Object);

            _faker = new Faker<CreateUserCommand>()
                .RuleFor(c => c.Username, f => f.Internet.UserName())
                .RuleFor(c => c.Password, f => f.Internet.Password(8, true))
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("(##) #####-####"))
                .RuleFor(c => c.Status, f => f.PickRandom<UserStatus>())
                .RuleFor(c => c.Role, f => f.PickRandom<UserRole>())
                .RuleFor(c => c.Name, f => new NameDto
                {
                    Firstname = f.Name.FirstName(),
                    Lastname = f.Name.LastName()
                })
                .RuleFor(c => c.Address, f => new AddressDto
                {
                    Street = f.Address.StreetName(),
                    City = f.Address.City(),
                    Number = f.Random.Int(1, 1000),
                    ZipCode = f.Address.ZipCode(),
                    Geolocation = new GeoLocationDto
                    {
                        Lat = f.Address.Latitude().ToString(),
                        Long = f.Address.Longitude().ToString()
                    }
                });
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenCommandIsValid()
        {
            // Arrange
            var faker = new Faker();
            var command = _faker.Generate();
            command.Password = "S3." + faker.Internet.Password(10);
            command.Phone = "11948558102";
            command.Role = (UserRole)faker.Random.Int(1, 3);
            command.Status = (UserStatus)faker.Random.Int(1, 3);
            var user = new User { Email = command.Email,  };
            var createdUser = new User { Id = 1, Email = command.Email };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            _mapperMock.Setup(mapper => mapper.Map<User>(command)).Returns(user);
            _passwordHasherMock.Setup(hasher => hasher.HashPassword(command.Password)).Returns("hashedPassword");
            _userRepositoryMock.Setup(repo => repo.CreateAsync(user, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);
            _mapperMock.Setup(mapper => mapper.Map<CreateUserResult>(createdUser))
                .Returns(new CreateUserResult { Id = createdUser.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdUser.Id, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateUserCommand(); // Invalid command
            var validator = new CreateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains(validationResult.Errors, e => exception.Errors.Select(err => err.ErrorMessage).Contains(e.ErrorMessage));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenUserAlreadyExists()
        {
            // Arrange
            var faker = new Faker();
            var command = _faker.Generate();
            command.Password = "S3." + faker.Internet.Password(10);
            command.Phone = "11948558102";
            command.Role = (UserRole)faker.Random.Int(1, 3);
            command.Status = (UserStatus)faker.Random.Int(1, 3);
            var existingUser = new User { Email = command.Email };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"User with email {command.Email} already exists", exception.Message);
        }
    }
}
