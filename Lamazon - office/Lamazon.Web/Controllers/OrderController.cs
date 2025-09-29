using Lamazon.Domain.Constants;
using Lamazon.Services.Interfaces;
using Lamazon.ViewModels.Models;
using Lamazon.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Lamazon.Web.Controllers
{
    
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
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
                        DiscountedPercentage = product.DiscountPercentage,
                 };
                orderLineItem.TotalPrice = orderLineItem.Quantity * orderLineItem.ProductPrice * (1 - orderLineItem.DiscountedPercentage / 100);
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

            orderViewModel.IpAddress = "123";
            orderViewModel.CountryCode = "MK";
            orderViewModel.CountryFlagUrl = "url";

            await _orderService.CreateOrder(orderViewModel);

            Response.Cookies.Delete(Cookies.ShoppingCart);
            AddNotificationMesssage("You have successfully placed an order.");

            return RedirectToAction("Index", "Home");
        }
    }
    
}
