using SV21T1020081.DomainModels;

namespace _21T1020081.Web.Models
{
    public class CategorySearchResult:PaginationSearchResult
    {
        public required List<Category> Data { get; set; }

    }
}
