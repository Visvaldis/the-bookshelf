using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class TagDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<BookDTO> Books { get; set; }

		public TagDTO()
		{
			Books = new List<BookDTO>();
		}
	}
}
