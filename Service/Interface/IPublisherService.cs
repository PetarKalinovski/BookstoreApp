using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Domain.Domain;

namespace Service.Interface
{
    public interface IPublisherService
    {
        List<Publisher> GetAllPublishers();
        Publisher GetDetailsForPublisher(Guid? id);
        void CreateNewPublisher(Publisher b);
        void UpdateExistingPublisher(Publisher b);
        void DeletePublisher(Guid id);
    }
}
