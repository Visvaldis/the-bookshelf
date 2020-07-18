using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class TagDTO
	{
		/// <summary>
		/// Tag identifier
		/// </summary>
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// Tag title
		/// </summary>
		[Required]
		public string Name { get; set; }
		/// <summary>
		/// Count of books with this tag
		/// </summary>
		public int BookCount { get; set; }

	}
}
