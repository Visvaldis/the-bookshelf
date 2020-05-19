using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.DTO
{
	public class BookDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public ICollection<AuthorDTO> Authors { get; set; }
		public ICollection<TagDTO> Tags { get; set; }
	//	public ICollection<UserDTO> FanUsers { get; set; }
		public UserDTO Creator { get; set; }
		public int CreatorId { get; set; }
		public string FileUrl { get; set; }
		public DateTime PublishDate { get; set; }
		public DateTime AddedDate { get; set; }
		public int Assessment { get; set; }
		public BookDTO()
		{
			Authors = new List<AuthorDTO>();
			Tags = new List<TagDTO>();
	//		FanUsers = new List<UserDTO>();
		}
	}
}
