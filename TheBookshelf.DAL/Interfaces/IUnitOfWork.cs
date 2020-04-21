using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Identity;

namespace TheBookshelf.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Author> Authors { get; }
		IRepository<Tag> Tags { get; }
		IRepository<Book> Books { get; }
		IRepository<User> Users { get; }
		AppUserManager UserManager { get; }
		AppRoleManager RoleManager { get; }

		void Save();
		Task SaveAsync();
	}
}
