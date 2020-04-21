using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.DAL.Entities
{
	[Table("Users")]
	public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
	{
		public virtual ICollection<Book> LikedBooks { get; set; }
		public virtual ICollection<Book> AddedBooks { get; set; }
		public string AvatarUrl { get; set; }

		public User()
		{
			LikedBooks = new List<Book>();
			AddedBooks = new List<Book>();
		}

		public User(ICollection<Book> likedBooks, ICollection<Book> addedBooks, string avatarUrl)
		{
			LikedBooks = likedBooks;
			AddedBooks = addedBooks;
			AvatarUrl = avatarUrl;
		}
	}

	public class UserLogin : IdentityUserLogin<int>
	{
	}

	public class Role : IdentityRole<int, UserRole>
	{
	}

	public class UserRole : IdentityUserRole<int>
	{
	}

	public class UserClaim : IdentityUserClaim<int>
	{
	}
}
