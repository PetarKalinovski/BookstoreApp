using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Domain.Domain;
using Repository;
using Service.Interface;

namespace Web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // GET: Publishers
        public IActionResult Index()
        {
            return View(_publisherService.GetAllPublishers());
        }

        // GET: Publishers/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _publisherService.GetDetailsForPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Id")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _publisherService.CreateNewPublisher(publisher);
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _publisherService.GetDetailsForPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Id")] Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _publisherService.UpdateExistingPublisher(publisher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id))
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
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _publisherService.GetDetailsForPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _publisherService.DeletePublisher(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(Guid id)
        {
            return _publisherService.GetAllPublishers().Any(e => e.Id == id);
        }
    }
}
