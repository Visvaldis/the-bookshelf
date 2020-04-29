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
		public int Id { get; set; }
		//[Required, MaxLength(256, ErrorMessage = "Author`s name can't more than 256 symbols")]
		public string Name { get; set; }
		public string Bio { get; set; }
		public DateTime Birthday { get; set; }
		public virtual ICollection<Book> Books { get; set; }

		public Author()
		{
			Books = new List<Book>();
		}
	}
}
