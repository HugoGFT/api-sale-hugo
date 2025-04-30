using Ambev.DeveloperEvaluation.Application.Users.ListUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUser;

/// <summary>
/// Profile for mapping between Application and API ListUser responses
/// </summary>
public class ListUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListUser feature
    /// </summary>
    public ListUserProfile()
    {
        CreateMap<ListUserRequest, ListUserCommand>();
        CreateMap<ListUserResult, ListUserResponse>();
    }
}
