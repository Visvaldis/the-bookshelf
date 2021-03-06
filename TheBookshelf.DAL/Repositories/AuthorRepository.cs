﻿using System;
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
		public int Create(Author item)
		{
			db.Authors.Add(item);
			db.SaveChanges();
			return item.Id;
		}

		public void Delete(int id)
		{
			Author author = db.Authors.Find(id);
			if (author != null)
				db.Authors.Remove(author);
		}

		public IEnumerable<Author> Find(Expression<Func<Author, bool>> predicate)
		{
			return GetAllQuary().Where(predicate).ToList();
		}

		public Author Get(int id)
		{
			return GetAllQuary().FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Author> GetAll()
		{
			return GetAllQuary().ToList();
		}

		public void Update(Author item)
		{
			var entity = db.Authors.Find(item.Id);
			if (entity == null)
			{
				return;
			}
			db.Entry(entity).CurrentValues.SetValues(item);
		}

		private IQueryable<Author> GetAllQuary()
		{
			return db.Authors.Include(x => x.Books.Select(y => y.Tags));
		}

	}
}
