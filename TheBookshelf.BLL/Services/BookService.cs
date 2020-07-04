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
			Mapper = Mappers.BookshelfMapper;
		}

		public ICollection<BookDTO> GetBooksByName(string bookName)
		{
			return GetWithFilter(x => x.Name.Contains(bookName));
		}

		public ICollection<BookDTO> GetAll()
		{
			var query = Database.Books.GetAll();
			var books = query.ToList();
			return Mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);

		}


		public BookDTO Get(int id)
		{
			var book = Database.Books.Get(id);
			if (book == null)
				throw new ValidationException("Book is not found");

			return Mapper.Map<Book, BookDTO>(book);
		}

		public ICollection<BookDTO> GetWithFilter(Expression<Func<Book, bool>> filter)
		{
			var a = Database.Books.Find(filter);
			var list = a.AsEnumerable<Book>();
			return Mapper.Map<IEnumerable<Book>, List<BookDTO>>
				(list);
			
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
			var tags = Database.Books.Get(id).Tags;
			var t = tags.Where(x => x.Books.Count == 1).ToList();
			foreach (var tag in t)
			{
				Database.Tags.Delete(tag.Id);
			}
			Database.Books.Delete(id);
			Database.Save();
		}

		public void Dispose()
		{
			Database.Dispose();
		}
/*
		public ICollection<BookDTO> GetBooksByTag(int tagId)
		{
			var tag = Mapper.Map<TagDTO>(Database.Tags.Get(tagId));
			return GetWithFilter(x => x.Tags.Contains(tag));
		}
*/
		public bool Exist(int id)
		{
			var book = Database.Books.Get(id);
			if (book == null) return false;
			else return true;
		}
	}
}
