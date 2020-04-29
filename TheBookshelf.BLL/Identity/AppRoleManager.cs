using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.BLL.Identity
{
	public class ApplicationRoleManager : RoleManager<Role, int>
	{
		public ApplicationRoleManager(IRoleStore<Role, int> store)
					: base(store)
		{ }
	}
}
