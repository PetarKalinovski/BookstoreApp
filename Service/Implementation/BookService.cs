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
    public class BookService : IBookService
    {

        private readonly IRepository<Book> bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public void CreateNewBook(Book b)
        {
            bookRepository.Insert(b);
        }

        public void DeleteBook(Guid id)
        {
            Book b = bookRepository.Get(id);
            bookRepository.Delete(b);
        }

        public List<Book> GetAllBooks()
        {
           return bookRepository.GetAll().ToList();
        }

        public Book GetDetailsForBook(Guid? id)
        {
            return bookRepository.Get(id);
        }

        public void UpdateExistingBook(Book b)
        {
            bookRepository.Update(b);
        }
    }
}
