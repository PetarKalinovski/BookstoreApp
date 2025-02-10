using Domain.Domain_Models;
using Eshop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Interface
{
    public interface IOrderService
    {
        List<Order> GetOrdersForUser(string userId);
        Order GetOrderDetails(Guid orderId);
        void UpdateOrderStatus(Guid orderId, OrderStatus status);
        void DeleteOrder(Guid orderId);

        List<Order> GetAllOrders();
    }
}
