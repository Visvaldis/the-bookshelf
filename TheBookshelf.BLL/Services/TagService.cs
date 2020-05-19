using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.BLL.DTO;
using TheBookshelf.DAL.Interfaces;
using TheBookshelf.DAL.Entities;
using AutoMapper;
using TheBookshelf.BLL.Infrastructure;

namespace TheBookshelf.BLL.Services
{
	public class TagService : ITagService
	{
		IUnitOfWork Database { get; set; }
		IMapper Mapper { get; set; }
		public TagService(IUnitOfWork uow)
		{
			Database = uow;
			Mapper = Mappers.TagMapper;
		}
		public int Add(TagDTO item)
		{
			if (item == null)
				throw new ArgumentNullException("Current tag is null. Try again.");

			var tag = Database.Tags.Find(x => x.Name.ToLower() == item.Name.ToLower()).FirstOrDefault();

			if (tag != default(Tag))
				throw new ValidationException(tag.Id.ToString());
			else {
				int id = Database.Tags.Create(Mapper.Map<TagDTO, Tag>(item));
				return id;
			}
		}

		public void Delete(int id)
		{
			Database.Tags.Delete(id);
			Database.Save();
		}

		public ICollection<TagDTO> GetWithFilter(Func<TagDTO, bool> filter)
		{

			var mapper = new MapperConfiguration(
				cfg => cfg.CreateMap<Func<TagDTO, bool>,
				Expression<Func<Tag, bool>>>())
					.CreateMapper();

			var expression = mapper.Map<Expression<Func<Tag, bool>>>(filter);


			return Mapper.Map<ICollection<Tag>, List<TagDTO>>
				(Database.Tags.Find(expression).ToList());
		}

		public TagDTO Get(int id)
		{
			var tag = Database.Tags.Get(id);
			if (tag == null)
				throw new ValidationException("Current tag is not found");

			return Mapper.Map<Tag, TagDTO>(tag);
		}

		public ICollection<TagDTO> GetAll()
		{
			var tags = Database.Tags.GetAll();
			return Mapper.Map<IEnumerable<Tag>, List<TagDTO>>(tags);
		}

		public void Update(TagDTO item)
		{
			Database.Tags.Update(Mapper.Map<TagDTO, Tag>(item));
			Database.Save();
		}

		public void Dispose()
		{
			Database.Dispose();
		}

		public bool Exist(int id)
		{
			var tag = Database.Tags.Get(id);
			if (tag == null) return false;
			else return true;
		}

		public bool Exist(string Name)
		{
			var tag = Database.Tags.Find(x => x.Name.ToLower() == Name.ToLower())
				.FirstOrDefault();
			if (tag == default(Tag)) return false;
			else return true;
		}

		public IEnumerable<BookDTO> GetBooksByTag(int tagId)
		{
			var tag = Database.Tags.Get(tagId);
			if (tag == null)
				throw new ValidationException("Current tag is not found");

			var books = tag.Books;
			var bookDto = Mappers.BookMapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
			return bookDto;
		}
	}
}
