﻿using System;
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
			Mapper = Mappers.BookshelfMapper;
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

		public ICollection<TagDTO> GetWithFilter(Expression<Func<Tag, bool>> filter)
		{
			var a = Database.Tags.Find(filter);
			var list = a.AsEnumerable<Tag>();
			return Mapper.Map<IEnumerable<Tag>, List<TagDTO>>
				(list);
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
			var bookDto = Mappers.BookshelfMapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
			return bookDto;
		}

		public ICollection<TagDTO> GetByName(string name)
		{
			return GetWithFilter(x => x.Name.Contains(name));
		}
	}
}
