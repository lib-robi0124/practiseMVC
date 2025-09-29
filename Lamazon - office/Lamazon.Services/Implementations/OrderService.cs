using AutoMapper;
using Lamazon.DataAccess.Interfaces;
using Lamazon.Domain.Entities;
using Lamazon.Services.Interfaces;
using Lamazon.ViewModels.Models;

namespace Lamazon.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            this.mapper = mapper;
        }
        public async Task CreateOrder(OrderViewModel orderViewModel)
        {
            orderViewModel.OrderDate = DateTime.UtcNow;
            orderViewModel.OrderStatus = Domain.Enums.OrderStatusEnum.Pending;
            int maxId = await _orderRepository.GetMaxIdAsync();
            orderViewModel.OrderNumber = $"{++maxId}/{DateTime.Now.Year}";

            var order = mapper.Map<Order>(orderViewModel);
            int orderId = await _orderRepository.InsertAsync(order);

            if (orderId <= 0)
            {
                throw new Exception("Failed to create order.");
            }
        }
    }
}
