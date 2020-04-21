﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TheBookshelf.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheBookshelf.DAL.Context
{
	class BookshelfContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
	{
		public DbSet<Author> Authors { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Book> Books { get; set; }

		public BookshelfContext(string connectionString)
				: base(connectionString)
		{ }
		public BookshelfContext() : base("BookshelfContext")
		{ }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(a => a.AddedBooks)
				.WithRequired(p => p.Creator);
			modelBuilder.Entity<Book>()
				.HasMany(p => p.FanUser)
				.WithMany(c => c.LikedBooks)
				.Map(m =>
				  {
					  m.ToTable("LikedBooks");

				  });
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<Role>().ToTable("Roles");
			modelBuilder.Entity<UserRole>().ToTable("UserRoles");
			modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
			modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
		}
	}
}
