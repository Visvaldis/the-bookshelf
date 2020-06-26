using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.DAL.Entities
{
	public class Author
	{
		[Key]
		public int Id { get; set; }
		[Required, MaxLength(256, ErrorMessage = "Author`s name can't more than 256 symbols")]
		public string Name { get; set; }
		[Required]
		public string Bio { get; set; }
		[Required]
		public DateTime Birthday { get; set; }
		public virtual ICollection<Book> Books { get; set; }

		public Author()
		{
			Books = new List<Book>();
		}
	}
}
