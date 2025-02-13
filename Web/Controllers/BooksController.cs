using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Domain.Domain;
using Repository;
using Project.Service.Interface;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Service.Implementation;
using Domain.Identity_Models;

namespace Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        private readonly UserManager<IntegratedSystemsUser> _userManager;
        private readonly IShoppingCartService _shoppingCartService;

        public BooksController(IBookService bookService, IAuthorService authorService, IPublisherService publisherService,
            UserManager<IntegratedSystemsUser> userManager, IShoppingCartService shoppingCartService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _publisherService = publisherService;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
        }
        // GET: Books
        public IActionResult Index(string searchString)
        {
            var books=_bookService.GetAllBooks();
            if (!string.IsNullOrEmpty(searchString))
            {
                books = _bookService.GetBooksByName(searchString);
            }

            return View(books);
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }
            List<Author>? authors = book?.Authors?.ToList();
            ViewData["Publisher"] = book.Publisher;
            ViewData["Authors"]=authors;
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(Guid id, int quantity)
        {
            var book = _bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            try
            {
                _shoppingCartService.AddToCart(user.Id, id, quantity);
                TempData["Success"] = $"{book.Title} has been added to your cart.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There was an error adding the item to your cart.";
            }

            return RedirectToAction(nameof(Details), new { id = book.Id });
        }
        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(_authorService.GetAuthorList(), "Id", "Name");
            ViewBag.Publishers = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public async Task<IActionResult> Create([Bind("ISBN,Title,Description,Date,Price,PictureUrl,Id,AuthorIds,PublisherId")] Book book)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Challenge();
                }

                book.UserId = user.Id;

                var selectedAuthors = _authorService.GetAuthorList().Where(a => book.AuthorIds.Contains(a.Id));
                var publisher = _publisherService.GetDetailsForPublisher(book.PublisherId);
                if (selectedAuthors.Any())
                {
                    book.Authors = selectedAuthors.ToList();
                    foreach (Author a in selectedAuthors)
                    {
                        if (a.Books == null)
                        {
                            a.Books = new List<Book>();
                        }
                        a.Books.Add(book);
                    }
                }
                if (publisher != null)
                {
                    book.Publisher = publisher;
                    if (publisher.Books == null)
                    {
                        publisher.Books = new List<Book>();
                    }
                    publisher.Books.Add(book);
                }
                _bookService.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Authors = new SelectList(_authorService.GetAuthorList(), "Id", "Name");
            ViewBag.Publishers = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name");
            return View(book);
        }

        // GET: Books/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewBag.Authors = new SelectList(_authorService.GetAuthorList(), "Id", "Name");
            ViewBag.Publishers = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name");
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, [Bind("ISBN,Title,Description,Date,Price,PictureUrl,Id, AuthorIds, PublisherId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return Challenge();
                    }

                    book.UserId = user.Id;

                    var existingBook = _bookService.GetDetailsForBook(id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    // Handle Authors
                    var selectedAuthors = _authorService.GetAuthorList().Where(a => book.AuthorIds.Contains(a.Id));

                    // Remove book from old authors' Books collections
                    if (existingBook.Authors != null)
                    {
                        foreach (var oldAuthor in existingBook.Authors)
                        {
                            oldAuthor.Books?.Remove(existingBook);
                        }
                    }

                    // Add book to new authors' Books collections
                    existingBook.Authors = selectedAuthors.ToList();
                    foreach (var newAuthor in selectedAuthors)
                    {
                        if (newAuthor.Books == null)
                        {
                            newAuthor.Books = new List<Book>();
                        }
                        newAuthor.Books.Add(existingBook);
                    }

                    // Handle Publisher
                    if (existingBook.PublisherId != book.PublisherId)
                    {
                        // Remove from old publisher's Books collection
                        if (existingBook.Publisher != null)
                        {
                            existingBook.Publisher.Books?.Remove(existingBook);
                        }

                        // Add to new publisher's Books collection
                        var newPublisher = _publisherService.GetDetailsForPublisher(book.PublisherId);
                        if (newPublisher != null)
                        {
                            existingBook.Publisher = newPublisher;
                            if (newPublisher.Books == null)
                            {
                                newPublisher.Books = new List<Book>();
                            }
                            newPublisher.Books.Add(existingBook);
                        }
                    }
                    existingBook.UserId=user.Id;
                    existingBook.ISBN = book.ISBN;
                    existingBook.Title = book.Title;
                    existingBook.Description = book.Description;
                    existingBook.Date = book.Date;
                    existingBook.Price = book.Price;
                    existingBook.PictureUrl = book.PictureUrl;

                    _bookService.UpdateExistingBook(existingBook);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Authors = new SelectList(_authorService.GetAuthorList(), "Id", "Name");
            ViewBag.Publishers = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name");
            return View(book);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _bookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return _bookService.GetAllBooks().Any(e => e.Id == id);
        }
    }
}
