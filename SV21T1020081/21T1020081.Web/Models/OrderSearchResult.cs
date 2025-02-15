using SV21T1020081.DomainModels;

namespace _21T1020081.Web.Models
{
    public class OrderSearchResult:PaginationSearchResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
