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
/*
namespace TheBookshelf.DAL.Repositories
{
	class UserRepository : IRepository<User>
	{
		private BookshelfContext db;
		public UserRepository(BookshelfContext context)
		{
			this.db = context;
		}

		public void Create(User item)
		{
			db.Users.Add(item);

		}

		public void Delete(int id)
		{
			User user = db.Users.Find(id);
			if (user != null)
				db.Users.Remove(user);
		}

		public IQueryable<User> Find(Expression<Func<User, bool>> predicate)
		{
			return db.Users
				.Include(x => x.AddedBooks).Include(x => x.LikedBooks)
				.Where(predicate);
		}

		public User Get(int id)
		{
			return db.Users
				.Include(x => x.AddedBooks).Include(x => x.LikedBooks)
				.FirstOrDefault(x=> x.Id == id);
		}

		public IEnumerable<User> GetAll()
		{
			return db.Users; 
		}

		public void Update(User item)
		{
			db.Users.Attach(item);
			db.Entry(item).State = EntityState.Modified;
		}
	}
}
*/