using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;

namespace TheBookshelf.BLL.Services
{
	public class BookService: IBookService
	{
		IUnitOfWork Database { get; set; }
		IMapper Mapper { get; set; }
		public BookService(IUnitOfWork uow)
		{
			Database = uow;
			Mapper = Mappers.BookMapper;
		}

		public ICollection<BookDTO> GetBooksByName(string bookName)
		{
			return GetWithFilter(x => x.Name.Contains(bookName));
		}

		public ICollection<BookDTO> GetAll()
		{
			return Mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.GetAll());

		}

		public BookDTO Get(int id)
		{
			var book = Database.Books.Get(id);
			if (book == null)
				throw new ValidationException("Book is not found");

			return Mapper.Map<Book, BookDTO>(book);
		}

		public ICollection<BookDTO> GetWithFilter(Func<BookDTO, bool> filter)
		{
			var mapper = new MapperConfiguration(
			cfg => cfg.CreateMap<Func<BookDTO, bool>,
			Expression<Func<Book, bool>>>())
				.CreateMapper();
			var expression = mapper.Map<Expression<Func<Book, bool>>>(filter);
			
			return Mapper.Map<IEnumerable<Book>, List<BookDTO>>
				(Database.Books.Find(expression).ToList());

		}

		public int Add(BookDTO item)
		{
			if (item == null)
				throw new ArgumentNullException("Book is null. Try again.");
			var book = Mapper.Map<BookDTO, Book>(item);
			int id = Database.Books.Create(book);
		//	Database.Save();
			return id;

		}

		public void Update(BookDTO item)
		{
			Database.Books.Update(Mapper.Map<BookDTO, Book>(item));
			Database.Save();
		}

		public void Delete(int id)
		{
			Database.Books.Delete(id);
			Database.Save();
		}

		public void Dispose()
		{
			Database.Dispose();
		}

		public ICollection<BookDTO> GetBooksByTag(int tagId)
		{
			var tag = Mapper.Map<TagDTO>(Database.Tags.Get(tagId));
			return GetWithFilter(x => x.Tags.Contains(tag));
		}

		public bool Exist(int id)
		{
			var book = Database.Books.Get(id);
			if (book == null) return false;
			else return true;
		}
	}
}
