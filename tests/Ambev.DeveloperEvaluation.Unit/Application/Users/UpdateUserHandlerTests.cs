using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Dto.UserDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class UpdateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly UpdateUserHandler _handler;
        private readonly Faker _faker;

        public UpdateUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new UpdateUserHandler(_userRepositoryMock.Object, _mapperMock.Object, _passwordHasherMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldUpdateUser_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id = _faker.Random.Int(1, 1000),
                Username = _faker.Internet.UserName(),
                Password = _faker.Internet.Password(10)+".S3",
                Email = _faker.Internet.Email(),
                Phone = "11948558102",
                Status = UserStatus.Active,
                Role = UserRole.Customer,
                Name = new NameDto
                {
                    Firstname = _faker.Name.FirstName(),
                    Lastname = _faker.Name.LastName()
                },
                Address = new AddressDto
                {
                    Street = _faker.Address.StreetName(),
                    City = _faker.Address.City(),
                    Number = _faker.Random.Int(1, 1000),
                    ZipCode = _faker.Address.ZipCode()
                }
            };

            var existingUser = new User
            {
                Id = command.Id,
                Username = command.Username,
                Email = command.Email
            };

            var updatedUser = new User
            {
                Id = command.Id,
                Username = command.Username,
                Email = command.Email
            };

            var result = new UpdateUserResult
            {
                Id = command.Id,
                Username = command.Username,
                Email = command.Email
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            _mapperMock.Setup(mapper => mapper.Map<User>(command))
                .Returns(existingUser);

            _passwordHasherMock.Setup(hasher => hasher.HashPassword(command.Password))
                .Returns("hashedPassword");

            _userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedUser);

            _mapperMock.Setup(mapper => mapper.Map<UpdateUserResult>(updatedUser))
                .Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(command.Id);
            response.Username.Should().Be(command.Username);
            response.Email.Should().Be(command.Email);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateUserCommand(); // Invalid command with missing required fields

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id = _faker.Random.Int(1, 1000),
                Username = _faker.Internet.UserName(),
                Password = "S3." + _faker.Internet.Password(10),
                Email = _faker.Internet.Email(),
                Phone = "11948558102",
                Status = UserStatus.Active,
                Role = UserRole.Customer,
                Name = new NameDto
                {
                    Firstname = _faker.Name.FirstName(),
                    Lastname = _faker.Name.LastName()
                },
                Address = new AddressDto
                {
                    Street = _faker.Address.StreetName(),
                    City = _faker.Address.City(),
                    Number = _faker.Random.Int(1, 1000),
                    ZipCode = _faker.Address.ZipCode()
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"User with Id {command.Id} not exists");
        }
    }
}
