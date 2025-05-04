using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Dto.UserDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class GetUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserHandler _handler;

        public GetUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetUserHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var faker = new Faker();
            var userId = faker.Random.Int(1, 1000);

            var user = new Faker<User>()
                .RuleFor(u => u.Id, userId)
                .RuleFor(u => u.Firstname, f => f.Name.FirstName())
                .RuleFor(u => u.Lastname, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .Generate();

            var result = new GetUserResult
            {
                Id = userId,
                Name = new NameDto { Firstname = user.Firstname, Lastname = user.Lastname },
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                Status = user.Status
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(mapper => mapper.Map<GetUserResult>(user))
                .Returns(result);

            var command = new GetUserCommand(userId);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(userId, response.Id);
            Assert.Equal(user.Firstname, response.Name.Firstname);
            Assert.Equal(user.Lastname, response.Name.Lastname);
            Assert.Equal(user.Email, response.Email);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new GetUserCommand(0); // Invalid ID
            var validator = new GetUserValidator();
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Id", "Id must be greater than 0") });

            var validatorMock = new Mock<IValidator<GetUserCommand>>();
            validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var faker = new Faker();
            var userId = faker.Random.Int(1, 1000);

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var command = new GetUserCommand(userId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
