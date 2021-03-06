﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IAuthorService : IService<AuthorDTO>
	{
		ICollection<BookDTO> GetBooksByAuthor(int authorId);
	}
}
