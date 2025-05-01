using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUser;

/// <summary>
/// API response model for ListUser operation
/// </summary>
public class ListUserResponse
{
    public int TotalItems { get; set; }
    public IEnumerable<GetUserResponse> Data { get; set; } = new List<GetUserResponse>();
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
