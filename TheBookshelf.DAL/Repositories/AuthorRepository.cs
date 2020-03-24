using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Context;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;
using System.Data.Entity;
using System.Linq.Expressions;

namespace TheBookshelf.DAL.Repositories
{
	class AuthorRepository : IRepository<Author>
	{
		private BookshelfContext db;
		public AuthorRepository(BookshelfContext context)
		{
			this.db = context;
		}
		public void Create(Author item)
		{
			db.Authors.Add(item);
		}

		public void Delete(int id)
		{
			Author author = db.Authors.Find(id);
			if (author != null)
				db.Authors.Remove(author);
		}

		public IQueryable<Author> Find(Expression<Func<Author, bool>> predicate)
		{
			return db.Authors.Include(x => x.Books).Where(predicate);
		}

		public Author Get(int id)
		{
			return db.Authors.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Author> GetAll()
		{
			return db.Authors;
		}

		public void Update(Author item)
		{
			db.Entry(item).State = EntityState.Modified;
		}
	}
}
