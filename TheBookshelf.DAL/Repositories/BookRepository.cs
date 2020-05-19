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
					/*
					aut = new Author {Bio = author.Bio, Birthday = author.Birthday, Books = author.Books, Name = author.Name };
					db.Authors.Add(aut);*/
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
			var tags = item.Tags;
			item.Tags = new List<Tag>();
			foreach (var booktag in tags)
			{
				Tag tag = db.Tags.SingleOrDefault(t => t.Name == booktag.Name);
				if (tag == null)
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
					/*
					aut = new Author {Bio = author.Bio, Birthday = author.Birthday, Books = author.Books, Name = author.Name };
					db.Authors.Add(aut);*/
					continue;
				}
				item.Authors.Add(aut);
			}

			db.Books.Attach(item);
			db.Entry(item).State = EntityState.Modified;
		}
	}
}
