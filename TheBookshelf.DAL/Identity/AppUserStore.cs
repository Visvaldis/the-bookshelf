using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Context;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.DAL.Identity
{
	class AppUserStore : IUserStore<User, int>, IUserPasswordStore<User, int>
	{
		private BookshelfContext db;
		public AppUserStore (BookshelfContext context)
		{
			this.db = context;
		}


		public Task CreateAsync(User user)
		{
			db.Users.Add(user);
			db.Configuration.ValidateOnSaveEnabled = false;
			return db.SaveChangesAsync();
		}

		public Task DeleteAsync(User user)
		{
			db.Users.Remove(user);
			db.Configuration.ValidateOnSaveEnabled = false;
			return db.SaveChangesAsync();
		}

		public void Dispose()
		{
			db.Dispose();
		}

		public Task<User> FindByIdAsync(int userId)
		{
			return db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
		}

		public Task<User> FindByNameAsync(string userName)
		{
			return db.Users.Where(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync();
		}

		public async Task<string> GetPasswordHashAsync(User user)
		{
			return user.PasswordHash;
		}

		public async Task<bool> HasPasswordAsync(User user)
		{
			return user.PasswordHash != null;
		}

		public async Task SetPasswordHashAsync(User user, string passwordHash)
		{
			user.PasswordHash = passwordHash; 
		}

		public Task UpdateAsync(User user)
		{
			db.Users.Attach(user);
			db.Entry(user).State = EntityState.Modified;
			db.Configuration.ValidateOnSaveEnabled = false;
			return db.SaveChangesAsync();
		}
	}
}
