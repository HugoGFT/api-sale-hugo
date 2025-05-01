using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.ListUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
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
        CreateMap<ListUserResult, ListUserResponse>();
        CreateMap<GetUserResult, GetUserResponse>();

    }
}
