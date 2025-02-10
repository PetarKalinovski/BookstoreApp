using Domain.Domain_Models;

namespace Eshop.Domain.Domain
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }

}
