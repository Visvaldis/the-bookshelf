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
using TheBookshelf.DAL.Identity;
using Microsoft.AspNet.Identity;

namespace TheBookshelf.DAL.Repositories
{
	public class UnitOfWork : IUnitOfWork
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
		private AppUserStore userStore;

		public IUserStore<User, int> UserStore
		{
			get
			{
				if (userStore == null)
					userStore = new AppUserStore(db);
				return userStore;
			}
		}

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

	
		public async Task SaveAsync()
		{
			await db.SaveChangesAsync();
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
