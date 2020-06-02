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
		//		public string Password { get; set; }
	//	public DateTime RegistrationDate { get; set; }
		public ICollection<RoleDTO> Roles { get; }
		//	public virtual ICollection<BookDTO> LikedBooks { get; set; }
		//		public virtual ICollection<BookDTO> AddedBooks { get; set; }
		public string AvatarUrl { get; set; }
/*
		public UserDTO()
		{
			LikedBooks = new List<BookDTO>();
			AddedBooks = new List<BookDTO >();
		}

*/
	}

	public class RoleDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

}
