using Domain.Domain_Models;

namespace Eshop.Domain.Domain
{
    public class BookInOrder : BaseEntity
    {
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }
    }

}
