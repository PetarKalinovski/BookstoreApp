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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> authorRepository;

        public AuthorService(IRepository<Author> _authorRepository)
        {
            authorRepository = _authorRepository;   
        } 

        public void CreateNewAuthor(Author b)
        {
           authorRepository.Insert(b);
        }

        public void DeleteAuthor(Guid id)
        {
            authorRepository.Delete(authorRepository.Get(id));
        }

        public List<Author> GetAuthorList()
        {
            return authorRepository.GetAll().ToList();
        }

        public Author GetDetailsForAuthor(Guid? id)
        {
           return authorRepository.Get(id);
        }

        public void UpdateExistingAuthor(Author b)
        {
           authorRepository.Update(b);
        }
    }
}
