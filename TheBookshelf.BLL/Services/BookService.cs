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
	class BookService: IBookService
	{
		IUnitOfWork Database { get; set; }
		IMapper Mapper { get; set; }
		public BookService(IUnitOfWork uow)
		{
			Database = uow;
			Mapper = new MapperConfiguration(cfg => {
				cfg.CreateMap<Book, BookDTO>();
				cfg.CreateMap<BookDTO, Book>();
			})
			.CreateMapper();
		}

		public BookDTO GetBookByName(string bookName)
		{
			throw new NotImplementedException();
		}

		public ICollection<BookDTO> GetAll()
		{
			return Mapper.Map<IEnumerable<Book>, List<BookDTO>>(Database.Books.GetAll());

		}

		public BookDTO Get(int id)
		{
			if (id < 1)
				throw new ArgumentException($"id = {id}");
		
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
	
			int id = Database.Books.Create(Mapper.Map<BookDTO, Book>(item));
			Database.Save();
			return id;

		}

		public void Update(BookDTO item)
		{
			if (item == null)
				throw new ArgumentNullException("Book is null. Try again.");

			Database.Books.Update(Mapper.Map<BookDTO, Book>(item));
			Database.Save();
		}

		public void Delete(int id)
		{
			if (id < 1)
				throw new ArgumentException($"id = {id}");
			Database.Books.Delete(id);
			Database.Save();
		}

		public void Dispose()
		{
			Database.Dispose();
		}

		public bool Exist(int id)
		{
			throw new NotImplementedException();
		}

		public bool Exist(string Name)
		{
			throw new NotImplementedException();
		}
	}
}
