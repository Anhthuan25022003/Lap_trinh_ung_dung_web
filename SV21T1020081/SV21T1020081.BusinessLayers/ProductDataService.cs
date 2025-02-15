using SV21T1020081.DataLayers;
using SV21T1020081.DataLayers.SQLServer;
using SV21T1020081.DomainModels;

namespace SV21T1020081.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            string connectionString = "server=THUANIT\\SQLEXPRESS;user id=sa;password=1234;database=LiteCommerceDB;TrustServerCertificate=true";
            productDB = new ProductDAL(connectionString);
        }
        /// <summary>
        /// Tìm kiếm và lâý thông tin mặt hàng dưới dạng không phân trang
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        //public static List<Product> ListProducts(string searchValue = "")
        //{
        //    return productDB.List(1, int.MaxValue, searchValue, 0, 0, 0, 0);
        //}
        /// <summary>
        /// Tìm kiếm và lâý thông tin mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice);
        }
        public static List<Product> ListProduct()
        {
            return productDB.List();
        }
        public static Product? GetProduct(int productId)
        {
            return productDB.Get(productId);
        }
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        public static bool DeleteProduct(int productId)
        {
            if (productDB.InUsed(productId))
                return false;
            return productDB.Delete(productId);
        }
        public static bool InUsedProduct(int productId)
        {
            return productDB.InUsed(productId);
        }
        public static IList<ProductPhoto> ListPhotos(int productID)
        {

            return productDB.ListPhotos(productID);
        }
        public static ProductPhoto? GetPhoto(long photoID)
        {
            return productDB.GetPhoto(photoID);
        }
        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        public static bool DeletePhoto(long photoID)
        {

            return productDB.DeletePhoto(photoID);
        }
        public static IList<ProductAttribute> ListAttributes(int productID)
        {

            return productDB.ListAttributes(productID);
        }
        public static ProductAttribute? GetAttribute(long attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        public static bool DeleteAttribute(long attributeID)
        {

            return productDB.DeleteAttribute(attributeID);
        }


    }
}