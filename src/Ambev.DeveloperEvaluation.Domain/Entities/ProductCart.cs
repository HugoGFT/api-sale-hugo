using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class ProductCart : BaseEntity
    {
        public int IdUser { get; set; }
        public int IdCart { get; set; }
        public int IdProduct { get; set; }
        public int Count { get; set; }
    }
}
