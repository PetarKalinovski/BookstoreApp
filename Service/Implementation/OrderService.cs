using Domain.Domain_Models;
using Eshop.Domain.Domain;
using Project.Service.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{

    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetOrdersForUser(string userId)
        {
            return _orderRepository.GetAll()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetOrderDetails(Guid orderId)
        {
            return _orderRepository.Get(orderId);
        }

        public void UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            var order = _orderRepository.Get(orderId);
            if (order != null)
            {
                order.Status = status;
                _orderRepository.Update(order);
            }
        }
        public void DeleteOrder(Guid orderId)
        {
            var order = _orderRepository.Get(orderId);
            if (order != null)
            {
                _orderRepository.Delete(order);
            }
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAll().ToList();
        }
    }
}
