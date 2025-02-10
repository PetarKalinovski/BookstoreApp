using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Domain_Models;
using Eshop.Domain.Domain;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _cartRepository;
        private readonly IRepository<ShoppingCartItem> _cartItemRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Book> _bookRepository;

        public ShoppingCartService(
            IRepository<ShoppingCart> cartRepository,
            IRepository<ShoppingCartItem> cartItemRepository,
            IRepository<Order> orderRepository,
            IRepository<Book> bookRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }

        public ShoppingCart GetOrCreateCart(string userId)
        {
            var cart = _cartRepository.GetAll()
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _cartRepository.Insert(cart);
            }

            return cart;
        }

        public void AddToCart(string userId, Guid bookId, int quantity)
        {
            var cart = GetOrCreateCart(userId);
            var existingItem = cart.CartItems.FirstOrDefault(i => i.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _cartItemRepository.Update(existingItem);
            }
            else
            {
                var cartItem = new ShoppingCartItem
                {
                    ShoppingCartId = cart.Id,
                    BookId = bookId,
                    Quantity = quantity,
                    Book = _bookRepository.Get(bookId),
                    
                };
                _cartItemRepository.Insert(cartItem);
                cart.CartItems.Add(cartItem);
            }
        }

        public void UpdateQuantity(string userId, Guid bookId, int quantity)
        {
            var cart = GetOrCreateCart(userId);
            var item = cart.CartItems.FirstOrDefault(i => i.BookId == bookId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    _cartItemRepository.Delete(item);
                }
                else
                {
                    item.Quantity = quantity;
                    _cartItemRepository.Update(item);
                }
            }
        }

        public void RemoveFromCart(string userId, Guid bookId)
        {
            var cart = GetOrCreateCart(userId);
            var item = cart.CartItems.FirstOrDefault(i => i.BookId == bookId);

            if (item != null)
            {
                _cartItemRepository.Delete(item);
            }
        }

        public void ClearCart(string userId)
        {
            var cart = GetOrCreateCart(userId);
            foreach (var item in cart.CartItems.ToList())
            {
                _cartItemRepository.Delete(item);
            }
        }

        public Order CreateOrderFromCart(string userId)
        {
            var cart = GetOrCreateCart(userId);

            if (!cart.CartItems.Any())
                throw new InvalidOperationException("Cannot create order from empty cart");
            decimal price = 0;
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                BookInOrders = cart.CartItems.Select(item => new BookInOrder
                {
                    BookId = (Guid)item.BookId,
                    Quantity = (int)item.Quantity,
                    Book= _bookRepository.Get(item.BookId)

                }).ToList(),
                TotalPrice = (decimal)cart.CartItems.Sum(item => item.Book.Price * item.Quantity)
            };

            _orderRepository.Insert(order);
            ClearCart(userId);

            return order;
        }
    }
}
