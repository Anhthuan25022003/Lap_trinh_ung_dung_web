namespace _21T1020081.Web.Models
{
    public class ProductSearchInput:PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 0;
    }
}
