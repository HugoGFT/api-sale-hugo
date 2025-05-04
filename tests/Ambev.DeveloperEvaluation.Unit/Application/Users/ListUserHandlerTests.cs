using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.ListUser;
using Ambev.DeveloperEvaluation.Domain.Dto.User;
using Ambev.DeveloperEvaluation.Domain.Dto.UserDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using MediatR;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class ListUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ListUserHandler _handler;

        public ListUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new ListUserHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var faker = new Faker();
            var command = new ListUserCommand(faker.Random.Int(1, 10), faker.Random.Int(1, 50), faker.Random.Word());
            var filter = new ListUserFilter { Page = command.Page, PageSize = command.PageSize, Order = command.Order };
            var repositoryResult = new ListUserResultDto(
                faker.Random.Int(1, 100),
                faker.Random.Int(1, 10),
                faker.Random.Int(1, 10),
                new List<User>());

            var expectedResult = new ListUserResult
            {
                TotalItems = repositoryResult.TotalItems,
                TotalPages = repositoryResult.TotalPages,
                CurrentPage = repositoryResult.CurrentPage,
                Data = new List<GetUserResult>()
            };

            _mapperMock.Setup(m => m.Map<ListUserFilter>(command)).Returns(filter);
            _userRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(repositoryResult);
            _mapperMock.Setup(m => m.Map<ListUserResult>(repositoryResult)).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.TotalItems, result.TotalItems);
            Assert.Equal(expectedResult.TotalPages, result.TotalPages);
            Assert.Equal(expectedResult.CurrentPage, result.CurrentPage);
            Assert.Equal(expectedResult.Data, result.Data);

            _mapperMock.Verify(m => m.Map<ListUserFilter>(command), Times.Once);
            _userRepositoryMock.Verify(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ListUserResult>(repositoryResult), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResult_WhenRepositoryReturnsNull()
        {
            // Arrange
            var faker = new Faker();
            var command = new ListUserCommand(faker.Random.Int(1, 10), faker.Random.Int(1, 50), faker.Random.Word());
            var filter = new ListUserFilter { Page = command.Page, PageSize = command.PageSize, Order = command.Order };

            _mapperMock.Setup(m => m.Map<ListUserFilter>(command)).Returns(filter);
            _userRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync((ListUserResultDto?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
