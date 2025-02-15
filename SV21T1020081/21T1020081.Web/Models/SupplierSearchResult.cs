using SV21T1020081.DomainModels;

namespace _21T1020081.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
