﻿using System;
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
			var tags = item.Tags;
			item.Tags = new List<Tag>();
			foreach (var booktag in tags)
			{
				Tag tag = db.Tags.SingleOrDefault(t => t.Name == booktag.Name);
				if(tag == null)
				{
					tag = new Tag { Name = booktag.Name };
					db.Tags.Add(tag);
				}
				item.Tags.Add(tag);
			}

			var authors = item.Authors;
			item.Authors = new List<Author>();
			foreach (var author in authors)
			{
				Author aut = db.Authors.SingleOrDefault(t => t.Name == author.Name);
				if (aut == null)
				{
					continue;
				}
				item.Authors.Add(aut);
			}

			db.Books.Add(item);
			db.SaveChanges();
			return item.Id;
		}

		public void Delete(int id)
		{
			Book book = db.Books.Find(id);
			if (book != null)
				db.Books.Remove(book);
		}

		public IEnumerable<Book> Find(Expression<Func<Book, bool>> predicate)
		{
			return GetAllQuary()
				.Where(predicate).ToList();
		}

		public Book Get(int id)
		{
			return GetAllQuary()
				.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Book> GetAll()
		{
			return GetAllQuary();
		}

		public void Update(Book item)
		{
			var entity = db.Books.Find(item.Id);
			if (entity == null)
			{
				return;
			}

			var tags = item.Tags;
			entity.Tags = new List<Tag>();
			foreach (var booktag in tags)
			{
				Tag tag = db.Tags.SingleOrDefault(t => t.Name == booktag.Name);
				if (tag == null)
				{
					tag = new Tag { Name = booktag.Name };
					db.Tags.Add(tag);
				}
				entity.Tags.Add(tag);
			}

			var authors = item.Authors;
			entity.Authors = new List<Author>();
			foreach (var author in authors)
			{
				Author aut = db.Authors.SingleOrDefault(t => t.Name == author.Name);
				if (aut == null)
				{
					continue;
				}
				entity.Authors.Add(aut);
			}
			db.Entry(entity).CurrentValues.SetValues(item);
		}

		private IQueryable<Book> GetAllQuary()
		{
			return db.Books
					.Include(x => x.Tags).Include(x => x.Authors).Include(x => x.FanUsers);
		}
	}
}
