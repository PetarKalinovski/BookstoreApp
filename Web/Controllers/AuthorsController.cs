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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Domain.Identity_Models;

namespace Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly UserManager<IntegratedSystemsUser> _userManager;

        public AuthorsController(IAuthorService authorService, UserManager<IntegratedSystemsUser> userManager)
        {
            _authorService = authorService;
            _userManager = userManager;
        }




        // GET: Authors
        public IActionResult Index()
        {
            return View(_authorService.GetAuthorList());
        }

        // GET: Authors/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author =  _authorService.GetDetailsForAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name,Biography,Id")] Author author)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Challenge();
                }
                author.UserId= user.Id;
                _authorService.CreateNewAuthor(author);
                return RedirectToAction("index", "Books");
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.GetDetailsForAuthor(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Biography,Id")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    _authorService.UpdateExistingAuthor(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("index","Books");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.GetDetailsForAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _authorService.DeleteAuthor(id);

            return RedirectToAction("index", "Books");
        }

        private bool AuthorExists(Guid id)
        {
            return _authorService.GetDetailsForAuthor(id)!=null;
        }
    }
}
