using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class AuthorDTO
	{
		/// <summary>
		/// Author identifier
		/// </summary>
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// Author full name
		/// </summary>
		[Required, MaxLength(256, ErrorMessage = "Author`s name can't more than 256 symbols")]
		public string Name { get; set; }
		/// <summary>
		/// Author biography
		/// </summary>
		[Required]
		public string Bio { get; set; }
		/// <summary>
		/// Author bithday
		/// </summary>
		[Required]
		public DateTime Birthday { get; set; }

	}
}
