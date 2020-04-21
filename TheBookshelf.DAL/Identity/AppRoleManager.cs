using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.DAL.Identity
{
	public class AppRoleManager : RoleManager<Role, int>
	{
		public AppRoleManager(IRoleStore<Role, int> store)
					: base(store)
		{ }
	}
}
