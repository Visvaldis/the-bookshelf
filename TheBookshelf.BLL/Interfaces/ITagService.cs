using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;

namespace TheBookshelf.BLL.Interfaces
{
	public interface ITagService :IService<TagDTO>
	{
		IEnumerable<BookDTO> GetBooksByTag(int tagId);
		//	int GetBookCountByTag(int tagId);
		bool Exist(int id);
		bool Exist(string Name);
	}
}
