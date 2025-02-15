using SV21T1020081.DomainModels;

namespace _21T1020081.Web.Models
{
    public class ShipperSearchResult :PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}
