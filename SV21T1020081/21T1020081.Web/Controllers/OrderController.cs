using _21T1020081.Web;
using _21T1020081.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020081.BusinessLayers;
using SV21T1020081.DomainModels;
using SV21T1020081.Web;

using System.Globalization;

namespace _21T1020081.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]

    public class OrderController : Controller
    {
        public const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";
        public const int PAGE_SIZE = 20;

        private const int PRODUCT_PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchSale";

        public const string SHOPPING_CART = "ShoppingCart";
        public IActionResult Index()
        {
            OrderSearchInput? condition = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH_CONDITION);
            if (condition == null)
            {
                var cultureInfo = new CultureInfo("en-GB");

                condition = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    TimeRange = $"{DateTime.Today.AddDays(-720).ToString("dd/MM/yyyy", cultureInfo)}-{DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)}",
                };

            }
            return View(condition);
        }
        public IActionResult Search(OrderSearchInput condition)
        {
            int rowCount;
            var data = OrderDataService.ListOrders(out rowCount, condition.Page, condition.PageSize, condition.Status,
                condition.FromTime,
                condition.ToTime,
                condition.SearchValue ?? "");
            OrderSearchResult model = new OrderSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                Status = condition.Status,
                TimeRange = condition.TimeRange,
                RowCount = rowCount,

                Data = data
            };
            ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            var condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {


                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PRODUCT_PAGE_SIZE,
                    SearchValue = ""

                };

            }
            return View(condition);
        }
        public IActionResult SearchProduct(ProductSearchInput condition)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize,
                condition.SearchValue ?? "");
            var model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",

                RowCount = rowCount,

                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
                return RedirectToAction("Index");
            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            {
                Order = order,
                Details = details
            };
            return View(model);
        }
        public IActionResult EditDetail(int id = 0, int ProductId = 0)
        {
            if (id <= 0 || ProductId <= 0)
            {
                return RedirectToAction("Details", new { id });
            }

            // Lấy thông tin chi tiết đơn hàng
            var orderDetail = OrderDataService.GetOrderDetail(id, ProductId);
            if (orderDetail == null)
            {
                return RedirectToAction("Details", new { id });
            }

            // Truyền đối tượng OrderDetail vào View
            return View(orderDetail);
        }
        [HttpPost]
        public IActionResult UpdateDetail(int id, int productId, int quantity, decimal salePrice)
        {
            var orderDetail = OrderDataService.GetOrderDetail(id, productId);

            // Kiểm tra dữ liệu đầu vào
            if (quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Số lượng phải lớn hơn 0.");
            }

            if (salePrice < 0)
            {
                ModelState.AddModelError("SalePrice", "Giá bán phải lớn hơn 0.");
            }

            // Nếu có lỗi, render lại view Details kèm thông báo lỗi
            if (!ModelState.IsValid)
            {
                return View("EditDetail", orderDetail);
            }
            // Thực hiện cập nhật nếu dữ liệu hợp lệ
            bool success = OrderDataService.SaveOrderDetail(id, productId, quantity, salePrice);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể cập nhật chi tiết đơn hàng.");
                return View("EditDetail", orderDetail);
            }

            // Nếu cập nhật thành công, quay lại trang Details
            return RedirectToAction("Details", new { id });
        }



        public IActionResult Shipping(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var order = OrderDataService.GetOrder(id);
            if (order == null)
                return RedirectToAction("Index");

            var shippers = CommonDataService.ListShipper();
            ViewBag.Shippers = shippers; // Dữ liệu cho dropdown

            return View(order); // Gửi thông tin đơn hàng sang View
        }
        // Accepted - Duyệt chấp nhận đơn hàng
        [HttpGet]
        public IActionResult Accept(int id)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return View("Details", new { id });
            }

            // Cập nhật trạng thái đơn hàng thành "Accepted"
            bool success = OrderDataService.AcceptOrder(id);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể chấp nhận đơn hàng.");
                return View("Details", new { id });
            }

            // Nếu thành công, quay lại trang Details
            return RedirectToAction("Details", new { id });
        }

        // Shipping - Xác nhận đã chuyển hàng
        [HttpPost]
        public IActionResult Shipping(int id, int shipperID)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return View("Details", new { id });
            }

            // Cập nhật trạng thái đơn hàng thành "Shipping"
            bool success = OrderDataService.ShipOrder(id, shipperID);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể chuyển giao đơn hàng.");
                return View("Details", new { id });
            }

            // Nếu thành công, quay lại trang Details
            return RedirectToAction("Details", new { id });
        }

        // Finished - Xác nhận hoàn thành đơn hàng
        [HttpGet]
        public IActionResult Finish(int id)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return View("Details", new { id });
            }

            // Cập nhật trạng thái đơn hàng thành "Finished"
            bool success = OrderDataService.FinishOrder(id);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể hoàn thành đơn hàng.");
                return View("Details", new { id });
            }

            // Nếu thành công, quay lại trang Details
            return RedirectToAction("Details", new { id });
        }

        // Canceled - Hủy bỏ đơn hàng
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return View("Details", new { id });
            }

            // Cập nhật trạng thái đơn hàng thành "Canceled"
            bool success = OrderDataService.CancelOrder(id);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể hủy đơn hàng.");
                return View("Details", new { id });
            }

            // Nếu thành công, quay lại trang Details
            return RedirectToAction("Details", new { id });
        }

        // Rejected - Từ chối đơn hàng
        [HttpGet]
        public IActionResult Reject(int id)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return View("Details", new { id });
            }

            // Cập nhật trạng thái đơn hàng thành "Rejected"
            bool success = OrderDataService.RejectOrder(id);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể từ chối đơn hàng.");
                return View("Details", new { id });
            }

            // Nếu thành công, quay lại trang Details
            return RedirectToAction("Details", new { id });
        }
        private List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }

            return shoppingCart;
        }


        public IActionResult AddToCart(CartItem item)
        {

            if (item.SalePrice < 0 || item.Quantity <= 0)
                return Json("Giá bán và số lượng không hợp lệ");
            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == item.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(item);
            }

            else
            {
                existsProduct.Quantity += item.Quantity;
                existsProduct.SalePrice = item.SalePrice;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult ShoppingCart()
        {
            return View(GetShoppingCart());
        }
        public IActionResult Init(int customerID = 0, string deliveryProvince = "", string deliveryAddress = "")
        {
            try
            {
                var shoppingCart = GetShoppingCart();
                if (shoppingCart.Count == 0)
                    return Json("Giỏ hàng trống. Vui lòng chọn mặt hàng cần bán");
                if (customerID == 0 || string.IsNullOrWhiteSpace(deliveryProvince) || string.IsNullOrWhiteSpace(deliveryAddress))
                    return Json("Vui lòng nhập đầy đủ thông tin khách hàng và nơi giao hàng");

                var userData = User.GetUserData();
                int employeeID = 0; //TODO: Thay bởi ID của nhân viên đang login vào hệ thống
                if (userData != null) 
                {
                    employeeID = int.Parse(userData.UserId);
                }
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var item in shoppingCart)
                {
                    orderDetails.Add(new OrderDetail()
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        SalePrice = item.SalePrice

                    });
                }
                int orderID = OrderDataService.InitOrder(employeeID, customerID, deliveryProvince, deliveryAddress, orderDetails);
                ClearCart();
                return Json(orderID);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi để kiểm tra
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý đơn hàng.");
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Lấy thông tin đơn hàng cần xóa
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return RedirectToAction("Index");
            }

            // Thực hiện xóa đơn hàng
            bool success = OrderDataService.DeleteOrder(id);
            if (!success)
            {
                ModelState.AddModelError("loii", "Không thể xóa đơn hàng. Chỉ có thể xóa các đơn hàng ở trạng thái khởi tạo, hủy hoặc từ chối.");
                return RedirectToAction("Details", new { id });
            }

            // Nếu xóa thành công, quay lại danh sách đơn hàng
            return RedirectToAction("Index");
        }
 
        [HttpGet]
        public IActionResult DeleteDetail(int id, int productId)
        {
            // Kiểm tra thông tin đơn hàng
            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                ModelState.AddModelError("", "Đơn hàng không tồn tại.");
                return RedirectToAction("Details", new { id });
            }

            // Kiểm tra trạng thái đơn hàng trước khi cho phép xóa
            if (order.Status != Constants.ORDER_INIT && order.Status != Constants.ORDER_ACCEPTED)
            {
                ModelState.AddModelError("", "Chỉ có thể xóa chi tiết đơn hàng khi đơn hàng ở trạng thái 'Init' hoặc 'Accepted'.");
                return RedirectToAction("Details", new { id });
            }

            // Xóa chi tiết đơn hàng
            bool success = OrderDataService.DeleteOrderDetail(id, productId);
            if (!success)
            {
                ModelState.AddModelError("", "Không thể xóa chi tiết đơn hàng.");
            }

            // Quay lại trang chi tiết đơn hàng
            return RedirectToAction("Details", new { id });
        }




    }
}