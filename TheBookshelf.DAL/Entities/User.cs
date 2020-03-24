using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.DAL.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime RegistrationDate { get; set; }
		public virtual ICollection<Book> LikedBooks { get; set; }
		public virtual ICollection<Book> AddedBooks { get; set; }
		public string AvatarUrl { get; set; }

		public User()
		{
			LikedBooks = new List<Book>();
			AddedBooks = new List<Book>();
		}
	}
}
