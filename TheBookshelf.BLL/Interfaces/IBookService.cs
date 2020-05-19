using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IBookService: IService<BookDTO>
	{
		ICollection<BookDTO> GetBooksByTag(int tagId);
	//	UserDTO GetBookCreator(int bookId);
	//	void AddTagToBook(int bookId, int tagId);
		ICollection<BookDTO> GetBooksByName(string bookName);
	//	ICollection<AuthorDTO> GetAuthorsByBook(int bookId);
	}
}
