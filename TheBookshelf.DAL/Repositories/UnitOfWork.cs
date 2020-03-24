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
	class UnitOfWork : IUnitOfWork
	{
		public UnitOfWork(string connectionString)
		{
			db = new BookshelfContext(connectionString);
		}
		private BookshelfContext db;
		private BookRepository bookRepository;
		private AuthorRepository authorRepository;
		private UserRepository userRepository;
		private TagRepository tagRepository;
		public IRepository<Author> Authors
		{
			get
			{
				if (authorRepository == null)
					authorRepository = new AuthorRepository(db);
				return authorRepository;
			}
		}

		public IRepository<Tag> Tags
		{
			get
			{
				if (tagRepository == null)
					tagRepository = new TagRepository(db);
				return tagRepository;
			}
		}

		public IRepository<Book> Books
		{
			get
			{
				if (bookRepository == null)
					bookRepository = new BookRepository(db);
				return bookRepository;
			}
		}

		public IRepository<User> Users
		{
			get
			{
				if (userRepository == null)
					userRepository = new UserRepository(db);
				return userRepository;
			}
		}
		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Save()
		{
			db.SaveChanges();
		}
	}
}
