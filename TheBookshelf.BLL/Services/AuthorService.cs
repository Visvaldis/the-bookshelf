using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;

namespace TheBookshelf.BLL.Services
{
	public class AuthorService : IAuthorService
	{
		IUnitOfWork Database { get; set; }
		IMapper Mapper { get; set; }
		public AuthorService(IUnitOfWork uow)
		{
			Database = uow;
			Mapper = Mappers.BookshelfMapper;
		}

		public int Add(AuthorDTO item)
		{
			if (item == null)
				throw new ArgumentNullException("Author is null. Try again.");
			var author = Mapper.Map<AuthorDTO, Author>(item);
			int id = Database.Authors.Create(author);
			return id;
		}

		public void Delete(int id)
		{
			Database.Authors.Delete(id);
			Database.Save();
		}

		public void Dispose()
		{
			Database.Dispose();
		}

		public bool Exist(int id)
		{
			var author = Database.Authors.Get(id);
			if (author == null) return false;
			else return true;
		}

		public AuthorDTO Get(int id)
		{
			var author = Database.Authors.Get(id);
			if (author == null)
				throw new ValidationException("Author is not found");

			return Mapper.Map<Author, AuthorDTO>(author);
		}

		public ICollection<AuthorDTO> GetAll()
		{
			var query = Database.Authors.GetAll();
			var authors = query.ToList();
			return Mapper.Map<IEnumerable<Author>, List<AuthorDTO>>(authors);
		}

		public ICollection<BookDTO> GetBooksByAuthor(int authorId)
		{
			var author = Database.Authors.Get(authorId);
			if (author == null)
				throw new ValidationException("Current author is not found");

			var books = author.Books;
			var bookDto = Mappers.BookshelfMapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
			return bookDto;
		}

		public void Update(AuthorDTO item)
		{
			Database.Authors.Update(Mapper.Map<AuthorDTO, Author>(item));
			Database.Save();
		}
	}
}
