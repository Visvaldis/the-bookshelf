using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class UserDTO
	{
		/// <summary>
		/// Tag identifier
		/// </summary>
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// UserName
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// Colection of roles
		/// </summary>
		public ICollection<RoleDTO> Roles { get; }

	}

	public class RoleDTO
	{
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// Role name
		/// </summary>
		public string Name { get; set; }
	}

}
