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
	class TagRepository : IRepository<Tag>
	{
		private BookshelfContext db;
		public TagRepository(BookshelfContext context)
		{
			this.db = context;
		}
		public int Create(Tag item)
		{
			db.Tags.Add(item);
			db.SaveChanges();
			return item.Id;
		}

		public void Delete(int id)
		{
			Tag tag = db.Tags.Find(id);
			if (tag != null)
				db.Tags.Remove(tag);
		}

		public IEnumerable<Tag> Find(Expression<Func<Tag, bool>> predicate)
		{
			return db.Tags.Where(predicate).ToList();
		}

		public Tag Get(int id)
		{
			return GetAllQuary()
				.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Tag> GetAll()
		{
			return db.Tags.ToArray();
		}

		public void Update(Tag item)
		{
			var entity = db.Tags.Find(item.Id);
			if (entity == null)
			{
				return;
			}
			db.Entry(entity).CurrentValues.SetValues(item);
		}


		private IQueryable<Tag> GetAllQuary()
		{
			return db.Tags
					.Include(x => x.Books.Select(y => y.Authors));
		}
	}
}
