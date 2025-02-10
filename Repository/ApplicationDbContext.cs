using Domain.Domain_Models;
using Domain.Identity_Models;
using Eshop.Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace Repository
{
    public class ApplicationDbContext : IdentityDbContext<IntegratedSystemsUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        public virtual DbSet<BookInOrder> BookInOrders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasMany(b => b.Authors)
                    .WithMany(a => a.Books);
                entity.HasOne(b => b.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(b => b.PublisherId);
            });

            modelBuilder.Entity<BookInOrder>(entity =>
            {
                entity.HasOne(bo => bo.Order)
                    .WithMany(o => o.BookInOrders)
                    .HasForeignKey(bo => bo.OrderId);

                entity.HasOne(bo => bo.Book)
                    .WithMany()
                    .HasForeignKey(bo => bo.BookId);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasOne(sc => sc.User)
                    .WithOne()
                    .HasForeignKey<ShoppingCart>(sc => sc.UserId);
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasOne(sci => sci.ShoppingCart)
                    .WithMany(sc => sc.CartItems)
                    .HasForeignKey(sci => sci.ShoppingCartId);

                entity.HasOne(sci => sci.Book)
                    .WithMany()
                    .HasForeignKey(sci => sci.BookId);
            });
        }
    }
}
