using Microsoft.AspNet.Identity;

using TheBookshelf.DAL.Entities;

namespace TheBookshelf.DAL.Identity
{
	public class AppUserManager : UserManager<User, int>
	{
		public AppUserManager(IUserStore<User, int> store) : base(store)
		{
		}
	}
}
