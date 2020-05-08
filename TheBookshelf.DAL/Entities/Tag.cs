using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.DAL.Entities
{
	public class Tag
	{
		public int Id { get; set; }
		//[Required]
		public string Name { get; set; }
		public virtual ICollection<Book> Books { get; set; }
		public Tag()
		{
			Books = new List<Book>();
		}
	}
}
