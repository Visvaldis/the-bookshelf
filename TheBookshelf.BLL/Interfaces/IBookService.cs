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
		ICollection<BookDTO> GetWithFilter(Expression<Func<Book, bool>> filter);
		ICollection<BookDTO> GetRandomBooks(int count);
	}
}
