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
	public class TagService : ITagService<TagDTO>
	{
		IUnitOfWork Database { get; set; }
		IMapper Mapper { get; set; }
		public TagService(IUnitOfWork uow)
		{
			Database = uow;
			Mapper = new MapperConfiguration(cfg => {
				cfg.CreateMap<Tag, TagDTO>();
				cfg.CreateMap<TagDTO, Tag>();})
			.CreateMapper();
		}
		public void Add(TagDTO item)
		{
			if (item == null)
				throw new ArgumentNullException("Current tag is null. Try again.");

			var tag = Database.Tags.Find(x => x.Name.ToLower() == item.Name.ToLower());
			if (tag.ToList().Count == 0)
				Database.Tags.Create(Mapper.Map<TagDTO, Tag>(item));
		}

		public void Delete(int id)
		{
			Database.Tags.Delete(id);
		}

		public IEnumerable<TagDTO> GetWithFilter(Func<TagDTO, bool> filter)
		{

			var mapper = new MapperConfiguration(
				cfg => cfg.CreateMap<Func<TagDTO, bool>,
				Expression<Func<Tag, bool>>>())
					.CreateMapper();

			var expression = mapper.Map<Expression<Func<Tag, bool>>>(filter);


			return Mapper.Map<IEnumerable<Tag>, List<TagDTO>>
				(Database.Tags.Find(expression).ToList());
		}

		public TagDTO Get(int id)
		{
			var tag = Database.Tags.Get(id);
			if (tag == null)
				throw new ValidationException("Current tag is not found");

			return Mapper.Map<Tag, TagDTO>(tag);
		}

		public IEnumerable<TagDTO> GetAll()
		{
			return Mapper.Map<IEnumerable<Tag>, List<TagDTO>>(Database.Tags.GetAll());
		}

		public void Update(TagDTO item)
		{
			Database.Tags.Update(Mapper.Map<TagDTO, Tag>(item));
		}

		public void Dispose()
		{
			Database.Dispose();
		}
	}
}