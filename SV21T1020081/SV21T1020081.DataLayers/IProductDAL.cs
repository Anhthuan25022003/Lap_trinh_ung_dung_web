using SV21T1020081.DomainModels;

namespace SV21T1020081.DataLayers
{
    public interface IProductDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);

        int Count(string searchValue = "", int categoryID = 0,int supplierID=0,decimal minPrice=0,decimal maxPrice=0);

        Product? Get(int productID);

        int Add(Product data);
        bool Update(Product data);
        bool Delete(int productID);
        bool InUsed(int productID);

        IList<ProductPhoto> ListPhotos(int productID);

        ProductPhoto? GetPhoto(long photoID);
        long AddPhoto(ProductPhoto data);
        bool UpdatePhoto(ProductPhoto data);
        bool DeletePhoto(long photoID);

        IList<ProductAttribute> ListAttributes(int productID);
        ProductAttribute? GetAttribute(long atrributeID);
        long AddAttribute(ProductAttribute data);
        bool UpdateAttribute(ProductAttribute data);
        bool DeleteAttribute(long atrributeID);

    }
}
