using _21T1020081.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020081.BusinessLayers;
using SV21T1020081.DomainModels;
using SV21T1020081.Web;

namespace _21T1020081.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]

    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 30;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        public IActionResult Index()
        {

            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0
                };
            return View(condition);
        }
        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID,
                condition.SupplierID,
                condition.MinPrice,
                condition.MaxPrice);
            ProductSearchResult model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng mới";
            var data = new Product()
            {
                ProductID = 0,
                IsSelling = false,
            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = id == 0 ? "Bổ sung mặt hàng mới" : "Cập nhật thông tin mặt hàng";
            //ViewBag.Title = "Cập nhật thông tin mặt hàng mới";
            //var data = ProductDataService.GetProduct(id);
            var data = id == 0 ? new Product { ProductID = 0, IsSelling = false } : ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Delete(int id = 0)
        {
            ViewBag.Title = "Xoá mặt hàng";
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View(new ProductPhoto { ProductID = id });
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh cho mặt hàng";
                    var photo = ProductDataService.GetPhoto(photoId);
                    //return View();
                    if (photo == null)
                    {
                        return RedirectToAction("Index"); // Handle if photo not found
                    }
                    return View(photo);
                case "delete":
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }

        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    ProductAttribute addProductAttribute = new() { AttributeID = attributeId, ProductID = id, DisplayOrder = 1 };
                    return View(addProductAttribute);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    ProductAttribute? editProductAttribute = ProductDataService.GetAttribute(attributeId);
                    if (editProductAttribute == null)
                    {
                        return RedirectToAction("Edit", new { id = id });
                    }
                    return View(editProductAttribute);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Save(Product data, IFormFile? _Photo)
        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng mới" : "Cập nhật thông tin mặt hàng";

            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên không được để trống");

            if (data.CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Loại hàng không được để trống");

            if (data.SupplierID == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Tên nhà cung cấp không được để trống");

            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");

            if (data.Price < 0)
                ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá lớn hơn 0");


            if (!ModelState.IsValid) // NẾu trường hợp Modelstate không hợp lệ
            {

                return View("Edit", data);
            }
            if (_Photo != null)
            {

                //Tên file sẽ lưu trên server
                string fileName = $"{DateTime.Now.Ticks}_{_Photo.FileName}";
                //Đường dẫn đến file sẽ lưu trên server (vd: D:\MyWeb\wwwroot\images\employees\photo.png)
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _Photo.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            if (data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);
                return RedirectToAction("Edit", new { id });
            }
            else
            {
                ProductDataService.UpdateProduct(data);
                return RedirectToAction("Index", new { id = data.ProductID });
            }

        }

        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? _Photo)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var product = ProductDataService.GetProduct(data.ProductID);
            if (product == null)
            {
                ModelState.AddModelError("ProductID", "Sản phẩm không tồn tại.");
                return RedirectToAction("Edit", new { id = data.ProductID }); // Trở về trang chỉnh sửa sản phẩm nếu không tồn tại
            }

            // Kiểm tra xem người dùng có chọn ảnh không
            if (_Photo == null)
            {
                ModelState.AddModelError(nameof(data.Photo), "Vui lòng chọn ảnh sản phẩm.");

            }
            if (string.IsNullOrWhiteSpace(data.Description))
                ModelState.AddModelError(nameof(data.Description), "Vui lòng nhập mô tả ảnh mặt hàng");
            if (data.DisplayOrder <= 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng nhập thứ tự hiển thị");






            // Kiểm tra tính hợp lệ của dữ liệu trước khi lưu
            if (ModelState.IsValid == false)
            {
                return View("Photo", data);
            }
            // Lưu ảnh vào thư mục trên server
            string fileName = $"{DateTime.Now.Ticks}-{_Photo.FileName}";
            string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/products", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                _Photo.CopyTo(stream);
            }

            // Lưu thông tin ảnh vào đối tượng ProductPhoto
            data.Photo = fileName;
            data.DisplayOrder = 1; // Bạn có thể thay đổi DisplayOrder nếu cần
            data.IsHidden = false; // Thay đổi giá trị này nếu cần

            try
            {
                // Thêm ảnh vào cơ sở dữ liệu
                ProductDataService.AddPhoto(data);

                // Sau khi lưu thành công, chuyển hướng về trang chỉnh sửa sản phẩm
                return RedirectToAction("Edit", new { id = data.ProductID });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và thông báo cho người dùng
                ModelState.AddModelError("Error", $"Có lỗi xảy ra khi lưu ảnh: {ex.Message}");
                return RedirectToAction("Edit", new { id = data.ProductID }); // Trở về trang chỉnh sửa sản phẩm nếu có lỗi
            }
        }




        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var product = ProductDataService.GetProduct(data.ProductID);
            if (product == null)
            {
                ModelState.AddModelError("ProductID", "Sản phẩm không tồn tại.");
                return RedirectToAction("Edit", new { id = data.ProductID }); // Trở về trang chỉnh sửa sản phẩm
            }

            // Kiểm tra tính hợp lệ của thuộc tính
            if (string.IsNullOrWhiteSpace(data.AttributeName))
            {
                ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(data.AttributeValue))
            {
                ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị thuộc tính không được để trống.");
            }
            if (data.DisplayOrder <= 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng nhập thứ tự hiển thị");

            // Kiểm tra ModelState có hợp lệ không
            if (ModelState.IsValid == false)
            {
                return View("Attribute", data);
            }

            try
            {
                // Nếu ID thuộc tính là 0, tức là đang thêm mới thuộc tính
                if (data.AttributeID == 0)
                {
                    ProductDataService.AddAttribute(data);
                }
                else // Nếu có AttributeID, tức là đang chỉnh sửa thuộc tính
                {
                    ProductDataService.UpdateAttribute(data);
                }

                // Sau khi lưu thành công, chuyển hướng về trang Edit của sản phẩm
                return RedirectToAction("Edit", new { id = data.ProductID });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và thông báo cho người dùng
                ModelState.AddModelError("Error", $"Có lỗi xảy ra khi lưu thuộc tính: {ex.Message}");
                return RedirectToAction("Edit", new { id = data.ProductID }); // Trở về trang chỉnh sửa sản phẩm nếu có lỗi
            }
        }

    }
}