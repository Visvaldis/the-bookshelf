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
	class TagRepository : IRepository<Tag>
	{
		private BookshelfContext db;
		public TagRepository(BookshelfContext context)
		{
			this.db = context;
		}
		public void Create(Tag item)
		{
			db.Tags.Add(item);
		}

		public void Delete(int id)
		{
			Tag tag = db.Tags.Find(id);
		}

		public IQueryable<Tag> Find(Expression<Func<Tag, bool>> predicate)
		{
			return db.Tags.Where(predicate);
		}

		public Tag Get(int id)
		{
			return db.Tags.Find(id);
		}

		public IEnumerable<Tag> GetAll()
		{
			return db.Tags;
		}

		public void Update(Tag item)
		{
			db.Entry(item).State = EntityState.Modified;
		}
	}
}
