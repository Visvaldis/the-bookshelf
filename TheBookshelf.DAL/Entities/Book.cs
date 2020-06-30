using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.DAL.Entities
{
	public class Book
	{
		[Key]
		public int Id { get; set; }
		[Required, MaxLength(256, ErrorMessage = "Book`s name can't more than 256 symbols")]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		public ICollection<Author> Authors { get; set; }
		public ICollection<Tag> Tags { get; set; }
		public ICollection<User> FanUsers { get; set; }
		public int CreatorId { get; set; }
		public virtual User Creator { get; set; }
		[Url] 
		public string FileUrl { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? PublishDate { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? AddedDate { get; set; }
		public int Assessment { get; set; } 
		public Book()
		{
			Authors = new List<Author>();
			Tags = new List<Tag>();
			FanUsers = new List<User>();
		}
	}
}
