using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;

namespace TheBookshelf.BLL.Interfaces
{
	interface IBookService: IService<BookDTO>
	{
	//	ICollection<TagDTO> GetTagsByBook(int bookId);
	//	UserDTO GetBookCreator(int bookId);
	//	void AddTagToBook(int bookId, int tagId);
		BookDTO GetBookByName(string bookName);
	//	ICollection<AuthorDTO> GetAuthorsByBook(int bookId);
	}
}
