using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TheBookshelf.DAL.Entities
{
	[Table("Users")]
	public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IUser<int>
	{
		public virtual ICollection<Book> LikedBooks { get; set; }

		public User()
		{
			LikedBooks = new List<Book>();
		}

		public User(ICollection<Book> likedBooks, string avatarUrl)
		{
			LikedBooks = likedBooks;
		}
	}


	public class UserLogin : IdentityUserLogin<int>
	{
	}

	public class Role : IdentityRole<int, UserRole>, IRole<int>
	{
	}

	public class UserRole : IdentityUserRole<int>
	{
	}

	public class UserClaim : IdentityUserClaim<int>
	{
	}
}
