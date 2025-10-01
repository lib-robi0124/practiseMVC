using Lamazon.Domain.Constants;
using Lamazon.Services.Interfaces;
using Lamazon.Services.Models;
using Lamazon.ViewModels.Models;
using Lamazon.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamazon.Web.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IGeoTrackerService _geoTracerService;

        public OrderController(IOrderService orderService, IProductService productService, IGeoTrackerService geoTracerService)
        {
            _orderService = orderService;
            _productService = productService;
            _geoTracerService = geoTracerService;
        }
        public IActionResult ShoppingCart()
        {
            var shoppingCart = GetShoppingCart();
            if(!shoppingCart.ShoppingCartItems.Any())
            {
                return RedirectToAction("Index", "Products");
            }
           var orderLineItems = new List<OrderLineItemViewModel>();
            foreach(var shoppingCartItem in shoppingCart.ShoppingCartItems)
            {
                var product = _productService.GetProductById(shoppingCartItem.Id);
               
                 var orderLineItem = new OrderLineItemViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        ProductPrice = product.DiscountedPrice,
                        ProductDescription = product.Description,
                        ProductImage = product.ImageUrl,
                        DiscountPercentage = product.DiscountPercentage,
                 };
                orderLineItem.TotalPrice = orderLineItem.Quantity * orderLineItem.ProductPrice * (1 - orderLineItem.DiscountPercentage / 100);
                orderLineItems.Add(orderLineItem);
            }
            return View(orderLineItems);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(List<OrderLineItemViewModel> orderLineItems)
        {
            var orderViewModel = new OrderViewModel();

            orderViewModel.UserId = User.GetUserId();
            orderViewModel.OrderLineItems = orderLineItems;
            orderViewModel.TotalAmount = orderLineItems.Sum(oli => oli.TotalPrice);

            // Getting IP Address and Country Info from Middleware
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
            orderViewModel.IpAddress = ipAddress != null ? ipAddress.ToString() : "123";
            var ipGeoInfo = await _geoTracerService.GetIpGeoInfoAsync(orderViewModel.IpAddress);

            orderViewModel.CountryCode = ipGeoInfo.CountryCode;

            orderViewModel.CountryFlagUrl = "url";

            await _orderService.CreateOrder(orderViewModel);

            Response.Cookies.Delete(Cookies.ShoppingCart);
            AddNotificationMesssage("You have successfully placed an order.");

            return RedirectToAction("Index", "Home");
        }
    }
    
}
