using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.DAL.Entities
{
	public class Book
	{
		public int Id { get; set; }
	//	[Required, MaxLength(256, ErrorMessage = "Book`s name can't more than 256 symbols")]
		public string Name { get; set; }
		public string Description { get; set; }
		public ICollection<Author> Authors { get; set; }
		public ICollection<Tag> Tags { get; set; }
		public ICollection<User> FanUser { get; set; }
		public User Creator { get; set; }
		public string FileUrl { get; set; }
		public DateTime PublishDate { get; set; }
		public DateTime AddedDate { get; set; }
		public int Assessment { get; set; } 
		public Book()
		{
			Authors = new List<Author>();
			Tags = new List<Tag>();
			FanUser = new List<User>();
		}
	}
}
