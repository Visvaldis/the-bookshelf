using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class AuthorDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Bio { get; set; }
		public DateTime Birthday { get; set; }
		public virtual ICollection<BookDTO> Books { get; set; }

		public AuthorDTO()
		{
			Books = new List<BookDTO>();
		}
	}
}
