using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int UserID { get; set; }
        public string Date { get; set; } = string.Empty;
    }
}
