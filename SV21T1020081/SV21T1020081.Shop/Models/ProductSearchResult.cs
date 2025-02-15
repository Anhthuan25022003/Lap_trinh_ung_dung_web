using SV21T1020081.DomainModels;

namespace SV21T1020081.Shop.Models
{
    public class ProductSearchResult:PaginationSearchResult
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 0;
        public required List<Product> Data { get; set; }
    }
}
