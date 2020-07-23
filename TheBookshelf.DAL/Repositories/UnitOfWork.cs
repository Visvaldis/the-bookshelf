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
using Microsoft.AspNet.Identity.EntityFramework;

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
		private TagRepository tagRepository;
		private AppUserStore userStore;
		private RoleStore<Role, int, UserRole> roleStore;

		public IRoleStore<Role, int> RoleStore
		{
			get
			{
				if (roleStore == null)
					roleStore = new RoleStore<Role, int, UserRole>(db);
				return roleStore;
			}
		}
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
