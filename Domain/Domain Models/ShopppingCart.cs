using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Identity_Models;

namespace Domain.Domain_Models
{
    public class ShoppingCart : BaseEntity
    {
        public string UserId { get; set; }
        public IntegratedSystemsUser User { get; set; }
        public ICollection<ShoppingCartItem> CartItems { get; set; }
    }
}
