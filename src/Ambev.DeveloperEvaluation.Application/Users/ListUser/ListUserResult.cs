using Ambev.DeveloperEvaluation.Application.Users.GetUser;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser
{
    public class ListUserResult
    {
        public int TotalItems { get; set; }
        public IEnumerable<GetUserResult> Data { get; set; } = new List<GetUserResult>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

    }
}
