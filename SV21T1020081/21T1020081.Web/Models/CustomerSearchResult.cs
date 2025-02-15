using SV21T1020081.DomainModels;

namespace _21T1020081.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List <Customer> Data { get; set; }
    }
}
