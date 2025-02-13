using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using CsvHelper;
using System.Globalization;
using Project.Service.Interface;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Service.Interface;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can export data
    public class ExportController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IOrderService _orderService;
        private readonly IPublisherService _publisherService;

        public ExportController(
            IBookService bookService,
            IAuthorService authorService,
            IOrderService orderService,
            IPublisherService publisherService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _orderService = orderService;
            _publisherService = publisherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportBooks()
        {
            var books = _bookService.GetAllBooks();
            var bookRecords = books.Select(b => new
            {
                b.ISBN,
                b.Title,
                b.Description,
                b.Date,
                b.Price,
                Authors = string.Join(", ", b.Authors?.Select(a => a.Name) ?? Array.Empty<string>()),
                Publisher = b.Publisher?.Name,
            });

            return ExportToCsv(bookRecords, "books.csv");
        }

        [HttpPost]
        public IActionResult ExportAuthors()
        {
            var authors = _authorService.GetAuthorList();
            var authorRecords = authors.Select(a => new
            {
                a.Name,
                a.Biography,
                BooksCount = a.Books?.Count ?? 0,
                Books = string.Join(", ", a.Books?.Select(b => b.Title) ?? Array.Empty<string>())
            });

            return ExportToCsv(authorRecords, "authors.csv");
        }

        [HttpPost]
        public IActionResult ExportOrders()
        {
            var orders = _orderService.GetAllOrders();
            var orderRecords = orders.Select(o => new
            {
                OrderId = o.Id,
                o.OrderDate,
                o.Status,
                o.TotalPrice,
                ItemCount = o.BookInOrders?.Count ?? 0,
                Books = string.Join(", ", o.BookInOrders?.Select(b => b.Book?.Title) ?? Array.Empty<string>())
            });

            return ExportToCsv(orderRecords, "orders.csv");
        }

        [HttpPost]
        public IActionResult ExportPublishers()
        {
            var publishers = _publisherService.GetAllPublishers();
            var publisherRecords = publishers.Select(p => new
            {
                p.Name,
                BooksCount = p.Books?.Count ?? 0,
                Books = string.Join(", ", p.Books?.Select(b => b.Title) ?? Array.Empty<string>())
            });

            return ExportToCsv(publisherRecords, "publishers.csv");
        }

        private FileResult ExportToCsv<T>(IEnumerable<T> records, string fileName)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(records);
            writer.Flush();
            return File(memoryStream.ToArray(), "text/csv", fileName);
        }
    }
}