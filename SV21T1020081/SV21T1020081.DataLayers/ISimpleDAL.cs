namespace SV21T1020081.DataLayers
{
    public interface ISimpleDAL<T> where T : class
    {
        /// <summary>
        /// Truy vấn và lấy toàn bộ dữ liệu của bảng
        /// </summary>
        /// <returns></returns>
        List<T> List();
    }
}
