using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Context;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.DAL.Identity
{
	class AppUserStore : IUserStore<User, int>, IUserPasswordStore<User, int>, IUserRoleStore<User,int>
	{
		private BookshelfContext db;
		public AppUserStore (BookshelfContext context)
		{
			this.db = context;
		}

		public Task AddToRoleAsync(User user, string roleName)
		{
			var role = db.Roles.Where(r => r.Name == roleName).FirstOrDefault();
			user.Roles.Add(new UserRole {RoleId=role.Id, UserId=user.Id});
			return db.SaveChangesAsync();
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

		public async Task<IList<string>> GetRolesAsync(User user)
		{
			var res = new List<string>();
			var idlist = user.Roles.Select(x => x.RoleId).ToList();
			idlist.ForEach(id => res.Add(db.Roles.Where(r => r.Id == id).FirstOrDefault().Name));
			return res;
		}

		public async Task<bool> HasPasswordAsync(User user)
		{
			return  user.PasswordHash != null;
		}

		public async Task<bool> IsInRoleAsync(User user, string roleName)
		{
			var role = db.Roles.Where(r => r.Name == roleName).FirstOrDefault();
			var list = user.Roles.Where(u => u.RoleId == role.Id).FirstOrDefault();
			return list != null;
		}

		public Task RemoveFromRoleAsync(User user, string roleName)
		{
			var role = db.Roles.Where(r => r.Name == roleName).FirstOrDefault();
			user.Roles.Remove(new UserRole { RoleId = role.Id, UserId = user.Id });
			return db.SaveChangesAsync();
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
