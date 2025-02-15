using SV21T1020081.DomainModels;

namespace _21T1020081.Web.Models
{
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }

    }
}
