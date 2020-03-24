using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Context;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;

namespace TheBookshelf.DAL.Repositories
{
	class BookRepository : IRepository<Book>
	{
		private readonly BookshelfContext db;
		public BookRepository(BookshelfContext context)
		{
			this.db = context;
		}
		public void Create(Book item)
		{
			db.Books.Add(item);
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<Book> Find(Expression<Func<Book, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public Book Get(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Book> GetAll()
		{
			throw new NotImplementedException();
		}

		public void Update(Book item)
		{
			throw new NotImplementedException();
		}
	}
}
