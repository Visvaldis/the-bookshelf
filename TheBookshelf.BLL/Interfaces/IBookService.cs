using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IBookService: IService<BookDTO>
	{
	//	ICollection<BookDTO> GetBooksByTag(int tagId);
	//	UserDTO GetBookCreator(int bookId);
	//	void AddTagToBook(int bookId, int tagId);
		ICollection<BookDTO> GetBooksByName(string bookName);
		ICollection<BookDTO> GetWithFilter(Expression<Func<Book, bool>> filter);
		//	ICollection<AuthorDTO> GetAuthorsByBook(int bookId);
	}
}
