using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Context;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;
using System.Data.Entity;


namespace TheBookshelf.DAL.Repositories
{
	class BookRepository : IRepository<Book>
	{
		private readonly BookshelfContext db;
		public BookRepository(BookshelfContext context)
		{
			this.db = context;
		}
		public int Create(Book item)
		{
			db.Books.Add(item);
			return item.Id;
		}

		public void Delete(int id)
		{
			Book book = db.Books.Find(id);
			if (book != null)
				db.Books.Remove(book);
		}

		public IQueryable<Book> Find(Expression<Func<Book, bool>> predicate)
		{
			return db.Books
				.Include(x => x.Tags).Include(x => x.Creator).Include(x => x.Authors)
				.Where(predicate);
		}

		public Book Get(int id)
		{
			return db.Books
				.Include(x => x.Tags).Include(x => x.Creator).Include(x => x.Authors)
				.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Book> GetAll()
		{
			return db.Books
				.Include(x => x.Tags).Include(x => x.Creator).Include(x => x.Authors);
		}

		public void Update(Book item)
		{
			db.Books.Attach(item);
			db.Entry(item).State = EntityState.Modified;
		}
	}
}
