using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class BookDTO
	{
		/// <summary>
		/// Book identifier
		/// </summary>
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// Book  title
		/// </summary>
		[Required, MaxLength(256, ErrorMessage = "Book`s name can't more than 256 symbols")]
		public string Name { get; set; }
		/// <summary>
		/// Book annotation
		/// </summary>
		[Required]
		public string Description { get; set; }
		/// <summary>
		/// Collection of book's authors
		/// </summary>
		public ICollection<AuthorDTO> Authors { get; set; }
		/// <summary>
		/// Collection of book's tags
		/// </summary>
		public ICollection<TagDTO> Tags { get; set; }
		public ICollection<UserDTO> FanUsers { get; set; }
		/// <summary>
		/// Url to book cover image
		/// </summary>
		[Url]
		public string CoverUrl { get; set; }
		public int Assessment { get; set; }
		public BookDTO()
		{
			Authors = new List<AuthorDTO>();
			Tags = new List<TagDTO>();
			FanUsers = new List<UserDTO>();
		}
	}
}
