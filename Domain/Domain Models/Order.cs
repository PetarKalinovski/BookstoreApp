using Domain.Domain_Models;
using Domain.Identity_Models;

namespace Eshop.Domain.Domain
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public IntegratedSystemsUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<BookInOrder>? BookInOrders { get; set; }
        public decimal TotalPrice { get; set; }
    }

}

