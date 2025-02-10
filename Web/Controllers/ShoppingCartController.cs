using Domain.Identity_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _cartService;
        private readonly UserManager<IntegratedSystemsUser> _userManager;

        public ShoppingCartController(
            IShoppingCartService cartService,
            UserManager<IntegratedSystemsUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var cart = _cartService.GetOrCreateCart(user.Id);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid bookId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            try
            {
                _cartService.AddToCart(user.Id, bookId, quantity);
                TempData["CartSuccess"] = "Item added to cart successfully!";
            }
            catch (Exception ex)
            {
                TempData["CartError"] = "Error adding item to cart.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid bookId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            try
            {
                _cartService.UpdateQuantity(user.Id, bookId, quantity);
                TempData["CartSuccess"] = "Cart updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["CartError"] = "Error updating cart.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            _cartService.RemoveFromCart(user.Id, bookId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var order = _cartService.CreateOrderFromCart(user.Id);
            return RedirectToAction("Details", "Orders", new { id = order.Id });
        }
    }
}
