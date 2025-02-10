using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Domain.Domain;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;

        public PublisherService(IRepository<Publisher> bookRepository)
        {
            this._publisherRepository = bookRepository;
        }

        public void CreateNewPublisher(Publisher b)
        {
            _publisherRepository.Insert(b);
        }

        public void DeletePublisher(Guid id)
        {
            Publisher b = _publisherRepository.Get(id);
            _publisherRepository.Delete(b);
        }

        public List<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAll().ToList();
        }

        public Publisher GetDetailsForPublisher(Guid? id)
        {
            return _publisherRepository.Get(id);
        }

        public void UpdateExistingPublisher(Publisher b)
        {
            _publisherRepository.Update(b);
        }
    }
}
