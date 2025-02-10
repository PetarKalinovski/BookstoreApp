using Domain.Domain_Models;
using Eshop.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        //string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            if (typeof(T).IsAssignableFrom(typeof(Book)))
            {
                return entities
                    .Include("Authors")
                    .Include("Publisher")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Author)))
            {
                return entities
                    .Include("Books")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Order)))
            {
                return entities
                    .Include("BookInOrders")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(BookInOrder)))
            {
                return entities
                    .Include("Order")
                    .Include("Book")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Publisher)))
            {
                return entities
                    .Include("Books")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(ShoppingCart)))
            {
                return entities
                    .Include("CartItems")
                    .Include("CartItems.Book")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Order)))
            {
                return entities
                    .Include("BookInOrders")
                    .Include("BookInOrders.Book")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(BookInOrder)))
            {
                return entities
                    .Include("Book")
                    .AsEnumerable();
            }

            return entities.AsEnumerable();
            
        }

        public T Get(Guid? id)
        {
            if (typeof(T).IsAssignableFrom(typeof(Book)))
            {
                return entities
                    .Include("Authors")
                    .Include("Publisher")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Author)))
            {
                return entities
                    .Include("Books")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Order)))
            {
                return entities
                    .Include("BookInOrders")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(BookInOrder)))
            {
                return entities
                    .Include("Order")
                    .Include("Book")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Publisher)))
            {
                return entities
                    .Include("Books")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(ShoppingCart)))
            {
                return entities
                    .Include("CartItems")
                    .Include("CartItems.Book")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Order)))
            {
                return entities
                    .Include("BookInOrders")
                    .Include("BookInOrders.Book")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(BookInOrder)))
            {
                return entities
                    .Include("Book")
                    .First(s => s.Id == id);
            }
            return entities.First(s => s.Id == id);
            

        }
        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
            return entity;
        }

        public List<T> InsertMany(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            entities.AddRange(entities);
            context.SaveChanges();
            return entities;
        }
    }

}
