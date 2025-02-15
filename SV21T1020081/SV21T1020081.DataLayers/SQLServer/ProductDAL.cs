using Dapper;
using SV21T1020081.DomainModels;
using System.Data;

namespace SV21T1020081.DataLayers.SQLServer
{
    public class ProductDAL : BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {

        }

        public int Add(Product data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"insert into Products(ProductName,ProductDescription,SupplierID,CategoryID
                            ,Unit,Price,Photo,IsSelling)
                            values (@ProductName,@ProductDescription,
							@SupplierID,@CategoryID,
							@Unit,@Price,@Photo,@IsSelling)
							select scope_identity();";
                var parameters = new
                {
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,

                    CategoryID = data.CategoryID ,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling,

                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            using (var connection = OpenConnection())
            {
                var productExists = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Products WHERE ProductID = @ProductID", new { ProductID = data.ProductID }) > 0;

                if (!productExists)
                {
                    throw new Exception($"ProductID {data.ProductID} không tồn tại trong bảng Products.");
                }
                var sql = @"
					INSERT INTO ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder)
                            VALUES (@ProductID, @AttributeName, @AttributeValue, @DisplayOrder);
                            SELECT SCOPE_IDENTITY()
				";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder
                };
                return connection.ExecuteScalar<long>(sql, parameters);
            }
        }

        public long AddPhoto(ProductPhoto data)
        {
            using (var connection = OpenConnection())
            {
                var sql = @"
					INSERT INTO ProductPhotos (ProductID, Photo, Description, DisplayOrder, IsHidden)
					VALUES (@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden)
					SELECT SCOPE_IDENTITY()
				";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "", // Thêm mô tả nếu có
                    DisplayOrder = data.DisplayOrder, // Thêm DisplayOrder vào câu truy vấn
                    IsHidden = data.IsHidden
                };
                return connection.ExecuteScalar<long>(sql, parameters);
            }
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT count(*)    
                            FROM Products
                            WHERE (@searchValue = N'' OR ProductName LIKE @searchValue)
                            AND (@categoryID = 0 OR CategoryID = @categoryID)
                            AND (@supplierID = 0 OR SupplierID = @supplierID)
                            AND (Price >= @minPrice)
                            AND (@maxPrice <= 0 OR Price <= @maxPrice)";
                var parameters = new
                {
                    searchValue = searchValue,
                    categoryID = categoryID,
                    supplierID = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
            return count;
        }

        public bool Delete(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where ProductID=@ProductId
                            delete from ProductAttributes where ProductID=@ProductId
                            delete from products where ProductID=@ProductId";
                var parameters = new
                {
                    ProductId = productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }


        public bool DeleteAttribute(long atrributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM ProductAttributes WHERE AttributeID = @attributeID";
                var parameters = new
                {
                    attributeID = atrributeID,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM ProductPhotos WHERE PhotoID = @photoID";
                var parameters = new
                {
                    photoID = photoID,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {

            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products where ProductID=@ProductID";
                var parameters = new
                {
                    ProductID = productID,
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long atrributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductAttributes WHERE AttributeID = @attributeID";
                var parameters = new
                {
                    attributeID = atrributeID,
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductPhotos WHERE PhotoID = @photoID";
                var parameters = new
                {
                    photoID = photoID,
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (SELECT * FROM OrderDetails WHERE ProductID = @ProductID)
                                SELECT 1
                            ELSE
                                SELECT 0";
                var parameters = new
                {
                    ProductID = productID,
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"
						SELECT *
						FROM (
							SELECT *,
							ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber
							FROM Products
							WHERE (@searchValue = N'' OR ProductName LIKE @searchValue)
							AND (@categoryID = 0 OR CategoryID = @categoryID)
							AND (@supplierID = 0 OR SupplierID = @supplierID)
							AND (Price >= @minPrice)
							AND (@maxPrice <= 0 OR Price <= @maxPrice)
						) AS t
						WHERE (@pageSize = 0)
						OR (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
					";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue,//ben trai la ten tham so trong cau lenh sql, ben phai la value truyen cho tham so
                    categoryID = categoryID,
                    supplierID = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                data = connection.Query<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (var connection = OpenConnection())            {

                var sql = "SELECT * FROM ProductAttributes WHERE ProductID = @ProductID ORDER BY DisplayOrder ASC";
                var parameters = new
                {
                    ProductID = productID,
                };
                data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> data = new List <ProductPhoto>();

            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductPhotos WHERE ProductID = @productID";
                var parameters = new
                {
                    productID = productID,
                };
                data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
							UPDATE Products
							SET ProductName = @ProductName,
								ProductDescription = @ProductDescription,
								SupplierID = @SupplierID,
								CategoryID = @CategoryID,
								Unit = @Unit,
								Price = @Price,
								Photo = @Photo,
								IsSelling = @IsSelling
							WHERE ProductID = @ProductID;
						";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
					UPDATE ProductAttributes
                            SET ProductID = @ProductID,
                                AttributeName = @AttributeName,
                                AttributeValue = @AttributeValue,
                                DisplayOrder = @DisplayOrder
                            WHERE AttributeID = @AttributeID
				";
                var parameters = new
                {
                    ProductID=data.ProductID,
                    AttributeID = data.AttributeID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder=data.DisplayOrder 

                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
					UPDATE ProductPhotos
                            SET ProductID = @ProductID,
                                Photo = @Photo,
                                Description = @Description,
                                DisplayOrder = @DisplayOrder,
                                IsHidden = @IsHidden
                            WHERE PhotoID = @PhotoID
				";
                var parameters = new
                {
                    PhotoID = data.PhotoID,
                    Photo = data.Photo ?? "",
                    ProductID= data.ProductID,
                    DisplayOrder= data.DisplayOrder,
                    IsHidden=data.IsHidden,
                    Description=data.Description,



                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
