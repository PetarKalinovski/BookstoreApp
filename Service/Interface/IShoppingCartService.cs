using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Domain_Models;
using Eshop.Domain.Domain;

namespace Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCart GetOrCreateCart(string userId);
        void AddToCart(string userId, Guid bookId, int quantity);
        void UpdateQuantity(string userId, Guid bookId, int quantity);
        void RemoveFromCart(string userId, Guid bookId);
        void ClearCart(string userId);
        Order CreateOrderFromCart(string userId);
    }

}
