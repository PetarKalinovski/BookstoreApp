using Eshop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Interface
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetDetailsForBook(Guid? id);
        void CreateNewBook(Book b);
        void UpdateExistingBook(Book b);
        void DeleteBook(Guid id);

        List<Book> GetBooksByName(string name);
    }
}
