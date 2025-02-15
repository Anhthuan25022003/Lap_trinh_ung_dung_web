namespace SV21T1020081.Shop.Models
{
    /// <summary>
    /// luư giữ cacsc thông tin đầu vào sử dụng cho chức năng tìm kiếm và hiển tị dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
    }
}
