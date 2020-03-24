using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TheBookshelf.DAL.Entities;
namespace TheBookshelf.DAL.Context
{
	class BookshelfContext :DbContext
	{
		public DbSet<Author> Authors { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<User> Users { get; set; }

		public BookshelfContext(string connectionString)
				: base(connectionString)
		{ }
	}


}
