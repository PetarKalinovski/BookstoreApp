using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Domain.Domain;

namespace Domain.Domain_Models
{
    public class ShoppingCartItem : BaseEntity
    {
        public Guid? BookId { get; set; }
        public Book? Book { get; set; }
        public int? Quantity { get; set; }
        public Guid? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
    }
}
