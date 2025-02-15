namespace SV21T1020081.DataLayers
{/// <summary>
/// dinh nghia cac phep xu li du lieu thuong dung tren cac bang
/// </summary>
/// <typeparam name="T"></typeparam>
    public interface ICommonDAL <T> where T:class{
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">Trang can hien thi</param>
        /// <param name="pageSize">So dong duoc hien thi tren moi trang(bang 0 neu khong phan trang)</param>
        /// <param name="searchValue">Gia tri can tim kiem chuoi rong neu khong tim kiem</param>
        /// <returns></returns>
        List<T> List(int page = 1, int pageSize = 0, string searchValue = "");
        /// <summary>
        /// Dem so luong dong
        /// </summary>
        /// <param name="searchValue">Gia tri can tim kiem</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        T? Get(int id);
        /// <summary>
        /// Lay mot ban ghi du lieu fua vao khoa chinh/id tra ve null neu khong co
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// Cap nhat thong tin bang du lieu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xoa du lieu cua mot hang
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// Kiem tra xem mot bang ghi du lieu co khoa la id hien dang co du lieu tham chieu khong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }

    
}
