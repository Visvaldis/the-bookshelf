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
		[Required]
		public int Id { get; set; }
		/// <summary>
		/// Author name
		/// </summary>
		[MaxLength(30)]
		public string Name { get; set; }
		/// <summary>
		/// authorbio
		/// </summary>
		public string Bio { get; set; }
		public DateTime Birthday { get; set; }

	}
}
