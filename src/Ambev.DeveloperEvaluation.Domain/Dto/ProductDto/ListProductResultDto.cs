namespace Ambev.DeveloperEvaluation.Domain.Dto.ProductDto
{
    public class ListProductResultDto
    {
        public ListProductResultDto(int totalItems, int totalPages, int currentPage, IEnumerable<Entities.Product> Products)
        {
            TotalItems = totalItems;
            Data = Products;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
        public int TotalItems { get; set; }
        public IEnumerable<Entities.Product> Data { get; set; } = new List<Entities.Product>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
