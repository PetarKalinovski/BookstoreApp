using Domain.Domain_Models;

namespace Eshop.Domain.Domain
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }

}
