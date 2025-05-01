namespace Ambev.DeveloperEvaluation.Domain.Dto.UserDto
{
    public class ListUserResultDto
    {
        public ListUserResultDto(int totalItems, int totalPages, int currentPage, IEnumerable<Entities.User> users)
        {
            TotalItems = totalItems;
            Data = users;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
        public int TotalItems { get; set; }
        public IEnumerable<Entities.User> Data { get; set; } = new List<Entities.User>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
