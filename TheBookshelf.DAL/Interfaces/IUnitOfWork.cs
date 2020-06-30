using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Author> Authors { get; }
		IRepository<Tag> Tags { get; }
		IRepository<Book> Books { get; }
	//	IRepository<User> Users { get; }
		IUserStore<User, int> UserStore { get; }
		IRoleStore<Role, int> RoleStore { get; }

		void Save();
		Task SaveAsync();
	}
}
