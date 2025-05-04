using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser
{
    public class ListUserCommand : IRequest<ListUserResult>
    {
        public ListUserCommand() { }
        public ListUserCommand(int? page, int? size, string? order)
        {
            Page = page ?? 1;
            PageSize = size ?? 10;
            Order = order ?? string.Empty;
        }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Order { get; set; }
    }
}
