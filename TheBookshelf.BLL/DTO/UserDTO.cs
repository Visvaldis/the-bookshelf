using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }

		public ICollection<RoleDTO> Roles { get; }

	}

	public class RoleDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

}
