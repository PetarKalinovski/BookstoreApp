using Domain.Domain_Models;
using Domain.Identity_Models;
using Eshop.Domain.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Service.Interface;

namespace Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IntegratedSystemsUser> _userManager;
        private readonly IBookService _bookService; 

        public OrdersController(IOrderService orderService, UserManager<IntegratedSystemsUser> userManager,
            IBookService bookService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _bookService = bookService;
        }

        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var orders = _orderService.GetOrdersForUser(user.Id);

            
            return View(orders);
        }
        public IActionResult ListAll()
        {
            var orders = _orderService.GetAllOrders();
            return View(orders);
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var order = _orderService.GetOrderDetails(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            if (order.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return NotFound();
            }

            foreach (var item in order.BookInOrders)
            {
                Book b = _bookService.GetDetailsForBook(item.BookId);
                item.Book = b;
            }

            return View(order);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public  IActionResult UpdateStatus(Guid id, OrderStatus newStatus)
        {
            var order = _orderService.GetOrderDetails(id);
            if (order == null)
            {
                return NotFound();
            }

            _orderService.UpdateOrderStatus(id, newStatus);

            TempData["Success"] = "Order status updated successfully.";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var order = _orderService.GetOrderDetails(id.Value);
            if (order == null || order.UserId != user.Id)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var order = _orderService.GetOrderDetails(id);
            if (order == null || order.UserId != user.Id)
            {
                return NotFound();
            }

            _orderService.DeleteOrder(id);
            TempData["Success"] = "Order has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
